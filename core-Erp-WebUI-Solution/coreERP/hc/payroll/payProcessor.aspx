<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="payProcessor.aspx.cs" Inherits="coreERP.hc.payroll.payProcessor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Process Pay
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Process Employee Pay</h3>
    <nav>
        Pay Month: <telerik:RadComboBox runat="server" ID="cboPayCalendar"></telerik:RadComboBox>
        <telerik:RadButton runat="server" ID="btnProcessPay" Text="Process Pay" OnClick="btnProcessPay_Click"></telerik:RadButton> 
    </nav>
</asp:Content>
