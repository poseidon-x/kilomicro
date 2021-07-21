<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="processRefund.aspx.cs" Inherits="coreERP.ln.loans.processRefund" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Process Refunds
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" ID="dtDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Check Number
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox runat="server" ID="txtCheckNo"></telerik:RadTextBox>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Mode Of Payment
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboModeOfPayment" CssClass="inputControl"></telerik:RadComboBox>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Bank
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboBank" CssClass="inputControl"></telerik:RadComboBox>
            </div>
        </div>
    </div>
    <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="1000px" AllowMultiRowSelection="true" >
        <MasterTableView ShowFooter="true" DataKeyNames="multiPaymentClientID" >
            <Columns>
                <telerik:GridHyperLinkColumn DataNavigateUrlFields="multiPaymentClientID" Text="Apply To Loan"
                    DataNavigateUrlFormatString="/ln/cashier/applyChecks.aspx?mpcID={0}" HeaderText="Apply To Loan"></telerik:GridHyperLinkColumn>
                <telerik:GridBoundColumn DataField="multiPaymentClientID" HeaderText="accountNumber" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="invoiceDate" HeaderText="Check Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                <telerik:GridNumericColumn DataField="balance" HeaderText="Refund Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridClientSelectColumn></telerik:GridClientSelectColumn> 
             </Columns> 
        </MasterTableView>
        <ClientSettings><Selecting AllowRowSelect="true"  /></ClientSettings>
    </telerik:RadGrid>
    <telerik:RadButton Visible="true" runat="server" Text="Print Refunds" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton Visible="true" runat="server" Text="Post Refunds" ID="btnPost" OnClick="btnPost_Click"></telerik:RadButton>
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server" 
            BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" 
             />
</asp:Content>
