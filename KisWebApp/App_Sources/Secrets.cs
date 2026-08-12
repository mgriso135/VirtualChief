using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace KIS.App_Code
{
    /// <summary>
    /// Central secret reader. Values are taken from environment variables first
    /// (VC_*), falling back to the application configuration. No real secret is
    /// ever committed to the repository: config files hold placeholders only.
    /// </summary>
    public static class Secrets
    {
        /// <summary>Reads an appSettings-style value from an env var, else from config.</summary>
        public static string Get(string envName, string configKey)
        {
            string v = Environment.GetEnvironmentVariable(envName);
            if (!string.IsNullOrEmpty(v))
            {
                return v;
            }

            if (!string.IsNullOrEmpty(configKey))
            {
                string c = ConfigurationManager.AppSettings[configKey];
                if (!string.IsNullOrEmpty(c))
                {
                    return c;
                }
            }

            return "";
        }

        /// <summary>Reads a connection string from an env var, else from config.</summary>
        public static string GetConnectionString(string envName, string configName)
        {
            string v = Environment.GetEnvironmentVariable(envName);
            if (!string.IsNullOrEmpty(v))
            {
                return v;
            }

            if (!string.IsNullOrEmpty(configName))
            {
                ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[configName];
                if (cs != null && !string.IsNullOrEmpty(cs.ConnectionString))
                {
                    return cs.ConnectionString;
                }
            }

            return "";
        }

        /// <summary>SMTP credentials (VC_SMTP_USER / VC_SMTP_PASS, else config).</summary>
        public static NetworkCredential GetSmtpCredentials()
        {
            string user = Get("VC_SMTP_USER", "smtpUsername");
            string pass = Get("VC_SMTP_PASS", "smtpPassword");
            return new NetworkCredential(user, pass);
        }

        /// <summary>Applies SMTP credentials to the given SmtpClient.</summary>
        public static SmtpClient ConfigureSmtp(SmtpClient smtpcli)
        {
            smtpcli.Credentials = GetSmtpCredentials();
            return smtpcli;
        }
    }
}