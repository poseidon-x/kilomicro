/*
UI Scripts for Loan Category Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var categoryApiUrl = coreERPAPI_URL_Root + "/crud/Category";
var categoryPdfExportUrl = coreERPAPI_URL_Root + "/Export/Category/Pdf?token=" + authToken;
var categoryListApiUrl = coreERPAPI_URL_Root + "/crud/CategoryChecklist";

$(function () {
    var ui = new categoryUI();
    ui.renderGrid();
    $("#toolbar").click(exportToPdf);
});

var categoryUI = (function () {
    function categoryUI() {
    }
    categoryUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: categoryApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: categoryApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: categoryApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: categoryApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "categoryID"
                    }
                }
            },
            detailInit: this.detailInit,
            columns: [
                {
                    field: "categoryID", title: "Category ID", width: 200,
                    filterable: false
                },
                { field: "categoryName", title: "Category Name", width: 400 },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                { name: "create" },
                {
                    name: "pdf",
                    text: "Export to PDF",
                    imageClass: "k-icon k-i-custom",
                    imageUrl: "/images/pdf.jpg",
                    url: categoryPdfExportUrl,
                    template: kendo.template($("#toolbar-icon-pdf").html())
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true
        });
    };

    categoryUI.prototype.detailInit = function (e) {
        $("<div/>").appendTo(e.detailCell).kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: categoryListApiUrl,
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: categoryListApiUrl,
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: categoryListApiUrl,
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: categoryListApiUrl,
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "categoryCheckListID",
                        fields: {
                            categoryID: {
                                editable: true,
                                defaultValue: e.data.categoryID
                            },
                            categoryCheckListID: {
                                editable: false,
                                nullable: true
                            },
                            description: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            isMandatory: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            }
                        }
                    }
                },
                filter: { field: "categoryID", operator: "eq", value: e.data.categoryID }
            },
            scrollable: false,
            sortable: true,
            pageable: true,
            editable: "popup",
            columns: [
                { field: "description", title: "Checklist Item", width: "410px" },
                {
                    field: "isMandatory", title: "Mandatory?", editor: mandatoryCheckbox,
                    template: '<input type="checkbox" data-bind="checked: ' + 'isMandatory' + '" disabled="disabled" ></input>'
                },
                /*{
                field: "categoryID", disabled: true, /*editor: categoryIDEditor,
                title: "Category ID"
                },*/
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                { name: "create" }
            ]
        });
    };
    return categoryUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function categoryIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function exportToPdf(e) {
    alert(categoryPdfExportUrl);
    window.open(categoryPdfExportUrl, "_blank");
    return false;
}
//# sourceMappingURL=category.js.map
