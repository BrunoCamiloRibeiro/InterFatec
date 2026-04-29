# Manual de Versionamento - Projeto Integrador

Este documento define as regras básicas de organização para o nosso repositório. O foco é praticidade e evitar conflitos de código.

---

## 📂 1. Estrutura de Pastas

* **`/Documents`**: Arquivos de texto, PDFs e roteiros.
* **`/SQL`**: Scripts `.sql` (criação de tabelas, inserção de dados).
* **`/Project`**: O código-fonte da aplicação ASP.NET.

---

## 💬 2. Padrão de Mensagens de Commit

Use um prefixo para sabermos de bater o olho o que foi alterado:

* **[DOCS]** - Para alterações em documentos.
* **[SQL]** - Para novos scripts de banco de dados.
* **[PROJ]** - Para código no ASP.NET.
* **[GERAL]** - README ou configurações gerais.

**Exemplos:**
- `[PROJ] Adiciona botão de voltar na tela de login`
- `[SQL] Cria tabela de clientes`

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