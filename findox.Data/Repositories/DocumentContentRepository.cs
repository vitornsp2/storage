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
    public class DocumentContentRepository : BaseRepository, IDocumentContentRepository
    {
        public DocumentContentRepository(NpgsqlConnection connection) : base(connection)
        {
            _connection = connection;
        }

        public async Task<long> Create(DocumentContent documentContent)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_content_create({documentContent.DocumentId}, :data );";

                var npgsqlParameter = new NpgsqlParameter("data", NpgsqlTypes.NpgsqlDbType.Bytea);
                npgsqlParameter.Value = documentContent.Data;

                command.Parameters.Add(npgsqlParameter);

                long newId = 0;
                var reader = await RunQuery(command);
                var newUser = new User();
                while (await reader.ReadAsync())
                {
                    newId = long.Parse($"{reader["id"].ToString()}");
                }
                reader.Close();
                return newId;
            }
        }

        public async Task<DocumentContent> ReadByDocumentId(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_content_get_by_document_id({id});";

                var reader = await RunQuery(command);
                var documentContent = new DocumentContent();
                while (await reader.ReadAsync())
                {
                    var map = new DatabaseMap();
                    documentContent = map.DocumentContent(reader);
                }
                reader.Close();
                return documentContent;
            }
        }

        public async Task<bool> DeleteByDocumentId(long id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = $"SELECT * FROM storage.documents_content_delete_by_document_id({id});";

                var result = await RunScalar(command);
                var success = false;
                if (result is not null)
                {
                    success = bool.Parse($"{result.ToString()}");
                }
                return success;
            }
        }
    }
}
