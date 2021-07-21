<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="tenor.aspx.cs" Inherits="coreERP.ln.setup.tenor" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Loan Tenor
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
        <h3>Manage Loan Tenor</h3>  
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnItemCreated="RadGrid1_ItemCreated"
        OnUpdateCommand="RadGrid1_UpdateCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="PopUp"
            datakeynames="tenorID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="1260px">
    <Columns>
        <telerik:GridBoundColumn DataField="tenorID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="tenorID" ReadOnly="True" 
            SortExpression="tenorID" UniqueName="tenorID" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="tenor1" DefaultInsertValue="" 
            HeaderText="tenor (Months)" SortExpression="tenor1" UniqueName="tenor1" ItemStyle-Width="220px">
        </telerik:GridBoundColumn>  
        <telerik:GridDropDownColumn DataField="loanTypeID" DataSourceID="eds2" ListValueField="loanTypeID"
             ListTextField="loanTypeName" HeaderText="Loan Product"></telerik:GridDropDownColumn>
        <telerik:GridNumericColumn DataField="defaultInterestRate" DefaultInsertValue="" 
            HeaderText="Int. Rate" SortExpression="defaultInterestRate" UniqueName="defaultInterestRate" ItemStyle-Width="80px">
        </telerik:GridNumericColumn>  
        <telerik:GridNumericColumn DataField="defaultPenaltyRate" DefaultInsertValue="" 
            HeaderText="Penalty Rate" SortExpression="defaultPenaltyRate" UniqueName="defaultPenaltyRate" ItemStyle-Width="80px">
        </telerik:GridNumericColumn>  
        <telerik:GridNumericColumn DataField="defaultGracePeriod" DefaultInsertValue="" 
            HeaderText="Grace Period" SortExpression="defaultGracePeriod" UniqueName="defaultGracePeriod" ItemStyle-Width="80px">
        </telerik:GridNumericColumn>  
        <telerik:GridNumericColumn DataField="defaultApplicationFeeRate" DefaultInsertValue="" 
            HeaderText="App. Fee Rate" SortExpression="defaultApplicationFeeRate" UniqueName="defaultApplicationFeeRate" ItemStyle-Width="80px">
        </telerik:GridNumericColumn>  
        <telerik:GridNumericColumn DataField="defaultProcessingFeeRate" DefaultInsertValue="" 
            HeaderText="Proc. Fee Rate" SortExpression="defaultProcessingFeeRate" UniqueName="defaultProcessingFeeRate" ItemStyle-Width="80px">
        </telerik:GridNumericColumn>  
        <telerik:GridNumericColumn DataField="defaultCommissionRate" DefaultInsertValue="" 
            HeaderText="Commission Rate" SortExpression="defaultCommissionRate" UniqueName="defaultCommissionRate" ItemStyle-Width="80px">
        </telerik:GridNumericColumn>  
        <telerik:GridTemplateColumn DataField="defaultRepaymentModeID" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Repayment Mode" 
            SortExpression="defaultRepaymentModeID" UniqueName="defaultRepaymentModeID" ItemStyle-Width="80px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# RepyamentModeName(DataBinder.Eval(Container.DataItem, "defaultRepaymentModeID"))%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                 <telerik:RadComboBox ID="defaultRepaymentModeID" runat="server" Height="150px" Width="200px"
                    DropDownWidth=" 355px" EmptyMessage="Select a Repayment Mode" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""    
                    EnableLoadOnDemand="true" AutoPostBack="true" SelectedValue='<%# DataBinder.Eval(Container.DataItem,"defaultRepaymentModeID") %>'>                    
                </telerik:RadComboBox>
                </span>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected tenor?">
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
        EntitySetName="tenors">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="eds2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="loanTypes">
    </ef:EntityDataSource>
    </asp:Content>
