namespace Readers.Models.ViewModels
{
    public class LanguageSwitcherViewModel
    {
        public string CurrentCultureName { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public List<CultureOption> Cultures { get; set; } = new();
    }

    public class CultureOption
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}