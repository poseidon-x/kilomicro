<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="loanBalances.aspx.cs" Inherits="coreERP.ln.reports.loanBalances" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Loan Balance by Product Category
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Balance by Product Category</h3>
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
            <div class="subFormLabel">
                Expiry
            </div>
            <div class="subFormInput">
                <asp:RadioButton runat="server" CssClass="inputControl" ID="rbAll" Checked="true" GroupName="Opt" Text="All"   
                     AutoPostBack="true" OnCheckedChanged="rbAll_CheckedChanged" CausesValidation="false" />
                <asp:RadioButton runat="server" CssClass="inputControl" ID="rbExpired" Checked="false" GroupName="Opt" Text="Expired"
                     AutoPostBack="true" OnCheckedChanged="rbExpired_CheckedChanged" CausesValidation="false" />
                <asp:RadioButton runat="server" CssClass="inputControl" ID="rbRunning" Checked="false" GroupName="Opt" Text="Running"   
                     AutoPostBack="true" OnCheckedChanged="rbRunning_CheckedChanged" CausesValidation="false" />
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
                Branch
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboBranch" CssClass="inputControl" MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select a branch to limit report to only that branch/cost center"
                    EmptyMessage="Select a branch">
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Performance
            </div>
            <div class="subFormInput">
                <asp:RadioButton runat="server" CssClass="inputControl" ID="rbDetail2" Checked="true" GroupName="Opt6" Text="All"   
                     AutoPostBack="true" OnCheckedChanged="rbDetail_CheckedChanged" CausesValidation="false" />
                <asp:RadioButton runat="server" CssClass="inputControl" ID="rbSummary2" Checked="false" GroupName="Opt6" Text="Delinquent"
                     AutoPostBack="true" OnCheckedChanged="rbSummary_CheckedChanged" CausesValidation="false" />
                <asp:RadioButton runat="server" CssClass="inputControl" ID="rbComplete2d" Checked="false" GroupName="Opt6" Text="Performing"   
                     AutoPostBack="true" OnCheckedChanged="rbCompleted_CheckedChanged" CausesValidation="false" />
            </div>
            <div class="subFormInput">
                <asp:Button runat="server" ID="btnRun" CssClass="inputControl" Text="Generate Report"
                    OnClick="btnRun_Click" />
            </div>
        </div>
            <div class="subFormColumnRight">
                <div class="subFormLabel">
                    Assigned Staff
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox ID="cboStaff" runat="server"
                        DropDownWidth="355px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true" CssClass="inputControl"
                        MarkFirstMatch="true" AutoCompleteSeparator="">
                    </telerik:RadComboBox>
                </div>
                <div class="subFormLabel">
                    Detail Level
                </div>
                <div class="subFormInput">
                    <asp:RadioButton runat="server" CssClass="inputControl" Text="Summary" ID="opt1" Checked="true" GroupName="Opt7" />
                    <asp:RadioButton runat="server" CssClass="inputControl" Text="Detailed" ID="opt2" GroupName="Opt7" />
                </div>
                <div class="subFormLabel">
                   Repayment Status
                </div>
                <div class="subFormInput">
                    <asp:RadioButton runat="server" CssClass="inputControl" ID="rbAll2" Checked="true" GroupName="Opt3" Text="All"   
                         AutoPostBack="true" OnCheckedChanged="rbAll2_CheckedChanged" CausesValidation="false" />
                    <asp:RadioButton runat="server" CssClass="inputControl" ID="rbFull" Checked="false" GroupName="Opt3" Text="Full"
                         AutoPostBack="true" OnCheckedChanged="rbFull_CheckedChanged" CausesValidation="false" />
                    <asp:RadioButton runat="server" CssClass="inputControl" ID="rbPartial" Checked="false" GroupName="Opt3" Text="Partial"   
                         AutoPostBack="true" OnCheckedChanged="rbPartial_CheckedChanged" CausesValidation="false" />
                    <asp:RadioButton runat="server" CssClass="inputControl" ID="rbUnpaid" Checked="false" GroupName="Opt3" Text="Unpaid"   
                         AutoPostBack="true" OnCheckedChanged="rbUnpaid_CheckedChanged" CausesValidation="false" />
                    <asp:RadioButton runat="server" CssClass="inputControl" ID="rbNone" Checked="false" GroupName="Opt3" Text="None"   
                         AutoPostBack="true" OnCheckedChanged="rbNone_CheckedChanged" CausesValidation="false" />
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
