using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages support
builder.Services.AddRazorPages();

// Add MVC support for .aspx pages via legacy handler
builder.Services.AddControllersWithViews();

// Configure the app
var app = builder.Build();

// Configure request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Map Razor Pages
app.MapRazorPages();

// MapControllers for API support
app.MapControllers();

// Map fallback for .aspx pages - serve them as content
app.MapFallbackToFile("/{*path}", "index.html");

// Run the app
app.Run();
