<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="coreERP.admin.users" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Setup Users and Access Levels
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
                + " will remove OU information"
                + "for all transactions with their OU set to it. Also all sub units will be removed."
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
                    if (treeNode.get_value().split(':')[0] != '__root__') {
                        menuItem.set_enabled(false);
                    }
                    else {
                        menuItem.set_enabled(true);
                        formatMenuItem(menuItem, treeNode, 'Create new User');
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
        <h3>Setup Users and Access Levels
         </h3> 
    
        
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="600px" Width="100%">
        <telerik:RadPane ID="RadPane1" runat="server" Width="320" >
                    
        <telerik:RadTreeView ID="RadTreeView2" runat="server" Height="100%" Width="300px" 
                AllowNodeEditing="false"
                OnContextMenuItemClick="RadTreeView2_ContextMenuItemClick" OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                OnClientContextMenuShowing="onClientContextMenuShowing" >
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
            <asp:Panel runat="server" ID="pnlEditUser" Visible="false">
                   <table>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>Username:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtUserName" MaxLength="30" Width="150" Enabled="true"></asp:TextBox> 
                                <asp:TextBox runat="server" ID="txtOriginalUserName" MaxLength="30" Width="150" Visible="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUserName"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the username.">
                                </asp:RequiredFieldValidator>                            
                                </td>
                        </tr>
                        <tr>
                            <td>Fullname:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFullname" MaxLength="100" Width="200"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFullname"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the user's display name.">
                                </asp:RequiredFieldValidator>                            
                            </td>                            
                        </tr>
                        <tr>
                            <td>Is Active:</td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkActive" Text="" />                     
                            </td>                            
                        </tr>
                        <tr>
                            <td>Password:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPassword" MaxLength="100" Width="200" TextMode="Password"></asp:TextBox>
                                                     
                            </td>                            
                        </tr>
                        <tr>
                            <td>Access Level:</td>
                            <td>
                               <telerik:RadComboBox ID="cboAL" runat="server"></telerik:RadComboBox>
                            </td>                            
                        </tr>
                        <tr>
                            <td>Role Memberships:</td>
                            <td>
                                 <asp:CheckBoxList ID="clRoles" runat = "server" AppendDataBoundItems="false"
                                 DataSourceID="rolesDataSource" DataTextField="description" DataValueField="role_name"
                                 RepeatColumns="2"></asp:CheckBoxList>                          
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSaveUser" runat="server" Text="Save" onclick="btnSaveUser_Click" 
                                    OnClientClick="showProcessing()"/>
                                <asp:Button ID="btnCancelUser" runat="server" Text="Cancel" 
                                    CausesValidation="false" onclick="btnCancelUser_Click" 
                                    OnClientClick="showProcessing()"/>
                            </td>
                            <td></td>
                        </tr>
                   </table>
                    <telerik:RadGrid runat="server" ID="gridActivity" AllowSorting="true" AutoGenerateColumns="false">
                        <MasterTableView>
                            <Columns>
                                <telerik:GridDateTimeColumn HeaderText="Date Time" DataField="actionDateTime" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" SortExpression="actionDateTime"></telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn HeaderText="Full Name" DataField="user.full_name" SortExpression="user.full_name"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Module Name" DataField="module.module_name" SortExpression="module.module_name"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="URL" DataField="url" SortExpression="module.url"></telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
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
 </asp:Content>
