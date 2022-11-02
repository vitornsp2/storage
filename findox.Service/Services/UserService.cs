using AutoMapper;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using FluentValidation;

namespace findox.Service.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IValidator<UserDto> _validator;
        private readonly IValidator<UserSessionDto> _sessionValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _jwt;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,
                           ITokenService tokenService,
                           IMapper mapper,
                           IValidator<UserDto> validator,
                           IValidator<UserSessionDto> sessionValidator
                           )
        {
            _unitOfWork = unitOfWork;
            _jwt = tokenService;
            _mapper = mapper;
            _validator = validator;
            _sessionValidator = sessionValidator;
        }

        public async Task<ApiReponse> Create(UserDto userDto)
        {
            var response = new ApiReponse();
            
            var validationResult = _validator.Validate(userDto);

            if (validationResult.IsValid)
            {
                try
                {
                    var user = _mapper.Map<User>(userDto);

                    var existingNameCount = await _unitOfWork.UsersRepository.CountByColumnValue("name", user.Name);
                    if (existingNameCount > 0)
                    {
                        addMessage(response.ValidationErros, "Name", "Requested user name is already in use.");
                        return response;
                    }

                    var existingEmailCount = await _unitOfWork.UsersRepository.CountByColumnValue("email", user.Email);
                    if (existingEmailCount > 0)
                    {
                        addMessage(response.ValidationErros, "Email", "Requested user email is already in use.");
                        return response;
                    }

                    var newUser = await _unitOfWork.UsersRepository.Create(user);

                    response.Data = _mapper.Map<UserDto>(newUser);
                }
                catch (Exception err)
                {
                    response.Erros = ToDictionary(err);
                }
            }
            else
            {
                response.ValidationErros = validationResult.ToDictionary();
            }

            return response;
        }

        public async Task<ApiReponse> ReadAll()
        {
            var response = new ApiReponse();

            try
            {
                var users = await _unitOfWork.UsersRepository.ReadAll();
                response.Data = _mapper.Map<List<UserDto>>(users);
            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }
        public async Task<ApiReponse> ReadById(long? id)
        {
            var response = new ApiReponse();

            if (!id.HasValue || id == 0)
            {
                addMessage(response.ValidationErros, "Id", "Id is required to find a user by user id.");
                return response;
            }

            try
            {
                var user = await _unitOfWork.UsersRepository.ReadById(id.Value);

                if (user?.Id != id)
                {
                    addMessage(response.ValidationErros, "Id", "User was not found with the specified id.");
                    return response;
                }

                var userGroups = await _unitOfWork.UserGroupsRepository.ReadByUserId(user.Id);

                if (userGroups.Any())
                {
                    user.UserGroups = userGroups.ToList();
                }

                var permissions = await _unitOfWork.PermissionsRepository.ReadByUserId(user.Id);
                if (permissions.Any())
                {
                    user.Permissions = permissions.ToList();
                }

                response.Data = _mapper.Map<UserAllDto>(user);

            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> Update(UserDto userDto)
        {
            var response = new ApiReponse();

            if (!userDto.Id.HasValue || userDto.Id == 0)
            {
                addMessage(response.ValidationErros, "Id", "Id is required to update a user.");
                return response;
            }

            try
            {
                var user = _mapper.Map<User>(userDto);

                var existing = await _unitOfWork.UsersRepository.ReadById(user.Id);
                if (existing?.Id != user.Id)
                {
                    addMessage(response.ValidationErros, "Id", "Requested user id for update does not exist.");
                    return response;
                }

                var successful = await _unitOfWork.UsersRepository.UpdateById(user);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "User", "User was not updated.");
                    return response;
                }
            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> UpdatePassword(UserDto userDto)
        {
            var response = new ApiReponse();

            if (!userDto.Id.HasValue || userDto?.Id == 0 || userDto?.Password is null)
            {
                addMessage(response.ValidationErros, "User", "Id and Password are required to update a user password.");
                return response;
            }

            try
            {
                var user = _mapper.Map<User>(userDto);

                var existing = await _unitOfWork.UsersRepository.ReadById(user.Id);
                if (existing?.Id != user.Id)
                {
                    addMessage(response.ValidationErros, "User", "Requested user or user id for update does not exist.");
                    return response;
                }

                var successful = await _unitOfWork.UsersRepository.UpdatePasswordById(user);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "User", "User password was not updated.");
                    return response;
                }
            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> DeleteById(long? id)
        {
            var response = new ApiReponse();

            if (!id.HasValue)
            {
                addMessage(response.ValidationErros, "User", "Id is required to delete a user by user id.");
                return response;
            }

            try
            {
                var existing = await _unitOfWork.UsersRepository.ReadById(id.Value);
                if (existing?.Id != id)
                {
                    addMessage(response.ValidationErros, "User", "Requested user id for delete does not exist.");
                    return response;
                }

                var userGroupsCount = await _unitOfWork.UserGroupsRepository.CountByColumnValue("user_id", id.Value);
                if (userGroupsCount > 0)
                {
                    var successfulDeleteUserGroups = await _unitOfWork.UserGroupsRepository.DeleteByUserId(id.Value);

                    if (!successfulDeleteUserGroups.Value)
                    {
                        addMessage(response.ValidationErros, "User", "User was not deleted because existing userGroups could not be deleted.");
                        return response;
                    }
                }

                var permitsCount = await _unitOfWork.PermissionsRepository.CountByColumnValue("user_id", id.Value);
                if (permitsCount > 0)
                {
                    var successfulDeletePermits = await _unitOfWork.PermissionsRepository.DeleteByUserId((long)id);

                    if (!successfulDeletePermits.Value)
                    {
                        addMessage(response.ValidationErros, "User", "User was not deleted because existing permits could not be deleted.");
                        return response;
                    }
                }

                var successfulDeleteUser = await _unitOfWork.UsersRepository.DeleteById(id.Value);

                if (!successfulDeleteUser.Value)
                {
                    addMessage(response.ValidationErros, "User", "User was not deleted");
                    return response;
                }
            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> CreateSession(UserSessionDto userSessionDto)
        {
            var response = new ApiReponse();

            var validationResult = _sessionValidator.Validate(userSessionDto);

            if (validationResult.IsValid)
            {
                try
                {
                    var user = _mapper.Map<User>(userSessionDto);

                    var authenticated = await _unitOfWork.UsersRepository.Authenticate(user);

                    if (authenticated is null)
                    {
                        addMessage(response.ValidationErros, "Login", "No match found for Email and Password.");
                        return response;
                    }

                    response.Data = _jwt.Encode(authenticated.Id, authenticated.Role);
                }
                catch
                {
                    response.ValidationErros = validationResult.ToDictionary();
                }
            }
            else
            {
                response.ValidationErros = validationResult.ToDictionary();
            }

            return response;
        }
    }
}