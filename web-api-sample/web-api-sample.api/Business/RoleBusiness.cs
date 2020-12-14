using System.Collections.Generic;
using System.Threading.Tasks;
using web_api_sample.api.Models.Entities;
using web_api_sample.api.Repositories.Interfaces;

namespace web_api_sample.api.Business
{
    public class RoleBusiness
    {
        private readonly IRoleRepository _roleRepository;

        public RoleBusiness(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> AddAsync(Role role)
        {
            bool result = false;

            if (role != null && !await _roleRepository.NameExistsAsync(role.Name))
            {
                role.NewEntity();

                await _roleRepository.AddAsync(role);

                result = true;
            }

            return result;
        }

        public async Task<bool> UpdateAsync(Role role)
        {
            bool result = false;

            if (role != null && await _roleRepository.ExistsAsync(role.Id))
            {
                await _roleRepository.UpdateAsync(role);

                result = true;
            }

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool result = false;

            if (await _roleRepository.ExistsAsync(id))
            {
                await _roleRepository.DeleteAsync(id);

                result = true;
            }

            return result;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<bool> ValidateRoleNameAsync(Role role)
        {
            bool result = false;

            Role oldRole = await _roleRepository.GetByIdAsync(role.Id);

            if (oldRole != null && oldRole.Name != role.Name)
            {
                if (!await _roleRepository.NameExistsAsync(role.Name))
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }

            return result;
        }
    }
}