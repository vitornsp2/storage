using AutoMapper;
using findox.Domain.Interfaces.DAL;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;

namespace findox.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ITokenService _jwt;

        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwt = tokenService;
            _mapper = mapper;
        }

        public async Task<IUserServiceResponse> Create(IUserServiceRequest request)
        {
            var response = new UserServiceResponse();

            if (
                request.Item?.Name is null || string.IsNullOrWhiteSpace(request.Item.Name) ||
                request.Item?.Email is null || string.IsNullOrWhiteSpace(request.Item.Email) ||
                request.Item?.Password is null || string.IsNullOrWhiteSpace(request.Item.Password))
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Name, Email, Password, and Role (regular, manager, or admin) are required.";
                return response;
            }

            try
            {
                var user = _mapper.Map<User>(request.Item);

                _unitOfWork.Begin();

                var existingNameCount = await _unitOfWork.users.CountByColumnValue("name", request.Item.Name);
                if (existingNameCount > 0)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested user name is already in use.";
                    return response;
                }

                var existingEmailCount = await _unitOfWork.users.CountByColumnValue("email", request.Item.Email);
                if (existingEmailCount > 0)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested user email is already in use.";
                    return response;
                }

                var newUser = await _unitOfWork.users.Create(user);

                _unitOfWork.Commit();
                
                response.Item = _mapper.Map<UserDto>(newUser);

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IUserServiceResponse> ReadAll(IUserServiceRequest request)
        {
            var response = new UserServiceResponse();

            try
            {
                var users = await _unitOfWork.users.ReadAll();

                var userDtos = new List<IUserDto>();
                foreach (var user in users) userDtos.Add(_mapper.Map<UserDto>(user));
                response.List = userDtos;

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }
        public async Task<IUserServiceResponse> ReadById(IUserServiceRequest request)
        {
            var response = new UserServiceResponse();


            if (!request.Id.HasValue || request.Id == 0)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required to find a user by user id.";
                return response;
            }

            try
            {
                var user = await _unitOfWork.users.ReadById((long)request.Id);

                if (user.Id != request.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "User was not found with the specified id.";
                    return response;
                }

                var userGroups = await _unitOfWork.userGroups.ReadByUserId(user.Id);

                if (userGroups.Any())
                {
                    user.UserGroups = userGroups;
                }

                var permissions = await _unitOfWork.permissions.ReadByUserId(user.Id);
                if (permissions.Any())
                {
                    user.Permissions = permissions;
                }

                response.AllDto = _mapper.Map<UserAllDto>(user);

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }


        public async Task<IUserServiceResponse> Update(IUserServiceRequest request)
        {
            var response = new UserServiceResponse();

            if (request.Item?.Id is null || request.Item.Id == 0)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required to update a user.";
                return response;
            }

            try
            {
                var user = _mapper.Map<User>(request.Item);

                _unitOfWork.Begin();

                var existing = await _unitOfWork.users.ReadById(user.Id);
                if (existing.Id != user.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested user id for update does not exist.";
                    return response;
                }

                var successful = await _unitOfWork.users.UpdateById(user);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "User was not updated.";
                    return response;
                }

                _unitOfWork.Commit();

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IUserServiceResponse> UpdatePassword(IUserServiceRequest request)
        {
            var response = new UserServiceResponse();

            if (request.Item?.Id is null || request.Item?.Id == 0 || request.Item?.Password is null)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id and Password are required to update a user password.";
                return response;
            }

            try
            {
                var user = _mapper.Map<User>(request.Item);

                _unitOfWork.Begin();

                var existing = await _unitOfWork.users.ReadById(user.Id);
                if (existing.Id != user.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested user or user id for update does not exist.";
                    return response;
                }

                var successful = await _unitOfWork.users.UpdatePasswordById(user);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "User password was not updated.";
                    return response;
                }
                
                _unitOfWork.Commit();

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IUserServiceResponse> DeleteById(IUserServiceRequest request)
        {
            var response = new UserServiceResponse();

            if (!request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required to delete a user by user id.";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existing = await _unitOfWork.users.ReadById((long)request.Id);
                if (existing.Id != request.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested user id for delete does not exist.";
                    return response;
                }

                var userGroupsCount = await _unitOfWork.userGroups.CountByColumnValue("user_id", (long)request.Id);
                if (userGroupsCount > 0)
                {
                    var successfulDeleteUserGroups = await _unitOfWork.userGroups.DeleteByUserId((long)request.Id);

                    if (!successfulDeleteUserGroups)
                    {
                        _unitOfWork.Rollback();
                        response.Outcome = OutcomeType.Fail;
                        response.ErrorMessage = "User was not deleted because existing userGroups could not be deleted.";
                        return response;
                    }
                }

                var permitsCount = await _unitOfWork.permissions.CountByColumnValue("user_id", (long)request.Id);
                if (permitsCount > 0)
                {
                    var successfulDeletePermits = await _unitOfWork.permissions.DeleteByUserId((long)request.Id);

                    if (!successfulDeletePermits)
                    {
                        _unitOfWork.Rollback();
                        response.Outcome = OutcomeType.Fail;
                        response.ErrorMessage = "User was not deleted because existing permits could not be deleted.";
                        return response;
                    }
                }

                var successfulDeleteUser = await _unitOfWork.users.DeleteById((long)request.Id);

                if (!successfulDeleteUser)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "User was not deleted.";
                    return response;
                }

                _unitOfWork.Commit();

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IUserServiceResponse> CreateSession(IUserServiceRequest request)
        {
            var response = new UserServiceResponse();

            if (request.UserSessionDto?.Email is null || request.UserSessionDto.Password is null)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Email (TEXT) and Password (TEXT) are required to authenticate a user.";
                return response;
            }

            try
            {
                var user = _mapper.Map<User>(request.UserSessionDto);

                var authenticated = await _unitOfWork.users.Authenticate(user);

                if (authenticated.Id == 0)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "No match found for Email and Password.";
                    return response;
                }
                response.Item = _mapper.Map<UserDto>(authenticated);

                response.Token = _jwt.Encode(authenticated.Id, authenticated.Role);

                response.Outcome = OutcomeType.Success;
            }
            catch
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }
    }
}