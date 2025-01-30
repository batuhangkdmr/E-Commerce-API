using ECommerceAPI.Application.DTOs.Comments;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Comments.GetCommentsByProductId
{
    public class GetCommentsByProductIdQueryHandler : IRequestHandler<GetCommentsByProductIdQueryRequest, GetCommentsByProductIdQueryResponse>
    {
        private readonly IReadRepository<Comment> _commentReadRepository;

        public GetCommentsByProductIdQueryHandler(IReadRepository<Comment> commentReadRepository)
        {
            _commentReadRepository = commentReadRepository;
        }

        public async Task<GetCommentsByProductIdQueryResponse> Handle(GetCommentsByProductIdQueryRequest request, CancellationToken cancellationToken)
        {
            var comments = await _commentReadRepository.Table
                .Where(c => c.ProductId == request.ProductId)
                .Select(c => new CommentDTO
                {
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    Rating = c.Rating,
                    UserName = c.User.NameSurname
                })
                .ToListAsync();

            return new GetCommentsByProductIdQueryResponse { Comments = comments };
        }
    }
}
