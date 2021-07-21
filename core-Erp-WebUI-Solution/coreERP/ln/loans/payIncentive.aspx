<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="payIncentive.aspx.cs" Inherits="coreERP.ln.loans.payIncentive" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Pay Payroll Loan Incentive
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Incentive</h3>
    <table>
        <tr>
            <td>
                Sales Agent 
            </td>
            <td colspan="2">
                <telerik:RadComboBox ID="cboAgent" runat="server" 
                    DropDownWidth="355px" EmptyMessage="Select an Agent" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr> 
        <tr>
            <td>Start Date:</td>
            <td> <telerik:RadDatePicker runat="server" DateInput-DateFormat="dd-MMM-yyyy" ID="dtpStartDate"></telerik:RadDatePicker></td>
            <td>End Date:</td>
            <td> <telerik:RadDatePicker runat="server" DateInput-DateFormat="dd-MMM-yyyy" ID="dtpEndDate"></telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td></td>
            <td><telerik:RadButton runat="server" ID="btnLoad" Text="Load Data" OnClick="btnLoad_Click"></telerik:RadButton></td>
        </tr>
        <tr>
             <td>Payment Mode</td>
            <td colspan="1">
                <telerik:RadComboBox ID="cboPaymentMode" runat="server" >
                    <Items>
                        <telerik:RadComboBoxItem Text="Cash" Value="1" />
                        <telerik:RadComboBoxItem Text="Cheque" Value="2" />
                        <telerik:RadComboBoxItem Text="BankTransfer" Value="3" />
                    </Items>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Cheque No</td>
            <td><telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtCheckNo"></telerik:RadTextBox></td>
            <td>Bank</td>
            <td><telerik:RadComboBox runat="server" ID="cboBank"></telerik:RadComboBox></td>
        </tr>
    </table>
    <asp:Repeater runat="server" ID="rpPenalty">
        <HeaderTemplate>
            <table width="1200px">
                <tr>
                    <td style="width:200px">
                        Agent Name
                    </td>
                    <td style="width:150px">
                        Client Acc. Num.
                    </td>
                    <td style="width:200px">
                        Client Name
                    </td>
                    <td style="width:100px">
                        Loan ID
                    </td>
                    <td style="width:200px">
                        Incentive Date
                    </td>
                    <td style="width:200px">
                        Loan Amount
                    </td>
                    <td style="width:150px">
                        Incentive Amount
                    </td> 
                    <td style="width:100px">
                        Selected
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table width="1200px">
                <tr>
                    <td style="width:200px">
                        <asp:Label ID="Label1" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "agent.surName").ToString() + ", " + DataBinder.Eval(Container.DataItem, "agent.otherNames").ToString() %>'></asp:Label>                        
                    </td>
                    <td style="width:150px">
                        <asp:Label ID="lblID" Visible="false" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loanIncentiveID") %>'></asp:Label>
                        <asp:Label ID="lblAccNum" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.client.AccountNumber") %>'></asp:Label>                        
                    </td>
                    <td style="width:200px">
                        <asp:Label ID="lblClientName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.client.surName").ToString() + ", " + DataBinder.Eval(Container.DataItem, "loan.client.otherNames").ToString() %>'></asp:Label>                        
                    <td style="width:100px">
                        <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.loanNo") %>'></asp:Label>                        
                    </td>
                    <td style="width:200px">
                        <telerik:RadDatePicker DateInput-DateFormat="dd-MMM-yyyy" runat="server" ID="dtDate" SelectedDate='<%#  (DateTime)DataBinder.Eval(Container.DataItem, "incetiveDate") %>'></telerik:RadDatePicker>                        
                    </td>
                    <td style="width:150px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" Width="100px" ID="txtDisb" Enabled="false" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "loanAmount") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:150px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" Width="100px" ID="txtProposedAmount" Enabled="false" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "incentiveAmount") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:100px">
                        <asp:CheckBox runat="server" ID="chkSelected" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <telerik:RadButton runat="server" Text="Pay Approved Incentives" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Incentives" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
