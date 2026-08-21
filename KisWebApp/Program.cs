using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
// NOTE: Pages/Index.cshtml is served at the site root ("/") automatically
// by the Razor Pages routing convention - no AddPageRoute needed.

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Legacy static assets (Content/, Styles/, Scripts/, img/) live outside
// wwwroot; expose them without duplicating the files.
var legacyAssetDirs = new[] { "Content", "Styles", "Scripts", "img" };
foreach (var dir in legacyAssetDirs)
{
    var fullPath = Path.Combine(app.Environment.ContentRootPath, dir);
    if (Directory.Exists(fullPath))
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(fullPath),
            RequestPath = "/" + dir
        });
    }
}

app.UseRouting();

app.MapRazorPages();
app.MapControllers();

app.Run();
