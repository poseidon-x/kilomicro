<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="level.aspx.cs" Inherits="coreERP.hc.setup.level" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Employee Levels
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
        <h3>Manage Level of Employees</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true" AllowPaging="true" 
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnItemCreated="RadGrid1_ItemCreated"
        OnUpdateCommand="RadGrid1_UpdateCommand" MasterTableView-DataKeyNames="levelID">
         
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="levelID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px">
    <DetailTables>
        <telerik:GridTableView DataSourceID="EntityDataSource4" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="levelNotchID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Notch to this Level" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="levelID" MasterKeyField="levelID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="levelNotchID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="levelNotchID" ReadOnly="True" 
                    SortExpression="levelNotchID" UniqueName="levelNotchID" Visible="false">
                </telerik:GridBoundColumn>  
                <telerik:GridBoundColumn DataField="notchName" UniqueName="notchName" SortExpression="notchName"
                     HeaderText="Notch Name">
                </telerik:GridBoundColumn> 
                <telerik:GridNumericColumn DataField="sortOrder" UniqueName="sortOrder" SortExpression="sortOrder"
                     HeaderText="Sort Order">
                </telerik:GridNumericColumn>
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected notch?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
        <telerik:GridTableView DataSourceID="EntityDataSource2" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="levelAllowanceID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add An Allowance for this Level" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="levelID" MasterKeyField="levelID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="levelAllowanceID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="levelAllowanceID" ReadOnly="True" 
                    SortExpression="levelAllowanceID" UniqueName="levelAllowanceID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridDropDownColumn DataField="allowanceTypeID" DataSourceID="EntityDatasource7" ListTextField="allowanceTypeName"
                     ListValueField="allowanceTypeID" HeaderText="Allowance Type Name" EnableEmptyListItem="true"
                     EmptyListItemText="Select an allowance type" EmptyListItemValue="0">
                </telerik:GridDropDownColumn>  
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected level allowance?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
        <telerik:GridTableView DataSourceID="EntityDataSource3" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="levelDeductionID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Deduction for this Level" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="levelID" MasterKeyField="levelID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="levelDeductionID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="levelDeductionID" ReadOnly="True" 
                    SortExpression="levelDeductionID" UniqueName="levelDeductionID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridDropDownColumn DataField="deductionTypeID" DataSourceID="EntityDatasource8" ListTextField="deductionTypeName"
                     ListValueField="deductionTypeID" HeaderText="Deduction Type Name" EnableEmptyListItem="true"
                     EmptyListItemText="Select an deduction type" EmptyListItemValue="0">
                </telerik:GridDropDownColumn>  
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected level deduction?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
        <telerik:GridTableView DataSourceID="EntityDataSource5" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="levelBenefitsInKindID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Benefit in Kind for this Level" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="levelID" MasterKeyField="levelID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="levelBenefitsInKindID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="levelBenefitsInKindID" ReadOnly="True" 
                    SortExpression="levelBenefitsInKindID" UniqueName="levelBenefitsInKindID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridDropDownColumn DataField="benefitsInKindID" DataSourceID="EntityDatasource9" ListTextField="benefitsInKindName"
                     ListValueField="benefitsInKindID" HeaderText="Benefits In Kind Name" EnableEmptyListItem="true"
                     EmptyListItemText="Select a benefit in kind" EmptyListItemValue="0">
                </telerik:GridDropDownColumn>  
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected level benefit in kind?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
        <telerik:GridTableView DataSourceID="EntityDataSource6" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="levelLeaveID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Leave Type for this Level" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="levelID" MasterKeyField="levelID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="levelLeaveID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="levelLeaveID" ReadOnly="True" 
                    SortExpression="levelLeaveID" UniqueName="levelLeaveID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridDropDownColumn DataField="leaveTypeID" DataSourceID="EntityDatasource10" ListTextField="leaveTypeName"
                     ListValueField="leaveTypeID" HeaderText="Leave Type Name" EnableEmptyListItem="true"
                     EmptyListItemText="Select a leave type" EmptyListItemValue="0">
                </telerik:GridDropDownColumn>  
                <telerik:GridBoundColumn DataField="maxDaysPerAnnum" UniqueName="maxDaysPerAnnum" SortExpression="maxDaysPerAnnum"
                     HeaderText="Max Days Per Annum">
                </telerik:GridBoundColumn>
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected level leave?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="levelID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="levelID" ReadOnly="True" 
            SortExpression="levelID" UniqueName="levelID" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="levelName" DefaultInsertValue="" 
            HeaderText="Level Name" SortExpression="levelName" UniqueName="levelName"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>  
        <telerik:GridBoundColumn DataField="sortOrder" DefaultInsertValue="" 
            HeaderText="Sort Order" SortExpression="sortOrder" UniqueName="sortOrder"  
            ItemStyle-Width="250px"> 
        </telerik:GridBoundColumn>  
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
        ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
        ConfirmText="Are you sure you want to delete the selected level?">
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
        EntitySetName="levels">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="levelAllowances" Where="it.level.levelID == @levelID">
        <WhereParameters>
            <asp:SessionParameter Name="levelID" Type="Int32" />
        </WhereParameters> 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="levelDeductions" Where="it.level.levelID == @levelID">
        <WhereParameters>
            <asp:SessionParameter Name="levelID" Type="Int32" />
        </WhereParameters> 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource4" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="levelNotches" Where="it.level.levelID == @levelID">
        <WhereParameters>
            <asp:SessionParameter Name="levelID" Type="Int32" />
        </WhereParameters> 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource5" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="levelBenefitsInKinds" Where="it.level.levelID == @levelID">
        <WhereParameters>
            <asp:SessionParameter Name="levelID" Type="Int32" />
        </WhereParameters> 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource6" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="levelLeaves" Where="it.level.levelID == @levelID">
        <WhereParameters>
            <asp:SessionParameter Name="levelID" Type="Int32" />
        </WhereParameters> 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource7" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="allowanceTypes" > 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource8" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="deductionTypes" > 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource9" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="benefitsInKind" > 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource10" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="leaveTypes" > 
    </ef:EntityDataSource>
    </asp:Content>
