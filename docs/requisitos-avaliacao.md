# Requisitos da Avaliação

## Obrigatórios em todos os níveis

- Utilizar conceitos de orientação a objetos.
- Utilizar .NET Framework 4.0 ou superior.
- Priorizar a implementação do micro-ondas, sem foco no visual do formulário.
- Separar camadas de interface de usuário e negócio.
- O programa deve funcionar conforme os requisitos de cada nível.

## Desejáveis e diferenciais

- Observar princípios SOLID.
- Utilizar design patterns quando fizer sentido.
- Manter boas práticas e qualidade de código.
- Prevenir uso incorreto das classes, protegendo acesso a dados e métodos.
- Documentar o código quando necessário.
- Implementar testes unitários para a camada de negócio.

## Nível 1

- Interface deve permitir informar tempo e potência.
- Deve permitir entrada por teclado digital e/ou teclado físico.
- Deve haver método para iniciar aquecimento com tempo e potência.
- Tempo manual mínimo: 1 segundo.
- Tempo manual máximo: 2 minutos.
- Potência varia de 1 a 10.
- Potência padrão: 10 quando não informada.
- Tempo maior que 60 e menor que 100 segundos deve ser exibido em formato de minutos, por exemplo `90` vira `1:30`.
- Tempo fora dos limites deve exibir mensagem solicitando tempo válido.
- Potência fora dos limites deve exibir mensagem.
- Início rápido: iniciar sem tempo e potência usa 30 segundos e potência 10.
- Acionar início durante aquecimento acrescenta 30 segundos ao tempo restante.
- String de aquecimento usa `.` no aquecimento manual.
- A quantidade de caracteres por segundo é igual à potência.
- Exemplos: tempo 10 e potência 1 gera `. . . . . . . . . .`; tempo 5 e potência 3 gera `... ... ... ... ...`.
- Ao final, concatenar `Aquecimento concluído`.
- Botão único de pausa/cancelamento:
  - se estiver aquecendo, pausa;
  - se estiver pausado, cancela e limpa as informações;
  - se acionado antes do aquecimento, limpa tempo e potência;
  - iniciar com sessão pausada retoma do ponto em que parou.

## Nível 2

- Disponibilizar 5 programas pré-definidos.
- Cada programa tem nome, alimento, tempo, potência, caractere/string de aquecimento e instruções.
- Caracteres dos programas não podem repetir e não podem ser `.`.
- Programas pré-definidos não podem ser alterados ou excluídos.
- Ao selecionar programa, tempo e potência são preenchidos automaticamente e não podem ser alterados.
- Programas pré-definidos não permitem acréscimo de tempo.
- Pausa e cancelamento são permitidos.
- Programas: Pipoca, Leite, Carnes de boi, Frango e Feijão.

## Nível 3

- Permitir cadastro de programas customizados.
- Nome, alimento, potência, caractere de aquecimento e tempo são obrigatórios.
- Instruções são opcionais.
- Caractere de aquecimento não pode repetir com nenhum programa e não pode ser `.`.
- Programas customizados devem aparecer junto aos pré-definidos, em itálico na UI.
- Persistência pode ser JSON ou SQL Server.

## Nível 4

- Exportar regras para Web API.
- Métodos de negócio de aquecimento e programas devem possuir endpoints.
- API deve usar autenticação Bearer Token.
- O programa deve indicar se autenticou com sucesso.
- Sem autenticação, nenhuma função deve executar.
- Credenciais devem ser configuradas em seção específica.
- Campo de senha deve ser mascarado.
- Senha deve ser criptografada usando SHA-256 para persistência.
- Se usar banco de dados, connection string deve ser criptografada e documentada descriptografada.
- Criar mecanismo padrão de tratamento de exceptions.
- Criar exception específica para regras de negócio.
- Exceptions não tratadas devem ser logadas em arquivo texto ou banco, com exception, inner exception, stacktrace e informações relevantes.
