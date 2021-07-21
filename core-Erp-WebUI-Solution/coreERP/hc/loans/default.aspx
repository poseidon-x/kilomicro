<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.hc.loans._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Staff Loans Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Staff Loans Management</h3>
   <table>
       <tr>
           <td rowspan="20">
               <telerik:RadTreeView ID="tree" runat="server" OnNodeExpand="tree_NodeExpand"></telerik:RadTreeView>
           </td>
           <td style="height:30px">Surname</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" Width="150"></telerik:RadTextBox></td>
           <td style="height:30px">Other Names</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" Width="150"></telerik:RadTextBox></td> 
           <td style="height:30px">Staff No.</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccNo" runat="server" Width="150"></telerik:RadTextBox></td> 
           <td style="height:30px"><telerik:RadButton ID="btnFind" runat="server" Text="Find Staff" OnClick="btnFind_Click"></telerik:RadButton></td>
       </tr>
       <tr>
           <td colspan="8">
               <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="false" OnDetailTableDataBind="grid_DetailTableDataBind">
                   <MasterTableView DataKeyNames="staffID">
                       <Columns>
                           <telerik:GridHyperLinkColumn DataTextField="staffNo"  HeaderText="Staff No."
                               DataNavigateUrlFields="staffID" DataNavigateUrlFormatString="/hc/staff/staff.aspx?id={0}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn> 
                       </Columns>
                       <DetailTables>
                           <telerik:GridTableView  ShowFooter="true" DataKeyNames="staffID">  
                                <Columns>                             
                                   <telerik:GridHyperLinkColumn DataTextField="staffLoanID"  HeaderText="View Loan"
                                       DataNavigateUrlFields="staffLoanID" DataNavigateUrlFormatString="/hc/loans/loan.aspx?id={0}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridHyperLinkColumn DataTextField="staffLoanID"  HeaderText="Approve Loan"
                                       DataNavigateUrlFields="staffLoanID" DataNavigateUrlFormatString="/hc/loans/approve.aspx?id={0}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridHyperLinkColumn DataTextField="staffLoanID"  HeaderText="Post Loan"
                                       DataNavigateUrlFields="staffLoanID" DataNavigateUrlFormatString="/hc/loans/post.aspx?id={0}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridNumericColumn DataField="principal" HeaderText="Amount"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                   <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Principal Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                   <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Interest Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                </Columns>  
                           </telerik:GridTableView>                           
                       </DetailTables>
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
   </table>    
</asp:Content>
