<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="True" CodeBehind="country.aspx.cs" Inherits="coreERP.common.country" %>
<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %> 
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Countries, Regions, Districts, Cities &amp; Locations
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
        <h5>Countries, Regions, Districts, Cities &amp; Locations</h5> 
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" ClientEvents-OnRequestStart="mngRequestStarted"
    EnableAJAX="true">
    <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticDeletes="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnUpdateCommand="RadGrid1_UpdateCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="PopUp"
            datakeynames="country_id" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True"
               CommandItemDisplay="Top" Width="960px">
             <CommandItemSettings AddNewRecordText="Add A Country" />
     
     <DetailTables>
        <telerik:GridTableView AutoGenerateColumns="false" EditMode="PopUp"
        DataKeyNames="region_id" DataSourceID="EntityDataSource2" 
        AllowAutomaticDeletes="true"
        CommandItemDisplay="Top" AllowAutomaticUpdates="true"
        Width="500px">
             <CommandItemSettings AddNewRecordText="Add A Region" />
             <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="country_id" MasterKeyField="country_id" />
             </ParentTableRelation>
     
     
     
             <DetailTables>
                <telerik:GridTableView AutoGenerateColumns="false" EditMode="PopUp"
                DataKeyNames="district_id" DataSourceID="EntityDataSource3" 
                AllowAutomaticDeletes="true"
                CommandItemDisplay="Top" AllowAutomaticUpdates="true"
                Width="300px">
                     <CommandItemSettings AddNewRecordText="Add A District" />
                     <ParentTableRelation>
                        <telerik:GridRelationFields DetailKeyField="region_id" MasterKeyField="region_id" />
                     </ParentTableRelation>
                     
                         <DetailTables>
                            <telerik:GridTableView AutoGenerateColumns="false" EditMode="PopUp"
                            DataKeyNames="city_id" DataSourceID="EntityDataSource4" 
                            AllowAutomaticDeletes="true"
                            CommandItemDisplay="Top" AllowAutomaticUpdates="true"
                            Width="300px">                                 
                                 <CommandItemSettings AddNewRecordText="Add A City" />
                                 <ParentTableRelation>
                                    <telerik:GridRelationFields DetailKeyField="district_id" MasterKeyField="district_id" />
                                 </ParentTableRelation>
                                 
                         <DetailTables>
                            <telerik:GridTableView AutoGenerateColumns="false" EditMode="PopUp"
                            DataKeyNames="location_id" DataSourceID="EntityDataSource5" 
                            AllowAutomaticDeletes="true"
                            CommandItemDisplay="Top" AllowAutomaticUpdates="true"
                            Width="400px">                                 
                                 <CommandItemSettings AddNewRecordText="Add A Location" />
                                 <ParentTableRelation>
                                    <telerik:GridRelationFields DetailKeyField="city_id" MasterKeyField="city_id" />
                                 </ParentTableRelation>
                                 <Columns>
                                    <telerik:GridBoundColumn DataField="location_id" DataType="System.Int32" 
                                        DefaultInsertValue="" HeaderText="location_id" ReadOnly="True" 
                                        SortExpression="location_id" UniqueName="location_id" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="location_code" DefaultInsertValue="" 
                                        HeaderText="Location Code" SortExpression="location_code" UniqueName="location_code" 
                                        ItemStyle-Width="80px">
                                    </telerik:GridBoundColumn> 
                                    <telerik:GridBoundColumn DataField="location_name" DefaultInsertValue="" 
                                        HeaderText="Location Name" SortExpression="location_name" UniqueName="location_name" 
                                        ItemStyle-Width="250px">
                                    </telerik:GridBoundColumn> 
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
                                <EditFormSettings InsertCaption="Add new Location" CaptionFormatString="Edit Location: {0}" 
                                    CaptionDataField="location_name" EditFormType="Template" PopUpSettings-Modal="true">
                                    <FormTemplate>
                                       <table style="width: 450px">
                                        <tbody>
                                         <tr>
                                          <td>
                                            <label id="Label2" runat="server">Location Code:</label>
                                          </td>
                                          <td>
                                           <asp:TextBox ID="txtLocationCode" runat="server"
                                                Text='<%# Bind("location_code") %>' Width="50px" MaxLength="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLocationCode"
                                            ToolTip="Kindly provide location code before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                            </asp:RequiredFieldValidator>
                                            </td>
                                         </tr>
                                         <tr>
                                          <td>
                                            <label id="Label1" runat="server">Location Name:</label>
                                          </td>
                                          <td>
                                           <asp:TextBox ID="txtLocationName" runat="server"
                                                Text='<%# Bind("location_name") %>' Width="200px" MaxLength="150"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLocationName"
                                            ToolTip="Kindly provide location name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                            </asp:RequiredFieldValidator>
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
                                    <telerik:GridBoundColumn DataField="city_id" DataType="System.Int32" 
                                        DefaultInsertValue="" HeaderText="city_id" ReadOnly="True" 
                                        SortExpression="city_id" UniqueName="city_id" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="city_name" DefaultInsertValue="" 
                                        HeaderText="City Name" SortExpression="city_name" UniqueName="city_name" 
                                        ItemStyle-Width="250px">
                                    </telerik:GridBoundColumn> 
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
                                <EditFormSettings InsertCaption="Add new City" CaptionFormatString="Edit City: {0}" 
                                    CaptionDataField="city_name" EditFormType="Template" PopUpSettings-Modal="true">
                                    <FormTemplate>
                                       <table style="width: 450px">
                                        <tbody>
                                         <tr>
                                          <td>
                                            <label id="lblCityName" runat="server">City Name:</label>
                                          </td>
                                          <td>
                                           <asp:TextBox ID="txtCityName" runat="server"
                                                Text='<%# Bind("city_name") %>' Width="200px" MaxLength="150"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCityName" runat="server" ControlToValidate="txtCityName"
                                            ToolTip="Kindly provide city name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                            </asp:RequiredFieldValidator>
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
                            </telerik:GridTableView>
                         </DetailTables>
     
     
     
                     <Columns>
                        <telerik:GridBoundColumn DataField="district_id" DataType="System.Int32" 
                            DefaultInsertValue="" HeaderText="district_id" ReadOnly="True" 
                            SortExpression="district_id" UniqueName="district_id" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="district_name" DefaultInsertValue="" 
                            HeaderText="District Name" SortExpression="district_name" UniqueName="district_name" 
                            ItemStyle-Width="250px">
                        </telerik:GridBoundColumn> 
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
                    <EditFormSettings InsertCaption="Add new District" CaptionFormatString="Edit District: {0}" 
                        CaptionDataField="district_name" EditFormType="Template" PopUpSettings-Modal="true">
                        <FormTemplate>
                           <table style="width: 450px">
                            <tbody>
                             <tr>
                              <td>
                                <label id="lblDistrictName" runat="server">District Name:</label>
                              </td>
                              <td>
                               <asp:TextBox ID="txtDistrictName" runat="server"
                                    Text='<%# Bind("district_name") %>' Width="200px" MaxLength="150"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDistrictName" runat="server" ControlToValidate="txtDistrictName"
                                ToolTip="Kindly provide district name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                </asp:RequiredFieldValidator>
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
                </telerik:GridTableView>
             </DetailTables>
     
     
     
             <Columns>
                <telerik:GridBoundColumn DataField="region_id" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="region_id" ReadOnly="True" 
                    SortExpression="region_id" UniqueName="region_id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="region_name" DefaultInsertValue="" 
                    HeaderText="Region Name" SortExpression="region_name" UniqueName="region_name" ItemStyle-Width="250px">
                </telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="abbrev" DefaultInsertValue="" 
                    HeaderText="Short Name" SortExpression="abbrev" 
                    UniqueName="abbrev" ItemStyle-Width="85px">
                </telerik:GridBoundColumn> 
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
            <EditFormSettings InsertCaption="Add new Region" CaptionFormatString="Edit Region: {0}" 
                CaptionDataField="region_name" EditFormType="Template" PopUpSettings-Modal="true">
                <FormTemplate>
                   <table style="width: 450px">
                    <tbody>
                     <tr>
                      <td>
                        <label id="lblCountryName" runat="server">Region Name:</label>
                      </td>
                      <td>
                       <asp:TextBox ID="txtRegionName" runat="server"
                            Text='<%# Bind("region_name") %>' Width="200px" MaxLength="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRegionName" runat="server" ControlToValidate="txtRegionName"
                        ToolTip="Kindly provide region name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                        </asp:RequiredFieldValidator>
                        </td>
                     </tr>
                     <tr>
                      <td>
                        <label id="lblAbbrev" runat="server">Short Name:</label>
                      </td>
                      <td>
                       <asp:TextBox ID="txtAbbrev" runat="server"
                            Text='<%# Bind("abbrev") %>' Width="70px" MaxLength="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAbbrev" runat="server" ControlToValidate="txtAbbrev"
                        ToolTip="Kindly provide short name before saving"    ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                        </asp:RequiredFieldValidator>
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
        </telerik:GridTableView>
     </DetailTables>
     
     
    <Columns>
        <telerik:GridBoundColumn DataField="country_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="country_id" ReadOnly="True" 
            SortExpression="country_id" UniqueName="country_id" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="country_name" DefaultInsertValue="" 
            HeaderText="Country Name" SortExpression="country_name" UniqueName="country_name" ItemStyle-Width="220px">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="nationality" DefaultInsertValue="" 
            HeaderText="Nationality" SortExpression="nationality" UniqueName="nationality" ItemStyle-Width="220px">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="country_code" DefaultInsertValue="" 
            HeaderText="Intl. Code" SortExpression="country_code" 
            UniqueName="country_code" ItemStyle-Width="85px">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="abbrev" DefaultInsertValue="" 
            HeaderText="Intl. Abbrev" SortExpression="abbrev" 
            UniqueName="abbrev" ItemStyle-Width="85px">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="currency_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="Official Currency" 
            SortExpression="currency_id" UniqueName="currency_id" ItemStyle-Width="150px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# CurrencyName((int)DataBinder.Eval(Container.DataItem, "country_id")) %></span>
            </ItemTemplate>
             <EditItemTemplate>
                <span>
                 <telerik:RadComboBox ID="cboCur" runat="server" Height="150px" Width=" 65px"
                    DropDownWidth=" 255px" EmptyMessage="Select a GL Account" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""    
                    EnableLoadOnDemand="true" AutoPostBack="true">
                    <HeaderTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr> 
                                <td style="width: 80px;">
                                    Cur Sym</td>
                                <td style="width: 150px;">
                                    Cur Name</td>
                                <td style="width: 80px;">
                                    Rate</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 80px;" >
                                    <%# DataBinder.Eval(Container, "Attributes['major_symbol']")%>
                                </td>
                                <td style="width: 150px;">
                                    <%# DataBinder.Eval(Container, "Attributes['major_name']")%>
                                </td> 
                                <td style="width: 80px;">
                                    <%# DataBinder.Eval(Container, "Attributes['current_buy_rate']")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
                </span>
            </EditItemTemplate>
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
    <EditFormSettings InsertCaption="Add new Country" CaptionFormatString="Edit Country: {0}" 
        CaptionDataField="country_name" EditFormType="Template" PopUpSettings-Modal="true">
        <FormTemplate>
           <table style="width: 450px">
            <tbody>
             <tr>
              <td>
                <label id="lblCountryName" runat="server">Country Name:</label>
              </td>
              <td>
               <asp:TextBox ID="txtCountryName" runat="server"
                    Text='<%# Bind("country_name") %>' Width="200px" MaxLength="150"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCountryName" runat="server" ControlToValidate="txtCountryName"
                ToolTip="Kindly provide country name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                </asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr>
              <td>
                <label id="lblNationality" runat="server">Nationality:</label>
              </td>
              <td>
               <asp:TextBox ID="txtNationality" runat="server"
                    Text='<%# Bind("nationality") %>' Width="150px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNationality" runat="server" ControlToValidate="txtNationality"
                 ToolTip="Kindly provide nationality before saving"   ErrorMessage="!" ForeColor="Red"  Font-Bold="true" >
                </asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr>
              <td>
                <label id="lblCode" runat="server">Intl. Code:</label>
              </td>
              <td>
               <asp:TextBox ID="txtCode" runat="server"
                    Text='<%# Bind("country_code") %>' Width="70px" MaxLength="5"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode"
                ToolTip="Kindly provide international code before saving"   ErrorMessage="!" ForeColor="Red"  Font-Bold="true"  >
                </asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr>
              <td>
                <label id="lblAbbrev" runat="server">Intl. Abbrev:</label>
              </td>
              <td>
               <asp:TextBox ID="txtAbbrev" runat="server"
                    Text='<%# Bind("abbrev") %>' Width="70px" MaxLength="3"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAbbrev" runat="server" ControlToValidate="txtAbbrev"
                ToolTip="Kindly provide international abbreviation before saving"    ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                </asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr>
              <td>
                <label id="lblCurrency" runat="server">Currency:</label>
              </td>
              <td> 
                    <telerik:RadComboBox ID="cboCur" runat="server" Height="200px" Width="200px"
                            DropDownWidth="298px" EmptyMessage="Select a Currency" HighlightTemplatedItems="true"
                            MarkFirstMatch="true" Filter="Contains" OnItemsRequested="cboCur_ItemsRequested" 
                            EnableLoadOnDemand="true" OnSelectedIndexChanged="cboCur_SelectedIndexChanged" AutoPostBack="true"
                            OnLoad="cboCur_Load">
                            <HeaderTemplate>
                                <table style="width: 275px" cellspacing="0" cellpadding="0">
                                    <tr> 
                                        <td style="width: 177px;">
                                            Currency Name</td>
                                        <td style="width: 60px;">
                                            Rate</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 275px" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 177px;">
                                            <%# DataBinder.Eval(Container, "Text")%>
                                        </td>
                                        <td style="width: 60px;">
                                            <%# DataBinder.Eval(Container, "Attributes['current_buy_rate']")%>
                                        </td> 
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>                
                <asp:RequiredFieldValidator ID="rfvCurrency" runat="server" ControlToValidate="cboCur"
                ToolTip="Kindly provide currency before saving"    ErrorMessage="!" ForeColor="Red"  
                Font-Bold="true">
                </asp:RequiredFieldValidator>
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
     </telerik:RadAjaxPanel> 
    
    <asp:ImageButton ID="btnExcel" OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord"  OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" />
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="countries">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="regions" Where="it.countries.country_id == @country_id">
        <WhereParameters>
            <asp:SessionParameter Name="country_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="districts" Where="it.regions.region_id == @region_id">
        <WhereParameters>
            <asp:SessionParameter Name="region_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource4" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="cities" Where="it.districts.district_id == @district_id">
        <WhereParameters>
            <asp:SessionParameter Name="district_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource5" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="locations" Where="it.cities.city_id == @city_id">
        <WhereParameters>
            <asp:SessionParameter Name="city_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
</asp:Content>
