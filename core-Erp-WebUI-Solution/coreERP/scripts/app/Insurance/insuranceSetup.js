/*
UI Scripts for Loan insuranceSetup Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var insuranceSetupApiUrl = coreERPAPI_URL_Root + "/crud/insuranceSetup";
var accountApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var loanTypeApiUrl = coreERPAPI_URL_Root + "/crud/LoanType";

var accounts = [];
var loanTypes = [];

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

        $.ajax({
            url: loanTypeApiUrl + "/Get",
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            loanTypes = data;

            var ui = new insuranceSetupUI();
            ui.renderGrid();
        });
    });
});

var insuranceSetupUI = (function () {
    function insuranceSetupUI() {
    }
    insuranceSetupUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: insuranceSetupApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: insuranceSetupApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: insuranceSetupApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: insuranceSetupApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "insuranceSetupID",
                        fields:
                           {
                               insuranceSetupID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               loanTypeID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               insurancePercent: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               insuranceAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               isEnabled: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },                              
                           }
                    }
                }
            },
            columns: [               
                {
                    field: "loanTypeID",
                    title: "Loan Type ",
                    width: 150,
                    editor: this.loanTypeDropDownEditor,
                    template: "#= getLoanTypeName(loanTypeID) #"
                },
                {
                    field: "insurancePercent",
                    title: "Insurance Percent",
                    width: 75,
                    editor: numericEditor,
                    format: "{0: #,##0.#0}",
                },
                {
                    field: "insuranceAccountID",
                    title: "Insurance Account",
                    width: 150,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(insuranceAccountID) #",
                },
                {
                    field: "isEnabled",
                    title: "Enabled?",
                    width: 75,
                    editor: checkBoxTextEditor,
                    template: '<input type="checkbox" disabled="disabled" data-bind="checked: isEnabled" #= isEnabled? checked="checked":"" #/>',
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
        });
    };

    insuranceSetupUI.prototype.accountDropDownEditor = function (container, options) {
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

    insuranceSetupUI.prototype.loanTypeDropDownEditor = function (container, options) {
        try {
            $('<input required data-text-field="loanTypeName" data-value-field="loanTypeID" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: loanTypes
                });
        }
        catch (e) { }
    }

    insuranceSetupUI.prototype.planDropDownEditor = function (container, options) {
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

    return insuranceSetupUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function insuranceSetupIDEditor(container, options) {
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

function getLoanTypeName(loanTypeID) {
    var loanTypeName = "";

    try {
        for (var i = 0; i < loanTypes.length; i++) {
            if (loanTypes[i].loanTypeID === loanTypeID) {
                loanTypeName = loanTypes[i].loanTypeName;
            }
        }
    }
    catch (e) { }

    return loanTypeName;
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
            decimals: 2,
            min: 0,
            format: '#,##0.#0'
        });
};

function booleanCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

