//*******************************************
//***   BATCH GROUP LOAN DISBURSEMENT JAVASCRIPT                
//***   CREATOR: SAMUEL WENDOLIN KETECHIE 
//***   DATE: JANUARY 9,2020	
//*******************************************

"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var CombinedDisburseApiUrl = coreERPAPI_URL_Root + "/crud/BatchGroupLoanDisbursement";

//Declaration of variables to store records retrieved from the database
var loanGroups = {};
var dueGroupDisbursements = {};
var disburseModel = {};

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
    $('#disburseDate').width('80%').kendoDatePicker({
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
    $('#disburseGrid').html('');

    //Retrieve value enter validate
    for (var i = 0; i < loanGroups.length; i++) {
        if (loanGroups[i].loanGroupId == id) {
            exist = true;
            getDueDisbursements();
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Group', 'ERROR');
        $("#groupName").data("kendoComboBox").value("");        
    }
}

function getDueDisbursements() {
    disburseModel.groupId = $("#group").data("kendoComboBox").value();
    disburseModel.approvalDate = $("#disburseDate").data("kendoDatePicker").value();

    displayLoadingDialog();
    $.ajax({
        url: CombinedDisburseApiUrl + "/GetBatchGroupLoanDisbursement",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(disburseModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        if (data.length > 0) {
            dueGroupDisbursements = data;
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
    $('#disburseGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(dueGroupDisbursements);
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
                        approved: { type: "boolean", editable: true, validation: { required: true } },
                        BranchName: { editable: false }
                    }
                }
            },
            group: [
                {
                    field: "BranchName",
                    title: "Branch",
                    aggregates: [
                        { field: "amountDisbursed", aggregate: "sum" },
                        { field: "loanNumber", aggregate: "count" }
                    ]
                }
            ],
            aggregate: [
                    { field: "amountDisbursed", aggregate: "sum" }
            ]
            
        },
        columns: [
			{
			    field: 'clientFullName',
			    title: 'Client',
			    width: '20%'
			},
			{
			    field: 'loanNumber',
			    title: 'Loan No.',
			    groupFooterTemplate: "Grp Count: #= count#"
			},
			{
			    field: 'approvalDate',
			    title: 'App. Date',
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
			    //footerTemplate: "Total Disbursed: #: sum #",
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
        sortable: true,
        selectable: false,
        resizable: true,
        scrollable: true,
        pageable: {
            pageSize: 150,
            pageSizes: [150, 300, 600, 1000,2000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        },
        toolbar: [
				{
				    name: "save",
				    className: 'saveChanges',
				    text: 'Disburse Checked Items',
				}
        ],
        editable: "inline"

    });

    $(".saveChanges").click(function () {
        saveDisbursements();
    });
}

function saveDisbursements() {
    var validator = $("#myform").kendoValidator().data("kendoValidator");

    if (validator.validate()) {
        var grid = $('#disburseGrid').data('kendoGrid');
        grid.dataSource.group([]);
        var disbusements = $("#disburseGrid").data().kendoGrid.dataSource.view();
        if (disbusements.length > 0) {
            displayLoadingDialog();
            saveToServer(disbusements);
        } else {
            smallerWarningDialog('Please add Schedule to pay for', 'NOTE');
        }
    } else { smallerWarningDialog('One or more input is empty or invalid', 'WARNING'); }
}
function saveToServer(disbusementData) {
    var postModel = {
        approvalItemsList: disbusementData,
        approvalDate: $('#disburseDate').data('kendoDatePicker').value(),
        disbursementDate: $('#disburseDate').data('kendoDatePicker').value(),
        groupId: $("#group").val()
    }
    $.ajax({
        url: CombinedDisburseApiUrl + "/PostBatchGroupLoanDisbursement",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(postModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        successDialog('Group Loan Disbursement Successfully Recieved',
		'SUCCESS', function () { window.location = "/ln/setup/postTill.aspx"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}


