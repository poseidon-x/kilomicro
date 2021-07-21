"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanApiUrl = coreERPAPI_URL_Root + "/crud/LoanPendingApproval";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var loanTypeApiUrl = coreERPAPI_URL_Root + "/crud/lnType";
var interestTypeApiUrl = coreERPAPI_URL_Root + "/crud/IntrstType";
var approvalStageApiUrl = coreERPAPI_URL_Root + "/crud/AppStage";


//Declaration of variables to store records retrieved from the database
var loans = {};
var clients = {};
var loanTypes = {};
var interestTypes = {}; 
var approvalStages = {};
var profileTypes = [
    {value:"R",text:"Role"},
    {value:"U",text:"User"},
    {value:"A",text:"Access Level"}
];
var profileTypes = [
    {value:"R",text:"Role"},
    {value:"U",text:"User"},
    {value:"A",text:"Access Level"}
];
var approvalActions = [
    {value:"A",text:"Approved"},
    {value:"R",text:"Reject"},
];

var loanAjax = $.ajax({
    url: LoanApiUrl + "/GetLoansPendingMyApproval",
	type: 'Get',
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
var approvalStageAjax = $.ajax({
    url: approvalStageApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {
    $.when(loanAjax, clientAjax, loanTypeAjax, interestTypeAjax, approvalStageAjax)
	.done(function (dataLoan, dataClient, dataLoanType, dataInterestType, dataApprovalStage) {
		loans = dataLoan[2].responseJSON;
		clients = dataClient[2].responseJSON;
		loanTypes = dataLoanType[2].responseJSON;
		interestTypes = dataInterestType[2].responseJSON;
		approvalStages = dataApprovalStage[2].responseJSON;

		//Prepares UI
		renderGrid();
		dismissLoadingDialog();
	});
}

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});
//render Grid
function renderGrid() {
	$('#tabs').kendoTabStrip();
    $('#grid').kendoGrid({
            dataSource: {
                transport: {
					read: {
						url: LoanApiUrl + '/GetLoansPendingMyApproval',
						type: 'Get',
						contentType: "application/json",						
						beforeSend: function (req) {
							req.setRequestHeader('Authorization', "coreBearer " + authToken);
						}
					},
                    parameterMap: function (data) { return JSON.stringify(data); }
				},
                schema: {
                    model: {
                        id: "loanID",
                        fields: {
                            loanID: { editable: false, type: "number" },
							clientID: { editable: false },
							loanNo: { editable: false },
							loanTypeID: { editable: false },
							applicationDate: { type: "date",editable: false },                            
							amountRequested: { editable: false }
                        }
                    }
                },
            },		
			columns: [
				{ field: 'clientID', title: 'Client', template: '#= getClient(clientID) #' },			
				{ field: 'loanNo', title: 'Loan ID' },
				{ field: 'loanTypeID', title: 'Product', template: '#= getLoanType(loanTypeID) #'  },				
				{ field: 'applicationDate', title: 'Loan Date', format: '{0: dd-MMM-yyyy}' },
				{ field: 'amountRequested', title: 'Amount Requested', format: '{0: #,###,#0}' },
				{ command: [gridApproveButton], width: "100px" }
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5
			},
			detailTemplate: 'Approvals: <div class="grid"></div>',
			detailInit: grid_detailInit
		}
	);
}
var onFundingDatechange = function(){
	funding.fundingDate = $("#fundingDate").data("kendoDatePicker").value();
}

var gridApproveButton = {
		name: "gridapprove",
		text: "Approve..",
		click: function(e) {
			var tr = $(e.target).closest("tr"); // get the current table row (tr)
			var data = this.dataItem(tr);
			window.location = "/LoanSetup/LoanApproval/" + data.loanID.toString();  
		}
	};

function grid_detailInit(e) {
    e.detailRow.find(".grid").kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
						if (typeof (e.data.loanApprovals) === "undefined") {
							e.data.loanApprovals = [];
						}
						entries.success(e.data.loanApprovals);
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                        id: "loanApprovalId",
                        fields: {
                            loanApprovalId: { editable: false, type: "number" },
                            loanId: { editable: false, type: "number"  },
							approvalDate: { editable: false, type:"date" },
							approvalAction: { editable: false },
							approvedBy: { editable: false, type:"string" },
							approvalStageId: { editable: false },
							amountApproved: { editable: false }
						}
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
				{ field: 'approvalDate', title: 'Date', format: '{0: dd-MMM-yyyy}' },
				{ field: 'approvalAction', title: 'Action', template: '#= getApprovalAction(approvalAction) #' },
				{ field: 'approvedBy', title: 'Approved By' },
				{ field: 'approvalStageId', title: 'Stage', template: '#= getApprovalStage(approvalStageId) #' },
				{ field: 'amountApproved', title: 'amountApproved', format: '{0: #,##0.#0}' }
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5
        },
		mobile: true
    }).data("kendoGrid");
}
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

function ordinaryEditor(container, options) {
    $('<input id="ordinal" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: approvalLevels,
        dataValueField: "value",
        dataTextField: "text",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function profileTypeEditor(container, options) {
    $('<input id="profileType" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: profileTypes,
        dataValueField: "value",
        dataTextField: "text",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function getLoanType(id) {
    for (var i = 0; i < loanTypes.length; i++) {
        if (loanTypes[i].loanTypeID == id)
            return loanTypes[i].loanTypeName;
    }
}
function getProfileType(val) {
    for (var i = 0; i < profileTypes.length; i++) {
        if (profileTypes[i].value == val)
            return profileTypes[i].text;
    }
}
function getApprovalStage(val) {
    for (var i = 0; i < approvalStages.length; i++) {
        if (approvalStages[i].loanApprovalStageId == val)
            return approvalStages[i].name;
    }
}
function getClient(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id)
            return clients[i].clientName;
    }
}
function getApprovalAction(val) {
    for (var i = 0; i < approvalActions.length; i++) {
        if (approvalActions[i].value == val)
            return approvalActions[i].text;
    }
}
function saveApprovalStages() {
    LoanTypeApprovals = $("#approvalTypes").data().kendoGrid.dataSource.view();
    if (loansToDisburse.loans.length > 0) {
        displayLoadingDialog();
        saveToServer();
    } else {
        smallerWarningDialog('Please add Loans to disburse', 'NOTE');
    }
}

function saveToServer() {
    $.ajax({
        url: LoanTypeApiUrl + "/Post",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(LoanTypeApprovals),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        successDialog('Approval Stage(s) Saved successfully',
		'SUCCESS', function () { window.location = "/dash/home.aspx"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

