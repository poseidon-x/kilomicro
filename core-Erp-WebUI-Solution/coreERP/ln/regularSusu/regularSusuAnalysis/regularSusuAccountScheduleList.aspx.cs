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
    public partial class regularSusuAccountScheduleList : System.Web.UI.Page
    {

        private List<getRegularSusuAccountSchedule_Result> data;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtDate.SelectedDate = DateTime.Now;
                Session["getRegularSusuAccountSchedule"] = null;
            }
            else
            {
                if (Session["getRegularSusuAccountSchedule"] != null)
                {
                    data = Session["getRegularSusuAccountSchedule"] as List<getRegularSusuAccountSchedule_Result>;
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
                                    backColor = System.Drawing.Color.Red;
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
            var rent = new coreReports.reportEntities();
            rent.Database.CommandTimeout = 10000;
            Session["getRegularSusuAccountSchedule"] = null;
            data = rent.getRegularSusuAccountSchedule(dtDate.SelectedDate, dtDate.SelectedDate, null)
                .Where(p=> p.plannedContributionDate==dtDate.SelectedDate).ToList();
            Session["getRegularSusuAccountSchedule"] = data;
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
                grid.ExportSettings.FileName = "RegularSusuAccountSchedule_on_" + dtDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + ".pdf";
            }
            if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                grid.ExportSettings.FileName = "RegularSusuAccountSchedule_on_" + dtDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + ".xls";
            }
            if (e.CommandName == RadGrid.ExportToWordCommandName)
            {
                grid.ExportSettings.FileName = "RegularSusuAccountSchedule_on_" + dtDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + ".doc";
            }
        }
    }
}