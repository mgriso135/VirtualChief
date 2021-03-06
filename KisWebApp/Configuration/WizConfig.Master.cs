using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Sources;

namespace KIS.Configuration
{
    public partial class WizConfig : System.Web.UI.MasterPage
    {
        public String section;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ActiveWorkspace_Name"] != null)
            {
                UserAccount usr = new UserAccount(Session["ActiveWorkspace_Name"].ToString());
                Workspace ws = new Workspace(Session["ActiveWorkspace_Name"].ToString());
                if(usr!=null && usr.id!=-1)
                {
                    usr.loadGroups(ws.id);
                    Boolean isAdmin = false;
                    try
                    {
                        var idgrp = usr.groups.First(y => y.Nome == "Admin");
                        isAdmin = true;
                    }
                    catch(Exception ex)
                    {
                        isAdmin = false;
                    }
                    if(isAdmin)
                    { 
                        liLogo.Attributes.Remove("class");
                        liReparto.Attributes.Remove("class");
                        liPostazioni.Attributes.Remove("class");
                        liUtenti.Attributes.Remove("class");
                        liAdmin.Attributes.Remove("class");
                        liTimezone.Attributes.Remove("class");

                        switch (section)
                        {
                            case "Admin":
                                liAdmin.Attributes.Add("class", "active");
                                break;
                            case "TimeZone":
                                liTimezone.Attributes.Add("class", "active");
                                break;
                            case "Logo":
                                liLogo.Attributes.Add("class", "active");
                                break;
                            case "Reparti":
                                liReparto.Attributes.Add("class", "active");
                                break;
                            case "Postazioni":
                                liPostazioni.Attributes.Add("class", "active");
                                break;
                            case "Utenti":
                                liUtenti.Attributes.Add("class", "active");
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
            else
            {

            }
        }
    }
}