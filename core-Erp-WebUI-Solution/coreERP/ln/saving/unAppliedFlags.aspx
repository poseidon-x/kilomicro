<%@ Page Title="" Language="C#" MasterPageFile="~/setupPage.Master" AutoEventWireup="true" CodeBehind="unAppliedFlags.aspx.cs" Inherits="coreERP.ln.saving.unAppliedFlags" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Apply Flagged Accounts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server"> 
    <script src="../../scripts/app/Loans/savings/unAppliedFlags.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3 class="pageBanner">Apply Flagged Accounts For Migration</h3>  
    <div id="setupGrid" ></div>
    <div class="btn btn-primary" id="approveSelected">Apply All Selected</div>
    <div class="btn btn-primary" id="rejectSelected">Reject All Selected</div>
</asp:Content>
