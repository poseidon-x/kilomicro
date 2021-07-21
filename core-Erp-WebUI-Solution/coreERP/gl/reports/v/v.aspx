<%@ Page Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="v.aspx.cs" Inherits="coreERP.gl.reports.v.v" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
::Voucher Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Voucher Report</h3>
    <div style="text-align:center">
        <table style="padding-left:100px">
            <tr>
                <td colspan="2">
                    <asp:LinkButton runat="server" ID="btnReturn" Text="Return" 
                        onclick="btnReturn_Click"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-weight:bold">
                    Batch Number:
                </td>
                <td align="left">
                    <telerik:RadComboBox ID="cboBatch" runat="server" Height="150px" Width=" 255px"
                    DropDownWidth=" 355px" EmptyMessage="Select A Batch" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" 
                     DataTextField="batch_no" DataValueField="batch_no">
                    <HeaderTemplate>
                        <table style="width: 355px" cellspacing="0" cellpadding="0">
                            <tr> 
                                <td style="width: 100px;">
                                    Batch No</td>
                                <td style="width: 150px;">
                                    Date</td>
                                <td style="width: 150px;">
                                    Customer/Recipient</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 355px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 100px;" >
                                    <%# Eval("batch_no")%>
                                </td>
                                <td style="width: 150px;">
                                    <%# Eval("tx_date")%>
                                </td> 
                                <td style="width: 150px;">
                                    <%# Eval("recipient")%>
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
    <cr:crystalreportviewer ID="rvw" AutoDataBind="true" runat="server" 
            BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" 
            ondatabinding="rvw_DataBinding" ToolPanelView="None"  /> 
</asp:Content>
