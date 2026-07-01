# Hangfire Scheduler API

Minimal API responsável por simular e acompanhar processamentos assíncronos utilizando Hangfire.

A aplicação recebe um identificador, cria um job em background e realiza consultas periódicas até que o processamento seja concluído.

## Endpoints

### Agendar processamento

```http
POST /agendar/{identificador}
```

Exemplo:

```http
POST /agendar/abc123
```

Esse endpoint envia o identificador para o `Scheduler`, que cria um job no Hangfire para iniciar a consulta.

---

### Consultar agendamento

```http
GET /consultar-agendamento/{identificador}
```

Exemplo:

```http
GET /consultar-agendamento/abc123
```

Esse endpoint simula uma integração externa. Nas duas primeiras consultas, retorna o status `102` indicando processamento em andamento. Na terceira, retorna `200`, indicando conclusão.

## Fluxo do processamento

1. O cliente chama `POST /agendar/{identificador}`.
2. O Hangfire adiciona o job `BuscarDados` à fila.
3. O job consulta a integração externa usando o identificador.
4. Caso o status ainda não seja `200`, um novo job é agendado para executar após 30 segundos.
5. Quando a integração retorna sucesso, o processamento é encerrado.

## Tecnologias

* .NET Minimal API
* Hangfire
* HttpClient
* Newtonsoft.Json
* Injeção de Dependência
* Background Jobs

## Dashboard

Após iniciar a aplicação, o dashboard do Hangfire pode ser acessado em:

```text
/hangfire
```
