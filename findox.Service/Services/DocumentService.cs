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
using Microsoft.AspNetCore.Mvc;

namespace findox.Service.Services
{
    public class DocumentService : BaseService, IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiReponse> Create(IDocumentServiceRequest request)
        {
            var response = new ApiReponse();

            if (request.Metadata is null)
            {
                addMessage(response.ValidationErros, "Document", "Request form-data filename field is required.");
                return response;
            }
            if (request.File is null)
            {
                addMessage(response.ValidationErros, "Document", "Request binary file upload is required.");
                return response;
            }
            if (request.PrincipalUser is null)
            {
                addMessage(response.ValidationErros, "Document", "An authorization bearer token issued to an authorized user is required.");
                return response;
            }

            try
            {
                var userPostedString = request.PrincipalUser.FindFirst("id")?.Value;

                if (userPostedString is null)
                {
                    addMessage(response.ValidationErros, "Document", "Error reading authorized user id from bearer token.");
                    return response;
                }

                var documentDto = new DocumentDto()
                {
                    Filename = request.Metadata["filename"],
                    ContentType = request.File.ContentType,
                    Description = request.Metadata["description"],
                    Category = request.Metadata["category"],
                    UserId = Convert.ToInt64(userPostedString)
                };
                if (documentDto.Filename is null || documentDto.ContentType is null)
                {
                    addMessage(response.ValidationErros, "Document", "Filename and ContentType are required.");
                    return response;
                }

                byte[] binaryData;
                using (var ms = new MemoryStream())
                {
                    await request.File.CopyToAsync(ms);
                    binaryData = ms.ToArray();
                }
                if (binaryData is null || binaryData is not { Length: > 0 })
                {
                    addMessage(response.ValidationErros, "Document", "File (binary file data sent with the Http request) is required.");
                    return response;
                }

                var document = _mapper.Map<Document>(documentDto);

                var user = await _unitOfWork.UsersRepository.ReadById(document.UserId);
                if (user?.Id != document.UserId)
                {
                    addMessage(response.ValidationErros, "Document", "Bearer token issued to a user Id that does not exist.");
                    return response;
                }

                var existingNameCount = await _unitOfWork.DocumentsRepository.CountByColumnValue("filename", document.Filename);
                if (existingNameCount > 0)
                {
                    addMessage(response.ValidationErros, "Document", "File name already in use.");
                    return response;
                }
                var newDocument = await _unitOfWork.DocumentsRepository.Create(document);

                if (newDocument.Id == 0)
                {
                    addMessage(response.ValidationErros, "Document", "Document was not created.");
                    return response;
                }

                var documentContent = new DocumentContent()
                {
                    DocumentId = newDocument.Id,
                    Data = binaryData
                };

                await _unitOfWork.DocumentContentsRepository.Create(documentContent);


                response.Data = _mapper.Map<DocumentDto>(newDocument);

            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> ReadAllPermissioned(IDocumentServiceRequest request)
        {
            var response = new ApiReponse();

            if (request.PrincipalUser is null)
            {
                addMessage(response.ValidationErros, "Document", "An authorization bearer token issued to an authorized user is required.");
                return response;
            }

            try
            {
                var userIdString = request.PrincipalUser.FindFirst("id")?.Value;
                if (String.IsNullOrWhiteSpace(userIdString))
                {
                    addMessage(response.ValidationErros, "Document", "Error reading authorized user id from bearer token.");
                    return response;
                }
                var userId = Convert.ToInt64(userIdString);

                var user = await _unitOfWork.UsersRepository.ReadById(userId);

                List<Document> documents = new List<Document>();
                if (user?.Role == "admin") documents = (await _unitOfWork.DocumentsRepository.ReadAll()).ToList();
                else documents = (await _unitOfWork.DocumentsRepository.ReadAllPermitted(userId)).ToList();

                var documentDtos = new List<DocumentDto>();
                foreach (var document in documents) documentDtos.Add(_mapper.Map<DocumentDto>(document));
                response.Data = documentDtos;
            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> ReadByIdPermissioned(IDocumentServiceRequest request)
        {
            var response = new ApiReponse();

            if (request.Id is null || !request.Id.HasValue)
            {
                addMessage(response.ValidationErros, "Document", "Id is required.");
                return response;
            }
            if (request.PrincipalUser is null)
            {
                addMessage(response.ValidationErros, "Document", "An authorization bearer token issued to an authorized user is required.");
                return response;
            }

            try
            {
                var userIdString = request.PrincipalUser.FindFirst("id")?.Value;
                if (userIdString is null)
                {
                    addMessage(response.ValidationErros, "Document", "Error reading authorized user id from bearer token.");
                    return response;
                }
                var userId = Convert.ToInt64(userIdString);

                var user = await _unitOfWork.UsersRepository.ReadById(userId);

                Document? document = new Document();
                if (user?.Role == "admin") document = await _unitOfWork.DocumentsRepository.ReadById((long)request.Id);
                else document = await _unitOfWork.DocumentsRepository.ReadByIdPermitted((long)request.Id, userId);

                var documentContent = await _unitOfWork.DocumentContentsRepository.ReadByDocumentId((long)request.Id);

                var fileContentResult = new FileContentResult(documentContent?.Data, document?.ContentType)
                {
                    FileDownloadName = document?.Filename
                };

                response.Data = fileContentResult;

            }
            catch (Exception err)
            {
                response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> Update(IDocumentServiceRequest request)
        {
            var response = new ApiReponse();

            if (request.Item?.Id is null || !request.Item.Id.HasValue)
            {
                addMessage(response.ValidationErros, "Document", "Id is required.");
                return response;
            }

            try
            {
                var document = _mapper.Map<Document>(request.Item);

                var existing = await _unitOfWork.DocumentsRepository.ReadById(document.Id);
                if (existing?.Id != document.Id)
                {
                    addMessage(response.ValidationErros, "Document", "Requested document id for update does not exist.");
                    return response;
                }

                var successful = await _unitOfWork.DocumentsRepository.UpdateById(document);

                if (!successful.Value)
                {
                    addMessage(response.ValidationErros, "Document", "Document was not updated.");
                    return response;
                }
            }
            catch (Exception err)
            {
                 response.Erros = ToDictionary(err);
            }

            return response;
        }

        public async Task<ApiReponse> DeleteById(IDocumentServiceRequest request)
        {
            var response = new ApiReponse();

            if (request.Id is null || !request.Id.HasValue)
            {
                addMessage(response.ValidationErros,  "Document", "Id is required");
                return response;
            }

            try
            {
                var existing = await _unitOfWork.DocumentsRepository.ReadById((long)request.Id);
                if (existing?.Id != request.Id)
                {
                    addMessage(response.ValidationErros,  "Document", "Requested document id for delete does not exist.");
                    return response;
                }
                var successfulFiledataDelete = await _unitOfWork.DocumentContentsRepository.DeleteByDocumentId((long)request.Id);

                if (!successfulFiledataDelete.Value)
                {
                    addMessage(response.ValidationErros,  "Document", "Document was not deleted.");
                    return response;
                }

                var successfulDocumentDelete = await _unitOfWork.DocumentsRepository.DeleteById((long)request.Id);

                if (!successfulDocumentDelete.Value)
                {
                    addMessage(response.ValidationErros,  "Document", "Document was not deleted.");
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