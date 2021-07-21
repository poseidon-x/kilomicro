<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approveInterest.aspx.cs" Inherits="coreERP.ln.saving.approveInterest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Approve Savings Interest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Approve Savings Interest</h3>
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
            <tr>
                <td>
                    Currency
                </td>
                <td>
                    <telerik:RadComboBox ID="cboCur" runat="server" AutoPostBack="true" 
                       OnSelectedIndexChanged="cboCur_SelectedIndexChanged" ></telerik:RadComboBox>
                </td>
                <td>
                    Exchange Rate
                </td>
                <td>
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtFxRate"></telerik:RadNumericTextBox>
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
                        Savings ID
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
                        <asp:Label ID="lblID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "savingScheduleID") %>'></asp:Label>
                        <asp:Label ID="lblAccNum" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "saving.client.AccountNumber") %>'></asp:Label>                        
                    </td>
                    <td style="width:200px">
                        <asp:Label ID="lblClientName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "saving.client.surName").ToString() + ", " + DataBinder.Eval(Container.DataItem, "saving.client.otherNames").ToString() %>'></asp:Label>                        
                    <td style="width:100px">
                        <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "saving.savingNo") %>'></asp:Label>                        
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
