using AutoMapper;
using Business.UnitTest.Data;
using Businnes.AutoMapper;
using Businnes.Csv;
using Businnes.FireForgetService;
using Domain.ApiKataEsPublico;
using Domain.Configuration;
using Domain.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Business.UnitTest.Services
{
    public class CsvServiceTest
    {
        private readonly CsvService _csvService;
        private readonly Mock<ILogger<CsvService>> _mockLogger;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        private readonly Mock<IOptions<AppSettingsConfiguration>> _mockAppSettingsConfiguration;

        private readonly IFireForgetService _fireForgetService;

        const string CSV_EXTENSION = ".csv";

        public CsvServiceTest()
        {
            _mockLogger = new Mock<ILogger<CsvService>>();

            _configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _mockAppSettingsConfiguration = new Mock<IOptions<AppSettingsConfiguration>>();


            AppSettingsConfiguration settings = new AppSettingsConfiguration()
            {
                PathReport = "C:\\Temp\\OnlineOrder",
                NameReport = $"OnlineOrder_{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                TaskUntilWaiting = 150,
                MilisecondsWaitingBetweenTask = 1000,
                BulkInsertRows = 5000
            };

            _mockAppSettingsConfiguration.Setup(ap => ap.Value).Returns(settings);

            _fireForgetService = ServicesHelper.GetRequiredService<IFireForgetService>() ?? throw new ArgumentNullException(nameof(IFireForgetService));

            _csvService = new CsvService(_mapper, _fireForgetService, _mockLogger.Object);
        }

        [Test]
        public async Task ShouldCreateCsvFile()
        {
            PageOrderApiKataResponse pageOrderApiKataResponse = new PageOrderApiKataResponse();

            var onlineOrdersApiKataResponse = GetOnlineOrderApiKataResponse();

            var onlineOrderModel = _mapper.Map<List<OnlineOrderModel>>(onlineOrdersApiKataResponse);

            var filePath = Path.Combine(_mockAppSettingsConfiguration.Object.Value.PathReport,
                $"{_mockAppSettingsConfiguration.Object.Value.NameReport}{CSV_EXTENSION}");

            await _csvService.WriteCsvAsync(onlineOrderModel, filePath);
        }

        private List<OnlineOrderApiKataResponse> GetOnlineOrderApiKataResponse()
        {
            var helper = new DataGeneratorHelper();
            List<OnlineOrderApiKataResponse> onlineOrdersApiKataResponse = helper.GetOnlineOrdersApiKataResponse();

            return onlineOrdersApiKataResponse;
        }
    }
}
