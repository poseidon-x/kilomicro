/*
UI Scripts for Loan depositType Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var JDefault2ApiUrl = coreERPAPI_URL_Root + "/crud/JDefault2";
var branchApiUrl = coreERPAPI_URL_Root + "/crud/Branch";

$(function () {
    renderBranches();
    var ui = new jDefault2Ui();
    ui.renderGrid(-1);
    ui.renderAppGrid(-1);
    ui.renderUndGrid(-1);
    ui.renderTopTenGrid(-1);
});

var jDefault2Ui = (function () {
    function jDefault2Ui() {
    }
    jDefault2Ui.prototype.renderGrid = function (selectedBranch) {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#dueGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: JDefault2ApiUrl + "/GetDue/" + selectedBranch,
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
                pageSize: 5,
                schema: {
                    // the array of repeating data elements (employees)
                    data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "loanID",
                        fields:
                           {
                               clientID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               loanNo: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               clientName: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountDue: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               dateDue: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               staffName: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               loanID: {
                                   validation: {
                                       required: true,
                                   },
                               },
                           }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },
            columns: [
                {
                    field: "clientID",
                    title: "Client ID",
                    width: 100
                },
                {
                    field: "loanNo",
                    title: "Loan NO",
                    width: 100
                }, 
                {
                    field: "clientName",
                    title: "Clent Name",
                    width: 150
                },
                {
                    field: "amountDue",
                    title: "Amount Due",
                    width: 100,
                    format: "{0: #,##0.#0}",
                },
               {
                   field: "dateDue",
                   title: "Date Due",
                   width: 100,
                   format: "{0:dd-MMM-yyyy}"           
               },
               {
                   field: "staffName",
                   title: "Rel. Officer",
                   width: 150
               }, 
                {
                    command: [
                        {
                            name: "receive",
                            text: "Receive",
                            click: function (e) {
                                // e.target is the DOM element representing the button
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                // get the data bound to the current table row
                                var data = this.dataItem(tr);
                                window.location = '/ln/cashier/receipt.aspx?id=' + data.loanID;
                            }
                        },
                    ],
                    width: 100,
                },
            ],
            filterable: true,
            sortable: {
                mode: "multiple",
            },
            pageable: {
                pageSize: 5,
                pageSizes: [5, 10, 25, 50, 100, 1000, ],
                previousNext: true,
                buttonCount: 5,
            },
            groupable: true,
            selectable: true,
            toolbar: ["excel", "pdf"],
            excel: {
                fileName: "und.xlsx",
                allPages: true,
            },
            reorderable: true,
            resizable: true,
            pdf: {
                paperKind: "A3",
                fileName: "und.pdf",
                landscape: true,
                allPages: true
            }
        });
    };
    //
    jDefault2Ui.prototype.renderAppGrid = function (selectedBranch) {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#appGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: JDefault2ApiUrl + "/GetApp/" + selectedBranch,
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
                pageSize: 5,
                schema: {
                    // the array of repeating data elements (employees)
                    data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "loanID",
                        fields:
                           {
                               clientID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               loanNo: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               clientName: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountRequested: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountApproved: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },
                               finalApprovalDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               applicationDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               loanID: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               staffName: {
                                   validation: {
                                       required: true,
                                   },
                               }
                           }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },
            columns: [
                {
                    field: "clientID",
                    title: "Client ID",
                    width: 100
                },
                {
                    field: "loanNo",
                    title: "Loan NO",
                    width: 100
                },
                {
                    field: "clientName",
                    title: "Clent Name",
                    width: 150
                },
                {
                    field: "amountRequested",
                    title: "Amnt Requested",
                    width: 100,
                    format: "{0: #,##0.#0}",
                },
               {
                   field: "applicationDate",
                   title: "App Date",
                   width: 100,
                   format: "{0:dd-MMM-yyyy}"
               },
               {
                   field: "staffName",
                   title: "Rel. Officer",
                   width: 150
               },
                {
                    command: [
                        {
                            name: "checkList",
                            text: "Check List",
                            click: function (e) {
                                // e.target is the DOM element representing the button
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                // get the data bound to the current table row
                                var data = this.dataItem(tr);
                                window.location = '/ln/loans/loanCheckList.aspx?id=' + data.loanID + '&catID=' + data.categoryID;
                            }
                        },
                        {
                            name: "approve",
                            text: "Approve",
                            click: function (e) {
                                // e.target is the DOM element representing the button
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                // get the data bound to the current table row
                                var data = this.dataItem(tr);
                                window.location = '/ln/loans/approve.aspx?id=' + data.loanID + '&catID=' + data.categoryID;
                            }
                        },
                    ],
                    width: 200,
                },
            ],
            filterable: true,
            sortable: {
                mode: "multiple",
            },
            pageable: {
                pageSize: 5,
                pageSizes: [5, 10, 25, 50, 100, 1000, ],
                previousNext: true,
                buttonCount: 5,
            },
            groupable: true,
            selectable: true,
            toolbar: ["excel", "pdf"],
            excel: {
                fileName: "app.xlsx",
                allPages: true,
            },
            reorderable: true,
            resizable: true,
            pdf: {
                paperKind: "A3",
                fileName: "app.pdf",
                landscape: true,
                allPages: true
            }
        });
    };
    //
    jDefault2Ui.prototype.renderUndGrid = function (selectedBranch) {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#undGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: JDefault2ApiUrl + "/GetUnd/" + selectedBranch,
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
                pageSize: 5,
                schema: {
                    // the array of repeating data elements (employees)
                    data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "loanID",
                        fields:
                           {
                               clientID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               loanNo: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               clientName: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountRequested: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountApproved: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },
                               finalApprovalDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               loanID: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               staffName: {
                                   validation: {
                                       required: true,
                                   },
                               },//
                               applicationDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               categoryID: {
                                   validation: {
                                       required: true,
                                   },
                               }
                           }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },
            columns: [
                {
                    field: "clientID",
                    title: "Client ID",
                    width: 100
                },
                {
                    field: "loanNo",
                    title: "Loan NO",
                    width: 100
                },
                {
                    field: "clientName",
                    title: "Clent Name",
                    width: 150
                },
                {
                    field: "amountRequested",
                    title: "Amount Requested",
                    width: 100,
                    format: "{0: #,##0.#0}",
                },//
                {
                    field: "applicationDate",
                    title: "Application Date",
                    width: 150,
                    format: "{0:dd-MMM-yyyy}"
                },
                {
                    field: "amountApproved",
                    title: "Amount Approved",
                    width: 100,
                    format: "{0: #,##0.#0}",
                },
               {
                   field: "finalApprovalDate",
                   title: "Approved Date",
                   width: 100,
                   format: "{0:dd-MMM-yyyy}"
               },
               {
                   field: "staffName",
                   title: "Rel. Officer",
                   width: 150
               },
                {
                    command: [                     
                        {
                            name: "disburse",
                            text: "Disburse",
                            click: function (e) {
                                // e.target is the DOM element representing the button
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                // get the data bound to the current table row
                                var data = this.dataItem(tr);
                                window.location = '/ln/cashier/disburse.aspx?id=' + data.loanID + '&catID=' + data.categoryID;
                            }
                        },
                    ],
                    width: 100,
                },
            ],
            filterable: true,
            sortable: {
                mode: "multiple",
            },
            pageable: {
                pageSize: 5,
                pageSizes: [5, 10, 25, 50, 100, 1000, ],
                previousNext: true,
                buttonCount: 5,
            },
            groupable: true,
            selectable: true,
            toolbar: ["excel", "pdf"],
            excel: {
                fileName: "und.xlsx",
                allPages: true,
            },
            reorderable: true,
            resizable: true,
            pdf: {
                paperKind: "A3",
                fileName: "und.pdf",
                landscape: true,
                allPages: true
            }
        });
    };
    //
    jDefault2Ui.prototype.renderTopTenGrid = function (selectedBranch) {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#toptenGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: JDefault2ApiUrl + "/GetTopTen/" + selectedBranch,
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
                pageSize: 5,
                schema: {
                    // the array of repeating data elements (employees)
                    data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "loanID",
                        fields:
                           {
                               loanID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               clientName: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountDisbursed: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },
                               totalDue: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },
                               lastPaymentDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               staffName: {
                                   validation: {
                                       required: true,
                                   },
                               },                              
                           }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },
            columns: [
                {
                    field: "loanNo",
                    title: "Loan NO",
                    width: 100
                },
                {
                    field: "clientName",
                    title: "Clent Name",
                    width: 150
                },
                {
                    field: "amountDisbursed",
                    title: "Amount Disbursed",
                    width: 100,
                    format: "{0: #,##0.#0}",
                },
                {
                    field: "totalDue",
                    title: "Total Due",
                    width: 100,
                    format: "{0: #,##0.#0}",
                },
               {
                   field: "lastPaymentDate",
                   title: "Last Payment Date",
                   width: 100,
                   format: "{0:dd-MMM-yyyy}"
               },
               {
                   field: "staffName",
                   title: "Rel. Officer",
                   width: 150
               },
                {
                    command: [
                        {
                            name: "manage",
                            text: "Manage",
                            click: function (e) {
                                // e.target is the DOM element representing the button
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                // get the data bound to the current table row
                                var data = this.dataItem(tr);
                                window.location = '/ln/loans/loan.aspx?id=' + data.loanID + '&catID=' + data.categoryID;
                            }
                        },
                    ],
                    width:100,
                }
            ],
            filterable: true,
            sortable: {
                mode: "multiple",
            }, 
            pageable: {
                pageSize: 5,
                pageSizes: [5, 10, 25, 50, 100, 1000, ],
                previousNext: true,
                buttonCount: 5,
            },
            groupable: true,
            selectable: true,
            toolbar: ["excel", "pdf"],
            excel: {
                fileName: "topTenLoans.xlsx",
                allPages: true,
            },
            reorderable: true,
            resizable: true,
            pdf: {
                paperKind: "A3",
                fileName: "topTenLoans.pdf",
                landscape: true,
                allPages: true
            }
        });
    };

    return jDefault2Ui;
})();
  

function renderBranches() {

    try {
        $('#cboBranch').kendoDropDownList({
            optionLabel: " ",
            autoBind: true,
            dataValueField: "branchID",
            dataTextField: "branchName",
            dataSource: {
                transport: {
                    read: {
                        url: branchApiUrl + "/Get",
                        type: "GET",
                        contentType: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    parameterMap: function (data, type) {
                        return kendo.stringify(data);
                    },
                },
            },
            change: function () {
                var selectedBranch = $('#cboBranch').val();
                var ui = new jDefault2Ui();
                ui.renderGrid(selectedBranch);
                ui.renderAppGrid(selectedBranch);
                ui.renderUndGrid(selectedBranch);
                ui.renderTopTenGrid(selectedBranch);
            },
        });
    }
    catch (e) { }
}
