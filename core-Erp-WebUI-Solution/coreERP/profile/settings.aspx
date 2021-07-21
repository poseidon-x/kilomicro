<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="settings.aspx.cs" Inherits="coreERP.profile.settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <table>
        <tr>
            <td>
                <h2 runat="server" id="h2"></h2>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ChangePassword runat="server" ID="chPassword" SuccessTextStyle-ForeColor="Green"  SuccessPageUrl="~"
                     OnChangingPassword="chPassword_ChangingPassword" OnChangedPassword="chPassword_ChangedPassword">
                </asp:ChangePassword>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblMessage"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
