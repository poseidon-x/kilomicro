

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
var gridData = {};

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
    if (openningBalanceBatch.openningBalanceBatchId > 0) {
        renderControls();
        populateUi();
        renderGrid();
        dismissLoadingDialog();
    }
    else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        renderControls();
        renderGrid();
        dismissLoadingDialog();
    }

    //Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {

        var validator = $("#myform").kendoValidator().data("kendoValidator");
        var undefineLocation = locations.indexOf($("#locationId").val());
        //var undefineLocation = (locations.indexOf($('#locationId').data('kendoComboBox').value()));
        gridData = $("#openingBalanceGrid").data().kendoGrid.dataSource.view();


        parseInt(undefineLocation);

        if (!validator.validate() || !(isFinite(undefineLocation)))
        {
            smallerWarningDialog('Please specify a correct Date & Location First', 'ERROR');
        } else {
            if (gridData.length > 0) {
                displayLoadingDialog();
                //save Grid data
                saveOpeningBalanceBatch();
            }
            else {
                smallerWarningDialog('Please Add Balance Details', 'NOTE');
            }
        }
    });
}


// rendering the ui components
function renderControls() {
	$('#tabs').kendoTabStrip();
    $("#locationId").width('90%').kendoComboBox({
        dataSource: locations,
        dataValueField: 'locationId',
        dataTextField: 'locationName',
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

    $('#openningBalanceBatchDate').width('90%').kendoDatePicker({
        format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
    });
}


function populateUi() {
    $('#locationId').data('kendoComboBox').value(openningBalanceBatch.locationId);
    $('#openningBalanceBatchDate').data('kendoDatePicker').value(openningBalanceBatch.balanceDate);
}

// rendering the grid that displays the opening balances
function renderGrid() {
    $('#openingBalanceGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function(entries) {
                    entries.success(openningBalanceBatch.openningBalances);
                },
                create: function(entries) {
                    entries.success(entries.data);
                },
                update: function(entries) {
                    entries.success();
                },
                destroy: function(entries) {
                    entries.success();
                }
            }, //transport
            schema: {
                model: {
                    id: 'openningBalanceId',
                    fields: {
                        openningBalanceBatchId: { type: 'number', defaultValue: openningBalanceBatch.openningBalanceBatchId },
                        openningBalanceId: { type: 'number', editable: false },
                        inventoryItemId: { validation: { min: 1 } },
                        batchNumber: { type: 'string' },
                        quantityOnHand: { type: 'number', validation: { min: 1 } },
                        unitOfMeasurementId: { validation: { required: true } }
                    } //fields
                } //model
            } //schema
        }, //datasource
        editable: 'popup',
        columns: [
			{ field: 'inventoryItemId', title: 'Inventory Item', editor: inventoryItemEditor, template: '#= getInventoryItem(inventoryItemId) #' },
			{ field: 'batchNumber', title: 'Batch Number' },
			{ field: 'quantityOnHand', title: 'Quantity' },
			{ field: 'unitOfMeasurementId', title: 'Unit Of Measurement', editor: unitOfMeasurementEditor, template: '#= getUnitOfMeasurement(unitOfMeasurementId) #' },          
			{ command: ['edit', 'delete'],    width: 160 } 
		],
        toolbar: [{ name: 'add', text: 'Add Details' }],
		edit: function (e) {
			var editWindow = this.editable.element.data("kendoWindow");
			editWindow.wrapper.css({ width: 500 });
			editWindow.title("Edit Details");
		},
    });
}

//prepare inventory item Drop Down for shrinkage grid 
function inventoryItemEditor(container, options) {
    $('<input type="text" id="inventoryItem" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoComboBox({
        dataSource: inventoryItems,
        dataValueField: "inventoryItemId",
        dataTextField: "inventoryItemName",
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
	currentModel = options.model;
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
	tooltipElement.appendTo(container);
}

//prepare inventory item Drop Down for shrinkage grid 
function productEditor(container, options) {
    $('<input type="text" id="product" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width('100%')
    .kendoComboBox({
        dataSource: products,
        dataValueField: "productId",
        dataTextField: "productName",
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
}

//prepare inventory item Drop Down for shrinkage grid 
function brandEditor(container, options) {
    $('<input type="text" id="brand" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoComboBox({
        dataSource: brands,
        dataValueField: "brandId",
        dataTextField: "brandName",
        ofilter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
    });
}

var oninventoryItemChange = function (e) {
    var productId;
    var brandId;

    var id = e.sender.value();
    for (var i = 0; i < inventoryItems.length; i++) {
        if (inventoryItems[i].inventoryItemId == id) {
            productId = inventoryItems[i].productId;
        }
    }
    for (var i = 0; i < inventoryItems.length; i++) {
        if (inventoryItems[i].inventoryItemId == id) {
            brandId = inventoryItems[i].brandId;
        }
    }
    $("#product").data('kendoComboBox').value(productId);
    $("#brand").data('kendoComboBox').value(brandId);

}

//prepare inventory item Drop Down for shrinkage grid 
function unitOfMeasurementEditor(container, options) {
    $('<input type="text" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoComboBox({
        dataSource: unitOfMeasurements,
        dataValueField: "unitOfMeasurementId",
        dataTextField: "unitOfMeasurementName",
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
}

//prepare inventory item Drop Down for shrinkage grid 
function crAccountEditor(container, options) {
    $('<input type="text" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoComboBox({
        dataSource: accts,
        dataValueField: "acct_id",
        dataTextField: "acc_name",
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

function saveOpeningBalanceBatch() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    openningBalanceBatch.balanceDate = $('#openningBalanceBatchDate').data('kendoDatePicker').value();
    openningBalanceBatch.locationId = $('#locationId').data('kendoComboBox').value();
	openningBalanceBatch.openningBalances = $("#openingBalanceGrid").data().kendoGrid.dataSource.view();
}


function saveToServer() {
    $.ajax({
        url: openningBalanceBatchApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(openningBalanceBatch),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        successDialog('Opening Balance Saved Successfuly', 'SUCCESS', function () { window.location = "/OpeningBalances/OpenningBalances/"; });

    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
} //func saveToServer
/*
function saveInventoryBalanceData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            openningBalanceBatch.openningBalances.push(data[i]);
        }
    }
    else openningBalanceBatch.openningBalances.push(data[0]);
}*/

