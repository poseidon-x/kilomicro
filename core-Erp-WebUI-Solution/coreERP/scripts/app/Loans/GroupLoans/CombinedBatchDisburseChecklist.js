"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var CombinedDisburseApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupBatchRepayment";

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
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
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
    //Reset Grid to empty
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
        url: CombinedDisburseApiUrl + "/GetBatchCombinedDisburseChecklist",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(approvalModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        if (data.length > 0) {
            dueGroupApprovals = data;
            renderGrid();
        }
        else {
            warningDialog('There is no Loans or Undisbursed Loans for the selected Group ', 'ERROR');
        }

    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}




//render Grid
function renderGrid() {
    $('#approvalGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(dueGroupApprovals);
                },
                update: function (entries) {
                    entries.success(entries.data);
                },
                destroy: function (entries) {
                    entries.success(entries.data);
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
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
                        amountDisbursed: { type: "number", editable: true },
                        amountRequested: { editable: false },
                        approved: { type: "boolean", editable: true, validation: { required: true } }
                    }
                }
            },
            aggregate: [
                    { field: "amountDisbursed", aggregate: "sum" }
            ]
            
        },
        columns: [
			{ field: 'clientFullName', title: 'Client', width: '18%' },
			{ field: 'loanNumber', title: 'Loan No.' },
			{
			    field: 'approvalDate',
			    title: 'Disb. Date',
			    format: "{0: dd-MMM-yyyy}",
			    width: '10%'
			},
            {
                field: 'amountRequested',
                title: 'Amt. Requested',
                format: '{0: #,###.#0}',
                width: '10%'
            },
			{
			    field: 'amountDisbursed',
			    title: 'Amt. Disbursed',
			    format: '{0: #,###.#0}',
			    footerTemplate: "Total Disbursed: #: sum #",
			    groupFooterTemplate: "Total To Disburse :  #= kendo.toString(sum, '0,000.00') #",
			    width: '10%'
			},
            {
                field: 'approved', title: 'Disburse ?',
                template: '<input type="checkbox" #= approved ? \'checked="approved"\' : "" # disabled="disabled" />'
            },
			{
			    command: [
                    {
                        name: "edit",
                        text: "Mark As Disbursed"
                    }
			    ],
			    width: '20%'
			}
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
				    text: 'Disburse Checked Items',
				}
        ],
        editable: "inline",
        detailTemplate: 'CheckList Items: <div class="grid"></div>',
        detailInit: function (e) {
            e.detailRow.find(".grid").kendoGrid({
                dataSource: {
                    transport: {
                        read: function (entries) {
                            entries.success(e.data.checkListItems);
                        },
                        parameterMap: function (options, operation) {
                            if (operation !== "read" && options.models) {
                                return { models: kendo.stringify(options.models) };
                            }
                        }
                    },
                    schema: {
                        model: {
                            id: "categoryId",
                            fields: {
                                categoryId: { editable: false },
                                description: { editable: false },
                                isMandatory: { type: "boolean", editable: true, validation: { required: true } },

                            }
                        }
                    },
                },
                columns: [
			            { field: 'description', title: 'Checklist Description', width: '70%' },

                        {
                            field: 'isMandatory', title: 'Mandatory ?',
                            template: '<input type="checkbox" #= isMandatory ? \'checked="isMandatory"\' : "" # />'
                        },
                ]
            });
        }

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
        if (approvals.length > 0) {
            displayLoadingDialog();
            saveToServer(approvals);
        } else {
            smallerWarningDialog('Please add Schedule to pay for', 'NOTE');
        }
    } else { smallerWarningDialog('One or more input is empty or invalid', 'WARNING'); }
}
function saveToServer(approvalData) {
    var postModel = {
        approvalItemsList: approvalData,
        approvalDate: $('#paymentDate').data('kendoDatePicker').value(),
        disbursementDate: $('#paymentDate').data('kendoDatePicker').value(),
        groupId: $("#group").val()
    }
    $.ajax({
        url: CombinedDisburseApiUrl + "/PostBatchCombinedDisburseChecklist",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(postModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        successDialog('Loan Checklist and Disbursement Successfully Recieved',
		'SUCCESS', function () { window.location = "/dash/home.aspx"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}


