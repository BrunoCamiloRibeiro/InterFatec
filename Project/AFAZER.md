# 📋 AFAZER: Desenvolvimento do Sistema

## 1. Estrutura Base (Model -> Repository -> Service -> Controller)

**Especialidades**
- [ ] Criar Model `Especialidade`
- [ ] Criar Repository `IEspecialidadeRepository` e `EspecialidadeRepository`
- [ ] Criar Service `IEspecialidadeService` e `EspecialidadeService`
- [ ] Implementar métodos no Controller `EspecialidadesController`

**Produtos**
- [ ] Criar Model `Produto`
- [ ] Criar Repository `IProdutoRepository` e `ProdutoRepository`
- [ ] Criar Service `IProdutoService` e `ProdutoService`
- [ ] Implementar métodos no Controller `ProdutosController`

**Serviços**
- [ ] Criar Model `Servico`
- [ ] Criar Repository `IServicoRepository` e `ServicoRepository`
- [ ] Criar Service `IServicoService` e `ServicoService`
- [ ] Implementar métodos no Controller `ServicosController`

**Agendamentos**
- [ ] Criar Model `Agendamento`
- [ ] Criar Repository `IAgendamentoRepository` e `AgendamentoRepository`
- [ ] Criar Service `IAgendamentoService` e `AgendamentoService`
- [ ] Implementar métodos no Controller `AgendamentosController`

**Tabelas Auxiliares (N:N)**
- [ ] Criar Model `Servicos_Agendados` (com chave composta `AgendamentoNr + ServicoId`, horário, obs, funcionário)
- [ ] Criar Model `Produtos_Agendados` (com chave composta `AgendamentoNr + ProdutoCodigo`)

---

## 2. Lógica Específica de Negócio (Camada Service)

**Agendamentos (O que foge do CRUD normal)**
- [ ] Implementar criação de Agendamento recebendo lista de serviços e/ou produtos.
- [ ] Implementar validação de `ClienteId` e `Status` na criação.
- [ ] Implementar lógica de cálculo do **Total** (somando serviços e produtos).
- [ ] Envolver a criação do cabeçalho e dos itens (`Servicos_Agendados` e `Produtos_Agendados`) em uma transação do banco.
- [ ] Congelar o preço atual do produto/serviço na tabela de itens do agendamento (histórico financeiro).

**Clientes**
- [ ] Implementar método `Excluir` no `ClientesController`.

---

## 3. ViewModels (A Criar)

**Especialidades**
- [ ] Criar `EspecialidadeRegistroViewModel`
- [ ] Criar `EspecialidadeEditarViewModel`
- [ ] Criar `EspecialidadeListagemViewModel`
- [ ] Criar `EspecialidadeDetalhesViewModel`

**Produtos**
- [ ] Criar `ProdutoRegistroViewModel`
- [ ] Criar `ProdutoEditarViewModel`
- [ ] Criar `ProdutoListagemViewModel`
- [ ] Criar `ProdutoDetalhesViewModel`

**Serviços**
- [ ] Criar `ServicoRegistroViewModel`
- [ ] Criar `ServicoEditarViewModel`
- [ ] Criar `ServicoListagemViewModel`
- [ ] Criar `ServicoDetalhesViewModel`

**Agendamentos**
- [ ] Criar `AgendamentoRegistroViewModel`
- [ ] Criar `AgendamentoEditarViewModel`
- [ ] Criar `AgendamentoListagemViewModel`
- [ ] Criar `AgendamentoDetalhesViewModel`
- [ ] Criar `ProdutoAgendadoViewModel` (para itens de produto no agendamento)

---

## 4. Objetos de Banco de Dados (Exigência: 3 de cada)

### VIEWS (Consultas)
- [ ] **Criar SQL:** `vw_ResumoAgendamentos` (Agendamento + Cliente + Funcionario + Total).
- [ ] **Mapeamento C#:** Criar classe modelo e configurar com `.HasNoKey().ToView("vw_ResumoAgendamentos")` no `DbContext`.
- [ ] **Criar SQL:** `vw_EstoqueProdutosComMarca` (Produto + Marca + Estoque).
- [ ] **Mapeamento C#:** Criar classe modelo e configurar com `.HasNoKey().ToView("vw_EstoqueProdutosComMarca")` no `DbContext`.
- [ ] **Criar SQL:** `vw_DesempenhoFuncionarios` (Funcionario + Qtde Atendimentos + Receita Bruta).
- [ ] **Mapeamento C#:** Criar classe modelo e configurar com `.HasNoKey().ToView("vw_DesempenhoFuncionarios")` no `DbContext`.

### STORED PROCEDURES (Ações em Massa)
- [ ] **Criar SQL:** `sp_FinalizarAgendamentoEBaixarEstoque` (Altera status do agendamento e dá baixa nos produtos em transação).
- [ ] **Uso C#:** Chamar via `ExecuteSqlRawAsync` no `AgendamentoService`.
- [ ] **Criar SQL:** `sp_CancelarAgendaFuncionarioDia` (Cancela todos os agendamentos pendentes de um funcionário num dia específico).
- [ ] **Uso C#:** Chamar via `ExecuteSqlRawAsync` no `FuncionarioService`.
- [ ] **Criar SQL:** `sp_ReajustarPrecosPorMarca` (Aplica porcentagem de aumento/desconto em produtos de uma marca).
- [ ] **Uso C#:** Chamar via `ExecuteSqlRawAsync` no `ProdutoService`.

### FUNCTIONS E TRIGGERS (Regras e Automação)
- [ ] **Criar SQL:** Function TVF `fn_ObterHorariosDisponiveis` (Recebe FuncionarioId e Data, retorna lista de horários livres).
- [ ] **Mapeamento C#:** Criar classe sem chave e configurar `.HasDbFunction()` no `DbContext`.
- [ ] **Criar SQL:** Function Escalar `fn_CalcularComissaoFuncionario` (Recebe FuncionarioId e Mês, calcula valor a pagar).
- [ ] **Mapeamento C#:** Criar método estático no `DbContext` decorado com `[DbFunction]`.
- [ ] **Criar SQL:** Trigger `trg_AuditarAgendamentoCancelado` (AFTER UPDATE na tabela de Agendamentos para gravar log; **INCLUIR `SET NOCOUNT ON;`**).
- [ ] **Uso C#:** Criar tabela `Agendamentos_Cancelados_Log` no banco. (Nenhuma ação extra necessária no C#, o SQL resolve sozinho).