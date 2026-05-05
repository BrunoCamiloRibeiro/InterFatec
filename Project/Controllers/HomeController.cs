using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}