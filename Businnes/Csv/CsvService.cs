using AutoMapper;
using Businnes.FireForgetService;
using CsvHelper;
using Domain.Csv;
using Domain.Model;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Businnes.Csv
{
    public class CsvService : ICsvService
    {
        private readonly IMapper _mapper;
        private readonly IFireForgetService _fireForgetService;
        private readonly ILogger<CsvService> _logger;

        public CsvService(IMapper mapper,
            IFireForgetService fireForgetService,
            ILogger<CsvService> logger)
        {
            _mapper = mapper;
            _fireForgetService = fireForgetService;
            _logger = logger;
        }

        public async Task WriteCsvAsync(List<OnlineOrderModel> onlineOrdersModel,
            string filePath)
        {
            try
            {
                var message = $"Se va a crear el fichero {filePath}";
                _logger.LogInformation(message);

                var onlineOrdersCsv = _mapper.Map<List<OnlineOrderCsv>>(onlineOrdersModel);

                CreatePathIfNotExist(filePath);

                using var writer = new StreamWriter(filePath);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                await csv.WriteRecordsAsync(onlineOrdersCsv.OrderBy(o => long.Parse(o.Id)));

                message = $"Fichero creado correctamente: {filePath}";
                _logger.LogInformation(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el fichero .csv");
            }
        }

        public void WriteCsvFireAndForget(List<OnlineOrderModel> onlineOrdersModel,
            string filePath)
        {
            _fireForgetService.Execute(async (csvServiceWork) =>
            {
                await csvServiceWork.WriteCsvAsync(onlineOrdersModel, filePath);
            });
        }

        public void WriteCsvGenericList<T>(List<T> list, string filePath)
        {
            try
            {
                CreatePathIfNotExist(filePath);

                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(list);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el archivo .csv");
            }
        }

        private void CreatePathIfNotExist(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directoryPath) &&
                    !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
        }
    }
}
