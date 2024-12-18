using Domain.Model;

namespace Businnes.Csv
{
    public interface ICsvService
    {
        void WriteCsvGenericList<T>(List<T> list, string filePath);

        void WriteCsvFireAndForget(List<OnlineOrderModel> onlineOrdersModel, string filePath);

        Task WriteCsvAsync(List<OnlineOrderModel> onlineOrdersModel, string filePath);
    }
}
