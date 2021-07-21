<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="True" CodeBehind="def_acct.aspx.cs" Inherits="coreERP.gl.accounts.def_acct" %>
<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %> 
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Default G/L Accounts
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
        <h3>Default G/L Accounts</h3> 
    <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticDeletes="true"
        AllowSorting="true" ShowFooter="true"  OnItemCreated="RadGrid1_ItemCreated"
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnUpdateCommand="RadGrid1_UpdateCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
    <MasterTableView autogeneratecolumns="False" EditMode="PopUp"
            datakeynames="def_acct_id" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True"
               CommandItemDisplay="Top" Width="640px">
             <CommandItemSettings AddNewRecordText="Add A Default Account" />
     
    <Columns>
        <telerik:GridBoundColumn DataField="def_acct_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="def_acct_id" ReadOnly="True" 
            SortExpression="def_acct_id" UniqueName="def_acct_id" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="code" DataType="System.String" 
            DefaultInsertValue="" HeaderText="Description" 
            SortExpression="code" UniqueName="code" ItemStyle-Width="200px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# CodeName(DataBinder.Eval(Container.DataItem, "code").ToString())%></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="acct_id" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Account" 
            SortExpression="acct_id" UniqueName="acct_id" ItemStyle-Width="200px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# AccountName(int.Parse(DataBinder.Eval(Container.DataItem, "def_acct_id").ToString()))%></span>
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
    <EditFormSettings InsertCaption="Add new Default Account" CaptionFormatString="Edit Default Account: {0}" 
        CaptionDataField="description" EditFormType="Template" PopUpSettings-Modal="true">
        <FormTemplate>
           <table style="width: 450px">
            <tbody>
             <tr>
              <td>
                <label id="Label2" runat="server">Description:</label>
              </td>
              <td> 
                    <telerik:RadComboBox ID="cboCode" runat="server" Height="200px" Width=" 255px"
                            DropDownWidth=" 255px" EmptyMessage="Select a GL Account" HighlightTemplatedItems="true"
                            Filter="Contains"  DataTextField="description" DataValueField="code"
                            OnSelectedIndexChanged="cboCode_SelectedIndexChanged" AutoPostBack="true"
                            MarkFirstMatch="true">
                            <HeaderTemplate>
                                <table style="width: 255px" cellspacing="0" cellpadding="0">
                                    <tr> 
                                        <td style="width: 80px;">
                                            Code</td>
                                        <td style="width: 230px;">
                                            Description</td> 
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 255px" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 80px;" >
                                            <%# Eval("code")%>
                                        </td>
                                        <td style="width: 230px;">
                                            <%# Eval("description")%>
                                        </td> 
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>                
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cboCode"
                ToolTip="Kindly provide description before saving"    ErrorMessage="!" ForeColor="Red"  
                Font-Bold="true">
                </asp:RequiredFieldValidator>
              </td>
             </tr>
             <tr>
              <td>
                <label id="lblGLAcc" runat="server">G/L Account:</label>
              </td>
              <td> 
                    <telerik:RadComboBox ID="cboGLAcc" runat="server" Height="200px" Width=" 255px"
                            DropDownWidth=" 255px" EmptyMessage="Select a GL Account" HighlightTemplatedItems="true"
                            Filter="Contains"  DataTextField="acc_name" DataValueField="acct_id"
                            OnSelectedIndexChanged="cboGLAcc_SelectedIndexChanged" AutoPostBack="true"
                            MarkFirstMatch="true">
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
                                            <%# Eval("acc_num")%>
                                        </td>
                                        <td style="width: 150px;">
                                            <%# Eval("acc_name")%>
                                        </td> 
                                        <td style="width: 80px;">
                                            <%# Eval("major_name")%>
                                        </td> 
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>                
                <asp:RequiredFieldValidator ID="rfvCurrency" runat="server" ControlToValidate="cboGLAcc"
                ToolTip="Kindly provide G/L Account before saving"    ErrorMessage="!" ForeColor="Red"  
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
    
    <asp:ImageButton ID="btnExcel" OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord"  OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" />
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="def_accts">
    </ef:EntityDataSource>
</asp:Content>
