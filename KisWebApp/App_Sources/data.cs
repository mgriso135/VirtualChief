/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Globalization;
using MySql.Data.MySqlClient;

/// <summary>
/// Descrizione di riepilogo per Class1
/// </summary>

namespace Dati
{
    public class Dati
    {
        public Dati()
        {
            //
            // TODO: aggiungere qui la logica del costruttore
            //
        }

        public string GetConnectionString()
        {
            string connStr = String.Format(System.Configuration.ConfigurationManager.ConnectionStrings["masterDB"].ConnectionString);
            return connStr;
        }

        public MySql.Data.MySqlClient.MySqlConnection mycon()
        {
            return new MySqlConnection(GetConnectionString());
        }

        public String GetMainConnectionString()
        {
            string connStr = String.Format(System.Configuration.ConfigurationManager.ConnectionStrings["vcmain"].ConnectionString);
            return connStr;
        }

        public MySql.Data.MySqlClient.MySqlConnection VCMainConn()
        {
            return new MySqlConnection(GetMainConnectionString());
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
            MySqlConnection conn = (new Dati()).mycon();
            conn.Open();
            Boolean ret = false;
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO userslog(time, user, type, detail, querystring, ip) VALUES('" +
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")
                + "', '" + user + "', '"+type+"', '" + detail + "', '" + querystring + "', '"+ipAddr+"')";
            try
            {
                cmd.ExecuteNonQuery();
                ret = true;
            }
            catch { }
            conn.Close();
            return ret;
        }

        public static Boolean Syslog(String user, String module, String itemtype, String itemid, String parameter, String oldvalue, String newvalue, String notes="")
        {
            MySqlConnection conn = (new Dati()).mycon();
            conn.Open();
            Boolean ret = false;
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO syslog(date, user, module, itemtype, parameter, itemid, oldvalue, newvalue, notes) VALUES(@datetime, "
                + "@user, @module, @itemtype, @parameter, @itemid, @oldvalue, @newvalue, @notes)";

            cmd.Parameters.AddWithValue("@datetime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@module", module);
            cmd.Parameters.AddWithValue("@itemtype", itemtype);
            cmd.Parameters.AddWithValue("@parameter", parameter);
            cmd.Parameters.AddWithValue("@itemid", itemid);
            cmd.Parameters.AddWithValue("@oldvalue", oldvalue);
            cmd.Parameters.AddWithValue("@newvalue", newvalue);
            cmd.Parameters.AddWithValue("@notes", notes);

            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch {
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }
    }
}