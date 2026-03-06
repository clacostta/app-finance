using FinanceApp.Application.DTOs.Categories;

namespace FinanceApp.Application.Abstractions.Services;

public interface ICategoryCrudService
{
    Task<IReadOnlyCollection<CategoryDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Guid userId, UpsertCategoryDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid userId, Guid id, UpsertCategoryDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
