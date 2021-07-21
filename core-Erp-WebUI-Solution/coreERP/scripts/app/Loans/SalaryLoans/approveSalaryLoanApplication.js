//Salary Loan Application UI

var salaryLoanConfigApiUrl = coreERPAPI_URL_Root + "/crud/salaryLoanConfig";
var salaryLoanApiUrl = coreERPAPI_URL_Root + "/crud/salaryLoan";
var employerApiUrl = coreERPAPI_URL_Root + "/crud/employer";
var employerDirectorApiUrl = coreERPAPI_URL_Root + "/crud/employerDirector";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/ClientLookUp";

var salaryLoan = {};

$(function() {
    loadForm();
});

function loadForm() {
    displayLoadingDialog();
    $.ajax({
        url: salaryLoanApiUrl + "/Get/" + salaryLoanId,
        type: "GET",
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function(data) {
        salaryLoan = data;
        prepareUi();
        populateUi();

        dismissLoadingDialog();
    }).error(function(xhr, error, data) {
        errorDialog(xhr.responseJSON.ExceptionMessage, "Error Approving Salary Loan");
        dismissLoadingDialog();
    });
}

function prepareUi() {
    $('#applicationDate').kendoDatePicker({
        format: "dd-MMM-yyyy",
        parseFormats: ["dd-MM-yyyy", "dd.MM.yyyy", "dd/MM/yyyy", "dd-MM-yy", "dd.MM.yy", "dd/MM/yy", "dd-MMM-yy",
            "yyyy-MM-dd"],
        value: new Date(), 
    })
    .data("kendoDatePicker")
    .enable(false);
    $('#employmentStartDate').kendoDatePicker({
        format: "dd-MMM-yyyy",
        parseFormats: ["dd-MM-yyyy", "dd.MM.yyyy", "dd/MM/yyyy", "dd-MM-yy", "dd.MM.yy", "dd/MM/yy", "dd-MMM-yy",
            "yyyy-MM-dd"]
    })
    .data("kendoDatePicker")
    .enable(false);
    
    $('#client')
        .width('90%')
        .kendoComboBox({
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: clientApiUrl + "/GetEmployeeClients",
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                }
            }
        },
        dataTextField: 'clientName',
        dataValueField: 'clientID',
        change: function () {
            var clientId = $('#client').data('kendoComboBox').value();
            onClientChange(clientId);
        },
        enabled: false
        })
    .data("kendoComboBox")
    .enable(false);
    $('#basicSalary').kendoNumericTextBox({
        format: '#,##0.#0',
        decimalDigits: 4,
        enabled: false
    })
    .data("kendoNumericTextBox")
    .enable(false); 
    $('#nominalDeductions').kendoNumericTextBox({
        format: '#,##0.#0',
        decimalDigits: 4,
        enabled: false
    })
    .data("kendoNumericTextBox")
    .enable(false);
    $('#totalAllowances').kendoNumericTextBox({
        format: '#,##0.#0',
        decimalDigits: 4,
        enabled: false
    })
    .data("kendoNumericTextBox");
    $('#applicationAmount').kendoNumericTextBox({
        format: '#,##0.#0',
        decimalDigits: 4,
        enabled: false
    })
    .data("kendoNumericTextBox")
    .enable(false);
    $('#approvalDate').kendoDatePicker({
        format: "dd-MMM-yyyy",
        parseFormats: [
            "dd-MM-yyyy", "dd.MM.yyyy", "dd/MM/yyyy", "dd-MM-yy", "dd.MM.yy", "dd/MM/yy", "dd-MMM-yy",
            "yyyy-MM-dd"
        ],
        value: new Date(),
        enabled: true
    });
    $('#approvedAmount').kendoNumericTextBox({
        format: '#,##0.#0',
        decimalDigits: 4,
        enabled: true
    })
    $('#save').click(function(e) {
        retrieveValues();
        saveToServer();
    });
}

function populateUi() {
    if (salaryLoan.salaryLoanId > 0) {
        $('#applicationDate').data('kendoDatePicker').value(salaryLoan.applicationDate);
        $('#employmentStartDate').data('kendoDatePicker').value(salaryLoan.employmentStartDate);
        $('#client').data('kendoComboBox').value(salaryLoan.clientId);
        $('#basicSalary').data('kendoNumericTextBox').value(salaryLoan.basicSalary);
        $('#nominalDeductions').data('kendoNumericTextBox').value(salaryLoan.nominalDeductions);
        $('#totalAllowances').data('kendoNumericTextBox').value(salaryLoan.totalAllowances);
        $('#applicationAmount').data('kendoNumericTextBox').value(salaryLoan.applicationAmount);
        onClientChange(salaryLoan.clientId);
    }
}

function onClientChange(clientId) {
    $('#approvingDirector').replaceWith('<input type="text" id="approvingDirector" class="inputControl"/>');
    $('#employer').replaceWith('<input type="text" id="employer" class="inputControl"/>');
    $('#config').replaceWith('<input type="text" id="config" class="inputControl"/>');
    $('#config')
        .width('90%')
        .kendoComboBox({
            dataSource: {
                transport: {
                    read: {
                        type: "GET",
                        url: salaryLoanConfigApiUrl + "/GetByClient?clientId="+clientId,
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                }
            },
            dataTextField: 'productName',
            dataValueField: 'salaryLoanConfigId',
            value: salaryLoan.salaryLoanConfigId,
            enabled: false
        })
    .data("kendoComboBox")
    .enable(false);
    $('#employer')
        .width('90%')
        .kendoComboBox({
            dataSource: {
                transport: {
                    read: {
                        type: "GET",
                        url: employerApiUrl + "/GetByClient?clientId=" + clientId,
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                }
            },
            dataTextField: 'employerName',
            dataValueField: 'employerID',
            value: salaryLoan.employerId
        })
    .data("kendoComboBox")
    .enable(false);
    $('#approvingDirector')
        .width('90%')
        .kendoComboBox({
            dataSource: {
                transport: {
                    read: {
                        type: "GET",
                        url: employerDirectorApiUrl + "/GetLookUp?clientId=" + clientId,
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                }
            },
            dataTextField: 'Description',
            dataValueField: 'ID',
            value: salaryLoan.approvingDirectorId,
            enabled: false
        })
    .data("kendoComboBox")
    .enable(false);
}

function retrieveValues() {
    salaryLoan.applicationDate = $('#applicationDate').data('kendoDatePicker').value();
    salaryLoan.employmentStartDate = $('#employmentStartDate').data('kendoDatePicker').value();
    salaryLoan.clientId = $('#client').data('kendoComboBox').value();
    salaryLoan.basicSalary = $('#basicSalary').data('kendoNumericTextBox').value();
    salaryLoan.nominalDeductions = $('#nominalDeductions').data('kendoNumericTextBox').value();
    salaryLoan.totalAllowances = $('#totalAllowances').data('kendoNumericTextBox').value();
    salaryLoan.applicationAmount = $('#applicationAmount').data('kendoNumericTextBox').value();
    salaryLoan.approvingDirectorId = $('#approvingDirector').data('kendoComboBox').value();
    salaryLoan.employerId = $('#employer').data('kendoComboBox').value();
    salaryLoan.salaryLoanConfigId = $('#config').data('kendoComboBox').value();
    salaryLoan.approvalDate = $('#approvalDate').data('kendoDatePicker').value();
    salaryLoan.approvedAmount = $('#approvedAmount').data('kendoNumericTextBox').value();
}

function saveToServer() {
    displayLoadingDialog();

    $.ajax({
        url: salaryLoanApiUrl + "/Approve",
        type: 'POST',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        },
        data: JSON.stringify(salaryLoan)
    }).success(function (data) {
        successDialog("Successfully Approved Salary Loan", "Success", function () {
            window.location = '/ln/SalaryLoans/PendingApproval';
        }); 

        dismissLoadingDialog();
    }).error(function (xhr, error, data) {
        errorDialog(xhr.responseJSON.ExceptionMessage, "Error Approving Salary Loan");

        dismissLoadingDialog();
    });
}