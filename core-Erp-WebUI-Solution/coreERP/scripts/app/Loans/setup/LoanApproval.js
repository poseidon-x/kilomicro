"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanApiUrl = coreERPAPI_URL_Root + "/crud/LoanApproval";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var loanTypeApiUrl = coreERPAPI_URL_Root + "/crud/lnType";
var interestTypeApiUrl = coreERPAPI_URL_Root + "/crud/IntrstType";
var repaymentModeApiUrl = coreERPAPI_URL_Root + "/crud/LoanRepaymentMode";


//Declaration of variables to store records retrieved from the database
var loan = {};
var clients = {};
var loanTypes = {};
var interestTypes = {}; 
var repaymentModes = {};
var loanApprovalStages = {};
var remainingApprovalStages = [];
var myApprovalStages = {};
var myRemainingApprovalStages = {};
var myNextApprovalStage = [];
var maxLoanTypeOrdinal;
var isLastApprovalStage = false;

var approvalActions = [
    {value:"A",text:"Approved"},
    {value:"R",text:"Reject"},
];

var loanAjax = $.ajax({
    url: LoanApiUrl + "/GetLoan/"+loanID,
	type: 'Post',
	contentType: "application/json",
	beforeSend: function (req) {
		req.setRequestHeader('Authorization', "coreBearer " + authToken);
	}
});
var clientAjax = $.ajax({
    url: clientApiUrl + "/Get",
	type: 'Get',
	contentType: "application/json",
	beforeSend: function (req) {
		req.setRequestHeader('Authorization', "coreBearer " + authToken);
	}
});
var loanTypeAjax = $.ajax({
    url: loanTypeApiUrl + '/Get',
    type: 'Get',
    contentType: "application/json",
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var interestTypeAjax = $.ajax({
    url: interestTypeApiUrl + '/Get',
    type: 'Get',
    contentType: "application/json",
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var repaymentModeAjax = $.ajax({
    url: repaymentModeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var loanApprovalStageAjax = $.ajax({
    url: LoanApiUrl + '/GetLoanApprovalStages/'+loanID,
    type: 'Post',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var myLoanApprovalStageAjax = $.ajax({
    url: LoanApiUrl + '/GetMyLoanApprovalStages/'+loanID,
    type: 'Post',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {
    $.when(loanAjax, clientAjax, loanTypeAjax, interestTypeAjax, repaymentModeAjax,
			loanApprovalStageAjax, myLoanApprovalStageAjax)
        .done(function (dataLoan, dataClient, dataLoanType, dataInterestType, dataRepaymentMode, 
			dataLoanApprovalStage, dataMyLoanApprovalStages) {
				loan = dataLoan[2].responseJSON;
				clients = dataClient[2].responseJSON;
				loanTypes = dataLoanType[2].responseJSON;
				interestTypes = dataInterestType[2].responseJSON;
				repaymentModes = dataRepaymentMode[2].responseJSON;
				loanApprovalStages = dataLoanApprovalStage[2].responseJSON;
				myApprovalStages = dataMyLoanApprovalStages[2].responseJSON;
				populateRemainingStages(loan.loanApprovals);
				
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

function prepareUi(){
	$('#client').width('100%').kendoComboBox({
		dataSource: clients,
		dataValueField: 'clientID',
		dataTextField: 'clientName'
    });
	$('#loanNo').width('100%').kendoMaskedTextBox();
	$('#amountRequested').width('100%').kendoNumericTextBox();
	$('#applicationDate').width('100%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
    });
    $('#loanType').width('100%').kendoComboBox({
		dataSource: loanTypes,
		dataValueField: 'loanTypeID',
		dataTextField: 'loanTypeName'
    });
	$('#tenure').width('100%').kendoNumericTextBox({
		format: "0 'Month(s)'"
	});
	$('#interestRate').width('100%').kendoNumericTextBox({
		format: "0 '%'"
	});
	$('#interestType').width('100%').kendoComboBox({
		dataSource: interestTypes,
		dataValueField: 'interestTypeID',
		dataTextField: 'interestTypeName'
    });
	$('#repaymentMode').width('100%').kendoComboBox({
		dataSource: repaymentModes,
		dataValueField: 'repaymentModeID',
		dataTextField: 'repaymentModeName'
    });
	$('#loanScheme').width('100%').kendoComboBox({
		optionLabel: 'Not Applicable'
    });
	$('#insurance').width('100%').kendoNumericTextBox();
	if(loan.loanID > 1){
		populateUi();
		renderGrid();
	}
	
}

function populateUi(){
	$('#client').data('kendoComboBox').value(loan.clientID);
	$('#loanNo').data('kendoMaskedTextBox').value(loan.loanNo);
	$('#amountRequested').data('kendoNumericTextBox').value(loan.amountRequested);
	$('#applicationDate').data('kendoDatePicker').value(loan.applicationDate);
	$('#loanType').data('kendoComboBox').value(loan.loanTypeID);	
	$('#tenure').data('kendoNumericTextBox').value(loan.loanTenure);
	$('#interestRate').data('kendoNumericTextBox').value(loan.interestRate);
	$('#interestType').data('kendoComboBox').value(loan.interestTypeID);
	$('#repaymentMode').data('kendoComboBox').value(loan.repaymentModeID);
	
	var ordinals = [];
	for (var i = 0; i < loanApprovalStages.length; i++) {
        ordinals.push(parseInt(loanApprovalStages[i].ordinal));
    }	
	maxLoanTypeOrdinal = Math.max.apply(null, ordinals);
	if(myNextApprovalStage == maxLoanTypeOrdinal){
		isLastApprovalStage = true;
		$("#insurance").show();
	}else{
		$("#insurance").hide();
	}
}
//render Grid
function renderGrid() {
	$('#tabs').kendoTabStrip();
    $('#approvals').kendoGrid({
            dataSource: {
					transport: {
						read: function (entries) {
							entries.success(loan.loanApprovals);                    
						},
						create: function (entries) {
							var data = entries.data;
							if(data.approvedTenure == 0){
								data.approvedTenure = $("#tenur").data("kendoNumericTextBox").value();
							}
							getNextStage(data.approvalStageId,data.action)
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
							id: "loanApprovalId",
							fields: {
								loanApprovalId: { editable: false, type: "number" },
								loanId: {editable: false },
								approvalDate: {type:"date",editable: true, validation: {required :true} },                            
								approvalAction: { editable: true, validation: {required :true} },
								approvalStageId: { editable: true, validation: {required :true} },
								amountApproved: { editable: true, validation: {required :true} },  
								approvedTenure: { type:"number",editable: true, validation: {required :true} },                        																
								approvalComment: { editable: true, validation: {required :true} }                        								
							}
						}
					}
				},				
			columns: [
				{ field: 'approvalStageId', title: 'Approval Stage',editor: stageEditor, template: '#= getStage(approvalStageId) #' },
				{ field: 'approvalDate', title: 'Approval Date',editor: dateEditor, format:"{0: dd-MMM-yyyy}" },
				{ field: 'approvalAction', title: 'Action',editor: actionEditor, template: '#= getAction(approvalAction) #' },				
				{ field: 'amountApproved', title: 'Amount Approved',editor: amountEditor },
				{ field: 'approvedTenure', title: 'Tenure(Months)',editor: tenureEditor },
				{ field: 'approvalComment', title: 'Comments',editor: commentEditor, template: '#= getComment(approvalComment) #' },				
				{ command: ['edit',gridViewButton], width: "180px" },
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
					className: 'addApproval',
					text: 'Add Approval',
				},
				{
					name: "save",
					className: 'saveChanges',
					text: 'Save Approval',
				}
			],
			editable: "popup",
			edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
				editWindow.wrapper.css({ width: 500 });
                editWindow.title("Loan Approval");
            }
        });
		$(".saveChanges").click(function () {
			saveApproval();
		});
}

var gridViewButton = {
	text: "View",
	click: function(e) {
		var tr = $(e.target).closest("tr"); // get the current table row (tr)
		var data = this.dataItem(tr);
				
		$("#myModalView").kendoWindow({
			title: "Approval Comments",
			scrollable: true,
			resizable: false
		});
		var dialog = $("#myModalView").data("kendoWindow");
		dialog.center();
		dialog.content(data.approvalComment);
		dialog.setOptions({
		  //width: 400,
		  height: 150
		});
	}
};

var gridDeleteButton = {
    name: "destroy",
    text: "Delete"
};
function getClient(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id)
            return clients[i].clientNameWithAccountNO;
    }
}
function getDay(id) {
    for (var i = 0; i < days.length; i++) {
        if (days[i].loanGroupDayId == id) 
            return days[i].dayName;
    }
}
function getLoanTypeApproval(id){
    displayLoadingDialog();
    $.ajax({
        url: LoanTypeApiUrl + "/GetApproval/" + id,
        type: 'POST',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        LoanTypeApprovals = data;
        dismissLoadingDialog();
        $('#groupGrid').html("");
        renderGrid();
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

function dateEditor(container, options) {
    $('<input id="ordinal" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        }
    });
}
function actionEditor(container, options) {
    $('<input id="action" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: approvalActions,
        dataValueField: "value",
        dataTextField: "text",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		change: onActionChanged,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function stageEditor(container, options) {
	$('<input id="stage" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: myNextApprovalStage,
        dataValueField: "loanApprovalStageId",
        dataTextField: "name",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		change: onStageChange,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function amountEditor(container, options) {
	$('<input id="amount" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoNumericTextBox({
		min:0
	});
}
function tenureEditor(container, options) {
	$('<input id="tenur" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoNumericTextBox({
		min:1,
		format: "0 'Month(s)'"
	});
}
function commentEditor(container, options) {
	$('<textarea class="form-control" id="comment" data-bind="value: ' + options.field + '" cols="3" rows="4"></textarea>')
        .appendTo(container)
		.width("100%");
}
function getAction(val) {
    for (var i = 0; i < approvalActions.length; i++) {
        if (approvalActions[i].value == val)
            return approvalActions[i].text;
    }
}
function getStage(id) {
    for (var i = 0; i < loanApprovalStages.length; i++) {
        if (loanApprovalStages[i].loanApprovalStageId == id)
            return loanApprovalStages[i].name;
    }
}
function getComment(content) {
    var commentToShowOnGrid = content.substr(0,7);
	return commentToShowOnGrid+"...";
}
function populateRemainingStages(loanApproval) {
	myRemainingApprovalStages = [];
	var approvedStageIds = [];
	var remainingStageIds = [];
	if(loanApproval.length > 0){
		for (var i = 0; i < loanApproval.length; i++) {
			approvedStageIds.push(loanApproval[i].approvalStageId);
		}
	}
    for (var i = 0; i < myApprovalStages.length; i++) {
		var ind = approvedStageIds.indexOf(myApprovalStages[i].loanApprovalStageId);
        if (ind < 0)
		{myRemainingApprovalStages.push(myApprovalStages[i]);}
    }
	
	var ordinals = [];
	for (var i = 0; i < myRemainingApprovalStages.length; i++) {
        ordinals.push(parseInt(myRemainingApprovalStages[i].ordinal));
    }
	
	var nextStageOrinal = Math.min.apply(null, ordinals);
	for (var i = 0; i < myRemainingApprovalStages.length; i++) {
		if(myRemainingApprovalStages[i].ordinal == nextStageOrinal)
        myNextApprovalStage.push(myRemainingApprovalStages[i]);
    }
}
function getNextStage(id,act) {
	if(act == "R"){myNextApprovalStage = [];}
	else{
		for (var i = 0; i < myApprovalStages.length; i++) {
			if (myApprovalStages[i].loanApprovalStageId == id) {
				myApprovalStages.splice(i, 1);
				break;
			}
		}
		var approvedStageIds = [];
		myNextApprovalStage = [];
		myRemainingApprovalStages = [];
		
		for (var i = 0; i < myApprovalStages.length; i++) {
			var ind = approvedStageIds.indexOf(myApprovalStages[i].loanApprovalStageId);
			if (ind < 0)
			{myRemainingApprovalStages.push(myApprovalStages[i]);}
		}
		
		var ordinals = [];
		for (var i = 0; i < myRemainingApprovalStages.length; i++) {
			ordinals.push(parseInt(myRemainingApprovalStages[i].ordinal));
		}
		
		if(ordinals.length > 0){
			var nextStageOrinal = Math.min.apply(null, ordinals);
			for (var i = 0; i < myRemainingApprovalStages.length; i++) {
				if(myRemainingApprovalStages[i].ordinal == nextStageOrinal)
				myNextApprovalStage.push(myRemainingApprovalStages[i]);
			}
		}
	}
}

var onActionChanged = function(){
	var act = $("#action").data("kendoComboBox").value();
	var amount = $("#amount").data("kendoNumericTextBox");
	if(act == "R")
	{
		amount.enable(false);
	}else{
		amount.enable(true);
	} 
}
var onStageChange = function(){
    var id = $("#stage").data("kendoComboBox").value();
    var exist = false;
	
	var tenure = $("#tenur").data("kendoNumericTextBox");

		//Retrieve value enter validate
    for (var i = 0; i < loanApprovalStages.length; i++) {
        if (loanApprovalStages[i].loanApprovalStageId == id) {
            exist = true;
			tenure.value(loan.loanTenure);
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Satge', 'ERROR');
        $("#stage").data("kendoComboBox").value("");
		tenure.value("");
    }
}

function saveApproval() {
	loan.loanApprovals = $("#approvals").data().kendoGrid.dataSource.view();
	var approvals = loan.loanApprovals;
	var loanCurrentApprovalOrdinals = [];
	for (var i = 0; i < approvals.length; i++) {
        loanCurrentApprovalOrdinals.push(parseInt(approvals[i].approvalStageId));
    }
	var maxLoanApprovalOrdinal = Math.max.apply(null, loanCurrentApprovalOrdinals);
	for (var i = 0; i < loanApprovalStages.length; i++) {
		if(parseInt(loanApprovalStages[i].loanApprovalStageId) == maxLoanApprovalOrdinal){
			maxLoanApprovalOrdinal = parseInt(loanApprovalStages[i].ordinal);
			break;
		}
    }
	
	if(maxLoanTypeOrdinal == maxLoanApprovalOrdinal){
		loan.insuranceAmount = $('#insurance').data('kendoNumericTextBox').value();
	}
		
    if (loan.loanApprovals.length > 0) {		
        displayLoadingDialog();
        saveToServer();
    } else {
        smallerWarningDialog('Please add Approval', 'NOTE');
    }
}

function saveToServer() {
    $.ajax({
        url: LoanApiUrl + "/PostApproval",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(loan),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        successDialog('Loan Approval Saved successfully',
		'SUCCESS', function () { window.location = "/dash/home.aspx"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

