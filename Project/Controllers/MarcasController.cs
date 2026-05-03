using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

public class MarcasController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}