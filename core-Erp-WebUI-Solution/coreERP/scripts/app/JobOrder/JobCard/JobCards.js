//**********************************************//
//  			CREDIT MEMO - JAVASCRIPT        //
// 		CREATOR: EMMANUEL OWUSU(MAN)			//
//		WEEK: JUNE(8TH - 12TH), 2015 			//
//**********************************************//

"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var jobCardApiUrl = coreERPAPI_URL_Root + "/crud/jobCard";
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var workOrderApiUrl = coreERPAPI_URL_Root + "/crud/workOrder";

var jobCard = {};
var customers = {};
var workOrders = {};

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

var workOrderAjax = $.ajax({
    url: workOrderApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {

    $.when(customerAjax, workOrderAjax)
        .done(function (dataCustomer, dataWorkOrder) {
			customers = dataCustomer[2].responseJSON;
            workOrders = dataWorkOrder[2].responseJSON;
            
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
                    url: jobCardApiUrl + '/Get',
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
                        jobCardId: { type: 'number', defaultValue: jobCard.jobCardId },
                        jobNumber: { type: 'string', editable: false },						
                        jobDate: { type: 'date', editable: false },
						//customerId: { type: 'date', editable: false },						
                        totalLabour: { type: 'number', editable: false },
                        totalMaterial: { type: 'number', format: "{0:c}", editable: false },
                        approved: { type: 'bool', editable: false },						
                        invoiced: { type: 'bool', editable: false },						
                        signed: { type: 'bool', editable: false },						
                        fulfilled: { type: 'bool', editable: false }						
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        }, //datasource
        columns: [
            { field: 'jobNumber', title: 'Job Number'},
            //{ field: 'customerId', title: 'Customer', template: '#= getCustomer(customerId) #' },
            { field: 'jobDate', title: 'Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'totalLabour', title: 'Total Labour', format: "{0:#,##0.00}"},
            { field: 'totalMaterial', title: 'Total Material', format: "{0:#,##0.00}"},
            { field: 'approved', title: 'Approved', width: 95, template: '<input type="checkbox" disabled="disabled" data-bind="checked: approved" #= approved? checked="checked":"" #/>' },
            { field: 'invoiced', title: 'Invoiced', width: 95,  template: '<input type="checkbox" disabled="disabled" data-bind="checked: invoiced" #= invoiced? checked="checked":"" #/>' },
            { field: 'signed', title: 'Signed', width: 95, template: '<input type="checkbox" disabled="disabled" data-bind="checked: signed" #= signed? checked="checked":"" #/>' },
            { field: 'fulfilled', title: 'Fulfilled', width: 95, template: '<input type="checkbox" disabled="disabled" data-bind="checked: fulfilled" #= fulfilled? checked="checked":"" #/>' },
			{
                command: [approveButton, fulfillButton, invoiceButton, signButton],
                width: 300

            }
		],
        toolbar: [
            {
                className: 'addJobCard',
                text: 'Add New Job Card'
            },
            "pdf",
            "excel"
        ], //toolbar 


        excel: {
            fileName: "JobCard.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "JobCard.pdf"
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

	$(".addJobCard").click(function () {
        window.location = "/JobCard/EnterJobCard";
    });
}
		

function getCustomer(id) {
    for (i = 0; i < customers.length; i++) {
        if (customers[i].customerId == id) {
            return customers[i].customerName;
        }
    }
}


var approveButton = {
    name: "approve",
    text: "Approve",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if(!data.approved ){
            window.location = "/JobCard/ApproveJobCard/" + data.jobCardId.toString();
        } else {
            errorDialog("Job Card Approved Already", "NOTE");
        }
    },
};

var invoiceButton = {
    name: "invoice",
    text: "Invoice",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if(data.approved && !data.invoiced){
            window.location = "/ar/ArInvoice/EnterInvoice/";
        } else {
            errorDialog("Only Unapproved Cards can be Invoice", "NOTE");
        }
    },
};

var fulfillButton = {
    name: "fulfill",
    text: "Fulfill",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if(data.approved && !data.fulfilled){
            window.location = "/JobCard/FulfillJobCard/" + data.jobCardId.toString();
        } else {
            errorDialog("Only Invoice Job Cards be fulfilled", "NOTE");
        }
    },
};


var signButton = {
    name: "sign",
    text: "Sign",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if(!data.approved && !data.posted){
            window.location = "/JobCard/SignJobCard/" + data.jobCardId.toString();
        } else {
            errorDialog("Only fullfilled Job Cards can be sign", "NOTE");
        }
    },
};

