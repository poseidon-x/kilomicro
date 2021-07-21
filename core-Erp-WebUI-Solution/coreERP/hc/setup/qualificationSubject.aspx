<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="qualificationSubject.aspx.cs" Inherits="coreERP.hc.setup.qualificationSubject" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Qualication Areas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <script src="../../scripts/app/hc/qualificationSubject.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Qualication Areas</h3>  
    <div id="setupGrid" ></div>
</asp:Content>