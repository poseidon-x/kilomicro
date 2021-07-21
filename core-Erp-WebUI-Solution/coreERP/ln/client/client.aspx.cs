using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;
using System.Data.Entity;

namespace coreERP.ln.client
{
    public partial class client : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client cl;
        coreLogic.clientAddress phyAddr;
        coreLogic.clientAddress mailAddr;
        coreLogic.clientPhone workPhone;
        coreLogic.clientPhone homePhone;
        coreLogic.clientPhone mobilePhone;
        coreLogic.smeCategory sme;
        coreLogic.employeeCategory emp;
        coreLogic.staffCategory1 staff;
        coreLogic.groupCategory grp;
        coreLogic.microBusinessCategory mic;
        List<coreLogic.smeDirector> smeDirectors;
        List<coreLogic.clientDocument> documents;
        List<coreLogic.nextOfKin> noks;
        List<coreLogic.clientBankAccount> banks;
        coreSecurityEntities secEnt;

        string categoryID = null;
        private IIDGenerator idGen;

        protected int GetUserStaffBranchId(string logginUser)
        {
            //var logginUser = User?.Identity?.Name?.ToLower();
            var staffBranchId = le?.staffs?.FirstOrDefault(e => e.userName.ToLower() == logginUser.ToLower())?.branchID;
            return staffBranchId.Value;
        }
        protected int GetUserStaffId(string logginUser)
        {
            //var logginUser = User?.Identity?.Name?.ToLower();
            var staffBranchId = le?.staffs?.FirstOrDefault(e => e.userName.ToLower() == logginUser.ToLower())?.staffID;
            return staffBranchId.Value;
        }

        private bool IsOwner(string userName)
        {
            try
            {
                secEnt = new coreSecurityEntities();
                var userRoles = secEnt.user_roles
                    .Include(r => r.users)
                    .Include(w => w.roles)
                    .Where(p => p.roles.role_name.Trim().ToLower() == "owner" && p.users.user_name.Trim().ToLower() == userName.ToLower()).ToList();
                if (userRoles != null && userRoles.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool IsBranchAdmin(string userName)
        {
            try
            {
                secEnt = new coreSecurityEntities();
                var userRoles = secEnt.user_roles
                    .Include(r => r.users)
                    .Include(w => w.roles)
                    .Where(p => p.roles.role_name.Trim().ToLower() == "branchadmin" && p.users.user_name.Trim().ToLower() == userName.ToLower()).ToList();
                if (userRoles != null && userRoles.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            idGen = new IDGenerator();
            categoryID = Request.Params["catID"];
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();

            if (!IsPostBack)
            {
                Session["imageFromCamera"] = null;
                Session["id"] = null;
                cboMaritalStatus.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.maritalStatus)
                {
                    cboMaritalStatus.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.maritalStatusName, r.maritalStatusID.ToString()));
                }
                cboLOB.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.lineOfBusinesses)
                {
                    cboLOB.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.lineOfBusinessName, r.lineOfBusinessID.ToString()));
                }
                cboCategory.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.categories)
                {
                    cboCategory.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.categoryName, r.categoryID.ToString()));
                }
                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                //TODO: Check against login user
                var logginUser = User?.Identity?.Name?.ToLower();
                var branches = le.branches.ToList();
                var loanGrps = le.loanGroups.Include(r=>r.staff).ToList();
                if (!IsOwner(logginUser))
                {

                    var staffBranchId = GetUserStaffBranchId(logginUser);
                    var staffId = GetUserStaffId(logginUser);
                    branches = branches.Where(v => v.branchID == staffBranchId).ToList();
                    if (IsBranchAdmin(logginUser))
                    {

                        loanGrps = loanGrps.Where(r => r.staff.branchID == staffBranchId).ToList();

                    }
                    else
                    {
                        loanGrps = loanGrps.Where(r => r.staff.staffID == staffId).ToList();
                    }
                }
                foreach (var r in branches)
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
                //Loan Group Combo Box
                cboLoanGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in loanGrps)
                {
                    cboLoanGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanGroupName, r.loanGroupId.ToString()));
                }

                cboIndustry.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.industries)
                {
                    cboIndustry.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.industryName, r.industryID.ToString()));
                }
                cboIDType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.idNoTypes.Where(p => p.isNational == true))
                {
                    cboIDType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.idNoTypeName, r.idNoTypeID.ToString()));
                }
                cboIDType2.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.idNoTypes.Where(p => p.isNational == false))
                {
                    cboIDType2.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.idNoTypeName, r.idNoTypeID.ToString()));
                }
                cboNOKIDType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.idNoTypes)
                {
                    cboNOKIDType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.idNoTypeName, r.idNoTypeID.ToString()));
                }
                cboSector.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.sectors)
                {
                    cboSector.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.sectorName, r.sectorID.ToString()));
                }
                cboEmploymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.employmentTypes)
                {
                    cboEmploymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.employmentTypeName, r.employmentTypeID.ToString()));
                }
                cboEmployer.Items.Clear();
                cboEmployer.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.employers)
                {
                    cboEmployer.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.employerName, r.employerID.ToString()));
                }
                cboGroup.Items.Clear();
                cboGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.groups)
                {
                    cboGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.groupName, r.groupID.ToString()));
                }
                this.cboStaffEmp.Items.Clear();
                cboStaffEmp.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.employers)
                {
                    cboStaffEmp.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.employerName, r.employerID.ToString()));
                }
                this.cboStaffContract.Items.Clear();
                cboStaffContract.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.employeeContractTypes)
                {
                    cboStaffContract.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.employeeContractTypeName, r.employeeContractTypeID.ToString()));
                }
                cboBank.Items.Clear();

                RadComboBoxItem item2 = new Telerik.Web.UI.RadComboBoxItem("", "");
                cboBank.Items.Add(item2);
                foreach (var r in ent.banks.OrderBy(p => p.bank_name))
                {
                    item2 = new Telerik.Web.UI.RadComboBoxItem(r.bank_name, r.bank_id.ToString());
                    cboBank.Items.Add(item2);
                }

                RadComboBoxItem item3 = new Telerik.Web.UI.RadComboBoxItem("", "");
                cboClientType.Items.Add(item3);
                foreach (var r in le.clientTypes.OrderBy(p => p.clientTypeName))
                {
                    item3 = new Telerik.Web.UI.RadComboBoxItem(r.clientTypeName, r.clientTypeId.ToString());
                    cboClientType.Items.Add(item3);
                }

                cboAccountType.Items.Clear();
                cboAccountType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.bankAccountTypes)
                {
                    cboAccountType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.accountTypeName, r.accountTypeID.ToString()));
                }
                cboRegion.Items.Clear();
                cboRegion.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.regions)
                {
                    cboRegion.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.regionName, r.regionID.ToString()));
                }
                var clCnfg = le.clientConfigs.FirstOrDefault();
                if (clCnfg != null && clCnfg.admissionFeeEnabled)
                {
                    pnlAdmission.Visible = true;
                    txtAdmissionFee.Value = clCnfg.admissionFeeAmount;
                    txtAdmissionFee.Enabled = false;
                }


                int id = 0;
                if (Request.Params["id"] != null && int.TryParse(Request.Params["id"], out id))
                {
                    Session["id"] = id;
                    cl = le.clients.FirstOrDefault(p => p.clientID == id);
                    if (cl != null)
                    {
                        //cl.idNoReference.Load();
                        //cl.idNo1Reference.Load();
                        //cl.clientAddresses.Load();
                        //cl.clientPhones.Load();
                        //cl.clientEmails.Load();
                        //cl.smeCategories.Load();
                        //cl.employeeCategories.Load();
                        //cl.groupCategories.Load();
                        //cl.microBusinessCategories.Load();
                        ////cl.clientImages.Load();
                        //cl.clientDocuments.Load();
                        //cl.nextOfKins.Load();
                        //cl.staffCategory1.Load();
                        ////cl.clientBankAccounts.Load();
                        ////cl.clientCompanyReference.Load();

                        foreach (var r in cl.clientImages)
                        {
                            //r.imageReference.Load();
                        }
                        RenderImages();

                        phyAddr = cl.clientAddresses.FirstOrDefault(p => p.addressTypeID == 1);
                        mailAddr = cl.clientAddresses.FirstOrDefault(p => p.addressTypeID == 2);

                        workPhone = cl.clientPhones.FirstOrDefault(p => p.phoneTypeID == 1);
                        mobilePhone = cl.clientPhones.FirstOrDefault(p => p.phoneTypeID == 2);
                        homePhone = cl.clientPhones.FirstOrDefault(p => p.phoneTypeID == 3);

                        var officeEmail = cl.clientEmails.FirstOrDefault(p => p.emailTypeID == 1);
                        var personalEmail = cl.clientEmails.FirstOrDefault(p => p.emailTypeID == 2);

                        sme = cl.smeCategories.FirstOrDefault();
                        if (sme != null)
                        {
                            //sme.smeDirectors.Load();
                            smeDirectors = sme.smeDirectors.ToList();
                            foreach (var r in smeDirectors)
                            {
                                //r.idNoReference.Load();
                                //r.phoneReference.Load();
                                //r.emailReference.Load();
                                //r.imageReference.Load();
                            }
                        }
                        else
                        {
                            smeDirectors = new List<coreLogic.smeDirector>();
                        }
                        Session["smeDirectors"] = smeDirectors;
                        gridSMEDirector.DataSource = smeDirectors;
                        gridSMEDirector.DataBind();

                        noks = cl.nextOfKins.ToList();
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

                        banks = cl.clientBankAccounts.ToList();
                        foreach (var r in banks)
                        {
                            //r.bankAccountTypeReference.Load();                           
                        }
                        Session["banks"] = banks;
                        gridBank.DataSource = banks;
                        gridBank.DataBind();

                        if (cl.categoryID == 0)
                        {
                            tab1.Tabs[9].Visible = true;
                        }
                        if (cl.categoryID == 1)
                        {
                            tab1.Tabs[4].Visible = true;
                            tab1.Tabs[8].Visible = true;
                        }
                        emp = cl.employeeCategories.FirstOrDefault();
                        if (cl.categoryID == 2)
                            tab1.Tabs[5].Visible = true;
                        grp = cl.groupCategories.FirstOrDefault();
                        if (cl.categoryID == 3)
                            tab1.Tabs[6].Visible = true;
                        mic = cl.microBusinessCategories.FirstOrDefault();
                        if (cl.categoryID == 4)
                            tab1.Tabs[7].Visible = true;
                        staff = cl.staffCategory1.FirstOrDefault();
                        if (cl.categoryID == 5)
                        {
                            tab1.Tabs[10].Visible = true;
                            tab1.Tabs[11].Visible = true;
                        }

                        documents = cl.clientDocuments.ToList();
                        Session["clientDocuments"] = documents;

                        gridDocument.DataSource = documents;
                        gridDocument.DataBind();

                        cboBranch.SelectedValue = cl.branchID.ToString();
                        cboLoanGroup.SelectedValue = cl.loanGroupClients.Where(r => r.clientId == cl.clientID).FirstOrDefault().loanGroupId.ToString();
                        cboCategory.SelectedValue = cl.categoryID.ToString();
                        if (cl.idNo != null)
                        {
                            cboIDType.SelectedValue = cl.idNo.idNoTypeID.ToString();
                            txtIDNo.Text = cl.idNo.idNo1;
                            if (cl.idNo.expriryDate != null)
                            {
                                dpExpiryDate.SelectedDate = cl.idNo.expriryDate;
                            }
                        }
                        if (cl.idNo1 != null)
                        {
                            cboIDType2.SelectedValue = cl.idNo1.idNoTypeID.ToString();
                            txtIDNo2.Text = cl.idNo1.idNo1;
                            if (cl.idNo1.expriryDate != null)
                            {
                                dpExpiryDate2.SelectedDate = cl.idNo1.expriryDate;
                            }
                        }
                        cboIndustry.SelectedValue = cl.industryID.ToString();
                        cboMaritalStatus.SelectedValue = cl.maritalStatusID.ToString();
                        cboSector.SelectedValue = cl.sectorID.ToString();
                        txtAccNum.Text = cl.accountNumber;
                        txtOtherNames.Text = cl.otherNames;
                        txtSurname.Text = cl.surName;
                        txtSecondOtherNames.Text = cl.secondOtherNames;
                        txtSecondSurname.Text = cl.secondSurName;
                        txtThirdOtherNames.Text = cl.thrifOtherNames;
                        txtThirdSurname.Text = cl.thirdSurName;
                        txtJointAccountName.Text = cl.accountName;
                        dpDOB.SelectedDate = cl.DOB;
                        rblSex.SelectedValue = cl.sex;
                        cboClientType.SelectedValue = cl.clientTypeID.ToString();
                        chkIsCompany.Checked = cl.isCompany;
                        chkIsCompany_CheckedChanged(chkIsCompany, EventArgs.Empty);

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
                                img.ResizeMode = BinaryImageResizeMode.Fill;
                                img.DataValue = item.image.image1;
                                RadRotatorItem it = new RadRotatorItem();
                                it.Controls.Add(img);
                                rotator1.Items.Add(it);
                            }
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
                        if (officeEmail != null)
                        {
                            //officeEmail.emailReference.Load();
                            txtOfficeEmail.Text = officeEmail.email.emailAddress;
                        }
                        if (personalEmail != null)
                        {
                            //personalEmail.emailReference.Load();
                            txtPersonalEmail.Text = personalEmail.email.emailAddress;
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
                        if (cl.clientCompany != null)
                        {
                            //cl.clientCompany.phoneReference.Load();
                            //cl.clientCompany.addressReference.Load();
                            //cl.clientCompany.emailReference.Load();
                        }
                        if (cl.clientCompany != null && (cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5))
                        {
                            tab1.Tabs[12].Visible = true;
                            txtCompanyName.Text = cl.companyName;
                            txtContactOtherNames.Text = cl.clientCompany.contactOtherNames;
                            txtContactSurname.Text = cl.clientCompany.contactSurname;
                            if (cl.clientCompany.phone != null)
                            {
                                txtCompanyPhone.Text = cl.clientCompany.phone.phoneNo;
                            }
                            if (cl.clientCompany.address != null)
                            {
                                txtBusinessAddress.Text = cl.clientCompany.address.addressLine1;
                                txtCompanyCity.Text = cl.clientCompany.address.cityTown;
                            }
                            if (cl.clientCompany.email != null)
                            {
                                txtCompanyEmail.Text = cl.clientCompany.email.emailAddress;
                            }
                        }
                        if (cl.clientTypeID == 6)
                        {
                            pnlJoint.Visible = true;
                            pnlJoint2.Visible = true;
                            divSurnameLabel.InnerText = "1st Surname";
                            divOtherNamesLabel.InnerText = "1st Other Names";
                        }
                        else
                        {
                            pnlJoint.Visible = false;
                            pnlJoint2.Visible = false;
                            divSurnameLabel.InnerText = "Surname";
                            divOtherNamesLabel.InnerText = "Other Names";
                        }
                        if (sme != null)
                        {
                            txtSMECompName.Text = sme.companyName;
                            txtSMERegNo.Text = sme.regNo;
                            if (sme.regDate != null) dtSMERegDate.SelectedDate = sme.regDate;
                            if (sme.incDate != null) dtSMEIncDate.SelectedDate = sme.incDate;
                            //sme.address1Reference.Load();
                            //sme.addressReference.Load();
                            if (sme.address != null)
                            {
                                txtSMEPhyAddr1.Text = sme.address.addressLine1;
                                txtSMEPhyAddr2.Text = sme.address.addressLine2;
                                txtSMEPhyCity.Text = sme.address.cityTown;
                            }
                            if (sme.address1 != null)
                            {
                                txtSMERegAddr1.Text = sme.address1.addressLine1;
                                txtSMERegAddr2.Text = sme.address1.addressLine2;
                                this.txtSMERegAddrCity.Text = sme.address1.cityTown;
                            }
                        }
                        if (emp != null)
                        {
                            //emp.employerReference.Load();
                            if (emp.employer != null)
                            {
                                //emp.employer.employerDirectors.Load();
                                cboDirector.Items.Clear();
                                cboDirector.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                                foreach (var r in emp.employer.employerDirectors)
                                {
                                    cboDirector.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames, r.employerDirectorID.ToString()));
                                }

                                //emp.employer.employmentTypeReference.Load();
                                cboEmployer.SelectedValue = emp.employerID.ToString();
                                if (emp.employerDirectorID != null) cboDirector.SelectedValue = emp.employerDirectorID.ToString();
                                if (emp.employmentTypeID != null) cboEmploymentType.SelectedValue = emp.employmentTypeID.ToString();

                                //emp.employer.addressReference.Load();
                                if (emp.employer.address != null)
                                {
                                    txtEmpAddr1.Text = emp.employer.address.addressLine1;
                                    txtEmpAddr2.Text = emp.employer.address.addressLine2;
                                    txtEmpAddrCity.Text = emp.employer.address.cityTown;
                                }
                            }
                        }
                        if (grp != null)
                        {
                            if (grp.joinDate != null) dtGroupJoinDate.SelectedDate = grp.joinDate;
                            txtMembershipNo.Text = grp.membershipNo;
                            //grp.groupReference.Load();
                            if (grp.group != null)
                            {
                                cboGroup.SelectedValue = grp.groupID.ToString();
                                txtGroupName.Text = grp.group.groupName;
                                if (grp.group.groupSize != null) txtGroupSize.Value = grp.group.groupSize.Value;

                                //grp.group.addressReference.Load();
                                if (grp.group.address != null)
                                {
                                    txtGroupAddr1.Text = grp.group.address.addressLine1;
                                    txtGroupAddr2.Text = grp.group.address.addressLine2;
                                    txtGroupAddrCity.Text = grp.group.address.cityTown;
                                }
                            }
                        }
                        if (mic != null)
                        {
                            if (mic.lineOfBusinessID != null) cboLOB.SelectedValue = mic.lineOfBusinessID.Value.ToString();
                            txtMicroBOwner.Text = mic.businessOwner;
                            if (mic.dateEstablished != null) dtDateEstablished.SelectedDate = mic.dateEstablished;
                            if (mic.numberOfCloseCompetitors != null) txtMicroBComp.Value = mic.numberOfCloseCompetitors;
                        }
                        if (staff != null)
                        {
                            //staff.employerReference.Load();

                            cboStaffEmp.SelectedValue = staff.employerID.ToString();

                            if (staff.employer != null)
                            {
                                //staff.employer.employmentTypeReference.Load();
                                //staff.employer.employerDirectors.Load();
                                //staff.employer.employerDepartments.Load();
                                cboStaffDep.Items.Clear();
                                cboStaffDep.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                                foreach (var r in staff.employer.employerDepartments)
                                {
                                    cboStaffDep.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.departmentName, r.employerDepartmentID.ToString()));
                                }
                            }

                            if (staff.authOfficerName != null)
                                txtAuthOfficer.Text = staff.authOfficerName;
                            if (staff.authOfficerPosition != null)
                                txtAuthOfficerPosition.Text = staff.authOfficerPosition;
                            if (staff.authOfficerPhone != null)
                                txtAuthOfficerPhone.Text = staff.authOfficerPhone;

                            //staff.staffCategoryDirectors.Load();

                            //staff.employer.addressReference.Load();
                            if (staff.empAddress1 != null)
                            {
                                this.txtStaffAdd1.Text = staff.empAddress1;
                            }
                            if (staff.empAddress2 != null)
                            {
                                txtStaffAddr2.Text = staff.empAddress2;
                            }
                            if (staff.empAddressCity != null)
                            {
                                this.txtStaffCityTown.Text = staff.empAddressCity;
                            }

                            if (staff.employeeContractTypeID != null) cboStaffContract.SelectedValue = staff.employeeContractTypeID.Value.ToString();
                            if (staff.employerDepartmentID != null) cboStaffDep.SelectedValue = staff.employerDepartmentID.ToString();
                            txtSSN.Text = staff.ssn;
                            txtEmployeeNo.Text = staff.employeeNumber;
                            txtEmployeeNoOld.Text = staff.employeeNumberOld;
                            txtPosition.Text = staff.position;
                            if (staff.employmentStartDate != null)
                            {
                                dtpEmpStartDate.SelectedDate = staff.employmentStartDate;
                                txtLOS.Value = (DateTime.Now - staff.employmentStartDate.Value).TotalDays / 30;
                                txtLOS.Text = txtLOS.Value.Value.ToString();
                            }
                            if (staff.regionID != null)
                            {
                                cboRegion.SelectedValue = staff.regionID.ToString();
                            }

                        }

                    }
                }
                else
                {
                    cl = new coreLogic.client();
                    documents = new List<coreLogic.clientDocument>();
                    noks = new List<coreLogic.nextOfKin>();
                    banks = new List<coreLogic.clientBankAccount>();
                    Session["loan.cl"] = cl;
                    Session["noks"] = noks;
                    Session["clientDocuments"] = documents;
                    Session["banks"] = banks;
                    var st = le.staffs.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                    if (st != null)
                    {
                        cl.branchID = st.branchID;
                    }
                    else if (categoryID == "5")
                    {
                        cl.branchID = le.branches.OrderByDescending(p => p.branchID).FirstOrDefault().branchID;
                    }
                    else
                    {
                        cl.branchID = le.branches.OrderBy(p => p.branchID).FirstOrDefault().branchID;
                    }
                    cboBranch.SelectedValue = cl.branchID.ToString();

                }
                Session["loan.cl"] = cl;
                multi1.SelectedIndex = 0;
                tab1.SelectedIndex = 0;
            }
            else
            {
                int? id = null;
                if (Session["id"] != null)
                {
                    id = (int)Session["id"];
                }
                else if (Request.Params["id"] != null && Request.Params["id"].Length < 9)
                {
                    id = int.Parse(Request.Params["id"]);
                }
                LoadClient(id);
                ent = new coreLogic.core_dbEntities();
                txtLOS.Value = 0;
            }
            if (categoryID == "5")
            {
                cboCategory.SelectedValue = categoryID;
                cboCategory.Enabled = false;
                cboClientType.SelectedValue = "2";
                cboClientType.Enabled = false;
                cboSector.SelectedValue = "9";
                cboSector.Enabled = false;
                cboIndustry.SelectedValue = "4";
                cboIndustry.Enabled = false;
                cboRegion.Visible = true;
                divReg1.Visible = true;
                divReg2.Visible = true;

                tab1.Tabs[10].Visible = true;
                tab1.Tabs[11].Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        { 
            
            if (cl != null && txtSurname.Text != "" && txtOtherNames.Text != "" && cboCategory.SelectedValue != ""
                && cboIDType.SelectedValue != "" && txtIDNo.Text != "" && rblSex.SelectedValue != null && rblSex.SelectedValue != ""
                && cboClientType.SelectedValue != "")
            {

                //var isNokDetails = SaveNextOfKinDetails();
                //if (!isNokDetails)
                //{
                //    Session["noks"] = null;
                //    gridNOK.DataSource = null;
                //    HtmlHelper.MessageBox("Phone number must be at least 10 digits.",
                //                   "coreERP©: Invalid Input", IconType.warning);
                //}
                //else
                //{
                    if (Save())
                    {
                        Session["id"] = null;
                        HtmlHelper.MessageBox2("Client Data Saved Successfully!", ResolveUrl("~/ln/client/default.aspx?catID=" +
                            cboCategory.SelectedValue), "coreERP©: Success", IconType.ok);
                        Session["override"] = null;
                        Session["override"] = false;
                    }
                //}
            }
        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            if (cl != null && txtSurname.Text != "" && txtOtherNames.Text != "" && cboCategory.SelectedValue != ""
                && cboIDType.SelectedValue != "" && txtIDNo.Text != "" && rblSex.SelectedValue != null && rblSex.SelectedValue != ""
                && cboClientType.SelectedValue != "")
            {
                //var isNokDetails = SaveNextOfKinDetails();
                //if (!isNokDetails)
                //{
                //    Session["noks"] = null;
                //    gridNOK.DataSource = null;
                //    HtmlHelper.MessageBox("Phone number must be at least 10 digits.",
                //                   "coreERP©: Invalid Input", IconType.warning);
                //}
                //else
                //{
                    if (Save())
                    {
                        banks = cl.clientBankAccounts.ToList();
                        noks = cl.nextOfKins.ToList();
                        documents = cl.clientDocuments.ToList();

                        Session["id"] = cl.clientID;

                        gridDocument.DataSource = documents;
                        gridDocument.DataBind();
                        gridNOK.DataSource = noks;
                        gridNOK.DataBind();
                        gridBank.DataSource = banks;
                        gridBank.DataBind();

                        HtmlHelper.MessageBox("Client Data Saved Successfully!");
                    }
                //}
            }
        }

        private bool Save()
        {
            if (cl != null && txtSurname.Text != "" && cboCategory.SelectedValue != ""
                && cboIDType.SelectedValue != "" && txtIDNo.Text != "" && rblSex.SelectedValue != null && rblSex.SelectedValue != ""
                && cboClientType.SelectedValue != ""
                && dpDOB.SelectedDate != null)
            {
               

                if (cl.clientID >= 0 && (Session["override"] == null || (bool)Session["override"] == false))
                {
                    var surName = txtSurname.Text.ToLower().Trim();
                    var otherNames = txtOtherNames.Text.Trim().ToLower();
                    //var dob = dpDOB.SelectedDate.Value;

                    if (le.clients.FirstOrDefault(p => p.surName.Trim().ToLower() == surName && p.otherNames.Trim().ToLower() == otherNames) != null)
                    {
                        HtmlHelper.MessageBox("A client with the same name exist in the system. If this is indeed a different client click save again, otherwise exit the sreen and try again.",
                           "coreERP: Duplicate Client", IconType.warning);
                        Session["override"] = true;
                        return false;
                    }

                }
                if (dpDOB.SelectedDate == null || dpDOB.SelectedDate.Value > DateTime.Today)
                {
                    Session["noks"] = null;
                    gridNOK.DataSource = null;
                    
                    HtmlHelper.MessageBox("Invalid Client Birth Date: In the Future!",
                                   "coreERP©: Invalid Input", IconType.deny);
                    return false;
                }
                if (dpDOB.SelectedDate == null || dpDOB.SelectedDate.Value > DateTime.Today.AddYears(-17))
                {
                    Session["noks"] = null;
                    gridNOK.DataSource = null;
                    HtmlHelper.MessageBox("Invalid Client Birth Date: Too Young!",
                                   "coreERP©: Invalid Input", IconType.deny);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(cboLoanGroup.SelectedValue))
                {
                    Session["noks"] = null;
                    gridNOK.DataSource = null;
                    HtmlHelper.MessageBox("Please select a Loan Group!",
                                   "coreERP©: Invalid Input", IconType.deny);
                    return false;
                }

                if (cboBranch.SelectedValue != "") cl.branchID = int.Parse(cboBranch.SelectedValue);

                var clCnfg = le.clientConfigs.FirstOrDefault();
                var prof = ent.comp_prof.FirstOrDefault();
                cl.categoryID = int.Parse(cboCategory.SelectedValue);
                cl.creation_date = DateTime.Now;
                cl.creator = User.Identity.Name;
                cl.DOB = dpDOB.SelectedDate;
                cl.isCompany = chkIsCompany.Checked;
                if (clCnfg != null && clCnfg.admissionFeeEnabled)
                {
                    cl.admissionFee = txtAdmissionFee.Value.Value;
                }
                if (chkIsCompany.Checked == false)
                {
                    cl.companyName = "";
                }
                else
                {
                    cl.companyName = txtSurname.Text;
                }
                var idNo = cl.idNo;
                if (idNo == null && cboIDType.SelectedValue != "")
                {
                    idNo = new coreLogic.idNo
                    {
                        idNo1 = txtIDNo.Text,
                        idNoTypeID = int.Parse(cboIDType.SelectedValue)
                    };
                }
                if (dpExpiryDate.IsEmpty == false)
                {
                    idNo.expriryDate = dpExpiryDate.SelectedDate;
                }
                idNo.idNo1 = txtIDNo.Text;
                idNo.idNoTypeID = int.Parse(cboIDType.SelectedValue);
                cl.idNo = idNo;
                if (cboIDType2.SelectedValue != "" && txtIDNo2.Text != "")
                {
                    var idNo1 = cl.idNo1;
                    if (idNo1 == null && cboIDType2.SelectedValue != "")
                    {
                        idNo1 = new coreLogic.idNo
                        {
                            idNo1 = txtIDNo2.Text,
                            idNoTypeID = int.Parse(cboIDType2.SelectedValue)
                        };
                    }
                    if (dpExpiryDate2.IsEmpty == false)
                    {
                        idNo1.expriryDate = dpExpiryDate2.SelectedDate;
                    }
                    cl.idNo1 = idNo1;
                }
                if (cboIndustry.SelectedValue != "") cl.industryID = int.Parse(cboIndustry.SelectedValue);
                if (cboMaritalStatus.SelectedValue != "") cl.maritalStatusID = int.Parse(cboMaritalStatus.SelectedValue);
                cl.otherNames = txtOtherNames.Text;
                cl.secondOtherNames = txtSecondOtherNames.Text;
                cl.secondSurName = txtSecondSurname.Text;
                cl.thrifOtherNames = txtThirdOtherNames.Text;
                cl.thirdSurName = txtThirdSurname.Text;
                cl.accountName = txtJointAccountName.Text;
                if (cboSector.SelectedValue != "") cl.sectorID = int.Parse(cboSector.SelectedValue);
                cl.sex = rblSex.SelectedValue;
                cl.surName = txtSurname.Text;
                cl.clientTypeID = int.Parse(cboClientType.SelectedValue);
                if (cl.clientID <= 0)
                {
                    if (prof.traditionalLoanNo == true)
                    {
                        var t = cboCategory.SelectedValue;
                        if (t == "0") t = "1";
                        cl.accountNumber = //cboBranch.SelectedItem.Text.Substring(0, 2).ToUpper() +
                            idGen.NewClientAccountNumber(cl.branchID.Value, cl.categoryID.Value);
                    }
                    else
                    {
                        cl.accountNumber = //cboBranch.SelectedItem.Text.Substring(0, 2).ToUpper() +
                            coreLogic.coreExtensions.NextSystemNumber("loan.cl.accountNumber." + cboBranch.SelectedItem.Text.Substring(0, 2).ToUpper());
                    }
                    if (cl.clientTypeID == 8)
                    {
                        coreLogic.agent agnt = new coreLogic.agent();
                        agnt.branchID = cl.branchID;
                        agnt.accountTypeID = 1;
                        //agnt.bankBranchID = 
                        agnt.DOB = cl.DOB;
                        agnt.otherNames = cl.otherNames;
                        agnt.sex = cl.sex;
                        agnt.surName = cl.surName;
                        agnt.accountNumber = cboBranch.SelectedItem.Text.Substring(0, 2).ToUpper()
                        + coreLogic.coreExtensions.NextSystemNumber("loan.agent.agentNo." + cboBranch.SelectedItem.Text.Substring(0, 2).ToUpper());
                        agnt.accountName = agnt.otherNames + " " + agnt.surName;
                        le.agents.Add(agnt);
                    }
                    le.clients.Add(cl);
                }
                else if (txtAccNum.Text.Trim().Length > 3)
                {
                    cl.accountNumber = cboBranch.SelectedItem.Text.Substring(0, 2).ToUpper() + txtAccNum.Text;
                }
                if (cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5)
                {
                    if (txtCompanyName.Text != "")
                    {
                        if (cl.clientCompany == null)
                        {
                            cl.clientCompany = new coreLogic.clientCompany
                            {
                                companyName = txtCompanyName.Text,
                                address = new coreLogic.address
                                {
                                    addressLine1 = txtBusinessAddress.Text,
                                    addressLine2 = "",
                                    cityTown = txtCompanyCity.Text
                                },
                                phone = new coreLogic.phone
                                {
                                    phoneTypeID = 1,
                                    phoneNo = txtCompanyPhone.Text
                                },
                                email = new coreLogic.email
                                {
                                    emailAddress = txtCompanyEmail.Text,
                                    emailTypeID = 1
                                },
                                contactOtherNames = txtContactOtherNames.Text,
                                contactSurname = txtContactSurname.Text
                            };
                        }
                        else
                        {
                            cl.clientCompany.contactSurname = txtContactSurname.Text;
                            cl.clientCompany.contactOtherNames = txtContactOtherNames.Text;
                            cl.clientCompany.companyName = txtCompanyName.Text;
                            if (cl.clientCompany.phone == null)
                            {
                                cl.clientCompany.phone = new coreLogic.phone
                                {
                                    phoneTypeID = 1,
                                    phoneNo = txtCompanyPhone.Text
                                };
                            }
                            else
                            {
                                cl.clientCompany.phone.phoneNo = txtCompanyPhone.Text;
                            }
                            if (cl.clientCompany.email == null)
                            {
                                cl.clientCompany.email = new coreLogic.email
                                {
                                    emailAddress = txtCompanyEmail.Text,
                                    emailTypeID = 1
                                };
                            }
                            else
                            {
                                cl.clientCompany.email.emailAddress = txtCompanyEmail.Text;
                            }
                            if (cl.clientCompany.address == null)
                            {
                                cl.clientCompany.address = new coreLogic.address
                                {
                                    addressLine1 = txtBusinessAddress.Text,
                                    addressLine2 = "",
                                    cityTown = txtCompanyCity.Text
                                };
                            }
                            else
                            {
                                cl.clientCompany.address.cityTown = txtCompanyCity.Text;
                                cl.clientCompany.address.addressLine1 = txtBusinessAddress.Text;
                            }
                        }
                        cl.companyName = txtCompanyName.Text;
                    }
                }
                if (txtPhyAddr1.Text != "" && txtPhyCityTown.Text != "")
                {
                    phyAddr = cl.clientAddresses.FirstOrDefault(p => p.addressTypeID == 1);
                    if (phyAddr == null)
                    {
                        phyAddr = new coreLogic.clientAddress();
                        cl.clientAddresses.Add(phyAddr);
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
                bool found = false;
                foreach (Telerik.Web.UI.UploadedFile item in upload3.UploadedFiles)
                {
                    coreLogic.clientImage img = new coreLogic.clientImage();
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
                    cl.clientImages.Add(img);
                    found = true;
                }
                if (found == false && Session["imageFromCamera"] != null)
                {
                    coreLogic.clientImage img = new coreLogic.clientImage();
                    byte[] b = Convert.FromBase64String(Session["imageFromCamera"].ToString());

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
                        description = "Webcam-Picure",
                        image1 = b,
                        content_type = "image/png"
                    };
                    img.image = i;
                    cl.clientImages.Add(img);
                }
                if (txtMailAddr1.Text != "" && txtMailAddrCity.Text != "")
                {
                    mailAddr = cl.clientAddresses.FirstOrDefault(p => p.addressTypeID == 2);
                    if (mailAddr == null)
                    {
                        mailAddr = new coreLogic.clientAddress();
                        cl.clientAddresses.Add(mailAddr);
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
                    workPhone = cl.clientPhones.FirstOrDefault(p => p.phoneTypeID == 1);
                    if (workPhone == null)
                    {
                        workPhone = new coreLogic.clientPhone();
                        cl.clientPhones.Add(workPhone);
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
                if (!string.IsNullOrWhiteSpace(txtMobilePhone.Text) )
                {
                    if (txtMobilePhone.Text.Length != 10) {

                        HtmlHelper.MessageBox("Mobile number should be 10 digits",
                                   "coreERP©: Invalid Input", IconType.deny);
                        
                        return false;
                    }
                    mobilePhone = cl.clientPhones.FirstOrDefault(p => p.phoneTypeID == 2);
                    if (mobilePhone == null)
                    {
                        mobilePhone = new coreLogic.clientPhone();
                        cl.clientPhones.Add(mobilePhone);
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
                    homePhone = cl.clientPhones.FirstOrDefault(p => p.phoneTypeID == 3);
                    if (homePhone == null)
                    {
                        homePhone = new coreLogic.clientPhone();
                        cl.clientPhones.Add(homePhone);
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
                if (txtOfficeEmail.Text != "")
                {
                    var officeEmail = cl.clientEmails.FirstOrDefault(p => p.emailTypeID == 1);
                    if (officeEmail == null)
                    {
                        officeEmail = new coreLogic.clientEmail();
                        cl.clientEmails.Add(officeEmail);
                        officeEmail.email = new coreLogic.email();
                    }
                    else
                    {
                        //officeEmail.emailReference.Load();
                    }
                    officeEmail.email.emailAddress = txtOfficeEmail.Text;
                    officeEmail.emailTypeID = 1;
                    officeEmail.email.emailTypeID = 1;
                }
                if (txtPersonalEmail.Text != "")
                {
                    var personalEmail = cl.clientEmails.FirstOrDefault(p => p.emailTypeID == 1);
                    if (personalEmail == null)
                    {
                        personalEmail = new coreLogic.clientEmail();
                        cl.clientEmails.Add(personalEmail);
                        personalEmail.email = new coreLogic.email();
                    }
                    else
                    {
                        //personalEmail.emailReference.Load();
                    }
                    personalEmail.email.emailAddress = txtPersonalEmail.Text;
                    personalEmail.emailTypeID = 2;
                    personalEmail.email.emailTypeID = 2;
                }
                if (cboCategory.SelectedValue == "0")
                {
                    foreach (var r in noks)
                    {
                        if (!cl.nextOfKins.Contains(r) && r.nextOfKinID <= 0)
                        {
                            try
                            {
                                cl.nextOfKins.Add(r);
                            }
                            catch (Exception x2) { }
                        }
                    }
                    for (int i = cl.nextOfKins.Count - 1; i >= 0; i--)
                    {
                        var r = cl.nextOfKins.ToList()[i];
                        if (!noks.Contains(r))
                        {
                            cl.nextOfKins.Remove(r);
                        }
                    }
                }
                foreach (var r in banks)
                {
                    if (r.clientBankAccountID == 0 && !cl.clientBankAccounts.Contains(r))
                    {
                        try
                        {
                            cl.clientBankAccounts.Add(r);
                        }
                        catch (Exception) { }
                    }
                }
                for (int i = cl.clientBankAccounts.Count - 1; i >= 0; i--)
                {
                    var r = cl.clientBankAccounts.ToList()[i];
                    if (!banks.Contains(r))
                    {
                        cl.clientBankAccounts.Remove(r);
                    }
                }
                if (cboCategory.SelectedValue == "1")
                {
                    sme = cl.smeCategories.FirstOrDefault();
                    if (sme == null)
                    {
                        sme = new coreLogic.smeCategory();
                        sme.address = new coreLogic.address();
                        sme.address1 = new coreLogic.address();
                        cl.smeCategories.Add(sme);
                    }
                    else
                    {
                        //sme.address1Reference.Load();
                        //sme.addressReference.Load();
                    }
                    sme.companyName = txtSMECompName.Text;
                    if (dtSMERegDate.SelectedDate != null) sme.regDate = dtSMERegDate.SelectedDate;
                    if (dtSMEIncDate.SelectedDate != null) sme.incDate = dtSMEIncDate.SelectedDate;
                    sme.regNo = txtSMERegNo.Text;
                    sme.address.addressLine1 = txtSMEPhyAddr1.Text;
                    sme.address.addressLine2 = txtSMEPhyAddr2.Text;
                    sme.address.cityTown = txtSMEPhyCity.Text;
                    sme.address1.addressLine1 = txtSMERegAddr1.Text;
                    sme.address1.addressLine2 = txtSMERegAddr2.Text;
                    sme.address1.cityTown = txtSMERegAddrCity.Text;
                    foreach (var r in smeDirectors)
                    {
                        if (!sme.smeDirectors.Contains(r))
                        {
                            sme.smeDirectors.Add(r);
                        }
                    }
                }
                if (cboCategory.SelectedValue == "2" && cboEmployer.SelectedValue != "")
                {
                    emp = cl.employeeCategories.FirstOrDefault();
                    if (emp == null)
                    {
                        emp = new coreLogic.employeeCategory();
                        cl.employeeCategories.Add(emp);
                    }
                    emp.employerID = int.Parse(cboEmployer.SelectedValue);
                    if (cboEmploymentType.SelectedValue != "")
                    {
                        emp.employmentTypeID = int.Parse(cboEmploymentType.SelectedValue);
                    }
                    if (cboDirector.SelectedValue != "")
                    {
                        emp.employerDirectorID = int.Parse(cboDirector.SelectedValue);
                    }
                }
                if (cboCategory.SelectedValue == "5" && cboStaffEmp.SelectedValue != "")
                {
                    if (cl.clientID > 0)
                    {
                        staff = cl.staffCategory1.FirstOrDefault();
                    }
                    if (staff == null)
                    {
                        staff = new coreLogic.staffCategory1();
                        cl.staffCategory1.Add(staff);
                    }
                    var ln2 = le.staffCategory1.FirstOrDefault(p => p.employeeNumber == txtEmployeeNo.Text.Trim());
                    if (ln2 != null && ln2.staffCategoryID != staff.staffCategoryID)
                    {
                        Session["noks"] = null;
                        gridNOK.DataSource = null;
                        HtmlHelper.MessageBox("There exist another client with same employee number", "CoreERP: Invalid",
                             IconType.deny);
                        return false;
                    }

                    staff.employerID = int.Parse(cboStaffEmp.SelectedValue);
                    staff.ssn = txtSSN.Text;
                    staff.employeeNumber = txtEmployeeNo.Text;
                    staff.employeeNumberOld = txtEmployeeNoOld.Text;
                    staff.lengthOfService = txtLOS.Value.Value;
                    staff.employmentStartDate = dtpEmpStartDate.SelectedDate;
                    staff.position = txtPosition.Text;

                    staff.authOfficerName = txtAuthOfficer.Text;
                    staff.authOfficerPhone = txtAuthOfficerPhone.Text;
                    staff.authOfficerPosition = txtAuthOfficerPosition.Text;

                    staff.empAddress1 = txtStaffAdd1.Text;
                    staff.empAddress2 = txtStaffAddr2.Text;
                    staff.empAddressCity = txtStaffCityTown.Text;

                    if (cboRegion.SelectedValue != "")
                    {
                        staff.regionID = int.Parse(cboRegion.SelectedValue);
                    }
                    if (cboStaffContract.SelectedValue != "")
                    {
                        staff.employeeContractTypeID = int.Parse(cboStaffContract.SelectedValue);
                    }
                    if (cboStaffDep.SelectedValue != "")
                    {
                        staff.employerDepartmentID = int.Parse(cboStaffDep.SelectedValue);
                    }
                }
                if (cboCategory.SelectedValue == "3")
                {
                    grp = cl.groupCategories.FirstOrDefault();
                    if (grp == null)
                    {
                        grp = new coreLogic.groupCategory();
                        cl.groupCategories.Add(grp);
                    }
                    if (cboGroup.SelectedValue != "") grp.groupID = int.Parse(cboGroup.SelectedValue);
                    if (dtGroupJoinDate.SelectedDate != null) grp.joinDate = dtGroupJoinDate.SelectedDate;
                    grp.membershipNo = txtMembershipNo.Text;
                }
                if (cboCategory.SelectedValue == "4")
                {
                    mic = cl.microBusinessCategories.FirstOrDefault();
                    if (mic == null)
                    {
                        mic = new coreLogic.microBusinessCategory();
                        cl.microBusinessCategories.Add(mic);
                    }
                    if (cboLOB.SelectedValue != "")
                    {
                        mic.lineOfBusiness = cboLOB.SelectedItem.Text;
                        mic.lineOfBusinessID = int.Parse(cboLOB.SelectedValue);
                    }
                    mic.businessOwner = txtMicroBOwner.Text;
                    if (dtDateEstablished.SelectedDate != null) mic.dateEstablished = dtDateEstablished.SelectedDate;
                    if (txtMicroBComp.Text != null && txtMicroBComp.Text != "") mic.numberOfCloseCompetitors = (int)txtMicroBComp.Value;
                }
               
                var selectedLoanGroupId = int.Parse(cboLoanGroup.SelectedValue);
                cl.loanGroupClients.Add(new loanGroupClient
                {
                    loanGroupId=selectedLoanGroupId,
                    created = DateTime.Now,
                    creator = User.Identity.Name
                });
                
                try
                {
                    le.SaveChanges();
                }
                catch (Exception x)
                {
                }

                Session["imageFromCamera"] = null;
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
            for (int i = 3; i < 11; i++)
            {
                tab1.Tabs[i].Visible = false;
            }
            if (cboCategory.SelectedValue == "0")
            {
                tab1.Tabs[9].Visible = true;
            }
            if (cboCategory.SelectedValue == "1")
            {
                tab1.Tabs[4].Visible = true;
                tab1.Tabs[8].Visible = true;
            }
            if (cboCategory.SelectedValue == "2")
                tab1.Tabs[5].Visible = true;
            if (cboCategory.SelectedValue == "3")
                tab1.Tabs[6].Visible = true;
            if (cboCategory.SelectedValue == "4")
                tab1.Tabs[7].Visible = true;
            if (cboCategory.SelectedValue == "5")
            {
                tab1.Tabs[10].Visible = true;
                tab1.Tabs[11].Visible = true;
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
                foreach (var r in bank.bank_branches.OrderBy(p => p.branch_name))
                {
                    cboBankBranch.Items.Add(new RadComboBoxItem(r.branch_name, r.branch_id.ToString()));
                }
            }
        }

        protected void cboEmployer_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboEmployer.SelectedValue != null && cboEmployer.SelectedValue != "")
            {
                var id = int.Parse(cboEmployer.SelectedValue);
                var empy = le.employers.FirstOrDefault(p => p.employerID == id);

                if (empy != null)
                {
                    //empy.employmentTypeReference.Load();
                    //empy.employerDirectors.Load();
                    cboDirector.Items.Clear();
                    cboDirector.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                    foreach (var r in empy.employerDirectors)
                    {
                        cboDirector.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames, r.employerDirectorID.ToString()));
                    }

                    cboDirector.SelectedValue = "";
                    if (empy.employmentType != null) cboEmploymentType.SelectedValue = empy.employmentTypeID.ToString();

                    //empy.addressReference.Load();
                    if (empy.address != null)
                    {
                        txtEmpAddr1.Text = empy.address.addressLine1;
                        txtEmpAddr2.Text = empy.address.addressLine2;
                        txtEmpAddrCity.Text = empy.address.cityTown;
                    }
                }
                else
                {
                    cboDirector.SelectedValue = "";
                    cboEmploymentType.SelectedValue = "";

                    txtEmpAddr1.Text = "";
                    txtEmpAddr2.Text = "";
                    txtEmpAddrCity.Text = "";
                }
            }
        }

        protected void cboStaffEmp_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboStaffEmp.SelectedValue != null && cboStaffEmp.SelectedValue != "")
            {
                var id = int.Parse(cboStaffEmp.SelectedValue);
                var empy = le.employers.FirstOrDefault(p => p.employerID == id);

                if (empy != null)
                {
                    //empy.employmentTypeReference.Load();
                    //empy.employerDirectors.Load(); 
                    //empy.employerDepartments.Load();
                    cboStaffDep.Items.Clear();
                    cboStaffDep.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                    foreach (var r in empy.employerDepartments)
                    {
                        cboStaffDep.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.departmentName, r.employerDepartmentID.ToString()));
                    }
                }
                else
                {
                }
            }
        }

        protected void gridSMEDirector_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = smeDirectors[e.Item.ItemIndex];
                if (g != null)
                {
                    txtSMEOtherNames.Text = g.otherNames;
                    txtSMESurname.Text = g.surName;

                    if (g.phone != null)
                    {
                        txtSMEPhoneNo.Text = g.phone.phoneNo;
                    }
                    if (g.email != null)
                    {
                        txtSMEEmailAddress.Text = g.email.emailAddress;
                    }
                    if (g.idNo != null)
                    {
                        txtSMEIDNo.Text = g.idNo.idNo1;
                        cboSMEIDType.SelectedValue = g.idNo.idNoTypeID.ToString();
                    }
                    Session["smeDirector"] = g;
                    btnAddSMEDirector.Text = "Update Director Details";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    var dir = smeDirectors[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted = lent.smeDirectors.FirstOrDefault(p => p.smeDirectorID == dir.smeDirectorID);
                        if (toBeDeleted != null)
                        {
                            lent.smeDirectors.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }
                smeDirectors.RemoveAt(e.Item.ItemIndex);
                Session["smeDirectors"] = smeDirectors;
            }
            gridSMEDirector.DataSource = smeDirectors;
            gridSMEDirector.DataBind();
        }

        protected void btnAddSMEDirector_Click(object sender, EventArgs e)
        {
            if (txtSMESurname.Text != "" && txtSMEOtherNames.Text != "")
            {
                coreLogic.smeDirector g;
                if (btnAddSMEDirector.Text == "Add Director Details")
                {
                    g = new coreLogic.smeDirector();
                }
                else
                {
                    g = Session["smeDirector"] as coreLogic.smeDirector;
                }
                g.surName = txtSMESurname.Text;
                g.otherNames = txtSMEOtherNames.Text;
                if (cboSMEIDType.SelectedValue != "" && txtSMEIDNo.Text != "")
                {
                    if (g.idNo == null) g.idNo = new coreLogic.idNo();
                    g.idNo.idNoTypeID = int.Parse(cboSMEIDType.SelectedValue);
                    g.idNo.idNo1 = txtSMEIDNo.Text;
                }
                if (txtSMEPhoneNo.Text != "")
                {
                    if (g.phone == null) g.phone = new coreLogic.phone();
                    g.phone.phoneNo = txtSMEPhoneNo.Text;
                    g.phone.phoneTypeID = 1;
                }
                if (txtSMEEmailAddress.Text != "")
                {
                    if (g.email == null) g.email = new coreLogic.email();
                    g.email.emailAddress = txtSMEEmailAddress.Text;
                    g.email.emailTypeID = 1;
                }
                if (upload2.UploadedFiles.Count == 1)
                {
                    var item = upload2.UploadedFiles[0];
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
                if (btnAddSMEDirector.Text == "Add Director Details")
                {
                    smeDirectors.Add(g);
                }
                Session["smeDirectors"] = smeDirectors;
                gridSMEDirector.DataSource = smeDirectors;
                gridSMEDirector.DataBind();

                txtSMEOtherNames.Text = "";
                txtSMESurname.Text = "";
                txtSMEPhoneNo.Text = "";
                txtSMEEmailAddress.Text = "";
                txtSMEIDNo.Text = "";
                cboSMEIDType.SelectedValue = "";
                btnAddSMEDirector.Text = "Add Director Details";
            }
        }

        protected void gridDocument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    var doc = documents[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted = lent.clientDocuments.FirstOrDefault(p => p.clientDocumentID == doc.clientDocumentID);
                        if (toBeDeleted != null)
                        {
                            lent.clientDocuments.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }
                Session["clientDocuments"] = documents;
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
                        var g = new coreLogic.clientDocument
                        {
                            client = cl,
                            document = i
                        };
                        documents.Add(g);
                    }
                }
                Session["clientDocuments"] = documents;
                gridDocument.DataSource = documents;
                gridDocument.DataBind();

                txtDocDesc.Text = "";
                btnAddDcoument.Text = "Add Document";
            }
        }

        protected void cboGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboGroup.SelectedValue != null && cboGroup.SelectedValue != "")
            {
                var id = int.Parse(cboGroup.SelectedValue);
                var grp = le.groups.FirstOrDefault(p => p.groupID == id);

                if (grp != null)
                {
                    txtGroupName.Text = grp.groupName;
                    if (grp.groupSize != null) txtGroupSize.Value = grp.groupSize.Value;

                    //grp.addressReference.Load();
                    if (grp.address != null)
                    {
                        txtGroupAddr1.Text = grp.address.addressLine1;
                        txtGroupAddr2.Text = grp.address.addressLine2;
                        txtGroupAddrCity.Text = grp.address.cityTown;
                    }
                }
                else
                {
                    txtGroupName.Text = "";
                    txtGroupSize.Value = null;

                    txtGroupAddr1.Text = "";
                    txtGroupAddr2.Text = "";
                    txtGroupAddrCity.Text = "";
                }
            }
        }

        private bool SaveNextOfKinDetails() {

            if (txtNOKSurname.Text != "" && txtNOKOtherNames.Text != "")
            {
                coreLogic.nextOfKin g;
                if (btnAddNOK.Text == "Add Next of Kin Details")
                {
                    g = new coreLogic.nextOfKin();
                }
                else
                {
                    g = Session["nok"] as coreLogic.nextOfKin;
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
                if (!string.IsNullOrWhiteSpace(txtNOKPhoneNumber.Text))
                {
                    if (txtNOKPhoneNumber.Text.Length != 10)
                    {

                        HtmlHelper.MessageBox("Next of kin Phone Number should be 10 digits",
                                   "coreERP©: Invalid Input", IconType.deny);

                        return false;
                    }
                    else
                    {
                        if (g.phone == null) g.phone = new coreLogic.phone();
                        g.phone.phoneNo = txtNOKPhoneNumber.Text;
                        g.phone.phoneTypeID = 1;
                    }
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
                if (string.IsNullOrWhiteSpace(g.phone.phoneNo))
                {
                    Session["noks"] = null;
                    gridNOK.DataSource = null;
                    HtmlHelper.MessageBox("Phone number should be 10 digits",
                                   "coreERP©: Invalid Input", IconType.warning);
                    return false;
                }
                else
                {
                    if (btnAddNOK.Text == "Add Next of Kin Details")
                    {
                        noks.Add(g);
                    }
                    Session["noks"] = noks;
                    gridNOK.DataSource = noks;
                    gridNOK.DataBind();

                    //txtNOKOtherNames.Text = "";
                    //txtNOKSurname.Text = "";
                    //txtNOKPhoneNumber.Text = "";
                    //txtNOKEmailAddress.Text = "";
                    //txtNOKIDNumber.Text = "";
                    //cboNOKIDType.SelectedValue = "";
                    //txtRelation.Text = "";
                    btnAddNOK.Text = "Add Next of Kin Details";
                }

            }
            else {
                Session["noks"] = null;
                gridNOK.DataSource = null;
                HtmlHelper.MessageBox("Please provide Next of Kin's Surname and Othername",
                               "coreERP©: Invalid Input", IconType.warning);
                return false;
            }
            return true;
        }

        protected void btnAddNOK_Click(object sender, EventArgs e)
        {
            if (txtNOKSurname.Text != "" && txtNOKOtherNames.Text != "")
            {
                coreLogic.nextOfKin g;
                if (btnAddNOK.Text == "Add Next of Kin Details")
                {
                    g = new coreLogic.nextOfKin();
                }
                else
                {
                    g = Session["nok"] as coreLogic.nextOfKin;
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
                    if (txtNOKPhoneNumber.Text.Length != 10)
                    {

                        HtmlHelper.MessageBox("Phone Number should be 10 digits",
                                   "coreERP©: Invalid Input", IconType.deny);

                       
                    }
                    else { 
                    if (g.phone == null) g.phone = new coreLogic.phone();
                    g.phone.phoneNo = txtNOKPhoneNumber.Text;
                    g.phone.phoneTypeID = 1;
                    }
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
                //if (string.IsNullOrWhiteSpace(g.phone))
                if (g.phone == null) 
                {
                    Session["noks"] = null;
                    gridNOK.DataSource = null;
                    HtmlHelper.MessageBox(" Next of Kin's phone number should be 10 digits.",
                                   "coreERP©: Invalid Input", IconType.warning);
                    return;
                }
                else
                {
                    if (btnAddNOK.Text == "Add Next of Kin Details")
                    {
                        noks.Add(g);
                    }
                    Session["noks"] = noks;
                    gridNOK.DataSource = noks;
                    gridNOK.DataBind();

                    //txtNOKOtherNames.Text = "";
                    //txtNOKSurname.Text = "";
                    //txtNOKPhoneNumber.Text = "";
                    //txtNOKEmailAddress.Text = "";
                    //txtNOKIDNumber.Text = "";
                    //cboNOKIDType.SelectedValue = "";
                    //txtRelation.Text = "";
                    btnAddNOK.Text = "Add Next of Kin Details";
                }
                
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
                try
                {
                    var nok = noks[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted = lent.nextOfKins.FirstOrDefault(p => p.nextOfKinID == nok.nextOfKinID);
                        if (toBeDeleted != null)
                        {
                            lent.nextOfKins.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }
                noks.RemoveAt(e.Item.ItemIndex);
                Session["noks"] = noks;
            }
            gridNOK.DataSource = noks;
            gridNOK.DataBind();
        }

        public string GetBank(int branchID)
        {
            var b = "";
            var branch = (new coreLogic.core_dbEntities()).bank_branches.FirstOrDefault(p => p.branch_id == branchID);
            if (branch != null)
            {
                //branch.banksReference.Load();
                b = branch.banks.bank_name;
            }

            return b;
        }

        public string GetBranch(int branchID)
        {
            var b = "";
            var branch = (new coreLogic.core_dbEntities()).bank_branches.FirstOrDefault(p => p.branch_id == branchID);
            if (branch != null)
            {
                b = branch.branch_name;
            }

            return b;
        }

        protected void btnAddBank_Click(object sender, EventArgs e)
        {
            if (txtAccountName.Text != "" && txtAccountNo.Text != "" && cboAccountType.SelectedValue != ""
                && cboBank.SelectedValue != "" && cboBankBranch.SelectedValue != "")
            {
                coreLogic.clientBankAccount g;
                if (btnAddBank.Text == "Add Bank Account")
                {
                    g = new coreLogic.clientBankAccount();
                }
                else
                {
                    g = Session["bank"] as coreLogic.clientBankAccount;
                }
                g.accountName = txtAccountName.Text;
                g.accountNumber = txtAccountNo.Text;
                g.accountTypeID = int.Parse(cboAccountType.SelectedValue);
                g.branchID = int.Parse(cboBankBranch.SelectedValue);
                g.isPrimary = chkPrimary.Checked;
                if (btnAddBank.Text == "Add Bank Account")
                {
                    banks.Add(g);
                }
                Session["banks"] = banks;
                gridBank.DataSource = banks;
                gridBank.DataBind();

                txtAccountName.Text = "";
                txtAccountNo.Text = "";
                cboBankBranch.SelectedValue = "";
                cboAccountType.SelectedValue = "";
                cboBank.SelectedValue = "";
                chkPrimary.Checked = false;
                btnAddBank.Text = "Add Bank Account";
            }
        }

        protected void gridBank_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = banks[e.Item.ItemIndex];
                if (g != null)
                {
                    txtAccountNo.Text = g.accountNumber;
                    txtAccountName.Text = g.accountName;

                    if (g.bankAccountType != null)
                    {
                        cboAccountType.SelectedValue = g.bankAccountType.accountTypeID.ToString();
                    }
                    if (g.branchID > 0)
                    {
                        var br = ent.bank_branches.FirstOrDefault(p => p.branch_id == g.branchID);
                        if (br != null)
                        {
                            cboIT.SelectedValue = br.banks.institution_type;
                            cboIT_SelectedIndexChanged(cboIT, new RadComboBoxSelectedIndexChangedEventArgs("", "", "", ""));
                            cboBank.SelectedValue = br.banks.bank_id.ToString();
                            cboBank_SelectedIndexChanged(cboBank, new RadComboBoxSelectedIndexChangedEventArgs("", "", "", ""));
                            cboBankBranch.SelectedValue = g.branchID.ToString();
                        }
                    }
                    Session["bank"] = g;
                    btnAddBank.Text = "Update Bank Account";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    var bank = banks[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted = lent.clientBankAccounts.FirstOrDefault(p => p.clientBankAccountID == bank.clientBankAccountID);
                        if (toBeDeleted != null)
                        {
                            lent.clientBankAccounts.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }
                banks.RemoveAt(e.Item.ItemIndex);
                Session["banks"] = banks;
            }
            gridBank.DataSource = banks;
            gridBank.DataBind();
        }

        protected void dtpEmpStartDate_SelectedDateChanged(object sender, EventArgs args)
        {
            if (dtpEmpStartDate.SelectedDate != null)
            {
                txtLOS.Value = Math.Round((DateTime.Now - dtpEmpStartDate.SelectedDate.Value).TotalDays / 30, 0);
            }
            else if (txtLOS.Value == null)
            {
                txtLOS.Value = 0;
            }

        }

        protected void cboIT_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cboBank.Items.Clear();
            RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem("", "");
            if (cboIT.SelectedValue != "")
            {
                cboBank.Items.Add(item);
                foreach (var r in ent.banks.Where(p => p.institution_type == cboIT.SelectedValue).OrderBy(p => p.bank_name))
                {
                    item = new Telerik.Web.UI.RadComboBoxItem(r.bank_name, r.bank_id.ToString());
                    cboBank.Items.Add(item);
                }
            }
            else
            {
                cboBank.Items.Add(item);
                foreach (var r in ent.banks.OrderBy(p => p.bank_name))
                {
                    item = new Telerik.Web.UI.RadComboBoxItem(r.bank_name, r.bank_id.ToString());
                    cboBank.Items.Add(item);
                }
            }
        }

        protected void chkIsCompany_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsCompany.Checked == true)
            {
            }
            else
            {
            }
        }

        protected void cboClientType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClientType.SelectedValue != "")
            {
                int id = int.Parse(cboClientType.SelectedValue);
                if (id == 3 || id == 4 || id == 5)
                {
                    tab1.Tabs[12].Visible = true;
                }
                else
                {
                    tab1.Tabs[12].Visible = false;
                }
                if (id == 6)
                {
                    pnlJoint.Visible = true;
                    pnlJoint2.Visible = true;
                    divSurnameLabel.InnerText = "1st Surname";
                    divOtherNamesLabel.InnerText = "1st Other Names";
                }
                else
                {
                    pnlJoint.Visible = false;
                    pnlJoint2.Visible = false;
                    divSurnameLabel.InnerText = "Surname";
                    divOtherNamesLabel.InnerText = "Other Names";
                }
            }
        }

        private void RenderImages()
        {
            bool found = false;
            if (cl.clientImages != null)
            {
                var i = cl.clientImages.FirstOrDefault();
                if (i != null)
                {
                    RadBinaryImage mg = new RadBinaryImage();
                    mg.Width = 216;
                    mg.Height = 216;
                    mg.ResizeMode = BinaryImageResizeMode.Fill;
                    mg.DataValue = i.image.image1;
                    RadBinaryImage1.DataValue = mg.DataValue;
                }

                foreach (var item in cl.clientImages)
                {
                    //item.imageReference.Load();
                    RadBinaryImage img = new RadBinaryImage();
                    img.Width = 216;
                    img.Height = 216;
                    img.ResizeMode = BinaryImageResizeMode.Fill;
                    img.DataValue = item.image.image1;
                    RadRotatorItem it = new RadRotatorItem();
                    it.Controls.Add(img);
                   // rotator2.Items.Add(it);
                    found = true;
                }
            }
            if (found == false && Session["imageFromCamera"] != null)
            {
                RadBinaryImage img = new RadBinaryImage();
                img.Width = 216;
                img.Height = 216;
                img.ResizeMode = BinaryImageResizeMode.Fill;
                var data = Convert.FromBase64String(Session["imageFromCamera"].ToString());
                img.DataValue = data;
                RadRotatorItem it = new RadRotatorItem();
                it.Controls.Add(img);
                //rotator2.Items.Add(it);
            }
        }

        private void LoadClient(int? id)
        {
            cl = le.clients.FirstOrDefault(p => p.clientID == id);
            if (cl != null)
            {
                //cl.idNoReference.Load();
                //cl.idNo1Reference.Load();
                //cl.clientAddresses.Load();
                //cl.clientPhones.Load();
                //cl.clientEmails.Load();
                //cl.smeCategories.Load();
                //cl.employeeCategories.Load();
                //cl.groupCategories.Load();
                //cl.microBusinessCategories.Load();
                ////cl.clientImages.Load();
                //cl.clientDocuments.Load();
                //cl.nextOfKins.Load();
                //cl.staffCategory1.Load();
                //cl.clientBankAccounts.Load();
                //cl.clientCompanyReference.Load();

                foreach (var r in cl.clientImages)
                {
                    //r.imageReference.Load();
                }

                sme = cl.smeCategories.FirstOrDefault();
                if (sme != null)
                {
                    //sme.smeDirectors.Load();
                    smeDirectors = sme.smeDirectors.ToList();
                    foreach (var r in smeDirectors)
                    {
                        //r.idNoReference.Load();
                        //r.phoneReference.Load();
                        //r.emailReference.Load();
                        //r.imageReference.Load();
                    }
                }
                else
                {
                    smeDirectors = new List<coreLogic.smeDirector>();
                }

                if (Session["smeDirectors"] != null)
                {
                    var g = Session["smeDirectors"] as List<coreLogic.smeDirector>;
                    if (g != null)
                    {
                        foreach (var r in g)
                        {
                            if (r.smeDirectorID <= 0)
                            {
                                smeDirectors.Add(r);
                            }
                        }
                    }
                }
                Session["smeDirectors"] = smeDirectors;

                noks = cl.nextOfKins.ToList();
                foreach (var r in noks)
                {
                    //r.idNoReference.Load();
                    //r.phoneReference.Load();
                    //r.emailReference.Load();
                    //r.imageReference.Load();
                }
                if (Session["noks"] != null)
                {
                    var g = Session["noks"] as List<coreLogic.nextOfKin>;
                    if (g != null)
                    {
                        foreach (var r in g)
                        {
                            if (r.nextOfKinID <= 0)
                            {
                                noks.Add(r);
                            }
                        }
                    }
                }
                Session["noks"] = noks;

                banks = cl.clientBankAccounts.ToList();
                foreach (var r in banks)
                {
                    //r.bankAccountTypeReference.Load();
                }
                if (Session["banks"] != null)
                {
                    var g = Session["banks"] as List<coreLogic.clientBankAccount>;
                    if (g != null)
                    {
                        foreach (var r in g)
                        {
                            if (r.clientBankAccountID <= 0)
                            {
                                banks.Add(r);
                            }
                        }
                    }
                }
                Session["banks"] = banks;

                var officeEmail = cl.clientEmails.FirstOrDefault(p => p.emailTypeID == 1);
                var personalEmail = cl.clientEmails.FirstOrDefault(p => p.emailTypeID == 2);

                emp = cl.employeeCategories.FirstOrDefault();
                grp = cl.groupCategories.FirstOrDefault();
                mic = cl.microBusinessCategories.FirstOrDefault();
                staff = cl.staffCategory1.FirstOrDefault();

                documents = cl.clientDocuments.ToList();
                if (Session["clientDocuments"] != null)
                {
                    var g = Session["clientDocuments"] as List<coreLogic.clientDocument>;
                    if (g != null)
                    {
                        foreach (var r in g)
                        {
                            if (r.clientDocumentID <= 0)
                            {
                                documents.Add(r);
                            }
                        }
                    }
                }
                Session["clientDocuments"] = documents;

                if (phyAddr != null)
                {
                    //phyAddr.addressReference.Load();
                }
                if (mailAddr != null)
                {
                    //mailAddr.addressReference.Load();
                }
                if (officeEmail != null)
                {
                    //officeEmail.emailReference.Load();
                }
                if (personalEmail != null)
                {
                    //personalEmail.emailReference.Load();
                }
                if (workPhone != null)
                {
                    //workPhone.phoneReference.Load();
                }
                if (mobilePhone != null)
                {
                    //mobilePhone.phoneReference.Load();
                }
                if (homePhone != null)
                {
                    //homePhone.phoneReference.Load();
                }
                if (cl.clientCompany != null)
                {
                    //cl.clientCompany.phoneReference.Load();
                    //cl.clientCompany.addressReference.Load();
                    //cl.clientCompany.emailReference.Load();
                }
                if (sme != null)
                {
                    //sme.address1Reference.Load();
                    //sme.addressReference.Load();
                }
                if (emp != null)
                {
                    //emp.employerReference.Load();
                    if (emp.employer != null)
                    {
                        //emp.employer.employerDirectors.Load();

                        //emp.employer.employmentTypeReference.Load();
                        //emp.employer.addressReference.Load();
                    }
                }
                if (grp != null)
                {
                    //grp.groupReference.Load();
                    if (grp.group != null)
                    {
                        //grp.group.addressReference.Load();
                    }
                }
                if (staff != null)
                {
                    //staff.employerReference.Load();

                    if (staff.employer != null)
                    {
                        //staff.employer.employmentTypeReference.Load();
                        //staff.employer.employerDirectors.Load();
                        //staff.employer.employerDepartments.Load();
                    }
                    //staff.staffCategoryDirectors.Load();
                    //staff.employer.addressReference.Load();
                }
                RenderImages();
                Session["loan.cl"] = cl;
            }
            else
            {
                cl = new coreLogic.client();
                Session["loan.cl"] = cl;

                if (Session["noks"] == null)
                {
                    noks = new List<coreLogic.nextOfKin>();
                    Session["noks"] = noks;
                }
                else
                {
                    noks = Session["noks"] as List<coreLogic.nextOfKin>;
                }
                if (Session["smeDirectors"] == null)
                {
                    smeDirectors = new List<coreLogic.smeDirector>();
                    Session["smeDirectors"] = smeDirectors;
                }
                else
                {
                    smeDirectors = Session["smeDirectors"] as List<coreLogic.smeDirector>;
                }
                if (Session["clientDocuments"] == null)
                {
                    documents = new List<coreLogic.clientDocument>();
                    Session["clientDocuments"] = documents;
                }
                else
                {
                    documents = Session["clientDocuments"] as List<coreLogic.clientDocument>;
                }
                if (Session["banks"] == null)
                {
                    banks = new List<coreLogic.clientBankAccount>();
                    Session["banks"] = banks;
                }
                else
                {
                    banks = Session["banks"] as List<coreLogic.clientBankAccount>;
                }
                RenderImages();
            }
        }

        protected void btClearImage_Click(object sender, EventArgs e)
        {
            if (cl != null)
            {
                int id = cl.clientID;
                var imgs = cl.clientImages.ToList();
                for (int i = imgs.Count - 1; i >= 0; i--)
                {
                    var ci = imgs[i];
                    var img = ci.image;
                    le.clientImages.Remove(ci);
                    if (img != null)
                    {
                        le.images.Remove(img);
                    }
                }
                le.SaveChanges();
                LoadClient(id);
            }
        }
    }
}