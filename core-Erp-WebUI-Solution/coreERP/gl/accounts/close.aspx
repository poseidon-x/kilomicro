<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="close.aspx.cs" Inherits="coreERP.gl.accounts.close" %>

<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>


<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Close Accounting Period
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
        <h3>Close Accounting Period
         </h3>  
                   <table>
                        <tr>
                            <td colspan="2">
                               <a href="~/common/prof.aspx" runat="server" id="linkCompProf" 
                                target="_blank"><img id="Img1" src="~/images/comp_prof.jpg" 
                                     runat="server" alt="Company Profile"
                                     width="32" height="32" title="Company Profile" />
                               </a>
                               <a href="~/gl/accounts/default.aspx" runat="server" id="linkCA" 
                                target="_blank"><img id="Img2" src="../../images/chart_of_accounts/chart_of_accounts.jpg" 
                                     runat="server" alt="Chart of Accounts"
                                     width="32" height="32" title="Chart of Accunts" />
                               </a>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100px">Period End Date:</td>
                            <td>
                                <telerik:RadDatePicker ID="dtTransactionDate" runat="server" 
                                    DateInput-DisplayDateFormat="dd-MMM-yyyy"  OnSelectedDateChanged="Date_Changed"
                                 ></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="dtTransactionDate"
                                     ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the transaction date.">
                                </asp:RequiredFieldValidator>                     
                            </td> 
                        </tr> 
                       <tr>
                <td>Cost Center (Department)</td>
                <td>
                 <telerik:RadComboBox ID="cboCC" runat="server" Height="200px" Width=" 225px"
                    DropDownWidth=" 155px" EmptyMessage="Cost Center" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" 
                     DataTextField="ou_name" DataValueField="ou_id">
                    <HeaderTemplate>
                        <table style="width: 155px" cellspacing="0" cellpadding="0">
                            <tr> 
                                <td style="width: 150px;">
                                    Cost Center</td> 
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 155px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 150px;" >
                                    <%# Eval("ou_name")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
                </td>
            </tr>
                        <tr>
                            <td><asp:Button runat="server" ID="btnFetch" Text="Get Balances" 
                                    onclick="btnFetch_Click" /></td>
                            <td><asp:Button runat="server" ID="btnClose" Text="Close Period" Enabled="false"
                             OnClick="btnClose_Click" /></td>
                        </tr>
                   </table>
            <div id="divError" runat="server" style="visibility:hidden">
                <span id="spanError" class="error" runat="server"></span>
            </div>      
     <telerik:RadGrid ID="RadGrid1" runat="server" 
        GridLines="Both" AllowAutomaticInserts="false" AllowAutomaticDeletes="false" AllowAutomaticUpdates="false"
        AllowSorting="true" ShowFooter="true" OnItemDataBound="RadGrid1_ItemDataBound">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False"
            datakeynames="acct_id" AllowAutomaticDeletes="false" AllowAutomaticInserts="false" AllowAutomaticUpdates="false"
               CommandItemDisplay="Top" Width="660px">
    <Columns>
        <telerik:GridBoundColumn DataField="acct_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="acct_id" ReadOnly="True" 
            SortExpression="acct_id" UniqueName="acct_id" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="acc_num" DefaultInsertValue="" 
            HeaderText="Acc Num" SortExpression="acc_num" UniqueName="acc_num" ItemStyle-Width="150px">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="acc_name" DefaultInsertValue="" 
            HeaderText="Acc Name" SortExpression="acc_name" UniqueName="acc_name" ItemStyle-Width="250px">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn DataField="loc_end_bal" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Debit" 
            SortExpression="loc_end_bal" UniqueName="loc_end_bal" ItemStyle-Width="100px"
            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
            FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true">
            <ItemTemplate>
                <span><%# Debit(DataBinder.Eval(Container.DataItem, "cat_code"),
                          DataBinder.Eval(Container.DataItem, "loc_end_bal")).ToString("#,###.#0;(#,###.#0);0")%></span>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="frgn_end_bal" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Credit" 
            SortExpression="frgn_end_bal" UniqueName="frgn_end_bal" ItemStyle-Width="100px"
            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
            FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true" FooterStyle-Font-Underline="true">
            <ItemTemplate>
                <span><%# Credit(DataBinder.Eval(Container.DataItem, "cat_code"),
                          DataBinder.Eval(Container.DataItem, "loc_end_bal")).ToString("#,###.#0;(#,###.#0);0")%></span>
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
</MasterTableView>
       <ClientSettings>
           <Selecting AllowRowSelect="true" />
           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
           <Selecting AllowRowSelect="true" />
       </ClientSettings>
    </telerik:RadGrid>
         
</asp:Content>
