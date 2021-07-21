<%@ Page Title="" Language="C#" MasterPageFile="~/setupPage.Master" AutoEventWireup="true" CodeBehind="chapter.aspx.cs" Inherits="coreERP.cu.edit.chapter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Credit Union Chapters Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server"> 
    <script src="../../scripts/app/CreditUnion/chapter.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3 class="pageBanner">Credit Union Chapters Management</h3>  
    <div id="setupGrid" ></div>
</asp:Content>
