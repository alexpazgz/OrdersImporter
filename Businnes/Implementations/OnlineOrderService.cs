using AutoMapper;
using Businnes.FireForgetService;
using Businnes.Interfaces;
using Businnes.Repositories.Interfaces;
using Domain.ApiKataEsPublico;
using Domain.Entities;
using Domain.Model;
using Microsoft.Extensions.Logging;

namespace Businnes.Implementations
{
    public class OnlineOrderService : IOnlineOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFireForgetService _fireForgetService;
        private readonly IMapper _mapper;
        private readonly ILogger<OnlineOrderService> _logger;

        public OnlineOrderService(IOrderRepository orderRepository,
            IFireForgetService fireForgetService,
            IMapper mapper,
            ILogger<OnlineOrderService> logger)
        {
            _orderRepository = orderRepository;
            _fireForgetService = fireForgetService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task InsertOrdersApiKata(List<OnlineOrderApiKataResponse> onlineOrdersApiKataResponse)
        {
            try
            {
                var onlineOrders = _mapper.Map<List<OnlineOrder>>(onlineOrdersApiKataResponse);

                await _orderRepository.InsertOrders(onlineOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al mapear e intentar insertar ordenes en BBDD");
            }
        }

        public void InsertOrdersApiKataFireAndForget(List<OnlineOrderApiKataResponse> onlineOrdersApiKataResponse)
        {
            _fireForgetService.Execute(async (databaseWork) =>
            {
                await databaseWork.InsertOrdersApiKata(onlineOrdersApiKataResponse);
            });
        }

        public async Task InsertOrders(List<OnlineOrderModel> onlineOrdersModel)
        {
            try
            {
                var onlineOrders = _mapper.Map<List<OnlineOrder>>(onlineOrdersModel);

                await _orderRepository.InsertOrders(onlineOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al mapear e intentar insertar ordenes en BBDD");
            }
        }

        public void InsertOrdersFireAndForget(List<OnlineOrderModel> onlineOrdersModel)
        {
            _fireForgetService.Execute(async (databaseWork) =>
            {
                await databaseWork.InsertOrders(onlineOrdersModel);
            });
        }
    }
}
