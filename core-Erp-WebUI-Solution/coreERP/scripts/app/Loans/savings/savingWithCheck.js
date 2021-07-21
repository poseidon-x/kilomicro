/*
UI Scripts for Loan saving Management
Creator: man@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var savingApiUrl = coreERPAPI_URL_Root + "/crud/saving";
var savingTypeApiUrl = coreERPAPI_URL_Root + "/crud/savingType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var depositRepaymentModeApiUrl = coreERPAPI_URL_Root + "/crud/DepositRepaymentMode";
var fieldAgentsApiUrl = coreERPAPI_URL_Root + "/crud/Agent";
var relationshipOfficersApiUrl = coreERPAPI_URL_Root + "/crud/Staff";
var banksApiUrl = coreERPAPI_URL_Root + "/crud/banks";
var paymentModeApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var currencyNoteApiUrl = coreERPAPI_URL_Root + "/crud/currencyNote";
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/cashierTill";



var saving = {};
var savingTypes = {};
var clients = {};
var paymentModes = {};
var banks = {};
var depositRepaymentModes = {};
var ralationOfficers = {};
var agents = {};
var currencies = {};
var currencyNotes = {};
var staff = {};
var notes = [];
var remainingNotes = [];
var coins = [];
var remainingCoins = [];
var cashierFunds = {};
var cashierFundToday = {};

var noteAdditionalId = -1;
var coinAdditionalId = -1;

var savingAjax = $.ajax({
    url: savingApiUrl + '/GetNewSavingAccount/',
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
    url: clientApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var paymentModeAjax = $.ajax({
    url: paymentModeApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var bankAjax = $.ajax({
    url: banksApiUrl + "/Get",
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
var depositRepaymentModeAjax = $.ajax({
    url: depositRepaymentModeApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var relationshipOfficerAjax = $.ajax({
    url: relationshipOfficersApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var fieldAgentAjax = $.ajax({
    url: fieldAgentsApiUrl + "/Get",
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
    $.when(savingAjax,savingTypeAjax,currencyAjax,clientAjax,paymentModeAjax,bankAjax,currencyAjax,depositRepaymentModeAjax,
	relationshipOfficerAjax,fieldAgentAjax,currencyNoteAjax,cashierFundAjax,cashierFundTodayAjax)
        .done(function (dataSaving,dataSavingType,dataCurrency,dataClient,dataPaymentMode,dataBank,dataCurrency,dataDepositRepaymentMode,
		dataRelationshipOfficer,dataFieldAgent,dataCurrencyNote,dataCashierFund,dataCashierFundToday) {
            saving = dataSaving[2].responseJSON;
            savingTypes = dataSavingType[2].responseJSON;
            clients = dataClient[2].responseJSON;
            paymentModes = dataPaymentMode[2].responseJSON;
            banks = dataBank[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
			depositRepaymentModes = dataDepositRepaymentMode[2].responseJSON;
			ralationOfficers = dataRelationshipOfficer[2].responseJSON;
			agents = dataFieldAgent[2].responseJSON;
            currencyNotes = dataCurrencyNote[2].responseJSON;
            cashierFunds = dataCashierFund[2].responseJSON;
            cashierFundToday = dataCashierFundToday[2].responseJSON;
			getCurrencyTypes(currencyNotes);

			//Prepares UI
			dismissLoadingDialog();
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
	renderControls();
	if(saving.savingID>0){
		populateUi();
	}
    $('#save').click(function (event) {
		if (confirm("Are you sure, you want to save this account")) {
            displayLoadingDialog();
			saveAccount();				
		} else
		{
            smallerWarningDialog('Please review and save later', 'NOTE');
        }
	});
}
//Apply kendo Style to the input fields
function renderControls() {
	$("#tabs").kendoTabStrip();
	$("#client").keyup(function(){
		var txt = $("#client").data("kendoMaskedTextBox").value();
		
		if(txt.length > 2){
			var input = {searchString:txt};
			getClients(input);
		}
	});
	$("#client").width("90%")
	.kendoMaskedTextBox();
	$("#firstSavingDate").width("90%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
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
	$('#paymentType').width('90%').kendoComboBox({
		dataSource: paymentModes,
		dataValueField: 'ID',
		dataTextField: 'Description',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onPaymentTypChange,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
	});
	$("#annualInterestRate").width("90%")
	.kendoNumericTextBox({
		format: "0.#0 '%'",
		min: 0,
		change: function(e) {
			onAnnualInterestRateChange();
		}
	});
	$("#monthlyInterestRate").width("90%")
	.kendoNumericTextBox({
		format: "0.#0 '%'",
		min: 0,
		change: function(e) {
			onMonthlyInterestRateChange();
		}
	});
	$('#checkNo').width('90%').kendoMaskedTextBox();
	$('#bank').width('90%').kendoComboBox({
		dataSource: banks,
		dataValueField: 'bankId',
		dataTextField: 'bankName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
	});
	$("#savingAmount").width("90%")
	.kendoNumericTextBox({
		min: 0,
		change: function(e) {
			//onAnnualInterestRateChange();
		}
	});
	$('#naration').width('90%').kendoMaskedTextBox();
	$("#principalRepaymentMode").width("90%")
	.kendoComboBox({
		dataSource: depositRepaymentModes,
		filter: "contains",
		suggest: true,
		dataValueField: "repaymentModeDays",
		dataTextField: "repaymentModeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#interestRepaymentMode").width("90%")
	.kendoComboBox({
		dataSource: depositRepaymentModes,
		filter: "contains",
		suggest: true,
		dataValueField: "repaymentModeDays",
		dataTextField: "repaymentModeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#agent").width("90%")
	.kendoComboBox({
		dataSource: agents,
		filter: "contains",
		suggest: true,
		dataValueField: "agentId",
		dataTextField: "agentNameWithNo",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#staff").width("90%")
	.kendoComboBox({
		dataSource: ralationOfficers,
		filter: "contains",
		suggest: true,
		dataValueField: "staffId",
		dataTextField: "staffName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true
	});
	dismisChequeDetails();
	$("#tabs").hide();
	$("#save").hide();
	
}
function dismisChequeDetails(){
	$("#checkNo").data("kendoMaskedTextBox").value("");
	$("#bank").data("kendoComboBox").value("");
	$("#chequeDetails").hide();	
}
function setUpCurrencies(){
	$('#savingAmount').data('kendoNumericTextBox').value("");
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
var onPaymentTypChange = function () {
    var id = $("#paymentType").data("kendoComboBox").value();
    var exist = false;
	
	var cheque = $("#checkNo").data("kendoMaskedTextBox");
	var bank = $("#bank").data("kendoComboBox");

	//Retrieve value enter validate
    for (var i = 0; i < paymentModes.length; i++) {
        if (paymentModes[i].ID == id) {
			if(id == 1)
			{setUpCurrencies();}
			else if(id)
			{displayChequeDetails();}
			$("#save").show();
            exist = true;
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid paymeny type', 'ERROR');
    }
}

function renderGrid(){
	$('#notes').kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
							entries.success(saving.savingNotes);                    
					},
					create: function (entries) {
						var data = entries.data;						
						data.savingNoteId = noteAdditionalId;
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
                    id: 'savingNoteId',
                    fields: {
						savingNoteId: { type: 'number' },
						savingID: { type: 'number' },
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
            { field: 'quantityDeposited', title: 'Pieces Received', editor: quantityDepositedEditor, format: "{0: #,###}" },  
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}" },            			
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}" },            
            { field: 'totalDeposited', title: 'Amount Received (GHS)',editor: totalDepositedEditor, format: "{0: #,###.#0}" },            
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
							entries.success(saving.savingCoins);                    
					},
					create: function (entries) {
						var data = entries.data;
						data.savingCoinId = coinAdditionalId;
						coinAdditionalId += -1;
						data.quantityDeposited = $("#quantityDep").data("kendoNumericTextBox").value();
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
                    id: 'savingCoinId',
                    fields: {
						savingCoinId: { type: 'number' },
						savingID: { type: 'number' },
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
            { field: 'quantityDeposited', title: 'Pieces Received', editor: quantityDepositedEditor, format: "{0: #,###}" },            
            { field: 'quantityBD', title: 'Pieces B/D', editor: quantityBDEditor, format: "{0: #,###}" }, 
            { field: 'quantityCD', title: 'Pieces C/D', editor: quantityCDEditor,format: "{0: #,###}" },            			
            { field: 'totalDeposited', title: 'Amount Received (GHS)',editor: totalDepositedEditor, format: "{0: #,###.#0}" },            
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

function retrieveValues(){
	saving.clientID = $("#client").data("kendoComboBox").value();
	saving.firstSavingDate = $("#firstSavingDate").data("kendoDatePicker").value();
	saving.savingTypeID = $("#savingType").data("kendoComboBox").value();
	saving.interestRate = $("#monthlyInterestRate").data("kendoNumericTextBox").value();
	saving.annualInterestRate = $("#annualInterestRate").data("kendoNumericTextBox").value();
	saving.interestRepaymentModeID = $("#interestRepaymentMode").data("kendoComboBox").value();
	saving.principalRepaymentModeID = $("#principalRepaymentMode").data("kendoComboBox").value();	
	var paymentTypeId = $("#paymentType").data("kendoComboBox").value();
	saving.agentId = $("#agent").data("kendoComboBox").value();
	saving.staffID = $("#staff").data("kendoComboBox").value();
	
	var additional = {
		savingDate : $('#firstSavingDate').data('kendoDatePicker').value(),
		savingAmount : $('#savingAmount').data('kendoNumericTextBox').value(),
		modeOfPaymentID : $('#paymentType').data('kendoComboBox').value(),
		fxRate : $('#monthlyInterestRate').data('kendoNumericTextBox').value(),
		naration : $('#naration').data('kendoMaskedTextBox').value(),
	}
	if(paymentTypeId == 2){
		additional.checkNo = $("#checkNo").data("kendoMaskedTextBox").value();
		additional.bankID = $("#bank").data("kendoComboBox").value();
	}else{
		saving.savingNotes = $('#notes').data().kendoGrid.dataSource.view();
		saving.savingCoins = $('#coins').data().kendoGrid.dataSource.view();		
	}
	saving.savingAdditionals = [];
	saving.savingAdditionals.push(additional);
}
function saveAccount() {
    retrieveValues();
    saveToServer();
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: savingApiUrl + '/PostSavings',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(saving),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		//retrive Saving Addition and get receipt;
		var firstAdditional = {};
		var additionals = data.savingAdditionals
		for(var i = 0;i<additionals.length;i++){
			firstAdditional = additionals[i];
			break;
		}
        successDialog('Savings Account Successfully Saved. A/C No: '+ data.savingNo
		, 'SUCCESS', function () { window.location = '/ln/savingReports/addReceipt.aspx?id='+firstAdditional.savingAdditionalID; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer

function onAnnualInterestRateChange(){
    var annualRate = $('#annualInterestRate').data('kendoNumericTextBox').value();

    if(annualRate != null && annualRate != 'undefined'){
        var monthylyRate = annualRate/12;
        $('#monthlyInterestRate').data('kendoNumericTextBox').value(monthylyRate);
    }
}

//Calculate Interest Rate
function onMonthlyInterestRateChange(){
    var monthlyRate = $('#monthlyInterestRate').data('kendoNumericTextBox').value();

    if(monthlyRate != null && monthlyRate != 'undefined'){
        var yearlyRate = monthlyRate*12;
        $('#annualInterestRate').data('kendoNumericTextBox').value(yearlyRate);
    }
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
function getClients(input) {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetAllClientsBySearch',
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
			clients = data;
			setUpClientCombox();
		}
		else{
			clients = [];
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
		dataSource: clients,
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
function displayClientImage(data){
	$("#clientImage").attr("src","data:image/png;base64,"+data);
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
var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            exist = true;
			$("#firstSavingDate").data("kendoDatePicker").value(new Date());
			getClientPicture(id);
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
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
