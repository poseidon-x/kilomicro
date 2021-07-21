/*
UI Scripts for Setcors Management
Creator: prince@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var lineOfBusinessApiUrl = coreERPAPI_URL_Root + "/crud/lineOfBusiness";
var lineOfBusinessPdfExportUrl = coreERPAPI_URL_Root + "/Export/lineOfBusiness/Pdf?token=" + authToken;

$(function () {
    var ui = new lineOfBusinessUI();
    ui.renderGrid();
});

var lineOfBusinessUI = (function () {
    function lineOfBusinessUI() {
    }
    lineOfBusinessUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: lineOfBusinessApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: lineOfBusinessApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: lineOfBusinessApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: lineOfBusinessApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "lineOfBusinessID"
                    }
                }
            },
            columns: [
                { field: "lineOfBusinessName", title: "Line Of Business Name", width: 400 },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                {
                    name: "create",
                    title: "Add New Line Of Business",
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true
        });
    };

    return lineOfBusinessUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function lineOfBusinessIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function exportToPdf(e) {
    alert(lineOfBusinessPdfExportUrl);
    window.open(lineOfBusinessPdfExportUrl, "_blank");
    return false;
}
//# sourceMappingURL=lineOfBusiness.js.map
