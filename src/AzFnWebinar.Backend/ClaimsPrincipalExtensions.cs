using System.Security.Claims;

namespace AzFnWebinar.Backend
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal.HasClaim("roles", "ProductAdmin") ||
                principal.HasClaim("http://schemas.microsoft.com/2017/07/functions/claims/authlevel", "Admin");
        }
    }
}
