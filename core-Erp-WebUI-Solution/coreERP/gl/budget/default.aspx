<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.gl.budget._default" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>


<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Plan Budget
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
                break;
            case "NewAccount":  
                break;
            case "Edit":  
                break;
            case "Delete":
                var itemType = treeNode.get_value().split(':')[0];
                var msg = "Deleting the " + treeNode.get_text() + " "
                    + treeNode.get_parent().get_text()
                    + " will remove OU information"
                    + "for all transactions with their OU set to it. Also all sub units will be removed."
                    + "\nDo you still want to continue?";
                if (itemType == "c") {
                    msg = "Deleting the " + treeNode.get_text() + " cost center "
                    + "will remove all its corresponding budget items"
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
                    var msg = 'Enter Budget for New Cost Center';
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
                    if (nodeType != 'c') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                    } 
                    break;
                case "Delete":
                    if (nodeType != 'c') {
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
    <style type="text/css">
        .wrap {    
            overflow:hidden;
            width:100%;
        }
        
        .wrapLeft {
            width:48%;
            float:left;
            margin:1px;
        }
        
        .wrapRight {
            width:48%;
            float:left;
            margin:1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Budget and Financial Planning</h3>     
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="95%" OnClientLoaded="SplitterLoadedInner" >
        <telerik:RadPane ID="RadPane1" runat="server" Width="350">
                    
        <telerik:RadTreeView ID="RadTreeView1" runat="server" 
                AllowNodeEditing="false"  CausesValidation="false" OnNodeExpand="RadTreeView1_NodeExpand"
                OnNodeClick="RadTreeView1_NodeClick" EnableEmbeddedScripts="true"
                OnContextMenuItemClick="RadTreeView1_ContextMenuItemClick" 
                OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                OnClientContextMenuShowing="onClientContextMenuShowing" > 
                <ContextMenus>
                    <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                        <Items>
                            <telerik:RadMenuItem Value="New" Text="Enter Budget for New Cost Center" ImageUrl="~/images/new.jpg">
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="Edit" Text="Edit Budget for Cost Center" Enabled="false" ImageUrl="~/images/edit.jpg"
                                >
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="Delete" Text="Delete All Budget for this Cost Center" ImageUrl="~/images/delete.jpg">
                            </telerik:RadMenuItem>
                        </Items>
                        <CollapseAnimation Type="none" />
                    </telerik:RadTreeViewContextMenu>
                    <telerik:RadTreeViewContextMenu ID="EmptyFolderContextMenu" runat="server">
                        <Items>
                            <telerik:RadMenuItem Value="New" Text="Enter Budget for New Cost Center" ImageUrl="~/images/new.jpg">
                            </telerik:RadMenuItem>
                        </Items>
                        <CollapseAnimation Type="none" />
                    </telerik:RadTreeViewContextMenu>
                </ContextMenus> 
        </telerik:RadTreeView>
    
        </telerik:RadPane> 
        <telerik:RadPane ID="contentPane" runat="server" >
              <asp:Panel runat="server" ID="plnEdit" Visible="false"> 
                  <div>
                    <div class="subFormColumnLeft">
                        <div class="subFormLabel">
                            Month End Date:
                        </div>
                        <div class="subFormInput">
                            <telerik:RadDatePicker runat="server" ID="dtpDate" CssClass="inputControl"  DateInput-DateFormat="dd-MMM-yyyy"
                                AutoPostBack="true" OnSelectedDateChanged="dtpDate_SelectedDateChanged"></telerik:RadDatePicker>
                        </div>
                        <div class="subFormLabel">
                            Cost Center:
                        </div>
                        <div class="subFormInput">
                            <telerik:RadComboBox runat="server" ID="cboCC" CssClass="inputControl" AutoPostBack="true"
                                OnSelectedIndexChanged="cboCC_SelectedIndexChanged"></telerik:RadComboBox>
                        </div>
                        <div class="subFormLabel">
                            <telerik:RadButton runat="server" ID="btnSave" Text="Save Budget"
                                OnClick="btnSave_Click"></telerik:RadButton>
                        </div>
                    </div>
                  </div> 
                  <br />
                  <div class="wrap">
                       <div class="wrapLeft">
                          <h4>Budget for Balance Accounts</h4>
                          <asp:Repeater runat="server" ID="rpBudgetBS">
                              <HeaderTemplate>
                                  <table>
                                      <tr>
                                          <td style="width:100px;border-bottom:solid 1px #0094ff;">
                                              Acct #
                                          </td>
                                          <td style="width:280px;border-bottom:solid 1px #0094ff;">
                                              Account Name
                                          </td>
                                          <td style="width:120px;border-bottom:solid 1px #0094ff;text-align:right">
                                              Budgeted Balance (EOM)
                                          </td>
                                      </tr>
                                  </table>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <table>
                                      <tr>
                                          <td style="width:100px;border-bottom:solid 1px #0094ff;">
                                              <asp:Label runat="server" ID="lblAcctNum" Text='<%# GetAccountNumber(Eval("acct_id")) %>'></asp:Label>
                                              <asp:Label runat="server" ID="lblAcctID" Text='<%# Bind("acct_id") %>' Visible="false"></asp:Label>
                                          </td>
                                          <td style="width:280px;border-bottom:solid 1px #0094ff;">
                                              <asp:Label runat="server" ID="lblAcctName" Text='<%# GetAccountName(Eval("acct_id")) %>'></asp:Label>
                                          </td>
                                          <td style="width:120px;border-bottom:solid 1px #0094ff;text-align:right">
                                              <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server"
                                                   Value='<%# Eval("budgetAmount") %>' Width="100"
                                                   Text='<%# Eval("budgetAmount") %>' ></telerik:RadNumericTextBox>
                                          </td>
                                      </tr>
                                  </table>
                              </ItemTemplate>
                          </asp:Repeater>
                        </div>
                       <div class="wrapRight">
                           <h4>Budget for Income Statement Accounts</h4>
                          <asp:Repeater runat="server" ID="rpBudgetIS">
                              <HeaderTemplate>
                                  <table>
                                      <tr>
                                          <td style="width:100px;border-bottom:solid 1px #0094ff;">
                                              Acct #
                                          </td>
                                          <td style="width:280px;border-bottom:solid 1px #0094ff;">
                                              Account Name
                                          </td>
                                          <td style="width:120px;border-bottom:solid 1px #0094ff;text-align:right">
                                              Budgeted Amount (Monthly)
                                          </td>
                                      </tr>
                                  </table>
                              </HeaderTemplate>
                              <ItemTemplate>
                                  <table>
                                      <tr>
                                          <td style="width:100px;border-bottom:solid 1px #0094ff;">
                                              <asp:Label runat="server" ID="lblAcctNum" Text='<%# GetAccountNumber(Eval("acct_id")) %>'></asp:Label>
                                              <asp:Label runat="server" ID="lblAcctID" Text='<%# Bind("acct_id") %>' Visible="false"></asp:Label>
                                          </td>
                                          <td style="width:280px;border-bottom:solid 1px #0094ff;">
                                              <asp:Label runat="server" ID="lblAcctName" Text='<%# GetAccountName(Eval("acct_id")) %>'></asp:Label>
                                          </td>
                                          <td style="width:120px;border-bottom:solid 1px #0094ff;text-align:right">
                                              <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server"
                                                   Value='<%# Eval("budgetAmount") %>' Width="100" 
                                                  Text='<%# Eval("budgetAmount") %>'></telerik:RadNumericTextBox>
                                          </td>
                                      </tr>
                                  </table>
                              </ItemTemplate>
                          </asp:Repeater>
                        </div>
                    </div>
              </asp:Panel>
        </telerik:RadPane>
    </telerik:RadSplitter>         
     
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
