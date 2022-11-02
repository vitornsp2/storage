using System.Text;
using System.Text.Json;
using findox.Domain.Models.Service;
using findox.Test.TestObjects;
using Microsoft.AspNetCore.Mvc.Testing;

namespace findox.Test.End2End
{
    [Collection("Sequential")]
    public class SessionE2ETests : BaseTestClass, IClassFixture<WebApplicationFactory<Program>>
    {
        public SessionE2ETests(WebApplicationFactory<Program> webApplicationFactory) : base(webApplicationFactory)
        {
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostSessionsWithValidCredentialsShouldCreateNewToken()
        {

            //await _db.RecycleDb();
            var user = _dto.TestUserSessionAdminDto;
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                response = await client.PostAsync("/v1/sessions", content);
            }

            Assert.NotNull(response);
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ApiReponse>(responseJson, _jsonOpts);
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse?.Status);
            Assert.Equal("Success", controllerResponse?.Status);
            Assert.NotNull(controllerResponse?.Data);
            var token = controllerResponse?.Data.ToString();
            Assert.NotNull(token);
        }
    }
}