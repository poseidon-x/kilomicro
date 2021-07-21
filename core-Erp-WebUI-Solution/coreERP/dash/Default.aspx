<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_Default" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Welcome</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .wrapper {
         width:100%;
         overflow:hidden;
        }

        .left {
         width:32%;
         float:left;
         margin-right:1%;
        }

        .right {
         width:33%;
         float:right;
        }
        
        .middle {
         width:32%; 
         float:left;
         margin-left:1%;
        }

        .headerTitle {
            font-family:sans-serif;
            font-size:14pt; 
            text-decoration:overline underline;
        }

	div.RadTileList.rtlistResponsive .RadTile.rtileSquare
    {
        width: 136px;
        height: 120px;
    }
 
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <telerik:RadTileList runat="server" ID="tile1"  RenderMode="Mobile"  
            SelectionMode="Multiple" EnableDragAndDrop="true">
        <Groups>
            <telerik:TileGroup>
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Client"  runat="server"
                        Text="Manage Client Data" NavigateUrl="/ln/client/default.aspx" ></telerik:RadTextTile>
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Client"  runat="server"
                        Text="Manage Client Data (Payroll)" NavigateUrl="/ln/client/default.aspx?catID=5" BackColor="Orange"></telerik:RadTextTile>
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Loan"  runat="server"
                        Text="New Loan Application (SME)" NavigateUrl="/ln/loans/loan.aspx" BackColor="LightGray"></telerik:RadTextTile>
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Loan"  runat="server"
                        Text="New Loan Application (Payroll)" NavigateUrl="/ln/loans/loan.aspx?catID=5"></telerik:RadTextTile>
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Cashier"  runat="server"
                        Text="SME Cashier" NavigateUrl="/ln/cashier/default3.aspx" BackColor="DarkGray"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Cashier"  runat="server"
                        Text="Payroll Cashier" NavigateUrl="/ln/cashier/default3.aspx?catID=5" ></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Ledger"  runat="server"
                        Text="Manage Journal Transactions" NavigateUrl="/gl/journal/default.aspx" BackColor="Orange"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Ledger"  runat="server"
                        Text="Manage Petty Cash" NavigateUrl="/gl/pc/pc.aspx" ></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Ledger"  runat="server"
                        Text="Post Journal Transactions" NavigateUrl="/gl/journal/post.aspx" BackColor="LightGray"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="H/R"  runat="server"
                        Text="Manage Staff Data" NavigateUrl="/fa/staff/default.aspx" BackColor="DarkGray"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="H/R"  runat="server"
                        Text="Staff HRIS Master" NavigateUrl="/hc/staff/default2.aspx" BackColor="Orange"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="H/R"  runat="server"
                        Text="Staff Payroll Master" NavigateUrl="/hc/staff/default.aspx" ></telerik:RadTextTile>
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Financial"  runat="server"
                        Text="Trial Balance" NavigateUrl="/gl/reports/trial_bal_std.aspx" BackColor="LightGray"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Financial"  runat="server"
                        Text="Balance Sheet" NavigateUrl="/gl/reports/bal_sht_std.aspx" BackColor="DarkGray"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Financial"  runat="server"
                        Text="Transactions By Account" NavigateUrl="/gl/reports/tx_by_acc.aspx" ></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Financial"  runat="server"
                        Text="Income Statement" NavigateUrl="/gl/reports/op_stmt_std.aspx"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Financial"  runat="server"
                        Text="C/T Income Statement" NavigateUrl="/gl/reports/op_stmt_ct.aspx" BackColor="Orange"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="F/A"  runat="server"
                        Text="Fixed Assets" NavigateUrl="/fa/assets/default.aspx" ></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"   Title-Text="Savings & Investments"  runat="server"
                        Text="New Term Deposit Account" NavigateUrl="/ln/deposit/deposit.aspx" BackColor="LightGray"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Savings & Investments"  runat="server"
                        Text="Manage Term Deposits" NavigateUrl="/ln/deposit/default.aspx" BackColor="Orange"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Savings & Investments"  runat="server"
                        Text="New Savings Account" NavigateUrl="/ln/saving/saving.aspx" ></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Savings & Investments"  runat="server"
                        Text="Manage Savings Accounts" NavigateUrl="/ln/saving/default.aspx"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Savings & Investments"  runat="server"
                        Text="Detailed Savings Accounts Balances" NavigateUrl="/ln/savingReports/savings.aspx" BackColor="Orange"></telerik:RadTextTile> 
                <telerik:RadTextTile Font-Size="11pt" Font-Bold="true"  Title-Text="Savings & Investments"  runat="server"
                        Text="Detailed Term Deposit Balances" NavigateUrl="/ln/depositReports/deposits.aspx" ></telerik:RadTextTile> 
            </telerik:TileGroup>  
        </Groups>
    </telerik:RadTileList>    
</asp:Content>