var authToken = coreERPAPI_Token;
var inventoryMethodApiUrl = coreERPAPI_URL_Root + "/crud/InventoryMethod";

var inventoryMethod = {};

$(function () {
    $.ajax({
        url: inventoryMethodApiUrl + "/Get",
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        inventoryMethod = data;
        renderGrid();
       
    });
});

function renderGrid() {
    $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: inventoryMethodApiUrl + "/Get",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: inventoryMethodApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: inventoryMethodApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: inventoryMethodApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
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
                        id: "inventoryMethodID",
                        fields:
                        {
                            inventoryMethodID: {
                                validation: {
                                    required: true,
                                },
                            },
                            inventoryMethodName: {
                                validation: {
                                    required: true,
                                },
                            },
                        }//fields
                    }//model
                },//schema
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },//datasource
            columns: [
                {
                    field: "inventoryMethodName",
                    title: "Inventory Method Name",
                    width: 50,

                },//col3
                //{
                //    command: ["edit", "destroy"],
                //    width: 120,
                //},//col4
            ],//columns
            toolbar: [
                //{
                //    name: "create",
                //    text: "Add New inventory Method",
                //},//tool1
                "pdf",
                "excel"
            ],//toolbar
            excel: {
                fileName: "inventoryMethod.xlsx"
            },
            pdf: {
                paperKind: "A3",
                landscape: true,
                fileName: "inventoryMethod.pdf"
            },
            filterable: true,
            sortable: {
                mode: "multiple",
            },
           // editable: "popup",
            pageable: {
                pageSize: 10,
                pageSizes: [10, 25, 50, 100, 1000],
                previousNext: true,
                buttonCount: 5,
            },
            groupable: true,
            selectable: true,
            edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
                editWindow.wrapper.css({ width: 700 });
                editWindow.title("Edit Inventory Methods Data");
            },
            save: function (e) {
                $('.k-grid-update').css('display', 'none');
            },
            mobile: true,
            reorderable: true,
            resizable: true,
        });//kendogrid
    };// inventoryMethodUI.prototype.renderGrid = function() 


