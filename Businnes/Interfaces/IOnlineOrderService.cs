using Domain.ApiKataEsPublico;
using Domain.Model;

namespace Businnes.Interfaces
{
    public interface IOnlineOrderService
    {
        Task InsertOrdersApiKata(List<OnlineOrderApiKataResponse> onlineOrdersApiKataResponse);

        void InsertOrdersApiKataFireAndForget(List<OnlineOrderApiKataResponse> onlineOrdersApiKataResponse);

        Task InsertOrders(List<OnlineOrderModel> onlineOrdersModel);

        void InsertOrdersFireAndForget(List<OnlineOrderModel> onlineOrdersModel);
    }
}
