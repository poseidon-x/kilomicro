<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="reverserp.aspx.cs" Inherits="coreERP.ln.cashier.reverserp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Reverse Loan Repayment
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Repayment Reversal</h3>
        <table border="1">
            <tr>
                <td colspan="2">Confirm the reversal of the below repayment</td>
            </tr>
            <tr>
                <td>Repayment Date</td>
                <td><asp:Label ID="lbRepaymentDate" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Type of Repayment</td>
                <td><asp:Label ID="lblTypeOfRepayment" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Amount Paid</td>
                <td><asp:Label ID="lblAmountPaid" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Check No.</td>
                <td><asp:Label ID="lblCheckNo" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="2"><telerik:RadButton ID="btnOk" runat="server" Text="Okay" OnClick="btnOk_Click"></telerik:RadButton></td>
            </tr>
        </table>
</asp:Content>
