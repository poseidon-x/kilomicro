/*
UI Scripts for Loan Category Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var chargeTypeApiUrl = coreERPAPI_URL_Root + "/crud/ChargeType";
var providerPdfExportUrl = coreERPAPI_URL_Root + "/Export/MomoProvider/Pdf?token=" + authToken;
var chargeTypeTierApiUrl = coreERPAPI_URL_Root + "/crud/ChargeTypeTier";
var chargesApiUrl = coreERPAPI_URL_Root + "/crud/MomoProviderServiceCharge";

$(function () {
    var ui = new chargeTypeUI();
    ui.renderGrid();
    $("#toolbar").click(exportToPdf);
});

var chargeTypeUI = (function () {
    function chargeTypeUI() {
    }
    chargeTypeUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: chargeTypeApiUrl +"/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: chargeTypeApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: chargeTypeApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: chargeTypeApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "chargeTypeID",
                        fields: {
                            chargeTypeID: {
                                editable: false,
                                type: "number"
                            }, 
                            chargeTypeCode: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            chargeTypeName: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            automatic: {
                                editable: true,
                                validation: {
                                    required: true
                                }, 
                            },
                        }
                    }
                }
            },
            detailInit: this.tierInit,
            columns: [ 
                {
                    field: "chargeTypeCode", title: "Code",
                },
                {
                    field: "chargeTypeName", title: "Charge Name",
                    editor: longTextEditor,
                },
                {
                    field: "automatic", title: "Automatic Deduction?",
                    editor: checkBoxTextEditor,
                    template: '<input type="checkbox" disabled="disabled" data-bind="checked: automatic" #= automatic? checked="checked":"" #/>'
                },
                {
                    command: ["edit", "destroy"
                    ]
                },
            ],
            toolbar: [
                { name: "create", text: "Add Charge Type" },
                {
                    name: "pdf",
                    text: "Export to PDF",
                    imageClass: "k-icon k-i-custom",
                    imageUrl: "/images/pdf.jpg",
                    url: providerPdfExportUrl,
                    template: kendo.template($("#toolbar-icon-pdf").html())
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true
        });
    };

    chargeTypeUI.prototype.tierInit = function (e) {
        $("<div/>").appendTo(e.detailCell).kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: chargeTypeTierApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: chargeTypeTierApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: chargeTypeTierApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: chargeTypeTierApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "chargeTypeTierId",
                        fields: {
                            chargeTypeId: {
                                editable: true,
                                defaultValue: e.data.chargeTypeID
                            },
                            chargeTypeTierId: {
                                editable: false,
                                nullable: true
                            },
                            percentCharge: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            minimumTransactionAmount: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            maximumTransactionAmount: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            minChargeAmount: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            maturityPercentCharge: {
                                editable: true,
                                validation: {
                                    required: true,
                                },
                            },
                        }
                    }
                },
                filter: {
                    field: "chargeTypeId",
                    operator: "eq", value: e.data.chargeTypeID
                },
            },
            scrollable: false,
            sortable: true,
            pageable: true,
            editable: "popup", 
            columns: [ 
                {
                    field: "minimumTransactionAmount", title: "Min. Transaction Amount",
                    editor: numericEditor,
                    format: '{0:#,##0.#0}',
                },
                {
                    field: "maximumTransactionAmount", title: "Max. Transaction Amount",
                    editor: numericEditor,
                    format: '{0:#,##0.#0}',
                },
                {
                    field: "maturityPercentCharge", title: "Before Maturity Charge (%)",
                    editor: numericEditor,
                    format: '{0:#,##0.#0}',
                },
                {
                    field: "percentCharge", title: "Below Available Bal. Charge (%)",
                    editor: numericEditor,
                    format: '{0:#,##0.#0}',
                },
                {
                    field: "minChargeAmount", title: "Min. Charge",
                    editor: numericEditor,
                    format: '{0:#,##0.#0}',
                },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                { name: "create", text: "Add Charge Tier" }
            ]
        });
    };
     
    return chargeTypeUI;
})();

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

function categoryIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function exportToPdf(e) { 
    window.open(providerPdfExportUrl, "_blank");
    return false;
} 
