//**********************************************//
//  			CREDIT MEMO - JAVASCRIPT        //
// 		CREATOR: EMMANUEL OWUSU(MAN)			//
//		WEEK: JUNE(8TH - 12TH), 2015 			//
//**********************************************//

"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var workOrderApiUrl = coreERPAPI_URL_Root + "/crud/workOrder";

var customers = {};
var workOrder = {};

$(function () {
    displayLoadingDialog();
    loadForm();
});

var customerAjax = $.ajax({
    url: customerApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


function loadForm() {

    $.when(customerAjax)
        .done(function (dataCustomer) {
			customers = dataCustomer[2].responseJSON;
            
            //Prepares UI
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
                    url: workOrderApiUrl + '/Get',
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
                        workOrderId: { type: 'number', defaultValue: workOrder.workOrderId },
                        workOrderNumber: { type: 'string', editable: false },												
                        customerId: { type: 'number', editable: false },
                        workOrderDate: { type: 'date', editable: false },
                        status: { type: 'boll', format: "{0:c}", editable: false },
                        creator: { type: 'string', editable: false }										
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        }, //datasource
        columns: [
            { field: 'workOrderNumber', title: 'Order Number' },
            //{ field: 'customerId', title: 'Customer', template: '#= getCustomer(customerId) #' },
            { field: 'workOrderDate', title: 'Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'status', title: 'Closed', template: '<input type="checkbox" disabled="disabled" data-bind="checked: status" #= status? checked="checked":"" #/>' },
            { field: 'creator', title: 'Entered By' },
		],
        toolbar: [
            {
                className: 'addWorkOrder',
                text: 'Add New Work Order'
            },
            "pdf",
            "excel"
        ], //toolbar 


        excel: {
            fileName: "WorkOrder.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "WorkOrder.pdf"
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

	$(".addWorkOrder").click(function () {
        window.location = "/WorkOrder/EnterWorkOrder";
    });
}

function getCustomer(id) {
    for (i = 0; i < customers.length; i++) {
        if (customers[i].customerId == id) {
            return customers[i].customerName;
        }
    }
}


