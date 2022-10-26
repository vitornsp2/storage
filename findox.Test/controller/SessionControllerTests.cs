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
            var response = new UserServiceResponse() { Outcome = OutcomeType.Success, Token = token };
            _userService.Setup(u => u.CreateSession(It.IsAny<UserServiceRequest>())).ReturnsAsync(response);
            var sessionController = new SessionController(_userService.Object);

            var actionResult = await sessionController.Create((UserSessionDto)userSessionDto);

            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);

            var okObjectResult = (OkObjectResult)actionResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.NotNull(okObjectResult.Value);
            Assert.IsType<ControllerResponse>(okObjectResult.Value);

            var controllerResponse = (ControllerResponse)okObjectResult.Value;
            Assert.NotNull(controllerResponse);
            Assert.Equal("success", controllerResponse.Status);
            Assert.Equal(token, controllerResponse.Data);
        }

        [Fact]
        public async Task CreateSessionWithServiceFailShouldReturnBadRequestWithErrorMessage()
        {
            RefreshMocks();
            var userSessionDto = _dto.TestUserSessionBasicDto;
            var response = new UserServiceResponse() { Outcome = OutcomeType.Fail, ErrorMessage = "No match found for Email and Password." };
            _userService.Setup(u => u.CreateSession(It.IsAny<UserServiceRequest>())).ReturnsAsync(response);
            var sessionController = new SessionController(_userService.Object);

            var actionResult = await sessionController.Create((UserSessionDto)userSessionDto);

            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);

            var badRequestObjectResult = (BadRequestObjectResult)actionResult;
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.NotNull(badRequestObjectResult.Value);
            Assert.IsType<ControllerResponse>(badRequestObjectResult.Value);

            var controllerResponse = (ControllerResponse)badRequestObjectResult.Value;
            Assert.NotNull(controllerResponse);
            Assert.Equal("fail", controllerResponse.Status);
            Assert.Equal("No match found for Email and Password.", controllerResponse.Message);
        }

        [Fact]
        public async Task CreateSessionWithServiceErrorShouldReturnObjectResult()
        {
            RefreshMocks();
            var userSessionDto = _dto.TestUserSessionBasicDto;
            var response = new UserServiceResponse() { Outcome = OutcomeType.Error };
            _userService.Setup(u => u.CreateSession(It.IsAny<UserServiceRequest>())).ReturnsAsync(response);
            var sessionController = new SessionController(_userService.Object);

            var actionResult = await sessionController.Create((UserSessionDto)userSessionDto);

            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = (ObjectResult)actionResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<ControllerResponse>(objectResult.Value);

            var controllerResponse = (ControllerResponse)objectResult.Value;
            Assert.NotNull(controllerResponse);
            Assert.Equal("error", controllerResponse.Status);
            Assert.Equal("An error occurred while processing your request.", controllerResponse.Message);
        }

        #endregion CREATE
    }
}