//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var salesOrderApiUrl = coreERPAPI_URL_Root + "/crud/salesOrder";
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var paymentTermApiUrl = coreERPAPI_URL_Root + "/crud/paymentTerm";
var salesTypeApiUrl = coreERPAPI_URL_Root + "/crud/salesType";



var salesOrder = {};
var customers = {};
var currencies = {};
var paymentTerms = {};
var salesTypes = {};


var customerAjax = $.ajax({
    url: customerApiUrl + '/Get',
    type: 'Get',
    contentType: 'application/json',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var currencyAjax = $.ajax({
    url: currencyApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store paymentTerm table ajax call in it
var paymentTermAjax = $.ajax({
    url: paymentTermApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var salesTypeAjax = $.ajax({
    url: salesTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

$(function () {
    displayLoadingDialog();
    loadForm();
});

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {

    $.when(customerAjax, currencyAjax, paymentTermAjax, salesTypeAjax)
        .done(function (dataCustomer, dataCurrency, dataPaymentTerm, dataSalesType) {
            customers = dataCustomer[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
            paymentTerms = dataPaymentTerm[2].responseJSON;
            salesTypes = dataSalesType[2].responseJSON;
            //prepare UI
            prepareUi();
    });
}

//Function to prepare user interface
function prepareUi() {
    $('#tabs').kendoTabStrip();
    renderGrid();
    dismissLoadingDialog();     
}



function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: salesOrderApiUrl + '/Get',
                    type: 'Post',
                    contentType: 'application/json',
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                create: function(entries) {
                    entries.success(entries.data);
                },
                update: function(entries) {
                    entries.success();
                },
                destroy: function(entries) {
                    entries.success();
                },
                parameterMap: function(data) {
                    return JSON.stringify(data);
                },

            }, //transport
            pageSize: 10,
            schema: {
                // the array of repeating data elements (depositType)
                data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                total: "Count",
                model: {
                    id: 'salesOrderId',
                    fields: {
                        salesOrderId: { type: 'number', defaultValue: salesOrder.salesOrderId },
                        orderNumber: { type: 'string', editable: false },
                        customerName: { type: 'string', editable: false },
                        salesDate: { type: 'date', editable: false },						
                        totalAmount: { type: 'number', editable: false },
                        salesTypeId: { type: 'number', editable: false },
                        currencyId: { type: 'number', editable: false },
                        paymentTermId: { type: 'number', editable: false }
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        }, //datasource
        columns: [
            { field: 'orderNumber', title: '#' },
            { field: 'customerName', title: 'Name' },
            { field: 'salesDate', title: 'Sales Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'totalAmount', title: 'Total Amount' },
            { field: 'salesTypeId', title: 'Sales Type', template: '#= getSalesType(salesTypeId) #'  },
			{ field: 'currencyId', title: 'Currency', template: '#= getCurrency(currencyId) #' },
            { field: 'paymentTermId', title: 'Payment Term', template: '#= getPaymentTerm(paymentTermId) #' },
        ],
        toolbar: [
            {
                className: 'addSalesOrder',
                text: 'Place Sales Order'
            },
            "pdf",
            "excel"
        ], //toolbar  


        excel: {
            fileName: "SalesOrder.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "SalesOrder.pdf"
        },
        filterable: true,
        sortable: {
            mode: "multiple"
        },
        editable: "popup",
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5
        },
        groupable: true,
        selectable: true
    });

    $(".addSalesOrder").click(function () {
        window.location = "/SalesOrder/EnterSalesOrder";
    });
}

function getPaymentTerm(id) {
    for (i = 0; i < paymentTerms.length; i++) {
        if (paymentTerms[i].paymentTermID == id) {
            return paymentTerms[i].paymentTerms;
        }
    }
}

function getCurrency(id) {
    for (i = 0; i < currencies.length; i++) {
        if (currencies[i].currency_id == id) {
            return currencies[i].major_name;
        }
    }
}

function getSalesType(id) {
    for (i = 0; i < salesTypes.length; i++) {
        if (salesTypes[i].salesTypeID == id) {
            return salesTypes[i].salesTypeName;
        }
    }
}

