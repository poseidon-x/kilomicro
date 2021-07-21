<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="deposits.aspx.cs" Inherits="coreERP.ln.depositReports.deposits" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Detailed Client Investment Balances Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Detailed Client Investment Balances Report</h3>
    <div style="text-align:center">
        <table style="padding-left:100px">
            <tr>
                <td  style="font-weight:bold">
                    Satrt Date:
                </td>
                <td >
                    <telerik:RadDatePicker ID="dtpStartDate" Runat="server">
                                </telerik:RadDatePicker>
                </td>
                <td  style="font-weight:bold">
                    End Date:
                </td>
                <td >
                    <telerik:RadDatePicker ID="dtpEndDate" Runat="server">
                                </telerik:RadDatePicker>
                </td>
                <td></td>
            </tr>
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
                <td>
                    &nbsp;</td>
            </tr>   
            <tr>
                <td>Branch</td>
                <td>
                    <telerik:RadComboBox runat="server" ID="cboBranch" Width="300" MarkFirstMatch="true" AutoCompleteSeparator=""
                         ToolTip="Select a branch to limit report to only that branch/cost center"
                         EmptyMessage="Select a branch"></telerik:RadComboBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Assing Staff</td>
                <td>
                    <telerik:RadComboBox ID="cboStaff" runat="server"
                        DropDownWidth="355px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true" CssClass="inputControl"
                        MarkFirstMatch="true" AutoCompleteSeparator="">
                    </telerik:RadComboBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:RadioButton runat="server" Text="Type-1" ID="opt1" GroupName="Opt"
                        AutoPostBack="true" OnCheckedChanged="opt1_CheckedChanged" />
                    <asp:RadioButton runat="server" Text="Type-2" ID="opt2" Checked="true" GroupName="Opt" 
                        AutoPostBack="true" OnCheckedChanged="opt2_CheckedChanged"/>
                </td>
                <td>
                    &nbsp;</td>
            </tr> 
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnRun" Text="Get Report" 
                        onclick="btnRun_Click" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>  
                        <tr>
                            <td colspan="2">              
                                <div id="divProc" runat="server" style="visibility:hidden">
                                </div>
                                <div id="divError" runat="server" style="visibility:hidden">
                                    <span id="spanError" class="error" runat="server"></span>
                                </div>            
                            </td>
                            <td>              
                                
                            </td>
                        </tr>
           </table>
    </div>
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server" 
            BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" 
            ondatabinding="rvw_DataBinding" ToolPanelView="None"  />
</asp:Content>
