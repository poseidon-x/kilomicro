var authToken = coreERPAPI_Token;
var locationTypeApiUrl = coreERPAPI_URL_Root + "/crud/LocationType";

var locationType = {};

$(function () {
    renderGrid();
});

function renderGrid() {
    //    // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
    $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                read:  {
                    url: locationTypeApiUrl + '/Get',
                    type: 'Post',
                    //contentType: 'application/json',
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                create: function(entries) {
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
                    entries.success();
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
                        id: "locationTypeID",
                        fields:{ 
							locationTypeID: {validation: {  required: true } },
							locationTypeCode: { validation: { required: true } },
                            locationTypeName: { validation: { required: true}  },
                        }//fields
                    }//model
                },//schema
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },//datasource
		editable: 'inline',			
            columns: [
                { field: "locationTypeCode", title: "locationType Code",  width: 75, },//col2
                { field: "locationTypeName", title: "locationType Name", width: 50 },//col3
                { command: ["edit", "destroy"], width: 120 },//col4
            ],//columns
            toolbar: [
                {
                    name: "create",
                    text: "Add New locationType",
                },//tool1
 
            ],//toolbar
            excel: {
                fileName: "locationType.xlsx"
            },
            pdf: {
                paperKind: "A3",
                landscape: true,
                fileName: "locationType.pdf"
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
                editWindow.title("Edit locationTypes Data");
            },
            //save: function (e) {
            //    $('.k-grid-update').css('display', 'none');
            //},
            mobile: true,
            reorderable: true,
            resizable: true,
        });//kendogrid
    };// locationTypeUI.prototype.renderGrid = function() 


   

