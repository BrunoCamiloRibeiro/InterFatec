$body = "Nome=Teste&Telefone=11999999999&Data=2026-10-10&Servicos[0].ServicoId=1&Servicos[0].FuncionarioId=1&Servicos[0].Horario=14:00:00"

Invoke-RestMethod -Uri "http://localhost:5188/Agendamentos/Agendar" -Method Post -ContentType "application/x-www-form-urlencoded" -Body $body -SessionVariable session

# Login creates session
Invoke-RestMethod -Uri "http://localhost:5188/Login/Login" -Method Post -ContentType "application/x-www-form-urlencoded" -Body "Telefone=11999999999&Senha=123" -WebSession $session
# Now post again
Invoke-RestMethod -Uri "http://localhost:5188/Agendamentos/Agendar" -Method Post -ContentType "application/x-www-form-urlencoded" -Body $body -WebSession $session
