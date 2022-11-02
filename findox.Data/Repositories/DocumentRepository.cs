using Dapper;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Models.Database;

namespace findox.Data.Repositories
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        public DocumentRepository() : base()
        {
        }

        public async Task<Document> Create(Document document)
        {
            var procedureName = "storage.documents_create";

            var param = new DynamicParameters();

            param.Add("filename", document.Filename);
            param.Add("content_type", document.ContentType);
            param.Add("user_id", document.UserId);
            param.Add("description", string.IsNullOrWhiteSpace(document.Description) ? null : document.Description);
            param.Add("category", string.IsNullOrWhiteSpace(document.Category) ? null : document.Category);

            var id = ((long?)await base.ExecuteEscalar(procedureName, param));
            document.Id = id.HasValue ? id.Value : 0;
            return document;
        }

        public async Task<IEnumerable<Document>> ReadAll()
        {
            var procedureName = "storage.documents_get_all";
            var documents = await base.Query(procedureName);
            return documents;
        }

        public async Task<Document?> ReadById(long id)
        {
            var procedureName = "storage.documents_get_by_id";
            var param = new { id = id };
            var documents = await base.Query(procedureName, param);
            return documents?.FirstOrDefault();
        }

        public async Task<IEnumerable<Document>> ReadAllPermitted(long requestingUserId)
        {
            var procedureName = "storage.documents_get_all_permitted";
            var param = new { id_user = requestingUserId };
            return await base.Query(procedureName, param);
        }

        public async Task<Document?> ReadByIdPermitted(long documentId, long userId)
        {
            var procedureName = "storage.documents_get_by_id_permitted";
            var param = new { id_document = documentId, id_user = userId };
            var documents = await base.Query(procedureName, param);
            return documents?.FirstOrDefault();
        }

        public async Task<bool?> UpdateById(Document document)
        {
            var param = new DynamicParameters();
            var procedureName = "storage.documents_update";

            param.Add("id", document.Id);
            param.Add("filename", string.IsNullOrWhiteSpace(document.Filename) ? null : document.Filename);
            param.Add("description", string.IsNullOrWhiteSpace(document.Description) ? null : document.Description);
            param.Add("category", string.IsNullOrWhiteSpace(document.Category) ? null : document.Category);

            return (bool?)await base.ExecuteEscalar(procedureName, param);
        }

        public async Task<bool?> DeleteById(long id)
        {
            var procedureName = "storage.documents_delete_by_id";
            var param = new { id = id };
            return (bool?)await base.ExecuteEscalar(procedureName, param);
        }

        public async Task<int?> CountByColumnValue(string column, string value)
        {
            var procedureName = "storage.documents_count_by_column_value_text";
            var param = new { column_name = column, column_value = value };
            return (int?)await base.ExecuteEscalar(procedureName, param);
        }
    }
}
