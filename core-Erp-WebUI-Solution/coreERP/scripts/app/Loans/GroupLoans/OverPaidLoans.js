"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanOutstandingApiUrl = coreERPAPI_URL_Root + "/crud/LoanOutstanding";


var globalDatePicked = {};
var datepicked = {};
var theDate = {};
var record = 0;

//Function to call load form function
$(function () {
    prepareUi();
    renderGrid();
});


function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}

function prepareUi() {
    $('#paymentDate').width('80%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        change: dateChange,

    });
}


var dateChange = function () {
    displayLoadingDialog();

    var loan = [];
    datepicked = $("#paymentDate").data("kendoDatePicker").value();
    console.log("Changed", datepicked);
    globalDatePicked.date = datepicked;
    theDate = formatDate(globalDatePicked.date)

    $('#tabs').kendoTabStrip();
    dismissLoadingDialog();
    $('#overpaymentGrid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: LoanOutstandingApiUrl + '/GetOverPayments?date=' + theDate,
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
            pageSize: 10,
            schema: {
                // the array of repeating data elements (employees)
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
                        BranchName: { type: "string", editable: false }
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
                    aggregates:
                        [
                            { field: "outstanding", aggregate: "sum" },
                            { field: "amountDisbursed", aggregate: "count" }
                        ]
                },
                {
                    field: "loanGroupName",
                    title: "Group",
                    aggregates:
                        [
                            { field: "outstanding", aggregate: "sum" },
                            { field: "amountDisbursed", aggregate: "count" }
                        ]
                }
            ],
            aggregate: [
              { field: "outstanding", aggregate: "sum" }
            ]
        },
        columns: [

            { title: "#", template: "#= ++record #", width: 50 },
			{ field: 'clientName', title: 'Client', width: "17%" },
			{ field: 'loanNo', title: 'Loan No.' },
            {
                field: 'disbursementDate', title: 'Disb. Date', format: '{0:dd-MMM-yyyy}',
                template: "#= kendo.toString(kendo.parseDate(disbursementDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
            },
			{ field: 'amountDisbursed', title: 'Loan Amount', format: '{0: #,###.#0}', groupFooterTemplate: "Grp Count: #= count#" },
			{ field: 'Payable', title: 'Payable', format: '{0: #,###.#0}' },
            { field: 'Paid', title: 'Paid ', format: '{0: #,###.#0}' },
            {
                field: 'outstanding', title: 'Amt OverPaid', format: '{0: #,###.#0}', footerTemplate: "Overall Total OverPaid: #: sum #",
                groupFooterTemplate: "Group Total OverPaid :  #: sum #"
            },
			{
			    field: 'LastDueDate ', title: 'Last Due Date', format: '{0:dd-MMM-yyyy}',
			    template: "#= kendo.toString(kendo.parseDate(LastDueDate, 'yyyy-MM-ddTHH:mm:ss'), 'dd MMM yyyy') #"
			},

        ],
        //toolbar: [
        //   { name: "pdf", text: "Export to PDF" },
        //   { name: "excel", text: "Export to Excel" },

        //],
        pdf: {
            fileName: "OverPaidLoans.pdf",
            multiPage: true,
            allPages: true
        },
        excel: {
            fileName: "OverPaidLoans.xlsx",
            allPages: true
        },
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        navigatable: true,
        groupable: false,
        pageable: {
            pageSize: 50,
            pageSizes: [50, 150, 300, 600, 1000],
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

console.log(globalDatePicked);

//render Grid
function renderGrid() {



    $('#tabs').kendoTabStrip();



}



