USE Fabys_Unha
GO

-- ==========================================
-- 1. PESSOAS / FUNCIONÁRIOS / CLIENTES
-- ==========================================

/* 
 * VIEW: vw_ListaFuncionarios
 * Propósito: Retorna uma lista detalhada dos funcionários ativos e inativos, combinando informações pessoais e profissionais.
 * Ideal para exibição em telas de listagem de recursos humanos ou profissionais do salão.
 *
 * SELECT: Traz o ID, Nome e Telefone da pessoa, além da Especialidade (descrição), Salário e o Status (numérico e descritivo).
 * JOINs: 
 *   - Utiliza um produto cartesiano (com WHERE atuando como INNER JOIN) entre Pessoas (p), Funcionarios (f) e Especialidades (e).
 *   - p.id = f.pessoa_id: Relaciona a entidade base 'Pessoa' com seus dados específicos de 'Funcionário'.
 *   - e.id = f.especialidade_id: Busca a descrição da 'Especialidade' do funcionário.
 */
CREATE OR ALTER VIEW vw_ListaFuncionarios AS
SELECT 
    p.id, 
    p.Nome, 
    p.Telefone,
    e.descricao AS Especialidade,
    f.salario,
    p.status AS Status_Id,
    -- Converte o código numérico de status em uma string legível
    CASE p.status
        WHEN 0 THEN 'Ativo'
        WHEN 1 THEN 'Inativo'
        ELSE 'Desconhecido'
    END AS Status_Descricao
FROM Pessoas p, Funcionarios f, Especialidades e
WHERE p.id = f.pessoa_id 
  AND e.id = f.especialidade_id
GO

/* 
 * VIEW: vw_ListaClientes
 * Propósito: Apresenta a relação de todos os clientes cadastrados com seus dados básicos de contato e situação atual.
 *
 * SELECT: Seleciona as colunas essenciais (ID, Nome, Telefone) da tabela de Pessoas, focando em clientes.
 * JOINs:
 *   - Relaciona Pessoas (p) e Clientes (c) via WHERE p.id = c.pessoa_id (funcionando como INNER JOIN).
 *   - Isso garante que apenas pessoas que são de fato 'Clientes' apareçam na listagem.
 */
CREATE OR ALTER VIEW vw_ListaClientes AS
SELECT 
    p.id, 
    p.Nome, 
    p.Telefone,
    p.status AS Status_Id,
    -- Estrutura condicional para traduzir o status do cliente
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
 * Propósito: Fornecer a listagem de marcas de produtos de forma amigável, traduzindo o campo status.
 *
 * SELECT: Obtém todos os campos relevantes (id, nome e status).
 * Não há JOINs, pois todos os dados derivam de uma única tabela (Marcas).
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
 * Propósito: Retornar o catálogo de serviços oferecidos pelo estabelecimento (ex: manicure, pedicure), com preço e tempo estimado.
 *
 * SELECT: Extrai as colunas da tabela de Servicos e decodifica o código de status para uma descrição de texto.
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
 * Propósito: Criar um catálogo unificado de produtos exibindo o nome de sua respectiva marca.
 *
 * SELECT: Mostra o código, nome do produto, nome da marca (resolvido via JOIN), preço, caminho da imagem e status.
 * JOINs:
 *   - Associa a tabela Produtos (p) com Marcas (m) pela condição p.marca_id = m.id.
 *   - Substitui o ID da marca pelo seu nome descritivo para facilitar o entendimento na camada visual.
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
 * Propósito: Gerar uma visão agregada que mostra cada especialidade e a quantidade de funcionários que a possuem.
 *
 * SELECT: Retorna a descrição da especialidade e realiza uma contagem de registros (COUNT(*)).
 * JOINs e GROUP BY:
 *   - Relaciona Funcionarios (f) e Especialidades (e).
 *   - O GROUP BY e.descricao consolida (agrupa) os dados para que a contagem (COUNT) reflita o total por cada especialidade.
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
 * Propósito: Visão geral e simplificada dos agendamentos efetuados, exibindo quem é o cliente e o valor total.
 *
 * SELECT: Busca número, data, total do agendamento, nome do cliente (da tabela Pessoas) e resolve a descrição do status do agendamento.
 * JOINs:
 *   - Envolve 3 tabelas: Agendamentos (a), Pessoas (p), e Clientes (c).
 *   - c.pessoa_id = a.cliente_id identifica quem foi o cliente do agendamento.
 *   - p.id = c.pessoa_id traz as informações textuais do cliente (seu Nome).
 */
CREATE OR ALTER VIEW vw_ListaAgendamento AS
SELECT
    a.nr AS NúmeroAgendamento,
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
 * Propósito: Detalhar os serviços específicos vinculados a um agendamento, mostrando qual funcionário realizou e em que horário.
 *
 * SELECT: Inclui dados pontuais do serviço agendado (horário, observação, valor cobrado), nome do serviço e nome do funcionário responsável.
 * JOINs:
 *   - Conecta Servicos_Agendados (sa) com Agendamentos (a) pelo número (agendamento_nr).
 *   - Traz detalhes do Serviço através de s.id = sa.servico_id.
 *   - Identifica o Funcionário com f.pessoa_id = sa.funcionario_id, e em seguida alcança a tabela Pessoas (p) para obter seu Nome (p.id = f.pessoa_id).
 */
CREATE OR ALTER VIEW vw_ListaServicoAgendamento AS
SELECT
    sa.agendamento_nr AS NúmeroAgendamento,
    s.descricao AS NomeServico,
    sa.obs AS Observação,
    sa.horario AS Horário,
    p.Nome AS Funcionário,
    sa.valor AS Valor
FROM Servicos_Agendados sa, Agendamentos a, Servicos s, Funcionarios f, Pessoas p
WHERE p.id = f.pessoa_id 
  AND s.id = sa.servico_id 
  AND f.pessoa_id = sa.funcionario_id 
  AND sa.agendamento_nr = a.nr
GO

/* 
 * VIEW: vw_ListaProdutoAgendamento
 * Propósito: Mostrar os produtos consumidos ou utilizados durante um serviço específico dentro de um agendamento.
 *
 * SELECT: Exibe número do agendamento, serviço realizado, observações, nome do produto utilizado, sua marca e o preço do item.
 * JOINs:
 *   - Associa Servicos_Agendados (sa) com Servicos (s) para saber de qual serviço estamos tratando.
 *   - Conecta com Produtos_Agendados (pa) pelas chaves conjuntas (agendamento_nr e servico_id).
 *   - Busca o nome do produto (p) usando pa.produto_codigo = p.codigo.
 *   - Pega o nome da marca associada ao produto resolvendo m.id = p.marca_id.
 */
CREATE OR ALTER VIEW vw_ListaProdutoAgendamento AS
SELECT
    pa.agendamento_nr AS NúmeroAgendamento,
    s.descricao AS NomeServico,
    sa.obs AS Observação,
    p.nome AS NomeProduto,
    m.nome AS Marca,
    pa.preco AS Preço
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
 * Propósito: Agrupar e contabilizar quantos serviços cada funcionário realizou, útil para cálculo de comissões ou relatórios de produtividade.
 *
 * SELECT: Traz o nome do funcionário e a soma total de serviços prestados por ele.
 * JOINs e GROUP BY:
 *   - Conecta a tabela Servicos_Agendados (sa) com Funcionarios (f) e, posteriormente, com Pessoas (p) para acessar o nome.
 *   - Agrupa os resultados pelo nome do funcionário (GROUP BY p.Nome) para aplicar a função de agregação COUNT(*).
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
 * Propósito: Relatório quantitativo que mostra o total de itens de estoque vinculados a cada marca.
 *
 * SELECT: Nome da marca e contagem correspondente.
 * JOINs e GROUP BY:
 *   - Associa Marcas (m) aos seus Produtos (p).
 *   - O GROUP BY m.nome reúne todos os produtos sob a mesma marca e realiza a contagem (COUNT).
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
-- SELECTS PARA TESTE
-- ==========================================

/* Consultas rápidas para validar os retornos de cada View. Útil para debug e checagem de dados. */
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