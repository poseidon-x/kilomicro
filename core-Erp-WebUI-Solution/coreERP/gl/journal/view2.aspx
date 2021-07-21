<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="view2.aspx.cs" Inherits="coreERP.gl.journal.view2" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>


<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
View Journal Entries
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
                    var msg = 'Create new Batch of Journal Entries'; 
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
        <h3>View G/L Journal Entries</h3>
          
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="1500px" Width="1448px">
        <telerik:RadPane ID="RadPane1" runat="server" Width="250px">
                    
        <telerik:RadTreeView ID="RadTreeView1" runat="server" Height="1400px" Width="240px" 
                AllowNodeEditing="false"  CausesValidation="false"
                OnNodeClick="RadTreeView1_NodeClick" EnableEmbeddedScripts="true" 
                OnContextMenuItemClick="RadTreeView1_ContextMenuItemClick" 
                OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                OnClientContextMenuShowing="onClientContextMenuShowing"
                CheckBoxes="true" OnNodeCheck="RadTreeView1_NodeCheck" > 
        <ContextMenus>
                    <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                        <Items> 
                            <telerik:RadMenuItem Value="Edit" Text="View Batch" Enabled="false" ImageUrl="~/images/edit.jpg"
                                >
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="Delete" Text="Delete Batch" ImageUrl="~/images/delete.jpg">
                            </telerik:RadMenuItem>
                        </Items>
                        <CollapseAnimation Type="none" />
                    </telerik:RadTreeViewContextMenu>                </ContextMenus> 
        </telerik:RadTreeView>
    
        </telerik:RadPane> 
        <telerik:RadPane ID="contentPane" runat="server" Width="100%"> 
            <asp:Panel runat="server" ID="pnlEditJournal" Visible="true">
                   <table>
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
                            <td colspan="2">
                                <span class="leftboldtxt" style="width:300px">Edit Journal Entry Transactions</span>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:100px">Start Transaction Date:</td>
                            <td>
                                <telerik:RadDatePicker ID="dtTransactionDate" runat="server" DateInput-DisplayDateFormat="dd-MMM-yyyy"
                                 ></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="dtTransactionDate"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the transaction date.">
                                </asp:RequiredFieldValidator>                     
                            </td> 
                            <td style="width:100px">End Transaction Date:</td>
                            <td>
                                <telerik:RadDatePicker ID="dtTransactionDate2" runat="server" DateInput-DisplayDateFormat="dd-MMM-yyyy"
                                 ></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dtTransactionDate2"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the transaction date.">
                                </asp:RequiredFieldValidator>                     
                            </td> 
                        </tr>  
                        <tr>
                            <td>Reference Number:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtRefNo" MaxLength="20" 
                                    Width="100">
                                </telerik:RadTextBox>                     
                            </td> 
                            <td>Batch Number:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtBatchNumber" MaxLength="20" 
                                    Width="100" >
                                </telerik:RadTextBox>                     
                            </td> 
                        </tr>  
                       <tr>
                           <td></td>
                            <td><telerik:RadButton ID="btnView" Text="View Journals" runat="server" OnClick="btnView_Click"></telerik:RadButton></td>
                       </tr>
                        <tr>
                            <td colspan="4"> 
                                <telerik:RadGrid ID="RadGrid1" runat="server"  
        GridLines="Both" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" AllowAutomaticUpdates="false" 
        AllowSorting="true" ShowFooter="true" EnableLinqExpressions="false" AutoGenerateColumns="false"
        OnDetailTableDataBind="RadGrid1_DetailTableDataBind" 
        OnItemDataBound="RadGrid1_ItemDataBound"  >
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"          
               CommandItemDisplay="None" Width="860px" DataKeyNames="jnl_batch_id">  
            <DetailTables><telerik:GridTableView DataMember="jnls">
                <Columns>
                    <telerik:GridBoundColumn DataField="jnl_id" DataType="System.Int32" 
                        DefaultInsertValue="" HeaderText="jnl_id" ReadOnly="True" 
                        SortExpression="jnl_id" UniqueName="jnl_id" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="ref_no" DataType="System.Double" 
                        DefaultInsertValue="" HeaderText="Ref No" 
                        SortExpression="ref_no" UniqueName="ref_no" ItemStyle-Width="50px"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Aggregate="Count"
                        FooterAggregateFormatString="{0:#,###}" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
                        <ItemTemplate>
                            <span><%# DataBinder.Eval(Container.DataItem, "ref_no")%></span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span>
                                <asp:TextBox ID="txtref_no" runat="server" Text='<%# Bind("ref_no") %>'  
                                    Width="50px">
                                </asp:TextBox>
                            </span>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>  
                    <telerik:GridTemplateColumn DataField="acct_id" DataType="System.Double" 
                        DefaultInsertValue="" HeaderText="Account" 
                        SortExpression="acct_id" UniqueName="acct_id" ItemStyle-Width="200px"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <span><%# AccountName(int.Parse(DataBinder.Eval(Container.DataItem, "jnl_id").ToString()))%></span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span>
                             <telerik:RadComboBox ID="cboGLAcc" runat="server" Height="150px" Width=" 255px"
                                DropDownWidth=" 255px" EmptyMessage="Select a GL Account" HighlightTemplatedItems="true"
                                MarkFirstMatch="true" AutoCompleteSeparator=""    
                                EnableLoadOnDemand="true" AutoPostBack="true">
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
                            </span>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="description" DefaultInsertValue="" 
                        HeaderText="Description" SortExpression="description" UniqueName="description" ItemStyle-Width="220px">
                    </telerik:GridBoundColumn> 
                    <telerik:GridTemplateColumn DataField="dbt_amt" DataType="System.Double" 
                        DefaultInsertValue="" HeaderText="Debit" 
                        SortExpression="dbt_amt" UniqueName="dbt_amt" ItemStyle-Width="80px"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" Aggregate="Sum"
                        FooterAggregateFormatString="{0:#,###.#0}" FooterStyle-HorizontalAlign="Right"
                        FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
                        <ItemTemplate>
                            <span><%# ((double)DataBinder.Eval(Container.DataItem, "dbt_amt")).ToString("#,###.#0;(#,###.#0);0")%></span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span>
                                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtDbt" runat="server" 
                                    Value='<%# Bind("dbt_amt") %>' EnabledStyle-HorizontalAlign="Right" 
                                    Width="80px">
                                </telerik:RadNumericTextBox>
                            </span>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn> 
                    <telerik:GridTemplateColumn DataField="crdt_amt" DataType="System.Double" 
                        DefaultInsertValue="" HeaderText="Credit" 
                        SortExpression="crdt_amt" UniqueName="crdt_amt" ItemStyle-Width="80px"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" Aggregate="Sum"
                        FooterAggregateFormatString="{0:#,###.#0}" FooterStyle-HorizontalAlign="Right"
                        FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
                        <ItemTemplate>
                            <span><%# ((double)DataBinder.Eval(Container.DataItem, "crdt_amt")).ToString("#,###.#0;(#,###.#0);0")%></span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span>
                                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtCrdt" runat="server" 
                                    Value='<%# Bind("crdt_amt") %>' EnabledStyle-HorizontalAlign="Right"
                                    Width="80px">
                                </telerik:RadNumericTextBox>
                            </span>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="gl_ou_id" DataType="System.Double" 
                        DefaultInsertValue="" HeaderText="Cost Center" 
                        SortExpression="gl_ou_id" UniqueName="gl_ou_id" ItemStyle-Width="100px"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <span><%# CostCenterName(int.Parse(DataBinder.Eval(Container.DataItem, "jnl_id").ToString()))%></span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span>
                             <telerik:RadComboBox ID="cboCC" runat="server" Height="200px" Width=" 125px"
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
                </Columns>  
          </telerik:GridTableView>
    </DetailTables>
    <Columns>
        <telerik:GridBoundColumn DataField="batch_no" UniqueName="batch_no" HeaderText="Batch No"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="creator" UniqueName="creator" HeaderText="Batch Originator"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="source" UniqueName="source" HeaderText="Batch Source"></telerik:GridBoundColumn>
        <telerik:GridCheckBoxColumn DataField="posted" UniqueName="posted" HeaderText="Posted" ></telerik:GridCheckBoxColumn>
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
              
              
            <div id="divProc" runat="server" style="visibility:hidden">
                <asp:Image ID="Image1" ImageUrl="~/images/animated/processing.gif" runat="server" />
            </div>
            <div id="divError" runat="server" style="visibility:hidden">
                <asp:Image ID="Image2" ImageUrl="~/images/errors/error.jpg" runat="server" />
                <span id="spanError" class="error" runat="server"></span>
            </div>            
        </telerik:RadPane>
    </telerik:RadSplitter>         
     
</asp:Content>
