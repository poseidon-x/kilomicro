<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="reverseProvision.aspx.cs" Inherits="coreERP.ln.loans.reverseProvision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Post Loan Provisions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Provision Reverse</h3>
    <table>
        <tr>
            <td>
                Year :
            </td>
            <td>
                <telerik:RadComboBox ID="cboYear" runat="server" Width="100px" 
                        DropDownWidth="255px" EmptyMessage="Select a Year" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                  AutoPostBack="true" OnSelectedIndexChanged="cboYear_SelectedIndexChanged"></telerik:RadComboBox>
            </td>
            <td>
                Month :
            </td>
            <td>
                <telerik:RadComboBox ID="cboMonth" runat="server" Width="100px"
                        DropDownWidth="255px" EmptyMessage="Select a month" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                  AutoPostBack="true" OnSelectedIndexChanged="cboMonth_SelectedIndexChanged"></telerik:RadComboBox>
            </td>
        </tr> 
        <tr>
            <td></td>
            <td>
                <telerik:RadButton runat="server" Text="Initialize" ID="btnInit" OnClick="btnInit_Click"></telerik:RadButton>
            </td> 
        </tr>
    </table>
    <asp:Repeater runat="server" ID="rpPenalty">
        <HeaderTemplate>
            <table width="1000px">
                <tr>
                    <td style="width:100px">
                        Client Acc. Num.
                    </td>
                    <td style="width:200px">
                        Client Name
                    </td>
                    <td style="width:100px">
                        Loan ID
                    </td>
                    <td style="width:100px">
                        Type of Security
                    </td>
                    <td style="width:100px">
                        Outstanding Principal
                    </td>
                    <td style="width:100px">
                        Days Due
                    </td>
                    <td style="width:100px">
                        Provision
                    </td> 
                    <td style="width:100px">
                        Security Value
                    </td>  
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table width="1000px">
                <tr>
                    <td style="width:100px">
                        <asp:Label ID="lblID" Visible="false" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loanProvisionID") %>'></asp:Label>
                        <asp:Label ID="lblAccNum" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.client.AccountNumber") %>'></asp:Label>                        
                    </td>
                    <td style="width:200px">
                        <asp:Label ID="lblClientName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.client.surName").ToString() + ", " + DataBinder.Eval(Container.DataItem, "loan.client.otherNames").ToString() %>'></asp:Label>                        
                    <td style="width:100px">
                        <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loan.loanNo") %>'></asp:Label>                        
                    </td>
                    <td style="width:100px">
                        <asp:Label ID="Label1" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "typeOfSecurity") %>'></asp:Label>
                    </td>
                    <td style="width:100px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtPrinc" Width="100px" Enabled="false" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "principalBalance") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:100px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtDays" Enabled="false" Width="100px" NumberFormat-DecimalDigits="0" runat="server" Value='<%#  double.Parse(DataBinder.Eval(Container.DataItem, "daysDue").ToString()) %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:100px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmt"  Width="100px" Enabled="false" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "proposedAmount") %>'></telerik:RadNumericTextBox>                        
                    </td> 
                    <td style="width:100px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtSec"  Width="100px" Enabled="false" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "securityValue") %>'></telerik:RadNumericTextBox>                        
                    </td>  
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <br /> 
    <telerik:RadButton runat="server" Text="Reverse Posted Provisions" ID="btnPost" OnClick="btnPost_Click"></telerik:RadButton> 
</asp:Content>
