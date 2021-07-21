<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="defaultClosure.aspx.cs" Inherits="coreERP.ln.loans.defaultClosure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Close Loan Accounts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
   <h3>Close Loan Accounts</h3>
   <table>
       <tr> 
           <td style="height:30px">Surname</td>
           <td style="height:30px">
               <telerik:RadTextBox ID="txtSurname" runat="server" Width="150"> 
                </telerik:RadTextBox>
           </td> 
           <td style="height:30px">Other Names</td>
           <td style="height:30px">
               <telerik:RadTextBox ID="txtOthernames" runat="server" Width="150"> 
                </telerik:RadTextBox>
           </td>
           <td style="height:30px">Account No.</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccNo" runat="server" Width="100"></telerik:RadTextBox></td>  
       </tr>
       <tr>
           <td style="height:30px">Staff ID</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtStaffID" runat="server" Width="100"></telerik:RadTextBox></td>  
           <td style="height:30px">Agent ID</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAgentID" runat="server" Width="100"></telerik:RadTextBox></td> 
           <td style="height:30px"><telerik:RadButton ID="btnFind" runat="server" Text="Find Client" OnClick="btnFind_Click"></telerik:RadButton></td>
       </tr>
       <tr>
           <td colspan="9">
               <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="false">
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
                           <telerik:GridImageColumn DataImageUrlFields="clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=64&h=64" HeaderText="P"
                                ImageWidth="64" ImageHeight="64" ItemStyle-Width="64"></telerik:GridImageColumn>
                           <telerik:GridHyperLinkColumn DataTextField="accountNumber"  HeaderText="Account No."
                               DataNavigateUrlFields="clientID" DataNavigateUrlFormatString="/ln/loans/defaultLoanClosure.aspx?clientID={0}"></telerik:GridHyperLinkColumn>
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
