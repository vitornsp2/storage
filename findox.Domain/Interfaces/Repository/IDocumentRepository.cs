using findox.Domain.Models.Database;

namespace findox.Domain.Interfaces.Repository
{
    public interface IDocumentRepository : IBaseRepository<Document>
    {
        Task<Document> Create(Document document);
        Task<IEnumerable<Document>> ReadAll();
        Task<Document?> ReadById(long id);
        Task<IEnumerable<Document>> ReadAllPermitted(long requestingUserId);
        Task<Document?> ReadByIdPermitted(long documentId, long userId);
        Task<bool?> UpdateById(Document document);
        Task<bool?> DeleteById(long id);
        Task<int?> CountByColumnValue(string column, string value);
    }
}