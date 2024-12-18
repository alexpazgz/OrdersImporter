using Domain.Model;
using Domain.Summary;

namespace Businnes.Interfaces
{
    public interface IOrderImporterService
    {
        Task<SummaryViewModel> GetOrdersWithThreadsAsync();
        Task<SummaryViewModel> GetOrdersWithoutThreadsAsync();
        void SaveData(List<OnlineOrderModel> onlineOrdersModel);
        void GenerateFile(List<OnlineOrderModel> onlineOrdersModel);
        SummaryViewModel CreateSummary(List<OnlineOrderModel> onlineOrdersModel);
    }
}
