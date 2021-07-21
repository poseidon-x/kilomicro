<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="invoiceLoan.aspx.cs" Inherits="coreERP.ln.reports3.invoiceLoan" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Invoice Loans By Supplier
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div style="text-align:center">
        <table style="padding-left:100px">
            <tr>
                <td align="right" style="font-weight:bold">
                    Start Date:
                </td>
                <td align="left">
                    <telerik:RadDateTimePicker runat="server" ID="dtpStartDate"  
                        Culture="English (United States)" DateInput-DateFormat="dd-MMM-yyyy"> 
                    </telerik:RadDateTimePicker>
                </td>
                <td align="right" style="font-weight:bold">
                    End Date:
                </td>
                <td align="left">
                    <telerik:RadDateTimePicker runat="server" ID="dtpEndDate"  
                        Culture="English (United States)" DateInput-DateFormat="dd-MMM-yyyy"> 
                    </telerik:RadDateTimePicker>
                </td>
            </tr> 
            <tr>
                <td>Client</td>
                <td>
                <telerik:RadComboBox ID="cboClient" runat="server"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number" ></telerik:RadComboBox>
                </td>
                <td align="right" style="font-weight:bold">
                   Outstanding Only
                </td>
                <td align="left">
                    <asp:CheckBox runat="server" ID="chkOutstanding" />
                </td>
            </tr> 
            <tr>
                <td>Supplier</td>
                <td>
                <telerik:RadComboBox ID="cboSupplier" runat="server" 
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name of supplier" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
                </td> 
                <td>Branch</td>
                <td>
                    <telerik:RadComboBox runat="server" ID="cboBranch" Width="300" MarkFirstMatch="true" AutoCompleteSeparator=""
                         ToolTip="Select a branch to limit report to only that branch/cost center"
                         EmptyMessage="Select a branch"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>Assigned Staff</td>
                <td>
                    <telerik:RadComboBox ID="cboStaff" runat="server"
                        DropDownWidth="355px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true" Width="300"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
                </td> 
                <td></td>
                <td>
                    <asp:RadioButton runat="server" Text="All Transactions" ID="opt1" Checked="true" GroupName="Opt" />
                    <asp:RadioButton runat="server" Text="Only with Pmts" ID="opt2" GroupName="Opt" />
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
