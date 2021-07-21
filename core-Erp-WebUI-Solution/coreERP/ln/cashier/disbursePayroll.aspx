<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="disbursePayroll.aspx.cs" Inherits="coreERP.ln.cashier.disbursePayroll" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Payroll Disbursement</h3>
    <table> 
        <tr>
            <td>Closing Date</td>
            <td><telerik:RadDatePicker ID="dtDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker></td>
             <td>Payment Mode</td>
            <td colspan="2">
                <telerik:RadComboBox ID="cboPaymentMode" runat="server" >
                    <Items>
                        <telerik:RadComboBoxItem Text="Cash" Value="1" />
                        <telerik:RadComboBoxItem Text="Check" Value="2" />
                        <telerik:RadComboBoxItem Text="BankTransfer" Value="3" />
                    </Items>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Cheque No</td>
            <td><telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtCheckNo"></telerik:RadTextBox></td>
            <td>Bank</td>
            <td><telerik:RadComboBox runat="server" ID="cboBank" Width="300px"></telerik:RadComboBox></td>
        </tr>
    </table>
    <telerik:RadGrid ID="gridLoans" runat="server" AutoGenerateColumns="false" Width="800px" MasterTableView-AllowSorting="true"
         >
        <MasterTableView ShowFooter="true" DataKeyNames="loanID" >
            <Columns> 
                <telerik:GridBoundColumn DataField="loanID" HeaderText="accountNumber" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="loanNo" HeaderText="Loan ID"></telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="approvalDate" HeaderText="Approval Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="amountApproved" HeaderText="Amount Approved" DataFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn>
                    <ItemTemplate>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" runat="server"></telerik:RadTextBox>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <span>Cheque No.</span>
                    </HeaderTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn>
                    <ItemTemplate>
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server" Value='<%# Eval("amountApproved") %>'></telerik:RadNumericTextBox>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <span>Amount  to Disburse</span>
                    </HeaderTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn>
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkSelected" />
                    </ItemTemplate>
                    <HeaderTemplate>
                        <span>Select to Disburse</span>
                    </HeaderTemplate>
                </telerik:GridTemplateColumn>
             </Columns> 
        </MasterTableView> 
    </telerik:RadGrid>
    <br />
    <telerik:RadButton runat="server" Text="Disburse Selected Amounts" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server" 
            BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" 
             />
</asp:Content>
