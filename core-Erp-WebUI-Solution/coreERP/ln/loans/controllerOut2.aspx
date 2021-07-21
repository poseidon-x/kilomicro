<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="controllerOut2.aspx.cs" Inherits="coreERP.ln.loans.controllerOut2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Controller Output File Processing
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Controller Output File</h3>
    
    <telerik:RadMultiPage runat="server" ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server"> 
            <h2>Please select the controller file to continue</h2>
            <table>
                <tr>
                    <td style="width:200px">Controller Output File</td>
                    <td style="width:200px">
                        <telerik:RadComboBox runat="server" ID="cboFile" Width="200px"
                             AutoPostBack="true" OnSelectedIndexChanged="cboFile_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>Month (YYYYMM)</td>
                    <td>
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" Enabled="false" runat="server" id="txtMonth" Decimal-Digits="0"></telerik:RadNumericTextBox>
                    </td>
                </tr>    
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadButton Enabled="true" runat="server" id="btnNext1" OnClick="btnNext1_Click" Text="Next"></telerik:RadButton>
                    </td>
                </tr>    
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server"> 
            <h2>Please verify the details of the file below to continue</h2> 
             <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1" OnItemCommand="RadGrid1_OnItemCommand"
                GridLines="Both" AllowSorting="true" ShowFooter="true" AllowPaging="true">
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" BorderStyle="None" PageSizes="10,20,50,100,500,1000" />
                <MasterTableView autogeneratecolumns="False" EditMode="InPlace"
                            datakeynames="fileDetailID" datasourceid="EntityDataSource1" 
                               CommandItemDisplay="Top" Width="1260px" PageSize="1000">
                    <Columns>
                        <telerik:GridBoundColumn DataField="fileDetailID" DataType="System.Int32" 
                            DefaultInsertValue="" HeaderText="fileDetailID" ReadOnly="True" 
                            SortExpression="fileDetailID" UniqueName="fileDetailID" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="staffID" DefaultInsertValue="" 
                            HeaderText="Staff ID" SortExpression="staffID" UniqueName="staffID" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="employeeName" DefaultInsertValue="" 
                            HeaderText="Employee Name" SortExpression="employeeName" UniqueName="employeeName" ItemStyle-Width="320px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="managementUnit" DefaultInsertValue="" 
                            HeaderText="Management Unit" SortExpression="managementUnit" UniqueName="managementUnit" ItemStyle-Width="320px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridNumericColumn DataField="monthlyDeduction" DefaultInsertValue=""  DataFormatString="{0:#,##0.#0}"
                            HeaderText="Deducted Amount" SortExpression="monthlyDeduction" UniqueName="monthlyDeduction" ItemStyle-Width="150px">
                        </telerik:GridNumericColumn>   
                        <telerik:GridNumericColumn DataField="balBF" DefaultInsertValue=""  DataFormatString="{0:#,##0.#0}"
                            HeaderText="Bal B/F" SortExpression="balBF" UniqueName="balBF" ItemStyle-Width="150px">
                        </telerik:GridNumericColumn>  
                        <telerik:GridBoundColumn DataField="repaymentSchedule.loan.loanNo" DefaultInsertValue="" 
                            HeaderText="Matching Loan" SortExpression="repaymentSchedule.loan.loanNo" UniqueName="repaymentSchedule.loan.loanNo" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>   
                        <telerik:GridBoundColumn DataField="repaymentSchedule.repaymentDate" DefaultInsertValue="" DataFormatString="{0:dd-MMM-yyyy}"
                            HeaderText="Matching Date" SortExpression="repaymentSchedule.repaymentDate" UniqueName="repaymentSchedule.repaymentDate" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>   
                        <telerik:GridNumericColumn DataField="overage" DefaultInsertValue="" DataFormatString="{0:#,##0.#0}"
                            HeaderText="Difference" SortExpression="overage" UniqueName="overage" ItemStyle-Width="150px">
                        </telerik:GridNumericColumn> 
                        <telerik:GridBoundColumn DataField="remarks" DefaultInsertValue="" 
                            HeaderText="Remarks" SortExpression="remarks" UniqueName="remarks" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridButtonColumn CommandName="Swap" Text="Swap" HeaderText="Swap Loans"></telerik:GridButtonColumn>  
                    </Columns> 
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToWordButton="true" />    
                </MasterTableView> 
                 <ExportSettings ExportOnlyData="true" IgnorePaging="true">
                     <Excel Format="ExcelML" />
                 </ExportSettings>
            </telerik:RadGrid>
            <telerik:RadButton runat="server" id="btnNext2" OnClick="btnNext2_Click" Text="Next"></telerik:RadButton>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView3" runat="server"> 
            <h2 runat="server" id="h2Label">Please verify the details of the exact matches below and post them</h2> 
             <telerik:RadGrid ID="RadGrid2" runat="server" DataSourceID="EntityDataSource2" OnItemCommand="RadGrid2_OnItemCommand"
                GridLines="Both" AllowSorting="true" ShowFooter="true" AllowPaging="true">
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" BorderStyle="None"  PageSizes="10,20,50,100,500,1000"/>
                <MasterTableView autogeneratecolumns="False" EditMode="InPlace"
                            datakeynames="fileDetailID" datasourceid="EntityDataSource2" 
                               CommandItemDisplay="Top" Width="1260px" PageSize="1000">
                    <Columns>
                        <telerik:GridBoundColumn DataField="fileDetailID" DataType="System.Int32" 
                            DefaultInsertValue="" HeaderText="fileDetailID" ReadOnly="True" 
                            SortExpression="fileDetailID" UniqueName="fileDetailID" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="staffID" DefaultInsertValue="" 
                            HeaderText="Staff ID" SortExpression="staffID" UniqueName="staffID" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="employeeName" DefaultInsertValue="" 
                            HeaderText="Employee Name" SortExpression="employeeName" UniqueName="employeeName" ItemStyle-Width="320px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="managementUnit" DefaultInsertValue="" 
                            HeaderText="Management UNit" SortExpression="managementUnit" UniqueName="managementUnit" ItemStyle-Width="320px">
                        </telerik:GridBoundColumn> 
                        <telerik:GridNumericColumn DataField="monthlyDeduction" DefaultInsertValue=""  DataFormatString="{0:#,##0.#0}"
                            HeaderText="Deducted Amount" SortExpression="monthlyDeduction" UniqueName="monthlyDeduction" ItemStyle-Width="150px">
                        </telerik:GridNumericColumn>   
                        <telerik:GridNumericColumn DataField="balBF" DefaultInsertValue=""  DataFormatString="{0:#,##0.#0}"
                            HeaderText="Bal B/F" SortExpression="balBF" UniqueName="balBF" ItemStyle-Width="150px">
                        </telerik:GridNumericColumn>  
                        <telerik:GridBoundColumn DataField="repaymentSchedule.loan.loanNo" DefaultInsertValue="" 
                            HeaderText="Matching Loan" SortExpression="repaymentSchedule.loan.loanNo" UniqueName="repaymentSchedule.loan.loanNo" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>     
                        <telerik:GridBoundColumn DataField="repaymentSchedule.repaymentDate" DefaultInsertValue="" DataFormatString="{0:dd-MMM-yyyy}"
                            HeaderText="Matching Date" SortExpression="repaymentSchedule.repaymentDate" UniqueName="repaymentSchedule.repaymentDate" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridNumericColumn DataField="overage" DefaultInsertValue="" DataFormatString="{0:#,##0.#0}"
                            HeaderText="Difference" SortExpression="overage" UniqueName="overage" ItemStyle-Width="150px">
                        </telerik:GridNumericColumn> 
                        <telerik:GridBoundColumn DataField="remarks" DefaultInsertValue="" 
                            HeaderText="Remarks" SortExpression="remarks" UniqueName="remarks" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>    
                        <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                            <ItemTemplate>
                              <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="ToggleRowSelection"
                                AutoPostBack="True" />
                            </ItemTemplate>
                            <HeaderTemplate>
                              <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState"
                                AutoPostBack="True" />
                            </HeaderTemplate>
                          </telerik:GridTemplateColumn>
                        <telerik:GridButtonColumn CommandName="Swap" Text="Swap" HeaderText="Swap Loans"></telerik:GridButtonColumn>  
                    </Columns> 
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToWordButton="true" />    
                </MasterTableView> 
                 <ExportSettings ExportOnlyData="true" IgnorePaging="true">
                     <Excel Format="ExcelML" />
                 </ExportSettings>
            </telerik:RadGrid>
            <telerik:RadButton runat="server" id="btnPrev" OnClick="btnPrev_Click" Text="Move Previous"></telerik:RadButton>
            <telerik:RadButton runat="server" id="btnPost" OnClick="btnPost_Click" Text="Post Current"></telerik:RadButton>
            <telerik:RadButton runat="server" id="btnNext3" OnClick="btnNext3_Click" Text="Move Next"></telerik:RadButton>
            <telerik:RadButton runat="server" id="btnExport" OnClick="btnExport_Click" Text="Save to Excel" enabled="false"></telerik:RadButton>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" Where="it.fileID == @fileID && it.authorized == false"
        EntitySetName="controllerFileDetails" Include="repaymentSchedule, repaymentSchedule.loan">
        <WhereParameters>
            <asp:Parameter Name="fileID" Type="Int32" DefaultValue="0" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" Where="it.fileID == @fileID && it.remarks == @remarks && it.authorized == false"
        EntitySetName="controllerFileDetails" Include="repaymentSchedule, repaymentSchedule.loan">
        <WhereParameters>
            <asp:Parameter Name="fileID" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="remarks" Type="String" DefaultValue="" />
        </WhereParameters>
    </ef:EntityDataSource>
</asp:Content>
