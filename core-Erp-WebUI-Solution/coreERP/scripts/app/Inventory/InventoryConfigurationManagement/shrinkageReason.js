var authToken = coreERPAPI_Token;
var shrinkageReasonApiUrl = coreERPAPI_URL_Root + "/crud/ShrinkageReason";

var shrinkageReason = {};

$(function() {
    $.ajax({
        url: shrinkageReasonApiUrl + "/Get",
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function(data) {
        shrinkageReason = data,
        renderGrid();
        });

    });

function renderGrid() {
    $("#setupGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: shrinkageReasonApiUrl + "/Get",
                    type: "POST",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                create: {
                    url: shrinkageReasonApiUrl + "/Post",
                    type: "POST",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                update: {
                    url: shrinkageReasonApiUrl + "/Put",
                    type: "PUT",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                destroy: {
                    url: shrinkageReasonApiUrl + "/Delete",
                    type: "DELETE",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                }
            },
            //transport

            pageSize: 10,
            schema: {
                // the array of repeating data elements (depositType)
                data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                total: "Count",
                model: {
                    id: "shrinkageReasonId",
                    fields:
                    {
                        shrinkageReasonId: {
                            validation: {
                                required: true,
                            },
                        },
                        shrinkageReasonCode: {
                            validation: {
                                required: true,
                            },
                        },
                        shrinkageReasonName: {
                            validation: {
                                required: true,
                            },
                        },
                    } //fields
                } //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }, //datasource
        columns: [
            {
                field: "shrinkageReasonCode",
                title: "shrinkage Reason Code",
                width: 75,
                
            }, //col2
            {
                field: "shrinkageReasonName",
                title: "shrinkage Reason Name",
                width: 50,

            }, //col3
            {
                command: ["edit", "destroy"],
                width: 120,
            }, //col4
        ], //columns
        toolbar: [
            {
                name: "create",
                text: "Add New Shrinkage Reason",
            }, //tool1
            "pdf",
            "excel"
        ], //toolbar
        excel: {
            fileName: "shrinkageReason.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "brand.pdf"
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
        edit: function(e) {
            var editWindow = this.editable.element.data("kendoWindow");
            editWindow.wrapper.css({ width: 700 });
            editWindow.title("Edit Shrinkage Reason Data");
        },
        save: function(e) {
            $('.k-grid-update').css('display', 'none');
           // saveBrand();
        },

        mobile: true,
        reorderable: true,
        resizable: true,
    }); // brandUI.prototype.renderGrid = function() 
}





  