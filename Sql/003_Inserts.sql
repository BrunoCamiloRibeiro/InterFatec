use Fabys_Unha;

-- ==========================================
-- GRUPO 1: Pessoas, Clientes, Especialidades, Funcionarios
-- ==========================================
INSERT INTO Pessoas (Nome, Telefone) VALUES
('Ana Clara', '11988880001'), ('Bruno Costa', '11988880002'), ('Carla Dias', '11988880003'),
('Daniela Silva', '11988880004'), ('Eduardo Lima', '11988880005'), ('Fernanda Alves', '11988880006'),
('Gabriela Santos', '11988880007'), ('Helena Souza', '11988880008'), ('Igor Martins', '11988880009'),
('Juliana Rocha', '11988880010'), ('Karla Mendes', '11988880011'), ('Lucas Ferreira', '11988880012'),
('Mariana Gomes', '11988880013'), ('Nathalia Ribeiro', '11988880014'), ('Otavio Pires', '11988880015'),
('Patricia Castro', '11988880016'), ('Quintino Araujo', '11988880017'), ('Rafaela Moraes', '11988880018'),
('Sabrina Nunes', '11988880019'), ('Tatiana Barros', '11988880020'),
('Ursula Farias', '11988880021'), ('Valeria Pinto', '11988880022'), ('Wagner Melo', '11988880023'),
('Xenia Viana', '11988880024'), ('Yara Teixeira', '11988880025'), ('Zelia Dantas', '11988880026'),
('Alice Carvalho', '11988880027'), ('Breno Nogueira', '11988880028'), ('Camila Borges', '11988880029'),
('Diego Leite', '11988880030'), ('Elisa Monteiro', '11988880031'), ('Fabio Tavares', '11988880032'),
('Giovana Lemos', '11988880033'), ('Hugo Resende', '11988880034'), ('Isabela Macedo', '11988880035'),
('Joao Batista', '11988880036'), ('Karen Peixoto', '11988880037'), ('Leonardo Guedes', '11988880038'),
('Marcos Vianna', '11988880039'), ('Neide Camargo', '11988880040');

INSERT INTO Clientes (pessoa_id) VALUES
(1), (2), (3), (4), (5), (6), (7), (8), (9), (10),
(11), (12), (13), (14), (15), (16), (17), (18), (19), (20);

INSERT INTO Especialidades (descricao) VALUES
('Manicure Clássica'), ('Pedicure Clássica'), ('Alongamento em Gel'),
('Alongamento em Fibra'), ('Spa dos Pés'), ('Plástica dos Pés'),
('Francesinha Definitiva'), ('Esmaltação em Gel'), ('Nail Art 3D'),
('Podologia Básica'), ('Cutilagem Russa'), ('Banho de Gel'),
('Manutenção de Gel'), ('Remoção de Alongamento'), ('Massagem Relaxante Pés'),
('Reflexologia Podal'), ('Blindagem de Unhas'), ('Reconstrução de Unha'),
('Decoração com Pedrarias'), ('Esmaltação Infantil');

INSERT INTO Funcionarios (pessoa_id, salario, especialidade_id) VALUES
(21, 2500.00, 1), (22, 2600.00, 2), (23, 3500.00, 3), (24, 3800.00, 4), (25, 2200.00, 5),
(26, 3000.00, 6), (27, 2800.00, 7), (28, 3100.00, 8), (29, 3600.00, 9), (30, 4000.00, 10),
(31, 3200.00, 11), (32, 2900.00, 12), (33, NULL, 13), 
(34, 1800.00, 14), (35, 2100.00, 15), (36, 2500.00, 16), (37, 2700.00, 17),
(38, 2000.00, NULL), 
(39, NULL, NULL),    
(40, 1500.00, 20);




-- ==========================================
-- GRUPO 2: Marcas, Produtos
-- ==========================================
INSERT INTO Marcas (nome) VALUES
('Risqué'), ('Colorama'), ('Impala'), ('Dailus'), ('Vult'),
('Anita'), ('Avon'), ('Natura'), ('OPI'), ('Essie'),
('Hits'), ('Blant'), ('Studio 35'), ('Haskell'), ('Latika'),
('Bella Brazil'), ('Top Beauty'), ('Novo Toque'), ('Nati'), ('Cora');

INSERT INTO Produtos (nome, marca_id, preco, PathImg) VALUES
('Esmalte Vermelho Carmim', 1, 8.50, '/img/prod01.jpg'),
('Base Fortalecedora', 2, 9.00, '/img/prod02.jpg'),
('Top Coat Brilho', 3, 12.00, '/img/prod03.jpg'),
('Esmalte Preto Fosco', 4, 10.50, '/img/prod04.jpg'),
('Esmalte Branco Puríssimo', 5, 8.90, '/img/prod05.jpg'),
('Removedor de Esmalte', 6, 15.00, '/img/prod06.jpg'),
('Algodão 500g', 7, 20.00, '/img/prod07.jpg'),
('Creme Esfoliante Pés', 8, 35.00, '/img/prod08.jpg'),
('Gel Construtor Clear', 9, 85.00, '/img/prod09.jpg'),
('Fibra de Vidro Rolo', 10, 45.00, '/img/prod10.jpg'),
('Primer Ácido', 11, 25.00, '/img/prod11.jpg'),
('Óleo Secante', 12, 7.50, '/img/prod12.jpg'),
('Lixa Banana (Pct 100)', 13, 30.00, '/img/prod13.jpg'),
('Espátula Inox', 14, 18.00, '/img/prod14.jpg'),
('Alicate de Cutícula', 15, 40.00, '/img/prod15.jpg'),
('Prep Higienizador', 16, 22.00, '/img/prod16.jpg'),
('Esmalte Nude Clássico', 17, 9.50, '/img/prod17.jpg'),
('Esmalte Neon Rosa', 18, 11.00, '/img/prod18.jpg'),
('Base Bomba Aceleradora', 19, 14.00, '/img/prod19.jpg'),
('Amolecedor de Cutículas', 20, 13.50, '/img/prod20.jpg');




-- ==========================================
-- GRUPO 3: Servicos
-- ==========================================
INSERT INTO Servicos (preco, descricao, tempo) VALUES
(35.00, 'Manicure Simples', '00:45:00'),
(40.00, 'Pedicure Simples', '00:50:00'),
(70.00, 'Mão e Pé Casadinho', '01:30:00'),
(150.00, 'Alongamento Fibra de Vidro', '02:30:00'),
(130.00, 'Alongamento em Gel', '02:00:00'),
(80.00, 'Manutenção de Alongamento', '01:30:00'),
(60.00, 'Banho de Gel', '01:00:00'),
(40.00, 'Remoção de Alongamento', '00:40:00'),
(55.00, 'Spa dos Pés Completo', '01:00:00'),
(85.00, 'Plástica dos Pés', '01:15:00'),
(25.00, 'Apenas Esmaltação', '00:20:00'),
(45.00, 'Esmaltação em Gel', '00:45:00'),
(15.00, 'Francesinha/Inglesinha', '00:15:00'),
(20.00, 'Nail Art (por unha)', '00:15:00'),
(10.00, 'Conserto de Unha (unidade)', '00:10:00'),
(90.00, 'Podologia Preventiva', '01:00:00'),
(45.00, 'Cutilagem Russa Mão', '00:45:00'),
(50.00, 'Blindagem Diamante', '01:00:00'),
(30.00, 'Massagem Relaxante Pés', '00:30:00'),
(35.00, 'Manicure Infantil', '00:30:00');




-- ==========================================
-- GRUPO 4: Status, Agendamentos, Servicos_Agendados, Produtos_Agendados
-- ==========================================
INSERT INTO Status (descricao) VALUES
('Agendado'), ('Confirmado'), ('Em Atendimento'), ('Concluído'), ('Aguardando Pagamento'),
('Pago'), ('Cancelado Cliente'), ('Cancelado Salão'), ('Faltou'), ('Remarcado'),
('Atrasado'), ('Em Espera'), ('Triagem'), ('Pausado'), ('Finalizado com Nota'),
('Estornado'), ('Bloqueado'), ('Reagendamento Req.'), ('Aguardando Material'), ('Arquivado');

INSERT INTO Agendamentos (data, total, cliente_id, status_id) VALUES
('2026-05-10', 35.00, 1, 4), ('2026-05-11', 40.00, 2, 6), ('2026-05-12', 70.00, 3, 1),
('2026-05-13', 150.00, 4, 2), ('2026-05-14', 130.00, 5, 3), ('2026-05-15', 80.00, 6, 5),
('2026-05-16', 60.00, 7, 7), ('2026-05-17', 40.00, 8, 8), ('2026-05-18', 55.00, 9, 9),
('2026-05-19', 85.00, 10, 10), ('2026-05-20', 25.00, 11, 11), ('2026-05-21', 45.00, 12, 12),
('2026-05-22', 15.00, 13, 13), ('2026-05-23', 20.00, 14, 14), ('2026-05-24', 10.00, 15, 15),
('2026-05-25', 90.00, 16, 16), ('2026-05-26', 45.00, 17, 17), ('2026-05-27', 50.00, 18, 18),
('2026-05-28', 30.00, 19, 19), ('2026-05-29', 35.00, 20, 20);

INSERT INTO Servicos_Agendados (agendamento_nr, servico_id, obs, horario, funcionario_id, valor) VALUES
(1, 1, 'Cliente pediu esmalte vermelho', '09:00:00', 21, 35.00),
(2, 2, 'Cuidado extra com calo', '10:00:00', 22, 40.00),
(3, 3, 'Sem cutícula funda', '11:00:00', 21, 70.00),
(4, 4, 'Formato Almond', '14:00:00', 23, 150.00),
(5, 5, 'Formato Quadrado', '15:00:00', 24, 130.00),
(6, 6, 'Repor 1 unha quebrada', '16:00:00', 23, 80.00),
(7, 7, 'Nenhuma', '09:30:00', 25, 60.00),
(8, 8, 'Dói um pouco a unha do dedão', '10:30:00', 26, 40.00),
(9, 9, 'Relaxamento máximo', '13:00:00', 27, 55.00),
(10, 10, 'Fissuras no calcanhar', '14:30:00', 28, 85.00),
(11, 11, 'Trazer próprio esmalte', '16:00:00', 29, 25.00),
(12, 12, 'Cor Nude', '08:00:00', 30, 45.00),
(13, 13, 'Traço fino', '09:00:00', 31, 15.00),
(14, 14, 'Flor no dedo anelar', '10:00:00', 32, 20.00),
(15, 15, 'Urgência', '11:00:00', 33, 10.00),
(16, 16, 'Unha encravada', '13:30:00', 34, 90.00),
(17, 17, 'Nenhuma', '14:30:00', 35, 45.00),
(18, 18, 'Cliente com pressa', '15:30:00', 36, 50.00),
(19, 19, 'Óleo essencial de lavanda', '17:00:00', 37, 30.00),
(20, 20, 'Criança agitada', '18:00:00', 40, 35.00);

INSERT INTO Produtos_Agendados (agendamento_nr, servico_id, produto_codigo, preco) VALUES
(1, 1, 1, 8.50),
(2, 2, 8, 35.00),
(3, 3, 2, 9.00),
(4, 4, 10, 45.00),
(5, 5, 9, 85.00),
(6, 6, 9, 85.00),
(7, 7, 3, 12.00),
(8, 8, 6, 15.00),
(9, 9, 8, 35.00),
(10, 10, 13, 30.00),
(11, 11, 5, 8.90),
(12, 12, 16, 22.00),
(13, 13, 5, 8.90),
(14, 14, 18, 11.00),
(15, 15, 11, 25.00),
(16, 16, 14, 18.00),
(17, 17, 15, 40.00),
(18, 18, 19, 14.00),
(19, 19, 12, 7.50),
(20, 20, 17, 9.50);