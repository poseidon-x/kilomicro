<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="staff.aspx.cs" Inherits="coreERP.hc.staff.staff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Staff Master Data Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Staff Master Data Management</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="216" Height="216" runat="server" ID="rotator2" ItemHeight="216" ItemWidth="216" 
                    OnItemDataBound="rotator1_ItemDataBound" FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="imageFrame">
                <telerik:RadAsyncUpload runat="server" ID="upload3" InputSize="20" 
                    Width="200px" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" Localization-Select="Select Pic" MaxFileSize="102400000"
                            ToolTip="Select the picture to upload the staff here"></telerik:RadAsyncUpload> 
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Staff ID
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server"  ID="txtAccNum" CssClass="inputControl"
                    ToolTip="The staff number will be generated and placed here. To override, you can manually type it here"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
               Surname
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtSurname" CssClass="inputControl"
                    ToolTip="Enter the surname of the staff here"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
               Other Names
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtOtherNames" CssClass="inputControl"
                    ToolTip="Enter the other names (firstname) of the staff here"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
               Date of Birth
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" ID="dpDOB" DateInput-DateFormat="dd-MMM-yyyy" MinDate="1/1/1900 12:00:00 AM" CssClass="inputControl"
                    ToolTip="Select the date of birth the staff here"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
               Marital Status
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboMaritalStatus" CssClass="inputControl"
                    ToolTip="Select the marital status the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Sex (Gender)
            </div>
            <div class="subFormInput">
                <asp:RadioButtonList runat="server" ID="rblSex" RepeatDirection="Horizontal" CssClass="inputControl"
                    ToolTip="Select the gender the staff here">
                       <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                       <asp:ListItem Value="M" Text="Male"></asp:ListItem>
                   </asp:RadioButtonList>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
               Staff Category
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboCategory" AutoPostBack="True" 
                       OnSelectedIndexChanged="cboCategory_SelectedIndexChanged" CssClass="inputControl"
                    ToolTip="Select the employee category the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Job Title
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboJobTitle" CssClass="inputControl"
                    ToolTip="Select the job title the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Branch
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboBranch" CssClass="inputControl"
                        ToolTip="Select the branch of the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Username
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtUserName" CssClass="inputControl" MaxLength="30"
                    ToolTip="Enter the system (coreERP) user name of the staff here"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
               Employment Status
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboEmploymentStatus" CssClass="inputControl"
                    ToolTip="Select the current employment status the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Employment Start Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" ID="dtpEmploymentStartDate" DateInput-DateFormat="dd-MMM-yyyy"  CssClass="inputControl"
                    DateInput-ToolTip="Select the start date of employment the staff here"></telerik:RadDatePicker>
            </div>
        </div>
    </div>
    <div class="subForm">
        <telerik:RadButton runat="server" ID="btnSave" Text="Save Staff Data" OnClick="btnSave_Click"
                    ToolTip="Click to save the staff's records into the system"></telerik:RadButton>
    </div> 
    <br />
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Salary Details" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Allowances">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Deductions">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Benefits In Kind">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Pension Schemes">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Tax Reliefs">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Level">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Physical Address">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Mailing Address"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Telephone Numbers"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Supporting Documents"> 
            </telerik:RadTab>  
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server"  ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Basic Salary
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtBasicSalary" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Bank Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboBank" runat="server" CssClass="inputControl" DropDownAutoWidth="Enabled" MaxHeight="400"
                             AutoPostBack="true" OnSelectedIndexChanged="cboBank_SelectedIndexChanged"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Social Security No.
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSSN" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Bank Branch
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboBankBranch" runat="server" 
                            CssClass="inputControl" DropDownAutoWidth="Enabled" MaxHeight="400"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Bank Account No.
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtBankAccountNo" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
            </div>  
        </telerik:RadPageView>     
        <telerik:RadPageView ID="RadPageView3" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Allowance Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboAllowanceType" runat="server" CssClass="inputControl"
                             AutoPostBack="true" OnSelectedIndexChanged="cboAllowanceType_SelectedIndexChanged"
                             DropDownAutoWidth="Enabled" MaxHeight="400"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Percentage
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtAllowancePercent" Value="0" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Enabled
                    </div>
                    <div class="subFormInput">
                        <asp:CheckBox runat="server" ID="chkAllowanceEnabled" Text="" CssClass="inputControl" />
                    </div> 
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Amount Value
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtAllowanceAmount" Value="0" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div> 
                </div>
            </div> 
            <div class="subForm">
                <telerik:RadButton ID="btnAddAllowance" runat="server" 
                    Text="Add Allowance" OnClick="btnAddAllowance_Click"></telerik:RadButton>
            </div>
            <br /> 
            <telerik:RadGrid ID="gridAllowance" runat="server" AutoGenerateColumns="false" OnItemCommand="gridAllowance_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="allowanceType.alllowanceTypeName" HeaderText="Allowance Type"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amount" HeaderText="Amount Value"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="percentValue" HeaderText="Percentage"></telerik:GridNumericColumn>
                        <telerik:GridCheckBoxColumn DataField="isEnabled" HeaderText="Enabled?"></telerik:GridCheckBoxColumn> 
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="Edit" Text="Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Deduction Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboDeductionType" CssClass="inputControl" AutoPostBack="true" OnSelectedIndexChanged="cboDeductionType_SelectedIndexChanged"
                             runat="server" DropDownAutoWidth="Enabled" MaxHeight="400"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Percentage
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtDeductionPercent" Value="0" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Enabled
                    </div>
                    <div class="subFormInput">
                       <asp:CheckBox runat="server" ID="chkDeductionEnabled" Text="" CssClass="inputControl"/>
                    </div> 
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Amount Value
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtDeductionAmount" Value="0" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div> 
                </div>
            </div> 
            <div class="subForm">
                <telerik:RadButton ID="btnAddDeduction" runat="server" 
                    Text="Add Deduction" OnClick="btnAddDeduction_Click"></telerik:RadButton>
            </div>
            <br />   
            <telerik:RadGrid ID="gridDeduction" runat="server" AutoGenerateColumns="false" OnItemCommand="gridDeduction_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="DeductionType.DeductionTypeName" HeaderText="Deduction Type"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amount" HeaderText="Amount Value"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="percentValue" HeaderText="Percentage"></telerik:GridNumericColumn>
                        <telerik:GridCheckBoxColumn DataField="isEnabled" HeaderText="Enabled?"></telerik:GridCheckBoxColumn> 
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="Edit" Text="Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView5" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Benefits In Kind Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboBenefitsInKind" CssClass="inputControl"
                             runat="server" DropDownAutoWidth="Enabled" MaxHeight="300"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Percentage
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtBenefitsInKindPercent" Value="0" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Enabled
                    </div>
                    <div class="subFormInput">
                       <asp:CheckBox runat="server" ID="chkBenefitsInKindIsEnabled" Text=""  CssClass="inputControl" />
                    </div> 
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Amount Value
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtBenefitsInKindAmount" Value="0" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div> 
                </div>
            </div> 
            <div class="subForm">
                <telerik:RadButton ID="btnAddBenefitsInKind" runat="server" 
                    Text="Add BenefitsInKind" OnClick="btnAddBenefitsInKind_Click"></telerik:RadButton>
            </div>
            <br />    
            <telerik:RadGrid ID="gridBenefitsInKind" runat="server" AutoGenerateColumns="false" OnItemCommand="gridBenefitsInKind_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="benefitsInKind.BenefitsInKindName" HeaderText="Benefits In Kind"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amount" HeaderText="Amount Value"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="percentValue" HeaderText="Percentage"></telerik:GridNumericColumn>
                        <telerik:GridCheckBoxColumn DataField="isEnabled" HeaderText="Enabled?"></telerik:GridCheckBoxColumn> 
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="Edit" Text="Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView6" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Pension Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboPensionType" CssClass="inputControl" AutoPostBack="true" OnSelectedIndexChanged="cboPensionType_SelectedIndexChanged"
                             runat="server" DropDownAutoWidth="Enabled" MaxHeight="400"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Employee Value
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl"
                            ID="txtPensionEmployeeAmount" Value="0"></telerik:RadNumericTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Enabled
                    </div>
                    <div class="subFormInput">
                       <asp:CheckBox runat="server" ID="chkPensionIsEnabled" Text="" CssClass="inputControl" />
                    </div> 
                    <div class="subFormLabel">
                        Is Percentage
                    </div>
                    <div class="subFormInput">
                       <asp:CheckBox runat="server" ID="chkPensionIsPercentage" Text="" CssClass="inputControl"/>
                    </div> 
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Employer Value
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtPensionEmployerAmount" Value="0" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div> 
                </div>
            </div> 
            <div class="subForm">
                <telerik:RadButton ID="btnAddPension" runat="server" 
                    Text="Add Pension" OnClick="btnAddPension_Click"></telerik:RadButton>
            </div>
            <br />     
            <telerik:RadGrid ID="gridPension" runat="server" AutoGenerateColumns="false" OnItemCommand="gridPension_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="pensionType.pensionTypeName" HeaderText="Pension Type"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="employerAmount" HeaderText="Employer Value"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="employeeAmount" HeaderText="Employee Value"></telerik:GridNumericColumn> 
                        <telerik:GridCheckBoxColumn DataField="isPercent" HeaderText="Percentage?"></telerik:GridCheckBoxColumn> 
                        <telerik:GridCheckBoxColumn DataField="isEnabled" HeaderText="Enabled?"></telerik:GridCheckBoxColumn> 
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="Edit" Text="Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView7" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Tax Relief Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboTaxRelief" CssClass="inputControl"
                             runat="server" DropDownAutoWidth="Enabled" MaxHeight="400"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Percentage
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="RadNumericTextBox1" Value="0" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Enabled
                    </div>
                    <div class="subFormInput">                       
                        <asp:CheckBox runat="server" ID="chkTaxReliefEnabled" Text="" CssClass="inputControl" />
                    </div> 
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Amount 
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtTaxReliefAmount" Value="0" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div> 
                </div>
            </div> 
            <div class="subForm">
                <telerik:RadButton ID="btnAddTaxRelief" runat="server" 
                    Text="Add Tax Relief" OnClick="btnAddTaxRelief_Click" ></telerik:RadButton>
            </div>
            <br />     
            <telerik:RadGrid ID="gridTaxRelief" runat="server" AutoGenerateColumns="false" OnItemCommand="gridTaxRelief_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="taxReliefType.taxReliefTypeName" HeaderText="Tax Relief Type"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amount" HeaderText="Amount"></telerik:GridNumericColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="Edit" Text="Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
         <telerik:RadPageView runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Line Manager
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboStaffManager"  CssClass="inputControl"
                            DropDownAutoWidth="Enabled" MaxHeight="300"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Employment Start Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker runat="server" CssClass="inputControl"
                            ID="dtpEmploymentStartDate2" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Level
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboLevel" DropDownAutoWidth="Enabled"
                             MaxHeight="300" AutoPostBack="true" CssClass="inputControl"
                             OnSelectedIndexChanged="cboLevel_SelectedIndexChanged"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Notch
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboNotch"  CssClass="inputControl"
                            DropDownAutoWidth="Enabled" MaxHeight="300"></telerik:RadComboBox>
                    </div>
                </div>
            </div> 
        </telerik:RadPageView> 
        <telerik:RadPageView runat="server">  
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="imageFrame">
                        <telerik:RadRotator Width="216" Height="216" runat="server" ID="rotator1" 
                            ItemHeight="216" ItemWidth="216" 
                            OnItemDataBound="rotator1_ItemDataBound" FrameDuration="30000"></telerik:RadRotator>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Map (Location Pic)
                    </div>
                    <div class="subFormInput">
                        <telerik:RadAsyncUpload runat="server" ID="upload1" InputSize="20" 
                            CssClass="inputControl" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" 
                            Localization-Select="Select Pic" MaxFileSize="102400000"></telerik:RadAsyncUpload>                        
                    </div>
                    <div class="subFormLabel">
                        Address_Line_1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyAddr1" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyAddr2" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyCityTown" runat="server" CssClass="inputControl"></telerik:RadTextBox>
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
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddr1" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddr2" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddrCity" runat="server" CssClass="inputControl"></telerik:RadTextBox>
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
                       <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMobilePhone" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div> 
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Work Telephone
                    </div>
                    <div class="subFormInput">
                         <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtWorkPhone" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Home Telephone
                    </div>
                    <div class="subFormInput">
                         <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtHomePhone" runat="server" CssClass="inputControl"></telerik:RadTextBox>
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
                <div class="subFormColumnRight">
                    <div class="imageFrame">  
                         <telerik:RadButton ID="btnAddDcoument" runat="server" 
                             Text="Add Document" OnClick="btnAddDcoument_Click"></telerik:RadButton>
                    </div>
                </div>
            </div>  
            <telerik:RadGrid ID="gridDocument" runat="server" AutoGenerateColumns="false" OnItemCommand="gridDocument_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="document.description" HeaderText="Surname"></telerik:GridBoundColumn>
                        <telerik:GridHyperLinkColumn DataTextField="document.filename"  HeaderText="Download Document"
                            DataNavigateUrlFields="document.documentID" DataNavigateUrlFormatString="/ln/loans/document.aspx?id={0}"
                            Target="_blank"></telerik:GridHyperLinkColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Guarantor" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
    </telerik:RadMultiPage> 
</asp:Content>
