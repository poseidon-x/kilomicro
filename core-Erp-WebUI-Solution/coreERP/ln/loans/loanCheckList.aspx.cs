using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class loanCheckList : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.client client;
        coreLogic.loan ln;
        List<coreLogic.loanGurantor> guarantors;
        List<coreLogic.loanCollateral> collaterals;
        List<coreLogic.loanDocument> documents;

        string categoryID = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            categoryID = Request.Params["catID"];
            le = new coreLogic.coreLoansEntities();
            if (!IsPostBack)
            {
                cboRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.repaymentModes)
                {
                    cboRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentModeName, r.repaymentModeID.ToString()));
                }

                cboInterestType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.interestTypes)
                {
                    cboInterestType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.interestTypeName, r.interestTypeID.ToString()));
                }

                cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanTypes)
                {
                    cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanTypeName, r.loanTypeID.ToString()));
                }

                cboLoanProduct.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanProducts)
                {
                    cboLoanProduct.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanProductName, r.loanProductID.ToString()));
                }

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    ln = le.loans.FirstOrDefault(p => p.loanID == id);

                    if (ln != null)
                    {
                        client = ln.client;
                        Session["loan.cl"] = client;

                        guarantors = ln.loanGurantors.ToList();
                        collaterals = ln.loanCollaterals.ToList();
                        documents = ln.loanDocuments.ToList();

                        Session["loanGuarantors"] = guarantors;
                        Session["loanCollaterals"] = collaterals;
                        Session["loanDocuments"] = documents;

                        gridDocument.DataSource = documents;
                        gridDocument.DataBind();

                        gridCollateral.DataSource = collaterals;
                        gridCollateral.DataBind();

                        gridGuarantor.DataSource = guarantors;
                        gridGuarantor.DataBind();
                         
                        txtTenure.Value = ln.loanTenure;
                        lblLoanID.Text = ln.loanNo;
                        txtRate.Value = ln.interestRate; 
                        txtAmountRequested.Value = ln.amountRequested; 
                        txtProcFee.Value = ln.processingFee; 
                        cboRepaymentMode.SelectedValue = ln.repaymentModeID.ToString();
                        cboLoanType.SelectedValue = ln.loanTypeID.ToString();

                        cboInterestType.SelectedValue = ln.interestTypeID.ToString();
                        if (ln.client.clientTypeID == 6)
                        {
                            pnlJoint.Visible = true;
                            pnlRegular.Visible = false;
                            txtJointAccountName.Text = ln.client.accountName;
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

                        if (ln.loanSchemeId != null)
                        {
                            cboLoanScheme.SelectedValue = ln.loanSchemeId.ToString();
                            txtRate.Enabled = false;
                            txtTenure.Enabled = false;
                            cboRepaymentMode.Enabled = false;
                        }

                        txtAccountNo.Text = client.accountNumber; 
                        dtAppDate.SelectedDate = ln.applicationDate;
                        RenderImages();
                        gridSchedule.DataSource = ln.repaymentSchedules;
                        gridSchedule.DataBind();
                        txtCreditManagerNotes.Text = (ln.creditManagerNotes == null) ? "" : ln.creditManagerNotes;

                        var detail = ln.prLoanDetails.FirstOrDefault();
                        if (detail != null)
                        {
                            cboLoanProduct.SelectedValue = detail.loanProductID.ToString();
                        }

                        if (ln.loanCheckLists.Count == 0)
                        {
                            foreach (var item in client.category.categoryCheckLists)
                            {
                                var cl = new coreLogic.loanCheckList
                                {
                                    categoryCheckListID = item.categoryCheckListID,
                                    passed = false,
                                    categoryCheckList = item,
                                    description = item.description
                                };
                                ln.loanCheckLists.Add(cl);
                            }
                            if (categoryID != "5")
                            {
                                foreach (var item in le.genericCheckLists)
                                {
                                    var cl = new coreLogic.loanCheckList
                                    {
                                        categoryCheckListID = null,
                                        passed = false,
                                        categoryCheckList = null,
                                        description = item.description
                                    };
                                    ln.loanCheckLists.Add(cl);
                                }
                            }
                            le.SaveChanges();
                        }
                        rpCheckList.DataSource = ln.loanCheckLists;
                        rpCheckList.DataBind();
                        RenderImages();
                    }

                    Session["loan"] = ln;
                }
                else
                {
                    ln = new coreLogic.loan();
                    Session["loan"] = ln;

                    guarantors = new List<coreLogic.loanGurantor>();
                    Session["loanGuarantors"] = guarantors;

                    collaterals = new List<coreLogic.loanCollateral>();
                    Session["loanCollaterals"] = collaterals;
                    documents = new List<coreLogic.loanDocument>();
                    Session["loanDocuments"] = documents;
                }
            }
            else
            {
                int? id = null;
                if (Session["id"] != null)
                {
                    id = int.Parse(Session["id"].ToString());
                }
                LoadLoan(id);
            }
            if (categoryID == "5")
            {
                cboLoanType.Visible = false;
                cboLoanProduct.Visible = true;
                pnlFees.Visible = false;
            }
            if (ln.loanStatusID == 4 || ln.loanStatusID == 7 || ln.loanStatusID==3)
            {
                btnApprove.Enabled = false;
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            var passed = true;
            foreach (RepeaterItem item in rpCheckList.Items)
            {
                var lblDesc = item.FindControl("lblDesc") as Label;
                var chkPassed = item.FindControl("chkPassed") as CheckBox;
                var chkMandatory = item.FindControl("chkMandatory") as CheckBox;
                var txtComments = item.FindControl("txtComments") as TextBox;
                if (lblDesc != null && chkPassed != null && txtComments != null)
                {
                    var cl = ln.loanCheckLists.FirstOrDefault(p => p.description == lblDesc.Text);
                    if (cl != null)
                    {
                        cl.passed = chkPassed.Checked;
                        cl.comments = txtComments.Text;
                        cl.creationDate = DateTime.Now;
                    }
                    if (categoryID == "5")
                    {
                        if (chkMandatory.Checked==true && chkPassed.Checked == false)
                        {
                            passed = false;
                        }
                    }
                    else
                    {
                        if (chkPassed.Checked == false)
                        {
                            passed = false;
                        }
                    }
                }
            }
            if (dtAppDate.SelectedDate != null)
            { 
                ln.modification_date = DateTime.Now;
                ln.last_modifier = User.Identity.Name; 
                ln.interestRate = txtRate.Value.Value;
                ln.interestTypeID = int.Parse(cboInterestType.SelectedValue); 
                ln.processingFee = txtProcFee.Value.Value;  
                ln.processingFeeBalance = txtProcFee.Value.Value;
                ln.creditManagerNotes = txtCreditManagerNotes.Text;

                if (passed)
                {
                    ln.loanStatusID = 2;
                    ln.checkedBy = User.Identity.Name;
                }
                ln.loanTenure = txtTenure.Value.Value;
                ln.loanTypeID = int.Parse(cboLoanType.SelectedValue);
                ln.repaymentModeID = int.Parse(cboRepaymentMode.SelectedValue);
                ln.tenureTypeID = 1;
                 
                le.SaveChanges();
                
                Session["loanGuarantors"] = null;
                Session["loanCollaterals"] = null;
                Session["loan.cl"] = null;
                Session["loan"] = null;
                HtmlHelper.MessageBox2("Loan Checklist Data Saved Successfully!", ResolveUrl("~/ln/loans/default.aspx?catID="
                    + ln.client.categoryID.ToString()), "coreERP©: Successful", IconType.ok);  
            }
        } 

        protected void gridGuarantor_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                
            }
            else if (e.CommandName == "DeleteItem")
            {
            }
            gridGuarantor.DataSource = guarantors;
            gridGuarantor.DataBind();
        }
         
        protected void gridCollateral_ItemCommand(object sender, GridCommandEventArgs e)
        {
           
        }

        private void RenderImages()
        {
            if (client.clientImages != null)
            {
                foreach (var item in client.clientImages)
                { 
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

        protected void gridCollateral_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            try
            {
                GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
                if (dataItem.ItemIndex < ln.loanCollaterals.Count)
                {
                    e.DetailTableView.DataSource = ln.loanCollaterals.ToList()[dataItem.ItemIndex].collateralImages;
                }
            }
            catch (Exception) { }
        }

        protected bool GetMandatory(object id)
        {
            try
            {
                var mand = false;
                var loanCheckListID = -1;
                if (id != null)
                {
                    mand = (id as coreLogic.categoryCheckList).isMandatory;
                }
                return mand;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void chkPassedAll_CheckedChanged(object sender, EventArgs e)
        {
            var s = sender as CheckBox;
            if (s != null)
            { 
                foreach (RepeaterItem item in rpCheckList.Items)
                {
                    var chkPassed = item.FindControl("chkPassed") as CheckBox;
                    if (chkPassed != null)
                    {
                        chkPassed.Checked = s.Checked;
                    }
                } 
            }
        }
        private void LoadLoan(int? id)
        {
            if (id != null)
            {
                ln = le.loans.FirstOrDefault(p => p.loanID == id);

                if (ln != null)
                { 
                    client = ln.client;  
                    Session["loan.cl"] = client;

                    guarantors = ln.loanGurantors.ToList();
                    collaterals = ln.loanCollaterals.ToList();
                    documents = ln.loanDocuments.ToList();
                      
                    Session["loanGuarantors"] = guarantors;
                    Session["loanCollaterals"] = collaterals; 
                    Session["loanDocuments"] = documents;

                    RenderImages(); 
                }

                Session["loan"] = ln;
            }
            else
            {
                if (Session["loan.cl"] != null)
                {
                    client = Session["loan.cl"] as coreLogic.client;
                }
                if (Session["loan"] != null)
                {
                    ln = Session["loan"] as coreLogic.loan;
                }
                else
                {
                    ln = new coreLogic.loan();
                    Session["loan"] = ln;
                }
                if (Session["loanGuarantors"] != null)
                {
                    guarantors = Session["loanGuarantors"] as List<coreLogic.loanGurantor>;
                }
                else
                {
                    guarantors = new List<coreLogic.loanGurantor>();
                    Session["loanGuarantors"] = guarantors;
                } 
                if (Session["loanDocuments"] != null)
                {
                    documents = Session["loanDocuments"] as List<coreLogic.loanDocument>;
                }
                else
                {
                    documents = new List<coreLogic.loanDocument>();
                    Session["loanDocuments"] = documents;
                }
                if (Session["loanCollaterals"] != null)
                {
                    collaterals = Session["loanCollaterals"] as List<coreLogic.loanCollateral>;
                }
                else
                {
                    collaterals = new List<coreLogic.loanCollateral>();
                    Session["loanCollaterals"] = collaterals;
                }
            }
        }
    }
}