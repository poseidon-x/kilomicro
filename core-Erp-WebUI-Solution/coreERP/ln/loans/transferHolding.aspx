﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="transferHolding.aspx.cs" Inherits="coreERP.ln.loans.transferHolding" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Transfer from Holding Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Transfer From Holding Account</h3>
    <table> 
        <tr>
            <td>Month (YYYYMM)</td>
            <td>
                <telerik:RadComboBox runat="server" id="cboFile" Width="400px"
                        OnSelectedIndexChanged="cboFile_SelectedIndexChanged" AutoPostBack="true"></telerik:RadComboBox>
            </td>
            <td>Date:</td>
            <td><telerik:RadDatePicker ID="dtAppDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td>Total Deducted by Controller</td>
            <td><asp:Label runat="server" ID="lblTotal"></asp:Label></td>
            <td>Total Applied To Loans</td>
            <td><asp:Label runat="server" ID="lblApplied"></asp:Label></td>
        </tr>
        <tr>
            <td>Total Available for Refund</td>
            <td><asp:Label runat="server" ID="lblRefund"></asp:Label></td>
        </tr> 
    </table>
             <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
                GridLines="Both" AllowSorting="true" ShowFooter="true" AllowPaging="true" AllowMultiRowSelection="true">
                    <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                <MasterTableView autogeneratecolumns="False" EditMode="InPlace"
                            datakeynames="fileDetailID" datasourceid="EntityDataSource1" 
                               CommandItemDisplay="Top" Width="960px">
                    <Columns>
                        <telerik:GridBoundColumn DataField="fileDetailID" DataType="System.Int32" 
                            DefaultInsertValue="" HeaderText="fileDetailID" ReadOnly="True" 
                            SortExpression="fileDetailID" UniqueName="fileDetailID" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="employeeName" DefaultInsertValue="" 
                            HeaderText="Employee Name" SortExpression="employeeName" UniqueName="employeeName" ItemStyle-Width="220px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="staffID" DefaultInsertValue="" 
                            HeaderText="Staff ID" SortExpression="staffID" UniqueName="staffID" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridBoundColumn DataField="managementUnit" DefaultInsertValue="" 
                            HeaderText="Management UNit" SortExpression="managementUnit" UniqueName="managementUnit" ItemStyle-Width="220px">
                        </telerik:GridBoundColumn>  
                        <telerik:GridNumericColumn DataField="monthlyDeduction" DefaultInsertValue=""  DataFormatString="{0:#,##0.#0}"
                            HeaderText="Controller Deduction" SortExpression="monthlyDeduction" UniqueName="monthlyDeduction" ItemStyle-Width="150px">
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
                        <telerik:GridClientSelectColumn></telerik:GridClientSelectColumn>   
                    </Columns> 
                </MasterTableView> 
                 <CLientSettings> 
                     <Selecting AllowRowSelect="true" />
                 </CLientSettings>
            </telerik:RadGrid>
            <telerik:RadButton runat="server" id="btnPost" OnClick="btnPost_Click" Text="Transfer for Refund"></telerik:RadButton>
            <telerik:RadButton runat="server" id="btnPost2" OnClick="btnPost2_Click" Text="Transfer for Manual Receipting"></telerik:RadButton>
    
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" Where="it.fileID == @fileID && it.transferred == FALSE && it.overage>0"
        EntitySetName="controllerFileDetails" Include="repaymentSchedule, repaymentSchedule.loan">
        <WhereParameters>
            <asp:Parameter Name="fileID" Type="Int32" DefaultValue="0" />
        </WhereParameters>
    </ef:EntityDataSource>
</asp:Content>
