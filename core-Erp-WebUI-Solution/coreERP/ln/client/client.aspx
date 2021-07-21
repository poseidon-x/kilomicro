<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="client.aspx.cs" Inherits="coreERP.ln.client.client" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Clients Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 344px;
        }
        .btClearImage{
            vertical-align:top;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Client Management</h3>
        <div class="subForm">
            <div class="subFormColumnLeft">
                <div class="imageFrame">
                    <telerik:RadBinaryImage runat="server" ID="RadBinaryImage1"
                                AutoAdjustImageControlSize="false" Width="216" Height="216" ToolTip='<%#Eval("ContactName", "Photo of {0}") %>'  />
                   <asp:CheckBox runat="server" ID="chkIsCompany" Visible="false" Checked="false" Text="This is an Institutional Client"
                        AutoPostBack="true" OnCheckedChanged="chkIsCompany_CheckedChanged" />
                </div> 
                <div class="imageFrame">
                    <div class="subFormLabel">
                        <telerik:RadAsyncUpload runat="server" ID="upload3" InputSize="6" Width="150px" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" Localization-Select="Upload Pic" 
                                MaxFileSize="102400000" Height="19px"
                                ToolTip="Select the picture file of the client or client contact person"></telerik:RadAsyncUpload>
                    </div>
                    <div class="subFormInput"> 
                        <button id="btnCam" onclick="LoadCam();"  value="Use Cam/Webcam" title="Use Camera/Webcam" type="button" >Use (Web)Cam</button> 
                        <asp:Button runat="server" ID="btClearImage" Text="Clear Pic" OnClick="btClearImage_Click" OnClientClick="OnClearImage()"
                            CssClass="btnClearImage" />
                    </div>
                </div>
                <div class="subFormLabel">
                    Client Type
                </div>
                <div class="subFormInput"> 
                    <telerik:RadComboBox runat="server" ID="cboClientType" CssClass="inputControl" CausesValidation="false" 
                        AutoPostBack="true" OnSelectedIndexChanged="cboClientType_SelectedIndexChanged"
                                ToolTip="Select the type of client">
                   </telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cboClientType" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply a client type"></asp:RequiredFieldValidator>
                </div>
                <div class="subFormLabel">
                    Client Category
                </div>
                <div class="subFormInput"> 
                    <telerik:RadComboBox runat="server" ID="cboCategory" AutoPostBack="True" CssClass="inputControl"
                        CausesValidation="false" OnSelectedIndexChanged="cboCategory_SelectedIndexChanged"
                                ToolTip="select the category of the client or client contact person"></telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="cboCategory" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply a category"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="subFormColumnMiddle">
                <div class="subFormLabel">
                   Account Number
                </div>
                <div class="subFormInput">                   
                   <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" Enabled="false" ID="txtAccNum" CssClass="form-control"
                                ToolTip="The account number of the client will be generated and placed here. If you want to override it manually enter the account number here"></telerik:RadTextBox>
                </div>
                <asp:Panel runat="server" ID="pnlJoint2" Visible="false">
                    <div class="subFormLabel">
                       Joint Account Name
                    </div>
                    <div class="subFormInput">                   
                       <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtJointAccountName" CssClass="form-control"
                                    ToolTip="Enter the account name of the joint account"></telerik:RadTextBox> 
                    </div>
                </asp:Panel> 
                <div class="subFormLabel" runat="server" id="divSurnameLabel">
                   Surname
                </div>
                <div class="subFormInput">                   
                   <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtSurname" CssClass="form-control"
                                ToolTip="Enter the surname of the client or client contact person"></telerik:RadTextBox>
                   <asp:RequiredFieldValidator ID="rfvSurname" runat="server" ControlToValidate="txtSurname" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply a surname"></asp:RequiredFieldValidator>
                </div>
                <div class="subFormLabel" runat="server" id="divOtherNamesLabel">
                    Other Names
                </div>
                <div class="subFormInput"> 
                    <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtOtherNames" CssClass="form-control"
                                ToolTip="Enter the other names of the client or client contact person"></telerik:RadTextBox>
                </div> 
                <asp:Panel runat="server" ID="pnlAdmission" Visible="false">
                    <div class="subFormLabel">
                       Admission Fee
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAdmissionFee" runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
                    </div>
                </asp:Panel>
               
                <asp:Panel runat="server" ID="pnlJoint" Visible="false">
                    <div class="subFormLabel">
                       2nd Surname
                    </div>
                    <div class="subFormInput">                   
                       <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtSecondSurname" CssClass="form-control"
                                    ToolTip="Enter the surname of the 2nd joint client"></telerik:RadTextBox> 
                    </div>
                    <div class="subFormLabel">
                        2nd Other Names
                    </div>
                    <div class="subFormInput"> 
                        <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtSecondOtherNames" CssClass="form-control"
                                ToolTip="Enter the other names of the 2nd joint client"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                       3rd Surname
                    </div>
                    <div class="subFormInput">                   
                       <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtThirdSurname" CssClass="form-control"
                                    ToolTip="Enter the surname of the 2nd joint client"></telerik:RadTextBox> 
                    </div>
                    <div class="subFormLabel">
                        3rd Other Names
                    </div>
                    <div class="subFormInput"> 
                        <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtThirdOtherNames" CssClass="form-control"
                                ToolTip="Enter the other names of the 2nd joint client"></telerik:RadTextBox>
                    </div> 
                    
                </asp:Panel>
                <div class="subFormLabel">
                    Date of Birth
                </div>
                <div class="subFormInput">  
                   <telerik:RadDatePicker runat="server" ID="dpDOB" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" MinDate="1/1/1900 12:00:00 AM"
                                DateInput-ToolTip="Select the date of birth of the client or client contact person"></telerik:RadDatePicker>
                   <asp:RequiredFieldValidator ID="rfvDOB" runat="server" ControlToValidate="dpDOB" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply a date of birth"></asp:RequiredFieldValidator>
                </div>
                <div class="subFormLabel">
                    Primary ID No:
                </div>
                <div class="subFormInput">
                   <telerik:RadComboBox runat="server" ID="cboIDType" CssClass="inputControlHalf"
                                ToolTip="Select the type of Identification card presented by the client or client contact person"></telerik:RadComboBox>
                   <asp:RequiredFieldValidator ID="rfvIDType" runat="server" ControlToValidate="cboIDType" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply an ID Type"></asp:RequiredFieldValidator> 
                   <telerik:RadTextBox WrapperCssClass="inputControlHalf" runat="server" ID="txtIDNo" CssClass="inputControlHalf"
                                ToolTip="Enter the number of the Identification card presented by the client or client contact person"></telerik:RadTextBox>
                   <asp:RequiredFieldValidator ID="rfvIDNo" runat="server" ControlToValidate="txtIDNo" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply an ID No"></asp:RequiredFieldValidator>
                </div>
                <div class="subFormLabel">
                    ID Expiry Date
                </div>
                <div class="subFormInput">
                    <telerik:RadDatePicker runat="server" ID="dpExpiryDate" DateInput-DateFormat="dd-MMM-yyyy" CssClass="inputControl"
                                ToolTip="Select the expiry date (if applicable) of Identification card presented by the client or client contact person"></telerik:RadDatePicker>
                </div>
            </div>
            <div class="subFormColumnRight">
                <div class="subFormLabel">
                    Sex (Gender)
                </div>
                <div class="subFormInput">
                   <asp:RadioButtonList runat="server" ID="rblSex" RepeatDirection="Horizontal" CssClass="inputControl"
                                ToolTip="select the gender of the client or client contact person">
                       <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                       <asp:ListItem Value="M" Text="Male"></asp:ListItem>
                   </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfvSex" runat="server" ControlToValidate="rblSex" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply a sex"></asp:RequiredFieldValidator>
                </div>
                <div class="subFormLabel">
                    Marital Status
                </div>
                <div class="subFormInput"> 
                   <telerik:RadComboBox runat="server" ID="cboMaritalStatus" CssClass="inputControl"
                                ToolTip="Select the marital status of the client or client contact person"></telerik:RadComboBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboMaritalStatus" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply a marital status"></asp:RequiredFieldValidator>
                </div>
                <div class="subFormLabel">
                    Industry
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox runat="server" ID="cboIndustry" CssClass="inputControl"
                                ToolTip="Select othe industry of the client"></telerik:RadComboBox>
                </div>
                <div class="subFormLabel">
                    Branch
                </div>
                <div class="subFormInput"> 
                    <telerik:RadComboBox runat="server" ID="cboBranch" Enabled="true" DropDownAutoWidth="Enabled" CssClass="inputControl"
                                ToolTip="Select the branch of this client"></telerik:RadComboBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cboBranch" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply a branch"></asp:RequiredFieldValidator>
                </div>
                <div class="subFormLabel">
                    Group
                </div>
                <div class="subFormInput"> 
                    <telerik:RadComboBox runat="server" ID="cboLoanGroup" Enabled="true" DropDownAutoWidth="Enabled" CssClass="inputControl"
                                ToolTip="Add client to a group"></telerik:RadComboBox>
                   <asp:RequiredFieldValidator ID="rfvLoanGroup" runat="server" ControlToValidate="cboLoanGroup" ErrorMessage="Required !" ForeColor="Red"
                       ToolTip="Please supply a Loan Group"></asp:RequiredFieldValidator>
                </div>
                <div class="subFormLabel">
                    Sector
                </div>
                <div class="subFormInput"> 
                    <telerik:RadComboBox runat="server" ID="cboSector" DropDownAutoWidth="Enabled" CssClass="inputControl"
                                ToolTip="Select the sector of the client"></telerik:RadComboBox>
                   <asp:RequiredFieldValidator ID="rfvSector" runat="server" ControlToValidate="cboSector" ErrorMessage="!" ForeColor="Red"
                       ToolTip="Please supply a sector"></asp:RequiredFieldValidator>
                </div>
                <div class="subFormLabel">
                    Secondary ID No:
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox runat="server" ID="cboIDType2" CssClass="inputControlHalf"></telerik:RadComboBox> 
                   &nbsp;&nbsp;&nbsp;&nbsp;
                   <telerik:RadTextBox WrapperCssClass="inputControlHalf" runat="server" ID="txtIDNo2" Width="20px" CssClass="form-control"></telerik:RadTextBox> 
                </div>
                <div class="subFormLabel">
                    ID Expiry Date
                </div>
                <div class="subFormInput">
                    <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dpExpiryDate2" DateInput-DateFormat="dd-MMM-yyyy" ></telerik:RadDatePicker>                    
                </div>
                <div class="subFormLabel" runat ="server" id="divReg1" visible="false">
                    Region
                </div>
                <div class="subFormInput" runat ="server" id="divReg2" visible="false">
                    <telerik:RadComboBox runat="server" CssClass="inputControl" ID="cboRegion" visible="false"
                                ToolTip="Select the region of the client"></telerik:RadComboBox>
                </div>
            </div>
        </div>    
        <div class="subForm">
            <div class="subFormColumnLeft">
                    <telerik:RadButton runat="server" ID="btnSave" Text="Save Client Data" OnClick="btnSave_Click" CausesValidation="true"
                                ToolTip="Click to save this client's record and exit this screen"></telerik:RadButton>
                   <telerik:RadButton runat="server" ID="btnSave2" Text="Save To Continue Later" OnClick="btnSave2_Click" CausesValidation="true"
                                ToolTip="Click to save this client's record and continue to stay on this screen"></telerik:RadButton>
            </div>
        </div>
    <br />
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="100%" CausesValidation="false">
        <Tabs>
            <telerik:RadTab runat="server" Text="Physical Address" Selected="True"
                                ToolTip="Click to input the physical address of the client">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Mailing Address"
                                ToolTip="Click to input the mailing address of the client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Telephone Numbers"
                                ToolTip="Click to input the telephone numbers of the client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Email Addresses"
                                ToolTip="Click to input the email addresses of the client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="SME Category" Visible="false"
                                ToolTip="Click to enter details of this SME client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Employee Category" Visible="false"
                                ToolTip="Click to enter details of this Employee client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Group Category" Visible="false"
                                ToolTip="Click to enter details of this Group client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Micro-Business Category" Visible="false"
                                ToolTip="Click to enter details of this Micro-Business client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="SME Directors" Visible="false"
                                ToolTip="Click to enter directors of this SME client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Next of Kin" Visible="false"
                                ToolTip="Click to enter next of kins of this client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Payroll Category" Visible="false"
                                ToolTip="Click to enter details of this payroll client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Bank Accounts" Visible="false"
                                ToolTip="Click to enter bank accounts of this client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Corporate Client" Visible="false"
                                ToolTip="Click to enter details of this corporate client"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Supporting Documents"
                                ToolTip="Click to upload supporting documents of this client"> 
            </telerik:RadTab>  
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server"  ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView runat="server"> 
        <div class="subForm">
            <div class="subFormColumnLeft">
                <div class="subFormLabel">
                    Physical Address
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyAddr1" runat="server" CssClass="form-control"
                                ToolTip="Enter the physical address here"></telerik:RadTextBox>
                </div>
                <div class="subFormLabel">
                    Landmark
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyAddr2" runat="server" CssClass="form-control"
                                ToolTip="Enter the nearest landmark of the client address here"></telerik:RadTextBox>
                </div>
                <div class="subFormLabel">
                    Town/City
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyCityTown" runat="server" CssClass="form-control"
                                ToolTip="Enter the physical location of the client"></telerik:RadTextBox>
                </div>
                <div class="subFormLabel">
                    Location (Map/Pic)
                </div>
                <div class="subFormInput">
                    <telerik:RadAsyncUpload runat="server" ID="upload1" InputSize="20" CssClass="inputControl" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" Localization-Select="Select Pic" MaxFileSize="102400000"
                                ToolTip="Select the pictures of the location of the client"></telerik:RadAsyncUpload>
                </div>
            </div>
            <div class="subFormColumnLeft">
                <div class="imageFrame">
                    <telerik:RadRotator Width="200" Height="200" runat="server" ID="rotator1" ItemHeight="200" ItemWidth="200" 
                        OnItemDataBound="rotator1_ItemDataBound" FrameDuration="30000"></telerik:RadRotator>
                </div>
            </div>
        </div> 
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView1" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Address_Line_1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddr1" runat="server" CssClass="form-control"
                                    ToolTip="Enter the mailing address here"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddr2" runat="server" CssClass="form-control"
                                    ToolTip="Enter the mailing address here"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddrCity" runat="server" CssClass="form-control"
                                    ToolTip="Enter the city of the address here"></telerik:RadTextBox>
                    </div> 
                </div> 
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Mobile Telephone
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMobilePhone" runat="server" CssClass="form-control"
                                ToolTip="Enter the client's mobile phone number here"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="rfvMobileNo" runat="server" ControlToValidate="txtMobilePhone" ErrorMessage="Required !" ForeColor="Red"
                       ToolTip="Please provide client's mobile number"></asp:RequiredFieldValidator>
                    </div>
                    <div class="subFormLabel">
                       Work Telephone
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtWorkPhone" runat="server" CssClass="form-control"
                                ToolTip="Enter the client's work/office phone number here"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Home Telephone
                    </div>
                    <div class="subFormInput">
                       <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtHomePhone" runat="server" CssClass="form-control"
                                ToolTip="Enter the client's home phone number here"></telerik:RadTextBox>
                    </div> 
                </div> 
            </div> 
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView9" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Office Email
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOfficeEmail" runat="server" CssClass="form-control"
                                ToolTip="Enter the client's office/work email address here"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                       Personal Email
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPersonalEmail" runat="server" CssClass="form-control"
                                ToolTip="Enter the client's personal email address here"></telerik:RadTextBox>
                    </div> 
                </div> 
            </div>  
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView3" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Company Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMECompName" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                       Incorporation Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker ID="dtSMEIncDate" runat="server"  CssClass="inputControl"
                            DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div> 
                    <div class="subFormLabel">
                       Phy. Addr Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMEPhyCity" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                       Reg. Addr Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMERegAddrCity" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Reg. No
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMERegNo" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                       Phy. Address Line 1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMEPhyAddr1" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                       Reg. Address Line 1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMERegAddr1" runat="server" CsssClass="form-control"
                            ></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Reg. Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker ID="dtSMERegDate" runat="server" CssClass="inputControl"
                            DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>
                    <div class="subFormLabel">
                       Phy. Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMEPhyAddr2" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                       Reg. Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMERegAddr2" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
            </div>   
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Select Employer
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboEmployer" CssClass="inputControl" 
                           AutoPostBack="true" OnSelectedIndexChanged="cboEmployer_SelectedIndexChanged"
                           Height="300px"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                       Emp. Address Line 1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" Enabled="false" ID="txtEmpAddr1" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                       Phy. Addr Town/City
                    </div> 
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Authorising Officer
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" CssClass="inputControl" ID="cboDirector"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                       Emp. Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" Enabled="false" ID="txtEmpAddr2" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                       Reg. Address Line 1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="RadTextBox6" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Employment Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboEmploymentType" runat="server" CssClass="inputControl"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                       Emp. Addr Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" Enabled="false" ID="txtEmpAddrCity" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
            </div>   
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView5" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Select Group
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboGroup" AutoPostBack="true"  CssClass="inputControl"
                           OnSelectedIndexChanged="cboGroup_SelectedIndexChanged"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                       Group Size
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" Enabled="false" ID="txtGroupSize" CssClass="form-control"
                            runat="server" Type="Number" ></telerik:RadNumericTextBox>
                    </div> 
                    <div class="subFormLabel">
                       Group Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtGroupAddr2" runat="server" Enabled="false" CssClass="form-control"></telerik:RadTextBox>
                    </div>  
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Group Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" Enabled="false" ID="txtGroupName" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                       Join Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker ID="dtGroupJoinDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div> 
                    <div class="subFormLabel">
                       Group Addr Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtGroupAddrCity" Enabled="false" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Membership No.
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMembershipNo" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                       Group Address Line 1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtGroupAddr1" runat="server" Enabled="false" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
            </div>   
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView6" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Line of Business
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboLOB" runat="server" CssClass="inputControl"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                       Date Established
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker ID="dtDateEstablished" runat="server" CssClass="inputControl"
                            DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>  
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Business Owner
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMicroBOwner" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        No. of Competitors
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtMicroBComp" runat="server" CssClass="form-control" Type="Number" ></telerik:RadNumericTextBox>
                    </div> 
                </div> 
            </div>    
        </telerik:RadPageView>        
        <telerik:RadPageView ID="RadPageView7" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Surname
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMESurname" CssClass="form-control" runat="server"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                       Email Address
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMEEmailAddress" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>  
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Other Names
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMEOtherNames" CssClass="form-control" runat="server"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                        Phone Number
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMEPhoneNo" CssClass="form-control" runat="server"></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        ID Number
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" CssClass="inputControl" ID="cboSMEIDType"></telerik:RadComboBox>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSMEIDNo" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                        Picture
                    </div>
                    <div class="subFormInput">
                        <telerik:RadAsyncUpload runat="server" ID="upload2" InputSize="20"  CssClass="inputControl"
                                                        AllowedFileExtensions="png,jpg,jpeg,gif,tiff" Localization-Select="Select Pic" MaxFileSize="100024000" UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </div> 
                </div> 
            </div> 
            <div class="subForm"   >
                <div class="subFormColumnLeft">
                    <telerik:RadButton ID="btnAddSMEDirector" runat="server" 
                        Text="Add Director Details" OnClick="btnAddSMEDirector_Click"></telerik:RadButton>
                </div>
            </div> 
            <telerik:RadGrid ID="gridSMEDirector" runat="server" AutoGenerateColumns="false" OnItemCommand="gridSMEDirector_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="idNo.idNo1" HeaderText="ID No"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="phone.phoneNo" HeaderText="Phone No."></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="email.emailAddress" HeaderText="Email Address"></telerik:GridBoundColumn>
                        <telerik:GridBinaryImageColumn HeaderText="Picture" ResizeMode="Fit" DataField="image.image1" ImageWidth="48px" ImageHeight="48px"></telerik:GridBinaryImageColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Director" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Director" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView10" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Surname
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKSurname" CssClass="form-control" runat="server"></telerik:RadTextBox>
                        <%--<asp:RequiredFieldValidator ID="rfvNokSurname" runat="server" ControlToValidate="txtNOKSurname" ErrorMessage="Required !" ForeColor="Red"
                            ToolTip="Please provide Next of Kin Surname"></asp:RequiredFieldValidator>--%>
                    </div>
                    <div class="subFormLabel">
                       Email Address
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKEmailAddress" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>  
                    <div class="subFormLabel">
                       Picture
                    </div>
                    <div class="subFormInput">
                        <telerik:RadAsyncUpload runat="server" ID="upload5" InputSize="20"  
                            CssClass="inputControl" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" 
                            Localization-Select="Select Pic" MaxFileSize="100024000" 
                            UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </div>  
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Other Names
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKOtherNames" CssClass="form-control" runat="server"></telerik:RadTextBox>
                        <%--<asp:RequiredFieldValidator ID="rfvNokOname" runat="server" ControlToValidate="txtNOKOtherNames" ErrorMessage="Required !" ForeColor="Red"
                            ToolTip="Please provide Next of Kin other name"></asp:RequiredFieldValidator>--%>
                    </div> 
                    <div class="subFormLabel">
                        Phone Number
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKPhoneNumber" CssClass="form-control" runat="server"></telerik:RadTextBox>
                        <%--<asp:RequiredFieldValidator ID="rfvNoKPhoneNo" runat="server" ControlToValidate="txtNOKPhoneNumber" ErrorMessage="Required !" ForeColor="Red"
                            ToolTip="Please provide client's Next of Kin mobile number"></asp:RequiredFieldValidator>--%>
                    </div> 
                </div> 
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        ID Number
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" CssClass="inputControl" ID="cboNOKIDType"></telerik:RadComboBox>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKIDNumber" CssClass="form-control" runat="server"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                         Relation to Client
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtRelation" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
            </div> 
            <div class="subForm"   >
                <div class="subFormColumnLeft">
                    <telerik:RadButton ID="btnAddNOK" runat="server" 
                        Text="Add Next of Kin Details" OnClick="btnAddNOK_Click"></telerik:RadButton>
                </div>
            </div>  
            <telerik:RadGrid ID="gridNOK" runat="server" AutoGenerateColumns="false" OnItemCommand="gridNOK_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="idNo.idNo1" HeaderText="ID No"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="phone.phoneNo" HeaderText="Phone No."></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="email.emailAddress" HeaderText="Email Address"></telerik:GridBoundColumn>
                        <telerik:GridBinaryImageColumn HeaderText="Picture" ResizeMode="Fit" DataField="image.image1" ImageWidth="48px" ImageHeight="48px"></telerik:GridBinaryImageColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit NOK" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete NOK" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView11" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Select Employer
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboStaffEmp" DropDownWidth="300px" CssClass="inputControl" 
                            AutoPostBack="true" OnSelectedIndexChanged="cboStaffEmp_SelectedIndexChanged"
                           Height="350px"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                       Authorising Officer Telephone
                    </div>
                    <div class="subFormInput">
                        <asp:TextBox runat="server" CssClass="inputControl" ID="txtAuthOfficerPhone" ></asp:TextBox>
                    </div>  
                    <div class="subFormLabel">
                       Employee Number
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtEmployeeNo" runat="server" CssClass="form-control"></telerik:RadTextBox>                    
                    </div>    
                    <div class="subFormLabel">
                       Employment Start Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker ID="dtpEmpStartDate" runat="server" CssClass="inputControl" AutoPostBack="true"  MinDate="1/1/1900 12:00:00 AM"
                            OnSelectedDateChanged="dtpEmpStartDate_SelectedDateChanged" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>  
                    <div class="subFormLabel">
                       Employer Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtStaffAddr2" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>  
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Authorising Officer (Name)
                    </div>
                    <div class="subFormInput">
                        <asp:TextBox runat="server" ID="txtAuthOfficer" CssClass="form-control"></asp:TextBox>
                    </div> 
                    <div class="subFormLabel">
                        Employing Department
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboStaffDep" Height="350px" CssClass="inputControl"></telerik:RadComboBox>
                    </div>  
                    <div class="subFormLabel">
                        Social Security No.
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSSN" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                        Length of Service (mths)
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtLOS" runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
                    </div> 
                    <div class="subFormLabel">
                        Employer Addr Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtStaffCityTown" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Authorising Officer (Position)
                    </div>
                    <div class="subFormInput">
                        <asp:TextBox runat="server" ID="txtAuthOfficerPosition" CssClass="form-control"></asp:TextBox>
                    </div> 
                    <div class="subFormLabel">
                         Contract Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboStaffContract" CssClass="inputControl" 
                            DropDownWidth="300px" Height="350px"></telerik:RadComboBox>
                    </div> 
                    <div class="subFormLabel">
                         Old Emp. Number
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtEmployeeNoOld" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                         Employer Address Line 1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtStaffAdd1" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                         Position
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPosition" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
            </div>  
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView12" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Bank Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboIT" CssClass="inputControl" AutoPostBack="true"
                             OnSelectedIndexChanged="cboIT_SelectedIndexChanged" Width="">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="" />
                                <telerik:RadComboBoxItem Value="Bank" Text="Bank" />
                                <telerik:RadComboBoxItem Value="Rural Bank" Text="Rural Bank" />
                                <telerik:RadComboBoxItem Value="Savings &amp; Loans" Text="Savings &amp; Loans" />
                            </Items>
                        </telerik:RadComboBox>
                        <telerik:RadComboBox runat="server" MarkFirstMatch="true" AutoCompleteSeparator=""   
                            DropDownAutoWidth="Enabled" AppendDataBoundItems="true" 
                            ID="cboBank" AutoPostBack="true" OnSelectedIndexChanged="cboBank_SelectedIndexChanged"
                            EmptyMessage="Select a Bank" HighlightTemplatedItems="true" Height="350px"> 
                        </telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                       Account No.
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>  
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                       Branch
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboBankBranch" Height="350px" CssClass="inputControl"></telerik:RadComboBox>
                    </div> 
                    <div class="subFormLabel">
                        Account Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountName" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Account Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboAccountType" CssClass="inputControl" AutoPostBack="true"></telerik:RadComboBox>
                    </div> 
                    <div class="subFormLabel">
                        Primary?
                    </div>
                    <div class="subFormInput">
                        <asp:CheckBox ID="chkPrimary" CssClass="inputControl" runat="server"/>
                    </div> 
                </div> 
            </div> 
            <div class="subForm"   >
                <div class="subFormColumnLeft">
                    <telerik:RadButton ID="btnAddBank" runat="server" Text="Add Bank Account" 
                            OnClick="btnAddBank_Click"></telerik:RadButton>
                </div>
            </div> 
            <telerik:RadGrid ID="gridBank" runat="server" AutoGenerateColumns="false" OnItemCommand="gridBank_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="bankAccountType.accountTypeName" HeaderText="Account Type"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="accountName" HeaderText="Account Name"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="accountNumber" HeaderText="Account No."></telerik:GridBoundColumn> 
                        <telerik:GridTemplateColumn HeaderText="Bank" DataField="branchID" >
                            <ItemTemplate>
                                <%# GetBank((int)Eval("branchID")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Branch">
                            <ItemTemplate>
                                <%# GetBranch((int)Eval("branchID")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridCheckBoxColumn DataField="isPrimary" HeaderText="Primary"></telerik:GridCheckBoxColumn> 
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="accountNumber" HeaderText="View/Edit Acct" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="accountNumber" HeaderText="Delete Acct" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Company Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCompanyName" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                       Phone Number
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCompanyPhone" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>  
                    <div class="subFormLabel">
                       Contact Other Names
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtContactOtherNames" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div>  
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                       Business Address
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtBusinessAddress" runat="server" CssClasss="form-control"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                        Email Address
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCompanyEmail" runat="server" CssClass="form-control"></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCompanyCity" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div> 
                    <div class="subFormLabel">
                        Contact Surname
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtContactSurname" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div> 
                </div> 
            </div>  
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView8" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Document Description
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtDocDesc" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div> 
                </div> 
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                       Documents
                    </div>
                    <div class="subFormInput">
                        <telerik:RadAsyncUpload runat="server" ID="upload4" InputSize="20"  
                            CssClass="inputControl" AllowedFileExtensions="pdf,txt,docx,xlsx,doc,xls,html,jpg,jpeg,png,gif" 
                            Localization-Select="Select Pic" MaxFileSize="100024000" 
                            UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </div>  
                </div>  
            </div> 
            <div class="subForm"   >
                <div class="subFormColumnLeft">
                    <telerik:RadButton ID="btnAddDcoument" runat="server" 
                        Text="Add Document" OnClick="btnAddDcoument_Click"></telerik:RadButton>
                </div>
            </div>  
            <telerik:RadGrid ID="gridDocument" runat="server" AutoGenerateColumns="false" OnItemCommand="gridDocument_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="document.description" HeaderText="Document Description"></telerik:GridBoundColumn>
                        <telerik:GridHyperLinkColumn DataTextField="document.filename"  HeaderText="Download Document"
                            DataNavigateUrlFields="document.documentID" DataNavigateUrlFormatString="/ln/loans/document.aspx?id={0}"
                            Target="_blank"></telerik:GridHyperLinkColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="document.description" HeaderText="Delete Document" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
    </telerik:RadMultiPage> 
    <script type="text/javascript">
        function LoadCam() {
            var w = 920;
            var h = 650;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            var win = window.open("/capture/", "_blank", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width='+w+', height='+h+', top='+top+', left='+left);
            win.parentWindow = this.window;
        }
        function CamLoaded() {   
            document.forms[0].submit();
        };

        function AddRotatorItem() { 
            var radRotatorItemData = { Html: "<div><img src='/capture/default.aspx?d=1' style='max-height: 216px; max-width: 216px;' /></div>" };
                GetRadRotator().addRotatorItem(radRotatorItemData, 1); 
        }
        
        

        function OnClearImage() {
            return confirm("Do you really want to clear picture of this client?");
        }
    </script>
</asp:Content>
