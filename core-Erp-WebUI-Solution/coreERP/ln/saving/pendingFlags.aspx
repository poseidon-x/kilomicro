<%@ Page Title="" Language="C#" MasterPageFile="~/setupPage.Master" AutoEventWireup="true" CodeBehind="pendingFlags.aspx.cs" Inherits="coreERP.ln.saving.pendingFlags" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Approve/Deny Flagged Accounts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server"> 
    <script src="../../scripts/app/Loans/savings/pendingFlags.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3 class="pageBanner">Approve/Deny Flagged Accounts</h3>  
    <div id="setupGrid" ></div>
    <div class="btn btn-primary" id="approveSelected">Approve All Selected</div>
    <div class="btn btn-primary" id="denySelected">Deny All Selected</div>
</asp:Content>
