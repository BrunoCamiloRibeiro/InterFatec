<div align="center">

# SQL
#### Deve conter apenas `scripts SQL`, imagens e `diagramas` relacionados.

</div>

## Commits rápidos: SQL

```
"[SQL] feat: 
```
```
"[SQL] fix: 
```
```
"[SQL] refactor:
```
```
"[SQL] docs:
```
```
"[SQL] chore:
```

## O que deve conter nessa pasta:

* Scripts SQL para criação de tabelas, inserção de dados, etc.
* Imagens ou diagramas relacionados ao banco de dados, se necessário.


## Templates de Prompts para Inserts

### Prompt 1:

```
Crie um único código sql que faça ao menos 20 inserções em cada tabela do banco de dados que lhe fornecerei abaixo, abrangendo o máximo de situações que você conseguir prever e separando (com 3 quebras de linhas) em grupos de inserts de acordo com a organização dos selects abaixo:

------------SELECTS---------------

select * from Pessoas
select * from Clientes
select * from Especialidades
select * from Funcionarios

select * from Marcas
select * from Produtos

select * from Servicos

select * from Status
select * from Agendamentos
select * from Servicos_Agendados
select * from Produtos_Agendados

----------------------------------

Agora irei colar o sql do banco criado:
```
