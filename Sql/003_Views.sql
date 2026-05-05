use Fabys_Unha
go

------------TABELAS---------------

select * from Pessoas
select * from Clientes
select * from Especialidades
select * from Funcionarios
go

select * from Marcas
select * from Produtos
go

select * from Servicos
go

select * from Status
select * from Agendamentos
select * from Servicos_Agendados
select * from Produtos_Agendados
go

----------------------------------

-- ==========================================
-- GRUPO 1: Pessoas, Clientes, Especialidades, Funcionarios
-- ==========================================

-- Lista básica de Clientes com seus dados de contato
CREATE VIEW vw_ListaClientes AS
SELECT 
    p.id, 
    p.Nome, 
    p.Telefone
FROM Pessoas p
JOIN Clientes c ON p.id = c.pessoa_id;
GO


-- Lista detalhada de Funcionários, incluindo especialidade e salário
CREATE VIEW vw_ListaFuncionarios AS
SELECT 
    p.id, 
    p.Nome, 
    p.Telefone,
    e.descricao AS Especialidade,
    f.salario
FROM Pessoas p
JOIN Funcionarios f ON p.id = f.pessoa_id
LEFT JOIN Especialidades e ON f.especialidade_id = e.id;
GO


-- Agenda de Contatos Geral (Unificando quem é Cliente e quem é Funcionário)
CREATE VIEW vw_ContatosGeral AS
SELECT 
    p.Nome, 
    p.Telefone,
    CASE 
        WHEN c.pessoa_id IS NOT NULL AND f.pessoa_id IS NOT NULL THEN 'Ambos'
        WHEN c.pessoa_id IS NOT NULL THEN 'Cliente'
        WHEN f.pessoa_id IS NOT NULL THEN 'Funcionário'
        ELSE 'Apenas Pessoa'
    END AS Perfil
FROM Pessoas p
LEFT JOIN Clientes c ON p.id = c.pessoa_id
LEFT JOIN Funcionarios f ON p.id = f.pessoa_id;
GO



-- ==========================================
-- GRUPO 2: Marcas, Produtos
-- ==========================================

-- Catálogo de Produtos com o nome da Marca e formatação de preço
CREATE VIEW vw_CatalogoProdutos AS
SELECT 
    p.codigo,
    p.nome AS Produto,
    m.nome AS Marca,
    p.preco,
    p.PathImg
FROM Produtos p
JOIN Marcas m ON p.marca_id = m.id;
GO


-- Relatório de Marcas e quantidade de produtos cadastrados
CREATE VIEW vw_ResumoMarcas AS
SELECT 
    m.nome AS Marca,
    COUNT(p.codigo) AS Qtd_Produtos,
    AVG(p.preco) AS Preco_Medio
FROM Marcas m
LEFT JOIN Produtos p ON m.id = p.marca_id
GROUP BY m.nome;
GO



-- ==========================================
-- GRUPO 3: Servicos
-- ==========================================

-- Menu de Serviços ordenado por tempo de execução
CREATE VIEW vw_MenuServicos AS
SELECT 
    id,
    descricao,
    preco,
    tempo
FROM Servicos;
GO



-- ==========================================
-- GRUPO 4: Status, Agendamentos, Servicos_Agendados, Produtos_Agendados
-- ==========================================

-- Agenda Completa: O "coração" do sistema (Quem, Quando, O quê, Com quem e Status)
CREATE VIEW vw_AgendaDetalhada AS
SELECT 
    a.nr AS Agendamento_Nr,
    a.data AS Data_Agendamento,
    pc.Nome AS Cliente,
    s.descricao AS Servico,
    sa.horario AS Hora,
    pf.Nome AS Profissional,
    st.descricao AS Status_Atual,
    sa.valor AS Valor_Servico
FROM Agendamentos a
JOIN Status st ON a.status_id = st.id
JOIN Pessoas pc ON a.cliente_id = pc.id
JOIN Servicos_Agendados sa ON a.nr = sa.agendamento_nr
JOIN Servicos s ON sa.servico_id = s.id
JOIN Pessoas pf ON sa.funcionario_id = pf.id;
GO


-- Financeiro por Agendamento: Soma de serviços + soma de produtos consumidos
CREATE VIEW vw_FinanceiroAgendamento AS
SELECT 
    a.nr,
    a.data,
    p.Nome AS Cliente,
    ISNULL((SELECT SUM(valor) FROM Servicos_Agendados WHERE agendamento_nr = a.nr), 0) AS Total_Servicos,
    ISNULL((SELECT SUM(preco) FROM Produtos_Agendados WHERE agendamento_nr = a.nr), 0) AS Total_Produtos,
    a.total AS Total_Geral
FROM Agendamentos a
JOIN Pessoas p ON a.cliente_id = p.id;
GO


-- Ranking de Produtividade: Qual funcionário mais realizou serviços
CREATE VIEW vw_RankingFuncionarios AS
SELECT 
    p.Nome AS Funcionario,
    COUNT(sa.servico_id) AS Qtd_Servicos_Realizados,
    SUM(sa.valor) AS Total_Gerado
FROM Pessoas p
JOIN Servicos_Agendados sa ON p.id = sa.funcionario_id
GROUP BY p.Nome;
GO


-- Painel de Status: Quantidade de agendamentos por situação
CREATE VIEW vw_DashboardStatus AS
SELECT 
    s.descricao AS Situacao,
    COUNT(a.nr) AS Total_Ocorrencias
FROM Status s
LEFT JOIN Agendamentos a ON s.id = a.status_id
GROUP BY s.descricao;
GO


-- Auditoria de Materiais: Quais produtos foram usados em quais atendimentos
CREATE VIEW vw_UsoProdutosAtendimento AS
SELECT 
    a.nr AS Agendamento,
    a.data,
    s.descricao AS Servico_Relacionado,
    prod.nome AS Produto_Usado,
    pa.preco AS Preco_Na_Venda
FROM Agendamentos a
JOIN Servicos_Agendados sa ON a.nr = sa.agendamento_nr
JOIN Servicos s ON sa.servico_id = s.id
JOIN Produtos_Agendados pa ON sa.agendamento_nr = pa.agendamento_nr AND sa.servico_id = pa.servico_id
JOIN Produtos prod ON pa.produto_codigo = prod.codigo;
GO





-------------VIEWS----------------

select * from vw_ListaClientes
select * from vw_ListaFuncionarios
select * from vw_ContatosGeral
go

select * from vw_CatalogoProdutos
select * from vw_ResumoMarcas
go

select * from vw_MenuServicos
go

select * from vw_AgendaDetalhada
select * from vw_FinanceiroAgendamento
select * from vw_RankingFuncionarios
select * from vw_DashboardStatus
select * from vw_UsoProdutosAtendimento
go

----------------------------------