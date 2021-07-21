using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Web.Services;
using System.Data;
using coreReports;

namespace coreERP.ln.loans
{
    public partial class defaultClosure : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        string categoryID = null;
        private int serialNumber = 0;

        public string GetSerialNumber()
        {
            serialNumber = serialNumber + 1;

            return serialNumber.ToString("#,##0");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            categoryID = Request.Params["catID"];
            if (categoryID == null)
            {
                categoryID = "";
            }
            le = new coreLogic.coreLoansEntities();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            string otherNames = this.txtOthernames.Text.Trim().Replace(";", "");
            var surName = txtSurname.Text.Trim().Replace(";", "");
            int? catID = -1;
            if (categoryID != null && categoryID.Trim() != "") catID = int.Parse(categoryID);
            List<coreLogic.client> clients = null;
            if (surName.Length > 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(surName) || p.accountName.Contains(surName)).Where(
                    p => p.otherNames.Contains(otherNames)).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (surName.Length == 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(otherNames)).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (surName.Length > 0 && otherNames.Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(surName) || p.accountName.Contains(surName)).Where(
                    p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (surName.Length > 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(otherNames)).Where(
                    p => p.surName.Contains(surName) || p.accountName.Contains(surName)).ToList();
            else if (surName.Length == 0 && otherNames.Length == 0 && txtAccNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccNo.Text.Trim())).ToList();
            else if (surName.Length > 0 && otherNames.Length == 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(surName) || p.accountName.Contains(surName)).ToList();
            else if (surName.Length == 0 && otherNames.Length > 0 && txtAccNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(otherNames)).ToList();
            else
                clients = le.clients.ToList();
            for (var i = clients.Count - 1; i >= 0;i--)
            {
                var cl = clients[i];
                //cl.branchReference.Load();
                if (cl.clientTypeID == 6)
                {
                    cl.surName = cl.accountName;
                    cl.otherNames = "";
                }
                else if(cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5){
                    cl.surName = cl.companyName;
                    cl.otherNames = "";
                }
                //cl.loans.Load();
                //cl.staffCategory1.Load();
                foreach (var l in cl.loans)
                {
                    //l.staffReference.Load();
                }
                if (cl.loans
                    .Where(p=> p.closed == null ||p.closed == false)
                    .Count(p => (p.loanStatusID != 7 && (txtStaffID.Text.Trim().Length == 0 || categoryID!="5" ||
                    (p.client.staffCategory1.Count>0 && p.client.staffCategory1.First().employeeNumber.Contains(txtStaffID.Text.Trim()))) 
                    && (txtAgentID.Text.Trim().Length == 0 ||
                    (p.agent != null && p.agent.agentNo.Contains(txtAgentID.Text.Trim()))))) == 0)
                {
                    clients.Remove(cl);
                }
            }
            grid.DataSource = clients;
            grid.DataBind();
        }
         
    }
}