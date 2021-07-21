using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Upload;
using coreLogic;

namespace coreERP.hc.payroll
{
    public partial class post : System.Web.UI.Page
    {
        PayProcessor payprocessor = new PayProcessor();
        private String[] months = new String[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        coreLoansEntities le = new coreLoansEntities();
        core_dbEntities ent = new core_dbEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Do not display SelectedFilesCount progress indicator.
                //RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;

                cboPayCalendar.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.payCalendars.OrderByDescending(p => p.year).ThenByDescending(p => p.month))
                {
                    cboPayCalendar.Items.Add(new RadComboBoxItem(r.year.ToString()
                        + ", " + months[r.month-1], r.payCalendarID.ToString()));
                }
            }

            //RadProgressArea1.Localization.Uploaded = "Total Progress";
            //RadProgressArea1.Localization.UploadedFiles = "Progress";
            //RadProgressArea1.Localization.CurrentFileName = "Posting Staff Pay: ";
        }

        protected void btnProcessPay_Click(object sender, EventArgs e)
        {
            UpdateProgressContext();            
        }

        private void UpdateProgressContext()
        {
            if (cboPayCalendar.SelectedValue != "")
            {
                int id = int.Parse(cboPayCalendar.SelectedValue);
                var calendar = le.payCalendars.FirstOrDefault(p => p.payCalendarID == id);
                if (calendar != null && calendar.isPosted==false)
                {
                    var staffs = le.staffs.Where(p => p.employmentStatusID == 1).ToList();
                    //RadProgressContext progress = RadProgressContext.Current;
                    //progress.Speed = "N/A";

                    var startTime = DateTime.Now;
                    for (int i = 0; i < staffs.Count; i++)
                    {
                        //progress.PrimaryTotal = 1;
                        //progress.PrimaryValue = 1;
                        //progress.PrimaryPercent = 100;

                        //progress.SecondaryTotal = staffs.Count;
                        //progress.SecondaryValue = i;
                        //progress.SecondaryPercent = ((double)i/(double)staffs.Count )*100.0;

                        //progress.CurrentOperationText = staffs[i].surName + ", " + staffs[i].otherNames;

                        //if (!Response.IsClientConnected)
                        //{
                        //    //Cancel button was clicked or the browser was closed, so stop processing
                        //    break;
                        //}

                        //var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                        //progress.TimeEstimated = ((staffs.Count - i)/(staffs.Count))*elapsed;
                        //Stall the current thread for 0.1 seconds

                        payprocessor.PostPay(staffs[i].staffID, calendar, le, ent, User.Identity.Name);
                    }
                    calendar.isPosted = true;
                    ent.SaveChanges();
                    le.SaveChanges();
                    HtmlHelper.MessageBox("Payroll successfully posted");
                }
                else
                {
                    HtmlHelper.MessageBox2("Selected Pay Calendar Month has been posted and cannot be reposted",
                        ResolveUrl("~/hc/payroll/post.aspx"), "coreERP©: Successful", IconType.ok);
                }
            }
        }     
    }
}