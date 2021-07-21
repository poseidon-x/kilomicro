/*
Creator: damien@acsghana.com
*/
"use strict";

var authToken = coreERPAPI_Token;
var cashierTransactionApiUrl = coreERPAPI_URL_Root + "/crud/CashierTransaction";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var currencyNoteApiUrl = coreERPAPI_URL_Root + "/crud/currencyNote";
var transactionTypeApiUrl = coreERPAPI_URL_Root + "/crud/transactionType";

var query = {};
var transactions = {};
var transactionTypes = {};
var cashierTransactions = {};
var currencies = {};
var currencyNotes = {};
var cashierFunds = {};
var notes = [];
var coins = [];

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
var transactionTypeAjax = $.ajax({
    url: transactionTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var cashierAjax = $.ajax({
    url: cashierTransactionApiUrl + '/getCurrentCashier',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

$(function () {
	displayLoadingDialog();
	loadData();
});

var currentCashier ={};

function loadData(){
	$.when(currencyAjax, currencyNoteAjax, transactionTypeAjax, cashierAjax)
        .done(function (dataCurrency, dataCurrencyNote, dataTransactionType, dataCashier) {
            currencies = dataCurrency[2].responseJSON;
            currencyNotes = dataCurrencyNote[2].responseJSON;
            transactionTypes = dataTransactionType[2].responseJSON;
			currentCashier = dataCashier[2].responseJSON;
			
			getCurrencyTypes(currencyNotes);
			getCashierNotes();
			
            //Prepares UI
			renderControls();
			dismissLoadingDialog();
        }
	);
}

//Apply kendo Style to the input fields
function renderControls() {
	$("#tabs").kendoTabStrip();
	$("#cashierName").width("95%").kendoMaskedTextBox();
	$("#fundDate").width("95%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
		change: onTransactionDateChange,
		max: new Date()
	});
	$("#fundAmount").width("95%").kendoNumericTextBox({
		decimals: 1
	});
	$("#fundBalance").width("95%").kendoNumericTextBox({
		decimals: 1
	});
}

function onTransactionDateChange(){
	query.selectedDate = $("#fundDate").data("kendoDatePicker").value();
	query.cashierName = currentCashier;
	if(query.selectedDate <= new Date()){
		
		getAllCashierTransactions(query);

	
		$.ajax({
			url: cashierTransactionApiUrl + '/GetTransactions',
			type: 'Post',
			contentType: 'application/json',
			data: JSON.stringify(query),
			beforeSend: function(req) {
				req.setRequestHeader('Authorization', "coreBearer " + authToken);
			}
		}).success(function (data) {
			dismissLoadingDialog();
			if(data!=null && data != "undefined"){
				transactions = data;
				populateUI();
				
				if(transactions.cashierReceivals != null || transactions.cashierWithdrawals != null ||
				transactions.cashierReceivals.length > 0 || transactions.cashierWithdrawals.length > 0) {
					renderGrid();
				}
			}
			else{ warningDialog('There no transaction for the selected date.','ERROR'); }
		}).error(function (xhr, data, error) {
			dismissLoadingDialog();
			warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
		});
	}
	else{
		warningDialog('There are no transactions for future date. ','WARNING');
	}
}

function populateUI() {
	$("#cashierName").data("kendoMaskedTextBox").value(transactions.cashierName);
	$("#fundAmount").data("kendoNumericTextBox").value(transactions.fundAmount);
	$("#fundBalance").data("kendoNumericTextBox").value(transactions.fundBalance);
}

function renderGrid() {
    $('#receivalGrid').kendoGrid({
            dataSource: {
				transport: {
					read: function (entries) {
						entries.success(transactions.cashierReceivals);                    
					},
					parameterMap: function (data) { return JSON.stringify(data); }
				},
				schema: {
					model: {
						id: "transactionId",
						fields: {
							transactionId: { editable: false, type: "number" },                                                    
							totalReceiptAmount: { editable: false, type: "number" },                          
							balanceBD: { editable: false, type: "number" },                          
							balanceCD: { editable: false, type: "number" },                          
							created: { type: "date", editable: false }
						}
					}
				}
			},
			columns: [
				{ field: 'transactionId', title: 'Transaction Type', template: '#= getTransactionType(transactionTypeId) #'},
				{ field: 'transactionId', title: 'Account No.', template: '#= getAccountNo(transactionTypeId, transactionId) #'},				
				{ field: 'totalReceiptAmount', title: 'Amount', format: "{0:#,###.#0}"},
				{ field: 'balanceBD', title: 'Bal BD', format: "{0:#,###.#0}"},
				{ field: 'balanceCD', title: 'Bal CD', format: "{0:#,###.#0}"},
				{ field: 'created', title: 'Time', format: "{0:HH:mm}"}
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5
			},
			detailTemplate: kendo.template($("#detail-template").html()),
			detailInit: receivalGrid_detailInit,
        }
	);
	
	$('#withdrawalGrid').kendoGrid({
            dataSource: {
				transport: {
					read: function (entries) {
						entries.success(transactions.cashierWithdrawals);                    
					},
					parameterMap: function (data) { return JSON.stringify(data); }
				},
				schema: {
					model: {
						id: "transactionTypeId",
						fields: {
							transactionTypeId: { editable: false, type: "number" },                          
							created: { type: "date", editable: false } 
						}
					}
				}
			},				
			columns: [
				{ field: 'transactionTypeId', title: 'Transaction Type', template: '#= getTransactionType(transactionTypeId) #'},
				{ field: 'transactionTypeId', title: 'Account No.', template: '#= getAccountNo(transactionTypeId, transactionId) #'},				
				{ field: 'totalReceiptAmount', title: 'Amount', format: "{0:#,###.#0}"},
				{ field: 'balanceBD', title: 'Bal BD', format: "{0:#,###.#0}"},
				{ field: 'balanceCD', title: 'Bal CD', format: "{0:#,###.#0}"},
				{ field: 'created', title: 'Time', format: "{0:HH:mm}"}
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5
			},
			detailTemplate: kendo.template($("#detail-template").html()),
			detailInit: withdrawalGrid_detailInit,			
        }
	);
}

function receivalGrid_detailInit(e) {
	var detailRow = e.detailCell;
    var model = e.data; //keep reference to the model

    detailRow.find(".tabstrip").kendoTabStrip({
        animation: {
            open: { effects: "fadeIn" }
        }
    });
	
    e.detailCell.find(".notes").kendoGrid({
        dataSource: {
			transport: {
				read: function (entries) {
					var curs = e.data.cashierTransactionReceiptCurrencies,
						notesWallet = [];
					
					for(var i=0; i < curs.length; i++) {
						if(curs[i].currencyNoteId <= 6){
							notesWallet.push(curs[i]);
						}
					};
					entries.success(notesWallet);
				},
				parameterMap: function (data) { return JSON.stringify(data); },
			},
			pageSize: 10,
			schema: {
				model: {
					id: 'currencyNoteId',
					fields: {
						currencyNoteId: { type: 'number' },
						quantity: { type: 'number' },
						total: { type: 'number' },
					}
				},
			},
		},
        scrollable: false,
        sortable: true,
		pageable: true,
        columns: [
            { field: 'currencyNoteId',title: '<b>Notes (GHS)</b>', template: '#= getNote(currencyNoteId) #' },
            { field: 'quantity', title: '<b>Quantity</b>' },            
            { field: 'total', title: '<b>Total</b>', format: "{0: #,###.#0}" }
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		mobile: true
    }).data("kendoGrid");
	
	e.detailCell.find(".coins").kendoGrid({
        dataSource: {
			transport: {
				read: function (entries) {
					var curs = e.data.cashierTransactionReceiptCurrencies,
						coinsWallet = [];
						
					for(var i=0; i < curs.length; i++) {
						if(curs[i].currencyNoteId > 6){
							coinsWallet.push(curs[i]);
						}
					};
					entries.success(coinsWallet);
				},
				parameterMap: function (data) { return JSON.stringify(data); },
			},
			pageSize: 10,
			schema: {
				model: {
					id: 'currencyNoteId',
					fields: {
						currencyNoteId: { type: 'number' },
						quantity: { type: 'number' },
						total: { type: 'number' },
					}
				},
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: '<b>Coins (Gp)</b>', template: '#= getCoin(currencyNoteId) #' },
            { field: 'quantity', title: '<b>Quantity</b>' },            
            { field: 'total', title: '<b>Total</b>', format: "{0: #,###.#0}" },            
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		mobile: true
    }).data("kendoGrid");
}

function withdrawalGrid_detailInit(e) {
	var detailRow = e.detailCell;
    var model = e.data; //keep reference to the model

    detailRow.find(".tabstrip").kendoTabStrip({
        animation: {
            open: { effects: "fadeIn" }
        }
    });
	
    e.detailCell.find(".notes").kendoGrid({
        dataSource: {
			transport: {
				read: function (entries) {
					var curs = e.data.cashierTransactionWithdrawalCurrencies,
						widthdrawalNotesWallet = [];
					
					for(var i=0; i < curs.length; i++) {
						if(curs[i].currencyNoteId <= 6){
							widthdrawalNotesWallet.push(curs[i]);
						}
					};
					entries.success(widthdrawalNotesWallet);
				},
				parameterMap: function (data) { return JSON.stringify(data); },
			},
			pageSize: 10,
			schema: {
				model: {
					id: 'currencyNoteId',
					fields: {
						currencyNoteId: { type: 'number' },
						quantity: { type: 'number' },
						total: { type: 'number' },
					}
				},
			},
		},
        scrollable: false,
        sortable: true,
        columns: [			
			{ field: 'currencyNoteId',title: '<b>Notes (GHS)</b>', template: '#= getNote(currencyNoteId) #' },
            { field: 'quantity', title: '<b>Quantity</b>' },            
            { field: 'total', title: '<b>Total</b>', format: "{0: #,###.#0}" },
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		mobile: true
    });
	
	e.detailCell.find(".coins").kendoGrid({
        dataSource: {
			transport: {
				read: function (entries) {
					var curs = e.data.cashierTransactionWithdrawalCurrencies,
						widthdrawalCoinsWallet = [];
					
					for(var i=0; i < curs.length; i++) {
						if(curs[i].currencyNoteId > 6){
							widthdrawalCoinsWallet.push(curs[i]);
						}
					};
					entries.success(widthdrawalCoinsWallet);
				},
				parameterMap: function (data) { return JSON.stringify(data); },
			},
			pageSize: 10,
			schema: {
				model: {
					id: 'currencyNoteId',
					fields: {
						currencyNoteId: { type: 'number' },
						quantity: { type: 'number' },
						total: { type: 'number' },
					}
				},
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'currencyNoteId',title: '<b>Coins (Gp)</b>', template: '#= getCoin(currencyNoteId) #' },
            { field: 'quantity', title: '<b>Quantity</b>' },            
            { field: 'total', title: '<b>Total</b>', format: "{0: #,###.#0}" },            
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		mobile: true
    });
}

function getNote(id) {
    for (var i = 0; i < notes.length; i++) {
        if (notes[i].currencyNoteId == id)
		{
			return notes[i].noteName;
		}
    }
}

function getCoin(id) {
    for (var i = 0; i < coins.length; i++) {
        if (coins[i].currencyNoteId == id)
		{
			return coins[i].noteName;
		}
    }
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

function getTransactionType(id) {
    for (var i = 0; i < transactionTypes.length; i++) {
        if (transactionTypes[i].transactionTypeId == id)
		{
			return transactionTypes[i].transactionTypeName;
		}
    }
}

function getAccountNo(transTypeID, transID) {
	var CTArray = cashierTransactions;
    for(var i=0; i < CTArray.length; i++) {
		if(CTArray[i].transTypeID == transTypeID && CTArray[i].transID == transID) {
			return CTArray[i].accountNo;
		}
	}
}

function getCashierNotes(){
	var nts = cashierFunds.cashierFundNotes;
	for(var i=0;i< 10;i++){
		for(var j=0;j<1;j++){
			if(notes[j].currencyNoteId == 0){
				remainingNotes.push(notes[j]);			
			}
		}	
	}
}

function getAllCashierTransactions(cashier){
	var CTAllModel = {};
		CTAllModel.cashierName = cashier.cashierName;
		CTAllModel.transactionDate = cashier.selectedDate;
	$.ajax({
		url: cashierTransactionApiUrl + '/getAllCashierTransactions',
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify(CTAllModel),
		beforeSend: function (req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}
	}).success(function (data) {
		cashierTransactions = data;
	}).error(function (xhr, data, error) {
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
}