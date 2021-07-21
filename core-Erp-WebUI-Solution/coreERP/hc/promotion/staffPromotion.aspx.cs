using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.hc.promotion
{
    public partial class staffPromotion : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!IsPostBack)
            {
                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffs.OrderBy(p => p.surName).ThenBy(p => p.otherNames))
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName+ ", " + r.otherNames
                        + " (" + r.staffNo + ")", r.staffID.ToString()));
                }
                if (Request.Params["sid"] != null && Request.Params["id"]==null)
                {
                    cboStaff.SelectedValue = Request.Params["sid"];
                    cboStaff_SelectedIndexChanged(cboStaff, new RadComboBoxSelectedIndexChangedEventArgs("", "", "", ""));
                }
                cboOldLevel.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.levels.OrderBy(p => p.levelName))
                {
                    cboOldLevel.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.levelName, r.levelID.ToString()));
                }
                cboNewLevel.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.levels.OrderBy(p => p.levelName))
                {
                    cboNewLevel.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.levelName, r.levelID.ToString()));
                }
                cboOldJobTitle.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.jobTitles.OrderBy(p => p.jobTitleName))
                {
                    cboOldJobTitle.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.jobTitleName, r.jobTitleID.ToString()));
                }
                cboNewJobTitle.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.jobTitles.OrderBy(p => p.jobTitleName))
                {
                    cboNewJobTitle.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.jobTitleName, r.jobTitleID.ToString()));
                }
                cboOldManagerStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffs.OrderBy(p => p.surName).ThenBy(p => p.otherNames))
                {
                    cboOldManagerStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames
                        + " (" + r.staffNo + ")", r.staffID.ToString()));
                }
                cboNewManagerStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffs.OrderBy(p => p.surName).ThenBy(p => p.otherNames))
                {
                    cboNewManagerStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames
                        + " (" + r.staffNo + ")", r.staffID.ToString()));
                }
                cboOldNotch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.levelNotches.OrderBy(p => p.notchName))
                {
                    cboOldNotch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.notchName, r.levelNotchID.ToString()));
                }
                dtPromotionDate.SelectedDate = DateTime.Now;
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    var promotion = le.staffPromotions.FirstOrDefault(p => p.staffPromotionID == id);
                    if (promotion != null)
                    {
                        cboStaff.SelectedValue = promotion.staffID.ToString();
                        cboOldJobTitle.SelectedValue =((promotion.oldJobTitleID==null)?"":
                                    promotion.oldJobTitleID.ToString());
                        cboNewJobTitle.SelectedValue = ((promotion.newJobTitleID == null) ? "" :
                                    promotion.newJobTitleID.ToString());
                        cboOldLevel.SelectedValue = ((promotion.oldLevelID == null) ? "" :
                                    promotion.oldLevelID.ToString());
                        cboNewLevel.SelectedValue = ((promotion.newLevelID == null) ? "" :
                                    promotion.newLevelID.ToString());
                        cboOldNotch.SelectedValue = ((promotion.oldNotchID == null) ? "" :
                                    promotion.oldNotchID.ToString());
                        cboNewNotch.SelectedValue = ((promotion.newNotchID == null) ? "" :
                                    promotion.newNotchID.ToString());
                        cboOldManagerStaff.SelectedValue = ((promotion.oldManagerStaffID == null) ? "" :
                                    promotion.oldManagerStaffID.ToString());
                        cboNewManagerStaff.SelectedValue = ((promotion.newManagerStaffID == null) ? "" :
                                    promotion.newManagerStaffID.ToString());
                        fsNewDetails.Visible = true;
                        fsOldDetails.Visible = true;
                    }
                }
                RenderTree();
            }
        }

        protected void cboNewStaffLevel_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboNewLevel.SelectedValue != "")
            {
                var id = int.Parse(cboNewLevel.SelectedValue);
                cboNewNotch.Items.Clear();
                cboNewNotch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.levelNotches.Where(p=>p.levelID==id).OrderBy(p => p.notchName))
                {
                    cboNewNotch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.notchName, r.levelNotchID.ToString()));
                }
            }
        }

        protected void cboStaff_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboStaff.SelectedValue != "")
            {
                int id = int.Parse(cboStaff.SelectedValue);
                var staff = le.staffs.FirstOrDefault(p => p.staffID == id);
                if (staff != null)
                {
                    //staff.staffManagers1.Load();
                    var org = staff.staffManagers1.FirstOrDefault();
                    if (org != null)
                    {
                        cboOldJobTitle.SelectedValue = staff.jobTitleID.ToString();
                        cboOldLevel.SelectedValue = org.levelID.ToString();
                        cboOldNotch.SelectedValue = ((org.levelNotchID == null) ? "" : org.levelNotchID.ToString());
                        cboOldManagerStaff.SelectedValue = ((org.managerStaffID == null) ? "" : org.managerStaffID.ToString());
                    }
                    fsNewDetails.Visible = true;
                    fsOldDetails.Visible = true;
                }
            }
        }

        private void RenderTree()
        {
            string[] str = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string[] str2 = new string[] { "ABC", "DEF", "GHI", "JKL", "MNO", "PQR", "STU", "VWX", "YZ" };
            var rnode = new RadTreeNode("Staff", "_root_");
            rnode.ImageUrl = "~/images/tree/folder_open.jpg";
            rnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
            tree.Nodes.Add(rnode);
            rnode.Expanded = true;
            RadTreeNode node = null;
            int j = 0;
            for (int i = 0; i < 26; i++)
            {
                if (i % 3 == 0)
                {
                    node = new RadTreeNode(str2[j], str2[j]);
                    j++;
                    node.ImageUrl = "~/images/tree/folder_open.jpg";
                    node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    rnode.Nodes.Add(node);
                }
                string str3 = str[i];
                RadTreeNode node2 = new RadTreeNode(str3, "s:" + str3);
                node2.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                node2.ImageUrl = "~/images/tree/folder_open.jpg";
                node2.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                node.Nodes.Add(node2);
            }
        }

        private void RenderTree(RadTreeNode node2)
        {
            var str3 = node2.Value.Split(':')[1];
            var staffs = le.staffs.Where(p => p.surName.StartsWith(str3) || p.surName.StartsWith(str3.ToLower())).ToList();
            foreach (var cl in staffs)
            {
                RadTreeNode node3 = new RadTreeNode(cl.surName + ", " + cl.otherNames, "i:" + cl.staffNo);
                node3.ImageUrl = "~/images/tree/folder_open.jpg";
                node3.ExpandedImageUrl = "~/images/tree/folder_open.jpg";
                node3.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                node2.Nodes.Add(node3);
            }
        }

        private void RenderInnerTree(RadTreeNode node2)
        {
            var str3 = node2.Value.Split(':')[1];
            var staff = le.staffs.Where(p => p.staffNo==str3).FirstOrDefault();
            //staff.staffPromotions2.Load();
            RadTreeNode node3;
            foreach (var cl in staff.staffPromotions2)
            {
                node3 = new RadTreeNode(cl.promotionDate.ToString("dd-MMM-yyyy"), cl.staffPromotionID.ToString());
                node3.ImageUrl = "~/images/new.jpg";
                node3.ExpandedImageUrl = "~/images/new.jpg";
                node3.NavigateUrl = "~/hc/promotion/staffPromotion.aspx?id=" + cl.staffPromotionID.ToString();
                node2.Nodes.Add(node3);
            }
            node3 = new RadTreeNode("Promote " + staff.otherNames, "-1-" + staff.staffID.ToString());
            node3.ImageUrl = "~/images/new.jpg";
            node3.ExpandedImageUrl = "~/images/new.jpg";
            node3.NavigateUrl = "~/hc/promotion/staffPromotion.aspx?sid=" + staff.staffID.ToString();
            node2.Nodes.Add(node3);
        }

        protected void tree_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("s:"))
            {
                RenderTree(e.Node);
            }
            else
            {
                RenderInnerTree(e.Node);
            }
        }

        protected void btnPromote_Click(object sender, EventArgs e)
        {
            if (cboStaff.SelectedValue != "" && dtPromotionDate.SelectedDate != null && cboNewLevel.SelectedValue!="")
            {
                coreLogic.staffPromotion promotion;
                coreLogic.staffManager org;
                int sid = int.Parse(cboStaff.SelectedValue);
                var staff = le.staffs.FirstOrDefault(p => p.staffID == sid);
                //staff.staffManagers1.Load();
                org = staff.staffManagers1.FirstOrDefault();
                if (org == null)
                {
                    org = new coreLogic.staffManager();
                    org.staffID = staff.staffID;
                    le.staffManagers.Add(org);
                }
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    promotion = le.staffPromotions.FirstOrDefault(p => p.staffPromotionID == id);
                }
                else
                {
                    promotion = new coreLogic.staffPromotion();
                    promotion.creationDate = DateTime.Now;
                    promotion.promotedBy = User.Identity.Name;
                    promotion.promotionDate = dtPromotionDate.SelectedDate.Value;
                    promotion.staffID = int.Parse(cboStaff.SelectedValue);
                    le.staffPromotions.Add(promotion);
                }
                if (cboNewJobTitle.SelectedValue != "")
                {
                    promotion.newJobTitleID = int.Parse(cboNewJobTitle.SelectedValue);
                    staff.jobTitleID = int.Parse(cboNewJobTitle.SelectedValue);
                }
                if (cboOldJobTitle.SelectedValue != "")
                {
                    promotion.oldJobTitleID = int.Parse(cboOldJobTitle.SelectedValue);
                }
                if (cboNewLevel.SelectedValue != "")
                {
                    promotion.newLevelID = int.Parse(cboNewLevel.SelectedValue);
                    org.levelID = int.Parse(cboNewLevel.SelectedValue);
                }
                if (cboOldLevel.SelectedValue != "")
                {
                    promotion.oldLevelID = int.Parse(cboOldLevel.SelectedValue);
                }
                if (cboOldNotch.SelectedValue != "")
                {
                    promotion.oldNotchID = int.Parse(cboOldNotch.SelectedValue);
                }
                if (cboNewNotch.SelectedValue != "")
                {
                    promotion.newNotchID = int.Parse(cboNewNotch.SelectedValue);
                    org.levelNotchID = int.Parse(cboNewNotch.SelectedValue);
                }
                if (cboNewManagerStaff.SelectedValue != "")
                {
                    promotion.newManagerStaffID = int.Parse(cboNewManagerStaff.SelectedValue);
                    org.managerStaffID = int.Parse(cboNewManagerStaff.SelectedValue);
                }
                if (cboOldManagerStaff.SelectedValue != "")
                {
                    promotion.oldManagerStaffID = int.Parse(cboOldManagerStaff.SelectedValue);
                }
                le.SaveChanges();
                HtmlHelper.MessageBox2("Staff Promoted Successfully!", ResolveUrl("~/hc/promotion/staffPromotion.aspx"), "coreERP©: Successful", IconType.ok);
            }
            else
            {
                HtmlHelper.MessageBox("Data Is Incomplete. Please correct and try saving again!", "coreERP©: Failed", IconType.deny);
            }
        }

    }
}