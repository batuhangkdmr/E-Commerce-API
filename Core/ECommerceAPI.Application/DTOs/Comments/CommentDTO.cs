using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Comments
{
    public class CommentDTO
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public string UserName { get; set; }
    }
}
