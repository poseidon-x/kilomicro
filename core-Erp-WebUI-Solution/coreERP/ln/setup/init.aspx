<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="init.aspx.cs" Inherits="coreERP.ln.setup.init" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Initialize Database
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Intialize Database</h3>
    <asp:CheckBox ID="chkAsset" Text="Asset" runat="server" />
    <br />
    <asp:CheckBox ID="chkStaff" Text="Staff" runat="server" />
    <br />
    <asp:CheckBox ID="chkLoan" Text="Loans" runat="server" />
    <br />
    <asp:CheckBox ID="chkClient" Text="Clients" runat="server" />
    <br />
    <asp:CheckBox ID="chkJournal" Text="Journals + PC + Vocuhers" runat="server" />
    <br />
    <asp:CheckBox ID="chkAccount" Text="Accounts" runat="server" />
    <br />
    <telerik:RadButton runat="server" ID="btnInit" Text="Initialize Database" OnClick="btnInit_Click"></telerik:RadButton>
</asp:Content>
