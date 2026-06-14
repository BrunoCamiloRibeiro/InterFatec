namespace FabysUnha.Models;

/// <summary>
/// Representa um funcionário do estabelecimento.
/// Aplica o conceito de herança, derivando de 'Pessoas' para reaproveitar propriedades comuns (como Nome, CPF, Telefone),
/// e define atributos específicos relacionados ao contexto do funcionário.
/// </summary>
public class Funcionarios : Pessoas
{
    /// <summary>
    /// Valor monetário referente ao salário fixo ou base do funcionário.
    /// </summary>
    public decimal Salario { get; set; }

    /// <summary>
    /// Chave estrangeira indicando a qual especialidade este funcionário pertence.
    /// O uso do tipo anulável (int?) permite que o funcionário possa não ter uma especialidade vinculada inicialmente.
    /// </summary>
    // FK
    public int? EspecialidadeId { get; set; }

    /// <summary>
    /// Propriedade de navegação para acessar os dados da especialidade do funcionário.
    /// </summary>
    public Especialidades? Especialidade { get; set; }

    /// <summary>
    /// Lista contendo todos os serviços agendados para este funcionário executar.
    /// </summary>
    // Instancia uma lista vazia para prevenir problemas de referência nula ao manipular a coleção do objeto recém-instanciado
    public ICollection<Servicos_Agendados> Servicos_Agendados { get; set; } = new List<Servicos_Agendados>();
}