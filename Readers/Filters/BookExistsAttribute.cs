using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Readers.Core.Contracts;

namespace Readers.Web.Filters
{
    public class BookExistsAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Try to get the "id" from the action's arguments (e.g., Details(Guid id))
            if (context.ActionArguments.TryGetValue("id", out object? idObj) && idObj is Guid bookId)
            {
                IBookService? bookService = context.HttpContext.RequestServices.GetService<IBookService>();

                if (bookService != null)
                {
                    var book = await bookService.GetByIdAsync(bookId);

                    if (book == null)
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