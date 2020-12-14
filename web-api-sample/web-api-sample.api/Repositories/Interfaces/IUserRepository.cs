using System.Threading.Tasks;
using web_api_sample.api.Models.Entities;

namespace web_api_sample.api.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task AddRolesAsync(int userId, int roleId);

        Task DeleteUserRolesAsync(int id);

        Task<bool> UsernameExistsAsync(string userName);

        Task<User> GetByUsernameAndPasswordAsync(string username, string password);
    }
}