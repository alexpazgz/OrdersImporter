namespace Domain.Configuration.Clients
{
    public class ApiKataEsPublicoConfiguration
    {
        public const string SectionName = "ApiKataEsPublicoSettings";

        public string BaseAddress { get; set; }
        public string MethodGetOrders { get; set; }
        public int TotalOrders { get; set; }
        public int MaxRowsPerPage { get; set; }
    }
}
