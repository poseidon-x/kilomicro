<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="assetRegister.aspx.cs" Inherits="coreERP.fa.reports.assetRegister" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
::Fixed Assets Register
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Fixed Assets Register</h3>
    <div style="text-align:center">
        <table style="padding-left:100px"> 
            <tr>
                <td>Asset Category</td>
                <td>
                    <telerik:RadComboBox ID="cboCat" runat="server"
                        DropDownWidth="355px" EmptyMessage="Select an asset category" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
                </td>
            </tr>  
            <tr>
                <td>Organizational Unit</td>
                <td>
                    <telerik:RadComboBox ID="cboOU" runat="server"
                        DropDownWidth="355px" EmptyMessage="Select an org unit" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
                </td>
            </tr>  
            <tr>
                <td>Staff</td>
                <td>
                    <telerik:RadComboBox ID="cboStaff" runat="server"
                        DropDownWidth="355px" EmptyMessage="Select a staff" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
                </td>
            </tr>  
            <tr>
                 <td colspan="2">
                    <asp:RadioButton runat="server" GroupName="CHeck" ID="chkCat" Checked="false"
                         Text="By Category" />
                    <asp:RadioButton runat="server" GroupName="CHeck" ID="chkOU" Checked="true"
                         Text="By Organizational Unit" />
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
