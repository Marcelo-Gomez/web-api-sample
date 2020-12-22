using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_api_sample.api.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> AddAsync(T role);

        Task UpdateAsync(T role);

        Task DeleteAsync(int id);

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}