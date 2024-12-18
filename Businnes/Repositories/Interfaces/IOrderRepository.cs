using Domain.Entities;

namespace Businnes.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task InsertOrders(List<OnlineOrder> onlineOrders,
            CancellationToken cancellationToken = default);
    }
}
