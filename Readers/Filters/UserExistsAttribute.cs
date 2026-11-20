using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Readers.Core.Contracts;
using Readers.Data.DataModels;

namespace Readers.Web.Filters
{
    public class UserExistsAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Try to get the "id" from the action's arguments (e.g., Details(Guid id))
            if (context.ActionArguments.TryGetValue("id", out object? idObj) && idObj is string UserId)
            {
                UserManager<ApplicationUser> userManager = context.HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>();

                if (userManager != null)
                {
                    ApplicationUser user = await userManager.FindByIdAsync(UserId);
                    if (user == null)
                    {
                        // Short-circuit
                        context.Result = new NotFoundResult();
                        return;
                    }
                }
            }

            await next();
        }
    }
}
