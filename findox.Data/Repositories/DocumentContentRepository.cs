using Dapper;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Models.Database;

namespace findox.Data.Repositories
{
    public class DocumentContentRepository : BaseRepository<DocumentContent>, IDocumentContentRepository
    {
        public DocumentContentRepository() : base()
        {
        }
        public async Task<long?> Create(DocumentContent documentContent)
        {
            var procedureName = "storage.documents_content_create";

            var param = new DynamicParameters();

            param.Add("document_id", documentContent.DocumentId);
            param.Add("data", documentContent.Data);

            return ((long?)await base.ExecuteEscalar(procedureName, param));
        }

        public async Task<DocumentContent?> ReadByDocumentId(long id)
        {
            var procedureName = "storage.documents_content_get_by_document_id";
            var param = new { id = id };
            var documents = await base.Query(procedureName, param);
            return documents?.FirstOrDefault();
        }

        public async Task<bool?> DeleteByDocumentId(long id)
        {
            var procedureName = "storage.documents_content_delete_by_document_id";
            var param = new { id = id };
            return (bool?)await base.ExecuteEscalar(procedureName, param);
        }
    }
}
