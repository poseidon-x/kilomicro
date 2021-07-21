<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="Http404ErrorPage.aspx.cs" Inherits="coreERP.errors.Http404ErrorPage" %><script runat="server">
  protected HttpException ex = null;

  protected void Page_Load(object sender, EventArgs e)
  {
    // Log the exception and notify system operators
    ex = new HttpException("HTTP 404:" + Request.ServerVariables["HTTP_REFERER"]);
    global_asax.LogException(ex);
  }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
  <div>
    <h2>Requested Menu not found on this application</h2>
    <br />
    The menu at <%=Request.ServerVariables["HTTP_REFERER"] %> is not defined. kindly check and try again
    <br />
    Or you can return to the <a href='/Default.aspx'> Home Page</a>
  </div>
</asp:Content>
