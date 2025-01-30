using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public string NameSurname { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public ICollection<Basket> Baskets { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

     
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedDate { get; set; }
        public string? Bio { get; set; }
    }
}
