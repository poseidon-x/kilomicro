var authToken = coreERPAPI_Token;
var inventoryLocationApiUrl = coreERPAPI_URL_Root + "/crud/InventoryLocation";

var inventoryLocation = {};

$(function () {
    $.ajax({
        url: inventoryLocationApiUrl + "/Get",
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        inventoryLocation = data;
        renderGrid();

    });
});

function renderGrid() {
    //    // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
    $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: inventoryLocationApiUrl + "/Get",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: inventoryLocationApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: inventoryLocationApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: inventoryLocationApiUrl + "/Delete",
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
                        id: "locationID",
                        fields:
                        {
                            locationID: {
                                validation: {
                                    required: true,
                                },
                                //defaultValue: 0,
                            },
                            locationCode: {
                                validation: {
                                    required: true,
                                },
                               // defaultValue: 0,
                            },
                            LocationName: {
                                validation: {
                                    required: true,
                                },
                             //    defaultValue: 0,
                            },
                            locationTypeID: {
                                validation: {
                                    required: true,
                                },
                             //   defaultValue: 0,
                            },
                            physicalAddress: {
                                validation: {
                                    required: true,
                                },
                             //   defaultValue: 0,
                            },
                            cityID: {
                                validation: {
                                    required: true,
                                },
                            //    defaultValue: 0,
                            },
                            isActive: {
                                validation: {
                                    required: true,
                                },
                             //   defaultValue: 0,
                            },
                            longitude: {
                                validation: {
                                    required: true,
                                },
                              //  defaultValue: 0,
                            },
                            lattitude: {
                                validation: {
                                    required: true,
                                },
                             //   defaultValue: 0,
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
                    field: "locationCode",
                    title: "Inventory Location Code",
                    width: 150,
                   
                },//col1
                {
                    field: "locationName",
                    title: "Inventory Location Name",
                    width: 75,
                  
                },//col2
                {
                    field: "locationTypeID",
                    title: "Inventory Location Type",
                    width: 50,


                },//col3
                {
                    field: "physicalAddress",
                    title: "Physical Address",
                    width: 50,

                },//col4
                                {
                    field: "cityID",
                    title: "City",
                    width: 50,

                },//col5
                {
                    field: "isActive",
                    title: "Is Active?",
                    width: 50,
                    editor: checkBoxTextEditor,

                },//col6
                {
                    field: "longitude",
                    title: "Longitude",
                    width: 50,

                },//col7
                {
                    field: "lattitude",
                    title: "Lattitude",
                    width: 50,

                },//col8
                {
                    command: ["edit", "destroy"],
                    width: 120,
                },//col last
            ],//columns
            toolbar: [
                {
                    name: "create",
                    text: "Add New inventory Location",
                },//tool1
                "pdf",
                "excel"
            ],//toolbar
            excel: {
                fileName: "inventoryLocation.xlsx"
            },
            pdf: {
                paperKind: "A3",
                landscape: true,
                fileName: "inventoryLocation.pdf"
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
            edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
                editWindow.wrapper.css({ width: 700 });
                editWindow.title("Edit inventory Locations Data");
            },
            save: function (e) {
                $('.k-grid-update').css('display', 'none');
            },
            mobile: true,
            reorderable: true,
            resizable: true,
        });//kendogrid
    };// inventoryLocationUI.prototype.renderGrid = function() 

   
    var checkBoxTextEditor = function (container, options) {
        $('<input type="checkbox" class="input-control" data-bind="checked:' + options.field + '" />')
            .appendTo(container);
    }

   