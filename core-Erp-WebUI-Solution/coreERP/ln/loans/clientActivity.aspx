<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="clientActivity.aspx.cs" Inherits="coreERP.ln.loans.clientActivity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Client Activity Log Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Client Activity Log</h3>
    <table>
        <tr>
            <td style="width:150px">
                Client Name:
            </td>
            <td style="width:200px">
                <select id="client"></select>
            </td>
            <td style="width:150px">
                Loan ID:
            </td>
            <td style="width:200px">
                <select id="loan"></select>
            </td>
        </tr> 
    </table>
    <asp:Panel runat="server" Visible="false" ID="pnlEdit">
         
    </asp:Panel>
    <div id="grid"></div>
    <script src="../../scripts/app/Clients/clientActivity.js"></script>
</asp:Content>
