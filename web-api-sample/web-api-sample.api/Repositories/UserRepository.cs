using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using web_api_sample.api.Models.Entities;
using web_api_sample.api.Repositories.Interfaces;

namespace web_api_sample.api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection db;
        private string sql;

        public UserRepository(IConfiguration configuration)
        {
            db = new MySqlConnection(configuration.GetConnectionString("MysqlConnection"));
        }

        public async Task<int> AddAsync(User user)
        {
            sql = @"
            INSERT INTO User
            (Name, UserName, Password, Email, Active, CreatedAt)
            VALUES
            (@Name, @UserName, @Password, @Email, @Active, @CreatedAt);";

            await db.ExecuteAsync(sql, user);

            sql = "SELECT LAST_INSERT_ID();";

            return await db.QueryFirstOrDefaultAsync<int>(sql);
        }

        public async Task AddRolesAsync(int userId, int roleId)
        {
            sql = @"
            INSERT INTO UserRole
            (UserId, RoleId)
            VALUES
            (@UserId, @RoleId);";

            await db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
        }

        public async Task UpdateAsync(User user)
        {
            sql = @"
            UPDATE User
            SET Name = @Name, UserName = @UserName, Password = @Password, Email = @Email, Active = @Active
            WHERE Id = @Id;";

            await db.ExecuteAsync(sql, user);
        }

        public async Task DeleteAsync(int id)
        {
            sql = "DELETE FROM User WHERE Id = @Id;";

            await db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task DeleteUserRolesAsync(int id)
        {
            sql = "DELETE FROM UserRole WHERE UserId = @UserId;";

            await db.ExecuteAsync(sql, new { UserId = id });
        }

        public async Task<List<User>> GetAllAsync()
        {
            sql = "SELECT * FROM User;";

            return (await db.QueryAsync<User>(sql)).ToList();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            sql = "SELECT * FROM User WHERE Id = @Id LIMIT 1;";

            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            sql = "SELECT COUNT(1) FROM User WHERE Id = @Id;";

            return await db.ExecuteScalarAsync<bool>(sql, new { Id = id });
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            sql = "SELECT COUNT(1) FROM User WHERE Username = @Username;";

            return await db.ExecuteScalarAsync<bool>(sql, new { Username = username });
        }

        public async Task<User> GetByUsernameAndPasswordAsync(string username, string password)
        {
            sql = @"
            SELECT * FROM user 
            WHERE Username = @Username 
            AND Password = @Password 
            AND Active = true;";

            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username, Password = password });
        }
    }
}