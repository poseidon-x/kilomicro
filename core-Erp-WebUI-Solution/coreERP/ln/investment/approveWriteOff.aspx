<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approveWriteOff.aspx.cs" Inherits="coreERP.ln.investment.approveWriteOff" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Write Off Interest on Investment Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Interest Write-Off Approval</h3>
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
            <td>Select Deposit Account</td>
            <td>
                <telerik:RadComboBox ID="cboAmount" runat="server"
                        DropDownAutoWidth="Enabled" EmptyMessage="Select the investment account" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td> 
        </tr>
        <tr>
            <td>Date</td>
            <td>
                <telerik:RadDatePicker ID="dtInterestDate1" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td> Amount</td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtAmount" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
            </td>
        </tr>
    </table> 
    <br />
    <telerik:RadButton runat="server" Text="Approve Write Off" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
