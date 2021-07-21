using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreReports;
using Telerik.Web.UI;

namespace coreERP.ln.regularSusu.analysis
{

    public partial class regularSusuAccountStatusUndisbursed : System.Web.UI.Page
    {


        private List<getRegularSusuAccountStatus_Result> data;
        protected double stageID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["stageID"] != null)
            {
                stageID = double.Parse(Request.Params["stageID"]);
            }
            if (!IsPostBack)
            {
                dtDate.SelectedDate = DateTime.Now;
                Session["getRegularSusuAccountStatus"] = null;
            }
            else
            {
                if (Session["getRegularSusuAccountStatus"] != null)
                {
                    data = Session["getRegularSusuAccountStatus"] as List<getRegularSusuAccountStatus_Result>;
                }
            }
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            if (data != null)
            {
                grid.DataSource = data;
            }
        }

        protected void grid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (Request.Params["color"] == "y")
            {
                var item = e.Item as GridDataItem;
                if (item != null)
                {
                    var keyID = item.GetDataKeyValue("regularSusuAccountID");
                    if (keyID != null)
                    {
                        int id = int.Parse(keyID.ToString());
                        var r = data.FirstOrDefault(p => p.regularSusuAccountID == id);
                        if (r != null)
                        {
                            System.Drawing.Color backColor;
                            System.Drawing.Color foreColor;
                            switch (r.statusID)
                            {
                                case 1:
                                    backColor = System.Drawing.Color.White;
                                    foreColor = System.Drawing.Color.Black;
                                    break;
                                case 2:
                                    backColor = System.Drawing.Color.Red;
                                    foreColor = System.Drawing.Color.White;
                                    break;
                                case 3:
                                    backColor = System.Drawing.Color.Indigo;
                                    foreColor = System.Drawing.Color.White;
                                    break;
                                case 4:
                                    backColor = System.Drawing.Color.Yellow;
                                    foreColor = System.Drawing.Color.Black;
                                    break;
                                case 5:
                                    backColor = System.Drawing.Color.White;
                                    foreColor = System.Drawing.Color.Black;
                                    break;
                                case 6:
                                    backColor = System.Drawing.Color.Green;
                                    foreColor = System.Drawing.Color.Black;
                                    break;
                                case 7:
                                    backColor = System.Drawing.Color.Orange;
                                    foreColor = System.Drawing.Color.Black;
                                    break;
                                case 8:
                                    backColor = System.Drawing.Color.Blue;
                                    foreColor = System.Drawing.Color.White;
                                    break;
                                default:
                                    backColor = System.Drawing.Color.Wheat;
                                    foreColor = System.Drawing.Color.Black;
                                    break;
                            }
                            item.BackColor = backColor;
                            item.ForeColor = foreColor;
                            item.Font.Bold = true;
                        }
                    }
                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            Session["getRegularSusuAccountStatus"] = null;
            var rent = new coreReports.reportEntities();
            rent.Database.CommandTimeout = 10000;
            if (stageID == 1)
            {
                data = rent.getRegularSusuAccountStatus(dtDate.SelectedDate, null)
                    .Where(p => (p.statusID == 1 || p.statusID == 2 || p.statusID == 4))
                    .OrderBy(p => p.startDate)
                    .ThenBy(p => p.clientName)
                    .ToList();
                try
                {
                    grid.MasterTableView.Columns.FindByUniqueName("allContributions").Visible = false;
                    grid.MasterTableView.Columns.FindByUniqueName("disbursementDate").Visible = false;
                }
                catch (Exception) { }
            }
            else if (stageID == 2)
            {
                data = rent.getRegularSusuAccountStatus(dtDate.SelectedDate, null)
                    .Where(p => p.statusID == 6 || p.statusID == 7)
                    .OrderBy(p => p.startDate)
                    .ThenBy(p => p.clientName)
                    .ToList();
            }
            else if (stageID == 1.5)
            {
                data = rent.getRegularSusuAccountStatus(dtDate.SelectedDate, null)
                    .Where(p =>p.statusID == 3)
                    .OrderBy(p => p.startDate)
                    .ThenBy(p => p.clientName)
                    .ToList();
                try
                {
                    grid.MasterTableView.Columns.FindByUniqueName("allContributions").Visible = false;
                    grid.MasterTableView.Columns.FindByUniqueName("disbursementDate").Visible = false;
                }
                catch (Exception) { }
            }
            else
            {
                data = rent.getRegularSusuAccountStatus(dtDate.SelectedDate, null)
                    .OrderBy(p=> p.startDate)
                    .ThenBy(p=> p.clientName)
                    .ToList();
            }
            Session["getRegularSusuAccountStatus"] = data;
            grid.DataSource = data;
            grid.DataBind();
        }

        protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (data != null)
            {
                grid.DataSource = data;
            }
        }

        protected void grid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExportToPdfCommandName)
            {
                grid.ExportSettings.FileName = "RegularSusuAccountStatus_at_" + dtDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + ".pdf";
            }
            if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                grid.ExportSettings.FileName = "RegularSusuAccountStatus_at_" + dtDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + ".xls";
            }
            if (e.CommandName == RadGrid.ExportToWordCommandName)
            {
                grid.ExportSettings.FileName = "RegularSusuAccountStatus_at_" + dtDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + ".doc";
            }
        }

        protected string GetDate(object dt)
        {
            string str = "";

            if (dt != null)
            {
                DateTime d = (DateTime)dt;
                if (d > new DateTime(2005, 1, 1))
                {
                    str = d.ToString("dd-MMM-yyyy");
                }
            }

            return str;
        }

    }
}