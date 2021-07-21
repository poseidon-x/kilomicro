<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="savings.aspx.cs" Inherits="coreERP.ln.savingReports.savings" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Detailed Regular Deposit Balances Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Detailed Regular Deposit Balances Report</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                End Date:
            </div>
            <div class="subFormInput">
                <telerik:raddatetimepicker runat="server" id="dtpEndDate"
                    culture="English (United States)">
<Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>

<TimeView CellSpacing="-1" TimeFormat="H:mm"></TimeView>

<TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>

<DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>

<DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy"></DateInput>
                    </telerik:raddatetimepicker>
            </div>

        </div>


        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Client
            </div>
            <div class="subFormInput">
                <telerik:radcombobox id="cboClient" runat="server"
                    dropdownautowidth="Enabled" emptymessage="Type name or account number of client" width="300px" highlighttemplateditems="true"
                    markfirstmatch="true" autocompleteseparator=""
                    enableloadondemand="true" onitemsrequested="cboClient_ItemsRequested"
                    loadingmessage="Loading client data: type name or account number"
                    tooltip="Begin typing any of the surname, other names or account number of the client to process"></telerik:radcombobox>
            </div>


            <div class="subFormLabel">
                Branch
            </div>
            <div class="subFormInput">
                <telerik:radcombobox runat="server" id="cboBranch" width="300" markfirstmatch="true" autocompleteseparator=""
                    tooltip="Select a branch to limit report to only that branch/cost center"
                    emptymessage="Select a branch"></telerik:radcombobox>
            </div>
        </div>

        
    </div>
    <div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Assing Staff
        </div>
        <div class="subFormInput">
            <telerik:radcombobox id="cboStaff" runat="server"
                dropdownwidth="355px" emptymessage="Select a Staff" highlighttemplateditems="true" cssclass="inputControl"
                markfirstmatch="true" autocompleteseparator="">
                    </telerik:radcombobox>
        </div>
        </div>
        <asp:RadioButton runat="server" Text="Type-1" ID="opt1" Checked="true" GroupName="Opt" />
        <asp:RadioButton runat="server" Text="Type-2" ID="opt2" GroupName="Opt" />
    </div>
    <div class="subFormInput" style="margin-top: 15px">
        <asp:Button runat="server" ID="btnRun" Text="Get Report"
            OnClick="btnRun_Click" />
    </div>
    <div>
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
