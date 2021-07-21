<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="clientCheck.aspx.cs" Inherits="coreERP.ln.loans.clientCheck" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Client Post Dated Checks
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Post Dated Checks</h3>
    <table>
        <tr>
            <td style="width:150px">
                Client Name:
            </td>
            <td style="width:200px">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
            <td style="width:150px">
                Loan ID:
            </td>
            <td style="width:200px">
                <telerik:RadComboBox ID="cboLoan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboLoan_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select a Loan" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <telerik:RadButton runat="server" ID="btnAddNotes" Text="Add Check" OnClick="btnAddNotes_Click"></telerik:RadButton>
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" Visible="false" ID="pnlEdit">
        <table>
            <tr>
                <td style="width:150px">
                    Check Number:
                </td>
                <td style="width:300px">
                    <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtCheckNo"></telerik:RadTextBox>
                </td>
                <td style="width:150px"> 
                    Expected Due Date 
                </td>
                <td style="width:300px">
                    <telerik:RadDatePicker runat="server" ID="dtCheckDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td>
                    Check Amount:
                </td>
                <td>
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtCheckAmount"></telerik:RadNumericTextBox>
                </td>
                <td> 
                    Source Bank:
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="cboSourceBank" DropDownAutoWidth="Enabled" MaxHeight="400px"></telerik:RadComboBox>
                </td>
            </tr> 
            <tr>
                <td style="width:150px">
                    Check Type
                </td>
                <td style="width:200px">
                    <telerik:RadComboBox runat="server" ID="cboCheckType"></telerik:RadComboBox>
                </td>
                <td> 
                    Target Bank Account (Optional):
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="cboBank" DropDownAutoWidth="Enabled" MaxHeight="400px"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <telerik:RadButton runat="server" ID="btnSave" Text="Save Check" OnClick="btnSave_Click"></telerik:RadButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Repeater runat="server" ID="rpNotes" >
        <HeaderTemplate>
            <table>
                <tr> 
                    <td style="width:100px">Check Number</td>
                    <td style="width:200px">Client Name</td>
                    <td style="width:200px">Check Date</td> 
                    <td style="width:100px">Check Amount</td>
                    <td style="width:200px">Source Bank</td> 
                    <td style="width:100px">Check Type</td> 
                    <td style="width:150px">Cashed</td>
                    <td style="width:150px">Edit</td>
                    <td style="width:150px">Apply to Loan</td>
                    <td style="width:150px">Refund</td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table>
                <tr> 
                    <td style="width:100px">
                        <asp:Label runat="server" ID="lblCheckNumber" Text='<%# Bind("checkNumber") %>'></asp:Label>
                    </td>
                    <td style="width:200px">
                        <asp:Label runat="server" ID="Label2" Text='<%# Bind("client.surName") %>'></asp:Label>
                    </td>
                    <td style="width:200px">
                        <telerik:RadDatePicker runat="server" ID="dtpCkDate" SelectedDate='<%# Bind("checkDate") %>' DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </td> 
                    <td style="width:100px">
                        <telerik:RadNumericTextBox runat="server" ID="txtCkAmount" Value='<%# Bind("checkAmount") %>'></telerik:RadNumericTextBox>
                    </td>
                    <td style="width:200px">
                        <asp:Label runat="server" ID="lblBank" Text='<%# GetSourceBankName(Eval("sourceBankID")) %>'></asp:Label>
                    </td> 
                    <td style="width:100px">
                        <asp:Label runat="server" ID="Label1" Text='<%# GetCheckType(Eval("checkTypeID")) %>'></asp:Label>
                    </td> 
                    <td style="width:150px">
                        <asp:CheckBox runat="server" ID="ckChecked" Checked='<%# Bind("cashed") %>' />
                    </td>
                    <td style="width:150px">
                        <telerik:RadButton ID='btnEdit' CommandArgument='<%# Eval("loanCheckID").ToString() %>'
                             Text="Edit" runat="server" OnClick="btnEdit_Click"></telerik:RadButton>
                    </td>
                    <td style="width:150px">
                        <telerik:RadButton ID='btnApply' CommandArgument='<%# Eval("loanCheckID").ToString() %>'
                             Text="Apply to Loan" runat="server" OnClick="btnApply_Click" Visible="false"></telerik:RadButton>
                    </td>
                    <td style="width:150px"><telerik:RadButton ID='btnRefund' CommandArgument='<%# Eval("loanCheckID").ToString() %>'
                             Text="Return Check" runat="server" OnClick="btnRefund_Click" Visible="false"></telerik:RadButton></td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
