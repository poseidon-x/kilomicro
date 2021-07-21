<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.ln.investment._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Manage Company Investments
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
   <h3>Manage Company Investments</h3>
   <table>
       <tr>
           <td rowspan="20">
               <telerik:RadTreeView ID="tree" runat="server"></telerik:RadTreeView>
           </td>
           <td style="height:30px">Surname</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" Width="150"></telerik:RadTextBox></td>
           <td style="height:30px">Other Names</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" Width="150"></telerik:RadTextBox></td> 
           <td style="height:30px">Account No.</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccNo" runat="server" Width="100"></telerik:RadTextBox></td> 
           <td style="height:30px"><telerik:RadButton ID="btnFind" runat="server" Text="Find Client" OnClick="btnFind_Click"></telerik:RadButton></td>
       </tr>
       <tr>
           <td colspan="7">
               <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="false" OnDetailTableDataBind="grid_DetailTableDataBind">
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
                           <telerik:GridHyperLinkColumn DataTextField="accountNumber"  HeaderText="Account No."
                               DataNavigateUrlFields="clientID" DataNavigateUrlFormatString="/ln/client/client.aspx?id={0}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn> 
                       </Columns>
                       <DetailTables>
                           <telerik:GridTableView  ShowFooter="true" DataKeyNames="client.clientID">  
                                <Columns>                             
                                   <telerik:GridHyperLinkColumn DataTextField="investmentNo"  HeaderText="Investment No."
                                       DataNavigateUrlFields="investmentID" DataNavigateUrlFormatString="/ln/investment/investment.aspx?id={0}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridNumericColumn DataField="amountInvested" HeaderText="Amount Invested"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                  <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Princ. Balance"  DataFormatString="{0:#,###.00}" ></telerik:GridNumericColumn>
                                  <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Int. Balance"  DataFormatString="{0:#,###.00}" ></telerik:GridNumericColumn> 
                                </Columns>  
                           </telerik:GridTableView>                           
                       </DetailTables>
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
   </table>    
</asp:Content>
