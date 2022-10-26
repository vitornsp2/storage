

using findox.Domain.Models.Database;

namespace findox.Domain.Interfaces.Repository
{
    public interface IDocumentContentRepository : IBaseRepository
    {
        Task<long> Create(DocumentContent documentContent);
        Task<DocumentContent> ReadByDocumentId(long document_id);
        Task<bool> DeleteByDocumentId(long document_id);
    }
}