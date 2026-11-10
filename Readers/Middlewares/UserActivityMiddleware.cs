using System.Security.Claims;

namespace Readers.Web.Middlewares
{
    public class UserActivityMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<UserActivityMiddleware> Logger;

        // RequestDelegate is the next piece of middleware in the pipeline
        public UserActivityMiddleware(RequestDelegate next, ILogger<UserActivityMiddleware> logger)
        {
            Next = next;
            Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Will not track annonymous users, only authenticated users
            if (context.User.Identity?.IsAuthenticated == true)
            {
                // 2. Extract their UserId from the cookie claims
                string? userId = context.User.FindFirstValue(ClaimTypes.Email);
                string path = context.Request.Path;
                string method = context.Request.Method;
                DateTime timestamp = DateTime.UtcNow;

                // using the loger now instead of saving to the database, it would fill it with gunk
                Logger.LogInformation("{Timestamp}---User {UserId} accessed {Method} {Path} ---ACTIVITY_TRACKER", timestamp, userId, method, path);
            }

            // Send the request to the next middleware (or the MVC router)
            await Next(context);
        }
    }
}