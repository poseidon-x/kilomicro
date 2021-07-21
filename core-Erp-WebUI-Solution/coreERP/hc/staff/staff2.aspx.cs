using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.hc.staff
{
    public partial class staff2 : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.staff cl;
        coreLogic.staffAddress phyAddr;
        coreLogic.staffAddress mailAddr;
        coreLogic.staffPhone workPhone;
        coreLogic.staffPhone homePhone;
        coreLogic.staffPhone mobilePhone;
        coreLogic.staffManager org;
        List<coreLogic.staffDocument> documents;
        List<coreLogic.staffQualification> qualifications;
        List<coreLogic.staffRelation> relations;
        private IIDGenerator idGen;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            idGen = new IDGenerator();
            if (!IsPostBack)
            {
                Session["le"] = le;
                cboMaritalStatus.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.maritalStatus)
                {
                    cboMaritalStatus.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.maritalStatusName, r.maritalStatusID.ToString()));
                }
                cboCategory.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffCategories)
                {
                    cboCategory.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.staffCategoryName, r.staffCategoryID.ToString()));
                }
                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches)
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
                cboJobTitle.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.jobTitles)
                {
                    cboJobTitle.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.jobTitleName, r.jobTitleID.ToString()));
                }
                int? id2=null;
                if (Request.Params["id"] != null && Request.Params["id"] != "")
                {
                    id2 = int.Parse(Request.Params["id"]);
                }
                cboStaffManager.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.staffs.Where(p=> p.staffID!= id2).OrderBy(p=>p.surName).ThenBy(p=>p.otherNames))
                {
                    cboStaffManager.Items.Add(new RadComboBoxItem(r.surName + ", " + r.otherNames, r.staffID.ToString()));
                }
                cboLevel.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.levels.OrderBy(p=>p.sortOrder))
                {
                    cboLevel.Items.Add(new RadComboBoxItem(r.levelName, r.levelID.ToString()));
                }
                cboQualificationType.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.qualificationTypes.OrderBy(p => p.qualificationTypeName))
                {
                    cboQualificationType.Items.Add(new RadComboBoxItem(r.qualificationTypeName, r.qualificationTypeID.ToString()));
                }
                cboQualificationSubject.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.qualificationSubjects.OrderBy(p => p.qualificationSubjectName))
                {
                    cboQualificationSubject.Items.Add(new RadComboBoxItem(r.qualificationSubjectName, r.qualificationSubjectID.ToString()));
                }
                cboRelationType.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.relationTypes.OrderBy(p => p.relationTypeName))
                {
                    cboRelationType.Items.Add(new RadComboBoxItem(r.relationTypeName, r.relationTypeID.ToString()));
                }
                cboEmploymentStatus.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.employmentStatus.OrderBy(p => p.employmentStatusName))
                {
                    cboEmploymentStatus.Items.Add(new RadComboBoxItem(r.employmentStatusName, r.employmentStatusID.ToString()));
                } 
                int id=0;
                if (Request.Params["id"] != null && int.TryParse(Request.Params["id"], out id))
                {
                    cl = le.staffs.FirstOrDefault(p => p.staffID == id);
                    if (cl != null)
                    {
                        //cl.staffAddresses.Load();
                        //cl.staffPhones.Load();
                        //cl.staffImages.Load();
                        //cl.staffDocuments.Load();
                        //cl.staffManagers.Load();
                        //cl.staffQualifications.Load();
                        //cl.staffRelations.Load();
                        //cl.staffManagers1.Load();

                        foreach (var r in cl.staffImages)
                        {
                            //r.imageReference.Load();
                        }

                        phyAddr = cl.staffAddresses.FirstOrDefault(p => p.addressTypeID == 1);
                        mailAddr = cl.staffAddresses.FirstOrDefault(p => p.addressTypeID == 2);

                        workPhone = cl.staffPhones.FirstOrDefault(p => p.phoneTypeID == 1);
                        mobilePhone = cl.staffPhones.FirstOrDefault(p => p.phoneTypeID == 2);
                        homePhone = cl.staffPhones.FirstOrDefault(p => p.phoneTypeID == 3);

                        org = cl.staffManagers1.FirstOrDefault();

                        documents = cl.staffDocuments.ToList();
                        foreach (var i in documents)
                        {
                            //i.documentReference.Load();
                        }
                        Session["staffDocuments"] = documents;
                        gridDocument.DataSource = documents;
                        gridDocument.DataBind();

                        qualifications = cl.staffQualifications.ToList();
                        foreach (var i in qualifications)
                        {
                            //i.qualificationTypeReference.Load();
                            //i.qualificationSubjectReference.Load();
                        }
                        Session["qualifications"] = qualifications;
                        gridQualification.DataSource = qualifications;
                        gridQualification.DataBind();


                        relations = cl.staffRelations.ToList();
                        foreach (var i in relations)
                        {
                            //i.relationTypeReference.Load();
                        }
                        Session["relations"] = relations;
                        gridRelation.DataSource = relations;
                        gridRelation.DataBind();
                         
                        cboBranch.SelectedValue = cl.branchID.ToString();
                        cboCategory.SelectedValue = cl.staffCategoryID.ToString();

                        cboMaritalStatus.SelectedValue = cl.maritalStatusID.ToString();
                        cboJobTitle.SelectedValue = cl.jobTitleID.ToString();
                        txtAccNum.Text = cl.staffNo;
                        txtOtherNames.Text = cl.otherNames;
                        txtSurname.Text = cl.surName;
                        dpDOB.SelectedDate = cl.DOB;
                        rblSex.SelectedValue = cl.sex;
                        dtpEmploymentStartDate.SelectedDate = cl.employmentStartDate;
                        if (cl.userName != null)
                        {
                            txtUserName.Text = cl.userName;
                        }
                        if (cl.employmentStatusID != null)
                        {
                            cboEmploymentStatus.SelectedValue = cl.employmentStatusID.ToString();
                        }
                        if (phyAddr != null)
                        {
                            //phyAddr.addressReference.Load();
                            txtPhyAddr1.Text = phyAddr.address.addressLine1;
                            txtPhyAddr2.Text = phyAddr.address.addressLine2;
                            txtPhyCityTown.Text = phyAddr.address.cityTown;
                            //phyAddr.address.addressImages.Load();
                            foreach (var item in phyAddr.address.addressImages)
                            {
                                //item.imageReference.Load();
                                RadBinaryImage img = new RadBinaryImage();
                                img.Width = 320;
                                img.Height = 180;
                                img.ResizeMode = BinaryImageResizeMode.Fit;
                                img.DataValue = item.image.image1;
                                RadRotatorItem it = new RadRotatorItem();
                                it.Controls.Add(img);
                                rotator1.Items.Add(it);
                            }
                        }
                        foreach (var item in cl.staffImages)
                        {
                            //item.imageReference.Load();
                            RadBinaryImage img = new RadBinaryImage();
                            img.Width = 320;
                            img.Height = 180;
                            img.ResizeMode = BinaryImageResizeMode.Fit;
                            img.DataValue = item.image.image1;
                            RadRotatorItem it = new RadRotatorItem();
                            it.Controls.Add(img);
                            rotator2.Items.Add(it);
                        }
                        if (mailAddr != null)
                        {
                            //mailAddr.addressReference.Load();
                            txtMailAddr1.Text = mailAddr.address.addressLine1;
                            txtMailAddr2.Text = mailAddr.address.addressLine2;
                            txtMailAddrCity.Text = mailAddr.address.cityTown;
                        }
                        if (workPhone != null)
                        {
                            //workPhone.phoneReference.Load();
                            txtWorkPhone.Text = workPhone.phone.phoneNo;
                        }
                        if (mobilePhone != null)
                        {
                            //mobilePhone.phoneReference.Load();
                            txtMobilePhone.Text = mobilePhone.phone.phoneNo;
                        }
                        if (homePhone != null)
                        {
                            //homePhone.phoneReference.Load();
                            txtHomePhone.Text = homePhone.phone.phoneNo;
                        }

                        if (org != null)
                        {
                            if (org.managerStaffID != null) cboStaffManager.SelectedValue = org.managerStaffID.ToString();
                            cboLevel.SelectedValue = org.levelID.ToString();
                            cboLevel_SelectedIndexChanged(cboLevel, null);
                            if (org.levelNotchID != null) cboNotch.SelectedValue = org.levelNotchID.ToString();
                        }
                    }
                }
                else
                {
                    cl = new coreLogic.staff();
                    documents = new List<coreLogic.staffDocument>();
                    Session["staffDocuments"] = documents;
                    qualifications = new List<coreLogic.staffQualification>();
                    Session["qualifications"] = qualifications; ;
                    relations = new List<coreLogic.staffRelation>();
                    Session["relations"] = relations; 
                }
                Session["loan.cl"] = cl;
                multi1.SelectedIndex = 0;
                tab1.SelectedIndex = 0;
            }
            else{
                if (Session["staffDocuments"] != null)
                    {
                        documents = Session["staffDocuments"] as List<coreLogic.staffDocument>;
                    }
                    else
                    {
                        documents = new List<coreLogic.staffDocument>();
                        Session["staffDocuments"] = documents;
                    }
                if (Session["qualifications"] != null)
                {
                    qualifications = Session["qualifications"] as List<coreLogic.staffQualification>;
                }
                else
                {
                    qualifications = new List<coreLogic.staffQualification>();
                    Session["qualifications"] = qualifications;
                }
                if (Session["relations"] != null)
                {
                    relations = Session["relations"] as List<coreLogic.staffRelation>;
                }
                else
                {
                    relations = new List<coreLogic.staffRelation>();
                    Session["relations"] = relations;
                } 
                if (Session["loan.cl"] != null)
                {
                    cl = Session["loan.cl"] as coreLogic.staff;
                }
                else
                {
                    cl = new coreLogic.staff();
                } 
                if (Session["le"] != null)
                {
                    le = Session["le"] as coreLogic.coreLoansEntities;
                }
                else
                {
                    le = new coreLogic.coreLoansEntities();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cl != null)
            {
                if(cboBranch.SelectedValue!="")cl.branchID = int.Parse(cboBranch.SelectedValue);
                cl.staffCategoryID = int.Parse(cboCategory.SelectedValue); 
                cl.DOB = dpDOB.SelectedDate;  
                if(cboMaritalStatus.SelectedValue!="")cl.maritalStatusID = int.Parse(cboMaritalStatus.SelectedValue);
                cl.otherNames = txtOtherNames.Text;
                if (cboJobTitle.SelectedValue != "") cl.jobTitleID = int.Parse(cboJobTitle.SelectedValue);
                cl.sex = rblSex.SelectedValue;
                cl.surName = txtSurname.Text;
                if (txtUserName.Text != "")
                    cl.userName = txtUserName.Text;
                else
                    cl.userName = null;
                cl.employmentStartDate = dtpEmploymentStartDate.SelectedDate;
                if (cboEmploymentStatus.SelectedValue != "")
                {
                    cl.employmentStatusID = int.Parse(cboEmploymentStatus.SelectedValue);
                }
                if (cl.staffID <= 0)
                {
                    le.staffs.Add(cl);
                }
                var startDate = DateTime.Now;
                if (cl.employmentStartDate != null)
                {
                    startDate = cl.employmentStartDate.Value;
                }
                if (txtAccNum.Text == "")
                {
                    cl.staffNo = idGen.NewStaffNumber(cl.branchID.Value, startDate);
                }
                else
                {
                    cl.staffNo = txtAccNum.Text;
                }
                if (txtPhyAddr1.Text != "" && txtPhyCityTown.Text != "")
                {
                    phyAddr = cl.staffAddresses.FirstOrDefault(p => p.addressTypeID == 1);
                    if (phyAddr == null)
                    {
                        phyAddr = new coreLogic.staffAddress();
                        cl.staffAddresses.Add(phyAddr);
                        phyAddr.address = new coreLogic.address();
                    }
                    else
                    {
                        //phyAddr.addressReference.Load();
                    }
                    phyAddr.address.addressLine1 = txtPhyAddr1.Text;
                    phyAddr.address.addressLine2 = txtPhyAddr2.Text;
                    phyAddr.address.cityTown = txtPhyCityTown.Text;
                    phyAddr.addressTypeID = 1;
                    foreach (Telerik.Web.UI.UploadedFile item in upload1.UploadedFiles)
                    {
                        coreLogic.addressImage img = new coreLogic.addressImage();
                        byte[] b = new byte[item.InputStream.Length];
                        item.InputStream.Read(b, 0, b.Length);
                        
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                        System.Drawing.Image i2 = System.Drawing.Image.FromStream(ms);
                        i2 = i2.GetThumbnailImage(480, 480, null, IntPtr.Zero);
                        ms = null;
                        ms = new System.IO.MemoryStream();
                        i2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        b = ms.ToArray();
                        i2 = null;
                        ms = null;

                        var i = new coreLogic.image
                        {
                            description = item.FileName,
                            image1 = b,
                            content_type = item.ContentType
                        };
                        img.image = i;
                        phyAddr.address.addressImages.Add(img);
                    }
                }
                foreach (Telerik.Web.UI.UploadedFile item in upload3.UploadedFiles)
                {
                    coreLogic.staffImage img = new coreLogic.staffImage();
                    byte[] b = new byte[item.InputStream.Length];
                    item.InputStream.Read(b, 0, b.Length);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                    System.Drawing.Image i2 = System.Drawing.Image.FromStream(ms);
                    i2 = i2.GetThumbnailImage(480, 480, null, IntPtr.Zero);
                    ms = null;
                    ms = new System.IO.MemoryStream();
                    i2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    b = ms.ToArray();
                    i2 = null;
                    ms = null;

                    var i = new coreLogic.image
                    {
                        description = item.FileName,
                        image1 = b,
                        content_type = item.ContentType
                    };
                    img.image = i;
                    cl.staffImages.Add(img);
                }
                if (txtMailAddr1.Text != "" && txtMailAddrCity.Text != "")
                {
                    mailAddr = cl.staffAddresses.FirstOrDefault(p => p.addressTypeID == 2);
                    if (mailAddr == null)
                    {
                        mailAddr = new coreLogic.staffAddress();
                        cl.staffAddresses.Add(mailAddr);
                        mailAddr.address = new coreLogic.address();
                    }
                    else
                    {
                        //mailAddr.addressReference.Load();
                    }
                    mailAddr.address.addressLine1 = txtMailAddr1.Text;
                    mailAddr.address.addressLine2 = txtMailAddr2.Text;
                    mailAddr.address.cityTown = txtMailAddrCity.Text;
                    mailAddr.addressTypeID = 2;
                }
                if (txtWorkPhone.Text != "")
                {
                    workPhone = cl.staffPhones.FirstOrDefault(p => p.phoneTypeID == 1);
                    if (workPhone == null)
                    {
                        workPhone = new coreLogic.staffPhone();
                        cl.staffPhones.Add(workPhone);
                        workPhone.phone = new coreLogic.phone();
                    }
                    else
                    {
                        //workPhone.phoneReference.Load();
                    }
                    workPhone.phone.phoneNo = txtWorkPhone.Text;
                    workPhone.phoneTypeID = 1;
                    workPhone.phone.phoneTypeID = 1;
                }
                if (txtMobilePhone.Text != "")
                {
                    mobilePhone = cl.staffPhones.FirstOrDefault(p => p.phoneTypeID == 2);
                    if (mobilePhone == null)
                    {
                        mobilePhone = new coreLogic.staffPhone();
                        cl.staffPhones.Add(mobilePhone);
                        mobilePhone.phone = new coreLogic.phone();
                    }
                    else
                    {
                        //mobilePhone.phoneReference.Load();
                    }
                    mobilePhone.phone.phoneNo = txtMobilePhone.Text;
                    mobilePhone.phoneTypeID = 2;
                    mobilePhone.phone.phoneTypeID = 2;
                }
                if (txtHomePhone.Text != "")
                {
                    homePhone = cl.staffPhones.FirstOrDefault(p => p.phoneTypeID == 3);
                    if (homePhone == null)
                    {
                        homePhone = new coreLogic.staffPhone();
                        cl.staffPhones.Add(homePhone);
                        homePhone.phone = new coreLogic.phone();
                    }
                    else
                    {
                        //homePhone.phoneReference.Load();
                    }
                    homePhone.phone.phoneNo = txtMobilePhone.Text;
                    homePhone.phoneTypeID = 3;
                    homePhone.phone.phoneTypeID = 3;
                }
                org = cl.staffManagers1.FirstOrDefault();
                if (org == null)
                {
                    org = new coreLogic.staffManager();
                    org.staff = cl;
                    if (cl.staffID > 0)
                    {
                        org.staffID = cl.staffID;
                    }
                    cl.staffManagers1.Add(org);
                }
                if (cboStaffManager.SelectedValue!="")org.managerStaffID = int.Parse(cboStaffManager.SelectedValue);
                if (cboLevel.SelectedValue != "") org.levelID = int.Parse(cboLevel.SelectedValue);
                if (cboNotch.SelectedValue != "") org.levelNotchID = int.Parse(cboNotch.SelectedValue); 
                foreach (var i in qualifications)
                {
                    if (!cl.staffQualifications.Contains(i))
                    {
                        cl.staffQualifications.Add(i);
                    }
                }
                for (int i = cl.staffQualifications.Count - 1; i >= 0; i--)
                {
                    var r = cl.staffQualifications.ToList()[i];
                    if (qualifications.Contains(r) == false)
                    {
                        cl.staffQualifications.Remove(r);
                    }
                }
                foreach (var i in relations)
                {
                    if (!cl.staffRelations.Contains(i))
                    {
                        cl.staffRelations.Add(i);
                    }
                }
                for (int i = cl.staffRelations.Count - 1; i >= 0; i--)
                {
                    var r = cl.staffRelations.ToList()[i];
                    if (relations.Contains(r) == false)
                    {
                        cl.staffRelations.Remove(r);
                    }
                } 
                le.SaveChanges(); 
                HtmlHelper.MessageBox2("Staff Data saved successfully!", ResolveUrl("~/hc/staff/default2.aspx"), "coreERP©: Successful", IconType.ok);
            }
            else
            {
                HtmlHelper.MessageBox("Data Is Incomplete. Please correct and try saving again!", "coreERP©: Failed", IconType.deny);
            }
        }

        private void GTA(object sender, EventArgs e)
        {
        }

        protected void rotator1_ItemDataBound(object sender, Telerik.Web.UI.RadRotatorEventArgs e)
        {

        }

        protected void cboCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        { 
        }
 
        protected void gridDocument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
            }
            else if (e.CommandName == "DeleteItem")
            {
                documents.RemoveAt(e.Item.ItemIndex);
            }
            gridDocument.DataSource = documents;
            gridDocument.DataBind();
        }

        protected void btnAddDcoument_Click(object sender, EventArgs e)
        {
            if (txtDocDesc.Text != "")
            {
                if (upload4.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile item in upload4.UploadedFiles)
                    {
                        byte[] b = new byte[item.InputStream.Length];
                        item.InputStream.Read(b, 0, b.Length);


                        var i = new coreLogic.document
                        {
                            description = txtDocDesc.Text,
                            documentImage = b,
                            contentType = item.ContentType,
                            fileName = item.FileName
                        };
                        var g = new coreLogic.staffDocument
                        {
                            staff = cl,
                            document = i
                        };
                        documents.Add(g);
                    }
                }
                Session["staffDocuments"] = documents;
                gridDocument.DataSource = documents;
                gridDocument.DataBind();

                txtDocDesc.Text = "";
                btnAddDcoument.Text = "Add Document";
            }
        }
         
        protected void btnAddQualification_Click(object sender, EventArgs e)
        {
            if (cboQualificationType.SelectedValue != "" && cboQualificationSubject.SelectedValue!= "")
            {
                coreLogic.staffQualification g;
                if (btnAddQualification.Text == "Add Qualification")
                {
                    g = new coreLogic.staffQualification();
                }
                else
                {
                    g = Session["qualification"] as coreLogic.staffQualification;
                }
                int id = int.Parse(cboQualificationType.SelectedValue);
                int id2 = int.Parse(cboQualificationSubject.SelectedValue);
                g.qualificationType = le.qualificationTypes.FirstOrDefault(p => p.qualificationTypeID == id);
                g.qualificationSubject = le.qualificationSubjects.FirstOrDefault(p => p.qualificationSubjectID == id2);
                g.startDate = dtStartDate.SelectedDate;
                g.endDate = dtEndDate.SelectedDate;
                g.expiryDate = dtExpiryDate.SelectedDate;
                g.institutionName = txtInstitutionName.Text;

                if (btnAddQualification.Text == "Add Qualification")
                {
                    qualifications.Add(g);
                }
                Session["qualifications"] = qualifications;
                gridQualification.DataSource = qualifications;
                gridQualification.DataBind();

                txtInstitutionName.Text = "";
                cboQualificationSubject.SelectedValue = "";
                cboQualificationType.SelectedValue = "";
                dtExpiryDate.SelectedDate = null;
                dtEndDate.SelectedDate = null;
                dtStartDate.SelectedDate = null;

                btnAddQualification.Text = "Add Qualification";
            }
        }

        protected void gridQualification_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = qualifications[e.Item.ItemIndex];
                if (g != null)
                {
                    txtInstitutionName.Text = g.institutionName;
                    dtStartDate.SelectedDate = g.startDate;
                    dtExpiryDate.SelectedDate = g.expiryDate;
                    dtEndDate.SelectedDate = g.endDate;
                    cboQualificationType.SelectedValue = g.qualificationTypeID.ToString();
                    cboQualificationSubject.SelectedValue = g.qualificationSubjectID.ToString();

                    Session["qualification"] = g;
                    btnAddQualification.Text = "Update Qualification";
                    gridQualification.DataSource = qualifications;
                    gridQualification.DataBind();
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                qualifications.RemoveAt(e.Item.ItemIndex);
            }
            gridQualification.DataSource = qualifications;
            gridQualification.DataBind();
        }
         
        protected void btnAddRelation_Click(object sender, EventArgs e)
        {
            if (cboRelationType.SelectedValue != "" && txtRelationSurname.Text != "" && txtRelationOtherNames.Text!="")
            {
                coreLogic.staffRelation g;
                if (btnAddRelation.Text == "Add Relation")
                {
                    g = new coreLogic.staffRelation();
                }
                else
                {
                    g = Session["relation"] as coreLogic.staffRelation;
                }
                int id = int.Parse(cboRelationType.SelectedValue);
                g.relationType = le.relationTypes.FirstOrDefault(p => p.relationTypeID == id);
                g.surName = txtRelationSurname.Text;
                g.otherNames = txtRelationOtherNames.Text;
                g.dob = dtRelationDOB.SelectedDate;

                if (btnAddRelation.Text == "Add Relation")
                {
                    relations.Add(g);
                }
                Session["relations"] = relations;
                gridRelation.DataSource = relations;
                gridRelation.DataBind();

                txtRelationOtherNames.Text = "";
                txtRelationSurname.Text = "";
                dtRelationDOB.SelectedDate = null;
                cboRelationType.SelectedValue = "";

                btnAddRelation.Text = "Add Relation";
            }
        }

        protected void gridRelation_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = relations[e.Item.ItemIndex];
                if (g != null)
                {
                    txtRelationOtherNames.Text = g.otherNames;
                    txtRelationSurname.Text = g.surName;
                    dtRelationDOB.SelectedDate = g.dob;
                    cboRelationType.SelectedValue = g.relationTypeID.ToString();

                    Session["relation"] = g;
                    btnAddRelation.Text = "Update Relation";
                    gridRelation.DataSource = relations;
                    gridRelation.DataBind();
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                relations.RemoveAt(e.Item.ItemIndex);
            }
            gridRelation.DataSource = relations;
            gridRelation.DataBind();
        }

        protected void cboLevel_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboLevel.SelectedValue != null)
            {
                int id = int.Parse(cboLevel.SelectedValue);
                var level = le.levels.FirstOrDefault(p => p.levelID == id);
                if (level != null)
                {
                    //level.levelNotches.Load();
                    cboNotch.Items.Clear();
                    cboNotch.Items.Add(new RadComboBoxItem("", ""));
                    foreach (var r in level.levelNotches.OrderBy(p => p.notchName))
                    {
                        cboNotch.Items.Add(new RadComboBoxItem(r.notchName, r.levelNotchID.ToString()));
                    }
                }
            }
        }
    }
}