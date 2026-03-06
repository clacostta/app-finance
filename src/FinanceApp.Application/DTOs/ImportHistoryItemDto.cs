using FinanceApp.Domain.Enums;

namespace FinanceApp.Application.DTOs;

public record ImportHistoryItemDto(
    Guid BatchId,
    string FileName,
    DateTime ImportedAt,
    ImportBatchStatus Status,
    int TotalRecords,
    int ImportedRecords,
    int DuplicatedRecords,
    int FailedRecords);
