/*
UI Scripts for Loan insuranceSetup Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/Currency";
var accountApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var CurrencyApiUrl = coreERPAPI_URL_Root + "/crud/Currency";

var accounts = [];
var currencies = [];

$(function () {
    var ui = new currencyUI();
    ui.renderGrid();
});

var currencyUI = (function () {
    function currencyUI() {
    }
    currencyUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: currencyApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: currencyApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: currencyApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: currencyApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "currency_id ",
                        fields:
                           {
                               currency_id: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               major_name: {
                                   validation: {
                                       required: true,
                                   }, 
                               },
                               minor_name: {
                                   validation: {
                                       required: true,
                                   }, 
                               },
                               major_symbol: {
                                   validation: {
                                       required: true,
                                   }, 
                               },
                               minor_symbol: {
                                   validation: {
                                       required: true,
                                   }, 
                               },
                               //

                               current_buy_rate: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 1,
                               },
                               current_sell_rate: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 1,
                               },
                               creation_date: {
                                   validation: {
                                       required: false,
                                   },
                                   defaultValue: new Date(),
                               },//
                               creator: {
                                   validation: {
                                       required:  false,
                                   }, 
                               },
                               modification_date: {
                                   validation: {
                                       required: false,
                                   },
                                   defaultValue: new Date(),
                               },
                               //
                               last_modifier: {
                                   validation: {
                                       required: false,
                                   }, 
                               }
                           }
                    }
                }
            },
            columns: [
                
                {
                    field: "major_name",
                    title: "Major Name",
                    width: 150,
                },
                {
                    field: "minor_name",
                    title: "Minor Name",
                    width: 150,
                },
                {
                    field: "major_symbol",
                    title: "Major Symbol",
                    width: 140,                  
                },//
                {
                    field: "minor_symbol",
                    title: "Minor Symbol",
                    width: 75,
                },
                {
                    field: "current_buy_rate",
                    title: "Buy",
                    width: 150,
                    format: "{0: #,##0.#0}",
                    editor: numericEditor,
                },
                {
                    field: "current_sell_rate",
                    title: "Sell",
                    width: 75,
                    format: "{0: #,##0.#0}",
                    editor: numericEditor,
                },//                      
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
                    title: "Add New Currency",
                }
            ],
        });
    };

    currencyUI.prototype.accountDropDownEditor = function (container, options) {
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

    currencyUI.prototype.CurrencyDropDownEditor = function (container, options) {
        try {
            $('<input required data-text-field="currency_id" data-value-field="currency_id" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: currencies
                });
        }
        catch (e) { }
    }

    currencyUI.prototype.planDropDownEditor = function (container, options) {
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

    return currencyUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function insuranceSetupIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function getCurrencyID(currency_id) {
    var CurrencyName = "";

    try {
        for (var i = 0; i < currencies.length; i++) {
            if (Currencys[i].currency_id === currency_id) {
                CurrencyName = currencies[i].currency_id;
            }
        }
    }
    catch (e) { }

    return CurrencyName;
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

