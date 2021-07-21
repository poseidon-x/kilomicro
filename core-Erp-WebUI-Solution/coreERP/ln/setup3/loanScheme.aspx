<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="loanScheme.aspx.cs" Inherits="coreERP.ln.setup3.loanScheme" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Scheme Loans Configuration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

</telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h3>Scheme Loans Configuration</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted"
        OnUpdateCommand="RadGrid1_UpdateCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="loanSchemeID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px" allowSorting="true">
    <Columns>
        <telerik:GridBoundColumn DataField="loanSchemeID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="loanSchemeID" ReadOnly="True" 
            SortExpression="loanSchemeID" UniqueName="loanSchemeID" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridDropDownColumn DataField="loanTypeId" DataSourceID="EntityDataSource3"
             ListValueField="loanTypeID" ListTextField="loanTypeName" 
            HeaderText="Loan Type" EnableEmptyListItem="true" 
            EmptyListItemText="Select the type of loan" EmptyListItemValue="">
        </telerik:GridDropDownColumn>
        <telerik:GridDropDownColumn DataField="employerId" DataSourceID="EntityDataSource2"
             ListValueField="employerID" ListTextField="employerName" 
            HeaderText="Employer Name" EnableEmptyListItem="true" 
            EmptyListItemText="Select the employer" EmptyListItemValue="">
        </telerik:GridDropDownColumn> 
        <telerik:GridBoundColumn UniqueName="loanSchemeName" DataField="loanSchemeName"
             SortExpression="loanSchemeName" HeaderText="Scheme Name"></telerik:GridBoundColumn>
        <telerik:GridNumericColumn DataField="tenure" DefaultInsertValue="" 
            HeaderText="Tenure" SortExpression="tenure" UniqueName="tenure" 
            DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>   
        <telerik:GridNumericColumn DataField="rate" DefaultInsertValue="" 
            HeaderText="Interest Rate" SortExpression="rate" UniqueName="rate" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>        
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected Loan Scheme?">
          </telerik:GridButtonColumn>
    </Columns> 
</MasterTableView> 
    </telerik:RadGrid>

    <asp:ImageButton ID="btnExcel" Text="Export to Excel" OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" Text="Export to PDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord" Text="Export to Word" OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" />
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="loanSchemes">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="employers" OrderBy="it.employerName ASC">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="loanTypes" OrderBy="it.loanTypeName ASC"> 
    </ef:EntityDataSource>
    </asp:Content>
