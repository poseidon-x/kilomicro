<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.ln.asset._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Search For Asset
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Search For Asset</h3>
   <table>
       <tr>
           <td rowspan="20">
               <telerik:RadTreeView ID="tree" runat="server"></telerik:RadTreeView>
           </td>
           <td style="height:30px">Description</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtDesc" runat="server" Width="150"></telerik:RadTextBox></td>
          <td style="height:30px">Asset Tag.</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccNo" runat="server" Width="100"></telerik:RadTextBox></td> 
           <td style="height:30px"><telerik:RadButton ID="btnFind" runat="server" Text="Find Asset" OnClick="btnFind_Click"></telerik:RadButton></td>
       </tr>
       <tr>
           <td colspan="7">
               <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="false">
                   <MasterTableView>
                       <Columns>
                           <telerik:GridHyperLinkColumn DataTextField="assetTag"  HeaderText="Asset Tag"
                               DataNavigateUrlFields="assetID" DataNavigateUrlFormatString="/fa/assets/asset.aspx?id={0}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="assetDescription" HeaderText="Description"></telerik:GridBoundColumn>
                       </Columns>
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
   </table>    
</asp:Content>
