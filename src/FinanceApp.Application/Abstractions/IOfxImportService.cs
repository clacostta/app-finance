using FinanceApp.Application.DTOs;

namespace FinanceApp.Application.Abstractions;

public interface IOfxImportService
{
    Task<ImportPreviewDto> PreviewAsync(Guid userId, string fileName, Stream contentStream, CancellationToken cancellationToken = default);
    Task<ImportExecutionResultDto> ImportAsync(Guid userId, string fileName, Stream contentStream, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ImportHistoryItemDto>> GetHistoryAsync(Guid userId, CancellationToken cancellationToken = default);
}
