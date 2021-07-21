<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="invoiceLoanConfig.aspx.cs" Inherits="coreERP.ln.setup2.invoiceLoanConfig" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Invoice Loan Configuration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h3>Invoice Loan Configuration</h3>  
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted"
        OnUpdateCommand="RadGrid1_UpdateCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="invoiceLoanConfigID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px" allowSorting="true">
    <Columns>
        <telerik:GridBoundColumn DataField="invoiceLoanConfigID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="invoiceLoanConfigID" ReadOnly="True" 
            SortExpression="invoiceLoanConfigID" UniqueName="invoiceLoanConfigID" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridDropDownColumn DataField="clientID" DataSourceID="EntityDataSource3"
             ListValueField="clientID" ListTextField="clientName" HeaderText="Client Name">
        </telerik:GridDropDownColumn>
        <telerik:GridDropDownColumn DataField="supplierID" DataSourceID="EntityDataSource2"
             ListValueField="supplierID" ListTextField="supplierName" HeaderText="Supplier Name">
        </telerik:GridDropDownColumn> 
        <telerik:GridNumericColumn DataField="ceilRate" DefaultInsertValue="" 
            HeaderText="Disbursement Rate" SortExpression="ceilRate" UniqueName="ceilRate" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>   
        <telerik:GridNumericColumn DataField="standardInterestrate" DefaultInsertValue="" 
            HeaderText="Interest Rate" SortExpression="standardInterestrate" UniqueName="standardInterestrate" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>       
        <telerik:GridNumericColumn DataField="standardProcessingFeerate" DefaultInsertValue="" 
            HeaderText="Proc. Fee Rate" SortExpression="standardProcessingFeerate" UniqueName="standardProcessingFeerate" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>        
        <telerik:GridCheckBoxColumn DataField="allowPODisbursement" DefaultInsertValue="" 
            HeaderText="Allow Use of PO" SortExpression="allowPODisbursement" UniqueName="allowPODisbursement">
        </telerik:GridCheckBoxColumn>        
        <telerik:GridNumericColumn DataField="maximumExposure" DefaultInsertValue="" 
            HeaderText="Max. Exposure" SortExpression="maximumExposure" UniqueName="maximumExposure" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>      
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected Allowance Type?">
          </telerik:GridButtonColumn>
    </Columns> 
</MasterTableView>
       <ClientSettings>
           <ClientEvents />
           <Selecting AllowRowSelect="true" />
           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
           <Selecting AllowRowSelect="true" />
       </ClientSettings>
    </telerik:RadGrid> 
    <asp:ImageButton ID="btnExcel" OnClick="btnExcel_Click" runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" OnClick="btnPDF_Click" runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord" OnClick="btnWord_Click" runat="server" ImageUrl="~/images/word.jpg" />
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="invoiceLoanConfigs">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="suppliers">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server" 
        ConnectionString="name=reportEntities" DefaultContainerName="reportEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="vwClients" OrderBy="it.clientName ASC"> 
    </ef:EntityDataSource>
    </asp:Content>
