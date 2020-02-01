using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Union.Backend.API
{
    public enum PermissionType
    {
        Admin,
        User,
        All
    }

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
        public bool ToIgnore { get; set; }
        private readonly PermissionType permission;

        public AuthorizeActionFilter(PermissionType permission)
        {
            this.permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.Filters
                .Where(item => item is AuthorizeActionFilter && item != this)
                .ToList()
                .ForEach(f => ((AuthorizeActionFilter)f).ToIgnore = true);

            if (!ToIgnore && !CheckToken(context.HttpContext.Request, permission))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool CheckToken(HttpRequest req, PermissionType permission)
        {
            return permission.Equals(PermissionType.All); //Juste pour tester
        }
    }
}
