<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="regularSusuAuthorize.aspx.cs" Inherits="coreERP.ln.susu.regularSusuAuthorize" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Authorize Normal Susu Account for Payout
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">  
    <h3>Authorize Normal Susu Account for Payout</h3>  
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
                    <telerik:RadButton ID="btnFind" runat="server" Text="Find Client" Visible="false"
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
                    Application Date
                </div>
                <div class="subFormInput">
                    <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"
                        DateInput-ToolTip="Select the date of the loan application"></telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dtAppDate" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the application date for the susu account"></asp:RequiredFieldValidator>
                </div>         
                <div class="subFormLabel">
                    Approval Date
                </div>
                <div class="subFormInput">
                    <telerik:RadDatePicker ID="dtApproval" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false" 
                        DateInput-ToolTip="Select the date of the loan application"></telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="dtApproval" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the approval date for the susu account"></asp:RequiredFieldValidator>
                </div>    
        
                <div class="subFormLabel">
                     Total Contribution
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2" ID="txtTotalContribution" Enabled="false"></telerik:RadNumericTextBox>
                </div> 
            </div>
            <div class="subFormColumnRight">   
                <div class="subFormLabel">
                     Account Status
                </div> 
                <div class="subFormInput">
                    <asp:Label runat="server" CssClass="inputControl" ID="lblStatus"></asp:Label>
                </div>   
                <div class="subFormLabel">
                     Contribution Amount
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2" ID="txtContribAmount" Enabled="false"></telerik:RadNumericTextBox>
                </div>     
                <div class="subFormLabel">
                     Total Commission
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" NumberFormat-DecimalDigits="2" ID="txtCommission" Enabled="false"></telerik:RadNumericTextBox>
                </div>  
                <div class="subFormLabel">
                     Net Entitled Amount
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2" ID="txtNetAmount" Enabled="false"></telerik:RadNumericTextBox>
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
            </div>
        </div>
        <div class="subForm">
            <telerik:RadButton ID="btnSave" Text="Authorize Susu Account for Payout" runat="server" OnClick="btnSave_Click"
                ToolTip="Click to approve the Normal Susu account application and exit this screen"></telerik:RadButton> 
        </div>
</asp:Content>
