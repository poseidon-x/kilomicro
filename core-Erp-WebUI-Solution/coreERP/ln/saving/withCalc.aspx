<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="withCalc.aspx.cs" Inherits="coreERP.ln.saving.withCalc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Regular Deposit Account Withdrawal Calculator
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Regular Deposit Account Withdrawal Calculator</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Client
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="cboClient_SelectedIndexChanged" CssClass="inputControl"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client"
                     HighlightTemplatedItems="true" CausesValidation="false"
                     EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox> 
            </div>
            <div class="subFormLabel">
                Take Home Amount
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox runat="server" ID="txtTakeHomeAmount"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Available Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox  Enabled="False" runat="server" ID="txtAvailPrincBal"></telerik:RadNumericTextBox>
            </div><div class="subFormLabel">
                Total Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox  Enabled="False" runat="server" ID="txtCurPrincBal"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Maturity Date
            </div>            
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" ID="dtpMatDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Principal That Can Be Withdrawn
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox  Enabled="False" runat="server" ID="txtPrincWith"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Gross Withdrawal Amount
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox  Enabled="False" runat="server" ID="txtGrossWith"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Regular Deposit Account
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboSavings" runat="server" AutoPostBack="true"  
                        DropDownAutoWidth="Enabled" EmptyMessage="Select Deposit Account" 
                    Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox> 
            </div> 
            <div class="subFormLabel">
                <telerik:RadButton runat="server" ID="btnCalc" Text="Calculate Charges"
                     OnClick="btnCalc_Click"></telerik:RadButton>
            </div> 
            <div class="subFormInput">
                <telerik:RadButton runat="server" ID="btnEmptyAccount" Text="Empty Account"
                     OnClick="btnEmptyAccount_Click"></telerik:RadButton>
            </div> 
            <div class="subFormLabel">
                Remaining Interest
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox  Enabled="False" runat="server" ID="txtAvailIntBal"></telerik:RadNumericTextBox>
            </div>  
            <div class="subFormLabel">
                Total Charges
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox  Enabled="False" runat="server" ID="txtTotalCharges"></telerik:RadNumericTextBox>
            </div> 
            <div class="subFormLabel">
                Interest that can be withdrawn
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox  Enabled="False" runat="server" ID="txtIntWIth"></telerik:RadNumericTextBox>
            </div> 
            <div class="subFormLabel">
                Net Amount that can be withdrawn
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox  Enabled="False" runat="server" ID="txtNetWith"></telerik:RadNumericTextBox>
            </div>
        </div>
    </div>
</asp:Content>
