using AutoMapper;
using Business.UnitTest.Data;
using Businnes.AutoMapper;
using Businnes.Implementations;
using Businnes.Interfaces;
using Domain.ApiKataEsPublico;
using Domain.Model;
using Domain.Summary;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace Business.UnitTest.Services
{
    public class SummaryServiceTest
    {
        private readonly ISummaryService _summaryService;
        private readonly Mock<ILogger<SummaryService>> _mockLogger;

        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public SummaryServiceTest()
        {
            _mockLogger = new Mock<ILogger<SummaryService>>();
            _summaryService = new SummaryService(_mockLogger.Object);

            _configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public async Task ShouldReturnGoodSummary()
        {
            var onlineOrdersApiKataResponse = GetOnlineOrderApiKataResponse();

            var onlineOrderModel = _mapper.Map<List<OnlineOrderModel>>(onlineOrdersApiKataResponse);

            var createdSummary = _summaryService.Get(onlineOrderModel);

            var summary = GetSummary();

            var createdSummaryJson = JsonConvert.SerializeObject(createdSummary);
            var summaryJson = JsonConvert.SerializeObject(summary);

            Assert.That(createdSummaryJson, Is.EqualTo(summaryJson));
        }

        private List<OnlineOrderApiKataResponse> GetOnlineOrderApiKataResponse()
        {
            var helper = new DataGeneratorHelper();
            List<OnlineOrderApiKataResponse> onlineOrdersApiKataResponse = helper.GetOnlineOrdersApiKataResponse();

            return onlineOrdersApiKataResponse;
        }

        private SummaryViewModel GetSummary()
        {
            var helper = new DataGeneratorHelper();
            SummaryViewModel summary = helper.GetSummary();

            return summary;
        }
    }
}
