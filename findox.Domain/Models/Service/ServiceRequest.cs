using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Dto;
using Microsoft.AspNetCore.Http;

namespace findox.Domain.Models.Service
{
    public interface IServiceRequest<T>
    {
        long? Id { get; }
        T? Item { get; }
        System.Security.Claims.ClaimsPrincipal? PrincipalUser { get; }
    }

    public class ServiceRequest<T> : IServiceRequest<T>
    {
        public long? Id { get; set; }

        public T? Item { get; set; }

        public System.Security.Claims.ClaimsPrincipal? PrincipalUser { get; set; }
    }

    
    public interface IDocumentServiceRequest : IServiceRequest<DocumentDto>
    {
        IFormCollection? Metadata { get; }
        IFormFile? File { get; }
    }

    public class DocumentServiceRequest : ServiceRequest<DocumentDto>, IDocumentServiceRequest
    {
        public IFormCollection? Metadata { get; set; }
        public IFormFile? File { get; set; }

        public DocumentServiceRequest()
        {
            Item = new DocumentDto();
        }

        public DocumentServiceRequest(DocumentDto document)
        {
            Item = document;
        }

        public DocumentServiceRequest(long id)
        {
            Id = id;
        }

        public DocumentServiceRequest(IFormCollection metadata, IFormFile file, System.Security.Claims.ClaimsPrincipal principalUser)
        {
            Metadata = metadata;
            File = file;
            PrincipalUser = principalUser;
        }
    }
}