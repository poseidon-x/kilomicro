/*
UI Scripts for Loan salaryLoanConfig Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var salaryLoanConfigApiUrl = coreERPAPI_URL_Root + "/crud/salaryLoanConfig";
var employerApiUrl = coreERPAPI_URL_Root + "/crud/employer";

var employers = [];
 
$(function () {
    $.ajax({
        url: employerApiUrl + "/Get",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        employers = data;

        employers.push({
            employerID: 0,
            employerName: "Not Selected",
        });

        var ui = new SalaryLoanConfigUi();
        ui.renderGrid();
    });
});

var SalaryLoanConfigUi = (function () {
    function SalaryLoanConfigUi() {
    }
    SalaryLoanConfigUi.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: salaryLoanConfigApiUrl + "/Get",
                        type: "POST",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: salaryLoanConfigApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: salaryLoanConfigApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: salaryLoanConfigApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    // the array of repeating data elements (salaryLoanConfig)
                    data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "salaryLoanConfigId",
                        fields:
                        {
                            salaryLoanConfigId: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            penaltyRate: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            interestRate: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            tenure: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            productName: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: '',
                            },
                            processingFeeRate: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            employerId: {
                                validation: {
                                    required: true,
                                }, 
                            },
                            isActive: {
                                validation: {
                                    required: true,
                                }, 
                            },  
                        }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },
            columns: [
                {
                    field: "employerId",
                    title: "Employer Name",
                    width: 220,
                    editor: this.employerDropDownEditor,
                    template: "#= getEmployerName(employerId ) #",
                },
                {
                    field: "productName",
                    title: "Product Name",
                    width: 150,
                    editor: longTextEditor
                },
                {
                    field: "interestRate",
                    title: "Interest Rate",
                    width: 125,
                    editor: numericEditor,
                    format: "{0: #,##0.###0}",
                },
                {
                    field: "penaltyRate",
                    title: "Penalty Rate",
                    width: 125,
                    editor: numericEditor,
                    format: "{0: #,##0.###0}",
                },
                {
                    field: "processingFeeRate",
                    title: "Proc Fee Rate",
                    width: 125,
                    editor: numericEditor,
                    format: "{0: #,##0.###0}",
                },
                {
                    field: "tenure",
                    title: "Tenure",
                    width: 100,
                    editor: numericEditor,
                    format: "{0: #,##0}",
                },
                {
                    field: "isActive",
                    title: "Active?",
                    width: 50,
                    editor: checkBoxTextEditor,
                    template: "<input type='checkbox' #= isActive?'checked':'' # disabled='true' />",
                },  
                {
                    command: ["edit", "destroy"],
                    width: 120,
                },
            ],
            toolbar: [
                {
                    name: "create",
                    text: "Add New Salary Loan Product",
                },
                "pdf",
                "excel"
            ],
            excel: {
                fileName: "salaryLoanProducts.xlsx"
            },
            pdf: {
                paperKind: "A4",
                landscape: true,
                fileName: "salaryLoanProducts.pdf"
            },
            filterable: true,
            sortable: {
                mode: "multiple",
            },
            editable: "popup",
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

    SalaryLoanConfigUi.prototype.employerDropDownEditor = function (container, options) {
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
     
    return SalaryLoanConfigUi;
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

