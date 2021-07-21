<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="closeCheckTill.aspx.cs" Inherits="coreERP.ln.setup.closeCheckTill" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Open Or Close Cashiers Till
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Open Or Close Cashiers Till</h3>
    <br />
    <table>
        <tr>
            <td>Date:</td>
            <td><telerik:RadDatePicker ID="dtDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy" AutoPostBack="true" OnSelectedDateChanged="dtDate_SelectedDateChanged"></telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td>Cashier:</td>
            <td><telerik:RadComboBox runat="server" Width="200px" DropDownAutoWidth="Enabled" MarkFirstMatch="true"  AutoPostBack="true" OnSelectedIndexChanged="cboUserName_SelectedIndexChanged"
                 AutoCompleteSeparator=" " ID="cboUserName"></telerik:RadComboBox></td>
        </tr>
        <tr><td colspan="2">
        <asp:Repeater runat="server" ID="rpPenalty" OnItemDataBound="rpPenalty_ItemDataBound">
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
                            Loan ID
                        </td>
                        <td style="width:100px">
                            Interest Date
                        </td>
                        <td style="width:200px">
                            Amount
                        </td>
                        <td style="width:100px">
                            Check No
                        </td>
                        <td style="width:100px">
                            Bank
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
                            <asp:Label ID="lblID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "cashierReceiptID") %>'></asp:Label>
                            <asp:Label ID="lblAccNum" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.client.AccountNumber") %>'></asp:Label>                        
                        </td>
                        <td style="width:200px">
                            <asp:Label ID="lblClientName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.client.surName").ToString() + ", " + DataBinder.Eval(Container.DataItem, "loan.client.otherNames").ToString() %>'></asp:Label>                        
                        <td style="width:100px">
                            <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.loanNo") %>'></asp:Label>                        
                        </td>
                            <td style="width:100px">
                            <telerik:RadDatePicker DateInput-DateFormat="dd-MMM-yyyy" runat="server" ID="dtDate" SelectedDate='<%#  (DateTime)DataBinder.Eval(Container.DataItem, "txDate") %>'></telerik:RadDatePicker>                        
                        </td>
                        <td style="width:200px">
                            <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtProposedAmount" runat="server" Enabled="false" Value='<%#  DataBinder.Eval(Container.DataItem, "amount") %>'></telerik:RadNumericTextBox>                        
                        </td>
                        <td style="width:100px">
                            <telerik:RadTextBox WrapperCssClass="inputControl" ID="RadNumericTextBox1" runat="server" Enabled="false" Text='<%#  DataBinder.Eval(Container.DataItem, "checkNo") %>'></telerik:RadTextBox>                        
                        </td>
                        <td style="width:100px">
                            <telerik:RadComboBox ID="cboBank" runat="server"></telerik:RadComboBox>
                        </td>
                        <td style="width:100px">
                            <asp:CheckBox runat="server" ID="chkSelected" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>        
        <br />
        </td>
        </tr>
        <tr> 
            <td><telerik:RadButton ID="btnClose" runat="server" Text="Close Till" OnClick="btnClose_Click"></telerik:RadButton></td>
        </tr>
    </table>
</asp:Content>
