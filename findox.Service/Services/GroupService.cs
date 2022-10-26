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
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IGroupServiceResponse> Create(IGroupServiceRequest request)
        {
            var response = new GroupServiceResponse();

            if (request.Item?.Name is null)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Name is required.";
                return response;
            }

            try
            {
                var group = _mapper.Map<Group>(request.Item);

                _unitOfWork.Begin();

                var existingNameCount = await _unitOfWork.groups.CountByColumnValue("name", group.Name);
                if (existingNameCount > 0)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Group name is already in use.";
                    return response;
                }

                var newGroup = await _unitOfWork.groups.Create(group);

                _unitOfWork.Commit();

                response.Item = _mapper.Map<GroupDto>(newGroup);

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IGroupServiceResponse> ReadAll(IGroupServiceRequest request)
        {
            var response = new GroupServiceResponse();

            try
            {
                var groups = await _unitOfWork.groups.ReadAll();

                var groupDtos = new List<IGroupDto>();
                foreach (var group in groups) groupDtos.Add(_mapper.Map<GroupDto>(group));
                response.List = groupDtos;

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IGroupServiceResponse> ReadById(IGroupServiceRequest request)
        {
            var response = new GroupServiceResponse();

            if (!request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                var group = await _unitOfWork.groups.ReadById((long)request.Id);

                if (group.Id != request.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Group was not found.";
                    return response;
                }

                var userGroups = await _unitOfWork.userGroups.ReadByGroupId(group.Id);
                if (userGroups.Count() > 0)
                {
                    group.UserGroups = userGroups;
                }

                var permissions = await _unitOfWork.permissions.ReadByGroupId(group.Id);

                if (permissions.Any())
                {
                    group.Permissions = permissions;
                }

                response.AllDto = _mapper.Map<GroupAllDto>(group);

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IGroupServiceResponse> Update(IGroupServiceRequest request)
        {
            var response = new GroupServiceResponse();

            if (request.Item?.Id is null || !request.Item.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                var group = _mapper.Map<Group>(request.Item);

                _unitOfWork.Begin();

                var existing = await _unitOfWork.groups.ReadById(group.Id);
                if (existing.Id != request.Item.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested group id does not exist.";
                    return response;
                }

                var successful = await _unitOfWork.groups.UpdateById(group);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Group was not updated.";
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

        public async Task<IGroupServiceResponse> DeleteById(IGroupServiceRequest request)
        {
            var response = new GroupServiceResponse();

            if (!request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existing = await _unitOfWork.groups.ReadById((long)request.Id);
                if (existing.Id != request.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested group id for delete does not exist.";
                    return response;
                }

                var successful = await _unitOfWork.groups.DeleteById((long)request.Id);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Group was not deleted.";
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