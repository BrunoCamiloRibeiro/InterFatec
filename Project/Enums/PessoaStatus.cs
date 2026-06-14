namespace FabysUnha.Enums;

/// <summary>
/// Define o estado de atividade de uma pessoa (cliente, funcionário, etc.) no sistema.
/// </summary>
/// <remarks>
/// Trabalhar com 'Status' em vez de deletar dados fisicamente preserva a integridade 
/// referencial do banco de dados (exclusão lógica ou 'soft delete').
/// </remarks>
public enum PessoaStatus
{
    /// <summary>
    /// A pessoa está com seu cadastro ativo e tem permissão para interagir com o sistema.
    /// </summary>
    Ativo = 0, // Estado normal de cadastro habilitado (0).

    /// <summary>
    /// A pessoa teve seu acesso ou cadastro inativado/bloqueado.
    /// </summary>
    Inativo = 1 // Estado que impede operações ativas da pessoa (1).
}