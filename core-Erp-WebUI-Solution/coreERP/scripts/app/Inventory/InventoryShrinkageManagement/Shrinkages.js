//Declaration of variables 
var authToken = coreERPAPI_Token;

// crud is the route to the api controllers
var shrinkageBatchApiUrl = coreERPAPI_URL_Root + "/crud/shrinkageBatch";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";


var shrinkageBatch = {};
var locations = {};

$(function () {
    displayLoadingDialog();
    $.ajax({
        url: locationApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        locations = data;
        renderGrid();
    }).error(function (xhr) {
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });

    dismissLoadingDialog();
});


function renderGrid() {

    $('#ShrinkageGrid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: shrinkageBatchApiUrl + '/Get',
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
                    id: 'shrinkageBatchId',
                    fields: {
                        shrinkageBatchId: { type: 'number', defaultValue: shrinkageBatch.shrinkageBatchId },
                        shrinkageDate: { type: 'date', editable: false },
                        locationId: { type: 'number', editable: false },
                        enteredBy: { type: 'string', editable: false },
                        created: { type: 'date', editable: false },
                        approved: { type: 'bool', editable: false },
                        posted: { type: 'bool', editable: false }
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }, //datasource
        columns: [
            { field: 'shrinkageDate', title: 'Shrinkage Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'locationId', title: 'Location', template: '#= getLocation(locationId) #' },
            { field: 'enteredBy', title: 'Entered By' },
            { field: 'created', title: 'Entry Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'approved', title: 'Approved', template: '<input type="checkbox" disabled="disabled" data-bind="checked: approved" #= approved? checked="checked":"" #/>' },
            { field: 'posted', title: 'Posted', template: '<input type="checkbox" disabled="disabled" data-bind="checked: posted" #= posted? checked="checked":"" #/>' },
            {
                command: [gridEditButton, gridApproveButton, gridPostButton],
                width: 230

            }
        ],
        toolbar: [
            {
                className: 'addShrinkage',
                text: 'Add New Shrinkage',
            },
            "pdf",
            "excel"
        ], //toolbar  


        excel: {
            fileName: "Shrinkages.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "Shrinkages.pdf"
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
        window.location = "/InventoryShrinkage/EnterShrinkage";
    });
}

function getLocation(id) {
    for (i = 0; i < locations.length; i++) {
        if (locations[i].locationId == id) {
            return locations[i].locationName;
        }
    }
}


var gridEditButton = {
    name: "Edit Shrinkage",
    text: "Edit",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if(!data.approved && !data.posted){
            window.location = "/InventoryShrinkage/EnterShrinkage/" + data.shrinkageBatchId.toString();
        } else {
            errorDialog("Only Unapproved Batches can be Edited", "Edit Batch");
        }
    },

};

var gridApproveButton = {
    name: "Approve Shrinkage",
    text: "Approve",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if (!data.approved) {
            window.location = "/InventoryShrinkage/ApproveShrinkage/" + data.shrinkageBatchId.toString();
        } else {
            errorDialog("Only unapproved Batches can be Approved", "Approve Batch");
        }
    },

};

var gridPostButton = {
    name: "Post Shrinkage",
    text: "Post",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if (data.approved && !data.posted) {
            window.location = "/InventoryShrinkage/PostShrinkage/" + data.shrinkageBatchId.toString();
        } else {
            errorDialog("Only Approved but Unposted Batches can be posted", "Post Batch");
        }
    }

};
