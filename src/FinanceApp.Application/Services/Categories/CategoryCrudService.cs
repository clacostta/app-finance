using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Categories;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Categories;

public class CategoryCrudService : ICategoryCrudService
{
    private readonly IAppDbContext _context;

    public CategoryCrudService(IAppDbContext context) => _context = context;

    public async Task<IReadOnlyCollection<CategoryDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.TransactionCategories
            .Where(x => x.UserId == userId || x.IsSystemDefault)
            .OrderBy(x => x.Name)
            .Select(x => new CategoryDto(x.Id, x.Name, x.IsSystemDefault))
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TransactionCategories
            .Where(x => (x.UserId == userId || x.IsSystemDefault) && x.Id == id)
            .Select(x => new CategoryDto(x.Id, x.Name, x.IsSystemDefault))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid> CreateAsync(Guid userId, UpsertCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = new TransactionCategory(userId, dto.Name);
        _context.TransactionCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);
        return category.Id;
    }

    public async Task<bool> UpdateAsync(Guid userId, Guid id, UpsertCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = await _context.TransactionCategories
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id && !x.IsSystemDefault, cancellationToken);
        if (category is null) return false;

        category.Rename(dto.Name);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _context.TransactionCategories
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id && !x.IsSystemDefault, cancellationToken);
        if (category is null) return false;

        _context.TransactionCategories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
