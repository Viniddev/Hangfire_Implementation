using Api.Configuration;
using Domain.Abstraction;
using Hangfire;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

#region configs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerConfiguration();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddHttpClient();

builder.Services.AddHangFireSqlServer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard("/hangfire");
    app.UseSwaggerConfiguration();
}

app.UseHttpsRedirection();
#endregion

app.MapPost("/agendar-tarefa/{identificador}", async ([FromServices] IScheduler _scheduler, string identificador) =>
{
    var result = await _scheduler.AgendarBusca(identificador);

    return $"Tarefa {identificador} Agendada: {result}";
})
.WithName("agendar-tarefa");

app.MapGet("/consultar-agendamento/{identificador}", async ([FromServices] IIntegracaoExemplo _integracao, string identificador) =>
{
    var result = await _integracao.Integracao(identificador);

    return result;
})
.WithName("consultar-agendamento");

app.Run();