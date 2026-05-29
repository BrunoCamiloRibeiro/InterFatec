USE Fabys_Unha
GO

-- ==========================================
-- 1. PESSOAS / FUNCIONÁRIOS / CLIENTES
-- ==========================================
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
-- Marcas
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

-- Serviços
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

-- Produtos com Marca (join)
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

-- Especialidades
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
-- Agendamentos Básico
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

-- Serviços do Agendamento
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

-- Produtos do Agendamento
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
-- Produção por Funcionário
CREATE OR ALTER VIEW vw_FuncionarioProducao AS 
SELECT
    p.Nome AS Funcionario,
    COUNT(*) AS TotalServicos
FROM Pessoas p, Funcionarios f, Servicos_Agendados sa
WHERE p.id = f.pessoa_id 
  AND f.pessoa_id = sa.funcionario_id  
GROUP BY p.Nome
GO

-- Quantidade de Produtos por Marca
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
-- =================================
-- Criação de View para Login do Cliente

CREATE OR ALTER VIEW vw_ValidarClienteLogin AS
SELECT 
    p.id,
    p.nome,
    p.telefone,
    p.senha,
    a.nr AS agendamento_nr,
    a.codigo_rastreio,
    a.data,
    a.total,
    a.status
FROM Pessoas p, Agendamentos a, Clientes c
WHERE p.id = c.pessoa_id and c.pessoa_id = a.cliente_id
GO



-- ==========================================
-- SELECTS PARA TESTE
-- ==========================================

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



