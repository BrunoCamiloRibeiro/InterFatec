namespace FabysUnha.ViewModels;

/// <summary>
/// Modelo de visualização (ViewModel) voltado especificamente para a operação de atualização de um agendamento existente.
/// Como a estrutura de dados de edição e criação é quase a mesma, ele estende 'AgendamentoRegistroViewModel' 
/// (que lida com validações e coletas da interface) aproveitando todo o seu código.
/// </summary>
public class AgendamentoEditarViewModel : AgendamentoRegistroViewModel
{
    /// <summary>
    /// Número de identificação único (ID) do agendamento que está sendo manipulado.
    /// </summary>
    // É o diferencial obrigatório desta classe para uma simples classe de criação. 
    // É necessário para apontar no banco de dados qual registro exatamente precisa ser atualizado (UPDATE).
    public int Nr { get; set; }
}