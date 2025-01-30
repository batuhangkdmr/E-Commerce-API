using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Domain.Entities.Common;
using ECommerceAPI.Domain.Entities.Identity;

namespace ECommerceAPI.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public int Rating { get; set; } // Kullanıcının verdiği oy

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
