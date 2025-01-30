using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Users
{
    public class UpdatedUserDTO
    {
        public string Id { get; set; }
        public string NameSurname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Bio { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string PhoneNumber { get; set; }
    }

}
