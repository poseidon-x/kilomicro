<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.ln.client._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Search For Client
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Search Client</h3>
   <table>
       <tr>
           <td rowspan="20">
               <telerik:RadTreeView ID="tree" runat="server" OnNodeExpand="tree_NodeExpand"></telerik:RadTreeView>
           </td>
           <td style="height:30px">Surname</td>
           <td style="height:30px">
               <telerik:RadAutoCompleteBox ID="txtSurname" runat="server" Width="150">
                    <WebServiceSettings Method="GetClientSurNames" Path="default.aspx" />
                </telerik:RadAutoCompleteBox>
           </td>
           <td style="height:30px">Other Names</td>
           <td style="height:30px">
               <telerik:RadAutoCompleteBox ID="txtOthernames" runat="server" Width="150">
                    <WebServiceSettings Method="GetClientOtherNames" Path="default.aspx" />
                </telerik:RadAutoCompleteBox>
           </td>
           <td style="height:30px">Account No.</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccNo" runat="server" Width="100"></telerik:RadTextBox></td> 
           <td style="height:30px"><telerik:RadButton ID="btnFind" runat="server" Text="Find Client" OnClick="btnFind_Click"></telerik:RadButton></td>
       </tr>
       <tr>
           <td colspan="7">
               <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    OnSortCommand="grid_SortCommand">
                   <MasterTableView>
                       <Columns>
                           <telerik:GridTemplateColumn>
                               <HeaderTemplate>
                                   S/N
                               </HeaderTemplate>
                               <ItemTemplate>
                                   <%# GetSerialNumber() %>
                               </ItemTemplate>
                           </telerik:GridTemplateColumn>
                           <telerik:GridImageColumn DataImageUrlFields="clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=64&h=64" HeaderText="P"
                                ImageWidth="64" ImageHeight="64" ItemStyle-Width="64"></telerik:GridImageColumn>
                           <telerik:GridHyperLinkColumn DataTextField="accountNumber"  HeaderText="Account No." SortExpression="accountNumber"
                               DataNavigateUrlFields="clientID,categoryID" DataNavigateUrlFormatString="/ln/client/client.aspx?id={0}&catID={1}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn> 
                           <telerik:GridBoundColumn DataField="branch.branchName" HeaderText="Branch"></telerik:GridBoundColumn> 
                       </Columns>
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
   </table>    
</asp:Content>
