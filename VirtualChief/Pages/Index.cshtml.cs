using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KisApp.App_Sources;

namespace VirtualChief.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string TenantName { get; set; }

        public void OnGet()
        {
            // Show the tenant addition form
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(TenantName))
                {
                    try
                    {
                        // Create a new analysis service for the tenant
                        var analysis = new KisApp.App_Sources.Analysis(TenantName);
                        return Content($"Success: Tenant '{TenantName}' created");
                    }
                    catch (Exception ex)
                    {
                        return Content($"Error: {ex.Message}");
                    }
                }
            }
            return Page();
        }
    }
}