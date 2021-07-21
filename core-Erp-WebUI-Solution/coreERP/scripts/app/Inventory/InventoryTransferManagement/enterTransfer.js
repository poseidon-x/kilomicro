//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var inventoryTransferApiUrl = coreERPAPI_URL_Root + "/crud/inventoryTransfer";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/inventoryItem";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var detailLine = {};

var lineCount = 0;

//Declaration of variables to store records retrieved from the database
var inventoryTransfer = {};
var locations = {};
var inventoryItems = {};
var unitOfMeasurements = {};
var accts = {};

var inventoryItemUnitCost = 0.00;

var curDetail = {};
var transDet = {};


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

var formErrorMsg = "";
var locationIds = [];

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
    //If inventoryTransferId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (inventoryTransfer.inventoryTransferId > 0) {
        renderControls();
        populateUi();
        renderGrid();
        dismissLoadingDialog();
    } else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        renderControls();
        renderGrid();
        dismissLoadingDialog();
    }

    getLocationIds(locations);

//Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {

		//var childGrid = $('#transferGrid').closest(".k-grid").data().kendoGrid.dataSource.view();
        var gridData = $("#transferGrid").data().kendoGrid.dataSource.view();
		
		//warningDialog(JSON.stringify(inventoryTransfer), 'ERROR');

         var validator = $("#myform").kendoValidator().data("kendoValidator");
         formValidator();

         validator.validate();

         if (!formErrorMsg == "") {
             smallerWarningDialog(formErrorMsg, 'ERROR');
             formErrorMsg = "";
         } 
		 else {
             if (gridData.length > 0) {
			     //for (var i = 0; i < inventoryTransfer.inventoryTransferDetails.length; i++) {
                     //var detail = inventoryTransfer.inventoryTransferDetails[i];
                     //if (detail.inventoryTransferDetailLines.length > 0) {
                         //displayLoadingDialog();
                         saveTransfer();
                     //} else {
                         //smallerWarningDialog('Please Add Transfer Detail Line', 'NOTE');
                     //}
                 
             }
             else {
                 smallerWarningDialog('Please Add Transfer Details', 'NOTE');
			 }
        }
         
    });
}
/*
function saveInventoryTransferGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            inventoryTransfer.inventoryTransferDetails.push(data[i]);

        }
    } else {
        inventoryTransfer.inventoryTransferDetails.push(data[0]);
    }
}
*/

function formValidator() {
    var fromLocationValue = $("#fromLocation").val();
    var toLocationValue = $("#toLocation").val();

    if (!$("#requisitionDate").val()=="" && new Date($("#requisitionDate").val()) == 'Invalid Date') {
        formErrorMsg += "Invalid Requisition Date <br/>";
    }
    if ($("#requisitionDate").val() == "") { formErrorMsg += "Requisition Date is required<br/>"; }

    if (!fromLocationValue == "" && locationIds.indexOf(fromLocationValue) < 0) {
        formErrorMsg += "Invalid From Location <br/>";
    }
    if ($("#fromLocation").val() == 0) { formErrorMsg += "From Location is required<br/>"; }

    if (!toLocationValue == "" && locationIds.indexOf(toLocationValue) < 0) {
        formErrorMsg += "Invalid To Location <br/>";
    }
    if ($("#toLocation").val() == 0) { formErrorMsg += "To Location is required<br/>"; }

    if ($("#toLocation").val() == $("#fromLocation").val()) { formErrorMsg += "From & To Location cannot be same<br/>"; }


}


//Apply kendo Style to the input fields
function renderControls() {
	$('#tabs').kendoTabStrip();
    $("#fromLocation")
        .width('90%')
        .kendoComboBox({
        dataSource: locations,
        dataValueField: 'locationId',
        dataTextField: 'locationName',
        optionLabel: '',
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

    $("#toLocation")
        .width('90%')
        .kendoComboBox({
        dataSource: locations,
        dataValueField: 'locationId',
        dataTextField: 'locationName',
        optionLabel: '',
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

    $('#requisitionDate')
	.width('90%')
	.kendoDatePicker({
        format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
    });

}

//Populate the input fields for an update
function populateUi() {
    $('#fromLocation').data('kendoComboBox').value(inventoryTransfer.fromLocationId);
    $('#toLocation').data('kendoComboBox').value(inventoryTransfer.toLocationId);
    $('#requisitionDate').data('kendoDatePicker').value(inventoryTransfer.requisitionDate);
}

//render Grid
function renderGrid() {
    $('#transferGrid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(inventoryTransfer.inventoryTransferDetails);
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
                    id: 'inventoryTransferDetailId',
                    fields: {
                        inventoryTransferId: { type: 'number', defaultValue: inventoryTransfer.inventoryTransferId },
                        inventoryTransferDetailId: { type: 'number', editable: false, },
                        inventoryItemId: {type: 'number', validation: {required: true,},},
                        quantityTransferred: { type: 'number', validation: { required: true }, },

                    }, //fields
                }, //model
            }, //schema
        }, //datasource
        editable: 'popup',
        columns: [
            { field: 'inventoryItemId', title: 'Inventory Item', editor: inventoryItemEditor, template: '#= getInventoryItem(inventoryItemId) #' },            { field: 'quantityTransferred', title: 'Quantity Transferred', },
            { command: ['edit', 'destroy'], width: 170 }
        ],
        edit: function (e) {
            var editWindow = this.editable.element.data("kendoWindow");
            editWindow.wrapper.css({ width: 400 });
            editWindow.title("Edit Inventory Transfer");
        },
        toolbar: [{ name: 'create', text: 'Add Transfer Details', }],
        detailInit: lineInit,
		dataBound: function ()
            {
                this.expandRow(this.tbody.find("tr.k-master-row").first());
            },
    });

}


  function lineInit(e) {
        $("<div/>").appendTo(e.detailCell).kendoGrid({
            dataSource: {
                transport: {
                    read: function (entries) {
                        if (typeof (e.data.inventoryTransferDetailLines) === "undefined") {
                            e.data.inventoryTransferDetailLines = [];
                        }
                        entries.success(e.data.inventoryTransferDetailLines); 
					},
                    create: function (entries) {
                        entries.success(entries.data);
                    },
                    update: function (entries) {
                        entries.success();
                    },
                    destroy: function (entries) {
                        entries.success();
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: 'inventoryTransferDetailLineId',
                        fields: {
                            inventoryTransferDetailId: { type: 'number', defaultValue: e.data.inventoryTransferDetailId },
                            inventoryTransferDetailLineId: { type: 'number', editable: false},
                            quantityTransferred: { type: 'number', validation: { required: true } },
                            mfgDate: { type: 'date', validation: { required: true }},
                            expiryDate: { type: 'date', validation: { required: true } },
                            unitCost: { type: 'number', validation: { required: true } }
                        }, //fields
                    }, //model
                }, //schema
            },
            scrollable: false,
            sortable: true,
            pageable: true,
            editable: "popup", 
            columns: [
                { field: 'quantityTransferred', title: 'Quantity Transferred'},
                { field: 'mfgDate', title: 'Manufacturing Date', format: '{0:dd-MMM-yyyy}' },
                { field: 'expiryDate', title: 'Expiry Date', format: '{0:dd-MMM-yyyy}'},
                { field: 'unitCost', title: 'Unit Cost'},
                { command: ['edit', 'destroy'] }
            ],
            toolbar: [{ name: 'create', text: 'Add Transfer Details Line', }],
        }).data("kendoGrid");
    };

//retrieve values from from Input Fields and save 
  function saveTransfer() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
	inventoryTransfer.inventoryTransferDetails = [];
    inventoryTransfer.fromLocationId = $('#fromLocation').data('kendoComboBox').value();
    inventoryTransfer.toLocationId = $('#toLocation').data('kendoComboBox').value();
    inventoryTransfer.requisitionDate = $('#requisitionDate').data('kendoDatePicker').value();
	inventoryTransfer.inventoryTransferDetails = $("#transferGrid").data().kendoGrid.dataSource.view();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: inventoryTransferApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(inventoryTransfer),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Transfer Saved Successfuly', 'SUCCESS', function () { window.location = "/InventoryTransfer/Transfers/"; });
        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer



//prepare from Account Drop Down for Transefer detail grid 
function fromAccountEditor(container, options) {
    $('<input type="text" id="fromAccount" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
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

//prepare to Account Drop Down for Transefer detail grid 
function toAccountEditor(container, options) {
    $('<input type="text" id="toAccount" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
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

function inventoryItemEditor(container, options) {
    $('<input type="text" id="toAccount" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
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
}
//function to retrieve account name base on the account id
function getAccount(id) {
    for (i = 0; i < accts.length; i++) {
        if (accts[i].acct_id == id) {
            return accts[i].acc_name;
        }
    }
}

//function to retrieve inventory Item name base on the inventory id from Grid
function getInventoryItem(id) {
    for (i = 0; i < inventoryItems.length; i++) {
        if (inventoryItems[i].inventoryItemId == id) 
        return inventoryItems[i].inventoryItemName;
    
}
}

//function to retrieve location name base on the location id from Grid
function getLocationIds(data) {
    if (data.length >= 1) {
        for (var i = 0; i < data.length; i++) {

            locationIds.push(JSON.stringify(data[i].locationId));
        }
    }
}

function getInventoryItemUnitCost() {
        for (var i = 0; i < inventoryItems.length; i++) {
            if (inventoryItems[i].inventoryItemId == curDetail.inventoryItemId)
            inventoryItemUnitCost = inventoryItems[i].unitCost;
            return inventoryItems[i].unitCost;
    }    
}




