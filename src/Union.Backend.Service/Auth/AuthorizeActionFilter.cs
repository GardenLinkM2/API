using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using Union.Backend.Model;
using Union.Backend.Model.DAO;
using Union.Backend.Service.Dtos;
using static Union.Backend.Service.Auth.Utils;

namespace Union.Backend.Service.Auth
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(PermissionType permission = PermissionType.User)
            : base(typeof(AuthorizeActionFilter))
        {
            Order = (int)permission;
            Arguments = new object[] { permission };
        }
    }

    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly GardenLinkContext db;
        private readonly IOptions<AuthSettings> auth;
        private readonly PermissionType permission;

        public AuthorizeActionFilter(
            GardenLinkContext gardenLinkContext,
            IOptions<AuthSettings> auth, 
            PermissionType permission)
        {
            db = gardenLinkContext;
            this.auth = auth;
            this.permission = permission;
        }

        public bool ToIgnore { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.Filters
                .ToList()
                .ForEach(f =>
                {
                    if (f is AuthorizeActionFilter fa && f != this)
                    {
                        fa.ToIgnore = true;
                    }
                });

            if (permission.Equals(PermissionType.All))
                return;

            if (!ToIgnore)
            {
                CheckToken(context, permission);
            }
        }

        private void CheckToken(AuthorizationFilterContext context, PermissionType necessary)
        {
            try
            {
                context.HttpContext.Request.Headers.TryGetValue(HttpRequestHeader.Authorization.ToString(), out var token);
                var accessToken = ValidateAndGetToken<TokenDto>(token, auth.Value.BackSecret);
                if (!db.Users.Any(u => u.Id.Equals(new Guid(accessToken.Uuid))))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var granted = (accessToken.IsAdmin ?? false) ? PermissionType.Admin : PermissionType.User;
                if ((int)granted > (int)necessary)
                    context.Result = new StatusCodeResult(403);
            }
            catch (ArgumentException)
            {
                context.Result = new UnauthorizedResult();
            }
            catch (Exception)
            {
                context.Result = new StatusCodeResult(500);
            }
        }
    }
}
