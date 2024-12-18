using Domain.Model;
using Domain.Summary;

namespace Businnes.Interfaces
{
    public interface ISummaryService
    {
        SummaryViewModel Get(List<OnlineOrderModel> onlineOrdersModel);
    }
}
