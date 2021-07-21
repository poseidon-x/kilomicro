/*
UI Scripts for Loan Category Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var walletApiUrl = coreERPAPI_URL_Root + "/crud/MomoWallet";
var walletPdfExportUrl = coreERPAPI_URL_Root + "/Export/MomoWallet/Pdf?token=" + authToken;
var providerApiUrl = coreERPAPI_URL_Root + "/crud/MomoProvider";
var accountApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var cashierApiUrl = coreERPAPI_URL_Root + "/crud/Cashier";

var accounts = [];
var cashiers = [];
var providers = [];

$(function () {

    $.ajax({
        url: accountApiUrl + "/Get",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        accounts = data;

        $.ajax({
            url: cashierApiUrl + "/Get",
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            cashiers = data;

            $.ajax({
                url: providerApiUrl + "/Get",
                beforeSend: function (req) {
                    req.setRequestHeader('Authorization', "coreBearer " + authToken);
                }
            }).done(function (data) {
                providers = data;

                var ui = new walletUI();
                ui.renderGrid();
                $("#toolbar").click(exportToPdf);
            });
        });
    });

});

var walletUI = (function () {
    function walletUI() {
    }
    walletUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: walletApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: walletApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: walletApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: walletApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "walletID",
                        fields: {
                            walletID: {
                                editable: false,
                                type: "number"
                            }, 
                            accountNumber: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            cashiersTillID: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            walletAccountID: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            chargesIncomeAccountID: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            chargesExpenseAccountID: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            balance: {
                                editable: false,
                                validation: {
                                    required: true
                                },
                                defaultValue: 0
                            },
                            providerID: {
                                editable: true,
                                validation: {
                                    required: true
                                },
                                type: "number"
                            }
                        }
                    }
                }
            }, 
            columns: [ 
                { field: "accountNumber", title: "Account Number", width: 150 },
                {
                    field: "providerID", title: "Wallet Provider", width: 200,
                    editor: this.providerDropDownEditor,
                    template: "#= getProviderName(providerID) #"
                },
                {
                    field: "cashiersTillID", title: "Cashier's Name", width: 250,
                    editor: this.cashierDropDownEditor,
                    template: "#= getCashierName(cashiersTillID) #"
                },
                {
                    field: "walletAccountID", title: "Wallet Account", width: 200,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(walletAccountID) #"
                },
                {
                    field: "chargesIncomeAccountID", title: "Income Account", width: 200,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(chargesIncomeAccountID) #"
                },
                {
                    field: "chargesExpenseAccountID", title: "Expenses Account", width: 200,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(chargesExpenseAccountID) #"
                },
                {
                    field: "balance", title: "Balance", width: 100
                },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                { name: "create", text: "Add Wallet" },
                {
                    name: "pdf",
                    text: "Export to PDF",
                    imageClass: "k-icon k-i-custom",
                    imageUrl: "/images/pdf.jpg",
                    url: walletPdfExportUrl,
                    template: kendo.template($("#toolbar-icon-pdf").html())
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true,
            edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
                editWindow.wrapper.css({ width: 700 });
            }
        });
    };

    walletUI.prototype.providerDropDownEditor = function(container, options) {
        $('<input required data-text-field="momoProductName" data-value-field="providerID" data-bind="value:' + options.field + '"/>')
            .width(450)
            .appendTo(container) 
            .kendoDropDownList({
                autoBind: false,
                optionLabel: " ",
                dataSource: providers
            });
    }

    walletUI.prototype.accountDropDownEditor = function (container, options) {
        $('<input required data-text-field="acc_name" data-value-field="acct_id" data-bind="value:' + options.field + '"/>')
            .width(450)
            .appendTo(container)
            .kendoDropDownList({
                optionLabel: " ",
                autoBind: false,
                dataSource: accounts
            });
    }

    walletUI.prototype.cashierDropDownEditor = function (container, options) {
        $('<input required data-text-field="Description" data-value-field="ID" data-bind="value:' + options.field + '"/>')
            .width(450)
            .appendTo(container)
            .kendoDropDownList({
                optionLabel: " ",
                autoBind: false,
                dataSource: cashiers 
            });
    }

    return walletUI;
})();

function exportToPdf(e) { 
    window.open(walletPdfExportUrl, "_blank");
    return false;
} 

function getAccountName(accountID) {
    var accountName = "";

    for (var i = 0; i < accounts.length; i++) {
        if (accounts[i].acct_id == accountID) {
            accountName = accounts[i].acc_name;
        }
    }

    return accountName;
}

function getCashierName(cashierID) {
    var cashierName = "";

    for (var i = 0; i < cashiers.length; i++) {
        if (cashiers[i].ID == cashierID) {
            cashierName = cashiers[i].Description;
        }
    }

    return cashierName;
}

function getProviderName(providerID) {
    var providerName = "";
   
    for (var i = 0; i < providers.length; i++) {
        if (providers[i].providerID == providerID) {
            providerName = providers[i].momoProductName;
        }
    }

    return providerName;
}
