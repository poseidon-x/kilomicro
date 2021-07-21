//Declaration of variables 
var authToken = coreERPAPI_Token;

// crud is the route to the api controllers
var openningBalanceBatchApiUrl = coreERPAPI_URL_Root + "/crud/OpeningBalanceBatch";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";


var openningBalanceBatch = {};
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

    $('#openingBalanceGrid').kendoGrid({
        dataSource: {
            transport: {
                read:  {
                    url: openningBalanceBatchApiUrl + '/Get',
                    type: 'Post',
                    contentType: 'application/json',
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    },
                },
                create: function (entries) {
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
                    entries.success();
                },
                parameterMap: function (data) {
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
                    id: 'openningBalanceBatchId',
                    fields: {
                        openningBalanceBatchId: { type: 'number', defaultValue: openningBalanceBatch.openningBalanceBatchId },
                        openningBalanceId: { type: 'number', editable: false, },
                        balanceDate: { type: 'date', editable: false, },
                        locationId: { type: 'number', editable: false, },
                        enteredBy: { type: 'string', editable: false, },
                        created: { type: 'date', editable: false, },
                        approved: { type: 'bool', editable: false, },
                        posted: { type: 'bool', editable: false, },


                    } //fields
                } //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }, //datasource
        columns: [
            { field: 'balanceDate', title: 'Balance Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'locationId', title: 'Location', template: '#= getLocation(locationId) #' },
            { field: 'enteredBy', title: 'Entered By' },
            { field: 'created', title: 'Entered Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'approved', title: 'Approved', template: '<input type="checkbox" disabled="disabled" data-bind="checked: approved" #= approved? checked="checked":"" #/>' },
            { field: 'posted', title: 'Posted', template: '<input type="checkbox" disabled="disabled" data-bind="checked: posted" #= posted? checked="checked":"" #/>' },

            {
                command: [gridEditButton, gridApproveButton, gridPostButton],
                width: 230

            },
            ],

        toolbar: [
            {
                className: 'addBalance',
                text: 'Add New Balance',
            },
        "pdf",
        "excel"
        ],
        excel: {
            fileName: "Opening Balances.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "Opening Balances.pdf"
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

    $(".addBalance").click(function () {
        window.location = "/OpeningBalances/EnterBalance";
    });
}

function getLocation(id) {
    for (i = 0; i < locations.length; i++) {
        if (locations[i].locationId == id) {
            return locations[i].locationName;
        }
    }
}

function getDateOnly(date) {
    var newDate = date.toDateString();
        return newDate;
    
}

var gridEditButton = {

    name: "edit Balance",
    text: "Edit",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if (!data.approved && !data.posted) {
            window.location = "/OpeningBalances/EnterBalance/" + data.openningBalanceBatchId.toString();
        } else {
            errorDialog("Only Unapproved Balance can be Edited", "Edit Balance");
        }
    },

}

var gridApproveButton = {
    name: "Approve Balance",
    text: "Approve",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if (!data.approved) {
            window.location = "/OpeningBalances/ApproveBalances/" + data.openningBalanceBatchId.toString();
        } else {
            errorDialog("Balance Approved Already", "Approve Balance");
        }
    }

}

var gridPostButton = {
    name: "Post Balance",
    text: "Post",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if (data.approved && !data.posted) {
            window.location = "/OpeningBalances/PostBalance/" + data.openningBalanceBatchId.toString();
        } else {
            errorDialog("Balance Posted Already", "Post Balance");
        }
    }
}



