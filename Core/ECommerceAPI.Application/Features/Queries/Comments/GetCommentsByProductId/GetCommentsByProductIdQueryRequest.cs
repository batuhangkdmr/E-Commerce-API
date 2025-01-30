using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Comments.GetCommentsByProductId
{
    public class GetCommentsByProductIdQueryRequest : IRequest<GetCommentsByProductIdQueryResponse>
    {
        public Guid ProductId { get; set; }
    }
}
