<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="assetCategory.aspx.cs" Inherits="coreERP.ln.setup.assetCategory" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Asset Categories
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
        <h3>Manage Asset Categories</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnItemCreated="RadGrid1_ItemCreated"
        OnUpdateCommand="RadGrid1_UpdateCommand" MasterTableView-DataKeyNames="assetCategoryID">
         
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="assetCategoryID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px">
    <DetailTables>
        <telerik:GridTableView DataSourceID="EntityDataSource2" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="assetSubCategoryID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Sub Category" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="assetCategoryID" MasterKeyField="assetCategoryID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="assetSubCategoryID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="assetSubCategoryID" ReadOnly="True" 
                    SortExpression="assetSubCategoryID" UniqueName="assetSubCategoryID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="assetSubCategoryName" DefaultInsertValue="" 
                    HeaderText="Asset Sub Category" SortExpression="assetSubCategoryName" UniqueName="assetSubCategoryName" 
                    ItemStyle-Width="200px">
                </telerik:GridBoundColumn>   
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected assetCategory?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="assetCategoryID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="assetCategory ID" ReadOnly="True" 
            SortExpression="assetCategoryID" UniqueName="assetCategoryID" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="assetCategoryName" DefaultInsertValue="" 
            HeaderText="Asset Category Name" SortExpression="assetCategoryName" UniqueName="assetCategoryName"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn HeaderText="Depreciation Method" UniqueName="depreciationMethod" ItemStyle-Width="150px">
            <ItemTemplate>
                <%# GetDescription(Eval("depreciationMethod")) %>
            </ItemTemplate>
            <EditItemTemplate>
                <telerik:RadComboBox runat="server" ID="cboDepMeth" AutoPostBack="true" OnSelectedIndexChanged="cboDepMeth_SelectedIndexChanged"
                     EmptyMessage="Please select depreciation method">
                    <Items>
                        <telerik:RadComboBoxItem Value="" Text="" />
                        <telerik:RadComboBoxItem Value="1" Text="Straight Line" />
                        <telerik:RadComboBoxItem Value="2" Text="Reducing Balance" />
                    </Items>
                </telerik:RadComboBox>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="depreciationAccountID" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Depreciation Account" 
            SortExpression="depreciationAccountID" UniqueName="depreciationAccountID" ItemStyle-Width="200px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# AccountName(DataBinder.Eval(Container.DataItem, "depreciationAccountID"))%></span>
            </ItemTemplate>
            <EditItemTemplate>  
                 <telerik:RadComboBox ID="depreciationAccountID" runat="server" Height="150px" Width="300px" DataTextField="fullname" DataValueField="acct_id" 
                       DropDownWidth=" 500px" EmptyMessage="Type the number, name of a GL Account" HighlightTemplatedItems="true"
                        MarkFirstMatch="true" AutoCompleteSeparator="" OnItemsRequested="cboGLAcc_ItemsRequested"   
                        EnableLoadOnDemand="true" AutoPostBack="true">
                        <HeaderTemplate>
                            <table style="width: 500px">
                                <tr> 
                                    <td style="width: 80px;">
                                        Acc Num</td>
                                    <td style="width: 320px;">
                                        Acc Name</td>
                                    <td style="width: 100px;">
                                        Acc Currency</td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 500px">
                                <tr>
                                    <td style="width: 80px;" >
                                        <%# Eval("acc_num")%>
                                    </td>
                                    <td style="width: 320px;">
                                        <%# Eval("acc_name")%>
                                    </td> 
                                    <td style="width: 100px;">
                                        <%# Eval("major_name")%>
                                    </td> 
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:RadComboBox>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="accumulatedDepreciationAccountID" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Accum. Depreciation Account" 
            SortExpression="accumulatedDepreciationAccountID" UniqueName="accumulatedDepreciationAccountID" ItemStyle-Width="200px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# AccountName(DataBinder.Eval(Container.DataItem, "accumulatedDepreciationAccountID"))%></span>
            </ItemTemplate>
            <EditItemTemplate>   
                 <telerik:RadComboBox ID="accumulatedDepreciationAccountID" runat="server" Height="150px" Width="300px" DataTextField="fullname" DataValueField="acct_id" 
                       DropDownWidth=" 500px" EmptyMessage="Type the number, name of a GL Account" HighlightTemplatedItems="true"
                        MarkFirstMatch="true" AutoCompleteSeparator="" OnItemsRequested="cboGLAcc_ItemsRequested"   
                        EnableLoadOnDemand="true" AutoPostBack="true">
                        <HeaderTemplate>
                            <table style="width: 500px">
                                <tr> 
                                    <td style="width: 80px;">
                                        Acc Num</td>
                                    <td style="width: 320px;">
                                        Acc Name</td>
                                    <td style="width: 100px;">
                                        Acc Currency</td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 500px">
                                <tr>
                                    <td style="width: 80px;" >
                                        <%# Eval("acc_num")%>
                                    </td>
                                    <td style="width: 320px;">
                                        <%# Eval("acc_name")%>
                                    </td> 
                                    <td style="width: 100px;">
                                        <%# Eval("major_name")%>
                                    </td> 
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:RadComboBox>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
        ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
        ConfirmText="Are you sure you want to delete the selected assetCategory?">
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
        EntitySetName="assetCategories">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="assetSubCategories" Where="it.assetCategory.assetCategoryID == @assetCategoryID"
        >
        <WhereParameters>
            <asp:SessionParameter Name="assetCategoryID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    </asp:Content>
