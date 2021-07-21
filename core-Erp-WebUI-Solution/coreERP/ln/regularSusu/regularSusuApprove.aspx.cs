using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;

namespace coreERP.ln.susu
{
    public partial class regularSusuApprove : System.Web.UI.Page
    {
        private coreLoansEntities le = new coreLoansEntities();
        private coreLogic.regularSusuAccount sa;
        private coreLogic.client client;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtApproval.SelectedDate=DateTime.Now;
                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " ("
                            + r.staffNo + ")", r.staffID.ToString()));
                }

                cboAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.agents.OrderBy(p => p.surName))
                {
                    cboAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " ("
                            + r.agentNo + ")", r.agentID.ToString()));
                }

          
                Session["id"] = null;
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    LoadAccount(id);
                    if (sa != null)
                    {
                        txtAmountDisbursed.Value = sa.amountTaken; 
                        dtAppDate.SelectedDate = sa.applicationDate;
                        dtAppDate.Enabled = false;
                        if (sa.approvalDate != null) dtApproval.SelectedDate = sa.approvalDate;
                        else dtApproval.SelectedDate = sa.applicationDate; 
                        txtNetAmount.Value = sa.netAmountEntitled;
                        txtInterestDed.Value = sa.interestAmount;
                        txtCommission.Value = sa.regularSusCommissionAmount; 
                            lblStatus.Text = "Normal Susu Account Application Received"; 
                  
                        cboClient.SelectedValue = sa.clientID.ToString();
                        cboClient.Items.Clear();
                        cboClient.Items.Add(new RadComboBoxItem(
                            (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            client.companyName : client.surName +
                                ", " + client.otherNames + " (" + client.accountNumber + ")", client.clientID.ToString()));
                        cboClient.SelectedIndex = 0;
                        txtAccountNo.Text = client.accountNumber;
                        if (sa.client.clientTypeID == 6)
                        {
                            pnlJoint.Visible = true;
                            pnlRegular.Visible = false;
                            txtJointAccountName.Text = sa.client.accountName;
                        }
                        else
                        {
                            pnlJoint.Visible = false;
                            pnlRegular.Visible = true;
                            txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                client.companyName : client.surName;
                            txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                " " : client.otherNames;
                        }
                        cboClient.Enabled = false;
                        if (sa.staffID != null)
                        {
                            cboStaff.SelectedValue = sa.staffID.ToString();
                        }
                        if (sa.agentID != null)
                        {
                            cboAgent.SelectedValue = sa.agentID.ToString();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "New Account Application";
                    }
                }
            }
            else
            {
                int? id = null;
                if (Session["id"] != null)
                {
                    id = int.Parse(Session["id"].ToString());
                }
                LoadAccount(id);
            }
        }

        private void LoadAccount(int? id)
        {
            if (id != null)
            {
                sa = le.regularSusuAccounts.FirstOrDefault(p => p.regularSusuAccountID == id);
                if (sa != null)
                {
                    //sa.clientReference.Load();
                    //sa.//client.clientImages.Load();
                    client = sa.client;
                    Session["loan.cl"] = client;
                }
            }
            else
            {
                sa = new coreLogic.regularSusuAccount();
                if (Session["loan.cl"] != null)
                {
                    client = Session["loan.cl"] as coreLogic.client;
                }
            }
            Session["regularSusuAccount"] = sa;
            RenderImages();
        }

        protected void cboSusuGrade_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        protected void cboSusuPosition_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            var conf = le.susuConfigs.FirstOrDefault();
            if (conf != null)
            {
                txtInterestDed.Value = 0.0;
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            int? catID = -1; 
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else
                clients = le.clients.Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((item.clientTypeID == 3 || item.clientTypeID == 4 || item.clientTypeID == 5) ? item.companyName : ((item.clientTypeID == 6) ? item.accountName : item.surName + ", " + item.otherNames) + " (" + item.accountNumber + ")", item.clientID.ToString()));
            }
        }

        protected void cboClient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                if (client != null)
                {
                    Session["loan.cl"] = client; 
                    //client.clientAddresses.Load();
                    rotator1.Items.Clear();
                    //client.clientImages.Load();
                    foreach (var r in client.clientImages)
                    {
                        //r.imageReference.Load();
                    }
                    if (client.clientTypeID == 6)
                    {
                        pnlJoint.Visible = true;
                        pnlRegular.Visible = false;
                        txtJointAccountName.Text = client.accountName;
                        txtAccountNo.Text = client.accountNumber;
                    }
                    else
                    {
                        pnlJoint.Visible = false;
                        pnlRegular.Visible = true;
                        txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            client.companyName : client.surName;
                        txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            " " : client.otherNames;
                        txtAccountNo.Text = client.accountNumber;
                    }

                    RenderImages();
                }
            }
        }

        private void RenderImages()
        {
            if (client != null && client.clientImages != null)
            {
                foreach (var item in client.clientImages)
                {
                    //item.imageReference.Load();
                    RadBinaryImage img = new RadBinaryImage();
                    img.Width = 216;
                    img.Height = 216;
                    img.ResizeMode = BinaryImageResizeMode.Fill;
                    img.DataValue = item.image.image1;
                    RadRotatorItem it = new RadRotatorItem();
                    it.Controls.Add(img);
                    rotator1.Items.Add(it);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (client != null && dtAppDate.SelectedDate != null )  
            {
                if (sa.approvalDate == null)
                {
                    sa.applicationDate = dtAppDate.SelectedDate.Value; 
                    if (sa.regularSusuAccountID <= 0)
                    {
                        return;
                    } 
                  
                    sa.approvalDate = dtApproval.SelectedDate;
                    sa.approvedBy = User.Identity.Name;
                    sa.netAmountEntitled = txtNetAmount.Value.Value;
                    sa.interestAmount = 0.0;

                    if (cboStaff.SelectedValue != "")
                    {
                        sa.staffID = int.Parse(cboStaff.SelectedValue);
                    }
                    if (cboAgent.SelectedValue != "")
                    {
                        sa.agentID = int.Parse(cboAgent.SelectedValue);
                    } 
                    CalculateSchedule();
                    le.SaveChanges();
                    HtmlHelper.MessageBox2("Normal Susu Account Approved Successfully", ResolveUrl("~/ln/regularSusu/regularSusuDefault.aspx"), "coreERP powered by ACS", IconType.ok);
                }
            }
        }

        private DateTime? ComputeDueDate(DateTime startDate)
        {
            DateTime? dt = null;

            var conf = le.susuConfigs.FirstOrDefault();
            if (conf != null)
            {
                dt = startDate;
                for (int i = 0; i < conf.regularSusuPeriodsInCycle; i++ )
                {
                    int j = 1;
                    while (true)
                    {
                        var d = dt.Value.AddDays(1);
                        dt = d;
                        if (d.DayOfWeek == DayOfWeek.Saturday && conf.excludeSaturdays == true) continue;
                        if (d.DayOfWeek == DayOfWeek.Sunday && conf.excludeSundays == true) continue;
                        j = j + 1;
                        if (j > conf.regularSusuDaysInPeriod) break;
                    }
                }
            }

            return dt;
        }

        private DateTime? ComputeEntitledDate(DateTime startDate)
        {
            DateTime? dt = null;

          
            var conf = le.susuConfigs.FirstOrDefault();
            if ( conf != null)
            {
                dt = startDate;
                int j = 1;
                while (true)
                {
                    var d = dt.Value.AddDays(1);
                    dt = d;
                    if (d.DayOfWeek == DayOfWeek.Saturday && conf.excludeSaturdays == true) continue;
                    if (d.DayOfWeek == DayOfWeek.Sunday && conf.excludeSundays == true) continue;
                    j = j + 1;
                    if (j > conf.regularSusuPeriodsInCycle * conf.regularSusuDaysInPeriod) break;
                }
            }

            return dt;
        }

        private string GetRomanNumeral(int no)
        {
            var rn = "";

            var nums = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX" };

            if (no > nums.Length) rn = no.ToString();
            else rn = nums[no - 1];

            return rn;
        }

        private void CalculateSchedule()
        {
            DateTime? dt = null;
            var startDate = sa.approvalDate;
            for (int k = sa.regularSusuContributionSchedules.Count - 1; k >= 0; k--)
            {
                sa.regularSusuContributionSchedules.Remove(sa.regularSusuContributionSchedules.ToArray()[k]);
            }
          //  var pos = le.susuPositions.FirstOrDefault(p => p.susuPositionID == sa.susuPositionID);
            var conf = le.susuConfigs.FirstOrDefault();
            if (conf != null)
            {
                dt = startDate;
                for (int i = 0; i < conf.periodsInCycle; i++)
                {
                    int j = 1;
                    while (true)
                    {
                        var d = dt.Value; 
                        dt = dt.Value.AddDays(1);
                        if (d.DayOfWeek == DayOfWeek.Saturday && conf.excludeSaturdays == true) continue;
                        if (d.DayOfWeek == DayOfWeek.Sunday && conf.excludeSundays == true) continue;
                        sa.regularSusuContributionSchedules.Add(new regularSusuContributionSchedule
                        {
                            amount = sa.contributionAmount,
                            plannedContributionDate = d,
                            balance = sa.contributionAmount
                        });
                        j = j + 1;
                        if (j > conf.regularSusuDaysInPeriod) break;
                    }
                }
            }
        }
    }
}