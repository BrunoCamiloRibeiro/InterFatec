using System.ComponentModel.DataAnnotations;
using FabysUnha.Enums;

namespace FabysUnha.Models;

public class Agendamentos
{
    /// <summary>
    /// Chave primária do agendamento.
    /// O atributo [Key] indica ao Entity Framework que esta propriedade é a chave de identificação única no banco de dados.
    /// </summary>
    [Key]
    public int Nr { get; set; }

    /// <summary>
    /// Data e hora para a qual o agendamento está marcado.
    /// </summary>
    public DateTime Data { get; set; }

    /// <summary>
    /// Valor total do agendamento.
    /// Representa a soma dos valores de serviços e produtos associados a este agendamento.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Situação atual do agendamento.
    /// </summary>
    // Por padrão, inicia como 'Pendente' ao ser instanciado no sistema.
    public AgendamentoStatus Status { get; set; } = AgendamentoStatus.Pendente;

    /// <summary>
    /// Identificador do cliente vinculado ao agendamento (Chave Estrangeira).
    /// </summary>
    // FK - Foreign Key relacionando à tabela de Clientes
    public int ClienteId { get; set; }

    /// <summary>
    /// Propriedade de navegação para acessar os dados completos do cliente que fez o agendamento.
    /// </summary>
    public Clientes? Cliente { get; set; }

    /// <summary>
    /// Coleção de serviços que compõem este agendamento.
    /// </summary>
    // Inicializamos com uma lista vazia para evitar NullReferenceException ao adicionar novos serviços
    public ICollection<Servicos_Agendados> Servicos_Agendados { get; set; } = new List<Servicos_Agendados>();

    /// <summary>
    /// Coleção de produtos vendidos ou aplicados durante este agendamento.
    /// </summary>
    // Inicializa a coleção para garantir que a propriedade não fique nula e que métodos como .Add() funcionem imediatamente
    public ICollection<Produtos_Agendados> Produtos_Agendados { get; set; } = new List<Produtos_Agendados>();
}