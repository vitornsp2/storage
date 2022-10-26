using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Service;

namespace findox.Domain.Interfaces.Service
{
    public interface IDocumentService
    {
        Task<IDocumentServiceResponse> Create(IDocumentServiceRequest request);
        Task<IDocumentServiceResponse> ReadAllPermissioned(IDocumentServiceRequest request);
        Task<IDocumentServiceResponse> ReadByIdPermissioned(IDocumentServiceRequest request);
        Task<IDocumentServiceResponse> Update(IDocumentServiceRequest request);
        Task<IDocumentServiceResponse> DeleteById(IDocumentServiceRequest request);
    }
}