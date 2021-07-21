<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="True" CodeBehind="glou.aspx.cs" Inherits="coreERP.common.glou" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Manage Cost Centers
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
                var result = confirm("Deleting the " + treeNode.get_text() + " " 
                + treeNode.get_parent().get_text()
                + " will remove cost center information"
                + "for all transactions with their OU set to it. Also all sub cost centers will be removed."
                + "\nDo you still want to continue?");
                args.set_cancel(!result);
                break;
        }
    }

    //this method disables the appropriate context menu items
    function setMenuItemsState(menuItems, treeNode) {
        for (var i = 0; i < menuItems.get_count(); i++) {
            var menuItem = menuItems.getItem(i);
            switch (menuItem.get_value()) {
                case "New":
                    //alert(treeNode.get_nodes().get_count());
                    if (treeNode.get_value().split(':')[0] != 'c') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                        formatMenuItem(menuItem, treeNode, 'Create new "{0}"');
                    }
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
                    if (treeNode.get_parent() == treeNode.get_treeView()) {
                        menuItem.set_enabled(false);
                    }
                    else if (treeNode.get_value().split(':')[0]=='c') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                    }
                    //formatMenuItem(menuItem, treeNode, 'Delete "{0}"');
                    break;  
            }
        }
    }

    //formats the Text of the menu item
    function formatMenuItem(menuItem, treeNode, formatString) {
        var nodeValue = treeNode.get_value();
        if (nodeValue && nodeValue.indexOf("_Private_") == 0) {
            menuItem.set_enabled(false);
        }
        else {
            menuItem.set_enabled(true);
        }
        var newText = String.format(formatString, extractTitleWithoutMails(treeNode));
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
        <h5>Define Cost Centers
         </h5> 
     
    <telerik:RadSplitter ID="RadSplitter1" runat="server">
        <telerik:RadPane ID="RadPane1" runat="server" Width="30%" >
                    
        <telerik:RadTreeView ID="RadTreeView1" runat="server" Height="100%" Width="300px" 
                AllowNodeEditing="false" EnableEmbeddedScripts="true"
                OnContextMenuItemClick="RadTreeView1_ContextMenuItemClick" OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                OnClientContextMenuShowing="onClientContextMenuShowing" OnNodeEdit="RadTreeView1_NodeEdit">
        <ContextMenus>
                    <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                        <Items>
                            <telerik:RadMenuItem Value="New" Text="Create Child Category" ImageUrl="~/images/new.jpg">
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
    
    <asp:ImageButton ID="btnExcel"  OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord" OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" />
    
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" />
        <telerik:RadPane ID="contentPane" runat="server">
            <asp:Panel runat="server" ID="pnlEdit" Visible="false">
                   <table>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>Cost Center Category:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCategoryName" MaxLength="200" Width="300" Enabled="false"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtCatID" Visible="false" Enabled="false"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtOrgID" Visible="false" Enabled="false"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtParentOrgID" Visible="false" Enabled="false"></asp:TextBox>
                                </td>
                        </tr>
                        <tr>
                            <td>Cost Center Name:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtUnitName" MaxLength="300" Width="200"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUnitName" runat="server" ControlToValidate="txtUnitName"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Orbanizational Unit's name.">
                                </asp:RequiredFieldValidator>                            
                            </td>                            
                        </tr>
                        <tr>
                            <td>Unit Manager:</td>
                            <td><telerik:RadComboBox runat="server" ID="cboUnitManager" Width="200"></telerik:RadComboBox></td>
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
 </asp:Content>
