using AspNetCoreWebRazor01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebRazor01.Authorization
{
    public class MovieAdministratorAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, Movie>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Movie resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Constants.MovieAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
