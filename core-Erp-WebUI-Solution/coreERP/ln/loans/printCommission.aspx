<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="printCommission.aspx.cs" Inherits="coreERP.ln.reports.printCommission" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
::Loan Agent's Commission Printout
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 133px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Agents Commission Report</h3>
    <table>
        <tr>
            <td>
                Sales Agent 
            </td>
            <td colspan="2">
                <telerik:RadComboBox ID="cboAgent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboAgent_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select an Agent" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>Start Date:</td>
            <td>
                <telerik:RadDatePicker runat="server" ID="dtStart" DateInput-DateFormat="dd-MMM-yyyy"
                     AutoPostBack="true" OnSelectedDateChanged="dtStart_SelectedDateChanged"></telerik:RadDatePicker>
            </td>
            <td>End Date:</td>
            <td>
                <telerik:RadDatePicker runat="server" ID="dtEnd" DateInput-DateFormat="dd-MMM-yyyy"
                     AutoPostBack="true" OnSelectedDateChanged="dtStart_SelectedDateChanged"></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">
                Loans 
            </td>
            <td colspan="3"> 
                <asp:CheckBoxList runat="server" ID="cblLoans" Height="150px" Width="408px"></asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td></td>
            <td><telerik:RadButton runat="server" ID="btnRun" Text="Run Report" OnClick="btnRun_Click"></telerik:RadButton></td>
        </tr>
     </table>
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server" 
            BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" 
         OnDataBinding="rvw_DataBinding"
            />
</asp:Content>
