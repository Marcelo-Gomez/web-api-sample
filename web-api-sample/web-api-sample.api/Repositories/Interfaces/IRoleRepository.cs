using System.Collections.Generic;
using System.Threading.Tasks;
using web_api_sample.api.Models.Entities;

namespace web_api_sample.api.Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<bool> NameExistsAsync(string name);

        Task<List<Role>> GetByUserIdAsync(int id);
    }
}