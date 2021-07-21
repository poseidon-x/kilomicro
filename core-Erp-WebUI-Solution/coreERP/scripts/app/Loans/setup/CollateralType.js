/*
UI Scripts for Setcors Management
Creator: prince@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var collateralApiUrl = coreERPAPI_URL_Root + "/crud/CollateralType";
var SectorPdfExportUrl = coreERPAPI_URL_Root + "/Export/CollateralType/Pdf?token=" + authToken;

$(function () {
    var ui = new collateralU();
    ui.renderGrid();
});

var collateralU = (function () {
    function collateralU() {
    }
    collateralU.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: collateralApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: collateralApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: collateralApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: collateralApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "collateralTypeID"
                    }
                }
            },
            columns: [
                { field: "collateralTypeName", title: "Collateral Name", width: 400 },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                {
                    name: "create",
                    title: "Add New Collateral Type",
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true
        });
    };

    return collateralU;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function collateralIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function exportToPdf(e) {
    alert(SectorPdfExportUrl);
    window.open(SectorPdfExportUrl, "_blank");
    return false;
}
//# sourceMappingURL=Sector.js.map
