var authToken = coreERPAPI_Token;
var brandApiUrl = coreERPAPI_URL_Root + "/crud/Brand";

var brand = {};

$(function() {
    $.ajax({
        url: brandApiUrl + "/Get",
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function(data) {
    
        brand = data,
        renderGrid();
        });

    });

function renderGrid() {
    $("#setupGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: brandApiUrl + "/Get",
                    type: "POST",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                create: {
                    url: brandApiUrl + "/Post",
                    type: "POST",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                update: {
                    url: brandApiUrl + "/Put",
                    type: "PUT",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                destroy: {
                    url: brandApiUrl + "/Delete",
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
                    id: "brandId",
                    fields:
                    {
                        brandId: {
                            validation: {
                                required: true,
                            },
                        },
                        brandCode: {
                            validation: {
                                required: true,
                            },
                        },
                        brandName: {
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
                field: "brandCode",
                title: "Brand Code",
                width: 75,
                
            }, //col2
            {
                field: "brandName",
                title: "Brand Name",
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
                text: "Add a New Brand",
            }, //tool1
            "pdf",
            "excel"
        ], //toolbar
        excel: {
            fileName: "brand.xlsx"
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
            editWindow.title("Edit Brands Data");
        },
        mobile: true,
        reorderable: true,
        resizable: true,
    }); // brandUI.prototype.renderGrid = function() 
}





  