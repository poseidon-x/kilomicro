<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="op_stmt_std.aspx.cs" Inherits="coreERP.gl.budget.op_stmt_std" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Income Statement (Budget v/s Actual)
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Income Statement (Budget v/s Actual)</h3>
    <div style="text-align:center">
        <table style="padding-left:100px">
            <tr>
                <td align="right" style="font-weight:bold">
                    Date:
                </td>
                <td align="left">
                    <telerik:RadDateTimePicker runat="server" ID="dtpEndDate" 
                        Culture="English (United States)">
<Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>

<TimeView CellSpacing="-1" TimeFormat="H:mm"></TimeView>

<TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>

<DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>

<DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                    </telerik:RadDateTimePicker>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-weight:bold">
                    Accounts W/O Tx
                </td>
                <td align="left">
                    <asp:CheckBox ID="chkNoTx" runat="server" Checked="false" />
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
           </table>
    </div>
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server" 
            BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" 
            ondatabinding="rvw_DataBinding" ToolPanelView="None"  />
</asp:Content>
