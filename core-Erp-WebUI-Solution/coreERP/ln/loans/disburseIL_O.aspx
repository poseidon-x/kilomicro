<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="disburseIL_O.aspx.cs" Inherits="coreERP.ln.loans.disburseIL_O" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Invoice Loan
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Invoice Loan Disbursement</h3>
     <table>
            <tr>
                <td style="width:150px">Account No.</td>
                <td style="width:400px">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" Width="100"></telerik:RadTextBox>
                </td>
                <td rowspan="7">
                    <telerik:RadRotator Width="177" Height="123" runat="server" ID="rotator1" ItemHeight="123" ItemWidth="177" FrameDuration="30000"></telerik:RadRotator>
                </td>
            </tr>
            <tr>
                <td>Surname</td>
                <td>
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" Width="200"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Other Names</td>
                <td>
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" Width="200"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top"></td>
                <td >
                    <telerik:RadButton ID="btnFind" runat="server" Text="Find Client" ></telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td>
                    Client Selected
                </td>
                <td>
                    <telerik:RadComboBox ID="cboClient" Enabled="false" runat="server" AutoPostBack="true" Width="321px"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    
        <table>
            <tr>
                <td style="width:150px">
                    Invoice Amount
                </td>
                <td style="width:200px">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" Enabled="false" ID="txtAmount" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged"> </telerik:RadNumericTextBox>
                </td>
                <td style="width:150px">
                    Disbursement Date
                </td>
                <td style="width:200px">
                    <telerik:RadDatePicker ID="dtAppDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                </td>
            </tr>
            <tr> 
                <td style="width:150px"> 
                    Payment Type
                </td>
                <td style="width:200px">
                    <telerik:RadComboBox id="cboPaymentType" runat="server"></telerik:RadComboBox>
                </td>
                <td style="width:150px"> 
                    Check No.
                </td>
                <td style="width:200px">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" runat="server" Width="100px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr> 
                <td>
                    Bank
                </td>
                <td>
                    <telerik:RadComboBox ID="cboBank" runat="server"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="width:150px">
                    Withholding Tax
                </td>
                <td style="width:200px">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" Enabled="false" ID="txtWith" runat="server" Width="100px"></telerik:RadNumericTextBox>
                </td>
                <td style="width:150px"> 
                    Invoice Description
                </td>
                <td style="width:200px">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtDesc" runat="server" TextMode="MultiLine" Enabled="false" Rows="3" Width="300px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width:150px">
                    Invoice No
                </td>
                <td style="width:200px">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtInvoiceNo" runat="server" Width="100px" ReadOnly="true"></telerik:RadTextBox>
                </td> 
                <td style="width:150px">
                    Supplier
                </td>
                <td style="width:200px">
                    <telerik:RadComboBox ID="cboSupplier" runat="server" Width="321px"
                        DropDownWidth="355px" EmptyMessage="Select a Supplier" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" Enabled="false"></telerik:RadComboBox>
                </td> 
            </tr>
            <tr>
                <td style="width:150px">
                    Disb Rate
                </td>
                <td style="width:200px">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtDisbRate" Enabled="false" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="txtDisbRate_TextChanged"></telerik:RadNumericTextBox>
                </td>
                <td style="width:150px">
                    Disbursement Amount
                </td>
                <td style="width:200px">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtDisbAmt" Enabled="false" runat="server" Width="100px"></telerik:RadNumericTextBox>
                </td>
            </tr> 
            <tr>
                <td style="width:150px">
                    Add Processing Fee to Application
                </td>
                <td style="width:200px">
                    <asp:CheckBox runat="server" ID="chkAddFees" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="3"><telerik:RadButton ID="btnSave" runat="server" Text="Save Invoice Loan" OnClick="btnSave_Click"></telerik:RadButton></td>
            </tr>
        </table>
</asp:Content>
