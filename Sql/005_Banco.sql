create database Fabys_Unha
go

use Fabys_Unha
go



create table Pessoas
	(
--		Nome		Tipo			Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		id			int				not null		primary key		identity					,
		Nome		varchar(100)	not null													,
		Telefone	varchar(11)		not null											unique	,
		status		int				not null		default 0 --status virou atributo
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
		descricao	varchar(80)	not null													,
		status		int			not null		default 0 --ativo
	)
go

create table Funcionarios
	(
--		Nome				Tipo			Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		pessoa_id			int				not null		primary key									,
		salario				decimal(10,2)	not null	    											,
		especialidade_id	int				not null													,
--Salario ajustado 
--Especialidade não pode ser null			
		constraint CK_Funcionarios_SalarioMin check (salario >= 1412.00),
		foreign key(pessoa_id) references Pessoas(id),
		foreign key(especialidade_id) references Especialidades(id)
	)
go



create table Marcas
	(
--		Nome		Tipo		Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		id			int			not null		primary key		identity					,
		nome		varchar(50)	not null													,
		status		int			not null		default 0
	)
go

create table Produtos
	(
--		Nome		Tipo			Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		codigo		int				not null		primary key		identity					,	
		nome		varchar(50)		not null													,
		marca_id	int				not null													,
		preco		decimal(10,2)	not null													,
		PathImagem	varchar(100)	not null													,
		status		int				not null		default 0

		foreign key(marca_id) references Marcas(id)
	)
go



create table Servicos
	(
--		Nome		Tipo			Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		id			int				not null		primary key		identity					,
		preco		decimal(10,2)	not null													,
		descricao	varchar(80)		not null													,
		tempo		time(0)			not null													,
		status		int				not null		default 0
	)
go

create table Agendamentos
	(
--		Nome			Tipo			Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		nr				int				not null		primary key		identity					,
		data			datetime2(0)	not null													,
		total			decimal(10,2)	not null													,
		cliente_id		int				not null													,
		status			int				not null		default 0									,

		foreign key(cliente_id) references Clientes(pessoa_id)
	)
go

create table Servicos_Agendados
	(
--		Nome			Tipo			Nulo/Não Nulo	Chave-Primaria	Auto-Incremento		Unico	,
		agendamento_nr	int				not null													,
		servico_id		int				not null													,
		obs				varchar(200)	not null													,
		horario			time			not null													,
		funcionario_id	int				not null													,
		valor			decimal(10,2)	not null													,

		primary key(agendamento_nr, servico_id),
		foreign key(agendamento_nr) references Agendamentos(nr),
		foreign key(servico_id) references Servicos(id),
		foreign key(funcionario_id) references Funcionarios(pessoa_id)
	)
go

create table Produtos_Agendados
(
    agendamento_nr      int             not null,
    servico_id          int             not null,
    produto_codigo      int             not null,
    preco               decimal(10,2)   not null

    primary key(agendamento_nr, servico_id, produto_codigo),
    foreign key(agendamento_nr, servico_id) references Servicos_Agendados(agendamento_nr, servico_id),
    foreign key(produto_codigo) references Produtos(codigo)
)
go




