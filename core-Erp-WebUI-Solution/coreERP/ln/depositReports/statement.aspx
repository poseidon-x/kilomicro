<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="statement.aspx.cs" Inherits="coreERP.ln.depositReports.statement" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Client Investment Statement of Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Client Investment Statement of Account</h3>
    <div style="text-align:center">
        <table style="padding-left:100px">
            <tr>
                <td>Client</td>
                <td>
                <telerik:RadComboBox ID="cboClient" runat="server"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" 
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number"
                    ToolTip="Begin typing any of the surname, other names or account number of the client to process" ></telerik:RadComboBox>
                </td>
                <td style="padding:0 50px"><b>End Date:</b></td>
                <td>
                    <telerik:RadDatePicker runat="server" ID="dpkEndDate" Width="200px" DateInput-DateFormat="dd-MMM-yyyy" 
                        DateInput-DisplayDateFormat="dd-MMM-yyyy" ></telerik:RadDatePicker>
                </td>
                <td></td>
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
