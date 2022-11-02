using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Api.Controllers;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using findox.Test.TestObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace findox.Test.controller
{
    public class SessionControllerTests
    {
        private ObjectsDtoTest _dto;
        private Mock<IUserService> _userService;

        public SessionControllerTests()
        {
            _dto = new ObjectsDtoTest();
            _userService = new Mock<IUserService>();
        }

        private void RefreshMocks()
        {
            _userService = new Mock<IUserService>();
        }

        #region CREATE

        [Fact]
        public async Task CreateSessionWithServiceSuccessShouldReturnOkWithToken()
        {
            RefreshMocks();
            var userSessionDto = _dto.TestUserSessionBasicDto;
            var token = "faketokenstring";
            var response = new ApiReponse() { Data = token };
            _userService.Setup(u => u.CreateSession(It.IsAny<UserSessionDto>())).ReturnsAsync(response);
            var sessionController = new SessionController(_userService.Object);

            var actionResult = await sessionController.Create((UserSessionDto)userSessionDto);

            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);

            var okObjectResult = (OkObjectResult)actionResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.NotNull(okObjectResult.Value);
            Assert.IsType<ApiReponse>(okObjectResult.Value);

            var controllerResponse = (ApiReponse)okObjectResult?.Value;
            Assert.NotNull(controllerResponse);
            Assert.Equal("Success", controllerResponse?.Status);
            Assert.Equal(token, controllerResponse?.Data);
        }

        [Fact]
        public async Task CreateSessionWithServiceFailShouldReturnBadRequestWithErrorMessage()
        {
            RefreshMocks();
            var userSessionDto = _dto.TestUserSessionBasicDto;
            var validationErros = new Dictionary<string, string[]>();
            validationErros.Add("User", new List<string> { "No match found for Email and Password."}.ToArray());
            var response = new ApiReponse() { ValidationErros = validationErros };
            _userService.Setup(u => u.CreateSession(It.IsAny<UserSessionDto>())).ReturnsAsync(response);
            var sessionController = new SessionController(_userService.Object);

            var actionResult = await sessionController.Create((UserSessionDto)userSessionDto);

            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);

            var badRequestObjectResult = (BadRequestObjectResult)actionResult;
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.NotNull(badRequestObjectResult.Value);
            Assert.IsType<ApiReponse>(badRequestObjectResult.Value);

            var controllerResponse = (ApiReponse)badRequestObjectResult.Value;
            Assert.NotNull(controllerResponse);
            Assert.Equal("Error", controllerResponse.Status);
        }

        #endregion CREATE
    }
}