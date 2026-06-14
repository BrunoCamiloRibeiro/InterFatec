namespace FabysUnha.Enums;

/// <summary>
/// Define o status atual de uma especialidade dentro do sistema.
/// </summary>
/// <remarks>
/// A utilização de enumerações (enum) para controlar status (Ativo/Inativo) 
/// é uma boa prática conhecida como "exclusão lógica" (soft delete), 
/// onde o registro não é apagado do banco de dados, apenas inativado.
/// </remarks>
public enum EspecialidadeStatus
{
    /// <summary>
    /// A especialidade está ativa e pode ser utilizada em novos cadastros e consultas.
    /// </summary>
    Ativo = 0, // O valor 0 indica o estado normal e habilitado de funcionamento.

    /// <summary>
    /// A especialidade está inativa, ou seja, não deve aparecer para novas seleções no sistema.
    /// </summary>
    Inativo = 1 // O valor 1 sinaliza que o registro está desabilitado, mantendo o histórico de dados.
}