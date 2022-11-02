using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using findox.Test.TestObjects;
using Microsoft.AspNetCore.Mvc.Testing;

namespace findox.Test.End2End
{
[Collection("Sequential")]
    public class GroupsE2ETests : BaseTestClass, IClassFixture<WebApplicationFactory<Program>>
    {
        public GroupsE2ETests(WebApplicationFactory<Program> webApplicationFactory) : base(webApplicationFactory)
        {
        }

        #region CREATE GROUP

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostGroupsAsAdminRoleWithNewGroupDtoShouldCreateNewGroupReturn200Ok()
        {
         
           // await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var group = _dto.TestGroupNewDto;
            var json = JsonSerializer.Serialize(group);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;


            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync("/v1/groups", content);
            }


            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("OK", response.ReasonPhrase.ToString());
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ApiReponse>(responseJson, _jsonOpts);
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse.Status);
            Assert.Equal("Success", controllerResponse.Status);
            Assert.NotNull(controllerResponse.Data);
            var dataString = controllerResponse.Data.ToString();
            Assert.NotNull(dataString);
            var newGroup = JsonSerializer.Deserialize<GroupDto>(dataString, _jsonOpts);
            Assert.NotNull(newGroup);
            Assert.NotNull(newGroup.Id);
        }

        #endregion CREATE GROUP
    }
}