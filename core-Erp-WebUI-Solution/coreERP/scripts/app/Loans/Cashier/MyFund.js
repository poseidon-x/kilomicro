"use strict"
/* Creator: damien@acsghana.com */

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/CashierTill";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var currencyNoteApiUrl = coreERPAPI_URL_Root + "/crud/currencyNote";

var myFunds = {};
var currencies = {};
var currencyNotes = {};
var cashierFunds = {};
var cashierUsers = {};
var notes = [];
var coins = [];

var myFundAjax = $.ajax({
    url: cashierTillApiUrl + '/GetMyFunds',
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

//Function to call load form function
$(function () {
    displayLoadingDialog();
	loadData();
});

function loadData(){
	$.when(myFundAjax, currencyAjax, currencyNoteAjax)
        .done(function (dataMyFund, dataCurrency, dataCurrencyNote) {
            myFunds = dataMyFund[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
            currencyNotes = dataCurrencyNote[2].responseJSON;
			
			getCurrencyTypes(currencyNotes);
			getCashierNotes();
			
            //Prepares UI
            renderMyFundGrid();
			dismissLoadingDialog();
        }).fail(function(xhr, data, error) {
			dismissLoadingDialog();
			warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
			window.location = '/dash/home.aspx';
		});
}

function renderMyFundGrid() {
	$('#refresh').click(function (event) {
		location.reload();					
	});
	
	$("#tabs").kendoTabStrip();
	$("#tabstrip").html('<div style="padding:8px;"><b>My Funds @ ' + new Date().toJSON().slice(0,10) + '</b></div>');
    $('#myFundGrid').kendoGrid({
		dataSource: {
				transport: {
					read: function (entries) {
						entries.success(myFunds);                    
					},
					parameterMap: function (data) { return JSON.stringify(data); }
				},
				schema: {
					model: {
						id: "cashierFundId",
						fields: {
							myFunds: { editable: false},
							fundDate: { type:"date", editable: false },                            
							transferAmount: { type:"number", editable: false},  
							till: { type:"number", editable: false},  
						}
					}
				}
			},				
		columns: [
			{ field: 'cashierName', title: '<b>Cashier</b>'  },
			{ field: 'fundDate', title: '<b>Fund Date</b>', format:"{0:MMM dd, yyyy}" },
			{ field: 'fundAmount', title: '<b>Amount Funded</b>', format:'{0: #,###.#0}' },				
			{ field: 'tillData.currentBalance', title: '<b>Till Balance</b>', format:'{0: #,###.#0}' }
		],
		pageable: {
			pageSize: 10,
			pageSizes: [10, 25, 50, 100, 1000],
			previousNext: true,
			buttonCount: 5
		},
		detailTemplate: kendo.template($("#detail-template").html()),
		detailInit: fundGrid_detailInit,
	});
}

function fundGrid_detailInit(e) {
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
						var gg = e.data.tillData.cashierRemainingNotes;
						if (typeof (e.data.tillData.cashierRemainingNotes) === "undefined") {
							e.data.tillData.cashierRemainingNotes = [];
							var data = e.data.tillData.cashierRemainingNotes;
							e.data.tillData.cashierRemainingNotes = getNote(data);
						}
						entries.success(e.data.tillData.cashierRemainingNotes);
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
            schema: {
				model: {
                    id: 'cashierRemainingNoteId',
                    fields: {
						cashierFundNoteId: { type: 'number' },
						currencyNoteId: { editable: false, validation: { required: true } },
						quantity: { type: 'number', validation: { required: true } },
						total: { type: 'number',editable: false,validation: { required: true } },
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
						var gg = e.data.tillData.cashierRemainingCoins;
						if (typeof (e.data.tillData.cashierRemainingCoins) === "undefined") {
							//getNotes();
							e.data.tillData.cashierRemainingCoins = [];
							var data = e.data.tillData.cashierRemainingCoins;
							e.data.tillData.cashierRemainingCoins = getCoins(data);
						}
						entries.success(e.data.tillData.cashierRemainingCoins);
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'cashierRemainingCoinsId',
                    fields: {
						cashierFundNoteId: { type: 'number' },
						currencyNoteId: { editable: false, validation: { required: true } },
						quantity: { type: 'number', validation: { required: true } },
						total: { type: 'number',editable: false,validation: { required: true } },
					} //fields
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

function getCashierName() {
	return myFunds.cashierName;
}

function getFundDate() {
	return myFunds.fundDate;
}

function getFundAmount() {
	return myFunds.fundAmount;
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

function getNotesTotal(){
	var total = 0;
	for (var i = 0; i < myFunds.cashierFundNotes.length; i++) {
		total += myFunds.cashierFundNotes[i].total;
    }
	return total;
}

function getCoinsTotal(){
	var total = 0;
	for (var i = 0; i < myFunds.cashierFundCoins.length; i++) {
		total += myFunds.cashierFundCoins[i].total;
    }
	return total;
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
	var nts = cashierFunds.cashierFundNotes;
	for(var i=0;i< 10;i++){
		for(var j=0;j<1;j++){
			if(notes[j].currencyNoteId == 0){
				remainingNotes.push(notes[j]);			
			}
		}	
	}
}