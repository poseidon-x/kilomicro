﻿"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var LoanGroupBatchApprovalApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupBatchRepayment";

//Declaration of variables to store records retrieved from the database
var loanGroups = {};
var dueGroupApprovals = {};
var approvalModel = {};

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
	$.ajax({
        url: LoanGroupApiUrl + '/Get',
		type: 'Get',
		beforeSend: function (req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}      
    }).done(function (data) {
		dismissLoadingDialog();
		loanGroups = data;
		prepareUi();
	}).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
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
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        value: new Date()
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
		optionLabel: '',
	});
	$('#tabs').kendoTabStrip();
}
var onGroupChange = function () {
    var id = $("#group").data("kendoComboBox").value();
    var exist = false;
    //Reseet Grid to empty
    $('#approvalGrid').html('');
	
	//Retrieve value enter validate
    for (var i = 0; i < loanGroups.length; i++) {
        if (loanGroups[i].loanGroupId == id) {
            exist = true;
			getDueApprovals();
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

function getDueApprovals() {
    approvalModel.groupId = $("#group").data("kendoComboBox").value();
    approvalModel.approvalDate = $("#paymentDate").data("kendoDatePicker").value();
    
	displayLoadingDialog();	
	$.ajax({
	    url: LoanGroupBatchApprovalApiUrl + "/GetGroupsLoanPendingApproval",
		type: 'POST',
        contentType: "application/json",
		data: JSON.stringify(approvalModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }      
    }).success(function (data) {
		dismissLoadingDialog();
		if(data.length > 0)
		{
			dueGroupApprovals = data;
			renderGrid();				
		}
		else{
			//groups.setDataSource(loanGroups);
			warningDialog('There is no Loans for the selected Group ','ERROR');
			//groups.enable(false);
		}

	}).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}




//render Grid
function renderGrid() {
    $('#approvalGrid').kendoGrid({
		dataSource: {
			transport: {
				read: function(entries) {
                    entries.success(dueGroupApprovals);
                },
                update: function (entries) {
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
					    loanId: { editable: false },
						clientFullName: { editable: false },                            
						loanNumber: { editable: false },
						approvalDate: { editable: false, type: "date" },
						amountRequested: { editable: false },
						amountApproved: { editable: true },
						approved: { type: "boolean", editable: true, validation: { required: true } },
						groupName: { type: "string", editable: false, validation: { required: true } },

					    
					}
				}
			},
			aggregate: [
                    { field: "amountRequested", aggregate: "sum" },
                    { field: "amountApproved", aggregate: "sum" },

			]
		    /*
            ,
			group: [
                { field: "groupName", aggregates: [{ field: "amountDue", aggregate: "sum" }] },
			]
            */
		},		
		columns: [
			{ field: 'clientFullName', title: 'Client', width: '20%' },
			{ field: 'loanNumber', title: 'Loan No.' },
            { field: 'groupName', title: 'Group', width: '20%' },
			{ field: 'approvalDate', title: 'App Date', format: "{0: dd-MMM-yyyy}" },
			{
			    field: 'amountRequested', title: 'Amt. Requested', format: '{0: #,###.#0}', footerTemplate: "Total Requsted: #: sum #",
			    groupFooterTemplate: "Total Requested :  #= kendo.toString(sum, '0,000.00') #"
			},
            {
                field: 'amountApproved', title: 'Amt. Approved', format: '{0: #,###.#0}', footerTemplate: "Total Approved: #: sum #",
                groupFooterTemplate: "Total Approved :  #= kendo.toString(sum, '0,000.00') #"
            },
            {
                field: 'approved', title: 'Approved ?',
                template: '<input type="checkbox" #= approved ? \'checked="approved"\' : "" # disabled="disabled" />'
            },
			{ command: [{ name: "edit", text: "Mark As Approved" }],width:'20%'}
		],
		navigatable: true,
		groupable: false,
		sortable: false,
		pageable: {
			pageSize: 50,
			pageSizes: [50, 150, 300, 600, 1000],
			previousNext: true,
			buttonCount: 5,
		},
		toolbar: [				
				{
					name: "save",
					className: 'saveChanges',
					text: 'Approve Checked Items',
				}
			],
		editable: "inline"
	});
	$(".saveChanges").click(function () {
		saveApproval();
	});
}
function saveApproval() {
	var validator = $("#myform").kendoValidator().data("kendoValidator");
		
	if (validator.validate()) {
	    var grid = $('#approvalGrid').data('kendoGrid');
	    grid.dataSource.group([]);
		var approvals = $("#approvalGrid").data().kendoGrid.dataSource.view();
		if (approvals.length > 0 ) {
			displayLoadingDialog();
			saveToServer(approvals);
		} else {
			smallerWarningDialog('Please add Schedule to pay for', 'NOTE');
		}
	}else{smallerWarningDialog('One or more input is empty or invalid', 'WARNING');}
}
function saveToServer(approvalData) {
    var postModel = {
        approvalItemsList: approvalData,
        approvalDate: $('#paymentDate').data('kendoDatePicker').value()
    }
    $.ajax({
        url: LoanGroupBatchApprovalApiUrl + "/PostBatchApproval",
		type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(postModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }      
    }).done(function (data) {
		dismissLoadingDialog();
		successDialog('Loan Approval Successfully Recieved',
		'SUCCESS', function () { window.location = "/dash/home.aspx"; });
	}).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}


