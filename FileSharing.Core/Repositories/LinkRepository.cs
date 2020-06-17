using Dapper;
using FileSharing.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LinkSharing.Core.Repositories
{
    public class LinkRepository
    {
        private readonly string connectionString;
        public LinkRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Link entity)
        {
            string sql = @"INSERT INTO Links (FileId, Email, Url, Count, PublicId) values (@FileId, @Email, @Url, @Count, @PublicId);
                           SELECT SCOPE_IDENTITY();";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int id = connection.Query<int>(sql, entity).FirstOrDefault();
                return id;
            }
        }

        public Link Get(int id)
        {
            string sql = @"Select * FROM Links WHERE Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Link entity = connection.Query<Link>(sql, new { Id = id }).FirstOrDefault();
                return entity;
            }
        }

        public Link Get(Guid publicId)
        {
            string sql = @"Select * FROM Links WHERE publicId = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Link entity = connection.Query<Link>(sql, new { Id = publicId }).FirstOrDefault();
                return entity;
            }
        }

        public IEnumerable<Link> GetAll(int fileId)
        {
            string sql = @"Select * FROM Links Where FileId = @FileId";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                IEnumerable<Link> result = connection.Query<Link>(sql, new { FileId = fileId });
                return result;
            }
        }

        public int Update(Link entity)
        {
            string sql = @"UPDATE [dbo].[Links]
                    SET [FileId] = @FileId
                    ,[Email] = @Email
                    ,Url = @Url
                    ,Count = @Count
                    ,PublicId = @PublicId
                     WHERE  Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int rowsCount = connection.Execute(sql, entity);
                return rowsCount;
            }
        }


        public bool Delete(int id)
        {
            string sql = @"Delete FROM [dbo].[Links] WHERE  Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int rowsCount = connection.Execute(sql, new { Id = id });
                return rowsCount > 0;
            }
        }
        public bool Delete(Link entity)
        {
            return Delete(entity.Id);
        }
    }
}
