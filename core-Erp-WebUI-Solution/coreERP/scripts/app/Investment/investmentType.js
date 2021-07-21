/*
UI Scripts for Loan investmentType Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var investmentTypeApiUrl = coreERPAPI_URL_Root + "/crud/investmentType";
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

        var ui = new investmentTypeUI();
        ui.renderGrid();
    });
});

var investmentTypeUI = (function () {
    function investmentTypeUI() {
    }
    investmentTypeUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: investmentTypeApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: investmentTypeApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: investmentTypeApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: investmentTypeApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "investmentTypeID",
                        fields:
                           {
                               investmentTypeID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               investmentTypeName: {
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
                                       required: false,
                                   },
                                   defaultValue: 0,
                               },
                               fxRealizedGainLossAccountID: {
                                   validation: {
                                       required: false,
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
                                       required: false,
                                   },
                                   defaultValue: 0,
                               },
                               interestReceivableAccountID: {
                                   validation: {
                                       required: false,
                                   },
                                   defaultValue: 0,
                               },

                           }
                    }
                }
            },
            columns: [
                {
                    field: "investmentTypeName",
                    title: "Investment Product",
                    width: 150,
                    editor: longTextEditor,
                },
                {
                    field: "interestRate",
                    title: "Int Rate",
                    width: 75,
                    editor: numericEditor,
                    format: "{0: #,##0.####0}",
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
                    template: "<input type='checkbox' #= allowsInterestWithdrawal? 'checked' : '' # enabled='false' />",
                },
                {
                    field: "allowsPrincipalWithdrawal",
                    title: "Princ. With?",
                    width: 75,
                    editor: checkBoxTextEditor,
                    template: "<input type='checkbox' #= allowsPrincipalWithdrawal? 'checked' :'' # enabled='false' /> ",
                },
                {
                    field: "vaultAccountID",
                    title: "Vault Acc",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(vaultAccountID ) #",
                },
                {
                    field: "accountsPayableAccountID",
                    title: "Acc Receivable (Princ)",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(accountsPayableAccountID ) #",
                },
                {
                    field: "interestExpenseAccountID",
                    title: "Int Income",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(interestExpenseAccountID) #",
                },
                {
                    field: "chargesIncomeAccountID",
                    title: "Charges Income",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(chargesIncomeAccountID ) #",
                },
                {
                    field: "interestReceivableAccountID",
                    title: "Int. Receivable",
                    width: 120,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(interestReceivableAccountID ) #",
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
                    title: "Add New Record",
                }
            ],
            edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
                editWindow.wrapper.css({ width: 550 });
                editWindow.title("Edit Client Investment Products");
            },
            save: function (e) {
                $('.k-grid-update').css('display', 'none');
            },
        });
    };

    investmentTypeUI.prototype.accountDropDownEditor = function (container, options) {
        try {
            $('<input style="width: 350px" required data-text-field="acc_name" data-value-field="acct_id" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: accounts
                });
        }
        catch (e) { }
    }

    investmentTypeUI.prototype.planDropDownEditor = function (container, options) {
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

    return investmentTypeUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function investmentTypeIDEditor(container, options) {
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
    $('<input class="input-control" data-bind="value:' + options.field + '" style="width:350px;"/>')
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
            format: '#,##0.####0'
        });
};

function booleanCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

