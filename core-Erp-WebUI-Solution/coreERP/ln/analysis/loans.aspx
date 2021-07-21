<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="loans.aspx.cs" Inherits="coreERP.ln.analysis.loans" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>


<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
View Loans By Staff`
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
        <h3>View Loans By Staff`</h3>  
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
                                <telerik:RadGrid ID="RadGrid1" runat="server"
        GridLines="Both" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" AllowAutomaticUpdates="false" 
        AllowSorting="true" ShowFooter="true" EnableLinqExpressions="false"  AllowPaging="true"
        OnItemDataBound="RadGrid1_ItemDataBound" PageSize="20" OnLoad="RadGrid1_Load" Width="2000px">
            <PagerStyle Mode="NextPrevNumericAndAdvanced" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace" ShowGroupFooter="true"
            datakeynames="loanID" AllowAutomaticDeletes="True" AllowAutomaticInserts="False" AllowAutomaticUpdates="False"
            CommandItemDisplay="TopAndBottom"> 
            <GroupByExpressions> 
                <telerik:GridGroupByExpression>
                    <SelectFields>
                        <telerik:GridGroupByField FieldAlias="Staff" FieldName="staffName"
                            HeaderValueSeparator=": "></telerik:GridGroupByField>
                    </SelectFields>
                    <GroupByFields>
                        <telerik:GridGroupByField FieldName="staffName" SortOrder="Ascending" ></telerik:GridGroupByField>
                    </GroupByFields>
                </telerik:GridGroupByExpression> 
            </GroupByExpressions>
    <Columns>  
        <telerik:GridImageColumn DataImageUrlFields="clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=64&h=64" HeaderText="P"
                                ImageWidth="64" ImageHeight="64" ItemStyle-Width="64"></telerik:GridImageColumn>
        <telerik:GridBoundColumn DataField="clientName" UniqueName="clientName" HeaderText="Client Name"
             SortExpression="clientName" ItemStyle-Width="200"></telerik:GridBoundColumn> 
        <telerik:GridBoundColumn DataField="loanNo" UniqueName="loanNo" HeaderText="Loan ID"
             SortExpression="loanNo"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="disbursementDate" UniqueName="disbursementDate" HeaderText="Disb. Date"
             SortExpression="disbursementDate"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="amountDisbursed" UniqueName="amountDisbursed" HeaderText="Disb. Amount"
             SortExpression="amountDisbursed" ></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="daysDue" UniqueName="daysDue" HeaderText="Days Delinquent"
             SortExpression="daysDue" DataFormatString="{0:#,##0}"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="amountDue" HeaderText="Amt. Due"
             DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn> 
        <telerik:GridBoundColumn DataField="expiryDate" UniqueName="expiryDate" HeaderText="Expiry Date"
             SortExpression="expiryDate" DataFormatString="{0:dd-MMM-yyyy}" ></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="totalBalance" HeaderText="Balance"
             DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn> 
        <telerik:GridBoundColumn DataField="mobilePhone" UniqueName="mobilePhone" HeaderText="Mobile #"
             SortExpression="mobilePhone"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="homePhone" UniqueName="homePhone" HeaderText="Home #"
             SortExpression="homePhone"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="workPhone" UniqueName="workPhone" HeaderText="Work #"
             SortExpression="workPhone"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="directions" DefaultInsertValue="" 
            HeaderText="Loc. Directions" SortExpression="directions" UniqueName="directions" ItemStyle-Width="200px">
        </telerik:GridBoundColumn>  
        <telerik:GridBoundColumn DataField="addressLine1" DefaultInsertValue="" 
            HeaderText="Postal Address" SortExpression="addressLine1" UniqueName="addressLine1" ItemStyle-Width="200px">
        </telerik:GridBoundColumn>   
        <telerik:GridBoundColumn DataField="cityTown" UniqueName="cityTown" HeaderText="City"
             SortExpression="cityTown"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="collateralType" UniqueName="collateralType" HeaderText="Collateral Type"
             SortExpression="collateralType"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="collateralValue" UniqueName="collateralValue" HeaderText="Collateral Value"
             SortExpression="collateralValue"  DataFormatString="{0:#,##0.#0;(#,##0.#0);0.0}"></telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="staffName" UniqueName="staffName" HeaderText="Assigned Staff"
             SortExpression="staffName"></telerik:GridBoundColumn>
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
     
              
            <div id="divProc" runat="server" style="visibility:hidden">
                <asp:Image ID="Image1" ImageUrl="~/images/animated/processing.gif" runat="server" />
            </div>
            <div id="divError" runat="server" style="visibility:hidden">
                <asp:Image ID="Image2" ImageUrl="~/images/errors/error.jpg" runat="server" />
                <span id="spanError" class="error" runat="server"></span>
            </div>  
</asp:Content>
