<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="post.aspx.cs" Inherits="coreERP.gl.v.post" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>


<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Post Voucher Transactions
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
                    var msg = 'Create new Voucher Transactions'; 
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
        <h3>Post Voucher Transactions</h3>
          
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="500px" Width="1448px">
        <telerik:RadPane ID="RadPane1" runat="server" Width="250px">
                    
        <telerik:RadTreeView ID="RadTreeView1" runat="server" Height="100%" Width="240px" 
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
            <asp:CheckBox runat="server" ID="chkConsolidate" Text="Consolidate?" Checked="true" />
            <asp:Button ID="btnPreview" runat="server" Text="Preview Proposed Entries" 
                Width="156px" onclick="btnPreview_Click" 
                OnClientClick="showProcessing();" /> 
            <asp:Button ID="btnPost" runat="server" Text="Post Selected Batches" 
                Width="147px" onclick="btnPost_Click" OnClientClick="showProcessing();" Enabled="false" /> 
            <asp:Panel runat="server" ID="pnlEditJournal" Visible="false">
                   <table>
                        <tr>
                            <td colspan="4">
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
                                <span class="leftboldtxt" style="width:300px">Proposed Journal Entries</span>
                            </td>
                        </tr> 
                        <tr>
                            <td colspan="4"> 
                                <telerik:RadGrid ID="RadGrid1" runat="server"
        GridLines="Both" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" AllowAutomaticUpdates="false" 
        AllowSorting="true" ShowFooter="true" EnableLinqExpressions="false" 
        OnItemDataBound="RadGrid1_ItemDataBound">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace" DataSourceID="EntityDataSource1"
            datakeynames="jnl_batch_id" AllowAutomaticDeletes="false" AllowAutomaticInserts="false" AllowAutomaticUpdates="false"
               CommandItemDisplay="None" Width="860px"> 
               <DetailTables>
                    <telerik:GridTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="jnl_id" AllowAutomaticDeletes="false" AllowAutomaticInserts="false" AllowAutomaticUpdates="false"
               CommandItemDisplay="None" Width="960px" DataSourceID="EntityDataSource2">
                     <ParentTableRelation>
                        <telerik:GridRelationFields DetailKeyField="jnl_batch_id" MasterKeyField="jnl_batch_id" />
                     </ParentTableRelation>
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
                            </telerik:GridTemplateColumn>  
                            <telerik:GridTemplateColumn DataField="acct_id" DataType="System.Double" 
                                DefaultInsertValue="" HeaderText="Account" 
                                SortExpression="acct_id" UniqueName="acct_id" ItemStyle-Width="200px"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <span><%# AccountName(int.Parse(DataBinder.Eval(Container.DataItem, "acct_id").ToString()))%></span>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="description" DefaultInsertValue="" 
                                HeaderText="Description" SortExpression="description" UniqueName="description" ItemStyle-Width="220px">
                            </telerik:GridBoundColumn> 
                            <telerik:GridTemplateColumn DataField="currency_id" DataType="System.Double" 
                                DefaultInsertValue="" HeaderText="Currency" 
                                SortExpression="currency_id" UniqueName="currency_id" ItemStyle-Width="150px"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <span><%# CurrencyName(int.Parse(DataBinder.Eval(Container.DataItem, "currency_id").ToString())) %></span>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="rate" DataType="System.Double" 
                                DefaultInsertValue="" HeaderText="Rate" 
                                SortExpression="rate" UniqueName="rate" ItemStyle-Width="35px"
                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" >
                                <ItemTemplate>
                                    <span><%# ((double)DataBinder.Eval(Container.DataItem, "rate")).ToString("#,###.#0;(#,###.#0);0")%></span>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>  
                            <telerik:GridBoundColumn DataField="description" DefaultInsertValue="" 
                                HeaderText="Description" SortExpression="description" UniqueName="description" ItemStyle-Width="180px">
                            </telerik:GridBoundColumn> 
                            <telerik:GridTemplateColumn DataField="dbt_amt" DataType="System.Double" 
                                DefaultInsertValue="" HeaderText="Debit" 
                                SortExpression="dbt_amt" UniqueName="dbt_amt" ItemStyle-Width="80px"
                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" Aggregate="Sum"
                                FooterAggregateFormatString="{0:#,###.#0;(#,###.#0);0}" FooterStyle-HorizontalAlign="Right"
                                FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
                                <ItemTemplate>
                                    <span><%# ((double)DataBinder.Eval(Container.DataItem, "dbt_amt")).ToString("#,###.#0;(#,###.#0);0")%></span>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn> 
                            <telerik:GridTemplateColumn DataField="crdt_amt" DataType="System.Double" 
                                DefaultInsertValue="" HeaderText="Credit" 
                                SortExpression="crdt_amt" UniqueName="crdt_amt" ItemStyle-Width="80px"
                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" Aggregate="Sum" 
                                FooterAggregateFormatString="{0:#,###.#0;(#,###.#0);0}" FooterStyle-HorizontalAlign="Right"
                                FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
                                <ItemTemplate>
                                    <span><%# ((double)DataBinder.Eval(Container.DataItem, "crdt_amt")).ToString("#,###.#0;(#,###.#0);0")%></span>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="gl_ou_id" DataType="System.Double" 
                                DefaultInsertValue="" HeaderText="Cost Center" 
                                SortExpression="gl_ou_id" UniqueName="gl_ou_id" ItemStyle-Width="100px"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <span><%# CostCenterName(DataBinder.Eval(Container.DataItem, "cost_center_id"))%></span>
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
                    </telerik:GridTableView>
                </DetailTables>
               <Columns>
                    <telerik:GridBoundColumn DataField="jnl_batch_id" DataType="System.Int32" 
                        DefaultInsertValue="" HeaderText="jnl_batch_id" ReadOnly="True" 
                        SortExpression="jnl_batch_id" UniqueName="jnl_batch_id" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="batch_no" DataType="System.String" 
                        DefaultInsertValue="" HeaderText="Batch Number" 
                        SortExpression="batch_no" UniqueName="batch_no" ItemStyle-Width="150px"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <span><%# DataBinder.Eval(Container.DataItem, "batch_no")%></span>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="creator" DataType="System.String" 
                        DefaultInsertValue="" HeaderText="Originator" 
                        SortExpression="creator" UniqueName="creator" ItemStyle-Width="150px"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <span><%# DataBinder.Eval(Container.DataItem, "creator")%></span>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="source" DataType="System.String" 
                        DefaultInsertValue="" HeaderText="Source" 
                        SortExpression="source" UniqueName="source" ItemStyle-Width="150px"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <span><%# DataBinder.Eval(Container.DataItem, "source")%></span>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
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
         
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=coreGLEntities" DefaultContainerName="coreGLEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="jnl_batch_stg" Where="it.creator=@creator">
        <WhereParameters>
            <asp:Parameter ConvertEmptyStringToNull="true" Name="creator" Type="String" />
        </WhereParameters>
    </ef:EntityDataSource>  
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreGLEntities" DefaultContainerName="coreGLEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="jnl_stg" Where="it.jnl_batch_stg.jnl_batch_id=@jnl_batch_id">
        <WhereParameters>
            <asp:Parameter Name="jnl_batch_id" Type="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>
</asp:Content>
