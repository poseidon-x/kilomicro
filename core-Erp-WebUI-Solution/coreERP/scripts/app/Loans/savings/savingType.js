/*
UI Scripts for Loan savingType Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var savingTypeApiUrl = coreERPAPI_URL_Root + "/crud/savingType";
var accountApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";

var accounts = [];

var plans = [
    { planId: null, planName: "None", },
    { planId: 1, planName: "Daily", },
    { planId: 7, planName: "Weekly", },
    { planId: 14, planName: "Forthnightly", },
    { planId: 30, planName: "Monthly", },
    { planId: 92, planName: "Quarterly", },
    { planId: 182, planName: "Half Yearly", },
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

        var ui = new savingTypeUI();
        ui.renderGrid();
    });
});

var savingTypeUI = (function () {
    function savingTypeUI() {
    }
    savingTypeUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: savingTypeApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: savingTypeApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: savingTypeApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: savingTypeApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "savingTypeID",
                        fields:
                           {
                               savingTypeID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               savingTypeName: {
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
                               interestPayableAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               chargesIncomeAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               minPlanAmount: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               maxPlanAmount: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               planID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               earlyWithdrawalChargeRate: {
                                   validation: {
                                       required: true,
                                   }
                               },
                               minDaysBeforeInterest: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               }                                                           
                           }
                    }
                }
            },
            columns: [
                {
                    field: "savingTypeName",
                    title: "Deposit Product",
                    width: 150
                },
                {
                    field: "interestRate",
                    title: "Int Rate",
                    width: 75,
                    editor: numericEditor,
                    format: "{0: #,##0.#####0}",
                },
                {
                    field: "defaultPeriod",
                    title: "Default Period",
                    width: 75,
                    editor: numericEditor,
                    format: "{0: #,##0}",
                },
                {
                    field: "allowsInterestWithdrawal",
                    title: "Int With?",
                    width: 75,
                    editor: checkBoxTextEditor,
                    //template: "#= <input type='checkbox' data-bind='checked:allowsInterestWithdrawal' enabled='false' /> #",
                },
                {
                    field: "allowsPrincipalWithdrawal",
                    title: "Princ. With?",
                    width: 75,
                    editor: checkBoxTextEditor,
                    //template: "#= <input type='checkbox' data-bind='checked:allowsPrincipalWithdrawal' enabled='false' /> #",
                },
                {
                    field: "vaultAccountID",
                    title: "Vault Account ID",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(vaultAccountID ) #",
                },
                {
                    field: "accountsPayableAccountID",
                    title: "Payable Account ID",
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
                    field: "interestPayableAccountID",
                    title: "Int Payable",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(interestPayableAccountID ) #",
                },
                {
                    field: "chargesIncomeAccountID",
                    title: "Charges Income",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(chargesIncomeAccountID ) #",
                },
                {
                    field: "planID",
                    title: "Deposit Plan",
                    width: 120,
                    editor: this.planDropDownEditor,
                    template: "#= getPlanName(planID ) #",
                },
                {
                    field: "minPlanAmount",
                    title: "Min Plan Amount",
                    width: 120,
                    editor: numericEditor,
                    format: "{0: #,##0.#0}",
                },
                {
                    field: "maxPlanAmount",
                    title: "Max Plan Amount",
                    width: 120,
                    editor: numericEditor,
                    format: "{0: #,##0.#0}",
                },
                {
                    field: "earlyWithdrawalChargeRate",
                    title: "Early Charges",
                    width: 120,
                    editor: numericEditor,
                    format: "{0: #,##0.#0}",
                },
                {
                    field: "minDaysBeforeInterest",
                    title: "Days Before Int",
                    width: 120,
                    editor: numericEditor,
                    format: "{0: #,##0.#0}",
                },
                {
                    command: ["edit", "destroy"],
                    width: 120,
                },
            ],
            sortable: true,
            filterable: false,
            editable: "popup",
            pageable: true,
            toolbar: [
                {
                    name: "create",
                    title: "Add New Loan Type",
                }
            ],
        });
    };

    savingTypeUI.prototype.accountDropDownEditor = function (container, options) {
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

    savingTypeUI.prototype.planDropDownEditor = function (container, options) {
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

    return savingTypeUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function savingTypeIDEditor(container, options) {
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

