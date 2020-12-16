using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using MvcApplication.Support;
using Owin;
using System.Security.Principal;

[assembly: OwinStartup(typeof(KIS.App_Sources.Startup))]

namespace KIS.App_Sources
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Auth0 parameters
            string auth0Domain = ConfigurationManager.AppSettings["auth0:Domain"];
            string auth0ClientId = ConfigurationManager.AppSettings["auth0:ClientId"];
            string auth0ClientSecret = ConfigurationManager.AppSettings["auth0:ClientSecret"];
            string auth0Audience = ConfigurationManager.AppSettings["auth0:Audience"];
            string auth0RedirectUri = ConfigurationManager.AppSettings["auth0:RedirectUri"];
            string auth0PostLogoutRedirectUri = ConfigurationManager.AppSettings["auth0:PostLogoutRedirectUri"];

            // Set Cookies as default authentication type
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/Account/Login"),
                CookieSameSite = SameSiteMode.Lax
            });

            // Configure Auth0 authentication
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "Auth0",

                Authority = $"https://{auth0Domain}",

                ClientId = auth0ClientId,
                ClientSecret = auth0ClientSecret,

                RedirectUri = auth0RedirectUri,
                PostLogoutRedirectUri = auth0PostLogoutRedirectUri,

                ResponseType = OpenIdConnectResponseType.CodeIdTokenToken,
                Scope = "openid profile email",

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                },

                CookieManager = new SameSiteCookieManager(new SystemWebCookieManager()),

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = notification =>
                    {
                        
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", notification.ProtocolMessage.IdToken));
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim("access_token", notification.ProtocolMessage.AccessToken));
                        
                        String usrID = "";
                        foreach(var clm in notification.AuthenticationTicket.Identity.Claims)
                        {
                            if(clm.Type.Contains("nameidentifier"))
                            {
                                usrID = clm.Value;
                            }
                        }
                        String workspace = "";
                        if(usrID.Length > 0)
                        {
                            UserAccount curr = new UserAccount(usrID);
                            if(curr.id!=-1)
                            {
                                workspace = "123prova";
                            }


                        }
                        string workspace2 = "NOT FOUND " + notification.AuthenticationTicket.Properties.Dictionary.Count + "; ";
                        foreach(var m in notification.AuthenticationTicket.Properties.Dictionary)
                        {
                            workspace2 += m.Key + "=" + m.Value + "; ";
                            if(m.Key == "Workspace")
                            {
                                workspace2 = m.Value;
                            }
                        }
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim("workspace", workspace));

                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
                        {
                            // The context's ProtocolMessage can be used to pass along additional query parameters
                            // to Auth0's /authorize endpoint.
                            // 
                            // Set the audience query parameter to the API identifier to ensure the returned Access Tokens can be used
                            // to call protected endpoints on the corresponding API.
                            
                            notification.ProtocolMessage.SetParameter("audience", auth0Audience);
                        }
                        else if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                        {
                            var logoutUri = $"https://{auth0Domain}/v2/logout?client_id={auth0ClientId}";

                            var postLogoutUri = notification.ProtocolMessage.PostLogoutRedirectUri;
                            if (!string.IsNullOrEmpty(postLogoutUri))
                            {
                                if (postLogoutUri.StartsWith("/"))
                                {
                                    // transform to absolute
                                    var request = notification.Request;
                                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                                }
                                logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                            }

                            notification.Response.Redirect(logoutUri);
                            notification.HandleResponse();
                        }
                        return Task.FromResult(0);
                    }
                }
            });

            
        }
    }
}
