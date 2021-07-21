<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="topnloans.aspx.cs" Inherits="coreERP.ln.bogreports.topnloans" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
MF7-A | Top N Loan Exposures
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Top Loan Exposures</h3>
    <div style="text-align:center">
        <table style="padding-left:100px"> 
            <tr>
                <td style="font-weight:bold">
                    End Date:
                </td>
                <td>
                    <telerik:RadDateTimePicker runat="server" ID="dtpEndDate">                     
                            <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                    </telerik:RadDateTimePicker>
                </td>
            </tr>
            <tr>
                <td>Number of Top Exposures</td>
                <td>
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtNum" NumberFormat-DecimalDigits="0"
                         Value="20"></telerik:RadNumericTextBox>
                </td>
            </tr>
            <tr>
                <td>Client</td>
                <td>
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number" ></telerik:RadComboBox>
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
