using Businnes.Csv;
using Businnes.Interfaces;

namespace Businnes.FireForgetService
{
    public interface IFireForgetService
    {
        void Execute(Func<IOnlineOrderService, Task> databaseWork);

        void Execute(Func<ICsvService, Task> csvServiceWork);
    }
}
