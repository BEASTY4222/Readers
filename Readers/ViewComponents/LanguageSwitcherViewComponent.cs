using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Readers.Models.ViewModels;

namespace Readers.ViewComponents
{
    public class LanguageSwitcherViewComponent : ViewComponent
    {
        private readonly IOptions<RequestLocalizationOptions> _locOptions;

        public LanguageSwitcherViewComponent(IOptions<RequestLocalizationOptions> locOptions)
        {
            _locOptions = locOptions;
        }

        public IViewComponentResult Invoke()
        {
            IRequestCultureFeature requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();

            // Preserve the current path AND query string so the user lands back where they were
            string returnUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;

            LanguageSwitcherViewModel model = new LanguageSwitcherViewModel
            {
                CurrentCultureName = requestCulture?.RequestCulture.UICulture.Name ?? "en-US",
                ReturnUrl = returnUrl.ToString(),
                Cultures = _locOptions.Value.SupportedUICultures
                    .Select(c => new CultureOption { Name = c.Name, DisplayName = c.DisplayName })
                    .ToList()
            };

            return View(model);
        }
    }
}