//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var inventoryTransferApiUrl = coreERPAPI_URL_Root + "/crud/inventoryTransfer";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/inventoryItem";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";

//Declaration of variables to store records retrieved from the database
var inventoryTransfer = {};
var locations = {};
var inventoryItems = {};
var unitOfMeasurements = {};
var accts = {};



$(function () {
    displayLoadingDialog();
    loadForm();

}); function loadForm() {
    //Declare a variable and store location table ajax call in it
    var locationAjax = $.ajax({
        url: locationApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store inventoryItem table ajax call in it
    var inventoryItemAjax = $.ajax({
        url: inventoryItemApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });



    var accountAjax = $.ajax({
        url: acctsApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    $.when(locationAjax, inventoryItemAjax, accountAjax)
        .done(function (dataLocation, dataInventoryItem, dataAccount) {
            locations = dataLocation[2].responseJSON;
            inventoryItems = dataInventoryItem[2].responseJSON;
            accts = dataAccount[2].responseJSON;
            //Prepares UI
            renderGrid();
            dismissLoadingDialog();

        });
}



function renderGrid() {
    $('#TransferGrid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: inventoryTransferApiUrl + '/Get',
                    type: 'Post',
                    contentType: 'application/json',
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
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
                    id: 'inventoryTransferId',
                    fields: {
                        inventoryTransferId: { type: 'number', defaultValue: inventoryTransfer.inventoryTransferId },
                        fromLocationId: { type: 'number', editable: false, },
                        toLocationId: { type: 'number', editable: false, },
                        requisitionDate: { type: 'date', editable: false, },
                        enteredBy: { type: 'string', editable: false, },
                        created: { type: 'date', editable: false, },
                        approved: { type: 'bool', editable: false, },
                        posted: { type: 'bool', editable: false, },
                        delivered: { type: 'bool', editable: false, },
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }, //datasource
        columns: [
            { field: 'fromLocationId', title: 'From Location', template: '#= getLocation(fromLocationId) #' },
            { field: 'toLocationId', title: 'To Location', template: '#= getLocation(toLocationId) #' },
            { field: 'requisitionDate', title: 'Requisition Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'enteredBy', title: 'Entered By' },
            { field: 'created', title: 'Entry Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'approved', title: 'Approved', template: '<input type="checkbox" disabled="disabled" data-bind="checked: approved" #= approved? checked="checked":"" #/>' },
            { field: 'posted', title: 'Posted', template: '<input type="checkbox" disabled="disabled" data-bind="checked: posted" #= posted? checked="checked":"" #/>' },
            { field: 'delivered', title: 'Delivered', template: '<input type="checkbox" disabled="disabled" data-bind="checked: delivered" #= delivered? checked="checked":"" #/>' },
            {
                command: [gridEditButton, gridApproveButton, gridPostButton, gridDeliveredButton],
                width: 300

            },
        ],
        toolbar: [
            {
                className: 'addTransfer',
                text: 'Add New Transfer',
            },
            "pdf",
            "excel"
        ], //toolbar  


        excel: {
            fileName: "Transfers.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "Transfers.pdf"
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

        detailInit: detInit,

    });



    $(".addTransfer").click(function () {
        window.location = "/InventoryTransfer/EnterTransfer";
    });
}

function detInit(e) {
    $("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: inventoryTransferApiUrl + '/GetDetail/' + e.data.inventoryTransferId,
                     type: 'Get',
                     contentType: 'application/json',
                     beforeSend: function (req) {
                       req.setRequestHeader('Authorization', "coreBearer " + authToken);
                   }
                },
            },
            pageSize: 10,
            schema: {
                model: {
                    id: 'inventoryTransferId',
                    fields: {
                        inventoryTransferId: { type: 'number', defaultValue: e.data.inventoryTransferId },
                        inventoryTransferDetailId: { type: 'number', editable: false, },
                        inventoryItemId: { type: 'number', validation: { required: true }, },
                        quantityTransferred: { type: 'number', validation: { required: true }, },
                        fromAccountId: { type: 'number', validation: { required: true }, },
                        toAccountId: { type: 'number', validation: { required: true }, },
                        creator: { type: 'string', validation: { required: true }, },
                        created: { type: 'date', validation: { required: true }, },
                    }, //fields
                }, //model
            }, //schema
        },
        scrollable: false,
        sortable: true,
        pageable: true,
        columns: [
            { field: 'inventoryItemId', title: 'Inventory Item', template: '#= getInventoryItem(inventoryItemId) #' },
            { field: 'quantityTransferred', title: 'Quantity Transferred'},
            { field: 'fromAccountId', title: 'From GL Account', template: '#= getAccount(fromAccountId) #' },
            { field: 'toAccountId', title: 'To GL Account', template: '#= getAccount(toAccountId) #' },
            { field: 'creator', title: 'Entered By' },
            { field: 'created', title: 'Entry Date', format: '{0:dd-MMM-yyyy}' },
        ],
        detailInit: detLineInit,

    });
};

function detLineInit(e) {
    $("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: inventoryTransferApiUrl + '/GetDetailLine/' + e.data.inventoryTransferDetailId,
                    type: 'Get',
                    contentType: 'application/json',
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
            },
            pageSize: 10,
            schema: {
                model: {
                    id: 'inventoryTransferDetailId',
                    fields: {
                        inventoryTransferDetailId: { type: 'number', defaultValue: e.data.inventoryTransferDetailId },
                        inventoryTransferDetailLineId: { type: 'number', editable: false, },
                        quantityTransferred: { type: 'number', validation: { required: true }, },
                        batchNumber: { type: 'string', validation: { required: true }, },
                        mfgDate: { type: 'date', validation: { required: true }, },
                        expiryDate: { type: 'date', validation: { required: true }, },
                        unitCost: { type: 'number', validation: { required: true }, },     
                        creator: { type: 'string', validation: { required: true }, },
                        created: { type: 'date', validation: { required: true }, },
                    }, //fields
                }, //model
            }, //schema
        },
        scrollable: false,
        sortable: true,
        pageable: true,
        columns: [
            { field: 'quantityTransferred', title: 'Quantity Transferred' },
            { field: 'batchNumber', title: 'Batch Number'},
            { field: 'mfgDate', title: 'Manufacturing Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'expiryDate', title: 'Expiry Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'unitCost', title: 'Unit Cost' },
            { field: 'creator', title: 'Entered By' },
            { field: 'created', title: 'Entry Date', format: '{0:dd-MMM-yyyy}' },
        ],
    });
};

function getLocation(id) {
    for (i = 0; i < locations.length; i++) {
        if (locations[i].locationId == id) {
            return locations[i].locationName;
        }
    }
}



//retrieve inventory Item value from the Grid pop and display on the Grid
function getInventoryItem(id) {
    for (i = 0; i < inventoryItems.length; i++) {
        if (inventoryItems[i].inventoryItemId == id)
            return inventoryItems[i].inventoryItemName;

    }
}

function getAccount(id) {
    for (i = 0; i < accts.length; i++) {
        if (accts[i].acct_id == id) {
            return accts[i].acc_name;
        }
    }
}



var gridEditButton = {
    name: "EditTransfer",
    text: "Edit",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/InventoryTransfer/EnterTransfer/" + data.inventoryTransferId.toString();
    },

};

var gridApproveButton = {
    name: "ApproveTransfer",
    text: "Approve",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/InventoryTransfer/ApproveTransfer/" + data.inventoryTransferId.toString();
    },

};

var gridPostButton = {
    name: "PostTransfer",
    text: "Post",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/InventoryTransfer/PostTransfer/" + data.inventoryTransferId.toString();
    },

};


var gridDeliveredButton = {
    name: "DeliverTransfer",
    text: "Deliver",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/InventoryTransfer/DeliverTransfer/" + data.inventoryTransferId.toString();
    },

};
