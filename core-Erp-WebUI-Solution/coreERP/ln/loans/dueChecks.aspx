<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="dueChecks.aspx.cs" Inherits="coreERP.ln.loans.dueChecks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Apply Checks</h3>
    <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="1000px" AllowMultiRowSelection="true" >
        <MasterTableView ShowFooter="true" DataKeyNames="loanCheckID" >
            <Columns>
                <telerik:GridHyperLinkColumn DataNavigateUrlFields="loanCheckID" DataTextField="checkNumber"
                    DataNavigateUrlFormatString="/ln/loans/dueCheck.aspx?id={0}" HeaderText="Apply Check To Loan"></telerik:GridHyperLinkColumn>
                <telerik:GridBoundColumn DataField="loanCheckID" HeaderText="accountNumber" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="accountNumber" HeaderText="Acc. Num"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="checkDate" HeaderText="Check Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                <telerik:GridNumericColumn DataField="checkAmount" HeaderText="Check Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
             </Columns> 
        </MasterTableView>
        <ClientSettings><Selecting AllowRowSelect="true"  /></ClientSettings>
    </telerik:RadGrid>
    <br />
    <telerik:RadButton Visible="false" runat="server" Text="Apply Selected Checks" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton Visible="false" runat="server" ID="btnCancel" Text="Cancel Selected" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
