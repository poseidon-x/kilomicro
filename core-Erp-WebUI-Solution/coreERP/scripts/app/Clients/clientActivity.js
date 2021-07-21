var activityApiUrl = coreERPAPI_URL_Root + "/crud/ClientActivity"; 
var clientApiUrl = coreERPAPI_URL_Root + "/crud/Client";
var loanApiUrl = coreERPAPI_URL_Root + "/crud/Loan";
var clientActivityTypes = [];
var clients = [];
var loans = [];
var loanId = null;

$(function () {
    $.ajax({
        url: activityApiUrl + "/GetTypes",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        clientActivityTypes = data;
        $.ajax({
            url: clientApiUrl + "/Get",
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            clients = data;

            $('#client')
                .width(250)
                .kendoComboBox({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: clients,
                    dataValueField: "ID",
                    dataTextField: "Description",
                    change: function (e) {
                        var clientId = $('#client').val();
                        $.ajax({
                            url: loanApiUrl + "/Get/" + clientId,
                            beforeSend: function (req) {
                                req.setRequestHeader('Authorization', "coreBearer " + authToken);
                            }
                        }).done(function (data) {
                            loans = data;

                            $('#loan')
                                .width(250)
                                .kendoComboBox({
                                    optionLabel: " ",
                                    autoBind: false,
                                    dataSource: loans,
                                    dataValueField: "ID",
                                    dataTextField: "Description",
                                    change: function (e) {
                                        loanId = $('#loan').val();
                                    }
                                });
                        });
                        renderActivity(clientId);
                    }
                });
        });
    });
});

function renderActivity(clientId) {
    $("#grid").replaceWith('<div id="grid"></div>');
    // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
    $("#grid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: activityApiUrl + "/Get/" + clientId,
                    type: "POST",
                    contentType: "application/json",
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                create:{
                    url: activityApiUrl + "/Post",
                    type: "POST",
                    contentType: "application/json",
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                update: {
                    url: activityApiUrl + "/Put",
                    type: "PUT",
                    contentType: "application/json",
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                destroy: {
                    url: activityApiUrl + "/Delete",
                    type: "DELETE",
                    contentType: "application/json",
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data, type) {
                    return kendo.stringify(data);
                },
            },
            pageSize: 20,
            schema: {
                // the array of repeating data elements (employees)
                data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                total: "Count",
                model: {
                    id: "clientActivityLogID",
                    fields:
                       {
                           clientActivityLogID: {
                               validation: {
                                   //required: true,
                               },
                               type: "number",
                               editable:false
                               //defaultValue: 0,
                           },
                           clientID: {
                               validation: {
                                   required: true,
                               },
                               defaultValue: clientId,
                           },
                           clientActivityTypeID: {
                               validation: {
                                   required: true,
                               },
                               type: "number",
                               editable: true
                           },
                           activityDate: {
                               validation: {
                                   required: true,
                               },
                               type: "date",
                               defaultValue: new Date(),
                           },
                           activityNotes: {
                               validation: {
                                   required: true,
                               }, 
                           },
                           nextActionDate: {
                               validation: {
                                   required: false,
                               },
                               type: "date",
                           },
                           nextAction: {
                               validation: {
                                   required: false,
                               },
                           },
                           loanID: {
                               validation: {
                                   required: true,
                               },
                               defaultvalue: loanId, 
                           },
                           responsibleStaffID: {
                               validation: {
                                   required: true,
                               }, 
                           },
                           completed: {
                               validation: {
                                   required: true,
                                   type: "bool",
                               },
                           }, 
                       }
                }
            },
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        },
        columns: [
            {
                field: "clientActivityTypeID",
                title: "Activity Type",
                width: 150,
                editor: clientActivityTypeIDEditor,
                template: '#= getClientActivityTypeName(clientActivityTypeID) #',
            },
            {
                field: "activityNotes",
                title: "Activity Notes",
                width: 200,
            },
            {
                field: "activityDate",
                title: "Date",
                width: 100,
                editor: dateEditor,
                format: "{0: dd-MMM-yyyy}",
            },
            {
                field: "nextAction",
                title: "Next Action",
                width: 200,
            },
            {
                field: "nextActionDate",
                title: "Next Date",
                width: 100,
                editor: dateEditor,
                format: "{0: dd-MMM-yyyy}",
            },
            {
                field: "completed",
                title: "Completed?",
                width: 100,
                editor: checkBoxEditor,
                template: '<input type="checkbox" disabled="disabled" data-bind="checked: enabled" #= completed? checked="checked":"" #/>'
            },
            {
                command: ["edit", "destroy"],
                width: 120,
            },
        ],
        filterable: true,
        sortable: {
            mode: "multiple",
        },
        pageable: {
            pageSize: 20,
            pageSizes: [5, 10, 25, 50, 100, 1000, ],
            previousNext: true,
            buttonCount: 5,
        },
        groupable: true,
        selectable: true,
        toolbar: [
            "create",
            "excel",
            "pdf",
        ],
        excel: {
            fileName: getClientName(clientId)+ ' Activities.xlsx',
            allPages: true,
        },
        pdf: {
            fileName: getClientName(clientId) + ' Activities.pdf',
            allPages: true,
            pageSize: "A4",
            landscape: true,
            subject: $('#client').text(),
            title: $('#client').text(),
        },
        edit: function (e) {
            var editWindow = this.editable.element.data("kendoWindow");
            editWindow.wrapper.css({ width: 700 });
            editWindow.title("Edit Client Activity Log");
        },
        editable: 'popup',
        save: function (e) {
            $('.k-grid-update').css('display', 'none');
        },
        reorderable: true,
    });
};


var checkBoxEditor = function (container, options) {
    $('<input type="checkbox" class="input-control" data-bind="checked:' + options.field + '" />')
        .appendTo(container);
}

var dateEditor = function (container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDatePicker({
            format: "dd-MMM-yyyy"
        });
}

function getClientActivityTypeName(clientActivityTypeID) {
    var clientActivityTypeName = "";

    try {
        for (var i = 0; i < clientActivityTypes.length; i++) {
            if (clientActivityTypes[i].clientActivityTypeID === clientActivityTypeID) {
                clientActivityTypeName = clientActivityTypes[i].clientActivityTypeName;
            }
        }
    }
    catch (e) { }

    return clientActivityTypeName;
}

function clientActivityTypeIDEditor(container, options) {
    try {
        $('<input required data-text-field="clientActivityTypeName" data-value-field="clientActivityTypeID" data-bind="value:' + options.field + '"/>')
            .width(300)
            .appendTo(container)
            .kendoDropDownList({
                optionLabel: " ",
                autoBind: false,
                dataSource: clientActivityTypes,
                dataValueField: 'clientActivityTypeID',
                dataTextField: 'clientActivityTypeName',
                filter: "contains",
                highlightFirst: true,
                suggest: true,
                ignoreCase: true
            });
    }
    catch (e) { }
}

function getClientName(clientId) {
    var clientName = "";

    try {
        for (var i = 0; i < clients.length; i++) {
            if (clients[i].ID === clientId) {
                clientName = clients[i].Description;
            }
        }
    }
    catch (e) { }

    return clientName;
}