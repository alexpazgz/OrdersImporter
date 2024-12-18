using Businnes.Clients;
using Domain.ApiKataEsPublico;
using Domain.Configuration.Clients;
using Domain.Configuration;
using Domain.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Businnes.Interfaces;

namespace Businnes.Implementations
{
    public class ApiKataEsPublicoService : IApiKataEsPublicoService
    {
        private readonly IApiKataEsPublicoClient _apiKataEsPublicoClient;
        private readonly IOnlineOrderService _onlineOrderService;
        private readonly AppSettingsConfiguration _appSettingsConfiguration;
        private readonly ApiKataEsPublicoConfiguration _apiKataEsPublicoConfiguration;

        public ApiKataEsPublicoService(IApiKataEsPublicoClient apiKataEsPublicoClient,
            IOptions<AppSettingsConfiguration> appSettingsConfiguration,
            IOptions<ApiKataEsPublicoConfiguration> apiKataEsPublicoConfiguration,
            IOnlineOrderService onlineOrderService)
        {
            _apiKataEsPublicoClient = apiKataEsPublicoClient;
            _onlineOrderService = onlineOrderService;
            _appSettingsConfiguration = appSettingsConfiguration.Value;
            _apiKataEsPublicoConfiguration = apiKataEsPublicoConfiguration.Value;
        }

        public async Task<List<OnlineOrderApiKataResponse>> GetOrdersWithThreadsFromApiAsync()
        {
            var semaphoreSlim = new SemaphoreSlim(10, 10);

            var tasks = new List<Task<List<OnlineOrderApiKataResponse>>>();
            string baseAddress = $"{_apiKataEsPublicoConfiguration.BaseAddress}{_apiKataEsPublicoConfiguration.MethodGetOrders}";

            var batchSize = _apiKataEsPublicoConfiguration.MaxRowsPerPage;
            int numberOfBatches = (int)Math.Ceiling((double)_apiKataEsPublicoConfiguration.TotalOrders / batchSize);

            Dictionary<string, string> param;
            string uriGetOrders;
            for (int i = 1; i <= numberOfBatches; i++)
            {
                // Wait for MilisecondsWaitingBetweenTask, to avoid crashing the server
                if ((i % _appSettingsConfiguration.TaskUntilWaiting) == 0)
                {
                    Thread.Sleep(_appSettingsConfiguration.MilisecondsWaitingBetweenTask);
                }

                await semaphoreSlim.WaitAsync();

                param = new Dictionary<string, string>()
                {
                    { "page", i.ToString() },
                    { "max-per-page", _apiKataEsPublicoConfiguration.MaxRowsPerPage.ToString() }
                };

                uriGetOrders = QueryHelpers.AddQueryString(baseAddress, param);
                try
                {
                    tasks.Add((_apiKataEsPublicoClient.GetOrdersAsync(uriGetOrders)));
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }

            return (await Task.WhenAll(tasks)).SelectMany(u => u).ToList();

        }


        public async Task<List<OnlineOrderApiKataResponse>> GetOrdersAsync()
        {
            List<OnlineOrderApiKataResponse> onlineOrdersApiKataResponse = new List<OnlineOrderApiKataResponse>();

            var uriGetOrders = $"{_apiKataEsPublicoConfiguration.BaseAddress}{_apiKataEsPublicoConfiguration.MethodGetOrders}";

            bool hasMorePages = true;

            PageOrderApiKataResponse? pageOrderApiKataResponse;
            while (hasMorePages)
            {
                try
                {
                    pageOrderApiKataResponse = await _apiKataEsPublicoClient.GetOrderPageAsync(uriGetOrders);
                }
                catch (Exception)
                {
                    throw new OrderImporterFailedDependencyException("Error en la importación de ordenes de ApiKataEsPublico");
                }

                if (pageOrderApiKataResponse != null)
                {
                    onlineOrdersApiKataResponse.AddRange(pageOrderApiKataResponse.GetOrders());
                    hasMorePages = pageOrderApiKataResponse.CheckExistNextPage();

                    if (hasMorePages)
                    {
                        uriGetOrders = pageOrderApiKataResponse.Links.Next;
                    }
                }
            }
            return onlineOrdersApiKataResponse;
        }

        //IAsyncEnumerable manage orders

        //public async Task<List<OnlineOrderApiKataResponse>> GetAndSaveOrdersAsync()
        //{
        //    List<OnlineOrderApiKataResponse> onlineOrdersApiKataResponse = new List<OnlineOrderApiKataResponse>();

        //    await foreach (var ordersApiKataResponseItem in GetOrdersFromApi())
        //    {
        //        onlineOrdersApiKataResponse.AddRange(ordersApiKataResponseItem.ToList());

        //        _onlineOrderService.InsertOrdersApiKataFireAndForget(ordersApiKataResponseItem.ToList());
        //    }
        //    return onlineOrdersApiKataResponse;
        //}

        //private async IAsyncEnumerable<IEnumerable<OnlineOrderApiKataResponse>> GetOrdersFromApi()
        //{
        //    IEnumerable<OnlineOrderApiKataResponse> onlineOrdersApiKataResponse = new List<OnlineOrderApiKataResponse>();

        //    var uriGetOrders = $"{_apiKataEsPublicoConfiguration.BaseAddress}{_apiKataEsPublicoConfiguration.MethodGetOrders}";

        //    bool hasMorePages = true;

        //    while (hasMorePages)
        //    {
        //        PageOrderApiKataResponse? pageOrderApiKataResponse;
        //        try
        //        {
        //            pageOrderApiKataResponse = await _apiKataEsPublicoClient.GetOrderPageAsync(uriGetOrders);
        //        }
        //        catch (Exception)
        //        {
        //            throw new OrderImporterFailedDependencyException("Error en la importación de ordenes de ApiKataEsPublico");
        //        }

        //        if (pageOrderApiKataResponse != null)
        //        {
        //            onlineOrdersApiKataResponse = onlineOrdersApiKataResponse.Concat(pageOrderApiKataResponse.GetOrders());
        //            hasMorePages = pageOrderApiKataResponse.CheckExistNextPage();

        //            if (hasMorePages)
        //            {
        //                uriGetOrders = pageOrderApiKataResponse.Links.Next;
        //                if (onlineOrdersApiKataResponse.Count() == _appSettingsConfiguration.BulkInsertRows)
        //                {
        //                    yield return onlineOrdersApiKataResponse;
        //                    onlineOrdersApiKataResponse = Enumerable.Empty<OnlineOrderApiKataResponse>();
        //                }
        //            }
        //            else
        //            {
        //                yield return onlineOrdersApiKataResponse;
        //            }
        //        }
        //    }
        //}

        //Multithreading with next page

        //private static SemaphoreSlim semaphoreSlimm = new SemaphoreSlim(10, 10);
        //private static List<Task<List<OnlineOrderApiKataResponse>>>  taskss = new List<Task<List<OnlineOrderApiKataResponse>>>();
        //public async Task<List<OnlineOrderApiKataResponse>> GetOrdersWithThreadsFromApiAsync()
        //{
        //    var uriGetOrders = $"{_apiKataEsPublicoConfiguration.BaseAddress}{_apiKataEsPublicoConfiguration.MethodGetOrders}";

        //    var liiiisti = await Gettttt(uriGetOrders);

        //    return liiiisti;
        //}

        //private async Task<List<OnlineOrderApiKataResponse>> Gettttt(string uri)
        //{


        //    if (!string.IsNullOrEmpty(uri))
        //    {
        //        var pageOrderApiKataResponse = await _apiKataEsPublicoClient.GetOrderPageAsync(uri);

        //        if (pageOrderApiKataResponse != null)
        //        {
        //            var hasMorePages = pageOrderApiKataResponse.CheckExistNextPage();
        //            if (hasMorePages)
        //            {
        //                await semaphoreSlimm.WaitAsync();
        //                try
        //                {
        //                    taskss.Add(Gettttt(pageOrderApiKataResponse.Links.Next));
        //                }
        //                finally
        //                {
        //                    semaphoreSlimm.Release();
        //                }
        //            }

        //            return (await Task.WhenAll(taskss)).SelectMany(u => u).ToList();
        //        }
        //    }
        //    return new List<OnlineOrderApiKataResponse>();
        //}
    }
}
