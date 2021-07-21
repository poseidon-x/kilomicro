<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="True" CodeBehind="banks.aspx.cs" Inherits="coreERP.gl.accounts.banks" %>
<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %> 
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Banks, Branches &amp; Accounts
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
        <h3>Banks, Branches &amp; Accounts</h3> 
    <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticDeletes="true"
        AllowSorting="true" ShowFooter="true" AllowPaging="true"
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnUpdateCommand="RadGrid1_UpdateCommand" >
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
    <MasterTableView autogeneratecolumns="False" EditMode="PopUp"
            datakeynames="bank_id" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True"
               CommandItemDisplay="Top" Width="960px">
             <CommandItemSettings AddNewRecordText="Add A Bank" />
     
     <DetailTables>
        <telerik:GridTableView AutoGenerateColumns="false" EditMode="PopUp"
        DataKeyNames="branch_id" DataSourceID="EntityDataSource2" 
        AllowAutomaticDeletes="true"
        CommandItemDisplay="Top" AllowAutomaticUpdates="true"
        Width="500px">
             <CommandItemSettings AddNewRecordText="Add A Bank Branch" />
             <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="bank_id" MasterKeyField="bank_id" />
             </ParentTableRelation>
     
     
     
             <DetailTables>
                <telerik:GridTableView AutoGenerateColumns="false" EditMode="PopUp"
                DataKeyNames="bank_acct_id" DataSourceID="EntityDataSource3" 
                AllowAutomaticDeletes="true" AllowSorting="true"
                CommandItemDisplay="Top" AllowAutomaticUpdates="true"
                Width="700px">
                     <CommandItemSettings AddNewRecordText="Add A Bank Account" />
                     <ParentTableRelation>
                        <telerik:GridRelationFields DetailKeyField="branch_id" MasterKeyField="branch_id" />
                     </ParentTableRelation>
                          
                     <Columns>
                        <telerik:GridBoundColumn DataField="bank_acct_id" DataType="System.Int32" 
                            DefaultInsertValue="" HeaderText="bank_acct_id" ReadOnly="True" 
                            SortExpression="bank_acct_id" UniqueName="bank_acct_id" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="bank_acct_num" DefaultInsertValue="" 
                            HeaderText="Account Number" SortExpression="bank_acct_num" UniqueName="bank_acct_num" 
                            ItemStyle-Width="150px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="bank_acct_desc" DefaultInsertValue="" 
                            HeaderText="Account Description" SortExpression="bank_acct_desc" UniqueName="bank_acct_desc" 
                            ItemStyle-Width="250px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridTemplateColumn DataField="gl_acct_id" DataType="System.Int32" 
                            DefaultInsertValue="" HeaderText="GL Account" 
                            SortExpression="gl_acct_id" UniqueName="gl_acct_id" ItemStyle-Width="250px"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <span><%# AccountName((int)DataBinder.Eval(Container.DataItem, "bank_acct_id"))%></span>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="gl_acct_id2" DataType="System.Int32" 
                            DefaultInsertValue="" HeaderText="GL Account" 
                            SortExpression="gl_acct_id" UniqueName="gl_acct_id2" ItemStyle-Width="150px"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <span><%# AccountCurrency((int)DataBinder.Eval(Container.DataItem, "bank_acct_id"))%></span>
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
                            ConfirmText="Are you sure you want to delete the selected bank?">
                          </telerik:GridButtonColumn>
                    </Columns> 
                    <EditFormSettings InsertCaption="Add new Bank Account" CaptionFormatString="Edit Bank Account: {0}" 
                        CaptionDataField="bank_acct_num" EditFormType="Template" PopUpSettings-Modal="true">
                        <FormTemplate>
                           <table style="width: 450px">
                            <tbody>
                             <tr>
                              <td>
                                <label id="lblDistrictName" runat="server">Account Number:</label>
                              </td>
                              <td>
                               <asp:TextBox ID="txtDistrictName" runat="server"
                                    Text='<%# Bind("bank_acct_num") %>' Width="100px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDistrictName" runat="server" ControlToValidate="txtDistrictName"
                                ToolTip="Kindly provide district name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                </asp:RequiredFieldValidator>
                                </td>
                             </tr>
                             <tr>
                              <td>
                                <label id="Label1" runat="server">Account Description:</label>
                              </td>
                              <td>
                               <asp:TextBox ID="TextBox1" runat="server"
                                    Text='<%# Bind("bank_acct_desc") %>' Width="250px" MaxLength="250"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
                                ToolTip="Kindly provide district name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                                </asp:RequiredFieldValidator>
                                </td>
                             </tr>
                             <tr>
                              <td>
                                <label id="lblGLAcc" runat="server">GL Account:</label>
                              </td>
                              <td> 
                                    <telerik:RadComboBox ID="cboGLAcc" runat="server" Height="200px" Width=" 255px"
                                            DropDownWidth=" 255px" EmptyMessage="Select a GL Account" HighlightTemplatedItems="true"
                                            Filter="Contains"  
                                            EnableLoadOnDemand="true" OnSelectedIndexChanged="cboGLAcc_SelectedIndexChanged" AutoPostBack="true"
                                            MarkFirstMatch="true" OnLoad="cboGLAcc_Load">
                                            <HeaderTemplate>
                                                <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                    <tr> 
                                                        <td style="width: 80px;">
                                                            Acc Num</td>
                                                        <td style="width: 150px;">
                                                            Acc Name</td>
                                                        <td style="width: 80px;">
                                                            Acc Currency</td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 80px;" >
                                                            <%# DataBinder.Eval(Container, "Attributes['acc_num']")%>
                                                        </td>
                                                        <td style="width: 150px;">
                                                            <%# DataBinder.Eval(Container, "Attributes['acc_name']")%>
                                                        </td> 
                                                        <td style="width: 80px;">
                                                            <%# DataBinder.Eval(Container, "Attributes['acc_cur']")%>
                                                        </td> 
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>  
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
                <telerik:GridBoundColumn DataField="branch_id" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="branch_id" ReadOnly="True" 
                    SortExpression="branch_id" UniqueName="branch_id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="branch_name" DefaultInsertValue="" 
                    HeaderText="Branch Name" SortExpression="branch_name" UniqueName="branch_name" ItemStyle-Width="250px">
                </telerik:GridBoundColumn>   
                <telerik:GridTemplateColumn DataField="location_id" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="Branch Location" 
                    SortExpression="location_id" UniqueName="location_id" ItemStyle-Width="150px"
                    ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <span><%# LocationName((int)DataBinder.Eval(Container.DataItem, "branch_id"))%></span>
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
                    ConfirmText="Are you sure you want to delete the selected bank branch?">
                  </telerik:GridButtonColumn>
            </Columns> 
            <EditFormSettings InsertCaption="Add new Bank Branch" CaptionFormatString="Edit Bank Branch: {0}" 
                CaptionDataField="branch_name" EditFormType="Template" PopUpSettings-Modal="true">
                <FormTemplate>
                   <table style="width: 550px">
                    <tbody>
                     <tr>
                      <td>
                        <label id="lblCountryName" runat="server">Branch Name:</label>
                      </td>
                      <td>
                       <asp:TextBox ID="txtRegionName" runat="server"
                            Text='<%# Bind("branch_name") %>' Width="200px" MaxLength="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRegionName" runat="server" ControlToValidate="txtRegionName"
                        ToolTip="Kindly provide branch name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                        </asp:RequiredFieldValidator>
                        </td>
                     </tr>
                             <tr>
                              <td>
                                <label id="lblLoc" runat="server">Branch Location:</label>
                              </td>
                              <td> 
                                    <telerik:RadComboBox ID="cboLoc" runat="server" Height="200px" Width=" 255px"
                                            DropDownWidth=" 255px" EmptyMessage="Select a branch location" HighlightTemplatedItems="true"
                                            MarkFirstMatch="false" Filter="Contains"
                                            EnableLoadOnDemand="false" OnSelectedIndexChanged="cboLoc_SelectedIndexChanged" AutoPostBack="true"
                                            OnLoad="cboLoc_Load" >
                                            <HeaderTemplate>
                                                <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                    <tr> 
                                                        <td style="width: 100px;">
                                                            City</td>
                                                        <td style="width: 150px;">
                                                            Location</td> 
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table style="width: 255px" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <%# DataBinder.Eval(Container, "Attributes['city_name']")%>
                                                        </td> 
                                                        <td >
                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>                
                                
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
        <telerik:GridBoundColumn DataField="bank_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="bank_id" ReadOnly="True" 
            SortExpression="bank_id" UniqueName="bank_id" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="bank_name" DefaultInsertValue="" 
            HeaderText="Bank Name" SortExpression="bank_name" UniqueName="bank_name" ItemStyle-Width="220px">
        </telerik:GridBoundColumn> 
        <telerik:GridTemplateColumn DataField="commission_rate" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Commission Rate(%)" 
            SortExpression="commission_rate" UniqueName="commission_rate" ItemStyle-Width="150px"
            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" >
            <ItemTemplate>
                <span><%# ((double)DataBinder.Eval(Container.DataItem, "commission_rate")).ToString("#,##0.###0;(#,##0.####);0")%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                    <asp:TextBox ID="txtDbt" runat="server"
                        Text='<%# Bind("commission_rate") %>'>
                    </asp:TextBox>
                </span>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridBoundColumn DataField="full_name" DefaultInsertValue="" 
            HeaderText="Full Name" SortExpression="full_name" UniqueName="full_name" ItemStyle-Width="220px">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="institution_type" DataType="System.String" 
            DefaultInsertValue="" HeaderText="Institution Type" 
            SortExpression="institution_type" UniqueName="institution_type" ItemStyle-Width="150px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" >
            <ItemTemplate>
                <span><%# DataBinder.Eval(Container.DataItem, "institution_type") %></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                    <telerik:RadComboBox runat="server" ID="cboIT" SelectedValue='<%# Bind ("institution_type") %>'>
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                            <telerik:RadComboBoxItem Value="Bank" Text="Bank" />
                            <telerik:RadComboBoxItem Value="Rural Bank" Text="Rural Bank" />
                            <telerik:RadComboBoxItem Value="Savings &amp; Loans" Text="Savings &amp; Loans" />
                        </Items>
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
    <EditFormSettings InsertCaption="Add new Bank" CaptionFormatString="Edit Bank: {0}" 
        CaptionDataField="bank_name" EditFormType="Template" PopUpSettings-Modal="true">
        <FormTemplate>
           <table style="width: 450px">
            <tbody>
             <tr>
              <td>
                <label id="lblCountryName" runat="server">Bank Name:</label>
              </td>
              <td>
               <asp:TextBox ID="txtCountryName" runat="server"
                    Text='<%# Bind("bank_name") %>' Width="200px" MaxLength="150"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCountryName" runat="server" ControlToValidate="txtCountryName"
                ToolTip="Kindly provide bank name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                </asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr>
              <td>
                <label id="Label2" runat="server">Comm. Rate:</label>
              </td>
              <td>
               <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtCommRate" runat="server" 
                        Value='<%# Bind("commission_rate") %>' EnabledStyle-HorizontalAlign="Right" 
                        Width="150px">
                        <NumberFormat AllowRounding="false" DecimalDigits="4" KeepNotRoundedValue="true" />
                    </telerik:RadNumericTextBox>
                </span>
                </td>
             </tr>
             <tr>
              <td>
                <label id="Label3" runat="server">Full Name:</label>
              </td>
              <td>
               <asp:TextBox ID="TextBox2" runat="server"
                    Text='<%# Bind("full_name") %>' Width="200px" MaxLength="150"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox2"
                ToolTip="Kindly provide bank full name before saving"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                </asp:RequiredFieldValidator>
                </td>
             </tr>
                <tr>
                    <td>Institution Type:</td>
                    <td>
                         <telerik:RadComboBox runat="server" ID="cboIT" SelectedValue='<%# Bind ("institution_type") %>'>
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="" />
                                <telerik:RadComboBoxItem Value="Bank" Text="Bank" />
                                <telerik:RadComboBoxItem Value="Rural Bank" Text="Rural Bank" />
                                <telerik:RadComboBoxItem Value="Savings &amp; Loans" Text="Savings &amp; Loans" />
                            </Items>
                        </telerik:RadComboBox>
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
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="banks">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="bank_branches" Where="it.banks.bank_id == @bank_id">
        <WhereParameters>
            <asp:SessionParameter Name="bank_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="bank_accts" Where="it.bank_branches.branch_id == @branch_id">
        <WhereParameters>
            <asp:SessionParameter Name="branch_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
</asp:Content>
