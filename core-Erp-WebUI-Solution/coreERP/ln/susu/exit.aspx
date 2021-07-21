<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="exit.aspx.cs" Inherits="coreERP.ln.susu.exit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Pre-mature Exit from Susu Accounts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Pre-mature Exit from Susu Accounts</h3>   
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
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Other Names
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadTextBox>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlJoint" runat="server" Visible="false"> 
                    <div class="subFormLabel">
                        Account Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtJointAccountName" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadTextBox>
                    </div>
                </asp:Panel>     
                <div class="subFormLabel">
                    <telerik:RadButton ID="btnFind" runat="server" CssClass="inputControl" Text="Find Client" Visible="false"
                        OnClick="btnFind_Click" CausesValidation="false"></telerik:RadButton>
                </div>
                <div class="subFormInput">
                </div>  
            </div>
            <div class="subFormColumnMiddle">       
                <div class="subFormLabel">
                     Selected Client
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged" CssClass="inputControl"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client"  
                    MarkFirstMatch="true" AutoCompleteSeparator="" CausesValidation="false" Enabled="false"
                         EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested"
                        ToolTip="Select the  client whose application is being entered"></telerik:RadComboBox>
                </div>  
                <div class="subFormLabel">
                    Susu Grade
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox runat="server" ID="cboSusuGrade" CssClass="inputControl" AutoPostBack="true" CausesValidation="false"
                         OnSelectedIndexChanged="cboSusuGrade_SelectedIndexChanged"></telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="cboSusuGrade" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please select the grade of susu being applied for"></asp:RequiredFieldValidator>
                </div>  
                <div class="subFormLabel">
                    Susu Position
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox runat="server" ID="cboSusuPosition" CssClass="inputControl" AutoPostBack="true" CausesValidation="false"
                         OnSelectedIndexChanged="cboSusuPosition_SelectedIndexChanged"></telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="cboSusuPosition" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please select the position of susu being applied for"></asp:RequiredFieldValidator>
                </div>          
                <div class="subFormLabel">
                    Application Date
                </div>
                <div class="subFormInput">
                    <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"
                        DateInput-ToolTip="Select the date of the loan application"></telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dtAppDate" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the application date for the susu account"></asp:RequiredFieldValidator>
                </div>                      
                <div class="subFormLabel">
                    Assigned Staff
                </div>
               <div class="subFormInput">
                    <telerik:RadComboBox ID="cboStaff" runat="server" CssClass="inputControl"
                        DropDownWidth="355px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true"
                        MarkFirstMatch="true" AutoCompleteSeparator=""
                        ToolTip="Select the staff assigned to the loan"></telerik:RadComboBox>
                </div> 
                <div class="subFormLabel">
                    Assigned Agent
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox ID="cboAgent" runat="server" CssClass="inputControl"
                        DropDownWidth="355px" EmptyMessage="Select an Agent" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the agent who processed this loan application"></telerik:RadComboBox>
                </div>          
                <div class="subFormLabel">
                    Exit Approval Date
                </div>
                <div class="subFormInput">
                    <telerik:RadDatePicker ID="dtApproval" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"
                        DateInput-ToolTip="Select the date of the loan application"></telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="dtApproval" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the approval date for the susu account"></asp:RequiredFieldValidator>
                </div>    
                <div class="subFormLabel">
                    Assigned Susu Group
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox runat="server" ID="cboSusuGroup" CssClass="inputControl" CausesValidation="false"></telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cboSusuGroup" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please select the grade of susu being applied for"></asp:RequiredFieldValidator>
                </div> 
            </div>
            <div class="subFormColumnRight">   
                <div class="subFormLabel">
                     Account Status
                </div> 
                <div class="subFormInput">
                    <asp:TextBox Enabled="false" ReadOnly="true" runat="server" ID="lblStatus" CssClass="inputControl"></asp:TextBox>
                </div>   
                <div class="subFormLabel">
                     Contribution Amount
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2" ID="txtContribAmount" Enabled="false"></telerik:RadNumericTextBox>
                </div> 
                <div class="subFormLabel">
                     Amount To Be Contributed
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2" ID="txtAmountEntitled" Enabled="false"></telerik:RadNumericTextBox>
                </div>   
                <div class="subFormLabel">
                     Interest Deduction/Addition
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2" ID="txtInterestDed" Enabled="false"></telerik:RadNumericTextBox>
                </div>    
                <div class="subFormLabel">
                     Total Commission
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2" ID="txtCommission" Enabled="false"></telerik:RadNumericTextBox>
                </div>   
                <div class="subFormLabel">
                     Net Entitled Amount
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2" ID="txtNetAmount" Enabled="false"></telerik:RadNumericTextBox>
                </div> 
                <div class="subFormLabel">
                     Amount Disbursed
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" Value="0" NumberFormat-DecimalDigits="2" ID="txtAmountDisbursed" Enabled="false"></telerik:RadNumericTextBox>
                </div> 
                <div class="subFormLabel">
                     Date Due
                </div> 
                <div class="subFormInput">
                    <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtEntitled" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"></telerik:RadDatePicker>
                </div>  
                <div class="subFormLabel">
                     Expiry Date
                </div> 
                <div class="subFormInput">
                    <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtDue" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"></telerik:RadDatePicker>
                </div>
                <div class="subFormLabel">
                     Date Disbursed
                </div> 
                <div class="subFormInput">
                    <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtDisbursed" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"></telerik:RadDatePicker>
                </div>
            </div>
        </div>
        <div class="subForm">
            <telerik:RadButton ID="btnSave" Text="Approve Susu Account Exit" runat="server" OnClick="btnSave_Click"
                ToolTip="Click to approve the susu account application and exit this screen"></telerik:RadButton> 
        </div>
</asp:Content>
