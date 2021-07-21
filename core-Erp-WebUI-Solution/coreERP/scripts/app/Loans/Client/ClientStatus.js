//*******************************************
//***   CREATOR: BMB (bmensa-bonsu@acsghana.com)    	   
//***   WEEK: 3rd Jan, 2018  	
//*******************************************

"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanOutstandingApiUrl = coreERPAPI_URL_Root + "/crud/ClientLookUp";

var activeClients = {};
var flaggedClients = {};


function loadForm() {
            dismissLoadingDialog();
            renderGrid();      
}

//Function to call load form function

$(function () {
    displayLoadingDialog();
    loadForm();
});



function renderGrid() {
    displayLoadingDialog();

    $('#tabs').kendoTabStrip();
    dismissLoadingDialog();
    $('#activeGrid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: LoanOutstandingApiUrl + "/GetActiveClients",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 500,
            schema: {
                // the array of repeating data elements (employees)
                data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                total: "Count",
                model: {
                    id: "accountNumber",
                    fields: {
                        accountNumber: { editable: false },
                        Client_Name: { editable: false },
                        branchName: { editable: false },
                    }
                }
            },
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            group: [
                {
                    field: "branchName",
                    title: "Branch"                 
                }
            ]
        },
        columns: [
            { field: 'accountNumber', title: 'Account Number' },
			{ field: 'Client_Name', title: 'Client' }
			//{ field: 'branchName', title: 'Branch' },
            
        ],
        toolbar: [
           { name: "pdf", text: "Export to PDF" },
           { name: "excel", text: "Export to Excel" },

        ],
        pdf: {
            landscape: true,
            paperSize: "A3",
            fileName: "ActiveClients.pdf", allPages: true
        },
        excel: {
            fileName: "ActiveClients.xlsx", allPages: true
        },
        navigatable: true,
        groupable: false,
        pageable: {
            pageSize: 500,
            pageSizes: [500, 1000, 1500,3000,4500,7000],
            previousNext: true,
            buttonCount: 5
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });

    dismissLoadingDialog();
    $('#flaggedGrid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: LoanOutstandingApiUrl + "/GetFlaggedClients",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 500,
            schema: {
                // the array of repeating data elements (employees)
                data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                total: "Count",
                model: {
                    id: "accountNumber",
                    fields: {
                        accountNumber: { editable: false },
                        Client_Name: { editable: false },
                        branchName: { editable: false },
                    }
                }
            },
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            group: [
                {
                    field: "branchName",
                    title: "Branch"
                }
            ]
        },
        columns: [
            { field: 'accountNumber', title: 'Account Number' },
			{ field: 'Client_Name', title: 'Client' },
			//{ field: 'branchName', title: 'Branch' },

        ],
        toolbar: [
         { name: "pdf", text: "Export to PDF" },
         { name: "excel", text: "Export to Excel" },

        ],
        pdf: {
            landscape: true,
            paperSize: "A3",
            fileName: "FlaggedClients.pdf", allPages: true
        },
        excel: {
            fileName: "FlaggedClients.xlsx", allPages: true
        },
        navigatable: true,
        groupable: false,
        pageable: {
            pageSize: 500,
            pageSizes: [500, 1000, 1500, 3000,4500,7000],
            previousNext: true,
            buttonCount: 5,
        },

    });
}






