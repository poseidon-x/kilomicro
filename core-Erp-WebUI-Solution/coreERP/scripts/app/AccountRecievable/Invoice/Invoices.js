//Declaration of variables 
var authToken = coreERPAPI_Token;

// crud is the route to the api controllers
var invoiceApiUrl = coreERPAPI_URL_Root + "/crud/ArInvoice";
var salesOrderApiUrl = coreERPAPI_URL_Root + "/crud/salesOrder";

var arInvoice = {};
var salesOrder = {};



$(function () {
    displayLoadingDialog();
    $.ajax({
        url: salesOrderApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        salesOrder = data;
        renderGrid();
    }).error(function (xhr) {
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
    dismissLoadingDialog();
});


function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: invoiceApiUrl + '/Get',
                    type: 'Post',
                    contentType: 'application/json',
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    },
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
                    id: 'arInvoiceId',
                    fields: {
                        arInvoiceId: { type: 'number', defaultValue: arInvoice.arInvoiceId },
                        invoiceNumber: { type: 'string', editable: false, },
						invoiceDate: { type: 'date', editable: false, },
                        salesOrderId: { type: 'number', editable: false, },
                        customerName: { type: 'string', editable: false, },
                        totalAmount: { type: 'number', format: "{0:c}", editable: false, },
                        paid: { type: 'bool', editable: false, },
						posted: { type: 'bool', editable: false, },
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }, //datasource
        columns: [
            { field: 'invoiceNumber', title: 'Invoice #' },
            { field: 'invoiceDate', title: 'Invoice Date', format: '{0:dd-MMM-yyyy}' },			
            { field: 'salesOrderId', title: 'Sales Order', template: '#= getSalesOrder(salesOrderId) #' },			
            { field: 'customerName', title: 'Customer'},
            { field: 'totalAmount', title: 'Total Amount'},			
            { field: 'paid', title: 'Paid', template: '<input type="checkbox" disabled="disabled" data-bind="checked: paid" #= paid? checked="checked":"" #/>' },
            { field: 'posted', title: 'Posted', template: '<input type="checkbox" disabled="disabled" data-bind="checked: posted" #= posted? checked="checked":"" #/>' },
            {
                command: [postButton],
                width: 120
            },
        ],
        toolbar: [
            {
                className: 'addInvoice',
                text: 'Add New Invoice',
            },
            "pdf",
            "excel"
        ], //toolbar  


        excel: {
            fileName: "Invoices.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "Invoices.pdf"
        },
        filterable: true,
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

    $(".addShrinkage").click(function () {
        window.location = "/ArInvoice/EnterInvoice";
    });
}


var postButton = {
    name: "Post Invoice",
    text: "Post",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if (!data.posted) {
            window.location = "/ArInvoice/PostInvoice/" + data.arInvoiceId.toString();
        } else {
            errorDialog("Invoice posted Already", "Post Invoice");
        }
    }

};

function getSalesOrder(id) {
    for (i = 0; i < salesOrder.length; i++) {
        if (salesOrder[i].salesOrderId == id)
            return salesOrder[i].orderNumber;
    }
}
