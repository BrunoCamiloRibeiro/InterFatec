insert into Pessoas values('Fabiana', '1799999999')
insert into Pessoas values('Bruno', '1799888888')
insert into Pessoas values('Matheus', '1799777777')
insert into Pessoas values('Jo�o', '1799666666')
insert into Pessoas values('Guilherme', '1799555555')
insert into Pessoas values('Sergio', '1799444444')

insert into Clientes values(4)
insert into Clientes values(5)
insert into Clientes values(6)

insert into Tipos values('Manicure')
insert into Tipos values('Pedicure')
insert into Tipos values('Manicure especializada em unhas art�sticas')

insert into Profissionais_Beleza values(1, 1200.00, 1)
insert into Profissionais_Beleza values(2, 1100.00, 2)
insert into Profissionais_Beleza values(3, 1000.00, 3)

insert into Status values('A FAZER')
insert into Status values('FEITO')
insert into Status values('CANCELADO')
insert into Status values('PAGO')


insert into Marcas values('Risqu�')
insert into Marcas values('Colorama')
insert into Marcas values('Dailus')

insert into Produtos values('Esmalte Preto', 1, 10.20, 'caminhoImg')
insert into Produtos values('Esmalte Azul', 2, 10.20, 'caminhoImg')
insert into Produtos values('Esmalte Verde', 3, 10.20, 'caminhoImg')

insert into Servicos values(15.00, 'Fazer as Unhas', '00:30:00')
insert into Servicos values(30.00, 'Unhas em gel', '00:50:00')
insert into Servicos values(15.00, 'Limpeza', '00:30:00')

insert into Agendamentos values('2025-12-21', 45.00, 4, 1)
insert into Agendamentos values('2025-12-22', 30.00, 5, 2)
insert into Agendamentos values('2025-12-23', 60.00, 6, 3)

insert into Servicos_Agendados values(1, 1, 'obisserva', '13:00:00', 1, 10.20)
insert into Servicos_Agendados values(2, 2, 'Poss�vel atraso', '13:00:00', 1, 10.20)
insert into Servicos_Agendados values(3, 3, 'Idosa', '13:00:00', 2, 10.20)




insert into Produtos_Agendados values(1, 1, 4, 10.50)


select * from agendamentos
select * from servicos_agendados
select * from produtos_agendados