using Blazor.WebAssembly.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Blazor.WebAsembly.Services.Extensions
{
    public class CustomClaimsAuthorization
    {
        public static bool ValidateUserClaims(HttpContext context, string claimType, string claimValue)
        {
            var result = context.User.Identity.IsAuthenticated &&
                context.User.Claims.Any(c => c.Type == claimType &&
                c.Value.Contains(claimValue));
            return result;
        }
       
    }
    public class ClaimsAuthorizarionAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizarionAttribute(string claimType,string claimValue) : base(typeof(RequirementClaimFilter))
        {
            Arguments= new object[] {new Claim (claimType, claimValue)};
        }
       
    }
    public class RequirementClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequirementClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
            }
            if (!CustomClaimsAuthorization.ValidateUserClaims(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }

}
