<%@ Page Title="" Language="C#" MasterPageFile="~/setupPage.Master" AutoEventWireup="true" CodeBehind="momoProvider.aspx.cs" Inherits="coreERP.momo.setup.momoProvider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Mobile Money Providers Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server"> 
    <script src="../../scripts/app/MobileMoney/momoProvider.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3 class="pageBanner">Mobile Money Providers Management</h3>  
    <div id="setupGrid" ></div>
</asp:Content>
