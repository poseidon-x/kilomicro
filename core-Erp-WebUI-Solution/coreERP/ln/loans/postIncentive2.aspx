<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="postIncentive2.aspx.cs" Inherits="coreERP.ln.loans.postIncentive2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Post Payroll Loan Commissions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Payroll Loan Commisions</h3>
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
            <td>Posting Date:</td>
            <td> <telerik:RadDatePicker runat="server" DateInput-DateFormat="dd-MMM-yyyy" ID="dtDate"></telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td></td>
            <td><telerik:RadButton runat="server" ID="btnLoad" Text="Load Data" OnClick="btnLoad_Click"></telerik:RadButton></td>
        </tr>
    </table>
    <asp:Repeater runat="server" ID="rpPenalty">
        <HeaderTemplate>
            <table width="1350px">
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
                        Loan Date
                    </td>
                    <td style="width:200px">
                        Net Loan Amount
                    </td> 
                    <td style="width:150px">
                        Commission Amount
                    </td>
                    <td style="width:150px">
                        Withheld Amount
                    </td>
                    <td style="width:150px">
                        Net Amt (Adjusted)
                    </td>
                    <td style="width:100px">
                        Selected
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table width="1350px">
                <tr>
                    <td style="width:200px">
                        <asp:Label ID="Label1" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "agent.surName").ToString() + ", " + DataBinder.Eval(Container.DataItem, "agent.otherNames").ToString() %>'></asp:Label>                        
                    </td>
                    <td style="width:150px">
                        <asp:Label Visible="false" ID="lblID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loanIncentiveID") %>'></asp:Label>
                        <asp:Label ID="lblAccNum" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.client.AccountNumber") %>'></asp:Label>                        
                    </td>
                    <td style="width:200px">
                        <asp:Label ID="lblClientName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.client.surName").ToString() + ", " + DataBinder.Eval(Container.DataItem, "loan.client.otherNames").ToString() %>'></asp:Label>                        
                    </td>
                    <td style="width:100px">
                        <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.loanNo") %>'></asp:Label>                        
                    </td>
                        <td style="width:200px">
                        <telerik:RadDatePicker DateInput-DateFormat="dd-MMM-yyyy" runat="server" Enabled="false"
                            ID="dtDate" SelectedDate='<%#  (DateTime)DataBinder.Eval(Container.DataItem, "incetiveDate") %>'></telerik:RadDatePicker>                        
                    </td>
                    <td style="width:200px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtDisb"  Width="100px" Enabled="false" 
                           runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "loanAmount") %>'></telerik:RadNumericTextBox>                        
                    </td> 
                    <td style="width:150px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl"  Width="100px" ID="txtComm" runat="server" 
                           Value='<%#  DataBinder.Eval(Container.DataItem, "commissionAmount") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:150px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl"  Width="100px" ID="txtWith" 
                           runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "withHoldingAmount") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:150px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl"  Width="100px" ID="txtNet" 
                           runat="server" Value='<%#  (double)DataBinder.Eval(Container.DataItem, "netCommission") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:100px">
                        <asp:CheckBox runat="server" ID="chkSelected" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:Repeater>
            <table width="1350px">
                <tr>
                    <td style="width:200px"> 
                    </td>
                    <td style="width:150px"> 
                    </td>
                    <td style="width:200px"> 
                    </td>
                    <td style="width:100px"> 
                    </td>
                    <td style="width:200px"> 
                    </td>
                    <td style="width:200px">
                        <asp:Label runat="server" ID="lblNetAmount"></asp:Label>
                    </td> 
                    <td style="width:150px">
                        <asp:Label runat="server" ID="lblCommission"></asp:Label>
                    </td>
                    <td style="width:150px">
                        <asp:Label runat="server" ID="lblWithholding"></asp:Label>
                    </td>
                    <td style="width:150px">
                        <asp:Label runat="server" ID="lblNetCommission"></asp:Label>
                    </td>
                    <td style="width:100px">
                        Selected
                    </td>
                </tr>
            </table>
    <br />
    <telerik:RadButton runat="server" Text="Approve Proposed Commissions" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Incentives" OnClick="btnCancel_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnReCalc" Text="Recalc Totals" OnClick="btnReCalc_Click"></telerik:RadButton>
</asp:Content>
