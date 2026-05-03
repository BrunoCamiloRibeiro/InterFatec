using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

public class AgendamentosController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}