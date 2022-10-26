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
    public class GroupsE2ETests : IClassFixture<WebApplicationFactory<Program>>
    {
        private StorageDb _db;
        private readonly WebApplicationFactory<Program> _webApplicationFactory;
        private ObjectsDtoTest _dto;
        private JsonSerializerOptions _jsonOpts;


        public GroupsE2ETests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _db = new StorageDb();
            _webApplicationFactory = webApplicationFactory;
            _dto = new ObjectsDtoTest();
            _jsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        #region CREATE GROUP

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostGroupsAsAdminRoleWithNewGroupDtoShouldCreateNewGroupReturn200Ok()
        {
         
            await _db.RecycleDb();
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
            var controllerResponse = JsonSerializer.Deserialize<ControllerResponse>(responseJson, _jsonOpts);
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse.Status);
            Assert.Equal("success", controllerResponse.Status);
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