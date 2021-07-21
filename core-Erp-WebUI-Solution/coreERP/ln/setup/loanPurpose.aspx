<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="loanPurpose.aspx.cs" Inherits="coreERP.ln.setup.loanPurpose" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Loan Purposes
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
        <h3>Manage Loan Purposes</h3> 
     <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true"  
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted"
        OnUpdateCommand="RadGrid1_UpdateCommand" MasterTableView-DataKeyNames="loanPurposeID">
         
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="loanPurposeID" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px" >
            <CommandItemSettings AddNewRecordText="Add A Loan Purpose Item" />  
    <DetailTables>
        <telerik:GridTableView DataSourceID="EntityDataSource2" AllowAutomaticDeletes="true" 
         AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AutoGenerateColumns="false"
         CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="loanPurposeDetailID">
            <CommandItemSettings AddNewRecordText="Add A Detail Purpose Item" />            
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="loanPurposeID" MasterKeyField="loanPurposeID" />
            </ParentTableRelation>
            <Columns>
                <telerik:GridBoundColumn DataField="loanPurposeDetailID" DataType="System.Int32" 
                    DefaultInsertValue="" HeaderText="loanPurposeDetailID" ReadOnly="True" 
                    SortExpression="loanPurposeDetailID" UniqueName="loanPurposeDetailID" Visible="false">
                </telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="loanPurposeDetailName" DefaultInsertValue="" 
                    HeaderText="Detailed Purpose" SortExpression="loanPurposeDetailName" UniqueName="loanPurposeDetailName" 
                    ItemStyle-Width="550px">
                </telerik:GridBoundColumn> 
                <telerik:GridCheckBoxColumn DataField="accepted" DefaultInsertValue="" 
                    HeaderText="Accepted?" SortExpression="accepted" UniqueName="accepted" 
                    ItemStyle-Width="150px">
                </telerik:GridCheckBoxColumn>   
                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                    ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridEditCommandColumn>
                  <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                    ConfirmText="Are you sure you want to delete the selected detail?">
                  </telerik:GridButtonColumn>
            </Columns>
        </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="loanPurposeID" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="Category ID" ReadOnly="True" 
            SortExpression="loanPurposeID" UniqueName="loanPurposeID" visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="loanPurposeName" DefaultInsertValue="" 
            HeaderText="Loan Purpose" SortExpression="loanPurposeName" UniqueName="loanPurposeName" ItemStyle-Width="520px">
        </telerik:GridBoundColumn>  
        <telerik:GridCheckBoxColumn DataField="accepted" DefaultInsertValue="" 
            HeaderText="Accepted?" SortExpression="accepted" UniqueName="accepted" 
            ItemStyle-Width="150px">
        </telerik:GridCheckBoxColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
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
        EntitySetName="loanPurposes">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="loanPurposeDetails" Where="it.loanPurpose.loanPurposeID == @loanPurposeID">
        <WhereParameters>
            <asp:SessionParameter Name="loanPurposeID" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
    </asp:Content>
