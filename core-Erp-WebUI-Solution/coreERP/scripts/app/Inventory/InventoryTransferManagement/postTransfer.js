//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var inventoryTransferApiUrl = coreERPAPI_URL_Root + "/crud/inventoryTransfer";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/inventoryItem";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var curDetail = {};

//Declaration of variables to store records retrieved from the database
var inventoryTransfer = {};
var locations = {};
var inventoryItems = {};
var unitOfMeasurements = {};
var accts = {};


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    //Declare a variable and store shrinkageBatch table ajax call in it
    var inventoryTransferAjax = $.ajax({
        url: inventoryTransferApiUrl + '/Get/' + inventoryTransferId,
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



    var accountAjax = $.ajax({
        url: acctsApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });



    $.when(inventoryTransferAjax, locationAjax, inventoryItemAjax, accountAjax)
        .done(function (dataInventoryTransfer, dataLocation, dataInventoryItem, dataAccountAjax) {
            inventoryTransfer = dataInventoryTransfer[2].responseJSON;
            locations = dataLocation[2].responseJSON;
            inventoryItems = dataInventoryItem[2].responseJSON;
            accts = dataAccountAjax[2].responseJSON;
            //Prepares UI
            prepareUi();

        });
}


//Function to prepare user interface
function prepareUi() {

    renderControls();
    populateUi();
    renderGrid();

    var bt = '<input type="button" class="btn btn-primary" id="post" value="Post Transfer" />';
    var msg = '<font color="red" face="verdana">Transfer Posted!</font>';
    if (inventoryTransfer.posted == false) {
        $('#postButton').html(bt);
    } else {
        $('#postButton').html(msg);
    }

    dismissLoadingDialog();



    $('#post').click(function (event) {
        if ($("#postingComments").val() == "") {
            smallerWarningDialog('Please Add Your Posting Comments', 'NOTE');
        } else {
            if (confirm('Are you sure you want Post this Transfer?')) {
                displayLoadingDialog();
                saveTransfer();
            } else {
                smallerWarningDialog('Please review and post later', 'NOTE');
            }
        }

    });
}




//Apply kendo Style to the input fields
function renderControls() {
    $("#fromLocation").kendoComboBox({
        dataSource: locations,
        dataValueField: 'locationId',
        dataTextField: 'locationName',
        optionLabel: '',
    });

    $("#toLocation").kendoComboBox({
        dataSource: locations,
        dataValueField: 'locationId',
        dataTextField: 'locationName',
        optionLabel: '',
    });

    $('#requisitionDate').kendoDatePicker({
        format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
    });

    $("#created").kendoDatePicker({
        format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
    });

    $('#enteredBy').kendoMaskedTextBox({
    });

    $("#postingComments").kendoEditor();
}

//Populate the input fields for an update
function populateUi() {
    $('#fromLocation').data('kendoComboBox').value(inventoryTransfer.fromLocationId);
    $('#toLocation').data('kendoComboBox').value(inventoryTransfer.toLocationId);
    $('#requisitionDate').data('kendoDatePicker').value(inventoryTransfer.requisitionDate);
    $('#created').data('kendoDatePicker').value(inventoryTransfer.created);
    $('#enteredBy').data('kendoMaskedTextBox').value(inventoryTransfer.enteredBy);

    if (inventoryTransfer.posted) {
        $('#postingComments').data('kendoEditor').value(inventoryTransfer.postingComments);
        $($('#postingComments').data().kendoEditor.body).attr('contenteditable', false);
    }
}

//render Grid
function renderGrid() {
    $('#postTransferGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(inventoryTransfer.inventoryTransferDetails);
                }
            }, //transport
            schema: {
                model: {
                    id: 'inventoryTransferDetailId',
                    fields: {
                        inventoryTransferId: { type: 'number', defaultValue: inventoryTransfer.inventoryTransferId },
                        inventoryTransferDetailId: { type: 'number', editable: false, },
                        inventoryItemId: { type: 'number', validation: { required: true }, },
                        quantityTransferred: { type: 'number', validation: { required: true }, },
                        fromAccountId: { type: 'number', validation: { required: true }, },
                        toAccountId: { type: 'number', validation: { required: true }, },

                    }, //fields
                }, //model
            }, //schema
        }, //datasource
        editable: 'popup',
        columns: [
            { field: 'inventoryItemId', title: 'Inventory Item', template: '#= getInventoryItem(inventoryItemId) #' },
            { field: 'quantityTransferred', title: 'Quantity Transferred', },
            { field: 'fromAccountId', title: 'From GL Account', template: '#= getAccount(fromAccountId) #' },
            { field: 'toAccountId', title: 'To GL Account', template: '#= getAccount(toAccountId) #' },
        ],
        detailInit: lineInit,
    });

}


function lineInit(e) {
    $("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: inventoryTransferApiUrl + '/GetDetailLine/' + e.data.inventoryTransferDetailId,
                    type: 'Get',
                    contentType: 'application/json',
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
            },
            pageSize: 10,
            schema: {
                model: {
                    id: 'inventoryTransferDetailId',
                    fields: {
                        inventoryTransferDetailId: { type: 'number', defaultValue: e.data.inventoryTransferDetailId },
                        inventoryTransferDetailLineId: { type: 'number', editable: false, },
                        quantityTransferred: { type: 'number', validation: { required: true }, },
                        batchNumber: { type: 'string', validation: { required: true }, },
                        mfgDate: { type: 'date', validation: { required: true }, },
                        expiryDate: { type: 'date', validation: { required: true }, },
                        unitCost: { type: 'number', validation: { required: true }, },
                        creator: { type: 'string', validation: { required: true }, },
                        created: { type: 'date', validation: { required: true }, },
                    }, //fields
                }, //model
            }, //schema
        },
        scrollable: false,
        sortable: true,
        pageable: true,
        columns: [
            { field: 'quantityTransferred', title: 'Quantity Transferred' },
            { field: 'batchNumber', title: 'Batch Number' },
            { field: 'mfgDate', title: 'Manufacturing Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'expiryDate', title: 'Expiry Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'unitCost', title: 'Unit Cost' },
            { field: 'creator', title: 'Entered By' },
            { field: 'created', title: 'Entry Date', format: '{0:dd-MMM-yyyy}' },
        ],
    });
};


//retrieve values from from Input Fields and save 
function saveTransfer() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    inventoryTransfer.postingComments = $('#postingComments').data('kendoEditor').value();
}


//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: inventoryTransferApiUrl + '/TransferPost',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(inventoryTransfer),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Transfer Saved Successfuly', 'SUCCESS', function () { window.location = "/InventoryTransfer/Transfers/"; });
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}//func saveToServer

function getAccount(id) {
    for (i = 0; i < accts.length; i++) {
        if (accts[i].acct_id == id) {
            return accts[i].acc_name;
        }
    }
}

//retrieve inventory Item value from the Grid pop and display on the Grid
function getInventoryItem(id) {
    for (i = 0; i < inventoryItems.length; i++) {
        if (inventoryItems[i].inventoryItemId == id)
            return inventoryItems[i].inventoryItemName;

    }
}



