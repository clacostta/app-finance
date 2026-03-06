using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Goals;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Goals;

public class GoalService : IGoalService
{
    private readonly IAppDbContext _context;

    public GoalService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<GoalDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Goals
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.TargetDate)
            .Select(x => new GoalDto(
                x.Id,
                x.Name,
                x.TargetAmount,
                x.CurrentAmount,
                x.TargetDate,
                x.TargetAmount <= 0 ? 0 : (x.CurrentAmount / x.TargetAmount) * 100))
            .ToListAsync(cancellationToken);
    }

    public async Task<GoalDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Goals
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Id == id)
            .Select(x => new GoalDto(x.Id, x.Name, x.TargetAmount, x.CurrentAmount, x.TargetDate, x.TargetAmount <= 0 ? 0 : (x.CurrentAmount / x.TargetAmount) * 100))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid> CreateAsync(Guid userId, UpsertGoalDto dto, CancellationToken cancellationToken = default)
    {
        var goal = new Goal(userId, dto.Name, dto.TargetAmount);
        goal.Update(dto.Name, dto.TargetAmount, dto.CurrentAmount, dto.TargetDate);
        _context.Goals.Add(goal);
        await _context.SaveChangesAsync(cancellationToken);
        return goal.Id;
    }

    public async Task<bool> UpdateAsync(Guid userId, Guid id, UpsertGoalDto dto, CancellationToken cancellationToken = default)
    {
        var goal = await _context.Goals.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (goal is null) return false;

        goal.Update(dto.Name, dto.TargetAmount, dto.CurrentAmount, dto.TargetDate);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var goal = await _context.Goals.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (goal is null) return false;
        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
