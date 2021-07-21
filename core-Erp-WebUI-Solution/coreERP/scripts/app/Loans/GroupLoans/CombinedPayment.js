"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var CombinedPaymentApiUrl = coreERPAPI_URL_Root + "/crud/CombinedPayment";

//Declaration of variables to store records retrieved from the database
var loanGroups = {};
var combinedGroupPayments = [];
var paymentModel = {};

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
    var exist = true;
    //Reseet Grid to empty
    $('#combinedGrid').html('');
    getSavingsAndLoansCombined();
    if (!exist) {
        warningDialog('Invalid Group', 'ERROR');
        $("#groupName").data("kendoComboBox").value("");
        $("#day").data("kendoComboBox").value("");
    }
}

//make column uneditable
function nonEditor(container, options) {
    container.text(options.model[options.field]);
    container.removeClass("k-edit-cell");
}

function getSavingsAndLoansCombined() {
    paymentModel.groupId = $("#group").data("kendoComboBox").value();
    paymentModel.repaymentDate = $("#paymentDate").data("kendoDatePicker").value();

    displayLoadingDialog();
    $.ajax({
        url: CombinedPaymentApiUrl + "/GetCombined",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(paymentModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        if (data.length > 0) {
            combinedGroupPayments = data;
            //console.log("grid", combinedGroupPayments)
            renderGrid();
        }
        else {
            //groups.setDataSource(loanGroups);
            warningDialog('There is no Loans for the selected Group ', 'WARNING');
            //groups.enable(false);
        }

    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}




//render Grid
function renderGrid() {
    $('#combinedGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(combinedGroupPayments);
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
                    id: "savingID",
                    fields: {
                        savingNo: { editable: false },
                        loanId: { editable: false },
                        loanNo: { editable: false },
                        savingID: { editable: true, validation: { required: true } },
                        clientID: { editable: false, type: "number" },
                        clientName: { editable: false },
                        clientAccNum: { editable: false },
                        principalSavingsBalance: { editable: false },
                        amountDisbursed: { editable: false },
                        amountDue: { editable: false },
                        paymentAmount: { editable: true },
                        savingPlanAmount: { editable: true },
                        savingsPaid: { type: "boolean", editable: true, validation: { required: true } },
                        Branch:{editable:false}
                    }
                }
            },
            group: [
                {
                    field: "Branch",
                    aggregates: [
                       { field: "loanNo", aggregate: "count" }
                    ]
                }
            ]
            
        },
        columns: [
            { field: 'savingNo', title: 'Saving No.', editor: nonEditor },
            { field: 'clientName', title: 'Client Name.', editor: nonEditor },
			{
			    field: 'clientAccNum', title: 'Client Acc. No.', editor: nonEditor
			},
			{
			    field: 'principalSavingsBalance',
			    title: 'Savings Balance',
			    editor: nonEditor
			},
            {
                field: 'loanNo',
                title: 'loan No.',
                groupFooterTemplate: "No. of Clients: #= count#",
                editor: nonEditor
            },
            {
                field: 'amountDue',
                title: 'Amt Due.',
                editor: nonEditor
            },
            {
                field: 'paymentAmount',
                title: 'Repay Amt.'
            },
            {
                field: 'savingPlanAmount',
                title: 'Deposit Amt'
            },
            {
                field: 'savingsPaid',
                template: '<input type="checkbox" #= savingsPaid ? \'checked="savingsPaid"\' : "" # disabled="disabled"/>',
                title: 'Paid?'
            },

			{ command: [{ name: "edit", text: "Edit" }] }
        ],
        navigatable: true,
        groupable: false,
        resizable: true,
        selectable:true,
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
				    text: 'Save Amount',
				}
        ],
        editable: "inline"
    });
    $(".saveChanges").click(function () {
        savePayment();
    });
}
function savePayment() {
    var validator = $("#myform").kendoValidator().data("kendoValidator");

    if (validator.validate()) {
        var grid = $('#combinedGrid').data('kendoGrid');
        grid.dataSource.group([]);
        var payments = $("#combinedGrid").data().kendoGrid.dataSource.view();
        if (payments.length > 0) {
            displayLoadingDialog();
            saveToServer(payments);
        } else {
            smallerWarningDialog('Please add Schedule to pay for', 'NOTE');
        }
    } else { smallerWarningDialog('One or more input is empty or invalid', 'WARNING'); }
}
function saveToServer(paymentData) {
    var postModel = {
        combinedItems: paymentData,
        paymentDate: $('#paymentDate').data('kendoDatePicker').value()
    }
    $.ajax({
        url: CombinedPaymentApiUrl + "/PostBatchCombined",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(postModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        console.log("newdata", data);
        var message = "";
        if (data.successfulSavings.length) {
            message += data.successfulSavings.length + " successful savings\n";
            successDialog(message,
       'SUCCESS', function () { window.location = "/ln/setup/postTill.aspx"; });
        }
        if (data.failedSavings.length) {
            message += data.failedSavings.length + " failed savings\n";
            errorDialog(message,
		'FAILED', function () { window.location = "/dash/home.aspx"; });
        }
        //successDialog(message,
        //'', function () { window.location = "/dash/home.aspx"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}


