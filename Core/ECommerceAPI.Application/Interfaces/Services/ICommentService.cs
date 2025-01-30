using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task<CreateCommentResponseDTO> AddCommentAsync(CreateCommentDTO model); // Yorum ekleme
        Task<List<CommentDTO>> GetCommentsByProductIdAsync(Guid productId); // Ürüne ait yorumları listeleme
        Task DeleteCommentAsync(Guid commentId); // Yorum silme
    }
}
