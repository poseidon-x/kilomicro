<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="defaultLoanClosure.aspx.cs" Inherits="coreERP.ln.loans.defaultLoanClosure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Close Loan Accounts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
   <h3>Close Loan Accounts</h3> 
        <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="false">
            <MasterTableView DataKeyNames="loanID"> 
                <Columns>                             
                    <telerik:GridHyperLinkColumn DataTextField="loanNo"  HeaderText="Loan No."
                        DataNavigateUrlFields="loanID" DataNavigateUrlFormatString="/ln/loans/loanClosure.aspx?id={0}"></telerik:GridHyperLinkColumn>                         
                    <telerik:GridNumericColumn DataField="amountRequested" HeaderText="Amount Requested"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                    <telerik:GridNumericColumn DataField="amountApproved" HeaderText="Amount Approved"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                    <telerik:GridNumericColumn DataField="amountDisbursed" HeaderText="Amount Disbursed"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                    <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Principal Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                    <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Interest Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                    <telerik:GridNumericColumn DataField="totalBalance" HeaderText="Total Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                </Columns>  
            </MasterTableView>
        </telerik:RadGrid> 
</asp:Content>
