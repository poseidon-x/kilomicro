<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approveInterest.aspx.cs" Inherits="coreERP.ln.deposit.approveInterest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Approve Interest on Client Investment Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Approve Interest on Client Investment Account</h3>
    <table>
        <tr>
            <td>
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
            <td>
                <telerik:RadDatePicker ID="dtInterestDate1" runat="server" DateInput-DateFormat="dd-MMM-yyyy" AutoPostBack="true" OnSelectedDateChanged="dtInterestDate1_SelectedDateChanged"></telerik:RadDatePicker>
            </td>
        </tr>
    </table>
    <asp:Repeater runat="server" ID="rpPenalty">
        <HeaderTemplate>
            <table width="950px">
                <tr>
                    <td style="width:150px">
                        Client Acc. Num.
                    </td>
                    <td style="width:200px">
                        Client Name
                    </td>
                    <td style="width:100px">
                        Deposit ID
                    </td>
                    <td style="width:100px">
                        Interest Date
                    </td>
                    <td style="width:200px">
                        Principal Amount
                    </td>
                    <td style="width:200px">
                        Interest Amount
                    </td>
                    <td style="width:100px">
                        Selected
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table width="950px">
                <tr>
                    <td style="width:150px">
                        <asp:Label ID="lblID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "depositScheduleID") %>'></asp:Label>
                        <asp:Label ID="lblAccNum" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "deposit.client.AccountNumber") %>'></asp:Label>                        
                    </td>
                    <td style="width:200px">
                        <asp:Label ID="lblClientName" runat="server" Text='<%#  GetClientName(DataBinder.Eval(Container.DataItem, "depositScheduleID")) %>'></asp:Label>                        
                    <td style="width:100px">
                        <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "deposit.depositNo") %>'></asp:Label>                        
                    </td>
                        <td style="width:100px">
                         <telerik:RadDatePicker ID="dtInterestDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"
                             SelectedDate='<%#  (DateTime)DataBinder.Eval(Container.DataItem, "repaymentDate") %>'></telerik:RadDatePicker>
                    </td>
                    <td style="width:200px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="RadNumericTextBox1" runat="server" Enabled="false" Value='<%#  DataBinder.Eval(Container.DataItem, "principalPayment") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:200px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtProposedAmount" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "interestPayment") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:100px">
                        <asp:CheckBox runat="server" ID="chkSelected" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <telerik:RadButton runat="server" Text="Approve Interests" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Interests" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
