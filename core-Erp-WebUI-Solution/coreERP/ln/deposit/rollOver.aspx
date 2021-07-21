<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="rollOver.aspx.cs" Inherits="coreERP.ln.deposit.rollOver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Rollover Client Investment
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Rollover Client Investment</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                 Select Client
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Amount Invested
            </div>
            <div class="subFormInput">
                <asp:Label ID="lblAmount" runat="server"></asp:Label>
            </div>
            <div class="subFormLabel">
                 Maturity Date
            </div>
            <div class="subFormInput">
                <asp:Label ID="lblMaturityDate" runat="server"></asp:Label>
            </div>
            <div class="subFormLabel">
                 Interest Balance
            </div>
            <div class="subFormInput">
                <asp:Label ID="lblInte" runat="server"></asp:Label>
            </div>
            <div class="subFormLabel">
                 New Deposit Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtAppDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                 Principal Repayment Frequency
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboPrincRepaymentMode" CssClass="inputControl" runat="server"></telerik:RadComboBox>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                 Select Deposit
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboDeposit" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboDeposit_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 First Deposit Date
            </div>
            <div class="subFormInput">
                <asp:Label ID="lblDepDate" runat="server"></asp:Label>
            </div>
            <div class="subFormLabel">
                 Principal Balance
            </div>
            <div class="subFormInput">
                <asp:Label ID="lblPrinc" runat="server"></asp:Label>
            </div>
            <div class="subFormLabel">
                 New Interest Rate (per Annum)
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtRate" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Period of Deposit
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboPeriod" runat="server" CssClass="inputControl" AutoPostBack="true"
                    DropDownWidth="355px" EmptyMessage="Select deposit period" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select deposit period" ></telerik:RadComboBox>
            </div>
            <!--<div class="subFormLabel">
                 New Period (months)
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtPeriod" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
            </div>
                -->
            <div class="subFormLabel">
                 Interest Repayment Frequency
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboInterestRepaymentMode" CssClass="inputControl" runat="server"></telerik:RadComboBox>
            </div>
        </div>
    </div>
    <div class="subForm">
        <telerik:RadButton ID="btnRollOverAll" runat="server" Text="Rollover Principal + Interest" Enabled="false" OnClick="btnRollOverAll_Click"></telerik:RadButton>
        <telerik:RadButton ID="btnRollOverPrinc" runat="server" Text="Rollover Principal" Enabled="false" OnClick="btnRollOverPrinc_Click"></telerik:RadButton>
        <telerik:RadButton ID="btnRollOverInt" runat="server" Text="Rollover Interest" Enabled="false" OnClick="btnRollOverInt_Click"></telerik:RadButton>
    </div>    
</asp:Content>
