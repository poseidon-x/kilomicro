/*
UI Scripts for Loan savingType Management
Creator: man@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed

"use strict";
var authToken = coreERPAPI_Token;
var cashierReceiptWithCheckApiUrl = coreERPAPI_URL_Root + "/crud/CashierReceiptWithCheck";
var loanApiUrl = coreERPAPI_URL_Root + "/crud/ClientLoan";
var loanTypeApiUrl = coreERPAPI_URL_Root + "/crud/loanType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var banksApiUrl = coreERPAPI_URL_Root + "/crud/banks";
var modeOfPaymentApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var repaymentTypeApiUrl = coreERPAPI_URL_Root + "/crud/repaymentType";
var currencyNoteApiUrl = coreERPAPI_URL_Root + "/crud/currencyNote";
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/cashierTill";


var cashierReceipt = {};
var loanTypes = {};
var clients = {};
var loan = {};
var loanAccounts = {};
var currentLoan = {}; 
var currencies = {};
var banks = {};
var modeOfpayments = {};
var repaymentTypes = {};
var currencyNotes = {};
var filteredClients = [];
var notes = [];
var remainingNotes = [];
var coins = [];
var remainingCoins = [];
var cashierFunds = {};
var cashierFundToday = {};

var noteAdditionalId = -1;
var coinAdditionalId = -1;

var cashierReceiptAjax = $.ajax({
    url: cashierReceiptWithCheckApiUrl + '/Get/' + id,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var loanAjax = $.ajax({
    url: loanApiUrl + '/GetLoan/' + lnId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var loanTypeAjax = $.ajax({
    url: loanTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var currencyAjax = $.ajax({
    url: currencyApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var bankAjax = $.ajax({
    url: banksApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var paymentModeAjax = $.ajax({
    url: modeOfPaymentApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var repaymentTypeAjax = $.ajax({
    url: repaymentTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var currencyNoteAjax = $.ajax({
    url: currencyNoteApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var cashierFundAjax = $.ajax({
    url: cashierTillApiUrl + '/GetCashierFunds',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var cashierFundTodayAjax = $.ajax({
    url: cashierTillApiUrl + '/GetCashierFundToday',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
//Load page data
function loadData() {	
    $.when(cashierReceiptAjax,loanAjax,loanTypeAjax,currencyAjax,bankAjax,paymentModeAjax,
	repaymentTypeAjax,currencyNoteAjax,cashierFundAjax,cashierFundTodayAjax)
        .done(function (dataCashierReceipt,dataLoan,dataLoanType,dataCurrency,dataBank,dataPaymentMode,
		dataRepaymentType,dataCurrencyNote,dataCashierFund,dataCashierFundToday) {
            cashierReceipt = dataCashierReceipt[2].responseJSON;
			loanTypes = dataLoanType[2].responseJSON;
            currentLoan = dataLoan[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
			banks = dataBank[2].responseJSON;
            modeOfpayments = dataPaymentMode[2].responseJSON;
            repaymentTypes = dataRepaymentType[2].responseJSON;			
			currencyNotes = dataCurrencyNote[2].responseJSON;
            cashierFunds = dataCashierFund[2].responseJSON;
			cashierFundToday = dataCashierFundToday[2].responseJSON;
			getCurrencyTypes(currencyNotes);
		
			dismissLoadingDialog();

			//Prepare Ui if Login cashier has been funded for today.
			if(cashierFundToday!=null){
				//Prepares UI
				prepareUi();
				getCashierNotes();
				getCashierCoins();
				
				if(currentLoan.loanId > 0){
					//cashierReceipt.saving = currentSaving;
					setUpAccountDeatils();
				}else{
					allowClientSearch()
				}
			}else{
				successDialog("The login cashier has not been funded for Today, Make sure you have funds before you revisit this page","Note",
				function () { window.location = '/dash/home.aspx'; });
			}
        }
	);
}
$(function () {
    displayLoadingDialog();
    loadData();
});

function prepareUi(){
	//Render Controls
	renderControls();
	
    $('#save').click(function (event) {
		if (confirm("Are you sure you want receive Loan repayment")) {
            displayLoadingDialog();
			saveReceipt();				
		} else
		{
            smallerWarningDialog('Please review and receive later', 'NOTE');
        }
	});
}

//Apply kendo Style to the input fields
function renderControls() {
	$("#tabs").kendoTabStrip();
	$("#account").width("90%")
	.kendoComboBox({
		dataSource: loanAccounts,
		filter: "contains",
		suggest: true,
		dataValueField: "loanId",
		dataTextField: "loanNumber",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onAccountChange,
		optionLabel: ""
	});
    $("#interestBal").width("90%")
	.kendoNumericTextBox();
	$("#balance").width("90%")
	.kendoNumericTextBox();
	$("#repaymentDate").width("90%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$("#paymentType").width("90%")
	.kendoComboBox({
		dataSource: modeOfpayments,
		filter: "contains",
		suggest: true,
		dataValueField: "ID",
		dataTextField: "Description",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onPaymentTypeChange,
		optionLabel: ""
	});
	$("#repaymentType").width("90%")
	.kendoComboBox({
		dataSource: repaymentTypes,
		filter: "contains",
		suggest: true,
		dataValueField: "repaymentTypeID",
		dataTextField: "repaymentTypeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#amount").width("90%")
	.kendoNumericTextBox();
	$("#checkNo").width("90%")
	.kendoMaskedTextBox();
	$("#bank").width("90%")
	.kendoComboBox({
		dataSource: banks,
		filter: "contains",
		suggest: true,
		dataValueField: "bankId",
		dataTextField: "bankName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	dismisChequeDetails();
	$("#tabs").hide();
	$("#save").hide();	
}
function allowClientSearch(){
	$("#client").keyup(function(){
		var txt = $("#client").data("kendoMaskedTextBox").value();
		
		if(txt.length > 2){
			var input = {searchString:txt};
			getClients(input);
		}
	});
	
	$("#client").width("90%")
	.kendoMaskedTextBox();
}
function setUpAccountDeatils(){
	var lnacc = $("#account").data("kendoComboBox");
	getClientPicture(currentLoan.clientID);
	populateUi();
	var l = {
		loanId:currentLoan.loanId,
		loanNumber:currentLoan.loanNumber
	};
	var client = currentLoan.client;
	var cln = {
		clientID:currentLoan.clientID,
		clientName:client.surName+" "+client.otherNames
	};
	var loanAccou = [];
	loanAccou.push(l);
	lnacc.setDataSource(loanAccou);
	lnacc.value(currentLoan.loanID);
	filteredClients.push(cln);
	setUpClientCombox();
	$("#client").data("kendoComboBox").value(client.clientID);
	document.getElementById("client").readOnly = true;
	document.getElementById("account").readOnly = true;
}


var onPaymentTypeChange = function(){
	var payTyId = $("#paymentType").data("kendoComboBox").value();
	var exist = false;	
	//Retrieve value enter validate
    for (var i = 0; i < modeOfpayments.length; i++) {
        if (modeOfpayments[i].ID == payTyId) { 
			if(payTyId==1)
			{setUpCurrencies();}
			else if(payTyId)
			{displayChequeDetails();}
			$("#save").show();
            exist = true;
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Payment Type', 'ERROR');
        $("#paymentType").data("kendoComboBox").value("");
    }
}
function populateUi(){
	$("#save").show();
	$("#interestBal").data("kendoNumericTextBox").value(loan.interestBalance);
	$("#balance").data("kendoNumericTextBox").value(loan.balance);
	$("#repaymentType").data("kendoComboBox").value(1);
	$("#repaymentDate").data("kendoDatePicker").value(new Date());
}
//Reset controls of client or account
function resetControls(){
	$("#amount").data("kendoNumericTextBox").value("");	
	$("#tabs").hide();
	$("#save").hide();
	$("#interestBal").data("kendoNumericTextBox").value("");
	$("#repaymentType").data("kendoComboBox").value("");
	$("#balance").data("kendoNumericTextBox").value("");
	$("#repaymentDate").data("kendoDatePicker").value("");
	$("#paymentType").data("kendoComboBox").value("");
	$("#repaymentType").data("kendoComboBox").value("");
	$("#checkNo").data("kendoMaskedTextBox").value("");
	$("#bank").data("kendoComboBox").value("");
	$("#chequeDetails").hide();
}
function setUpCurrencies(){
	$("#tabs").show();
	dismisChequeDetails();
	document.getElementById("amount").readOnly = true;
	renderGrid();
}
function displayChequeDetails(){
	$("#tabs").hide();
	document.getElementById("amount").readOnly = false;
	$("#checkNo").data("kendoMaskedTextBox").value("");
	$("#bank").data("kendoComboBox").value("");
	$("#chequeDetails").show();	
}
function dismisChequeDetails(){
	$("#checkNo").data("kendoMaskedTextBox").value("");
	$("#bank").data("kendoComboBox").value("");
	$("#chequeDetails").hide();	
}
function renderGrid(){
	$('#notes').kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
							entries.success(cashierReceipt.cashierReceiptNotes);                    
					},
					create: function (entries) {
						var data = entries.data;						
						data.cashierReceiptNoteId = noteAdditionalId;
						noteAdditionalId += -1;
						data.quantityReceived = $("#quantityDep").data("kendoNumericTextBox").value();
						data.quantityBD = $("#quantityDB").data("kendoNumericTextBox").value();						
						data.quantityCD = $("#quantityCD").data("kendoNumericTextBox").value();
						data.totalReceived = $("#totDep").data("kendoNumericTextBox").value();
						removeAddedNote(data.currencyNoteId);
						entries.success(entries.data);
						getTotalDeposit();
					},
					update: function (entries) {				
						var data = entries.data;
						removeAddedNote(data.currencyNoteId);
						entries.success(entries.data);
						getTotalDeposit();
					},
					destroy: function (entries) {
						var data = entries.data;
						addNoteBack(data.currencyNoteId);
						entries.success(entries.data);
						getTotalDeposit();
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'cashierReceiptNoteId',
                    fields: {
						cashierReceiptNoteId: { type: 'number' },
						cashierReceiptID: { type: 'number' },
						currencyNoteId: { editable: true, validation: { required: true } },
						quantityReceived: { type: 'number', validation: { required: true, min: 1 } },
						quantityBD: { type: 'number', editable:true,validation: { required: true } },
						quantityCD: { type: 'number', editable:true,validation: { required: true } },
						totalReceived: { type: 'number', editable:true,validation: { required: true } }
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: 'Notes (GHS)', editor: notesEditor, template: '#= getNote(currencyNoteId) #' },
            { field: 'quantityReceived', title: 'Pieces Received', editor: quantityDepositedEditor, format: "{0: #,###}" },  
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}" },            			
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}" },            
            { field: 'totalReceived', title: 'Amount Received (GHS)',editor: totalDepositedEditor, format: "{0: #,###.#0}" },            
            { command: ['edit','destroy'],width:200 }
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		toolbar: [
			{
				name: "create",
				text: 'Receive Notes',
			}
		],
		editable: "inline",
		edit: function(e) {
			var data = e.model;
			if (data.currencyNoteId > 0) {
				addNoteBack(data.currencyNoteId);
			   //this.closeCell();
			}
			//e.preventDefault();
			
		  },
		mobile: true
    });
	
	
	$('#coins').kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
							entries.success(cashierReceipt.cashierReceiptCoins);                    
					},
					create: function (entries) {
						var data = entries.data;
						data.cashierReceiptCoinId = coinAdditionalId;
						coinAdditionalId += -1;
						data.quantityReceived = $("#quantityDep").data("kendoNumericTextBox").value();
						data.quantityCD = $("#quantityCD").data("kendoNumericTextBox").value();
						data.quantityBD = $("#quantityDB").data("kendoNumericTextBox").value();
						data.totalReceived = $("#totDep").data("kendoNumericTextBox").value();
						removeAddedCoin(data.currencyNoteId);
						entries.success(entries.data);
						getTotalDeposit();
					},
					update: function (entries) {
						var data = entries.data;
						removeAddedCoin(data.currencyNoteId);
						entries.success(entries.data);
						getTotalDeposit();
					},
					destroy: function (entries) {
						var data = entries.data;
						addCoinBack(data.currencyNoteId);
						entries.success(entries.data);
						getTotalDeposit();
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'cashierReceiptCoinId',
                    fields: {
						cashierReceiptCoinId: { type: 'number' },
						cashierReceiptID: { type: 'number' },
						currencyNoteId: { editable: true, validation: { required: true } },
						quantityReceived: { type: 'number', validation: { required: true, min: 1 } },
						quantityBD: { type: 'number', editable:true,validation: { required: true } },
						quantityCD: { type: 'number', editable:true,validation: { required: true } },
						totalReceived: { type: 'number', editable:true,validation: { required: true } }
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: 'Coins (Gp)', editor: coinsEditor, template: '#= getCoin(currencyNoteId) #' },
            { field: 'quantityReceived', title: 'Pieces Received', editor: quantityDepositedEditor, format: "{0: #,###}" },            
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}" }, 
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}" },            			
            { field: 'totalReceived', title: 'Amount Received (GHS)',editor: totalDepositedEditor, format: "{0: #,###.#0}" },            
            { command: ['edit','destroy'],width:200 }
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		toolbar: [
			{
				name: "create",
				text: 'Receive Coins',
			}
		],
		editable: "inline",
		edit: function(e) {
			var data = e.model;
			if (data.currencyNoteId > 0) {
				addCoinBack(data.currencyNoteId);
			   //this.closeCell();
			}
			//e.preventDefault();
		  },
		mobile: true
    });
}

function notesEditor(container, options) {
    $('<input required id="noteCombo" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: remainingNotes,
		filter: "contains",
		suggest: true,
		dataValueField: "currencyNoteId",
		dataTextField: "noteName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: function(e) {
                var id = this.value();
				currentNoteId = this.value();
                noteChange(id);
            },
		optionLabel: ""
    });
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function coinsEditor(container, options) {
    $('<input required id="coinCombo" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: remainingCoins,
		filter: "contains",
		suggest: true,
		dataValueField: "currencyNoteId",
		dataTextField: "noteName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: function(e) {
                var id = this.value();
				currentNoteId = this.value();
                coinChange(id);
            },
		optionLabel: ""
    });
}
function quantityDepositedEditor(container, options) {
    $('<input required id="quantityDep" type="number" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 0,
		change: quantityDepositedChange,
		spin: quantityDepositedChange
    });
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function quantityCDEditor(container, options) {
    $('<input  id="quantityCD" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 0
    });
}
function quantityBDEditor(container, options) {
    $('<input  id="quantityDB" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 0
    });
}
function totalDepositedEditor(container, options) {
    $('<input  id="totDep" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 0
    });
}
function noteChange(noteId){
	var cmd = noteId;
	var exist = false;	
	//Retrieve value enter validate
    for (var i = 0; i < notes.length; i++) {
        if (notes[i].currencyNoteId == noteId) {            
			getCashierAvialableNotes(noteId);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Note', 'ERROR');
        $("#noteCombo").data("kendoComboBox").value("");
    }
}
function coinChange(noteId){
	var exist = false;	
	//Retrieve value enter validate
    for (var i = 0; i < coins.length; i++) {
        if (coins[i].currencyNoteId == noteId) {            
			getCashierAvialableCoins(noteId);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Coin', 'ERROR');
        $("#coinCombo").data("kendoComboBox").value("");
    }
}
var onClientChange = function(){
	var clientId = $("#client").data("kendoComboBox").value();
	$("#account").data("kendoComboBox").value("");
	resetControls();
	getAllClientLoanAccounts(clientId);
	getClientPicture(clientId);
}
var onAccountChange = function(){
	var loanId = $("#account").data("kendoComboBox").value();
	getClientLoanAccount(loanId);
}
function getClientPicture(id) {
    displayLoadingDialog();
    $.ajax({
        url: clientApiUrl + "/GetClientImage/"+id,
        type: 'GET',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		displayClientImage(data);
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
		//warningDialog("Error while retrieving client Image", 'ERROR');
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}
function displayClientImage(data){
	$("#clientImage").attr("src","data:image/png;base64,"+data);
}
function getNote(id) {
    for (var i = 0; i < notes.length; i++) {
        if (notes[i].currencyNoteId == id)
            return notes[i].noteName;
    }
}
function getCoin(id) {
    for (var i = 0; i < coins.length; i++) {
        if (coins[i].currencyNoteId == id)
            return coins[i].noteName;
    }
}
function getAllClientLoanAccounts(id){
	displayLoadingDialog();
	
	var acc = $("#account").data("kendoComboBox");
	$.ajax({
        url: loanApiUrl + '/GetClientRunningLoans/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
		dismissLoadingDialog();

		if(data.length>0){
			loanAccounts = data;
			acc.setDataSource(loanAccounts);
		}else{
			warningDialog('Select Client has no running loan','NOTE');
		}
        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		//warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		warningDialog('Error retrieving data','ERROR'); 		
	});
}
function getClientLoanAccount(id){
	displayLoadingDialog();
	$.ajax({
        url: loanApiUrl + '/GetLoan/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        loan = data;
		populateUi();
		dismissLoadingDialog();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		//warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		warningDialog('Error retrieving data','ERROR'); 		
	});
}
function retrieveValues(){
	var paymentType = $("#paymentType").data("kendoComboBox").value();
	cashierReceipt.cashierReceipt.paymentModeID = paymentType;
	cashierReceipt.cashierReceipt.repaymentTypeID = $("#repaymentType").data("kendoComboBox").value();
	cashierReceipt.cashierReceipt.txDate = $("#repaymentDate").data("kendoDatePicker").value();	
	cashierReceipt.cashierReceipt.amount = $("#amount").data("kendoNumericTextBox").value();
	cashierReceipt.cashierReceipt.loanID = $("#account").data("kendoComboBox").value();
	
	if(paymentType==2){
			cashierReceipt.cashierReceipt.checkNo = $("#checkNo").data("kendoMaskedTextBox").value();
			cashierReceipt.cashierReceipt.bankID = $("#bank").data("kendoComboBox").value();
	}else{
		cashierReceipt.cashierReceiptNotes = $('#notes').data().kendoGrid.dataSource.view();
		cashierReceipt.cashierReceiptCoins = $('#coins').data().kendoGrid.dataSource.view();
	}
}
function saveReceipt() {
    retrieveValues();
    saveToServer();
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: cashierReceiptWithCheckApiUrl + '/PostCashierReceipt',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(cashierReceipt),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Loan Repayment received Successfully', 'SUCCESS',
		function () { window.location = '/dash/home.aspx' });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer
function getClients(input) {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetSearchedLoanClient',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(input),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		if(data.length>0){
			filteredClients = data;
			setUpClientCombox();
		}
		else{
			filteredClients = [];
			successDialog('No client found that match the searched criteria', 'SUCCESS');
		}        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}
function setUpClientCombox(){
	$("#client")
	.width("#90%")
	.kendoComboBox({
		dataSource: filteredClients,
		filter: "contains",
		suggest: true,
		dataValueField: "clientID",
		dataTextField: "clientName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onClientChange,
		optionLabel: ""
	});
}
function getCurrencyTypes(currencies){
	for(var i=0;i<currencies.length;i++){
		if(currencyNotes[i].currencyNoteTypeId == 1){
			notes.push(currencyNotes[i]);
		}
		else if(currencyNotes[i].currencyNoteTypeId == 2){
			coins.push(currencyNotes[i]);
		}
	}
}
function getCashierNotes(){
	for(var j=0;j<notes.length;j++){
		if(notes[j].currencyNoteTypeId == 1){
			remainingNotes.push(notes[j]);			
		}
	}	
}
function getCashierCoins(){
	for(var j=0;j<coins.length;j++){
		if(coins[j].currencyNoteTypeId == 2){
			remainingCoins.push(coins[j]);			
		}
	}	
}
function removeAddedNote(id){
	for (var i = 0; i < remainingNotes.length; i++) {
        if (remainingNotes[i].currencyNoteId == id) {
            remainingNotes.splice(i, 1);
            break;
        }
    }
}
function removeAddedCoin(id){
	for (var i = 0; i < remainingCoins.length; i++) {
        if (remainingCoins[i].currencyNoteId == id) {
            remainingCoins.splice(i, 1);
            break;
        }
    }
}
function addNoteBack(id) {
	var nt = $("#noteCombo").data("kendoComboBox");
    for (var i = 0; i < notes.length; i++) {
        if (notes[i].currencyNoteId == id) {
            remainingNotes.push(notes[i]);
			if(nt != "undefined"&& nt != null)
			{nt.setDataSource(remainingNotes);}
			break;
        }
    }
}
function addCoinBack(id) {
	var c = $("#coinCombo").data("kendoComboBox");
    for (var i = 0; i < coins.length; i++) {
        if (coins[i].currencyNoteId == id) {
            remainingCoins.push(coins[i]);
			if(c != "undefined" && c != null)
			{c.setDataSource(remainingCoins);}
            break;
        }
    }
}
function getCashierAvialableNotes(id) {
	var quanDep = $("#quantityDep").data("kendoNumericTextBox");
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox");
	var totDep = $("#totDep").data("kendoNumericTextBox");

	var fundNotes = cashierFunds.cashierRemainingNotes;
    for (var i = 0; i < fundNotes.length; i++) {
        if (fundNotes[i].currencyNoteId == id) {
			var nt = fundNotes[i];
			quanDep.value(0);
			totDep.value(0);
			quanCD.value(nt.quantity);
			quanBD.value(nt.quantity);
            break;
        }
    }
}
function getCashierAvialableCoins(id) {
	var quanDep = $("#quantityDep").data("kendoNumericTextBox");
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox");
	var totDep = $("#totDep").data("kendoNumericTextBox");

	var fundCoins = cashierFunds.cashierRemainingCoins;
    for (var i = 0; i < fundCoins.length; i++) {
        if (fundCoins[i].currencyNoteId == id) {
			var cn = fundCoins[i];
			quanDep.value(0);
			quanCD.value(cn.quantity);
			quanBD.value(cn.quantity);
			totDep.value(0);			
            break;
        }
    }
}
var currentNoteId = 0;
function quantityDepositedChange(){
	var noteId = currentNoteId;
	var quanDepit = $("#quantityDep").data("kendoNumericTextBox").value();
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox").value();
	var totDep = $("#totDep").data("kendoNumericTextBox");
	
	quanCD.value(quanBD+quanDepit);
	
	for (var i = 0; i < currencyNotes.length; i++) {
        if (currencyNotes[i].currencyNoteId == noteId) {
			var nt = currencyNotes[i];
			totDep.value(nt.value * quanDepit);			
            break;
        }
    }
	
}

function getTotalDeposit(){
	var dpAmt = $("#amount").data("kendoNumericTextBox");
	var tot = 0;
	var nts = $("#notes").data().kendoGrid.dataSource.view();
	if(nts.length > 0)
	for(var x=0;x<nts.length;x++){
		tot += getRowTotal(nts[x]); 
	}
	var cns = $("#coins").data().kendoGrid.dataSource.view();
	if(cns.length > 0)
	for(var y=0;y<cns.length;y++){
		tot += getRowTotal(cns[y]); 
	}
	dpAmt.value(tot);
}

function getRowTotal(data){
	for(var i=0;i<currencyNotes.length;i++){
		if(currencyNotes[i].currencyNoteId == data.currencyNoteId){
			return currencyNotes[i].value * data.quantityReceived;
		}
	}
}




function createConfirm(message, okHandler) {
    var confirm = '<p id="confirmMessage">'+message+'</p><div class="clearfix dropbig">'+
            '<input type="button" id="confirmYes" class="alignleft ui-button ui-widget ui-state-default" value="Yes" />' +
            '<input type="button" id="confirmNo" class="ui-button ui-widget ui-state-default" value="No" /></div>';

    $.fn.colorbox({html:confirm, 
        onComplete: function(){
            $("#confirmYes").click(function(){
                okHandler();
                $.fn.colorbox.close();
            });
            $("#confirmNo").click(function(){
                $.fn.colorbox.close();
            });
    }});
}



