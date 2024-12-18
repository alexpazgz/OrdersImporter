namespace OrdersImporterJob.Base
{
    public interface IExecute
    {
        Task Execute(string[] args);
    }
}
