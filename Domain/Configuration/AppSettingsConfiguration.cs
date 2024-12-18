using System.Diagnostics.CodeAnalysis;

namespace Domain.Configuration
{
    [ExcludeFromCodeCoverage]
    public class AppSettingsConfiguration
    {
        public string PathReport { get; set; }
        public string NameReport { get; set; }
        public int TaskUntilWaiting { get; set; }
        public int MilisecondsWaitingBetweenTask { get; set; }
        public int BulkInsertRows { get; set; }
    }
}
