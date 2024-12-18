using AutoMapper;
using Businnes.Interfaces;
using Domain.Configuration.Clients;
using Domain.Model;
using Domain.Summary;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Businnes.Implementations
{
    public class OrderImporterService : IOrderImporterService
    {
        private readonly IApiKataEsPublicoService _apiKataEsPublicoService;
        private readonly IOnlineOrderService _onlineOrderService;
        private readonly IFileGeneratorService _fileGeneratorService;
        private readonly ISummaryService _summaryService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderImporterService> _logger;

        private readonly Stopwatch _watch;
        public OrderImporterService(IApiKataEsPublicoService apiKataEsPublicoService,
            IOptions<ApiKataEsPublicoConfiguration> apiKataEsPublicoConfiguration,
            IOnlineOrderService onlineOrderService,
            IFileGeneratorService fileGeneratorService,
            ISummaryService summaryService,
            IMapper mapper,
            ILogger<OrderImporterService> logger)
        {
            _apiKataEsPublicoService = apiKataEsPublicoService;
            _onlineOrderService = onlineOrderService;
            _fileGeneratorService = fileGeneratorService;
            _summaryService = summaryService;
            _mapper = mapper;
            _logger = logger;
            _watch = new Stopwatch();
        }

        public async Task<SummaryViewModel> GetOrdersWithThreadsAsync()
        {
            _watch.Start();

            var onlineOrdersApiKataResponse = await _apiKataEsPublicoService.GetOrdersWithThreadsFromApiAsync();

            var onlineOrdersModel = _mapper.Map<List<OnlineOrderModel>>(onlineOrdersApiKataResponse);

            var summary = DoCommonTask(onlineOrdersModel);

            return summary;
        }

        public async Task<SummaryViewModel> GetOrdersWithoutThreadsAsync()
        {
            _watch.Start();

            var onlineOrdersApiKataResponse = await _apiKataEsPublicoService.GetOrdersAsync();

            var onlineOrdersModel = _mapper.Map<List<OnlineOrderModel>>(onlineOrdersApiKataResponse);

            var summary = DoCommonTask(onlineOrdersModel);

            return summary;
        }

        public void SaveData(List<OnlineOrderModel> onlineOrdersModel)
        {
            _onlineOrderService.InsertOrdersFireAndForget(onlineOrdersModel);
        }

        public void GenerateFile(List<OnlineOrderModel> onlineOrdersModel)
        {
            _fileGeneratorService.GenerateFile(onlineOrdersModel);
        }

        public SummaryViewModel CreateSummary(List<OnlineOrderModel> onlineOrdersModel)
        {
            return _summaryService.Get(onlineOrdersModel);
        }

        #region private methods
        private SummaryViewModel DoCommonTask(List<OnlineOrderModel> onlineOrdersModel)
        {
            StopWatch(onlineOrdersModel.Count);

            SaveData(onlineOrdersModel);

            GenerateFile(onlineOrdersModel);

            var summary = CreateSummary(onlineOrdersModel);

            return summary;
        }

        private void StopWatch(int ordersCount)
        {
            _watch.Stop();

            var seconds = _watch.Elapsed.TotalSeconds.ToString();

            var messageLog = $"Tiempo en obtener ordenes : {seconds} seg. total ordenes: {ordersCount}";
            _logger.LogInformation(messageLog);
        }
        #endregion private methods
    }
}
