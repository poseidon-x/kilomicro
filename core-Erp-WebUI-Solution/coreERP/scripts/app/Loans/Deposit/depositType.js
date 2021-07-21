/*
UI Scripts for Loan depositType Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var depositTypeApiUrl = coreERPAPI_URL_Root + "/crud/depositType";
var accountApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";

var accounts = [];

var plans = [
    { planId: null, planName: "None", },
    { planId: 1, planName: "Daily", },
    { planId: 7, planName: "Weekly", },
    { planId: 14, planName: "Forthnightly", },
    { planId: 30, planName: "Monthly", },
    { planId: 92, planName: "Quarterly", },
    { planId: 182, planName: "Half Yearly", }
];

$(function () {
    $.ajax({
        url: accountApiUrl + "/Get",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        accounts = data;

        accounts.push({
            acct_id: 0,
            acc_name: "Not Selected",
        });

        var ui = new depositTypeUI();
        ui.renderGrid();
    });
});

var depositTypeUI = (function () {
    function depositTypeUI() {
    }
    depositTypeUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: depositTypeApiUrl + "/Get",
                        type: "POST",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: depositTypeApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: depositTypeApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: depositTypeApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    // the array of repeating data elements (depositType)
                    data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "depositTypeID",
                        fields:
                        {
                            depositTypeID: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            depositTypeName: {
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
                            defaultPeriod: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            allowsInterestWithdrawal: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            allowsPrincipalWithdrawal: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            vaultAccountID: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            accountsPayableAccountID: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            interestExpenseAccountID: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            fxUnrealizedGainLossAccountID: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            fxRealizedGainLossAccountID: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            interestCalculationScheduleID: {
                                validation: {
                                    required: false,
                                },
                                defaultValue: 0,
                            },
                            chargesIncomeAccountID: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            interestPayableAccountID: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
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
                    field: "depositTypeName",
                    title: "Deposit Product",
                    width: 150
                },
                {
                    field: "interestRate",
                    title: "Int Rate",
                    width: 75,
                    editor: numericEditor,
                    format: "{0: #,##0.###0}",
                },
                {
                    field: "defaultPeriod",
                    title: "Default Period",
                    width: 50,
                    editor: numericEditor,
                    format: "{0: #,##0}",
                },
                {
                    field: "allowsInterestWithdrawal",
                    title: "Int With?",
                    width: 50,
                    editor: checkBoxTextEditor,
                    template: "<input type='checkbox' #= allowsInterestWithdrawal?'checked':'' # disabled='true' />",
                },
                {
                    field: "allowsPrincipalWithdrawal",
                    title: "Princ. With?",
                    width: 50,
                    editor: checkBoxTextEditor,
                    template: "<input type='checkbox' #= allowsPrincipalWithdrawal?'checked':'' # disabled='true' />",
                },
                {
                    field: "vaultAccountID",
                    title: "Vault Acc ID",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(vaultAccountID ) #",
                },
                {
                    field: "accountsPayableAccountID",
                    title: "Payable Acc ID",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(accountsPayableAccountID ) #",
                },
                {
                    field: "interestExpenseAccountID",
                    title: "Int Expense",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(interestExpenseAccountID ) #",
                },
                {
                    field: "chargesIncomeAccountID",
                    title: "Charges Income Account ID",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(chargesIncomeAccountID ) #",
                },
                {
                    field: "interestPayableAccountID",
                    title: "Int Payable",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(interestPayableAccountID ) #",
                },
                {
                    command: ["edit", "destroy"],
                    width: 120,
                },
            ],
            toolbar: [
                {
                    name: "create",
                    text: "Add New Deposit Product",
                },
                "pdf",
                "excel"
            ],
            excel: {
                fileName: "depositProducts.xlsx"
            },
            pdf: {
                paperKind: "A3",
                landscape: true,
                fileName: "depositProducts.pdf"
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
                editWindow.title("Edit Client Investment Products Data");
            },
            save: function(e) {
                $('.k-grid-update').css('display', 'none');
            },
            mobile: true,
            reorderable: true,
            resizable: true,
        });
    };

    depositTypeUI.prototype.accountDropDownEditor = function (container, options) {
        try {
            $('<input required data-text-field="acc_name" data-value-field="acct_id" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: accounts
                });
        }
        catch (e) { }
    }

    depositTypeUI.prototype.planDropDownEditor = function (container, options) {
        try {
            $('<input required data-text-field="planName" data-value-field="planId" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: plans
                });
        }
        catch (e) { }
    }

    return depositTypeUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function depositTypeIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function getAccountName(accountID) {
    var accountName = "";

    try {
        for (var i = 0; i < accounts.length; i++) {
            if (accounts[i].acct_id === accountID) {
                accountName = accounts[i].acc_name;
            }
        }
    }
    catch (e) { }

    return accountName;
}

function getPlanName(planId) {
    var planName = "";

    try {
        for (var i = 0; i < plans.length; i++) {
            if (plans[i].planId === planId) {
                planName = plans[i].planName;
            }
        }
    }
    catch (e) { }

    return planName;
}

var longTextEditor = function (container, options) {
    $('<input class="input-control" data-bind="value:' + options.field + '" style="width:500px;"/>')
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

