/*
UI Scripts for Setcors Management
Creator: prince@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var sectorApiUrl = coreERPAPI_URL_Root + "/crud/Sector";
var SectorPdfExportUrl = coreERPAPI_URL_Root + "/Export/Sector/Pdf?token=" + authToken;

$(function () {
    var ui = new sectorUI();
    ui.renderGrid();
});

var sectorUI = (function () {
    function sectorUI() {
    }
    sectorUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: sectorApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: sectorApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: sectorApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: sectorApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "sectorID"
                    }
                }
            },
            columns: [
                { field: "sectorName", title: "Sector Name", width: 400 },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                {
                    name: "create",
                    title: "Add New Sector",
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true
        });
    };

    return sectorUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function sectorIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function exportToPdf(e) {
    alert(SectorPdfExportUrl);
    window.open(SectorPdfExportUrl, "_blank");
    return false;
}
//# sourceMappingURL=Sector.js.map
