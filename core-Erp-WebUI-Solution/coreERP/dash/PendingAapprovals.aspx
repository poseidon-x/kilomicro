<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PendingAapprovals.aspx.cs" Inherits="PendingAapprovals" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Pending Loan Approvals</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .wrapper {
         width:1500px;
         overflow:hidden;
        }

        .left {
         width:49%;
         float:left;
         margin-right:1%;
        }

        .right {
         width:49%;
         float:right;
        }
         
        .headerTitle {
            font-family:Calibri;
            font-size:14pt;   
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Display Results for (Branch):
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" CssClass="inputControl" AutoPostBack="true"
                     ID="cboBranch" OnSelectedIndexChanged="cboBranch_SelectedIndexChanged">
                </telerik:RadComboBox>
            </div>
        </div>
    </div>
    <div class="wrapper"> 
        <div class="left">
            <h3 class="headerTitle">Applications Pending Approval</h3>
            <telerik:RadGrid ID="gridApp" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="10" AllowPaging="true" 
                AllowSorting="true" OnPageIndexChanged="gridApp_PageIndexChanged" OnSortCommand="gridApp_SortCommand"
                OnLoad="gridApp_Load">
                <MasterTableView ShowFooter="true"> 
                    <Columns>
                        <telerik:GridBoundColumn DataField="clientID" HeaderText="Client ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="loanNo" HeaderText="Loan ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amountRequested" HeaderText="Amount Requetsed" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="applicationDate" HeaderText="Date Applied"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="staffName" HeaderText="Rel. Officer"></telerik:GridBoundColumn> 
                        <telerik:GridHyperLinkColumn DataNavigateUrlFields="loanID,categoryID" DataNavigateUrlFormatString="~/ln/loans/loanChecklist.aspx?id={0}&catID={1}" Text="Checklist" HeaderText="Checklist"></telerik:GridHyperLinkColumn>
                        <telerik:GridHyperLinkColumn DataNavigateUrlFields="loanID,categoryID" DataNavigateUrlFormatString="~/ln/loans/approve.aspx?id={0}&catID={1}" Text="Approve" HeaderText="Approve"></telerik:GridHyperLinkColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <div class="right">
            <h3 class="headerTitle">Undisbursed Approved Loans</h3>
            <telerik:RadGrid ID="gridUnd" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="10" AllowPaging="true" 
                AllowSorting="true" OnPageIndexChanged="gridUnd_PageIndexChanged" OnSortCommand="gridUnd_SortCommand"
                OnLoad="gridUnd_Load">
                <MasterTableView ShowFooter="true"> 
                    <Columns>
                        <telerik:GridBoundColumn DataField="clientID" HeaderText="Client ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="loanNo" HeaderText="Loan ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amountRequested" HeaderText="Amount Requested" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="amountApproved" HeaderText="Amount Approved" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="finalApprovalDate" HeaderText="Date Approved"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="staffName" HeaderText="Rel. Officer"></telerik:GridBoundColumn> 
                        <telerik:GridHyperLinkColumn DataNavigateUrlFields="loanID,categoryID" DataNavigateUrlFormatString="~/ln/cashier/disburse.aspx?id={0}&catID={1}" Text="Disburse" HeaderText="Disburse"></telerik:GridHyperLinkColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
     
</asp:Content>