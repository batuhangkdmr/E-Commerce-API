using ECommerceAPI.Application.Interfaces.Services;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
    public class AuthorizationEndpointService : IAuthorizationEndpointService
    {
        readonly IReadRepository<Menu> _menuReadRepository;
        readonly IWriteRepository<Menu> _menuWriteRepository;
        readonly IReadRepository<Endpoint> _endpointReadRepository;
        readonly IWriteRepository<Endpoint> _endpointsWriteRepository;
        readonly IApplicationService _applicationService;
        readonly RoleManager<AppRole> _roleManager;

        public AuthorizationEndpointService(IReadRepository<Menu> menuReadRepository, IWriteRepository<Menu> menuWriteRepository, IReadRepository<Endpoint> endpointReadRepository, IWriteRepository<Endpoint> endpointsWriteRepository, IApplicationService applicationService, RoleManager<AppRole> roleManager)
        {
            _menuReadRepository = menuReadRepository;
            _menuWriteRepository = menuWriteRepository;
            _endpointReadRepository = endpointReadRepository;
            _endpointsWriteRepository = endpointsWriteRepository;
            _applicationService = applicationService;
            _roleManager = roleManager;
        }

        public async Task AssignRoleEndpointAsync(string[] roles, string menuName, string code, Type type)
        {
            // Menu nesnesini veritabanından menü adıyla alıyoruz
            Menu _menu = await _menuReadRepository.GetSingleAsync(m => m.Name == menuName);

            if (_menu == null)
            {
                _menu = new Menu
                {
                    Id = Guid.NewGuid(),
                    Name = menuName
                };

                await _menuWriteRepository.AddAsync(_menu);
                await _menuWriteRepository.SaveAsync();
            }

            Endpoint? endpoint = await _endpointReadRepository.Table
                .Include(e => e.Menu)
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menuName);

            if (endpoint == null)
            {
                // Yeni endpoint oluşturuyoruz
                endpoint = new Endpoint
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    ActionType = "GET",
                    HttpType = "GET",
                    Definition = "Admin paneli erişimi",
                    MenuId = _menu.Id,
                    Menu = _menu
                };

                await _endpointsWriteRepository.AddAsync(endpoint);
                await _endpointsWriteRepository.SaveAsync();
            }
        }


        public async Task<List<string>> GetRolesToEndpointAsync(string code, string menu)
        {
            Endpoint? endpoint = await _endpointReadRepository.Table.
                Include(e => e.Menu)
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);
            if(endpoint != null)
                return endpoint.Roles.Select(r => r.Name).ToList();
            return null;
        }
    }
}
