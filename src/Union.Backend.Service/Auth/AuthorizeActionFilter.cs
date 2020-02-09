using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

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
        public bool ToIgnore { get; set; }
        private readonly PermissionType permission;

        public AuthorizeActionFilter(PermissionType permission)
        {
            this.permission = permission;
        }

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

            if (!ToIgnore && !CheckToken(context.HttpContext.Request, permission))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool CheckToken(HttpRequest req, PermissionType necessary)
        {
            return necessary.Equals(PermissionType.All); //Juste pour tester
            //Règles de validation
            //si necessary est All
            //  retourner true
            //sinon
            //  vérifier que req.Headers.Authorization (token) contient un user valide
            //  si necessary est User
            //      retourner true
            //  si necessary est Admin
            //      retourner si l'user a isAdmin sur true
            //retourner false (fallback)
        }
    }
}
