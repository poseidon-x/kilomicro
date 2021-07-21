<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="cashierClientServiceCharges.aspx.cs" Inherits="coreERP.ln.reports.cashierClientServiceCharges" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Cashier's Till
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Cashier's Till</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Cashier:
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" CssClass="inputControl" DropDownAutoWidth="Enabled" MarkFirstMatch="true"
                    AutoCompleteSeparator=" " ID="cboUserName">
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Field Agent (Staff):
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" CssClass="inputControl" DropDownAutoWidth="Enabled" MarkFirstMatch="true"
                    AutoCompleteSeparator=" " ID="cboFieldAgent">
                </telerik:RadComboBox>
            </div>
        </div>

        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Start Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtpStartDate" 
                    Culture="English (United States)" DateInput-DateFormat="dd-MMM-yyyy" >
                </telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                <asp:Button runat="server" ID="btnRunAll" CssClass="inputControl" Text="Get Report (All)"
                    OnClick="btnRunAll_Click" />
            </div>
            <div class="subFormInput">
                <asp:Button runat="server" ID="btnRun" CssClass="inputControl" Text="Get Report (Posted)"
                    OnClick="btnRun_Click" />
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                End Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtpEndDate"
                    Culture="English (United States)" DateInput-DateFormat="dd-MMM-yyyy" >
                </telerik:RadDatePicker>
            </div>
        </div>
    </div>
    <div>
        <div id="divProc" runat="server" style="visibility: hidden">
        </div>
        <div id="divError" runat="server" style="visibility: hidden">
            <span id="spanError" class="error" runat="server"></span>
        </div>
    </div>

    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server"
        BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False"
        OnDataBinding="rvw_DataBinding" ToolPanelView="None"  />
</asp:Content>
