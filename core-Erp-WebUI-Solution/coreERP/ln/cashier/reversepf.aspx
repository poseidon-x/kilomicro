<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="reversepf.aspx.cs" Inherits="coreERP.ln.cashier.reversepf" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Reverse Loan Additional Interest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Additional Interest Reversal</h3>
        <table border="1">
            <tr>
                <td colspan="2">Confirm the reversal of the below processing fee</td>
            </tr>
            <tr>
                <td>Interest Date</td>
                <td><asp:Label ID="lbFeeDate" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Type of Fee</td>
                <td><asp:Label ID="lblTypeOfRepayment" runat="server">Processing Fee</asp:Label></td>
            </tr>
            <tr>
                <td>Fee Amount</td>
                <td><asp:Label ID="lblAmount" runat="server"></asp:Label></td>
            </tr> 
            <tr>
                <td colspan="2"><telerik:RadButton ID="btnOk" runat="server" Text="Okay" OnClick="btnOk_Click"></telerik:RadButton></td>
            </tr>
        </table>
</asp:Content>
