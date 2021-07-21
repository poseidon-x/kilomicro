//Declaration of variables 
var authToken = coreERPAPI_Token;

// crud is the route to the api controllers
var shrinkageBatchApiUrl = coreERPAPI_URL_Root + "/crud/shrinkageBatch";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/InventoryItem";
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/unitOfMeasurement";

var shrinkageBatch = {};
var locations = {};
var inventoryItems = {};
var unitOfMeasurements = {};



//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});



// loads all the form data using the ajax calls defined above
function loadForm() {
    //Declare a variable and store openingBalanceBatch table ajax call in it
    var shrinkageBatchAjax = $.ajax({
        url: shrinkageBatchApiUrl + '/Get/' + shrinkageBatchId,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store location table ajax call in it
    var locationAjax = $.ajax({
        url: locationApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store inventoryItem table ajax call in it
    var inventoryItemAjax = $.ajax({
        url: inventoryItemApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store unitOfMeasurement table ajax call in it
    var unitOfMeasurementAjax = $.ajax({
        url: unitOfMeasurementApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });


    $.when(shrinkageBatchAjax, locationAjax, inventoryItemAjax, unitOfMeasurementAjax)
        .done(function (datashrinkageBatch, dataLocation, dataInventoryItem, dataUnitOfMeasurement) {

            // the actual data is contained in the third position in the array hence accessing position index 2
            shrinkageBatch = datashrinkageBatch[2].responseJSON;
            locations = dataLocation[2].responseJSON;
            inventoryItems = dataInventoryItem[2].responseJSON;
            unitOfMeasurements = dataUnitOfMeasurement[2].responseJSON;
            //Prepares UI
            prepareUi();

        });
}


function prepareUi() {

    renderControls();
    populateUi();
    renderGrid();

    var btn = '<input type="button" class="btn btn-primary" id="post" value="Post Shrinkage" />';
    var msg = '<font color="red" face="verdana">Shrikage Posted!</font>';

        if (shrinkageBatch.posted == false) {
            $('#postButton').html(btn);
        } else {
            $('#postButton').html(msg);
        }
    

    dismissLoadingDialog();


    //Validate to Check Empty/Null input Fields
    $('#post').click(function (event) {
        //CHECK IF SHRINKAGE IS APPROVED
        if (shrinkageBatch.approved) {
            //CHECK IF COMMENT TEXTAREA IS NOT EMPTY
            if ($("#postingComments").val() == "") {
                smallerWarningDialog('Please Add Your Posting Comments', 'NOTE');
            } else {
                    if (confirm('Are you sure you want Post this Shrinkage?')) {
                        displayLoadingDialog();
                        savePost();
                } else {
                        smallerWarningDialog('Please review and post later', 'NOTE');
                }
        }
    }else
        {
            smallerWarningDialog("Sorry, this shrinkage has not been approved", "NOTE");

        }
    });



}

// rendering the ui components
function renderControls() {
    $('#shrinkageDate').kendoDatePicker({
        format: 'dd-MM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
    });

    $("#locationId").kendoComboBox({
        dataSource: locations,
        dataValueField: 'locationId',
        dataTextField: 'locationName',
        optionLabel: '',
    });

    $("#created").kendoDatePicker({
        format: 'dd-MM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
    });

    $('#enteredBy').kendoMaskedTextBox({
    });

    $("#postingComments").kendoEditor();

}


function populateUi() {
    $('#shrinkageDate').data('kendoDatePicker').value(shrinkageBatch.shrinkageDate);
    $('#locationId').data('kendoComboBox').value(shrinkageBatch.locationId);
    $('#created').data('kendoDatePicker').value(shrinkageBatch.created);
    $('#enteredBy').data('kendoMaskedTextBox').value(shrinkageBatch.enteredBy);

    if (shrinkageBatch.posted) {
        $('#postingComments').data('kendoEditor').value(shrinkageBatch.postingComments);
        $($('#postingComments').data().kendoEditor.body).attr('contenteditable', false);
    }
}

// rendering the grid that displays the opening balances
function renderGrid() {
    $('#postShrinkageGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(shrinkageBatch.shrinkages);
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
            }, //transport
            schema: {
                model: {
                    id: 'shrinkageId',
                    fields: {
                        shrinkageBatchId: { type: 'number', defaultValue: shrinkageBatch.shrinkageBatchId },
                        shrinkageId: { type: 'number', editable: false, },
                        inventoryItemId: { type: 'number', },
                        quantityShrunk: { type: 'number', },
                        unitOfMeasurementId: { type: 'number', },
                    }, //fields
                }, //model
            }, //schema
        }, //datasource
        columns: [
            { field: 'inventoryItemId', title: 'Inventory Item', template: '#= getInventoryItem(inventoryItemId) #' },
            { field: 'quantityShrunk', title: 'Quantity Shrunk', },
            { field: 'unitOfMeasurementId', title: 'Unit Of Measurement', template: '#= getUnitOfMeasurement(unitOfMeasurementId) #' },
        ],
    });
}

//retrieve inventory Item value from the Grid pop and display on the Grid
function getInventoryItem(id) {
    for (i = 0; i < inventoryItems.length; i++) {
        if (inventoryItems[i].inventoryItemId == id)
            return inventoryItems[i].inventoryItemName;
    }
}


//retrieve inventory Item value from the Grid pop and display on the Grid
function getUnitOfMeasurement(id) {
    for (i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id)
            return unitOfMeasurements[i].unitOfMeasurementName;
    }
}

function savePost() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    shrinkageBatch.postingComments = $('#postingComments').data('kendoEditor').value();
}

//func saveToServer
function saveToServer() {
    $.ajax({
        url: shrinkageBatchApiUrl + '/ShrinkagePost',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(shrinkageBatch),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        successDialog('Shrinkage Posted Successfuly', 'SUCCESS', function () { window.location = '/InventoryShrinkage/Shrinkages/'; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });

}
