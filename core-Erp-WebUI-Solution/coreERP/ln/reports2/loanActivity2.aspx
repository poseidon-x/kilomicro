<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="loanActivity2.aspx.cs" Inherits="coreERP.ln.reports.loanActivity2" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Cummulative Loan Activity Report Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Cummulative Loan Activity</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                End Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDateTimePicker runat="server" CssClass="inputControl" ID="dtpEndDate"
                    Culture="English (United States)">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>

                    <TimeView CellSpacing="-1" TimeFormat="H:mm"></TimeView>

                    <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>

                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>

                    <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                </telerik:RadDateTimePicker>
            </div>
        </div>

        <div class="subFormColumnMiddle">
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
