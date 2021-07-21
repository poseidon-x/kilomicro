<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="susuPosition.aspx.cs" Inherits="coreERP.ln.setup.susuPosition" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Susu Positions
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
        <h3>Manage Susu Positions</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted"
        OnUpdateCommand="RadGrid1_UpdateCommand" MasterTableView-DataKeyNames="susuPositionID">
         
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="susuPositionID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="760px">
    <DetailTables>
        <telerik:GridTableView DataSourceID="EntityDataSource2" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="susuGradePositionID">
            <CommandItemSettings AddNewRecordText="Attach a Grade to this Position" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="susuPositionID" MasterKeyField="susuPositionID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="susuGradePositionID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="susuGradePositionID" ReadOnly="True" 
                    SortExpression="susuGradePositionID" UniqueName="susuGradePositionID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridDropDownColumn DataField="susuGradeID" DataSourceID="eds2" ListTextField="susuGradeName"
                     ListValueField="susuGradeID" ItemStyle-Width="300px" HeaderText="Susu Grade" EmptyListItemText="(Select a Grade)" EmptyListItemValue="0"> 
                </telerik:GridDropDownColumn>
                <telerik:GridNumericColumn DataField="amountEntitled" DefaultInsertValue="" 
                    HeaderText="Entitlement Amount" SortExpression="amountEntitled" UniqueName="amountEntitled" ItemStyle-Width="200px">
                </telerik:GridNumericColumn>   
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to remove the selected Susu Grade from this Susu Position?">
                </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="susuPositionID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="susuPosition ID" ReadOnly="True"  Visible="false"
            SortExpression="susuPositionID" UniqueName="susuPositionID">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="susuPositionNo" DefaultInsertValue="" 
            HeaderText="Position Number" SortExpression="susuPositionNo" UniqueName="susuPositionNo" ItemStyle-Width="120px">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="susuPositionName" DefaultInsertValue="" 
            HeaderText="Name (Description)" SortExpression="susuPositionName" UniqueName="susuPositionName" ItemStyle-Width="320px">
        </telerik:GridBoundColumn>  
        <telerik:GridNumericColumn DataField="noOfWaitingPeriods" DefaultInsertValue="" 
            HeaderText="Waiting Periods" SortExpression="noOfWaitingPeriods" UniqueName="noOfWaitingPeriods" ItemStyle-Width="100px">
        </telerik:GridNumericColumn>
        <telerik:GridNumericColumn DataField="percentageInterest" DefaultInsertValue="" 
            HeaderText="Int. Rate" SortExpression="percentageInterest" UniqueName="percentageInterest" ItemStyle-Width="100px">
        </telerik:GridNumericColumn>
        <telerik:GridNumericColumn DataField="maxDefaultDays" DefaultInsertValue="" 
            HeaderText="Max Default Days" SortExpression="maxDefaultDays" UniqueName="maxDefaultDays" ItemStyle-Width="100px">
        </telerik:GridNumericColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to remove the selected Susu Position?">
        </telerik:GridButtonColumn>
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
        EntitySetName="susuPositions">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="susuGradePositions" Where="it.susuPositionID == @susuPositionID">
        <WhereParameters>
            <asp:SessionParameter Name="susuPositionID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="eds2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="susuGrades">
    </ef:EntityDataSource>
    </asp:Content>
