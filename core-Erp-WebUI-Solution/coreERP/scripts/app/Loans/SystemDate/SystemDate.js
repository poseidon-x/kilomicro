/*
UI Scripts for Loan savingType Management
Creator: man@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed

"use strict";
var authToken = coreERPAPI_Token;
var systemDateApiUrl = coreERPAPI_URL_Root + "/crud/systemDate";

$(function () {
    var ui = new systemDateUi();
	ui.renderGrid();
});

var systemDateUi = (function () {
    function systemDateUi() {
    }
    systemDateUi.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#tabs").kendoTabStrip();
		$("#grid")
            .kendoGrid({
            dataSource: {
                transport: {
                    read: {                        
                        url: systemDateApiUrl +"/GetKendoResponse",
                        type: "Post",
                        contentType: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
					update: {                        
                        url: systemDateApiUrl +"/PostSystemDate",
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
                        id: "systemDateID",
                        fields: {
                            systemDateID: { editable: false, type: "number" }, 
                            loanSystemDate: { type:"date", editable: true, validation: { required: false } },
                            depositSystemDate: { type:"date", editable: true, validation: { required: false } },							
							savingSystemDate: { type:"date", editable: true, validation: { required: false } },						
							investmentSystemDate: { type:"date", editable: true, validation: { required: false } }						
						}
					}                    
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },   
            columns: [
                { field: "loanSystemDate", title: "Loan", format: '{0: dd-MMM-yyyy}' },
                { field: "depositSystemDate", title: "Client Investment", format: '{0: dd-MMM-yyyy}' },
				{ field: "savingSystemDate", title: "Bank Account.", format: '{0: dd-MMM-yyyy}' },
                { field: "investmentSystemDate", title: "Company Investment", format: '{0: dd-MMM-yyyy}' },
				{ command: [ { name: "edit", text: "Update" } ], width: 130 },
            ], 
            filterable:true, 
            sortable: {
                mode: "multiple",
            }, 
			editable: "inline",
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
    
    return systemDateUi;
})();
