<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="GenericErrorPage.aspx.cs" Inherits="coreERP.errors.GenericErrorPage" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
  <div>
      <img src="/images/error.jpg" style="max-height:32px;max-width:32px;"/>
    <h3>
      An Error Occured Whilst Processing Your Request</h3>
    <br />
    Return to the <a href='/Default.aspx'> Default Page</a>
    <p>
      Error Message Details:<br />
      <asp:Label ID="exMessage" runat="server" Font-Bold="true" 
        Font-Size="Large" />
    </p>
    <h2>More Details:</h2>
    <pre>
      <asp:Label ID="exTrace" runat="server" Visible="false" />
    </pre>
    <asp:Panel ID="InnerErrorPanel" runat="server" Visible="false">
      <p>
        Inner Error Message:<br />
        <asp:Label ID="innerMessage" runat="server" Font-Bold="true" 
          Font-Size="Large" /><br />
      </p>
      <pre>
        <asp:Label ID="innerTrace" runat="server" />
      </pre>
    </asp:Panel>
  </div>
</asp:Content>
