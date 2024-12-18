using Domain.ApiKataEsPublico;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Businnes.Clients
{
    public class ApiKataEsPublicoClient : IApiKataEsPublicoClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiKataEsPublicoClient> _logger;

        private const int maxAttemps = 3;
        private TimeSpan initialDelay = TimeSpan.FromSeconds(1);

        public ApiKataEsPublicoClient(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor context,
            ILogger<ApiKataEsPublicoClient> logger)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<PageOrderApiKataResponse?> GetOrderPageAsync(string uriGetOrders)
        {
            var httpRequestMessage = CreateRequest(uriGetOrders, "Get");

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            var response = await GetResponse(httpResponseMessage);

            return response;
        }

        public async Task<List<OnlineOrderApiKataResponse>> GetOrdersAsync(string uriGetOrders)
        {
            List<OnlineOrderApiKataResponse> response = new List<OnlineOrderApiKataResponse>();
            var attemps = 0;
            bool getOrders = false;

            do
            {
                attemps++;
                try
                {
                    var httpRequestMessage = CreateRequest(uriGetOrders, "Get");

                    var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

                    response = await GetResponseOrder(httpResponseMessage);
                    getOrders = true;
                }
                catch (Exception ex) when (attemps < maxAttemps)
                {
                    var message = $"Error al obtener ordenes. Se reintenta uri: {uriGetOrders}, itentos: {attemps}";
                    _logger.LogError(ex, message);
                    var delay = (int)Math.Pow(2, attemps) * initialDelay.TotalMilliseconds;
                    Thread.Sleep(TimeSpan.FromMilliseconds(delay));
                }
                catch (Exception ex) when (attemps == maxAttemps)
                {
                    var message = $"Error al obtener ordenes. Reintentos excedidos para uri: {uriGetOrders}.";
                    _logger.LogError(ex, message);
                    getOrders = false;
                }
            } while (!getOrders && attemps < maxAttemps);

            return response;
        }

        private async Task<PageOrderApiKataResponse?> GetResponse(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resp = JsonConvert.DeserializeObject<PageOrderApiKataResponse>(responseContent);
                return resp;
            }
            else
            {
                var jsonResult = JsonConvert.DeserializeObject<string>(responseContent);

                throw new Exception(String.Format("Error al recuperar datos en la petición, status code: {0}", response.StatusCode));
            }
        }

        private async Task<List<OnlineOrderApiKataResponse>> GetResponseOrder(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resp = JsonConvert.DeserializeObject<PageOrderApiKataResponse>(responseContent);
                if (resp != null)
                {
                    return resp.GetOrders();
                }
            }

            var message = $"Se ha producido un error al recuperar ordenes, error: {responseContent}";
            throw new Exception(message);

        }

        private HttpRequestMessage CreateRequest(string endpoint, string opertationMetod)
        {
            var httpRequestMessage = new HttpRequestMessage(new HttpMethod(opertationMetod),
                                                                endpoint);
            return httpRequestMessage;
        }
    }
}
