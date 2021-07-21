//"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var LoanGroupBatchDisburseApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupBatchDisbursement";
var LoanGroupDayApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupDay";
var paymentModeApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var bankApiUrl = coreERPAPI_URL_Root + "/crud/bank";

//Declaration of variables to store records retrieved from the database
var loanGroups = {};
var groupLoans = {};
var remainingGroupLoans = [];
var paymentModes = {};
var banks = {};
var loansToDisburse = {};
var disburseDate = {};
var days = {};

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
var loanGroupBatchDisburseAjax = $.ajax({
    url: LoanGroupBatchDisburseApiUrl + '/Get',
    type: 'Get',
    contentType: "application/json",
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var disburseDateAjax = $.ajax({
    url: LoanGroupApiUrl + '/GetDisbursementDate',
    type: 'Get',
    contentType: "application/json",
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var daysAjax = $.ajax({
    url: LoanGroupDayApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(loanGroupAjax, paymentModeAjax, bankAjax, loanGroupBatchDisburseAjax, disburseDateAjax, daysAjax)
        .done(function (dataLoanGroup, dataPaymentMode, dataBank, dataLoanGroupBatchDisburse, disburseDate, dataDay) {
            //loanGroups = dataLoanGroup[2].responseJSON;
            paymentModes = dataPaymentMode[2].responseJSON;
            banks = dataBank[2].responseJSON;
            loansToDisburse = dataLoanGroupBatchDisburse[2].responseJSON;
            disburseDate = disburseDate[2].responseJSON;
            days = dataDay[2].responseJSON;


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
    $('#disbursementDate').width('100%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        change: onDisbursementDateChange
    });
    $("#day").width("100%").kendoComboBox({
        dataSource: days,
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
    $('#groupName').width('100%').kendoComboBox({
        //dataSource: loanGroups,
        dataValueField: 'loanGroupId',
        dataTextField: 'loanGroupName',
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        enable: false,
        change: onGroupChange,
        optionLabel: ''
    });
    $('#tabs').kendoTabStrip();
}
var onDisbursementDateChange = function () {
    getGroupLoans(0);
}
var onGroupChange = function () {
    var id = $("#groupName").data("kendoComboBox").value();
    var exist = false;

    //Retrieve value enter validate
    for (var i = 0; i < loanGroups.length; i++) {
        if (loanGroups[i].loanGroupId == id) {
            exist = true;
            getGroupLoans(id);
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Group', 'ERROR');
        $("#groupName").data("kendoComboBox").value("");
    }
}
var onDayChange = function () {
    var id = $("#day").data("kendoComboBox").value();
    $("#groupName").data("kendoComboBox").value("");
    var exist = false;

    //Retrieve value enter validate
    for (var i = 0; i < days.length; i++) {
        if (days[i].loanGroupDayId == id) {
            exist = true;
            getGroupsByDay(id);
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Day', 'ERROR');
        $("#day").data("kendoComboBox").value("");
    }
}

function getGroupsByDay(id) {
    //disburseDate.dibursementDate = $("#disbursementDate").data("kendoDatePicker").value();
    //disburseDate.groupId = id;

    var groups = $("#groupName").data("kendoComboBox");

    if (disburseDate.dibursementDate != undefined)
    displayLoadingDialog();
    $.ajax({
        url: LoanGroupDayApiUrl + "/GetGroupsByDate/" + id,
        type: 'GET',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        loanGroups = data;
        if (loanGroups.length > 0) {
            groups.setDataSource(loanGroups);
            groups.enable(true);
        }
        else {
            groups.setDataSource(loanGroups);
            warningDialog('There is no group for the selected date', 'ERROR');
            $("#groupName").data("kendoComboBox").value("");
            groups.enable(false);
            remainingGroupLoans = [];
            $('#groupLoanGrid').html('');
        }
        //groups.enable(true);

    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });

}

//render Grid
function renderGrid() {
    $('#groupLoanGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(loansToDisburse.loans);
                },
                create: function (entries) {
                    var data = entries.data;
                    data.amountApproved = $('#amountApproved').data('kendoNumericTextBox').value();
                    data.amountDisbursed = $('#amountDisdursed').data('kendoNumericTextBox').value();					
                    removeLoan(data.loanId)
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
                    entries.success();
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                }
                //parameterMap: function (data) { return JSON.stringify(data); }
            },
            schema: {
                model: {
                    id: "loanId",
                    fields: {
                        loanId: { editable: true, validation: { required: true } },
                        loanNumberWithName: { editable: false },
                        amountApproved: { editable: true },
                        amountDisbursed: { type: "number", editable: true },
                        paymentModeId: { editable: true },
                        bankId: { editable: true, validation: { required: false } },
                        chequeNumber: { editable: true, validation: { required: false } },
                    }
                }
            },
        },
        columns: [
			{ field: 'loanId', title: 'Loan', editor: loanEditor, template: '#= getLoan(loanId) #' },
			{ field: 'amountApproved', title: 'Amount Approved', editor: amountApprovedEditor },
			{ field: 'amountDisbursed', title: 'Amount Disbursed', editor: amountDisbursedEditor },
			{ field: 'paymentModeId', title: 'Payment Mode', editor: paymentModeEditor, template: '#= getpaymentMode(paymentModeId) #' },
			{ field: 'bankId', title: 'Bank', editor: bankEditor, template: '#= getBank(bankId) #' },
			{ field: 'chequeNumber', title: 'Cheque Number', editor: chequeEditor },
			{ command: ["destroy"], width: "110px" },
        ],
        navigatable: true,
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
        toolbar: [
					{
					    name: "create",
					    className: 'addLoan',
					    text: 'Add New Loan',
					},
					{
					    name: "save",
					    className: 'saveChanges',
					    text: 'Disburse Current Batch',
					}
        ],
        editable: "popup"
    });
    $(".saveChanges").click(function () {
        disburseLoans();
    });
}

function loanEditor(container, options) {
    $('<input type="text" id="loan" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width('100%')
    .kendoComboBox({
        dataSource: remainingGroupLoans,
        dataValueField: "loanId",
        dataTextField: "loanNumberWithName",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        change: onLoanChange,
        optionLabel: ""
    });
}
function paymentModeEditor(container, options) {
    $('<input type="text" id="paymentMode" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width('100%')
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
	.width("100%")
    .kendoComboBox({
        dataSource: banks,
        dataValueField: "bank_acct_id",
        dataTextField: "bank_acct_desc",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        optionLabel: ""
    });
}
function amountApprovedEditor(container, options) {
    $('<input data-bind="value:' + options.field + '" readonly id="amountApproved" />')
    .appendTo(container)
	.width("100%")
    .kendoNumericTextBox({});
}
function amountDisbursedEditor(container, options) {
    $('<input readonly data-bind="value:' + options.field + '" id="amountDisdursed"/>')
    .appendTo(container)
	.width("100%")
    .kendoNumericTextBox({
        min: 0
    });
}
function chequeEditor(container, options) {
    $('<input data-bind="value:' + options.field + '" id="chequeNumber"/>')
    .appendTo(container)
	.width("100%")
    .kendoMaskedTextBox({
        min: 0
    });
}
function getLoan(id) {
    for (var i = 0; i < groupLoans.length; i++) {
        if (groupLoans[i].loanId == id) {
            return groupLoans[i].loanNumberWithName;
        }
    }
}
function getpaymentMode(id) {
    for (var i = 0; i < paymentModes.length; i++) {
        if (paymentModes[i].ID == id) {
            return paymentModes[i].Description;
        }
    }
}
function getBank(id) {
    for (var i = 0; i < banks.length; i++) {
        if (banks[i].bank_acct_id == id) {
            return banks[i].bank_acct_desc;
        }
    }
}

function getGroupLoans(id) {
    loansToDisburse.dibursementDate = $("#disbursementDate").data("kendoDatePicker").value();
    loansToDisburse.groupId = id;

    if (id > 1 && loansToDisburse.dibursementDate != 'undefined') {

        displayLoadingDialog();
        $.ajax({
            url: LoanGroupBatchDisburseApiUrl + "/Get/",
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(loansToDisburse),
            beforeSend: function(req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function(data) {
            dismissLoadingDialog();
            groupLoans = data;
            populateRemainingClients(data);
            //render Grid
            if (data.length > 0) renderGrid();
            else {
                $('#groupLoanGrid').html('');
                warningDialog('The selected Group has no undisburesd Loans', 'NOTE');
            }

        }).error(function(xhr, data, error) {
            dismissLoadingDialog();
            warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
        });

    }
}
function removeLoan(id) {
    for (var i = 0; i < remainingGroupLoans.length; i++) {
        if (remainingGroupLoans[i].loanId == id) {
            remainingGroupLoans.splice(i, 1);
            break;
        }
    }
}
function addLoanBack(id) {
    for (var i = 0; i < groupLoans.length; i++) {
        if (groupLoans[i].loanId == id) {
            remainingGroupLoans.push(groupLoans[i]);
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

var onLoanChange = function () {
    var id = $("#loan").data("kendoComboBox").value();
    var exist = false;

    //Retrieve value enter validate
    for (var i = 0; i < groupLoans.length; i++) {
        if (groupLoans[i].loanId == id) {
            exist = true;
            $('#amountApproved').data('kendoNumericTextBox').value(groupLoans[i].amountApproved);
            var disb = $('#amountDisdursed').data('kendoNumericTextBox');
			disb.value(groupLoans[i].amountApproved);
            disb.max(groupLoans[i].amountApproved);
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Loan', 'ERROR');
        $("#loan").data("kendoComboBox").value("");
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
    } else {
        var bank = $('#bank').data('kendoComboBox');
        var cheque = $('#chequeNumber').data('kendoMaskedTextBox');
        if (mode.indexOf("cash") >= 0) {
            bank.value("");
            cheque.value("");

            bank.enable(false);
            cheque.enable(false);
        } else {
            bank.enable(true);
            cheque.enable(true);
        }
    }
}
function disburseLoans() {
    loansToDisburse.disbursementDate = $("#disbursementDate").data("kendoDatePicker").value();
    loansToDisburse.loans = $("#groupLoanGrid").data().kendoGrid.dataSource.view();
    if (loansToDisburse.loans.length > 0) {
        displayLoadingDialog();
        saveToServer();
    } else {
        smallerWarningDialog('Please add Loans to disburse', 'NOTE');
    }
}
function saveToServer() {
    $.ajax({
        url: LoanGroupBatchDisburseApiUrl + "/DisburseBatch",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(loansToDisburse),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        successDialog('Loans Successfully Disbursed',
		'SUCCESS', function () { window.location = "/dash/home.aspx"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}


