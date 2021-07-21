<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="susuAccountSchedule.aspx.cs" Inherits="coreERP.ln.susu.reports.susuAccountSchedule" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Susu Account Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Susu Account Details</h3>
    <div style="text-align:center">
        <table style="padding-left:100px"> 
            <tr>
                <td>Client</td>
                <td>
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" 
                    MarkFirstMatch="true" AutoCompleteSeparator="" CausesValidation="false"
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number" ></telerik:RadComboBox>
                </td> 
                <td>Branch</td>
                <td>
                    <telerik:RadComboBox runat="server" ID="cboBranch" Width="300" MarkFirstMatch="true" AutoCompleteSeparator=""
                         ToolTip="Select a branch to limit report to only that branch/cost center"
                         EmptyMessage="Select a branch"></telerik:RadComboBox>
                </td>
            </tr>   
            <tr>
                <td>Start Date</td>
                <td>
                   <telerik:RadDatePicker runat="server" ID="dtStartDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                </td> 
                <td>End Date</td>
                <td>
                   <telerik:RadDatePicker runat="server" ID="dtEndDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td>Group Name</td>
                <td>
                    <telerik:RadComboBox ID="cboGroup" runat="server"></telerik:RadComboBox>
                </td>
                <td>Grade Name</td>
                <td>
                    <telerik:RadComboBox ID="cboGrade" runat="server"></telerik:RadComboBox>
                </td>
            </tr>   
            <tr> 
                <td>Position Name</td>
                <td>
                    <telerik:RadComboBox ID="cboPosition" runat="server"></telerik:RadComboBox>
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
