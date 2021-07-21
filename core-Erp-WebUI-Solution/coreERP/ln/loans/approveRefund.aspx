<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approveRefund.aspx.cs" Inherits="coreERP.ln.loans.approveRefund" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Approve Refunds
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="1000px" AllowMultiRowSelection="true" >
        <MasterTableView ShowFooter="true" DataKeyNames="multiPaymentClientID" >
            <Columns>
                <telerik:GridBoundColumn DataField="multiPaymentClientID" HeaderText="accountNumber" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="invoiceDate" HeaderText="Check Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                <telerik:GridNumericColumn DataField="amount" HeaderText="Refund Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridClientSelectColumn></telerik:GridClientSelectColumn>  
             </Columns> 
        </MasterTableView>
        <ClientSettings><Selecting AllowRowSelect="true"  /></ClientSettings>
    </telerik:RadGrid>
    <br />
    <telerik:RadButton Visible="true" runat="server" Text="Approve Selected Refunds" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton Visible="false" runat="server" ID="btnCancel" Text="Cancel Selected" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
