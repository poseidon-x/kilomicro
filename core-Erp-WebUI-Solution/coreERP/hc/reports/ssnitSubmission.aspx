<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="ssnitSubmission.aspx.cs" Inherits="coreERP.hc.reports.ssnitSubmission" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
SSNIT SUBMISSION REPORT
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div style="text-align:center">
        <table style="padding-left:100px">
            <tr>
                <td>Pay Calendar Month</td>
                <td>
                    <telerik:RadComboBox ID="cboPayCalendar" runat="server"
                        DropDownWidth="355px" EmptyMessage="Select a pay calendar month" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" DropDownAutoWidth="Enabled" Width="300px"></telerik:RadComboBox>
                </td>
            </tr> 
            <tr>
                <td>Branch</td>
                <td>
                    <telerik:RadComboBox ID="cboBranch" runat="server" 
                        DropDownWidth="355px" EmptyMessage="Select a Branch" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" DropDownAutoWidth="Enabled" Width="300px"></telerik:RadComboBox>
                </td>
            </tr> 
            <tr>
                <td>Staff</td>
                <td>
                    <telerik:RadComboBox ID="cboClient" runat="server"
                        DropDownWidth="355px" EmptyMessage="Select a staff" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" DropDownAutoWidth="Enabled" Width="300px"></telerik:RadComboBox>
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
