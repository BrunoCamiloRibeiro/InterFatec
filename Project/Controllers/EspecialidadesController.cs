using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

public class EspecialidadesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}