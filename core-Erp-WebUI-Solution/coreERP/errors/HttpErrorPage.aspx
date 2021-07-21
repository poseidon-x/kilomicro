<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="HttpErrorPage.aspx.cs" Inherits="coreERP.errors.HttpErrorPage" %>
<script runat="server">
  protected HttpException ex = null;

  protected void Page_Load(object sender, EventArgs e)
  {
      try
      {
          ex = (HttpException)Server.GetLastError();

          int httpCode = ex.GetHttpCode();

          // Filter for Error Codes and set text
          if (httpCode >= 400 && httpCode < 500)
              ex = new HttpException
                  (httpCode, "Requested menu not found.", ex);
          else if (httpCode > 499)
              ex = new HttpException
                  (ex.ErrorCode, "There is a problem on the server. Kindly inform your administrator.", ex);
          else
              ex = new HttpException
                  (httpCode, "An unexpected error has occured.", ex);

          global_asax.LogException(ex);

          // Fill the page fields
          exMessage.Text = ex.Message;
          exTrace.Text = ex.StackTrace;

          // Show Inner Exception fields for local access
          if (ex.InnerException != null)
          {
              innerTrace.Text = ex.InnerException.StackTrace;
              InnerErrorPanel.Visible = Request.IsLocal;
              innerMessage.Text = string.Format("HTTP {0}: {1}",
                httpCode, ex.InnerException.Message);
          }
          // Show Trace for local access
          exTrace.Visible = User.IsInRole("admin");

          // Clear the error from the server
          Server.ClearError();
      }
      catch (Exception) { }
  }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div>
        <h2>
          An error has occured</h2>
        <asp:Panel ID="InnerErrorPanel" runat="server" Visible="false">
          <asp:Label ID="innerMessage" runat="server" Font-Bold="true" 
            Font-Size="Large" /><br />
          <pre>
            <asp:Label ID="innerTrace" runat="server" />
          </pre>
        </asp:Panel>
        Error Message:<br />
        <asp:Label ID="exMessage" runat="server" Font-Bold="true" 
          Font-Size="Large" />
        <pre>
          <asp:Label ID="exTrace" runat="server" Visible="false" />
        </pre>
        <br />
        Return to the <a href='/Default.aspx'>Default Page</a>
  </div>
</asp:Content>
