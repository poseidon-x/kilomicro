<%@ Page Title="" Language="C#" MasterPageFile="~/setupPage.Master" AutoEventWireup="true" CodeBehind="walletLoading.aspx.cs" Inherits="coreERP.momo.wallet.walletLoading" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Load Up Mobile Money Wallet
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server"> 
    <script src="../../scripts/app/MobileMoney/walletLoading.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3 class="pageBanner">Load Up Mobile Money Wallet</h3>  
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">Wallet</div>
            <div class="subFormInput"><div id="walletID" class="inputControl"></div></div>
            <div class="subFormLabel">Bank Account</div>
            <div class="subFormInput"><div id="bankID" class="inputControl"></div></div>
            <div class="subFormLabel">Amount Loaded</div>
            <div class="subFormInput"><input id="amountLoaded" type="number" /></div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">Mode of Payment</div>
            <div class="subFormInput"><div id="modeOfPaymentID" class="inputControl"></div></div>
            <div class="subFormLabel">Loading Date</div>
            <div class="subFormInput"><div id="loadingDate"></div></div>
        </div>
    </div>
    <button type="button" id="save" title="Save Loaded Wallet">Save Loaded Wallet</button>
</asp:Content>
