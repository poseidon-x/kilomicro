/*
UI Scripts for Loan Category Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var providerApiUrl = coreERPAPI_URL_Root + "/crud/MomoProvider";
var providerPdfExportUrl = coreERPAPI_URL_Root + "/Export/MomoProvider/Pdf?token=" + authToken;
var servicesApiUrl = coreERPAPI_URL_Root + "/crud/MomoProviderService";
var chargesApiUrl = coreERPAPI_URL_Root + "/crud/MomoProviderServiceCharge";

$(function () {
    var ui = new providerUI();
    ui.renderGrid();
    $("#toolbar").click(exportToPdf);
});

var providerUI = (function () {
    function providerUI() {
    }
    providerUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: providerApiUrl +"/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: providerApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: providerApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: providerApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "providerID",
                        fields: {
                            providerID: {
                                editable: false,
                                type: "number"
                            }, 
                            providerShortName: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            providerFullName: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            momoProductName: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            }
                        }
                    }
                }
            },
            detailInit: this.serviceInit,
            columns: [ 
                { field: "providerShortName", title: "Short Name", width: 200 },
                { field: "providerFullName", title: "Company Name", width: 300 },
                { field: "momoProductName", title: "Product Name", width: 300 },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                { name: "create", text: "Add Provider" },
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

    providerUI.prototype.serviceInit = function (e) {
        $("<div/>").appendTo(e.detailCell).kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: servicesApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: servicesApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: servicesApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: servicesApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "serviceID",
                        fields: {
                            providerID: {
                                editable: true,
                                defaultValue: e.data.providerID
                            },
                            serviceID: {
                                editable: false,
                                nullable: true
                            },
                            serviceName: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            } 
                        }
                    }
                },
                filter: { field: "providerID", operator: "eq", value: e.data.providerID }
            },
            scrollable: false,
            sortable: true,
            pageable: true,
            editable: "popup",
            detailInit: providerUI.prototype.chargesInit,
            columns: [
                { field: "serviceName", title: "Mobile Money Service Name", width: "410px" },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                { name: "create", text: "Add Provider Service" }
            ]
        });
    };

    providerUI.prototype.chargesInit = function (e) {
        $("<div/>").appendTo(e.detailCell).kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: chargesApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: chargesApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: chargesApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: chargesApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "serviceChargeID",
                        fields: {
                            serviceID: {
                                editable: true,
                                defaultValue: e.data.serviceID,
                                type: "number"
                            },
                            serviceChargeID: {
                                editable: false,
                                nullable: true,
                                type: "number"
                            },
                            minTranAmount: {
                                editable: true,
                                validation: {
                                    required: true
                                },
                                type: "number"
                            },
                            maxTranAmount: {
                                editable: true,
                                validation: {
                                    required: true
                                },
                                type: "number"
                            },
                            chargesValue: {
                                editable: true,
                                validation: {
                                    required: true
                                },
                                type: "number"
                            },
                            isPercent: {
                                editable: true,
                                type: "boolean"
                            }
                        }
                    }
                },
                filter: { field: "serviceID", operator: "eq", value: e.data.serviceID }
            },
            scrollable: false,
            sortable: true,
            pageable: true,
            editable: "popup",
            columns: [
                { field: "minTranAmount", title: "Min. Transaction", width: "150px" },
                { field: "maxTranAmount", title: "Max. Transaction", width: "150px" },
                { field: "chargesValue", title: "Charges Value", width: "150px" },
                {
                    field: "isPercent", title: "Percentage?", width: "150px",
                    template: '<input type="checkbox" data-bind="checked: isPercent" disabled="disabled" ></input>'
                },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                { name: "create", text: "Add Service Charge" }
            ]
        });
    };

    return providerUI;
})();

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
