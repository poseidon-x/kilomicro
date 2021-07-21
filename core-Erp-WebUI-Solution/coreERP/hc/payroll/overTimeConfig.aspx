<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="overTimeConfig.aspx.cs" Inherits="coreERP.hc.payroll.overTimeConfig" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Over Time Rates
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
        <h3>Manage Staff Over Time Rates </h3> 
      <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Set OverTime Rates
            </div>
        </div>
    </div>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="True" AllowAutomaticDeletes="True" AllowAutomaticUpdates="True"
        AllowSorting="True" ShowFooter="True"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted"
        OnUpdateCommand="RadGrid1_UpdateCommand" AutoGenerateColumns="False">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView EditMode="InPlace"
            datakeynames="overTimeConfigID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px" allowSorting="true" >
    <Columns>
        <telerik:GridBoundColumn DataField="overTimeConfigID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="overTimeConfigID" ReadOnly="True" 
            SortExpression="overTimeConfigID" UniqueName="overTimeConfigID" Visible="False">
        </telerik:GridBoundColumn>
        <telerik:GridDropDownColumn DataField="levelID" DataSourceID="EntityDataSource2" UniqueName="levelID"
             ListValueField="levelID" ListTextField="levelName" HeaderText="Level Name" ></telerik:GridDropDownColumn>         
        <telerik:GridNumericColumn DataField="saturdayHoursRate"  DefaultInsertValue="" 
            HeaderText="Saturday Hours Rate" SortExpression="saturdayHoursRate" UniqueName="saturdayHoursRate" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>  
        <telerik:GridNumericColumn DataField="sundayHoursRate" DefaultInsertValue="" 
            HeaderText="Sunday Hours Rate" SortExpression="sundayHoursRate" UniqueName="sundayHoursRate" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn> 
         <telerik:GridNumericColumn DataField="holidayHoursRate" DefaultInsertValue="" 
            HeaderText="Holiday Hours Rate" SortExpression="holidayHoursRate" UniqueName="holidayHoursRate" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>
        <telerik:GridNumericColumn DataField="weekdayAfterWorkHoursRate" DefaultInsertValue="" 
            HeaderText="Week Day After WorkHours Rate" SortExpression="weekdayAfterWorkHoursRate" UniqueName="weekdayAfterWorkHoursRate" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>
        <telerik:GridNumericColumn DataField="overTime5PerTax" DefaultInsertValue="" 
            HeaderText="(5%) Tax " SortExpression="overTime5PerTax" UniqueName="overTime5PerTax" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>
          <telerik:GridNumericColumn DataField="overTime10PerTax" DefaultInsertValue="" 
            HeaderText="(10%) Tax " SortExpression="overTime10PerTax" UniqueName="overTime10PerTax" DataFormatString="{0:#,###.#0}">
        </telerik:GridNumericColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
            <ItemStyle Height="32px" Width="32px" />
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected over time rate?">
          </telerik:GridButtonColumn>
    </Columns> 
    <EditFormSettings>
        <EditColumn FilterControlAltText="Filter EditCommandColumn1 column" UniqueName="EditCommandColumn1">
        </EditColumn>
    </EditFormSettings>
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
        EntitySetName="overTimeConfigs">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True"  
        EntitySetName="levels"> 
    </ef:EntityDataSource>
    </asp:Content>
