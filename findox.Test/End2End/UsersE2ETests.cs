

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using findox.Test.TestObjects;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace findox.Test.End2End
{
 [Collection("Sequential")]
    public class UsersE2ETests : BaseTestClass, IClassFixture<WebApplicationFactory<Program>>
    {
        public UsersE2ETests(WebApplicationFactory<Program> webApplicationFactory) : base(webApplicationFactory)
        {
        }

        #region CREATE USER

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersAsAdminRoleWithNewRegularUserDtoShouldCreateNewUserReturn200Ok()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _dto.TestUserNewRegularDto;
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync("/v1/users", content);
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
            var newUser = JsonSerializer.Deserialize<UserDto>(dataString, _jsonOpts);
            Assert.NotNull(newUser);
            Assert.NotNull(newUser.Id);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersAsManagerRoleWithNewRegularUserDtoShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateManager();
            var user = _dto.TestUserNewRegularDto;
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync("/v1/users", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersAsRegularRoleWithNewRegularUserDtoShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateRegular();
            var user = _dto.TestUserNewRegularDto;
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync("/v1/users", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersAsAdminRoleWithIncompleteUserDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _dto.TestUserIncompleteDto;
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync("/v1/users", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ApiReponse>(responseJson, _jsonOpts);
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse.Status);
            Assert.Equal("Error", controllerResponse.Status);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersAsAdminRoleWithExistingRegularUserDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _dto.TestUserRegularDto;
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync("/v1/users", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ApiReponse>(responseJson, _jsonOpts);
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse.Status);
            Assert.Equal("Error", controllerResponse.Status);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersAsAdminRoleWithEmptyStringsUserDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _dto.TestUserEmptyStringsDto;
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync("/v1/users", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ApiReponse>(responseJson, _jsonOpts);
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse.Status);
            Assert.Equal("Error", controllerResponse.Status);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersAsAdminRoleWithoutUserDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var json = JsonSerializer.Serialize("");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync("/v1/users", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        #endregion CREATE USER

        #region CREATE USER USERGROUP

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdUserGroupsAsAdminRoleWithNewUserGroupDtoShouldCreateNewUserGroupReturn200Ok()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var group = _db.GetGroupLaughingstocks();
            var user = _db.GetUserRegular();
            var userGroup = new UserGroupDto()
            {
                GroupId = group.Id,
                UserId = user.Id
            };
            var json = JsonSerializer.Serialize(userGroup);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/userGroups", content);
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
            var newUserGroup = JsonSerializer.Deserialize<UserGroupDto>(dataString, _jsonOpts);
            Assert.NotNull(newUserGroup);
            Assert.NotNull(newUserGroup.Id);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdUserGroupsAsManagerRoleWithNewUserGroupDtoShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateManager();
            var group = _db.GetGroupLaughingstocks();
            var user = _db.GetUserRegular();
            var userGroup = new UserGroupDto()
            {
                GroupId = group.Id,
                UserId = user.Id
            };
            var json = JsonSerializer.Serialize(userGroup);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/userGroups", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdUserGroupsAsRegularRoleWithNewUserGroupDtoShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateRegular();
            var group = _db.GetGroupLaughingstocks();
            var user = _db.GetUserRegular();
            var userGroup = new UserGroupDto()
            {
                GroupId = group.Id,
                UserId = user.Id
            };
            var json = JsonSerializer.Serialize(userGroup);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/userGroups", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdUserGroupsAsAdminRoleWithIncompleteNewUserGroupDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var group = _db.GetGroupLaughingstocks();
            var user = _db.GetUserRegular();
            var userGroup = new UserGroupDto()
            {
                GroupId = group.Id
            };
            var json = JsonSerializer.Serialize(userGroup);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/userGroups", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdUserGroupsAsAdminRoleWithExistingUserGroupDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var group = _db.GetGroupAdmins();
            var user = _db.GetUserAdmin();
            var userGroup = new UserGroupDto()
            {
                GroupId = group.Id,
                UserId = user.Id
            };
            var json = JsonSerializer.Serialize(userGroup);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/userGroups", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdUserGroupsAsAdminRoleWithEmptyGuidUserGroupDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var userGroup = new UserGroupDto()
            {
                GroupId = 0,
                UserId = 0
            };
            var json = JsonSerializer.Serialize(userGroup);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;
            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{0}/userGroups", content);
            }
     
            Assert.NotNull(response.ReasonPhrase);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdUserGroupsAsAdminRoleWithoutUserGroupDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var json = JsonSerializer.Serialize("");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{0}/userGroups", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        #endregion CREATE USER USERGROUP

        #region CREATE USER PERMISSION

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdPermissionsAsAdminRoleWithNewPermissionDtoShouldCreateNewPermissionReturn200Ok()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var document = _db.GetDocumentApiDll();
            var user = _db.GetUserRegular();
            var permission = new PermissionDto()
            {
                DocumentId = document.Id,
                UserId = user.Id
            };
            var json = JsonSerializer.Serialize(permission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;
            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/permissions", content);
            }
            
            Assert.NotNull(response.ReasonPhrase);
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ApiReponse>(responseJson, _jsonOpts);
            Assert.Equal("OK", response.ReasonPhrase.ToString());
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse.Status);
            Assert.Equal("Success", controllerResponse.Status);
            Assert.NotNull(controllerResponse.Data);
            var dataString = controllerResponse.Data.ToString();
            Assert.NotNull(dataString);
            var newPermission = JsonSerializer.Deserialize<PermissionDto>(dataString, _jsonOpts);
            Assert.NotNull(newPermission);
            Assert.NotNull(newPermission.Id);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdPermissionsAsManagerRoleWithNewPermissionDtoShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateManager();
            var document = _db.GetDocumentApiDll();
            var user = _db.GetUserRegular();
            var permission = new PermissionDto()
            {
                DocumentId = document.Id,
                UserId = user.Id
            };
            var json = JsonSerializer.Serialize(permission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/permissions", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdPermissionsAsRegularRoleWithNewPermissionDtoShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateRegular();
            var document = _db.GetDocumentApiDll();
            var user = _db.GetUserRegular();
            var permission = new PermissionDto()
            {
                DocumentId = document.Id,
                UserId = user.Id
            };
            var json = JsonSerializer.Serialize(permission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/permissions", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdPermissionsAsAdminRoleWithIncompletePermissionDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var document = _db.GetDocumentApiDll();
            var user = _db.GetUserRegular();
            var permission = new PermissionDto()
            {
                DocumentId = document.Id
            };
            var json = JsonSerializer.Serialize(permission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/permissions", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdPermissionsAsAdminRoleWithExistingPermissionDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var document = _db.GetDocumentApiDll();
            var user = _db.GetUserAdmin();
            var permission = new PermissionDto()
            {
                DocumentId = document.Id,
                UserId = user.Id
            };
            var json = JsonSerializer.Serialize(permission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{user.Id}/permissions", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdPermissionsAsAdminRoleWithEmptyGuidPermissionDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var permission = new PermissionDto()
            {
                DocumentId = 0,
                UserId = 0
            };
            var json = JsonSerializer.Serialize(permission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{0}/permissions", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PostUsersIdPermissionsAsAdminRoleWithoutPermissionDtoShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var json = JsonSerializer.Serialize("");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PostAsync($"/v1/users/{0}/permissions", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        #endregion CREATE USER PERMISSION

        #region READ ALL USERS

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task GetUsersAsAdminRoleShouldReturnUsersList200Ok()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync("/v1/users");
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
            var users = JsonSerializer.Deserialize<List<UserDto>>(dataString, _jsonOpts);
            Assert.NotNull(users);
            Assert.True(users.Count > 0);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task GetUsersAsManagerRoleShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateManager();
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync("/v1/users");
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task GetUsersAsRegularRoleShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateRegular();
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync("/v1/users");
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        #endregion READ ALL USERS

        #region READ USER BY ID

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task GetUserByIdAsAdminRoleWithValidIdShouldReturnUser200Ok()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _db.GetUserRegular();
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync($"/v1/users/{user.Id}");
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
            var userReturned = JsonSerializer.Deserialize<UserAllDto>(dataString, _jsonOpts);
            Assert.NotNull(userReturned);
            Assert.NotNull(userReturned.UserGroups);
            Assert.NotNull(userReturned.Permissions);
            Assert.True(userReturned.UserGroups.Count > 0);
            Assert.True(userReturned.Permissions.Count > 0);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task GetUserByIdAsManagerRoleWithValidIdShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateManager();
            var user = _db.GetUserRegular();
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync($"/v1/users/{user.Id}");
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task GetUserByIdAsRegularRoleWithValidIdShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateRegular();
            var user = _db.GetUserRegular();
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync($"/v1/users/{user.Id}");
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

       [Fact]
        [Trait("TestType", "End2End")]
        public async Task GetUserByIdAsAdminRoleWithEmptyIdShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync($"/v1/users/{0}");
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task GetUserByIdAsAdminRoleWithNonexistingIdShouldReturn400BadRequest()
        {
            
            ////await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.GetAsync($"/v1/users/{Guid.NewGuid()}");
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        #endregion READ USER BY ID

        #region UPDATE USER

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PutUserAsAdminRoleWithUpdatedUserDtoShouldUpdateExistingUserReturn200Ok()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _db.GetUserRegular();
            user.Name = "Inigo Montoya";
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PutAsync($"/v1/users/{user.Id}", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("OK", response.ReasonPhrase.ToString());
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ApiReponse>(responseJson, _jsonOpts);
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse.Status);
            Assert.Equal("Success", controllerResponse.Status);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PutUserAsManagerRoleWithUpdatedUserDtoShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateManager();
            var user = _db.GetUserRegular();
            user.Name = "Inigo Montoya";
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PutAsync($"/v1/users/{user.Id}", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PutUserAsRegularRoleWithUpdatedUserDtoShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateRegular();
            var user = _db.GetUserRegular();
            user.Name = "Inigo Montoya";
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PutAsync($"/v1/users/{user.Id}", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PutUserAsAdminRoleWithUpdatedUserDtoWithUrlIdMistatchShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _db.GetUserRegular();
            user.Name = "Inigo Montoya";
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PutAsync($"/v1/users/{Guid.NewGuid()}", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }
        #endregion UPDATE USER

        #region UPDATE USER PASSWORD

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PutUserPasswordAsAdminRoleWithUpdatedUserPasswordShouldUpdateExistingUserPasswordReturn200Ok()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _db.GetUserRegular();
            user.Password = "1.2.3.4.";
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PutAsync($"/v1/users/{user.Id}/password", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ApiReponse>(responseJson, _jsonOpts);
            Assert.Equal("OK", response.ReasonPhrase.ToString());
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse.Status);
            Assert.Equal("Success", controllerResponse.Status);
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PutUserPasswordAsManagerRoleWithUpdatedUserPasswordShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateManager();
            var user = _db.GetUserRegular();
            user.Password = "1.2.3.4.";
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PutAsync($"/v1/users/{user.Id}/password", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PutUserPasswordAsRegularRoleWithUpdatedUserPasswordShouldReturn403Forbidden()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateRegular();
            var user = _db.GetUserRegular();
            user.Password = "1.2.3.4.";
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PutAsync($"/v1/users/{user.Id}/password", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Forbidden", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PutUserPasswordAsAdminRoleWithUrlIdMismatchShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _db.GetUserRegular();
            user.Password = "1.2.3.4.";
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PutAsync($"/v1/users/{Guid.NewGuid()}/password", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task PutUserPasswordAsAdminRoleWithoutNewPasswordShouldReturn400BadRequest()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var adminUser = _db.GetUserAdmin();
            var user = new UserDto()
            {
                Id = adminUser.Id
            };
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.PutAsync($"/v1/users/{user.Id}/password", content);
            }

            
            Assert.NotNull(response.ReasonPhrase);
            Assert.Equal("Bad Request", response.ReasonPhrase.ToString());
        }

        #endregion UPDATE USER PASSWORD

        #region DELETE USER

        [Fact]
        [Trait("TestType", "End2End")]
        public async Task DeleteUserAsAdminRoleWithExistingUserShouldDeleteUserReturn200Ok()
        {
            
            //await _db.RecycleDb();
            var token = _db.AuthenticateAdmin();
            var user = _db.GetUserRegular();
            HttpResponseMessage? response;

            
            using (var client = _webApplicationFactory.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.DeleteAsync($"/v1/users/{user.Id}");
            }

            
            Assert.NotNull(response.ReasonPhrase);
            var responseJson = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseJson);
            var controllerResponse = JsonSerializer.Deserialize<ApiReponse>(responseJson, _jsonOpts);
            Console.WriteLine("aqui");
            Assert.Equal("OK", response.ReasonPhrase.ToString());
            Assert.NotNull(controllerResponse);
            Assert.NotNull(controllerResponse.Status);
            Assert.Equal("Success", controllerResponse.Status);
        }

        #endregion DELETE USER

    }
}