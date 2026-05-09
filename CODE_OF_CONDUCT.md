# Manual de Versionamento - Projeto Integrador

Este documento define as regras básicas de organização para o nosso repositório. O foco é praticidade e evitar conflitos de código.

---

## 📂 1. Estrutura de Pastas

* **`/Documents`**: Arquivos de texto, PDFs e roteiros.
* **`/SQL`**: Scripts `.sql` (criação de tabelas, inserção de dados).
* **`/Project`**: O código-fonte da aplicação ASP.NET.

---

## 💬 2. Padrão de Mensagens de Commit

### *Onde foi alterado:*

* `[DOCS]` - Para alterações em documentos.
* `[SQL]` - Para novos scripts de banco de dados.
* `[PROJ]` - Para código no ASP.NET.
* `[GERAL]` - README ou configurações gerais.

### *Tipo de alteração:*
* `feat:` (Funcionalidade) -> Quando você implementa um **recurso** que novo.
* `fix:` (Correção) -> Quando você resolve um bug em código.
* `refactor:` (Refatoração) -> Quando você reestrutura um código sem alterar seu comportamento.
* `docs:` (Documentação) -> Alterações em arquivos de documentação. (Ignorar em caso de documentação em `[DOCS]`)
* `chore:` (Tarefas de manutenção) -> Alterações em arquivos que não alteram código funcional (Configurações, Ambiente, Atualizações de dependencias etc)

### *Exemplos:*
- `[PROJ] feat: implementando o Controller de Agendamentos`
- `[PROJ] fix: corrigindo erro de validação no modelo de Professores`
- `[PROJ] refactor: movendo lógica de acesso a dados para a camada de Service`
- `[PROJ] chore: instalando pacotes do Entity Framework Core Design e Tools`
  
<br>
  
- `[SQL] feat: script para criação das tabelas de Mensagens e Anexos`
- `[SQL] feat: adição de Procedure para relatório de estoque`
- `[SQL] fix: alteração do tamanho do campo de senha para suportar Hash`
- `[SQL] chore: inclusão de dados iniciais (Seed) para categorias de produtos`
- `[SQL] docs: atualização do README com prompt de insert`
  
<br>
  
- `[DOCS] Adição do Diagrama de Classes`
- `[DOCS] Correção da bibliografia`
  
<br>
  
- `[GERAL] chore: atualização da ConnectionString no appsettings.json para ambiente de produção`
- `[GERAL] docs: atualização do README com instruções de como rodar as Migrations`
- `[GERAL] chore: configuração de regras de CORS no Startup/Program.cs`
- `[GERAL] fix: correção de links quebrados no arquivo de licença`

---

## ⚡ 3. Fluxo de Trabalho (Direto na Main)

Nós vamos trabalhar **diretamente na branch principal (`main` ou `master`)**. Para isso dar certo sem um apagar o código do outro, siga os 3 passos sagrados:

1. **A Regra do `Pull`:** ANTES de começar a programar ou abrir o Visual Studio, dê um `git pull`. Sempre comece o dia com a versão mais atualizada.
2. **A Regra do Zap:** Se você for mexer em um arquivo muito importante ou central (ex: configurações gerais do ASP.NET ou a master page), avise no grupo do WhatsApp: *"Tô mexendo no arquivo X, não mexam lá agora"*.
3. **Commits Rápidos:** Fez uma alteração que está funcionando? Já dê o `git add`, `git commit` e `git push`. Quanto mais rápido seu código for pro GitHub, menor a chance de dar conflito com o código de outro colega.

*(Obs: Só crie uma branch se você for fazer uma mudança gigante que vai quebrar o projeto por vários dias. Fora isso, é tudo na main).*

---

## 🗄️ 4. Regras para o Banco de Dados (SQL)

Para ninguém ficar com o banco dessincronizado:
* **Nunca altere um script antigo.** * Se precisar mexer no banco de novo, crie um script novo e numerado:
  * `01_Tabelas_Iniciais.sql`
  * `02_Adiciona_Coluna_Idade.sql`
* Assim, é só rodar o script mais recente no SQL Server para atualizar o banco local.
