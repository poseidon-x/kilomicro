<%@ Page Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="leave.aspx.cs" Inherits="coreERP.hc.leave.leave" EnableEventValidation="false"  %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Leave of Absence Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Leave of Absence Management</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="132" Height="132" runat="server" ID="rotator1" ItemHeight="132" ItemWidth="132" 
                    FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="subFormLabel">
                Staff No.
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="inputControl"></telerik:RadTextBox>
            </div>
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
            <div class="imageFrame">
                <telerik:RadButton ID="btnFind" runat="server" 
                    Text="Find Staff" OnClick="btnFind_Click" CausesValidation="false"></telerik:RadButton>
            </div>
            <div class="subFormLabel">
                Staff Selected
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboStaff" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="cboStaff_SelectedIndexChanged" CssClass="inputControl"
                        DropDownWidth="300px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Leave Days
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtLeaveDays" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtLeaveDays" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the number of leave days to be taken"></asp:RequiredFieldValidator>
            </div>
            <div class="subFormLabel">
                Application Date
            </div>
            <div class="subFormInput">
               <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dtAppDate" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the application date for the loan"></asp:RequiredFieldValidator>
            </div>
            <div class="subFormLabel">
                Leave Type
            </div>
            <div class="subFormInput">
               <telerik:RadComboBox id="cboLeaveType" CssClass="inputControl" runat="server"></telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cboLeaveType" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please select the type of the leave of absence"></asp:RequiredFieldValidator>
            </div>
            <div class="subFormLabel">
                Leave Starts Date
            </div>
            <div class="subFormInput">
               <telerik:RadDatePicker ID="dtpLeaveStartsDate" CssClass="inputControl" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="dtpLeaveStartsDate" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the leave starts date"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="subForm">
        <telerik:RadButton ID="btnSave" Text="Save Leave Application" runat="server" OnClick="btnSave_Click"></telerik:RadButton>
    </div>    
</asp:Content>
