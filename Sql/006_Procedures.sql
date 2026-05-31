USE Fabys_Unha;
GO

-- =========================================================================
-- TABELA: Especialidades
-- =========================================================================

-- 1. Insert Especialidade
CREATE OR ALTER PROCEDURE sp_InsertEspecialidade
    @Descricao VARCHAR(80), 
    @Status INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Especialidades (descricao, status) 
    VALUES (@Descricao, @Status);
END
GO

-- 2. Update Especialidade
CREATE OR ALTER PROCEDURE sp_UpdateEspecialidade
    @Id INT, 
    @Descricao VARCHAR(80), 
    @Status INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Especialidades 
    SET descricao = @Descricao, status = @Status 
    WHERE id = @Id;
END
GO

-- =========================================================================
-- TABELA: Marcas
-- =========================================================================

-- 3. Insert Marca
CREATE OR ALTER PROCEDURE sp_InsertMarca
    @Nome VARCHAR(50), 
    @Status INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Marcas (nome, status) 
    VALUES (@Nome, @Status);
END
GO

-- 4. Update Marca
CREATE OR ALTER PROCEDURE sp_UpdateMarca
    @Id INT, 
    @Nome VARCHAR(50), 
    @Status INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Marcas 
    SET nome = @Nome, status = @Status 
    WHERE id = @Id;
END
GO

-- =========================================================================
-- TABELA: Servicos
-- =========================================================================

-- 5. Insert Servico
CREATE OR ALTER PROCEDURE sp_InsertServico
    @Preco DECIMAL(10,2), 
    @Descricao VARCHAR(80), 
    @Tempo TIME(0), 
    @Status INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Servicos (preco, descricao, tempo, status) 
    VALUES (@Preco, @Descricao, @Tempo, @Status);
END
GO

-- 6. Update Servico
CREATE OR ALTER PROCEDURE sp_UpdateServico
    @Id INT, 
    @Preco DECIMAL(10,2), 
    @Descricao VARCHAR(80), 
    @Tempo TIME(0), 
    @Status INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Servicos 
    SET preco = @Preco, descricao = @Descricao, tempo = @Tempo, status = @Status 
    WHERE id = @Id;
END
GO

-- =========================================================================
-- TABELA: Produtos 
-- =========================================================================

-- 7. Insert Produto
CREATE OR ALTER PROCEDURE sp_InsertProduto
    @Nome VARCHAR(50), 
    @Marca_Id INT, 
    @Preco DECIMAL(10,2), 
    @PathImagem VARCHAR(100), 
    @Status INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Produtos (nome, marca_id, preco, PathImagem, status) 
    VALUES (@Nome, @Marca_Id, @Preco, @PathImagem, @Status);
END
GO

-- 8. Update Produto
CREATE OR ALTER PROCEDURE sp_UpdateProduto
    @Codigo INT, 
    @Nome VARCHAR(50), 
    @Marca_Id INT, 
    @Preco DECIMAL(10,2), 
    @PathImagem VARCHAR(100), 
    @Status INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Produtos 
    SET nome = @Nome, marca_id = @Marca_Id, preco = @Preco, PathImagem = @PathImagem, status = @Status 
    WHERE codigo = @Codigo;
END
GO

-- =========================================================================
-- TABELA: Clientes (Com l�gica de Pessoas embutida)
-- =========================================================================

-- 9. Insert Cliente 
CREATE OR ALTER PROCEDURE sp_InsertCliente
    @Nome VARCHAR(100), 
    @Telefone VARCHAR(11), 
    @Status INT = 0,
    @Senha VARCHAR(100) = ''
AS
BEGIN
    DECLARE @Pessoa_ID INT;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone)
            RAISERROR('Erro: Telefone j� cadastrado.', 16, 1);

        BEGIN TRANSACTION
            INSERT INTO Pessoas (Nome, Telefone, status, senha) VALUES (@Nome, @Telefone, @Status, @Senha);
            SET @Pessoa_ID = SCOPE_IDENTITY();
            INSERT INTO Clientes (pessoa_id) VALUES (@Pessoa_ID);
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- 10. Update Cliente
CREATE OR ALTER PROCEDURE sp_UpdateCliente
    @Pessoa_ID INT, 
    @Nome VARCHAR(100), 
    @Telefone VARCHAR(11), 
    @Status INT,
    @Senha VARCHAR(100) = ''
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE pessoa_id = @Pessoa_ID)
            RAISERROR('Erro: Cliente n�o encontrado.', 16, 1);
            
        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone AND id <> @Pessoa_ID)
            RAISERROR('Erro: Telefone j� pertence a outra pessoa.', 16, 1);

        -- Como Clientes s� tem PK, atualizamos apenas a tabela Pessoas
        UPDATE Pessoas 
        SET Nome = @Nome, Telefone = @Telefone, status = @Status, senha = @Senha
        WHERE id = @Pessoa_ID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- =========================================================================
-- TABELA: Funcionarios (Com l�gica de Pessoas embutida)
-- =========================================================================

-- 11. Insert Funcionario
CREATE OR ALTER PROCEDURE sp_InsertFuncionario
    @Nome VARCHAR(100), 
    @Telefone VARCHAR(11), 
    @Status INT = 0, 
    @Salario DECIMAL(10,2), 
    @Especialidade_Id INT,
    @Senha VARCHAR(100)
AS
BEGIN
    DECLARE @Pessoa_ID INT;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone)
            RAISERROR('Erro: Telefone j� cadastrado.', 16, 1);

        IF @Salario < 1412.00
            RAISERROR('Erro: O sal�rio deve ser igual ou superior a 1412.00', 16, 1);

        BEGIN TRANSACTION
            INSERT INTO Pessoas (Nome, Telefone, status, senha) VALUES (@Nome, @Telefone, @Status, @Senha);
            SET @Pessoa_ID = SCOPE_IDENTITY();
            INSERT INTO Funcionarios(pessoa_id, salario, especialidade_id) VALUES (@Pessoa_ID, @Salario, @Especialidade_Id);
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- 12. Update Funcionario
CREATE OR ALTER PROCEDURE sp_UpdateFuncionario
    @Pessoa_ID INT, 
    @Nome VARCHAR(100), 
    @Telefone VARCHAR(11), 
    @Status INT, 
    @Salario DECIMAL(10,2), 
    @Especialidade_Id INT,
    @Senha VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Funcionarios WHERE pessoa_id = @Pessoa_ID)
            RAISERROR('Erro: Funcionario n�o encontrado.', 16, 1);

        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone AND id <> @Pessoa_ID)
            RAISERROR('Erro: Telefone j� pertence a outra pessoa.', 16, 1);

        IF @Salario < 1412.00
            RAISERROR('Erro: O sal�rio deve ser igual ou superior a 1412.00', 16, 1);

        BEGIN TRANSACTION
            UPDATE Pessoas 
            SET Nome = @Nome, Telefone = @Telefone, status = @Status, senha = @Senha 
            WHERE id = @Pessoa_ID;
            
            UPDATE Funcionarios 
            SET salario = @Salario, especialidade_id = @Especialidade_Id
            WHERE pessoa_id = @Pessoa_ID;
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- =========================================================================
-- TABELA: Agendamentos
-- =========================================================================

-- 13. Insert Agendamento 
CREATE OR ALTER PROCEDURE sp_InsertAgendamento
    @Data DATETIME2(0), 
    @Total DECIMAL(10,2), 
    @Cliente_id INT, 
    @Status INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Agendamentos (data, total, cliente_id, status)
    VALUES (@Data, @Total, @Cliente_id, @Status);
    
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

-- 14. Update Agendamento
CREATE OR ALTER PROCEDURE sp_UpdateAgendamento
    @Nr INT, 
    @Data DATETIME2(0), 
    @Total DECIMAL(10,2), 
    @Cliente_id INT, 
    @Status INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Agendamentos 
    SET data = @Data, total = @Total, cliente_id = @Cliente_id, status = @Status 
    WHERE nr = @Nr;
END
GO

-- =========================================================================
-- TABELA: Servicos_Agendados
-- =========================================================================

-- 15. Insert Servico_Agendado (Valida��o inline com JOIN IMPL�CITO)
CREATE OR ALTER PROCEDURE sp_InsertServicoAgendado
    @Agendamento_nr INT, 
    @Servico_id INT, 
    @Obs VARCHAR(200), 
    @Horario TIME, 
    @Funcionario_id INT, 
    @Valor DECIMAL(10,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Data DATETIME2(0);
    SELECT @Data = data FROM Agendamentos WHERE nr = @Agendamento_nr;

    -- JOIN Impl�cito (v�rgula no FROM) para checagem de conflito
    IF EXISTS (
        SELECT 1 
        FROM Servicos_Agendados sa, Agendamentos a
        WHERE sa.agendamento_nr = a.nr
          AND sa.funcionario_id = @Funcionario_id 
          AND CAST(a.data AS DATE) = CAST(@Data AS DATE) 
          AND sa.horario = @Horario 
          AND a.status <> 1
    )
    BEGIN
        RAISERROR('Erro: Este funcion�rio j� possui um servi�o agendado neste hor�rio exato.', 16, 1);
        RETURN;
    END

    INSERT INTO Servicos_Agendados (agendamento_nr, servico_id, obs, horario, funcionario_id, valor)
    VALUES (@Agendamento_nr, @Servico_id, @Obs, @Horario, @Funcionario_id, ISNULL(@Valor, (SELECT preco FROM Servicos WHERE id = @Servico_id)));
END
GO

-- 16. Update Servico_Agendado (Valida��o inline com JOIN IMPL�CITO)
CREATE OR ALTER PROCEDURE sp_UpdateServicoAgendado
    @Agendamento_nr INT, 
    @Servico_id INT, 
    @Obs VARCHAR(200), 
    @Horario TIME, 
    @Funcionario_id INT, 
    @Valor DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Data DATETIME2(0);
    SELECT @Data = data FROM Agendamentos WHERE nr = @Agendamento_nr;

    -- JOIN Impl�cito ignorando o pr�prio agendamento atual
    IF EXISTS (
        SELECT 1 
        FROM Servicos_Agendados sa, Agendamentos a
        WHERE sa.agendamento_nr = a.nr
          AND sa.funcionario_id = @Funcionario_id 
          AND CAST(a.data AS DATE) = CAST(@Data AS DATE) 
          AND sa.horario = @Horario 
          AND a.status <> 1
          AND sa.agendamento_nr <> @Agendamento_nr
    )
    BEGIN
        RAISERROR('Erro: Este funcion�rio j� possui um servi�o agendado neste hor�rio exato.', 16, 1);
        RETURN;
    END

    UPDATE Servicos_Agendados
    SET obs = @Obs, horario = @Horario, funcionario_id = @Funcionario_id, valor = @Valor
    WHERE agendamento_nr = @Agendamento_nr AND servico_id = @Servico_id;
END
GO

-- =========================================================================
-- TABELA: Produtos_Agendados 
-- =========================================================================

-- 17. Insert Produto_Agendado
CREATE OR ALTER PROCEDURE sp_InsertProdutoAgendado
    @Agendamento_nr INT, 
    @Servico_id INT, 
    @Produto_codigo INT, 
    @Preco DECIMAL(10,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Produtos_Agendados (agendamento_nr, servico_id, produto_codigo, preco)
    VALUES (@Agendamento_nr, @Servico_id, @Produto_codigo, ISNULL(@Preco, (SELECT preco FROM Produtos WHERE codigo = @Produto_codigo)));
END
GO

-- 18. Update Produto_Agendado
CREATE OR ALTER PROCEDURE sp_UpdateProdutoAgendado
    @Agendamento_nr INT, 
    @Servico_id INT, 
    @Produto_codigo INT, 
    @Preco DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Produtos_Agendados
    SET preco = @Preco
    WHERE agendamento_nr = @Agendamento_nr 
      AND servico_id = @Servico_id 
      AND produto_codigo = @Produto_codigo;
END
GO

-- =========================================================================
-- AUXILIAR: CRIA��O CONJUNTA DE AGENDAMENTO SIMPLES
-- =========================================================================

-- 19. Procedure Auxiliar - Agendamento Completo
CREATE OR ALTER PROCEDURE sp_CriarAgendamento (
    @Cliente_id INT, 
    @Data DATETIME2(0), 
    @Total DECIMAL(10,2), 
    @Servico_id INT,
    @Funcionario_id INT, 
    @Horario TIME, 
    @Obs VARCHAR(200) = '', 
    @Valor DECIMAL(10,2) = NULL
)
AS 
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (
            SELECT 1 
            FROM Servicos_Agendados sa, Agendamentos a
            WHERE sa.agendamento_nr = a.nr
              AND sa.funcionario_id = @Funcionario_id 
              AND CAST(a.data AS DATE) = CAST(@Data AS DATE) 
              AND sa.horario = @Horario 
              AND a.status <> 1
        )
        BEGIN
            RAISERROR('Erro: Este funcion�rio j� possui um servi�o agendado neste hor�rio exato.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION
        DECLARE @Nr_agendamentos INT;

        INSERT INTO Agendamentos (data, total, cliente_id, status) 
        VALUES (@Data, @Total, @Cliente_id, 0);
        
        SET @Nr_agendamentos = SCOPE_IDENTITY();

        INSERT INTO Servicos_Agendados (agendamento_nr, servico_id, obs, horario, funcionario_id, valor)
        VALUES (@Nr_agendamentos, @Servico_id, @Obs, @Horario, @Funcionario_id, ISNULL(@Valor, (SELECT preco FROM Servicos WHERE id = @Servico_id)));

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- =========================================================================
-- AUXILIAR: VALIDAÇÃO DE CLIENTE TEMPORÁRIO
-- =========================================================================

-- 20. Aqui ele vai usar o token dentro de agendamento para uma sessão temporária e permitir a a criação sem cadastro

CREATE OR ALTER PROCEDURE sp_ValidarAcessoAgendamento
    @Telefone VARCHAR(11)
AS
BEGIN
    SELECT a.* FROM Agendamentos a,Clientes c, Pessoas p
    WHERE p.id = c.pessoa_id and c.pessoa_id = a.cliente_id and
    p.Telefone = @Telefone AND p.senha <> '' -- Ou outra lógica de validação de login
END
GO

-- =========================================================================
-- AUXILIAR: VALIDAÇÃO DE CLIENTE SEM AGENDAMENTO
-- =========================================================================
-- (Sim eu não vi isso antes)

-- Procedure focada apenas em criar o usuário no sistema
CREATE OR ALTER PROCEDURE sp_CadastroDeCliente
    @Nome VARCHAR(100), 
    @Telefone VARCHAR(11), 
    @Senha VARCHAR(100) = ''
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone)
    BEGIN
        RAISERROR('Erro: Telefone já cadastrado.', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION
        INSERT INTO Pessoas (Nome, Telefone, status, senha) 
        VALUES (@Nome, @Telefone, 1, @Senha); -- Status 1 = Ativo
        
        DECLARE @Pessoa_ID INT = SCOPE_IDENTITY();
        
        INSERT INTO Clientes (pessoa_id) 
        VALUES (@Pessoa_ID);
    COMMIT 
END
GO