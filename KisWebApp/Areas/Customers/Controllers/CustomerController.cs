using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;
using System.Net.Mail;
using KIS.App_DB;
using System.Configuration;

namespace KIS.Areas.Customers.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customers/Customer

        /*Returns:
         * 0 if all is ok
         * 2 if customer does not exist
         * 3 if mail is incorrect, but contact has been correctly added
         * 4 if user already exists
         * 5 if Group CustomerUser is not found
         */
        [Authorize]
        public int addCustomerContact(String customer, String FirstName, String LastName, String Role,
            Boolean createUser, String username, String password, String password2, String language, String mailAddress)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Customers/Customer/addCustomerContact", "Customer="+customer, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Customers/Customer/addCustomerContact", "Customer = "+customer, ipAddr);
            }


            int ret = 0;
            Cliente cst = new Cliente(Session["ActiveWorkspace_Name"].ToString(), customer);
            if ((cst == null || cst.CodiceCliente.Length == 0) && ret == 0)
            {
                ret = 2;
            }
            UserAccount usr = new UserAccount(Session["ActiveWorkspace_Name"].ToString());
            if(usr.id != -1 && ret == 0)
            {
                ret = 4;
            }
            System.Net.Mail.MailAddress mail = null;
            if(ret == 0 && createUser)
            { 
            try
            {
                mail = new System.Net.Mail.MailAddress(mailAddress);
            }
            catch
            {
                ret = 3;
            }
            }
            Group grp = new Group("CustomerUser");
            if((grp == null || grp.ID == -1) && ret == 0)
            {
                ret = 5;
            }

            if (ret == 0)
            { 

            int contactID = cst.AddContatto(FirstName, LastName, Role);
                if(contactID !=-1 && mail!=null && mail.Address.Length > 0)
                {
                    Contatto cont = new Contatto(Session["ActiveWorkspace_Name"].ToString(), contactID);
                        cont.addEmail(mail, "Default");
                        /*if(createUser && password.Length>0 && password == password2)
                        {
                            UserAccount curr = new UserAccount(Session["ActiveWorkspace_Name"].ToString());
                            KISConfig cfg = new KISConfig(Session["ActiveWorkspace_Name"].ToString());
                            String checkUsrAdd = curr.add(username, password, FirstName, LastName, "User", cfg.Language,
                                false, mail);
                        curr = new UserAccount(username);
                        Boolean checkAddGrp = curr.addGruppo(grp);
                        curr.addEmail(mail.Address, "Default", true);

                        // Send e-mail
                        String baseURL = cfg.baseUrl + cfg.basePath;

                        System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();
                        mMessage.From = new MailAddress("virtualchief@kaizenkey.com", "Virtual Chief");
                        mMessage.To.Add(new MailAddress(mail.Address, FirstName + " " + LastName));
                        mMessage.Subject = "[Virtual Chief] " + ResCustomer.addCustomerContact.lblSubject;
                        mMessage.IsBodyHtml = true;

                        mMessage.Body = "<html><body><div>"
                            + ResCustomer.addCustomerContact.lblMail1
                            + " <a href=\"" + baseURL + "/Users/Users/ActivateUser?usr=" + username + "&checksum=" + checkUsrAdd
                            + "\" target=\"_blank\">"
                            + ResCustomer.addCustomerContact.lblMail2
                            + "</a> "
                            + ResCustomer.addCustomerContact.lblMail3
                            + " " + baseURL + "/Users/Users/ActivateUser?usr=" + username + "&checksum=" + checkUsrAdd
                            +"<br />"
                            + "<br />"
                            + ResCustomer.addCustomerContact.lblMail4
                            + "</div></body></html>";
                        try
                        {
                            SmtpClient smtpcli = new SmtpClient();
                            smtpcli.Send(mMessage);
                        }
                        catch { }
                    }*/
                    }
                }
            return ret;
        }
    }
}