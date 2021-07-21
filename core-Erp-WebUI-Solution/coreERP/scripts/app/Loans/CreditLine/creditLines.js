//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var creditLineApiUrl = coreERPAPI_URL_Root + "/crud/CreditLine";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";

//Declaration of variables to store records retrieved from the database
var creditLine = {};
var clients = {};

$(function () {
    displayLoadingDialog();
    loadForm();
});

function loadForm() {
    $.ajax(
        {
            url: clientApiUrl + '/GetCreditLineClients',
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            clients = data;
            prepareUi();
        }).error(function (error) {
            alert(JSON.stringify(error));
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
                    url: creditLineApiUrl + '/Get',
                    type: 'Post',
                    contentType: 'application/json',
                    beforeSend: function (req) {
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
                    id: 'creditLineId',
                    fields: {
                    creditLineId: { type: 'number', defaultValue: 0 },
                        creditLineNumber: { type: 'string', editable: false },
                        clientId: { type: 'number', editable: false },
                        tenure: { type: 'number', editable: false },						
                        amountRequested: { type: 'number', editable: false },
                        isApproved: { type: 'bool', editable: false },
                        amountApproved: { type: 'number', editable: false },
                        amountDisbursed: { type: 'number', editable: false }
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        }, //datasource
        columns: [
            { field: 'creditLineNumber', title: 'Credit Line Number' },
            { field: 'clientId', title: 'Client', template: '#= getClient(clientId) #' },
            { field: 'tenure', title: 'Tenure', template: '#= getTenure(tenure) #' },
            { field: 'amountRequested', title: 'Amount Requested' },
            { field: 'isApproved', title: 'Amount Requested' ,
            template: '<input type="checkbox" disabled="disabled" data-bind="checked: isApproved" #= isApproved? checked="checked":"" #/>'
            },
            { field: 'amountApproved', title: 'Amount Approved' },
			{ field: 'amountDisbursed', title: 'Amount Disbursed' },
            {
                command: [ gridApproveButton ]
            }
        ],
        toolbar: [
            {
                className: 'addCreditLine',
                text: 'Create New Credit Line'
            },
            "pdf",
            "excel"
        ], //toolbar  


        excel: {
            fileName: "CreditLine.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "CreditLine.pdf"
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

    $(".addCreditLine").click(function () {
        window.location = "/LoanSetup/EnterCreditLine";
    });
}

function getClient(id) {
    for (i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            return clients[i].clientName;
        }
    }
}

function getTenure(tenure) {

    if (tenure == 1) {
        return tenure + " Month";
    } else if (tenure > 1 && tenure%12 != 0) {
       return tenure + " Months";
    } else if (tenure % 12 == 0 && (tenure / 12) == 1) {
        return (tenure/12) + " Year";
    } else if (tenure % 12 == 0) {
        return (tenure / 12) + " Years";
    }
}

var gridApproveButton = {
    name: "ApproveCreditLine",
    text: "Approve",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        if (!data.isApproved) {
            window.location = "/CreditLine/ApproveCreditLine/" + data.creditLineId.toString();
        } else {
            errorDialog("Credit Line Approved Already", "Approve Credit Line");
        }
    }

};

