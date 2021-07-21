<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="True" CodeBehind="def_acct_names.aspx.cs" Inherits="coreERP.gl.accounts.def_acct_names" %>
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
            datakeynames="code" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True"
               CommandItemDisplay="Top" Width="640px">
             <CommandItemSettings AddNewRecordText="Add A Default Account Name" />
     
    <Columns>
        <telerik:GridBoundColumn DataField="code" DefaultInsertValue="" 
            HeaderText="Code" SortExpression="code" UniqueName="code" ItemStyle-Width="40px">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="description" DefaultInsertValue="" 
            HeaderText="Description" SortExpression="description" UniqueName="description" ItemStyle-Width="250px">
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
    <EditFormSettings InsertCaption="Add new Default Account Name" CaptionFormatString="Edit Default Account Name: {0}" 
        CaptionDataField="description" EditFormType="Template" PopUpSettings-Modal="true">
        <FormTemplate>
           <table style="width: 450px">
            <tbody>
             <tr>
              <td>
                <label id="lblCode" runat="server">Code:</label>
              </td>
              <td>
               <asp:TextBox ID="txtCode" runat="server"
                    Text='<%# Bind("code") %>' Width="200px" MaxLength="5"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCde" runat="server" ControlToValidate="txtCode"
                ToolTip="Kindly provide a code for the default account"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                </asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr>
              <td>
                <label id="Label1" runat="server">Description:</label>
              </td>
              <td>
               <asp:TextBox ID="txtDescription" runat="server"
                    Text='<%# Bind("description") %>' Width="200px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescription"
                ToolTip="Kindly provide a description for the default account"  ErrorMessage="!" ForeColor="Red"  Font-Bold="true">
                </asp:RequiredFieldValidator>
                </td>
             </tr>
            </tbody>
           </table>
               <asp:TextBox ID="txtCreator" runat="server"
                    Text='<%# Bind("creator") %>' Width="70px" MaxLength="50" Visible="false"></asp:TextBox> 
               <telerik:RadDateTimePicker ID="diCreationDate" runat="server"
                      Width="70px" Visible="false"></telerik:RadDateTimePicker>
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
        EntitySetName="def_acct_names">
    </ef:EntityDataSource>
</asp:Content>
