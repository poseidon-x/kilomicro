<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="invalid_lic.aspx.cs" Inherits="coreERP.security.invalid_lic" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Welcome</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <center>
        <h3>Invalid License</h3>
        <p> 
            The company in the profile has no valid license. Contact the system owners for a license.<br />
            Failure Reason: <b><%=Request.Params["licSt"] %></b>
        </p>
    </center>
</asp:Content>