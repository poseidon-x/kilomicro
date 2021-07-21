//*******************************************
//***   GROUP LOAN REPAYMENT BY GROUP DAY JAVASCRIPT                
//***   CREATOR: SAMUEL WENDOLIN KETECHIE    	   
//***   DATE: DEC 28TH, 2018	
//*******************************************


"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var LoanGroupBatchRepaymentApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupBatchRepayment";
var LoanGroupDayApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupDay";


//Declaration of variables to store records retrieved from the database
var dueGroupRepayments = {};
var repaymentModel = {};
var daysObj = {};

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.ajax({
        url: LoanGroupDayApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        daysObj = data;
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
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
    });

    $("#day").width("100%").kendoComboBox({
        dataSource: daysObj,
        dataValueField: 'loanGroupDayId',
        dataTextField: 'dayName',
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        ignoreCase: true,
        change: onDayChange,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });

    $('#tabs').kendoTabStrip();
}

var onDayChange = function () {
    var id = $("#day").data("kendoComboBox").value();
    var exist = false;
    //Retrieve value enter validate
    for (var i = 0; i < daysObj.length; i++) {
        if (daysObj[i].loanGroupDayId == id) {
            exist = true;
            getDueRepayments();
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Day or No Day selected', 'ERROR');
        $("#day").data("kendoComboBox").value("");
    }
}


function getDueRepayments() {
    repaymentModel.dayId = $("#day").data("kendoComboBox").value();
    repaymentModel.repaymentDate = $("#paymentDate").data("kendoDatePicker").value();

    displayLoadingDialog();
    $.ajax({
        url: LoanGroupBatchRepaymentApiUrl + "/GetGroupsLoanByDueRepaymForDay",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(repaymentModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        if (data.length > 0) {
            dueGroupRepayments = data;
            renderGrid();
        }
        else {
            warningDialog('There is no Loans for the selected Day or Groups ', 'ERROR');
        }

    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}


$('#click').click(function () {
    var groups = [];
    var grid = $("#repaymentGrid").data("kendoGrid");
    var gridgroups = grid.dataSource.group();
    for (var i = 0; i < gridgroups.length; i++) {
        groups.push(gridgroups[i].field);
    }

    alert(groups);
})

//render Grid
function renderGrid() {
    $('#repaymentGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(dueGroupRepayments);
                },
                update: function (entries) {
                    entries.success();
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                }
            },
            schema: {
                model: {
                    id: "repaymentScheduleId",
                    fields: {
                        repaymentScheduleId: { editable: true, validation: { required: true } },
                        clientFullName: { editable: false },
                        loanNumber: { editable: false },
                        repaymentDate: { editable: false, type: "date" },
                        amountDisbursed: { editable: false },
                        amountDue: { editable: false },
                        paid: { type: "boolean", editable: true, validation: { required: true } },
                        groupName: { type: "string", editable: false, validation: { required: true } },
                        dueSchedule: { editable: true, aggregates: ["sum"]}
                    }
                }
            },
            aggregate: [
                    { field: "amountDue", aggregate: "sum" },
                    { field: "dueSchedule", aggregate: "sum" }
            ],
            group: [
                {
                    field: "groupName",
                    aggregates: [
                      { field: "amountDue", aggregate: "sum" }
                    ]
                },
            ]
        },
        columns: [
			{ field: 'clientFullName', title: 'Client' },
			{ field: 'loanNumber', title: 'Loan No.' },
            { field: 'groupName', title: 'Group' },
			{ field: 'repaymentDate', title: 'Due Date', format: "{0: dd-MMM-yyyy}" },
			{ field: 'amountDisbursed', title: 'Amt. Disbursed', format: '{0: #,###.#0}' },
			{
			    field: 'amountDue', title: 'Amt. Due', format: '{0: #,###.#0}', footerTemplate: "Total Due: #: sum #",
			    groupFooterTemplate: "Total Due :  #= kendo.toString(sum, '0,000.00') #"
			},
            {
                field: 'dueSchedule', title: 'Due Schedule', format: '{0: #,###.#0}'
            },
            { field: 'paid', template: '<input type="checkbox" #= paid ? \'checked="paid"\' : "" # disabled="disabled" />' },



			{ command: [{ name: "edit", text: "Mark As Paid" }] }
        ],
        navigatable: true,
        groupable: false,
        sortable: false,
        resizable:true,
        pageable: {
            pageSize: 50,
            pageSizes: [50, 150, 300, 600, 1000],
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
				    text: 'Pay Marked Repayments',
				}
        ],
        editable: "inline"
    });
    $(".saveChanges").click(function () {
        saveRepayment();
    });
}
function saveRepayment() {
    var validator = $("#myform").kendoValidator().data("kendoValidator");

    if (validator.validate()) {
        var grid = $("#repaymentGrid").data("kendoGrid");
        var repayments = grid.dataSource.data();

        //var repayments = $("#repaymentGrid").data().kendoGrid.dataSource.view();
        if (repayments.length > 0) {
            displayLoadingDialog();
            saveToServer(repayments);
        } else {
            smallerWarningDialog('Please add Schedule to pay for', 'NOTE');
        }
    } else { smallerWarningDialog('One or more input is empty or invalid', 'WARNING'); }
}
function saveToServer(repaymentData) {
    var postModel = {
        repaymentItemsList: repaymentData,
        repaymentDate: $('#paymentDate').data('kendoDatePicker').value()
    }
    $.ajax({
        url: LoanGroupBatchRepaymentApiUrl + "/PostBashCashPayment",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(postModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        successDialog('Loan Repayment Successfully Recieved',
		'SUCCESS', function () { window.location = "/dash/home.aspx"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}


