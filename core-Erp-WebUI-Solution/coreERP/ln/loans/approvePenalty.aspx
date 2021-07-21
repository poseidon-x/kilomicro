<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approvePenalty.aspx.cs" Inherits="coreERP.ln.loans.approvePenalty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Approve Loan Additional Interest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Additional Interest Approval</h3>
    <table>
        <tr>
            <td>
                Date:
            </td>
            <td>
                <telerik:RadDatePicker runat="server" ID="dtDate" DateInput-DateFormat="dd-MMM-yyyy" AutoPostBack="true" OnSelectedDateChanged="dtDate_SelectedDateChanged"></telerik:RadDatePicker>
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
    </table>
    <asp:Repeater runat="server" ID="rpPenalty">
        <HeaderTemplate>
            <table Style="width:950px">
                <tr> 
                    <td style="width:300px">
                        Client Name
                    </td>
                    <td style="width:100px">
                        Loan ID
                    </td>
                    <td style="width:200px">
                        Interest Date
                    </td>
                    <td style="width:200px">
                        Loan Additional Interest
                    </td>
                    <td style="width:100px">
                        Selected
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table Style="width:950px">
                <tr> 
                    <td style="width:300px">
                        <asp:Label ID="lblID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loanId") %>' Font-Size="4pt"></asp:Label> 
                        <asp:Label ID="lblClientName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "clientName").ToString() %>'></asp:Label>                        
                    <td style="width:100px">
                        <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loanNo") %>'></asp:Label>                        
                    </td>
                    <td style="width:200px">
                        <telerik:RadDatePicker DateInput-DateFormat="dd-MMM-yyyy" runat="server" ID="dtDate2" SelectedDate='<%#  (DateTime)DataBinder.Eval(Container.DataItem, "penaltyDate") %>'></telerik:RadDatePicker>                        
                    </td>
                    <td style="width:200px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtProposedAmount" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "penaltyAmount") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:100px">
                        <asp:CheckBox runat="server" ID="chkSelected" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <telerik:RadButton runat="server" Text="Approve Adjusted Penalties" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Penalties" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
