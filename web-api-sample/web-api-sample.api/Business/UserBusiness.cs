using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using web_api_sample.api.Models.Dtos;
using web_api_sample.api.Models.Entities;
using web_api_sample.api.Repositories.Interfaces;
using web_api_sample.api.Services.Interfaces;

namespace web_api_sample.api.Business
{
    public class UserBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailService _emailService;

        public UserBusiness(IUserRepository userRepository, IRoleRepository roleRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _emailService = emailService;
        }

        public async Task<bool> AddAsync(User user)
        {
            bool result = false;

            if (user != null && !await _userRepository.UsernameExistsAsync(user.Username))
            {
                user.NewEntity();

                user.Password = PasswordEncryption(user.Password);
                user.Id = await _userRepository.AddAsync(user);

                if (user.Roles != null && user.Roles.Count > 0)
                {
                    foreach (Role role in user.Roles)
                    {
                        await _userRepository.AddRolesAsync(user.Id, role.Id);
                    }
                }

                await SendRegistrationEmail(user.Email);

                result = true;
            }

            return result;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            bool result = false;

            if (user != null && await _userRepository.ExistsAsync(user.Id))
            {
                user.Password = PasswordEncryption(user.Password);

                await _userRepository.UpdateAsync(user);

                await _userRepository.DeleteUserRolesAsync(user.Id);

                if (user.Roles != null && user.Roles.Count > 0)
                {
                    foreach (Role role in user.Roles)
                    {
                        await _userRepository.AddRolesAsync(user.Id, role.Id);
                    }
                }

                result = true;
            }

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool result = false;

            if (await _userRepository.ExistsAsync(id))
            {
                await _userRepository.DeleteUserRolesAsync(id);

                await _userRepository.DeleteAsync(id);

                result = true;
            }

            return result;
        }

        public async Task<List<User>> GetAllAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();

            if (users != null && users.Count > 0)
            {
                foreach (User user in users)
                {
                    user.Roles = await GetRoleByUserIdAsync(user.Id);
                }
            }

            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            User result = await _userRepository.GetByIdAsync(id);

            if (result != null)
            {
                result.Roles = await GetRoleByUserIdAsync(id);
            }

            return result;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            string encryptedPassword = PasswordEncryption(password);

            User result = await _userRepository.GetByUsernameAndPasswordAsync(username, encryptedPassword);

            if (result != null)
            {
                result.Roles = await GetRoleByUserIdAsync(result.Id);
            }

            return result;
        }

        public async Task<bool> ValidateUsernameAsync(User user)
        {
            bool result = false;

            User oldUser = await _userRepository.GetByIdAsync(user.Id);

            if (oldUser != null && oldUser.Username != user.Username)
            {
                if (!await _userRepository.UsernameExistsAsync(user.Username))
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

        private async Task<List<Role>> GetRoleByUserIdAsync(int userId)
        {
            return await _roleRepository.GetByUserIdAsync(userId);
        }

        private async Task SendRegistrationEmail(string userEmail)
        {
            EmailDto email = new EmailDto()
            {
                To = userEmail,
                Subject = "Cadastro de conta",
                Body = "Obrigado por realizar o cadastro!"
            };

            await _emailService.SendAsync(email);
        }

        private string PasswordEncryption(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.Unicode.GetBytes(password));
            string result = BitConverter.ToString(bytes).Replace("-", string.Empty);

            return result.ToLower();
        }
    }
}