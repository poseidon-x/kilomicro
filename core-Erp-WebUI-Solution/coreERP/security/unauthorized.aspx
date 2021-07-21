<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="unauthorized.aspx.cs" Inherits="coreERP.security.unauthorized" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Welcome</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <center>
        <h3>UnAuthorized</h3>
        <p> 
            Sorry, you are not authorized to access this module.<br />
            For any clarification consult your administrator.<br />
            You can click <a href="Login.aspx">Login</a> to sign in as a new user. <br/>
            Or Go <a href='<%= Request.QueryString["url"]%>'>back</a> to the denied module.
        </p>
    </center>
</asp:Content>