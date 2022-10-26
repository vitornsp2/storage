using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using findox.Test.TestObjects;
using Microsoft.AspNetCore.Mvc.Testing;

namespace findox.Test.End2End
{
    [Collection("Sequential")]
    public class DocumentE2ETests : IClassFixture<WebApplicationFactory<Program>>
    {
        private StorageDb _db;
        private readonly WebApplicationFactory<Program> _webApplicationFactory;
        private ObjectsDtoTest _dto;
        private JsonSerializerOptions _jsonOpts;


        public DocumentE2ETests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _db = new StorageDb();
            _webApplicationFactory = webApplicationFactory;
            _dto = new ObjectsDtoTest();
            _jsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        #region CREATE DOCUMENT

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostDocumentsAsAdminRoleWithNewDocumentShouldCreateNewDocumentReturn200Ok()
        {

            await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var filename = "findox.Test.pdb";
            var description = "A symbol file.";
            var category = "Symbols";
            FileStream fs = File.OpenRead(filename);
            HttpContent fileStreamContent = new StreamContent(fs);
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            HttpContent filenameContent = new StringContent(filename);
            HttpContent descriptionContent = new StringContent(description);
            HttpContent categoryContent = new StringContent(category);
            MultipartFormDataContent formdata = new MultipartFormDataContent();
            formdata.Add(fileStreamContent, "file", filename);
            formdata.Add(filenameContent, "filename");
            formdata.Add(descriptionContent, "description");
            formdata.Add(categoryContent, "category");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync("/v1/documents", formdata);
            }


            Assert.NotNull(response.ReasonPhrase);
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ControllerResponse>(responseJson, _jsonOpts);
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse?.Status);
            Assert.Equal("success", controllerResponse?.Status);
            Assert.NotNull(controllerResponse?.Data);
            var dataString = controllerResponse?.Data?.ToString();
            Assert.NotNull(dataString);
            var newDocument = JsonSerializer.Deserialize<DocumentDto>(dataString, _jsonOpts);
            Assert.NotNull(newDocument);
            Assert.NotNull(newDocument?.Id);
        }

        #endregion CREATE DOCUMENT
    }
}