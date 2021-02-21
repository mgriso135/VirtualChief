using System;
using System.Collections.Generic;
using KIS.App_Code;
using KIS.Commesse;
using System.Linq;

namespace KIS.Admin
{
    public partial class kisBilling : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptOrdiniMonth.Visible = false;
            rptTaskMonth.Visible = false;
            rptOrdiniYear.Visible = false;
            rptTasksYear.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Billing";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                rptOrdiniMonth.Visible = true;
                rptTaskMonth.Visible = true;
                rptOrdiniYear.Visible = true;
                rptTasksYear.Visible = true;
                if (!Page.IsPostBack)
                {
                    KISConfig kisCfg = new KISConfig(Session["ActiveWorkspace"].ToString());
                    lblExpiryDate.Text = GetLocalResourceObject("lblExpirydate").ToString() +
                        "&nbsp;<b>" + kisCfg.ExpiryDate.ToString("dd/MM/yyyy") +"</b>";


                    ElencoArticoli elArt = new ElencoArticoli(Session["ActiveWorkspace"].ToString(), new DateTime(1970, 1, 1), DateTime.UtcNow.AddMonths(1));
                    var elArtMonth = elArt.ListArticoli.Where(y=>y.DataInserimento>= DateTime.UtcNow.AddMonths(-13))
                        .OrderBy(z=>z.DataInserimento)
                        .GroupBy(x => new
                    {
                        Year = x.DataInserimento.Year,
                        Month = x.DataInserimento.Month
                    }).Select(x => new
                    {
                       Value = x.Count(),
                       Year = x.Key.Year,
                       Month = x.Key.Month
                   });

                    rptOrdiniMonth.DataSource = elArtMonth;
                    rptOrdiniMonth.DataBind();

                    ElencoTaskProduzione elTasks = new ElencoTaskProduzione(Session["ActiveWorkspace"].ToString(), new DateTime(1970, 1, 1), DateTime.UtcNow.AddMonths(1), 'F');
                    var elTasksMonth = elTasks.Tasks.Where(y=>y.DataFineTask >= DateTime.UtcNow.AddMonths(-13))
                        .OrderBy(z => z.DataFineTask)
                        .GroupBy(x => new
                    {
                        Year = x.DataFineTask.Year,
                        Month = x.DataFineTask.Month
                    }).Select(x => new
                    {
                        Value = x.Count(),
                        Year = x.Key.Year,
                        Month = x.Key.Month
                    });
                    rptTaskMonth.DataSource = elTasksMonth;
                    rptTaskMonth.DataBind();

                    var elArtYear = elArt.ListArticoli
                        .OrderBy(z=>z.DataInserimento)
                        .GroupBy(x => new
                    {
                        Year = x.DataInserimento.Year
                    }).Select(x => new
                    {
                        Value = x.Count(),
                        Year = x.Key.Year,
                    });

                    rptOrdiniYear.DataSource = elArtYear;
                    rptOrdiniYear.DataBind();

                    var elTasksYear = elTasks.Tasks.Where(y => y.DataFineTask >= DateTime.UtcNow.AddMonths(-13))
                        .OrderBy(z => z.DataFineTask)
                        .GroupBy(x => new
                        {
                            Year = x.DataFineTask.Year
                        }).Select(x => new
                        {
                            Value = x.Count(),
                            Year = x.Key.Year
                        });
                    rptTasksYear.DataSource = elTasksYear;
                    rptTasksYear.DataBind();
                }
            }
            else
            {

            }
         }
    }
}