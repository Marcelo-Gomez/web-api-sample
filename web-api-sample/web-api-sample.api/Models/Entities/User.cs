using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using web_api_sample.api.Models.SeedWork;

namespace web_api_sample.api.Models.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Roles = new List<Role>();
        }

        [Required]
        [MinLength(2)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public List<Role> Roles { get; set; }
    }
}