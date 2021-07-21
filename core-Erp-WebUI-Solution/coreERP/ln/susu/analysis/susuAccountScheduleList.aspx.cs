using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreReports;
using Telerik.Web.UI;

namespace coreERP.ln.susu.analysis
{
    public partial class susuAccountScheduleList : System.Web.UI.Page
    {
        private List<getSusuAccountSchedule_Result> data;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtDate.SelectedDate = DateTime.Now;
                Session["getSusuAccountSchedule"] = null;
                coreLogic.IcoreLoansEntities le = new coreLogic.coreLoansEntities();
                cboPosition.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.susuPositions.OrderBy(p => p.susuPositionName).ToList())
                {
                    cboPosition.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuPositionName, r.susuPositionNo.ToString()));
                }
                cboGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.susuGroups.OrderBy(p => p.susuGroupName).ToList())
                {
                    cboGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuGroupName, r.susuGroupNo.ToString()));
                }
                cboGrade.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.susuGrades.OrderBy(p => p.susuGradeName).ToList())
                {
                    cboGrade.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuGradeName, r.susuGradeNo.ToString()));
                }
            }
            else
            {
                if (Session["getSusuAccountSchedule"] != null)
                {
                    data = Session["getSusuAccountSchedule"] as List<getSusuAccountSchedule_Result>;
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
                    var keyID = item.GetDataKeyValue("susuAccountID");
                    if (keyID != null)
                    {
                        int id = int.Parse(keyID.ToString());
                        var r = data.FirstOrDefault(p => p.susuAccountID == id);
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
            int? gradeId = null;
            int? positionId = null;
            int? groupId = null;
            if (cboGrade.SelectedValue != "")
            {
                gradeId = int.Parse(cboGrade.SelectedValue);
            }
            if (cboPosition.SelectedValue != "")
            {
                positionId = int.Parse(cboPosition.SelectedValue);
            }
            if (cboGroup.SelectedValue != "")
            {
                groupId = int.Parse(cboGroup.SelectedValue);
            }
            Session["getSusuAccountSchedule"] = null;
            var rent = new coreReports.reportEntities();
            rent.Database.CommandTimeout = 10000;
            data = rent.getSusuAccountSchedule(dtDate.SelectedDate, dtDate.SelectedDate, null)
                .Where(p => p.plannedContributionDate == dtDate.SelectedDate)
                .Where(p => groupId == null || groupId == p.susuGroupNo)
                .Where(p => positionId == null || positionId == p.susuPositionNo)
                .Where(p => gradeId == null || gradeId == p.susuGradeNo)
                .ToList();
            Session["getSusuAccountSchedule"] = data;
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
                grid.ExportSettings.FileName = "susuAccountSchedule_on_" + dtDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + ".pdf";
            }
            if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                grid.ExportSettings.FileName = "susuAccountSchedule_on_" + dtDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + ".xls";
            }
            if (e.CommandName == RadGrid.ExportToWordCommandName)
            {
                grid.ExportSettings.FileName = "susuAccountSchedule_on_" + dtDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + ".doc";
            }
        }
    }
}