<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="post.aspx.cs" Inherits="coreERP.hc.payroll.post" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Process Pay
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Post Employee Pay</h3>
    <nav>
        Pay Month: <telerik:RadComboBox runat="server" ID="cboPayCalendar"></telerik:RadComboBox>
        <telerik:RadButton runat="server" ID="btnProcessPay" Text="Post Payroll" OnClick="btnProcessPay_Click"></telerik:RadButton>
        <%--<telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
        <telerik:RadProgressArea ID="RadProgressArea1" runat="server" />--%>
    </nav>
</asp:Content>
