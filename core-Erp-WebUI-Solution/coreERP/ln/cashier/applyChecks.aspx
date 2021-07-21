<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="applyChecks.aspx.cs" Inherits="coreERP.ln.cashier.applyChecks" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Apply Checks</h3>
    <table>
        <tr>
            <td colspan="2">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" CausesValidation="false"
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number"
                    ToolTip="Begin typing any of the surname, other names or account number of the client to process" ></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Closing Date</td>
            <td><telerik:RadDatePicker ID="dtDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy" DateInput-CausesValidation="false" AutoPostBack="true" OnSelectedDateChanged="dtDate_SelectedDateChanged"
                    DateInput-ToolTip="Select the date the client made the payment. Ypur till for that date must be openned before your can proceed."></telerik:RadDatePicker></td>
             <td>Payment Mode</td>
            <td colspan="2">
                <telerik:RadComboBox ID="cboPaymentMode" runat="server"
                    ToolTip="Select the method by which the client paid">
                    <Items>
                        <telerik:RadComboBoxItem Text="Cash" Value="1" />
                        <telerik:RadComboBoxItem Text="Check" Value="2" />
                        <telerik:RadComboBoxItem Text="BankTransfer" Value="3" />
                    </Items>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Check No</td>
            <td><telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtCheckNo"
                    ToolTip="Enter the check number presented by the client (if applicable)"></telerik:RadTextBox></td>
            <td>Bank</td>
            <td><telerik:RadComboBox runat="server" ID="cboBank"
                    ToolTip="Select the bank account to which the payment was made by the client (if applicable)"></telerik:RadComboBox></td>
        </tr>
        <tr>
            <td>Amount</td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtValue"
                    ToolTip="Enter the total amount paid by the client"></telerik:RadNumericTextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfv1" ControlToValidate="txtValue"
                 ErrorMessage="You must kindly enter the amount paid by the client" ForeColor="Red" Font-Bold="true" Text="!"
                    ></asp:RequiredFieldValidator>
            </td>
            <td>Remaining Amount</td>
            <td><telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server"  Enabled="false" ID="txtRemAmt"
                    ToolTip="The remainder amount to be returned to the client will be displaid here"></telerik:RadNumericTextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <telerik:RadButton runat="server" ID="btnAllocate" Text="Auto Allocate" OnClick="btnAllocate_Click"
                    ToolTip="Click to automatically allocate the amount entered above among the client's outstanding loands below"></telerik:RadButton>
                <telerik:RadButton runat="server" ID="btnAllocate2" Text="Allocate" OnClick="btnAllocate2_Click"
                    ToolTip="Click to manually allocate the amount entered above among the client's outstanding loands below"></telerik:RadButton>
            </td>
        </tr>
    </table>
    <telerik:RadGrid ID="gridLoans" runat="server" AutoGenerateColumns="false" Width="800px" MasterTableView-AllowSorting="true"
         OnSortCommand="gridLoans_SortCommand">
        <MasterTableView ShowFooter="true" DataKeyNames="loanID" >
            <Columns> 
                <telerik:GridBoundColumn DataField="loanID" HeaderText="accountNumber" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="loanNo" HeaderText="Loan ID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="invoiceNo" HeaderText="Invoice No."></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="supplierName" HeaderText="Supplier"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="disbursementDate" HeaderText="Disbursement Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="principalOutstanding" HeaderText="Principal" DataFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="interestOutstanding" HeaderText="Interest" DataFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="processingFee" HeaderText="Fees" DataFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="totalOutstanding" HeaderText="Total Outstanding"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn>
                    <ItemTemplate>
                        <telerik:RadComboBox runat="server" ID="cboPaymentType"  ToolTip="Select the payment payment" >
                            <Items>
                               <telerik:RadComboBoxItem Text="Principal and Interest" Value="1" />
                               <telerik:RadComboBoxItem Text="Principal Only" Value="2" Selected="true" />
                               <telerik:RadComboBoxItem Text="Interest Only" Value="3" />
                               <telerik:RadComboBoxItem Text="Processing Fee" Value="6" />
                               <telerik:RadComboBoxItem Text="All" Value="8" />
                       </Items>
                            </telerik:RadComboBox>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <span>Payment Type</span>
                    </HeaderTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn>
                    <ItemTemplate>
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server"></telerik:RadNumericTextBox>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <span>Amount  to Apply</span>
                    </HeaderTemplate>
                </telerik:GridTemplateColumn>
             </Columns> 
        </MasterTableView> 
    </telerik:RadGrid>
    <br />
    <telerik:RadButton runat="server" Text="Apply Allocated Amounts" ID="btnOK" OnClick="btnOK_Click"
                    ToolTip="Click to process the client's payment. Your till must be posted by the administrator before it will reflect on the client's account"></telerik:RadButton>
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server" 
            BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" 
             />
</asp:Content>
