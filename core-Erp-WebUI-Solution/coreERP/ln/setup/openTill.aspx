<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="openTill.aspx.cs" Inherits="coreERP.ln.setup.openCloseTill" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Open Or Close Cashiers Till
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Open Or Close Cashier's Till</h3>
    <br />
    <table>
        <tr>
            <td>Till Date:</td>
            <td><telerik:RadDatePicker ID="dtStartDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker></td>
        </tr> 
        <tr>
            <td>Cashier:</td>
            <td><telerik:RadComboBox runat="server" Width="300px" DropDownAutoWidth="Enabled" MarkFirstMatch="true" 
                 AutoCompleteSeparator=" " ID="cboUserName"></telerik:RadComboBox></td>
        </tr>
        <tr>
            <td><telerik:RadButton ID="btnOpen" runat="server" Text="Open Cashier's Till" OnClick="btnOpen_Click"></telerik:RadButton></td>
            <td><telerik:RadButton ID="btnClose" runat="server" Text="Close Cashier's Till" OnClick="btnClose_Click"></telerik:RadButton></td>
        </tr>
    </table>
</asp:Content>
