using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.hc.staff
{
    public partial class staff : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.staff cl;
        coreLogic.staffAddress phyAddr;
        coreLogic.staffAddress mailAddr;
        coreLogic.staffPhone workPhone;
        coreLogic.staffPhone homePhone;
        coreLogic.staffPhone mobilePhone;
        coreLogic.staffBenefit benefits;
        List<coreLogic.staffDocument> documents;
        List<coreLogic.staffAllowance> allowances;
        List<coreLogic.staffDeduction> deductions;
        List<coreLogic.staffBenefitsInKind> benefitsInKind;
        List<coreLogic.staffPension> pensions;
        List<coreLogic.staffTaxRelief> taxReliefs;
        coreLogic.staffManager org;
        private IIDGenerator idGen;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            idGen = new IDGenerator();
            if (!IsPostBack)
            {
                Session["le"] = le;
                int? id2 = null;
                if (Request.Params["id"] != null && Request.Params["id"] != "")
                {
                    id2 = int.Parse(Request.Params["id"]);
                }
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
                cboBank.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in ent.banks.OrderBy(p=>p.bank_name))
                {
                    cboBank.Items.Add(new RadComboBoxItem(r.bank_name, r.bank_name));
                }
                cboAllowanceType.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.allowanceTypes.OrderBy(p => p.alllowanceTypeName))
                {
                    cboAllowanceType.Items.Add(new RadComboBoxItem(r.alllowanceTypeName, r.allowanceTypeID.ToString()));
                }
                cboDeductionType.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.deductionTypes.OrderBy(p => p.deductionTypeName))
                {
                    cboDeductionType.Items.Add(new RadComboBoxItem(r.deductionTypeName, r.deductionTypeID.ToString()));
                }
                cboBenefitsInKind.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.benefitsInKinds.OrderBy(p => p.benefitsInKindName))
                {
                    cboBenefitsInKind.Items.Add(new RadComboBoxItem(r.benefitsInKindName, r.benefitsInKindID.ToString()));
                }
                cboPensionType.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.pensionTypes.OrderBy(p => p.pensionTypeName))
                {
                    cboPensionType.Items.Add(new RadComboBoxItem(r.pensionTypeName, r.pensionTypeID.ToString()));
                }
                cboEmploymentStatus.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.employmentStatus.OrderBy(p => p.employmentStatusName))
                {
                    cboEmploymentStatus.Items.Add(new RadComboBoxItem(r.employmentStatusName, r.employmentStatusID.ToString()));
                }
                cboStaffManager.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.staffs.Where(p => p.staffID != id2).OrderBy(p => p.surName).ThenBy(p => p.otherNames))
                {
                    cboStaffManager.Items.Add(new RadComboBoxItem(r.surName + ", " + r.otherNames, r.staffID.ToString()));
                }
                cboLevel.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.levels.OrderBy(p => p.sortOrder))
                {
                    cboLevel.Items.Add(new RadComboBoxItem(r.levelName, r.levelID.ToString()));
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
                        //cl.staffBenefits.Load();
                        //cl.staffAllowances.Load();
                       // cl.staffDeductions.Load();
                        //cl.staffBenefitsInKinds.Load();
                        //cl.staffPensions.Load();
                        //cl.staffTaxReliefs.Load();
                        //cl.staffManagers1.Load();
                        //cl.staffManagers.Load();

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

                        benefits = cl.staffBenefits.FirstOrDefault();

                        documents = cl.staffDocuments.ToList();
                        foreach (var i in documents)
                        {
                            //i.documentReference.Load();
                        }
                        Session["staffDocuments"] = documents;
                        gridDocument.DataSource = documents;
                        gridDocument.DataBind();

                        allowances = cl.staffAllowances.ToList();
                        foreach (var i in allowances)
                        {
                            //i.allowanceTypeReference.Load();
                        }
                        Session["allowances"] = allowances;
                        gridAllowance.DataSource = allowances;
                        gridAllowance.DataBind();


                        deductions = cl.staffDeductions.ToList();
                        foreach (var i in deductions)
                        {
                            //i.deductionTypeReference.Load();
                        }
                        Session["deductions"] = deductions;
                        gridDeduction.DataSource = deductions;
                        gridDeduction.DataBind();

                        taxReliefs = cl.staffTaxReliefs.ToList();
                        foreach (var i in taxReliefs)
                        {
                            //i.taxReliefTypeReference.Load();
                        }
                        Session["taxReliefs"] = taxReliefs;
                        gridTaxRelief.DataSource = taxReliefs;
                        gridTaxRelief.DataBind();

                        benefitsInKind = cl.staffBenefitsInKinds.ToList();
                        foreach (var i in benefitsInKind)
                        {
                            //i.benefitsInKindReference.Load();
                        }
                        Session["benefitsInKind"] = benefitsInKind;
                        gridBenefitsInKind.DataSource = benefitsInKind;
                        gridBenefitsInKind.DataBind();

                        pensions = cl.staffPensions.ToList();
                        foreach (var i in pensions)
                        {
                           // i.pensionTypeReference.Load();
                        }
                        Session["pensions"] = pensions;
                        gridPension.DataSource = pensions;
                        gridPension.DataBind();

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

                        if (benefits != null)
                        {
                            txtBankAccountNo.Text = benefits.bankAccountNo;
                            cboBank.SelectedValue = benefits.bankName;
                            cboBank_SelectedIndexChanged(cboBank, null);
                            cboBankBranch.SelectedValue = benefits.bankBranchName;
                            txtBasicSalary.Value = benefits.basicSalary;
                            txtSSN.Text = benefits.ssn;
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
                    allowances = new List<coreLogic.staffAllowance>();
                    Session["allowances"] = allowances; ;
                    deductions = new List<coreLogic.staffDeduction>();
                    Session["deductions"] = deductions;
                    benefitsInKind = new List<coreLogic.staffBenefitsInKind>();
                    Session["benefitsInKind"] = benefitsInKind;
                    pensions = new List<coreLogic.staffPension>();
                    Session["pensions"] = pensions;
                    taxReliefs = new List<coreLogic.staffTaxRelief>();
                    Session["taxReliefs"] = taxReliefs;
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
                if (Session["allowances"] != null)
                {
                    allowances = Session["allowances"] as List<coreLogic.staffAllowance>;
                }
                else
                {
                    allowances = new List<coreLogic.staffAllowance>();
                    Session["allowances"] = allowances;
                }
                if (Session["deductions"] != null)
                {
                    deductions = Session["deductions"] as List<coreLogic.staffDeduction>;
                }
                else
                {
                    deductions = new List<coreLogic.staffDeduction>();
                    Session["deductions"] = deductions;
                }
                if (Session["taxReliefs"] != null)
                {
                    taxReliefs = Session["taxReliefs"] as List<coreLogic.staffTaxRelief>;
                }
                else
                {
                    taxReliefs = new List<coreLogic.staffTaxRelief>();
                    Session["taxReliefs"] = taxReliefs;
                }
                if (Session["benefitsInKind"] != null)
                {
                    benefitsInKind = Session["benefitsInKind"] as List<coreLogic.staffBenefitsInKind>;
                }
                else
                {
                    benefitsInKind = new List<coreLogic.staffBenefitsInKind>();
                    Session["benefitsInKind"] = benefitsInKind;
                }
                if (Session["pensions"] != null)
                {
                    pensions = Session["pensions"] as List<coreLogic.staffPension>;
                }
                else
                {
                    pensions = new List<coreLogic.staffPension>();
                    Session["pensions"] = pensions;
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
                if (txtBasicSalary.Value != null)
                {
                    benefits = cl.staffBenefits.FirstOrDefault();
                    if (benefits == null)
                    {
                        benefits = new coreLogic.staffBenefit();
                        cl.staffBenefits.Add(benefits);
                    }
                    benefits.ssn = txtSSN.Text;
                    benefits.basicSalary = txtBasicSalary.Value.Value;
                    benefits.bankBranchName = cboBankBranch.SelectedValue;
                    benefits.bankName = cboBank.SelectedValue;
                    benefits.bankAccountNo = txtBankAccountNo.Text;
                } 
                org = cl.staffManagers1.FirstOrDefault();
                if (org == null)
                {
                    org = new coreLogic.staffManager();
                    org.staff= cl;
                    if (cl.staffID > 0)
                    {
                        org.staffID = cl.staffID;
                    }
                    cl.staffManagers1.Add(org);
                }
                if (cboStaffManager.SelectedValue!="") org.managerStaffID = int.Parse(cboStaffManager.SelectedValue);                   
                if (cboLevel.SelectedValue != "") org.levelID = int.Parse(cboLevel.SelectedValue);
                if (cboNotch.SelectedValue != "") org.levelNotchID = int.Parse(cboNotch.SelectedValue); 
                foreach (var i in allowances)
                {
                    if (!cl.staffAllowances.Contains(i))
                    {
                        cl.staffAllowances.Add(i);
                    }
                }
                for (int i = cl.staffAllowances.Count - 1; i >= 0; i--)
                {
                    var r = cl.staffAllowances.ToList()[i];
                    if (allowances.Contains(r) == false)
                    {
                        cl.staffAllowances.Remove(r);
                    }
                }
                foreach (var i in deductions)
                {
                    if (!cl.staffDeductions.Contains(i))
                    {
                        cl.staffDeductions.Add(i);
                    }
                }
                for (int i = cl.staffDeductions.Count - 1; i >= 0; i--)
                {
                    var r = cl.staffDeductions.ToList()[i];
                    if (deductions.Contains(r) == false)
                    {
                        cl.staffDeductions.Remove(r);
                    }
                }
                foreach (var i in benefitsInKind)
                {
                    if (!cl.staffBenefitsInKinds.Contains(i))
                    {
                        cl.staffBenefitsInKinds.Add(i);
                    }
                }
                for (int i = cl.staffBenefitsInKinds.Count - 1; i >= 0; i--)
                {
                    var r = cl.staffBenefitsInKinds.ToList()[i];
                    if (benefitsInKind.Contains(r) == false)
                    {
                        cl.staffBenefitsInKinds.Remove(r);
                    }
                }
                foreach (var i in pensions)
                {
                    if (!cl.staffPensions.Contains(i))
                    {
                        cl.staffPensions.Add(i);
                    }
                }
                for (int i = cl.staffPensions.Count - 1; i >= 0; i--)
                {
                    var r = cl.staffPensions.ToList()[i];
                    if (pensions.Contains(r) == false)
                    {
                        cl.staffPensions.Remove(r);
                    }
                }
                foreach (var i in taxReliefs)
                {
                    if (!cl.staffTaxReliefs.Contains(i))
                    {
                        cl.staffTaxReliefs.Add(i);
                    }
                }
                for (int i = cl.staffTaxReliefs.Count - 1; i >= 0; i--)
                {
                    var r = cl.staffTaxReliefs.ToList()[i];
                    if (taxReliefs.Contains(r) == false)
                    {
                        cl.staffTaxReliefs.Remove(r);
                    }
                }
                le.SaveChanges();
                Session["smeDirectors"] = null;
                HtmlHelper.MessageBox2("Staff Data saved successfully!", ResolveUrl("~/hc/staff/default.aspx"), "coreERP©: Successful", IconType.ok);
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

        protected void cboBank_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboBank.SelectedValue != null)
            {
                var bank = ent.banks.FirstOrDefault(p => p.bank_name == cboBank.SelectedValue);
                if (bank != null)
                {
                    //bank.bank_branches.Load();
                    cboBankBranch.Items.Clear();
                    cboBankBranch.Items.Add(new RadComboBoxItem("", ""));
                    foreach (var r in bank.bank_branches.OrderBy(p=>p.branch_name))
                    {
                        cboBankBranch.Items.Add(new RadComboBoxItem(r.branch_name, r.branch_name));
                    }
                }
            }
        }

        protected void btnAddAllowance_Click(object sender, EventArgs e)
        {
            if (cboAllowanceType.SelectedValue != "" && txtAllowanceAmount.Text != "")
            {
                coreLogic.staffAllowance g;
                if (btnAddAllowance.Text == "Add Allowance")
                {
                    g = new coreLogic.staffAllowance();
                }
                else
                {
                    g = Session["allowance"] as coreLogic.staffAllowance;
                }
                int id = int.Parse(cboAllowanceType.SelectedValue);
                g.allowanceType = le.allowanceTypes.FirstOrDefault(p => p.allowanceTypeID == id);
                g.amount = txtAllowanceAmount.Value.Value;
                g.percentValue = txtAllowancePercent.Value.Value;
                g.isEnabled = chkAllowanceEnabled.Checked;

                if (btnAddAllowance.Text == "Add Allowance")
                {
                    allowances.Add(g);
                }
                Session["allowances"] = allowances;
                gridAllowance.DataSource = allowances;
                gridAllowance.DataBind();

                txtAllowanceAmount.Value = 0;
                txtAllowancePercent.Value = 0;

                btnAddAllowance.Text = "Add Allowance";
            }
        }

        protected void gridAllowance_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = allowances[e.Item.ItemIndex];
                if (g != null)
                {
                    txtAllowanceAmount.Value = g.amount;
                    txtAllowancePercent.Value = g.percentValue;
                    cboAllowanceType.SelectedValue = g.allowanceTypeID.ToString();
                    chkAllowanceEnabled.Checked = g.isEnabled;

                    Session["allowance"] = g;
                    btnAddAllowance.Text = "Update Allowance";
                    gridAllowance.DataSource = allowances;
                    gridAllowance.DataBind();
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                allowances.RemoveAt(e.Item.ItemIndex);
            }
            gridAllowance.DataSource = allowances;
            gridAllowance.DataBind();
        }

        protected void btnAddPension_Click(object sender, EventArgs e)
        {
            if (cboPensionType.SelectedValue != "" && txtPensionEmployerAmount.Text != "" && txtPensionEmployeeAmount.Text != "")
            {
                coreLogic.staffPension g;
                if (btnAddPension.Text == "Add Pension")
                {
                    g = new coreLogic.staffPension();
                }
                else
                {
                    g = Session["pension"] as coreLogic.staffPension;
                }
                int id = int.Parse(cboPensionType.SelectedValue);
                g.pensionType = le.pensionTypes.FirstOrDefault(p => p.pensionTypeID == id);
                g.employerAmount = txtPensionEmployerAmount.Value.Value;
                g.employeeAmount = txtPensionEmployeeAmount.Value.Value;
                g.isEnabled = chkPensionIsEnabled.Checked;
                g.isPercent = chkPensionIsPercentage.Checked;

                if (btnAddPension.Text == "Add Pension")
                {
                    pensions.Add(g);
                }
                Session["pensions"] = pensions;
                gridPension.DataSource = pensions;
                gridPension.DataBind();

                txtPensionEmployerAmount.Value = 0;
                txtPensionEmployeeAmount.Value = 0; 

                btnAddPension.Text = "Add Pension";
            }
        }

        protected void gridPension_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = pensions[e.Item.ItemIndex];
                if (g != null)
                {
                    txtPensionEmployerAmount.Value = g.employerAmount;
                    txtPensionEmployeeAmount.Value = g.employeeAmount; 
                    cboPensionType.SelectedValue = g.pensionTypeID.ToString();
                    chkPensionIsEnabled.Checked = g.isEnabled;
                    chkPensionIsPercentage.Checked = g.isPercent;

                    Session["pension"] = g;
                    btnAddPension.Text = "Update Pension";
                    gridPension.DataSource = pensions;
                    gridPension.DataBind();
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                pensions.RemoveAt(e.Item.ItemIndex);
            }
            gridPension.DataSource = pensions;
            gridPension.DataBind();
        }

        protected void btnAddBenefitsInKind_Click(object sender, EventArgs e)
        {
            if (cboBenefitsInKind.SelectedValue != "" && txtBenefitsInKindAmount.Text != "")
            {
                coreLogic.staffBenefitsInKind g;
                if (btnAddBenefitsInKind.Text == "Add BenefitsInKind")
                {
                    g = new coreLogic.staffBenefitsInKind();
                }
                else
                {
                    g = Session["benefitInKind"] as coreLogic.staffBenefitsInKind;
                }
                int id = int.Parse(cboBenefitsInKind.SelectedValue);
                g.benefitsInKind = le.benefitsInKinds.FirstOrDefault(p => p.benefitsInKindID == id);
                g.amount = txtBenefitsInKindAmount.Value.Value;
                g.percentValue = txtBenefitsInKindPercent.Value.Value;
                g.isEnabled = chkBenefitsInKindIsEnabled.Checked;

                if (btnAddBenefitsInKind.Text == "Add BenefitsInKind")
                {
                    benefitsInKind.Add(g);
                }
                Session["benefitsInKinds"] = benefitsInKind;
                gridBenefitsInKind.DataSource = benefitsInKind;
                gridBenefitsInKind.DataBind();

                txtBenefitsInKindAmount.Value = 0;
                txtBenefitsInKindPercent.Value = 0;

                btnAddBenefitsInKind.Text = "Add BenefitsInKind";
            }
        }

        protected void gridBenefitsInKind_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = benefitsInKind[e.Item.ItemIndex];
                if (g != null)
                {
                    txtBenefitsInKindAmount.Value = g.amount;
                    txtBenefitsInKindPercent.Value = g.percentValue;
                    cboBenefitsInKind.SelectedValue = g.benefitsInKindID.ToString();
                    chkBenefitsInKindIsEnabled.Checked = g.isEnabled;

                    Session["benefitInKind"] = g;
                    btnAddBenefitsInKind.Text = "Update BenefitsInKind";
                    gridBenefitsInKind.DataSource = benefitsInKind;
                    gridBenefitsInKind.DataBind();
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                benefitsInKind.RemoveAt(e.Item.ItemIndex);
            }
            gridBenefitsInKind.DataSource = benefitsInKind;
            gridBenefitsInKind.DataBind();
        }

        protected void btnAddDeduction_Click(object sender, EventArgs e)
        {
            if (cboDeductionType.SelectedValue != "" && txtDeductionAmount.Text != "")
            {
                coreLogic.staffDeduction g;
                if (btnAddDeduction.Text == "Add Deduction")
                {
                    g = new coreLogic.staffDeduction();
                }
                else
                {
                    g = Session["deduction"] as coreLogic.staffDeduction;
                }
                int id = int.Parse(cboDeductionType.SelectedValue);
                g.deductionType = le.deductionTypes.FirstOrDefault(p => p.deductionTypeID == id);
                g.amount = txtDeductionAmount.Value.Value;
                g.percentValue = txtDeductionPercent.Value.Value;
                g.isEnabled = chkDeductionEnabled.Checked;

                if (btnAddDeduction.Text == "Add Deduction")
                {
                    deductions.Add(g);
                }
                Session["deductions"] = deductions;
                gridDeduction.DataSource = deductions;
                gridDeduction.DataBind();

                txtDeductionAmount.Value = 0;
                txtDeductionPercent.Value = 0;

                btnAddDeduction.Text = "Add Deduction";
            }
        }

        protected void gridDeduction_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = deductions[e.Item.ItemIndex];
                if (g != null)
                {
                    txtDeductionAmount.Value = g.amount;
                    txtDeductionPercent.Value = g.percentValue;
                    cboDeductionType.SelectedValue = g.deductionTypeID.ToString();
                    chkDeductionEnabled.Checked = g.isEnabled;

                    Session["deduction"] = g;
                    btnAddDeduction.Text = "Update Deduction";
                    gridDeduction.DataSource = deductions;
                    gridDeduction.DataBind();
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                deductions.RemoveAt(e.Item.ItemIndex);
            }
            gridDeduction.DataSource = deductions;
            gridDeduction.DataBind();
        }

        protected void btnAddTaxRelief_Click(object sender, EventArgs e)
        {
            if (cboTaxRelief.SelectedValue != "" && txtTaxReliefAmount.Text != "" )
            {
                coreLogic.staffTaxRelief g;
                if (btnAddTaxRelief.Text == "Add Tax Relief")
                {
                    g = new coreLogic.staffTaxRelief();
                }
                else
                {
                    g = Session["taxRelief"] as coreLogic.staffTaxRelief;
                }
                int id = int.Parse(cboTaxRelief.SelectedValue);
                g.taxReliefType = le.taxReliefTypes.FirstOrDefault(p => p.taxReliefTypeID == id);
                g.amount = txtTaxReliefAmount.Value.Value; 
                g.isEnabled = chkTaxReliefEnabled.Checked; 

                if (btnAddTaxRelief.Text == "Add Tax Relief")
                {
                    taxReliefs.Add(g);
                }
                Session["taxReliefs"] = taxReliefs;
                gridTaxRelief.DataSource = taxReliefs;
                gridTaxRelief.DataBind();

                txtTaxReliefAmount.Value = 0; 

                btnAddTaxRelief.Text = "Add Tax Relief";
            }
        }

        protected void gridTaxRelief_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = taxReliefs[e.Item.ItemIndex];
                if (g != null)
                {
                    txtTaxReliefAmount.Value = g.amount; 
                    cboTaxRelief.SelectedValue = g.taxReliefTypeID.ToString();
                    chkTaxReliefEnabled.Checked = g.isEnabled; 

                    Session["taxRelief"] = g;
                    btnAddPension.Text = "Update Tax Relief";
                    gridTaxRelief.DataSource = taxReliefs;
                    gridTaxRelief.DataBind();
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                taxReliefs.RemoveAt(e.Item.ItemIndex);
            }
            gridTaxRelief.DataSource = taxReliefs;
            gridTaxRelief.DataBind();
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

        protected void cboPensionType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboPensionType.SelectedValue != "" && cboPensionType.SelectedValue != null)
            {
                int id = int.Parse(cboPensionType.SelectedValue);
                var pt = le.pensionTypes.FirstOrDefault(p => p.pensionTypeID == id);
                if (pt != null)
                {
                    txtPensionEmployeeAmount.Value = pt.employeeAmount;
                    txtPensionEmployerAmount.Value = pt.employerAmount;
                    chkPensionIsPercentage.Checked = pt.isPercent;
                    chkPensionIsEnabled.Checked = true;
                }
            }
        }

        protected void cboDeductionType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboDeductionType.SelectedValue != "" && cboDeductionType.SelectedValue != null)
            {
                int id = int.Parse(cboDeductionType.SelectedValue);
                var dt = le.deductionTypes.FirstOrDefault(p => p.deductionTypeID == id);
                if (dt != null)
                {
                    txtDeductionAmount.Value = (dt.isPercent==false) ? dt.amount : 0; ;
                    txtDeductionPercent.Value = (dt.isPercent) ? dt.amount : 0;
                    chkDeductionEnabled.Checked = true;
                }
            }
        }

        protected void cboAllowanceType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboAllowanceType.SelectedValue != "" && cboAllowanceType.SelectedValue != null)
            {
                int id = int.Parse(cboAllowanceType.SelectedValue);
                var dt = le.allowanceTypes.FirstOrDefault(p => p.allowanceTypeID == id);
                if (dt != null)
                {
                    txtAllowanceAmount.Value = (dt.isPercent == false) ? dt.amount : 0; ;
                    txtAllowancePercent.Value = (dt.isPercent) ? dt.taxPercent : 0; 
                    chkAllowanceEnabled.Checked = true;
                }
            }
        }
    }
}