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
    public class GroupService : BaseService, IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<GroupDto> _validator;

        public GroupService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<GroupDto> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ApiReponse> Create(GroupDto groupDto)
        {
            var response = new ApiReponse();

            var validationResult = _validator.Validate(groupDto);

            if (validationResult.IsValid)
            {
                try
                {
                    var group = _mapper.Map<Group>(groupDto);

                    var existingNameCount = await _unitOfWork.GroupsRepository.CountByColumnValue("name", group.Name);
                    if (existingNameCount > 0)
                    {
                        addMessage(response.ValidationErros, "Group", "Group name is already in use.");
                        return response;
                    }

                    var newGroup = await _unitOfWork.GroupsRepository.Create(group);

                    response.Data = _mapper.Map<GroupDto>(newGroup);

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
                var groups = await _unitOfWork.GroupsRepository.ReadAll();

                var groupDtos = _mapper.Map<List<GroupDto>>(groups);
                response.Data = groupDtos;
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

            if (!id.HasValue)
            {
                addMessage(response.ValidationErros, "Group", "Id is required.");
                return response;
            }

            try
            {
                var group = await _unitOfWork.GroupsRepository.ReadById((long)id.Value);

                if (group?.Id != id)
                {
                    addMessage(response.ValidationErros, "Group", "Group was not found.");
                    return response;
                }

                var userGroups = await _unitOfWork.UserGroupsRepository.ReadByGroupId(group.Id);
                if (userGroups.Count() > 0)
                {
                    group.UserGroups = userGroups.ToList();
                }

                var permissions = await _unitOfWork.PermissionsRepository.ReadByGroupId(group.Id);

                if (permissions.Any())
                {
                    group.Permissions = permissions.ToList();
                }

                response.Data = _mapper.Map<GroupAllDto>(group);

            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> Update(GroupDto groupDto)
        {
            var response = new ApiReponse();

            if (groupDto?.Id is null || !groupDto.Id.HasValue)
            {
                addMessage(response.ValidationErros, "Group", "Id is required.");
                return response;
            }

            try
            {
                var group = _mapper.Map<Group>(groupDto);

                var existing = await _unitOfWork.GroupsRepository.ReadById(group.Id);
                if (existing?.Id != groupDto.Id)
                {
                    addMessage(response.ValidationErros, "Group", "Requested group id does not exist.");
                    return response;
                }

                var successful = await _unitOfWork.GroupsRepository.UpdateById(group);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "Group", "Group was not updated.");
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
                addMessage(response.ValidationErros, "Group", "Id is required.");
                return response;
            }

            try
            {
                var existing = await _unitOfWork.GroupsRepository.ReadById(id.Value);
                if (existing?.Id != id)
                {
                    addMessage(response.ValidationErros, "Group", "IRequested group id for delete does not exist.");
                    return response;
                }

                var successful = await _unitOfWork.GroupsRepository.DeleteById(id.Value);

                if (!successful.Value)
                {

                    addMessage(response.ValidationErros, "Group", "Group was not deleted.");
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