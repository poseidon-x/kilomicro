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
    public partial class performanceContract : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.performanceContract contract;
        coreLogic.performanceContractItem editedItem;
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
                if (Request.Params["id"] != null)
                {
                    var id = int.Parse(Request.Params["id"]);
                    contract = le.performanceContracts.FirstOrDefault(p => p.performanceContractID == id);
                    if (contract != null)
                    {
                        var staff = le.staffs.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                        if (staff != null && staff.staffID!=contract.staffID)
                        {
                            HtmlHelper.MessageBox2("This performance contract does not belong to you.",
                                ResolveUrl("~/hc/ipf/performanceContract.aspx"), "coreERP: Failed", IconType.deny);
                            return;
                        }
                        //contract.performanceContractItems.Load();
                        foreach (var r in contract.performanceContractItems)
                        {
                           // r.performanceContractTargets.Load();
                        }
                        dtpEndDate.SelectedDate = contract.endDate;
                        dtpStartDate.SelectedDate = contract.startDate;
                        cboStatus.SelectedValue = contract.performanceContractStatusID.ToString();
                    }
                    Session["performanceContract"] = contract;
                    Bind();
                }
                else
                {
                    var staff = le.staffs.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                    if (staff != null)
                    {
                        contract = new coreLogic.performanceContract();
                        contract.staffID = staff.staffID;
                        Session["performanceContract"] = contract;
                    }
                }
                RenderTree();
            }
            else
            {
                if (Session["performanceContract"] != null)
                {
                    contract = Session["performanceContract"] as coreLogic.performanceContract;
                }
                if (Session["performanceContractItem"] != null)
                {
                    editedItem = Session["performanceContractItem"] as coreLogic.performanceContractItem;
                }
                if (Session["le"] != null)
                {
                    le = Session["le"] as coreLogic.coreLoansEntities;
                }
            }
        }

        protected void btnAddEdit_Click(object sender, EventArgs e)
        {
            if (contract != null && dtpStartDate.SelectedDate!=null && dtpEndDate.SelectedDate!=null
                && cboStatus.SelectedValue!="" && cboArea.SelectedValue!=null && txtWeight.Value!=null)
            {
                contract.startDate = dtpStartDate.SelectedDate.Value;
                contract.endDate = dtpEndDate.SelectedDate.Value;
                contract.performanceContractStatusID = int.Parse(cboStatus.SelectedValue);
                if (contract.performanceContractID <= 0)
                {
                    contract.enteredBy = User.Identity.Name;
                    contract.creationDate = DateTime.Now;
                }
                if (editedItem == null)
                {
                    editedItem = new coreLogic.performanceContractItem
                    {
                        performanceAreaID = int.Parse(cboArea.SelectedValue),
                        itemDescription = txtDesc.Text,
                        weight = txtWeight.Value.Value
                    };
                    editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                    {
                        performanceScoreID = 0,
                        targetCreteria = txt0.Text
                    });
                    editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                    {
                        performanceScoreID = 1,
                        targetCreteria = txt1.Text
                    });
                    editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                    {
                        performanceScoreID = 2,
                        targetCreteria = txt2.Text
                    });
                    editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                    {
                        performanceScoreID = 3,
                        targetCreteria = txt3.Text
                    });
                    editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                    {
                        performanceScoreID = 4,
                        targetCreteria = txt4.Text
                    });
                    contract.performanceContractItems.Add(editedItem);
                }
                else
                {
                    editedItem.performanceAreaID = int.Parse(cboArea.SelectedValue);
                    editedItem.itemDescription = txtDesc.Text;
                    editedItem.weight = txtWeight.Value.Value;

                    var t = editedItem.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 0);
                    if (t != null)
                    {
                        t.targetCreteria = txt0.Text;
                    }
                    else
                    {
                        editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                        {
                            performanceScoreID = 0,
                            targetCreteria = txt0.Text
                        });
                    }
                    t = editedItem.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 1);
                    if (t != null)
                    {
                        t.targetCreteria = txt1.Text;
                    }
                    else
                    {
                        editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                        {
                            performanceScoreID = 1,
                            targetCreteria = txt1.Text
                        });
                    }
                    t = editedItem.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 2);
                    if (t != null)
                    {
                        t.targetCreteria = txt2.Text;
                    }
                    else
                    {
                        editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                        {
                            performanceScoreID = 2,
                            targetCreteria = txt2.Text
                        });
                    }
                    t = editedItem.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 3);
                    if (t != null)
                    {
                        t.targetCreteria = txt3.Text;
                    }
                    else
                    {
                        editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                        {
                            performanceScoreID = 3,
                            targetCreteria = txt3.Text
                        });
                    }
                    t = editedItem.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 4);
                    if (t != null)
                    {
                        t.targetCreteria = txt4.Text;
                    }
                    else
                    {
                        editedItem.performanceContractTargets.Add(new coreLogic.performanceContractTarget
                        {
                            performanceScoreID = 4,
                            targetCreteria = txt4.Text
                        });
                    }
                }

                if (contract.performanceContractID <= 0)
                {
                    le.performanceContracts.Add(contract);
                }
                le.SaveChanges();
                txt0.Text = "";
                txt1.Text = "";
                txt2.Text = "";
                txt3.Text = "";
                txt4.Text = "";
                txtWeight.Value = null;
                cboArea.SelectedValue = "";
                cboStatus.SelectedValue = "";
                dtpEndDate.SelectedDate = null;
                dtpStartDate.SelectedDate = null;
                btnAddEdit.Text = "Add KPI Item";
                Session["performanceContractItem"] = null;
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

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            if (contract != null)
            {
                pnlAddEdit.Visible = true;
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

                    var t0 = item.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 0);
                    if (t0 != null)
                    {
                        txt0.Text = t0.targetCreteria;
                    }
                    t0 = item.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 1);
                    if (t0 != null)
                    {
                        txt1.Text = t0.targetCreteria;
                    }
                    t0 = item.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 2);
                    if (t0 != null)
                    {
                        txt2.Text = t0.targetCreteria;
                    }
                    t0 = item.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 3);
                    if (t0 != null)
                    {
                        txt3.Text = t0.targetCreteria;
                    }
                    t0 = item.performanceContractTargets.FirstOrDefault(p => p.performanceScoreID == 4);
                    if (t0 != null)
                    {
                        txt4.Text = t0.targetCreteria;
                    }
                    Session["performanceContractItem"] = item;
                    pnlAddEdit.Visible = true;
                    btnAddEdit.Text = "Update KPI Item";
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
                    ln.performanceContractID.ToString());
                node3.NavigateUrl = "/hc/ipf/performanceContract.aspx?id=" + ln.performanceContractID.ToString();
                node3.ImageUrl = "~/images/new.jpg";
                node3.ExpandedImageUrl = "~/images/new.jpg";
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
                RadTreeNode node1 = new RadTreeNode(year.Year.ToString(), "Y:" + year.Year.ToString());
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
            RenderTree(e.Node);
        }

    }
}