using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttribute;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.Comments.CreateComment;
using ECommerceAPI.Application.Features.Queries.Comments.GetCommentsByProductId;
using ECommerceAPI.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderService _orderService;

        public CommentsController(IMediator mediator, IOrderService orderService)
        {
            _mediator = mediator;
            _orderService = orderService;
        }

        // Yorum ekleme API
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        // Yorumları listeleme API
        [HttpGet("{productId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Get Comments ID")]
        public async Task<IActionResult> GetCommentsByProductId([FromRoute] Guid productId)
        {
            var query = new GetCommentsByProductIdQueryRequest { ProductId = productId };
            var result = await _mediator.Send(query);
            return Ok(result.Comments);
        }

        // Yorum silme API (eğer gerekirse)
        [HttpDelete("{commentId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Delete Comments")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            return NoContent(); // Bu örnekte NoContent döndürülüyor.
        }
    }
}
