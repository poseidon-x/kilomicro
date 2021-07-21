/*
UI Scripts for Loan savingType Management
Creator: man@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var savingWithdrawalApiUrl = coreERPAPI_URL_Root + "/crud/SavingWithdrawalWithCheck";
var savingApiUrl = coreERPAPI_URL_Root + "/crud/Saving";
var savingTypeApiUrl = coreERPAPI_URL_Root + "/crud/savingType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var modeOfPaymentApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var currencyNoteApiUrl = coreERPAPI_URL_Root + "/crud/currencyNote";
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/cashierTill";
var banksApiUrl = coreERPAPI_URL_Root + "/crud/banks";


var savingWithdrawal = {};
var savingTypes = {};
var clients = {};
var savingsAccounts = {};
var currentSaving = {}; 
var currencies = {};
var banks = {};
var modeOfpayments = {};
var currencyNotes = {};
var filteredClients = [];
var notes = [];
var remainingNotes = [];
var coins = [];
var remainingCoins = [];
var cashierFunds = {};
var cashierFundToday = {};


var noteWithdrawalId = -1;
var coinWithdrawalId = -1;

var savingWithdrawalAjax = $.ajax({
    url: savingWithdrawalApiUrl + '/Get/' + id,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var savingAjax = $.ajax({
    url: savingApiUrl + '/GetSavingAccount/' + savId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var savingTypeAjax = $.ajax({
    url: savingTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var clientAjax = $.ajax({
    url: clientApiUrl + '/GetSavingsClient',
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
    $.when(savingWithdrawalAjax,savingAjax,savingTypeAjax,clientAjax,currencyAjax,bankAjax,paymentModeAjax,currencyNoteAjax,cashierFundAjax,cashierFundTodayAjax)
        .done(function (dataSavingWithdrawal,dataSaving,dataSavingType,dataClient,dataCurrency,dataBank,dataPaymentMode,dataCurrencyNote,dataCashierFund,dataCashierFundToday) {
            savingWithdrawal = dataSavingWithdrawal[2].responseJSON;
			currentSaving = dataSaving[2].responseJSON;
            savingTypes = dataSavingType[2].responseJSON;
            clients = dataClient[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
			banks = dataBank[2].responseJSON;
            modeOfpayments = dataPaymentMode[2].responseJSON;
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
				
				if(currentSaving.savingID > 0){
					savingWithdrawal.saving = currentSaving;
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
		if (confirm("Are you sure you want Withdraw from savings account")) {
            displayLoadingDialog();
			saveWithdrawal();				
		} else
		{
            smallerWarningDialog('Please review and withdraw later', 'NOTE');
        }
	});
	$('input[type=radio][name=withdrawalType]').change(function() {
		var amt = $("#withdrawalAmount").data("kendoNumericTextBox");
		var sv = savingWithdrawal.saving;
        if (this.value == 'interest') {
			amt.max(sv.availableInterestBalance);
        }
        else if (this.value == 'principal') {
			amt.max(sv.availablePrincipalBalance);
        }
		else if (this.value == 'both') {
			amt.max(sv.availablePrincipalBalance+sv.availableInterestBalance);
        }
    });
}


//Apply kendo Style to the input fields
function renderControls() {	
	$("#tabs").kendoTabStrip();
	$("#account").width("90%")
	.kendoComboBox({
		dataSource: savingsAccounts,
		filter: "contains",
		suggest: true,
		dataValueField: "savingId",
		dataTextField: "savingAccountNo",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onAccountChange,
		optionLabel: ""
	});
    $("#savingType").width("90%")
	.kendoComboBox({
		dataSource: savingTypes,
		filter: "contains",
		suggest: true,
		dataValueField: "savingTypeID",
		dataTextField: "savingTypeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#currency").width("90%")
	.kendoComboBox({
		dataSource: currencies,
		filter: "contains",
		suggest: true,
		dataValueField: "currency_id",
		dataTextField: "major_name",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#principalBalance").width("90%")
	.kendoNumericTextBox();
	$("#availablePrincipalBalance").width("90%")
	.kendoNumericTextBox();
	$("#interestBalance").width("90%")
	.kendoNumericTextBox();
	$("#availableInterestBalance").width("90%")
	.kendoNumericTextBox();
	$("#withdrawalDate").width("90%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$("#withdrawalAmount").width("90%")
	.kendoNumericTextBox();
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
	$("#naration").width("90%")
	.kendoMaskedTextBox();
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
	resetControls();
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
	var savi = $("#account").data("kendoComboBox");
	getClientPicture(currentSaving.clientID);
	populateUi();
	var sa = {
		savingId:currentSaving.savingID,
		savingAccountNo:currentSaving.savingNo
	};
	var client = currentSaving.client;
	var cln = {
		clientID:client.clientID,
		clientNameWithAccountNO:client.surName+" "+client.otherNames
	};
	var savingAccou = [];
	savingAccou.push(sa);
	savi.setDataSource(savingAccou);
	savi.value(currentSaving.savingID);
	filteredClients.push(cln);
	setUpClientCombox();
	$("#client").data("kendoComboBox").value(client.clientID);
	var clt = document.getElementById("client");
	document.getElementById("client").readOnly = true;
	document.getElementById("account").readOnly = true;
}
function populateUi(){
	var amt = $("#withdrawalAmount").data("kendoNumericTextBox");
	var sav = savingWithdrawal.saving;
	if(sav.availablePrincipalBalance < 1){
		$("#tabs").hide();
		$("#save").hide();
		$("#both").prop("checked", false);
	}else{
		$("#both").prop("checked", true);
	}
	
	amt.max(sav.availablePrincipalBalance+sav.availableInterestBalance);
	$("#savingType").data("kendoComboBox").value(sav.savingTypeID);
	$("#principalBalance").data("kendoNumericTextBox").value(sav.principalBalance);
	$("#availablePrincipalBalance").data("kendoNumericTextBox").value(sav.availablePrincipalBalance);
	$("#interestBalance").data("kendoNumericTextBox").value(sav.interestBalance);
	$("#availableInterestBalance").data("kendoNumericTextBox").value(sav.availableInterestBalance);
	$("#withdrawalDate").data("kendoDatePicker").value(new Date());	
}
function resetControls(){
	$("#withdrawalAmount").data("kendoNumericTextBox").value("");	
	$("#tabs").hide();
	$("#save").hide();
	$("#interest").prop("checked", false);
	$("#principal").prop("checked", false);
	$("#both").prop("checked", false);
	$("#savingType").data("kendoComboBox").value("");
	$("#principalBalance").data("kendoNumericTextBox").value("");
	$("#availablePrincipalBalance").data("kendoNumericTextBox").value("");
	$("#interestBalance").data("kendoNumericTextBox").value("");
	$("#availableInterestBalance").data("kendoNumericTextBox").value("");
	$("#paymentType").data("kendoComboBox").value("");
	$("#checkNo").data("kendoMaskedTextBox").value("");
	$("#bank").data("kendoComboBox").value("");
	$("#chequeDetails").hide();
}
function renderGrid(){
	$('#notes').kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
							entries.success(savingWithdrawal.savingWithdrawalNotes);                    
					},
					create: function (entries) {
						var data = entries.data;						
						data.savingWithdrawalNoteId = noteWithdrawalId;
						noteWithdrawalId += -1;
						data.quantityWithdrawn = $("#quantityWith").data("kendoNumericTextBox").value();
						data.quantityCD = $("#quantityCD").data("kendoNumericTextBox").value();
						data.quantityBD = $("#quantityDB").data("kendoNumericTextBox").value();
						data.totalWithdrawn = $("#totWith").data("kendoNumericTextBox").value();
						removeAddedNote(data.currencyNoteId);
						entries.success(entries.data);
						getTotalWithdrawl();
					},
					update: function (entries) {				
						var data = entries.data;
						removeAddedNote(data.currencyNoteId);
						entries.success(entries.data);
						getTotalWithdrawl();
					},
					destroy: function (entries) {
						var data = entries.data;
						addNoteBack(data.currencyNoteId);
						entries.success(entries.data);
						getTotalWithdrawl();
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'savingWithdrawalNoteId',
                    fields: {
						savingWithdrawalNoteId: { type: 'number' },
						savingWithdrawalID: { type: 'number' },
						currencyNoteId: { editable: true, validation: { required: true } },
						quantityWithdrawn: { type: 'number', validation: { required: true, min: 1 } },
						quantityCD: { type: 'number', editable:true,validation: { required: true } },
						quantityBD: { type: 'number', editable:true,validation: { required: true } },
						totalWithdrawn: { type: 'number', editable:true,validation: { required: true } }
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: 'Notes (GHS)', editor: notesEditor, template: '#= getNote(currencyNoteId) #' },
            { field: 'quantityWithdrawn', title: 'Pieces Withdrawn', editor: quantityWithdrawnEditor, format: "{0: #,###}",width:140 },  
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}",width:140 },            			
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}",width:140 },            
            { field: 'totalWithdrawn', title: 'Amount Withdrawn (GHS)',editor: totalWithEditor, format: "{0: #,###.#0}",width:140 },            
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
				text: 'Withdraw Note',
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
							entries.success(savingWithdrawal.savingWithdrawalCoins);                    
					},
					create: function (entries) {
						var data = entries.data;
						data.savingWithdrawalCoinId = coinWithdrawalId;
						coinWithdrawalId += -1;
						data.quantityWithdrawn = $("#quantityWith").data("kendoNumericTextBox").value();
						data.quantityCD = $("#quantityCD").data("kendoNumericTextBox").value();
						data.quantityBD = $("#quantityDB").data("kendoNumericTextBox").value();
						data.totalWithdrawn = $("#totWith").data("kendoNumericTextBox").value();
						removeAddedCoin(data.currencyNoteId);
						entries.success(entries.data);
						getTotalWithdrawl();
					},
					update: function (entries) {
						var data = entries.data;
						removeAddedCoin(data.currencyNoteId);
						entries.success(entries.data);
						getTotalWithdrawl();
					},
					destroy: function (entries) {
						var data = entries.data;
						addCoinBack(data.currencyNoteId);
						entries.success(entries.data);
						getTotalWithdrawl();
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'savingWithdrawalCoinId',
                    fields: {
						savingWithdrawalCoinId: { type: 'number' },
						savingWithdrawalID: { type: 'number' },
						currencyNoteId: { editable: true, validation: { required: true } },
						quantityWithdrawn: { type: 'number', validation: { required: true, min: 1 } },
						quantityCD: { type: 'number', editable:true,validation: { required: true } },
						quantityBD: { type: 'number', editable:true,validation: { required: true } },
						totalWithdrawn: { type: 'number', editable:true,validation: { required: true } }
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: 'Coins (Gp)', editor: coinsEditor, template: '#= getCoin(currencyNoteId) #' },
            { field: 'quantityWithdrawn', title: 'Pieces Withdrawn', editor: quantityWithdrawnEditor, format: "{0: #,###}",width:140 },            
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}",width:140 }, 
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}",width:140 },            			
            { field: 'totalWithdrawn', title: 'Amount Withdrawn (GHS)',editor: totalWithEditor, format: "{0: #,###.#0}",width:140 },            
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
				text: 'Withdraw Coin',
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
    $('<input required  id="quantityWith" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 0,
		change: quantityWithChange,
		spin: quantityWithChange
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
function totalWithEditor(container, options) {
    $('<input  id="totWith" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 0
    });
}
function noteChange(noteId){
	var cmd = noteId;
	//var noteId = $("#note").data("kendoComboBox").value();
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
	getAllClientSavingsAccounts(clientId);
	getClientPicture(clientId);
}
var onAccountChange = function(){
	var savingId = $("#account").data("kendoComboBox").value();
	getClientSavingsAccount(savingId);
}
var onPaymentTypeChange = function(){
	if(checkForAvailableBal()){
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
	}else{	
		$("#paymentType").data("kendoComboBox").value("");
	}
}

function checkForAvailableBal(){
	var prinBal = $("#availablePrincipalBalance").data("kendoNumericTextBox").value();
	var intrsBal = $("#availableInterestBalance").data("kendoNumericTextBox").value();
	
	if(prinBal+intrsBal > 0)return true;
	else {
		warningDialog('Sorry client does not have enough Avialable balance', 'WARNING');
		return false;
	}
}
function resetWithdrawalDetailsControls(){
	$("#withdrawalDate").data("kendoDatePicker").value("");
	displayChequeDetails();
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
function getAllClientSavingsAccounts(id){
	displayLoadingDialog();
	var acc = $("#account").data("kendoComboBox");
	$.ajax({
        url: clientApiUrl + '/GetClientSavingsAccounts/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
		dismissLoadingDialog();
        savingsAccounts = data;
		acc.setDataSource(savingsAccounts);
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		//warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		warningDialog('Error retrieving data','ERROR'); 		
	});
}
function getClientSavingsAccount(id){
	displayLoadingDialog();
	$.ajax({
        url: savingApiUrl + '/GetSavingAccount/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
		dismissLoadingDialog();
        savingWithdrawal.saving = data;
		populateUi();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		//warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		warningDialog('Error retrieving data','ERROR'); 		
	});
}
function retrieveValues(){
	var sav = savingWithdrawal.saving;
	savingWithdrawal.savingWithdrawal.withdrawalDate = $("#withdrawalDate").data("kendoDatePicker").value();
	var withAmt = $("#withdrawalAmount").data("kendoNumericTextBox").value();
	savingWithdrawal.savingWithdrawal.modeOfPaymentID = $("#paymentType").data("kendoComboBox").value();
	savingWithdrawal.savingWithdrawal.naration = $("#naration").data("kendoMaskedTextBox").value();
	if(withAmt>sav.availableInterestBalance){
		savingWithdrawal.savingWithdrawal.interestWithdrawal = sav.availableInterestBalance;
		withAmt -= sav.availableInterestBalance;
	}
	savingWithdrawal.savingWithdrawal.principalWithdrawal = withAmt;
	savingWithdrawal.savingWithdrawal.principalBalance = sav.principalBalance  - savingWithdrawal.savingWithdrawal.principalWithdrawal;
	savingWithdrawal.savingWithdrawal.interestBalance = sav.interestBalance - savingWithdrawal.interestWithdrawal;
	
	if(savingWithdrawal.savingWithdrawal.modeOfPaymentID==2){
		savingWithdrawal.savingWithdrawal.checkNo = $("#checkNo").data("kendoMaskedTextBox").value();
		savingWithdrawal.savingWithdrawal.bankID = $("#bank").data("kendoComboBox").value();
	}else{
		savingWithdrawal.savingWithdrawalNotes = $('#notes').data().kendoGrid.dataSource.view();
		savingWithdrawal.savingWithdrawalCoins = $('#coins').data().kendoGrid.dataSource.view();
	}
}
function saveWithdrawal() {
    retrieveValues();
    saveToServer();
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: savingWithdrawalApiUrl + '/PostSavingsWithdrawal',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(savingWithdrawal),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('saving withdrawal Successful', 'SUCCESS',
		function () { window.location = '/ln/savingReports/withReceipt.aspx?id='+data.savingWithdrawalID; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer
function getClients(input) {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetSearchedClient',
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
			successDialog('saving withdrawal Successful', 'SUCCESS');
		}        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}
function setUpClientCombox(){
	$("#client")
	.width("90%")
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
			if(nt != "undefined" && nt != null)
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
	var quanWith = $("#quantityWith").data("kendoNumericTextBox");
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox");
	var totWit = $("#totWith").data("kendoNumericTextBox");

	var fundNotes = cashierFunds.cashierRemainingNotes;
    for (var i = 0; i < fundNotes.length; i++) {
        if (fundNotes[i].currencyNoteId == id) {
			var nt = fundNotes[i];
			quanWith.max(nt.quantity);
			quanWith.value(0);
			quanCD.value(nt.quantity);
			quanBD.value(nt.quantity);
			totWit.value(0);			
            break;
        }
    }
}
function getCashierAvialableCoins(id) {
	var quanWith = $("#quantityWith").data("kendoNumericTextBox");
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox");
	var totWit = $("#totWith").data("kendoNumericTextBox");

	var fundCoins = cashierFunds.cashierRemainingCoins;
    for (var i = 0; i < fundCoins.length; i++) {
        if (fundCoins[i].currencyNoteId == id) {
			var cn = fundCoins[i];
			quanWith.max(cn.quantity);
			quanWith.value(0);
			quanCD.value(cn.quantity);
			quanBD.value(cn.quantity);
			totWit.value(0);			
            break;
        }
    }
}
var currentNoteId = 0;
function quantityWithChange(){
	var noteId = currentNoteId;
	var quanWit = $("#quantityWith").data("kendoNumericTextBox").value();
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox").value();
	var totWit = $("#totWith").data("kendoNumericTextBox");
	
	quanCD.value(quanBD-quanWit);
	
	for (var i = 0; i < currencyNotes.length; i++) {
        if (currencyNotes[i].currencyNoteId == noteId) {
			var nt = currencyNotes[i];
			totWit.value(nt.value * quanWit);			
            break;
        }
    }
	
}

function getTotalWithdrawl(){
	var withAmt = $("#withdrawalAmount").data("kendoNumericTextBox");
	var tot = 0;
	var nts = $("#notes").data().kendoGrid.dataSource.view();
	if(nts.length > 0)
	for(var x=0;x<nts.length;x++){
		tot += getRowTotal(nts[x]); 
	}
	var cns = $("#coins").data().kendoGrid.dataSource.view();
	if(cns.length > 0)
	for(y=0;y<cns.length;y++){
		tot += getRowTotal(cns[y]); 
	}
	withAmt.value(tot);
}

function getRowTotal(data){
	for(var i=0;i<currencyNotes.length;i++){
		if(currencyNotes[i].currencyNoteId == data.currencyNoteId){
			return currencyNotes[i].value * data.quantityWithdrawn;
		}
	}
}



