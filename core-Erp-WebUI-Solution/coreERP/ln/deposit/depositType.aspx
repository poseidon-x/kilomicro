<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="depositType.aspx.cs" Inherits="coreERP.ln.setup.depositType" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
 Manage Client Investement Product
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <script src="../../scripts/app/Loans/Deposit/depositType.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Manage Client Investement Product</h3>  
    <div id="setupGrid" ></div>
</asp:Content>