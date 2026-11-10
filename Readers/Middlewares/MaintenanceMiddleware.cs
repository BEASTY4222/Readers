using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Readers.Web.Middlewares
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly IConfiguration Config;

        public MaintenanceMiddleware(RequestDelegate next, IConfiguration config)
        {
            Next = next;
            Config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Read from appsettings.json
            bool isMaintenanceMode = Config.GetValue<bool>("MaintenanceMode");

            if (isMaintenanceMode)
            {
                bool isAdmin = context.User.IsInRole("Admin");
                // This is a temporary band-aid because I dont know how roles work
                //bool isLocal = context.Connection.RemoteIpAddress?.ToString() == "127.0.0.1" || context.Connection.RemoteIpAddress?.ToString() == "::1";
                if (context.Request.Path.StartsWithSegments("/Maintenance"))
                {
                    await Next(context);
                    return;
                }
                    
                if (!isAdmin)
                {
                    // Redirect to the view
                    context.Response.Redirect("/Maintenance");
                    return;
                }
            }

            // If not in maintenance mode (or user is Admin), continue normally
            await Next(context);
        }
    }
}