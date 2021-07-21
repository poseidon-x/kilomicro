<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="processDueCheck.aspx.cs" Inherits="coreERP.ln.loans.processDueCheck" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Process Post Dated Checks
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <asp:Repeater runat="server" ID="rpNotes" >
        <HeaderTemplate>
            <table width="1500px">
                <tr> 
                    <td style="width:100px">Check Number</td>
                    <td style="width:200px">Client Name</td>
                    <td style="width:200px">Check Date</td> 
                    <td style="width:100px">Check Amount</td>
                    <td style="width:200px">Bank</td> 
                    <td style="width:100px">Check Type</td> 
                    <td style="width:150px">Cashed</td> 
                    <td style="width:150px">Apply to Loan</td>
                    <td style="width:150px">Refund</td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table width="1500px">
                <tr> 
                    <td style="width:100px">
                        <asp:Label runat="server" ID="lblCheckNumber" Text='<%# Bind("checkNumber") %>'></asp:Label>
                    </td>
                    <td style="width:200px">
                        <asp:Label runat="server" ID="Label2" Text='<%# GetClientName(Eval("clientID")) %>'></asp:Label>
                    </td>
                    <td style="width:200px">
                        <telerik:RadDatePicker runat="server" ID="dtpCkDate" SelectedDate='<%# Bind("checkDate") %>' DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </td> 
                    <td style="width:100px">
                        <telerik:RadNumericTextBox runat="server" ID="txtCkAmount" Value='<%# Bind("checkAmount") %>'></telerik:RadNumericTextBox>
                    </td>
                    <td style="width:200px">
                        <asp:Label runat="server" ID="lblBank" Text='<%# GetBankName(Eval("sourceBankID")) %>'></asp:Label>
                    </td> 
                    <td style="width:100px">
                        <asp:Label runat="server" ID="Label1" Text='<%# GetCheckType(Eval("checkTypeID")) %>'></asp:Label>
                    </td> 
                    <td style="width:150px">
                        <asp:CheckBox runat="server" ID="ckChecked" Checked='<%# Bind("cashed") %>' />
                    </td> 
                    <td style="width:150px">
                        <telerik:RadButton ID='btnApply' CommandArgument='<%# Eval("loanCheckID").ToString() %>'
                             Text="Apply to Loan" runat="server" OnClick="btnApply_Click"></telerik:RadButton>
                    </td>
                    <td style="width:150px"><telerik:RadButton ID='btnRefund' CommandArgument='<%# Eval("loanCheckID").ToString() %>'
                             Text="Return Check" runat="server" OnClick="btnRefund_Click"></telerik:RadButton></td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
