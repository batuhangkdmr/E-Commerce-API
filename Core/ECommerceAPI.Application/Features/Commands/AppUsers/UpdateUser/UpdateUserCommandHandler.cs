using AutoMapper;
using ECommerceAPI.Application.DTOs.Users;
using ECommerceAPI.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUsers.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
    {
        readonly IUserService _userService;
        readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            // Kullanıcı güncelleme işlemini gerçekleştiriyoruz
            var updateUserDTO = _mapper.Map<UpdatedUserDTO>(request);
            var updatedUser = await _userService.UpdateUserAsync(request.Id, updateUserDTO);

            // Güncelleme sonucu, bir UpdateUserCommandResponse olarak döndürülüyor
            return _mapper.Map<UpdateUserCommandResponse>(updatedUser);
        }
    }

}
