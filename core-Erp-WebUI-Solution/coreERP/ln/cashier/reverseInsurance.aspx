<%@ Page Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="reverseInsurance.aspx.cs" Inherits="coreERP.ln.cashier.reverseInsurance1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Reverse Loan Insurance Fee
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Insurance Payment</h3>
        <table border="1">
            <tr>
                <td colspan="2">Confirm the reversal of the below insurance fee</td>
            </tr>
            <tr>
                <td>Insurance Date</td>
                <td><asp:Label ID="lblInsuranceDate" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Type of Fee</td>
                <td><asp:Label ID="lblTypeOfRepayment" runat="server">Insurance Fee</asp:Label></td>
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


