using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.hc.loans
{
    public partial class post : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.staff staff;
        coreLogic.staffLoan ln;
        List<coreLogic.staffLoanSchedule> schedule;
        List<coreLogic.staffLoanRepayment> repayments;

        protected void Page_Load(object sender, EventArgs e)
        { 
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            {
                le = new coreLogic.coreLoansEntities();
                Session["le"] = le;

                cboLoanType.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.staffLoanTypes.OrderBy(p => p.loanTypeName))
                {
                    cboLoanType.Items.Add(new RadComboBoxItem(r.loanTypeName, r.loanTypeID.ToString()));
                }

                cboBank.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts.OrderBy(p=>p.bank_acct_desc).ToList())
                {
                    cboBank.Items.Add(new RadComboBoxItem(r.bank_acct_desc, r.bank_acct_id.ToString()));
                }

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    ln = le.staffLoans.FirstOrDefault(p => p.staffLoanID == id);

                    if (ln != null)
                    {
                        //ln.staffLoanSchedules.Load();
                        //ln.staffLoanRepayments.Load();
                        //ln.staffLoanTypeReference.Load();
                        //ln.staffReference.Load();
                        
                        staff = ln.staff;
                        //staff.staffAddresses.Load();
                        //staff.staffImages.Load();
                        foreach (var r in staff.staffImages)
                        {
                            //r.imageReference.Load();
                        }
                        Session["staffLoan.cl"] = staff;

                        schedule = ln.staffLoanSchedules.ToList();
                        repayments = ln.staffLoanRepayments.ToList();

                        txtAmount.Value = ln.principal;
                        txtCNotes.Text = ln.memo;
                        txtPrincBalance.Value = ln.principalBalance;
                        txtInterestBalance.Value = ln.interestBalance;
                        cboLoanType.SelectedValue = ln.loanTypeID.ToString();
                        txtInterestRate.Value = ln.rate;
                        chkAttractsInterest.Checked = ln.attractsInterest;
                        cboStaff.SelectedValue = ln.staffID.ToString();
                        dtAppDate.SelectedDate = ln.disbursementDate;
                        dtpDeductionStartsDate.SelectedDate = ln.deductionStartsDate;
                        cboStaff.Items.Clear();
                        cboStaff.Items.Add(new RadComboBoxItem(staff.surName + ", " + staff.otherNames + " ("
                            + staff.staffNo + ")", staff.staffID.ToString()));
                        cboStaff.SelectedValue = ln.staffID.ToString();
                        txtSurname.Text = staff.surName;
                        txtOtherNames.Text = staff.otherNames;
                        txtAccountNo.Text = staff.staffNo;
                        dtApprovalDate.SelectedDate = ln.approvedDate;

                        gridSchedule.DataSource = schedule;
                        gridSchedule.DataBind();
                        gridRepayment.DataSource = repayments;
                        gridRepayment.DataBind();

                        RenderImages();
                    } 

                    Session["staffLoan"] = ln;
                } 
                else
                {
                    ln = new coreLogic.staffLoan();
                    dtAppDate.SelectedDate = DateTime.Now;
                    Session["staffLoan"] = ln;
                    schedule = new List<coreLogic.staffLoanSchedule>();
                    repayments = new List<coreLogic.staffLoanRepayment>();
                } 
            }
            else
            {
                le = Session["le"] as coreLogic.coreLoansEntities;
                if (Session["staffLoan.cl"] != null)
                {
                    staff = Session["staffLoan.cl"] as coreLogic.staff;
                }
                if (Session["staffLoan"] != null)
                {
                    ln = Session["staffLoan"] as coreLogic.staffLoan;
                } 
            }  
        }

        protected void cboStaff_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboStaff.SelectedValue != "")
            {
                int staffID = int.Parse(cboStaff.SelectedValue);
                staff = le.staffs.FirstOrDefault(p => p.staffID == staffID);
                if (staff != null)
                {
                    Session["staffLoan.cl"] = staff; 
                    //staff.staffAddresses.Load();
                    rotator1.Items.Clear();

                    RenderImages();
                }
            }
        }
         
        private void RenderImages()
        {
            if (staff.staffImages != null)
            {
                foreach (var item in staff.staffImages)
                {
                    //item.imageReference.Load();
                    RadBinaryImage img = new RadBinaryImage();
                    img.Width = 209;
                    img.Height = 113;
                    img.ResizeMode = BinaryImageResizeMode.Fit;
                    img.DataValue = item.image.image1;
                    RadRotatorItem it = new RadRotatorItem();
                    it.Controls.Add(img);
                    rotator1.Items.Add(it);
                }
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        { 
            List<coreLogic.staff> staffs = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.surName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.staffNo.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.staffNo.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.surName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.staffNo.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                staffs = le.staffs.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) ).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.staffNo.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                staffs = le.staffs.Where(p => p.surName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                staffs = le.staffs.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).ToList();
            else
                staffs = le.staffs.ToList();
            cboStaff.Items.Clear();
            cboStaff.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in staffs)
            {
                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(item.surName + ", " + item.otherNames + " (" + item.staffNo + ")", item.staffID.ToString()));
            }
        }
         
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAmount.Value != null 
                && txtInterestRate.Value != null 
                && cboLoanType.SelectedValue != "" 
                && dtAppDate.SelectedDate != null
                && dtpDeductionStartsDate.SelectedDate != null
                && dtApprovalDate.SelectedDate != null)
            {
                if (Save() == true)
                {
                    HtmlHelper.MessageBox2("Loan Data Saved Successfully!", ResolveUrl("~/hc/loans/default.aspx"), "coreERP©: Successful", IconType.ok);
                }
            }
        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            if (txtAmount.Value != null
                && txtInterestRate.Value != null
                && cboLoanType.SelectedValue != ""
                && dtAppDate.SelectedDate != null
                && dtpDeductionStartsDate.SelectedDate != null
                && dtApprovalDate.SelectedDate != null)
            {
                if (Save() == true)
                {
                    HtmlHelper.MessageBox("Loan Data Saved Successfully!");
                }
            }
        }

        private bool Save()
        {
            if (txtAmount.Value != null
                && txtInterestRate.Value != null
                && cboLoanType.SelectedValue != ""
                && dtAppDate.SelectedDate != null
                && dtpDeductionStartsDate.SelectedDate != null
                && dtApprovalDate.SelectedDate != null
                && cboBank.SelectedValue!= "")
            {
                var pro=ent.comp_prof.FirstOrDefault();
                var acc = le.payrollPostingAccounts.FirstOrDefault();
                int? bankID = null;
                if (cboBank.SelectedValue != null && cboBank.SelectedValue != "")
                    bankID = int.Parse(cboBank.SelectedValue);
                var ba = ent.bank_accts.FirstOrDefault(p=>p.bank_acct_id==bankID);
                if (ln.approvedDate != null && ln.posted == false && acc != null && ba != null)
                {
                    //ba.acctsReference.Load();
                    var jb = journalextensions.Post("PR",
                        acc.loansReceivableAccountID, ba.accts.acct_id,
                        ln.principal, "Staff Loan: " + ln.staffLoanType.loanTypeName, pro.currency_id.Value,
                        ln.approvedDate.Value, ln.staffLoanID.ToString(), ent, User.Identity.Name, null);
                    ent.jnl_batch.Add(jb);
                    ln.bankID = bankID;
                    ln.checkNo = txtCheckNo.Text;
                    ln.posted = true;

                    le.SaveChanges();
                    ent.SaveChanges();

                    Session["staffLoan"] = ln;
                    return true;
                } 
            }

            return false;
        }   
    }
}