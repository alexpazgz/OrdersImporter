using System.Net;

namespace IntegrationTests.Controllers
{
    public class OrderControllerTests : IntegrationTest
    {

        public OrderControllerTests()
            : base()
        {
        }

        [Test]
        public async Task OrderController_ShouldReturnOK()
        {
            var getRequest = new HttpRequestMessage(HttpMethod.Get, "/Order/v1/GetOrders");

            var response = await _client.SendAsync(getRequest);

            response.EnsureSuccessStatusCode();

            //var responseString = await response.Content.ReadAsStringAsync();
            Task.Delay(2000).Wait();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }
    }
}
