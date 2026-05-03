using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

public class FuncionariosController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}