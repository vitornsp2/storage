using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Interfaces.Repository;
using findox.Domain.Maps;
using findox.Domain.Models.Database;
using Npgsql;

namespace findox.Data.Repositories
{
    public class DocumentRepository : BaseRepository, IDocumentRepository
    {
        public DocumentRepository(NpgsqlConnection connection) : base(connection)
        {
            _connection = connection;
        }

        public async Task<Document> Create(Document document)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_create('{document.Filename}', '{document.ContentType}', {document.UserId}";

                if (document.Description is not null) command.CommandText += $", '{document.Description}'";
                else command.CommandText += $", null";

                if (document.Category is not null) command.CommandText += $", '{document.Category}'";
                else command.CommandText += $", null";

                command.CommandText += $");";

                var reader = await RunQuery(command);

                var newDocument = new Document();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    newDocument = map.Document(reader);
                }
                reader.Close();
                return newDocument;
            }
        }

        public async Task<List<Document>> ReadAll()
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_get_all();";

                var reader = await RunQuery(command);
                var documents = new List<Document>();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    documents.Add(map.Document(reader));
                }
                reader.Close();
                return documents;
            }
        }

        public async Task<Document> ReadById(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_get_by_id({id});";

                var reader = await RunQuery(command);
                var document = new Document();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    document = map.Document(reader);
                }
                reader.Close();
                return document;
            }
        }

        public async Task<List<Document>> ReadAllPermitted(long requestingUserId)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_get_all_permitted({requestingUserId});";

                var reader = await RunQuery(command);
                var documents = new List<Document>();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    documents.Add(map.Document(reader));
                }
                reader.Close();
                return documents;
            }
        }

        public async Task<Document> ReadByIdPermitted(long documentId, long userId)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_get_by_id_permitted({documentId}, {userId});";

                var reader = await RunQuery(command);
                var document = new Document();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    document = map.Document(reader);
                }
                reader.Close();
                return document;
            }
        }

        public async Task<bool> UpdateById(Document document)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_update({document.Id}";

                if (document.Filename is not null) command.CommandText += $", '{document.Filename}'";
                else command.CommandText += $", null";

                if (document.Description is not null) command.CommandText += $", '{document.Description}'";
                else command.CommandText += $", null";

                if (document.Category is not null) command.CommandText += $", '{document.Category}'";
                else command.CommandText += $", null";

                command.CommandText += ");";

                var result = await RunScalar(command);
                var success = false;
                if (result is not null)
                {
                    success = bool.Parse($"{result.ToString()}");
                }
                return success;
            }
        }

        public async Task<bool> DeleteById(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_delete_by_id({id});";

                var result = await RunScalar(command);
                var success = false;
                if (result is not null)
                {
                    success = bool.Parse($"{result.ToString()}");
                }
                return success;
            }
        }

        public async Task<int> CountByColumnValue(string column, string value)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_count_by_column_value_text('{column}', '{value}');";

                var result = await RunScalar(command);
                int count = 0;
                if (result is not null)
                {
                    count = int.Parse($"{result.ToString()}");
                }
                return count;
            }
        }
    }
}
