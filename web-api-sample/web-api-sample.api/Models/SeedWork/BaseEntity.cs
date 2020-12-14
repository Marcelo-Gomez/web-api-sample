using System;

namespace web_api_sample.api.Models.SeedWork
{
    public class BaseEntity
    {
        public void NewEntity()
        {
            CreatedAt = DateTime.Now;
            Active = true;
        }

        public int Id { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}