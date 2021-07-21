<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="performanceArea.aspx.cs" Inherits="coreERP.hc.setup.performanceArea" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Performance Areas
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
        <h3>Manage Performance Areas</h3> 
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted"
        OnUpdateCommand="RadGrid1_UpdateCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
            <MasterTableView autogeneratecolumns="False" EditMode="InPlace"
                        datakeynames="performanceAreaID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
                           CommandItemDisplay="Top" Width="960px" allowSorting="true">
                <Columns>
                    <telerik:GridBoundColumn DataField="performanceAreaID" DataType="System.Int32" 
                        DefaultInsertValue="" HeaderText="performanceAreaID" ReadOnly="True" 
                        SortExpression="performanceAreaID" UniqueName="performanceAreaID" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="performanceAreaName" DefaultInsertValue="" 
                        HeaderText="Performance Area" SortExpression="performanceAreaName" UniqueName="performanceAreaName" ItemStyle-Width="220px">
                    </telerik:GridBoundColumn> 
                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                        ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                        ItemStyle-Width="32px" ItemStyle-Height="32px" >
                    </telerik:GridEditCommandColumn>
                      <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                        ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                        ConfirmText="Are you sure you want to delete the selected Performance Area?">
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
    </telerik:RadAjaxPanel>
    <asp:ImageButton ID="btnExcel" Text="Export to Excel" OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" Text="Export to PDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord" Text="Export to Word" OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" />
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="performanceAreas">
    </ef:EntityDataSource>
    </asp:Content>
