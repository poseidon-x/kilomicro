<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approveReApply.aspx.cs" Inherits="coreERP.ln.loans.approveReApply" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Apply Negative Balance on Loans
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Apply Negative Balance on Loans</h3>
    <table>
        <tr>
            <td>
                Date:
            </td>
            <td>
                <telerik:RadDatePicker runat="server" ID="dtDate" DateInput-DateFormat="dd-MMM-yyyy" ></telerik:RadDatePicker>
            </td>
            <td>
                Client:
            </td>
            <td>
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number" ></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                Apply to Loan ID:
            </td>
            <td>
                <telerik:RadComboBox ID="cboLoan" runat="server" 
                        DropDownWidth="355px" EmptyMessage="Select a Loan" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
    </table>
    <asp:Repeater runat="server" ID="rpPenalty">
        <HeaderTemplate>
            <table Style="width:850px">
                <tr>
                    <td style="width:150px">
                        Client Acc. Num.
                    </td>
                    <td style="width:300px">
                        Client Name
                    </td>
                    <td style="width:100px">
                        Loan ID
                    </td> 
                    <td style="width:200px">
                        Loan Balance
                    </td>
                    <td style="width:100px">
                        Selected
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table Style="width:850px">
                <tr>
                    <td style="width:150px">
                        <asp:Label ID="lblID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loanID") %>'></asp:Label>
                        <asp:Label ID="lblAccNum" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "accountNumber") %>'></asp:Label>                        
                    </td>
                    <td style="width:300px">
                        <asp:Label ID="lblClientName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "clientName").ToString() %>'></asp:Label>                        
                    <td style="width:100px">
                        <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loanNo") %>'></asp:Label>                        
                    </td> 
                    <td style="width:200px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtProposedAmount" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "Balance") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:100px">
                        <asp:CheckBox runat="server" ID="chkSelected" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <telerik:RadButton runat="server" Text="Apply Balance" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton> 
</asp:Content>
