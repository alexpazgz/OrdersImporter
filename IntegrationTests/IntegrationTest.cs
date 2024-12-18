using Xunit;

namespace IntegrationTests
{
    public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly HttpClient _client;
        protected IntegrationTest()
        {
            var fixture = new ApiWebApplicationFactory();
            _client = fixture.CreateClient();
        }
    }
}
