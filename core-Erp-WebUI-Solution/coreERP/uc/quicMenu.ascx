<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="quicMenu.ascx.cs" Inherits="coreERP.uc.quicMenu" EnableViewState="false" %>
<telerik:RadPanelBar runat="server" ID="pBar1" Skin="Metro" ExpandMode="SingleExpandedItem" Height="100%"> 
    <Items>
        <telerik:RadPanelItem runat="server" Text="Cashier" Expanded="true" ImageUrl="~/images/cashier.jpg">
            <Items>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/cashier/default3.aspx" Text="Cashier Home" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/reports/cashier.aspx" Text="Cashier Detailed Report" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/reports/cashier2.aspx" Text="Cashier Summary Report" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/reports/statement.aspx" Text="Print Client Loan Statement" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/depositReports/statement.aspx" Text="Print Term Deposit Statement" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/savingReports/statement.aspx" Text="Print Savings Account Statement" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem>
        <telerik:RadPanelItem runat="server" Text="Cashier Administration" ImageUrl="~/images/cashier2.jpg">
            <Items>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/setup/cashiersTill.aspx" Text="Create a Cashier Account" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/setup/openTill.aspx" Text="Open or Close a Cashier" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/setup/postTill.aspx" Text="Post Cashier Transactions" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/setup/closeCheckTill.aspx" Text="Clear Checks Posted By Cashier" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem>
        <telerik:RadPanelItem runat="server" Text="Clients" ImageUrl="~/images/client.jpg">
            <Items>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/client/client.aspx" Text="Enter a new Client" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/client/default.aspx" Text="Edit an existing Client" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem>
        <telerik:RadPanelItem runat="server" Text="Loans" ImageUrl="~/images/loan.jpg">
            <Items>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/loans/loan.aspx" Text="Enter a new Loan" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/loans/default.aspx" Text="Edit an existing Loan" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem>
        <telerik:RadPanelItem runat="server" Text="Savings" ImageUrl="~/images/saving.jpg">
            <Items>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/saving/saving.aspx" Text="Enter a new Savings Account" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/saving/default.aspx" Text="Edit an existing Savings Account" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem>
        <telerik:RadPanelItem runat="server" Text="Deposits" ImageUrl="~/images/deposit.jpg">
            <Items>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/deposit/deposit.aspx" Text="Enter a new Term Deposit" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
                <telerik:RadPanelItem runat="server" NavigateUrl="~/ln/deposit/default.aspx" Text="Edit an existing erm Deposit" ImageUrl="~/images/new.jpg">
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelItem>
    </Items>
</telerik:RadPanelBar>
