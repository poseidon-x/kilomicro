<%@ Page Title="" Language="C#" MasterPageFile="~/setupPage.Master" AutoEventWireup="true" CodeBehind="pending.aspx.cs" Inherits="coreERP.cu.edit.pending" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Post Transactions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server"> 
    <script src="../../scripts/app/CreditUnion/pendingTransactions.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3 class="pageBanner">Post Share Transactions</h3>  
    <div id="setupGrid" ></div>
</asp:Content>
