<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="open.aspx.cs" Inherits="coreERP.gl.accounts.open" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>


<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Open Accounting Period
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h3>Open Accounting Period</h3>
        <asp:Label runat="server" ID="lbl"></asp:Label> 
                   <table>
                        <tr>
                            <td colspan="2">
                               <a href="~/common/prof.aspx" runat="server" id="linkCompProf" 
                                target="_blank"><img id="Img1" src="~/images/comp_prof.jpg" 
                                     runat="server" alt="Company Profile"
                                     width="32" height="32" title="Company Profile" />
                               </a>
                               <a href="~/gl/accounts/default.aspx" runat="server" id="linkCA" 
                                target="_blank"><img id="Img2" src="../../images/chart_of_accounts/chart_of_accounts.jpg" 
                                     runat="server" alt="Chart of Accounts"
                                     width="32" height="32" title="Chart of Accunts" />
                               </a>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100px">Closed Period:</td>
                            <td>                                
                                <telerik:RadComboBox ID="cboPeriod" runat="server" Height="150px" Width=" 255px"
                                DropDownWidth=" 255px" EmptyMessage="Select Closed Period" HighlightTemplatedItems="true"
                                MarkFirstMatch="true" AutoCompleteSeparator="" 
                                 DataTextField="acct_period1" DataValueField="close_date">
                                <HeaderTemplate>
                                    <table style="width: 255px" cellspacing="0" cellpadding="0">
                                        <tr> 
                                            <td style="width: 100px;">
                                                Acct Period</td>
                                            <td style="width: 150px;">
                                                Period Date</td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 255px" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width: 100px;" >
                                                <%# Eval("acct_period1")%>
                                            </td>
                                            <td style="width: 150px;">
                                                <%# Eval("close_date")%>
                                            </td> 
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="cboPeriod"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly select the period to open.">
                                </asp:RequiredFieldValidator>                     
                            </td> 
                        </tr> 
                        <tr>
                            <td></td>
                            <td><asp:Button runat="server" ID="btnClose" Text="Open Period" Enabled="true"
                             OnClick="btnClose_Click" /></td>
                        </tr>
                   </table>
            <div id="divError" runat="server" style="visibility:hidden">
                <span id="spanError" class="error" runat="server"></span>
            </div>      
         
     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >                
     </telerik:RadAjaxPanel> 
      
</asp:Content>
