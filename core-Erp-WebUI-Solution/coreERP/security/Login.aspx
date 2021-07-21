<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Login.aspx.cs" Inherits="coreERP.Login" %>

<%@ Import Namespace="coreLogic" %>  

<!DOCTYPE>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="Cache-control" content="no-cache" />
    <title>
   Login |&nbsp;<%= coreERP.code.Settings.companyName%>|&nbsp;coreERP
    </title>
    <link href="~/styles/page.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="off"> 
        <div style="left:40%;top:30%;position:absolute;">
            <img src="/Content/images/coreerp-logo.png" width="236"  height="60"  style="max-height:60px;max-width:256px;"/>
	        <asp:Login ID="Login1" runat="server" OnLoggedIn="Login1_LoggedIn"
                LoginButtonText="Enter System" DisplayRememberMe="False" RememberMeText="" style="text-align: left">
                <CheckBoxStyle Font-Bold="True" Font-Names="Calibri Light" />
                <InstructionTextStyle Font-Italic="True" Font-Names="Calibri Light" />
                <LabelStyle Font-Bold="True" Font-Names="Calibri Light" />
                <LoginButtonStyle BackColor="#CC0000" BorderStyle="None" Font-Bold="True" Font-Names="Calibri Light" ForeColor="White" />
                <TitleTextStyle Font-Names="Calibri" Font-Size="XX-Large" Font-Bold="true" Font-Underline="false" />
            </asp:Login>  
	    </div>  
    </form>
</body>
</html>
