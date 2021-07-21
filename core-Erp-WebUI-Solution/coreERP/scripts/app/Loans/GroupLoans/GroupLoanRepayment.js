"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var LoanGroupBatchRepaymentApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupBatchRepayment";
//
//var LoanGroupBatchRepayApiUrl = coreERPAPI_URL_Root + "/crud/LoanBatchRepay";
var LoanRepaymentTypeApiUrl = coreERPAPI_URL_Root + "/crud/LoanRepaymentType";
var paymentModeApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var bankApiUrl = coreERPAPI_URL_Root + "/crud/banks";

//Declaration of variables to store records retrieved from the database
var loanGroups = {};
var dueGroupRepayments = {};
var remainingSchedulesLoans = [];
var repaymentModel = {};
var repaymentTypes = {};
var paymentModes = {};
var banks = {};
var currentSchedule = {};

var loanGroupAjax = $.ajax({
    url: LoanGroupApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var paymentModeAjax = $.ajax({
    url: paymentModeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var bankAjax = $.ajax({
    url: bankApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var loanGroupBatchRepaymentAjax = $.ajax({
    url: LoanGroupBatchRepaymentApiUrl + '/Get',
	type: 'Get',
	contentType: "application/json",						
	beforeSend: function (req) {
		req.setRequestHeader('Authorization', "coreBearer " + authToken);
	}
});
var repaymentModelAjax = $.ajax({
    url: LoanGroupBatchRepaymentApiUrl + '/GetModel',
	type: 'Get',
	contentType: "application/json",						
	beforeSend: function (req) {
		req.setRequestHeader('Authorization', "coreBearer " + authToken);
	}
});
var repaymentTypeAjax = $.ajax({
    url: LoanRepaymentTypeApiUrl + '/Get',
	type: 'Get',
	contentType: "application/json",						
	beforeSend: function (req) {
		req.setRequestHeader('Authorization', "coreBearer " + authToken);
	}
});
var paymentModeAjax = $.ajax({
    url: paymentModeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var bankAjax = $.ajax({
    url: bankApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(loanGroupAjax, loanGroupBatchRepaymentAjax,repaymentModelAjax,repaymentTypeAjax,paymentModeAjax,bankAjax)
        .done(function (dataLoanGroup, dataLoanGroupBatchRepayment,dataRepaymentModel,dataRepaymentType,dataPaymentMode,dataBank) {
            loanGroups = dataLoanGroup[2].responseJSON;
            //remainingGroupLoans = dataLoanGroupBatchRepayment[2].responseJSON;
			repaymentModel = dataRepaymentModel[2].responseJSON;
			repaymentTypes = dataRepaymentType[2].responseJSON;
			paymentModes = dataPaymentMode[2].responseJSON;
            banks = dataBank[2].responseJSON;
			
            //Prepares UI
            prepareUi();
			dismissLoadingDialog();
        });
}

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});
function prepareUi() {
	$('#paymentDate').width('80%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]      
	});
	
	$('#group').width('80%').kendoComboBox({
		dataSource: loanGroups,
		dataValueField: 'loanGroupId',
		dataTextField: 'loanGroupName',
		filter: "contains",
		highlightFirst: true,
        suggest: true,
		ignoreCase: true,
		change: onGroupChange, 
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		optionLabel: '',
	});

	$('#tabs').kendoTabStrip();
}
var onGroupChange = function () {
    var id = $("#group").data("kendoComboBox").value();
    var exist = false;
    //Reseet Grid to empty
    $('#repaymentGrid').html('');
	
	//Retrieve value enter validate
    for (var i = 0; i < loanGroups.length; i++) {
        if (loanGroups[i].loanGroupId == id) {
            exist = true;
			getDueRepayments();
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Group', 'ERROR');
        $("#groupName").data("kendoComboBox").value("");
        $("#day").data("kendoComboBox").value("");
		//day.enable() = true;
    }
}

function getDueRepayments() {
    repaymentModel.groupId = $("#group").data("kendoComboBox").value();
    repaymentModel.repaymentDate = $("#paymentDate").data("kendoDatePicker").value();
    
	displayLoadingDialog();	
	$.ajax({
	    url: LoanGroupBatchRepaymentApiUrl + "/Get",
		type: 'POST',
        contentType: "application/json",
		data: JSON.stringify(repaymentModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }      
    }).success(function (data) {
		dismissLoadingDialog();
		if(data.length > 0)
		{
			dueGroupRepayments = data;
			populateRemainingSchedules(data);
			renderGrid();				
		}
		else{
			groups.setDataSource(loanGroups);
			warningDialog('There is no Loans for the selected Group','ERROR');
			//groups.enable(false);
		}

	}).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}

function populateRemainingSchedules(data) {	
    for (var i = 0; i < data.length; i++) {
		remainingSchedulesLoans.push(data[i]);
    }
}

//render Grid
function renderGrid() {
    $('#repaymentGrid').kendoGrid({
		dataSource: {
			transport: {
				read: function(entries) {
                    entries.success(repaymentModel.repayments);
                },
                create: function(entries) {
					var data = entries.data;
					data.amountDisbursed = $('#amountDisbursed').data('kendoNumericTextBox').value();
					data.amountDue = $('#amountDue').data('kendoNumericTextBox').value();
					//Set paymentType to interest and principal
					data.paymentTypeId = 1;
					if(data != undefined)removeLoan(data.loanId)
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
					var data = entries.data;
					if(data != undefined)addLoanBack(data.loanId)
                    entries.success();
                },
				parameterMap: function(options, operation) {
					if (operation !== "read" && options.models) {
						return {models: kendo.stringify(options.models)};
					}
				}
			},
			schema: {
				model: {
					id: "loanId",
					fields: {
						loanId: { editable: true, validation:{required: true} },
						amountDisbursed: {type:"number" },                            
						paymentTypeId: {editable: true },                            
						amountDue: {editable: true },                            
						paymentAmount: {editable: true, validation:{required: true} },                            
						cashCollateral: {editable: true, validation:{required: true} },                            
						paymentModeId: {editable: true, validation:{required: true} },                            
						bankId: {editable: true, validation:{required: false} },                            
						chequeNumber: {editable: true, validation:{required: false}}                            
					}
				}
			},
		},		
		columns: [
			{ field: 'loanId', title: 'Loan', editor: LoanEditor, template: '#= getLoan(loanId) #'},
			{ field: 'amountDisbursed', title: 'Amt. Disbursed', editor: amountDisbursedEditor, format: '{0: #,###.#0}' },							
			{ field: 'amountDue', title: 'Amt. Due',editor: amountDueEditor, format: '{0: #,###.#0}' },				
			//{ field: 'paymentTypeId', title: 'Paym. Type',editor: paymentTypeEditor, template: '#= getRepaymentType(paymentTypeId) #' },				
			{ field: 'paymentAmount', title: 'Payment Amt.',editor: paymentAmtEditor, format: '{0: #,###.#0}' },				
			{ field: 'cashCollateral', title: 'Cash Collt.',editor: collateralAmtEditor, format: '{0: #,###.#0}' },				
			{ field: 'paymentModeId', title: 'Payment Mode',editor: paymentModeEditor, template: '#= getpaymentMode(paymentModeId) #' },				
			{ field: 'bankId', title: 'Bank',editor: bankEditor, template: '#= getBank(bankId) #' },				
			{ field: 'chequeNumber', title: 'Cheque No.',editor: chequeEditor },				
			{ command: ["destroy"], width: "100px" }
		],
		navigatable: true,
		pageable: {
			pageSize: 10,
			pageSizes: [10, 25, 50, 100, 1000],
			previousNext: true,
			buttonCount: 5,
		},
		edit: function (e) {
            var editWindow = this.editable.element.data("kendoWindow");
            editWindow.wrapper.css({ width: 500 });
            editWindow.title("Edit Payment");
        },
		toolbar: [
				{
					name: "create",
					className: 'addLoan',
					text: 'Add New Repayment',
				},
				{
					name: "save",
					className: 'saveChanges',
					text: 'Pay Current Repayment',
				}
			],
		editable: "popup"
	});
	$(".saveChanges").click(function () {
		saveRepayment();
	});
}

function LoanEditor(container, options) {
    $('<input type="text" id="repaymentSchedule" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width('130%')
    .kendoComboBox({
        dataSource: remainingSchedulesLoans,
        dataValueField: "loanId",
        dataTextField: "loanNumberWithClient",
		filter: "contains",
		highlightFirst: true,
        suggest: true,
		change: onSchedulesChange, 
        optionLabel: ""
    });
}
function amountDisbursedEditor(container, options) {
    $('<input data-bind="value:'  + options.field + '" readonly id="amountDisbursed" />')
    .appendTo(container)
	.width("130%")
    .kendoNumericTextBox({
		min:0
	});
}
function principalBalEditor(container, options) {
    $('<input data-bind="value:'  + options.field + '" readonly id="amountApproved" />')
    .appendTo(container)
	.width("130%")
    .kendoNumericTextBox();
}
function paymentTypeEditor(container, options) {
    $('<input type="text" id="paymentType" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width('130%')
    .kendoComboBox({
        dataSource: repaymentTypes,
        dataValueField: "repaymentTypeID",
        dataTextField: "repaymentTypeName",
		filter: "contains",
		highlightFirst: true,
        suggest: true,
        optionLabel: "",
		//change: onPaymentTypeChange		
    });
}
function amountDueEditor(container, options) {
    $('<input data-bind="value:'  + options.field + '" readonly id="amountDue" />')
    .appendTo(container)
	.width("130%")
    .kendoNumericTextBox({});
}
function paymentAmtEditor(container, options) {
    $('<input data-bind="value:'  + options.field + '" id="paymentAmount" />')
    .appendTo(container)
	.width("130%")
    .kendoNumericTextBox({
		min:0		
	});
}
function collateralAmtEditor(container, options) {
    $('<input data-bind="value:'  + options.field + '" id="collateralAmount" />')
    .appendTo(container)
	.width("130%")
    .kendoNumericTextBox({
		min:0		
	});
}
function paymentModeEditor(container, options) {
    $('<input type="text" id="paymentMode" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width('130%')
    .kendoComboBox({
        dataSource: paymentModes,
        dataValueField: "ID",
        dataTextField: "Description",
		filter: "contains",
		highlightFirst: true,
        suggest: true,
		change: onPayModeChange, 		
        optionLabel: ""
    });
}
function bankEditor(container, options) {
    $('<input data-bind="value:' + options.field + '" id="bank"/>')
    .appendTo(container)
	.width("130%")
    .kendoComboBox({	
        dataSource: banks,
        dataValueField: "bankId",
        dataTextField: "bankName",
		filter: "contains",
		highlightFirst: true,
        suggest: true,
        optionLabel: ""
    });
}

function chequeEditor(container, options) {
    $('<input data-bind="value:' + options.field + '" id="chequeNumber"/>')
    .appendTo(container)
	.width("130%")
    .kendoMaskedTextBox({
			min: 0
	});
}
function getLoan(id){
	for (var i = 0; i < dueGroupRepayments.length; i++) {
		if (dueGroupRepayments[i].loanId == id) {
		return dueGroupRepayments[i].loanNumberWithClient;
		}
	} 
} 

function getRepaymentType(id){
	for (var i = 0; i < repaymentTypes.length; i++) {
		if (repaymentTypes[i].repaymentTypeID == id) {
		return repaymentTypes[i].repaymentTypeName;
		}
	} 
} 

function getpaymentMode(id){
	for (var i = 0; i < paymentModes.length; i++) {
		if (paymentModes[i].ID == id) {
		return paymentModes[i].Description;
		}
	} 
} 
function getBank(id) {
	var exist = false;
    for (var i = 0; i < banks.length; i++) {
        if (banks[i].bankId == id) {
			exist = true;
            return banks[i].bankName;
        }
    }
	if(!exist)return '';
}
function removeLoan(id) {
    for (var i = 0; i < remainingSchedulesLoans.length; i++) {
        if (remainingSchedulesLoans[i].loanId == id) {
			remainingSchedulesLoans.splice(i,1);
			break;
		}
    }
}

function addLoanBack(id) {
    for (var i = 0; i < dueGroupRepayments.length; i++) {
        if (dueGroupRepayments[i].loanId == id) {
			remainingSchedulesLoans.push(dueGroupRepayments[i]);
			break;
		}
    }
}

function populateRemainingClients(data) {
	remainingGroupLoans = [];
    for (var i = 0; i < data.length; i++) {
		remainingGroupLoans.push(data[i]);
    }
}

var onSchedulesChange = function () {
    var id = $("#repaymentSchedule").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < dueGroupRepayments.length; i++) {
        if (dueGroupRepayments[i].loanId == id) {
            exist = true;
			$('#amountDisbursed').data('kendoNumericTextBox').value(dueGroupRepayments[i].amountDisbursed);
			$('#amountDue').data('kendoNumericTextBox').value(dueGroupRepayments[i].amountDue);
			//$('#paymentType').data('kendoComboBox').value(1);
			//currentSchedule = dueGroupRepayments[i];
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Loan', 'ERROR');
        $("#repaymentSchedule").data("kendoComboBox").value("");
    }
}
var onPaymentTypeChange = function () {
    var id = $("#paymentType").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < repaymentTypes.length; i++) {
        if (repaymentTypes[i].repaymentTypeID == id) {
            exist = true;
			setAmountDue(id);
			
			//currentSchedule = dueGroupRepayments[i];
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Payment Type', 'ERROR');
        $("#paymentType").data("kendoComboBox").value("");
    }
}
var onPayModeChange = function () {
    var payMode = $("#paymentMode").data("kendoComboBox");
	var id = payMode.value();
	var mode = payMode.text().toLowerCase();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < paymentModes.length; i++) {
        if (paymentModes[i].ID == id) {
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Payment Mode', 'ERROR');
        $("#paymentMode").data("kendoComboBox").value("");
    }else{
		var bank = $('#bank').data('kendoComboBox');
		var cheque = $('#chequeNumber').data('kendoMaskedTextBox');
		if(mode.indexOf("cash") >= 0){
			bank.value("");
			cheque.value("");

			bank.enable(false);
			cheque.enable(false);
		}else{
			bank.enable(true);
			cheque.enable(true);
		}
	}
}
function saveRepayment() {
	var validator = $("#myform").kendoValidator().data("kendoValidator");
		
    if (validator.validate()) {
		repaymentModel.repaymentDate = $("#paymentDate").data("kendoDatePicker").value();
		repaymentModel.repayments = $("#repaymentGrid").data().kendoGrid.dataSource.view();
		if (repaymentModel.repayments.length > 0 ) {
			displayLoadingDialog();
			saveToServer();
		} else {
			smallerWarningDialog('Please add Schedule to pay for', 'NOTE');
		}
	}else{smallerWarningDialog('One or more input is empty or invalid', 'WARNING');}
}
function saveToServer() {
    $.ajax({
        url: LoanGroupBatchRepaymentApiUrl + "/PayBatch",
		type: 'POST',
        contentType: "application/json",
		data: JSON.stringify(repaymentModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }      
    }).done(function (data) {
		dismissLoadingDialog();
		successDialog('Loan Repayment Successfully Recieved',
		'SUCCESS', function () { window.location = "/dash/home.aspx"; });
	}).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}


