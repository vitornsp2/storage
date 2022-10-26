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
using Microsoft.AspNetCore.Mvc;

namespace findox.Service.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDocumentServiceResponse> Create(IDocumentServiceRequest request)
        {
            var response = new DocumentServiceResponse();

            if (request.Metadata is null)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Request form-data filename field is required.";
                return response;
            }
            if (request.File is null)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Request binary file upload is required.";
                return response;
            }
            if (request.PrincipalUser is null)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "An authorization bearer token issued to an authorized user is required.";
                return response;
            }

            try
            {
                var userPostedString = request.PrincipalUser.FindFirst("id")?.Value;
                if (userPostedString is null)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Error reading authorized user id from bearer token.";
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
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Filename and ContentType are required.";
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
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "File (binary file data sent with the Http request) is required.";
                    return response;
                }
                
                var document = _mapper.Map<Document>(documentDto);

                _unitOfWork.Begin();

                var user = await _unitOfWork.users.ReadById(document.UserId);
                if (user.Id != document.UserId)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Bearer token issued to a user Id that does not exist.";
                    return response;
                }

                var existingNameCount = await _unitOfWork.documents.CountByColumnValue("filename", document.Filename);
                if (existingNameCount > 0)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "File name already in use.";
                    return response;
                }
                var newDocument = await _unitOfWork.documents.Create(document);

                if (newDocument.Id == 0)
                {
                    response.Outcome = OutcomeType.Error;
                    return response;
                }

                var documentContent = new DocumentContent()
                {
                    DocumentId = newDocument.Id,
                    Data = binaryData
                };
                await _unitOfWork.documentContents.Create(documentContent);

                _unitOfWork.Commit();

                response.Item = _mapper.Map<DocumentDto>(newDocument);

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IDocumentServiceResponse> ReadAllPermissioned(IDocumentServiceRequest request)
        {
            var response = new DocumentServiceResponse();

            if (request.PrincipalUser is null)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "An authorization bearer token issued to an authorized user is required.";
                return response;
            }

            try
            {
                var userIdString = request.PrincipalUser.FindFirst("id")?.Value;
                if (String.IsNullOrWhiteSpace(userIdString))
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Error reading authorized user id from bearer token.";
                    return response;
                }
                var userId = Convert.ToInt64(userIdString);
                
                var user = await _unitOfWork.users.ReadById(userId);

                List<Document> documents = new List<Document>();
                if (user.Role == "admin") documents = await _unitOfWork.documents.ReadAll(); 
                else documents = await _unitOfWork.documents.ReadAllPermitted(userId); 

                var documentDtos = new List<IDocumentDto>();
                foreach (var document in documents) documentDtos.Add(_mapper.Map<DocumentDto>(document));
                response.List = documentDtos;

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IDocumentServiceResponse> ReadByIdPermissioned(IDocumentServiceRequest request)
        {
            var response = new DocumentServiceResponse();

            if (request.Id is null || !request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }
            if (request.PrincipalUser is null)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "An authorization bearer token issued to an authorized user is required.";
                return response;
            }

            try
            {
                var userIdString = request.PrincipalUser.FindFirst("id")?.Value;
                if (userIdString is null)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Error reading authorized user id from bearer token.";
                    return response;
                }
                var userId = Convert.ToInt64(userIdString);

                var user = await _unitOfWork.users.ReadById(userId);

                Document document = new Document();
                if (user.Role == "admin") document = await _unitOfWork.documents.ReadById((long)request.Id); 
                else document = await _unitOfWork.documents.ReadByIdPermitted((long)request.Id, userId);

                var documentContent = await _unitOfWork.documentContents.ReadByDocumentId((long)request.Id);

                var fileContentResult = new FileContentResult(documentContent.Data, document.ContentType)
                {
                    FileDownloadName = document.Filename
                };
                response.FileContentResult = fileContentResult;

                response.Outcome = OutcomeType.Success;
            }
            catch (Exception)
            {
                response.Outcome = OutcomeType.Error;
            }

            return response;
        }

        public async Task<IDocumentServiceResponse> Update(IDocumentServiceRequest request)
        {
            var response = new DocumentServiceResponse();

            if (request.Item?.Id is null || !request.Item.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required.";
                return response;
            }

            try
            {
                var document = _mapper.Map<Document>(request.Item);

                _unitOfWork.Begin();

                var existing = await _unitOfWork.documents.ReadById(document.Id);
                if (existing.Id != document.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested document id for update does not exist.";
                    return response;
                }

                var successful = await _unitOfWork.documents.UpdateById(document);

                if (!successful)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Document was not updated.";
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

        public async Task<IDocumentServiceResponse> DeleteById(IDocumentServiceRequest request)
        {
            var response = new DocumentServiceResponse();

            if (request.Id is null || !request.Id.HasValue)
            {
                response.Outcome = OutcomeType.Fail;
                response.ErrorMessage = "Id is required";
                return response;
            }

            try
            {
                _unitOfWork.Begin();

                var existing = await _unitOfWork.documents.ReadById((long)request.Id);
                if (existing.Id != request.Id)
                {
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Requested document id for delete does not exist.";
                    return response;
                }
                var successfulFiledataDelete = await _unitOfWork.documentContents.DeleteByDocumentId((long)request.Id);

                if (!successfulFiledataDelete)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Document was not deleted.";
                    return response;
                }

                var successfulDocumentDelete = await _unitOfWork.documents.DeleteById((long)request.Id);

                if (!successfulDocumentDelete)
                {
                    _unitOfWork.Rollback();
                    response.Outcome = OutcomeType.Fail;
                    response.ErrorMessage = "Document was not deleted.";
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