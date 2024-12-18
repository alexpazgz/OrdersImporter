using Businnes.Csv;
using Businnes.Interfaces;
using Domain.Configuration;
using Domain.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Businnes.Implementations
{
    public class FileGeneratorService : IFileGeneratorService
    {
        private readonly ICsvService _csvService;
        private readonly ILogger<FileGeneratorService> _logger;
        private readonly string _filePath;
        const string CSV_EXTENSION = ".csv";

        public FileGeneratorService(IOptions<AppSettingsConfiguration> appSettingsConfiguration,
            ICsvService csvService,
            ILogger<FileGeneratorService> logger)
        {
            _csvService = csvService;
            _logger = logger;
            var _appSettingsConfiguration = appSettingsConfiguration.Value;

            _filePath = Path.Combine(_appSettingsConfiguration.PathReport,
                $"{_appSettingsConfiguration.NameReport}{CSV_EXTENSION}");
        }

        public void GenerateFile(List<OnlineOrderModel> onlineOrdersModel)
        {
            try
            {
                _csvService.WriteCsvFireAndForget(onlineOrdersModel,
                    _filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el fichero");
            }
        }
    }
}
