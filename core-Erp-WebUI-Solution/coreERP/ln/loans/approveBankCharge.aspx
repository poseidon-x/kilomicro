<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approveBankCharge.aspx.cs" Inherits="coreERP.ln.loans.approveBankCharge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Approve Bank Charges on Loan Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Approve Bank Charges on Loan Account</h3>
    <table>
        <tr>
            <td>
                Date:
            </td>
            <td>
                <telerik:RadDatePicker runat="server" ID="dtDate" DateInput-DateFormat="dd-MMM-yyyy" ></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td>
                Client:
            </td>
            <td>
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number" ></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Apply to Loan ID:
            </td>
            <td>
                <telerik:RadComboBox ID="cboLoan" runat="server" 
                        DropDownWidth="355px" EmptyMessage="Select a Loan" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Charge Amount
            </td>
            <td>
                <telerik:RadNumericTextBox runat="server" ID="txtAmount"></telerik:RadNumericTextBox>
            </td>
        </tr>
    </table> 
    <br />
    <telerik:RadButton runat="server" Text="Approve Bank Charges on Selected Loan Account" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Penalties" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
