<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="default3.aspx.cs" Inherits="coreERP.ln.loans._default3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loans Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loans Management3</h3>
    <table>
        <tr>
            <td rowspan="20">
                <telerik:radtreeview id="tree" runat="server"></telerik:radtreeview>
            </td>
            <td style="height: 30px">Surname</td>
            <td style="height: 30px">
                <telerik:radtextbox wrappercssclass="inputControl" id="txtSurname" runat="server" width="150"></telerik:radtextbox>
            </td>
            <td style="height: 30px">Other Names</td>
            <td style="height: 30px">
                <telerik:radtextbox wrappercssclass="inputControl" id="txtOtherNames" runat="server" width="150"></telerik:radtextbox>
            </td>
            <td style="height: 30px">Account No.</td>
            <td style="height: 30px">
                <telerik:radtextbox wrappercssclass="inputControl" id="txtAccNo" runat="server" width="100"></telerik:radtextbox>
            </td>
            <td style="height: 30px">
                <telerik:radbutton id="btnFind" runat="server" text="Find Client" onclick="btnFind_Click"></telerik:radbutton>
            </td>
        </tr>
        <tr>
            <td colspan="7">
                <telerik:radgrid id="grid" runat="server" autogeneratecolumns="false" ondetailtabledatabind="grid_DetailTableDataBind" showfooter="true"
                     OnItemCommand="grid_ItemCommand">
                   <MasterTableView DataKeyNames="clientID">
                       <Columns>
                           <telerik:GridHyperLinkColumn DataTextField="accountNumber"  HeaderText="Account No."
                               DataNavigateUrlFields="clientID" DataNavigateUrlFormatString="/ln/client/client.aspx?id={0}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn> 
                       </Columns>
                       <DetailTables>
                           <telerik:GridTableView  ShowFooter="true" DataKeyNames="invoiceLoanMasterID">  
                               <DetailTables>
                                   <telerik:GridTableView  ShowFooter="true" DataKeyNames="invoiceLoanID"> 
                                       <Columns>
                                            <telerik:GridHyperLinkColumn DataTextField="invoiceNo"  HeaderText="Invoice No."
                                                DataNavigateUrlFields="invoiceLoanID,invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/invoiceloan.aspx?ilm={1}"></telerik:GridHyperLinkColumn>
                                            <telerik:GridHyperLinkColumn DataTextField="invoiceNo"  HeaderText="Print Disb. Sheet"
                                                DataNavigateUrlFields="invoiceLoanID,invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/printInvoiceLoan.aspx?ilm={1}"></telerik:GridHyperLinkColumn>
                                            <telerik:GridHyperLinkColumn DataTextField="invoiceNo"  HeaderText="Approve Loan"
                                                DataNavigateUrlFields="invoiceLoanID,invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/approveIL.aspx?ilm={1}"></telerik:GridHyperLinkColumn>
                                            <telerik:GridHyperLinkColumn DataTextField="invoiceNo"  HeaderText="Disburse Loan"
                                                DataNavigateUrlFields="invoiceLoanID,invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/disburseIL.aspx?ilm={1}"></telerik:GridHyperLinkColumn>
                                           <telerik:GridNumericColumn DataField="invoiceAmount" HeaderText="Invoice Amount"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>   
                                           <telerik:GridNumericColumn DataField="proposedAmount" HeaderText="Net Amount"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>  
                                            <telerik:GridNumericColumn DataField="amountApproved" HeaderText="Amount Approved"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                            <telerik:GridNumericColumn DataField="amountDisbursed" HeaderText="Amount Disbursed"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                                           <telerik:GridBoundColumn DataField="invoiceDescription" HeaderText="Invoice Description" ItemStyle-Width="350px"></telerik:GridBoundColumn>  
                                          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg"  Visible="False"
                                            ConfirmText="Are you sure you want to delete the selected invoice loan?">
                                          </telerik:GridButtonColumn>
                                        </Columns>
                                   </telerik:GridTableView>
                               </DetailTables>
                                <Columns>                                                                 
                                   <telerik:GridHyperLinkColumn DataTextField="client.accountNumber"  HeaderText="Loan No."
                                       DataNavigateUrlFields="invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/invoiceloan.aspx?ilm={0}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridHyperLinkColumn DataTextField="client.accountNumber"  HeaderText="Print Disb. Sheet"
                                       DataNavigateUrlFields="invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/printInvoiceLoan.aspx?ilm={0}"></telerik:GridHyperLinkColumn>
                                    <telerik:GridHyperLinkColumn DataTextField="client.accountNumber"  HeaderText="Approve Loan"
                                       DataNavigateUrlFields="invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/approveIL.aspx?ilm={0}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridHyperLinkColumn DataTextField="client.accountNumber"  HeaderText="Disburse Loan"
                                       DataNavigateUrlFields="invoiceLoanMasterID" DataNavigateUrlFormatString="/ln/loans/disburseIL.aspx?ilm={0}"></telerik:GridHyperLinkColumn>
                                   <telerik:GridDropDownColumn DataField="supplierID" ListValueField="supplierID" ListTextField="supplierName"
                                        DataSourceID="EntityDataSource2" HeaderText="Supplier Name"></telerik:GridDropDownColumn>
                                    <telerik:GridBoundColumn DataField="invoicedate" HeaderText="Application Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                                    <telerik:GridCheckBoxColumn DataField="approved" HeaderText="Approved"></telerik:GridCheckBoxColumn>
                                    <telerik:GridCheckBoxColumn DataField="disbursed" HeaderText="Disbursed"></telerik:GridCheckBoxColumn>
                                    <telerik:GridBoundColumn DataField="approvalDate" HeaderText="Approval Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                                </Columns>  
                           </telerik:GridTableView>                           
                       </DetailTables>
                   </MasterTableView>
               </telerik:radgrid>
            </td>
        </tr>
    </table>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server"
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities"
        EnableDelete="True" EnableInsert="True" EnableUpdate="True"
        EntitySetName="suppliers">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server"
        ConnectionString="name=reportEntities" DefaultContainerName="reportEntities"
        EnableDelete="True" EnableInsert="True" EnableUpdate="True"
        EntitySetName="vwClients">
    </ef:EntityDataSource>
</asp:Content>
