﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="default2.aspx.cs" Inherits="coreERP.ln.loans._default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loans Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loans Management2</h3>
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
                           <telerik:GridHyperLinkColumn DataTextField="accountNumber"  HeaderText="Account No."
                               DataNavigateUrlFields="clientID" DataNavigateUrlFormatString="/ln/client/client.aspx?id={0}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn> 
                       </Columns>
                       <DetailTables>
                           <telerik:GridTableView  ShowFooter="true" DataKeyNames="clientID">  
                                <Columns>                             
                                   <telerik:GridHyperLinkColumn DataTextField="invoiceNo"  HeaderText="Loan No."
                                       DataNavigateUrlFields="invoiceLoanID,invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/invoiceloan.aspx?id={0}&ilm={1}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridHyperLinkColumn DataTextField="invoiceNo"  HeaderText="Print Disb. Sheet"
                                       DataNavigateUrlFields="invoiceLoanID,invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/printInvoiceLoan.aspx?id={0}&ilm={1}"></telerik:GridHyperLinkColumn>
                                    <telerik:GridHyperLinkColumn DataTextField="invoiceNo"  HeaderText="Approve Loan"
                                       DataNavigateUrlFields="invoiceLoanID,invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/approveIL.aspx?id={0}&ilm={1}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridHyperLinkColumn DataTextField="invoiceNo"  HeaderText="Disburse Loan"
                                       DataNavigateUrlFields="invoiceLoanID,invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/disburseIL.aspx?id={0}&ilm={1}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridNumericColumn DataField="invoiceAmount" HeaderText="Invoice Amount"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>   
                                   <telerik:GridNumericColumn DataField="proposedAmount" HeaderText="Net Amount"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>  
                                  <telerik:GridNumericColumn DataField="amountApproved" HeaderText="Amount Approved"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                  <telerik:GridNumericColumn DataField="amountDisbursed" HeaderText="Amount Disbursed"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                    <telerik:GridBoundColumn DataField="invoiceDescription" HeaderText="Invoice Description" ItemStyle-Width="350px"></telerik:GridBoundColumn>  
                                </Columns>  
                           </telerik:GridTableView>                           
                       </DetailTables>
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
   </table>    
</asp:Content>
