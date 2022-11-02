using AutoMapper;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Database;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using FluentValidation;

namespace findox.Service.Services
{
    public class PermissionService : BaseService, IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<PermissionDto> _validator;

        public PermissionService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<PermissionDto> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ApiReponse> Create(PermissionDto permissionDto)
        {
            var response = new ApiReponse();

            var validationResult = _validator.Validate(permissionDto);

            if (validationResult.IsValid)
            {
                try
                {
                    var permission = _mapper.Map<Permission>(permissionDto);

                    var existingDocument = await _unitOfWork.DocumentsRepository.ReadById(permission.DocumentId);
                    if (existingDocument?.Id != permission.DocumentId)
                    {
                        addMessage(response.ValidationErros, "Permission", "Requested new permission's DocumentId does not exist.");
                        return response;
                    }

                    if (permission.UserId.HasValue)
                    {
                        var existingUser = await _unitOfWork.UsersRepository.ReadById((long)permission.UserId);
                        if (existingUser?.Id != permission.UserId)
                        {
                            addMessage(response.ValidationErros, "Permission", "Requested new permission's UserId does not exist.");
                            return response;
                        }
                    }

                    if (permission.GroupId.HasValue)
                    {
                        var existingGroup = await _unitOfWork.GroupsRepository.ReadById((long)permission.GroupId);
                        if (existingGroup?.Id != permission.GroupId)
                        {
                            addMessage(response.ValidationErros, "Permission", "Requested new permission's GroupId does not exist.");
                            return response;
                        }
                    }

                    var existingCount = await _unitOfWork.PermissionsRepository.PermissionMatchCount(permission);
                    if (existingCount > 0)
                    {
                        addMessage(response.ValidationErros, "Permission", "Requested new permission already exists.");
                        return response;
                    }

                    var newPermission = await _unitOfWork.PermissionsRepository.Create(permission);

                    response.Data = _mapper.Map<PermissionDto>(newPermission);

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

        public async Task<ApiReponse> ReadByDocumentId(long? id)
        {
            var response = new ApiReponse();

            if (!id.HasValue)
            {
                addMessage(response.ValidationErros, "Permission", "Id is required.");
                return response;
            }

            try
            {
                var permissions = await _unitOfWork.PermissionsRepository.ReadByDocumentId((long)id.Value);

                var permissionDtos = new List<IPermissionDto>();
                foreach (var permission in permissions) permissionDtos.Add(_mapper.Map<PermissionDto>(permission));
                response.Data = permissionDtos;

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
                addMessage(response.ValidationErros, "Permission", "Id is required.");
                return response;
            }

            try
            {
                var existing = await _unitOfWork.PermissionsRepository.ReadById(id.Value);
                if (existing?.Id != id)
                {
                    addMessage(response.ValidationErros, "Permission", "Requested permission id for delete does not exist.");
                    return response;
                }

                var successful = await _unitOfWork.PermissionsRepository.DeleteById(id.Value);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "Permission", "Permission was not deleted.");
                    return response;
                }
            }
            catch (Exception err)
            {
                 response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> DeleteByDocumentId(long? id)
        {
            var response = new ApiReponse();

            if (!id.HasValue)
            {
                addMessage(response.ValidationErros, "Permission", "Id is required.");
                return response;
            }

            try
            {
                var existingCount = await _unitOfWork.PermissionsRepository.CountByColumnValue("document_id", id.Value);
                if (existingCount < 1)
                {
                    addMessage(response.ValidationErros, "Permission", "Requested permissions(s) for delete not found.");
                    return response;
                }

                var successful = await _unitOfWork.PermissionsRepository.DeleteByDocumentId(id.Value);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "Permission", "Permission(s) not deleted.");
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
                addMessage(response.ValidationErros, "Permission", "Id is required.");
                return response;
            }

            try
            {
                var existingCount = await _unitOfWork.PermissionsRepository.CountByColumnValue("user_id", id.Value);
                if (existingCount < 1)
                {
                    addMessage(response.ValidationErros, "Permission", "Id is required.");
                    return response;
                }

                var successful = await _unitOfWork.PermissionsRepository.DeleteByUserId(id.Value);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "Permission", "Permission(s) not deleted.");
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
                addMessage(response.ValidationErros,"Permission", "Id is required.");
                return response;
            }

            try
            {
                var existingCount = await _unitOfWork.PermissionsRepository.CountByColumnValue("group_id",id.Value);
                if (existingCount < 1)
                {
                    addMessage(response.ValidationErros,"Permission", "Requested permissions(s) for delete not found.");
                    return response;
                }

                var successful = await _unitOfWork.PermissionsRepository.DeleteByGroupId(id.Value);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros,"Permission", "Permission(s) not deleted.");
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