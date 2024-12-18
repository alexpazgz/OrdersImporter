using Domain.ApiKataEsPublico;

namespace Businnes.Interfaces
{
    public interface IApiKataEsPublicoService
    {
        Task<List<OnlineOrderApiKataResponse>> GetOrdersWithThreadsFromApiAsync();

        Task<List<OnlineOrderApiKataResponse>> GetOrdersAsync();
    }
}
