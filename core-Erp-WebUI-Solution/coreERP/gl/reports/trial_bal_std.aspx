<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="trial_bal_std.aspx.cs" Inherits="coreERP.gl.reports.trial_bal_std" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
::Trial Balance (Standard)
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Trial Balance (Standard)</h3>
    <div style="text-align:center">
        <table style="padding-left:100px">
            <tr>
                <td align="right" style="font-weight:bold">
                    Start Date:
                </td>
                <td align="left">
                    <telerik:RadDatePicker runat="server" ID="dtpStartDate"  
                        Culture="English (United States)">
                        <Calendar runat="server" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>  
                        <DatePopupButton></DatePopupButton>
                        <DateInput runat="server" DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                    </telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-weight:bold">
                    Ebd Date:
                </td>
                <td align="left">
                    <telerik:RadDatePicker runat="server" ID="dtpEndDate"  
                        Culture="English (United States)">
                        <Calendar runat="server" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>  
                        <DatePopupButton></DatePopupButton>
                        <DateInput runat="server" DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                    </telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-weight:bold">
                    Accounts W/O Tx
                </td>
                <td align="left">
                    <asp:CheckBox ID="chkNoTx" runat="server" Checked="false" />
                </td>
                <td align="right" style="font-weight:bold">
                    Show Summary
                </td>
                <td align="left">
                    <asp:CheckBox ID="chkSum" runat="server" Checked="false" />
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
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnRun" Text="Get Report" 
                        onclick="btnRun_Click" />
                </td>
            </tr>  
                        <tr>
                            <td colspan="2">              
                                <div id="divProc" runat="server" style="visibility:hidden">
                                </div>
                                <div id="divError" runat="server" style="visibility:hidden">
                                    <span id="spanError" class="error" runat="server"></span>
                                </div>            
                            </td>
                        </tr>
           </table>
    </div>
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server" 
            BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" 
            ondatabinding="rvw_DataBinding" ToolPanelView="None"  />
</asp:Content>
