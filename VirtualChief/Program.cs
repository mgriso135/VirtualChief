using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using VirtualChief.Pages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
// Blazor Server circuits: embedded components inside Razor Pages
// (e.g. Pages/Customers/Customer/List hosts Components/Customer/CustomerList).
builder.Services.AddServerSideBlazor();

// Workspace database gate: a workspace without its own database must not
// serve any page (see Pages/TenantDatabaseGate.cs).
builder.Services.AddSingleton<TenantDatabaseChecker>();

// Authentication:
//  - primary: Auth0 via OpenID Connect, porting the legacy OWIN Startup.cs
//    (virtualchief.eu.auth0.com, scope openid profile email, audience param);
//    the client secret comes from VC_AUTH0_CLIENT_SECRET (Phase 0 secrets policy).
//  - fallback: forms login against the per-tenant users tables (legacy login.aspx),
//    kept for environments without Auth0 connectivity.
// Every page requires an authenticated user (fallback policy); the active
// workspace travels in the "tenant" cookie claim — there is no default tenant.
var auth0Domain = builder.Configuration["Auth0:Domain"] ?? "";
var auth0ClientId = builder.Configuration["Auth0:ClientId"] ?? "";
var auth0Audience = builder.Configuration["Auth0:Audience"] ?? "";
var auth0ClientSecret = builder.Configuration["VC_AUTH0_CLIENT_SECRET"]
    ?? Environment.GetEnvironmentVariable("VC_AUTH0_CLIENT_SECRET") ?? "";

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/login";
        options.AccessDeniedPath = "/Login/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    })
    .AddOpenIdConnect("Auth0", options =>
    {
        options.Authority = $"https://{auth0Domain}";
        options.ClientId = auth0ClientId;
        options.ClientSecret = auth0ClientSecret;
        options.ResponseType = OpenIdConnectResponseType.CodeIdTokenToken;
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.SaveTokens = true;

        // Legacy TokenValidationParameters.NameClaimType = "name"
        options.TokenValidationParameters.NameClaimType = "name";

        options.Events.OnRedirectToIdentityProvider = ctx =>
        {
            // Legacy RedirectToIdentityProvider: pass the API audience so the
            // returned access token targets the Virtual Chief API.
            if (!string.IsNullOrEmpty(auth0Audience))
            {
                ctx.ProtocolMessage.SetParameter("audience", auth0Audience);
            }
            return Task.CompletedTask;
        };

        options.Events.OnRedirectToIdentityProviderForSignOut = ctx =>
        {
            // Legacy RedirectToIdentityProvider (logout branch): terminate the
            // session at Auth0 (/v2/logout) then come back to the app root.
            var postLogoutUri = ctx.Properties?.RedirectUri ?? "/";
            if (postLogoutUri.StartsWith("/"))
            {
                var req = ctx.Request;
                postLogoutUri = $"{req.Scheme}://{req.Host}{req.PathBase}{postLogoutUri}";
            }
            var logoutUri = $"https://{auth0Domain}/v2/logout?client_id={auth0ClientId}"
                + $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
            ctx.Response.Redirect(logoutUri);
            ctx.HandleResponse();
            return Task.CompletedTask;
        };

        options.Events.OnRemoteFailure = ctx =>
        {
            var logger = ctx.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>().CreateLogger("Auth0");
            logger.LogError(ctx.Failure, "Autenticazione Auth0 fallita");
            ctx.Response.Redirect("/Login/login?error=auth0");
            ctx.HandleResponse();
            return Task.CompletedTask;
        };

        options.Events.OnTokenValidated = Auth0TicketHandler.OnTokenValidated;
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Blazor Server circuits hosted by Razor Pages at NESTED urls (e.g.
// /Customers/Customer/List): blazor.server.js resolves its SignalR hub url
// against the document base URI ("<page>/_blazor"), but MapBlazorHub only
// maps the hub at the root. Rewrite page-scoped circuit paths onto it,
// otherwise no @onclick/@bind event ever reaches the components.
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";
    var idx = path.LastIndexOf("/_blazor", StringComparison.Ordinal);
    if (idx > 0)
    {
        var rest = path[idx..];
        if (rest.Length == "/_blazor".Length || rest["/_blazor".Length] == '/')
        {
            context.Request.Path = rest;
        }
    }
    await next();
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Workspace database gate: when the active workspace's tenant database does
// not exist, every request is redirected to /Error — no page may work.
app.UseMiddleware<TenantDatabaseGateMiddleware>();

app.MapRazorPages();
app.MapBlazorHub();

// Keep culture handling identical to legacy Global.asax (per-user language).
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

app.Run();
