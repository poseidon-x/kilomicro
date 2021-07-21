<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="True" CodeBehind="sup.aspx.cs" Inherits="coreERP.ap.sup" %>
<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %> 
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Suppliers
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <link  href="~/styles/grid.css" rel="stylesheet" type="text/css" />
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
        <h3>Manage Suppliers</h3> 
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="800px" Width="99%">
        <telerik:RadPane ID="RadPane1" runat="server" Width="350px">
        
    <telerik:RadSplitter ID="RadSplitter2" runat="server" Height="100%" Width="98%" Orientation="Horizontal">
        <telerik:RadPane ID="RadPane2" runat="server" Height="200px" >
            <telerik:RadGrid ID="RadGrid4" runat="server" DataSourceID="EntityDataSource6"
        GridLines="Both" AllowAutomaticDeletes="true" AllowAutomaticInserts="true"
        AllowAutomaticUpdates="true" OnItemCommand="RadGrid4_ItemCommand"
        OnInsertCommand="RadGrid4_InsertCommand"
        AllowSorting="true" ShowFooter="true" PagerStyle-Visible="true">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" /> 
    <MasterTableView autogeneratecolumns="False"  EditMode="InPlace"
            datakeynames="sup_type_id" datasourceid="EntityDataSource6" AllowAutomaticDeletes="True"
               CommandItemDisplay="Top" Width="100%" Caption = "Supplier Types" PagerStyle-AlwaysVisible="true">
             <CommandItemSettings AddNewRecordText="Add A Supplier Type" />
          
    <Columns>
        <telerik:GridBoundColumn DataField="sup_type_id" DataType="System.String" 
            DefaultInsertValue="" HeaderText="sup_type_id" 
            SortExpression="sup_type_id" UniqueName="sup_type_id" ItemStyle-Width="40px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="sup_type_name" DataType="System.String" 
            DefaultInsertValue="" HeaderText="Supplier Type" 
            SortExpression="sup_type_name" UniqueName="sup_type_name" ItemStyle-Width="200px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# DataBinder.Eval(Container.DataItem, "sup_type_name")%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtsup_type_name" runat="server" Text='<%# Bind("sup_type_name") %>'
                 MaxLength="100" Width="200px"></asp:TextBox>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridBoundColumn DataField="creation_date" DataType="System.DateTime" 
            DefaultInsertValue="" HeaderText="creation_date" SortExpression="creation_date" 
            UniqueName="creation_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="creator" DefaultInsertValue=User.Identity.Name 
            HeaderText="creator" SortExpression="creator" UniqueName="creator" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="modification_date" 
            DataType="System.DateTime" DefaultInsertValue='' HeaderText="modification_date" 
            SortExpression="modification_date" UniqueName="modification_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="last_modifier" DefaultInsertValue="" 
            HeaderText="last_modifier" SortExpression="last_modifier" 
            UniqueName="last_modifier" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected supplier type?\nNote that suppliers created using this type and their corresponding transactions\nwill also be deleted.">
          </telerik:GridButtonColumn>
    </Columns>  
</MasterTableView> 
    </telerik:RadGrid>
    
        </telerik:RadPane>
        <telerik:RadPane ID="RadPane4" runat="server" Height="200px" >
            <telerik:RadGrid ID="RadGrid2" runat="server" DataSourceID="EntityDataSource4"
        GridLines="Both" AllowAutomaticDeletes="true" AllowAutomaticInserts="true"
        AllowAutomaticUpdates="true" OnItemCommand="RadGrid2_ItemCommand" OnInsertCommand="RadGrid2_InsertCommand"
        AllowSorting="true" ShowFooter="true" PagerStyle-Visible="true">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
    <MasterTableView autogeneratecolumns="False"  EditMode="InPlace"
            datakeynames="addr_type" datasourceid="EntityDataSource4" AllowAutomaticDeletes="True"
               CommandItemDisplay="Top" Width="100%" Caption = "Address Types" PagerStyle-AlwaysVisible="true">
             <CommandItemSettings AddNewRecordText="Add An Address Type" />
          
    <Columns>
        <telerik:GridTemplateColumn DataField="addr_type" DataType="System.String" 
            DefaultInsertValue="" HeaderText="Code" 
            SortExpression="addr_type" UniqueName="addr_type" ItemStyle-Width="40px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# DataBinder.Eval(Container.DataItem, "addr_type") %></span>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtCode" runat="server" Text='<%# Bind("addr_type") %>'
                 MaxLength="1" Width="30px"></asp:TextBox>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="addr_type_name" DataType="System.String" 
            DefaultInsertValue="" HeaderText="Address Type" 
            SortExpression="addr_type_name" UniqueName="addr_type_name" ItemStyle-Width="150px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# DataBinder.Eval(Container.DataItem, "addr_type_name")%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtaddr_type_name" runat="server" Text='<%# Bind("addr_type_name") %>'
                 MaxLength="100" Width="150px"></asp:TextBox>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridBoundColumn DataField="creation_date" DataType="System.DateTime" 
            DefaultInsertValue="" HeaderText="creation_date" SortExpression="creation_date" 
            UniqueName="creation_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="creator" DefaultInsertValue=User.Identity.Name 
            HeaderText="creator" SortExpression="creator" UniqueName="creator" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="modification_date" 
            DataType="System.DateTime" DefaultInsertValue='' HeaderText="modification_date" 
            SortExpression="modification_date" UniqueName="modification_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="last_modifier" DefaultInsertValue="" 
            HeaderText="last_modifier" SortExpression="last_modifier" 
            UniqueName="last_modifier" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected address type?\nNote that adresses created using this type will also be deleted.">
          </telerik:GridButtonColumn>
    </Columns>  
</MasterTableView> 
    </telerik:RadGrid>
    
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar2" runat="server" CollapseMode="Forward" />
        <telerik:RadPane ID="RadPane3" runat="server">
             <telerik:RadGrid ID="RadGrid3" runat="server" DataSourceID="EntityDataSource5"
        GridLines="Both" AllowAutomaticDeletes="true" AllowAutomaticInserts="true"
        AllowAutomaticUpdates="true" OnItemCommand="RadGrid3_ItemCommand" OnInsertCommand="RadGrid3_InsertCommand"
        AllowSorting="true" ShowFooter="true" PagerStyle-Visible="true">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
    <MasterTableView autogeneratecolumns="False"  EditMode="InPlace"
            datakeynames="phon_type" datasourceid="EntityDataSource5" AllowAutomaticDeletes="True"
               CommandItemDisplay="Top" Width="100%" Caption = "Phone Number Types" PagerStyle-AlwaysVisible="true">
             <CommandItemSettings AddNewRecordText="Add A Phone Number Type" />
          
    <Columns>
        <telerik:GridTemplateColumn DataField="phon_type" DataType="System.String" 
            DefaultInsertValue="" HeaderText="Code" 
            SortExpression="phon_type" UniqueName="phon_type" ItemStyle-Width="40px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# DataBinder.Eval(Container.DataItem, "phon_type") %></span>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtPCode" runat="server" Text='<%# Bind("phon_type") %>'
                 MaxLength="1" Width="30px"></asp:TextBox>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="phon_type_name" DataType="System.String" 
            DefaultInsertValue="" HeaderText="Phone Number Type" 
            SortExpression="phon_type_name" UniqueName="phon_type_name" ItemStyle-Width="150px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# DataBinder.Eval(Container.DataItem, "phon_type_name")%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtphon_type_name" runat="server" Text='<%# Bind("phon_type_name") %>'
                 MaxLength="100" Width="150px"></asp:TextBox>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridBoundColumn DataField="creation_date" DataType="System.DateTime" 
            DefaultInsertValue="" HeaderText="creation_date" SortExpression="creation_date" 
            UniqueName="creation_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="creator" DefaultInsertValue=User.Identity.Name 
            HeaderText="creator" SortExpression="creator" UniqueName="creator" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="modification_date" 
            DataType="System.DateTime" DefaultInsertValue='' HeaderText="modification_date" 
            SortExpression="modification_date" UniqueName="modification_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="last_modifier" DefaultInsertValue="" 
            HeaderText="last_modifier" SortExpression="last_modifier" 
            UniqueName="last_modifier" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected phone number type?\nNote that phone numbers created using this type will also be deleted.">
          </telerik:GridButtonColumn>
    </Columns>  
</MasterTableView> 
    </telerik:RadGrid>
        </telerik:RadPane>
     </telerik:RadSplitter>
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" />
        <telerik:RadPane ID="contentPane" runat="server">
            <asp:Panel runat="server" ID="pnlEditCat" Visible="true">
      
    <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticDeletes="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnUpdateCommand="RadGrid1_UpdateCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
    <MasterTableView autogeneratecolumns="False" EditMode="PopUp"
            datakeynames="sup_id" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True"
               CommandItemDisplay="Top" Width="960px">
             <CommandItemSettings AddNewRecordText="Add A Supplier" />
     <CommandItemSettings AddNewRecordText="Add A New Supplier" />
           
                         <DetailTables>
                            <telerik:GridTableView AutoGenerateColumns="false" EditMode="PopUp"
                            DataKeyNames="sup_addr_id" DataSourceID="EntityDataSource2" 
                            AllowAutomaticDeletes="true" Width="800px"
                            CommandItemDisplay="Top" AllowAutomaticUpdates="true">                                 
                                 <CommandItemSettings AddNewRecordText="Add A Supplier Address" />
                                 <ParentTableRelation>
                                    <telerik:GridRelationFields DetailKeyField="sup_id" MasterKeyField="sup_id" />
                                 </ParentTableRelation>
                                 <Columns>
                                    <telerik:GridBoundColumn DataField="sup_addr_id" DataType="System.Int32" 
                                        DefaultInsertValue="" HeaderText="sup_addr_id" ReadOnly="True" 
                                        SortExpression="sup_addr_id" UniqueName="sup_addr_id" Visible="false">
                                    </telerik:GridBoundColumn> 
                                    <telerik:GridTemplateColumn DataField="addr_type" DataType="System.String" 
                                        DefaultInsertValue="" HeaderText="Address Type" 
                                        SortExpression="addr_type" UniqueName="addr_type" ItemStyle-Width="100px"
                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <span><%# AddressTypeName((int)DataBinder.Eval(Container.DataItem, "sup_addr_id"))%></span>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="addr_line_1" DefaultInsertValue="" 
                                        HeaderText="Address Line 1" SortExpression="addr_line_1" UniqueName="addr_line_1" 
                                        ItemStyle-Width="150px">
                                    </telerik:GridBoundColumn> 
                                    <telerik:GridBoundColumn DataField="addr_line_2" DefaultInsertValue="" 
                                        HeaderText="Address Line 2" SortExpression="addr_line_2" UniqueName="addr_line_2" 
                                        ItemStyle-Width="150px">
                                    </telerik:GridBoundColumn> 
                                    <telerik:GridTemplateColumn DataField="city_id" DataType="System.Int32" 
                                        DefaultInsertValue="" HeaderText="City" 
                                        SortExpression="city_id" UniqueName="city_id" ItemStyle-Width="90px"
                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <span><%# CityName((int)DataBinder.Eval(Container.DataItem, "sup_addr_id"))%></span>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="is_default" DataType="System.Int32" 
                                        DefaultInsertValue="" HeaderText="Default?" 
                                        SortExpression="is_default" UniqueName="is_default" ItemStyle-Width="40px"
                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDefault" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "is_default")%>' /></span>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="creation_date" DataType="System.DateTime" 
                                        DefaultInsertValue="" HeaderText="creation_date" SortExpression="creation_date" 
                                        UniqueName="creation_date" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="creator" DefaultInsertValue="" 
                                        HeaderText="creator" SortExpression="creator" UniqueName="creator" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="modification_date" 
                                        DataType="System.DateTime" DefaultInsertValue="" HeaderText="modification_date" 
                                        SortExpression="modification_date" UniqueName="modification_date" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="last_modifier" DefaultInsertValue="" 
                                        HeaderText="last_modifier" SortExpression="last_modifier" 
                                        UniqueName="last_modifier" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                                        ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                                        ItemStyle-Width="32px" ItemStyle-Height="32px" >
                                    </telerik:GridEditCommandColumn>
                                      <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                        ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                        ConfirmText="Are you sure you want to delete the selected currency?">
                                      </telerik:GridButtonColumn>
                                </Columns> 
                                <EditFormSettings InsertCaption="Add new Supplier Address" CaptionFormatString="Edit Supplier Address: {0}" 
                                    CaptionDataField="addr_types.addr_types" EditFormType="Template" PopUpSettings-Modal="true">
                                    <FormTemplate>
                                       <table style="width: 450px">
                                        <tbody>
                                         <tr>
                                          <td>
                                            <label id="Label2" runat="server">Address Type:</label>
                                          </td>
                                          <td>                                           
                                            <telerik:RadComboBox ID="cboAddrType" runat="server" Height="200px" Width=" 255px"
                                                    DropDownWidth=" 255px" EmptyMessage="Select an address type" HighlightTemplatedItems="true"
                                                    MarkFirstMatch="true" Filter="Contains" OnItemsRequested="cboAddrType_ItemsRequested" 
                                                    EnableLoadOnDemand="true" OnSelectedIndexChanged="cboAddrType_SelectedIndexChanged" AutoPostBack="true"
                                                    OnLoad="cboAddrType_Load">
                                                    <HeaderTemplate>
                                                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                            <tr> 
                                                                <td style="width: 80px;">
                                                                    Code</td>
                                                                <td style="width: 170px;">
                                                                    Name</td> 
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 80px;" >
                                                                    <%# DataBinder.Eval(Container, "Attributes['code']")%>
                                                                </td>
                                                                <td style="width: 150px;">
                                                                    <%# DataBinder.Eval(Container, "Attributes['name']")%>
                                                                </td>  
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cboAddrType"
                                            ToolTip="Kindly provide address type before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                            </asp:RequiredFieldValidator>
                                            </td>
                                         </tr>
                                         <tr>
                                          <td>
                                            <label id="Label1" runat="server">Address Line 1:</label>
                                          </td>
                                          <td>
                                           <asp:TextBox ID="txtAddrLine1" runat="server"
                                                Text='<%# Bind("addr_line_1") %>' Width="200px" MaxLength="150"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddrLine1"
                                            ToolTip="Kindly provide address before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                            </asp:RequiredFieldValidator>
                                            </td>
                                         </tr>
                                         <tr>
                                          <td>
                                            <label id="Label8" runat="server">Address Line 2:</label>
                                          </td>
                                          <td>
                                           <asp:TextBox ID="TextBox5" runat="server"
                                                Text='<%# Bind("addr_line_2") %>' Width="200px" MaxLength="150"></asp:TextBox>
                                           </td>
                                         </tr>
                                         <tr>
                                          <td>
                                            <label id="Label9" runat="server">City:</label>
                                          </td>
                                          <td>                                           
                                            <telerik:RadComboBox ID="cboCity" runat="server" Height="200px" Width=" 255px"
                                                    DropDownWidth=" 255px" EmptyMessage="Select an address type" HighlightTemplatedItems="true"
                                                    MarkFirstMatch="true" Filter="Contains" OnItemsRequested="cboCity_ItemsRequested" 
                                                    EnableLoadOnDemand="true" OnSelectedIndexChanged="cboCity_SelectedIndexChanged" AutoPostBack="true"
                                                    OnLoad="cboCity_Load">
                                                    <HeaderTemplate>
                                                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                            <tr> 
                                                                <td style="width: 80px;">
                                                                    City</td>
                                                                <td style="width: 170px;">
                                                                    Country</td> 
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 80px;" >
                                                                    <%# DataBinder.Eval(Container, "Attributes['city']")%>
                                                                </td>
                                                                <td style="width: 150px;">
                                                                    <%# DataBinder.Eval(Container, "Attributes['country']")%>
                                                                </td>  
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cboAddrType"
                                            ToolTip="Kindly provide address type before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                            </asp:RequiredFieldValidator>
                                            </td>
                                         </tr>
                                         <tr>
                                          <td>
                                            <label id="Label10" runat="server">Default Address?:</label>
                                          </td>
                                          <td>
                                          <asp:CheckBox ID="chkDefault" runat="server"
                                                Checked='<%# Bind("is_default") %>'></asp:CheckBox> 
                                            </td>
                                         </tr>
                                        </tbody>
                                       </table>
                                           <asp:TextBox ID="TextBox2" runat="server"
                                                Text='<%# Bind("creator") %>' Width="70px" MaxLength="50" Visible="false"></asp:TextBox> 
                                           <telerik:RadDateTimePicker ID="diCreationDate" runat="server"
                                                 SelectedDate='<%# Bind("creation_date") %>' Width="70px" Visible="false"></telerik:RadDateTimePicker>
                                       <br /> 
                                        <asp:Button ID="Button1" CommandName="PerformInsert"
                                        Visible='<%# DataBinder.Eval(Container, "OwnerTableView.IsItemInserted") %>' 
                                            Text="Save" runat="server" />
                                        <asp:Button ID="Button2" CommandName="Update"
                                        Visible='<%# !((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) %>' 
                                            Text="Save" runat="server" />
                                        <asp:Button ID="Button3" CommandName="Cancel" 
                                            Text="Cancel" runat="server" CausesValidation="false"/>
                                    </FormTemplate>
                            </EditFormSettings>
                            </telerik:GridTableView>
                            
                            
                            <telerik:GridTableView AutoGenerateColumns="false" EditMode="PopUp"
                            DataKeyNames="sup_phon_id" DataSourceID="EntityDataSource3" 
                            AllowAutomaticDeletes="true" Width="450px"
                            CommandItemDisplay="Top" AllowAutomaticUpdates="true">                                 
                                 <CommandItemSettings AddNewRecordText="Add A Supplier Telephone Number" />
                                 <ParentTableRelation>
                                    <telerik:GridRelationFields DetailKeyField="sup_id" MasterKeyField="sup_id" />
                                 </ParentTableRelation>
                                 <Columns>
                                    <telerik:GridBoundColumn DataField="sup_phon_id" DataType="System.Int32" 
                                        DefaultInsertValue="" HeaderText="sup_phon_id" ReadOnly="True" 
                                        SortExpression="sup_phon_id" UniqueName="sup_phon_id" Visible="false">
                                    </telerik:GridBoundColumn>  
                                    <telerik:GridTemplateColumn DataField="phon_type" DataType="System.String" 
                                        DefaultInsertValue="" HeaderText="Phone No. Type" 
                                        SortExpression="phon_type" UniqueName="phon_type" ItemStyle-Width="150px"
                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <span><%# PhoneTypeName((int)DataBinder.Eval(Container.DataItem, "sup_phon_id"))%></span>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="phon_num" DefaultInsertValue="" 
                                        HeaderText="Telephone Number" SortExpression="phon_num" UniqueName="phon_num" 
                                        ItemStyle-Width="250px">
                                    </telerik:GridBoundColumn>   
                                    <telerik:GridTemplateColumn DataField="is_default" DataType="System.Int32" 
                                        DefaultInsertValue="" HeaderText="Default?" 
                                        SortExpression="is_default" UniqueName="is_default" ItemStyle-Width="40px"
                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDefault" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "is_default")%>' /></span>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="creation_date" DataType="System.DateTime" 
                                        DefaultInsertValue="" HeaderText="creation_date" SortExpression="creation_date" 
                                        UniqueName="creation_date" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="creator" DefaultInsertValue="" 
                                        HeaderText="creator" SortExpression="creator" UniqueName="creator" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="modification_date" 
                                        DataType="System.DateTime" DefaultInsertValue="" HeaderText="modification_date" 
                                        SortExpression="modification_date" UniqueName="modification_date" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="last_modifier" DefaultInsertValue="" 
                                        HeaderText="last_modifier" SortExpression="last_modifier" 
                                        UniqueName="last_modifier" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                                        ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                                        ItemStyle-Width="32px" ItemStyle-Height="32px" >
                                    </telerik:GridEditCommandColumn>
                                      <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                        ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                        ConfirmText="Are you sure you want to delete the selected currency?">
                                      </telerik:GridButtonColumn>
                                </Columns> 
                                <EditFormSettings InsertCaption="Add new Supplier Telephone Number" CaptionFormatString="Edit Supplier Telephone Number: {0}" 
                                    CaptionDataField="phon_num" EditFormType="Template" PopUpSettings-Modal="true">
                                    <FormTemplate>
                                       <table style="width: 450px">
                                        <tbody>
                                         <tr>
                                          <td>
                                            <label id="Label2" runat="server">Phone No. Type:</label>
                                          </td>
                                          <td>                                           
                                            <telerik:RadComboBox ID="cboPhonType" runat="server" Height="200px" Width=" 255px"
                                                    DropDownWidth=" 255px" EmptyMessage="Select an address type" HighlightTemplatedItems="true"
                                                    MarkFirstMatch="true" Filter="Contains" OnItemsRequested="cboPhonType_ItemsRequested" 
                                                    EnableLoadOnDemand="true" OnSelectedIndexChanged="cboPhonType_SelectedIndexChanged" AutoPostBack="true"
                                                    OnLoad="cboPhonType_Load">
                                                    <HeaderTemplate>
                                                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                            <tr> 
                                                                <td style="width: 80px;">
                                                                    Code</td>
                                                                <td style="width: 170px;">
                                                                    Name</td> 
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 80px;" >
                                                                    <%# DataBinder.Eval(Container, "Attributes['code']")%>
                                                                </td>
                                                                <td style="width: 150px;">
                                                                    <%# DataBinder.Eval(Container, "Attributes['name']")%>
                                                                </td>  
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cboPhonType"
                                            ToolTip="Kindly provide address type before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                            </asp:RequiredFieldValidator>
                                            </td>
                                         </tr>
                                         <tr>
                                          <td>
                                            <label id="Label1" runat="server">Telephone Number:</label>
                                          </td>
                                          <td>
                                           <asp:TextBox ID="txtphon_num" runat="server"
                                                Text='<%# Bind("phon_num") %>' Width="200px" MaxLength="150"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtphon_num"
                                            ToolTip="Kindly provide phone number before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                            </asp:RequiredFieldValidator>
                                            </td>
                                         </tr>
                                         <tr>
                                          <td>
                                            <label id="Label10" runat="server">Default Number?:</label>
                                          </td>
                                          <td>
                                          <asp:CheckBox ID="chkDefault" runat="server"
                                                Checked='<%# Bind("is_default") %>'></asp:CheckBox> 
                                            </td>
                                         </tr>
                                        </tbody>
                                       </table>
                                           <asp:TextBox ID="TextBox2" runat="server"
                                                Text='<%# Bind("creator") %>' Width="70px" MaxLength="50" Visible="false"></asp:TextBox> 
                                           <telerik:RadDateTimePicker ID="diCreationDate" runat="server"
                                                 SelectedDate='<%# Bind("creation_date") %>' Width="70px" Visible="false"></telerik:RadDateTimePicker>
                                       <br /> 
                                        <asp:Button ID="Button1" CommandName="PerformInsert"
                                        Visible='<%# DataBinder.Eval(Container, "OwnerTableView.IsItemInserted") %>' 
                                            Text="Save" runat="server" />
                                        <asp:Button ID="Button2" CommandName="Update"
                                        Visible='<%# !((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) %>' 
                                            Text="Save" runat="server" />
                                        <asp:Button ID="Button3" CommandName="Cancel" 
                                            Text="Cancel" runat="server" CausesValidation="false"/>
                                    </FormTemplate>
                            </EditFormSettings>
                            </telerik:GridTableView>
                         </DetailTables>
     
    <Columns>
        <telerik:GridBoundColumn DataField="sup_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="sup_id" ReadOnly="True" 
            SortExpression="sup_id" UniqueName="sup_id" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="acc_num" DefaultInsertValue="" 
            HeaderText="Account #" SortExpression="acc_num" UniqueName="acc_num" ItemStyle-Width="60px">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="sup_name" DefaultInsertValue="" 
            HeaderText="Supplier Name" SortExpression="sup_name" UniqueName="sup_name" ItemStyle-Width="200px">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="sup_type_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="Type" 
            SortExpression="sup_type_id" UniqueName="sup_type_id" ItemStyle-Width="60px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# SupplierTypeName((int)DataBinder.Eval(Container.DataItem, "sup_id")) %></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="currency_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="Currency" 
            SortExpression="currency_id" UniqueName="currency_id" ItemStyle-Width="70px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# CurrencyName((int)DataBinder.Eval(Container.DataItem, "sup_id"))%></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridBoundColumn DataField="contact_person" DefaultInsertValue="" 
            HeaderText="Contact Person" SortExpression="contact_person" UniqueName="contact_person" ItemStyle-Width="80px">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="ap_acct_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="AR Account" 
            SortExpression="ap_acct_id" UniqueName="ap_acct_id" ItemStyle-Width="100px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# AccountName((int)DataBinder.Eval(Container.DataItem, "sup_id"),1)%></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="vat_acct_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="VAT Account" 
            SortExpression="vat_acct_id" UniqueName="vat_acct_id" ItemStyle-Width="100px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# AccountName((int)DataBinder.Eval(Container.DataItem, "sup_id"),2)%></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridBoundColumn DataField="creation_date" DataType="System.DateTime" 
            DefaultInsertValue="" HeaderText="creation_date" SortExpression="creation_date" 
            UniqueName="creation_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="creator" DefaultInsertValue="" 
            HeaderText="creator" SortExpression="creator" UniqueName="creator" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="modification_date" 
            DataType="System.DateTime" DefaultInsertValue="" HeaderText="modification_date" 
            SortExpression="modification_date" UniqueName="modification_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="last_modifier" DefaultInsertValue="" 
            HeaderText="last_modifier" SortExpression="last_modifier" 
            UniqueName="last_modifier" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected supplier?\n Note that any related transactions will also be deleted.">
          </telerik:GridButtonColumn>
    </Columns> 
    <EditFormSettings InsertCaption="Add new Supplier" CaptionFormatString="Edit Supplier: {0}" 
        CaptionDataField="sup_name" EditFormType="Template" PopUpSettings-Modal="true">
        <FormTemplate>
           <table style="width: 450px">
            <tbody>
             <tr>
              <td>
                <label id="Label1" runat="server">Supplier Account #:</label>
              </td>
              <td>
               <asp:TextBox ID="TextBox1" runat="server"
                    Text='<%# Bind("acc_num") %>' Width="80px" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
                ToolTip="Kindly provide supplier account number before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                </asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr>
              <td>
                <label id="Label2" runat="server">Supplier Name:</label>
              </td>
              <td>
               <asp:TextBox ID="TextBox2" runat="server"
                    Text='<%# Bind("sup_name") %>' Width="230px" MaxLength="250"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox2"
                ToolTip="Kindly provide supplier name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                </asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr>
              <td>
                <label id="lblCountryName" runat="server">Supplier Type:</label>
              </td>
              <td>
                    <telerik:RadComboBox ID="cboType" runat="server" Height="200px" Width=" 230px"
                            DropDownWidth=" 230px" EmptyMessage="Select a supplier type" HighlightTemplatedItems="true"
                            MarkFirstMatch="true" Filter="Contains" OnItemsRequested="cboType_ItemsRequested" 
                            EnableLoadOnDemand="true" OnSelectedIndexChanged="cboType_SelectedIndexChanged" AutoPostBack="true"
                            OnLoad="cboType_Load">
                            <HeaderTemplate>
                                <table style="width: 230px" cellspacing="0" cellpadding="0">
                                    <tr>  
                                        <td style="width: 210px;">
                                            Supplier Type</td> 
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 210px" cellspacing="0" cellpadding="0">
                                    <tr> 
                                        <td >
                                            <%# DataBinder.Eval(Container, "Text")%>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                <asp:RequiredFieldValidator ID="rfvCountryName" runat="server" ControlToValidate="cboType"
                ToolTip="Kindly provide supplier type before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                </asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr>
              <td>
                <label id="Label3" runat="server">Internal Representative:</label>
              </td>
              <td>
                    <telerik:RadComboBox ID="cboEmp" runat="server" Height="200px" Width=" 255px"
                            DropDownWidth=" 230px" EmptyMessage="Select a rep employee" HighlightTemplatedItems="true"
                            MarkFirstMatch="true" Filter="Contains" OnItemsRequested="cboEmp_ItemsRequested" 
                            EnableLoadOnDemand="true" OnSelectedIndexChanged="cboEmp_SelectedIndexChanged" AutoPostBack="true"
                            OnLoad="cboEmp_Load">
                            <HeaderTemplate>
                                <table style="width: 230px" cellspacing="0" cellpadding="0">
                                    <tr>  
                                        <td style="width: 210px;">
                                            Employee</td> 
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 230px" cellspacing="0" cellpadding="0">
                                    <tr> 
                                        <td  style="width: 210px;">
                                            <%# DataBinder.Eval(Container, "Text")%>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                </td>
             </tr>
             <tr>
              <td>
                <label id="Label4" runat="server">Contact Person:</label>
              </td>
              <td>
               <asp:TextBox ID="TextBox3" runat="server"
                    Text='<%# Bind("contact_person") %>' Width="200px" MaxLength="100"></asp:TextBox> 
                </td>
             </tr>  
            <tr>
                <td>Supplier Currency:</td>
                <td>
                    <telerik:RadComboBox ID="cboCur" runat="server" Height="200px" Width="200px"
                DropDownWidth="230px" EmptyMessage="Select the Supplier's Currency" HighlightTemplatedItems="true"
                MarkFirstMatch="true" Filter="Contains" OnItemsRequested="cboCur_ItemsRequested" 
                            EnableLoadOnDemand="true" OnSelectedIndexChanged="cboCur_SelectedIndexChanged" AutoPostBack="true"
                            OnLoad="cboCur_Load">
                <HeaderTemplate>
                    <table style="width: 230px" cellspacing="0" cellpadding="0">
                        <tr> 
                            <td style="width: 150px;">
                                Currency Name</td>
                            <td style="width: 60px;">
                                Rate</td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 230px" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 150px;">
                                <%# DataBinder.Eval(Container, "Text")%>
                            </td>
                            <td style="width: 60px;">
                                <%# DataBinder.Eval(Container, "Attributes['current_buy_rate']")%>
                            </td> 
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="cboCur"
                         ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly select the Supplier's Currency."></asp:RequiredFieldValidator></td> 
            </tr> 
             <tr>
              <td>
                <label id="Label5" runat="server">AP Account:</label>
              </td>
              <td>
                    <telerik:RadComboBox ID="cboARAcct" runat="server" Height="200px" Width=" 255px"
                            DropDownWidth=" 230px" EmptyMessage="Select a rep employee" HighlightTemplatedItems="true"
                            MarkFirstMatch="true" Filter="Contains" OnItemsRequested="cboARAcct_ItemsRequested" 
                            EnableLoadOnDemand="true" OnSelectedIndexChanged="cboARAcct_SelectedIndexChanged" AutoPostBack="true"
                            OnLoad="cboARAcct_Load">
                            <HeaderTemplate>
                                <table style="width: 230px" cellspacing="0" cellpadding="0">
                                    <tr> 
                                        <td style="width: 60px;">
                                            Acc Num</td>
                                        <td style="width: 100px;">
                                            Acc Name</td>
                                        <td style="width: 60px;">
                                            Acc Currency</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 230px" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 60px;" >
                                            <%# DataBinder.Eval(Container, "Attributes['acc_num']")%>
                                        </td>
                                        <td style="width: 100px;">
                                            <%# DataBinder.Eval(Container, "Attributes['acc_name']")%>
                                        </td> 
                                        <td style="width: 60px;">
                                            <%# DataBinder.Eval(Container, "Attributes['acc_cur']")%>
                                        </td> 
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                </td>
             </tr>
             <tr>
              <td>
                <label id="Label6" runat="server">VAT Account:</label>
              </td>
              <td>
                    <telerik:RadComboBox ID="cboVATAcct" runat="server" Height="200px" Width=" 250px"
                            DropDownWidth=" 230px" EmptyMessage="Select a rep employee" HighlightTemplatedItems="true"
                            MarkFirstMatch="true" Filter="Contains" OnItemsRequested="cboVATAcct_ItemsRequested" 
                            EnableLoadOnDemand="true" OnSelectedIndexChanged="cboVATAcct_SelectedIndexChanged" AutoPostBack="true"
                            OnLoad="cboVATAcct_Load">
                            <HeaderTemplate>
                                <table style="width: 230px" cellspacing="0" cellpadding="0">
                                    <tr> 
                                        <td style="width: 60px;">
                                            Acc Num</td>
                                        <td style="width: 100px;">
                                            Acc Name</td>
                                        <td style="width: 60px;">
                                            Acc Currency</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 230px" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 60px;" >
                                            <%# DataBinder.Eval(Container, "Attributes['acc_num']")%>
                                        </td>
                                        <td style="width: 100px;">
                                            <%# DataBinder.Eval(Container, "Attributes['acc_name']")%>
                                        </td> 
                                        <td style="width: 60px;">
                                            <%# DataBinder.Eval(Container, "Attributes['acc_cur']")%>
                                        </td> 
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                </td>
             </tr>
             <tr>
              <td>
                <label id="Label7" runat="server">Debit Terms:</label>
              </td>
              <td>
               <asp:TextBox ID="TextBox4" runat="server"
                    Text='<%# Bind("debit_terms") %>' Width="230px" MaxLength="4000"
                    TextMode="MultiLine" Rows="5"></asp:TextBox>
                </td>
             </tr>
            </tbody>
           </table>
               <asp:TextBox ID="txtCreator" runat="server"
                    Text='<%# Bind("creator") %>' Width="70px" MaxLength="50" Visible="false"></asp:TextBox> 
               <telerik:RadDateTimePicker ID="diCreationDate" runat="server"
                     SelectedDate='<%# Bind("creation_date") %>' Width="70px" Visible="false"></telerik:RadDateTimePicker>
           <br /> 
            <asp:Button ID="btnInsert" CommandName="PerformInsert"
            Visible='<%# DataBinder.Eval(Container, "OwnerTableView.IsItemInserted") %>' 
                Text="Save" runat="server" />
            <asp:Button ID="btnUpdate" CommandName="Update"
            Visible='<%# !((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) %>' 
                Text="Save" runat="server" />
            <asp:Button ID="btnCancel" CommandName="Cancel" 
                Text="Cancel" runat="server" CausesValidation="false"/>
        </FormTemplate>
    </EditFormSettings>
</MasterTableView>
       <ClientSettings>
           <ClientEvents OnPopUpShowing="PopUpShowing"/>
           <Selecting AllowRowSelect="true" />
           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
           <Selecting AllowRowSelect="true" />
       </ClientSettings>
    </telerik:RadGrid>
    <asp:ImageButton ID="btnExcel" OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord"  OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" />
        <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnExcel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnExcel" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnWord">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnWord" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="mngBtnCSVProxy">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="mngBtnCSVProxy" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnPDF">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnPDF" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
              </asp:Panel>  
            <div id="divProc" runat="server" style="visibility:hidden">
                <asp:Image ID="Image1" ImageUrl="~/images/animated/processing.gif" runat="server" />
            </div>
            <div id="divError" runat="server" style="visibility:hidden">
                <asp:Image ID="Image2" ImageUrl="~/images/errors/error.jpg" runat="server" />
                <span id="spanError" class="error" runat="server"></span>
            </div>            
        </telerik:RadPane>
    </telerik:RadSplitter>
    </telerik:RadAjaxPanel>
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="sups">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="sup_addr" Where="it.sup_id == @sup_id">
        <WhereParameters>
            <asp:SessionParameter Name="sup_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="sup_phons" Where="it.sup_id == @sup_id">
        <WhereParameters>
            <asp:SessionParameter Name="sup_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource4" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="addr_types">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource5" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="phon_types"> 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource6" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="sup_types"> 
    </ef:EntityDataSource>
</asp:Content>
