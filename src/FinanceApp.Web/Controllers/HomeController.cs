using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
