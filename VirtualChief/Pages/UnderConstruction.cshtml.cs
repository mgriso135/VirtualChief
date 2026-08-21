using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VirtualChief.Pages
{
    /// <summary>
    /// Placeholder shown by menu entries whose legacy WebForms/MVC page has not
    /// been migrated to Razor yet. The menu mirrors the group configuration
    /// faithfully; this page makes the migration state visible instead of
    /// hiding entries.
    /// </summary>
    public class UnderConstructionModel : PageModel
    {
        public string Title { get; set; } = "Sezione in migrazione";

        public string LegacyUrl { get; set; } = "";

        public void OnGet(string title, string legacy)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Title = System.Net.WebUtility.HtmlEncode(title);
            }
            if (!string.IsNullOrEmpty(legacy))
            {
                LegacyUrl = System.Net.WebUtility.HtmlEncode(legacy);
            }
        }
    }
}
