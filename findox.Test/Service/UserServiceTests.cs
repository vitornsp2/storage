using AutoMapper;
using findox.Domain.Interfaces.DAL;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using findox.Service.Services;
using findox.Test.TestObjects;
using Moq;

namespace findox.Test.Service
{
    public class UserServiceTests
    {
        private DomainTestObjects _domain;
        private ObjectsDtoTest _dto;
        private Mock<IUnitOfWork> _uow;
        private Mock<ITokenService> _token;
        private Mock<IMapper> _mapper;

        public UserServiceTests()
        {
            _domain = new DomainTestObjects();
            _dto = new ObjectsDtoTest();
            _uow = new Mock<IUnitOfWork>();
            _token = new Mock<ITokenService>();
            _mapper = new Mock<IMapper>();
        }

        private void RefreshMocks()
        {
            _uow = new Mock<IUnitOfWork>();
            _token = new Mock<ITokenService>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task CreateUserWithCompleteSuccessShouldReturnOutcomeSuccessWithUser()
        {
            RefreshMocks();
            var userDomain = _domain.TestRegularUser;
            var userDto = _dto.TestUserRegularDto;
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(userDomain);
            _uow.Setup(u => u.Begin()).Verifiable();
            _uow.Setup(u => u.users.CountByColumnValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(0);
            _uow.Setup(u => u.users.Create(userDomain)).ReturnsAsync(userDomain);
            _uow.Setup(u => u.Commit()).Verifiable();
            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)userDto);
            var userService = new UserService(_uow.Object, _token.Object, _mapper.Object);

            var serviceResponse = await userService.Create(new UserServiceRequest(userDto));

            Assert.NotNull(serviceResponse);
            Assert.IsType<UserServiceResponse>(serviceResponse);
            Assert.Null(serviceResponse.ErrorMessage);
            Assert.Equal(OutcomeType.Success, serviceResponse.Outcome);
            Assert.NotNull(serviceResponse.Item);
            Assert.Equal(userDto, serviceResponse.Item);
        }

        [Fact]
        public async Task CreateUserWithFailMissingRequiredParamsShouldReturnOutcomeFailWithErrorMessage()
        {
            RefreshMocks();
            var userDomain = _domain.TestRegularUser;
            var userDto = _dto.TestUserIncompleteDto;
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(userDomain);
            _uow.Setup(u => u.Begin()).Verifiable();
            _uow.Setup(u => u.users.CountByColumnValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(0);
            _uow.Setup(u => u.users.Create(userDomain)).ReturnsAsync(userDomain);
            _uow.Setup(u => u.Commit()).Verifiable();
            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)userDto);
            var userService = new UserService(_uow.Object, _token.Object, _mapper.Object);

            var serviceResponse = await userService.Create(new UserServiceRequest(userDto));

            Assert.NotNull(serviceResponse);
            Assert.IsType<UserServiceResponse>(serviceResponse);
            Assert.NotNull(serviceResponse.ErrorMessage);
            Assert.Equal(OutcomeType.Fail, serviceResponse.Outcome);
            Assert.Null(serviceResponse.Item);
        }

        [Fact]
        public async Task CreateUserWithFailExistingShouldReturnOutcomeFailWithErrorMessage()
        {
            RefreshMocks();
            var userDomain = _domain.TestRegularUser;
            var userDto = _dto.TestUserRegularDto;
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(userDomain);
            _uow.Setup(u => u.Begin()).Verifiable();
            _uow.Setup(u => u.users.CountByColumnValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(1);
            _uow.Setup(u => u.users.Create(userDomain)).ReturnsAsync(userDomain);
            _uow.Setup(u => u.Commit()).Verifiable();
            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)userDto);
            var userService = new UserService(_uow.Object, _token.Object, _mapper.Object);

            var serviceResponse = await userService.Create(new UserServiceRequest(userDto));

            Assert.NotNull(serviceResponse);
            Assert.IsType<UserServiceResponse>(serviceResponse);
            Assert.NotNull(serviceResponse.ErrorMessage);
            Assert.Equal(OutcomeType.Fail, serviceResponse.Outcome);
            Assert.Null(serviceResponse.Item);
        }

        [Fact]
        public async Task CreateUserWithExceptionShouldReturnOutcomeError()
        {
            RefreshMocks();
            var userDomain = _domain.TestRegularUser;
            var userDto = _dto.TestUserNewRegularDto;
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(userDomain);
            _uow.Setup(u => u.Begin()).Verifiable();
            _uow.Setup(u => u.users.CountByColumnValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(0);
            _uow.Setup(u => u.users.Create(userDomain)).Throws(new Exception());
            _uow.Setup(u => u.Commit()).Verifiable();
            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)userDto);
            var userService = new UserService(_uow.Object, _token.Object, _mapper.Object);

            var serviceResponse = await userService.Create(new UserServiceRequest(userDto));

            Assert.NotNull(serviceResponse);
            Assert.IsType<UserServiceResponse>(serviceResponse);
            Assert.Null(serviceResponse.ErrorMessage);
            Assert.Equal(OutcomeType.Error, serviceResponse.Outcome);
            Assert.Null(serviceResponse.Item);
        }
    }
}