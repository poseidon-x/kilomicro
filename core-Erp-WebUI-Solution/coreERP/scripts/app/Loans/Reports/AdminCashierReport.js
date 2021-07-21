/**
* Detailed Cashier Report Script
*
*Creator: Samuel Wendolin Ketechie
*Date: August 29, 2019
*/

"use strict"

//Declaration of variables to API controllers
//let authToken = coreERPAPI_Token;
let LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
let CashierReportApiUrl = coreERPAPI_URL_Root + "/crud/CashierReport";
let LoanOutstandingApiUrl = coreERPAPI_URL_Root + "/crud/LoanOutstanding";

//Global variables
let cashiers = {};
let fieldAgents = {};
let record = 0;
let repayRecord = 0;
let servRecord = 0;
let susuRecord = 0;
let feeRecord = 0;
let disbRecord = 0;
let disAddRecord = 0;
let disWithRecord = 0;
let savAddRecord = 0;
let savWithRecord = 0;
let arrearsRecord = 0;

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


//Get All the Cashiers
let cashierAjax = $.ajax({
    url: CashierReportApiUrl + '/GetAllCashiers',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Get All the Field Agents
let agentAjax = $.ajax({
    url: CashierReportApiUrl + '/GetAllAgents',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});



//Get All Deposit Clients
function loadForm() {
    $.when(cashierAjax, agentAjax)
        .done(function (cashierData, agentData) {           
            cashiers = cashierData[2].responseJSON;
            fieldAgents = agentData[2].responseJSON;
            prepareUI();
            dismissLoadingDialog();
        });
}

//Function to prepare user interface
function prepareUI() {
    let today = new Date();
    let month = today.getMonth();
    let year = today.getFullYear();
    $('#startDate').width('80%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        value: new Date(year, month, 1)
    });

    $('#endDate').width('80%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        value: new Date(year,month + 1,0)
    });

    $('#txtCashier').width('80%').kendoComboBox({
        dataSource: cashiers,
        dataValueField: 'cashierUserName',
        dataTextField: 'cashierFullName',
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        ignoreCase: true,
       // change: onCashierChange,
        optionLabel: '',
    });

    $('#txtagent').width('80%').kendoComboBox({
        dataSource: fieldAgents,
        dataValueField: 'agentId',
        dataTextField: 'agentNameWithNo',
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        ignoreCase: true,
        // change: onAgentChange,
        optionLabel: '',
    });

    $('#getReport').click(function (event) {
        //displayLoadingDialog();
        showKendoTabs();
        //dismissLoadingDialog();
    });
}

//Function to Show the Tabs

function showKendoTabs() {
    $("#tabs-wrapper").show("slow");
    $("#tabstrip").kendoTabStrip({
        animation: { open: { effects: "fadeIn" } }
    }).data('kendoTabStrip');
    
    //model for getting data
    let endDateData=$("#endDate").data("kendoDatePicker").value();
    let reportModel = {
        startDate: $("#startDate").data("kendoDatePicker").value(),
        endDate: endDateData,
        cashierUsername: $("#txtCashier").data("kendoComboBox").value(),
        fieldAgentName: $("#txtagent").data("kendoComboBox").value()
    };

    //call the rendering grids here
    renderLoanRepaymentGrid(reportModel);
    renderClientServiceChargeGrid(reportModel);    
    //renderSuSuGrid(reportModel);
    renderFeesGrid(reportModel);
    renderDisbursementGrid(reportModel);
    //renderDepositAdditionalGrid(reportModel);
    //renderDepositWithdrawalGrid(reportModel);
    renderSavingAdditionalGrid(reportModel);
    renderSavingWithdrawalGrid(reportModel);    
    renderOutstandingAndSavingGrid(reportModel);
    renderArrearsGrid(reportModel);
    //dismissLoadingDialog();
    
}

//Function to Render Loan Repayment Grid
function renderLoanRepaymentGrid(request) {
    displayLoadingDialog();    
    $('#loanRepayGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetLoanRepayments",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "clientID",
                    fields: {
                        clientID: { editable: false },
                        clientName: { type: "string", editable: false },
                        repaymentDate: { editable: false },
                        loanNo: { type: "string", editable: false },
                        modeOfPaymentName: { editable: false },
                        interestPaid: { type: "number", editable: false },
                        amountPaid: { type: "number", editable: false },
                        feePaid: { type: "number", editable: false },
                        userName: { type: "string", editable: false },
                        principalPaid: { type: "number", editable: false },
                        Branch: { type: "string", editable: false },
                        GroupName: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [

                {
                    field: "Branch",
                    title: "Branch",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "principalPaid", aggregate: "sum" },
                            { field: "interestPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "userName",
                    title: "Cashier",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "principalPaid", aggregate: "sum" },
                            { field: "interestPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "GroupName",
                    title: "GroupName",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "principalPaid", aggregate: "sum" },
                            { field: "interestPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                }
            ],
            aggregate: [
              { field: "amountPaid", aggregate: "sum" },
              { field: "principalPaid", aggregate: "sum" },
              { field: "interestPaid", aggregate: "sum" },
              { field: "loanNo", aggregate: "count" }
            ]
        },

        columns: [
            {
                title: "#",
                template: "#= ++repayRecord #",
                width: 50
            },
            {
                field: 'clientName',
                title: 'Client',
                width: "20%"
            },
            {
                field: 'loanNo',
                title: 'Loan No.',
                width: 80,
                groupFooterTemplate: "Number of Clients: #= count#",
                footerTemplate: "Overall Total Clients: #= count#"
            },
            {
                field: 'repaymentDate ',
                title: 'Date',
                format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(repaymentDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
            
            {
                field: 'amountPaid',
                title: 'Amt Received',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Received: #= kendo.toString(sum, '000.00') #",
                groupFooterTemplate: "Amt Received :  #= kendo.toString(sum, '000.00') #"
            },
            {
                field: 'principalPaid',
                title: 'Principal Paid',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Princ. : #= kendo.toString(sum, '000.00') #",
                groupFooterTemplate: "Total Princ. :  #= kendo.toString(sum, '000.00') #",
                width: 100
                
            },
            {
                field: 'interestPaid',
                title: 'Interest ',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Interest: #= kendo.toString(sum, '000.00') #",
                groupFooterTemplate: "Total Interest :  #= kendo.toString(sum, '000.00') #",
                width: 100
            },
            
            {
                field: 'modeOfPaymentName',
                title: 'Payment Mode',
                width: 100
            }
        ],

        pdf: {
            multiPage: true,
            fileName: "LoanRepaymet_Report.pdf",
            allPages: true
        },
        excel: {
            fileName: "LoanRepaymet_Report.xlsx",
            allPages: true
        },
        dataBinding: function () {
            repayRecord = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable:true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000, 5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }

    });

    dismissLoadingDialog();
}

//Function to Render Client Service Charges Grid
function renderClientServiceChargeGrid(request) {
    displayLoadingDialog();
    $('#serviceChargeGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetClientServiceCharges",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "clientID",
                    fields: {
                        clientID: { editable: false },
                        clientName: { type: "string", editable: false },
                        repaymentDate: { editable: false },
                        loanNo: { type: "string", editable: false },
                        repaymentTypeName: { editable: false },
                        interestPaid: { type: "number", editable: false },
                        amountPaid: { type: "number", editable: false },
                        feePaid: { type: "number", editable: false },
                        userName: { type: "string", editable: false },
                        Branch: { type: "string", editable: false },
                        GroupName: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [

                {
                    field: "Branch",
                    title: "Branch",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "userName",
                    title: "Cashier",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "GroupName",
                    title: "GroupName",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                }
            ],
            aggregate: [
              { field: "amountPaid", aggregate: "sum" },
              { field: "loanNo", aggregate: "count" }
            ]
        },

        columns: [
            {
                title: "#",
                template: "#= ++servRecord #",
                width: 50
            },
            {
                field: 'clientName',
                title: 'Client',
                width: "20%"
            },
            {
                field: 'loanNo',
                title: 'Loan No',
                groupFooterTemplate: "Number of Clients: #= count#",
                footerTemplate: "Overall Total Clients: #= count#"
            },
            {
                field: 'repaymentDate',
                title: 'Payment Date',
                format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(repaymentDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },

            {
                field: 'feePaid',
                title: 'Charge Amount',
                format: '{0: #,###.#0}'
            },
            
            {
                field: 'amountPaid',
                title: 'Amount Paid',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Paid: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Amt Paid :  #= kendo.toString(sum, '0,000.00') #"
            },
            {
                field: 'repaymentTypeName',
                title: 'Charge Type'
            }
        ],

        pdf: {
            multiPage: true,
            fileName: "ClientServiceCharges_Report.pdf",
            allPages: true
        },
        excel: {
            fileName: "ClientServiceCharges_Report.xlsx",
            allPages: true
        },
        dataBinding: function () {
            servRecord = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable: true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000,5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    dismissLoadingDialog();
}

//Function to Render SuSu Grid
function renderSuSuGrid(request) {
    displayLoadingDialog();
    $('#susuGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetSusuReport",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "clientID",
                    fields: {
                        clientID: { editable: false },
                        clientName: { type: "string", editable: false },
                        repaymentDate: { editable: false },
                        loanNo: { type: "string", editable: false },
                        repaymentTypeName: { editable: false },
                        interestPaid: { type: "number", editable: false },
                        amountPaid: { type: "number", editable: false },
                        feePaid: { type: "number", editable: false },
                        userName: { type: "string", editable: false },
                        Branch: { type: "string", editable: false },
                        GroupName: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [

                {
                    field: "Branch",
                    title: "Branch",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "userName",
                    title: "Cashier",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "GroupName",
                    title: "GroupName",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                }
            ],
            aggregate: [
              { field: "amountPaid", aggregate: "sum" },
              { field: "loanNo", aggregate: "count" }
            ]
        },

        columns: [
            {
                title: "#",
                template: "#= ++susuRecord #",
                width: 50
            },
            {
                field: 'clientName',
                title: 'Client',
                width: "20%"
            },
            {
                field: 'loanNo',
                title: 'Loan No',
                groupFooterTemplate: "Number of Clients: #= count#",
                footerTemplate: "Overall Total Clients: #= count#"
            },
            {
                field: 'feePaid',
                title: 'Charge Amount',
                format: '{0: #,###.#0}'
            },
            {
                field: 'repaymentDate',
                title: 'Payment Date',
                format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(repaymentDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
            {
                field: 'repaymentTypeName',
                title: 'Charge Type'
            },
            {
                field: 'amountPaid',
                title: 'Amount Paid',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Paid: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Amt Paid :  #= kendo.toString(sum, '0,000.00') #"
            }
            
        ],

        pdf: {
            multiPage: true,
            fileName: "SuSu_Report.pdf",
            allPages: true
        },
        excel: {
            fileName: "SuSu_Report.xlsx",
            allPages: true
        },
        dataBinding: function () {
            susuRecord = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable: true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000,5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    dismissLoadingDialog();
}

//Function to Render Fees Grid
function renderFeesGrid(request) {
    displayLoadingDialog();
    $('#feesGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetFeesReport",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "clientID",
                    fields: {
                        clientID: { editable: false },
                        clientName: { type: "string", editable: false },
                        repaymentDate: { editable: false },
                        loanNo: { type: "string", editable: false },
                        repaymentTypeName: { editable: false },
                        interestPaid: { type: "number", editable: false },
                        amountPaid: { type: "number", editable: false },
                        feePaid: { type: "number", editable: false },
                        userName: { type: "string", editable: false },
                        Branch: { type: "string", aditable: false },
                        GroupName: { type: "string", aditable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [

                {
                    field: "Branch",
                    title: "Branch",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "userName",
                    title: "Cashier",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "GroupName",
                    title: "GroupName",
                    aggregates:
                        [
                            { field: "amountPaid", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                }
            ],
            aggregate: [
              { field: "amountPaid", aggregate: "sum" },
              { field: "loanNo", aggregate: "count" }
            ]
        },

        columns: [
            {
                title: "#",
                template: "#= ++feeRecord #",
                width: 50
            },
            {
                field: 'clientName',
                title: 'Client',
                width: "20%"
            },
            {
                field: 'loanNo',
                title: 'Loan No',
                groupFooterTemplate: "Number of Clients: #= count#",
                footerTemplate: "Overall Total Clients: #= count#"
            },

            {
                field: 'feePaid',
                title: 'Charge Amount',
                format: '{0: #,###.#0}'
            },
            {
                field: 'repaymentDate',
                title: 'Payment Date',
                format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(repaymentDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
            {
                field: 'repaymentTypeName',
                title: 'Charge Type'
            },
            {
                field: 'amountPaid',
                title: 'Amount Paid',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Paid: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Amt Paid :  #= kendo.toString(sum, '0,000.00') #"
            }
            
        ],

        pdf: {
            multiPage: true,
            fileName: "Fees_Report.pdf",
            allPages: true
        },
        excel: {
            fileName: "Fees_Report.xlsx",
            allPages: true
        },
        dataBinding: function () {
            feeRecord = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable:true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000,5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    dismissLoadingDialog();
}

//Function to Render Disbursement Grid
function renderDisbursementGrid(request) {
    displayLoadingDialog();
    $('#disbursementGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetDisbursementReport",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "clientID",
                    fields: {
                        clientID: { editable: false },
                        clientName: { type: "string", editable: false },
                        disbursementDate: { editable: false },
                        loanAmount: { type: "number", editable: false },
                        loanNo: { type: "string", editable: false },
                        modeOfPaymentName: { editable: false },
                        amountDisbursed: { type: "number", editable: false },                        
                        userName: { type: "string", editable: false },
                        Branch: { type: "string", editable: false },
                        GroupName: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [

                {
                    field: "Branch",
                    title: "Branch",
                    aggregates:
                        [
                            { field: "amountDisbursed", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "userName",
                    title: "Cashier",
                    aggregates:
                        [
                            { field: "amountDisbursed", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "GroupName",
                    title: "GroupName",
                    aggregates:
                        [
                            { field: "amountDisbursed", aggregate: "sum" },
                            { field: "loanNo", aggregate: "count" }
                        ]
                }
                
            ],
            aggregate: [
              { field: "amountDisbursed", aggregate: "sum" },
              { field: "loanNo", aggregate: "count" }
            ]
        },        

        columns: [
            {
                title: "#",
                template: "#= ++disbRecord #",
                width: 50
            },
            {
                field: 'clientName',
                title: 'Client',
                width: "20%"
            },

            {
                field: 'loanNo',
                title: 'Loan No',
                groupFooterTemplate: "Number of Clients: #= count#",
                footerTemplate: "Overall Total Clients: #= count#"
            },

            {
                field: 'disbursementDate',
                title: 'Payment Date',
                format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(disbursementDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },

            {
                field: 'amountDisbursed',
                title: 'Amount Disbursed',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Disbursed: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Disbursed :  #= kendo.toString(sum, '0,000.00') #"
            },

            {
                field: 'modeOfPaymentName',
                title: 'Payment Mode'
            }
        ],

        pdf: {
            multiPage: true,
            fileName: "Disbursement_Report.pdf",
            allPages: true
        },
        excel: {
            fileName: "Disbursement_Report.xlsx",
            allPages: true
        },
        dataBinding: function () {
            disbRecord = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable:true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000,5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    dismissLoadingDialog();
}

//Function to Render Deposit Additional Grid
function renderDepositAdditionalGrid(request) {
    displayLoadingDialog();
    $('#depositAdditionGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetDepositAdditionals",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "clientID",
                    fields: {
                        clientID: { editable: false },
                        clientName: { type: "string", editable: false },
                        depositDate: { editable: false },                        
                        depositAmount: { type: "number", editable: false },
                        depositNo: { type: "string", editable: false },
                        modeOfPaymentName: { editable: false },
                        balance: { type: "number", editable: false },
                        depositTypeName: { editable: false },
                        creator: { type: "string", editable: false },
                        interest: { type: "number", editable: false },
                        interestRate: { type: "number", editable: false },
                        paidInt: { type: "number", editable: false },
                        branchName: { type: "string", editable: false },
                        GroupName: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [

                {
                    field: "branchName",
                    title: "branchName",
                    aggregates:
                        [
                            { field: "depositAmount", aggregate: "sum" },
                            {field: "depositNo", aggregate: "count"}
                        ]
                },
                {
                    field: "creator",
                    title: "Cashier",
                    aggregates:
                        [
                            { field: "depositAmount", aggregate: "sum" },
                            { field: "depositNo", aggregate: "count" }
                        ]
                },
                {
                    field: "GroupName",
                    title: "GroupName",
                    aggregates:
                        [
                            { field: "depositAmount", aggregate: "sum" },
                            { field: "depositNo", aggregate: "count" }
                        ]
                }
            ],
            aggregate: [
              { field: "depositAmount", aggregate: "sum" },
              { field: "depositNo", aggregate: "count" }
            ]
        },

        columns: [
            {
                title: "#",
                template: "#= ++disAddRecord #",
                width: 50
            },
            {
                field: 'clientName',
                title: 'Client',
                width: "20%"
            },
             {
                 field: 'depositNo',
                 title: 'Deposit No',
                 groupFooterTemplate: "Number of Clients: #= count#",
                 footerTemplate: "Overall Total Clients: #= count#"
             },

            {
                field: 'depositDate',
                title: 'Date',
                format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(depositDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
            {
                field: 'depositAmount',
                title: 'Deposit Amount',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Deposit: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Deposit Amt :  #= kendo.toString(sum, '0,000.00') #"
            },

            {
                field: 'balance',
                title: 'Deposit Balance',
                format: '{0: #,###.#0}'
            },
            
            {
                field: 'modeOfPaymentName',
                title: 'Payment Mode'
            }
        ],

        pdf: {
            multiPage: true,
            fileName: "DepositAdditionals_Report.pdf",
            allPages: true
        },
        excel: {
            fileName: "DepositAdditionals_Report.xlsx",
            allPages: true
        },
        dataBinding: function () {
            disAddRecord = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable:true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000,5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    dismissLoadingDialog();
}

//Function to Render Deposit Withdrawal Grid
function renderDepositWithdrawalGrid(request) {
    displayLoadingDialog();
    $('#depositWithdrawGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetDepositWithdrawals",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "clientID",
                    fields: {
                        clientID: { editable: false },
                        clientName: { type: "string", editable: false },
                        withdrawalDate: { editable: false },
                        amountWithdrawn: { type: "number", editable: false },
                        depositNo: { type: "string", editable: false },
                        modeOfPaymentName: { editable: false },
                        principalWithdrawal: { type: "number", editable: false },
                        creator: { type: "string", editable: false },
                        interestExpected: { type: "number", editable: false },
                        interestRate: { type: "number", editable: false },
                        principalBalance: { type: "number", editable: false },
                        branchName: { type: "string", editable: false },
                        GroupName: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [

                {
                    field: "branchName",
                    title: "branchName",
                    aggregates:
                        [
                            { field: "amountWithdrawn", aggregate: "sum" },
                            { field: "depositNo", aggregate: "count" }
                        ]
                },
                {
                    field: "creator",
                    title: "Cashier",
                    aggregates:
                        [
                            { field: "amountWithdrawn", aggregate: "sum" },
                            { field: "depositNo", aggregate: "count" }
                        ]
                },
                {
                    field: "GroupName",
                    title: "GroupName",
                    aggregates:
                        [
                            { field: "amountWithdrawn", aggregate: "sum" },
                            { field: "depositNo", aggregate: "count" }
                        ]
                }
            ],
            aggregate: [
              { field: "amountWithdrawn", aggregate: "sum" },
              { field: "depositNo", aggregate: "count" }
            ]
        },

        columns: [
            {
                title: "#",
                template: "#= ++disWithRecord #",
                width: 50
            },
            {
                field: 'clientName',
                title: 'Client',
                width: "20%"
            },

            {
                field: 'depositNo',
                title: 'Deposit No',
                groupFooterTemplate: "Number of Clients: #= count#",
                footerTemplate: "Overall Total Clients: #= count#"
            },
            
            {
                field: 'withdrawalDate',
                title: 'Date Withdrawn',
                format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(withdrawalDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
            {
                field: 'amountWithdrawn',
                title: 'Amount Withdrawn',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Withdrawn: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Amt Withdrawn :  #= kendo.toString(sum, '0,000.00') #"
            },

            {
                field: 'modeOfPaymentName',
                title: 'Payment Mode'
            }
        ],

        pdf: {
            multiPage: true,
            fileName: "DepositWithdrawals_Report.pdf",
            allPages: true
        },
        excel: {
            fileName: "DepositWithdrawals_Report.xlsx",
            allPages: true
        },
        dataBinding: function () {
            disWithRecord = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable:true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000,5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    dismissLoadingDialog();
}

//Function to Render Savings Additional Grid
function renderSavingAdditionalGrid(request) {
    displayLoadingDialog();
    $('#savingAddGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetSavingAdditionals",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
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
                // the array of repeating data elements 
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "clientID",
                    fields: {
                        clientID: { editable: false },
                        clientName: { type: "string", editable: false },
                        TransDate: { editable: false },
                        savingAmount: { type: "number", editable: false },
                        savingNo: { type: "string", editable: false },
                        modeOfPaymentName: { editable: false },
                        principalBalance: { type: "number", editable: false },
                        SavingTypeName: { editable: false },
                        creator: { type: "string", editable: false },
                        interestBalance: { type: "number", editable: false },
                        interestRate: { type: "number", editable: false },
                        SavingBalance: { type: "number", editable: false },
                        Branch: { type: "string", editable: false },
                        GroupName: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [

                {
                    field: "Branch",
                    title: "Branch",
                    aggregates:
                        [
                            { field: "savingAmount", aggregate: "sum" },
                            { field: "savingNo", aggregate: "count" }

                        ]
                },
                {
                    field: "creator",
                    title: "Cashier",
                    aggregates:
                        [
                            { field: "savingAmount", aggregate: "sum" },
                            { field: "savingNo", aggregate: "count" }

                        ]
                },
                {
                    field: "GroupName",
                    title: "GroupName",
                    aggregates:
                        [
                            { field: "savingAmount", aggregate: "sum" },
                            { field: "savingNo", aggregate: "count" }

                        ]
                }
            ],
            aggregate: [
              { field: "savingAmount", aggregate: "sum" },
              { field: "savingNo", aggregate: "count" }
            ]
        },

        columns: [
            {
                title: "#",
                template: "#= ++savAddRecord #",
                width: 50
            },
            {
                field: 'clientName',
                title: 'Client',
                width: "20%"
            },
             {
                 field: 'savingNo',
                 title: 'Saving No',
                 groupFooterTemplate: "Number of Clients: #= count#",
                 footerTemplate: "Overall Total Clients: #= count#"
             },
             {
                 field: 'TransDate',
                 title: 'Date',
                 format: '{0:dd-MMM-yyyy}',
                 template: "#= kendo.toString(kendo.parseDate(TransDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
             },

            {
                field: 'SavingTypeName',
                title: 'Saving Type'
            },
            {
                field: 'savingAmount',
                title: 'Saving Amount',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Saving: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Saving :  #= kendo.toString(sum, '0,000.00') #"
            },

            {
                field: 'SavingBalance',
                title: 'Saving Balance',
                format: '{0: #,###.#0}'
            },
             {
                 field: 'modeOfPaymentName',
                 title: 'Payment Mode'
             }
        ],

        pdf: {
            multiPage: true,
            fileName: "SavingAdditionals_Report.pdf",
            allPages: true
        },
        excel: {
            fileName: "SavingAdditionals_Report.xlsx",
            allPages: true
        },
        dataBinding: function () {
            savAddRecord = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable:true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000,5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    dismissLoadingDialog();
}

//Function to Render Savings Additional Grid
function renderSavingWithdrawalGrid(request) {
    displayLoadingDialog();
    $('#savingWithGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetSavingWithdrawals",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements 
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "clientID",
                    fields: {
                        clientID: { editable: false },
                        clientName: { type: "string", editable: false },
                        TransDate: { editable: false },
                        amountWithdrawn: { type: "number", editable: false },
                        savingNo: { type: "string", editable: false },
                        modeOfPaymentName: { editable: false },
                        principalBalance: { type: "number", editable: false },
                        SavingTypeName: { editable: false },
                        creator: { type: "string", editable: false },
                        interestBalance: { type: "number", editable: false },
                        interestRate: { type: "number", editable: false },
                        SavingBalance: { type: "number", editable: false },
                        Branch: { type: "string", editable: false },
                        GroupName: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [

                {
                    field: "Branch",
                    title: "Branch",
                    aggregates:
                        [
                            { field: "amountWithdrawn", aggregate: "sum" },
                            { field: "savingNo", aggregate: "count" }
                        ]
                },
                {
                    field: "creator",
                    title: "Cashier",
                    aggregates:
                        [
                            { field: "amountWithdrawn", aggregate: "sum" },
                            { field: "savingNo", aggregate: "count" }
                        ]
                },
                {
                    field: "GroupName",
                    title: "GroupName",
                    aggregates:
                        [
                            { field: "amountWithdrawn", aggregate: "sum" },
                            { field: "savingNo", aggregate: "count" }
                        ]
                }
            ],
            aggregate: [
              { field: "amountWithdrawn", aggregate: "sum" },
              { field: "savingNo", aggregate: "count" }
            ]
        },

        columns: [
            {
                title: "#",
                template: "#= ++savWithRecord #",
                width: 50
            },
            {
                field: 'clientName',
                title: 'Client',
                width: "20%"
            },
            {
                field: 'savingNo',
                title: 'Saving No',
                groupFooterTemplate: "Number of Clients: #= count#",
                footerTemplate: "Overall Total Clients: #= count#"
            },
            {
                field: 'TransDate',
                title: 'Date Withdrawn',
                format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(TransDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },

            {
                field: 'SavingTypeName',
                title: 'Saving Type'
            },
            {
                field: 'amountWithdrawn',
                title: 'Amount Withdrawn',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Withdrawn: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Amt Withdrawn :  #= kendo.toString(sum, '0,000.00') #"
            },
            {
                field: 'SavingBalance',
                title: 'Saving Balance',
                format: '{0: #,###.#0}'
            },
             {
                 field: 'modeOfPaymentName',
                 title: 'Payment Mode'
             }
        ],

        pdf: {
            multiPage: true,
            fileName: "SavingWithdrawals_Report.pdf",
            allPages: true
        },
        excel: {
            fileName: "SavingWithdrawals_Report.xlsx",
            allPages: true
        },
        dataBinding: function () {
            savWithRecord = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable:true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000,5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    dismissLoadingDialog();
}

//Function to Render Outstanding Grid
function renderOutstandingAndSavingGrid(request) {
    displayLoadingDialog();
    $('#outstandingLoanGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        pdf: {
            fileName: "OutstandingLoansAndSavingsReport.pdf",
            allPages: true,
            avoidLinks: true,
            landscape: true,
            repeatHeaders: true,
            filterable: true
        },
        excel: {
            fileName: "OutstandingLoansAndSavings_Report.xlsx",
            allPages: true,
            repeatHeaders: true,
            filterable: true
        },
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + "/GetCombinedSavingAndOutstandingLoans",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data: request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements
                data: "Data",
                // the total count of records in the whole dataset. used for paging.
                total: "Count",
                model: {
                    id: "ClientID",
                    fields: {
                        ClientID: { editable: false },
                        ClientName: { type: "string", editable: false },
                        AmountDisbursed: { type: "number", editable: false },
                        LoanGroupName: { type: "string" },
                        Paid: { type: "number", editable: false },
                        Outstanding: { type: "number", validation: { required: true } },
                        LoanNo: { type: "string", editable: false },
                        DisbursementDate: { editable: false },
                        DisbursedDateToString: { editable: false },
                        SavingNo: { type: "string", editable: false },
                        SavingBalance: { type: "number", editable: false },
                        Officer: { type: "string", editable: false },
                        Branch: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            group: [
                {
                    field: "Branch",
                    title: "Branch",
                    aggregates:
                        [
                            { field: "Outstanding", aggregate: "sum" },
                            { field: "SavingBalance", aggregate: "sum" },
                             { field: "LoanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "Officer",
                    title: "Loan Officer",
                    aggregates:
                        [
                            { field: "Outstanding", aggregate: "sum" },
                            { field: "SavingBalance", aggregate: "sum" },
                             { field: "LoanNo", aggregate: "count" }
                        ]
                },
                {
                    field: "LoanGroupName",
                    title: "Group",
                    aggregates:
                        [
                            { field: "Outstanding", aggregate: "sum" },
                            { field: "SavingBalance", aggregate: "sum" },
                             { field: "LoanNo", aggregate: "count" }
                        ]
                }
            ],
            aggregate: [
              { field: "Outstanding", aggregate: "sum" },
              { field: "SavingBalance", aggregate: "sum" },
              { field: "LoanNo", aggregate: "count" }
            ]
        },

        columns: [

            {
                title: "#",
                template: "#= ++record #",
                width: 50
            },
            {
                field: 'ClientName',
                title: 'Client',
                width: "20%"
            },
            {
                field: 'LoanNo',
                title: 'Loan No.',
                groupFooterTemplate: "Number of Clients: #= count#",
                footerTemplate: "Overall Total Clients: #= count#"
            },
            
            {
                field: 'DisbursedDateToString',
                title: 'Date Disbursed',
                format: '{0: dd-MMM-yyyy}',
                
            },
            {
                field: 'AmountDisbursed',
                title: 'Loan Amt',
                format: '{0: #,###.#0}'
            },
            //{
            //    field: 'Payable',
            //    title: 'Payable ',
            //    format: '{0: #,###.#0}'
            //},
            {
                field: 'Paid',
                title: 'Paid',
                format: '{0: #,###.#0}'
            },

            {
                field: 'Outstanding',
                width: "15%",
                title: 'Outstanding',
                format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Outstanding: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Outstanding :  #= kendo.toString(sum, '0,000.00') #"
            },
           
            {
                field: 'SavingNo',
                title: 'Saving No'
            },
            {
                field: 'SavingBalance',
                title: 'Saving Balance',
                format: '{0: #,###.#0}'
            }
        ],

        
        dataBinding: function () {
             record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable:true,
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000, 5000, 5500,7000,10000],
            previousNext: true,
            buttonCount: 5,
        },
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });

    dismissLoadingDialog();
}

function renderArrearsGrid(request) {
    dismissLoadingDialog();
    $('#arrearsGrid').kendoGrid({
        toolbar: ["pdf", "excel"],
        pdf: {
            fileName: "ArrearsReport.pdf",
            allPages: true,
            avoidLinks: true
        },
        excel: {
            fileName: "ArrearsReport.xlsx",
            allPages: true,
            repeatHeaders: true,
            filterable: true
        },
        dataSource: {
            transport: {
                read: {
                    url: CashierReportApiUrl + '/GetAdminArrears',
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    data:request,
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 2000,
            schema: {
                // the array of repeating data elements (arrears of clients)
                data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                total: "Count",
                model: {
                    id: "loanNo",
                    fields: {
                        loanNo: { type: "string", editable: false },
                        clientName: { type: "string", editable: false },
                        Payable: { editable: false },
                        Paid: { editable: false },
                        LastRepaymentDate: { editable: false },
                        LastDueDate: { editable: false },
                        disbursementDate: { editable: false },
                        amountDisbursed: { editable: false },
                        loanGroupName: { type: "string" },
                        outstanding: { type: "number", validation: { required: true } },
                        WriteOffDate: { editable: false },
                        WriteOffAmount: { type: "number", validation: { required: true } },
                        Officer: { type: "string", editable: false },
                        BranchName: { type: "string", editable: false },
                        DaysDefault: { type: "number", editable: false },
                        ClientPhone: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,

            group: [
                {
                    field: "BranchName",
                    title: "Branch",
                    aggregates: [
                        { field: "outstanding", aggregate: "sum" },
                        { field: "amountDisbursed", aggregate: "count" },
                        { field: "WriteOffAmount", aggregate: "sum" }
                    ]
                },
                {
                    field: "Officer",
                    title: "Loan Officer",
                    aggregates: [
                        { field: "outstanding", aggregate: "sum" },
                        { field: "amountDisbursed", aggregate: "count" },
                        { field: "WriteOffAmount", aggregate: "sum" }
                    ]
                },
                {
                    field: "loanGroupName",
                    title: "Group",
                    aggregates: [
                        { field: "outstanding", aggregate: "sum" },
                        { field: "amountDisbursed", aggregate: "count" },
                        { field: "WriteOffAmount", aggregate: "sum" }
                    ]
                }
            ],
            aggregate: [
              { field: "outstanding", aggregate: "sum" }
            ]
        },

        columns: [

            { title: "#", template: "#= ++arrearsRecord #", width: 50 },
            { field: 'clientName', title: 'Client', width: 130 },
			{
			    field: 'loanNo',
			    title: 'Loan No.',
			    width: 130
			},
            {
                field: 'disbursementDate',
                title: 'Disbursement Date',
                width: 130,
                format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(disbursementDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
			{
			    field: 'amountDisbursed',
			    title: 'Loan Amount',
			    width: 130,
			    format: '{0: #,###.#0}', groupFooterTemplate: "Grp Count: #= count#"
			},
			{
			    field: 'Payable',
			    title: 'Payable',
			    width: 130,
			    format: '{0: #,###.#0}'
			},
            {
                field: 'Paid',
                title: 'Paid ',
                width: 130,
                format: '{0: #,###.#0}'
            },
            {
                field: 'outstanding',
                width: 130,
                title: 'Outstanding', format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Outstanding: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Outstanding :  #= kendo.toString(sum, '0,000.00') #"
            },
			{
			    field: 'LastDueDate ',
			    title: 'Last Due Date',
			    format: '{0:dd-MMM-yyyy}',
			    width: 130,
			    template: "#= kendo.toString(kendo.parseDate(LastDueDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
			},

            {
                field: 'WriteOffAmount',
                title: 'Write Off',
                format: '{0: #,###.#0}',
                width: 130,
                groupFooterTemplate: "Write Off :  #= kendo.toString(sum, '0.00') #"
            },
            {
                field: 'DaysDefault',
                title: 'Days Default',
                width: 80
            },
            {
                field: 'ClientPhone',
                title: 'Phone No.',
                width: 130
            }
        ],

        dataBinding: function () {
            arrearsRecord = (this.dataSource.page() -1) * this.dataSource.pageSize();
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        filterable: {
            mode: "row"
        },
        pageable: {
            pageSize: 2000,
            pageSizes: [2000, 3000,5000,5500,7000,10000],
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
}
