<%@ Page Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="transaction.aspx.cs" Inherits="coreERP.ln.reports.transaction" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    .::EOD Transaction Report
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server"></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <br /><h3>EOD Transaction Report</h3><hr />
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel"><b>Branch:</b></div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboBranch" runat="server" AutoPostBack="false" Width="250"></telerik:RadComboBox>
            </div>
        </div>

        <div class="subFormColumnMiddle">
            <div class="subFormLabel"><b>Date:</b></div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dpkTransDate" runat="server" AutoPostBack="false" Width="200"></telerik:RadDatePicker>
            </div>
        </div>

        <div class="subFormColumnRight">
            <div class="subFormInput">
                <asp:Button runat="server" ID="btnRun" CssClass="inputControl" Text="Get Report" OnClick="btnRun_Click"/>
            </div>
        </div>

        <div>
            <div id="divProc" runat="server" style="visibility: hidden">
            </div>
            <div id="divError" runat="server" style="visibility: hidden">
                <span id="spanError" class="error" runat="server"></span>
            </div>
        </div>
    </div>

    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" OnDataBinding="rvw_DataBinding" 
        runat="server" ToolPanelView="None" BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False" />
    
</asp:Content>
