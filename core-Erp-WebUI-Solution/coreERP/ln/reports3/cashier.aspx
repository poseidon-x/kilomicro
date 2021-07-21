<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="cashier.aspx.cs" Inherits="coreERP.ln.reports3.cashier" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Cashier Transaction Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Cashier Transaction</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Start Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDateTimePicker runat="server" CssClass="inputControl" ID="dtpStartDate"
                    Culture="English (United States)" DateInput-DateFormat="dd-MMM-yyyy">
                </telerik:RadDateTimePicker>
            </div>
            <div class="subFormLabel">
                End Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDateTimePicker runat="server" CssClass="inputControl" ID="dtpEndDate"
                    Culture="English (United States)" DateInput-DateFormat="dd-MMM-yyyy">
                </telerik:RadDateTimePicker>
            </div>

        </div>

        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Name of Cashier
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboCashier" runat="server"
                    DropDownAutoWidth="Enabled" EmptyMessage="Select cashier to only h/er transactions" CssClass="inputControl">
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Branch
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboBranch" CssClass="inputControl" MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select a branch to limit report to only that branch/cost center"
                    EmptyMessage="Select a branch">
                </telerik:RadComboBox>
            </div>
        </div>

        <div class="subFormColumnRight">
            <div class="subFormInput">
                <asp:Button runat="server" ID="btnRun" CssClass="inputControl" Text="Get Report"
                    OnClick="btnRun_Click" />
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
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server"
        BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False"
        OnDataBinding="rvw_DataBinding" ToolPanelView="None" />
</asp:Content>
