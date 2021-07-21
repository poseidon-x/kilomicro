<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="payCalendar.aspx.cs" Inherits="coreERP.hc.payroll.payCalendar" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Define Organizational Structure
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

<script type="text/javascript">
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
                break;
            case "Edit":
                treeNode.startEdit();
                break;
            case "Delete":
                var result = confirm("Deleting this calendar month may cause loss of data.\nDo you still want to continue?");
                args.set_cancel(!result);
                break;
        }
    }

    function showProcessing() {
        var divEl = document.getElementById('<%= divProc.ClientID %>');
        if (divEl != null) divEl.style["visibility"] = "visible";

        return true;
    }
    //this method disables the appropriate context menu items
    function setMenuItemsState(menuItems, treeNode) {
        for (var i = 0; i < menuItems.get_count(); i++) {
            var menuItem = menuItems.getItem(i);
            switch (menuItem.get_value()) {
                case "New":
                    //alert(treeNode.get_nodes().get_count()); 
                        menuItem.set_enabled(true);
                        formatMenuItem(menuItem, treeNode, 'Create Calendar Month'); 
                    break;
                case "Edit":
                    if (treeNode.get_parent() == treeNode.get_treeView()) {
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
                    else {
                        menuItem.set_enabled(true);
                    }
                    if (treeNode.get_nodes().get_count() > 0) {
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
        </script>


</telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h3>Define Pay Calendar
         </h3> 
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="600px" Width="100%">
        <telerik:RadPane ID="RadPane1" runat="server" Width="320" >
                    
        <telerik:RadTreeView ID="RadTreeView1" runat="server" Height="100%" Width="300px" 
                AllowNodeEditing="false"
                OnContextMenuItemClick="RadTreeView1_ContextMenuItemClick" OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                OnClientContextMenuShowing="onClientContextMenuShowing" OnNodeEdit="RadTreeView1_NodeEdit">
        <ContextMenus>
                    <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                        <Items>
                            <telerik:RadMenuItem Value="New" Text="Create Calendar Month" ImageUrl="~/images/new.jpg">
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
                            <telerik:RadMenuItem Value="New" Text="Create Calendar Month" ImageUrl="~/images/new.jpg">
                            </telerik:RadMenuItem>
                        </Items>
                        <CollapseAnimation Type="none" />
                    </telerik:RadTreeViewContextMenu>
                </ContextMenus>
        </telerik:RadTreeView>
    
    <asp:ImageButton ID="ImageButton1"  OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="ImageButton2" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="ImageButton3" OnClick="btnWord_Click"
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
                            <td>Calendar Year</td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="cboYear" DropDownAutoWidth="Enabled" MaxHeight="200">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="" />
                                        <telerik:RadComboBoxItem Value="2014" Text="2014" />
                                        <telerik:RadComboBoxItem Value="2015" Text="2015" />
                                        <telerik:RadComboBoxItem Value="2016" Text="2016" />
                                        <telerik:RadComboBoxItem Value="2017" Text="2017" />
                                        <telerik:RadComboBoxItem Value="2018" Text="2018" />
                                        <telerik:RadComboBoxItem Value="2019" Text="2019" />
                                        <telerik:RadComboBoxItem Value="2020" Text="2020" /> 
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboYear"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly select a calendar year.">
                                </asp:RequiredFieldValidator>
                                </td>
                        </tr>
                        <tr>
                            <td>Calendar Month</td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="cboMonth" DropDownAutoWidth="Enabled" MaxHeight="200">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="" />
                                        <telerik:RadComboBoxItem Value="1" Text="January" />
                                        <telerik:RadComboBoxItem Value="2" Text="February" />
                                        <telerik:RadComboBoxItem Value="3" Text="March" />
                                        <telerik:RadComboBoxItem Value="4" Text="April" />
                                        <telerik:RadComboBoxItem Value="5" Text="May" />
                                        <telerik:RadComboBoxItem Value="6" Text="June" />
                                        <telerik:RadComboBoxItem Value="7" Text="July" />
                                        <telerik:RadComboBoxItem Value="8" Text="August" />
                                        <telerik:RadComboBoxItem Value="9" Text="September" />
                                        <telerik:RadComboBoxItem Value="10" Text="October" />
                                        <telerik:RadComboBoxItem Value="11" Text="November" />
                                        <telerik:RadComboBoxItem Value="12" Text="December" />
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rfvUnitName" runat="server" ControlToValidate="cboMonth"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly select a calendar month.">
                                </asp:RequiredFieldValidator>                            
                            </td>                            
                        </tr>
                        <tr>
                            <td>Days In the Month</td>
                            <td>
                                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtDaysInMonth"></telerik:RadNumericTextBox>
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
            <div id="divProc" runat="server" style="visibility:hidden">
                <asp:Image ID="Image1" ImageUrl="~/images/animated/processing.gif" runat="server" />
            </div>
            <div id="divError" runat="server" style="visibility:hidden">
                <asp:Image ID="Image2" ImageUrl="~/images/errors/error.jpg" runat="server" />
                <span id="spanError" class="error" runat="server"></span>
            </div>            
        </telerik:RadPane>
    </telerik:RadSplitter>         
    <asp:ImageButton ID="btnExcel"  OnClick="btnExcel_Click"
        runat="server" ImageUrl="~/images/excel.jpg" />
    <asp:ImageButton ID="btnPDF" OnClick="btnPDF_Click"
        runat="server" ImageUrl="~/images/pdf.jpg" />
    <asp:ImageButton ID="btnWord" OnClick="btnWord_Click"
        runat="server" ImageUrl="~/images/word.jpg" /> 
 </asp:Content>
