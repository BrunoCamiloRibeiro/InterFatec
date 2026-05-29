using Microsoft.AspNetCore.Mvc.Rendering;

namespace FabysUnha.ViewModels;

public class LoginViewModel
{
    // Cliente Login
    public string ClienteNome { get; set; } = string.Empty;
    public string ClienteTelefone { get; set; } = string.Empty;
    public string ClienteSenha { get; set; } = string.Empty;
    public string ClienteSenhaConfirmacao { get; set; } = string.Empty;

    // Funcionário Login
    public string FuncionarioTelefone { get; set; } = string.Empty;
    public string FuncionarioSenha { get; set; } = string.Empty;

    // Cadastro unificado
    public string CadastroTipo { get; set; } = "cliente";
    public string CadastroNome { get; set; } = string.Empty;
    public string CadastroTelefone { get; set; } = string.Empty;
    public string CadastroSenha { get; set; } = string.Empty;
    public string CadastroConfirmacaoSenha { get; set; } = string.Empty;
}
