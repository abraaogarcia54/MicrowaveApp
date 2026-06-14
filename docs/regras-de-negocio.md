# Regras de Negócio

## Nível 1

- Aquecimento manual aceita tempo de 1 a 120 segundos.
- Potência aceita valores de 1 a 10.
- Potência padrão é 10.
- Início rápido usa 30 segundos e potência 10.
- Iniciar durante aquecimento manual adiciona 30 segundos.
- Iniciar durante pausa retoma a sessão.
- Pausar/cancelar pausa quando aquece e cancela quando está pausado.

## Nível 2

- Programas pré-definidos podem ter tempo maior que 120 segundos.
- Programas pré-definidos não podem usar o caractere padrão `.`.
- Programas pré-definidos não podem ser alterados.
- Sessões iniciadas por programa pré-definido não permitem acréscimo de tempo.

## Nível 3

- Programas customizados exigem nome, alimento, tempo, potência e caractere.
- Instruções são opcionais.
- O caractere de aquecimento não pode ser `.`.
- O caractere de aquecimento não pode repetir com nenhum programa existente.
- Programas customizados são persistidos em SQL Server.
- Programas customizados são exibidos junto aos pré-definidos e aparecem em itálico na interface.

## Nível 4

- As operações de aquecimento e cadastro/listagem de programas são expostas pela Web API.
- Endpoints de negócio exigem Bearer Token.
- O Blazor exibe o status de autenticação e bloqueia os comandos quando não há token.
- Usuários são autenticados por usuário e senha; a senha é persistida com hash SHA-256.
- A connection string usada pela API fica criptografada na configuração e é descriptografada em runtime.
- Erros de regra de negócio retornam resposta JSON padronizada usando `BusinessException`.
- Exceptions inesperadas retornam resposta padronizada e são registradas em arquivo de log com trace id, rota, método, exception, inner exception e stacktrace.
