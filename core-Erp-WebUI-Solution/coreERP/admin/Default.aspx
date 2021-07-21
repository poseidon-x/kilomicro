<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.admin._default" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Security
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

<script type="text/javascript">
    function toPlural(item) {
        var rtr = item;
        var itemString = item.toString();
        var lastDigit = itemString.substring(itemString.length - 1, 1);
        var lastDigit2 = itemString.substring(itemString.length - 2, 1);
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
    //<!--
    function onClientContextMenuShowing(sender, args) {
        var treeNode = args.get_node();
        treeNode.set_selected(true);
        //enable/disable menu items
        setMenuItemsState(args.get_menu().get_items(), treeNode);
    }

    function onClientContextMenuShowing2(sender, args) {
        var treeNode = args.get_node();
        treeNode.set_selected(true);
        //enable/disable menu items
        setMenuItemsState2(args.get_menu().get_items(), treeNode);
    }

    function onClientContextMenuItemClicking(sender, args) {
        var menuItem = args.get_menuItem();
        var treeNode = args.get_node();
        menuItem.get_menu().hide();

        switch (menuItem.get_value()) {
            case "New":
                showProcessing();
                break;
            case "Edit":
                showProcessing();
                break;
            case "Delete":
                var result = confirm("Deleting the role " + treeNode.get_text()
                + " will also remove permissions assigned."
                + "\nDo you still want to continue?");
                args.set_cancel(!result);
                break;
        }
    }

    function onClientContextMenuItemClicking2(sender, args) {
        var menuItem = args.get_menuItem();
        var treeNode = args.get_node();
        menuItem.get_menu().hide();

        switch (menuItem.get_value()) {
            case "New":
                showProcessing();
                break;
            case "Edit":
                showProcessing();
                break;
            case "Delete":
                var result = confirm("Deleting the user " + treeNode.get_text()
                + " will also remove permissions assigned."
                + "\nDo you still want to continue?");
                args.set_cancel(!result);
                break;
        }
    }

    //this method disables the appropriate context menu items
    function setMenuItemsState(menuItems, treeNode) {
        for (var i = 0; i < menuItems.get_count() ; i++) {
            var menuItem = menuItems.getItem(i);
            formatMenuItem(menuItem, treeNode, 'Create new "{0}"');
        }
    }

    //this method disables the appropriate context menu items
    function setMenuItemsState2(menuItems, treeNode) {
        for (var i = 0; i < menuItems.get_count() ; i++) {
            var menuItem = menuItems.getItem(i);
            formatMenuItem2(menuItem, treeNode, 'Create new "{0}"');
        }
    }

    //formats the Text of the menu item
    function formatMenuItem(menuItem, treeNode, formatString) {
        var nodeValue = treeNode.get_value();
        var newText = '';
        var splitted = nodeValue.split(':');
        var key = splitted[0];
        var value = (splitted.length == 2) ? splitted[1] : '';
        if (menuItem.get_value() == 'New') {
            if (key == '__root__') {
                newText = 'Create New Role';
                menuItem.set_enabled(true);
            }
            else {
                menuItem.set_enabled(false);
            }
        }
        if (menuItem.get_value() == 'Edit') {
            if (key == 'r') {
                newText = 'Edit Role: ' + value;
                menuItem.set_enabled(true);
            }
            else {
                menuItem.set_enabled(false);
            }
        }
        if (menuItem.get_value() == 'Delete') {
            if (key == 'r') {
                newText = 'Delete Role: ' + value;
                menuItem.set_enabled(true);
            }
            else {
                menuItem.set_enabled(false);
            }
        }
        menuItem.set_text(newText);
    }

    //formats the Text of the menu item
    function formatMenuItem2(menuItem, treeNode, formatString) {
        var nodeValue = treeNode.get_value();
        var newText = '';
        var splitted = nodeValue.split(':');
        var key = splitted[0];
        var value = (splitted.length == 2) ? splitted[1] : '';
        if (menuItem.get_value() == 'New') {
            if (key == '__root__') {
                newText = 'Create New Role';
                menuItem.set_enabled(true);
            }
            else {
                menuItem.set_enabled(false);
            }
        }
        if (menuItem.get_value() == 'Edit') {
            if (key == 'u') {
                newText = 'Edit Role: ' + value;
                menuItem.set_enabled(true);
            }
            else {
                menuItem.set_enabled(false);
            }
        }
        if (menuItem.get_value() == 'Delete') {
            if (key == 'u') {
                newText = 'Delete Role: ' + value;
                menuItem.set_enabled(true);
            }
            else {
                menuItem.set_enabled(false);
            }
        }
        menuItem.set_text(newText);
    }

    //checks if the text contains (digit)
    function hasNodeMails(treeNode) {
        return treeNode.get_text().match(/\([\d]+\)/ig);
    }

    //removes the brackets with the numbers,e.g. Inbox (30)
    function extractTitleWithoutMails(treeNode) {
        return treeNode.get_text().replace(/\s*\([\d]+\)\s*/ig, "");
    }
    //-->

    function showProcessing() {
        var divEl = document.getElementById('<%= divProc.ClientID %>');
        if (divEl != null) divEl.style["visibility"] = "visible";

        return true;
    }
        </script>

</telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h5>Manage Security
         </h5> 
    
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="600px" Width="100%">
        <telerik:RadPane ID="RadPane1" runat="server" Width="320px" > 
        <h5>Roles
         </h5> 
    <telerik:RadTreeView ID="RadTreeView2" runat="server" Height="600px" Width="300px"  
            AllowNodeEditing="false" EnableEmbeddedScripts="true" CausesValidation="false"
            OnContextMenuItemClick="RadTreeView2_ContextMenuItemClick" 
            OnClientContextMenuShowing="onClientContextMenuShowing2">
    <ContextMenus>
                <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu1" runat="server">
                    <Items>
                        <telerik:RadMenuItem Value="New" Text="Create New User" ImageUrl="~/images/new.jpg">
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="Edit" Text="Edit" Enabled="false" ImageUrl="~/images/edit.jpg"
                            >
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="Delete" Text="Delete" ImageUrl="~/images/delete.jpg">
                        </telerik:RadMenuItem>
                    </Items>
                    <CollapseAnimation Type="none" />
                </telerik:RadTreeViewContextMenu>
                <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu2" runat="server">
                    <Items>
                        <telerik:RadMenuItem Value="New" Text="Create New User" ImageUrl="~/images/new.jpg">
                        </telerik:RadMenuItem>
                    </Items>
                    <CollapseAnimation Type="none" />
                </telerik:RadTreeViewContextMenu>
            </ContextMenus>
    </telerik:RadTreeView>
 
    </telerik:RadPane>    
    <telerik:RadSplitBar ID="RadSplitBar1" runat="server" /> 
        <telerik:RadPane ID="contentPane" runat="server">
            <asp:Panel runat="server" ID="pnlEditRole" Visible="false">
                   <table>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>Role Name:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRolename" MaxLength="30" Width="150" Enabled="true" OnTextChanged="txtRolename_TextChanged"></asp:TextBox> 
                                <asp:TextBox runat="server" ID="txtOriginalRolename" MaxLength="30" Width="150" Visible="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtRolename"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the role name.">
                                </asp:RequiredFieldValidator>                            
                                </td>
                        </tr>
                        <tr>
                            <td>Fullname:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRoleDescription" MaxLength="100" Width="200"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRoleDescription"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the user's display name.">
                                </asp:RequiredFieldValidator>                            
                            </td>                            
                        </tr> 
                        <tr>
                            <td>Role Members:</td>
                            <td>
                                 <asp:CheckBoxList ID="clMembers" runat = "server" AppendDataBoundItems="false"
                                 DataSourceID="usersDataSource" DataTextField="full_name" DataValueField="user_name"
                                 RepeatColumns="1"></asp:CheckBoxList>                          
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSaveRole_Click" 
                                    OnClientClick="showProcessing()"/>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                                    CausesValidation="false" onclick="btnCancelRole_Click" 
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
        </telerik:RadPane> 
    </telerik:RadSplitter>         
     
    <ef:EntityDataSource ID="rolesDataSource" runat="server" 
        ConnectionString="name=coreSecurityEntities" DefaultContainerName="coreSecurityEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="roles">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="usersDataSource" runat="server" 
        ConnectionString="name=coreSecurityEntities" DefaultContainerName="coreSecurityEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="users">
    </ef:EntityDataSource>
 </asp:Content>
