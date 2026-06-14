namespace FabysUnha.Enums;

/// <summary>
/// Enumera os perfis de usuários que podem acessar o sistema.
/// </summary>
/// <remarks>
/// Enums são excelentes para definir categorias restritas. Aqui, cada tipo
/// de usuário (role) determina quais permissões ou fluxos o sistema deve liberar.
/// Note que, neste caso, os valores numéricos começam a partir de 1.
/// </remarks>
public enum TipoUsuario
{
    /// <summary>
    /// Representa o perfil de um cliente, que possui acesso restrito,
    /// geralmente apenas aos seus próprios agendamentos e dados.
    /// </summary>
    Cliente = 1, // O valor 1 é explicitamente definido para representar o cliente.

    /// <summary>
    /// Representa o perfil de um funcionário ou administrador, que possui
    /// acesso à área administrativa (backoffice) do sistema.
    /// </summary>
    Funcionario = 2 // O valor 2 é atribuído para diferenciar o perfil administrativo.
}
