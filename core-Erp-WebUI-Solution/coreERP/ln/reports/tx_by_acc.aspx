<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="tx_by_acc.aspx.cs" Inherits="coreERP.ln.reports.tx_by_acc" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Transactions By Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Client Account Transactions</h3>

    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Start Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDateTimePicker runat="server" CssClass="inputControl" ID="dtpStartDate"
                    Culture="English (United States)" MinDate="1900-01-01">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>

                    <TimeView CellSpacing="-1" TimeFormat="H:mm"></TimeView>

                    <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>

                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>

                    <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                </telerik:RadDateTimePicker>
            </div>
            <div class="subFormLabel">
                End Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDateTimePicker runat="server" CssClass="inputControl" ID="dtpEndDate"
                    Culture="English (United States)" MinDate="1900-01-01">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>

                    <TimeView CellSpacing="-1" TimeFormat="H:mm"></TimeView>

                    <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>

                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>

                    <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                </telerik:RadDateTimePicker>
            </div>
        </div>

        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Foreign Currency
            </div>
            <div class="subFormInput">
                <asp:CheckBox ID="chkForeign" runat="server" CssClass="inputControl" Checked="false" />
            </div>
            <div class="subFormLabel">
                Currency
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboCur" runat="server" Height="150px" CssClass="inputControl"
                    DropDownWidth=" 255px" EmptyMessage="Transaction Currency" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    DataTextField="major_name" DataValueField="currency_id">
                    <HeaderTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 80px;">Cur Sym</td>
                                <td style="width: 150px;">Cur Name</td>
                                <td style="width: 80px;">Rate</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 80px;">
                                    <%# Eval("major_symbol")%>
                                </td>
                                <td style="width: 150px;">
                                    <%# Eval("major_name")%>
                                </td>
                                <td style="width: 80px;">
                                    <%# Eval("current_buy_rate")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Account
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboAcc" runat="server" Height="150px" CssClass="inputControl"
                    DropDownWidth="455px" EmptyMessage="Transaction Account" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" DataTextField="acc_name" DataValueField="acct_id"
                    AppendDataBoundItems="true">
                    <HeaderTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 80px;">Acc Num</td>
                                <td style="width: 250px;">Acc Name</td>
                                <td style="width: 80px;">Currency</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 255px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 80px;">
                                    <%# Eval("acc_num")%>
                                </td>
                                <td style="width: 250px;">
                                    <%# Eval("acc_name")%>
                                </td>
                                <td style="width: 80px;">
                                    <%# Eval("major_name")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Batch No.
            </div>
            <div class="subFormInput">
                <asp:TextBox ID="txtBatchNo" runat="server" CssClass="inputControl" MaxLength="15"></asp:TextBox>
            </div>
            <div class="subFormLabel">
                Cost Center
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboCC" runat="server" Height="200px" CssClass="inputControl"
                    DropDownWidth=" 155px" EmptyMessage="Cost Center" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    DataTextField="ou_name" DataValueField="ou_id">
                    <HeaderTemplate>
                        <table style="width: 155px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 150px;">Cost Center</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 155px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 150px;">
                                    <%# Eval("ou_name")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Ref No.
            </div>
            <div class="subFormInput">
                <asp:TextBox ID="txtRefNo" runat="server" CssClass="inputControl" MaxLength="15"></asp:TextBox>
            </div>

        </div>

        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Client
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" runat="server"
                    DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" CssClass="inputControl" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested"
                    LoadingMessage="Loading client data: type name or account number">
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Loan Category.
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboLoanType" runat="server" Height="200px" CssClass="inputControl"
                    DropDownWidth=" 255px" EmptyMessage="Please select a loan category" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="">
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
            <div class="subFormInput">
                <asp:RadioButton runat="server" ID="rbDetailed" CssClass="inputControl" Text="Detailed" GroupName="Options" AutoPostBack="true" EnableViewState="true" OnCheckedChanged="rbDetailed_CheckedChanged" />
                <asp:RadioButton runat="server" ID="rbGrouped" CssClass="inputControl" Text="Grouped" GroupName="Options" AutoPostBack="true" EnableViewState="true" OnCheckedChanged="rbDetailed_CheckedChanged" />
                <asp:RadioButton runat="server" ID="rbFiltered" CssClass="inputControl" Text="Filter Unbalanced" GroupName="Options" AutoPostBack="true" EnableViewState="true" OnCheckedChanged="rbDetailed_CheckedChanged" />
            </div>
            <div class="subFormInput">
                <asp:Button runat="server" ID="btnRun" CssClass="inputControl" Text="Get Report"
                    OnClick="btnRun_Click" />
            </div>
        </div>

    </div>
    <CR:CrystalReportViewer ID="rvw" HasToggleGroupTreeButton="true" AutoDataBind="true" runat="server"
        BorderStyle="Groove" BorderColor="#0033CC" HasCrystalLogo="False"
        OnDataBinding="rvw_DataBinding" ToolPanelView="None" />
</asp:Content>
