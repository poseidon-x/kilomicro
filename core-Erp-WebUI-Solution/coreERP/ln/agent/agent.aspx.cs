using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.agent
{
    public partial class agent : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.agent cl;
        coreLogic.agentAddress phyAddr;
        coreLogic.agentAddress mailAddr;
        coreLogic.agentPhone workPhone;
        coreLogic.agentPhone homePhone;
        coreLogic.agentPhone mobilePhone;
        List<coreLogic.agentDocument> documents;
        List<coreLogic.agentNextOfKin> noks;

        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            {
                le = new coreLogic.coreLoansEntities();
                Session["le"] = le; 
                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches)
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
                cboBank.Items.Clear();
                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.banks.OrderBy(p=>p.bank_name))
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_name, r.bank_id.ToString()));
                }
                cboAccountType.Items.Clear();
                cboAccountType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.bankAccountTypes)
                {
                    cboAccountType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.accountTypeName, r.accountTypeID.ToString()));
                } 
                int id=0;
                if (Request.Params["id"] != null && int.TryParse(Request.Params["id"], out id))
                {
                    cl = le.agents.FirstOrDefault(p => p.agentID == id);
                    if (cl != null)
                    {
                        //cl.agentAddresses.Load();
                        //cl.agentPhones.Load();
                        //cl.agentImages.Load();
                        //cl.agentDocuments.Load();
                        //cl.agentNextOfKins.Load();

                        foreach (var r in cl.agentImages)
                        {
                            //r.imageReference.Load();
                        }

                        phyAddr = cl.agentAddresses.FirstOrDefault(p => p.addressTypeID == 1);
                        mailAddr = cl.agentAddresses.FirstOrDefault(p => p.addressTypeID == 2);

                        workPhone = cl.agentPhones.FirstOrDefault(p => p.phoneTypeID == 1);
                        mobilePhone = cl.agentPhones.FirstOrDefault(p => p.phoneTypeID == 2);
                        homePhone = cl.agentPhones.FirstOrDefault(p => p.phoneTypeID == 3);

                        documents = cl.agentDocuments.ToList();
                        foreach (var i in documents)
                        {
                            //i.documentReference.Load();
                        }

                        noks = cl.agentNextOfKins.ToList();
                        foreach (var r in noks)
                        {
                            //r.idNoReference.Load();
                            //r.phoneReference.Load();
                            //r.emailReference.Load();
                            //r.imageReference.Load();
                        }
                        Session["noks"] = noks;
                        this.gridNOK.DataSource = noks;
                        gridNOK.DataBind();

                        Session["agentDocuments"] = documents;

                        gridDocument.DataSource = documents;
                        gridDocument.DataBind();

                        cboBranch.SelectedValue = cl.branchID.ToString();
                        if (cl.accountTypeID != null) cboAccountType.SelectedValue = cl.accountTypeID.ToString();

                        if (cl.bankBranchID != null)
                        {
                            var br = ent.bank_branches.FirstOrDefault(p => p.branch_id == cl.bankBranchID);
                            if (br != null)
                            {
                               // br.banksReference.Load();
                               // br.banks.bank_branches.Load();

                                cboBankBranch.Items.Clear();
                                cboBankBranch.Items.Add(new RadComboBoxItem("", ""));
                                foreach (var r in br.banks.bank_branches)
                                {
                                    cboBankBranch.Items.Add(new RadComboBoxItem(r.branch_name, r.branch_id.ToString()));
                                }
                                cboBank.SelectedValue = br.banks.bank_id.ToString();
                                cboBankBranch.SelectedValue = cl.bankBranchID.ToString();
                            }
                        }
                        txtAccountName.Text = cl.accountName;
                        txtAccountNo.Text = cl.accountNumber;
                        txtAccNum.Text = cl.agentNo;
                        txtOtherNames.Text = cl.otherNames;
                        txtSurname.Text = cl.surName;
                        dpDOB.SelectedDate = cl.DOB;
                        rblSex.SelectedValue = cl.sex;
                        
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
                        foreach (var item in cl.agentImages)
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
                        if (phyAddr != null)
                        {
                            //phyAddr.addressReference.Load();
                            txtPhyAddr1.Text = phyAddr.address.addressLine1;
                            txtPhyAddr2.Text = phyAddr.address.addressLine2;
                            txtPhyCityTown.Text = phyAddr.address.cityTown;
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
                    cl = new coreLogic.agent();
                    documents = new List<coreLogic.agentDocument>();
                    noks = new List<coreLogic.agentNextOfKin>();
                    Session["noks"] = noks;
                    Session["agentDocuments"] = documents;
                    cl.branchID = le.branches.FirstOrDefault().branchID;
                    cboBranch.SelectedValue = cl.branchID.ToString();
                }
                Session["agent.cl"] = cl;
                multi1.SelectedIndex = 0;
                tab1.SelectedIndex = 0;
            }
            else{
                if (Session["agentDocuments"] != null)
                    {
                        documents = Session["agentDocuments"] as List<coreLogic.agentDocument>;
                    }
                    else
                    {
                        documents = new List<coreLogic.agentDocument>();
                        Session["agentDocuments"] = documents;
                    }
                if (Session["agent.cl"] != null)
                {
                    cl = Session["agent.cl"] as coreLogic.agent;
                }
                else
                {
                    cl = new coreLogic.agent();
                }
                if (Session["noks"] != null)
                {
                    noks = Session["noks"] as List<coreLogic.agentNextOfKin>;
                }
                else
                {
                    noks = new List<coreLogic.agentNextOfKin>();
                    Session["noks"] = noks;
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
            if (Save()){
                Session["smeDirectors"] = null;
                HtmlHelper.MessageBox2("Agent Data saved successfully!", ResolveUrl("~/ln/agent/default.aspx"), "coreERP©: Successful", IconType.ok);
            }
        }

        private bool Save()
        {
            if (cl != null)
            {
                if (cboBranch.SelectedValue != "") cl.branchID = int.Parse(cboBranch.SelectedValue);
                if (cboAccountType.SelectedValue != "")
                    cl.accountTypeID = int.Parse(cboAccountType.SelectedValue);
                if (cboBankBranch.SelectedValue != "")
                    cl.bankBranchID = int.Parse(cboBankBranch.SelectedValue);
                cl.DOB = dpDOB.SelectedDate;
                cl.otherNames = txtOtherNames.Text;
                cl.sex = rblSex.SelectedValue;
                cl.surName = txtSurname.Text;
                cl.accountNumber = txtAccountNo.Text;
                cl.accountName = txtAccountName.Text;

                if (cl.agentID <= 0)
                {
                    le.agents.Add(cl);
                }
                if (txtAccNum.Text == "")
                {
                    cl.agentNo = cboBranch.SelectedItem.Text.Substring(0, 2).ToUpper()
                        + coreLogic.coreExtensions.NextSystemNumber("loan.agent.agentNo." + cboBranch.SelectedItem.Text.Substring(0, 2).ToUpper());

                }
                else
                {
                    cl.agentNo = txtAccNum.Text;
                }
                if (txtPhyAddr1.Text != "" && txtPhyCityTown.Text != "")
                {
                    phyAddr = cl.agentAddresses.FirstOrDefault(p => p.addressTypeID == 1);
                    if (phyAddr == null)
                    {
                        phyAddr = new coreLogic.agentAddress();
                        cl.agentAddresses.Add(phyAddr);
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
                    coreLogic.agentImage img = new coreLogic.agentImage();
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
                    cl.agentImages.Add(img);
                }
                if (txtMailAddr1.Text != "" && txtMailAddrCity.Text != "")
                {
                    mailAddr = cl.agentAddresses.FirstOrDefault(p => p.addressTypeID == 2);
                    if (mailAddr == null)
                    {
                        mailAddr = new coreLogic.agentAddress();
                        cl.agentAddresses.Add(mailAddr);
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
                    workPhone = cl.agentPhones.FirstOrDefault(p => p.phoneTypeID == 1);
                    if (workPhone == null)
                    {
                        workPhone = new coreLogic.agentPhone();
                        cl.agentPhones.Add(workPhone);
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
                    mobilePhone = cl.agentPhones.FirstOrDefault(p => p.phoneTypeID == 2);
                    if (mobilePhone == null)
                    {
                        mobilePhone = new coreLogic.agentPhone();
                        cl.agentPhones.Add(mobilePhone);
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
                    homePhone = cl.agentPhones.FirstOrDefault(p => p.phoneTypeID == 3);
                    if (homePhone == null)
                    {
                        homePhone = new coreLogic.agentPhone();
                        cl.agentPhones.Add(homePhone);
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
                foreach (var r in noks)
                {
                    if (!cl.agentNextOfKins.Contains(r))
                    {
                        cl.agentNextOfKins.Add(r);
                    }
                }
                for (int i = cl.agentNextOfKins.Count - 1; i >= 0; i--)
                {
                    var r = cl.agentNextOfKins.ToList()[i];
                    if (!noks.Contains(r))
                    {
                        cl.agentNextOfKins.Remove(r);
                    }
                }
                le.SaveChanges();
                return true;
            }

            return false;
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
                        var g = new coreLogic.agentDocument
                        {
                            agent = cl,
                            document = i
                        };
                        documents.Add(g);
                    }
                }
                Session["agentDocuments"] = documents;
                gridDocument.DataSource = documents;
                gridDocument.DataBind();

                txtDocDesc.Text = "";
                btnAddDcoument.Text = "Add Document";
            }
        }

        protected void cboBank_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboBank.SelectedValue != "")
            {
                int bankID = int.Parse(cboBank.SelectedValue);
                var bank = ent.banks.FirstOrDefault(p => p.bank_id == bankID);
                //bank.bank_branches.Load();

                cboBankBranch.Items.Clear();
                cboBankBranch.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in bank.bank_branches)
                {
                    cboBankBranch.Items.Add(new RadComboBoxItem(r.branch_name, r.branch_id.ToString()));
                }
            }
        }

        protected void btnAddNOK_Click(object sender, EventArgs e)
        {
            if (txtNOKSurname.Text != "" && txtNOKOtherNames.Text != "")
            {
                coreLogic.agentNextOfKin g;
                if (btnAddNOK.Text == "Add Next of Kin Details")
                {
                    g = new coreLogic.agentNextOfKin();
                }
                else
                {
                    g = Session["nok"] as coreLogic.agentNextOfKin;
                }
                g.surName = txtNOKSurname.Text;
                g.otherNames = txtNOKOtherNames.Text;
                g.relationship = txtRelation.Text;
                if (cboNOKIDType.SelectedValue != "" && txtNOKIDNumber.Text != "")
                {
                    if (g.idNo == null) g.idNo = new coreLogic.idNo();
                    g.idNo.idNoTypeID = int.Parse(cboNOKIDType.SelectedValue);
                    g.idNo.idNo1 = txtNOKIDNumber.Text;
                }
                if (txtNOKPhoneNumber.Text != "")
                {
                    if (g.phone == null) g.phone = new coreLogic.phone();
                    g.phone.phoneNo = txtNOKPhoneNumber.Text;
                    g.phone.phoneTypeID = 1;
                }
                if (txtNOKEmailAddress.Text != "")
                {
                    if (g.email == null) g.email = new coreLogic.email();
                    g.email.emailAddress = txtNOKEmailAddress.Text;
                    g.email.emailTypeID = 1;
                }
                if (upload5.UploadedFiles.Count == 1)
                {
                    var item = upload5.UploadedFiles[0];
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
                    g.image = i;
                }
                if (btnAddNOK.Text == "Add Next of Kin Details")
                {
                    noks.Add(g);
                }
                Session["noks"] = noks;
                gridNOK.DataSource = noks;
                gridNOK.DataBind();

                txtNOKOtherNames.Text = "";
                txtNOKSurname.Text = "";
                txtNOKPhoneNumber.Text = "";
                txtNOKEmailAddress.Text = "";
                txtNOKIDNumber.Text = "";
                cboNOKIDType.SelectedValue = "";
                txtRelation.Text = "";
                btnAddNOK.Text = "Add Next of Kin Details";
            }
        }

        protected void gridNOK_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = noks[e.Item.ItemIndex];
                if (g != null)
                {
                    txtNOKOtherNames.Text = g.otherNames;
                    txtNOKSurname.Text = g.surName;

                    if (g.phone != null)
                    {
                        this.txtNOKPhoneNumber.Text = g.phone.phoneNo;
                    }
                    if (g.email != null)
                    {
                        txtNOKEmailAddress.Text = g.email.emailAddress;
                    }
                    if (g.idNo != null)
                    {
                        txtNOKIDNumber.Text = g.idNo.idNo1;
                        cboNOKIDType.SelectedValue = g.idNo.idNoTypeID.ToString();
                    }
                    if (g.relationship != null)
                    {
                        txtRelation.Text = g.relationship;
                    }
                    Session["nok"] = g;
                    btnAddNOK.Text = "Update Next of Kin Details";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                noks.RemoveAt(e.Item.ItemIndex);
                Session["noks"] = noks;
            }
            gridNOK.DataSource = noks;
            gridNOK.DataBind();
        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                documents=cl.agentDocuments.ToList();
                noks = cl.agentNextOfKins.ToList();
                Session["agentDocuments"] = documents;
                Session["agent.cl"] = cl;
                Session["noks"] = noks;

                gridDocument.DataSource = documents;
                gridDocument.DataBind();
                gridNOK.DataSource = noks;
                gridNOK.DataBind();

                HtmlHelper.MessageBox("Agent Data saved successfully!");
            }
        }

    }
}