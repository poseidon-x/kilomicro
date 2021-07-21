//Salary Loan Applications Pending Approval UI

var salaryLoanConfigApiUrl = coreERPAPI_URL_Root + "/crud/salaryLoanConfig";
var salaryLoanApiUrl = coreERPAPI_URL_Root + "/crud/salaryLoan";
var employerApiUrl = coreERPAPI_URL_Root + "/crud/employer";
var employerDirectorApiUrl = coreERPAPI_URL_Root + "/crud/employerDirector";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/ClientLookUp";

var employers = [];
var directors = [];
var clients = [];
var configs = [];
 
$(function () {
    displayLoadingDialog();

    var empAjax = $.ajax({
        url: employerApiUrl + "/Get",
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });
    var directorAjax = $.ajax({
        url: employerDirectorApiUrl + "/GetLookUp",
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });
    var clientAjax = $.ajax({
        url: clientApiUrl + "/GetEmployeeClients",
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });
    var configAjax = $.ajax({
        url: salaryLoanConfigApiUrl + "/Get",
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });
    $.when(empAjax, directorAjax, clientAjax, configAjax)
    .done(function (data1, data2, data3, data4) {
        employers = data1[2].responseJSON;
        directors = data2[2].responseJSON;
        clients = data3[2].responseJSON;
        configs = data4[2].responseJSON;
 
        var ui = new SalaryLoanPendingApplicationsUi();
        ui.renderGrid();
        dismissLoadingDialog();
    });
});

var SalaryLoanPendingApplicationsUi = (function () {
    function SalaryLoanPendingApplicationsUi() {
    }
    SalaryLoanPendingApplicationsUi.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: salaryLoanApiUrl + "/GetForDisbursement",
                        type: "POST",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }, 
                },
                pageSize: 10,
                schema: {
                    // the array of repeating data elements (salaryLoanConfig)
                    data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "salaryLoanId",
                        fields:
                        {
                            salaryLoanId: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            applicationDate: {
                                validation: {
                                    required: true,
                                }, 
                                type: "Date"
                            },
                            employmentStartDate: {
                                validation: {
                                    required: true,
                                }, 
                                type: "Date"
                            },
                            clientId: {
                                validation: {
                                    required: true,
                                }, 
                            },
                            basicSalary: {
                                validation: {
                                    required: true,
                                }, 
                            },
                            nominalDeductions: {
                                validation: {
                                    required: true,
                                }, 
                            },
                            employerId: {
                                validation: {
                                    required: true,
                                }, 
                            },
                            totalAllowances: {
                                validation: {
                                    required: true,
                                }, 
                            },
                            applicationAmount: {
                                validation: {
                                    required: true,
                                },
                            },
                            approvingDirectorId: {
                                validation: {
                                    required: true,
                                },
                            },
                            salaryLoanConfigId: {
                                validation: {
                                    required: true,
                                },
                            }
                        }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },
            columns: [
                {
                    field: "clientId",
                    title: "Client Name",
                    width: 150,
                    editor: this.clientDropDownEditor,
                    template: "#= getClientName(clientId ) #",
                },
                {
                    field: "applicationDate",
                    title: "App. Date",
                    width: 100,
                    editor: dateEditor,
                    format: '{0:dd-MMM-yyyy}'
                },
                {
                    field: "employerId",
                    title: "Employer Name",
                    width: 150,
                    editor: this.employerDropDownEditor,
                    template: "#= getEmployerName(employerId ) #",
                },
                {
                    field: "employmentStartDate",
                    title: "Emp. Date",
                    width: 100,
                    editor: dateEditor,
                    format: '{0:dd-MMM-yyyy}'
                },
                {
                    field: "basicSalary",
                    title: "Basic",
                    width: 100,
                    editor: numericEditor,
                    format: "{0: #,##0.#0}",
                },
                {
                    field: "totalAllowances",
                    title: "Allowances",
                    width: 100,
                    editor: numericEditor,
                    format: "{0: #,##0.#0}",
                },
                {
                    field: "nominalDeductions",
                    title: "Deductions",
                    width: 100,
                    editor: numericEditor,
                    format: "{0: #,##0.#0}",
                },
                {
                    field: "applicationAmount",
                    title: "App. Amount",
                    width: 100,
                    editor: numericEditor,
                    format: "{0: #,##0.#0}",
                }, 
                {
                    field: "salaryLoanConfigId",
                    title: "Product Name",
                    width: 150,
                    editor: this.configDropDownEditor,
                    template: "#= getProductName(salaryLoanConfigId ) #",
                },
                {
                    field: "approvingDirectorId",
                    title: "Appr. Director",
                    width: 100,
                    editor: this.directorDropDownEditor,
                    template: "#= getDirectorName(approvingDirectorId ) #",
                },
                {
                    command: [
                      {
                          name: "disburse",
                          text: "Disburse",
                          click: function (e) {
                              // e.target is the DOM element representing the button
                              var tr = $(e.target).closest("tr"); // get the current table row (tr)
                              // get the data bound to the current table row
                              var data = this.dataItem(tr);
                              window.location = "/ln/SalaryLoans/Disburse/" + data.salaryLoanId.toString();
                          },
                      }],
                    width: 180,
                },
            ],
            toolbar: [ 
                "pdf",
                "excel"
            ],
            excel: {
                fileName: "salaryLoansPendingDisbursement.xlsx"
            },
            pdf: {
                paperKind: "A4",
                landscape: true,
                fileName: "salaryLoansPendingDisbursement.pdf"
            },
            filterable: true,
            sortable: {
                mode: "multiple",
            }, 
            pageable: {
                pageSize: 10,
                pageSizes: [10, 25, 50, 100, 1000],
                previousNext: true,
                buttonCount: 5,
            },
            groupable: true,
            selectable: true,
            edit: function(e) {
                var editWindow = this.editable.element.data("kendoWindow");
                editWindow.wrapper.css({ width: 700 });
                editWindow.title("Edit Salary Loan Products Data");
            },
            save: function(e) {
                $('.k-grid-update').css('display', 'none');
            },
            mobile: true,
            reorderable: true,
            resizable: true,
        });
    };

    SalaryLoanPendingApplicationsUi.prototype.employerDropDownEditor = function (container, options) {
        try {
            $('<input required data-text-field="employerName" data-value-field="employerID" data-bind="value:' + options.field + '"/>')
                .width(300)
                .appendTo(container)
                .kendoComboBox({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: employers,
                    width: 250
                });
        }
        catch (e) { }
    }

    SalaryLoanPendingApplicationsUi.prototype.directorDropDownEditor = function (container, options) {
        try {
            $('<input required data-text-field="Description" data-value-field="ID" data-bind="value:' + options.field + '"/>')
                .width(300)
                .appendTo(container)
                .kendoComboBox({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: directors,
                    width: 250
                });
        }
        catch (e) { }
    }

    SalaryLoanPendingApplicationsUi.prototype.clientDropDownEditor = function (container, options) {
        try {
            $('<input required data-text-field="clientName" data-value-field="clientID" data-bind="value:' + options.field + '"/>')
                .width(300)
                .appendTo(container)
                .kendoComboBox({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: clients,
                    width: 250
                });
        }
        catch (e) { }
    }

    SalaryLoanPendingApplicationsUi.prototype.configDropDownEditor = function (container, options) {
        try {
            $('<input required data-text-field="productName" data-value-field="salaryLoanConfigId" data-bind="value:' + options.field + '"/>')
                .width(300)
                .appendTo(container)
                .kendoComboBox({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: configs,
                    width: 250
                });
        }
        catch (e) { }
    }

    return SalaryLoanPendingApplicationsUi;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function salaryLoanConfigIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function getEmployerName(accountId) {
    var employerName = "";

    try {
        for (var i = 0; i < employers.length; i++) {
            if (employers[i].employerID === accountId) {
                employerName = employers[i].employerName;
            }
        }
    }
    catch (e) { }

    return employerName;
}

function getClientName(clientId) {
    var clientName = "";

    try {
        for (var i = 0; i < clients.length; i++) {
            if (clients[i].clientID === clientId) {
                clientName = clients[i].clientName;
            }
        }
    }
    catch (e) { }

    return clientName;
}

function getProductName(configId) {
    var productName = "";

    try {
        for (var i = 0; i < configs.length; i++) {
            if (configs[i].salaryLoanConfigId === configId) {
                productName = configs[i].productName;
            }
        }
    }
    catch (e) { }

    return productName;
}

function getDirectorName(directorId) {
    var directorName = "";

    try {
        for (var i = 0; i < directors.length; i++) {
            if (directors[i].ID === directorId) {
                directorName = directors[i].Description;
            }
        }
    }
    catch (e) { }

    return directorName;
}

var longTextEditor = function (container, options) {
    $('<input class="input-control" data-bind="value:' + options.field + '" style="width:300px;"/>')
        .appendTo(container);
}

var checkBoxTextEditor = function (container, options) {
    $('<input type="checkbox" class="input-control" data-bind="checked:' + options.field + '" />')
        .appendTo(container);
}

var dateEditor = function (container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDatePicker({
            format: "dd-MMM-yyyy"
        });
}

var numericEditor = function (container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            decimals: 6,
            min: 0,
            format: '#,##0.#####0'
        });
};

function booleanCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

