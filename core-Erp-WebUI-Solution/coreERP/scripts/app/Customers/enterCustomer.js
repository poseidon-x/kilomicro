//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var citiesApiUrl = coreERPAPI_URL_Root + "/crud/city";
var paymentTermApiUrl = coreERPAPI_URL_Root + "/crud/paymentTerm";
var shippingMethodApiUrl = coreERPAPI_URL_Root + "/crud/shippingMethod";
var countryApiUrl = coreERPAPI_URL_Root + "/crud/countries";



//Declaration of variables to store records retrieved from the database
var customers = {};
var currencies = {};
var accounts = {};
var cities = {};
var paymentTerms = {};
var shippingMethods = {};
var countries = {};

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

//Declare a variable and store customer table ajax call in it
var customerAjax = $.ajax({
    url: customerApiUrl + '/Get/' + customerId,
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

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(customerAjax, accountAjax, citiesAjax, currencyAjax, paymentTermAjax, shippingMethodAjax, countryAjax)
        .done(function (dataCustomer, dataAccount, dataCities, dataCurrency, dataPaymentTerm, dataShippingMethod, dataCountry) {
            customers = dataCustomer[2].responseJSON;
            accounts = dataAccount[2].responseJSON;
            cities = dataCities[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
            paymentTerms = dataPaymentTerm[2].responseJSON;
            shippingMethods = dataShippingMethod[2].responseJSON;
            countries = dataCountry[2].responseJSON;
            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() {

    //If customerId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (customers.customerId > 0) {
        renderControls();
        populateUi();
        dismissLoadingDialog();
    } else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        renderControls();
        dismissLoadingDialog();
    }

//Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {

        var validator = $("#myform").kendoValidator().data("kendoValidator");

        if (!validator.validate() ) {
            smallerWarningDialog('A form drop down field has invalid value', 'ERROR');
        } else {
            //if (businessAddressData.length > 0 && contactPersonData.length > 0 && emailData.length > 0
            //   && phoneData.length > 0 && shippingAddressData.length > 0) {
               displayLoadingDialog();

                saveBusinessAddressData();
                saveContactPersonData();
                saveEmailData();
                savePhoneData();
                saveShippingAddressData();

                saveCustomer();
            //}
        }
    });
}



function alphaOnly(event) {
    var key = event.customerName;
    return (key >= 0);
};


function saveBusinessAddressData() {
	if(	customers.customerBusinessAddresses.length == 0){
		customers.customerBusinessAddresses.push({});
	}
    customers.customerBusinessAddresses[0].customerBusinessAddressId = customers.customerBusinessAddresses[0].customerBusinessAddressId;	
    customers.customerBusinessAddresses[0].addressTypeId = $('#addressType').data('kendoComboBox').value();
    customers.customerBusinessAddresses[0].addressLine = $('#addressLine').val();
    customers.customerBusinessAddresses[0].landmark = $('#landmark').data('kendoMaskedTextBox').value();
    customers.customerBusinessAddresses[0].cityName = $('#cityName').data('kendoComboBox').value();
    customers.customerBusinessAddresses[0].countryName = $('#countryName').data('kendoComboBox').value();
}

function saveContactPersonData() {
	if(	customers.customerContactPersons.length==0){
		customers.customerContactPersons.push({});
	}
    customers.customerContactPersons[0].customerContactPersonId = customers.customerContactPersons[0].customerContactPersonId;	
    customers.customerContactPersons[0].contactPersonName = $('#contactPersonName').data('kendoMaskedTextBox').value();
    customers.customerContactPersons[0].jobTitle = $('#jobTitle').data('kendoMaskedTextBox').value();
    customers.customerContactPersons[0].mobilePhoneNumber = $('#mobilePhoneNumber').data('kendoMaskedTextBox').raw();
    customers.customerContactPersons[0].landlinePhoneNumber = $('#landlinePhoneNumber').data('kendoMaskedTextBox').raw();
    customers.customerContactPersons[0].officeExtension = $('#officeExtension').data('kendoMaskedTextBox').raw();
    customers.customerContactPersons[0].emailAddress = $('#emailAddress').data('kendoMaskedTextBox').value();
    customers.customerContactPersons[0].skypeId = $('#skypeId').data('kendoMaskedTextBox').value();
}

function saveEmailData() {
	if(	customers.customerEmails.length==0){
		customers.customerEmails.push({});
	}
    customers.customerEmails[0].customerEmailId = customers.customerEmails[0].customerEmailId;	
    customers.customerEmails[0].emailAddress1 = $('#emailAddress1').data('kendoMaskedTextBox').value();
    customers.customerEmails[0].emailAddress2 = $('#emailAddress2').data('kendoMaskedTextBox').value();
    customers.customerEmails[0].emailAddress3 = $('#emailAddress3').data('kendoMaskedTextBox').value();
}

function savePhoneData() {
	if(	customers.customerPhones.length==0){
		customers.customerPhones.push({});
	}
    customers.customerPhones[0].customerPhoneId = customers.customerPhones[0].customerPhoneId;	
    customers.customerPhones[0].mobilePhoneNumber = $('#contactMobilePhoneNumber').data('kendoMaskedTextBox').raw();
    customers.customerPhones[0].landlinePhoneNumber = $('#contactLandlinePhoneNumber').data('kendoMaskedTextBox').raw();
    customers.customerPhones[0].faxNumber = $('#faxNumber').data('kendoMaskedTextBox').raw();
}

function saveShippingAddressData() {
	if(	customers.customerShippingAddresses.length==0){
		customers.customerShippingAddresses.push({});
	}
    customers.customerShippingAddresses[0].customerShippingAddressId = customers.customerShippingAddresses[0].customerShippingAddressId;	
    customers.customerShippingAddresses[0].shipTo = $('#shipTo').data('kendoMaskedTextBox').value();
    customers.customerShippingAddresses[0].shippingMethodId = $('#shippingMethod').data('kendoComboBox').value();
    customers.customerShippingAddresses[0].addressLine1 = $('#shippingAddressLine1').val();
    customers.customerShippingAddresses[0].addressLine2 = $('#shippingAddressLine2').val();
    customers.customerShippingAddresses[0].cityName = $('#shippingCity').data('kendoComboBox').value();
    customers.customerShippingAddresses[0].countryName = $('#shippingCountry').data('kendoComboBox').value();

	}

//Apply kendo Style to the input fields
function renderControls() {
    $("#customerName").width('65%')
        .kendoMaskedTextBox();

    $("#paymentTermID").width('65%')
    .kendoComboBox({
        dataSource: paymentTerms,
        dataValueField: 'paymentTermID',
        dataTextField: 'paymentTerms',
	    change: onPaymentTermChange,			
        optionLabel: '',
    });

    $("#currency").width('65%')
    .kendoDropDownList({
        dataSource: currencies,
        dataValueField: "currency_id",
        dataTextField: "major_name",
    });

    $("#glAccount").width('65%')
    .kendoComboBox({
        dataSource: accounts,
        dataValueField: "acct_id",
        dataTextField: "acc_name",
	    change: onGlAccountChange,					
        optionLabel: ''
    });

    $("#location").width('65%')
    .kendoComboBox({
        dataSource: cities,
        dataValueField: 'city_id',
        dataTextField: 'city_name',
	    change: onLocationChange,					
        optionLabel: ''
    });

    $("#addressType").width('65%')
    .kendoComboBox({
        dataSource: addressTypes,
        dataValueField: "addressTypeId",
        dataTextField: "addressTypeName",
	    change: onAddressTypeChange,					
        optionLabel: ' '
    });

    $("#landmark").width('65%')
        .kendoMaskedTextBox();

    $("#cityName").width('65%')
    .kendoComboBox({
        dataSource: cities,
        dataValueField: "city_name",
        dataTextField: "city_name",
	    change: onCityNameChange,					
        optionLabel: ' '
    });

    $("#countryName").width('65%')
    .kendoComboBox({
        dataSource: countries,
        dataValueField: "country_name",
        dataTextField: "country_name",
	    change: onCountryNameChange,							
        optionLabel: ' '
    });

    $("#contactPersonName").width('65%')
        .kendoMaskedTextBox();
    
    $("#jobTitle").width('65%')
        .kendoMaskedTextBox();

    $("#mobilePhoneNumber").width('65%')
        .kendoMaskedTextBox({
		mask: "(000) 000 0000"								
		});

    $("#landlinePhoneNumber").width('65%')
        .kendoMaskedTextBox({
		mask: "(000) 000 0000"								
		});

    $("#officeExtension").width('65%')
        .kendoMaskedTextBox({
		mask: "(0000)"								
		});
    
    $("#emailAddress").width('65%')
        .kendoMaskedTextBox();

    $("#skypeId").width('65%')
        .kendoMaskedTextBox();

    $("#emailAddress1").width('65%')
        .kendoMaskedTextBox();
 
    $("#emailAddress2").width('65%')
    .kendoMaskedTextBox();

    $("#emailAddress3").width('65%')
        .kendoMaskedTextBox();
    
    $("#contactMobilePhoneNumber").width('65%')
        .kendoMaskedTextBox({
		mask: "(000) 000 0000"				
		});

    $("#contactLandlinePhoneNumber").width('65%')
        .kendoMaskedTextBox({
		mask: "(000) 000 0000"				
		});

    $("#faxNumber").width('65%')
        .kendoMaskedTextBox({
		mask: "(0000) 000 0000"								
		});

    $("#shipTo").width('65%')
        .kendoMaskedTextBox();

    $("#shippingMethod").width('65%')
    .kendoComboBox({
        dataSource: shippingMethods,
        dataValueField: "shippingMethodID",
        dataTextField: "shippingMethodName",
	    change: onShippingMethodChange,							
        optionLabel: ' '
    });
	
    $("#shippingCity").width('65%')
    .kendoComboBox({
        dataSource: cities,
        dataValueField: "city_name",
        dataTextField: "city_name",
	    change: onShippingCityChange,							
        optionLabel: ' '
    });	

    $("#shippingCountry").width('65%')
    .kendoComboBox({
        dataSource: countries,
        dataValueField: "country_name",
        dataTextField: "country_name",
	    change: onShippingCountryChange,							
        optionLabel: ' '
    });	
		
    $('#tabs').kendoTabStrip();
}

//Validate KendoComboBoxes on change
var onPaymentTermChange = function() {
    var id = $("#paymentTermID").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < paymentTerms.length; i++) {
        if (paymentTerms[i].paymentTermID == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Mode of Payment', 'ERROR');
        $("#paymentTermID").data("kendoComboBox").value("");
    }
}

//Validate KendoComboBoxes on change
var onGlAccountChange = function() {
    var id = $("#glAccount").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < accounts.length; i++) {
        if (accounts[i].acct_id == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Account', 'ERROR');
        $("#glAccount").data("kendoComboBox").value("");
    }
}

//Validate KendoComboBoxes on change
var onLocationChange = function() {
    var id = $("#location").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < cities.length; i++) {
        if (cities[i].city_id == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid City', 'ERROR');
        $("#location").data("kendoComboBox").value("");
    }
}

//Validate KendoComboBoxes on change
var onAddressTypeChange = function() {
    var id = $("#addressType").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < addressTypes.length; i++) {
        if (addressTypes[i].addressTypeId == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Address Type', 'ERROR');
        $("#addressType").data("kendoComboBox").value("");
    }
}

//Validate KendoComboBoxes on change
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
        warningDialog('Invalid City', 'ERROR');
        $("#cityName").data("kendoComboBox").value("");
    }
}

//Validate KendoComboBoxes on change
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

//Validate KendoComboBoxes on change
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

//Validate KendoComboBoxes on change
var onShippingCityChange = function() {
    var name = $("#shippingCity").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < cities.length; i++) {
        if (cities[i].city_name == name) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid City', 'ERROR');
        $("#shippingCity").data("kendoComboBox").value("");
    }
}

//Validate KendoComboBoxes on change
var onShippingCountryChange = function() {
    var name = $("#shippingCountry").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < countries.length; i++) {
        if (countries[i].country_name == name) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Country', 'ERROR');
        $("#shippingCountry").data("kendoComboBox").value("");
    }
}

//Populate the input fields for an update
function populateUi() {
    $('#customerName').data('kendoMaskedTextBox').value(customers.customerName);
    $('#paymentTermID').data('kendoComboBox').value(customers.paymentTermID);
    $('#currency').data('kendoDropDownList').value(customers.currencyId);
    $('#glAccount').data('kendoComboBox').value(customers.glAccountId);
    $('#location').data('kendoComboBox').value(customers.locationId);
	
	$('#addressType').data('kendoComboBox').value(customers.customerBusinessAddresses[0].addressTypeId);
	$('#addressLine').val(customers.customerBusinessAddresses[0].addressLine);
	$('#landmark').data('kendoMaskedTextBox').value(customers.customerBusinessAddresses[0].landmark);
	$('#cityName').data('kendoComboBox').value(customers.customerBusinessAddresses[0].cityName);
	$('#countryName').data('kendoComboBox').value(customers.customerBusinessAddresses[0].countryName);
	
	$('#contactPersonName').data('kendoMaskedTextBox').value(customers.customerContactPersons[0].contactPersonName);
	$('#jobTitle').data('kendoMaskedTextBox').value(customers.customerContactPersons[0].jobTitle);
	$('#mobilePhoneNumber').data('kendoMaskedTextBox').value(customers.customerContactPersons[0].mobilePhoneNumber);
	$('#landlinePhoneNumber').data('kendoMaskedTextBox').value(customers.customerContactPersons[0].landlinePhoneNumber);
	$('#officeExtension').data('kendoMaskedTextBox').value(customers.customerContactPersons[0].officeExtension);
	$('#emailAddress').data('kendoMaskedTextBox').value(customers.customerContactPersons[0].emailAddress);
	$('#skypeId').data('kendoMaskedTextBox').value(customers.customerContactPersons[0].skypeId);
	
	$('#emailAddress1').data('kendoMaskedTextBox').value(customers.customerEmails[0].emailAddress1);
	$('#emailAddress2').data('kendoMaskedTextBox').value(customers.customerEmails[0].emailAddress2);
	$('#emailAddress3').data('kendoMaskedTextBox').value(customers.customerEmails[0].emailAddress3);
	
	$('#contactMobilePhoneNumber').data('kendoMaskedTextBox').value(customers.customerPhones[0].mobilePhoneNumber);
	$('#contactLandlinePhoneNumber').data('kendoMaskedTextBox').value(customers.customerPhones[0].landlinePhoneNumber);
	$('#faxNumber').data('kendoMaskedTextBox').value(customers.customerPhones[0].faxNumber);
	
	$('#shipTo').data('kendoMaskedTextBox').value(customers.customerShippingAddresses[0].shipTo);
	$('#shippingMethod').data('kendoComboBox').value(customers.customerShippingAddresses[0].shippingMethodId);
	$('#shippingAddressLine1').val(customers.customerShippingAddresses[0].addressLine1);
	$('#shippingAddressLine2').val(customers.customerShippingAddresses[0].addressLine2);
	$('#shippingCity').data('kendoComboBox').value(customers.customerShippingAddresses[0].cityName);
	$('#shippingCountry').data('kendoComboBox').value(customers.customerShippingAddresses[0].countryName);
}




//retrieve values from from Input Fields and save 
function saveCustomer() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    customers.customerName = $("#customerName").data('kendoMaskedTextBox').value();
    customers.paymentTermID = $('#paymentTermID').data('kendoComboBox').value();
    customers.currencyId = $('#currency').data('kendoDropDownList').value();
    customers.glAccountId = $('#glAccount').data('kendoComboBox').value();
    customers.locationId = $('#location').data('kendoComboBox').value();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: customerApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(customers),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Customer Saved Successfuly', 'SUCCESS', function () { window.location = "/Customers/Customers/"; });

    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer









