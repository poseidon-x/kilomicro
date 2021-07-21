<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="transfer.aspx.cs" Inherits="coreERP.ln.saving.transfer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Transfer Between Regular Deposit Accounts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Transfer Between Regular Deposit Accounts</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                    <h3>Payer Client</h3>
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="cboClient_SelectedIndexChanged" CssClass="inputControl"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of payer client"
                     HighlightTemplatedItems="true" CausesValidation="false"
                     EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox> 
            </div> 
            <div class="imageFrame">
                <telerik:RadRotator Width="216" Height="216" runat="server" ID="rotator1" ItemHeight="216" 
                    ItemWidth="216" FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="subFormLabel">
                Client ID.
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="inputControl"></telerik:RadTextBox>
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
            <div class="subFormLabel">
                <telerik:RadButton ID="btnFind" runat="server" Text="Find Client" OnClick="btnFind_Click"></telerik:RadButton>
            </div>
            <div class="subFormInput"> 
            </div>
            <div class="subFormLabel">
                Payer Account
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboAccount"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Amount to Transfer
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox runat="server" ID="txtAmount" NumberFormat-DecimalDigits="6"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                <telerik:RadButton ID="btnSave" runat="server" Text="Transfer Funds" OnClick="btnSave_Click"></telerik:RadButton>
            </div>
            <div class="subFormInput"> 
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                    <h3>Payee Client</h3>
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient2" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="cboClient2_SelectedIndexChanged" CssClass="inputControl"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of payee client"
                     HighlightTemplatedItems="true" CausesValidation="false"
                     EnableLoadOnDemand="true" OnItemsRequested="cboClient2_ItemsRequested"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox> 
            </div> 
            <div class="imageFrame">
                <telerik:RadRotator Width="216" Height="216" runat="server" ID="rotator2" ItemHeight="216" 
                    ItemWidth="216" FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="subFormLabel">
                Account No.
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo2" runat="server" CssClass="inputControl"></telerik:RadTextBox>
            </div>
            <asp:Panel ID="pnlRegular2" runat="server" Visible="true">
                <div class="subFormLabel">
                    Surname
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname2" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                </div>
                <div class="subFormLabel">
                    Other Names
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames2" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlJoint2" runat="server" Visible="false"> 
                <div class="subFormLabel">
                    Account Name
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtJointAccountName2" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                </div>
            </asp:Panel>
            <div class="subFormLabel">
                    <telerik:RadButton ID="btnFind2" runat="server" Text="Find Client" OnClick="btnFind2_Click"></telerik:RadButton>
            </div>
            <div class="subFormInput"> 
            </div>
            <div class="subFormLabel">
                Payee Account
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboAccount2"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                  Transaction Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div> 
            <div class="subFormLabel">
                Narration
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox runat="server" ID="txtNarration" Rows="3" ></telerik:RadTextBox>
            </div>
        </div>
    </div>
</asp:Content>
