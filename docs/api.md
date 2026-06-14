# API

## Autenticação

Base URL local:

```text
http://localhost:5167
```

Endpoints públicos:

- `POST /api/auth/register`
- `POST /api/auth/login`

Body:

```json
{
  "username": "admin",
  "password": "123456"
}
```

Resposta:

```json
{
  "userId": 1,
  "username": "admin",
  "token": "jwt-token"
}
```

Os demais endpoints exigem o header:

```text
Authorization: Bearer {token}
```

## Aquecimento

- `POST /api/microwave/quick-start`
- `POST /api/microwave/start`
- `POST /api/microwave/programs/{programId}/start`
- `POST /api/microwave/add-time`
- `POST /api/microwave/pause`
- `POST /api/microwave/pause-or-cancel`
- `POST /api/microwave/resume`
- `POST /api/microwave/advance?seconds=1`
- `POST /api/microwave/cancel`

Body de `POST /api/microwave/start`:

```json
{
  "timeInSeconds": 90,
  "power": 10
}
```

## Programas de Aquecimento

- `GET /api/heating-programs`
- `POST /api/heating-programs`

Body de cadastro:

```json
{
  "name": "Café",
  "food": "Café",
  "timeInSeconds": 45,
  "power": 8,
  "heatingChar": "!",
  "instructions": "Usar recipiente adequado."
}
```

## Resposta de erro padrão

```json
{
  "errorCode": "BUSINESS_ERROR",
  "message": "Mensagem de erro.",
  "traceId": "trace-id"
}
```

Erros de regra de negócio usam `BusinessException`. Erros inesperados são logados em arquivo no diretório configurado em `ExceptionLogging:Directory`.
