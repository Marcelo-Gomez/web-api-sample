using System.Collections.Generic;

namespace web_api_sample.api.Models.Dtos
{
    public class UserDto
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public List<RoleDto> Roles { get; set; }
    }
}