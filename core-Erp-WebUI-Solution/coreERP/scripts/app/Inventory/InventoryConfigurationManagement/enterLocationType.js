var authToken = coreERPAPI_Token;
var locationTypeApiUrl = coreERPAPI_URL_Root + "/crud/LocationType";

var locationType = {};

$(function() {
    renderGrid();

    $('#save').click(function(event) {

        var grid = $("#setupGrid").data().kendoGrid.dataSource.view();

        if (grid.length > 0 ) {
            displayLoadingDialog();
            saveLocationTypeGridData(grid);

            //Retrieve & save Grid data
            saveLocationType();
        } else {
            smallerWarningDialog('One or More Details grid is/are empty', 'NOTE');
        }
    });
});

function renderGrid() {
    //    // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
    $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                read: function (entries) {
                    entries.success(locationType);
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
		    editable: "popup",			
            columns: [
                { field: "locationTypeCode", title: "locationType Code",  width: 75 },//col2
                { field: "locationTypeName", title: "locationType Name", width: 50 },//col3
                { command: ["edit", "destroy"], width: 120 }//col4
            ],//columns
            toolbar: [
                {
                    name: "create",
                    text: "Add New locationType"
                }//tool1
 
            ],//toolbar
            filterable: true,
            sortable: {
                mode: "multiple"
            },
           // editable: "popup",
            pageable: {
                pageSize: 10,
                pageSizes: [10, 25, 50, 100, 1000],
                previousNext: true,
                buttonCount: 5
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
            resizable: true
        });//kendogrid
    };// locationTypeUI.prototype.renderGrid = function() 

function saveLocationTypeGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            locationType[i]=data[i];
        }
    }
    else {
        locationType[0]= data[0];
    }
}

//Save to server function
function saveLocationType() {
    warningDialog(locationType, "NOTE");
    alert(locationType.length);

    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: locationTypeApiUrl + '/PostNew',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(locationType),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog("Location Type(s) saved successfully", 'SUCCESS');

    }).error(function (xhr, data, error) {
            //On error stop loading Dialog and alert a the specific message
            dismissLoadingDialog();
            warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
        });
}//func saveToServer
   

