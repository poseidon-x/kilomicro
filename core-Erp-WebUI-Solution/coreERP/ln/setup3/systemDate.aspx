<%@ Page Title="" Language="C#" MasterPageFile="~/setupPage.Master" AutoEventWireup="true" CodeBehind="systemDate.aspx.cs" Inherits="coreERP.ln.setup3.systemDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
System Dates Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server"> 
    <script src="../../scripts/app/Loans/setup/systemDate.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3 class="pageBanner">System Dates Management</h3>  
    <div id="setupGrid" ></div>
</asp:Content>
