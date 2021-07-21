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
    public partial class susuAccountSchedule : System.Web.UI.Page
    {
        private List<AppointmentInfo> data;

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
                    data = Session["getSusuAccountSchedule"] as List<AppointmentInfo>;
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
            var res = rent.getSusuAccountSchedule(dtDate.SelectedDate, dtDate.SelectedDate, null)
                .Where(
                p => p.plannedContributionDate == dtDate.SelectedDate)
                .Where(p => groupId == null || groupId == p.susuGroupNo)
                .Where(p => positionId == null || positionId == p.susuPositionNo)
                .Where(p => gradeId == null || gradeId == p.susuGradeNo)
                .ToList();
            data = new List<AppointmentInfo>();
            var clientList = "";
            int i = 0;
            foreach (var r in res)
            {
                i = i + 1;
                clientList += (i == 1 ? "" : " <br />") + i.ToString() + ". " + r.clientName +
                    " | " + r.susuAccountNo + " | " + (r.balance<=0? "Paid": "Not Paid") + " | " +
                    r.contributionAmount.ToString("#,##0.#0");
            }
            if (clientList != "")
            {
                data.Add(new AppointmentInfo
                {
                    Subject = "Due Contributions (On Course)",
                    Body = clientList,
                    Start = dtDate.SelectedDate.Value.AddHours(8),
                    End = dtDate.SelectedDate.Value.AddHours(17),
                    UserID = 1,
                    RecurrenceParentID = null,
                    RecurrenceRule = ""
                });
            } 
            res = (new reportEntities()).getSusuAccountSchedule(dtDate.SelectedDate, dtDate.SelectedDate, null)
                .Where(
                p => p.balance > 0 && p.plannedContributionDate < dtDate.SelectedDate.Value && p.statusID == 1)
                .Where(p => groupId == null || groupId == p.susuGroupNo)
                .Where(p => positionId == null || positionId == p.susuPositionNo)
                .Where(p => gradeId == null || gradeId == p.susuGradeNo)
                .ToList();
            var res2 = (from r in res
                        select new
                        {
                            r.clientName,
                            r.susuAccountNo,
                            r.contributionAmount
                        }).Distinct().ToList();
            clientList = "";
            i = 0;
            foreach (var r in res2)
            {
                i = i + 1;
                clientList += (i == 1 ? "" : " <br />") + i.ToString() + ". " + r.clientName +
                    " | " + r.susuAccountNo + " | " + r.contributionAmount.ToString("#,##0.#0");
            }
            if (clientList != "")
            {
                data.Add(new AppointmentInfo
                {
                    Subject = "Delayed Contributions",
                    Body = clientList,
                    Start = dtDate.SelectedDate.Value.AddHours(8),
                    End = dtDate.SelectedDate.Value.AddHours(17),
                    UserID = 2,
                    RecurrenceParentID = null,
                    RecurrenceRule = ""
                });
            } 
            res = (new reportEntities()).getSusuAccountSchedule(dtDate.SelectedDate, dtDate.SelectedDate, null)
                .Where(
                p => p.balance > 0 && p.plannedContributionDate < dtDate.SelectedDate.Value && p.statusID == 2)
                .Where(p => groupId == null || groupId == p.susuGroupNo)
                .Where(p => positionId == null || positionId == p.susuPositionNo)
                .Where(p => gradeId == null || gradeId == p.susuGradeNo)
                .ToList();
            var res3 = (from r in res
                        select new
                        {
                            r.clientName,
                            r.susuAccountNo,
                            r.contributionAmount
                        }).Distinct().ToList();
            clientList = "";
            i = 0;
            foreach (var r in res2)
            {
                i = i + 1;
                clientList += (i == 1 ? "" : " <br />") + i.ToString() + ". " + r.clientName +
                    " (" + r.susuAccountNo + "): " + r.contributionAmount.ToString("#,##0.#0");
            }
            if (clientList != "")
            {
                data.Add(new AppointmentInfo
                {
                    Subject = "Defaulted Contributions",
                    Body = clientList,
                    Start = dtDate.SelectedDate.Value.AddHours(8),
                    End = dtDate.SelectedDate.Value.AddHours(17),
                    UserID = 3,
                    RecurrenceParentID = null,
                    RecurrenceRule = ""
                });
            }
            RadScheduler1.SelectedDate = dtDate.SelectedDate.Value;
            RadScheduler1.DataSource = data;
            Session["getSusuAccountSchedule"] = data;
            RadScheduler1.Visible = true;
            RadScheduler1.DataBind();
        }

        protected void RadScheduler1_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {

        }

    }

    public class AppointmentInfo
    {
        private readonly string _id;
        private string _subject;
        private DateTime _start;
        private DateTime _end;
        private string _recurrenceRule;
        private string _recurrenceParentId;
        private string _reminder;
        private int? _userID;
        private string _clientList;

        public string ID
        {
            get
            {
                return _id;
            }
        }

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }
        
        public string Body
        {
            get
            {
                return _clientList;
            }
            set
            {
                _clientList = value;
            }
        }

        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
            }
        }

        public DateTime End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
            }
        }

        public string RecurrenceRule
        {
            get
            {
                return _recurrenceRule;
            }
            set
            {
                _recurrenceRule = value;
            }
        }

        public string RecurrenceParentID
        {
            get
            {
                return _recurrenceParentId;
            }
            set
            {
                _recurrenceParentId = value;
            }
        }

        public int? UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }

        public string Reminder
        {
            get
            {
                return _reminder;
            }
            set
            {
                _reminder = value;
            }
        }

        public AppointmentInfo()
        {
            _id = Guid.NewGuid().ToString();
        }

        public AppointmentInfo(string subject, DateTime start, DateTime end,
             string recurrenceRule, string recurrenceParentID, string reminder, int? userID)
            : this()
        {
            _subject = subject;
            _start = start;
            _end = end;
            _recurrenceRule = recurrenceRule;
            _recurrenceParentId = recurrenceParentID;
            _reminder = reminder;
            _userID = userID;
        }

        public AppointmentInfo(Appointment source)
            : this()
        {
            CopyInfo(source);
        }

        public void CopyInfo(Appointment source)
        {
            Subject = source.Subject;
            Start = source.Start;
            End = source.End;
            RecurrenceRule = source.RecurrenceRule;
            if (source.RecurrenceParentID != null)
            {
                RecurrenceParentID = source.RecurrenceParentID.ToString();
            }

            if (!String.IsNullOrEmpty(Reminder))
            {
                Reminder = source.Reminders[0].ToString();
            }

            Resource user = source.Resources.GetResourceByType("User");
            if (user != null)
            {
                UserID = (int?)user.Key;
            }
            else
            {
                UserID = null;
            }
        }
    }
}