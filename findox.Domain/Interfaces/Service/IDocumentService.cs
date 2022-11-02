using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Service;

namespace findox.Domain.Interfaces.Service
{
    public interface IDocumentService
    {
        Task<ApiReponse> Create(IDocumentServiceRequest request);
        Task<ApiReponse> ReadAllPermissioned(IDocumentServiceRequest request);
        Task<ApiReponse> ReadByIdPermissioned(IDocumentServiceRequest request);
        Task<ApiReponse> Update(IDocumentServiceRequest request);
        Task<ApiReponse> DeleteById(IDocumentServiceRequest request);
    }
}