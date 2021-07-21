<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="postCollection.aspx.cs" Inherits="coreERP.ln.loans.postCollection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Post Payroll Loan Collection Ratios
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Payroll Loan Collection Ratios</h3>
    <table>
        <tr>
            <td>Month (YYYYMM)</td>
            <td>
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" id="txtMonth" Decimal-Digits="0"></telerik:RadNumericTextBox>
            </td>
        </tr>    
        <tr>
            <td></td>
            <td><telerik:RadButton runat="server" ID="btnLoad" Text="Load Data" OnClick="btnLoad_Click"></telerik:RadButton></td>
        </tr>
    </table>
    <asp:Repeater runat="server" ID="rpPenalty">
        <HeaderTemplate>
            <table width="650px">
                <tr>
                    <td style="width:200px">
                        Loan Product
                    </td>
                    <td style="width:150px">
                        Month
                    </td> 
                    <td style="width:200px">
                        Collection
                    </td> 
                    <td style="width:100px">
                        Selected
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table width="650px">
                <tr>
                    <td style="width:200px">
                        <asp:Label ID="Label1" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "loanProduct.loanProductName").ToString() + ", " %>'></asp:Label>                        
                    </td>
                    <td style="width:150px">
                        <asp:Label Visible="false" ID="lblID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "collectionID") %>'></asp:Label>
                        <asp:Label ID="lblAccNum" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "month") %>'></asp:Label>                        
                    </td> 
                    <td style="width:200px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtColl"  Width="100px" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "collection1") %>'></telerik:RadNumericTextBox>                        
                    </td> 
                    <td style="width:100px">
                        <asp:CheckBox runat="server" ID="chkSelected" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <telerik:RadButton runat="server" Text="Approve Proposed Collection Ratios" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Ratio'\s" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
