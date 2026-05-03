using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

public class ClientesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}