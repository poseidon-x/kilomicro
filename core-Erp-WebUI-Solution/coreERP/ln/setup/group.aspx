<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="group.aspx.cs" Inherits="coreERP.ln.setup.group" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Groups
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
        <h3>Manage groups</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted"
        OnUpdateCommand="RadGrid1_UpdateCommand" MasterTableView-DataKeyNames="groupID">
         
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="groupID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px">
    <DetailTables>
        <telerik:GridTableView DataSourceID="EntityDataSource2" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="groupExecID" Width="800px">
            <CommandItemSettings AddNewRecordText="Add A Group Leader" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="groupID" MasterKeyField="groupID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="groupExecID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="groupExecID" ReadOnly="True" 
                    SortExpression="groupExecID" UniqueName="groupExecID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="surName" DefaultInsertValue="" 
                    HeaderText="Surname" SortExpression="surName" UniqueName="surName" 
                    ItemStyle-Width="200px">
                </telerik:GridBoundColumn>  
                <telerik:GridBoundColumn DataField="otherNames" DefaultInsertValue="" 
                    HeaderText="Other Names" SortExpression="otherNames" UniqueName="otherNames" 
                    ItemStyle-Width="200px">
                </telerik:GridBoundColumn>   
                <telerik:GridBoundColumn DataField="phone.phoneNo" DefaultInsertValue="" 
                    HeaderText="Phone" SortExpression="phone.phoneNo" UniqueName="phone.phoneNo" 
                    ItemStyle-Width="150px">
                </telerik:GridBoundColumn>   
                <telerik:GridBoundColumn DataField="email.emailAddress" DefaultInsertValue="" 
                    HeaderText="Email" SortExpression="email.emailAddress" UniqueName="email.emailAddress" 
                    ItemStyle-Width="150px">
                </telerik:GridBoundColumn>
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected group?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="groupID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="group ID" ReadOnly="True" 
            SortExpression="groupID" UniqueName="groupID" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="groupName" DefaultInsertValue="" 
            HeaderText="group Name" SortExpression="groupName" UniqueName="groupName"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>  
        <telerik:GridNumericColumn DataField="groupSize" DefaultInsertValue="" 
            HeaderText="group Size" SortExpression="groupSize" UniqueName="groupSize"  
            ItemStyle-Width="150px">
        </telerik:GridNumericColumn>  
        <telerik:GridBoundColumn DataField="address.addressLine1" DefaultInsertValue="" 
            HeaderText="Address" SortExpression="address.addressLine1" UniqueName="address.addressLine1"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>  
        <telerik:GridBoundColumn DataField="address.cityTown" DefaultInsertValue="" 
            HeaderText="Location" SortExpression="address.cityTown" UniqueName="address.cityTown"  
            ItemStyle-Width="250px">
        </telerik:GridBoundColumn>    
        <telerik:GridTemplateColumn UniqueName="groupTypeID" SortExpression="groupTypeID" DataField="groupTypeID">
            <HeaderTemplate>
                Group Type
            </HeaderTemplate>
            <ItemTemplate>
                <%# GetGroupType(Eval("groupTypeID")) %>
            </ItemTemplate>
            <EditItemTemplate>
                <telerik:RadComboBox ID="cbo" runat="server" SelectedValue='<%# Bind("groupTypeID") %>'>
                    <Items>
                        <telerik:RadComboBoxItem Value="1" Text="Formal" />
                        <telerik:RadComboBoxItem Value="2" Text="Informal" />
                    </Items>
                </telerik:RadComboBox>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
        ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
        ConfirmText="Are you sure you want to delete the selected group?">
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
        EntitySetName="groups" Include="address">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="groupExecs" Where="it.[group].groupID == @groupID"
        Include="phone,email">
        <WhereParameters>
            <asp:SessionParameter Name="groupID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    </asp:Content>
