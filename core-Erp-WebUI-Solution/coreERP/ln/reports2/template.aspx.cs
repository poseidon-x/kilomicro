using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using coreReports;
using coreLogic;

namespace coreERP.ln.reports2
{
    public partial class template : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports2/template.aspx"; }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rpt != null)
            {
                try
                {
                    rpt.Dispose();
                    rpt.Close();
                    rpt = null;
                }
                catch (Exception) { }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
                coreLoansEntities le = new coreLoansEntities();
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => (categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5)))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " - " + cl.accountNumber, cl.clientID.ToString()));
                }

                cboTemplate.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.loanTemplates.OrderBy(p=>p.templateName).ToList())
                {
                    cboTemplate.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.templateName + " - ", cl.loanTemplateID.ToString()));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (cboLoan.SelectedValue != ""  && cboTemplate.SelectedValue!="")
            {
                var loanID = int.Parse(cboLoan.SelectedValue);
                var templateID = int.Parse(cboTemplate.SelectedValue);
                Session["loanID"] = loanID;
                Session["templateID"] = templateID;
                Bind(loanID, templateID);
            }
            else
            {
                Session["loanID"] = null;
                Bind(null, null);
            }
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["loanID"] != null)
            {
                if (Session["loanID"] != null)
                {
                    var loanID = (int)Session["loanID"];
                    var templateID = (int)Session["templateID"];
                    Bind(loanID, templateID);
                }
                else
                {
                    Bind( null, null);
                }
            }
        }

        private void Bind(int? loanID, int? templateID)
        {
            var categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = ""; 
            rpt = new coreReports.ln3.rptLoanTemplate(); 
            if (loanID != null)
            {
                var temp = (new coreLoansEntities()).loanTemplates.FirstOrDefault(p => p.loanTemplateID == templateID);
                var vl = (new coreReports.reportEntities()).vwLoanAgreements.FirstOrDefault(p => p.loanID == loanID);
                rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                var tb = temp.templateBody.Replace("$$clientName$$", vl.clientName).Replace("$$phyAddress$$ ",
                    vl.phyAddr1).Replace("$$principalPayment$$",vl.principalPayment.ToString("#,###.#0")).Replace(
                    "$$principalPaymentInWords$$", NumberToWords(vl.principalPayment)).Replace("$$loanPurpose$$",
                    vl.loanTypeName).Replace("$$loanTenure$$", vl.loanTenure.ToString("#,###") + " Months").Replace(
                    "$$startDate$$", vl.disbursementDate.ToString("dd-MMM-yyyy")).Replace("$$endDate$$",
                    vl.disbursementDate.AddMonths((int)vl.loanTenure).ToString("dd-MMM-yyyy")).Replace("$$interestRate$$",
                    vl.interestRate.ToString("#,###.#0") + " % per Month").Replace("$$interestPayment$$",
                    vl.interestPayment.ToString("#,###.#0")).Replace("($$interestPaymentInWords$$",
                    NumberToWords(vl.interestPayment)).Replace("$$phones$$", vl.mobilePhone + ", " + vl.workPhone).Replace(
                    "$$totalPayment$$",(vl.principalPayment+vl.interestPayment).ToString("#,###.#0")).Replace(
                    "$$date$$", vl.disbursementDate.ToString("dd MMMM, yyyy")).Replace("$$collateralTypeName$$",
                    vl.collateralType);
                rpt.SetParameterValue("templateBody", tb);
                this.rvw.ReportSource = rpt;
            } 
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                coreLoansEntities le = new coreLoansEntities();
                var clientID = int.Parse(cboClient.SelectedValue);
                cboLoan.Items.Clear();
                cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.loans.Where(p=> p.clientID==clientID).ToList())
                {
                    cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.loanNo + " - " + cl.amountDisbursed.ToString("#,###.#0"), 
                        cl.loanID.ToString()));
                }
            }
        }

        public static string NumberToWords(double number)
        {
            int whole = (int)Math.Floor(number);
            int dec = (int)Math.Floor((Math.Round(number - Math.Floor(number), 2) * 100));
            string words = NumberToWords(whole) + " Ghana Cedies";
            if (dec > 0)
            {
                words = words + " and " + NumberToWords(dec) + " Ghana Pesewas";
            }

            return words;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
}
