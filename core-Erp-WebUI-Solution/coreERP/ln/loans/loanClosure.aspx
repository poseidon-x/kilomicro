<%@ Page Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="loanClosure.aspx.cs" Inherits="coreERP.ln.loans.loanClosure" EnableEventValidation="false"  %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Close Loan Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Close Loan Account</h3>
        <div class="subForm">
            <div class="subFormColumnLeft">
                <div class="imageFrame">
                    <telerik:RadRotator Width="216" Height="216" runat="server" ID="rotator1" ItemHeight="216" 
                        ItemWidth="216" FrameDuration="100000"></telerik:RadRotator>
                </div> 
                <div class="subFormLabel">
                    Account Number
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="inputControl"
                        ToolTip="Enter the account number of the client to search by it"></telerik:RadTextBox>
                </div> 
                <asp:Panel ID="pnlRegular" runat="server" Visible="true">
                    <div class="subFormLabel">
                        Surname
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Other Names
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlJoint" runat="server" Visible="false"> 
                    <div class="subFormLabel">
                        Account Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtJointAccountName" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </asp:Panel>   
            </div>
            <div class="subFormColumnMiddle">       
                <div class="subFormLabel">
                     Selected Client
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true"   CssClass="inputControl"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client"  
                    MarkFirstMatch="true" AutoCompleteSeparator="" CausesValidation="false" Enabled="False"
                         EnableLoadOnDemand="true" 
                        ToolTip="Select the  client whose application is being entered"></telerik:RadComboBox>
                </div>  
                <div class="subFormLabel">
                    Amount Requested
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmountRequested" runat="server" CssClass="inputControl"
                        ToolTip="Enter the amount requested by the client"></telerik:RadNumericTextBox>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtAmountRequested" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the application amount for the loan"></asp:RequiredFieldValidator>
                </div>         
                <div class="subFormLabel">
                    Loan Date
                </div>
                <div class="subFormInput">
                    <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"
                        DateInput-ToolTip="Select the date of the loan application"></telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dtAppDate" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the application date for the loan"></asp:RequiredFieldValidator>
                </div>         
                <div class="subFormLabel">
                    Loan Type
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox id="cboLoanType" runat="server" AutoPostBack="true" 
                         CausesValidation="false"
                         OnSelectedIndexChanged="cboLoanType_SelectedIndexChanged" CssClass="inputControl"
                        ToolTip="Select the loan type/category or product"></telerik:RadComboBox>
                    <asp:Panel runat="server" ID="pnlLT">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="cboLoanType" ErrorMessage="!"
                             ForeColor="Red" Font-Bold="true" ToolTip="Please select the type of the loan"></asp:RequiredFieldValidator>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlLP" Visible="false">
                        <telerik:RadComboBox id="cboLoanProduct" runat="server" Width="300px" 
                            visible="false" AutoPostBack="true" OnSelectedIndexChanged="cboLoanProduct_SelectedIndexChanged"
                            ToolTip="Select the loan type/category or product"></telerik:RadComboBox>
                    </asp:Panel>
                </div>            
                <div class="subFormLabel">
                    Scheme Loan
                </div>      
                <div class="subFormInput">
                    <telerik:RadComboBox id="cboLoanScheme" runat="server" AutoPostBack="true" CausesValidation="false"
                         OnSelectedIndexChanged="cboLoanScheme_SelectedIndexChanged" CssClass="inputControl"
                        ToolTip="Select the type of scheme loan"></telerik:RadComboBox>                    
                </div>   
                <div class="subFormLabel">
                   Repayment Mode
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox id="cboRepaymentMode" runat="server" CssClass="inputControl"
                        ToolTip="Select the loan repayment mode"></telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="cboRepaymentMode" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please select the repayment mode of the loan"></asp:RequiredFieldValidator>
                </div>
                <asp:Panel runat="server" ID="pnlCat5" Visible="true">
                    <div class="subFormLabel">
                        Tenure of Loan
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTenure" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2"  
                            OnTextChanged="txtTenure_TextChanged" AutoPostBack="true"
                            ToolTip="Enter the loan tenure in months"></telerik:RadNumericTextBox>
                            &nbsp;Months
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTenure" ErrorMessage="!"
                                ForeColor="Red" Font-Bold="true" ToolTip="Please enter the tenure of the loan"></asp:RequiredFieldValidator>                
                    </div>
                </asp:Panel> 
                <div class="subFormLabel">
                    Interest Rate
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtRate" runat="server" CssClass="inputControl"
                        ToolTip="Enter the loan interest rate in per month"></telerik:RadNumericTextBox>
                        &nbsp;per Month
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtRate" ErrorMessage="!"
                            ForeColor="Red" Font-Bold="true" ToolTip="Please enter the interest rate of the loan"></asp:RequiredFieldValidator>
                </div>  
            </div>
            <div class="subFormColumnRight">  
                <div class="subFormLabel">
                    Interest Calculation
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox id="cboInterestType" runat="server" CssClass="inputControl"
                        ToolTip="Select the loan interest calculation mode"></telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="cboInterestType" ErrorMessage="!"
                            ForeColor="Red" Font-Bold="true" ToolTip="Please select the interest calculation method of the loan"></asp:RequiredFieldValidator>
                </div>           
                <div class="subFormLabel">
                    Relationship Officer
                </div>
               <div class="subFormInput">
                    <telerik:RadComboBox ID="cboStaff" runat="server" CssClass="inputControl"
                        DropDownWidth="355px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true"
                        MarkFirstMatch="true" AutoCompleteSeparator=""
                        ToolTip="Select the staff assigned to the loan"></telerik:RadComboBox>
                </div>
                <div class="subFormLabel">
                    Field Agent
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox ID="cboAgent" runat="server" CssClass="inputControl"
                        DropDownWidth="355px" EmptyMessage="Select an Agent" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the agent who processed this loan application"></telerik:RadComboBox>
                </div>   
                <asp:Panel runat="server" ID="pnlCat51" Visible="false">
                             
                    <div class="subFormLabel">
                         Mode of Entry
                     </div>
                     <div class="subFormInput">
                        <telerik:RadComboBox ID="cboModeOfEntry" runat="server" CssClass="inputControl"
                            DropDownWidth="355px" EmptyMessage="Select a Mode of Entry" HighlightTemplatedItems="true"
                            MarkFirstMatch="true" AutoCompleteSeparator=""
                            ToolTip="Select the mode of entry of the loan application"></telerik:RadComboBox>
                     </div>    
                 </asp:Panel>
                <asp:Panel runat="server" ID="trInsurance" Visible="false">
                    <div class="subFormLabel">
                         Insurance
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtInsurance" runat="server" CssClass="inputControl" 
                            Value="0" NumberFormat-DecimalDigits="2"
                        ToolTip="Insurance on the loan is entered here"></telerik:RadNumericTextBox>
                    </div>
                </asp:Panel>
                <div class="subFormLabel">
                     Loan Status
                </div>
                <div class="subFormInput">
                    <asp:Label runat="server" CssClass="inputControl" ID="lblStatus"></asp:Label>
                </div>
                <div class="subFormLabel">
                    Amount Approved
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtAmountApproved" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
                </div>
                <div class="subFormLabel">
                        Amount Disbursed
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtDisbursed" 
                    runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
                </div>
                <div class="subFormLabel">
                    Principal Balance
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtBalance" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
                </div>
                <div class="subFormLabel">
                    Processing Fee
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtProcFee" runat="server" CssClass="inputControl" Value="0" NumberFormat-DecimalDigits="2"
                    ToolTip="Processing/Application Fee is entered here"></telerik:RadNumericTextBox>
                </div>
                <asp:Panel runat="server" ID="pnlFees" Visible="false">   
                    <div class="subFormLabel">
                         Grace Period
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtGracePeriod" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                            &nbsp;Days
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtGracePeriod" ErrorMessage="!"
                            ForeColor="Red" Font-Bold="true" ToolTip="Please enter the grace period of the loan"></asp:RequiredFieldValidator>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div class="subForm">
            <telerik:RadComboBox runat="server" ID="cboReason"></telerik:RadComboBox>
            <telerik:RadButton ID="btnSave" Text="Close Loan Account" runat="server" OnClick="btnSave_Click"
                ToolTip="Click to save the loan application and exit this screen"></telerik:RadButton> 
        </div>        
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="2" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Allowances" Visible="false"
                 ToolTip="Click to edit/view the allowances of the employee taking this loan">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Payroll Details" Visible="false"
                 ToolTip="Click to edit/view the salary details of the employee taking this loan">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Loan Guarantors" Selected="True"
                 ToolTip="Click to edit/view the guarantor information for this loan">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Loan Collaterals"
                 ToolTip="Click to edit/view the collateral information for this loan"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Credit Officer Notes"
                 ToolTip="Click to edit/view the credit offficer information for this loan"> 
            </telerik:RadTab>   
            <telerik:RadTab runat="server" Text="Post Dated Checks" Visible="false"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Loan Financials"
                 ToolTip="Click to edit/view the financial information for this loan"> 
            </telerik:RadTab>  
            <telerik:RadTab runat="server" Text="Supporting Documents"
                 ToolTip="Click to upload/edit/view the documents related to this loan application"> 
            </telerik:RadTab>  
            <telerik:RadTab runat="server" Text="Repayment Schedule"
                 ToolTip="Click to view the repayment schedule for this loan"> 
            </telerik:RadTab>  
            <telerik:RadTab runat="server" Text="Loan Repayments"
                 ToolTip="Click to view the repayments made so far on this loan"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Loan Additional Interest"
                 ToolTip="Click to view the penalties/additional interest applied to this loan"> 
            </telerik:RadTab>  
            <telerik:RadTab runat="server" Text="Approval Comments" Visible="false"
                 ToolTip="Click to view the approval comments for this loan entered by the manager"> 
            </telerik:RadTab>  
            <telerik:RadTab runat="server" Text="Credit Manager Comments" Visible="false"
                 ToolTip="Click to view the check list comments for this loan entered by the credit manager"> 
            </telerik:RadTab> 
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView10" runat="server" Selected="true"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Allowance Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboAllowanceType" runat="server" Height="350px"
                              ToolTip="Select the type/category of allowance" DropDownAutoWidth="Enabled" CssClass="inputControl"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Amount
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAllowanceAmount" runat="server" CssClass="inputControl"
                                    ToolTip="Select the amount of allowance"></telerik:RadNumericTextBox>
                    </div>
                </div>
            </div> 
            <telerik:RadGrid ID="gridAllowances" ShowFooter="true" runat="server" AutoGenerateColumns="false" OnItemCommand="gridAllowances_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridDropDownColumn DataField="allowanceTypeID" HeaderText="Allowance Type"
                             DataSourceID="eds1" ListTextField="allowanceTypeName" ListValueField="allowanceTypeID"></telerik:GridDropDownColumn>
                        <telerik:GridBoundColumn DataField="amount" HeaderText="Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="prAllowanceType.isPermanent" HeaderText="Permanent?"></telerik:GridCheckBoxColumn>
                         <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Allowance" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Allowance" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView9" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                         Basic Salary:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtBasicSalary" CssClass="inputControl" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the basic salary of the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                         Tax:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTax" CssClass="inputControl" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the tax deduction the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                         Net Salary:
                    </div>
                    <div class="subFormInput">
                         <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtNetSalary" CssClass="inputControl"  value="0" runat="server"
                                ToolTip="The net salary of the employee taking this loan appears here"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                         Authority Note No:
                    </div>
                    <div class="subFormInput">
                         <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAuthorityNoteNo" CssClass="inputControl" runat="server"
                                ToolTip="Enter the authority note number here"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                          Loan Detail Purpose
                    </div>
                    <div class="subFormInput">
                         <telerik:RadComboBox runat="server" id="cboLoanPurposeDetail" CssClass="inputControl"
                             Height="350px"
                                ToolTip="Select the detail purpose for this loan application"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Gross Salary:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtGrossSalary" CssClass="inputControl" runat="server" enabled="false"
                                ToolTip="Enter the gross salary of the employee taking this loan appears here"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Third Party Deductions:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTotalDeductions" CssClass="inputControl" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the third party deductions of the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        AMD:   
                    </div>                 
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" value="0" CssClass="inputControl" ID="txtAMD" runat="server"
                                ToolTip="The affordable monthly deduction of the employee taking this loan appears here"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Loan Advance No:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtLoanAdvanceNo" CssClass="inputControl" runat="server"
                                ToolTip="Enter the loan advance number here"></telerik:RadTextBox>
                    </div> 
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Soc Sec. /Welfare:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtSSWelfare" CssClass="inputControl" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the social security and/or welfare deduction of the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Deductions Not on Payslip:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTotalDedNotOnPR" CssClass="inputControl" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the social security and/or welfare deduction of the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Pre-audit verified
                    </div>
                    <div class="subFormInput">
                        <asp:CheckBox runat="server" ID="chkPreAudit" CssClass="inputControl"
                                ToolTip="Tick if preaudit has been verified for this loan application"/>
                    </div>
                    <div class="subFormLabel">
                         Loan Purpose
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" autopostback="true" CssClass="inputControl"
                             OnSelectedIndexChanged="cboLoanPurpose_SelectedIndexChanged" id="cboLoanPurpose" Height="350px"
                                ToolTip="Select the purpose for this loan application"></telerik:RadComboBox>
                    </div>
                </div>
            </div> 
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView1" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Surname
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtGSurname" runat="server" CssClass="inputControl"
                                ToolTip="Enter the surname of the guarantor for this loan application"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Other Names
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtGOtherNames" runat="server" CssClass="inputControl"
                                ToolTip="Enter the other names of the guarantor for this loan application"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        ID Number
                    </div>
                    <div class="subFormInput">
                       <telerik:RadComboBox runat="server" ID="cboIDType" CssClass="inputControl"
                                ToolTip="Enter the type of identification of the guarantor for this loan application"></telerik:RadComboBox>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtIDNo" runat="server" CssClass="inputControl"
                                ToolTip="Enter the ID number of the guarantor for this loan application"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        DOB
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker ID="dtGDOB" runat="server" CssClass="inputControl" MinDate="01-01-1900" DateInput-DateFormat="dd-MMM-yyyy" 
                            DateInput-MinDate="1/1/1900 12:00:00 AM"
                                ToolTip="Enter the date of birth of the guarantor for this loan application"></telerik:RadDatePicker>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Phone Number
                    </div>
                    <div class="subFormInput">
                       <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhoneNo" runat="server" CssClass="inputControl"
                                ToolTip="Enter the phone number of the guarantor for this loan application"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Guarantor Address
                    </div>
                    <div class="subFormInput">
                         <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAddress" runat="server" CssClass="inputControl"
                                ToolTip="Enter the address of the guarantor for this loan application"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        City/Town
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCity" runat="server" CssClass="inputControl"
                                ToolTip="Enter the location of the guarantor for this loan application"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Email Address
                    </div>
                    <div class="subFormInput">
                         <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtEmail" runat="server" CssClass="inputControl"
                                ToolTip="Enter the email address of the guarantor for this loan application"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Picture
                    </div>
                    <div class="subFormInput">
                        <telerik:RadAsyncUpload runat="server" ID="upload1" InputSize="20"  
                            CssClass="inputControl" AllowedFileExtensions="png,jpg,jpeg,gif,tiff"
                            Localization-Select="Select Pic" MaxFileSize="100024000" 
                            UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </div>
                </div> 
            </div> 
            <telerik:RadGrid ID="gridGuarantor" runat="server" AutoGenerateColumns="false" OnItemCommand="gridGuarantor_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="idNo.idNo1" HeaderText="ID No"></telerik:GridBoundColumn>
                        <telerik:GridBinaryImageColumn HeaderText="Picture" ResizeMode="Fit" DataField="image.image1" ImageWidth="48px" ImageHeight="48px"></telerik:GridBinaryImageColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Guarantor" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Guarantor" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="imageFrame">
                        <telerik:RadRotator Width="132" Height="132" runat="server" ID="rotator2" ItemHeight="132" 
                            ItemWidth="132" FrameDuration="30000"></telerik:RadRotator>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                         Collateral Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboCollateralType" runat="server" CssClass="inputControl"
                                ToolTip="Select the type of collateral presented for this loan application"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Collateral Pictures
                    </div>
                    <div class="subFormInput">
                       <telerik:RadAsyncUpload runat="server" ID="upload2" 
                           InputSize="20"  CssClass="inputControl" AllowedFileExtensions="png,jpg,jpeg,gif,tiff,docx,doc,xls,xlsx,pdf" 
                           Localization-Select="Select Pic" MaxFileSize="1024000" 
                           UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </div>
                    <div class="subFormLabel">
                        Collateral Description 
                    </div>
                    <div class="subFormInput">
                       <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCollateralDescription" TextMode="MultiLine" Rows="1" runat="server" CssClass="inputControl"
                                ToolTip="Enter the description the collateral presented for this loan application"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Fair Value
                    </div>
                    <div class="subFormInput">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtFairValue" runat="server" CssClass="inputControl"
                                ToolTip="Enter the value of the collateral presented for this loan application"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Legal Owner
                    </div>
                    <div class="subFormInput">
                         <telerik:RadComboBox ID="cboLegalOwner" runat="server" CssClass="inputControl"
                                ToolTip="Enter the legal owner the collateral presented for this loan application"></telerik:RadComboBox>
                    </div> 
                </div> 
            </div> 
            <telerik:RadGrid ID="gridCollateral" runat="server" AutoGenerateColumns="false" 
                ShowFooter="true" OnItemCommand="gridCollateral_ItemCommand" OnDetailTableDataBind="gridCollateral_DetailTableDataBind">
                <MasterTableView>
                    <Columns>
                        <telerik:GridDropDownColumn DataField="collateralTypeID" HeaderText="Collateral Type"
                             DataSourceID="eds3" ListValueField="collateralTypeID" ListTextField="collateralTypeName"></telerik:GridDropDownColumn>
                        <telerik:GridBoundColumn DataField="legalOwner" HeaderText="Legal Owner"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="fairValue" HeaderText="Fair Value" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Collateral" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Collateral" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                    <DetailTables>
                        <telerik:GridTableView>
                            <Columns>
                                <telerik:GridBinaryImageColumn DataField="image.image1"></telerik:GridBinaryImageColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView runat="server">
            <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCNotes" runat="server" Rows="20" TextMode="MultiLine"  Height="277px" Width="746px"
                                ToolTip="Input/Append all notes related to the loan application"></telerik:RadTextBox>
        </telerik:RadPageView>        
        <telerik:RadPageView ID="RadPageView7" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                         Bank Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboBank" runat="server" CssClass="inputControl"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                         Check Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" ID="dtCheckDate" ></telerik:RadDatePicker>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Check Amount
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" 
                            ID="txtCheckAmount" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Check Number
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
            </div> 
            <telerik:RadGrid ID="gridChecks" ShowFooter="true" runat="server" AutoGenerateColumns="false" OnItemCommand="gridChecks_ItemCommand">
                <MasterTableView>
                    <Columns> 
                        <telerik:GridBoundColumn DataField="checkDate" HeaderText="Check Date" DataFormatString="{0:dd-MMM-yyyy}" ></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="checkAmount" HeaderText="Check Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="checkNumber" HeaderText="Check No" ></telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="cashed" HeaderText="Cashed"></telerik:GridCheckBoxColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Check" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Check" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView5" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                         Financials
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboFinancialType" runat="server" CssClass="inputControl"
                                ToolTip="Select the type of financials for this applicant/client"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                         Other Costs
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtOtherCosts" runat="server" CssClass="inputControl" Value="0"
                                ToolTip="Enter the other costs for this applicant/client"></telerik:RadNumericTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Revenue
                    </div>
                    <div class="subFormInput">
                         <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtRevenue" runat="server" CssClass="inputControl"
                                ToolTip="Enter the revenue for this applicant/client"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Frequency
                    </div>
                    <div class="subFormInput">
                         <telerik:RadComboBox ID="cboFrequency" runat="server" CssClass="inputControl"
                                ToolTip="Select the frequency of the financials of this applicant/client"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Expenses
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtExpenses" runat="server" Value="0" CssClass="inputControl"
                                ToolTip="Enter the expenses for this applicant/client"></telerik:RadNumericTextBox>
                    </div>
                </div>
            </div>   
            <telerik:RadGrid ID="gridFinancials" ShowFooter="true" runat="server" AutoGenerateColumns="false" OnItemCommand="gridFinancials_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridDropDownColumn DataField="financialTypeID" HeaderText="Financial Type"
                             DataSourceID="eds2" ListTextField="financialTypeName" ListValueField="financialTypeID"></telerik:GridDropDownColumn>
                        <telerik:GridBoundColumn DataField="repaymentMode.repaymentModeName" HeaderText="Frequency"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="revenue" HeaderText="Revenue" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="expenses" HeaderText="Expenses" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="otherCosts" HeaderText="Other Costs" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Financials" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Financials" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView8" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                          Document Description
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtDocDesc" runat="server" CssClass="inputControl"
                                ToolTip="Input a description of the document"></telerik:RadTextBox>
                    </div> 
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Documents
                    </div>
                    <div class="subFormInput">
                          <telerik:RadAsyncUpload runat="server" ID="upload3" InputSize="20"  CssClass="inputControl" 
                              AllowedFileExtensions="pdf,txt,docx,xlsx,doc,xls,html,jpg,jpeg,png,gif"
                               Localization-Select="Select Pic" MaxFileSize="100024000" UploadedFilesRendering="BelowFileInput"
                                ToolTip="Click to selecte the file to upload"></telerik:RadAsyncUpload>  
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
        <telerik:RadPageView ID="RadPageView3" runat="server">             
            <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="1000px" AllowPaging="true" PageSize="10" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                 OnLoad="gridSchedule_Load">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="repaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="principalPayment" HeaderText="Principal Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestPayment" HeaderText="Interest Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridCalculatedColumn Expression="principalPayment+interestPayment" HeaderText="Total Payment" DataFormatString="{0:#,###.#0}" Aggregate="Sum" DataType="System.Double" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridCalculatedColumn>
                        <telerik:GridNumericColumn DataField="balanceCD" HeaderText="Principal C/D" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Interest Remaining" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Principal Remaining" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="proposedInterestWriteOff" HeaderText="Prop. Int. Write-Off" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestWritenOff" HeaderText="Interest Written-Off" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server">             
            <telerik:RadGrid ID="gridRepayment" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="repaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="repaymentType.repaymentTypeName" HeaderText="Payment Type" ></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amountPaid" HeaderText="Amount Paid" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principalPaid" HeaderText="Principal Paid" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestPaid" HeaderText="Interest Paid" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="feePaid" HeaderText="Fee Paid" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="penaltyPaid" HeaderText="Add. Interest Paid" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="commission_paid" HeaderText="Commission Paid" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        <telerik:GridBoundColumn DataField="checkNo"  HeaderText="CheckNo"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView6" runat="server">             
            <telerik:RadGrid ID="gridPenalty" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="penaltyDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridNumericColumn DataField="penaltyFee" HeaderText="Interest Amount" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="proposedAmount" HeaderText="Proposed Ineterest" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="penaltyBalance" HeaderText="Balance" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView11" runat="server">
            <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAppComments" Enabled="false" runat="server" Rows="20" TextMode="MultiLine"  Height="277px" Width="746px"></telerik:RadTextBox>
        </telerik:RadPageView> 
        <telerik:RadPageView ID="RadPageView12" runat="server">
            <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCreditManagerNotes" Enabled="false" runat="server" Rows="20" TextMode="MultiLine"  Height="277px" Width="746px"></telerik:RadTextBox>
        </telerik:RadPageView> 
    </telerik:RadMultiPage>
    <ef:EntityDataSource ID="eds1" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="prAllowanceTypes">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="eds2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="financialTypes">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="eds3" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="collateralTypes">
    </ef:EntityDataSource>
</asp:Content>
