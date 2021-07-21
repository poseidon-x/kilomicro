<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="reverseai.aspx.cs" Inherits="coreERP.ln.cashier.reverseai" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Reverse Loan Additional Interest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h>Loan Additional Interest Reverse</h3>
        <table border="1">
            <tr>
                <td colspan="2">Confirm the reversal of the below additional interest</td>
            </tr>
            <tr>
                <td>Interest Date</td>
                <td><asp:Label ID="lbInterestDate" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Type of Interest</td>
                <td><asp:Label ID="lblTypeOfRepayment" runat="server">Additional Interest</asp:Label></td>
            </tr>
            <tr>
                <td>Interest Amount</td>
                <td><asp:Label ID="lblAmount" runat="server"></asp:Label></td>
            </tr> 
            <tr>
                <td colspan="2"><telerik:RadButton ID="btnOk" runat="server" Text="Okay" OnClick="btnOk_Click"></telerik:RadButton></td>
            </tr>
        </table>
</asp:Content>
