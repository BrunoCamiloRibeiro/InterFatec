using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

public class ServicosController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}