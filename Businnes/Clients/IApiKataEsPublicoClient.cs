using Domain.ApiKataEsPublico;

namespace Businnes.Clients
{
    public interface IApiKataEsPublicoClient
    {
        Task<PageOrderApiKataResponse?> GetOrderPageAsync(string uriGetOrders);

        Task<List<OnlineOrderApiKataResponse>> GetOrdersAsync(string uriGetOrders);
    }
}
