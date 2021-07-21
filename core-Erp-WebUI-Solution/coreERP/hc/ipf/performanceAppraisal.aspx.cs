using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity.Core.Objects;

namespace coreERP.hc.ipf
{
    public partial class performanceAppraisal : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.performanceContract contract;
        coreLogic.performanceContractItem editedItem;
        coreLogic.performanceAppraisal appraisal;
        coreLogic.performanceAppraisalScore score;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                le = new coreLogic.coreLoansEntities();
                Session["le"] = le;
                cboArea.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.performanceAreas.OrderBy(p => p.performanceAreaName))
                {
                    cboArea.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.performanceAreaName, r.performanceAreaID.ToString()));
                }
                cboStatus.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.performanceContractStatus.Where(p=>p.performanceContractStatusID>0).OrderBy(p => p.performanceContractStatusName))
                {
                    cboStatus.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.performanceContractStatusName, 
                        r.performanceContractStatusID.ToString()));
                }
                cboAppraisalType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.performanceAppraisalTypes.OrderBy(p => p.performanceAppraisalTypeName))
                {
                    cboAppraisalType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.performanceAppraisalTypeName,
                        r.performanceAppraisalTypeID.ToString()));
                }
                if (Request.Params["id"] != null)
                {
                    var id = int.Parse(Request.Params["id"]);
                    contract = le.performanceContracts.FirstOrDefault(p => p.performanceContractID == id);
                    if (contract != null)
                    {
                        var staff = le.staffs.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                        if (staff != null && staff.staffID != contract.staffID)
                        {
                            HtmlHelper.MessageBox2("This performance contract does not belong to you.",
                                ResolveUrl("~/hc/ipf/performanceContract.aspx"), "coreERP: Failed", IconType.deny);
                            return;
                        }
                        //contract.performanceContractItems.Load();
                        foreach (var r in contract.performanceContractItems)
                        {
                            //r.performanceContractTargets.Load();
                        }
                        if (Request.Params["aid"] != null)
                        {
                            id = int.Parse(Request.Params["aid"]);
                            appraisal = le.performanceAppraisals.FirstOrDefault(p => p.performanceAppraisalID == id);
                            if (appraisal != null)
                            {
                                //appraisal.performanceAppraisalScores.Load();
                                foreach (var r in appraisal.performanceAppraisalScores)
                                {
                                    //r.performanceContractItemReference.Load();
                                }
                                dtpAppraisalDate.SelectedDate = appraisal.appraisalDate;
                                cboAppraisalType.SelectedValue = appraisal.performanceAppraisalTypeID.ToString();
                                Session["performanceAppraisal"] = appraisal;
                            }
                        }
                        else
                        {
                            appraisal = new coreLogic.performanceAppraisal();
                            appraisal.performanceContractID = contract.performanceContractID;
                            appraisal.staffComments = "";
                            appraisal.managerComments = "";
                            appraisal.hrComments = "";
                            Session["performanceAppraisal"] = appraisal;
                        }
                        cboStatus.SelectedValue = contract.performanceContractStatusID.ToString();
                    }
                    Session["performanceContract"] = contract;
                    Bind();
                } 
                RenderTree();
            }
            else
            {
                if (Session["performanceContract"] != null)
                {
                    contract = Session["performanceContract"] as coreLogic.performanceContract;
                }
                if (Session["performanceAppraisal"] != null)
                {
                    appraisal = Session["performanceAppraisal"] as coreLogic.performanceAppraisal;
                }
                if (Session["performanceContractItem"] != null)
                {
                    editedItem = Session["performanceContractItem"] as coreLogic.performanceContractItem;
                }
                if (Session["performanceAppraisalScore"] != null)
                {
                    score = Session["performanceAppraisalScore"] as coreLogic.performanceAppraisalScore;
                }
                if (Session["le"] != null)
                {
                    le = Session["le"] as coreLogic.coreLoansEntities;
                }
            }
        }

        protected void btnAddEdit_Click(object sender, EventArgs e)
        {
            if (contract != null && dtpAppraisalDate.SelectedDate!=null && cboAppraisalType.SelectedValue!= ""
                && cboStatus.SelectedValue!="" && cboArea.SelectedValue!=null && txtWeight.Value!=null)
            {
                contract.performanceContractStatusID = int.Parse(cboStatus.SelectedValue);
                appraisal.appraisalDate = dtpAppraisalDate.SelectedDate.Value;
                appraisal.performanceAppraisalTypeID = int.Parse(cboAppraisalType.SelectedValue);
                if (score == null)
                {
                    score = new coreLogic.performanceAppraisalScore
                    {
                        performanceContractItemID=editedItem.performanceContractItemID,
                        comments=txtMyComments.Text,
                        managerComments=""
                    };
                    if (chk0.Checked == true)
                    {
                        score.performanceScoreID = 0;
                    }
                    else if (chk1.Checked == true)
                    {
                        score.performanceScoreID = 1;
                    }
                    else if (chk2.Checked == true)
                    {
                        score.performanceScoreID = 2;
                    }
                    else if (chk3.Checked == true)
                    {
                        score.performanceScoreID = 3;
                    }
                    else if (chk4.Checked == true)
                    {
                        score.performanceScoreID = 4;
                    }
                    appraisal.performanceAppraisalScores.Add(score);
                }
                else
                {
                    score.comments = txtMyComments.Text;

                    if (chk0.Checked == true)
                    {
                        score.performanceScoreID = 0;
                    }
                    else if (chk1.Checked == true)
                    {
                        score.performanceScoreID = 1;
                    }
                    else if (chk2.Checked == true)
                    {
                        score.performanceScoreID = 2;
                    }
                    else if (chk3.Checked == true)
                    {
                        score.performanceScoreID = 3;
                    }
                    else if (chk4.Checked == true)
                    {
                        score.performanceScoreID = 4;
                    }
                }

                if (appraisal.performanceAppraisalID<=0)
                {
                    le.performanceAppraisals.Add(appraisal);
                }
                le.SaveChanges();
                chk0.Checked = false;
                chk1.Checked = false;
                chk2.Checked = false;
                chk3.Checked = false;
                chk4.Checked = false;
                txtWeight.Value = null;
                cboArea.SelectedValue = "";
                cboStatus.SelectedValue = "";
                txtMyComments.Text = "";
                Session["performanceContractItem"] = null;
                Session["performanceAppraisalScore"] = null;
                Bind();
                pnlAddEdit.Visible = false;
            }
        }

        private void Bind()
        {
            if (contract != null)
            {
                eds1.WhereParameters[0].DefaultValue = contract.performanceContractID.ToString();
                RadGrid1.DataBind();
            }
        }

        protected void RadGrid1_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "E")
            {
                var id = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["performanceContractItemID"].ToString());
                var item = contract.performanceContractItems.FirstOrDefault(p => p.performanceContractItemID == id);
                if (item != null)
                {
                    txtWeight.Value = item.weight;
                    txtDesc.Text = item.itemDescription;
                    cboArea.SelectedValue = item.performanceAreaID.ToString();
                    score = item.performanceAppraisalScores.FirstOrDefault();

                    if (score != null)
                    {
                        if (score.performanceScoreID == 0)
                        {
                            chk0.Checked = true;
                        }
                        else if (score.performanceScoreID == 1)
                        {
                            chk1.Checked = true;
                        }
                        else if (score.performanceScoreID == 2)
                        {
                            chk2.Checked = true;
                        }
                        else if (score.performanceScoreID == 3)
                        {
                            chk3.Checked = true;
                        }
                        else if (score.performanceScoreID == 4)
                        {
                            chk4.Checked = true;
                        }
                        txtMyComments.Text = score.comments;
                    }

                    Session["performanceContractItem"] = item;
                    Session["performanceAppraisalScore"] = score;
                    pnlAddEdit.Visible = true;
                    btnAddEdit.Text = "Modify KPI Score";
                }
            }
            else if (e.CommandName == "D")
            {
                var id = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["performanceContractItemID"].ToString());
                var item = contract.performanceContractItems.FirstOrDefault(p => p.performanceContractItemID == id);
                if (item != null)
                {
                    for (int i = item.performanceContractTargets.Count - 1; i >= 0;i-- )
                    {
                        var r = item.performanceContractTargets.ToList()[i];
                        le.performanceContractTargets.Remove(r);
                    } 
                    le.performanceContractItems.Remove(item);
                    le.SaveChanges();
                    RadGrid1.DataBind();
                }
            }
        }

        private void RenderInnerTree(RadTreeNode node2)
        {
            var id = int.Parse(node2.Value.Split(':')[1]);
            var lns = (from l in le.performanceContracts
                       where l.performanceContractID == id
                            && l.staff.userName.ToLower().Trim() == User.Identity.Name.ToLower().Trim()
                       select l).FirstOrDefault();
            //lns.performanceAppraisals.Load();
            RadTreeNode node3;
            foreach (var ln in lns.performanceAppraisals)
            {
                //ln.performanceAppraisalTypeReference.Load();
                node3 = new RadTreeNode(ln.performanceAppraisalType.performanceAppraisalTypeName,
                    ln.performanceAppraisalID.ToString());
                node3.NavigateUrl = "/hc/ipf/performanceAppraisal.aspx?id=" + ln.performanceContractID.ToString()
                    + "&aid=" + ln.performanceAppraisalID.ToString();
                node3.ImageUrl = "~/images/new.jpg";
                node3.ExpandedImageUrl = "~/images/new.jpg";
                node2.Nodes.Add(node3);
            }
            node3 = new RadTreeNode("Perform New Appriasal",
                    "-1");
            node3.NavigateUrl = "/hc/ipf/performanceAppraisal.aspx?id=" + lns.performanceContractID.ToString() ;
            node3.ImageUrl = "~/images/new.jpg";
            node3.ExpandedImageUrl = "~/images/new.jpg";
            node2.Nodes.Add(node3);
        }

        private void RenderTree(RadTreeNode node2)
        {
            var year =int.Parse(node2.Value.Split(':')[1]); 
            var lns = (from l in le.performanceContracts
                       where l.startDate.Year == year
                            && l.staff.userName.ToLower().Trim()==User.Identity.Name.ToLower().Trim()
                       select l).ToList();
            foreach (var ln in lns)
            {
                //ln.performanceContractStatuReference.Load(); 
                RadTreeNode node3 = new RadTreeNode(ln.startDate.ToString("dd-MMM-yyyy") +
                    " | " + ln.performanceContractStatu.performanceContractStatusName,
                    "c:" + ln.performanceContractID.ToString());
                node3.ImageUrl = "~/images/tree/folder_open.jpg";
                node3.ExpandedImageUrl = "~/images/tree/folder_open.jpg";
                node3.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                node2.Nodes.Add(node3);
            }
        }

        private void RenderTree()
        {
            var rnode = new RadTreeNode("My Performance Contracts", "_root_");
            rnode.ImageUrl = "~/images/tree/folder_open.jpg";
            rnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
            tree.Nodes.Add(rnode);
            rnode.Expanded = true;
            RadTreeNode node = null;

            var years = (from l in le.performanceContracts
                         where l.staff.userName.ToLower().Trim() == User.Identity.Name.ToLower().Trim()
                         select new { l.startDate.Year }).Distinct().OrderByDescending(p => p.Year).ToList();
            foreach (var year in years)
            {
                RadTreeNode node1 = new RadTreeNode(year.Year.ToString(), "y:" + year.Year.ToString());
                node1.ImageUrl = "~/images/tree/folder_open.jpg";
                node1.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                node1.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                rnode.Nodes.Add(node1); 
            }
        }

        private string MonthName(int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "Jan";
                    break;
                case 2:
                    monthName = "Feb";
                    break;
                case 3:
                    monthName = "Mar";
                    break;
                case 4:
                    monthName = "Apr";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "Jun";
                    break;
                case 7:
                    monthName = "Jul";
                    break;
                case 8:
                    monthName = "Aug";
                    break;
                case 9:
                    monthName = "Sep";
                    break;
                case 10:
                    monthName = "Oct";
                    break;
                case 11:
                    monthName = "Nov";
                    break;
                case 12:
                    monthName = "Dec";
                    break;
            }

            return monthName;
        }

        protected void tree_NodeExpand(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("y:"))
            {
                RenderTree(e.Node);
            }
            else
            {
                RenderInnerTree(e.Node);
            }
        }

    }
}