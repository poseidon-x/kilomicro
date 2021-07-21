<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.gl.accounts._default" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>


<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Setup Chart of Accounts
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
        //alert('Haha');
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
                var msg =  "Deleting the " + treeNode.get_text() + " " 
                    + treeNode.get_parent().get_text()
                    + " will remove OU information"
                    + "for all transactions with their OU set to it. Also all sub units will be removed."
                    + "\nDo you still want to continue?";
                if (itemType == "c"){
                    msg = "Deleting the " + treeNode.get_text() + " account head "
                    + "will remove all its child accounts and their corresponding transactions"
                    + ".\nDo you still want to continue?";
                }
                else if (itemType == "h") {
                    msg = "Deleting the " + treeNode.get_text() + " account sub head "
                    + "will remove all its child accounts and their corresponding transactions"
                    + ".\nDo you still want to continue?";
                }
                else if (itemType == "a") {
                    msg = "Deleting the " + treeNode.get_text() + " account "
                    + "will remove all its child  transactions"
                    + ".\nDo you still want to continue?";
                }
                var result = confirm(msg);
                args.set_cancel(!result);
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
                    if (nodeType == '__root__') {
                        menuItem.set_enabled(false);
                    }
                    if (nodeType == 'a') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true); 
                        var msg = 'Create new "{0}"'
                        if (nodeType == '__root__') {
                            menuItem.set_enabled(false);
                        }
                        else if (nodeType == 'c' || nodeType == 'h') {
                            msg = 'Create Sub Account Head';
                        } 
                        formatMenuItem(menuItem, treeNode, msg);
                    }
                    break;
                case "NewAccount":
                    //alert(treeNode.get_nodes().get_count());
                    if (nodeType != 'h') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                        var msg = 'Create New Account' 
                        formatMenuItem(menuItem, treeNode, msg);
                    }
                    break;
                case "Edit": 
                    if (nodeType == '__root__') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                    } 
                    break;
                case "Delete":
                    if (nodeType == '__root__' || nodeType == 'c') {
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
        var combo = document.getElementById("<%= cboCur.ClientID %>");
        combo.clearSelection();
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
        <h3>Define Organizational Structure
         </h3> 
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="100%" Width="95%" OnClientLoaded="SplitterLoadedInner">
        <telerik:RadPane ID="RadPane1" runat="server" Width="450px">
                    
        <telerik:RadTreeView ID="RadTreeView1" runat="server" Height="100%" Width="445px" 
                AllowNodeEditing="false"
                OnContextMenuItemClick="RadTreeView1_ContextMenuItemClick" OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                OnClientContextMenuShowing="onClientContextMenuShowing" OnNodeEdit="RadTreeView1_NodeEdit"
                EnableDragAndDrop="True" OnNodeDrop="RadTreeView1_HandleDrop"
                        OnClientNodeDropping="onNodeDropping" OnClientNodeDragging="onNodeDragging" 
                        MultipleSelect="true" EnableDragAndDropBetweenNodes="true">
        <ContextMenus>
                    <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                        <Items>
                            <telerik:RadMenuItem Value="New" Text="Create Child Category" ImageUrl="~/images/new.jpg">
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="NewAccount" Text="Create Account" ImageUrl="~/images/new.jpg" Enabled="false">
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="Edit" Text="Edit" Enabled="false" ImageUrl="~/images/edit.jpg"
                                >
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="Delete" Text="Delete" ImageUrl="~/images/delete.jpg">
                            </telerik:RadMenuItem>
                        </Items>
                        <CollapseAnimation Type="none" />
                    </telerik:RadTreeViewContextMenu>
                    <telerik:RadTreeViewContextMenu ID="EmptyFolderContextMenu" runat="server">
                        <Items>
                            <telerik:RadMenuItem Value="New" Text="Create Child Category" ImageUrl="~/images/new.jpg">
                            </telerik:RadMenuItem>
                        </Items>
                        <CollapseAnimation Type="none" />
                    </telerik:RadTreeViewContextMenu>
                </ContextMenus>
        </telerik:RadTreeView>
    
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" />
        <telerik:RadPane ID="contentPane" runat="server">
            <asp:Panel runat="server" ID="pnlEditCat" Visible="false">
                   <table>
                        <tr>
                            <td colspan="2">
                                <span class="leftboldtxt">Edit Account Main Head</span>
                            </td>
                        </tr> 
                        <tr>
                            <td>Account Main Head:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtCatName" MaxLength="100" Width="300"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rfvUnitName" runat="server" ControlToValidate="txtCatName"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Account main head name.">
                                </asp:RequiredFieldValidator> 
                                <asp:TextBox runat="server" ID="txtCatID" Visible="false" Enabled="false"></asp:TextBox>                   
                            </td>                            
                        </tr>  
                        <tr>
                            <td>Account Main Code:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" ReadOnly="true" Enabled="false" runat="server" ID="txtCode" MaxLength="1" Width="50"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Account main head code.">
                                </asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rvCode" runat="server" ControlToValidate="txtCode"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="Account main head code must be a number between 1 and 8." 
                                     MaximumValue="8" MinimumValue="1">
                                </asp:RangeValidator>                           
                            </td>                            
                        </tr>   
                        <tr>
                            <td>Min. Account Number:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtMin" MaxLength="20" Width="100"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMin"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the minimum account number.">
                                </asp:RequiredFieldValidator>                    
                            </td>   
                        </tr>  
                        <tr>
                            <td>Max. Account Number:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtMax" MaxLength="20" Width="100"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMax"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the maximum account number.">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvMaxMin" ControlToValidate="txtMax" ControlToCompare="txtMin"
                                  runat="server" Operator="GreaterThan"
                                  ErrorMessage="!" Font-Bold="true" ToolTip="Minimum account number must be less than maximu account number.">
                                </asp:CompareValidator>                       
                            </td> 
                        </tr>    
                        <tr>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" 
                                    OnClientClick="showProcessing()"/>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                                    CausesValidation="false" onclick="btnCancel_Click" 
                                    OnClientClick="showProcessing()"/>
                            </td>
                            <td></td>
                        </tr>
                   </table>
              </asp:Panel>  
            <asp:Panel runat="server" ID="pnlEditHead" Visible="false">
                   <table>
                        <tr>
                            <td colspan="2">
                                <span class="leftboldtxt">Edit Account Sub Head</span>
                            </td>
                        </tr> 
                        <tr>
                            <td>Parent Account Head:</td>
                            <td>
                                <asp:Label runat="server" ID="lblPath"></asp:Label> 
                                <asp:TextBox runat="server" ID="txtHeadID" Visible="false" Enabled="false"></asp:TextBox>  
                                <asp:TextBox runat="server" ID="txtParentHeadID" Visible="false" Enabled="false"></asp:TextBox>                 
                            </td>                            
                        </tr> 
                        <tr>
                            <td>Account Sub Head:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtHeadName" MaxLength="100" Width="300"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtHeadName"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Account sub head name.">
                                </asp:RequiredFieldValidator>                     
                            </td> 
                        </tr> 
                        <tr>
                            <td>Min. Account Number:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtHeadMin" MaxLength="20" Width="100"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtHeadMin"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the minimum account number.">
                                </asp:RequiredFieldValidator>                    
                            </td>   
                        </tr>  
                        <tr>
                            <td>Max. Account Number:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtHeadMax" MaxLength="20" Width="100"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtHeadMax"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the maximum account number.">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtHeadMax" ControlToCompare="txtHeadMin"
                                  runat="server" Operator="GreaterThan"
                                  ErrorMessage="!" Font-Bold="true" ToolTip="Minimum account number must be less than maximu account number.">
                                </asp:CompareValidator>                       
                            </td> 
                         </tr>    
                        <tr>
                            <td>
                                <asp:Button ID="btnSaveHead" runat="server" Text="Save" onclick="btnSaveHead_Click" 
                                    OnClientClick="showProcessing()"/>
                                <asp:Button ID="btnCancelHead" runat="server" Text="Cancel" 
                                    CausesValidation="false" onclick="btnCancelHead_Click" 
                                    OnClientClick="showProcessing()"/>
                            </td>
                            <td></td>
                        </tr>
                   </table>
              </asp:Panel>  
              
              
            <asp:Panel runat="server" ID="pnlEditAccount" Visible="false">
                   <table>
                        <tr>
                            <td colspan="2">
                                <span class="leftboldtxt">Edit Account</span>
                            </td>
                        </tr> 
                        <tr>
                            <td>Parent Account Head:</td>
                            <td>
                                <asp:Label runat="server" ID="lblAccountHead"></asp:Label> 
                                <asp:TextBox runat="server" ID="txtAccID" Visible="false" Enabled="false"></asp:TextBox>               
                            </td>                            
                        </tr> 
                        <tr>
                            <td>Account Number:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtAccNum" MaxLength="20" Width="100"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAccNum"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Account Number.">
                                </asp:RequiredFieldValidator>                     
                            </td> 
                        </tr> 
                        <tr>
                            <td>Account Name:</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtAccName" MaxLength="250" Width="350"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtAccName"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Account Name.">
                                </asp:RequiredFieldValidator>                     
                            </td> 
                        </tr>  
                        <tr>
                            <td>Account Currency:</td>
                            <td>
                                <telerik:RadComboBox ID="cboCur" runat="server" Height="200px" Width="200px"
                            DropDownWidth="298px" EmptyMessage="Select a Currency" HighlightTemplatedItems="true"
                            MarkFirstMatch="true" Filter="Contains">
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="cboCur"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Account Name."></asp:RequiredFieldValidator></td> 
                        </tr> 
                        <tr>
                            <td>Is Account Active:</td>
                            <td>
                                <asp:CheckBox ID="ckAccActive" runat="server" Text="" Checked="true" />                  
                            </td> 
                        </tr>  
                        <tr>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="Save" onclick="btnSaveAcc_Click" 
                                    OnClientClick="showProcessing()"/>
                                <asp:Button ID="Button2" runat="server" Text="Cancel" 
                                    CausesValidation="false" onclick="btnCancelAcc_Click" 
                                    OnClientClick="showProcessing()"/>
                            </td>
                            <td></td>
                        </tr>
                   </table>
              </asp:Panel>
              
              
            <div id="divProc" runat="server" style="visibility:hidden">
                <asp:Image ID="Image1" ImageUrl="~/images/animated/processing.gif" runat="server" />
            </div>
            <div id="divError" runat="server" style="visibility:hidden">
                <asp:Image ID="Image2" ImageUrl="~/images/errors/error.jpg" runat="server" />
                <span id="spanError" class="error" runat="server"></span>
            </div>   

    <asp:ImageButton ID="btnExcel"  OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord" OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" />
             
        </telerik:RadPane>
    </telerik:RadSplitter>         
    
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="currencies">
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
