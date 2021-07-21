<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="reverse.aspx.cs" Inherits="coreERP.ln.cashier.reverse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Reverse Repayment
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Reverse Repayment</h3>
     <table>
        <tr>
            <td>
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadComboBox ID="cboLoan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboLoan_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select a Loan" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
         <tr>
             <td>Loan Disbursements that can be reversed</td>
         </tr>
       <tr>
           <td>
               <telerik:RadGrid ID="grid2" runat="server" AutoGenerateColumns="false">
                   <MasterTableView DataKeyNames="loanTranchID">
                       <Columns>                                        
                            <telerik:GridHyperLinkColumn DataTextField="disbursementDate"  HeaderText="Reverse Disbursement"  DataTextFormatString="{0:dd-MMM-yyyy}"
                                DataNavigateUrlFields="loanTranchID" DataNavigateUrlFormatString="/ln/cashier/reversedb.aspx?id={0}"></telerik:GridHyperLinkColumn>  
                            <telerik:GridNumericColumn DataField="checkNumber" HeaderText="Check No."></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="amountDisbursed" HeaderText="Amount Disbursed"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
         <tr>
             <td>Loan Repayments that can be reversed</td>
         </tr>
       <tr>
           <td>
               <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="false">
                   <MasterTableView DataKeyNames="loanRepaymentID">
                       <Columns>                                        
                            <telerik:GridHyperLinkColumn DataTextField="repaymentDate"  HeaderText="Reverse Repayment"  DataTextFormatString="{0:dd-MMM-yyyy}"
                                DataNavigateUrlFields="loanRepaymentID" DataNavigateUrlFormatString="/ln/cashier/reverserp.aspx?id={0}"></telerik:GridHyperLinkColumn>  
                            <telerik:GridNumericColumn DataField="repaymentType.repaymentTypeName" HeaderText="Repayment Type"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="checkNo" HeaderText="Check No."></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="amountPaid" HeaderText="Amount Paid"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr> 
        <tr>
            <td>Loan Additional Interests that can be reversed</td>
        </tr>
       <tr>
           <td>
               <telerik:RadGrid ID="grid3" runat="server" AutoGenerateColumns="false">
                   <MasterTableView DataKeyNames="loanPenaltyID">
                       <Columns>                                        
                            <telerik:GridHyperLinkColumn DataTextField="penaltyDate"  HeaderText="Reverse Interest"  DataTextFormatString="{0:dd-MMM-yyyy}"
                                DataNavigateUrlFields="loanPenaltyID" DataNavigateUrlFormatString="/ln/cashier/reverseai.aspx?id={0}"></telerik:GridHyperLinkColumn>  
                            <telerik:GridNumericColumn DataField="penaltyFee" HeaderText="Interest Amount"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
        <tr>
            <td>Loan Processing Fees that can be reversed</td>
        </tr>
       <tr>
           <td>
               <telerik:RadGrid ID="grid4" runat="server" AutoGenerateColumns="false">
                   <MasterTableView DataKeyNames="loanFeeID">
                       <Columns>                                        
                            <telerik:GridHyperLinkColumn DataTextField="feeDate"  HeaderText="Reverse Fee"  DataTextFormatString="{0:dd-MMM-yyyy}"
                                DataNavigateUrlFields="loanFeeID" DataNavigateUrlFormatString="/ln/cashier/reversepf.aspx?id={0}"></telerik:GridHyperLinkColumn>  
                            <telerik:GridNumericColumn DataField="feeAmount" HeaderText="Fee Amount"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
        <tr>
            <td>Loan Insurance Fees that can be reversed</td>
        </tr>
       <tr>
           <td>
               <telerik:RadGrid ID="grid5" runat="server" AutoGenerateColumns="false">
                   <MasterTableView DataKeyNames="loanInsuranceID">
                       <Columns>                                        
                            <telerik:GridHyperLinkColumn DataTextField="insuranceDate"  HeaderText="Reverse Fee"  DataTextFormatString="{0:dd-MMM-yyyy}"
                                DataNavigateUrlFields="loanInsuranceID" DataNavigateUrlFormatString="/ln/cashier/reverseInsurance.aspx?id={0}"></telerik:GridHyperLinkColumn>  
                            <telerik:GridNumericColumn DataField="amount" HeaderText="Insurance Amount"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
    </table>
</asp:Content>
