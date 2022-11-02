using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using FluentValidation;

namespace findox.Service.Services
{
    public class UserGroupService : BaseService, IUserGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<UserGroupDto> _validator;

        public UserGroupService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<UserGroupDto> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ApiReponse> Create(UserGroupDto userGroupDto)
        {
            var response = new ApiReponse();

            var validationResult = _validator.Validate(userGroupDto);

            if (validationResult.IsValid)
            {
                try
                {
                    var userGroup = _mapper.Map<UserGroup>(userGroupDto);

                    var existingUserGroupCount = await _unitOfWork.UserGroupsRepository.CountByGroupAndUser(userGroup.GroupId, userGroup.UserId);
                    if (existingUserGroupCount > 0)
                    {
                        addMessage(response.ValidationErros, "UserGroup", "Requested userGroup already exists");
                        return response;
                    }

                    var newUserGroup = await _unitOfWork.UserGroupsRepository.Create(userGroup);

                    response.Data = _mapper.Map<UserGroupDto>(newUserGroup);
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

        public async Task<ApiReponse> DeleteById(long? id)
        {
            var response = new ApiReponse();

            if (!id.HasValue)
            {
                addMessage(response.ValidationErros, "UserGroup", "Id is required.");
                return response;
            }

            try
            {
                var existing = await _unitOfWork.UserGroupsRepository.ReadById(id.Value);
                if (existing?.Id != id)
                {
                    addMessage(response.ValidationErros, "UserGroup", "Requested userGroup for delete does not exist.");
                    return response;
                }

                var successful = await _unitOfWork.UserGroupsRepository.DeleteById(id.Value);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "UserGroup", "serGroup was not deleted.");
                    return response;
                }
            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> DeleteByGroupId(long? id)
        {
            var response = new ApiReponse();

            if (!id.HasValue)
            {
                addMessage(response.ValidationErros, "UserGroup", "Id is required.");
                return response;
            }

            try
            {
                var existingCount = await _unitOfWork.UserGroupsRepository.CountByColumnValue("group_id", id.Value);
                if (existingCount < 1)
                {
                    addMessage(response.ValidationErros, "UserGroup", "Requested userGroup(s) for delete not found.");
                    return response;
                }

                var successful = await _unitOfWork.UserGroupsRepository.DeleteByGroupId((long)id.Value);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "UserGroup", "UserGroup(s) not deleted.");
                    return response;
                }
            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> DeleteByUserId(long? id)
        {
            var response = new ApiReponse();

            if (!id.HasValue)
            {
                addMessage(response.ValidationErros, "UserGroup", "Id is required.");
                return response;
            }

            try
            {
                var existingCount = await _unitOfWork.UserGroupsRepository.CountByColumnValue("user_id", id.Value);
                if (existingCount < 1)
                {
                    addMessage(response.ValidationErros, "UserGroup", "Requested userGroup(s) for delete not found.");
                    return response;
                }

                var successful = await _unitOfWork.UserGroupsRepository.DeleteByUserId(id.Value);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "UserGroup", "UserGroup(s) not deleted.");
                    return response;
                }
            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }
    }
}