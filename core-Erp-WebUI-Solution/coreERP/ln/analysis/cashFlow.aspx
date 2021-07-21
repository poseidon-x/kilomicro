<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="cashFlow.aspx.cs" Inherits="coreERP.ln.analysis.cashFlow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loans Cashflow
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loans &amp; Deposits Cashflow Analysis</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Start Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtStart" DateInput-DateFormat="dd-MMM-yyyy">
                </telerik:RadDatePicker>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                End Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtEnd" DateInput-DateFormat="dd-MMM-yyyy">
                </telerik:RadDatePicker>
            </div>
        </div>
    </div>
    <div class="subForm">
        <telerik:RadButton runat="server" ID="btnLoad" OnClick="btnLoad_Click" Text="Load Data"></telerik:RadButton>
    </div>
    <telerik:RadPivotGrid runat="server" ID="pvg">
        <Fields>
            <telerik:PivotGridColumnField DataField="month" ></telerik:PivotGridColumnField>
        </Fields>
    </telerik:RadPivotGrid>
</asp:Content>
