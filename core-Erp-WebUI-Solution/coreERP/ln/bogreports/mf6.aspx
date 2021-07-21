<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="mf6.aspx.cs" Inherits="coreERP.ln.bogreports.mf6" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
MF6 | BOG Reports
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>BOG Report</h3>
    <div style="text-align:center">
        <table style="padding-left:100px"> 
            <tr>
                <td style="font-weight:bold">
                    End Date:
                </td>
                <td >
                    <telerik:RadDateTimePicker runat="server" ID="dtpEndDate">                     
                            <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                    </telerik:RadDateTimePicker>
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
