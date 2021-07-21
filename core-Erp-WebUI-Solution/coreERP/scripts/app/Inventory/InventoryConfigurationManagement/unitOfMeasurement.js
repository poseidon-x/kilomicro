var authToken = coreERPAPI_Token;
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/UnitOfMeasurement";

var unitOfMeasurement = {};

$(function () {
    $.ajax({
        url: unitOfMeasurementApiUrl + "/Get",
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        unitOfMeasurement = data;
        renderGrid();
    });
});

 function renderGrid() {
        //    // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: unitOfMeasurementApiUrl + "/Get",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: unitOfMeasurementApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: unitOfMeasurementApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: unitOfMeasurementApiUrl + "/Delete",
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
                        id: "unitOfMeasurementID",
                        fields:
                        {
                            unitOfMeasurementID: {
                                validation: {
                                    required: true,
                                },
                                
                            },
                            unitOfMeasurementName: {
                                validation: {
                                    required: true,
                                },
                              
                            },
                            complexDetailUnitOfMeasurementID: {
                                validation: {
                                    required: true,
                                },
                               
                            },
                            numberOfUnits: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            createdBy: {
                                validation: {
                                    required: true,
                                },
                               
                            },
                            creationDate: {
                                validation: {
                                    required: true,
                                },

                            },
                            modifiedBy: {
                                validation: {
                                    required: true,
                                },
                             
                            },
                            modifiedDate: {
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
                    field: "unitOfMeasurementName",
                    title: "Unit Of Measurement Name",
                    width: 150
                },//col1
                {
                    field: "complexDetailUnitOfMeasurementID",
                    title: "complex Detail Unit Of Measurement",
                    width: 75,
                   // editor: complexDetailDropDownEditor
                },//col2
                {
                    field: "numberOfUnits",
                    title: "Number Of Units",
                    width: 50,
                   // editor: numericEditor,

                },//col3
                {
                    field: "createdBy",
                    title: "Created By",
                    width: 50,

                },//col3
                {
                    field: "creationDate",
                    title: "Creation Date",
                    width: 50,
                    editor: dateEditor,

                },//col3
                {
                    field: "modifiedBy",
                    title: "Modified By",
                    width: 50,

                },//col3
                {
                    field: "modifiedDate",
                    title: "Modified Date",
                    width: 50,
                    editor: dateEditor,

                },//col3
                {
                    command: ["edit", "destroy"],
                    width: 120,
                },//col4
            ],//columns
            toolbar: [
                {
                    name: "create",
                    text: "Add New Unit Of Measurement",
                },//tool1
                "pdf",
                "excel"
            ],//toolbar
            excel: {
                fileName: "unitOfMeasurement.xlsx"
            },
            pdf: {
                paperKind: "A3",
                landscape: true,
                fileName: "unitOfMeasurement.pdf"
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
                editWindow.title("Edit unitOfMeasurements Data");
            },
            save: function (e) {
                $('.k-grid-update').css('display', 'none');
            },
            mobile: true,
            reorderable: true,
            resizable: true,
        });//kendogrid
    };// unitOfMeasurementUI.prototype.renderGrid = function() 

    var longTextEditor = function (container, options) {
        $('<input class="input-control" data-bind="value:' + options.field + '" style="width:500px;"/>')
            .appendTo(container);
    }

    var checkBoxTextEditor = function (container, options) {
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

    var numericEditor = function (container, options) {
        $('<input data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                decimals: 6,
                min: 0,
                format: '#,##0.#####0'
            });
    };

    function booleanCheckbox(container, options) {
        $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
    }
