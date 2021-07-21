<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="pc.aspx.cs" Inherits="coreERP.gl.pc.pc" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>


<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Petty Cash Transactions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

<script type="text/javascript">
    function toPlural(item) {
        var rtr = item;
        var itemString = item.toString();
        var lastDigit = itemString.substring(itemString.length-1,1);
        var lastDigit2 = itemString.substring(itemString.length-2,1);
        if (lastDigit == "s" || lastDigit2 == "es") { }
        else if (lastDigit == "y" || lastDigit == "i") {
            rtr = itemString.substring(0, itemString.length - 1);
        }
        else if (lastDigit == "ch" || lastDigit == "sh") {
        rtr = itemString + "es";
        }
        else {
            rtr = itemString + "s";
        }
        return rtr;
    } 
    function onClientContextMenuShowing(sender, args) {
        var treeNode = args.get_node();
        treeNode.set_selected(true);
        //enable/disable menu items
        setMenuItemsState(args.get_menu().get_items(), treeNode);
    }

    function onClientContextMenuItemClicking(sender, args) {
        var menuItem = args.get_menuItem();
        var treeNode = args.get_node();
        menuItem.get_menu().hide();

        switch (menuItem.get_value()) {
            case "New":
                showProcessing();
                break;
            case "NewAccount": 
                showProcessing();
                break;
            case "Edit": 
                showProcessing();
                break;
            case "Delete":
                var itemType = treeNode.get_value().split(':')[0];
                var msg = "Deleting the " + treeNode.get_text() + " "
                    + treeNode.get_parent().get_text()
                    + " will remove OU information"
                    + "for all transactions with their OU set to it. Also all sub units will be removed."
                    + "\nDo you still want to continue?";
                if (itemType == "b") {
                    msg = "Deleting the " + treeNode.get_text() + " batch "
                    + "will remove all its corresponding transactions"
                    + ".\nDo you still want to continue?";
                    var result = confirm(msg);
                    args.set_cancel(!result);
                }
                else {
                    args.set_cancel(true);
                }
                break;
        }
    }

    //this method disables the appropriate context menu items
    function setMenuItemsState(menuItems, treeNode) {
        for (var i = 0; i < menuItems.get_count(); i++) {
            var menuItem = menuItems.getItem(i);
            var nodeType = treeNode.get_value().split(':')[0];
            switch (menuItem.get_value()) {
                case "New":
                    //alert(treeNode.get_nodes().get_count());  
                    menuItem.set_enabled(true);
                    var msg = 'Create new Petty Cash Transactions'; 
                    formatMenuItem(menuItem, treeNode, msg);
                    break;
                case "NewAccount":
                    //alert(treeNode.get_nodes().get_count());
                    if (nodeType != 'h') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                        msg = 'Create New Account' 
                        formatMenuItem(menuItem, treeNode, msg);
                    }
                    break;
                case "Edit": 
                    if (nodeType != 'b') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                    } 
                    break;
                case "Delete":
                    if (nodeType != 'b') {
                        menuItem.set_enabled(false);
                    } 
                    else {
                        menuItem.set_enabled(true);
                    } 
                    break;  
            }
        }
    }

    //formats the Text of the menu item
    function formatMenuItem(menuItem, treeNode, formatString) {
        var nodeValue = treeNode.get_value();
        /*if (nodeValue && nodeValue.indexOf("_Private_") == 0) {
            menuItem.set_enabled(false);
        }
        else {
            menuItem.set_enabled(true);
        }
        var newText = String.format(formatString, extractTitleWithoutMails(treeNode));*/
        menuItem.set_text(formatString); 
    }

    function showProcessing() {
        var divEl = document.getElementById('<%= divProc.ClientID %>');
        if (divEl != null) divEl.style["visibility"] = "visible";
        
        return true;
    }

    function ClearSelection() {
         
    }
        </script>
   <script type="text/javascript">
       /* <![CDATA[ */
       function onNodeDragging(sender, args) {
           var target = args.get_htmlElement();

           if (!target) return;

           if (target.tagName == "INPUT") {
               target.style.cursor = "hand";
           }
       }
 
       function onNodeDropping(sender, args) {
           var dest = args.get_destNode();
           var src = args.get_sourceNode();
           if (dest) {
               if (dest.get_value() != src.get_parent().get_value()) {
                   var nodeType1 = dest.get_value().split(':')[0];
                   var nodeType2 = src.get_value().split(':')[0];
                   if (nodeType1 == "__root__" || (nodeType1 == "a")
                    || nodeType2 == "__root__" || (nodeType1 == "c" && nodeType2 == "a")
                    || nodeType2 == "c") {
                       args.set_cancel(true);
                       return;
                   }
               }
           }
           else {
               //dropOnHtmlElement(args);
           }
       }
       /* ]]> */
            </script>

</telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h3>Petty Cash Transactions</h3>
          
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="100%" Width="100%" OnClientLoaded="SplitterLoadedInner">
        <telerik:RadPane ID="RadPane1" runat="server" Width="15.0%">
                    
        <telerik:RadTreeView ID="RadTreeView1"  runat="server" Height="100%" Width="340px" 
                AllowNodeEditing="false"  CausesValidation="false"
                OnNodeExpand="RadTreeView1_NodeExpand"
                OnNodeClick="RadTreeView1_NodeClick" EnableEmbeddedScripts="true"
                OnContextMenuItemClick="RadTreeView1_ContextMenuItemClick" 
                OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                OnClientContextMenuShowing="onClientContextMenuShowing" > 
        <ContextMenus>
                    <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                        <Items>
                            <telerik:RadMenuItem Value="New" Text="Create new Petty Cash Transactions" ImageUrl="~/images/new.jpg">
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="Edit" Text="Edit Petty Cash" Enabled="false" ImageUrl="~/images/edit.jpg"
                                >
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="Delete" Text="Delete Petty Cash" ImageUrl="~/images/delete.jpg">
                            </telerik:RadMenuItem>
                        </Items>
                        <CollapseAnimation Type="none" />
                    </telerik:RadTreeViewContextMenu>
                    <telerik:RadTreeViewContextMenu ID="EmptyFolderContextMenu" runat="server">
                        <Items>
                            <telerik:RadMenuItem Value="New" Text="Create new Petty Cash Transactions" ImageUrl="~/images/new.jpg">
                            </telerik:RadMenuItem>
                        </Items>
                        <CollapseAnimation Type="none" />
                    </telerik:RadTreeViewContextMenu>
                </ContextMenus> 
        </telerik:RadTreeView>
    
        </telerik:RadPane> 
        <telerik:RadPane ID="contentPane" runat="server" Width="100%">
             
            <asp:Panel runat="server" ID="pnlEditJournal" Visible="true">
                   <table style="width: 100%">
                        <tr>
                            <td colspan="2">
                               <a href="~/common/prof.aspx" runat="server" id="linkCompProf" 
                                target="_blank"><img src="~/images/comp_prof.jpg" 
                                     runat="server" alt="Company Profile"
                                     width="32" height="32" title="Company Profile" />
                               </a>
                               <a href="~/gl/accounts/default.aspx" runat="server" id="linkCA" 
                                target="_blank"><img id="Img1" src="../../images/chart_of_accounts/chart_of_accounts.jpg" 
                                     runat="server" alt="Chart of Accounts"
                                     width="32" height="32" title="Chart of Accunts" />
                               </a>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <span class="leftboldtxt" style="width:300px">Edit Petty Cash Transactions</span>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:100px">Batch Date:</td>
                            <td>
                                <telerik:RadDatePicker ID="dtTransactionDate" runat="server" DateInput-DisplayDateFormat="dd-MMM-yyyy"
                                onselecteddatechanged="dtTransactionDate_SelectedDateChanged" AutoPostBack="true" ></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="dtTransactionDate"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the transaction date.">
                                </asp:RequiredFieldValidator>                     
                            </td> 
                            <td>Batch Originator:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtCreator" MaxLength="20" 
                                    Width="100" ReadOnly="true" Enabled="false" Text="UNPOSTED">
                                </telerik:RadTextBox>                     
                            </td> 
                        </tr>  
                        <tr>
                            <td>Batch Number:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtBatchNumber" MaxLength="20" 
                                    Width="100" ReadOnly="true" Enabled="false">
                                </telerik:RadTextBox>                     
                            </td> 
                            <td>Batch State:</td> 
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtBatchState" MaxLength="20" 
                                    Width="100" ReadOnly="true" Enabled="false">
                                </telerik:RadTextBox>                     
                            </td> 
                        </tr>
                        <tr>
                            <td >
                                Petty Cash Account
                            </td>
                            <td align="left">
                             <telerik:RadComboBox ID="cboAcc" runat="server" Height="150px" Width=" 300px" DataTextField="fullname" DataValueField="acct_id"
                                DropDownWidth=" 500px" EmptyMessage="Type name or number of Account" HighlightTemplatedItems="true"
                                MarkFirstMatch="true" AutoCompleteSeparator="" 
                                AppendDataBoundItems="true" AutoPostBack="true"
                               EnableLoadOnDemand="true" OnItemsRequested="cboGLAcc_ItemsRequested"
                                OnSelectedIndexChanged="cboAcc_SelectedIndexChanged">
                                <HeaderTemplate>
                                    <table style="width: 500px">
                                        <tr> 
                                            <td style="width: 80px;">
                                                Acc Num</td>
                                            <td style="width: 320px;">
                                                Acc Name</td>
                                            <td style="width: 100px;">
                                                Currency</td>
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
                            </td>
                            <td>Account Balance:</td> 
                            <td>
                                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID = "txtBalance" runat="server" Enabled="false" ReadOnly="true"></telerik:RadNumericTextBox>                     
                            </td> 
                        </tr>                      
                        <tr> 
                            <td>Currency:</td> 
                            <td>                  
                                 <telerik:RadComboBox ID="cboCur" runat="server" Height="150px" Width=" 255px"
                                    DropDownWidth=" 255px" EmptyMessage="Transaction Currency" HighlightTemplatedItems="true"
                                    MarkFirstMatch="true" AutoCompleteSeparator="" AutoPostBack="true"
                                     DataTextField="major_name" DataValueField="currency_id"
                                    OnSelectedIndexChanged="cboCur_SelectedIndexChanged">
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
                                                    <%# Eval("major_symbol")%>
                                                </td>
                                                <td style="width: 150px;">
                                                    <%# Eval("major_name")%>
                                                </td> 
                                                <td style="width: 80px;">
                                                    <%# Eval("current_buy_rate")%>
                                                </td> 
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>                   
                            </td> 
                            <td>Rate:</td> 
                            <td>
                                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID = "txtRate" runat="server"></telerik:RadNumericTextBox>                     
                            </td> 
                        </tr>  
                        <tr>
                            <td>Recipient:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtRecipient" MaxLength="100" 
                                    Width="300" ReadOnly="false" Enabled="true">
                                </telerik:RadTextBox>                     
                            </td> 
                        </tr>      
                        <tr>
                            <td colspan="4">
                                <div id="divProc" runat="server" style="visibility:hidden">
                                </div>
                                <div id="divError" runat="server" style="visibility:hidden">
                                    <span id="spanError" class="error" runat="server"></span>
                                </div>      
                            </td>
                        </tr>   
                        <tr>
                            <td colspan="4"> 
                                <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="false" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true" EnableLinqExpressions="false" 
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnItemCreated="RadGrid1_ItemCreated"
        OnUpdateCommand="RadGrid1_UpdateCommand" OnDeleteCommand="RadGrid1_DeleteCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="pc_dtl_id" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px">
               <CommandItemSettings AddNewRecordText="Add Transaction" />
    <Columns>
        <telerik:GridBoundColumn DataField="pc_dtl_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="pc_dtl_id" ReadOnly="True" 
            SortExpression="pc_dtl_id" UniqueName="pc_dtl_id" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="ref_no" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Ref No" 
            SortExpression="ref_no" UniqueName="ref_no" ItemStyle-Width="40px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Aggregate="Count"
            FooterAggregateFormatString="{0:#,###}" FooterStyle-HorizontalAlign="Left"
            FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
            <ItemTemplate>
                <span><%# DataBinder.Eval(Container.DataItem, "ref_no")%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                    <asp:TextBox ID="txtref_no" runat="server" Text='<%# Bind("ref_no") %>'  
                        Width="25px" MaxLength="10">
                    </asp:TextBox>
                </span>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>   
        <telerik:GridTemplateColumn DataField="tx_date" DataType="System.DateTime" 
            DefaultInsertValue="" HeaderText="Date" 
            SortExpression="tx_date" UniqueName="tx_date" ItemStyle-Width="100px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# ((DateTime)DataBinder.Eval(Container.DataItem, "tx_date")).ToString("dd-MMM-yyyy")%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                    <telerik:RadDatePicker runat="server" ID="diTxDate" DateFormat="dd-MMM-yyyy"
                        SelectedDate='<%# Bind("tx_date") %>' Width="85px" DateInput-DateFormat="dd-MMM-yyyy">
                    </telerik:RadDatePicker>
                </span>
            </EditItemTemplate>
        </telerik:GridTemplateColumn> 
        <telerik:GridTemplateColumn DataField="acct_id" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Account" 
            SortExpression="acct_id" UniqueName="acct_id" ItemStyle-Width="200px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# AccountName(int.Parse(DataBinder.Eval(Container.DataItem, "pc_dtl_id").ToString()))%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                 <telerik:RadComboBox ID="cboGLAcc" runat="server" Height="150px" Width="300px" DataTextField="fullname" DataValueField="acct_id"
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
                </span>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>  
        <telerik:GridBoundColumn DataField="description" DefaultInsertValue="" 
            HeaderText="Description" SortExpression="description" UniqueName="description" ItemStyle-Width="250px"
            >
        </telerik:GridBoundColumn> 
        <telerik:GridTemplateColumn DataField="amount" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Amount" 
            SortExpression="amount" UniqueName="amount" ItemStyle-Width="150px"
            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" >
            <ItemTemplate>
                <span><%# ((double)DataBinder.Eval(Container.DataItem, "amount")).ToString("#,###.#0;(#,###.#0);0")%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtDbt" runat="server" 
                        Value='<%# Bind("amount") %>' EnabledStyle-HorizontalAlign="Right" 
                        Width="150px">
                    </telerik:RadNumericTextBox>
                </span>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>   
        <telerik:GridBoundColumn DataField="check_no" DefaultInsertValue="" 
            HeaderText="Check No." SortExpression="check_no" UniqueName="check_no" ItemStyle-Width="100px"
            >   
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="gl_ou_id" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Cost Center" 
            SortExpression="gl_ou_id" UniqueName="gl_ou_id" ItemStyle-Width="65px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# CostCenterName(int.Parse(DataBinder.Eval(Container.DataItem, "pc_dtl_id").ToString()))%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                 <telerik:RadComboBox ID="cboCC" runat="server" Height="200px" Width="65px"
                    DropDownWidth=" 155px" EmptyMessage="Select a Cost Center" HighlightTemplatedItems="true"
                     
                    EnableLoadOnDemand="true" AutoPostBack="true">
                    <HeaderTemplate>
                        <table style="width: 155px" cellspacing="0" cellpadding="0">
                            <tr> 
                                <td style="width: 150px;">
                                    Cost Center</td> 
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 155px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 150px;" >
                                    <%# DataBinder.Eval(Container, "Attributes['ou_name']")%>
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
            ConfirmText="Are you sure you want to delete the selected petty cash transaction detail?">
          </telerik:GridButtonColumn>
    </Columns>  
</MasterTableView>
       <ClientSettings>
           <ClientEvents />
           <Selecting AllowRowSelect="true" />
           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
           <Selecting AllowRowSelect="true" />
       </ClientSettings>
    </telerik:RadGrid> 
                                <telerik:RadGrid ID="RadGrid2" runat="server" DataSourceID="EntityDataSource2"
         GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="false" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true" EnableLinqExpressions="false"  
        OnItemDataBound="RadGrid2_ItemDataBound" OnDeleteCommand="RadGrid2_DeleteCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="pc_dtl_id" datasourceid="EntityDataSource2" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="960px">
               <CommandItemSettings AddNewRecordText="Add Transaction" />
    <Columns>
        <telerik:GridBoundColumn DataField="pc_dtl_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="pc_dtl_id" ReadOnly="True" 
            SortExpression="pc_dtl_id" UniqueName="pc_dtl_id" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="ref_no" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Ref No" 
            SortExpression="ref_no" UniqueName="ref_no" ItemStyle-Width="40px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Aggregate="Count"
            FooterAggregateFormatString="{0:#,###}" FooterStyle-HorizontalAlign="Left"
            FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
            <ItemTemplate>
                <span><%# DataBinder.Eval(Container.DataItem, "ref_no")%></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn>   
        <telerik:GridTemplateColumn DataField="tx_date" DataType="System.DateTime" 
            DefaultInsertValue="" HeaderText="Date" 
            SortExpression="tx_date" UniqueName="tx_date" ItemStyle-Width="100px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# ((DateTime)DataBinder.Eval(Container.DataItem, "tx_date")).ToString("dd-MMM-yyyy")%></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn> 
        <telerik:GridTemplateColumn DataField="acct_id" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Account" 
            SortExpression="acct_id" UniqueName="acct_id" ItemStyle-Width="200px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# AccountName(int.Parse(DataBinder.Eval(Container.DataItem, "pc_dtl_id").ToString()))%></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn>  
        <telerik:GridBoundColumn DataField="description" DefaultInsertValue="" 
            HeaderText="Description" SortExpression="description" UniqueName="description" ItemStyle-Width="250px"
            >
        </telerik:GridBoundColumn> 
        <telerik:GridTemplateColumn DataField="amount" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Amount" 
            SortExpression="amount" UniqueName="amount" ItemStyle-Width="150px"
            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" >
            <ItemTemplate>
                <span><%# ((double)DataBinder.Eval(Container.DataItem, "amount")).ToString("#,###.#0;(#,###.#0);0")%></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn>   
        <telerik:GridBoundColumn DataField="check_no" DefaultInsertValue="" 
            HeaderText="Check No." SortExpression="check_no" UniqueName="check_no" ItemStyle-Width="100px"
            >   
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="gl_ou_id" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Cost Center" 
            SortExpression="gl_ou_id" UniqueName="gl_ou_id" ItemStyle-Width="65px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span><%# CostCenterName(int.Parse(DataBinder.Eval(Container.DataItem, "pc_dtl_id").ToString()))%></span>
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
    </Columns>  
</MasterTableView>
       <ClientSettings>
           <ClientEvents />
           <Selecting AllowRowSelect="true" />
           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
           <Selecting AllowRowSelect="true" />
       </ClientSettings>
    </telerik:RadGrid>       
                            </td> 
                        </tr> 
                   </table>
                   
    <asp:ImageButton ID="btnExcel"  OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord" OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" />
    
              </asp:Panel>
              
                     
        </telerik:RadPane>
    </telerik:RadSplitter>         
     
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=coreGLEntities" DefaultContainerName="coreGLEntities" 
        EnableDelete="False" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="pc_dtl" Where="it.pc_head.pc_head_id=@pc_head_id">
        <WhereParameters>
            <asp:Parameter ConvertEmptyStringToNull="true" Name="pc_head_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>     
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreGLEntities" DefaultContainerName="coreGLEntities" 
        EnableDelete="False" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="pc_dtl" Where="it.pc_head.pc_head_id=@pc_head_id">
        <WhereParameters>
            <asp:Parameter ConvertEmptyStringToNull="true" Name="pc_head_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>    
    
     <script type="text/javascript">
         function SplitterLoadedInner(splitter, arg) {
             var pane = splitter.getPaneById('<%= RadPane1.ClientID %>');
             var pane2 = splitter.getPaneById('<%= contentPane.ClientID %>');
             var height = 0;
             if (pane != undefined && pane != null)
                 height = pane.getContentElement().scrollHeight;
             var height2 = 0;
             if (pane2 != undefined && pane2 != null)
                 height2 = pane2.getContentElement().scrollHeight;
             height = (height > height2) ? height : height2;
             if (height < screen.availHeight - 180) {
                 height = screen.availHeight - 180;
             } 
             splitter.set_height(height);
             pane.set_height(height);
         }
    </script> 
 
</asp:Content>
