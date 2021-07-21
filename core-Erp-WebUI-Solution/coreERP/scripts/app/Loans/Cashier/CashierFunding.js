//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: OCT 20TH, 2015  	
//*******************************************


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var cashierFundingApiUrl = coreERPAPI_URL_Root + "/crud/CashierFunding";
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/CashierTill";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var bankAccountApiUrl = coreERPAPI_URL_Root + "/crud/BankGLAccount";
var transferTypeApiUrl = coreERPAPI_URL_Root + "/crud/CashierTransferType";
var currencyNoteApiUrl = coreERPAPI_URL_Root + "/crud/currencyNote";
var cashierUserApiUrl = coreERPAPI_URL_Root + "/crud/cashierUser";


//Declaration of variables to store records retrieved from the database
var funding = {};
var cashierTills = {};
var accts = {};
var bankGLAcctIds = {};
var bankAccts = [];
var cashAccts = [];
var transferTypes = {};
var currencyNotes = {};
var fundNotes = [];
var cashiers = [];
var cashierUsers = {};
var fundedCashierTills = [];



//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


var cashierFundingAjax = $.ajax({
    url: cashierFundingApiUrl + '/GetNew' ,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var cashierTillAjax = $.ajax({
    url: cashierTillApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var cashierAjax = $.ajax({
    url: cashierUserApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var acctAjax = $.ajax({
    url: acctsApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var transferTypeAjax = $.ajax({
    url: transferTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var bankAccountAjax = $.ajax({
    url: bankAccountApiUrl + '/GetGlAcctIds',
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
function loadForm() {
	
    $.when(cashierFundingAjax,cashierTillAjax,cashierAjax,acctAjax,transferTypeAjax,bankAccountAjax,currencyNoteAjax)
        .done(function (dataCashierFunding, dataCashierTill,dataCashier,dataAcct,dataTransferType,dataBankAccount,dataCurrencyNote) {
            funding = dataCashierFunding[2].responseJSON;
            cashierTills = dataCashierTill[2].responseJSON;
            cashierUsers = dataCashier[2].responseJSON;
            accts = dataAcct[2].responseJSON;
            transferTypes = dataTransferType[2].responseJSON;
            bankGLAcctIds = dataBankAccount[2].responseJSON;
            currencyNotes = dataCurrencyNote[2].responseJSON;
			getBankGlAccounts(bankGLAcctIds);
			getCashGlAccounts();
			getCashierFullName(cashierTills);
			
            //Prepares UI
            prepareUi();
			dismissLoadingDialog();

        });
}

//Function to prepare user interface
function prepareUi() 
{	
    renderControls();
	
	$('#refresh').click(function (event) {
		location.reload();					
	});

    $('#save').click(function (event) {
		if (confirm("Are you sure you want save Till funding?")) {
            displayLoadingDialog();
			saveCashiersFunding();				
		} else
		{
            smallerWarningDialog('Please review and save later', 'NOTE');
        }
	});
}

//Apply kendo Style to the input fields
function renderControls() {
	$("#tabs").kendoTabStrip();
	getFunding();
	
}


function getFunding(){
	funding.fundingDate = new Date();
	
	$.ajax({
        url: cashierFundingApiUrl + '/Get',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(funding),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        funding = data;
		getFundedCashierTill(funding.cashierFunds)
		renderGrid();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
}

var onFundingDatechange = function(){
	funding.fundingDate = $("#fundingDate").data("kendoDatePicker").value();
	
	$.ajax({
        url: cashierFundingApiUrl + '/Get',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(funding),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        funding = data;
		renderGrid();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
}

function renderGrid() {
    $('#cashierFundGrid').kendoGrid({
            dataSource: {
					transport: {
						read: function (entries) {
							entries.success(funding.cashierFunds);                    
						},
						create: function (entries) {
							var data = entries.data;
							data.fundDate = new Date();
							entries.success(entries.data);
						},
						update: function (entries) {
							entries.success(entries.data);
						},
						destroy: function (entries) {
							entries.success(entries.data);
						},
						parameterMap: function (data) { return JSON.stringify(data); }
					},
					schema: {
						model: {
							id: "cashierFundId",
							fields: {
								cashierFundId: { editable: false, type: "number" },
								cashierTillId: { editable: true },
								fundDate: { type:"date",editable: true, validation: {required :true} },                            
								transferTypeId: { editable: true, validation: {required :true} },                            
								bankAccountId: { editable: true, validation: {required :true} },
								cashInVaultId: { editable: true, validation: {required :true} },
								transferAmount: { type:"number",editable: false, validation: {required :true} },  
							}
						}
					}
				},				
			columns: [
				{ field: 'cashierTillId', title: 'Cashier',editor: cashierTillEditor, template: '#= getCashierTill(cashierTillId) #' },
				{ field: 'transferTypeId', title: 'Account Type',editor: transferTypeEditor, template: '#= getTransferType(transferTypeId) #' },
				{ field: 'bankAccountId', title: 'Bank Account',editor: bankEditor, template: '#= getAccount(bankAccountId) #' },				
				{ field: 'cashInVaultId', title: 'Cash Account',editor: cashEditor, template: '#= getAccount(cashInVaultId) #' },
				{ field: 'transferAmount', title: 'Amount', editor: cashupAmountEditor, format:'{0: #,###.#0}' },
				{ command: ['edit'], width: "110px" },
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5
			},
			toolbar: [
				{
					name: "create",
					text: 'Fund Cashier',
				}
			],
			editable: "popup",
			edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
				editWindow.wrapper.css({ width: 500 });
                editWindow.title("Cashier Fund");
            },
			detailTemplate: kendo.template($("#detail-template").html()),
			detailInit: fundGrid_detailInit,
        }
	);
	
	$('#cashierCashupGrid').kendoGrid({
            dataSource: {
					transport: {
						read: function (entries) {
							entries.success(funding.cashierCashups);                    
						},
						create: function (entries) {
							var data = entries.data;
							data.cashupDate = new Date();
							data.transferAmount = $("#cashupAmount").data("kendoNumericTextBox").value();
							entries.success(entries.data);
						},
						update: function (entries) {
							var data = entries.data;
							data.cashupDate = new Date();
							data.transferAmount = $("#cashupAmount").data("kendoNumericTextBox").value();
							entries.success(entries.data);
						},
						destroy: function (entries) {
							entries.success(entries.data);
						},
						parameterMap: function (data) { return JSON.stringify(data); }
					},
					schema: {
						model: {
							id: "cashierCashupId",
							fields: {
								cashierCashupId: { editable: false, type: "number" },
								cashierTillId: {editable: true, validation: {required :true} },
								transferTypeId: {editable: true, validation: {required :true} },
								cashupDate: { type:"date",editable: true, validation: {required :true} },                            								
								bankAccountId: { editable: true, validation: {required :true} },
								cashInVaultId: { editable: true, validation: {required :true} },
								transferAmount: { editable: true, validation: {required :true} },  
							}
						}
					}
				},				
			columns: [
				{ field: 'cashierTillId', title: 'Cashier',editor: CashupCashierTillEditor, template: '#= getCashierTill(cashierTillId) #' },
				{ field: 'transferTypeId', title: 'Account Type',editor: transferTypeEditor, template: '#= getTransferType(transferTypeId) #' },
				{ field: 'bankAccountId', title: 'Bank Account',editor: bankEditor, template: '#= getAccount(bankAccountId) #' },				
				{ field: 'cashInVaultId', title: 'Cash Account',editor: cashEditor, template: '#= getAccount(cashInVaultId) #' },
				{ field: 'transferAmount', title: 'Amount', editor: cashupAmountEditor, format:'{0: #,###.#0}' },
				{ command: ['edit'], width: "120px" },
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5
			},
			toolbar: [
				{
					name: "create",
					text: 'Cashup Cashier',
				}
			],
			editable: "popup",
			edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
				editWindow.wrapper.css({ width: 500 });
                editWindow.title("Cashier Cashup");
            },
			detailTemplate: kendo.template($("#detail-template").html()),
			detailInit: cashupGrid_detailInit,			
        }
	);
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
						var gg = e.data.cashierFundNotes;
						if (typeof (e.data.cashierFundNotes) === "undefined") {
							e.data.cashierFundNotes = [];
							var data = e.data.cashierFundNotes;
							e.data.cashierFundNotes = getNotes(data);
						}
						entries.success(e.data.cashierFundNotes);
					},
					create: function (entries) {
						//var data = entries.data;
						//entries.data.total = getCurrencyTotal(data.currencyNoteId,data.quantity);
						//total += data.total;
						//e.data.transferAmount = total;
						entries.success(entries.data);
					},
					update: function (entries) {
						var data = entries.data;
						entries.data.total = getCurrencyTotal(data.currencyNoteId,data.quantity);
						entries.success(entries.data);
					},
					destroy: function (entries) {
						entries.success();
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
            schema: {
				model: {
                    id: 'cashierFundNoteId',
                    fields: {
						cashierFundNoteId: { type: 'number' },
						cashierFundId: { type: 'number' },
						currencyNoteId: { editable: false, validation: { required: true } },
						quantity: { type: 'number', validation: { required: true } },
						total: { type: 'number',editable: false,validation: { required: true } },
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
		pageable: true,
		editable: "inline",
        columns: [
            { field: 'currencyNoteId',title: 'Notes (GHS)', template: '#= getCurrencyNote(currencyNoteId) #' },
            { field: 'quantity', title: 'Pieces/Quantity', format: "{0: #,###.#0}" },            
            { field: 'total', title: 'Total', format: "{0: #,###.#0}" },            
            { command: [{name:"edit",text:"Edit"}],width:110 }
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
						var gg = e.data.cashierFundCoins;
						if (typeof (e.data.cashierFundCoins) === "undefined") {
							//getNotes();
							e.data.cashierFundCoins = [];
							var data = e.data.cashierFundCoins;
							e.data.cashierFundCoins = getCoins(data);
						}
						entries.success(e.data.cashierFundCoins);
					},
					create: function (entries) {
						entries.success(entries.data);
					},
					update: function (entries) {
						var data = entries.data;
						entries.data.total = getCurrencyTotal(data.currencyNoteId,data.quantity);
						//total += data.total;
						//e.data.transferAmount = total;
						entries.success(entries.data);
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'cashierFundNoteId',
                    fields: {
						cashierFundNoteId: { type: 'number' },
						cashierFundId: { type: 'number' },
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
		editable: "inline",
		edit: function(e) {
			var grid = $("#cashierFundGrid").data("kendoGrid");
			var myVar = grid.dataItem($(this).closest("tr"));
			
			var parentData = $("#cashierFundGrid").data("kendoGrid").dataItem(e.sender.element.closest("tr").prev());
			//total = parentData.transferAmount;
		  },
		mobile: true
    }).data("kendoGrid");
}

function cashupGrid_detailInit(e) {
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
						if (typeof (e.data.cashierCashupNotes) === "undefined") {														
							e.data.cashierCashupNotes = [];
							var data = e.data.cashierCashupNotes;
							e.data.cashierCashupNotes = getCashupNotes(data,e.data.cashierTillId);
						}
						entries.success(e.data.cashierCashupNotes);
					},/*
					create: function (entries) {
						var data = entries.data;
						entries.data.total = getCurrencyTotal(data.currencyNoteId,data.quantity);
						//total += data.total;
						//e.data.transferAmount = total;
						entries.success(entries.data);
					},
					update: function (entries) {
						entries.success(entries.data);
					},*/
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'cashierCashupNoteId',
                    fields: {
						cashierCashupNoteId: { type: 'number' },
						cashierCashupId: { type: 'number' },
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
            { field: 'currencyNoteId',title: 'Notes (GHS)', template: '#= getCurrencyNote(currencyNoteId) #' },
            { field: 'quantity', title: 'Pieces/Quantity', format: "{0: #,###.#0}" },            
            { field: 'total', title: 'Total', format: "{0: #,###.#0}" },            
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },/*
		editable: "inline",
		edit: function(e) {
			var grid = $("#cashierFundGrid").data("kendoGrid");
			var myVar = grid.dataItem($(this).closest("tr"));
			
			var parentData = $("#cashierFundGrid").data("kendoGrid").dataItem(e.sender.element.closest("tr").prev());
			//total = parentData.transferAmount;
		  },*/
		mobile: true
    });
	
	e.detailCell.find(".coins").kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
						if (typeof (e.data.cashierCashupCoins) === "undefined") {
							//getNotes();
							e.data.cashierCashupCoins = [];
							var data = e.data.cashierCashupCoins;
							e.data.cashierCashupCoins = getCashupCoins(data,e.data.cashierTillId);
						}
						entries.success(e.data.cashierCashupCoins);
					},/*
					create: function (entries) {
						var data = entries.data;
						entries.data.total = getCurrencyTotal(data.currencyNoteId,data.quantity);
						//total += data.total;
						e.data.transferAmount = total;
						entries.success(entries.data);
					},
					update: function (entries) {
						entries.success(entries.data);
					},*/
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'cashierCashupNoteId',
                    fields: {
						cashierCashupNoteId: { type: 'number' },
						cashierCashupId: { type: 'number' },
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
            { field: 'currencyNoteId',title: 'Coins (Gp)', template: '#= getCurrencyNote(currencyNoteId) #' },
            { field: 'quantity', title: 'Pieces/Quantity', format: "{0: #,###.#0}" },            
            { field: 'total', title: 'Total', format: "{0: #,###.#0}" },            
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },/*
		editable: "inline",
		edit: function(e) {
			var grid = $("#cashierFundGrid").data("kendoGrid");
			var myVar = grid.dataItem($(this).closest("tr"));
			
			var parentData = $("#cashierFundGrid").data("kendoGrid").dataItem(e.sender.element.closest("tr").prev());
			//total = parentData.transferAmount;
		  },*/
		mobile: true
    });
}


function cashierTillEditor(container, options) {
    $('<input  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: cashierTills,
		filter: "contains",
		suggest: true,
		dataValueField: "cashiersTillID",
		dataTextField: "userName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
    });
}
function CashupCashierTillEditor(container, options) {
    $('<input id="cashupCashierTill" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: fundedCashierTills,
		filter: "contains",
		suggest: true,
		dataValueField: "cashiersTillID",
		dataTextField: "userName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		change: cashierChange,
		ignoreCase: true,
		optionLabel: ""
    });
}
function transferTypeEditor(container, options) {
    $('<input id="transferType" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: transferTypes,
		filter: "contains",
		suggest: true,
		dataValueField: "cashierTransferTypeId",
		dataTextField: "transferTypeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onTransferTypeChange,
		optionLabel: ""
    });
}
function bankEditor(container, options) {
    $('<input id="bank" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        filter: "contains",
		suggest: true,
		dataValueField: "acct_id",
		dataTextField: "acc_name",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: "",
		optionLabel: ""
    });
}
function cashEditor(container, options) {
    $('<input id="cash" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        filter: "contains",
		suggest: true,
		dataValueField: "acct_id",
		dataTextField: "acc_name",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: "",
		optionLabel: ""
    });
}
function cashupAmountEditor(container, options) {
    $('<input readonly id="cashupAmount" data-bind="value:'  + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoNumericTextBox();
}

var onTransferTypeChange = function () {
    var id = $("#transferType").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < transferTypes.length; i++) {
        if (transferTypes[i].cashierTransferTypeId == id) {
            setUpAccountComboBox(id);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Transfer Type', 'ERROR');
        $("#transferType").data("kendoComboBox").value("");
    }
}
var cashierChange = function () {
    var id = $("#cashupCashierTill").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < fundedCashierTills.length; i++) {
        if (fundedCashierTills[i].cashiersTillID == id) {
            $("#cashupAmount").data("kendoNumericTextBox").value(fundedCashierTills[i].currentBalance);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Cashier', 'ERROR');
        $("#cashupCashierTill").data("kendoComboBox").value("");
    }
}
function getFundedCashierTill(fundedTills) {
	for(var j=0;j<fundedTills.length;j++)
	{
		var tilId = fundedTills[j].cashierTillId;
		for (var i = 0; i < cashierTills.length; i++) {
			if(cashierTills[i].cashiersTillID == tilId){
				fundedCashierTills.push(cashierTills[i]);
			}		
		}
	}
}
function getCashierFullName(cashierTills) {
    for (var i = 0; i < cashierTills.length; i++) {
		var cashier = {
			tillId: cashierTills[i].cashiersTillID,
			fullName: getName(cashierTills[i].userName)
		};
		cashiers.push(cashier);
	}
}
function getName(name) {
    for (var i = 0; i < cashierUsers.length; i++) {
        if (cashierUsers[i].cashierUserName == name)
            return cashierUsers[i].cashierFullName;
    }
}
function getCashierTill(id) {
    for (var i = 0; i < cashiers.length; i++) {
        if (cashiers[i].tillId == id)
            return cashiers[i].fullName;
    }
}
function getTransferType(id) {
    for (var i = 0; i < transferTypes.length; i++) {
        if (transferTypes[i].cashierTransferTypeId == id)
            return transferTypes[i].transferTypeName;
    }
}
function getAccount(id) {
    for (var i = 0; i < accts.length; i++) {
        if (accts[i].acct_id == id)
            return accts[i].acc_name;
    }
	return "";
}
function getCurrencyNote(id) {
    for (var i = 0; i < currencyNotes.length; i++) {
        if (currencyNotes[i].currencyNoteId == id)
            return currencyNotes[i].noteName;
    }
}
function getCoins(data){
	for (var i = 0; i < currencyNotes.length; i++) {
		if(currencyNotes[i].currencyNoteTypeId == 2){
			var note = {
				cashierFundNoteId:-(i+1),
				cashierFundId:0,
				currencyNoteId: currencyNotes[i].currencyNoteId,
				quantity: 0,
				total:0
			};
			data.push(note);
		}
    }
	return data;
}
function getNotes(data){
	for (var i = 0; i < currencyNotes.length; i++) {
		if(currencyNotes[i].currencyNoteTypeId == 1){
			var note = {
				cashierFundNoteId:-(i+1),
				cashierFundId:0,
				currencyNoteId: currencyNotes[i].currencyNoteId,
				quantity: 0,
				total:0
			};
			data.push(note);
		}
    }
	return data;
}
function getCashupCoins(data,tillId){
	for (var i = 0; i < cashierTills.length; i++) {
		if(cashierTills[i].cashiersTillID == tillId){
			var tl = cashierTills[i];
			
			for(var j=0;j<tl.cashierRemainingCoins.length;j++){
				var remCns = tl.cashierRemainingCoins[j];
				var coin = {
					cashierCashupNoteId:-(i+1),
					cashierCashupId:0,
					currencyNoteId: remCns.currencyNoteId,
					quantity: remCns.quantity,
					total:remCns.total
				};
				data.push(coin);
			}
			
			break;
		}
    }
	return data;
}
function getCashupNotes(data,tillId){
	for (var i = 0; i < cashierTills.length; i++) {
		if(cashierTills[i].cashiersTillID == tillId){
			var tl = cashierTills[i];
			
			for(var j=0;j<tl.cashierRemainingNotes.length;j++){
				var remNt = tl.cashierRemainingNotes[j];
				var note = {
					cashierCashupNoteId:-(i+1),
					cashierCashupId:0,
					currencyNoteId: remNt.currencyNoteId,
					quantity: remNt.quantity,
					total:remNt.total
				};
				data.push(note);
			}
			
			break;
		}
    }
	return data;
}
function getCurrencyTotal(id,quantity){
	for (var i = 0; i < currencyNotes.length; i++) {
        if (currencyNotes[i].currencyNoteId == id)
            return currencyNotes[i].value * quantity;
    }
}
//retrieve values from from Input Fields and save 
function saveCashiersFunding() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {	
    funding.cashierFunds = $("#cashierFundGrid").data().kendoGrid.dataSource.view();
    funding.cashierCashups = $("#cashierCashupGrid").data().kendoGrid.dataSource.view();
}

//Save to server function
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: cashierFundingApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(funding),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Funding Successful:', 'SUCCESS', function () { window.location = '/Cashier/CashierFunding'; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer

function getBankGlAccounts(data){
	for(var i =0;i<data.length;i++){
		for(var j =0;j<accts.length;j++){
			if(accts[j].acct_id == data[i]) 
			{
				bankAccts.push(accts[j]);
				break;
			}
		}
	}
}

function getCashGlAccounts(){
	for(var i =0;i<accts.length;i++){
		var str = accts[i].acc_name;
		if(str.search(/cash in vault/i)>-1) 
		{
			cashAccts.push(accts[i]); 
		}
	}
}

function setUpAccountComboBox(id){
	var bank = $("#bank").data("kendoComboBox");
	var cash = $("#cash").data("kendoComboBox");

	if(id == 1){
		cash.setDataSource(cashAccts);
		bank.enable(false);
		cash.enable(true);
	}else if(id == 2){
		bank.setDataSource(bankAccts);
		cash.enable(false);
		bank.enable(true);
	}
}

