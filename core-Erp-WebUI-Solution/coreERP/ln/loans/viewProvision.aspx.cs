using coreLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class viewProvision : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            {
                cboYear.Items.Add(new RadComboBoxItem("", ""));
                for (int i = 0; i < 20; i++ )
                {
                    var year = 2010 + i;
                    if (year <= DateTime.Now.Year)
                    {
                        cboYear.Items.Add(new RadComboBoxItem(year.ToString(), year.ToString()));
                    }
                }

                cboMonth.Items.Add(new RadComboBoxItem("", ""));
                for (int i = 0; i < 12; i++)
                {
                    var month = new DateTime(2010,1,1).AddMonths(i);
                    cboMonth.Items.Add(new RadComboBoxItem(month.ToString("MMMM"), month.ToString("MM")));
                }
            }
        }

        protected void btnInit_Click(object sender, EventArgs e)
        { 
            le=new  coreLoansEntities();
            if (cboYear.SelectedValue != "" && cboMonth.SelectedValue != "")
            {
                int year = int.Parse(cboYear.SelectedValue);
                int month = int.Parse(cboMonth.SelectedValue);
                var date = new DateTime(year, month, 1).AddMonths(1).AddSeconds(-1);

                if (date <= DateTime.Now && le.loanProvisions.Any(p => p.provisionDate == date))
                {
                    coreReports.reportEntities rent = new coreReports.reportEntities();
                    rent.Database.CommandTimeout = 3000;
                    var proClses = le.provisionClasses.ToList();
                    var vl2s = rent.GetvwLoans2(date).ToList();
                    foreach (var vl2 in vl2s)
                    {
                        if (vl2.principalBalance + vl2.interestBalance > 1)
                        {
                            coreLogic.provisionClass proCls = null;
                            foreach (var cls in proClses)
                            {
                                if (cls.maxDays >= vl2.daysDue && cls.minDays <= vl2.daysDue)
                                {
                                    proCls = cls;
                                    break;
                                }
                            }
                            if (proCls != null)
                            {
                                var provision = vl2.principalBalance*proCls.provisionPercent/100.0;
                                var inc = le.loanProvisions.FirstOrDefault(p => p.provisionDate == date
                                                                                && p.loanID == vl2.loanID);
                                if (inc == null)
                                {

                                    inc = new coreLogic.loanProvision
                                    {
                                        daysDue = vl2.daysDue,
                                        interestBalance = vl2.interestBalance,
                                        loanID = vl2.loanID,
                                        posted = false,
                                        principalBalance = vl2.principalBalance,
                                        proposedAmount = provision,
                                        provisionAmount = 0,
                                        provisionClassID = proCls.provisionClassID,
                                        provisionDate = date,
                                        securityValue = vl2.collateralValue,
                                        typeOfSecurity = vl2.collateralType,
                                        edited = false
                                    };
                                    le.loanProvisions.Add(inc);
                                }
                                else if (!inc.edited)
                                {
                                    inc.interestBalance = vl2.interestBalance;
                                    inc.principalBalance = vl2.principalBalance;
                                    inc.provisionClassID = proCls.provisionClassID;
                                    inc.securityValue = vl2.collateralValue;
                                    inc.typeOfSecurity = vl2.collateralType;
                                    inc.proposedAmount = provision;
                                    inc.daysDue = vl2.daysDue;
                                }
                            }
                        }
                    }
                    le.SaveChanges();

                }
                else
                {
                    coreReports.reportEntities rent = new coreReports.reportEntities();
                    var proClses = le.provisionClasses.ToList();
                    var vl2s = rent.GetvwLoans2(date).ToList();
                    foreach (var vl2 in vl2s)
                    {
                        if (vl2.principalBalance + vl2.interestBalance > 1)
                        {
                            coreLogic.provisionClass proCls = null;
                            foreach (var cls in proClses)
                            {
                                if (cls.maxDays >= vl2.daysDue && cls.minDays <= vl2.daysDue)
                                {
                                    proCls = cls;
                                    break;
                                }
                            }
                            if (proCls != null )
                            {
                                var provision = vl2.principalBalance * proCls.provisionPercent / 100.0;
                                var inc =le.loanProvisions.FirstOrDefault(p=> p.provisionDate==date 
                                && p.loanID==vl2.loanID
                                        );

                                if (inc == null)
                                {
                                    inc = new coreLogic.loanProvision
                                    {
                                        daysDue = vl2.daysDue,
                                        interestBalance = vl2.interestBalance,
                                        loanID = vl2.loanID,
                                        posted = false,
                                        principalBalance = vl2.principalBalance,
                                        proposedAmount = provision,
                                        provisionAmount = 0,
                                        provisionClassID = proCls.provisionClassID,
                                        provisionDate = date,
                                        securityValue = vl2.collateralValue,
                                        typeOfSecurity = vl2.collateralType,
                                        edited = false
                                    };
                                    le.loanProvisions.Add(inc);
                                }
                                else if (!inc.edited)
                                {
                                    inc.interestBalance = vl2.interestBalance;
                                    inc.proposedAmount = vl2.provisionAmount;
                                    inc.principalBalance = vl2.principalBalance;
                                    inc.daysDue = vl2.daysDue;
                                    inc.provisionClassID = proCls.provisionClassID;
                                    inc.securityValue = vl2.collateralValue;
                                    inc.typeOfSecurity = vl2.collateralType;
                                    inc.posted = false;
                                }
                            }
                        }
                    }
                    le.SaveChanges();
                }
                
                var incs = le.loanProvisions
                    .Include(p=> p.loan)
                    .Include(p => p.loan.client)
                    .Where(p => p.provisionDate == date ).OrderBy(p => p.daysDue)
                    .ThenBy(p => p.loan.client.surName).ThenBy(p => p.loan.client.otherNames).ThenBy(p => p.loan.loanNo)
                    .ToList();
                
                rpPenalty.DataSource = incs;
                rpPenalty.DataBind();
            }
        }



        protected void cboMonth_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            btnInit_Click(sender, e);
        }

        protected void cboYear_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            btnInit_Click(sender, e);
        }

        public string GetClientName(object ln)
        {
            var l = ln as coreLogic.loan;
            if (l == null)
            {
                return "";
            }
            else if (l.client == null)
            {
                return "";
            }
            return l.client.surName + ", " + l.client.otherNames;
        }

        protected void txtAmt_OnTextChanged(object sender, EventArgs e)
        {
            
        }
    }
}