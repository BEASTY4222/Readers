using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Readers.Web.Filters
{
    public class TrackExecutionTimeAttribute : ActionFilterAttribute
    {
        private Stopwatch Stopwatch = new Stopwatch();
        private ILogger Logger = null!;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Use the built-in ILogger from the request pipeline
            Logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<TrackExecutionTimeAttribute>>();

            Stopwatch.Start();

            await next();

            Stopwatch.Stop();

            string? actionName = context.ActionDescriptor.DisplayName;
            Logger.LogInformation("---Action {ActionName} took {ElapsedMilliseconds} ms to load.---TIME_TAKEN",
                actionName, Stopwatch.ElapsedMilliseconds);
        }
    }
}