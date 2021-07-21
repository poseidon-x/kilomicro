'use strict';

var depositApiUrl = coreERPAPI_URL_Root + "/crud/Deposit";
var clientsApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var paymentModeApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var banksApiUrl = coreERPAPI_URL_Root + "/crud/banks";
var depositTypeApiUrl = coreERPAPI_URL_Root + "/crud/depositType"; 
var depositRepaymentModeApiUrl = coreERPAPI_URL_Root + "/crud/DepositRepaymentMode";
var relationshipOfficersApiUrl = coreERPAPI_URL_Root + "/crud/Staff";
var fieldAgentsApiUrl = coreERPAPI_URL_Root + "/crud/Agent";
/*var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var currencyNoteApiUrl = coreERPAPI_URL_Root + "/crud/currencyNote";
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/cashierTill";
*/
var deposit = {};
var clients = [];
var paymentModes = {};
var banks = {};
var currencies = {};
var currencyNotes = {};
var depositTypes = {};
var depositRepaymentModes = {};
var staff = {};
var fieldAgents = {};
/*
var notes = [];
var remainingNotes = [];
var coins = [];
var remainingCoins = [];
var cashierFunds = {};
var cashierFundToday = {};

var noteAdditionalId = -1;
var coinAdditionalId = -1;
*/
var depositPeriods = [
    { value:2, month:"60 Days" },
    { value:3, month:"91 Days" },
    { value:6, month:"182 Days" },	
    { value:12, month:"365 Days" }
];
var depositAjax = $.ajax({
    url: depositApiUrl + "/GetNewDeposit/",
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
var depositTypeAjax = $.ajax({
    url: depositTypeApiUrl + "/Get",
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

function loadData() {

    $.when(depositAjax, paymentModeAjax,currencyAjax, bankAjax, depositTypeAjax, relationshipOfficerAjax, 
	fieldAgentAjax)//,currencyNoteAjax,cashierFundAjax,cashierFundTodayAjax)
        .done(function (dataDeposit, dataPaymentMode, dataBank, dataDeposiType, dataRelationshipOfficer,
		dataFieldAgent) {//,dataCurrencyNote,dataCashierFund,dataCashierFundToday) {
            deposit = dataDeposit[2].responseJSON;
            paymentModes = dataPaymentMode[2].responseJSON;
            //currencies = dataCurrency[2].responseJSON;			
            banks = dataBank[2].responseJSON;
            depositTypes = dataDeposiType[2].responseJSON;
            staff = dataRelationshipOfficer[2].responseJSON;
            fieldAgents = dataFieldAgent[2].responseJSON;
			//currencyNotes = dataCurrencyNote[2].responseJSON;
            //cashierFunds = dataCashierFund[2].responseJSON;
			//cashierFundToday = dataCashierFundToday[2].responseJSON;
			//getCurrencyTypes(currencyNotes);

            dismissLoadingDialog();
			prepareUI();
        }
	);
}


$(function () {
    displayLoadingDialog();
    loadData();
});

function prepareUI() {
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
	$('#amountInvested').width('90%').kendoNumericTextBox();
	$('#depositDate').width('90%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
		change:function(e) {
			OnPeriodChange();
		} 
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
	$('#narration').width('90%').kendoMaskedTextBox();
	$('#investmentProduct').width('90%').kendoComboBox({
		dataSource: depositTypes,
		dataValueField: 'depositTypeID',
		dataTextField: 'depositTypeName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		change: onProductChange,
		optionLabel: ''
	});
	$('#relationshipOfficer').width('90%').kendoComboBox({
		dataSource: staff,
		dataValueField: 'staffId',
		dataTextField: 'staffName',
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
	$('#fieldAgent').width('90%').kendoComboBox({
		dataSource: fieldAgents,
		dataValueField: 'agentId',
		dataTextField: 'agentNameWithNo',
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
	$('#depositPeriod').width('90%').kendoComboBox({
		dataSource: depositPeriods,
		dataValueField: 'value',
		dataTextField: 'month',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: '',
		change: function(e) {
			period = this.value();
			OnPeriodChange();
		}   
	});
	$('#maturityDate').width('90%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$('#annualInterestRate').width('90%').kendoNumericTextBox({
		format: "0.#0 '%'",
		min: 0,
		change: function(e) {
			onAnnualInterestRateChange();
		}
	});
	$('#monthlyInterestRate').width('90%').kendoNumericTextBox({
		format: "0.#0 '%'",
		min: 0,
		change: function(e) {
			onMonthlyInterestRateChange();
		}
	});
	$('#interestExpected').width('90%').kendoNumericTextBox();
	$('#maturitySum').width('90%').kendoNumericTextBox();		
	
	$('#save').click(function () {
		savedeposit();
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
	$('#amountInvested').data('kendoNumericTextBox').value("");
	$("#tabs").show();
	dismisChequeDetails();
	document.getElementById("amountInvested").readOnly = true;
	renderGrid();
}
function displayChequeDetails(){
	$("#tabs").hide();
	document.getElementById("amountInvested").readOnly = false;
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
        $("#paymentType").data("kendoComboBox").value("");
		$("#checkNo").removeAttr('readonly');
		$("#bank").removeAttr('readonly');
    }
}
function renderGrid(){
	$('#notes').kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
							entries.success(deposit.depositNotes);                    
					},
					create: function (entries) {
						var data = entries.data;						
						data.depositNoteId = noteAdditionalId;
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
                    id: 'depositNoteId',
                    fields: {
						depositNoteId: { type: 'number' },
						depositID: { type: 'number' },
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
							entries.success(deposit.depositCoins);                    
					},
					create: function (entries) {
						var data = entries.data;
						data.depositCoinId = coinAdditionalId;
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
                    id: 'depositCoinId',
                    fields: {
						depositCoinId: { type: 'number' },
						depositID: { type: 'number' },
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

var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            exist = true;
			getClientPicture(id);
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}
var onProductChange = function () {
    var id = $("#investmentProduct").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < depositTypes.length; i++) {
        if (depositTypes[i].depositTypeID == id) {
            exist = true;
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Product', 'ERROR');
        $("#investmentProduct").data("kendoComboBox").value("");
    }
}


function retrieveValues() {
    deposit.clientID = $('#client').data('kendoComboBox').value();
    deposit.amountInvested = $('#amountInvested').data('kendoNumericTextBox').value();
    deposit.firstDepositDate = $('#depositDate').data('kendoDatePicker').value();
    deposit.depositTypeID = $('#investmentProduct').data('kendoComboBox').value();
    deposit.principalBalance = $('#amountInvested').data('kendoNumericTextBox').value();
    deposit.interestRate = $('#monthlyInterestRate').data('kendoNumericTextBox').value();
    deposit.period = $('#depositPeriod').data('kendoComboBox').value();
    deposit.annualInterestRate = $('#annualInterestRate').data('kendoNumericTextBox').value();
    deposit.maturityDate = $('#maturityDate').data('kendoDatePicker').value();
    deposit.staffID = $('#relationshipOfficer').data('kendoComboBox').value();
	deposit.interestExpected = $('#interestExpected').data('kendoNumericTextBox').value();
	
	var additional = {
		depositDate : $('#depositDate').data('kendoDatePicker').value(),
		depositAmount : $('#amountInvested').data('kendoNumericTextBox').value(),
		principalBalance : $('#amountInvested').data('kendoNumericTextBox').value(),
		modeOfPaymentID : $('#paymentType').data('kendoComboBox').value(),
		fxRate : $('#monthlyInterestRate').data('kendoNumericTextBox').value(),
		naration : $('#narration').data('kendoMaskedTextBox').value(),
	}
	if(additional.modeOfPaymentID ==2){
		additional.checkNo = $("#checkNo").data("kendoMaskedTextBox").value();
		additional.bankID = $("#bank").data("kendoComboBox").value();
	}else if(additional.modeOfPaymentID==1){
		deposit.depositNotes = $('#notes').data().kendoGrid.dataSource.view();
		deposit.depositCoins = $('#coins').data().kendoGrid.dataSource.view();
	}
	deposit.depositAdditionals = [];
	deposit.depositAdditionals.push(additional);
}

function savedeposit() {

    var validator = $("#myform").kendoValidator().data("kendoValidator");
    if (!validator.validate()) {
        warningDialog('One or More Fields are Empty','WARNING');
    } else {
        retrieveValues();
        saveToServer();
    }
}

function saveToServer() {
    displayLoadingDialog();
    $.ajax({
        url: depositApiUrl + "/PostDeposit",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(deposit),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		//retrive deposit Addition and get receipt;
		var firstAdditional = {};
		var additionals = data.depositAdditionals
		for(var i = 0;i<additionals.length;i++){
			firstAdditional = additionals[i];
			break;
		}
		successDialog('Investment Account successfully created. Account No: '+data.depositNo ,
            'SUCCESS', function () { window.location = '/ln/depositReports/addReceipt.aspx?id='+firstAdditional.depositAdditionalID; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

function getProduct(id){
    for (var i = 0; i < products.length; i++) {
        if (products[i].productId == id) {
            return products[i].productName;
        }
    } 
} 

var period = 0;
//Calculate Maturity Date
function OnPeriodChange(){
	period = $('#depositPeriod').data('kendoComboBox').value();
	period = parseInt(period);
	var d= $('#depositDate').data('kendoDatePicker').value();	
	
	var da = new Date(d.getFullYear(),d.getMonth(),d.getDate());
    if(da != null && da != 'undefined' && period > 0){
        var maturityDate = new Date(da);
        maturityDate.setMonth(maturityDate.getMonth() + period);
        $('#maturityDate').data('kendoDatePicker').value(maturityDate);
        calculateExpectedInterest();
    }
}

//Calculate Interest Rate
function onAnnualInterestRateChange(){
    var annualRate = $('#annualInterestRate').data('kendoNumericTextBox').value();

    if(annualRate != null && annualRate != 'undefined'){
        var monthylyRate = annualRate/12;
        $('#monthlyInterestRate').data('kendoNumericTextBox').value(monthylyRate);
        calculateExpectedInterest();
    }
}

//Calculate Interest Rate
function onMonthlyInterestRateChange(){
    var monthlyRate = $('#monthlyInterestRate').data('kendoNumericTextBox').value();

    if(monthlyRate != null && monthlyRate != 'undefined'){
        var yearlyRate = monthlyRate*12;
        $('#annualInterestRate').data('kendoNumericTextBox').value(yearlyRate);
        calculateExpectedInterest();
    }
}

//Calculate Interest Rate
function calculateExpectedInterest(){
    var rate = 0;
    var monthlyRate = $('#monthlyInterestRate').data('kendoNumericTextBox').value();
    var annualRate = $('#annualInterestRate').data('kendoNumericTextBox').value();
    var period = $('#depositPeriod').data('kendoComboBox').value();
    var amount = $('#amountInvested').data('kendoNumericTextBox').value();

    if(amount!=null&&amount!='undefined'){
        if (annualRate > 0) {
            rate = annualRate;
        } else if (monthlyRate>0) {
            rate = monthlyRate*12;
        }
        
        var periodInDays = 0;
        if(period == 2){periodInDays = 60;}
		else if(period == 3){periodInDays = 91;}
        else if(period == 6){periodInDays = 182;}
        else if(period == 12){periodInDays = 365;}
        
        var interestAmount = amount * (periodInDays / 365.0) * (rate/100.0);
        $('#interestExpected').data('kendoNumericTextBox').value(interestAmount);
        $('#maturitySum').data('kendoNumericTextBox').value(interestAmount + amount);
    }
}
function getClientPicture(id) {
    displayLoadingDialog();
    $.ajax({
        url: clientsApiUrl + "/GetClientImage/"+id,
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
        url: clientsApiUrl + '/GetAllClientsBySearch',
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
	var dpAmt = $("#amountInvested").data("kendoNumericTextBox");
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
	calculateExpectedInterest();
}

function getRowTotal(data){
	for(var i=0;i<currencyNotes.length;i++){
		if(currencyNotes[i].currencyNoteId == data.currencyNoteId){
			return currencyNotes[i].value * data.quantityDeposited;
		}
	}
}




