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
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbConnection db;
        private string sql;

        public RoleRepository(IConfiguration configuration)
        {
            db = new MySqlConnection(configuration.GetConnectionString("MysqlConnection"));
        }

        public async Task<int> AddAsync(Role role)
        {
            sql = @"
            INSERT INTO Role
            (Name, Description, Active, CreatedAt)
            VALUES
            (@Name, @Description, @Active, @CreatedAt);";

            await db.ExecuteAsync(sql, role);

            sql = "SELECT LAST_INSERT_ID();";

            return await db.QueryFirstOrDefaultAsync<int>(sql);
        }

        public async Task UpdateAsync(Role role)
        {
            sql = @"
            UPDATE Role 
            SET Name = @Name, Description = @Description, Active = @Active 
            WHERE Id = @Id;";

            await db.ExecuteAsync(sql, role);
        }

        public async Task DeleteAsync(int id)
        {
            sql = "DELETE FROM Role WHERE Id = @id;";

            await db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<List<Role>> GetAllAsync()
        {
            sql = "SELECT * FROM Role;";

            return (await db.QueryAsync<Role>(sql)).ToList();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            sql = "SELECT * FROM Role WHERE Id = @Id LIMIT 1;";

            return await db.QueryFirstOrDefaultAsync<Role>(sql, new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            sql = "SELECT COUNT(1) FROM Role WHERE Id = @Id;";

            return await db.ExecuteScalarAsync<bool>(sql, new { Id = id });
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            sql = "SELECT COUNT(1) FROM Role WHERE Name = @Name;";

            return await db.ExecuteScalarAsync<bool>(sql, new { Name = name });
        }

        public async Task<List<Role>> GetByUserIdAsync(int id)
        {
            sql = @"
            SELECT r.* 
            FROM Role r
            INNER JOIN UserRole ur ON r.Id = ur.RoleId
            WHERE ur.UserId = @UserId
            AND r.Active = true;";

            return (await db.QueryAsync<Role>(sql, new { UserId = id })).ToList();
        }
    }
}