create database Fabys_Unhas
go

use Fabys_Unhas
go

create table Pessoas
	(
--		Nome		Tipo		Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		id			int			not null		primary key		identity					,
		Nome		varchar(50)	not null													,
		Telefone	varchar(11)	not null											unique
	)
go

create table Clientes
	(
--		Nome		Tipo		Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		pessoa_id	int			not null		primary key									,

		foreign key(pessoa_id) references Pessoas(id)
	)
go

create table Especialidades
	(
--		Nome		Tipo		Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		id			int			not null		primary key		identity					,
		descricao	varchar(50)	not null													
	)
go

create table Funcionarios
	(
--		Nome		Tipo		Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		pessoa_id	int			not null		primary key									,
		salario		decimal(7,2)	null													,
		especialidade_id	int				null													

		foreign key(pessoa_id) references Pessoas(id),
		foreign key(especialidade_id) references Especialidades(id)
	)
go

create table Status
	(
--		Nome		Tipo		Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		id			int			not null		primary key		identity					,		
		descricao	varchar(20)	not null
	)
go

create table Marcas
	(
--		Nome		Tipo		Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		id			int			not null		primary key		identity					,		
		nome		varchar(50)	not null		
	)
go

create table Produtos
	(
--		Nome		Tipo			Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		codigo		int				not null		primary key		identity					,	
		nome		varchar(50)		not null													,
		marca_id	int				not null													,
		preco		decimal(30,2)	not null													,
		PathImg		varchar(100)	not null

		foreign key(marca_id) references Marcas(id)
	)
go

create table Servicos
	(
--		Nome		Tipo		Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		id			int			not null		primary key		identity					,		
		preco		decimal(7,2)not null													,
		descricao	varchar(50)	not null													,
		tempo		time(0)		not null
	)
go

create table Agendamentos
	(
--		Nome			Tipo		Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		nr				int			not null		primary key		identity					,
		data			date		not null													,
		total			decimal(7,2)not null													,
		cliente_id		int			not null													,		
		status_id		int			not null

		foreign key(cliente_id     ) references Clientes(pessoa_id),		
		foreign key(status_id      ) references Status(id)
	)
go

create table Servicos_Agendados
	(
--		Nome			Tipo			Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		agendamento_nr	int				not null													,
		servico_id		int				not null													,
		obs				varchar(50)		not null													,
		horario			time			not null													,
		funcionario_id	int				not null													,
		valor			decimal(30,2)

		primary key(agendamento_nr, servico_id),
		foreign key(agendamento_nr) references Agendamentos(nr),
		foreign key(servico_id) references Servicos(id),		
		foreign key(funcionario_id) references Funcionarios(pessoa_id)
	)
go

create table Produtos_Agendados
	(
--		Nome			Tipo				Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		agendamento_nr		int				not null												,
		servico_id			int				not null												,
		produto_codigo		int				not null												,
		preco				decimal(10,2)	not null

		primary key(agendamento_nr, servico_id, produto_codigo),
		foreign key(agendamento_nr, servico_id) references Servicos_Agendados(agendamento_nr, servico_id),
		foreign key(produto_codigo) references Produtos(codigo)
	)
go