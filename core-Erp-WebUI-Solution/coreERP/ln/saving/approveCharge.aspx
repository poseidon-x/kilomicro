<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approveCharge.aspx.cs" Inherits="coreERP.ln.saving.approveCharge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Approve Charges on Regular Deposit Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Approve Charges on Regular Deposit Account</h3>
    <table>
        <tr>
            <td>Select Client</td>
            <td>
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Select Savings  Account</td>
            <td>
                <telerik:RadComboBox ID="cboAmount" runat="server"
                        DropDownAutoWidth="Enabled" EmptyMessage="Select the savings account" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Select Charge Type</td>
            <td>
                <telerik:RadComboBox ID="cboChargeType" runat="server"
                        DropDownAutoWidth="Enabled" EmptyMessage="Select the charge type" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Memo (Comments) </td>
            <td>
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtMemo" TextMode="MultiLine" Rows="5" Width="400px"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>Date</td>
            <td>
                <telerik:RadDatePicker ID="dtInterestDate1" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td>Charge Amount</td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtAmount" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
            </td>
        </tr>
    </table> 
    <br />
    <telerik:RadButton runat="server" Text="Approve Charges" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Interests" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
