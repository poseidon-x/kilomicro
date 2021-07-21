/*
UI Scripts for Setcors Management
Creator: prince@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var genericCheckListApiUrl = coreERPAPI_URL_Root + "/crud/genericCheckList";
var genericCheckListPdfExportUrl = coreERPAPI_URL_Root + "/Export/genericCheckList/Pdf?token=" + authToken;

$(function () {
    var ui = new genericCheckListUI();
    ui.renderGrid();
});

var genericCheckListUI = (function () {
    function genericCheckListUI() {
    }
    genericCheckListUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: genericCheckListApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: genericCheckListApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: genericCheckListApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: genericCheckListApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "genericCheckListID"
                    }
                }
            },
            columns: [
                { field: "description", title: "Generic CheckList", width: 400 },
                { command: ["edit", "destroy"] }
            ],
            toolbar: [
                {
                    name: "create",
                    title: "Add Generic CheckList",
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true
        });
    };

    return genericCheckListUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function genericCheckListIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function exportToPdf(e) {
    alert(genericCheckListPdfExportUrl);
    window.open(genericCheckListPdfExportUrl, "_blank");
    return false;
}
//# sourceMappingURL=genericCheckList.js.map
