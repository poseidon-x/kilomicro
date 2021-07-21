

//Declaration of variables 
var authToken = coreERPAPI_Token;

// crud is the route to the api controllers
var openningBalanceBatchApiUrl = coreERPAPI_URL_Root + "/crud/OpeningBalanceBatch";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/InventoryItem";
var productApiUrl = coreERPAPI_URL_Root + "/crud/Product";
var brandApiUrl = coreERPAPI_URL_Root + "/crud/Brand";
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/unitOfMeasurement";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";


var openningBalanceBatch = {};
var locations = {};
var inventoryItems = {};
var products = {};
var brands = {};
var unitOfMeasurements = {};
var accts = {};




//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});



// loads all the form data using the ajax calls defined above
function loadForm() {
    //Declare a variable and store openingBalanceBatch table ajax call in it
    var openningBalanceBatchAjax = $.ajax({
        url: openningBalanceBatchApiUrl + '/Get/' + openningBalanceBatchId,
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

    //Declare a variable and store product table ajax call in it
    var productAjax = $.ajax({
        url: productApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store brand table ajax call in it
    var brandAjax = $.ajax({
        url: brandApiUrl + '/Get',
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

    //Declare a variable and store accounts table ajax call in it
    var acctsAjax = $.ajax({
        url: acctsApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });


    $.when(openningBalanceBatchAjax, locationAjax, inventoryItemAjax, productAjax, brandAjax, unitOfMeasurementAjax, acctsAjax)
        .done(function (dataOpenningBalanceBatch, dataLocation, dataInventoryItem, dataProduct, dataBrand, dataUnitOfMeasurement, dataAccts) {

            // the actual data is contained in the third position in the array hence accessing position index 2
            openningBalanceBatch = dataOpenningBalanceBatch[2].responseJSON;
            locations = dataLocation[2].responseJSON;
            inventoryItems = dataInventoryItem[2].responseJSON;
            products = dataProduct[2].responseJSON;
            brands = dataBrand[2].responseJSON;
            unitOfMeasurements = dataUnitOfMeasurement[2].responseJSON;
            accts = dataAccts[2].responseJSON;
            //Prepares UI
            prepareUi();

        });
}


function prepareUi() {

    renderControls();
        populateUi();
        renderGrid();

        var bt =  '<input type="button" class="btn btn-primary" id="approve" value="Approve" />';
        var msg = '<font color="red" face="verdana">Balance Approved!</font>';
        if (openningBalanceBatch.approved == false) {
            $('#approveButton').html(bt);
        } else {
            $('#approveButton').html(msg);
        }


        dismissLoadingDialog();


    //Validate to Check Empty/Null input Fields
        $('#approve').click(function (event) {
            if ($("#approvalComments").val() == "") {
                smallerWarningDialog('Please Add Your Approval Comments', 'NOTE');
            }
            else{
                if (confirm('Are you sure you want Approve this balance?')) {
                    displayLoadingDialog();
                    saveApproval();
                } else {
                    smallerWarningDialog('Please review and approve later', 'NOTE');
                }
            }

        });



}

// rendering the ui components
function renderControls() {
    $('#openningBalanceBatchDate').kendoDatePicker({
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

    $("#approvalComments").kendoEditor();
    
    if (openningBalanceBatch.approved) {
        $('#approvalComments').data('kendoEditor').value(openningBalanceBatch.approvalComments);
        $($('#approvalComments').data().kendoEditor.body).attr('contenteditable', false);
    }
    

}


function populateUi() {
    $('#openningBalanceBatchDate').data('kendoDatePicker').value(openningBalanceBatch.balanceDate);
    $('#locationId').data('kendoComboBox').value(openningBalanceBatch.locationId);
    $('#created').data('kendoDatePicker').value(openningBalanceBatch.created);
    $('#enteredBy').data('kendoMaskedTextBox').value(openningBalanceBatch.enteredBy);
}

// rendering the grid that displays the opening balances
function renderGrid() {
    $('#approveBalanceGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(openningBalanceBatch.openningBalances);
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
                    id: 'openningBalanceId',
                    fields: {
                        openningBalanceBatchId: { type: 'number', defaultValue: openningBalanceBatch.openningBalanceBatchId },
                        openningBalanceId: { type: 'number', editable: false, },
                        inventoryItemId: { type: 'number', },
                        quantityOnHand: { type: 'number', },
                        productId: { type: 'number', },
                        brandId: { type: 'number', },
                        unitOfMeasurementId: { type: 'number', },
                        drAccountId: { type: 'number', },
                        crAccountId: { type: 'number', },
                    }, //fields
                }, //model
            }, //schema
        }, //datasource
        columns: [
            { field: 'inventoryItemId', title: 'Inventory Item', template: '#= getInventoryItem(inventoryItemId) #' },
            { field: 'quantityOnHand', title: 'Quantity On-Hand', },
            { field: 'productId', title: 'Product',  template: '#= getProduct(productId) #' },
            { field: 'brandId', title: 'Brand', template: '#= getBrand(brandId) #' },
            { field: 'unitOfMeasurementId', title: 'Unit Of Measurement', template: '#= getUnitOfMeasurement(unitOfMeasurementId) #' },
            { field: 'drAccountId', title: 'Debit Account', template: '#= getDrAccount(drAccountId) #' },
            { field: 'crAccountId', title: 'Credit Account', template: '#= getCrAccount(crAccountId) #' },
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
function getProduct(id) {
    for (i = 0; i < products.length; i++) {
        if (products[i].productId == id)
            return products[i].productName;
    }
}

//retrieve inventory Item value from the Grid pop and display on the Grid
function getBrand(id) {
    for (i = 0; i < brands.length; i++) {
        if (brands[i].brandId == id)
            return brands[i].brandName;
    }
}

//retrieve inventory Item value from the Grid pop and display on the Grid
function getUnitOfMeasurement(id) {
    for (i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id)
            return unitOfMeasurements[i].unitOfMeasurementName;
    }
}

//retrieve inventory Item value from the Grid pop and display on the Grid
function getDrAccount(id) {
    for (i = 0; i < accts.length; i++) {
        if (accts[i].acct_id == id)
            return accts[i].acc_name;
    }
}

//retrieve inventory Item value from the Grid pop and display on the Grid
function getCrAccount(id) {
    for (i = 0; i < accts.length; i++) {
        if (accts[i].acct_id == id)
            return accts[i].acc_name;
    }
}



function saveApproval() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    openningBalanceBatch.approvalComments = $('#approvalComments').data('kendoEditor').value();
}

//func saveToServer
function saveToServer() {
    $.ajax({
        url: openningBalanceBatchApiUrl + '/PostApproval',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(openningBalanceBatch),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        successDialog('Balance Approved Successfuly', 'SUCCESS', function () { window.location = '/OpeningBalances/OpenningBalances/'; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
    
}
