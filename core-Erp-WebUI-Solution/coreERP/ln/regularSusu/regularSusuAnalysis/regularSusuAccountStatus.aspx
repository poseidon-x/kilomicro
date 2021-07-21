<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="regularSusuAccountStatus.aspx.cs" Inherits="coreERP.ln.regularSusu.analysis.regularSusuAccountStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Normal Susu Account Analysis
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Normal Susu Account Analysis</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Report as At
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker  runat="server" CssClass="inputControl" ID="dtDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Stage
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboStage" CssClass="inputControl">
                    <Items>
                        <telerik:RadComboBoxItem Value="1" Text="All Stages" Selected="true" />
                        <telerik:RadComboBoxItem Value="2" Text="Stage One" />
                        <telerik:RadComboBoxItem Value="3" Text="Stage Two" />
                        <telerik:RadComboBoxItem Value="4" Text="Due for Disbursement" />
                        <telerik:RadComboBoxItem Value="5" Text="Expired" />
                    </Items>
                </telerik:RadComboBox>
            </div>
        </div>
    </div>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <asp:Button runat="server" ID="btnShow" Text="Show Report Data" OnClick="btnShow_Click" />
        </div>
    </div>
    <telerik:RadGrid runat="server" ID="grid" AllowAutomaticDeletes="false" AllowAutomaticInserts="false" AllowAutomaticUpdates="false"
         AllowPaging="true" PageSize="20" AutoGenerateColumns="false" GroupingEnabled="true" AllowSorting="true" OnLoad="grid_Load"
         OnItemDataBound="grid_ItemDataBound" PagerStyle-Mode="NextPrevNumericAndAdvanced" ShowFooter="true" OnNeedDataSource="grid_NeedDataSource"
         GridLines="None" OnItemCommand="grid_ItemCommand">
        <MasterTableView DataKeyNames="regularSusuAccountID" ShowFooter="true" CommandItemDisplay="TopAndBottom">
            <Columns>
                <telerik:GridImageColumn DataImageUrlFields="clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=42&h=42" HeaderText="P" ImageWidth="42" ImageHeight="42" ItemStyle-Width="42"></telerik:GridImageColumn>
                <telerik:GridBoundColumn DataField="regularSusuAccountNo" HeaderText="Account Number" ItemStyle-Width="100px" SortExpression="susuAccountNo"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name" ItemStyle-Width="250px" SortExpression="clientName"></telerik:GridBoundColumn>
                <telerik:GridDateTimeColumn DataField="applicationDate" HeaderText="Application Date" ItemStyle-Width ="100px" DataFormatString="{0:dd-MMM-yyyy}" SortExpression="applicationDate"></telerik:GridDateTimeColumn>
                <telerik:GridBoundColumn DataField="amountEntitled" HeaderText="Expected Contributions" ItemStyle-Width="100px" SortExpression="amountEntitled" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="contributionsMade" HeaderText="Actual Contributions" ItemStyle-Width="100px" SortExpression="contributionsMade" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="contributionsDefaulted" HeaderText="Defaulted Contributions" ItemStyle-Width="100px" SortExpression="contributionsExpected" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="contributionsExpected" HeaderText="Min. Cont. for Payout" ItemStyle-Width="100px" SortExpression="contributionsExpected" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="interestAmount" HeaderText="Profit/Loss" ItemStyle-Width="100px" SortExpression="interestAmount" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="regularSusCommissionAmount" HeaderText="Commission" ItemStyle-Width="100px" SortExpression="commissionAmount" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="netAmountEntitled" HeaderText="Amount Entitled" ItemStyle-Width="100px" SortExpression="netAmountEntitled" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridDateTimeColumn DataField="entitledDate" HeaderText="Due Date" ItemStyle-Width ="100px" DataFormatString="{0:dd-MMM-yyyy}" SortExpression="entitledDate"></telerik:GridDateTimeColumn>
                <telerik:GridDateTimeColumn DataField="dueDate" HeaderText="Expiry Date" ItemStyle-Width ="100px" DataFormatString="{0:dd-MMM-yyyy}" SortExpression="dueDate"></telerik:GridDateTimeColumn>
            </Columns>
            <CommandItemSettings  ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToWordButton="true" />    
        </MasterTableView>
        <ClientSettings AllowColumnsReorder="True" AllowDragToGroup="True" ReorderColumnsOnClient="True"></ClientSettings>
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" UseItemStyles="false" ExportOnlyData="true">
            <Pdf PageHeight="210mm" PageWidth="297mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="45mm"
                BorderStyle="Medium" BorderColor="#666666">
            </Pdf>
            <Excel Format="ExcelML" />
        </ExportSettings>
    </telerik:RadGrid> 
</asp:Content>
