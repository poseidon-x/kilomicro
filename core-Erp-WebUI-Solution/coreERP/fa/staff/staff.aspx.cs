using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.ln.staff
{
    public partial class staff : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.staff cl;
        coreLogic.staffAddress phyAddr;
        coreLogic.staffAddress mailAddr;
        coreLogic.staffPhone workPhone;
        coreLogic.staffPhone homePhone;
        coreLogic.staffPhone mobilePhone; 
        List<coreLogic.staffDocument> documents;
        private IIDGenerator idGen;

        protected void Page_Load(object sender, EventArgs e)
        {
            idGen = new IDGenerator();
            if (!IsPostBack)
            {
                le = new coreLogic.coreLoansEntities();
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

                        foreach (var r in cl.staffImages)
                        {
                            //r.imageReference.Load();
                        }

                        phyAddr = cl.staffAddresses.FirstOrDefault(p => p.addressTypeID == 1);
                        mailAddr = cl.staffAddresses.FirstOrDefault(p => p.addressTypeID == 2);

                        workPhone = cl.staffPhones.FirstOrDefault(p => p.phoneTypeID == 1);
                        mobilePhone = cl.staffPhones.FirstOrDefault(p => p.phoneTypeID == 2);
                        homePhone = cl.staffPhones.FirstOrDefault(p => p.phoneTypeID == 3);
                         
                        documents = cl.staffDocuments.ToList();
                        foreach (var i in documents)
                        {
                            //i.documentReference.Load();
                        }

                        Session["staffDocuments"] = documents;

                        gridDocument.DataSource = documents;
                        gridDocument.DataBind();

                        cboBranch.SelectedValue = cl.branchID.ToString();
                        cboCategory.SelectedValue = cl.staffCategoryID.ToString();
                        dtpEmploymentStartDate.SelectedDate = cl.employmentStartDate;
                        cboMaritalStatus.SelectedValue = cl.maritalStatusID.ToString();
                        cboJobTitle.SelectedValue = cl.jobTitleID.ToString();
                        txtAccNum.Text = cl.staffNo;
                        txtOtherNames.Text = cl.otherNames;
                        txtSurname.Text = cl.surName;
                        dpDOB.SelectedDate = cl.DOB;
                        rblSex.SelectedValue = cl.sex;
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
                    } 
                }
                else
                {
                    cl = new coreLogic.staff();
                        documents = new List<coreLogic.staffDocument>();
                        Session["staffDocuments"] = documents;
                    
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
                cl.employmentStartDate = dtpEmploymentStartDate.SelectedDate;
                if (txtUserName.Text != "")
                    cl.userName = txtUserName.Text;
                else
                    cl.userName = null;
                if (cboEmploymentStatus.SelectedValue != "")
                {
                    cl.employmentStatusID = int.Parse(cboEmploymentStatus.SelectedValue);
                }
                if (cl.staffID <= 0)
                {
                    le.staffs.Add(cl);
                }
                cl.employmentStartDate = dtpEmploymentStartDate.SelectedDate;
                var startDate = DateTime.Now;
                if (cl.employmentStartDate != null)
                {
                    startDate = cl.employmentStartDate.Value;
                }
                if(txtAccNum.Text == "")
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
                le.SaveChanges();
                Session["smeDirectors"] = null;
                HtmlHelper.MessageBox2("Staff Data saved successfully!", ResolveUrl("~/fa/staff/default.aspx"), "coreERP©: Successful", IconType.ok);
                    
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

    }
}