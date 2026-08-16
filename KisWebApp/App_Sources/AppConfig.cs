/* Copyright © 2026 -  Tutti i diritti riservati */

namespace KIS.App_Code
{
    /// <summary>
    /// Typed, env-first reader for the application settings. The keys mirror the
    /// Web.config appSettings exactly; every value is overridden by its VC_*
    /// environment variable (same precedence as Secrets). No secret is stored
    /// here and nothing is ever committed with a real value.
    /// </summary>
    public static class AppConfig
    {
        /// <summary>SMTP account used by delays/warnings/license/quality e-mails.</summary>
        public static string SmtpUsername => Secrets.Get("VC_SMTP_USER", "smtpUsername");
        public static string SmtpPassword => Secrets.Get("VC_SMTP_PASS", "smtpPassword");

        public static string Auth0ClientId => Secrets.Get("VC_AUTH0_CLIENT_ID", "auth0:ClientId");
        public static string Auth0ClientSecret => Secrets.Get("VC_AUTH0_CLIENT_SECRET", "auth0:ClientSecret");
        public static string Auth0Domain => Secrets.Get("VC_AUTH0_DOMAIN", "auth0:Domain");
        public static string Auth0Audience => Secrets.Get("VC_AUTH0_AUDIENCE", "auth0:Audience");
        public static string Auth0RedirectUri => Secrets.Get("VC_AUTH0_REDIRECT_URI", "auth0:RedirectUri");
        public static string Auth0PostLogoutRedirectUri => Secrets.Get("VC_AUTH0_POST_LOGOUT_REDIRECT_URI", "auth0:PostLogoutRedirectUri");

        /// <summary>API key for the customer test endpoints (config placeholder, no secret).</summary>
        public static string CustomersTestApiKey => Secrets.Get("VC_CUSTOMERS_TEST_API_KEY", "customersTestApiKey");
    }
}