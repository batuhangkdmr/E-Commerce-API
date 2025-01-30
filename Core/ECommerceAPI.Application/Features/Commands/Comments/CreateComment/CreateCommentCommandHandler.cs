using ECommerceAPI.Application.Interfaces.Services;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Comments.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
    {
        private readonly IWriteRepository<Comment> _commentWriteRepository;

        public CreateCommentCommandHandler(IWriteRepository<Comment> commentWriteRepository)
        {
            _commentWriteRepository = commentWriteRepository;
        }

        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommandRequest request, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                Content = request.Content,
                ProductId = request.ProductId,
                UserId = request.UserId,
                Rating = request.Rating
            };

            await _commentWriteRepository.AddAsync(comment);
            await _commentWriteRepository.SaveAsync();

            return new CreateCommentCommandResponse
            {
                CommentId = comment.Id
            };
        }
    }
}
