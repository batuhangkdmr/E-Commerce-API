using ECommerceAPI.Application.DTOs.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Comments.GetCommentsByProductId
{
    public class GetCommentsByProductIdQueryResponse
    {
        public List<CommentDTO> Comments { get; set; }
    }
}
