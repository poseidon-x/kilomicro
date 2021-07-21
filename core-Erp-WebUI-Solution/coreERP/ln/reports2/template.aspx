<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="template.aspx.cs" Inherits="coreERP.ln.reports2.template" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Loans Repayment Schedule
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Client Loan Repayment Template</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Client
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" runat="server"
                    DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" CssClass="inputControl" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged">
                </telerik:RadComboBox>
            </div>
        </div>

        <div class="subFormColumnMiddle">

            <div class="subFormLabel">
                Loan ID
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboLoan" runat="server"
                    DropDownAutoWidth="Enabled" EmptyMessage="Type Loan ID" CssClass="inputControl" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="">
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Template Name
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboTemplate" CssClass="inputControl" runat="server"
                    DropDownWidth="355px" EmptyMessage="Select an Agreement Template" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="">
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
