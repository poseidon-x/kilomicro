<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="coreERP.cu.report.register" %>


<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>



<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=14.1.20.618, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Membership Register
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Union Membership Register Report</h3> 
    <telerik:ReportViewer ID="rvw" runat="server" Width="100%" Height="724px" ZoomMode="FullPage"
         ></telerik:ReportViewer>

    <script  type="text/javascript">
        $(document).load(function () {
            $('<%= rvw.ClientID %>').height(window.innerHeight * 0.65);
        });
    </script>
</asp:Content>
