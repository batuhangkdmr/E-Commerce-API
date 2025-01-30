using ECommerceAPI.Application.CustomAttribute;
using ECommerceAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerceAPI.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userName = context.HttpContext.User.Identity?.Name;
            var userRoles = context.HttpContext.User.Claims
                               .Where(c => c.Type == ClaimTypes.Role)
                               .Select(c => c.Value)
                               .ToList();

            if (!string.IsNullOrEmpty(userName))
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var attribute = descriptor?.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                var httpAttribute = descriptor?.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                if (attribute != null)
                {
                    // Generate the code used for checking permissions
                    var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";

                    // Check if the user has the appropriate role or permission to access this endpoint
                    var hasPermission = await _userService.HasRolePermissionToEndpointAsync(userName, code);

                    // Alternatively, check if the user's roles contain an expected role
                    var hasRequiredRole = attribute.RequiredRoles == null ||
                                          attribute.RequiredRoles.Any(role => userRoles.Contains(role));

                    if (!hasPermission && !hasRequiredRole)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }

                await next();
            }
            else
            {
                await next();
            }
        }
    }
}
