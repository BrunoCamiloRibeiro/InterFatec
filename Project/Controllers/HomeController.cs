using Microsoft.AspNetCore.Mvc;

namespace FabysUnha.Controllers;

/// <summary>
/// Controlador principal responsável pelo redirecionamento inicial da aplicação.
/// Este controlador avalia o tipo de usuário logado e o direciona para a página correspondente.
/// É uma ótima prática para centralizar a lógica de "onde ir" após entrar no sistema.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Ação padrão que é executada quando o usuário acessa a raiz do site (ex: www.seusite.com/).
    /// Ela verifica a sessão atual para decidir para onde o usuário deve ser redirecionado.
    /// </summary>
    /// <returns>Um redirecionamento (RedirectToAction) para a página adequada com base no perfil do usuário.</returns>
    public IActionResult Index()
    {
        // Obtém o tipo de usuário armazenado na sessão (Session).
        // A sessão guarda dados temporários do usuário enquanto ele navega pelo sistema.
        var usuarioTipo = HttpContext.Session.GetString("UsuarioTipo");

        // Verifica se o tipo de usuário logado é "Cliente".
        if (usuarioTipo == "Cliente")
        {
            // Redireciona o cliente para a ação "MeusAgendamentos" no controlador "Agendamentos".
            return RedirectToAction("MeusAgendamentos", "Agendamentos");
        }

        // Verifica se o tipo de usuário logado é "Funcionario".
        if (usuarioTipo == "Funcionario")
        {
            // Redireciona o funcionário para a ação "Index" no controlador "Funcionarios".
            return RedirectToAction("Index", "Funcionarios");
        }

        // Caso a sessão não tenha a chave "UsuarioTipo" (o usuário não está logado)
        // ou seja um valor não reconhecido, redirecionamos para a tela de Login.
        return RedirectToAction("Index", "Login");
    }
}