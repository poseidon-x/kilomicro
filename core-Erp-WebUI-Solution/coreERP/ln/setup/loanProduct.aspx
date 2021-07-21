<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="loanProduct.aspx.cs" Inherits="coreERP.ln.setup.loanProduct" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Loan Products
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script type="text/javascript">
   var popUp;
   function PopUpShowing(sender, eventArgs)
   {
       popUp = eventArgs.get_popUp();
       var gridWidth = sender.get_element().offsetWidth;
       var gridHeight = sender.get_element().offsetHeight;
       var popUpWidth = popUp.style.width.substr(0,popUp.style.width.indexOf("px"));
       var popUpHeight = popUp.style.height.substr(0,popUp.style.height.indexOf("px"));
       popUp.style.left = ((gridWidth - popUpWidth)/2 + sender.get_element().offsetLeft).toString() + "px";
       popUp.style.top = ((gridHeight - popUpHeight)/2 + sender.get_element().offsetTop).toString() + "px";
   }
 
            function mngRequestStarted(ajaxManager, eventArgs)
            {
                if (eventArgs.EventTarget == "btnExcel" || eventArgs.EventTarget == "btnWord" || eventArgs.EventTarget == "btnPDF")
              {
                 eventArgs.EnableAjax = false;
              }
            }
            function pnlRequestStarted(ajaxPanel, eventArgs)
            {
              if(eventArgs.EventTarget == "pnlBtnExcel" || eventArgs.EventTarget == "pnlBtnWord")
              {
                 eventArgs.EnableAjax = false;
              }
            }
            function gridRequestStart(grid, eventArgs)
            {
              if((eventArgs.EventTarget.indexOf("gridBtnExcel") != -1) || (eventArgs.EventTarget.indexOf("gridBtnWord") != -1))
              {
                 eventArgs.EnableAjax = false;
              }
            }
      </script>
</telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h3>Manage Loan Products</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted"
        OnUpdateCommand="RadGrid1_UpdateCommand" MasterTableView-DataKeyNames="loanProductID">
         
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="loanProductID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px" >
            <CommandItemSettings AddNewRecordText="Add A Loan Product Item" />      
    <DetailTables>
        <telerik:GridTableView DataSourceID="EntityDataSource2" AllowAutomaticDeletes="false" 
         AllowAutomaticInserts="false" AllowAutomaticUpdates="false" AutoGenerateColumns="false" 
         CommandItemDisplay="Top"  DataKeyNames="loanProductHistoryID">
            <CommandItemSettings AddNewRecordText="Add A Detail Purpose Item" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="loanProductID" MasterKeyField="loanProductID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="loanProductHistoryID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="loanProductHistoryID" ReadOnly="True" 
                    SortExpression="loanProductHistoryID" UniqueName="loanProductHistoryID" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="loanProductName" DefaultInsertValue="" 
                    HeaderText="Loan Product" SortExpression="loanProductName" UniqueName="loanProductName" ItemStyle-Width="320px">
                </telerik:GridBoundColumn>  
                <telerik:GridNumericColumn DataField="loanTenure" DefaultInsertValue="" 
                    HeaderText="Tenure" SortExpression="loanTenure" UniqueName="loanTenure" 
                    ItemStyle-Width="100px">
                </telerik:GridNumericColumn>  
                <telerik:GridNumericColumn DataField="rate" DefaultInsertValue="" 
                    HeaderText="Interest Rate" SortExpression="rate" UniqueName="rate" 
                    ItemStyle-Width="100px">
                </telerik:GridNumericColumn>  
                <telerik:GridNumericColumn DataField="minAge" DefaultInsertValue="" 
                    HeaderText="Min. Qual. Age" SortExpression="minAge" UniqueName="minAge" 
                    ItemStyle-Width="100px">
                </telerik:GridNumericColumn> 
                <telerik:GridNumericColumn DataField="maxAge" DefaultInsertValue="" 
                    HeaderText="Max. Qual. Age" SortExpression="maxAge" UniqueName="maxAge" 
                    ItemStyle-Width="100px">
                </telerik:GridNumericColumn>  
                <telerik:GridDateTimeColumn DataField="archiveDate" DefaultInsertValue="" DataFormatString="{0:dd-MMM-yyyy}"
                    HeaderText="Archived On" SortExpression="archiveDate" UniqueName="archiveDate" 
                    ItemStyle-Width="100px">
                </telerik:GridDateTimeColumn> 
            </Columns>
        </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="loanProductID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="Product ID" ReadOnly="True" 
            SortExpression="loanProductID" UniqueName="loanProductID" visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="loanProductName" DefaultInsertValue="" 
            HeaderText="Loan Product" SortExpression="loanProductName" UniqueName="loanProductName" ItemStyle-Width="320px">
        </telerik:GridBoundColumn>  
        <telerik:GridNumericColumn DataField="loanTenure" DefaultInsertValue="" 
            HeaderText="Tenure" SortExpression="loanTenure" UniqueName="loanTenure" 
            ItemStyle-Width="100px">
        </telerik:GridNumericColumn>  
        <telerik:GridNumericColumn DataField="rate" DefaultInsertValue="" 
            HeaderText="Interest Rate" SortExpression="rate" UniqueName="rate" 
            ItemStyle-Width="100px">
        </telerik:GridNumericColumn>  
        <telerik:GridNumericColumn DataField="procFeeRate" DefaultInsertValue="" 
            HeaderText="Proc. Fee Rate" SortExpression="procFeeRate" UniqueName="procFeeRate" 
            ItemStyle-Width="100px">
        </telerik:GridNumericColumn>   
        <telerik:GridNumericColumn DataField="minAge" DefaultInsertValue="" 
            HeaderText="Min. Qual. Age" SortExpression="minAge" UniqueName="minAge" 
            ItemStyle-Width="100px">
        </telerik:GridNumericColumn> 
        <telerik:GridNumericColumn DataField="maxAge" DefaultInsertValue="" 
            HeaderText="Max. Qual. Age" SortExpression="maxAge" UniqueName="maxAge" 
            ItemStyle-Width="100px">
        </telerik:GridNumericColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
    </Columns> 
</MasterTableView>
       <ClientSettings>
           <ClientEvents OnPopUpShowing="PopUpShowing"/>
           <Selecting AllowRowSelect="true" />
           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
           <Selecting AllowRowSelect="true" />
       </ClientSettings>
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
        EntitySetName="loanProducts">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="loanProductHistories" Where="it.loanProduct.loanProductID == @loanProductID">
        <WhereParameters>
            <asp:SessionParameter Name="loanProductID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    </asp:Content>
