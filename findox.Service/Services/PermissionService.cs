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
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PermissionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IPermissionServiceResponse> Create(IPermissionServiceRequest request)
        {
            var response = new PermissionServiceResponse();

            if (request.Item?.DocumentId is null || request.Item.DocumentId == 0)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "DocumentId is required.";
                return response;
            }
            if (
                (request.Item?.UserId is null || !request.Item.UserId.HasValue) &&
                (request.Item?.GroupId is null || !request.Item.GroupId.HasValue)
            )
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "UserId or GroupId is required.";
                return response;
            }

            try
            {
                var permission = _mapper.Map<Permission>(request.Item);

                _unitOfWork.Begin();

                var existingDocument = await _unitOfWork.documents.ReadById(permission.DocumentId);
                if (existingDocument.Id != permission.DocumentId)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested new permission's DocumentId does not exist.";
                    return response;
                }

                if (permission.UserId.HasValue)
                {
                    var existingUser = await _unitOfWork.users.ReadById((long)permission.UserId);
                    if (existingUser.Id != permission.UserId)
                    {
                        response.Outcome = OutcomeType.Fail;
                        response.ErrorMessage = "Requested new permission's UserId does not exist.";
                        return response;
                    }
                }


                if (permission.GroupId.HasValue)
                {
                    var existingGroup = await _unitOfWork.groups.ReadById((long)permission.GroupId);
                    if (existingGroup.Id != permission.GroupId)
                    {
                        response.Outcome = OutcomeType.Fail;
                        response.ErrorMessage = "Requested new permission's GroupId does not exist.";
                        return response;
                    }
                }

                var existingCount = await _unitOfWork.permissions.PermissionMatchCount(permission);
                if (existingCount > 0)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested new permission already exists.";
                    return response;
                }

                var newPermission = await _unitOfWork.permissions.Create(permission);

                _unitOfWork.Commit();

                response.Item = _mapper.Map<PermissionDto>(newPermission);

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IPermissionServiceResponse> ReadByDocumentId(IPermissionServiceRequest request)
        {
            var response = new PermissionServiceResponse();

            if (!request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                var permissions = await _unitOfWork.permissions.ReadByDocumentId((long)request.Id);

                var permissionDtos = new List<IPermissionDto>();
                foreach (var permission in permissions) permissionDtos.Add(_mapper.Map<PermissionDto>(permission));
                response.List = permissionDtos;

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IPermissionServiceResponse> DeleteById(IPermissionServiceRequest request)
        {
            var response = new PermissionServiceResponse();

            if (!request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existing = await _unitOfWork.permissions.ReadById((long)request.Id);
                if (existing.Id != request.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested permission id for delete does not exist.";
                    return response;
                }

                var successful = await _unitOfWork.permissions.DeleteById((long)request.Id);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Permission was not deleted.";
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

        public async Task<IPermissionServiceResponse> DeleteByDocumentId(IPermissionServiceRequest request)
        {
            var response = new PermissionServiceResponse();

            if (!request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existingCount = await _unitOfWork.permissions.CountByColumnValue("document_id", (long)request.Id);
                if (existingCount < 1)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested permissions(s) for delete not found.";
                    return response;
                }

                var successful = await _unitOfWork.permissions.DeleteByDocumentId((long)request.Id);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Permission(s) not deleted.";
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

        public async Task<IPermissionServiceResponse> DeleteByUserId(IPermissionServiceRequest request)
        {
            var response = new PermissionServiceResponse();

            if (!request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existingCount = await _unitOfWork.permissions.CountByColumnValue("user_id", (long)request.Id);
                if (existingCount < 1)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested permissions(s) for delete not found.";
                    return response;
                }

                var successful = await _unitOfWork.permissions.DeleteByUserId((long)request.Id);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Permission(s) not deleted.";
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

        public async Task<IPermissionServiceResponse> DeleteByGroupId(IPermissionServiceRequest request)
        {
            var response = new PermissionServiceResponse();

            if (!request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existingCount = await _unitOfWork.permissions.CountByColumnValue("group_id", (long)request.Id);
                if (existingCount < 1)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested permissions(s) for delete not found.";
                    return response;
                }

                var successful = await _unitOfWork.permissions.DeleteByGroupId((long)request.Id);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Permission(s) not deleted.";
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