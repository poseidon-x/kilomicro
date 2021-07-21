<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="tx_by_acc.aspx.cs" Inherits="coreERP.gl.reports.tx_by_acc" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
::Transactions By Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Transactions By Account</h3>

    <div style="text-align:center">
        <table style="padding-left:100px">
            <tr>
                <td align="right" style="font-weight:bold">
                    Start Date:
                </td>
                <td align="left">
                    <telerik:RadDateTimePicker runat="server" ID="dtpStartDate" 
                        Culture="English (United States)" MinDate="1900-01-01">
<Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>

<TimeView CellSpacing="-1" TimeFormat="H:mm"></TimeView>

<TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>

<DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>

<DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                    </telerik:RadDateTimePicker>
                </td>
                <td align="right" style="font-weight:bold">
                    End Date:
                </td>
                <td align="left">
                    <telerik:RadDateTimePicker runat="server" ID="dtpEndDate" 
                        Culture="English (United States)" MinDate="1900-01-01">
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
                    Foreign Currency
                </td>
                <td align="left">
                    <asp:CheckBox ID="chkForeign" runat="server" Checked="false" />
                </td>
                <td align="right" style="font-weight:bold">
                    Currency
                </td>
                <td align="left">                    
                 <telerik:RadComboBox ID="cboCur" runat="server" Height="150px" Width=" 255px"
                    DropDownWidth=" 255px" EmptyMessage="Transaction Currency" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" 
                     DataTextField="major_name" DataValueField="currency_id">
                    <HeaderTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr> 
                                <td style="width: 80px;">
                                    Cur Sym</td>
                                <td style="width: 150px;">
                                    Cur Name</td>
                                <td style="width: 80px;">
                                    Rate</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 80px;" >
                                    <%# Eval("major_symbol")%>
                                </td>
                                <td style="width: 150px;">
                                    <%# Eval("major_name")%>
                                </td> 
                                <td style="width: 80px;">
                                    <%# Eval("current_buy_rate")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-weight:bold">
                    Account
                </td>
                <td align="left">
                 <telerik:RadComboBox ID="cboAcc" runat="server" Height="150px" Width=" 300px" DataTextField="fullname" DataValueField="acct_id"
                    DropDownWidth=" 500px" EmptyMessage="Type the number, name of a GL Account" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" OnItemsRequested="cboGLAcc_ItemsRequested"   
                    EnableLoadOnDemand="true" AutoPostBack="true">
                    <HeaderTemplate>
                        <table style="width: 500px">
                            <tr> 
                                <td style="width: 80px;">
                                    Acc Num</td>
                                <td style="width: 320px;">
                                    Acc Name</td>
                                <td style="width: 100px;">
                                    Acc Currency</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 500px">
                            <tr>
                                <td style="width: 80px;" >
                                    <%# Eval("acc_num")%>
                                </td>
                                <td style="width: 320px;">
                                    <%# Eval("acc_name")%>
                                </td> 
                                <td style="width: 100px;">
                                    <%# Eval("major_name")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
                </td>
                <td align="right" style="font-weight:bold">
                    Batch No.
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBatchNo" runat="server" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-weight:bold">
                    Cost Center
                </td>
                <td align="left">
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
                <td align="right" style="font-weight:bold">
                    Ref No.
                </td>
                <td align="left">
                    <asp:TextBox ID="txtRefNo" runat="server" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>        
                <td></td>    
                <td align="right">
                    <asp:Button runat="server" ID="btnRun" Text="Get Report" 
                        onclick="btnRun_Click" />
                </td>
                <td colspan="2"></td>
            </tr>
           </table>

    </div>
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server" 
            BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" 
            ondatabinding="rvw_DataBinding" ToolPanelView="None"  />
</asp:Content>
