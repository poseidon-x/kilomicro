<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="payrollLoanEnquiry3.aspx.cs" Inherits="coreERP.ln.loans.payrollLoanEnquiry3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Payroll Loan Calculator
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Payroll Loan Calculator</h3>
    <table>
        <tr>
            <td style="width:200px">
                Principal
            </td>
            <td style="width:200px">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtPrinc" NumberFormat-DecimalDigits="2"
                     AutoPostBack="true" OnTextChanged="txtPrinc_TextChanged"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td>
                Tenure
            </td>
            <td>
                <telerik:RadComboBox runat="server" ID="cboTenure" AutoPostBack="true"
                     OnTextChanged="txtPrinc_TextChanged" DropDownAutoWidth="Enabled"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Processing Fee
            </td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtProcFee" NumberFormat-DecimalDigits="2"
                     Enabled="false"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td>
                Net (Take Away)
            </td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtNet" NumberFormat-DecimalDigits="2"
                     Enabled="false"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td>
                Interest
            </td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtInt" NumberFormat-DecimalDigits="2"
                     Enabled="false"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td>
                Total
            </td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtTotal" NumberFormat-DecimalDigits="2"
                     Enabled="false"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td>
                Rounded-up Monthly Deduction
            </td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtMD" NumberFormat-DecimalDigits="2"
                     Enabled="false"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td>
                Total Deduction
            </td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtTD" NumberFormat-DecimalDigits="2"
                     Enabled="false"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td>
                Check
            </td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtCheck" NumberFormat-DecimalDigits="2"
                     Enabled="false"></telerik:RadNumericTextBox>
            </td>
        </tr>
    </table>
</asp:Content>
