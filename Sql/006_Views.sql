USE Fabys_Unha
GO

-- ==========================================
-- 1. PESSOAS / FUNCIONÁRIOS / CLIENTES
-- ==========================================

/* 
 * VIEW: vw_ListaFuncionarios
 * Propósito: Fornece uma listagem detalhada de funcionários, trazendo inclusive a senha para autenticação de acesso e os dados de sua especialidade.
 *
 * SELECT: Busca informações de identificação, contato (da tabela Pessoas), especialidade, salário (Funcionários) e credenciais (senha).
 * JOINs: 
 *   - INNER JOIN Funcionarios f: Exige que a pessoa registrada na base seja efetivamente um funcionário.
 *   - LEFT JOIN Especialidades e: Traz a descrição da especialidade. O uso de LEFT JOIN garante que, se o funcionário ainda não tiver uma especialidade definida, ele não seja omitido da lista, preenchendo a coluna Especialidade com NULL.
 */
CREATE OR ALTER VIEW vw_ListaFuncionarios AS
SELECT 
    p.id, 
    p.Nome, 
    p.Telefone,
    e.descricao AS Especialidade,
    f.salario,
    p.senha,
    p.status AS Status_Id,
    CASE p.status
        WHEN 0 THEN 'Ativo'
        WHEN 1 THEN 'Inativo'
        ELSE 'Desconhecido'
    END AS Status_Descricao
FROM Pessoas p
INNER JOIN Funcionarios f ON p.id = f.pessoa_id
LEFT JOIN Especialidades e ON e.id = f.especialidade_id
GO

/* 
 * VIEW: vw_ListaClientes
 * Propósito: Exibe a lista de todos os clientes do sistema e seus dados de contato, convertendo o status para texto.
 *
 * SELECT: Retorna apenas os campos principais da tabela Pessoas para os registros identificados como clientes.
 * JOINs:
 *   - Produto cartesiano filtrado (WHERE p.id = c.pessoa_id), o que atua como um INNER JOIN entre Pessoas e Clientes.
 */
CREATE OR ALTER VIEW vw_ListaClientes AS
SELECT 
    p.id, 
    p.Nome, 
    p.Telefone,
    p.status AS Status_Id,
    CASE p.status
        WHEN 0 THEN 'Ativo'
        WHEN 1 THEN 'Inativo'
        ELSE 'Desconhecido'
    END AS Status_Descricao
FROM Pessoas p, Clientes c
WHERE p.id = c.pessoa_id
GO

-- ==========================================
-- 2. CATÁLOGOS BÁSICOS
-- ==========================================

/* 
 * VIEW: vw_ListaMarcas
 * Propósito: Simplifica a consulta da tabela de Marcas, exibindo os registros com o texto de status amigável ('Ativo', 'Inativo').
 *
 * SELECT: Extrai campos diretos da tabela Marcas, sem necessidade de relacionamentos.
 */
CREATE OR ALTER VIEW vw_ListaMarcas AS  
SELECT
    id,
    nome,
    status,
    CASE m.status
        WHEN 0 THEN 'Ativo'
        WHEN 1 THEN 'Inativo'
        ELSE 'Desconhecido'
    END AS Status_Descricao
FROM Marcas m
GO

/* 
 * VIEW: vw_ListaServicos
 * Propósito: Mostrar o portfólio de serviços do estabelecimento, agregando um campo descritivo de status para uso na interface.
 *
 * SELECT: Traz id, descrição, preço, tempo estimado e o mapeamento do código de status. Nenhuma outra tabela é cruzada.
 */
CREATE OR ALTER VIEW vw_ListaServicos AS
SELECT 
    id,
    descricao,
    preco,
    tempo,
    status AS Status_Id,
    CASE status
        WHEN 0 THEN 'Ativo'
        WHEN 1 THEN 'Inativo'
        ELSE 'Desconhecido'
    END AS Status_Descricao
FROM Servicos
GO

/* 
 * VIEW: vw_ListarProdutos
 * Propósito: Listar o catálogo de produtos integrando o nome da marca ao invés de exibir apenas o código da marca.
 *
 * SELECT: Retorna dados do produto, incluindo preço, imagem e nome da marca.
 * JOINs:
 *   - Conecta Produtos (p) com Marcas (m) pela condição WHERE p.marca_id = m.id, permitindo visualizar a que marca o produto pertence de forma transparente.
 */
CREATE OR ALTER VIEW vw_ListarProdutos AS
SELECT 
    p.codigo,
    p.nome AS Produto,
    m.nome AS Marca,
    p.preco,
    p.PathImagem,
    p.status AS Status_Id,
    CASE p.status
        WHEN 0 THEN 'Ativo'
        WHEN 1 THEN 'Inativo'
        ELSE 'Desconhecido'
    END AS Status_Descricao
FROM Produtos p, Marcas m
WHERE p.marca_id = m.id
GO

/* 
 * VIEW: vw_ListarEspecialidades
 * Propósito: Relatório sumarizado de todas as especialidades e o número de profissionais associados a cada uma delas.
 *
 * SELECT: Nome da especialidade e a contagem total de funcionários.
 * JOINs e GROUP BY:
 *   - Vincula Funcionarios e Especialidades usando f.especialidade_id = e.id.
 *   - Agrupa pelo nome (GROUP BY e.descricao), somando via COUNT(*) quantos registros existem no grupo.
 */
CREATE OR ALTER VIEW vw_ListarEspecialidades AS
SELECT
    e.descricao AS Especialidade,
    COUNT(*) AS TotalFuncionarios
FROM Funcionarios f, Especialidades e
WHERE f.especialidade_id = e.id  
GROUP BY e.descricao
GO

-- ==========================================
-- 3. AGENDAMENTOS
-- ==========================================

/* 
 * VIEW: vw_ListaAgendamento
 * Propósito: Resumo dos agendamentos, mostrando os detalhes principais (data, total, status legível) e identificando de forma amigável quem é o cliente.
 *
 * SELECT: Extrai os dados centrais do agendamento, além do nome completo do cliente.
 * JOINs:
 *   - Utiliza a tabela intermediária Clientes (c) para conectar Agendamentos (a) e Pessoas (p).
 *   - c.pessoa_id = a.cliente_id e p.id = c.pessoa_id.
 */
CREATE OR ALTER VIEW vw_ListaAgendamento AS
SELECT
    a.nr AS NumeroAgendamento,
    a.data,
    a.total,
    p.nome AS Cliente,
    a.status,
    CASE a.status
        WHEN 0 THEN 'Pendente'
        WHEN 1 THEN 'Cancelado'
        WHEN 2 THEN 'Finalizado'
        ELSE 'Desconhecido'
    END AS Status_Descricao
FROM Agendamentos a, Pessoas p, Clientes c
WHERE p.id = c.pessoa_id 
  AND c.pessoa_id = a.cliente_id
GO

/* 
 * VIEW: vw_ListaServicoAgendamento
 * Propósito: Detalhar o que foi feito dentro de um agendamento específico (qual serviço, por quem e a que horas).
 *
 * SELECT: Mostra o número do agendamento raiz, o serviço prestado, observações, horário e o nome do funcionário responsável.
 * JOINs:
 *   - Exige o relacionamento de 5 tabelas simultâneas: Servicos_Agendados, Agendamentos, Servicos, Funcionarios e Pessoas.
 *   - Onde o núcleo da junção resolve os IDs do serviço e do funcionário com as suas respectivas tabelas descritivas.
 */
CREATE OR ALTER VIEW vw_ListaServicoAgendamento AS
SELECT
    sa.agendamento_nr AS NumeroAgendamento,
    s.descricao AS NomeServico,
    sa.obs AS Observacao,
    sa.horario AS Horario,
    p.Nome AS Funcionario,
    sa.valor AS Valor
FROM Servicos_Agendados sa, Agendamentos a, Servicos s, Funcionarios f, Pessoas p
WHERE p.id = f.pessoa_id 
  AND s.id = sa.servico_id 
  AND f.pessoa_id = sa.funcionario_id 
  AND sa.agendamento_nr = a.nr
GO

/* 
 * VIEW: vw_ListaProdutoAgendamento
 * Propósito: Listar os produtos consumidos vinculados a um serviço específico que fez parte de um agendamento.
 *
 * SELECT: Dados do produto (nome e marca), valor e o serviço/agendamento atrelado.
 * JOINs:
 *   - Usa a tabela Produtos_Agendados (pa) cruzada com Servicos_Agendados (sa).
 *   - Além disso, vai até as tabelas base Produtos (p), Marcas (m) e Servicos (s) para obter descrições amigáveis no lugar dos IDs numéricos.
 */
CREATE OR ALTER VIEW vw_ListaProdutoAgendamento AS
SELECT
    pa.agendamento_nr AS NumeroAgendamento,
    s.descricao AS NomeServico,
    sa.obs AS Observacao,
    p.nome AS NomeProduto,
    m.nome AS Marca,
    pa.preco AS Preco
FROM Servicos_Agendados sa, Servicos s, Produtos p, Produtos_Agendados pa, Marcas m
WHERE sa.servico_id = s.id 
  AND pa.agendamento_nr = sa.agendamento_nr 
  AND pa.servico_id = sa.servico_id 
  AND pa.produto_codigo = p.codigo 
  AND p.marca_id = m.id
GO

-- ==========================================
-- 4. DASHBOARDS E RELATÓRIOS
-- ==========================================

/* 
 * VIEW: vw_FuncionarioProducao
 * Propósito: Dashboard consolidado para verificar quantos atendimentos/serviços um dado funcionário concluiu.
 *
 * SELECT: Retorna nome e o total calculado.
 * JOINs e GROUP BY:
 *   - Filtro conectando Serviços Agendados, Funcionários e Pessoas.
 *   - O GROUP BY p.Nome condensa todas as linhas do mesmo profissional e aplica a contagem no COUNT(*).
 */
CREATE OR ALTER VIEW vw_FuncionarioProducao AS 
SELECT
    p.Nome AS Funcionario,
    COUNT(*) AS TotalServicos
FROM Pessoas p, Funcionarios f, Servicos_Agendados sa
WHERE p.id = f.pessoa_id 
  AND f.pessoa_id = sa.funcionario_id  
GROUP BY p.Nome
GO

/* 
 * VIEW: vw_ProdutosPorMarca
 * Propósito: Dashboard que reflete o inventário ou diversidade de produtos, somando os itens disponíveis agrupados pela marca.
 *
 * SELECT: Traz o nome da marca e a soma de produtos (TotalProdutos).
 * JOINs e GROUP BY:
 *   - Conecta Produtos (p) com Marcas (m).
 *   - O agrupamento GROUP BY m.nome é a chave que permite que COUNT(*) não seja global, mas segmentado marca a marca.
 */
CREATE OR ALTER VIEW vw_ProdutosPorMarca AS  
SELECT
    m.nome AS Marca,
    COUNT(*) AS TotalProdutos
FROM Marcas m, Produtos p
WHERE m.id = p.marca_id
GROUP BY m.nome
GO

-- ==========================================
-- 5. VALIDAÇÃO DE LOGIN
-- ==========================================

/* 
 * VIEW: vw_ValidarClienteLogin
 * Propósito: Fornece uma maneira rápida de buscar clientes junto com sua senha e informações do histórico de agendamentos. Útil para validar o login e retornar os agendamentos na mesma consulta.
 *
 * SELECT: Busca dados confidenciais (senha), contato e dados resumidos do agendamento (número, data, total e status).
 * JOINs:
 *   - Conecta Pessoas e Clientes para garantir que o alvo seja cliente.
 *   - Relaciona com Agendamentos (a) para fornecer informações de consumo logo após a validação.
 */
CREATE OR ALTER VIEW vw_ValidarClienteLogin AS
SELECT 
    p.id,
    p.nome,
    p.telefone,
    p.senha,
    a.nr AS agendamento_nr,
    a.data,
    a.total,
    a.status
FROM Pessoas p, Agendamentos a, Clientes c
WHERE p.id = c.pessoa_id and c.pessoa_id = a.cliente_id
GO



-- ==========================================
-- SELECTS PARA TESTE
-- ==========================================

/* Bloco para os testes das views após criação ou alteração */
select * from vw_ListaClientes --join
select * from vw_ListaFuncionarios --join
select * from vw_ListaMarcas    
select * from vw_ListaServicos
select * from vw_ListarProdutos --join
select * from vw_ListarEspecialidades --join



select * from vw_ListaAgendamento --join
select * from vw_ListaServicoAgendamento --join
select * from vw_ListaProdutoAgendamento --join


select * from vw_FuncionarioProducao -- join
select * from vw_ProdutosPorMarca -- join
select * from vw_ValidarClienteLogin




