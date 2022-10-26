using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Database;
using Npgsql;

namespace findox.Domain.Maps
{
    public class DatabaseMap
    {
        public User User(NpgsqlDataReader reader)
        {
            return new User()
            {
                Id = Int64.Parse($"{reader["id"].ToString()}"),
                Name = $"{reader["name"].ToString()}",
                Password = $"{reader["password"].ToString()}",
                Email = $"{reader["email"].ToString()}",
                Role = $"{reader["role"].ToString()}",
                CreatedDate = DateTime.Parse($"{reader["created_date"].ToString()}")
            };
        }

        public Document Document(NpgsqlDataReader reader)
        {
            return new Document()
            {
                Id = Int64.Parse($"{reader["id"].ToString()}"),
                Filename = $"{reader["filename"].ToString()}",
                ContentType = $"{reader["content_type"].ToString()}",
                Description = reader["description"] is not DBNull ? $"{reader["description"].ToString()}" : null,
                Category = reader["category"] is not DBNull ? $"{reader["category"].ToString()}" : null,
                CreatedDate = DateTime.Parse($"{reader["created_date"].ToString()}"),
                UserId = Int64.Parse($"{reader["user_id"].ToString()}"),
            };
        }

        public DocumentContent DocumentContent(NpgsqlDataReader reader)
        {
            return new DocumentContent()
            {
                DocumentId = Int64.Parse($"{reader["document_id"].ToString()}"),
                Data = (byte[])reader["data"],
            };
        }

        public Group Group(NpgsqlDataReader reader)
        {
            return new Group()
            {
                Id = Int64.Parse($"{reader["id"].ToString()}"),
                Name = $"{reader["name"].ToString()}",
                Description = reader["description"] is not DBNull ? $"{reader["description"].ToString()}" : null,
                CreatedDate = DateTime.Parse($"{reader["created_date"].ToString()}"),
            };
        }

        public UserGroup UserGroup(NpgsqlDataReader reader)
        {
            return new UserGroup()
            {
                Id = Int64.Parse($"{reader["id"].ToString()}"),
                GroupId = Int64.Parse($"{reader["group_id"].ToString()}"),
                UserId = Int64.Parse($"{reader["user_id"].ToString()}"),
            };
        }

        public Permission Permission(NpgsqlDataReader reader)
        {
            return new Permission()
            {
                Id = Int64.Parse($"{reader["id"].ToString()}"),
                DocumentId = Int64.Parse($"{reader["document_id"].ToString()}"),
                UserId = reader["user_id"] is not DBNull ? Int64.Parse($"{reader["user_id"].ToString()}") : null,
                GroupId = reader["group_id"] is not DBNull ? Int64.Parse($"{reader["group_id"].ToString()}") : null,
            };
        }
    }
}