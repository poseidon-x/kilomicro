<%@ Page Title="" Language="C#" MasterPageFile="~/setupPage.Master" AutoEventWireup="true" CodeBehind="transaction.aspx.cs" Inherits="coreERP.cu.edit.transaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Member Transaction
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server"> 
    <script src="../../scripts/app/CreditUnion/transaction.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3 class="pageBanner">Member Transaction</h3>  
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">Member</div>
            <div class="subFormInput"><div id="creditUnionMemberID" class="inputControl"></div></div>
            <div class="subFormLabel">Bank Account</div>
            <div class="subFormInput"><div id="bankID" class="inputControl"></div></div>
            <div class="subFormLabel">Number of Shares</div>
            <div class="subFormInput"><input id="numberOfShares" type="number" class="inputControl" /></div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">Mode of Payment</div>
            <div class="subFormInput"><div id="modeOfPaymentID" class="inputControl"></div></div>
            <div class="subFormLabel">Transaction Date</div>
            <div class="subFormInput"><input id="transactionDate" type="date" /></div>
            <div class="subFormLabel">Transaction Type</div>
            <div class="subFormInput"><div id="transactionType"></div></div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">Cheque Number</div>
            <div class="subFormInput"><input id="chequeNumber" class="inputControl" /></div>
            <div class="subFormLabel">Price per Share</div>
            <div class="subFormInput"><input id="sharePrice" readonly="true" class="inputControl" /></div>
            <div class="subFormLabel">Transaction Amount</div>
            <div class="subFormInput"><input id="amount" readonly="true" class="inputControl" /></div>
        </div>
    </div>
    <button type="button" id="save" title="Save Transaction">Save Transaction</button>
</asp:Content>
