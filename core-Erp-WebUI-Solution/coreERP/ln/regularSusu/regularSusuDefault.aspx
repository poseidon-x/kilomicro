<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="regularSusuDefault.aspx.cs" Inherits="coreERP.ln.susu.regularSusuDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Normal Susu Accounts Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Normal Susu Accounts Management</h3>
   <table>
       <tr>
           <td rowspan="20">
               <telerik:RadTreeView ID="tree" runat="server" OnNodeExpand="tree_NodeExpand"></telerik:RadTreeView>
           </td>
           <td style="height:30px">Surname</td>
           <td style="height:30px">
               <telerik:RadAutoCompleteBox ID="txtSurname" runat="server" Width="150">
                    <WebServiceSettings Method="GetClientSurNames" Path="regularSusuDefault.aspx" />
                </telerik:RadAutoCompleteBox>
           </td>
           <td style="height:30px">Other Names</td>
           <td style="height:30px">
               <telerik:RadAutoCompleteBox ID="txtOthernames" runat="server" Width="150">
                    <WebServiceSettings Method="GetClientOtherNames" Path="regularSusuDefault.aspx" />
                </telerik:RadAutoCompleteBox>
           </td>
           <td style="height:30px">Account No.</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccNo" runat="server" Width="100"></telerik:RadTextBox></td>  
           <td style="height:30px">Staff ID</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtStaffID" runat="server" Width="100"></telerik:RadTextBox></td>  
           <td style="height:30px">Agent ID</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAgentID" runat="server" Width="100"></telerik:RadTextBox></td> 
           <td style="height:30px"><telerik:RadButton ID="btnFind" runat="server" Text="Find Client" OnClick="btnFind_Click"></telerik:RadButton></td>
       </tr>
       <tr>
           <td colspan="9">
               <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="False" OnDetailTableDataBind="grid_DetailTableDataBind">
                   <MasterTableView DataKeyNames="clientID">
                       <Columns>
                            <telerik:GridTemplateColumn>
                               <HeaderTemplate>
                                   S/N
                               </HeaderTemplate>
                               <ItemTemplate>
                                   <%# GetSerialNumber() %>
                               </ItemTemplate>
                           </telerik:GridTemplateColumn>
                           <telerik:GridImageColumn DataImageUrlFields="clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=64&h=64" HeaderText="Picture"
                                ImageWidth="64" ImageHeight="64" ItemStyle-Width="64">
<ItemStyle Width="64px"></ItemStyle>
                           </telerik:GridImageColumn>
                           <telerik:GridHyperLinkColumn DataTextField="accountNumber"  HeaderText="Account No."
                               DataNavigateUrlFields="clientID,categoryID" DataNavigateUrlFormatString="/ln/client/client.aspx?id={0}&amp;catID={1}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn> 
                           <telerik:GridBoundColumn DataField="branch.branchName" HeaderText="Branch"></telerik:GridBoundColumn> 
                       </Columns>
                       <DetailTables>
                           <telerik:GridTableView  ShowFooter="true" DataKeyNames="clientID">  
                                <Columns>                             
                                   <telerik:GridHyperLinkColumn DataTextField="regularSusuAccountNo"  HeaderText="Normal Susu Account"
                                       DataNavigateUrlFields="regularSusuAccountID,client.categoryID" DataNavigateUrlFormatString="/ln/regularSusu/regularSusuAccount.aspx?id={0}&amp;catID={1}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridHyperLinkColumn DataTextField="regularSusuAccountNo"  HeaderText="Verify Account"
                                       DataNavigateUrlFields="regularSusuAccountID,client.categoryID" DataNavigateUrlFormatString="/ln/susu/verify.aspx?id={0}&amp;catID={1}" Visible="false"></telerik:GridHyperLinkColumn>
                                   <telerik:GridHyperLinkColumn DataTextField="regularSusuAccountNo"  HeaderText="Approve Normal Susu Account"
                                       DataNavigateUrlFields="regularSusuAccountID,client.categoryID" DataNavigateUrlFormatString="/ln/regularSusu/regularSusuApprove.aspx?id={0}&amp;catID={1}"></telerik:GridHyperLinkColumn>
                                    <telerik:GridHyperLinkColumn DataTextField="regularSusuAccountNo"  HeaderText="Authorize for Payout"
                                       DataNavigateUrlFields="regularSusuAccountID,client.categoryID" DataNavigateUrlFormatString="/regularSusu/regularSusuAuthorize.aspx?id={0}&amp;catID={1}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridNumericColumn DataField="amountEntitled" HeaderText="Total Contribution"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                  <telerik:GridNumericColumn DataField="amountTaken" HeaderText="Total Withdrawal"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                   <telerik:GridNumericColumn DataField="netAmountEntitled" HeaderText="Outstanding Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                  <telerik:GridDateTimeColumn DataField="applicationDate" HeaderText="Date Applied"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn> 
                                  <telerik:GridDateTimeColumn DataField="approvalDate" HeaderText="Date Approved"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn> 
                                  <telerik:GridDateTimeColumn DataField="startDate" HeaderText="Date Started"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn> 
                                </Columns>  
                           </telerik:GridTableView>                           
                       </DetailTables>
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
   </table>    
</asp:Content>
