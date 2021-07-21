<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="modules.aspx.cs" Inherits="coreERP.admin.modules" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Modules &amp; Authorizations
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

<script type="text/javascript">
    function toPlural(item) {
        var rtr = item;
        var itemString = item.toString();
        var lastDigit = itemString.substring(itemString.lengtd - 1, 1);
        var lastDigit2 = itemString.substring(itemString.lengtd - 2, 1);
        if (lastDigit == "s" || lastDigit2 == "es") { }
        else if (lastDigit == "y" || lastDigit == "i") {
            rtr = itemString.substring(0, itemString.lengtd - 1);
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
                var result = confirm("Deleting tde " + treeNode.get_text() + " "
                + treeNode.get_parent().get_text()
                + " will remove tde module and all its"
                + " sub-modules."
                + "\nDo you still want to continue?");
                args.set_cancel(!result);
                break;
        }
    }

    //tdis metdod disables tde appropriate context menu items
    function setMenuItemsState(menuItems, treeNode) {
        for (var i = 0; i < menuItems.get_count(); i++) {
            var menuItem = menuItems.getItem(i);
            switch (menuItem.get_value()) {
                case "New":  
                    formatMenuItem(menuItem, treeNode, 'Create new Module');
                    menuItem.set_enabled(false);
                    break;
                case "Edit":
                    if (treeNode.get_parent() == treeNode.get_treeView()) {
                        menuItem.set_enabled(false);
                    }
                    else if (treeNode.get_value().split(':')[0] == 'c') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                    }
                    //formatMenuItem(menuItem, treeNode, 'Edit "{0}"');
                    break;
                case "Delete":
                    menuItem.set_enabled(false);
                    //formatMenuItem(menuItem, treeNode, 'Delete "{0}"');
                    break;
            }
        }
    }

    //formats tde Text of tde menu item
    function formatMenuItem(menuItem, treeNode, formatString) {
        var nodeValue = treeNode.get_value();
        if (nodeValue && nodeValue.indexOf("_Private_") == 0) {
            menuItem.set_enabled(false);
        }
        else {
            menuItem.set_enabled(true);
        }
        var newText = String.format(formatString, extractTitleWitdoutMails(treeNode));
        menuItem.set_text(newText);
    }

    //checks if tde text contains (digit)
    function hasNodeMails(treeNode) {
        return treeNode.get_text().match(/\([\d]+\)/ig);
    }

    //removes tde brackets witd tde numbers,e.g. Inbox (30)
    function extractTitleWitdoutMails(treeNode) {
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
        <h5>Manage Security - Modules &amp; Authorizations
         </h5> 
            
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="600px" width="100%" OnClientLoaded="SplitterLoaded2">
        <telerik:RadPane ID="RadPane1" runat="server" width="320px" > 
        <h5>Modules
         </h5> 
    <telerik:RadTreeView ID="RadTreeView1" runat="server" Height="600px" width="300px" 
            AllowNodeEditing="false" EnableEmbeddedScripts="true" CausesValidation="false"
            OnContextMenuItemClick="RadTreeView1_ContextMenuItemClick" OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
            OnClientContextMenuShowing="onClientContextMenuShowing" 
            OnNodeClick="RadTreeView1_NodeClick">
    <ContextMenus>
                <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                    <Items>
                        <telerik:RadMenuItem Value="New" Text="Create New Module" ImageUrl="~/images/new.jpg">
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
            <asp:Panel runat="server" ID="pnlEdit" Visible="false">
                   <table>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>Parent Module:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtParentModuleName" MaxLength="200" width="300" Enabled="false"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtParentModuleID" Visible="false" Enabled="false"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtModuleID" Visible="false" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Module Name:</td>
                            <td>
                                <asp:TextBox runat="server" Enabled="false" ID="txtModuleName" MaxLength="100" width="200"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUnitName" runat="server" ControlToValidate="txtModuleName"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter tde module's name.">
                                </asp:RequiredFieldValidator>                            
                            </td>                            
                        </tr>
                        <tr>
                            <td>Module URL:</td>
                            <td>
                                <asp:TextBox runat="server" Enabled="false" ID="txtURL" MaxLength="250" width="200"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtURL"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter tde module's URL.">
                                </asp:RequiredFieldValidator>                            
                            </td>                            
                        </tr>
                        <tr>
                            <td>Module Code:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtModuleCode" MaxLength="10" width="100"></asp:TextBox>  
                            </td>                            
                        </tr>
                        <tr>
                            <td>Sort Value:</td>
                            <td>
                                <asp:TextBox runat="server" Enabled="false" ID="txtSortValue" MaxLength="5" width="50" Text="0"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSortValue"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter tde module's URL.">
                                </asp:RequiredFieldValidator>  
                            </td>                            
                        </tr> 
                        <tr>
                            <td>Visible?:</td>
                            <td>
                               <asp:CheckBox runat="server" ID="chkVisible" Enabled="false" /> 
                            </td>                            
                        </tr> 
                        <tr>
                            <td >Authorizations</td>
                            <td>
                                 <asp:Repeater runat="server" ID="rptRoles" DataSourceID="rolesDataSource"> 
                                    <HeaderTemplate> 
                                        <table>                                        
                                            <tr>
                                                <td style="width:150px">
                                                    Role Name
                                                </td>
                                                <td style="width:150px">
                                                     Is Allowed
                                                </td>
                                                <td style="width:150px">
                                                     Is Denied
                                                </td>
                                             </tr>
                                        </table>
                                     </HeaderTemplate>
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td style="width:150px">
                                                    <asp:Label ID="lblRole" runat="server" 
                                                    Text='<%# Bind("description") %>' Font-Bold="true"></asp:Label>
                                                    <asp:Label runat="server" Visible="false" ID="lblRoleName"
                                                        Text='<%# Bind("role_name") %>'></asp:Label>
                                                </td>
                                                <td style="width:150px">
                                                     <asp:CheckBox runat="server" ID="chkAllow"  />
                                                </td>
                                                <td style="width:150px">
                                                     <asp:CheckBox runat="server" ID="chkDeny"  />
                                                </td>
                                             </tr>
                                        </table>
                                    </ItemTemplate>
                                 </asp:Repeater>                          
                            </td>                             
                        </tr>  
                        <tr>                       
                        </tr> 
                        <tr>
                            <td>
                                <asp:Button ID="btnSaveModule" runat="server" Text="Save" onclick="btnSaveModule_Click" 
                                    OnClientClick="showProcessing()"/>
                                <asp:Button ID="btnCancelModule" runat="server" Text="Cancel" 
                                    CausesValidation="false" onclick="btnCancelModule_Click" 
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
    
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="currencies">
    </ef:EntityDataSource>
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
    <ef:EntityDataSource ID="permsDataSource" runat="server" 
        ConnectionString="name=coreSecurityEntities" DefaultContainerName="coreSecurityEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="perms">
    </ef:EntityDataSource>
     <script type="text/javascript">
         function SplitterLoaded2(splitter, arg) {
             var pane = splitter.getPaneById('<%= contentPane.ClientID %>');
             var height = pane.getContentElement().scrollHeight;
             splitter.set_height(screen.availHeight - 200);
             pane.set_height(height);
         }
    </script> 
 </asp:Content>
