using System.ComponentModel.DataAnnotations;
using web_api_sample.api.Models.SeedWork;

namespace web_api_sample.api.Models.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [MinLength(2)]
        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}