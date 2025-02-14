﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="outstanding.aspx.cs" Inherits="coreERP.ln.reports.outstanding" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Loans Outstanding Payments
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Outstanding Loans Payments</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                End Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDateTimePicker runat="server" CssClass="inputControl" ID="dtpEndDate"
                    AutoPostBackControl="Both"
                    Culture="English (United States)">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>

                    <TimeView CellSpacing="-1" TimeFormat="H:mm"></TimeView>

                    <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>

                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>

                    <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                </telerik:RadDateTimePicker>
            </div>

            <%--<div class="subFormLabel">
                Loan Officer
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" CssClass="inputControl" DropDownAutoWidth="Enabled" MarkFirstMatch="true"
                    AutoCompleteSeparator=" " ID="cboOfficer">
                </telerik:RadComboBox>
            </div>--%>

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
        </div>

        <div class="subFormColumnRight">
            <div class="subFormInput">
                <asp:RadioButton Text="Expired" runat="server" CssClass="inputControl" GroupName="Exp" Checked="true" ID="optExpired"
                    AutoPostBack="true" OnCheckedChanged="optExpired_CheckedChanged" />
                <asp:RadioButton Text="Not Expired" runat="server" CssClass="inputControl" GroupName="Exp" ID="optNotExpired"
                    AutoPostBack="true" OnCheckedChanged="optExpired_CheckedChanged" />
                <asp:RadioButton Text="All" runat="server" CssClass="inputControl" GroupName="Exp" ID="optAll"
                    AutoPostBack="true" OnCheckedChanged="optExpired_CheckedChanged" />
            </div>
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
