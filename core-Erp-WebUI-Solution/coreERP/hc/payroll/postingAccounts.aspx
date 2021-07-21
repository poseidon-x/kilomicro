<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="postingAccounts.aspx.cs" Inherits="coreERP.hc.payroll.postingAccounts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Payroll Posting Accounts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Posting Employee Accounts</h3>

    <table>
        <tr>
            <td>Net Salary Payable Account</td>
            <td>
                <telerik:RadComboBox runat="server" Width="200px" DropDownAutoWidth="Enabled"
                     MarkFirstMatch="true" DataTextField="fullname" DataValueField="acct_id"
                    ID="cboNet" DataSourceID="EntityDataSource1"></telerik:RadComboBox>
            </td>
        </tr> 
        <tr>
            <td>Loans Receivable Account</td>
            <td>
                <telerik:RadComboBox runat="server" Width="200px" DropDownAutoWidth="Enabled"
                     MarkFirstMatch="true" DataTextField="fullname" DataValueField="acct_id"
                    ID="cboLoans" DataSourceID="EntityDataSource1"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Pensions Payable Account</td>
            <td>
                <telerik:RadComboBox runat="server" Width="200px" DropDownAutoWidth="Enabled"
                     MarkFirstMatch="true" DataTextField="fullname" DataValueField="acct_id"
                    ID="cboPension" DataSourceID="EntityDataSource1"></telerik:RadComboBox>
            </td>
        </tr> 
        <tr>
            <td>Payroll Expense Account</td>
            <td>
                <telerik:RadComboBox runat="server" Width="200px" DropDownAutoWidth="Enabled"
                     MarkFirstMatch="true" DataTextField="fullname" DataValueField="acct_id"
                    ID="cboExpense" DataSourceID="EntityDataSource1"></telerik:RadComboBox>
            </td>
        </tr>   
        <tr>
            <td>IRS Tax Payable Account</td>
            <td>
                <telerik:RadComboBox runat="server" Width="200px" DropDownAutoWidth="Enabled"
                     MarkFirstMatch="true" DataTextField="fullname" DataValueField="acct_id"
                    ID="cboTax" DataSourceID="EntityDataSource1"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Loans Deductions Account</td>
            <td>
                <telerik:RadComboBox runat="server" Width="200px" DropDownAutoWidth="Enabled"
                     MarkFirstMatch="true" DataTextField="fullname" DataValueField="acct_id"
                    ID="cboLoanDed" DataSourceID="EntityDataSource1"></telerik:RadComboBox>
            </td>
        </tr>   
        <tr>
            <td>Overtime Withholding Account</td>
            <td>
                <telerik:RadComboBox runat="server" Width="200px" DropDownAutoWidth="Enabled"
                     MarkFirstMatch="true" DataTextField="fullname" DataValueField="acct_id"
                    ID="cboOvertime" DataSourceID="EntityDataSource1"></telerik:RadComboBox>
            </td>
        </tr>  
        <tr>
            <td>Other Account</td>
            <td>
                <telerik:RadComboBox runat="server" Width="200px" DropDownAutoWidth="Enabled"
                     MarkFirstMatch="true" DataTextField="fullname" DataValueField="acct_id"
                    ID="cboDed" DataSourceID="EntityDataSource1"></telerik:RadComboBox>
            </td>
        </tr>  
        <tr>
            <td></td>
            <td><telerik:RadButton runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click"></telerik:RadButton></td>
        </tr> 
    </table>
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="false" EnableInsert="false" EnableUpdate="false" 
        EntitySetName="vw_accounts" OrderBy="it.fullname ASC">
    </ef:EntityDataSource>
</asp:Content>
