using Domain.Abstraction;
using Domain.Dtos;
using System.Collections.Concurrent;

namespace HangFire.Implementation;

public class IntegracaoExemplo : IIntegracaoExemplo
{
    private readonly ConcurrentDictionary<string, int> _tentativasPorProcessamento = new();

    public Task<IntegrationResponse> Integracao(string processingId)
    {
        if (string.IsNullOrWhiteSpace(processingId))
        {
            return Task.FromResult(new IntegrationResponse
            {
                StatusId = 400,
                StatusMessage = "ProcessingId inválido."
            });
        }

        var tentativaAtual = _tentativasPorProcessamento.AddOrUpdate(
            processingId,
            1,
            (_, tentativaAnterior) => tentativaAnterior + 1
        );

        Console.WriteLine($"Ainda processando a requisição {processingId}: {tentativaAtual} ...");

        if (tentativaAtual < 3)
        {
            return Task.FromResult(new IntegrationResponse
            {
                StatusId = 102,
                StatusMessage = $"Processamento em andamento. Consulta {tentativaAtual} de 3."
            });
        }

        _tentativasPorProcessamento.TryRemove(processingId, out _);

        return Task.FromResult(new IntegrationResponse
        {
            StatusId = 200,
            StatusMessage = "Processamento concluído com sucesso."
        });
    }
}
