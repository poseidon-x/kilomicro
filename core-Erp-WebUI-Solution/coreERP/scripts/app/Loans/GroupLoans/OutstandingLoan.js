//*******************************************
//***   CLIENTS ARREARS REPORT JAVASCRIPT                
//***   CREATOR: SAMUEL WENDOLIN KETECHIE    	   
//***   DATE: JULY 1, 2019 	
//*******************************************


//"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var loanOutstandingUrl = coreERPAPI_URL_Root + "/crud/LoanOutstanding";

var loanClients = {};
var branches = {};
var loans = {};
var expiryFlag = 1;
var dueRepayments = {};
var record = 0;

//initialize the branch, clients and Checker boxes
var branchAjax = $.ajax({
    url: loanOutstandingUrl + '/GetBranches',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var loanClientAjax = $.ajax({
    url: loanOutstandingUrl + '/GetClientsForOutstanding',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadInit() {
    $.when(branchAjax, loanClientAjax).done(
        function (dataBranch, dataClient) {
            branches = dataBranch[2].responseJSON;
            loanClients = dataClient[2].responseJSON;
            var todayDate = kendo.toString(kendo.parseDate(new Date()), 'dd-MMM-yyyy');
            $('#endDate').width('80%').kendoDatePicker({
                format: '{0:dd-MMM-yyyy}',
                parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
                value: todayDate
            });
            $('#client').width('80%').kendoComboBox({
                dataSource: loanClients,
                dataValueField: 'clientId',
                dataTextField: 'surName',
                filter: "contains",
                highlightFirst: true,
                suggest: true,
                ignoreCase: true,
                optionLabel: '',
            });
            $('#branch').width('80%').kendoComboBox({
                dataSource: branches,
                dataValueField: 'branchID',
                dataTextField: 'branchName',
                filter: "contains",
                highlightFirst: true,
                suggest: true,
                ignoreCase: true,
                optionLabel: '',
            });
            $('#checker').width('80%').kendoComboBox({
                dataValueField: 'value',
                dataTextField: 'text',
                filter: "contains",
                highlightFirst: true,
                suggest: true,
                ignoreCase: true,
                optionLabel: '',
            });
        });
}



//Function to call load form function
$(function () {
    displayLoadingDialog();
    prepareUi();
    dismissLoadingDialog();
});

function prepareUi() {
    loadInit();
    $('#tabs').kendoTabStrip();
}


$("#getReport").click(function () {
    renderGrid();
});
//render Grid
function renderGrid() {
    var clientId = $("#client").data("kendoComboBox").value();
    var branchId = $("#branch").data("kendoComboBox").value();
    var endDate = $("#endDate").data("kendoDatePicker").value();
    var expiryFlag = $("#checker :selected").val();

    var request = {
        ClientId: clientId,
        BranchID: branchId,
        ExpiredFlag: expiryFlag,
        EndDate: endDate
    };

    $('#outstandingGrid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: loanOutstandingUrl + "/GetOutstandingLoans",
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
            pageSize: 1000,
            schema: {
                // the array of repeating data elements (outstanding loans)
                data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                total: "Count",
                model: {
                    id: "LoanId",
                    fields: {
                        LoanId: { editable: false },
                        ClientName: { type: "string", editable: false },
                        LoanAmount: { type: "number", editable: false },
                        LoanGroupName: { type: "string" },
                        Collateral: { type: "number", editable: false },
                        OutstandingAmount: { type: "number", validation: { required: true } },
                        ExpiryDate: { editable: false },
                        WriteOffAmount: { type: "number", validation: { required: true } },
                        DaysDefault: { editable: false },
                        Officer: { type: "string", editable: false },
                        LoanNo: { type: "string", editable: false },
                        DisbursementDate: { editable: false },
                        BranchName: { type: "string", editable: false }
                    }
                }
            },
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            group: [
                {
                    field: "BranchName", title: "Branch",
                    aggregates:
                        [
                            { field: "OutstandingAmount", aggregate: "sum" },
                            { field: "LoanNo", aggregate: "count" },
                            { field: "WriteOffAmount", aggregate: "sum" }
                        ]
                },
                {
                    field: "Officer", title: "Loan Officer",
                    aggregates:
                        [
                            { field: "OutstandingAmount", aggregate: "sum"},
                            { field: "LoanNo", aggregate: "count" },
                            { field: "WriteOffAmount", aggregate: "sum" }
                        ]
                },
                {
                    field: "LoanGroupName", title: "Group",
                    aggregates:
                        [
                            { field: "OutstandingAmount", aggregate: "sum" },
                            { field: "LoanNo", aggregate: "count" },
                            { field: "WriteOffAmount", aggregate: "sum" }
                        ]
                }
            ],
            aggregate: [
              { field: "OutstandingAmount", aggregate: "sum" },
              { field: "LoanNo", aggregate: "count" },
              { field: "WriteOffAmount", aggregate: "sum" },
            ]
        },

        columns: [

            //{ title: "#", template: "#= ++record #", width: 50 },
           
            { field: 'ClientName', title: 'Client', width: "17%" },
            {
                field: 'LoanNo', title: 'Loan No.',
                groupFooterTemplate: "Number of Clients: #= count#",
                footerTemplate: "Overall Total Clients: #= count#"
            },
            {
                field: 'DisbursementDate ', title: 'Disbursed On', format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(DisbursementDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
            { field: 'LoanAmount', title: 'Loan Amt', format: '{0: #,###.#0}' },

            {
                field: 'OutstandingAmount',width: "15%", title: 'Outstanding', format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Outstanding: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Outstanding :  #= kendo.toString(sum, '0,000.00') #"
            },
            { field: 'Collateral', title: 'Collateral ', format: '{0: #,###.#0}' },
            { field: 'DaysDefault', title: 'Days Default' },
            {
                field: 'ExpiryDate ', title: 'Expiry Date', format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(ExpiryDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
            {
                field: 'WriteOffAmount', title: 'Write Off', format: '{0: #,###.#0}',
                groupFooterTemplate: "Write Off :  #= kendo.toString(sum) #",
                footerTemplate: "Overall Total WriteOff: #= kendo.toString(sum, '0.00') #",
            }
        ],

        pdf: {
            multiPage: true,
            fileName: "OutstandingLoans.pdf",
            allPages: true
        },
        excel: {
            fileName: "OutstandingLoans.xlsx", allPages: true
        },
        dataBinding: function () {
            //record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
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
            pageSize: 1000,
            pageSizes: [1000,1500,2000],
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

