//*******************************************
//***   GROUP ARREARS REPORT JAVASCRIPT                
//***   CREATOR: SAMUEL WENDOLIN KETECHIE    	   
//***   DATE: JULY 1ST, 2019 	
//*******************************************


"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanOutstandingApiUrl = coreERPAPI_URL_Root + "/crud/LoanOutstanding";

var record = 0;

//Function to call load form function
$(function () {
    prepareUi();
    renderGrid();
});


function prepareUi() {
    $('#paymentDate').width('80%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        change: dateChange,
    });
}


var dateChange = function () {
    displayLoadingDialog();
    //var loan = [];
    var datepicked = $("#paymentDate").data("kendoDatePicker").value();
    var realDate = datepicked.toLocaleDateString();
    $('#tabs').kendoTabStrip();
    $('#arrearsGrid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: LoanOutstandingApiUrl + '/GetFullArrears?date=' + realDate,
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
            pageSize: 1000,
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
                    field: "loanGroupName", title: "Group",
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

            //{ title: "#", template: "#= ++record #", width: 50 },
            { field: 'clientName', title: 'Client', width: 130 },
            { field: 'loanNo', width: 130, title: 'Loan No.' },
            
			
            {
                field: 'disbursementDate', width: 130,
                title: 'Disbursement Date', format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(disbursementDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
			{
			    field: 'amountDisbursed',
			    title: 'Loan Amount',
			    width: 130,
			    format: '{0: #,###.#0}',
			    groupFooterTemplate: "Grp Count: #= count#"
			},
			{ field: 'Payable',width: 130, title: 'Payable', format: '{0: #,###.#0}' },
            { field: 'Paid',width: 130, title: 'Paid ', format: '{0: #,###.#0}' },
            {
                field: 'outstanding',
                width: 130,
                title: 'Outstanding', format: '{0: #,###.#0}',
                footerTemplate: "Overall Total Outstanding: #= kendo.toString(sum, '0,000.00') #",
                groupFooterTemplate: "Outstanding :  #= kendo.toString(sum, '0,000.00') #"
            },
			{
			    field: 'LastDueDate ',width: 130, title: 'Last Due Date', format: '{0:dd-MMM-yyyy}',
			    template: "#= kendo.toString(kendo.parseDate(LastDueDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
			},

            {
                field: 'WriteOffAmount',width: 130, title: 'Write Off', format: '{0: #,###.#0}',
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

        pdf: {
            multiPage: true,
            fileName: "OutstandingLoans.pdf",
            allPages: true
        },
        excel: {
            fileName: "OutstandingLoans.xlsx", allPages: true
        },
        dataBinding: function () {
            //record = (this.dataSource.page() -1) * this.dataSource.pageSize();
        },
        filterable:{
            mode:"row"
        },
        pageable: true,
        navigatable: true,
        groupable: false,
        resizable: true,
        scrollable: true,
        selectable: true,
        pageable: {
            pageSize: 500,
            pageSizes: [500, 1000, 1500, 3000, 5000, 7000,10000],
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

//render Grid
function renderGrid() {

    $('#tabs').kendoTabStrip();

}



