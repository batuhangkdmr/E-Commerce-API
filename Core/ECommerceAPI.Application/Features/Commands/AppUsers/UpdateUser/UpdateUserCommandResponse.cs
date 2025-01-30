using ECommerceAPI.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUsers.UpdateUser
{
    public class UpdateUserCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public UpdatedUserDTO UpdatedUser { get; set; } // Güncellenen kullanıcı bilgileri
    }
}
