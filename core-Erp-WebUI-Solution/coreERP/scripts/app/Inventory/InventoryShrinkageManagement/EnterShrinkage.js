//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/inventoryItem";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var shrinkageBatchApiUrl = coreERPAPI_URL_Root + "/crud/shrinkageBatch";
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/unitOfMeasurement";
var shrinkageReasonApiUrl = coreERPAPI_URL_Root + "/crud/ShrinkageReason";


//Declaration of variables to store records retrieved from the database
var inventoryItem = {};
var inventoryItemDetails = {};
var locations = {};
var shrinkageBatch = {};
var unitOfMeasurements = {};
var shrinkageReasons = {};
var itemDetails = {};



var locationIds = [];

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//Declare a variable and store shrinkageBatch table ajax call in it
var shrinkageBatchAjax = $.ajax({
    url: shrinkageBatchApiUrl + '/Get/' + shrinkageBatchId,
    type: 'Get',
    contentType: 'application/json',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});



//Declare a variable and store location table ajax call in it
var locationsAjax = $.ajax({
    url: locationApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store unitOfMeasurement table ajax call in it
var unitOfMeasurementsAjax = $.ajax({
    url: unitOfMeasurementApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var shrinkageReasonAjax = $.ajax({
    url: shrinkageReasonApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(shrinkageBatchAjax, locationsAjax, unitOfMeasurementsAjax, shrinkageReasonAjax)
        .done(function (dataShrinkageBatch, datalocations, dataunitOfMeasurements, datashrinkageReasons) {
            shrinkageBatch = dataShrinkageBatch[2].responseJSON;
            locations = datalocations[2].responseJSON;
            unitOfMeasurements = dataunitOfMeasurements[2].responseJSON;
            shrinkageReasons = datashrinkageReasons[2].responseJSON;
            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() {
    //If shrinkageBatchId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (shrinkageBatch.shrinkageBatchId > 0) {
        renderControls();
        populateUi();
        renderGrid();
        dismissLoadingDialog();
    } else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        renderControls();
        dismissLoadingDialog();
    }
    getLocationIds(locations);

//Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {
        
        var validator = $("#myform").kendoValidator().data("kendoValidator");
        var locationValue = $("#locationId").val();


        //Retrieve & save Grid data
        var gridData = $("#grid").data().kendoGrid.dataSource.view();

        if (!validator.validate() 
            || new Date($("#shrinkageDate").val()) == 'Invalid Date' || (locationIds.indexOf(locationValue) < 0)) {
            smallerWarningDialog('Please specify a correct Date & Location First', 'ERROR');
        } else {
            if (gridData.length > 0) {
                displayLoadingDialog();
                saveShrinkageGridData(gridData);
                saveShrinkage();
                    }
                    else {
                        smallerWarningDialog('Please Add Shrinkage Details', 'NOTE');
                    }
        }
    });
}

function alphaOnly(event) {
    var key = event.locationId;
    return (key >= 0);
};

function saveShrinkageGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            shrinkageBatch.shrinkages.push(data[i]);
        }
    }
    else shrinkageBatch.shrinkages.push(data[0]);
}

//Apply kendo Style to the input fields
function renderControls() {
	$('#tabs').kendoTabStrip();
    $("#locationId").width('100%')
        .kendoComboBox({
        dataSource: locations,
        dataValueField: 'locationId',
        dataTextField: 'locationName',
        change: onLocationChange,
        filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
    });

    $('#shrinkageDate').width('100%')
        .kendoDatePicker({
        format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		}
        });

    $("#save").kendoButton({
        enable: false
    });
}

var onLocationChange = function () {
    
    displayLoadingDialog();

    var id = $("#locationId").data("kendoComboBox").value();
    document.getElementById("grid").innerHTML = "";
    $.ajax(
        {
            url: inventoryItemApiUrl + '/GetItemByLocation/' + id,
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            inventoryItem = data;
            if (inventoryItem.length > 0) {
                renderGrid();
                var button = $("#save").data("kendoButton");
                // enable button
                button.enable(true);
            } else document.getElementById("grid").innerHTML = "The Selected Location has no Inventory";
            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
}

//Populate the input fields for an update
function populateUi() {
    $('#shrinkageDate').data('kendoDatePicker').value(shrinkageBatch.shrinkageDate);
    $('#locationId').data('kendoComboBox').value(shrinkageBatch.locationId);
}

//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(shrinkageBatch.shrinkages);
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
            }, //transport
            schema: {
                model: {
                    id: 'shrinkageId',
                    fields: {
                        shrinkageBatchId: { type: 'number', defaultValue: shrinkageBatch.shrinkageBatchId },
                        shrinkageId: { type: 'number', editable: false, },
                        inventoryItemId: { type: 'number' },
                        batchNumber: { type: 'string' },
                        quantityShrunk: { type: 'number', validation: { min: 1 } },
                        unitOfMeasurementId: { type: 'number', },
                        shrinkageReasonId: { type: 'number', }

                    }, //fields
                }, //model
            }, //schema
        }, //datasource
        editable: 'popup',
        columns: [
            { field: 'inventoryItemId', title: 'Inventory Item', editor: inventoryItemEditor, template: '#= getInventoryItem(inventoryItemId) #' },
            { field: 'batchNumber', title: 'Batch Number', editor: batchNumberEditor },
            { field: 'quantityShrunk', title: 'Quantity Shrunk', editor: quantityShrunkEditor },
            { field: 'unitOfMeasurementId', title: 'Unit Of Measurement', editor: unitOfMeasurementEditor, template: '#= getUnitOfMeasurement(unitOfMeasurementId) #' },
            { field: 'shrinkageReasonId', title: 'Shrinkage Reason', editor: shrinkageReasonEditor, template: '#= getShrinkageReason(shrinkageReasonId) #' },
            { command: ['edit', 'destroy'] }
        ],
        toolbar: [{ name: 'create', text: 'Add Shrinkage Details', }],
    });
}


//retrieve values from from Input Fields and save 
function saveShrinkage() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    shrinkageBatch.locationId = $('#locationId').data('kendoComboBox').value();
    shrinkageBatch.shrinkageDate = $('#shrinkageDate').data('kendoDatePicker').value();
    shrinkageBatch.shrinkageDate = $('#shrinkageDate').data('kendoDatePicker').value();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: shrinkageBatchApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(shrinkageBatch),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Shrinkage Saved Successfuly', 'SUCCESS', function () { window.location = "/InventoryShrinkage/Shrinkages/"; });
        
    }).error(function (xhr, data, error) {
        shrinkageBatch = {};
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

//prepare inventory item Drop Down for shrinkage grid 
function inventoryItemEditor(container, options) {
    $('<input type="text" id="inventoryItem" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: inventoryItem,
        dataValueField: "inventoryItemId",
        dataTextField: "inventoryItemName",
        change: oninventoryItemChange,
        optionLabel: ' '
    });
}

var oninventoryItemChange = function (e) {        
    displayLoadingDialog();
    var id = $("#inventoryItem").data("kendoDropDownList").value();
    var dropDown = $("#inventoryItemDetail").data("kendoDropDownList");

    //If user selects nothing/the optionlabel, show alert message 
    if (id === null || id === ' ') {
        smallerSuccessDialog("Inventory Item cannot be empty", "ALERT");
    } else {
        $.ajax(
        {
            url: inventoryItemApiUrl + '/GetDetails/' + id,
            type: 'Get',
            beforeSend: function(req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function(data) {
            if (data.length < 1) {
                smallerSuccessDialog("There is no inventory for the selected Item", "ALERT");
                // enable button
                dropDown.enable(false);
            } else {
                inventoryItemDetails = data;
                itemDetails = data;
                dropDown.dataSource.data(inventoryItemDetails);

                // enable button
                dropDown.enable(true);
            }

            dismissLoadingDialog();
        }).error(function(error) {
            alert(JSON.stringify(error));
        });

    }
}

function batchNumberEditor(container, options) {
    $('<input type="text" id="inventoryItemDetail" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        enable: false,
        dataSource: inventoryItemDetails,
        dataValueField: "batchNumber",
        dataTextField: "batchNumber",
        change: onBatchNumberChange,
        optionLabel: ' '
    });

}

var onBatchNumberChange = function (e) {
    var batchNum = $("#inventoryItemDetail").data("kendoDropDownList").value();
    var dropDown = $("#quantityShrunk").data("kendoNumericTextBox");
    dropDown.enable(true);

    var maxQuantityShrunk = 0;
    for (var i = 0; i < itemDetails.length; i++) {
        if (itemDetails[i].batchNumber == batchNum) {
            maxQuantityShrunk = itemDetails[i].quantityOnHand;
            break;
        }
    }
    smallerSuccessDialog("Available Quantity:" +maxQuantityShrunk, "NOTE");
    dropDown.max(maxQuantityShrunk);
}

function quantityShrunkEditor(container, options) {
    $('<input type="text" id="quantityShrunk" disabled="disabled" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        min: 1
    });

}


//prepare inventory item Drop Down for shrinkage grid 
function shrinkageReasonEditor(container, options) {
    $('<input type="text" id="shrinkageReason"  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: shrinkageReasons,
        dataValueField: "shrinkageReasonId",
        dataTextField: "shrinkageReasonName",
        optionLabel: ' '
    });
}

//prepare unit of Measurement Drop Down for shrinkage grid 
function unitOfMeasurementEditor(container, options) {
    $('<input type="text" id="unitOfMeasurement" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: unitOfMeasurements,
        dataValueField: "unitOfMeasurementId",
        dataTextField: "unitOfMeasurementName",
        optionLabel: ' '
    });
}

function getLocationIds(data) {
    if (data.length >= 1) {
        for (var i = 0; i < data.length; i++) {
            locationIds.push(JSON.stringify(data[i].locationId));
        }
    }
}

//retrieve inventory Item value from the Grid pop and display on the Grid
function getInventoryItem(id) {
    for (i = 0; i < inventoryItem.length; i++) {
        if (inventoryItem[i].inventoryItemId == id) 
        return inventoryItem[i].inventoryItemName;
    }
}

//retrieve unit of Measurement value from the Grid pop and display on the Grid
function getUnitOfMeasurement(id) {
    for (i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id)
            return unitOfMeasurements[i].unitOfMeasurementName;
    }
}

function getShrinkageReason(id) {
    for (i = 0; i < shrinkageReasons.length; i++) {
        if (shrinkageReasons[i].shrinkageReasonId == id)
            return shrinkageReasons[i].shrinkageReasonName;
    }
}



