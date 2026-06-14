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
