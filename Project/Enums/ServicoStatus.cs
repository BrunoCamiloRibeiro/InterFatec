namespace FabysUnha.Enums;

/// <summary>
/// Define se um serviço oferecido pelo estabelecimento está disponível (ativo) ou não (inativo).
/// </summary>
/// <remarks>
/// Este enum facilita a manutenção do sistema. Se um serviço não é mais oferecido, 
/// basta mudar seu status para Inativo, preservando o histórico de quem já o utilizou.
/// </remarks>
public enum ServicoStatus
{
    /// <summary>
    /// O serviço está ativo e disponível para novos agendamentos.
    /// </summary>
    Ativo = 0, // Utiliza o valor numérico 0 para fácil persistência e comparação no código.

    /// <summary>
    /// O serviço foi desativado e não pode mais ser agendado pelos clientes.
    /// </summary>
    Inativo = 1 // Utiliza o valor numérico 1 para representar a indisponibilidade sem deletar o dado.
}