/*
UI Scripts for Setcors Management
Creator: prince@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var industryApiUrl = coreERPAPI_URL_Root + "/crud/Industry";
var IndustryPdfExportUrl = coreERPAPI_URL_Root + "/Export/Industry/Pdf?token=" + authToken;

$(function () {
    var ui = new industryUI();
    ui.renderGrid();
});

var industryUI = (function () {
    function industryUI() {
    }
    industryUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: industryApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: industryApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: industryApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: industryApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "industryID"
                    }
                }
            },
            columns: [
                { field: "industryName", title: "Industry Name", width: 400 },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                {
                    name: "create",
                    title: "Add New Industry",
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true
        });
    };

    return industryUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function industryIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function exportToPdf(e) {
    alert(IndustryPdfExportUrl);
    window.open(IndustryPdfExportUrl, "_blank");
    return false;
}
//# sourceMappingURL=Industry.js.map
