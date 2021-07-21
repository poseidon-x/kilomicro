<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="reversedb.aspx.cs" Inherits="coreERP.ln.cashier.reversedb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Reverse Loan Disbursement
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Disbursement Reversal</h3>
        <table border="1">
            <tr>
                <td colspan="2">Confirm the reversal of the below disbursment</td>
            </tr>
            <tr>
                <td>Repayment Date</td>
                <td><asp:Label ID="lbRepaymentDate" runat="server"></asp:Label></td>
            </tr> 
            <tr>
                <td>Amount Disbursed</td>
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
