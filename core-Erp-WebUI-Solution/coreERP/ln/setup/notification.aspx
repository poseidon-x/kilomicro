<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="notification.aspx.cs" Inherits="coreERP.ln.setup.notification" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Notifications
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
        <h3>Manage Notifications</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true" AllowPaging="true" 
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnItemCreated="RadGrid1_ItemCreated"
        OnUpdateCommand="RadGrid1_UpdateCommand" MasterTableView-DataKeyNames="notificationID">
         
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="notificationID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px">
    <DetailTables>
        <telerik:GridTableView DataSourceID="EntityDataSource2" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="notificationScheduleID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Schedule" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="notificationID" MasterKeyField="notificationID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="notificationScheduleID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="notificationScheduleID" ReadOnly="True" 
                    SortExpression="notificationScheduleID" UniqueName="notificationScheduleID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridTemplateColumn DataField="dayOfWeekID" DefaultInsertValue="" 
                    HeaderText="Day of Week" SortExpression="dayOfWeekID" UniqueName="dayOfWeekID" 
                    ItemStyle-Width="200px"> 
                    <ItemTemplate>
                        <%# GetDayOfWeek(Eval("dayOfWeekID")) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadComboBox runat="server" id="dayOfWeekID" SelectedValue='<%# Bind("dayOfWeekID") %>'></telerik:RadComboBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn> 
                <telerik:GridTemplateColumn DataField="frequencyID" DefaultInsertValue="" 
                    HeaderText="Schedule Frequency" SortExpression="frequencyID" UniqueName="frequencyID" 
                    ItemStyle-Width="200px"> 
                    <ItemTemplate>
                        <%# GetFrequency(Eval("frequencyID")) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadComboBox runat="server" id="frequencyID" SelectedValue='<%# Bind("frequencyID") %>'></telerik:RadComboBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>    
                <telerik:GridBoundColumn DataField="startTime" DefaultInsertValue="" 
                    HeaderText="Start Time" SortExpression="startTime" UniqueName="startTime" 
                    ItemStyle-Width="200px">
                </telerik:GridBoundColumn>     
                <telerik:GridBoundColumn DataField="endTime" DefaultInsertValue="" 
                    HeaderText="End Time" SortExpression="endTime" UniqueName="endTime" 
                    ItemStyle-Width="200px">
                </telerik:GridBoundColumn>    
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected schedule?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
        <telerik:GridTableView DataSourceID="EntityDataSource3" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="notificationRecipientID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Recipient" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="notificationID" MasterKeyField="notificationID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="notificationRecipientID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="notificationRecipientID" ReadOnly="True" 
                    SortExpression="notificationRecipientID" UniqueName="notificationRecipientID" Visible="false">
                </telerik:GridBoundColumn>  
                <telerik:GridTemplateColumn DataField="staffID" DefaultInsertValue="" 
                    HeaderText="Staff Name" SortExpression="staffID" UniqueName="staffID" 
                    ItemStyle-Width="450px"> 
                    <ItemTemplate>
                        <%# GetStaffName(Eval("staffID")) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadComboBox runat="server" id="staffID" SelectedValue='<%# Bind("staffID") %>'></telerik:RadComboBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>  
                <telerik:GridBoundColumn DataField="email" DefaultInsertValue="" 
                    HeaderText="Email Address" SortExpression="email" UniqueName="email" 
                    ItemStyle-Width="200px">
                </telerik:GridBoundColumn>     
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected recipient?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="notificationID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="notificationID" ReadOnly="True" 
            SortExpression="notificationID" UniqueName="notificationID" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="notificationCode" DefaultInsertValue="" 
            HeaderText="Code" SortExpression="notificationCode" UniqueName="notificationCode" 
            ItemStyle-Width="200px">
        </telerik:GridBoundColumn>   
        <telerik:GridBoundColumn DataField="notificationName" DefaultInsertValue="" 
            HeaderText="Notification Name" SortExpression="notificationName" UniqueName="notificationName" 
            ItemStyle-Width="300px">
        </telerik:GridBoundColumn>    
        <telerik:GridBoundColumn DataField="description" DefaultInsertValue="" 
            HeaderText="Description" SortExpression="description" UniqueName="description" 
            ItemStyle-Width="400px">
        </telerik:GridBoundColumn>    
        <telerik:GridCheckBoxColumn DataField="isActive" DefaultInsertValue="" 
            HeaderText="Is Active" SortExpression="isActive" UniqueName="isActive"  >
        </telerik:GridCheckBoxColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
        ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
        ConfirmText="Are you sure you want to delete the selected employer?">
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
        EntitySetName="notifications">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="notificationSchedules" Where="it.notification.notificationID == @notificationID">
        <WhereParameters>
            <asp:SessionParameter Name="notificationID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="notificationRecipients" Where="it.notification.notificationID == @notificationID"
        >
        <WhereParameters>
            <asp:SessionParameter Name="notificationID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    </asp:Content>
