using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Data.Entity;
using coreLogic;
using System.Linq;

public partial class JDefault : System.Web.UI.Page 
{
    coreLogic.coreLoansEntities le;

    protected void Page_Load(object sender, EventArgs e)
    {
        le = new coreLogic.coreLoansEntities();
        if (!Page.IsPostBack)
        {
            cboBranch.Items.Add(new RadComboBoxItem("All Branches", ""));
            foreach (var br in le.branches.OrderBy(p => p.branchName))
            {
                cboBranch.Items.Add(new RadComboBoxItem(br.branchName, br.branchID.ToString()));
            }
            Bind();
        }
        else
        {

        }
    }

    private bool IsAdmin()
    {
        using (var ent = new coreSecurityEntities())
        {
            var user = ent.users.FirstOrDefault(p => p.user_name == User.Identity.Name);
            if (user != null)
            {
                var rl = user.user_roles.FirstOrDefault(p => p.roles.role_name == "admin");
                return rl != null;
            }
        }

        return false;
    }

    private void Bind()
    {
        int? branchID = null;
        if (Session["branchID"] != null && Session["branchID"] != "")
        {
            branchID = int.Parse(Session["branchID"].ToString());
        }
        try
        {
            var isAdmin = IsAdmin();
             
        }
        catch (Exception) { }

        try
        {
           
        }
        catch (Exception) { }


        try
        {
            
        }
        catch (Exception) { }

        try
        {
 
        }
        catch (Exception) { }

        coreReports.reportEntities rent = new coreReports.reportEntities();
        try
        {
            var disbRec = rent.getDisbRecComparison(branchID).OrderByDescending(p => p.month).Take(6).OrderBy(p => p.month).ToList();

            chComp.DataSource = disbRec;
            chComp.PlotArea.Series[0].DataFieldY = "disbursed";
            chComp.PlotArea.Series[1].DataFieldY = "paid";
            chComp.PlotArea.XAxis.DataLabelsField = "month";
        }
        catch (Exception) { }
        chComp.DataBind();
        try
        {
            var col = rent.getCollectionRatioByMonth(branchID).OrderByDescending(p => p.month).Take(5).OrderBy(p => p.month).ToList();

            chCol.DataSource = col;
            chCol.PlotArea.Series[0].DataFieldY = "collection";
            chCol.PlotArea.XAxis.DataLabelsField = "month";
            chCol.DataBind();
        }
        catch (Exception) { }
    }
   
    protected void cboBranch_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Session["branchID"] = cboBranch.SelectedValue;
        Bind();
    }
  
}
