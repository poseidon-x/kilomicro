<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="eod.aspx.cs" Inherits="coreERP.ln.proc.eod" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    End of Day Procedure
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>End of Day Procedure</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Start Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtStartDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" ></telerik:RadDatePicker>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                End Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtEndDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" ></telerik:RadDatePicker>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                <telerik:RadButton runat="server" ID="btnProc" Text="Process" OnClick="btnProc_Click"></telerik:RadButton>
            </div>
            <div class="subFormInput"> 
            </div>
        </div>
    </div> 
    <div class="subFormLabel">
        <telerik:RadButton runat="server" ID="btnSave" Text="Save Proposed Changes" OnClick="btnSave_Click" Enabled="false"></telerik:RadButton>
    </div>
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Proposed Additional Interest" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Group Susu Dormant Accounts">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Group Susu Expired Accounts">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Savings Accounts Interests">   
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
     <telerik:RadMultiPage runat="server"  ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView runat="server">              
            <telerik:RadGrid ID="gridInterest" runat="server" AutoGenerateColumns="false" AllowPaging="true" OnLoad="gridInterest_Load"
                 AllowSorting="true">
                <MasterTableView>
                    <Columns>
                        <telerik:GridDateTimeColumn DataField="PenaltyDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Client ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LoanNo" HeaderText="Loan ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name" ItemStyle-Width="300"></telerik:GridBoundColumn> 
                        <telerik:GridNumericColumn DataField="oldBalance" HeaderText="Principal" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="additionalInterest" HeaderText="Interest" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="remainingBalance" HeaderText="Balance" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn> 
                        <telerik:GridNumericColumn DataField="newPenaltyAmount" HeaderText="Penalty" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView runat="server">              
            <telerik:RadGrid ID="gridDormant" runat="server" AutoGenerateColumns="false" AllowPaging="true" OnLoad="gridDormant_Load"
                AllowSorting="true">
                <MasterTableView>
                    <Columns>
                        <telerik:GridDateTimeColumn DataField="StatusDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Client ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SusuAccountNo" HeaderText="Account ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name" ItemStyle-Width="300"></telerik:GridBoundColumn> 
                        <telerik:GridCheckBoxColumn DataField="isDormant" HeaderText="Dormant"></telerik:GridCheckBoxColumn>
                        <telerik:GridNumericColumn DataField="ContributtionAmount" HeaderText="Cont. Rate" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="AmountContributed" HeaderText="Amount Contributed" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="DaysInDefault" HeaderText="Days In Default" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView runat="server">              
            <telerik:RadGrid ID="gridExpired" runat="server" AutoGenerateColumns="false" AllowPaging="true" OnLoad="gridExpired_Load"
                AllowSorting="true">
                <MasterTableView>
                    <Columns>
                        <telerik:GridDateTimeColumn DataField="StatusDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Client ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SusuAccountNo" HeaderText="Account ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name" ItemStyle-Width="300"></telerik:GridBoundColumn>  
                        <telerik:GridNumericColumn DataField="ContributtionAmount" HeaderText="Cont. Rate" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="AmountExpected" HeaderText="Amount Expected" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn> 
                        <telerik:GridNumericColumn DataField="AmountContributed" HeaderText="Amount Contributed" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="Balance" HeaderText="Balance" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="InterestAmount" HeaderText="Interest" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>  
                        <telerik:GridNumericColumn DataField="AmountCollected" HeaderText="Amount Collected" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView runat="server">              
            <telerik:RadGrid ID="gridSaving" runat="server" AutoGenerateColumns="false" AllowPaging="true" OnLoad="gridSaving_Load"
                AllowSorting="true">
                <MasterTableView>
                    <Columns>
                        <telerik:GridDateTimeColumn DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="AccountNumber" HeaderText="Client ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SavingNo" HeaderText="Account ID"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name" ItemStyle-Width="300"></telerik:GridBoundColumn>  
                        <telerik:GridNumericColumn DataField="OriginalPrincipalBalance" HeaderText="Princ. Bal." DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="OriginalInterestBalance" HeaderText="Int. Bal." DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn> 
                        <telerik:GridNumericColumn DataField="InterestAmount" HeaderText="Interest" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
     </telerik:RadMultiPage>
</asp:Content>
