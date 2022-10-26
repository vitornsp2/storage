using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using findox.Domain.Interfaces.DAL;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;

namespace findox.Service.Services
{
    public class UserGroupService : IUserGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IUserGroupServiceResponse> Create(IUserGroupServiceRequest request)
        {
            var response = new UserGroupServiceResponse();

            if (
                request.Item is null ||
                request.Item?.GroupId is null || request.Item.GroupId == 0 ||
                request.Item?.UserId is null || request.Item.UserId == 0
            )
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "GroupId and UserId are required.";
                return response;
            }

            try
            {
                var userGroup = _mapper.Map<UserGroup>(request.Item);
                
                _unitOfWork.Begin();

                var existingUserGroupCount = await _unitOfWork.userGroups.CountByGroupAndUser(userGroup.GroupId, userGroup.UserId);
                if (existingUserGroupCount > 0)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested userGroup already exists.";
                    return response;
                }

                var newUserGroup = await _unitOfWork.userGroups.Create(userGroup);

                _unitOfWork.Commit();

                response.Item = _mapper.Map<UserGroupDto>(newUserGroup);

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IUserGroupServiceResponse> DeleteById(IUserGroupServiceRequest request)
        {
            var response = new UserGroupServiceResponse();

            if (request.Id is null || !request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existing = await _unitOfWork.userGroups.ReadById((long)request.Id);
                if (existing.Id != request.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested userGroup for delete does not exist.";
                    return response;
                }

                var successful = await _unitOfWork.userGroups.DeleteById((long)request.Id);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "UserGroup was not deleted.";
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

        public async Task<IUserGroupServiceResponse> DeleteByGroupId(IUserGroupServiceRequest request)
        {
            var response = new UserGroupServiceResponse();

            if (request.Id is null || !request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existingCount = await _unitOfWork.userGroups.CountByColumnValue("group_id", (long)request.Id);
                if (existingCount < 1)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested userGroup(s) for delete not found.";
                    return response;
                }

                var successful = await _unitOfWork.userGroups.DeleteByGroupId((long)request.Id);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "UserGroup(s) not deleted.";
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

        public async Task<IUserGroupServiceResponse> DeleteByUserId(IUserGroupServiceRequest request)
        {
            var response = new UserGroupServiceResponse();

            if (request.Id is null || request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existingCount = await _unitOfWork.userGroups.CountByColumnValue("user_id", (long)request.Id);
                if (existingCount < 1)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested userGroup(s) for delete not found.";
                    return response;
                }

                var successful = await _unitOfWork.userGroups.DeleteByUserId((long)request.Id);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "UserGroup(s) not deleted.";
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
    }
}