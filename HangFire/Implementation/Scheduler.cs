using Domain.Abstraction;
using Domain.Dtos;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HangFire.Implementation;

public class Scheduler(
    IConfiguration configuration,
    IBackgroundJobClient backgroundClient,
    HttpClient client
) : IScheduler
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IBackgroundJobClient _backgroundClient = backgroundClient;
    private readonly HttpClient _client = client;

    public async Task<string> AgendarBusca(string textToSend)
    {
        if (string.IsNullOrEmpty(textToSend))
        {
            Console.WriteLine($"Erro: Body nao pode ser null ou empty");

            return await Task.FromResult($"Erro: Body nao pode ser null ou empty");
        }

        _backgroundClient.Enqueue<IScheduler>(j => j.BuscarDados(textToSend)); //aqui ele coloca o job na fila

        return await Task.FromResult($"Agendamento realizado para: {textToSend}");
    }

    public async Task<string> BuscarDados(string executionId)
    {
        var consulta = $"{_configuration["search-url"]}/{executionId}";

        var response = await _client.GetAsync(consulta);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Erro {(int)response.StatusCode}: {content}");

            return string.Empty;
        }

        var result = JsonConvert.DeserializeObject<IntegrationResponse>(content);

        if (result?.StatusId == 200)
        {
            Console.WriteLine($"Consulta concluída para {content}!");

            return content;
        }

        Console.WriteLine($"Ainda processando a requisição {executionId} ...");

        _backgroundClient.Schedule<IScheduler>(j => j.BuscarDados(executionId), TimeSpan.FromSeconds(30));

        return string.Empty;
    }
}
