<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approveDepreciation.aspx.cs" Inherits="coreERP.fa.assets.approveDepreciation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Approve Loan Additional Interest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Approve Loan Additional Interest</h3>
    <table>
        <tr>
            <td colspan="2">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select an Asset" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the asset to depreciate"></telerik:RadComboBox>
                <telerik:RadComboBox ID="cboCat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboCat_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select an Asset Category" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the asset category to depreciate"></telerik:RadComboBox>
                <telerik:RadComboBox ID="cboSCat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboSCat_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select a Sub Category" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the asset sub category to depreciate"></telerik:RadComboBox>
                <telerik:RadComboBox ID="cboOU" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboOU_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select an Org. Unit" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the organization unit whose assets will be depreciated"></telerik:RadComboBox>
            </td>
        </tr>
    </table>
    <telerik:RadButton ID="btnCheckAll" Text="Select All" runat="server" OnClick="btnCheckAll_Click"
                    ToolTip="Click to select all the asset depreciations proposed below"></telerik:RadButton>
    <telerik:RadButton ID="btnUncheckAll" Text="De-Select All" runat="server" OnClick="btnUncheckAll_Click"
                    ToolTip="Click to de-select all the asset depreciations proposed below"></telerik:RadButton>
    <asp:Repeater runat="server" ID="rpPenalty">
        <HeaderTemplate>
            <table width="850px">
                <tr> 
                    <td style="width:200px">
                        Asset Description
                    </td>
                    <td style="width:100px">
                        Asset ID
                    </td>
                    <td style="width:100px">
                        Dep Date
                    </td>
                    <td style="width:200px">
                        Depreciation Amount
                    </td>
                    <td style="width:100px">
                        Selected
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table width="850px">
                <tr> 
                    <td style="width:200px">
                        <asp:Label ID="lblID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "assetDepreciationID") %>' Visible="false"></asp:Label> 
                        <asp:Label ID="lblClientName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "asset.assetDescription").ToString() %>'></asp:Label>                        
                    <td style="width:100px">
                        <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "asset.assetTag") %>'></asp:Label>                        
                    </td>
                        <td style="width:100px">
                        <asp:Label ID="lblPenaltyDate" runat="server" Text='<%#  ((DateTime)DataBinder.Eval(Container.DataItem, "drepciationDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>                        
                    </td>
                    <td style="width:200px">
                       <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtProposedAmount" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem, "proposedAmount") %>'></telerik:RadNumericTextBox>                        
                    </td>
                    <td style="width:100px">
                        <asp:CheckBox runat="server" ID="chkSelected" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <telerik:RadButton runat="server" Text="Approve Adjusted Depreciations" ID="btnOK" OnClick="btnOK_Click"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Depreciations" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
