using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Comments
{
    public class CreateCommentDTO
    {
        public string Content { get; set; }
        public Guid ProductId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
    }
}
