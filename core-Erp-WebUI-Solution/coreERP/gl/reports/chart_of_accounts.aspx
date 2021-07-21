<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="chart_of_accounts.aspx.cs" Inherits="coreERP.gl.reports.chart_of_accounts" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
::Chart of Accounts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Chart of Accounts</h3> 
    <CR:CrystalReportViewer ID="chart_of_accounts_report" runat="server" BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" />
</asp:Content>
