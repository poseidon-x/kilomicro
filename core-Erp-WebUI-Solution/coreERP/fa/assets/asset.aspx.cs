//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using Telerik.Web.UI;
//using coreLogic;

//namespace coreERP.ln.asset
//{
//    public partial class asset : System.Web.UI.Page
//    {
//        coreLogic.coreLoansEntities le;
//        coreLogic.core_dbEntities ent;
//        coreLogic.asset cl;  
//        List<coreLogic.assetDocument> documents;

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            ent = new coreLogic.core_dbEntities();
//            if (!IsPostBack)
//            {
//                le = new coreLogic.coreLoansEntities();
//                Session["le"] = le; 
//                cboAssetCategory.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
//                foreach (var r in le.assetCategories)
//                {
//                    cboAssetCategory.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.assetCategoryName, r.assetCategoryID.ToString()));
//                }
//                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
//                foreach (var r in le.staffs)
//                {
//                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + "," + r.otherNames,
//                        r.staffID.ToString()));
//                }
//                cboOU.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
//                foreach (var r in ent.ou)
//                {
//                    cboOU.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.ou_name, r.ou_id.ToString()));
//                } 
//                int id=0;
//                if (Request.Params["id"] != null && int.TryParse(Request.Params["id"], out id))
//                {
//                    cl = le.assets.FirstOrDefault(p => p.assetID == id);
//                    if (cl != null)
//                    {  
//                        //cl.assetImages.Load();
//                        //cl.assetDocuments.Load();
//                        //cl.assetSubCategoryReference.Load();
//                        //cl.assetDepreciations.Load();
//                        //cl.depreciationSchedules.Load();
//                        //cl.assetSubCategory.assetCategoryReference.Load();

//                        foreach (var r in cl.assetImages)
//                        {
//                            //r.imageReference.Load();
//                        }
 
//                        documents = cl.assetDocuments.ToList();
//                        foreach (var i in documents)
//                        {
//                            //i.documentReference.Load();
//                        }

//                        Session["assetDocuments"] = documents;

//                        gridDocument.DataSource = documents;
//                        gridDocument.DataBind();

//                        gridDep.DataSource = cl.assetDepreciations.ToList();
//                        gridDep.DataBind();

//                        gridSched.DataSource = cl.depreciationSchedules.ToList();
//                        gridSched.DataBind();

//                        cboAssetCategory.SelectedValue = cl.assetSubCategory.assetCategoryID.ToString();
//                        if(cl.ouID!= null)cboOU.SelectedValue = cl.ouID.ToString();
//                        if (cl.staffID != null) cboStaff.SelectedValue = cl.staffID.ToString();

//                        txtAccNum.Text = cl.assetTag;
//                        dtPurchDate.SelectedDate = cl.assetPurchaseDate;
//                        txtAssetPrice.Value = cl.assetPrice;
//                        txtLifetime.Value = cl.assetLifetime;
//                        txtNotes.Text = cl.assetNotes;
//                        txtDesc.Text = cl.assetDescription;
//                        txtCurrentValue.Value = cl.assetCurrentValue;
//                        dtLastDep.SelectedDate = cl.lastDepreciationDate;
//                        txtRate.Value = cl.depreciationRate;
//                        if (cl.accumulatedDepreciationAccountID != null)
//                        {
//                            var acc = ent.vw_accounts.FirstOrDefault(p => p.acct_id == cl.accumulatedDepreciationAccountID);
//                            if (acc != null)
//                            {
//                                cboADA.Items.Add(new RadComboBoxItem(acc.fullname, acc.acct_id.ToString()));
//                            }
//                            cboADA.SelectedValue = cl.accumulatedDepreciationAccountID.ToString();
//                        }
//                        if (cl.depreciationAccountID != null)
//                        {
//                            var acc = ent.vw_accounts.FirstOrDefault(p => p.acct_id == cl.depreciationAccountID);
//                            if (acc != null)
//                            {
//                                cboDA.Items.Add(new RadComboBoxItem(acc.fullname, acc.acct_id.ToString()));
//                            }
//                            cboDA.SelectedValue = cl.depreciationAccountID.ToString();
//                        }
//                        if (cl.fixedAssetsAccountID != null)
//                        {
//                            var acc = ent.vw_accounts.FirstOrDefault(p => p.acct_id == cl.fixedAssetsAccountID);
//                            if (acc != null)
//                            {
//                                cboFAA.Items.Add(new RadComboBoxItem(acc.fullname, acc.acct_id.ToString()));
//                            }
//                            cboFAA.SelectedValue = cl.fixedAssetsAccountID.ToString();
//                        }
//                        lblDepMeth.Text = (cl.assetSubCategory.assetCategory.depreciationMethod == 1) ? "Straight Line" : "Reducing Balance";
//                        if (cl.assetSubCategory.assetCategory.depreciationMethod == 1)
//                        {
//                            txtLifetime.Enabled = true;
//                            txtRate.Enabled = false;
//                        }
//                        else
//                        {
//                            txtLifetime.Enabled = false;
//                            txtRate.Enabled = true;
//                            txtLifetime.Value = 0;
//                        }

//                        PopulateSubCategories(cl.assetSubCategory.assetCategoryID);
//                        cboSubCategory.SelectedValue = cl.assetSubCategoryID.ToString();

//                        foreach (var item in cl.assetImages)
//                        {
//                            //item.imageReference.Load();
//                            RadBinaryImage img = new RadBinaryImage();
//                            img.Width = 320;
//                            img.Height = 180;
//                            img.ResizeMode = BinaryImageResizeMode.Fit;
//                            img.DataValue = item.image.image1;
//                            RadRotatorItem it = new RadRotatorItem();
//                            it.Controls.Add(img);
//                            rotator2.Items.Add(it);
//                        } 
//                    } 
//                }
//                else
//                {
//                    cl = new coreLogic.asset();
//                        documents = new List<coreLogic.assetDocument>();
//                        Session["assetDocuments"] = documents;
                    
//                }
//                Session["asset.cl"] = cl;
//                multi1.SelectedIndex = 0;
//                tab1.SelectedIndex = 0;
//            }
//            else{
//                if (Session["assetDocuments"] != null)
//                    {
//                        documents = Session["assetDocuments"] as List<coreLogic.assetDocument>;
//                    }
//                    else
//                    {
//                        documents = new List<coreLogic.assetDocument>();
//                        Session["assetDocuments"] = documents;
//                    }
//                if (Session["asset.cl"] != null)
//                {
//                    cl = Session["asset.cl"] as coreLogic.asset;
//                }
//                else
//                {
//                    cl = new coreLogic.asset();
//                } 
//                if (Session["le"] != null)
//                {
//                    le = Session["le"] as coreLogic.coreLoansEntities;
//                }
//                else
//                {
//                    le = new coreLogic.coreLoansEntities();
//                }
//            }
//        }

//        protected void btnSave_Click(object sender, EventArgs e)
//        {
//            if (cl != null 
//                && txtRate.Value != null
//                && txtAssetPrice.Value != null
//                && txtLifetime.Value!=null
//                && txtDesc.Text!=""
//                && cboSubCategory.SelectedValue!=null 
//                && cboSubCategory.SelectedValue!= ""
//                )
//            {
//                if(cboOU.SelectedValue!="")cl.ouID = int.Parse(cboOU.SelectedValue);
//                var id=int.Parse(cboSubCategory.SelectedValue);
//                cl.assetSubCategoryID = id;
//                cl.assetSubCategory = le.assetSubCategories.FirstOrDefault(p => p.assetSubCategoryID == id);
//                //cl.assetSubCategory.assetCategoryReference.Load();
//                cl.assetPurchaseDate = dtPurchDate.SelectedDate.Value;  
//                if(cboStaff.SelectedValue!="")cl.staffID = int.Parse(cboStaff.SelectedValue);
//                cl.assetDescription = txtDesc.Text;
//                cl.assetPrice = txtAssetPrice.Value.Value;
//                cl.assetLifetime = (int)txtLifetime.Value.Value;
//                cl.assetNotes = txtNotes.Text;
//                if (cl.assetID <= 0 && txtAccNum.Text == "")
//                {
//                    cl.assetTag = cboAssetCategory.SelectedItem.Text.Substring(0, 2).ToUpper()
//                        + coreLogic.coreExtensions.NextSystemNumber("loan.asset.assetTag." + cboAssetCategory.SelectedItem.Text.Substring(0, 2).ToUpper());
//                    le.assets.Add(cl);
//                }
//                else
//                {
//                    cl.assetTag = txtAccNum.Text;
//                }
//                if (txtCurrentValue.Value == null || txtCurrentValue.Value.Value==0)
//                {
//                    cl.assetCurrentValue = txtAssetPrice.Value.Value;
//                }
//                else
//                {
//                    cl.assetCurrentValue = txtCurrentValue.Value.Value;
//                }
//                if (dtLastDep.SelectedDate != null)
//                {
//                    cl.lastDepreciationDate = dtLastDep.SelectedDate.Value;
//                }
//                if (cboFAA.SelectedValue != "" && cboFAA.SelectedValue != "")
//                {
//                    cl.fixedAssetsAccountID = int.Parse(cboFAA.SelectedValue);
//                }
//                if (cboDA.SelectedValue != "" && cboDA.SelectedValue != "")
//                {
//                    cl.depreciationAccountID = int.Parse(cboDA.SelectedValue);
//                }
//                if (cboADA.SelectedValue != "" && cboADA.SelectedValue != "")
//                {
//                    cl.accumulatedDepreciationAccountID = int.Parse(cboADA.SelectedValue);
//                }
//                cl.depreciationRate = txtRate.Value.Value;

//                for (int i= cl.depreciationSchedules.Count-1;i>=0;i--)
//                {
//                    var s = cl.depreciationSchedules.ToList()[i];
//                    le.depreciationSchedules.Remove(s);
//                }
//                var amount = cl.assetCurrentValue;
//                DateTime? date2 = null;
//                if (cl.lastDepreciationDate == null)
//                    date2 = cl.assetPurchaseDate;
//                else
//                    date2 = cl.lastDepreciationDate;
//                DateTime date3 = date2.Value;
//                var dateE = 12 - date3.Month+1;
//                var princ = amount;
//                int c = 1;
//                while (amount > 2)
//                {
//                    date3 = new DateTime(date3.Year, date3.Month, DateTime.DaysInMonth(date3.Year, date3.Month),
//                        23, 59, 59);
//                    var date4 = date2.Value.AddYears(cl.assetLifetime);
//                    date4=new DateTime(date4.Year,date4.Month, 1);
//                    if (cl.assetSubCategory.assetCategory.depreciationMethod != 1 || (date3 > date2 &&  date4> date3))
//                    {
//                        var dep = 0.0;
//                        if ((cl.assetSubCategory.assetCategory.depreciationMethod == 1))
//                        {
//                            if (date3.Month == 1)
//                            {
//                                dateE = 12;
//                            }
//                            dep = (1.0 / (dateE * cl.assetLifetime)) * cl.assetPrice;
//                        }
//                        else
//                        {
//                            date4 = date2.Value.AddYears(7);
//                            //date4 = new DateTime(date4.Year, date4.Month, 1);
//                            if ((date3 - date2.Value).TotalDays > 366 * 7 || date4 < date3)
//                                break; 
//                            if (date3.Month == 1)
//                            {
//                                princ = amount;
//                                dateE = 12;
//                                if (c == 7)
//                                {
//                                    dateE = date2.Value.Month - 1;
//                                    if (dateE <= 0) dateE = 12;
//                                }
//                                c += 1;
//                            }
//                            if (c == 7)
//                                dep = Math.Round(princ / dateE, 2);
//                            else
//                                dep = ((1.0 / dateE) * (cl.depreciationRate / 100.0)) * princ;
//                        }
//                        dep = Math.Round(dep, 2);
//                        if (dep > amount) dep = amount;

//                        if (dep > 0)
//                        {
//                            var inte = new coreLogic.depreciationSchedule
//                            {
//                                assetID = cl.assetID,
//                                assetValue = amount,
//                                depreciationAmount = dep,
//                                drepciationDate = date3,
//                                period = 1,
//                                startDate = date2
//                            };
//                            cl.depreciationSchedules.Add(inte);
//                            amount -= dep;
//                        }
//                    }
//                    else
//                    {
//                        break;
//                    }
//                    date3 = date3.AddMonths(1);
                    
//                }
//                foreach (Telerik.Web.UI.UploadedFile item in upload3.UploadedFiles)
//                {
//                    coreLogic.assetImage img = new coreLogic.assetImage();
//                    byte[] b = new byte[item.InputStream.Length];
//                    item.InputStream.Read(b, 0, b.Length);

//                    System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
//                    System.Drawing.Image i2 = System.Drawing.Image.FromStream(ms);
//                    i2 = i2.GetThumbnailImage(480, 480, null, IntPtr.Zero);
//                    ms = null;
//                    ms = new System.IO.MemoryStream();
//                    i2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
//                    b = ms.ToArray();
//                    i2 = null;
//                    ms = null;

//                    var i = new coreLogic.image
//                    {
//                        description = item.FileName,
//                        image1 = b,
//                        content_type = item.ContentType
//                    };
//                    img.image = i;
//                    cl.assetImages.Add(img);
//                }
                
//                le.SaveChanges();
//                Session["smeDirectors"] = null;
//                HtmlHelper.MessageBox2("Asset Saved Successfully!", ResolveUrl("~/fa/assets/default.aspx"), "coreERP©: Successful", IconType.ok);
//            }
//        }

//        private void GTA(object sender, EventArgs e)
//        {
//        }

//        protected void rotator1_ItemDataBound(object sender, Telerik.Web.UI.RadRotatorEventArgs e)
//        {

//        }

//        protected void cboCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
//        { 
//        }
 
//        protected void gridDocument_ItemCommand(object sender, GridCommandEventArgs e)
//        {
//            if (e.CommandName == "EditItem")
//            {
//            }
//            else if (e.CommandName == "DeleteItem")
//            {
//                documents.RemoveAt(e.Item.ItemIndex);
//            }
//            gridDocument.DataSource = documents;
//            gridDocument.DataBind();
//        }

//        protected void btnAddDcoument_Click(object sender, EventArgs e)
//        {
//            if (txtDocDesc.Text != "")
//            {
//                if (upload4.UploadedFiles.Count > 0)
//                {
//                    foreach (UploadedFile item in upload4.UploadedFiles)
//                    {
//                        byte[] b = new byte[item.InputStream.Length];
//                        item.InputStream.Read(b, 0, b.Length);


//                        var i = new coreLogic.document
//                        {
//                            description = txtDocDesc.Text,
//                            documentImage = b,
//                            contentType = item.ContentType,
//                            fileName = item.FileName
//                        };
//                        var g = new coreLogic.assetDocument
//                        {
//                            asset = cl,
//                            document = i
//                        };
//                        documents.Add(g);
//                    }
//                }
//                Session["assetDocuments"] = documents;
//                gridDocument.DataSource = documents;
//                gridDocument.DataBind();

//                txtDocDesc.Text = "";
//                btnAddDcoument.Text = "Add Document";
//            }
//        }

//        void PopulateSubCategories(int categoryID)
//        {
//            cboSubCategory.Items.Clear();
//            var cat = le.assetCategories.FirstOrDefault(p => p.assetCategoryID == categoryID);
//            if (cat != null)
//            {
//                //cat.assetSubCategories.Load();
//                foreach (var r in cat.assetSubCategories)
//                {
//                    cboSubCategory.Items.Add(new RadComboBoxItem(r.assetSubCategoryName, r.assetSubCategoryID.ToString()));
//                }
//            }
//        }

//        protected void cboAssetCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
//        {
//            if (cboAssetCategory.SelectedValue != "")
//            {
//                PopulateSubCategories(int.Parse(cboAssetCategory.SelectedValue));
//                var id = int.Parse(cboAssetCategory.SelectedValue);
//                var cat = le.assetCategories.FirstOrDefault(p => p.assetCategoryID == id);
//                if (cat != null)
//                {
//                    lblDepMeth.Text = (cat.depreciationMethod == 1) ? "Straight Line" : "Reducing Balance";
//                    if (cat.depreciationMethod == 1)
//                    {
//                        txtLifetime.Enabled = true;
//                        txtRate.Enabled = false;
//                    }
//                    else
//                    {
//                        txtLifetime.Enabled = false;
//                        txtRate.Enabled = true;
//                        txtLifetime.Value = 0;
//                    }
//                    if (cat.depreciationAccountID != null)
//                    {
//                        cboDA.ValidationGroup = "depreciationAccountID";
//                        ViewState[cboDA.ValidationGroup] = cat.depreciationAccountID;
//                        PopulateAccounts(cboDA);
//                    }
//                    if (cat.accumulatedDepreciationAccountID != null)
//                    {
//                        cboDA.ValidationGroup = "accumulatedDepreciationAccountID";
//                        ViewState[cboDA.ValidationGroup] = cat.accumulatedDepreciationAccountID;
//                        PopulateAccounts(cboDA);
//                    }
//                }
//            }
//        }

//        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc)
//        {
//            try
//            {
//                int? id = null;
//                if (ViewState[cboAcc.ValidationGroup] != null)
//                {
//                    id = int.Parse(ViewState[cboAcc.ValidationGroup].ToString());
//                }
//                var accs = (from a in ent.vw_accounts
//                            from c in ent.currencies
//                            where (a.currency_id == c.currency_id)
//                                && (a.acct_id == id)
//                            select new
//                            {
//                                a.acct_id,
//                                a.acc_num,
//                                a.acc_name,
//                                major_name = c.major_name,
//                                a.fullname
//                            }).ToList(); ;
//                cboAcc.Items.Clear();
//                cboAcc.DataSource = accs;
//                cboAcc.DataBind();
//                if (accs.Count() > 0)
//                {
//                    cboAcc.SelectedValue = id.ToString();
//                }
//            }
//            catch (Exception ex) { }
//        }

//        protected void cboGLAcc_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
//        {
//            try
//            {
//                var cbo = sender as RadComboBox;
//                if (e.Text.Trim().Length > 2 && cbo != null)
//                {
//                    using (core_dbEntities ent = new core_dbEntities())
//                    {
//                        var accs = (from a in ent.vw_accounts
//                                    from c in ent.currencies
//                                    where (a.acc_name.Contains(e.Text) || a.cat_name.Contains(e.Text) || a.head_name1.Contains(e.Text)
//                                        || a.head_name2.Contains(e.Text) || a.head_name3.Contains(e.Text))
//                                        && (a.currency_id == c.currency_id)
//                                    select new
//                                    {
//                                        a.acct_id,
//                                        a.acc_num,
//                                        a.acc_name,
//                                        major_name = c.major_name,
//                                        a.fullname
//                                    }).ToList();
//                        cbo.DataSource = accs;
//                        cbo.DataBind();
//                        cbo.DataTextField = "fullname";
//                        cbo.DataValueField = "acct_id";
//                    }
//                }
//            }
//            catch (Exception ex) { }
//        } 
 
//    }
//}