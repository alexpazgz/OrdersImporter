using Business.UnitTest.Data;
using Domain.ApiKataEsPublico;

namespace Business.UnitTest.Model
{
    public class PageOrderApiKataResponseTest
    {
        [Test]
        public async Task ShouldReturnEmptyList()
        {
            PageOrderApiKataResponse pageOrderApiKataResponse = new PageOrderApiKataResponse();

            var onlineOrdersApiKataResponse = pageOrderApiKataResponse.GetOrders();

            Assert.NotNull(onlineOrdersApiKataResponse);
            Assert.IsEmpty(onlineOrdersApiKataResponse);
        }

        [Test]
        public async Task ShouldReturnOnlineOrdersApiKataResponse()
        {
            PageOrderApiKataResponse pageOrderApiKataResponse = GetPageOrderApiKataResponse();

            var onlineOrdersApiKataResponse = pageOrderApiKataResponse.GetOrders();

            Assert.NotNull(onlineOrdersApiKataResponse);
            Assert.IsNotEmpty(onlineOrdersApiKataResponse);
        }

        [Test]
        public async Task ShouldReturnExistsNextPage()
        {
            PageOrderApiKataResponse pageOrderApiKataResponse = GetPageOrderApiKataResponse();

            var existNextPage = pageOrderApiKataResponse.CheckExistNextPage();

            Assert.That(existNextPage, Is.EqualTo(true));
        }

        [Test]
        public async Task ShouldReturnNotExistsNextPage()
        {
            PageOrderApiKataResponse pageOrderApiKataResponse = new PageOrderApiKataResponse();

            var existNextPage = pageOrderApiKataResponse.CheckExistNextPage();

            Assert.That(existNextPage, Is.EqualTo(false));
        }

        private PageOrderApiKataResponse GetPageOrderApiKataResponse()
        {
            var helper = new DataGeneratorHelper();
            PageOrderApiKataResponse pageOrderApiKataResponse = helper.GetPageOrderApiKataResponse();

            return pageOrderApiKataResponse;
        }
    }
}
