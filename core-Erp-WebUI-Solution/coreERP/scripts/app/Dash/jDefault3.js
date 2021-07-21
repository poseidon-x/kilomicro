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
    ui.renderInvestmentGrid(-1);
    ui.renderOverMaturedinvestmentGrid(-1);
    ui.renderTopTenInvestorsGrid(-1);
});

var jDefault2Ui = (function () {
    function jDefault2Ui() {
    }
    //
    jDefault2Ui.prototype.renderInvestmentGrid = function (selectedBranch) {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#investmentGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: JDefault2ApiUrl + "/GetMatureInvestment/" + selectedBranch,
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
                        id: "investmentId",
                        fields:
                           {
                               investmentId: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               clientID: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               clientName: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               investmentNo: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountDeposited: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },
                               interestAccrued: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountWithdrawn: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               firstDepositDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               maturityDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               interestRate: {
                                   validation: {
                                       required: true,
                                       type: "double",
                                   },
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
                    field: "clientName",
                    title: "Client Name",
                    width: 200
                },
                {
                    field: "investmentNo",
                    title: "Investment ID",
                    width: 100
                },
                {
                    field: "amountDeposited",
                    title: "Amt. Deposited",
                    width: 100,
                    format: "{0:#,##0.#0}",
                },
                {
                    field: "interestAccrued",
                    title: "Int. Accrued",
                    width: 100,
                    format: "{0:#,##0.#0}",
                },
               {
                   field: "amountWithdrawn",
                   title: "Amt. Widthdrawn",
                   width: 100,
                   format: "{0:#,##0.#0}",
               },
               {
                   field: "firstDepositDate",
                   title: "Start Date",
                   format: "{0:dd-MMM-yyyy}",
               },
               {
                   field: "maturityDate",
                   title: "Mat. Date",
                   format: "{0:dd-MMM-yyyy}",
               },
               {
                   field: "interestRate",
                   title: "Int Rate",
                   width: 100,
                   type:"double"
               },
               {
                   field: "staffName",
                   title: "Rel. Officer",
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
                fileName: "maturingInvestments.xlsx",
                allPages: true,
            },
            reorderable: true,
            resizable: true,
            pdf: {
                paperKind: "A3",
                fileName: "maturingInvestments.pdf",
                landscape: true,
                allPages: true
            }
        });
    };

    jDefault2Ui.prototype.renderOverMaturedinvestmentGrid = function (selectedBranch) {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#overMaturedinvestmentGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: JDefault2ApiUrl + "/GetOverMatureInvestment/" + selectedBranch,
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
                        id: "investmentId",
                        fields:
                           {
                               investmentId: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               clientID: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               clientName: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               investmentNo: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountDeposited: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },
                               interestAccrued: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountWithdrawn: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               firstDepositDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               maturityDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "date",
                               },
                               interestRate: {
                                   validation: {
                                       required: true,
                                       type: "double",
                                   },
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
                    field: "clientName",
                    title: "Client Name",
                    width: 200
                },
                {
                    field: "investmentNo",
                    title: "Investment ID",
                    width: 100
                },
                {
                    field: "amountDeposited",
                    title: "Amt. Deposited",
                    width: 100,
                    format: "{0:#,##0.#0}",
                },
                {
                    field: "interestAccrued",
                    title: "Int. Accrued",
                    width: 100,
                    format: "{0:#,##0.#0}",
                },
               {
                   field: "amountWithdrawn",
                   title: "Amt. Widthdrawn",
                   width: 100,
                   format: "{0:#,##0.#0}",
               },
               {
                   field: "firstDepositDate",
                   title: "Start Date",
                   format: "{0:dd-MMM-yyyy}",
               },
               {
                   field: "maturityDate",
                   title: "Mat. Date",
                   format: "{0:dd-MMM-yyyy}",
               },
               {
                   field: "interestRate",
                   title: "Int Rate",
                   width: 100,
                   type: "double"
               },
               {
                   field: "staffName",
                   title: "Rel. Officer",
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
                fileName: "overMaturedInvestments.xlsx",
                allPages: true,
            },
            reorderable: true,
            resizable: true,
            pdf: {
                paperKind: "A3",
                fileName: "overMaturedInvestments.pdf",
                landscape: true,
                allPages: true
            }
        });
    };

    //
    jDefault2Ui.prototype.renderTopTenInvestorsGrid = function (selectedBranch) {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#topTenInvestorsGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: JDefault2ApiUrl + "/GetTopTenInvestors/" + selectedBranch,
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
                        id: "investmentId",
                        fields:
                           {                              
                               clientID: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               clientName: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               investmentNo: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               amountInvested: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },
                               firstDepositDate: {
                                   validation: {
                                       required: true,
                                   },
                                   type:"date",
                               },
                               interestAccrued: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },//
                               principalBalance: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },//
                               interestBalance: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "double",
                               },//
                               amountWithdrawn: {
                                   validation: {
                                       required: true,
                                       type: "double",
                                   },
                               },//
                               phoneNumber: {
                                   validation: {
                                       required: true,
                                   },
                               },//
                               email: {
                                   validation: {
                                       required: true,
                                   },
                               },//
                               address: {
                                   validation: {
                                       required: true,
                                   },
                               },
                               currentBalance: {
                                   validation: {
                                       required: true,
                                   },
                                   type: "number",
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
                    title: "A/C No",
                    width: 100
                },
                {
                    field: "investmentNo",
                    title: "Inv No",
                    width: 100
                },
                {
                    field: "clientName",
                    title: "Client Name",
                    width: 200,
                },
                {
                    field: "amountInvested",
                    title: "Amnt Invested",
                    width: 100,
                    format: "{0:#,##0.#0}",
                },//
               {
                   field: "firstDepositDate ",
                   title: "Start Date",
                   width: 100,
                   format: "{0:dd-MMM-yyyy}",
               },
               {
                   field: "interestAccrued",
                   title: "Int Accrued",
                   width: 100,
                   format: "{0:#,##0.#0}",
               },
               {//
                   field: "amountWithdrawn",
                   title: "Amt With",
                   width: 100,
                   format: "{0:#,##0.#0}",
               },
               {//
                   field: "currentBalance",
                   title: "Cur. Bal.",
                   width: 100,
                   format: "{0:#,##0.#0}",
               },
               {//
                   field: "phoneNumber",
                   title: "Phone",
                   width: 70,
 
               },
               {
                   field: "staffName",
                   title: "Rel. Officer",
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
                fileName: "topTenInvestors.xlsx",
                allPages: true,
            },
            reorderable: true,
            resizable: true,
            pdf: {
                paperKind: "A3",
                fileName: "topTenInvestors.pdf",
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
                ui.renderInvestmentGrid(selectedBranch);
                ui.renderOverMaturedinvestmentGrid(selectedBranch);
                ui.renderTopTenInvestorsGrid(selectedBranch);
            },
        });
    }
    catch (e) { }
}
