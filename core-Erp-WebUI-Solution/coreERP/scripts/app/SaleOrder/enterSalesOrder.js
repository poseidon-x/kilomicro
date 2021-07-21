//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var salesOrderApiUrl = coreERPAPI_URL_Root + "/crud/salesOrder";
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var citiesApiUrl = coreERPAPI_URL_Root + "/crud/city";
var paymentTermApiUrl = coreERPAPI_URL_Root + "/crud/paymentTerm";
var shippingMethodApiUrl = coreERPAPI_URL_Root + "/crud/shippingMethod";
var countryApiUrl = coreERPAPI_URL_Root + "/crud/countries";
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/inventoryItem";
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/unitOfMeasurement";
var salesTypeApiUrl = coreERPAPI_URL_Root + "/crud/salesType";
var companyProfileApiUrl = coreERPAPI_URL_Root + "/crud/companyProfile";


//Declaration of variables to store records retrieved from the database
var salesOrder = {};
var customers = {};
var selectedCustomer = {};
var currencies = {};
var accounts = {};
var cities = {};
var paymentTerms = {};
var shippingMethods = {};
var countries = {};
var inventoryItems = {};
var unitOfMeasurements = {};
var salesTypes = {};
var companyProfile = {};

var salesTotalAmount = 0;
var localCurrencyId;
var customerCurrencyId;
var companyCurrencyId;

var companyRate = 0;
var customerRate = 0;
var itemRate = 0;



var addressTypes = [
    { addressTypeId: 1, addressTypeName: "Home" },
    { addressTypeId: 2, addressTypeName: "Mailing" },
    { addressTypeId: 3, addressTypeName: "Business" },
    { addressTypeId: 4, addressTypeName: "Legal" },
    { addressTypeId: 5, addressTypeName: "Billing" },
    { addressTypeId: 6, addressTypeName: "Other" }
];

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

var salesOrderAjax = $.ajax({
    url: salesOrderApiUrl + '/Get/' + salesOrderId,
    type: 'Get',
    contentType: 'application/json',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store customer table ajax call in it
var customerAjax = $.ajax({
    url: customerApiUrl + '/Get',
    type: 'Get',
    contentType: 'application/json',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store account table ajax call in it
var accountAjax = $.ajax({
    url: acctsApiUrl + '/GetByCategory?categoryId=1',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store cities table ajax call in it
var citiesAjax = $.ajax({
    url: citiesApiUrl + '/Get',
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

//Declare a variable and store paymentTerm table ajax call in it
var paymentTermAjax = $.ajax({
    url: paymentTermApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store shippingMethod table ajax call in it
var shippingMethodAjax = $.ajax({
    url: shippingMethodApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store country table ajax call in it
var countryAjax = $.ajax({
    url: countryApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var inventoryItemAjax = $.ajax({
    url: inventoryItemApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
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

var salesTypeAjax = $.ajax({
    url: salesTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
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
    $.when(salesOrderAjax, customerAjax, accountAjax, citiesAjax, currencyAjax, paymentTermAjax, shippingMethodAjax, countryAjax, inventoryItemAjax, unitOfMeasurementsAjax, salesTypeAjax, companyProfileAjax)
        .done(function (dataSalesOrder, dataCustomer, dataAccount, dataCities, dataCurrency, dataPaymentTerm, dataShippingMethod, dataCountry, dataInventoryItem, dataUnitOfMeasurements, dataSalesTypes, dataCompanyProfile) {
            salesOrder = dataSalesOrder[2].responseJSON;
            customers = dataCustomer[2].responseJSON;
            accounts = dataAccount[2].responseJSON;
            cities = dataCities[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
            paymentTerms = dataPaymentTerm[2].responseJSON;
            shippingMethods = dataShippingMethod[2].responseJSON;
            countries = dataCountry[2].responseJSON;
            inventoryItems = dataInventoryItem[2].responseJSON;
			unitOfMeasurements = dataUnitOfMeasurements[2].responseJSON;
			salesTypes = dataSalesTypes[2].responseJSON;
			companyProfile = dataCompanyProfile[2].responseJSON;
            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() {

    renderControls();
    renderOrderlineGrid();

    if (salesOrder.salesOrderId > 0) {
        populateUi();
    }



//Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {

        //Retrieve & save Grid data
        var salesOrderlineGridData = $("#salesOrderlineGrid").data().kendoGrid.dataSource.view();
        
        //if (!validator.validate()) {
        //    smallerWarningDialog('A form field has invalid value', 'ERROR');
        //} else {
            if (salesOrderlineGridData.length > 0 ) {
                displayLoadingDialog();
                //Empty child arrays if any
                salesOrder.salesOrderlines = [];
                
				saveSalesOrderBillings();
				saveSalesOrderShippings();				
                saveSalesOrderlineGridData(salesOrderlineGridData);
				
                saveSalesOrder();
            }else {
                smallerWarningDialog('Order Line Grid is empty', 'ERROR');
            }
        //}
    });
}


function alphaOnly(event) {
    var key = event.customerName;
    return (key >= 0);
};

function saveSalesOrderBillings() {
	if(	salesOrder.salesOrderBillings.length==0){
		salesOrder.salesOrderBillings.push({});
	}
    salesOrder.salesOrderBillings[0].billTo = $('#billTo').data('kendoMaskedTextBox').value();
    salesOrder.salesOrderBillings[0].addressLine1 = $('#addressLine1').val();
    salesOrder.salesOrderBillings[0].addressLine2 = $('#addressLine2').val();
    salesOrder.salesOrderBillings[0].cityName = $('#cityName').data('kendoComboBox').value();
}

function saveSalesOrderShippings() {
	if(	salesOrder.salesOrderShippings.length==0){
		salesOrder.salesOrderShippings.push({});
	}
	salesOrder.salesOrderShippings[0].shippingMethodId	= $('#shippingMethod').data('kendoComboBox').value();
    salesOrder.salesOrderShippings[0].shipTo  = $('#shipTo').data('kendoMaskedTextBox').value();
    salesOrder.salesOrderShippings[0].addressLine1 = $('#shippingAddressLine1').val();
    salesOrder.salesOrderShippings[0].addressLine2 = $('#shippingAddressLine2').val();
    salesOrder.salesOrderShippings[0].cityName = $('#shippingCityName').data('kendoComboBox').value();
    salesOrder.salesOrderShippings[0].countryName = $('#countryName').data('kendoComboBox').value();
	
}

function saveSalesOrderlineGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            salesOrder.salesOrderlines.push(data[i]);
			salesTotalAmount += data[i].totalAmount;
        }
    }
    else {
	salesOrder.salesOrderlines.push(data[0]);
				salesTotalAmount += data[0].totalAmount;
	}
}



//Apply kendo Style to the input fields
function renderControls() {
    $("#customer").width('75%')
    .kendoComboBox({
        autoBind: true,
        highlightFirst: true,
        suggest: true,		
        dataSource: customers,
        dataValueField: 'customerId',
        dataTextField: 'customerName',
        change: onCustomerChange,
        optionLabel: ''
    });

    $("#saleDate").width('75%')
    .kendoDatePicker({
        format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
        enable: false,
		min: new Date(), 

    });
    var saleDate = $('#saleDate').data("kendoDatePicker");


    $("#requiredDate").width('75%')
    .kendoDatePicker({
        format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
        enable: false,
		min: new Date(), // sets min date to Jan 1st, 2011
		max: new Date(new Date().setMonth(new Date().getMonth()+12))

    });
    var requiredDate = $('#requiredDate').data("kendoDatePicker");



    $("#currency").width('75%')
    .kendoComboBox({
        dataSource: currencies,
        dataValueField: 'currency_id',
        dataTextField: 'major_name',
        change: onCurrencyChange,		
        enable: false,
        optionLabel: ''
    });

    $("#salesType").width('75%')
    .kendoComboBox({
        dataSource: salesTypes,
        dataValueField: 'salesTypeID',
        dataTextField: 'salesTypeName',
        change: onSalesTypeChange,		
        enable: false,
        optionLabel: ''
    });	
	
    $("#paymentTerm").width('75%')
    .kendoComboBox({
        dataSource: paymentTerms,
        dataValueField: 'paymentTermID',
        dataTextField: 'paymentTerms',
        change: onPaymentTermChange,				
        enable: false,
        optionLabel: ''
    });

    $("#billTo").width('90%')
    .kendoMaskedTextBox({
        enable: false
    });

    $("#cityName").width('90%')
    .kendoComboBox({
        dataSource: cities,
        dataValueField: "city_name",
        dataTextField: "city_name",
        change: onCityNameChange,						
        enable: false,
        optionLabel: ' '
    });

    $("#shipTo").width('90%')
    .kendoMaskedTextBox({
        enable: false
    });

    $("#shippingMethod").width('90%')
    .kendoComboBox({
        dataSource: shippingMethods,
        dataValueField: "shippingMethodID",
        dataTextField: "shippingMethodName",
        change: onShippingMethodChange,								
        enable: false,
        optionLabel: ' '
    });

    $("#shippingCityName").width('90%')
    .kendoComboBox({
        dataSource: cities,
        dataValueField: "city_name",
        dataTextField: "city_name",
        change: onCityNameChange,								
        enable: false,
        optionLabel: ' '
    });

    $("#countryName").width('90%')
    .kendoComboBox({
        dataSource: countries,
        dataValueField: "country_name",
        dataTextField: "country_name",
        change: onCountryNameChange,								
        enable: false,
        optionLabel: ' '
    });
    
    $('#tabs').kendoTabStrip();

    $("#save").prop('disabled', true);

    dismissLoadingDialog();
    requiredDate.enable(false);


}

var onCustomerChange = function() {
    var todayDate = new Date();
    $('#saleDate').data("kendoDatePicker").value(todayDate);

    var saleDate = $('#saleDate').data("kendoDatePicker");
    var requiredDate = $('#requiredDate').data("kendoDatePicker");
    var currency = $('#currency').data("kendoComboBox");
    var paymentTerm = $('#paymentTerm').data("kendoComboBox");
    var billTo = $('#billTo').data("kendoMaskedTextBox");
    var cityName = $('#cityName').data("kendoComboBox");
    var addressLine1 = $('#addressLine1');
    var shipTo = $('#shipTo').data("kendoMaskedTextBox");
    var shippingMethod = $('#shippingMethod').data("kendoComboBox");
    var salesType = $('#salesType').data("kendoComboBox");
    var shippingCityName = $('#shippingCityName').data("kendoComboBox");
    var countryName = $('#countryName').data("kendoComboBox");
    var shippingAddressLine1 = $('#shippingAddressLine1');
    var shippingAddressLine2 = $('#shippingAddressLine2');

    var id = $("#customer").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < customers.length; i++) {
        if (customers[i].customerId == id) {
            selectedCustomer = customers[i];
            exist = true;
            //enable Controls
            requiredDate.enable(true);
            currency.enable(true);
            paymentTerm.enable(true);
            billTo.enable(true);
            cityName.enable(true);
            shipTo.enable(true);
            shippingMethod.enable(true);
            salesType.enable(true);			
            shippingCityName.enable(true);
            countryName.enable(true);

            $("#save").prop('disabled', false);
            break;
        }
    }

    if (exist) {
		customerCurrencyId = selectedCustomer.currencyId;
        currency.value(selectedCustomer.currencyId);
        paymentTerm.value(selectedCustomer.paymentTermID);
        billTo.value(selectedCustomer.customerName);
        cityName.value(selectedCustomer.customerBusinessAddresses[0].cityName);
        addressLine1.val(selectedCustomer.customerBusinessAddresses[0].addressLine);
        shipTo.value(selectedCustomer.customerShippingAddresses[0].shipTo);
        shippingMethod.value(selectedCustomer.customerShippingAddresses[0].shippingMethodId);
        shippingCityName.value(selectedCustomer.customerShippingAddresses[0].cityName);
        countryName.value(selectedCustomer.customerShippingAddresses[0].countryName);
        shippingAddressLine1.val(selectedCustomer.customerShippingAddresses[0].addressLine1);
        shippingAddressLine2.val(selectedCustomer.customerShippingAddresses[0].addressLine2);
    } else {
        warningDialog('Invalid Customer', 'ERROR');
        $("#customer").data("kendoComboBox").value("");
        dismissLoadingDialog();
    }
}

//Validate KendoComboBoxes on change
var onCurrencyChange = function() {

    var id = $("#currency").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < currencies.length; i++) {
        if (currencies[i].currency_id == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Currency', 'ERROR');
        $("#currency").data("kendoComboBox").value("");
    }else{
		if(companyProfile[0].currency_id != id){
			var localCurrencyId = companyProfile[0].currency_id;
			customerCurrencyId = id;
			for (var i = 0; i < currencies.length; i++) {
				if (currencies[i].currency_id == localCurrencyId) {
					companyRate = currencies[i].current_buy_rate;
				}
			}
			
			for (var i = 0; i < currencies.length; i++) {
				if (currencies[i].currency_id == customerCurrencyId) {
					customerRate = currencies[i].current_buy_rate;
				}
			}
		}
	}
}

var onSalesTypeChange = function() {

    var id = $("#salesType").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < salesTypes.length; i++) {
        if (salesTypes[i].salesTypeID == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Sales Type', 'ERROR');
        $("#salesType").data("kendoComboBox").value("");
    }
}

var onPaymentTermChange = function() {

    var id = $("#paymentTerm").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < paymentTerms.length; i++) {
        if (paymentTerms[i].paymentTermID == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Payment Term', 'ERROR');
        $("#paymentTerm").data("kendoComboBox").value("");
    }
}

var onCityNameChange = function() {
    var name = $("#cityName").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < cities.length; i++) {
        if (cities[i].city_name == name) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid City Name', 'ERROR');
        $("#cityName").data("kendoComboBox").value("");
    }
}

var onShippingMethodChange = function() {
    var id = $("#shippingMethod").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < shippingMethods.length; i++) {
        if (shippingMethods[i].shippingMethodID == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Shipping Method', 'ERROR');
        $("#shippingMethod").data("kendoComboBox").value("");
    }
}

var onCountryNameChange = function() {
    var name = $("#countryName").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < countries.length; i++) {
        if (countries[i].country_name == name) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Country', 'ERROR');
        $("#countryName").data("kendoComboBox").value("");
    }
}


//render Grid
var linNum = 1;
var nextLin;
var updateNextLin;
var description;
var itemUnitPrice;
var discountPercentage;
var totalAmount;
var discountAmount;
function renderOrderlineGrid() {

    $('#salesOrderlineGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(salesOrder.salesOrderlines);
                },
                create: function (entries) {
                    var data = entries.data;
     					data.description = description;
						data.unitPrice = itemUnitPrice;
						data.discountPercentage = discountPercentage;
						data.totalAmount = itemUnitPrice * data.quantity;
						data.discountAmount = (data.discountPercentage / 100) * data.quantity;
						data.netAmount = data.totalAmount - data.discountAmount;                    
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
                    id: 'salesOrderLineId',
                    fields: {
                        salesOrderId: { type: 'number', defaultValue: salesOrder.salesOrderId },
                        salesOrderLineId: { type: 'number', editable: false },
                        inventoryItemId: {validation: { required: true } },
                        description: { type: 'string', editable: false },
                        unitPrice: { type: 'number', editable: false },
                        unitOfMeasurementId: {validation: { required: true }},						
                        quantity: { type: 'number' },
                        discountPercentage: { type: 'number', editable: false  },
                        discountAmount: { type: 'number' },
                        totalAmount: { type: 'number', editable: false  },
                        netAmount: { type: 'number', editable: false  },
                        discountRate: { type: 'number', editable: false  }
						
                    } //fields
                }, //model
            } //schema
        }, //datasource
        editable: 'popup',
        sortable: 
				{
                    mode: "multiple",
                    allowUnsort: true
                },
        reorderable: true,		
		pageable: {
                            buttonCount: 5
                        },
		scrollable: false,				
        columns: [
            { field: 'inventoryItemId', title: 'Item', editor: inventoryItemEditor, template: '#= getInventoryItem(inventoryItemId) #' },
            { field: 'unitOfMeasurementId', title: 'Measurement Unit', editor: unitOfMeasurementEditor, template: '#= getUnitOfMeasurement(unitOfMeasurementId) #' },
            { field: 'quantity', title: 'Quantity' },
			{ field: 'description', title: 'Item Description' },
            { field: 'unitPrice', title: 'Unit Price', format: "{0:0.00}" },
            { field: 'discountPercentage', title: 'Discount (%)' },
            { field: 'totalAmount', title: 'Total Amount', format: "{0:0.00}" },
            { field: 'netAmount', title: 'Net Total', format: "{0:0.00}" },
            {
                command: ['edit', 'destroy']
            }
        ],
        toolbar: [{ name: 'create', text: 'Add Order Line' }]

    });
}




//retrieve values from from Input Fields and save 
function saveSalesOrder() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {	
	salesOrder.customerId = $("#customer").data('kendoComboBox').value();
    salesOrder.customerName = $("#customer").data('kendoComboBox').text();
    salesOrder.salesDate = $('#saleDate').data('kendoDatePicker').value();
    salesOrder.currencyId = $('#currency').data('kendoComboBox').value();
    salesOrder.requiredDate = $('#requiredDate').data('kendoDatePicker').value();
    salesOrder.shippedDate = $('#shippedDate').data('kendoDatePicker').value();
    salesOrder.totalAmount = salesTotalAmount;
    salesOrder.paymentTermId = $('#paymentTerm').data('kendoComboBox').value();
    salesOrder.salesTypeId = $('#salesType').data('kendoComboBox').value();	
	}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: salesOrderApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(salesOrder),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Sales Order Successfuly Saved', 'SUCCESS', function () { window.location = "/SalesOrder/salesOrders/"; });

    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer


function inventoryItemEditor(container, options) {
    $('<input type="text" id="inventoryItem" data-bind="value:' + options.field + '" required="required" />')
    .appendTo(container)
    .kendoComboBox({
        dataSource: inventoryItems,
        dataValueField: "inventoryItemId",
        dataTextField: "inventoryItemName",
        change: onInventoryItemChange,
        optionLabel: '------Select Item------'
    });
}

var onInventoryItemChange = function (e) {        
	var id = $("#inventoryItem").data("kendoComboBox").value();
	var exist = false;
	
	    for (var i = 0; i < inventoryItems.length; i++) {
        if (inventoryItems[i].inventoryItemId == id) {
            exist = true;
            break;
        }
    }

	if (exist) {
			var cusPaymentTerm = $("#paymentTerm").data("kendoComboBox").value();
			var unitOfMeasDropDown = $("#unitOfMeasurement").data("kendoComboBox");
			//var customerRate;

				for(var i = 0; i < inventoryItems.length; i++){
					if(inventoryItems[i].inventoryItemId == id){
					alert(inventoryItems[i].inventoryItemName);
					var itemCurrencyId = inventoryItems[i].currencyId;
					var itemPrice = inventoryItems[i].unitPrice;

					if(itemCurrencyId != customerCurrencyId){
					
					var companyUnitPrice = 0;
					var localPrice = 0;
						for (var i = 0; i < currencies.length; i++) {
							if (currencies[i].currency_id == itemCurrencyId) {
								alert(currencies[i].major_name);
								localPrice = itemPrice / currencies[i].current_buy_rate;								
								companyUnitPrice = localPrice * companyRate;
								
								alert('unit price: ' + itemPrice);						
								alert('Local unit price: ' + localPrice);
								alert('unit price in customers currency: ' +  companyUnitPrice);
								break;
							}
						} 

					}else 
					{
						itemUnitPrice = inventoryItems[i].unitPrice;
					}
						
						description = inventoryItems[i].inventoryItemName
						break;
					}
				}
				for(var i = 0; i < paymentTerms.length; i++){
					if(paymentTerms[i].paymentTermID == cusPaymentTerm){
						discountPercentage = paymentTerms[i].discountIfBeforeDays;
						break;
					}
				}
	}else{
	warningDialog('Invalid Item', 'ERROR');
	$("#inventoryItem").data("kendoComboBox").value("");	
	} 
}

//prepare unit of Measurement Drop Down for shrinkage grid 
function unitOfMeasurementEditor(container, options) {
    $('<input type="text" id="unitOfMeasurement" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoComboBox({
        dataSource: unitOfMeasurements,
        dataValueField: "unitOfMeasurementId",
        dataTextField: "unitOfMeasurementName",
        change: onUnitOfMeasChange,								
        optionLabel: '------Select Unit------'
    });
}

var onUnitOfMeasChange = function() {
    var id = $("#unitOfMeasurement").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Unit Of Measurement', 'ERROR');
        $("#unitOfMeasurement").data("kendoComboBox").value("");
    }
}

function getAccountIds(data) {
    if (data.length >= 1) {
        for (var i = 0; i < data.length; i++) {
            accountIds.push(JSON.stringify(data[i].acct_id));
        }
    }
}

//retrieve unit of Measurement value from the Grid pop and display on the Grid
function getshippingMethod(id) {
    for (i = 0; i < shippingMethods.length; i++) {
        if (shippingMethods[i].shippingMethodID == id)
            return shippingMethods[i].shippingMethodName;
    }
}

function reAssignLineNumbers(data){
    var gridData = $("#salesOrderlineGrid").data().kendoGrid.dataSource.view();
    alert(gridData.length);

    if (gridData.length > 1) {
        for (var i = 0; i < gridData.length; i++) {
            gridData[i].lineNumber = i+1;
        }
    } else gridData[0].lineNumber = 1;

}

function getInventoryItem(id) {
    for (i = 0; i < inventoryItems.length; i++) {
        if (inventoryItems[i].inventoryItemId == id)
            return inventoryItems[i].inventoryItemName;
    }
}

//retrieve unit of Measurement value from the Grid pop and display on the Grid
function getUnitOfMeasurement(id) {
    for (i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id)
            return unitOfMeasurements[i].unitOfMeasurementName;
    }
}








