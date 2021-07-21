<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="cashUpTill.aspx.cs" Inherits="coreERP.ln.setup.cashUpTill" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Cash Up Cashiers Till
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Cash Up Cashiers Till</h3>
    <br />
    <table>
        <tr>
            <td>Date:</td>
            <td><telerik:RadDatePicker ID="dtDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy" 
                AutoPostBack="true" OnSelectedDateChanged="dtDate_SelectedDateChanged"></telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td>Cashier:</td>
            <td><telerik:RadComboBox runat="server" Width="300px" DropDownAutoWidth="Enabled" 
                MarkFirstMatch="true"  AutoPostBack="true"
                 AutoCompleteSeparator=" " ID="cboUserName" OnSelectedIndexChanged="cboUserName_SelectedIndexChanged"></telerik:RadComboBox></td>
        </tr>
        <tr>
            <td>Balance In Till</td>
            <td><asp:Label runat="server" ID="lblBalance"></asp:Label></td>
        </tr>
        <tr>
            <td>Cash/Bank Account to Cashup</td>
            <td>
                <telerik:RadComboBox runat="server" ID="cboAcc" EmptyMessage="Please select the cash or Bank Account"
                    MarkFirstMatch="true" DropDownAutoWidth="Enabled" Width="250px"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td><telerik:RadButton ID="btnOpen" Enabled="false" runat="server" Text="Cashup Till" OnClick="btnOpen_Click"></telerik:RadButton></td> 
        </tr>
    </table>
</asp:Content>
