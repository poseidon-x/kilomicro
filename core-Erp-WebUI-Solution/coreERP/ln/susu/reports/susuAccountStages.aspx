<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="susuAccountStages.aspx.cs" Inherits="coreERP.ln.susu.reports.susuAccountStages" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Susu Account Stages
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Susu Account Stages</h3>
    <div style="text-align:center">
        <table style="padding-left:100px">  
            <tr>
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
                <td>Stage</td>
                <td>
                    <telerik:RadComboBox runat="server" ID="cboStage" Width="300px">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="All Stages" Selected="true" />
                            <telerik:RadComboBoxItem Value="1" Text="Stage 1A" />
                            <telerik:RadComboBoxItem Value="1.5" Text="Stage 1B" />
                            <telerik:RadComboBoxItem Value="2" Text="Stage 2" />
                        </Items>
                    </telerik:RadComboBox>
                </td> 
                <td>Status</td>
                <td>
                    <telerik:RadComboBox runat="server" ID="cboStatus" Width="300px">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="All Status" Selected="true" />
                            <telerik:RadComboBoxItem Value="1" Text="Dormant" />
                            <telerik:RadComboBoxItem Value="5" Text="Delayed - Not Collected" />
                            <telerik:RadComboBoxItem Value="2" Text="Okay - Not Collected" />
                            <telerik:RadComboBoxItem Value="3" Text="Okay - Collected" />
                            <telerik:RadComboBoxItem Value="4" Text="Not Okay - Collected" />
                            <telerik:RadComboBoxItem Value="6" Text="Exited Client" />
                        </Items>
                    </telerik:RadComboBox>
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
