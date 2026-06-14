# Níveis Atendidos

## Status atual

Nível 1 concluído.
Nível 2 implementado e pendente de validação manual.
Nível 3 implementado e pendente de validação manual.

## Foco atual

Validar manualmente o Nível 3 com SQL Server.

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
