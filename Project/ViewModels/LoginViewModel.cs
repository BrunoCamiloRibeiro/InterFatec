using Microsoft.AspNetCore.Mvc.Rendering;

using FabysUnha.Enums;

namespace FabysUnha.ViewModels;

/// <summary>
/// Modelo de visualização (ViewModel) para gerenciar os dados de autenticação e cadastro de usuários.
/// Agrupa informações de login de clientes e funcionários, além de permitir o cadastro unificado.
/// </summary>
public class LoginViewModel
{
    // ==========================================
    // SEÇÃO: Cliente Login
    // ==========================================

    /// <summary>
    /// Nome do cliente, utilizado para exibir mensagens personalizadas ou validações adicionais.
    /// </summary>
    public string ClienteNome { get; set; } = string.Empty;

    /// <summary>
    /// Telefone do cliente, que funciona como identificador (login) para o acesso ao sistema.
    /// </summary>
    public string ClienteTelefone { get; set; } = string.Empty;

    /// <summary>
    /// Senha de acesso do cliente.
    /// </summary>
    public string ClienteSenha { get; set; } = string.Empty;

    /// <summary>
    /// Confirmação da senha para validar operações de atualização ou redefinição de credenciais.
    /// </summary>
    public string ClienteSenhaConfirmacao { get; set; } = string.Empty;

    // ==========================================
    // SEÇÃO: Funcionário Login
    // ==========================================

    /// <summary>
    /// Telefone do funcionário, que também atua como identificador (login) para o painel administrativo.
    /// </summary>
    public string FuncionarioTelefone { get; set; } = string.Empty;

    /// <summary>
    /// Senha de acesso restrita do funcionário.
    /// </summary>
    public string FuncionarioSenha { get; set; } = string.Empty;

    // ==========================================
    // SEÇÃO: Cadastro Unificado
    // ==========================================

    /// <summary>
    /// Define o tipo de perfil de usuário que está sendo cadastrado no sistema.
    /// </summary>
    // Inicializa a seleção do tipo de cadastro como 'Cliente' por padrão para facilitar o uso comum.
    public TipoUsuario CadastroTipo { get; set; } = TipoUsuario.Cliente;

    /// <summary>
    /// Nome completo informado durante o novo cadastro.
    /// </summary>
    public string CadastroNome { get; set; } = string.Empty;

    /// <summary>
    /// Telefone informado durante o novo cadastro. Será validado para garantir unicidade na base.
    /// </summary>
    public string CadastroTelefone { get; set; } = string.Empty;

    /// <summary>
    /// Senha escolhida pelo usuário no momento da criação da conta.
    /// </summary>
    public string CadastroSenha { get; set; } = string.Empty;

    /// <summary>
    /// Confirmação da senha escolhida, utilizada para evitar erros de digitação e travamentos de acesso futuros.
    /// </summary>
    public string CadastroConfirmacaoSenha { get; set; } = string.Empty;
}
