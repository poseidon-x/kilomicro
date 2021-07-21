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


var withdrawalModel = {};
var savingTypes = {};
var clients = {};
var currencies = {};
var modeOfpayments = {};
var currencyNotes = {};
var filteredClients = [];
var notes = [];
var remainingNotes = [];
var coins = [];
var remainingCoins = [];
var cashierFunds = {};
var savingAjax = $.ajax({
    url: savingWithdrawalApiUrl + '/Get/' + id,
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
//Load page data
function loadData() {	
    $.when(savingAjax,savingTypeAjax,clientAjax,currencyAjax,paymentModeAjax,currencyNoteAjax,cashierFundAjax)
        .done(function (dataSaving,dataSavingType,dataClient,dataCurrency,dataPaymentMode,dataCurrencyNote,dataCashierFund) {
            savingWithdrawal = dataSaving[2].responseJSON;
            savingTypes = dataSavingType[2].responseJSON;
            clients = dataClient[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
            modeOfpayments = dataPaymentMode[2].responseJSON;
            currencyNotes = dataCurrencyNote[2].responseJSON;
            cashierFunds = dataCashierFund[2].responseJSON;			
			getCurrencyTypes(currencyNotes);
			getCashierNotes();
			
            //Prepares UI
            prepareUi();
			dismissLoadingDialog();

        }
	);
}
$(function () {
    displayLoadingDialog();
    loadData();
});

function prepareUi(){
	renderControls();
	if(savingWithdrawal.savingWithdrawalID>0){
		populateUi();
	}
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
/*function myFunction(){
	alert("Key up");
}
*/

//Apply kendo Style to the input fields
function renderControls() {
	$("#client").keyup(function(){
		var txt = $("#client").data("kendoMaskedTextBox").value();
		
		if(txt.length > 2){
			var input = {searchString:txt};
			getClients(input);
		}
	});
	
	$("#tabs").kendoTabStrip();
	$("#client").width("90%")
	.kendoMaskedTextBox()
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
}
function populateUi(){
	$("#client").data("kendoComboBox").value(withdrawalModel.saving.clientID);
	$("#savingType").data("kendoComboBox").value(withdrawalModel.saving.savingTypeID);
	$("#principalBalance").data("kendoNumericTextBox").value(withdrawalModel.saving.principalBalance);
	$("#availablePrincipalBalance").data("kendoNumericTextBox").value(withdrawalModel.saving.availablePrincipalBalance);
	$("#interestBalance").data("kendoNumericTextBox").value(withdrawalModel.saving.interestBalance);
	$("#availableInterestBalance").data("kendoNumericTextBox").value(withdrawalModel.saving.availableInterestBalance);
	renderGrid();
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
						//entries.data.total = getCurrencyTotal(data.currencyNoteId,data.quantity);
						//getTotalWithdrawl();
						data.quantityWithdrawn = $("#quantityWith").data("kendoNumericTextBox").value();
						data.quantityCD = $("#quantityCD").data("kendoNumericTextBox").value();
						data.quantityBD = $("#quantityDB").data("kendoNumericTextBox").value();
						data.totalWithdrawn = $("#totWith").data("kendoNumericTextBox").value();
						removeAddedNote(data.currencyNoteId)
						entries.success(entries.data);
					},
					update: function (entries) {
						var data = entries.data;
						entries.data.total = getCurrencyTotal(data.currencyNoteId,data.quantity);
						getTotalWithdrawl();
						entries.success(entries.data);
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
						quantityWithdrawn: { type: 'number', validation: { required: true } },
						quantityCD: { type: 'number', editable:true,validation: { required: true } },
						quantityBD: { type: 'number', editable:true,validation: { required: true } },
						totalWithdrawn: { type: 'number', editable:true,validation: { required: true } },
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: 'Notes (GHS)', editor: notesEditor, template: '#= getNote(currencyNoteId) #' },
            { field: 'quantityWithdrawn', title: 'Pieces Withdrawn', editor: quantityWithdrawnEditor, format: "{0: #,###.#0}" },            
            { field: 'quantityCD', title: 'Avialable', editor: quantityCDEditor,format: "{0: #,###.#0}" },            
            { field: 'quantityBD', title: 'Balance', editor: quantityBDEditor, format: "{0: #,###.#0}" },            
            { field: 'totalWithdrawn', title: 'Amount Withdrwan',editor: totalWithEditor, format: "{0: #,###.#0}" },            
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
				text: 'Add Notes',
			}
		],
		editable: "inline",
		/*edit: function(e) {
			var grid = $("#cashierFundGrid").data("kendoGrid");
			var myVar = grid.dataItem($(this).closest("tr"));
			
			var parentData = $("#cashierFundGrid").data("kendoGrid").dataItem(e.sender.element.closest("tr").prev());
			//total = parentData.transferAmount;
		  },*/
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
						entries.data.total = getCurrencyTotal(data.currencyNoteId,data.quantity);
						removeAddedCoin(data.currencyNoteId);
						entries.success(entries.data);
					},
					update: function (entries) {
						var data = entries.data;
						entries.data.total = getCurrencyTotal(data.currencyNoteId,data.quantity);
						getTotalWithdrawl();
						removeAddedCoin(data.currencyNoteId);
						entries.success(entries.data);
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
						quantity: { type: 'number', validation: { required: true } },
						total: { type: 'number',editable: true,validation: { required: true } },
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: 'Coins (Gp)', template: '#= getCurrencyNote(currencyNoteId) #' },
            { field: 'quantity', title: 'Pieces/Quantity', format: "{0: #,###.#0}" },            
            { field: 'total', title: 'Total', format: "{0: #,###.#0}" },            
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
		/*edit: function(e) {
			var grid = $("#cashierFundGrid").data("kendoGrid");
			var myVar = grid.dataItem($(this).closest("tr"));
			
			var parentData = $("#cashierFundGrid").data("kendoGrid").dataItem(e.sender.element.closest("tr").prev());
			//total = parentData.transferAmount;
		  },*/
		mobile: true
    });
}

function notesEditor(container, options) {
    $('<input type="text" id="note"  data-bind="value:' + options.field + '"/>')
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
		noteChange,
		optionLabel: ""
    });
}
function coinsEditor(container, options) {
    $('<input  data-bind="value:' + options.field + '"/>')
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
		optionLabel: ""
    });
}
function quantityWithdrawnEditor(container, options) {
    $('<input  id="quantityWith" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 0,
		change: quantityWithChange
    });
}
function quantityCDEditor(container, options) {
    $('<input  id="quantityCD" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 1
    });
}
function quantityBDEditor(container, options) {
    $('<input  id="quantityDB" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
        min: 1
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
        warningDialog('Invalid Payment Type', 'ERROR');
        $("#paymentType").data("kendoComboBox").value("");
    }
}
var onClientChange = function(){
	var clientId = $("#client").data("kendoComboBox").value();
	getClientSavingsAccount(clientId);
}
var onPaymentTypeChange = function(){
	var payTypeId = $("#paymentType").data("kendoComboBox").value();
	var exist = false;	
	//Retrieve value enter validate
    for (var i = 0; i < modeOfpayments.length; i++) {
        if (modeOfpayments[i].ID == payTypeId) {            
			checkPaymentType(payTypeId);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Payment Type', 'ERROR');
        $("#paymentType").data("kendoComboBox").value("");
    }
}
function checkPaymentType(id){
	var chk = $("#checkNo").data("kendoMaskedTextBox");
	var bk = $("#bank").data("kendoComboBox");
	if(id == 1){
		chk.value("Not Applicable");
		bk.text("Not Applicable");
		chk.enable(false);
		bk.enable(false);
	}else{
		chk.value("");
		bk.text("");
		chk.enable(true);
		bk.enable(true);
	}
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
function getClientSavingsAccount(id){
	$.ajax({
        url: savingApiUrl + '/GetSavingAccount/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        withdrawalModel.saving = data;
		populateUi();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		//warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		warningDialog('Error retrieving data','ERROR'); 		
	});
}
function retrieveValues(){
	var sav = withdrawalModel.saving;
	withdrawalModel.savingWithdrawal.withdrawalDate = $("#withdrawalDate").data("kendoDatePicker").value();
	var withAmt = $("#withdrawalAmount").data("kendoNumericTextBox").value();
	withdrawalModel.savingWithdrawal.modeOfPaymentID = $("#paymentType").data("kendoComboBox").value();
	withdrawalModel.savingWithdrawal.naration = $("#naration").data("kendoMaskedTextBox").value();
	if(withAmt>sav.availableInterestBalance){
		withdrawalModel.savingWithdrawal.interestWithdrawal = sav.availableInterestBalance;
		withAmt -= sav.availableInterestBalance;
	}
	withdrawalModel.savingWithdrawal.principalWithdrawal = withAmt;
	withdrawalModel.savingWithdrawal.principalBalance = sav.principalBalance  - withdrawalModel.savingWithdrawal.principalWithdrawal;
	withdrawalModel.savingWithdrawal.interestBalance = sav.interestBalance - savingWithdrawal.interestWithdrawal;

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
        data: JSON.stringify(withdrawalModel),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('saving withdrawal Successful', 'SUCCESS');        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer
function getClients(input) {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetSearchedSavingClient',
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
			setUpCombox();
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
function setUpCombox(){
	$("#client")
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
			remainingNotes.push(currencyNotes[i]);			
		}
		else if(currencyNotes[i].currencyNoteTypeId == 2){
			coins.push(currencyNotes[i]);
			remainingCoins.push(currencyNotes[i]);			
		}
	}
}
function getCashierNotes(){
	var nts = cashierFunds.cashierFundNotes;
	for(var i=0;i<nts.length;i++){
		for(var j=0;j<notes.length;j++){
			if(notes[j].currencyNoteId == nts[i].currencyNoteId){
				remainingNotes.push(notes[j]);			
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
    for (var i = 0; i < notes.length; i++) {
        if (notes[i].currencyNoteId == id) {
            remainingNotes.push(notes[i]);
            break;
        }
    }
}
function addCoinBack(id) {
    for (var i = 0; i < coins.length; i++) {
        if (coins[i].currencyNoteId == id) {
            remainingCoins.push(coins[i]);
            break;
        }
    }
}
function getCashierAvialableNotes(id) {
	var quanWith = $("#quantityWith").data("kendoNumericTextBox");
	var quanCD = $("#quantityCD").data("kendoNumericTextBox");
	var quanBD = $("#quantityDB").data("kendoNumericTextBox");
	var totWit = $("#totWith").data("kendoNumericTextBox");

	var fundNotes = cashierFunds.cashierFundNotes;
    for (var i = 0; i < fundNotes.length; i++) {
        if (fundNotes[i].currencyNoteId == id) {
			var nt = fundNotes[i];
			quanWith.value(0);
			quanCD.value(nt.quantity);
			quanBD.value(nt.quantity);
			totWit.value(0);			
            break;
        }
    }
}
var currentNoteId = 0;
var quantityWithChange = function(){
	var noteId = currentNoteId;
	var quanWith = $("#quantityWith").data("kendoNumericTextBox").value();
	var quanCD = $("#quantityCD").data("kendoNumericTextBox").value();
	var quanBD = $("#quantityDB").data("kendoNumericTextBox");
	var totWit = $("#totWith").data("kendoNumericTextBox");
	
	quanBD.value(quanCD-quanWith);
	
	for (var i = 0; i < notes.length; i++) {
        if (notes[i].currencyNoteId == noteId) {
			var nt = notes[i];
			totWit.value(nt.value * quanWith);			
            break;
        }
    }
	
}


