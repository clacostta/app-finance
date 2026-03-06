using FinanceApp.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace FinanceApp.Web.Models.Imports;

public class ImportOfxViewModel
{
    public IFormFile? File { get; set; }
    public ImportPreviewDto? Preview { get; set; }
    public ImportExecutionResultDto? Result { get; set; }
    public string? EncodedContent { get; set; }
    public string? FileName { get; set; }
}
