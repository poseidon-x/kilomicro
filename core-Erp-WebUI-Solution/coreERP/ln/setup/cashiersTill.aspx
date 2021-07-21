<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="cashiersTill.aspx.cs" Inherits="coreERP.ln.setup.cashiersTill" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Cashier's Till
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
        <h3>Manage Loan Types</h3> 
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnItemCreated="RadGrid1_ItemCreated"
        OnUpdateCommand="RadGrid1_UpdateCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="PopUp"
            datakeynames="cashiersTillID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="1260px">
    <Columns>
        <telerik:GridBoundColumn DataField="cashiersTillID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="cashiersTillID" ReadOnly="True" 
            SortExpression="cashiersTillID" UniqueName="cashiersTillID" Visible="false">
        </telerik:GridBoundColumn>  
        <telerik:GridTemplateColumn DataField="accountID" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="G/L Account" 
            SortExpression="accountID" UniqueName="accountID" ItemStyle-Width="280px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# AccountName(int.Parse(DataBinder.Eval(Container.DataItem, "accountID").ToString()))%></span>
            </ItemTemplate>
            <EditItemTemplate> 
                 <telerik:RadComboBox ID="accountID" runat="server" Height="150px" Width="300px" DataTextField="fullname" DataValueField="acct_id" 
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
        <telerik:GridTemplateColumn DataField="userName" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Cashier's User Name" 
            SortExpression="userName" UniqueName="userName" ItemStyle-Width="280px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# UserName(DataBinder.Eval(Container.DataItem, "userName").ToString())%></span>
            </ItemTemplate>
            <EditItemTemplate> 
                 <telerik:RadComboBox ID="userName" runat="server" Height="150px" Width="200px"
                    DropDownWidth=" 355px" EmptyMessage="Select a User" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""   
                    EnableLoadOnDemand="true" AutoPostBack="true" SelectedValue='<%# DataBinder.Eval(Container.DataItem,"userName") %>'
                    > 
                </telerik:RadComboBox> 
            </EditItemTemplate>
        </telerik:GridTemplateColumn> 
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected loanType?">
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
        EntitySetName="cashiersTills">
    </ef:EntityDataSource>
    </asp:Content>
