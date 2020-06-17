using Dapper;
using FileSharing.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FileSharing.Core.Repositories
{
    public class FileRepository
    {
        public readonly string connectionString;
        public FileRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(File entity)
        {
            string sql = @"INSERT INTO Files (Name, Path, DateCreated, DateExpiredOn) values (@Name, @Path, @DateCreated, @DateExpiredOn);
                           SELECT SCOPE_IDENTITY();";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int id = connection.Query<int>(sql, entity).FirstOrDefault();
                return id;
            }
        }

        public File Get(int id)
        {
            string sql = @"Select * FROM Files WHERE Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                File entity = connection.Query<File>(sql, new { Id = id }).FirstOrDefault();
                return entity;
            }
        }

        public IEnumerable<File> GetAll()
        {
            string sql = @"Select * FROM Files";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                IEnumerable<File> result = connection.Query<File>(sql);
                return result;
            }
        }

        public int Update(File entity)
        {
            string sql = @"UPDATE [dbo].[Files]
                    SET [Name] = @Name
                    ,[Path] = @Path
                    ,DateCreated = @DateCreated
                    ,DateExpiredOn = @DateExpiredOn
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
            string sql = @"
                    Delete from Links Where FileId = @Id
                    Delete FROM [dbo].[Files] WHERE  Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int rowsCount = connection.Execute(sql, new { Id = id });
                return rowsCount > 0;
            }
        }
        public bool Delete(File entity)
        {
            return Delete(entity.Id);
        }
    }
}
