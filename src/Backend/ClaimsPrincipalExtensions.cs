using AzFnWebinar.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace Backend
{

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal.IsInRole("ProductAdmin") || principal.HasClaim("http://schemas.microsoft.com/2017/07/functions/claims/authlevel", "Admin");
        }
    }
}
