using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var usuarioTipo = HttpContext.Session.GetString("UsuarioTipo");

        if (usuarioTipo == "Cliente")
        {
            return RedirectToAction("MeusAgendamentos", "Agendamentos");
        }

        if (usuarioTipo == "Funcionario")
        {
            return RedirectToAction("Index", "Funcionarios");
        }

        return RedirectToAction("Index", "Login");
    }
}