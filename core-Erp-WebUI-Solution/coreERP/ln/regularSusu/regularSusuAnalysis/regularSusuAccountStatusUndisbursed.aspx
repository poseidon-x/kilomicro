<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="regularSusuAccountStatusUndisbursed.aspx.cs" Inherits="coreERP.ln.regularSusu.analysis.regularSusuAccountStatusUndisbursed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    <%if (stageID==1) { %>
    Group Contribtuions Performance | Stage 1A
    <%} else if (stageID==1.5) { %>
    Group Contribtuions Performance | Stage 1B
    <%} else if (stageID==2) { %>
    Group Contribtuions Performance | Stage 2
    <%} else { %>
    Group Contribtuions Performance 
    <%} %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Report as At
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker  runat="server" CssClass="inputControl" ID="dtDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
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
                <telerik:GridBoundColumn DataField="clientName" HeaderText="Client Name" ItemStyle-Width="200px" SortExpression="clientName"></telerik:GridBoundColumn>
                <telerik:GridDateTimeColumn DataField="startDate" HeaderText="Start Date" ItemStyle-Width ="70px" DataFormatString="{0:dd-MMM-yyyy}" SortExpression="applicationDate"></telerik:GridDateTimeColumn>
                <telerik:GridDateTimeColumn DataField="entitledDate" HeaderText="Due Date" ItemStyle-Width ="70px" DataFormatString="{0:dd-MMM-yyyy}" SortExpression="entitledDate"></telerik:GridDateTimeColumn>
                <telerik:GridTemplateColumn HeaderText="Date Coll." ItemStyle-Width ="70px" SortExpression="disbursementDate" UniqueName="disbursementDate">
                    <ItemTemplate>
                        <%# GetDate(Eval("disbursementDate")) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="netAmountEntitled" HeaderText="Amount Payable 6 months" ItemStyle-Width="100px" SortExpression="netAmountEntitled" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="allContributions" DataField="allContributions" HeaderText="Amount Paid 6 months" ItemStyle-Width="100px" SortExpression="allContributions" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="allExpected" HeaderText="Amount Recble 6 months" ItemStyle-Width="100px" SortExpression="allExpected" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="periodExpected" HeaderText="Expected Cont. per Period" ItemStyle-Width="100px" SortExpression="periodExpected" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="periodContributions" HeaderText="Actual Cont. per Period" ItemStyle-Width="100px" SortExpression="periodContributions" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
                <telerik:GridCalculatedColumn Expression="periodExpected - periodContributions" HeaderText="Variance" ItemStyle-Width="100px" SortExpression="periodContributions" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" ></telerik:GridCalculatedColumn>
                <telerik:GridCalculatedColumn Expression="allExpected - periodContributions" HeaderText="Projected Variance" ItemStyle-Width="100px" SortExpression="periodContributions" DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}" ></telerik:GridCalculatedColumn>
                <telerik:GridBoundColumn DataField="daysDelayed" HeaderText="Days Del." ItemStyle-Width="70px" SortExpression="daysDelayed" DataFormatString="{0:#,##0;(#,##0);0.0}"></telerik:GridBoundColumn>
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
