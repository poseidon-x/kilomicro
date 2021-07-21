/*
UI Scripts for Loan savingType Management
Creator: man@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed

"use strict";
var authToken = coreERPAPI_Token;
var cashierFundsTransferApiUrl = coreERPAPI_URL_Root + "/crud/CashierFundsTransfer";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var currencyNoteApiUrl = coreERPAPI_URL_Root + "/crud/currencyNote";
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/cashierTill";


var cashierFundsTransfer = {};
var cashiers = {};
var currencies = {};
var currencyNotes = {};
var notes = [];
var remainingNotes = [];
var coins = [];
var remainingCoins = [];
var cashierFunds = {};
var cashierFundToday = {};

var noteWithdrawalId = -1;
var coinWithdrawalId = -1;

var cashierFundsTransferAjax = $.ajax({
    url: cashierFundsTransferApiUrl + '/Get/' + id,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var cashierAjax = $.ajax({
    url: cashierTillApiUrl + '/GetCashiersName/',
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
    $.when(cashierFundsTransferAjax,cashierAjax,currencyAjax,currencyNoteApiUrl,
	cashierFundAjax,cashierFundTodayAjax)
        .done(function (dataCashierFundsTransfer,dataCashier,dataCurrency,
		dataCurrencyNote,dataCashierFund,dataCashierFundToday) {
            cashierFundsTransfer = cashierFundsTransferAjax[2].responseJSON;
			cashiers = dataDeposit[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
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
		var b = validateInput();
		if(validateInput()){
			if (confirm("Are you sure you want withdraw from Investment account")) {
				displayLoadingDialog();
				saveWithdrawal();				
			} else
			{
				smallerWarningDialog('Please review and withdraw later', 'NOTE');
			}
		}else {
			warningDialog("Please make sure all required fields are provided",'WARNING');
		}
	});
}

//Apply kendo Style to the input fields
function renderControls() {
	$("#tabs").kendoTabStrip();
	$("#cashierTill").width("90%")
	.kendoComboBox({
		dataSource: depositAccounts,
		filter: "contains",
		suggest: true,
		dataValueField: "cashierTillId",
		dataTextField: "cashierFullName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onTillChange,
		optionLabel: ""
	});
    
	$("#transferAmount").width("90%")
	.kendoNumericTextBox({
		format: "#,##0.#0"
	});
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
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Account', 'ERROR');
        $("#paymentType").data("kendoComboBox").value("");
    }

}
function setUpAccountDeatils(){
	var depo = $("#account").data("kendoComboBox");
	getClientPicture(deposit.clientID);
	populateUi();
	var depos = {
		depositId:deposit.depositID,
		depositAccountNo:deposit.depositNo
	};
	var client = deposit.client;
	var cln = {
		clientID:client.clientID,
		clientNameWithAccountNO:client.surName+" "+client.otherNames
	};
	var depositAccou = [];
	depositAccou.push(depos);
	depo.setDataSource(depositAccou);
	depo.value(deposit.depositID);
	filteredClients.push(cln);
	setUpClientCombox();
	$("#client").data("kendoComboBox").value(client.clientID);
	var clt = document.getElementById("client");
	document.getElementById("client").readOnly = true;
	document.getElementById("account").readOnly = true;
}
function populateUi(){
	var amt = $("#withdrawalAmount").data("kendoNumericTextBox");
	var dep = depositWithdrawal.deposit;
	
	$("#save").show();
	
	$("#depositType").data("kendoComboBox").value(dep.depositTypeID);
	$("#avaialablePrincipalBalance").data("kendoNumericTextBox").value(dep.principalBalance);
	$("#avaialableInterestBalance").data("kendoNumericTextBox").value(dep.interestBalance);
	$("#interestRate").data("kendoNumericTextBox").value(dep.interestRate);
	$("#period").data("kendoNumericTextBox").value(dep.period);
	$("#withdrawalDate").data("kendoDatePicker").value(new Date());	
}
//Reset controls of client or account
function resetControls(){
	$("#withdrawalAmount").data("kendoNumericTextBox").value("");	
	$("#tabs").hide();
	$("#save").hide();
	$("#avaialablePrincipalBalance").data("kendoNumericTextBox").value("");
	$("#avaialableInterestBalance").data("kendoNumericTextBox").value("");
	$("#interestRate").data("kendoNumericTextBox").value("");
	$("#paymentType").data("kendoComboBox").value("");
	$("#depositType").data("kendoComboBox").value("");
	$("#checkNo").data("kendoMaskedTextBox").value("");
	$("#bank").data("kendoComboBox").value("");
	$("#period").data("kendoNumericTextBox").value("");
	$("#chequeDetails").hide();

}
function setUpCurrencies(){
	$("#tabs").show();
	dismisChequeDetails();
	document.getElementById("withdrawalAmount").readOnly = true;
	renderGrid();
}
function displayChequeDetails(){
	$("#tabs").hide();
	document.getElementById("withdrawalAmount").readOnly = false;
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
							entries.success(depositWithdrawal.depositWithdrawalNotes);                    
					},
					create: function (entries) {
						var data = entries.data;						
						data.depositWithdrawalNoteId = noteWithdrawalId;
						noteWithdrawalId += -1;
						data.quantityWithdrawn = $("#quantityWith").data("kendoNumericTextBox").value();
						data.quantityBD = $("#quantityDB").data("kendoNumericTextBox").value();						
						data.quantityCD = $("#quantityCD").data("kendoNumericTextBox").value();
						data.totalWithdrawn = $("#totWith").data("kendoNumericTextBox").value();
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
                    id: 'depositWithdrawalNoteId',
                    fields: {
						depositWithdrawalNoteId: { type: 'number' },
						depositWithdrawalID: { type: 'number' },
						currencyNoteId: { editable: true, validation: { required: true } },
						quantityWithdrawn: { type: 'number', validation: { required: true, min: 1 } },
						quantityBD: { type: 'number', editable:true,validation: { required: true } },
						quantityCD: { type: 'number', editable:true,validation: { required: true } },
						totalWithdrawn: { type: 'number', editable:true,validation: { required: true } }
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: 'Notes (GHS)', editor: notesEditor, template: '#= getNote(currencyNoteId) #' },
            { field: 'quantityWithdrawn', title: 'Pieces Withdrawn', editor: quantityWithdrawnEditor, format: "{0: #,###}" },  
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}" },            			
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}" },            
            { field: 'totalWithdrawn', title: 'Amount Withdrawn (GHS)',editor: totalWithdrawnEditor, format: "{0: #,###.#0}" },            
            { command: ["edit","destroy"],width:210 }
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
				text: 'Withdraw Notes',
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
							entries.success(depositWithdrawal.depositWithdrawalCoins);                    
					},
					create: function (entries) {
						var data = entries.data;
						data.depositWithdrawalCoinId = coinWithdrawalId;
						coinWithdrawalId += -1;
						data.quantityWithdrawn = $("#quantityWith").data("kendoNumericTextBox").value();
						data.quantityCD = $("#quantityCD").data("kendoNumericTextBox").value();
						data.quantityBD = $("#quantityDB").data("kendoNumericTextBox").value();
						data.totalWithdrawn = $("#totWith").data("kendoNumericTextBox").value();
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
                    id: 'depositWithdrawalCoinId',
                    fields: {
						depositWithdrawalCoinId: { type: 'number' },
						depositWithdrawalID: { type: 'number' },
						currencyNoteId: { editable: true, validation: { required: true } },
						quantityDeposited: { type: 'number', validation: { required: true, min: 1 } },
						quantityBD: { type: 'number', editable:true,validation: { required: true } },
						quantityCD: { type: 'number', editable:true,validation: { required: true } },
						totalDeposited: { type: 'number', editable:true,validation: { required: true } }
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: 'Coins (Gp)', editor: coinsEditor, template: '#= getCoin(currencyNoteId) #' },
            { field: 'quantityWithdrawn', title: 'Pieces Withdrawn', editor: quantityWithdrawnEditor, format: "{0: #,###}" },            
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}" }, 
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}" },            			
            { field: 'totalWithdrawn', title: 'Amount Withdrawn (GHS)',editor: totalWithdrawnEditor, format: "{0: #,###.#0}" },            
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
				text: 'Withdraw Coins',
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
function quantityWithdrawnEditor(container, options) {
    $('<input required id="quantityWith" type="number" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 0,
		change: quantityWithdrawnChange,
		spin: quantityWithdrawnChange
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
function totalWithdrawnEditor(container, options) {
    $('<input  id="totWith" readonly data-bind="value:' + options.field + '"/>')
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
	getAllClientDepositAccounts(clientId);
	getClientPicture(clientId);
}
var onAccountChange = function(){
	var depositId = $("#account").data("kendoComboBox").value();
	getClientDepositAccount(depositId);
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
function getAllClientDepositAccounts(id){
	var acc = $("#account").data("kendoComboBox");
	$.ajax({
        url: depositApiUrl + '/GetAllClientDepositAccounts/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        depositAccounts = data;
		acc.setDataSource(depositAccounts);
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		//warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		warningDialog('Error retrieving data','ERROR'); 		
	});
}
function getClientDepositAccount(id){
	$.ajax({
        url: depositApiUrl + '/GetDeposit/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        depositWithdrawal.deposit = data;
		populateUi();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		//warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		warningDialog('Error retrieving data','ERROR'); 		
	});
}
function retrieveValues(){
	var dep = depositWithdrawal.deposit;
	depositWithdrawal.depositWithdrawal.withdrawalDate = $("#withdrawalDate").data("kendoDatePicker").value();
	var withAmt = $("#withdrawalAmount").data("kendoNumericTextBox").value();
	var paymentType = $("#paymentType").data("kendoComboBox").value();
	depositWithdrawal.depositWithdrawal.modeOfPaymentID = paymentType;
	depositWithdrawal.depositWithdrawal.naration = $("#naration").data("kendoMaskedTextBox").value();
	if(depositWithdrawalType=="I"&&withAmt<=dep.interestBalance){
		depositWithdrawal.depositWithdrawal.interestWithdrawal = withAmt;
		dep.interestBalance -= withAmt;
	}
	else if(depositWithdrawalType=="P"&&withAmt<=dep.principalBalance){
		depositWithdrawal.depositWithdrawal.principalWithdrawal = withAmt;
		dep.principalBalance -= withAmt;
	}
	else if(depositWithdrawalType=="B"&&withAmt<=(dep.principalBalance+dep.interestBalance)){
		//if withdrawal type is both reduce principalBalance first then interestBalance
		if(withAmt<=dep.principalBalance){
			depositWithdrawal.depositWithdrawal.principalWithdrawal = withAmt;
			dep.principalBalance -= withAmt;
		}
		else if(withAmt>dep.principalBalance){
			depositWithdrawal.depositWithdrawal.principalWithdrawal = dep.principalBalance;
			dep.principalBalance -= dep.principalBalance;
			withAmt-=dep.principalBalance;
			depositWithdrawal.depositWithdrawal.interestWithdrawal = withAmt;
			dep.interestBalance -= withAmt;
		}
	}
	if(paymentType==2){
			depositWithdrawal.depositWithdrawal.checkNo = $("#checkNo").data("kendoMaskedTextBox").value();
			depositWithdrawal.depositWithdrawal.bankID = $("#bank").data("kendoComboBox").value();
	}else{
		depositWithdrawal.depositWithdrawalNotes = $('#notes').data().kendoGrid.dataSource.view();
		depositWithdrawal.depositWithdrawalCoins = $('#coins').data().kendoGrid.dataSource.view();
	}
	depositWithdrawal.depositWithdrawal.withdrawalDate = $("#withdrawalDate").data("kendoDatePicker").value();
	depositWithdrawal.withdrawalType = depositWithdrawalType;
}
function saveWithdrawal() {
	retrieveValues();
	saveToServer();    
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: depositWithdrawalApiUrl + '/PostDepositWithdrawal',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(depositWithdrawal),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		//var bal = (parseFloat(data.principalBalance)+parseFloat(data.interestBalance)).toFixed(2);
        successDialog('Investment withdrawal Successful', 
		'SUCCESS',function () { window.location = '/ln/depositReports/withReceipt.aspx?id='+data.depositWithdrawalID; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer

function validateInput(){ 
	var paymtType = $("#paymentType").data("kendoComboBox").value();
	var chk = $("#checkNo").data("kendoMaskedTextBox").value();
	var bnk = $("#bank").data("kendoComboBox").value();
	var withDate = $("#withdrawalDate").data("kendoDatePicker").value();
	var withAmt = $("#withdrawalAmount").data("kendoNumericTextBox").value();
	var payType = $("#paymentType").data("kendoComboBox").value();
	var narat = $("#naration").data("kendoMaskedTextBox").value();
	
	
	if(paymtType==2 && (chk=="" || chk == "undefined" || chk == null) 
	&& (bnk=="" || bnk == "undefined" || bnk == null) ||
	(withDate=="" || withDate == "undefined" || withDate == null) ||
	(withAmt=="" || withAmt == "undefined" || withAmt == null) ||
	(payType=="" || payType == "undefined" || payType == null) ||
	(narat=="" || narat == "undefined" || narat == null)){
		return false;
	}
	return true;
}

function getClients(input) {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetSearchedDepositClient',
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
	$("#client").width("90%")
	.kendoComboBox({
		dataSource: filteredClients,
		filter: "contains",
		suggest: true,
		dataValueField: "clientID",
		dataTextField: "clientNameWithAccountNO",
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
	var nts = cashierFunds.cashierRemainingNotes;
	for(var i=0;i<nts.length;i++){
		if(nts[i].quantity>0)
		for(var j=0;j<notes.length;j++){
			if(notes[j].currencyNoteId == nts[i].currencyNoteId){
				remainingNotes.push(notes[j]);			
			}
		}	
	}
}
function getCashierCoins(){
	var cns = cashierFunds.cashierRemainingCoins;
	for(var i=0;i<cns.length;i++){
		if(cns[i].quantity>0)
		for(var j=0;j<coins.length;j++){
			if(coins[j].currencyNoteId == cns[i].currencyNoteId){
				remainingCoins.push(coins[j]);			
			}
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
			nt.setDataSource(remainingNotes);
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
	var quanWith = $("#quantityWith").data("kendoNumericTextBox");
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox");
	var totWith = $("#totWith").data("kendoNumericTextBox");

	var fundNotes = cashierFunds.cashierRemainingNotes;
    for (var i = 0; i < fundNotes.length; i++) {
        if (fundNotes[i].currencyNoteId == id) {
			var nt = fundNotes[i];
			quanWith.value(0);
			quanWith.max(nt.quantity);
			quanCD.value(nt.quantity);
			quanBD.value(nt.quantity);
			totWith.value(0);			
            break;
        }
    }
}
function getCashierAvialableCoins(id) {
	var quanWith = $("#quantityWith").data("kendoNumericTextBox");
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox");
	var totWith = $("#totWith").data("kendoNumericTextBox");

	var fundCoins = cashierFunds.cashierRemainingCoins;
    for (var i = 0; i < fundCoins.length; i++) {
        if (fundCoins[i].currencyNoteId == id) {
			var cn = fundCoins[i];
			quanWith.value(0);
			quanCD.value(cn.quantity);
			quanBD.value(cn.quantity);
			totWith.value(0);			
            break;
        }
    }
}
var currentNoteId = 0;
function quantityWithdrawnChange(){
	var noteId = currentNoteId;
	var quanWithD = $("#quantityWith").data("kendoNumericTextBox").value();
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox").value();
	var totWith = $("#totWith").data("kendoNumericTextBox");
	
	quanCD.value(quanBD-quanWithD);
	
	for (var i = 0; i < currencyNotes.length; i++) {
        if (currencyNotes[i].currencyNoteId == noteId) {
			var nt = currencyNotes[i];
			totWith.value(nt.value * quanWithD);			
            break;
        }
    }
	
}

function getTotalDeposit(){
	var totalWithAmt = $("#withdrawalAmount").data("kendoNumericTextBox");
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
	totalWithAmt.value(tot);
}

function getRowTotal(data){
	for(var i=0;i<currencyNotes.length;i++){
		if(currencyNotes[i].currencyNoteId == data.currencyNoteId){
			return currencyNotes[i].value * data.quantityWithdrawn;
		}
	}
}