/*
UI Scripts for Loan savingType Management
Creator: man@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed

"use strict";
var authToken = coreERPAPI_Token;
var depositInterestUpgradeApiUrl = coreERPAPI_URL_Root + "/crud/depositInterestUpgrade";

$(function () {
  $("#tabs").kendoTabStrip();
    var ui = new depositInterestUpgradeUi();
	ui.renderGrid();
});

var depositInterestUpgradeUi = (function () {
    function depositInterestUpgradeUi() {
    }
    depositInterestUpgradeUi.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid

		$("#grid")
            .kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: depositInterestUpgradeApiUrl +"/GetProposedPendingApproval",
                        type: "POST",
                        contentType: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: depositInterestUpgradeApiUrl +"/PostApproval",
                        type: "POST",
                        contentType: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    parameterMap: function (data, type) {
                        return kendo.stringify(data);
                    },
                },
                pageSize: 10,
                schema: {
                    // the array of repeating data elements (employees)
                    data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "depositRateUpgradeId",
                        fields: {
                            depositRateUpgradeId: { editable: false, type: "number" },
                            depositId: { editable: false, validation: { required: true } },
							client: { editable: false, validation: { required: true } },
                            depositNo: { editable: false, validation: { required: true } },
                            currentPrincipalBalance: { editable: false, validation: { required: true } },
                            currentRate: { editable: false, validation: { required: true } },
							proposedRate: { editable: false, validation: { required: true } },
                            approved: { type: "boolean", editable: true, validation: { required: true } },

						}
					}
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },
            columns: [
				{ field: "client", title: "Client" },
                { field: "depositNo", title: "Investment No." },
				{ field: "currentPrincipalBalance", title: "Principal Bal.",format: "{0: #,###.#0}" },
                { field: "currentRate", title: "Current Rate" },
                { field: "proposedRate", title: "Proposed Rate"},
                { field: "approved", title: "Approve",template: '<input type="checkbox" #= approved ? \'checked="approved"\' : "" # disabled="disabled" />' },
                {
                    command: [
                      {
                          name: "edit",
                          text: "Approve"
                      },
                    ],
                    width: 110
                },
            ],
            filterable:true,
            sortable: {
                mode: "multiple",
            },
			editable: "popup",
            pageable: {
                pageSize: 10,
                pageSizes: [10, 25, 50, 100, 1000],
                previousNext: true,
                buttonCount: 5,
            },
            groupable: true,
            selectable: true,
        });
    };

    return depositInterestUpgradeUi;
})();
