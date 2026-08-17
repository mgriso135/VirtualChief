/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using MySql.Data.MySqlClient;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using Dapper;

/// <summary>
/// Descrizione di riepilogo per Class1
/// </summary>

namespace Dati
{
    public class Dati
    {
        public String Tenant { get { return this._Tenant; } }
        private String _Tenant;

        public Dati()
        {
            //
            // TODO: aggiungere qui la logica del costruttore
            //
        }

        public string GetConnectionString(String tenant)
        {
            /*String activeWorkspace = "";
            if (HttpContext.Current !=null && HttpContext.Current.Session!=null && HttpContext.Current.Session["ActiveWorkspace_Name"]!=null)
            { 
                activeWorkspace = HttpContext.Current.Session["ActiveWorkspace_Name"]?.ToString();
            }
            if (activeWorkspace.Length == 0)
            { 
                var ctx = HttpContext.Current.GetOwinContext(); 
                ClaimsPrincipal user = ctx.Authentication.User;
                var claimsIdentity = user.Identity as ClaimsIdentity;
                activeWorkspace = claimsIdentity?.FindFirst(c => c.Type.Contains("workspace"))?.Value;
            }*/
            string connStr = KIS.App_Code.Secrets.GetConnectionString("VC_MASTERDB_CONN", "masterDB");
            connStr = connStr.Replace("database=", "database=" + tenant);
            return connStr;
        }

        public MySql.Data.MySqlClient.MySqlConnection mycon(String tenant)
        {
            return new MySqlConnection(GetConnectionString(tenant));
        }

        public String GetMainConnectionString()
        {
            string connStr = KIS.App_Code.Secrets.GetConnectionString("VC_VCMAIN_CONN", "vcmain");
            return connStr;
        }

        public MySql.Data.MySqlClient.MySqlConnection VCMainConn()
        {
            return new MySqlConnection(GetMainConnectionString());
        }
        public int getActiveWorkspaceId()
        {
            return KIS.App_Code.WebEnv.ActiveWorkspaceId;
        }

        public String getActiveWorkspaceName()
        {
            return KIS.App_Code.WebEnv.ActiveWorkspaceName;
        }
    }

    public static class Utilities
    {
        /// <summary>
        /// Adds the given number of business days to the <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The date to be changed.</param>
        /// <param name="days">Number of business days to be added.</param>
        /// <returns>A <see cref="DateTime"/> increased by a given number of business days.</returns>
        public static DateTime AddBusinessDays(this DateTime current, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                }
                while (current.DayOfWeek == DayOfWeek.Saturday ||
                    current.DayOfWeek == DayOfWeek.Sunday);
            }
            return current;
        }

        /// <summary>
        /// Subtracts the given number of business days to the <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The date to be changed.</param>
        /// <param name="days">Number of business days to be subtracted.</param>
        /// <returns>A <see cref="DateTime"/> increased by a given number of business days.</returns>
        public static DateTime SubtractBusinessDays(this DateTime current, int days)
        {
            return AddBusinessDays(current, -days);
        }

        public static int GetWeekOfTheYear(DateTime time)
        {
            {
                // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
                // be the same week# as whatever Thursday, Friday or Saturday are,
                // and we always get those right
                DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
                if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                {
                    time = time.AddDays(3);
                }

                // Return the week of our adjusted day
                return CultureInfo
                    .InvariantCulture
                    .Calendar
                    .GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
        }

 
        public static Boolean LogAction(String user, String type /* Page, Controller */, String detail, String querystring, String ipAddr)
        {
            MySqlConnection conn = (new Dati()).VCMainConn();
            conn.Open();
            Boolean ret = false;
            string sql = "INSERT INTO userslog(time, user, type, detail, querystring, ip) VALUES(@time, "
                +"@usr, @type, @detail, @querystring, @ip)";
            try
            {
                conn.Execute(sql, new { time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"), usr = user, type = type, detail = detail, querystring = querystring, ip = ipAddr });
                ret = true;
            }
            catch { }
            conn.Close();
            return ret;
        }

        public static Boolean Syslog(String Tenant, String user, String module, String itemtype, String itemid, String parameter, String oldvalue, String newvalue, String notes="")
        {
            MySqlConnection conn = (new Dati()).mycon(Tenant);
            conn.Open();
            Boolean ret = false;
            string sql = "INSERT INTO syslog(date, user, module, itemtype, parameter, itemid, oldvalue, newvalue, notes) VALUES(@datetime, "
                + "@user, @module, @itemtype, @parameter, @itemid, @oldvalue, @newvalue, @notes)";

            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { datetime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"), user = user, module = module, itemtype = itemtype, parameter = parameter, itemid = itemid, oldvalue = oldvalue, newvalue = newvalue, notes = notes }, tr);
                tr.Commit();
                ret = true;
            }
            catch {
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }
    
        public static String getRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }
    }
}