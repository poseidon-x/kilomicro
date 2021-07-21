<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="parDetails.aspx.cs" Inherits="coreERP.ln.reports4.parDetails" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loan Portfolio at Risk Report | Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Portfolio at Risk Report | Details</h3>
    <div class="subForm">
        <div class="subFormColumnLeft"> 
            <div class="subFormLabel">
                End Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDateTimePicker runat="server" CssClass="inputControl" ID="dtpEndDate"
                    Culture="English (United States)" DateInput-DateFormat="dd-MMM-yyyy">
                </telerik:RadDateTimePicker>
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
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Client
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true"
                    DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" CssClass="inputControl" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested"
                    LoadingMessage="Loading client data: type name or account number">
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Loan Product/Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboLoanType" CssClass="inputControl" MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select a product to limit report to only it"
                    EmptyMessage="Select a product">
                </telerik:RadComboBox>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Relationship Officer
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboStaff" runat="server"
                    DropDownWidth="355px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true" CssClass="inputControl"
                    MarkFirstMatch="true" AutoCompleteSeparator="">
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Display As
            </div>
            <div class="subFormInput">
                <asp:RadioButton runat="server" CssClass="inputControl" Text="Detailed List" ID="rdbDetail" Checked="true" GroupName="Opt2" OnCheckedChanged="rdbDetail_CheckedChanged" AutoPostBack="true"/>
                <asp:RadioButton runat="server" CssClass="inputControl" Text="Summary by Branch" ID="rdbSummary" GroupName="Opt2" OnCheckedChanged="rdbSummary_CheckedChanged" AutoPostBack="true" />
            </div>
        </div>

            <div> 
                <div>
                    <asp:Button runat="server" ID="btnRun" Text="Get Report"
                        OnClick="btnRun_Click" />
                </div>
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
