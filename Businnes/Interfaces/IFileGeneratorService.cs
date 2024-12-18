using Domain.Model;

namespace Businnes.Interfaces
{
    public interface IFileGeneratorService
    {
        void GenerateFile(List<OnlineOrderModel> onlineOrdersModel);
    }
}
