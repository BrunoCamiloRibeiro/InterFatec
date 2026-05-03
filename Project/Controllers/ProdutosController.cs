using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

public class ProdutosController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}