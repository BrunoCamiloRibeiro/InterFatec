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
-- TABELA: Clientes (Com lógica de Pessoas embutida)
-- =========================================================================

-- 9. Insert Cliente 
CREATE OR ALTER PROCEDURE sp_InsertCliente
    @Nome VARCHAR(100), 
    @Telefone VARCHAR(11), 
    @Status INT = 0
AS
BEGIN
    DECLARE @Pessoa_ID INT;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone)
            RAISERROR('Erro: Telefone já cadastrado.', 16, 1);

        BEGIN TRANSACTION
            INSERT INTO Pessoas (Nome, Telefone, status) VALUES (@Nome, @Telefone, @Status);
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
    @Status INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE pessoa_id = @Pessoa_ID)
            RAISERROR('Erro: Cliente não encontrado.', 16, 1);
            
        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone AND id <> @Pessoa_ID)
            RAISERROR('Erro: Telefone já pertence a outra pessoa.', 16, 1);

        -- Como Clientes só tem PK, atualizamos apenas a tabela Pessoas
        UPDATE Pessoas 
        SET Nome = @Nome, Telefone = @Telefone, status = @Status 
        WHERE id = @Pessoa_ID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- =========================================================================
-- TABELA: Funcionarios (Com lógica de Pessoas embutida)
-- =========================================================================

-- 11. Insert Funcionario
CREATE OR ALTER PROCEDURE sp_InsertFuncionario
    @Nome VARCHAR(100), 
    @Telefone VARCHAR(11), 
    @Status INT = 0, 
    @Salario DECIMAL(10,2), 
    @Especialidade_Id INT,
    @Senha VARCHAR(25)
AS
BEGIN
    DECLARE @Pessoa_ID INT;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone)
            RAISERROR('Erro: Telefone já cadastrado.', 16, 1);

        IF @Salario < 1412.00
            RAISERROR('Erro: O salário deve ser igual ou superior a 1412.00', 16, 1);

        BEGIN TRANSACTION
            INSERT INTO Pessoas (Nome, Telefone, status) VALUES (@Nome, @Telefone, @Status);
            SET @Pessoa_ID = SCOPE_IDENTITY();
            INSERT INTO Funcionarios(pessoa_id, salario, especialidade_id, senha) VALUES (@Pessoa_ID, @Salario, @Especialidade_Id, @Senha);
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
    @Senha VARCHAR(25)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Funcionarios WHERE pessoa_id = @Pessoa_ID)
            RAISERROR('Erro: Funcionario não encontrado.', 16, 1);

        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone AND id <> @Pessoa_ID)
            RAISERROR('Erro: Telefone já pertence a outra pessoa.', 16, 1);

        IF @Salario < 1412.00
            RAISERROR('Erro: O salário deve ser igual ou superior a 1412.00', 16, 1);

        BEGIN TRANSACTION
            UPDATE Pessoas 
            SET Nome = @Nome, Telefone = @Telefone, status = @Status 
            WHERE id = @Pessoa_ID;
            
            UPDATE Funcionarios 
            SET salario = @Salario, especialidade_id = @Especialidade_Id, senha = @Senha 
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

-- 15. Insert Servico_Agendado (Validação inline com JOIN IMPLÍCITO)
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

    -- JOIN Implícito (vírgula no FROM) para checagem de conflito
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
        RAISERROR('Erro: Este funcionário já possui um serviço agendado neste horário exato.', 16, 1);
        RETURN;
    END

    INSERT INTO Servicos_Agendados (agendamento_nr, servico_id, obs, horario, funcionario_id, valor)
    VALUES (@Agendamento_nr, @Servico_id, @Obs, @Horario, @Funcionario_id, ISNULL(@Valor, (SELECT preco FROM Servicos WHERE id = @Servico_id)));
END
GO

-- 16. Update Servico_Agendado (Validação inline com JOIN IMPLÍCITO)
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

    -- JOIN Implícito ignorando o próprio agendamento atual
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
        RAISERROR('Erro: Este funcionário já possui um serviço agendado neste horário exato.', 16, 1);
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
-- AUXILIAR: CRIAÇÃO CONJUNTA DE AGENDAMENTO SIMPLES
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
            RAISERROR('Erro: Este funcionário já possui um serviço agendado neste horário exato.', 16, 1);
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