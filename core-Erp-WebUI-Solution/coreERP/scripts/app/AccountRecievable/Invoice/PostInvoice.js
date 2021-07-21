//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var invoiceApiUrl = coreERPAPI_URL_Root + "/crud/ArInvoice";
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/inventoryItem";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/unitOfMeasurement";
var paymentTermApiUrl = coreERPAPI_URL_Root + "/crud/paymentTerm";
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var salesOrderApiUrl = coreERPAPI_URL_Root + "/crud/salesOrder";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var companyProfileApiUrl = coreERPAPI_URL_Root + "/crud/companyProfile";



//Declaration of variables to store records retrieved from the database
var arInvoice = {}
var inventoryItems = {};
var locations = {};
var accounts = {};
var unitOfMeasurements = {};
var paymentTerms = {};
var customers = {};
var salesOrder = {};
var currencies = {};
var companyProfile = {};
var subTotalAfterDiscount = 0;
var invoiceTotal = 0;



//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//Declare a variable and store shrinkageBatch table ajax call in it
var invoiceAjax = $.ajax({
    url: invoiceApiUrl + '/Get/' + invoiceId,
    type: 'Get',
    contentType: 'application/json',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store location table ajax call in it
var inventoryItemsAjax = $.ajax({
    url: inventoryItemApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store unitOfMeasurement table ajax call in it
var locationsAjax = $.ajax({
    url: locationApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var accountsAjax = $.ajax({
    url: acctsApiUrl + '/Get',
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

var paymentTermAjax = $.ajax({
    url: paymentTermApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var customersAjax = $.ajax({
    url: customerApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var salesOrderAjax = $.ajax({
    url: salesOrderApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store currency table ajax call in it
var currencyAjax = $.ajax({
    url: currencyApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store currency table ajax call in it
var companyProfileAjax = $.ajax({
    url: companyProfileApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(invoiceAjax, inventoryItemsAjax, locationsAjax, accountsAjax, unitOfMeasurementsAjax, paymentTermAjax, customersAjax, salesOrderAjax, currencyAjax, companyProfileAjax)
        .done(function (dataInvoice, dataInventoryItems, dataLocations, dataAccounts, dataUnitOfMeasurements, dataPaymentTerm, dataCustomers, dataSalesOrder, dataCurrency, dataCompanyProfile) {
            arInvoice = dataInvoice[2].responseJSON;
			inventoryItems = dataInventoryItems[2].responseJSON;
            locations = dataLocations[2].responseJSON;
			accounts = dataAccounts[2].responseJSON;
            unitOfMeasurements = dataUnitOfMeasurements[2].responseJSON;
            paymentTerms = dataPaymentTerm[2].responseJSON;
            customers = dataCustomers[2].responseJSON;
            salesOrder = dataSalesOrder[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;	
            companyProfile = dataCompanyProfile[2].responseJSON;	

            //Prepares UI
            prepareUi();
        });
}

function prepareUi() {

    renderControls();
    populateUi();
    renderGrid();

    var btn = '<input type="button" class="btn btn-primary" id="post" value="Post Invoice" />';
    var msg = '<font color="red" face="verdana">Invoice Already Posted!</font>';

        if (!arInvoice.posted) {
            $('#postButton').html(btn);
        } else {
            $('#postButton').html(msg);
        }
    
    dismissLoadingDialog();


    //Validate to Check Empty/Null input Fields
    $('#post').click(function (event) 
	{
        //CHECK IF INVOICE IS NOT POSTED
        if (!arInvoice.posted) 
		{
            if (confirm('Are you sure you want Post this Invoice?')) 
			{
                displayLoadingDialog();
                savePost();
			} else 
			{
                smallerWarningDialog('Please review and post later', 'NOTE');
			}
		}
	});
}

// rendering the ui components
function renderControls() 
	{
		$("#salesOrder").width('75%')
			.kendoComboBox({
			dataSource: salesOrder,
			dataValueField: 'salesOrderId',
			dataTextField: 'orderNumber',
			optionLabel: '',
		});
		
		$("#invoiceDate").width('75%')
			.kendoDatePicker({
			format: 'dd-MMM-yyyy',
			parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
		});

		$("#customer").width('75%')
			.kendoComboBox({
			dataSource: customers,
			dataValueField: 'customerId',
			dataTextField: 'customerName',
			optionLabel: '',
		});
		
		$("#paymentTerm").width('75%')
			.kendoComboBox({
			dataSource: paymentTerms,
			dataValueField: 'paymentTermID',
			dataTextField: 'paymentTerms',
			optionLabel: ''
		});	
		
		$("#currency").width('75%')
		.kendoComboBox({
			dataSource: currencies,
			dataValueField: 'currency_id',
			dataTextField: 'major_name',
			optionLabel: ''
		});
	}


function populateUi() {
	if(arInvoice.salesOrderId > 0)
	{
		document.getElementById('salesOrder1').checked = true;
	}else
	{
		document.getElementById('salesOrder2').checked = true;
	}
	$('#salesOrder2').val(arInvoice.salesOrderId);
    $('#salesOrder').data('kendoComboBox').value(arInvoice.salesOrderId);
    $('#invoiceDate').data('kendoDatePicker').value(arInvoice.invoiceDate);	
    $('#customer').data('kendoComboBox').value(arInvoice.customerId);
    $('#paymentTerm').data('kendoComboBox').value(arInvoice.paymentTermId);
    $('#currency').data('kendoComboBox').value(arInvoice.currencyId);
	if(arInvoice.isWith) document.getElementById("isWith").checked = true;
	if(arInvoice.isVAT) document.getElementById("isVAT").checked = true;
	if(arInvoice.isNHIL) document.getElementById("isNHIL").checked = true;
}

//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(arInvoice.arInvoiceLines);
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
                    id: 'arInvoiceLineId',
                    fields: {
                        arInvoiceId: { type: 'number', defaultValue: arInvoice.arInvoiceId },
                        arInvoiceLineId: { type: 'number', editable: false },
                        inventoryItemId: { type: 'number', editable: false },
                        lineNumber: { type: 'number', editable: false },
                        description: { type: 'string', editable: false },
                        unitPrice: { type: 'number', format: "{0:c}", editable: false },
                        quantity: { type: 'number', editable: false },
                        unitOfMeasurementId: { type: 'number', editable: false },
                        discountPercentage: { type: 'number', editable: false },						
                        discountAmount: { type: 'number', editable: false },
                        totalAmount: { type: 'number', format: "{0:c}", editable: false },
                        netAmount: { type: 'number', format: "{0:c}", editable: false },
                    }, //fields
                }, //model
            }, //schema
        }, //datasource
        columns: [
			{ field: 'lineNumber', title: '#', width: 60},
            { field: 'description', title: 'Description', width: 250},
            { field: 'unitPrice', title: 'Unit Price', width: 130 },
            { field: 'quantity', title: 'Quantity', width: 130,  },
            { field: 'unitOfMeasurementId', title: 'Unit Of Measurement', width: 250 ,template: '#= getUnitOfMeasurement(unitOfMeasurementId) #'},
            { field: 'discountAmount', title: 'Discount' },
            { field: 'totalAmount', title: 'Line Total'},
            { field: 'netAmount', title: 'Line Net Total'}			
        ],
    });
}



//retrieve inventory Item value from the Grid pop and display on the Grid
function getUnitOfMeasurement(id) {
    for (i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id)
            return unitOfMeasurements[i].unitOfMeasurementName;
    }
}

function savePost() {
    saveToServer();
}

//func saveToServer
function saveToServer() {
    $.ajax({
        url: invoiceApiUrl + '/InvoicePost',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(arInvoice),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        successDialog('Invoice Successfully Posted', 'SUCCESS', function () { window.location = "/ArInvoice/Invoices/"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });

}
