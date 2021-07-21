<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="quicMenuGL.ascx.cs" Inherits="coreERP.uc.quicMenuGL" EnableViewState="false" %>
<telerik:RadPanelBar runat="server" ID="pBar1" Skin="Metro" ExpandMode="SingleExpandedItem" Height="100%">
    <Items>
        <telerik:RadPanelItem runat="server" Text="Journal/Bookkeeping" Expanded="true" ImageUrl="~/images/cashier.jpg">
            <Items>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/journal/default.aspx" Text="Enter Local Currency Transactions" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/journal/view.aspx" Text="View Journal Transactions" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/journal/post.aspx" Text="Post Temporary Journal" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem>
        <telerik:RadPanelItem runat="server" Text="Petty Cash" ImageUrl="~/images/cashier2.jpg">
            <Items>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/pc/pc.aspx" Text="Enter Petty Cash" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/pc/psot.aspx" Text="Post Petty Cash" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem> 
            </Items>
        </telerik:RadPanelItem >
        <telerik:RadPanelItem runat="server" Text="Financial Reports" ImageUrl="~/images/chart_of_accounts/account.jpg">
            <Items>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/reports/bal_sht_std.aspx" Text="Balance Sheet" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/reports/trial_bal_std.aspx" Text="Trial Balance" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/reports/op_stmt_std.aspx" Text="Income Statement" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/reports/op_stmt_ct.aspx" Text="Cross Tab Income Statement" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/reports/tx_by_acc.aspx" Text="Journal Transaction Details By Account" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/gl/reports/op_stmt_2yrs.aspx" Text="Comparative Income Statement" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem> 
    </Items>
</telerik:RadPanelBar>
