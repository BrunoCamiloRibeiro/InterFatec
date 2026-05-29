-- =====================================================================
-- SCRIPT: 007_Alteracoes_FuncionarioNullable.sql
-- Objetivo: Permitir cadastro de funcionário sem salário e especialidade
--           (serão preenchidos depois na área administrativa)
-- Data: 29/05/2026
-- =====================================================================

USE Fabys_Unha
GO

-- =====================================================================
-- 1. REMOVER CONSTRAINT DE SALÁRIO MÍNIMO
-- =====================================================================

ALTER TABLE Funcionarios
DROP CONSTRAINT CK_Funcionarios_SalarioMin
GO

-- =====================================================================
-- 2. ALTERAR COLUNA salario PARA ACEITAR NULL
-- =====================================================================

ALTER TABLE Funcionarios
ALTER COLUMN salario DECIMAL(10,2) NULL
GO

-- =====================================================================
-- 3. ALTERAR COLUNA especialidade_id PARA ACEITAR NULL
-- =====================================================================

ALTER TABLE Funcionarios
ALTER COLUMN especialidade_id INT NULL
GO

-- =====================================================================
-- 4. ATUALIZAR PROCEDURE DE INSERT FUNCIONARIO
--    (salario e especialidade_id agora são opcionais)
-- =====================================================================

ALTER PROCEDURE sp_InsertFuncionario
    @Nome VARCHAR(100), 
    @Telefone VARCHAR(11), 
    @Status INT = 0, 
    @Salario DECIMAL(10,2) = NULL, 
    @Especialidade_Id INT = NULL,
    @Senha VARCHAR(MAX) = ''
AS
BEGIN
    DECLARE @Pessoa_ID INT;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone)
            RAISERROR('Erro: Telefone já cadastrado.', 16, 1);

        IF @Salario IS NOT NULL AND @Salario < 1412.00
            RAISERROR('Erro: O salário deve ser igual ou superior a 1412.00', 16, 1);

        BEGIN TRANSACTION
            INSERT INTO Pessoas (Nome, Telefone, status, senha) 
            VALUES (@Nome, @Telefone, @Status, @Senha);
            SET @Pessoa_ID = SCOPE_IDENTITY();
            INSERT INTO Funcionarios(pessoa_id, salario, especialidade_id) 
            VALUES (@Pessoa_ID, @Salario, @Especialidade_Id);
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- =====================================================================
-- 5. ATUALIZAR PROCEDURE DE UPDATE FUNCIONARIO
--    (salario validado somente quando informado)
-- =====================================================================

ALTER PROCEDURE sp_UpdateFuncionario
    @Pessoa_ID INT, 
    @Nome VARCHAR(100), 
    @Telefone VARCHAR(11), 
    @Status INT, 
    @Salario DECIMAL(10,2) = NULL, 
    @Especialidade_Id INT = NULL,
    @Senha VARCHAR(MAX) = ''
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Funcionarios WHERE pessoa_id = @Pessoa_ID)
            RAISERROR('Erro: Funcionario não encontrado.', 16, 1);

        IF EXISTS (SELECT 1 FROM Pessoas WHERE Telefone = @Telefone AND id <> @Pessoa_ID)
            RAISERROR('Erro: Telefone já pertence a outra pessoa.', 16, 1);

        IF @Salario IS NOT NULL AND @Salario < 1412.00
            RAISERROR('Erro: O salário deve ser igual ou superior a 1412.00', 16, 1);

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

-- =====================================================================
-- VERIFICAÇÃO FINAL
-- =====================================================================

PRINT '✓ Script 007 Executado com Sucesso!'
PRINT '✓ Constraint CK_Funcionarios_SalarioMin removida'
PRINT '✓ Coluna "salario" agora aceita NULL'
PRINT '✓ Coluna "especialidade_id" agora aceita NULL'
PRINT '✓ Procedures atualizadas para parâmetros opcionais'
GO
