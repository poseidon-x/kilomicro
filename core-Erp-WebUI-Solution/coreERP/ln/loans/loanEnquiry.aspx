<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="loanEnquiry.aspx.cs" Inherits="coreERP.ln.loans.loanEnquiry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loan Enquiry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Enquiry</h3>
    <table>
        <tr>
            <td style="width:200px">
                Amount Requested
            </td>
            <td style="width:400px">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td style="width:200px">
                Date Loan Needed
            </td>
            <td style="width:400px">
                <telerik:RadDatePicker ID="dtLoanDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td style="width:200px">
                Grace Period
            </td>
            <td style="width:400px">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtGracePeriod" Value="0.0" runat="server" Height="18px" Width="56px" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
            &nbsp; Days</td>
        </tr>
        <tr>
            <td style="width:200px">
                Tenure of Loan
            </td>
            <td style="width:400px">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTenure" runat="server" Height="18px" Width="56px" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
            &nbsp; Months</td>
        </tr>
        <tr>
            <td style="width:200px">
                Interest Rate
            </td>
            <td style="width:400px">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtRate" runat="server" Height="18px" Width="56px" Value="6.5" Type="Percent"></telerik:RadNumericTextBox>
            &nbsp;per Month</td>
        </tr>
        <tr>
            <td style="width:200px">
                Interest Calculation Method
            </td>
            <td style="width:400px">
                <telerik:RadComboBox ID="cboInterestType" runat="server"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td style="width:200px">
                Repayment Mode
            </td>
            <td style="width:400px">
                <telerik:RadComboBox ID="cboRepaymentMode" runat="server"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <telerik:RadButton ID="btnCalculate" Text="Recalculate" runat="server" OnClick="btnCalculate_Click"></telerik:RadButton>
                <telerik:RadButton ID="btnTransfer" Text="Transfer to Application Entry" runat="server" OnClick="btnTransfer_Click"></telerik:RadButton>
            </td> 
        </tr>
    </table>
    <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="800px">
        <MasterTableView ShowFooter="true">
            <Columns>
                <telerik:GridBoundColumn DataField="repaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                <telerik:GridNumericColumn DataField="principalPayment" HeaderText="Principal Payment" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="interestPayment" HeaderText="Interest Payment" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridCalculatedColumn Expression="principalPayment+interestPayment" HeaderText="Total Payment" Aggregate="Sum" DataType="System.Double" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridCalculatedColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
