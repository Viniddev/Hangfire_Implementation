namespace Domain.Abstraction;

public interface IScheduler
{
    Task<string> AgendarBusca(string textToSend);
    Task<string> BuscarDados(string textRecived);
}
