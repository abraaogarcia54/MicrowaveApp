# Níveis Atendidos

## Status atual

Nível 1 concluído.
Nível 2 concluído.
Nível 3 concluído.
Nível 4 implementado e pendente de validação manual completa em Docker.

## Foco atual

Validar manualmente o Nível 4 com API autenticada, Blazor e SQL Server via Docker Compose.

## Critério de conclusão do Nível 1

- Regras de aquecimento manual implementadas no domínio/Application.
- Início rápido implementado.
- Acréscimo de 30 segundos durante aquecimento implementado.
- Pausa, retomada e cancelamento implementados.
- String informativa com mensagem final implementada.
- Testes unitários cobrindo os fluxos principais.

## Evidências atuais

- `dotnet build MicrowaveApp.sln` executado com sucesso.
- `MicrowaveApp.Domain.Tests` executado com sucesso.
- `MicrowaveApp.Application.Tests` executado com sucesso.
- Interface Blazor inicial criada para entrada de tempo, potência, teclado numérico, iniciar e pausar/cancelar.
- Nível 1 testado manualmente e aprovado em 2026-06-11.
- Nível 2 implementado com 5 programas pré-definidos, caracteres exclusivos, bloqueio de edição de dados pré-definidos e bloqueio de acréscimo de tempo em programa pré-definido.
- `MicrowaveApp.Domain.Tests` passou com 9 testes.
- `MicrowaveApp.Application.Tests` passou com 15 testes.
- Nível 3 implementado com cadastro de programas customizados via API e persistência SQL Server.
- Programas customizados são exibidos junto aos pré-definidos e diferenciados em itálico na UI.
- Migration inicial `InitialCreate` criada no projeto `MicrowaveApp.Infrastructure`.
- Nível 4 implementado com autenticação Bearer Token, endpoints protegidos para aquecimento e programas, status de autenticação no Blazor e bloqueio de funções sem token.
- Senhas persistidas com hash SHA-256 por meio de `PasswordHasher`.
- Connection string de runtime configurada de forma criptografada e descriptografada na inicialização da API.
- Tratamento global de exceptions criado com resposta JSON padronizada.
- `BusinessException` usada para regras de negócio e exceptions inesperadas logadas em arquivo.
- `dotnet build MicrowaveApp.sln` executado com sucesso após o Nível 4.
- `dotnet test MicrowaveApp.sln` executado com sucesso após o Nível 4: 24 testes aprovados.
