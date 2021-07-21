<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="postAsset.aspx.cs" Inherits="coreERP.fa.assets.postAsset" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Approve Loan Additional Interest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Approve Additional Interest</h3>
    <table>
        <tr>
            <td colspan="2">
                Asset: <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Select the asset" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""   
                    ToolTip="Select the asset to post"></telerik:RadComboBox>
                Asset Category: <telerik:RadComboBox ID="cboCat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboCat_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select an Asset Category" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the category of the assets to post"></telerik:RadComboBox>
                Sub Category: <telerik:RadComboBox ID="cboSCat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboSCat_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select a Sub Category" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the sub-category of the assets to post"></telerik:RadComboBox>
                Org Unit: <telerik:RadComboBox ID="cboOU" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboOU_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select an Org. Unit" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the organizational units of the assets to post"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                Credit Account: 
                 <telerik:RadComboBox ID="cboCrAcc" runat="server" Height="150px" Width=" 255px"
                    DropDownWidth="455px" EmptyMessage="Transaction Account" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" DataTextField="acc_name" DataValueField="acct_id"
                    AppendDataBoundItems="true"
                    ToolTip="Select the general ledger account to be credited">
                    <HeaderTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr> 
                                <td style="width: 80px;">
                                    Acc Num</td>
                                <td style="width: 250px;">
                                    Acc Name</td>
                                <td style="width: 80px;">
                                    Currency</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 80px;" >                                    
                                    <%# Eval("acc_num")%>
                                </td>
                                <td style="width: 250px;">                                   
                                    <%# Eval("acc_name")%>
                                </td> 
                                <td style="width: 80px;">                                   
                                    <%# Eval("major_name")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
                Debit Account: 
                 <telerik:RadComboBox ID="cboDebAcc" runat="server" Height="150px" Width=" 255px"
                    DropDownWidth="455px" EmptyMessage="Transaction Account" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" DataTextField="acc_name" DataValueField="acct_id"
                    AppendDataBoundItems="true"
                    ToolTip="Select the general ledger account to be debited">
                    <HeaderTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr> 
                                <td style="width: 80px;">
                                    Acc Num</td>
                                <td style="width: 250px;">
                                    Acc Name</td>
                                <td style="width: 80px;">
                                    Currency</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 80px;" >                                    
                                    <%# Eval("acc_num")%>
                                </td>
                                <td style="width: 250px;">                                   
                                    <%# Eval("acc_name")%>
                                </td> 
                                <td style="width: 80px;">                                   
                                    <%# Eval("major_name")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </td>
        </tr>
    </table>
    <telerik:RadButton ID="btnCheckAll" Text="Select All" runat="server" OnClick="btnCheckAll_Click"
                    ToolTip="Click to select all the assets displaid below"></telerik:RadButton>
    <telerik:RadButton ID="btnUncheckAll" Text="De-Select All" runat="server" OnClick="btnUncheckAll_Click"
                    ToolTip="Click to de-select all the assets displaid below"></telerik:RadButton>
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
                        Purchase Date
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
                        <asp:Label ID="lblID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "assetID") %>' Visible="false"></asp:Label> 
                        <asp:Label ID="lblClientName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "assetDescription").ToString() %>'></asp:Label>                        
                    <td style="width:100px">
                        <asp:Label ID="lblLoanNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "assetTag") %>'></asp:Label>                        
                    </td>
                        <td style="width:100px">
                        <asp:Label ID="lblPenaltyDate" runat="server" Text='<%#  ((DateTime)DataBinder.Eval(Container.DataItem, "assetPurchaseDate")).ToString("dd-MMM-yyyy") %>'></asp:Label>                        
                    </td>
                    <td style="width:100px">
                        <asp:CheckBox runat="server" ID="chkSelected" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <telerik:RadButton runat="server" Text="Post Assets" ID="btnOK" OnClick="btnOK_Click"
                    ToolTip="Click to post all the selected assets to the general ledger"></telerik:RadButton>
    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel Depreciations" OnClick="btnCancel_Click"></telerik:RadButton>
</asp:Content>
