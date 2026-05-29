USE Fabys_Unha;
GO

-- ==========================================
-- 1. ESPECIALIDADES
-- ==========================================
EXEC sp_InsertEspecialidade @Descricao = 'Manicure Clássica';
EXEC sp_InsertEspecialidade @Descricao = 'Pedicure Clássica';
EXEC sp_InsertEspecialidade @Descricao = 'Alongamento em Gel';
EXEC sp_InsertEspecialidade @Descricao = 'Alongamento em Fibra';
EXEC sp_InsertEspecialidade @Descricao = 'Spa dos Pés';
EXEC sp_InsertEspecialidade @Descricao = 'Plástica dos Pés';
EXEC sp_InsertEspecialidade @Descricao = 'Francesinha Definitiva';
EXEC sp_InsertEspecialidade @Descricao = 'Esmaltação em Gel';
EXEC sp_InsertEspecialidade @Descricao = 'Nail Art 3D';
EXEC sp_InsertEspecialidade @Descricao = 'Podologia Básica';
EXEC sp_InsertEspecialidade @Descricao = 'Cutilagem Russa';
EXEC sp_InsertEspecialidade @Descricao = 'Banho de Gel';
EXEC sp_InsertEspecialidade @Descricao = 'Manutenção de Gel';
EXEC sp_InsertEspecialidade @Descricao = 'Remoção de Alongamento';
EXEC sp_InsertEspecialidade @Descricao = 'Massagem Relaxante Pés';
EXEC sp_InsertEspecialidade @Descricao = 'Reflexologia Podal';
EXEC sp_InsertEspecialidade @Descricao = 'Blindagem de Unhas';
EXEC sp_InsertEspecialidade @Descricao = 'Reconstrução de Unha';
EXEC sp_InsertEspecialidade @Descricao = 'Decoração com Pedrarias';
EXEC sp_InsertEspecialidade @Descricao = 'Esmaltação Infantil';
GO

-- ==========================================
-- 2. CLIENTES (com senha conforme regra)
-- ==========================================
-- Visitantes (com Token) → senha vazia
EXEC sp_InsertCliente @Nome = 'Ana Clara',     @Telefone = '11988880001', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Bruno Costa',   @Telefone = '11988880002', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Carla Dias',    @Telefone = '11988880003', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Daniela Silva', @Telefone = '11988880004', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Eduardo Lima',  @Telefone = '11988880005', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Fernanda Alves',@Telefone = '11988880006', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Gabriela Santos',@Telefone = '11988880007', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Helena Souza',  @Telefone = '11988880008', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Igor Martins',  @Telefone = '11988880009', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Juliana Rocha', @Telefone = '11988880010', @Senha = '';
EXEC sp_InsertCliente @Nome = 'Karla Mendes',  @Telefone = '11988880011', @Senha = '';

-- Clientes Cadastrados (sem Token) → com senha
EXEC sp_InsertCliente @Nome = 'Lucas Ferreira',@Telefone = '11988880012', @Senha = 'senha123';
EXEC sp_InsertCliente @Nome = 'Marcos Silva',  @Telefone = '11988880013', @Senha = 'senha123';
EXEC sp_InsertCliente @Nome = 'Nathalia Ribeiro',@Telefone = '11988880014',@Senha = 'senha123';
EXEC sp_InsertCliente @Nome = 'Otavio Pires',  @Telefone = '11988880015', @Senha = 'senha123';
EXEC sp_InsertCliente @Nome = 'Patricia Castro',@Telefone = '11988880016', @Senha = 'senha123';
EXEC sp_InsertCliente @Nome = 'Quiriana Gomes',@Telefone = '11988880017', @Senha = 'senha123';
EXEC sp_InsertCliente @Nome = 'Rafaela Moraes',@Telefone = '11988880018', @Senha = 'senha123';
EXEC sp_InsertCliente @Nome = 'Sabrina Nunes', @Telefone = '11988880019', @Senha = 'senha123';
EXEC sp_InsertCliente @Nome = 'Tatiana Barros',@Telefone = '11988880020', @Senha = 'senha123';
GO

-- ==========================================
-- 3. FUNCIONÁRIOS
-- ==========================================
EXEC sp_InsertFuncionario @Nome = 'Ursula Farias',    @Telefone = '11988880021', @Salario = 2500.00, @Especialidade_Id = 1,  @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Valeria Pinto',    @Telefone = '11988880022', @Salario = 2600.00, @Especialidade_Id = 2,  @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Wagner Melo',      @Telefone = '11988880023', @Salario = 3500.00, @Especialidade_Id = 3,  @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Xenia Viana',      @Telefone = '11988880024', @Salario = 3800.00, @Especialidade_Id = 4,  @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Yara Teixeira',    @Telefone = '11988880025', @Salario = 2200.00, @Especialidade_Id = 5,  @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Zelia Dantas',     @Telefone = '11988880026', @Salario = 3000.00, @Especialidade_Id = 6,  @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Alice Carvalho',   @Telefone = '11988880027', @Salario = 2800.00, @Especialidade_Id = 7,  @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Breno Nogueira',   @Telefone = '11988880028', @Salario = 3100.00, @Especialidade_Id = 8,  @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Camila Borges',    @Telefone = '11988880029', @Salario = 3600.00, @Especialidade_Id = 9,  @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Diego Leite',      @Telefone = '11988880030', @Salario = 4000.00, @Especialidade_Id = 10, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Elisa Monteiro',   @Telefone = '11988880031', @Salario = 3200.00, @Especialidade_Id = 11, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Fabio Tavares',    @Telefone = '11988880032', @Salario = 2900.00, @Especialidade_Id = 12, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Giovana Lemos',    @Telefone = '11988880033', @Salario = 1900.00, @Especialidade_Id = 13, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Hugo Resende',     @Telefone = '11988880034', @Salario = 1800.00, @Especialidade_Id = 14, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Isabela Macedo',   @Telefone = '11988880035', @Salario = 2100.00, @Especialidade_Id = 15, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Joao Batista',     @Telefone = '11988880036', @Salario = 2500.00, @Especialidade_Id = 16, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Karen Peixoto',    @Telefone = '11988880037', @Salario = 2700.00, @Especialidade_Id = 17, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Leonardo Guedes',  @Telefone = '11988880038', @Salario = 2000.00, @Especialidade_Id = 18, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Marcos Vianna',    @Telefone = '11988880039', @Salario = 1700.00, @Especialidade_Id = 19, @Senha = 'senha123';
EXEC sp_InsertFuncionario @Nome = 'Neide Camargo',    @Telefone = '11988880040', @Salario = 1500.00, @Especialidade_Id = 20, @Senha = 'senha123';
GO

-- ==========================================
-- 4. MARCAS
-- ==========================================
EXEC sp_InsertMarca @Nome = 'Risqué';
EXEC sp_InsertMarca @Nome = 'Colorama';
EXEC sp_InsertMarca @Nome = 'Impala';
EXEC sp_InsertMarca @Nome = 'Dailus';
EXEC sp_InsertMarca @Nome = 'Vult';
EXEC sp_InsertMarca @Nome = 'Anita';
EXEC sp_InsertMarca @Nome = 'Avon';
EXEC sp_InsertMarca @Nome = 'Natura';
EXEC sp_InsertMarca @Nome = 'OPI';
EXEC sp_InsertMarca @Nome = 'Essie';
EXEC sp_InsertMarca @Nome = 'Hits';
EXEC sp_InsertMarca @Nome = 'Blant';
EXEC sp_InsertMarca @Nome = 'Studio 35';
EXEC sp_InsertMarca @Nome = 'Haskell';
EXEC sp_InsertMarca @Nome = 'Latika';
EXEC sp_InsertMarca @Nome = 'Bella Brazil';
EXEC sp_InsertMarca @Nome = 'Top Beauty';
EXEC sp_InsertMarca @Nome = 'Novo Toque';
EXEC sp_InsertMarca @Nome = 'Nati';
EXEC sp_InsertMarca @Nome = 'Cora';
GO

-- ==========================================
-- 5. PRODUTOS
-- ==========================================
EXEC sp_InsertProduto @Nome = 'Esmalte Vermelho Carmim', @Marca_Id = 1, @Preco = 8.50, @PathImagem = '/img/prod01.jpg';
EXEC sp_InsertProduto @Nome = 'Base Fortalecedora', @Marca_Id = 2, @Preco = 9.00, @PathImagem = '/img/prod02.jpg';
EXEC sp_InsertProduto @Nome = 'Top Coat Brilho', @Marca_Id = 3, @Preco = 12.00, @PathImagem = '/img/prod03.jpg';
EXEC sp_InsertProduto @Nome = 'Esmalte Preto Fosco', @Marca_Id = 4, @Preco = 10.50, @PathImagem = '/img/prod04.jpg';
EXEC sp_InsertProduto @Nome = 'Esmalte Branco Puríssimo', @Marca_Id = 5, @Preco = 8.90, @PathImagem = '/img/prod05.jpg';
EXEC sp_InsertProduto @Nome = 'Removedor de Esmalte', @Marca_Id = 6, @Preco = 15.00, @PathImagem = '/img/prod06.jpg';
EXEC sp_InsertProduto @Nome = 'Algodão 500g', @Marca_Id = 7, @Preco = 20.00, @PathImagem = '/img/prod07.jpg';
EXEC sp_InsertProduto @Nome = 'Creme Esfoliante Pés', @Marca_Id = 8, @Preco = 35.00, @PathImagem = '/img/prod08.jpg';
EXEC sp_InsertProduto @Nome = 'Gel Construtor Clear', @Marca_Id = 9, @Preco = 85.00, @PathImagem = '/img/prod09.jpg';
EXEC sp_InsertProduto @Nome = 'Fibra de Vidro Rolo', @Marca_Id = 10, @Preco = 45.00, @PathImagem = '/img/prod10.jpg';
EXEC sp_InsertProduto @Nome = 'Primer Ácido', @Marca_Id = 11, @Preco = 25.00, @PathImagem = '/img/prod11.jpg';
EXEC sp_InsertProduto @Nome = 'Óleo Secante', @Marca_Id = 12, @Preco = 7.50, @PathImagem = '/img/prod12.jpg';
EXEC sp_InsertProduto @Nome = 'Lixa Banana (Pct 100)', @Marca_Id = 13, @Preco = 30.00, @PathImagem = '/img/prod13.jpg';
EXEC sp_InsertProduto @Nome = 'Espátula Inox', @Marca_Id = 14, @Preco = 18.00, @PathImagem = '/img/prod14.jpg';
EXEC sp_InsertProduto @Nome = 'Alicate de Cutícula', @Marca_Id = 15, @Preco = 40.00, @PathImagem = '/img/prod15.jpg';
EXEC sp_InsertProduto @Nome = 'Prep Higienizador', @Marca_Id = 16, @Preco = 22.00, @PathImagem = '/img/prod16.jpg';
EXEC sp_InsertProduto @Nome = 'Esmalte Nude Clássico', @Marca_Id = 17, @Preco = 9.50, @PathImagem = '/img/prod17.jpg';
EXEC sp_InsertProduto @Nome = 'Esmalte Neon Rosa', @Marca_Id = 18, @Preco = 11.00, @PathImagem = '/img/prod18.jpg';
EXEC sp_InsertProduto @Nome = 'Base Bomba Aceleradora', @Marca_Id = 19, @Preco = 14.00, @PathImagem = '/img/prod19.jpg';
EXEC sp_InsertProduto @Nome = 'Amolecedor de Cutículas', @Marca_Id = 20, @Preco = 13.50, @PathImagem = '/img/prod20.jpg';
GO

-- ==========================================
-- 6. SERVIÇOS
-- ==========================================
EXEC sp_InsertServico @Preco = 35.00, @Descricao = 'Manicure Simples', @Tempo = '00:45:00';
EXEC sp_InsertServico @Preco = 40.00, @Descricao = 'Pedicure Simples', @Tempo = '00:50:00';
EXEC sp_InsertServico @Preco = 70.00, @Descricao = 'Mão e Pé Casadinho', @Tempo = '01:30:00';
EXEC sp_InsertServico @Preco = 150.00, @Descricao = 'Alongamento Fibra de Vidro', @Tempo = '02:30:00';
EXEC sp_InsertServico @Preco = 130.00, @Descricao = 'Alongamento em Gel', @Tempo = '02:00:00';
EXEC sp_InsertServico @Preco = 80.00, @Descricao = 'Manutenção de Alongamento', @Tempo = '01:30:00';
EXEC sp_InsertServico @Preco = 60.00, @Descricao = 'Banho de Gel', @Tempo = '01:00:00';
EXEC sp_InsertServico @Preco = 40.00, @Descricao = 'Remoção de Alongamento', @Tempo = '00:40:00';
EXEC sp_InsertServico @Preco = 55.00, @Descricao = 'Spa dos Pés Completo', @Tempo = '01:00:00';
EXEC sp_InsertServico @Preco = 85.00, @Descricao = 'Plástica dos Pés', @Tempo = '01:15:00';
EXEC sp_InsertServico @Preco = 25.00, @Descricao = 'Apenas Esmaltação', @Tempo = '00:20:00';
EXEC sp_InsertServico @Preco = 45.00, @Descricao = 'Esmaltação em Gel', @Tempo = '00:45:00';
EXEC sp_InsertServico @Preco = 15.00, @Descricao = 'Francesinha/Inglesinha', @Tempo = '00:15:00';
EXEC sp_InsertServico @Preco = 20.00, @Descricao = 'Nail Art (por unha)', @Tempo = '00:15:00';
EXEC sp_InsertServico @Preco = 10.00, @Descricao = 'Conserto de Unha (unidade)', @Tempo = '00:10:00';
EXEC sp_InsertServico @Preco = 90.00, @Descricao = 'Podologia Preventiva', @Tempo = '01:00:00';
EXEC sp_InsertServico @Preco = 45.00, @Descricao = 'Cutilagem Russa Mão', @Tempo = '00:45:00';
EXEC sp_InsertServico @Preco = 50.00, @Descricao = 'Blindagem Diamante', @Tempo = '01:00:00';
EXEC sp_InsertServico @Preco = 30.00, @Descricao = 'Massagem Relaxante Pés', @Tempo = '00:30:00';
EXEC sp_InsertServico @Preco = 35.00, @Descricao = 'Manicure Infantil', @Tempo = '00:30:00';
GO

-- ==========================================
-- 7. AGENDAMENTOS
-- ==========================================
-- Visitantes (com Token)
EXEC sp_InsertAgendamento @Data = '2026-05-10', @Total = 35.00, @Cliente_id = 1, @Status = 2, @CodigoRastreio = 'TOK001';
EXEC sp_InsertAgendamento @Data = '2026-05-11', @Total = 40.00, @Cliente_id = 2, @Status = 2, @CodigoRastreio = 'TOK002';
EXEC sp_InsertAgendamento @Data = '2026-05-12', @Total = 70.00, @Cliente_id = 3, @Status = 0, @CodigoRastreio = 'TOK003';
EXEC sp_InsertAgendamento @Data = '2026-05-13', @Total = 150.00, @Cliente_id = 4, @Status = 0, @CodigoRastreio = 'TOK004';
EXEC sp_InsertAgendamento @Data = '2026-05-14', @Total = 130.00, @Cliente_id = 5, @Status = 0, @CodigoRastreio = 'TOK005';
EXEC sp_InsertAgendamento @Data = '2026-05-15', @Total = 80.00, @Cliente_id = 6, @Status = 0, @CodigoRastreio = 'TOK006';
EXEC sp_InsertAgendamento @Data = '2026-05-16', @Total = 60.00, @Cliente_id = 7, @Status = 1, @CodigoRastreio = 'TOK007';
EXEC sp_InsertAgendamento @Data = '2026-05-17', @Total = 40.00, @Cliente_id = 8, @Status = 1, @CodigoRastreio = 'TOK008';
EXEC sp_InsertAgendamento @Data = '2026-05-18', @Total = 55.00, @Cliente_id = 9, @Status = 1, @CodigoRastreio = 'TOK009';
EXEC sp_InsertAgendamento @Data = '2026-05-19', @Total = 85.00, @Cliente_id = 10, @Status = 1, @CodigoRastreio = 'TOK010';
EXEC sp_InsertAgendamento @Data = '2026-05-20', @Total = 25.00, @Cliente_id = 11, @Status = 0, @CodigoRastreio = 'TOK011';

-- Clientes Cadastrados (sem Token)
EXEC sp_InsertAgendamento @Data = '2026-05-21', @Total = 45.00, @Cliente_id = 12, @Status = 0, @CodigoRastreio = NULL;
EXEC sp_InsertAgendamento @Data = '2026-05-22', @Total = 15.00, @Cliente_id = 13, @Status = 0, @CodigoRastreio = NULL;
EXEC sp_InsertAgendamento @Data = '2026-05-23', @Total = 20.00, @Cliente_id = 14, @Status = 0, @CodigoRastreio = NULL;
EXEC sp_InsertAgendamento @Data = '2026-05-24', @Total = 10.00, @Cliente_id = 15, @Status = 2, @CodigoRastreio = NULL;
EXEC sp_InsertAgendamento @Data = '2026-05-25', @Total = 90.00, @Cliente_id = 16, @Status = 1, @CodigoRastreio = NULL;
EXEC sp_InsertAgendamento @Data = '2026-05-26', @Total = 45.00, @Cliente_id = 17, @Status = 1, @CodigoRastreio = NULL;
EXEC sp_InsertAgendamento @Data = '2026-05-27', @Total = 50.00, @Cliente_id = 18, @Status = 1, @CodigoRastreio = NULL;
EXEC sp_InsertAgendamento @Data = '2026-05-28', @Total = 30.00, @Cliente_id = 19, @Status = 0, @CodigoRastreio = NULL;
EXEC sp_InsertAgendamento @Data = '2026-05-29', @Total = 35.00, @Cliente_id = 20, @Status = 1, @CodigoRastreio = NULL;
EXEC sp_InsertAgendamento @Data = '2026-05-30', @Total = 99.00, @Cliente_id = 20, @Status = 1, @CodigoRastreio = NULL;
GO

-- ==========================================
-- 8. SERVIÇOS AGENDADOS
-- ==========================================
EXEC sp_InsertServicoAgendado @Agendamento_nr = 1,  @Servico_id = 1,  @Obs = 'Cliente pediu esmalte vermelho', @Horario = '09:00:00', @Funcionario_id = 21, @Valor = 35.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 2,  @Servico_id = 2,  @Obs = 'Cuidado extra com calo',         @Horario = '10:00:00', @Funcionario_id = 22, @Valor = 40.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 3,  @Servico_id = 3,  @Obs = 'Sem cutícula funda',             @Horario = '11:00:00', @Funcionario_id = 21, @Valor = 70.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 4,  @Servico_id = 4,  @Obs = 'Formato Almond',                 @Horario = '14:00:00', @Funcionario_id = 23, @Valor = 150.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 5,  @Servico_id = 5,  @Obs = 'Formato Quadrado',               @Horario = '15:00:00', @Funcionario_id = 24, @Valor = 130.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 6,  @Servico_id = 6,  @Obs = 'Repor 1 unha quebrada',          @Horario = '16:00:00', @Funcionario_id = 23, @Valor = 80.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 7,  @Servico_id = 7,  @Obs = 'Nenhuma',                        @Horario = '09:30:00', @Funcionario_id = 25, @Valor = 60.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 8,  @Servico_id = 8,  @Obs = 'Dói um pouco a unha do dedinho', @Horario = '10:30:00', @Funcionario_id = 26, @Valor = 40.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 9,  @Servico_id = 9,  @Obs = 'Relaxamento máximo',             @Horario = '13:00:00', @Funcionario_id = 27, @Valor = 55.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 10, @Servico_id = 10, @Obs = 'Fissuras no calcanhar',          @Horario = '14:30:00', @Funcionario_id = 28, @Valor = 85.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 11, @Servico_id = 11, @Obs = 'Trazer próprio esmalte',         @Horario = '16:00:00', @Funcionario_id = 29, @Valor = 25.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 12, @Servico_id = 12, @Obs = 'Cor Nude',                       @Horario = '08:00:00', @Funcionario_id = 30, @Valor = 45.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 13, @Servico_id = 13, @Obs = 'Traço fino',                     @Horario = '09:00:00', @Funcionario_id = 31, @Valor = 15.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 14, @Servico_id = 14, @Obs = 'Flor no dedo anelar',            @Horario = '10:00:00', @Funcionario_id = 32, @Valor = 20.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 15, @Servico_id = 15, @Obs = 'Urgência',                       @Horario = '11:00:00', @Funcionario_id = 33, @Valor = 10.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 16, @Servico_id = 16, @Obs = 'Unha encravada',                 @Horario = '13:30:00', @Funcionario_id = 34, @Valor = 90.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 17, @Servico_id = 17, @Obs = 'Nenhuma',                        @Horario = '14:30:00', @Funcionario_id = 35, @Valor = 45.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 18, @Servico_id = 18, @Obs = 'Cliente com pressa',             @Horario = '15:30:00', @Funcionario_id = 36, @Valor = 50.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 19, @Servico_id = 19, @Obs = 'Óleo essencial de lavanda',      @Horario = '17:00:00', @Funcionario_id = 37, @Valor = 30.00;
EXEC sp_InsertServicoAgendado @Agendamento_nr = 20, @Servico_id = 20, @Obs = 'Criança agitada',                @Horario = '18:00:00', @Funcionario_id = 40, @Valor = 35.00;

-- Segundo serviço do agendamento 20
EXEC sp_InsertServicoAgendado @Agendamento_nr = 20, @Servico_id = 1, @Obs = 'Cliente pediu esmalte vermelho', @Horario = '12:00:00', @Funcionario_id = 21, @Valor = 35.00;
GO

-- ==========================================
-- 9. PRODUTOS AGENDADOS
-- ==========================================
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 1,  @Servico_id = 1,  @Produto_codigo = 1,  @Preco = 8.50;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 2,  @Servico_id = 2,  @Produto_codigo = 8,  @Preco = 35.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 3,  @Servico_id = 3,  @Produto_codigo = 2,  @Preco = 9.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 4,  @Servico_id = 4,  @Produto_codigo = 10, @Preco = 45.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 5,  @Servico_id = 5,  @Produto_codigo = 9,  @Preco = 85.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 6,  @Servico_id = 6,  @Produto_codigo = 9,  @Preco = 85.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 7,  @Servico_id = 7,  @Produto_codigo = 3,  @Preco = 12.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 8,  @Servico_id = 8,  @Produto_codigo = 6,  @Preco = 15.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 9,  @Servico_id = 9,  @Produto_codigo = 8,  @Preco = 35.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 10, @Servico_id = 10, @Produto_codigo = 13, @Preco = 30.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 11, @Servico_id = 11, @Produto_codigo = 5,  @Preco = 8.90;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 12, @Servico_id = 12, @Produto_codigo = 16, @Preco = 22.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 13, @Servico_id = 13, @Produto_codigo = 5,  @Preco = 8.90;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 14, @Servico_id = 14, @Produto_codigo = 18, @Preco = 11.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 15, @Servico_id = 15, @Produto_codigo = 11, @Preco = 25.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 16, @Servico_id = 16, @Produto_codigo = 14, @Preco = 18.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 17, @Servico_id = 17, @Produto_codigo = 15, @Preco = 40.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 18, @Servico_id = 18, @Produto_codigo = 19, @Preco = 14.00;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 19, @Servico_id = 19, @Produto_codigo = 12, @Preco = 7.50;
EXEC sp_InsertProdutoAgendado @Agendamento_nr = 20, @Servico_id = 20, @Produto_codigo = 17, @Preco = 9.50;
GO

-- ==========================================
-- 10. TESTE DE REALIDADE (CADASTRO SEM AGENDAMENTO)
-- ==========================================
-- Aqui usamos a exata mesma procedure que você já construiu:
EXEC sp_InsertCliente @Nome = 'Cliente Apenas Cadastro', @Telefone = '11900000000', @Senha = 'testeCadastro';
GO

