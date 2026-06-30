using Domain.Dtos;

namespace Domain.Abstraction;

public interface IIntegracaoExemplo
{
    Task<IntegrationResponse> Integracao(string processingId);
}
