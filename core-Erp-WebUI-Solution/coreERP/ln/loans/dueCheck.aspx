<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="dueCheck.aspx.cs" Inherits="coreERP.ln.loans.dueCheck" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Apply Due Cheque
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Client Due Checks</h3>
    <table>
        <tr>
            <td style="width:150px">
                Loan
            </td>
            <td style="width:200px">
                <telerik:RadComboBox runat="server" ID="cboLoan" MaxHeight="300px" DropDownAutoWidth="Enabled"
                     Width="300px"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td style="width:150px">
                Check Number
            </td>
            <td style="width:200px">
                <asp:Label runat="server" ID="lblCheckNumber"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:150px">
                Client ID
            </td>
            <td style="width:200px">
                <asp:Label runat="server" ID="lblClientID"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:150px">
                Client Name
            </td>
            <td style="width:200px">
                <asp:Label runat="server" ID="lblClientName"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:150px">
                Check Date
            </td>
            <td style="width:200px">
                <telerik:RadDatePicker runat="server" ID="dtDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td style="width:150px">
                Check Type
            </td>
            <td style="width:200px">
                <telerik:RadComboBox runat="server" ID="cboCheckType"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td style="width:150px">
                Bank
            </td>
            <td style="width:200px">
                    <telerik:RadComboBox ID="cboBank" runat="server"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <telerik:RadButton runat="server" ID="btnApply" Text="Apply Check" OnClick="btnApply_Click"></telerik:RadButton>
                <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Check" OnClick="btnCancel_Click"></telerik:RadButton>
            </td>
        </tr>
    </table>
</asp:Content>
