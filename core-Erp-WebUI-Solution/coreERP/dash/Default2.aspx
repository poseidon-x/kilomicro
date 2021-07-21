<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="Default2" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Welcome</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .wrapper {
         width:1350px;
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
                        <telerik:GridHyperLinkColumn DataNavigateUrlFields="loanID,categoryID" DataNavigateUrlFormatString="~/ln/loans/loanChecklist.aspx?id={0}&catID={1}" Text="Checklist" HeaderText="Checklist"></telerik:GridHyperLinkColumn>
                        <telerik:GridHyperLinkColumn DataNavigateUrlFields="loanID,categoryID" DataNavigateUrlFormatString="~/ln/loans/approve.aspx?id={0}&catID={1}" Text="Approve" HeaderText="Approve"></telerik:GridHyperLinkColumn>
                        <telerik:GridBoundColumn DataField="staffName" HeaderText="Rel. Officer"></telerik:GridBoundColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <div class="right">
            <h3 class="headerTitle">Due and Overdue Repayments</h3>
            <telerik:RadGrid ID="gridDue" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="10" AllowPaging="true" 
                AllowSorting="true" OnPageIndexChanged="gridDue_PageIndexChanged" OnSortCommand="gridDue_SortCommand"
                OnLoad="gridDue_Load">
                <MasterTableView ShowFooter="true"> 
                    <Columns>
                        <telerik:GridBoundColumn DataField="clientID" HeaderText="Client ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="loanNo" HeaderText="Loan ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amountDue" HeaderText="Amount Due" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="dateDue" HeaderText="Date Due"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="staffName" HeaderText="Rel. Officer"></telerik:GridBoundColumn> 
                        <telerik:GridHyperLinkColumn DataNavigateUrlFields="loanID" DataNavigateUrlFormatString="~/ln/cashier/receipt.aspx?id={0}" Text="Receive" HeaderText="Receive"></telerik:GridHyperLinkColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
    
    <div class="wrapper">
        <div class="left">
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
                        <telerik:GridHyperLinkColumn DataNavigateUrlFields="loanID,categoryID" DataNavigateUrlFormatString="~/ln/cashier/disburse.aspx?id={0}&catID={1}" Text="Disburse" HeaderText="Disburse"></telerik:GridHyperLinkColumn> 
                        <telerik:GridBoundColumn DataField="staffName" HeaderText="Rel. Officer"></telerik:GridBoundColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <div class="right">
            <h3 class="headerTitle">Top Ten Borrowers</h3>
            <telerik:RadGrid ID="gridTopBorrowers" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="10" AllowPaging="true" 
                AllowSorting="true" OnPageIndexChanged="gridTopBorrowers_PageIndexChanged" OnSortCommand="gridTopBorrowers_SortCommand"
                >
                <MasterTableView ShowFooter="true"> 
                    <Columns>
                        <telerik:GridBoundColumn DataField="loanID" HeaderText="Loan ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amountDisbursed" HeaderText="Amt. Disb." Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="totalDue" HeaderText="Balance Due" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="lastPaymentDate" HeaderText="Last Pmt. Date"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="staffName" HeaderText="Rel. Officer"></telerik:GridBoundColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
    
    <div class="wrapper">
        <div class="left">  
            <telerik:RadHtmlChart runat="server" ID="chComp" Width="600px"> 
                <Appearance>
                        <FillStyle BackgroundColor="White"></FillStyle>
                </Appearance>
                <ChartTitle Text="Disbursements vs Receipts">
                        <Appearance Align="Center" BackgroundColor="White" Position="Top"></Appearance>
                </ChartTitle>
                <Legend>
                        <Appearance BackgroundColor="White" Position="Bottom"></Appearance>
                </Legend>
                <PlotArea>
                        <Appearance>
                            <FillStyle BackgroundColor="White"></FillStyle>
                        </Appearance>
                        <XAxis AxisCrossingValue="0" Color="#b3b3b3" MajorTickType="Outside" MinorTickType="Outside"
                            Reversed="false"> 
                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                            <MajorGridLines Color="#EFEFEF" Width="1"></MajorGridLines>
                            <MinorGridLines Color="#F7F7F7" Width="1"></MinorGridLines>
                            <TitleAppearance Position="Center" RotationAngle="0" Text="Month"></TitleAppearance>
                        </XAxis>
                        <YAxis AxisCrossingValue="0" Color="#b3b3b3" MajorTickSize="1" MajorTickType="Outside"
                            MinorTickSize="1" MinorTickType="Outside" MinValue="0" Reversed="false"
                            >
                            <LabelsAppearance DataFormatString="${0}" RotationAngle="0"></LabelsAppearance>
                            <MajorGridLines Color="#EFEFEF" Width="1"></MajorGridLines>
                            <MinorGridLines Color="#F7F7F7" Width="1"></MinorGridLines>
                            <TitleAppearance Position="Center" RotationAngle="0" Text="Amount"></TitleAppearance>
                        </YAxis>
                        <Series>
                            <telerik:BarSeries Name="Disbursements" Stacked="false">
                                <Appearance>
                                    <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                </Appearance>
                                <LabelsAppearance DataFormatString="¢{0}" Position="Center">
                                </LabelsAppearance>
                                <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="¢{0}" Color="White"></TooltipsAppearance>
                            </telerik:BarSeries>
                            <telerik:BarSeries Name="Receipts">
                                <Appearance>
                                    <FillStyle BackgroundColor="#92b622"></FillStyle>
                                </Appearance>
                                <LabelsAppearance DataFormatString="¢{0}" Position="Center"></LabelsAppearance>
                                <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="¢{0}" Color="White"></TooltipsAppearance>
                            </telerik:BarSeries> 
                        </Series>
                </PlotArea>
            </telerik:RadHtmlChart>
        </div>
        <div class="right"> 
            <telerik:RadHtmlChart runat="server" ID="chCol"  Width="500px">                
                <Appearance>
                        <FillStyle BackgroundColor="White"></FillStyle>
                </Appearance>
                <ChartTitle Text="Collection Ratio by Month">
                        <Appearance Align="Center" BackgroundColor="White" Position="Top">
                        </Appearance>
                </ChartTitle>
                <Legend>
                        <Appearance BackgroundColor="White" Position="Bottom">
                        </Appearance>
                </Legend>
                <PlotArea>
                        <Appearance>
                            <FillStyle BackgroundColor="White"></FillStyle>
                        </Appearance>
                        <XAxis AxisCrossingValue="0" Color="#b3b3b3" MajorTickType="Outside" MinorTickType="Outside"
                            Reversed="false"> 
                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0">
                            </LabelsAppearance>
                            <MajorGridLines Color="#EFEFEF" Width="1"></MajorGridLines>
                            <MinorGridLines Color="#F7F7F7" Width="1"></MinorGridLines>
                            <TitleAppearance Position="Center" RotationAngle="0" Text="Month">
                            </TitleAppearance>
                        </XAxis>
                        <YAxis AxisCrossingValue="0" Color="#b3b3b3" MajorTickSize="1" MajorTickType="Outside"
                            MaxValue="100" MinorTickSize="1" MinorTickType="Outside" MinValue="0" Reversed="false"
                            Step="25">
                            <LabelsAppearance DataFormatString="{0}%" RotationAngle="0">
                            </LabelsAppearance>
                            <MajorGridLines Color="#EFEFEF" Width="1"></MajorGridLines>
                            <MinorGridLines Color="#F7F7F7" Width="1"></MinorGridLines>
                            <TitleAppearance Position="Center" RotationAngle="0" Text="Collection Ratio">
                            </TitleAppearance>
                        </YAxis>
                        <Series>
                            <telerik:LineSeries Name="Collection">
                                <Appearance>
                                    <FillStyle BackgroundColor="#5ab7de"></FillStyle>
                                </Appearance>
                                <LabelsAppearance DataFormatString="{0}%" Position="Above">
                                </LabelsAppearance>
                                <LineAppearance Width="1" />
                                <MarkersAppearance MarkersType="Circle" BackgroundColor="White" Size="8" BorderColor="#5ab7de"
                                    BorderWidth="2"></MarkersAppearance>
                                <TooltipsAppearance DataFormatString="{0}%"></TooltipsAppearance> 
                            </telerik:LineSeries> 
                        </Series>
                </PlotArea>
            </telerik:RadHtmlChart>
        </div>
    </div>
</asp:Content>