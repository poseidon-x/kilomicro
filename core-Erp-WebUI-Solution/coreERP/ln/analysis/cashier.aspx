<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="cashier.aspx.cs" Inherits="coreERP.ln.analysis.cashier" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>


<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Cashier Analysis
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
        <h3>Cashier Transactions Analysis
         </h3> 
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="500px" Width="1448px">
        <telerik:RadPane ID="RadPane1" runat="server" Width="2px">
                    
        <telerik:RadTreeView ID="RadTreeView1" runat="server" Height="100%" Width="240px" 
                AllowNodeEditing="false"  CausesValidation="false" EnableEmbeddedScripts="true" > 
        </telerik:RadTreeView>
    
        </telerik:RadPane> 
        <telerik:RadPane ID="contentPane" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Start Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtStartDate"
                             DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        End Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtpEndDate"
                             DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>
                </div>
            </div>
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Cashier Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" ID="txtCreator"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Client ID
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" ID="txtClientID"></telerik:RadTextBox>
                    </div>
                </div>
            </div>
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Product No.
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" ID="txtRefNo"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Naration
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" ID="txtDesc"></telerik:RadTextBox>
                    </div>
                </div>
            </div>
            <asp:Button ID="btnPreview" runat="server" Text="View Selected Batches" 
                Width="242px" onclick="btnPreview_Click" OnClientClick="showProcessing();" />  
            <asp:Panel runat="server" ID="pnlEditJournal" Visible="false">
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
                                <span class="leftboldtxt" style="width:300px">Cashier Transactions Details</span>
                            </td>
                        </tr>  
                        <tr>
                            <td colspan="2"> 
                                <telerik:RadGrid ID="RadGrid1" runat="server"
        GridLines="Both" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" AllowAutomaticUpdates="false" 
        AllowSorting="true" ShowFooter="true" EnableLinqExpressions="false"  AllowPaging="true"
        OnItemDataBound="RadGrid1_ItemDataBound" PageSize="50" OnLoad="RadGrid1_Load">
            <PagerStyle Mode="NextPrevNumericAndAdvanced" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace" ShowGroupFooter="true" 
            datakeynames="clientID" AllowAutomaticDeletes="True" AllowAutomaticInserts="False" AllowAutomaticUpdates="False"
           CommandItemDisplay="TopAndBottom"> 
            <GroupByExpressions> 
                <telerik:GridGroupByExpression>
                    <SelectFields>
                        <telerik:GridGroupByField FieldAlias="Date" FieldName="txDate" FormatString="{0:dd-MMM-yyyy}"
                             HeaderValueSeparator=": "></telerik:GridGroupByField>
                    </SelectFields>
                    <GroupByFields>
                        <telerik:GridGroupByField FieldName="txDate" SortOrder="Descending"></telerik:GridGroupByField>
                    </GroupByFields>
                </telerik:GridGroupByExpression>
                <telerik:GridGroupByExpression>
                    <SelectFields>
                        <telerik:GridGroupByField FieldAlias="EnteredBy" FieldName="full_name"
                            HeaderValueSeparator=": "></telerik:GridGroupByField>
                    </SelectFields>
                    <GroupByFields>
                        <telerik:GridGroupByField FieldName="full_name" SortOrder="Ascending" ></telerik:GridGroupByField>
                    </GroupByFields>
                </telerik:GridGroupByExpression>
                <telerik:GridGroupByExpression>
                    <SelectFields>
                        <telerik:GridGroupByField FieldAlias="Mode" FieldName="modeOfPaymentName"
                            HeaderValueSeparator=": "></telerik:GridGroupByField>
                    </SelectFields>
                    <GroupByFields>
                        <telerik:GridGroupByField FieldName="txType" SortOrder="Descending" ></telerik:GridGroupByField>
                    </GroupByFields>
                </telerik:GridGroupByExpression>
            </GroupByExpressions>
    <Columns> 
        <telerik:GridTemplateColumn DataField="loanNo" DataType="System.String" 
            DefaultInsertValue="" HeaderText="Product No" 
            SortExpression="loanNo" UniqueName="loanNo" ItemStyle-Width="70px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Aggregate="Count"
            FooterAggregateFormatString="{0:#,###}" FooterStyle-HorizontalAlign="Left" FooterText="# TX: "
            FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
            <ItemTemplate>
                <span><%# DataBinder.Eval(Container.DataItem, "loanNo")%></span>
            </ItemTemplate> 
        </telerik:GridTemplateColumn>  
        <telerik:GridBoundColumn DataField="txDate" UniqueName="txDate" HeaderText="Date"
             SortExpression="txDate" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="accountNumber" 
            DefaultInsertValue="" HeaderText="Client ID" 
            SortExpression="accountNumber" UniqueName="accountNumber" ItemStyle-Width="100px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="clientName" DefaultInsertValue="" 
            HeaderText="Client  Name" SortExpression="clientName" UniqueName="clientName" ItemStyle-Width="200px">
        </telerik:GridBoundColumn> 
        <telerik:GridBoundColumn DataField="naration" DefaultInsertValue="" 
            HeaderText="Naration" SortExpression="naration" UniqueName="naration" ItemStyle-Width="200px">
        </telerik:GridBoundColumn> 
        <telerik:GridTemplateColumn DataField="debit" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Debit" 
            SortExpression="debit" UniqueName="debit" ItemStyle-Width="80px"
            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" Aggregate="Sum"
            FooterAggregateFormatString="{0:#,###.#0}" FooterStyle-HorizontalAlign="Right" FooterText="Total DR: "
            FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
            <ItemTemplate>
                <span><%# ((double)DataBinder.Eval(Container.DataItem, "debit")).ToString("#,###.#0;(#,###.#0);0")%></span>
            </ItemTemplate> 
        </telerik:GridTemplateColumn> 
        <telerik:GridTemplateColumn DataField="credit" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Credit" 
            SortExpression="credit" UniqueName="credit" ItemStyle-Width="80px"
            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" Aggregate="Sum"
            FooterAggregateFormatString="{0:#,###.#0}" FooterStyle-HorizontalAlign="Right" FooterText="Total CR: "
            FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true" FooterStyle-Font-Overline="true">
            <ItemTemplate>
                <span><%# ((double)DataBinder.Eval(Container.DataItem, "credit")).ToString("#,###.#0;(#,###.#0);0")%></span>
            </ItemTemplate> 
        </telerik:GridTemplateColumn>  
        <telerik:GridBoundColumn DataField="modeOfPaymentName" DefaultInsertValue="" 
            HeaderText="Pmt Mode" SortExpression="modeOfPaymentName" UniqueName="modeOfPaymentName" ItemStyle-Width="100px">
        </telerik:GridBoundColumn>   
        <telerik:GridBoundColumn DataField="full_name" DefaultInsertValue="" 
            HeaderText="Cashier" SortExpression="full_name" UniqueName="full_name" ItemStyle-Width="100px">
        </telerik:GridBoundColumn> 
    </Columns>  
            <CommandItemSettings  ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToWordButton="true" />
</MasterTableView>
        <ClientSettings ReorderColumnsOnClient="True" AllowDragToGroup="True" AllowColumnsReorder="True">
            <Selecting AllowRowSelect="True"></Selecting>
            <Resizing AllowRowResize="True" AllowColumnResize="True" EnableRealTimeResize="True"
                ResizeGridOnColumnResize="False"></Resizing>
        </ClientSettings>
        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" UseItemStyles="true">
            <Pdf PageHeight="210mm" PageWidth="297mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="45mm"
                BorderStyle="Medium" BorderColor="#666666">
            </Pdf>
            <Excel Format="ExcelML" />
        </ExportSettings>
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
