using FinanceApp.Application.Abstractions;
using FinanceApp.Web.Models.Imports;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class ImportsController : Controller
{
    private const long MaxOfxSizeBytes = 2 * 1024 * 1024;
    private static readonly string[] AllowedExtensions = [".ofx"];

    private readonly IOfxImportService _ofxImportService;

    public ImportsController(IOfxImportService ofxImportService)
    {
        _ofxImportService = ofxImportService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = ResolveUserId();
        ViewBag.History = await _ofxImportService.GetHistoryAsync(userId, cancellationToken);
        return View(new ImportOfxViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Preview(ImportOfxViewModel model, CancellationToken cancellationToken)
    {
        if (model.File is null || model.File.Length == 0)
        {
            ModelState.AddModelError(nameof(model.File), "Selecione um arquivo OFX.");
            return await ReturnWithHistory(model, cancellationToken);
        }

        var extension = Path.GetExtension(model.File.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            ModelState.AddModelError(nameof(model.File), "Formato inválido. Envie apenas arquivos .ofx.");
            return await ReturnWithHistory(model, cancellationToken);
        }

        if (model.File.Length > MaxOfxSizeBytes)
        {
            ModelState.AddModelError(nameof(model.File), "Arquivo excede o limite de 2MB.");
            return await ReturnWithHistory(model, cancellationToken);
        }

        await using var ms = new MemoryStream();
        await model.File.CopyToAsync(ms, cancellationToken);
        ms.Position = 0;

        var userId = ResolveUserId();
        var preview = await _ofxImportService.PreviewAsync(userId, model.File.FileName, ms, cancellationToken);

        model.Preview = preview;
        model.EncodedContent = Convert.ToBase64String(ms.ToArray());
        model.FileName = model.File.FileName;

        return await ReturnWithHistory(model, cancellationToken);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(ImportOfxViewModel model, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(model.EncodedContent) || string.IsNullOrWhiteSpace(model.FileName))
        {
            ModelState.AddModelError(string.Empty, "Não foi possível confirmar. Refaça o preview do arquivo.");
            return await ReturnWithHistory(new ImportOfxViewModel(), cancellationToken);
        }

        var bytes = Convert.FromBase64String(model.EncodedContent);
        await using var ms = new MemoryStream(bytes);

        var userId = ResolveUserId();
        var result = await _ofxImportService.ImportAsync(userId, model.FileName, ms, cancellationToken);

        var response = new ImportOfxViewModel { Result = result };
        return await ReturnWithHistory(response, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> History(CancellationToken cancellationToken)
    {
        var userId = ResolveUserId();
        var history = await _ofxImportService.GetHistoryAsync(userId, cancellationToken);
        return View(history);
    }

    private Guid ResolveUserId() => UserIdResolver.Resolve(User);

    private async Task<IActionResult> ReturnWithHistory(ImportOfxViewModel model, CancellationToken cancellationToken)
    {
        var userId = ResolveUserId();
        ViewBag.History = await _ofxImportService.GetHistoryAsync(userId, cancellationToken);
        return View("Index", model);
    }
}
