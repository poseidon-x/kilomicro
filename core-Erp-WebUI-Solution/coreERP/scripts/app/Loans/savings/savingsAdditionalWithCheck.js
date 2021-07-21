/*
UI Scripts for Loan savingType Management
Creator: man@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed

"use strict";
var authToken = coreERPAPI_Token;
var savingAdditionalApiUrl = coreERPAPI_URL_Root + "/crud/savingAdditionalWithCheck";
var savingApiUrl = coreERPAPI_URL_Root + "/crud/Saving";
var savingTypeApiUrl = coreERPAPI_URL_Root + "/crud/savingType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var modeOfPaymentApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var currencyNoteApiUrl = coreERPAPI_URL_Root + "/crud/currencyNote";
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/cashierTill";
var banksApiUrl = coreERPAPI_URL_Root + "/crud/banks";

var savingAdditional = {};
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

var noteAdditionalId = -1;
var coinAdditionalId = -1;

var savingAdditionalAjax = $.ajax({
    url: savingAdditionalApiUrl + '/Get/' + id,
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
    $.when(savingAdditionalAjax,savingAjax,savingTypeAjax,clientAjax,currencyAjax,
	bankAjax,paymentModeAjax,currencyNoteAjax,cashierFundAjax,cashierFundTodayAjax)
        .done(function (savingAdditionalAjax,dataSaving,dataSavingType,dataClient,dataCurrency,dataBank,
		dataPaymentMode,dataCurrencyNote,dataCashierFund,dataCashierFundToday) {
            savingAdditional = savingAdditionalAjax[2].responseJSON;
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
					savingAdditional.saving = currentSaving;
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
		if (confirm("Are you sure you want make deposit")) {
            displayLoadingDialog();
			saveWithdrawal();				
		} else
		{
            smallerWarningDialog('Please review and deposit later', 'NOTE');
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
	$("#principalBalance").width("90%")
	.kendoNumericTextBox();
	$("#availablePrincipalBalance").width("90%")
	.kendoNumericTextBox();
	$("#interestBalance").width("90%")
	.kendoNumericTextBox();
	$("#availableInterestBalance").width("90%")
	.kendoNumericTextBox();
	$("#depositDate").width("90%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$("#savingAmount").width("90%")
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
	var sav = savingAdditional.saving;
	
	$("#save").show();
	
	$("#savingType").data("kendoComboBox").value(sav.savingTypeID);
	$("#principalBalance").data("kendoNumericTextBox").value(sav.principalBalance);
	$("#availablePrincipalBalance").data("kendoNumericTextBox").value(sav.availablePrincipalBalance);
	$("#interestBalance").data("kendoNumericTextBox").value(sav.interestBalance);
	$("#availableInterestBalance").data("kendoNumericTextBox").value(sav.availableInterestBalance);
	$("#depositDate").data("kendoDatePicker").value(new Date());

	
}
//Reset controls of client or account
function resetControls(){
	
	$("#savingAmount").data("kendoNumericTextBox").value("");	
	$("#tabs").hide();
	$("#save").hide();
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
							entries.success(savingAdditional.savingAdditionalNotes);                    
					},
					create: function (entries) {
						var data = entries.data;						
						data.savingAdditionalNoteId = noteAdditionalId;
						noteAdditionalId += -1;
						data.quantityDeposited = $("#quantityDep").data("kendoNumericTextBox").value();
						data.quantityBD = $("#quantityDB").data("kendoNumericTextBox").value();						
						data.quantityCD = $("#quantityCD").data("kendoNumericTextBox").value();
						data.totalDeposited = $("#totDep").data("kendoNumericTextBox").value();
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
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'savingAdditionalNoteId',
                    fields: {
						savingAdditionalNoteId: { type: 'number' },
						savingAdditionalID: { type: 'number' },
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
            { field: 'currencyNoteId',title: 'Notes (GHS)', editor: notesEditor, template: '#= getNote(currencyNoteId) #' },
            { field: 'quantityDeposited', title: 'Pieces Deposited', editor: quantityDepositedEditor, format: "{0: #,###}",width:110 },  
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}",width:110 },            			
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}",width:110 },            
            { field: 'totalDeposited', title: 'Amount Deposited (GHS)',editor: totalDepositedEditor, format: "{0: #,###.#0}",width:200 },            
            { command: ["edit"],width:110 }
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
				text: 'Add Notes',
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
							entries.success(savingAdditional.savingAdditionalCoins);                    
					},
					create: function (entries) {
						var data = entries.data;
						data.savingAdditionalCoinId = coinAdditionalId;
						coinAdditionalId += -1;
						data.quantityWithdrawn = $("#quantityDep").data("kendoNumericTextBox").value();
						data.quantityCD = $("#quantityCD").data("kendoNumericTextBox").value();
						data.quantityBD = $("#quantityDB").data("kendoNumericTextBox").value();
						data.totalDeposited = $("#totDep").data("kendoNumericTextBox").value();
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
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'savingAdditionalCoinId',
                    fields: {
						savingAdditionalCoinId: { type: 'number' },
						savingAdditionalID: { type: 'number' },
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
            { field: 'quantityDeposited', title: 'Pieces Deposited', editor: quantityDepositedEditor, format: "{0: #,###}",width:110 },            
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}",width:110 }, 
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}",width:110 },            			
            { field: 'totalDeposited', title: 'Amount Deposited (GHS)',editor: totalDepositedEditor, format: "{0: #,###.#0}",width:200 },            
            { command: ['edit'],width:110 }
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
				text: 'Add Coins',
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
	getAllClientSavingsAccounts(clientId);
	getClientPicture(clientId);
}
var onAccountChange = function(){
	var savingId = $("#account").data("kendoComboBox").value();
	getClientSavingsAccount(savingId);
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
function setUpCurrencies(){
	$("#tabs").show();
	dismisChequeDetails();
	document.getElementById("savingAmount").readOnly = true;
	renderGrid();
}
function displayChequeDetails(){
	$("#tabs").hide();
	document.getElementById("savingAmount").readOnly = false;
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
	var acc = $("#account").data("kendoComboBox");
	$.ajax({
        url: clientApiUrl + '/GetClientSavingsAccounts/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        savingsAccounts = data;
		acc.setDataSource(savingsAccounts);
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		//warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		warningDialog('Error retrieving data','ERROR'); 		
	});
}
function getClientSavingsAccount(id){
	$.ajax({
        url: savingApiUrl + '/GetSavingAccount/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        savingAdditional.saving = data;
		populateUi();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		//warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		warningDialog('Error retrieving data','ERROR'); 		
	});
}
function retrieveValues(){
	var sav = savingAdditional.saving;
	var paymentType = $("#paymentType").data("kendoComboBox").value();
	var withAmt = $("#savingAmount").data("kendoNumericTextBox").value();
	savingAdditional.savingAdditional.modeOfPaymentID = paymentType;
	savingAdditional.savingAdditional.naration = $("#naration").data("kendoMaskedTextBox").value();
	savingAdditional.savingAdditional.savingDate = $("#depositDate").data("kendoDatePicker").value();
	savingAdditional.savingAdditional.savingAmount = $("#savingAmount").data("kendoNumericTextBox").value();

	
	if(paymentType==2){
			savingAdditional.savingAdditional.checkNo = $("#checkNo").data("kendoMaskedTextBox").value();
			savingAdditional.savingAdditional.bankID = $("#bank").data("kendoComboBox").value();
	}else{
		savingAdditional.savingAdditionalNotes = $('#notes').data().kendoGrid.dataSource.view();
		savingAdditional.savingAdditionalCoins = $('#coins').data().kendoGrid.dataSource.view();
	}
}
function saveWithdrawal() {
    retrieveValues();
    saveToServer();
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: savingAdditionalApiUrl + '/PostSavingsAdditional',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(savingAdditional),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('saving Deposit Successful', 'SUCCESS',
		function () { window.location = '/ln/savingReports/addReceipt.aspx?id='+data.savingAdditionalID; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer
function getClients(input) {
	displayLoadingDialog();
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
			successDialog('No client found that match the searched criteria', 'SUCCESS');
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
			c.setDataSource(remainingCoins);
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
			quanCD.value(nt.quantity);
			quanBD.value(nt.quantity);
			totDep.value(0);			
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
	var quanDep = $("#quantityDep").data("kendoNumericTextBox").value();
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox").value();
	var totDep = $("#totDep").data("kendoNumericTextBox");
	
	quanCD.value(quanBD+quanDep);
	
	for (var i = 0; i < currencyNotes.length; i++) {
        if (currencyNotes[i].currencyNoteId == noteId) {
			var nt = currencyNotes[i];
			totDep.value(nt.value * quanDep);			
            break;
        }
    }
}

function getTotalDeposit(){
	var dpAmt = $("#savingAmount").data("kendoNumericTextBox");
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
			return currencyNotes[i].value * data.quantityDeposited;
		}
	}
}



