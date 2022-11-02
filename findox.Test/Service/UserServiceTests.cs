using AutoMapper;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using findox.Domain.Validator;
using findox.Service.Services;
using findox.Test.TestObjects;
using FluentValidation;
using FluentValidation.Results;
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
        private Mock<IValidator<UserSessionDto>> _sessionValidator;
        private Mock<IValidator<UserDto>> _validator;

        public UserServiceTests()
        {
            _domain = new DomainTestObjects();
            _dto = new ObjectsDtoTest();
            _uow = new Mock<IUnitOfWork>();
            _token = new Mock<ITokenService>();
            _mapper = new Mock<IMapper>();
            _validator = new Mock<IValidator<UserDto>>();
            _sessionValidator = new Mock<IValidator<UserSessionDto>>();
        }

        private void RefreshMocks()
        {
            _uow = new Mock<IUnitOfWork>();
            _token = new Mock<ITokenService>();
            _mapper = new Mock<IMapper>();
            _validator = new Mock<IValidator<UserDto>>();
            _sessionValidator = new Mock<IValidator<UserSessionDto>>();
        }

        [Fact]
        public async Task CreateUserWithCompleteSuccessShouldReturnOutcomeSuccessWithUser()
        {
            RefreshMocks();
            var userDomain = _domain.TestRegularUser;
            var userDto = _dto.TestUserRegularDto;
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(userDomain);

            _uow.Setup(u => u.UsersRepository.CountByColumnValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(0);
            _uow.Setup(u => u.UsersRepository.Create(userDomain)).ReturnsAsync(userDomain);

            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)userDto);

            var validationResult = new ValidationResult();
            _validator.Setup(m => m.Validate(userDto)).Returns(validationResult);

            var userService = new UserService(_uow.Object, _token.Object, _mapper.Object,_validator.Object, _sessionValidator.Object);

            var serviceResponse = await userService.Create(userDto);

            Assert.NotNull(serviceResponse);
            Assert.IsType<ApiReponse>(serviceResponse);
            Assert.Equal(serviceResponse.hasErros, false);
            Assert.Equal("Success", serviceResponse.Status);
            Assert.NotNull(serviceResponse.Data);
            Assert.Equal(userDto, serviceResponse.Data);
        }

        [Fact]
        public async Task CreateUserWithFailMissingRequiredParamsShouldReturnOutcomeFailWithErrorMessage()
        {
            RefreshMocks();
            var userDomain = _domain.TestRegularUser;
            var userDto = _dto.TestUserIncompleteDto;
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(userDomain);

            _uow.Setup(u => u.UsersRepository.CountByColumnValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(0);
            _uow.Setup(u => u.UsersRepository.Create(userDomain)).ReturnsAsync(userDomain);

            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)userDto);

            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("User", "missing required fields"));
            _validator.Setup(m => m.Validate(userDto)).Returns(validationResult);

            var userService = new UserService(_uow.Object, _token.Object, _mapper.Object,_validator.Object, _sessionValidator.Object);

            var serviceResponse = await userService.Create(userDto);

            Assert.NotNull(serviceResponse);
            Assert.IsType<ApiReponse>(serviceResponse);
            Assert.Equal(serviceResponse.hasValidationErros, true);
            Assert.Equal(serviceResponse.Status, "Error");
            Assert.Null(serviceResponse.Data);
        }

        [Fact]
        public async Task CreateUserWithFailExistingShouldReturnOutcomeFailWithErrorMessage()
        {
            RefreshMocks();
            var userDomain = _domain.TestRegularUser;
            var userDto = _dto.TestUserRegularDto;
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(userDomain);

            _uow.Setup(u => u.UsersRepository.CountByColumnValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(1);
            _uow.Setup(u => u.UsersRepository.Create(userDomain)).ReturnsAsync(userDomain);

            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)userDto);

            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("User", "internal server error"));
            _validator.Setup(m => m.Validate(userDto)).Returns(validationResult);

            var userService = new UserService(_uow.Object, _token.Object, _mapper.Object,_validator.Object, _sessionValidator.Object);

            var serviceResponse = await userService.Create(userDto);

            Assert.NotNull(serviceResponse);
            Assert.IsType<ApiReponse>(serviceResponse);
            Assert.Equal(serviceResponse.hasValidationErros, true);
            Assert.Equal(serviceResponse.Status, "Error");
            Assert.Null(serviceResponse.Data);
        }

        [Fact]
        public async Task CreateUserWithExceptionShouldReturnOutcomeError()
        {
            RefreshMocks();
            var userDomain = _domain.TestRegularUser;
            var userDto = _dto.TestUserNewRegularDto;
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(userDomain);
            _uow.Setup(u => u.UsersRepository.CountByColumnValue(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(0);
            _uow.Setup(u => u.UsersRepository.Create(userDomain)).Throws(new Exception());
            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((UserDto)userDto);

            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("User", "internal server error"));
            _validator.Setup(m => m.Validate(userDto)).Returns(validationResult);
            var userService = new UserService(_uow.Object, _token.Object, _mapper.Object,_validator.Object, _sessionValidator.Object);

            var serviceResponse = await userService.Create(userDto);

            Assert.NotNull(serviceResponse);
            Assert.IsType<ApiReponse>(serviceResponse);
            Assert.Equal(serviceResponse.Status, "Error");
            Assert.Null(serviceResponse.Data);
        }
    }
}