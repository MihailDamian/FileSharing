using Dapper;
using FileSharing.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSharing.Core.Repositories
{
    public class UserRepository
    {
        private readonly string connectionString;
        public UserRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(User entity)
        {
            string sql = @"INSERT INTO Users (Name, Password) values (@Name, @Password);
                           SELECT SCOPE_IDENTITY();";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int id = connection.Query<int>(sql, entity).FirstOrDefault();
                return id;
            }
        }

        public User Get(int id)
        {
            string sql = @"Select Name,Password FROM Users WHERE Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                User entity = connection.Query<User>(sql, new { Id = id }).FirstOrDefault();
                return entity;
            }
        }

        public IEnumerable<User> GetAll()
        {
            string sql = @"Select Name,Password FROM Users";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                IEnumerable<User> result = connection.Query<User>(sql);
                return result;
            }
        }

        public int Update(User entity)
        {
            string sql = @"UPDATE [dbo].[Users]
                    SET [Name] = @Name
                    ,[Password] = @Password
                     WHERE  Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int rowsCount = connection.Execute(sql, entity);
                return rowsCount;
            }
        }
    }
}
