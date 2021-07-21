/*
Creator: damien@acsghana.com
*/

//Declaration of variables to API controllers
var authToken = agencyAPI_Token;
var clientApiUrl = agencyAPI_URL_Root + "/Client";
var maritalStatusApiUrl = agencyAPI_URL_Root + "/MaritalStatus";
var idTypeApiUrl = agencyAPI_URL_Root + "/IdType";
var clientTypeApiUrl = agencyAPI_URL_Root + "/ClientType";
var clientCategoryApiUrl = agencyAPI_URL_Root + "/ClientCategory";
var industryApiUrl = agencyAPI_URL_Root + "/Industry";
var industryApiUrl = agencyAPI_URL_Root + "/Industry";
var branchApiUrl = agencyAPI_URL_Root + "/Branch";
var addressTypeApiUrl = agencyAPI_URL_Root + "/AddressType";
var phoneTypeApiUrl = agencyAPI_URL_Root + "/PhoneType";
var emailTypeApiUrl = agencyAPI_URL_Root + "/EmailType";
var sectorApiUrl = agencyAPI_URL_Root + "/sector";

//Declaration of variables to store records retrieved from the database
var clientModel = {};
var client = {};
var clientAddress = {};
var clientPhone = {};
var clientEmail = {};
var maritalStatus = {};
var idTypes = {};
var clientTypes = {};
var clientCategories = {};
var industries = {};
var branches = {};
var addressTypes = {};
var phoneTypes = {};
var emailTypes = {};
var sectors = {};

var gender = [
		{id: 'F', name:'Female'},
		{id: 'M', name: 'Male'}
	];


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//Declare a variable and store client table ajax call in it
var clientAjax = $.ajax({
    url: clientApiUrl + '/Get/' + clientId,
    type: 'Get',
    contentType: 'application/json',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store marital status table ajax call in it
var maritalStatusAjax = $.ajax({
    url: maritalStatusApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store id type table ajax call in it
var idTypeAjax = $.ajax({
    url: idTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store client type table ajax call in it
var clientTypeAjax = $.ajax({
    url: clientTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store client category table ajax call in it
var clientCategoryAjax = $.ajax({
    url: clientCategoryApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store industry table ajax call in it
var industryAjax = $.ajax({
    url: industryApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store branch table ajax call in it
var branchAjax = $.ajax({
    url: branchApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store address type table ajax call in it
var addressTypeAjax = $.ajax({
    url: addressTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store phone type table ajax call in it
var phoneTypeAjax = $.ajax({
    url: phoneTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store email type table ajax call in it
var emailTypeAjax = $.ajax({
    url: emailTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});

//Declare a variable and store email type table ajax call in it
var sectorAjax = $.ajax({
    url: sectorApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
    }
});



//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(clientAjax, maritalStatusAjax, idTypeAjax, clientTypeAjax, clientCategoryAjax, industryAjax, 
		branchAjax, addressTypeAjax, phoneTypeAjax, emailTypeAjax,sectorAjax)
        .done(function (dataClient, dataMaritalStatus, dataIdType, dataClientType, dataClientCategory, dataIndustry, 
			dataBranch, dataAddressType, dataPhoneType, dataEmailType, dataSector) {
            client = dataClient[2].responseJSON;
            maritalStatus = dataMaritalStatus[2].responseJSON;
            idTypes = dataIdType[2].responseJSON;
            clientTypes = dataClientType[2].responseJSON;
            clientCategories = dataClientCategory[2].responseJSON;
            industries = dataIndustry[2].responseJSON;
            branches = dataBranch[2].responseJSON;
            addressTypes = dataAddressType[2].responseJSON;
            phoneTypes = dataPhoneType[2].responseJSON;
            emailTypes = dataEmailType[2].responseJSON;
			sectors = dataSector[2].responseJSON;
			
            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() {

    //If clientID > 0, Its an Update/Put, Hence render UI with retrieved existing data
	
	if (client.clientID > 0) {
        renderControls();
        //populateUI();
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
            alert('Ooops! You seem to have left out some fields.\nCheck your form and Submit Again.');
        } else {
            displayLoadingDialog();
            saveClient();
        }
    });
}

//Apply kendo Style to the input fields
function renderControls() {
    $("#surName").width('100%').kendoMaskedTextBox();
    $("#otherNames").width('100%').kendoMaskedTextBox();
    $("#sex").width('100%').kendoComboBox({
		dataSource: gender,
        dataValueField: 'id',
        dataTextField: 'name',		
        optionLabel: '',
	});
    $("#maritalStatus").width('100%').kendoComboBox({
		dataSource: maritalStatus,
        dataValueField: 'maritalStatusID',
        dataTextField: 'maritalStatusName',		
        optionLabel: '',
	});
    $("#DOB").width('100%').kendoDatePicker({
	    format: '{0:dd-MMM-yyyy}',
	    parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$("#branch").width('100%').kendoComboBox({
		dataSource: branches,
        dataValueField: 'branchID',
        dataTextField: 'branchName',		
        optionLabel: '',
	});
	$("#idNo").width('100%').kendoMaskedTextBox();
	$("#idType").width('100%').kendoComboBox({
		dataSource: idTypes,
        dataValueField: 'idNoTypeID',
        dataTextField: 'idNoTypeName',		
        optionLabel: '',
	});
	$("#expriryDate").width('100%').kendoDatePicker({
	    format: '{0:dd-MMM-yyyy}',
	    parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$("#clientType").width('100%').kendoComboBox({
		dataSource: clientTypes,
        dataValueField: 'clientTypeId',
        dataTextField: 'clientTypeName',		
        optionLabel: '',
	});
	$("#clientCategory").width('100%').kendoComboBox({
		dataSource: clientCategories,
        dataValueField: 'categoryID',
        dataTextField: 'categoryName',		
        optionLabel: '',
	});
	$("#addressType").width('100%').kendoComboBox({
		dataSource: addressTypes,
        dataValueField: 'addressTypeID',
        dataTextField: 'addressTypeName',		
        optionLabel: '',
	});
	$("#cityTown").width('100%').kendoMaskedTextBox();
	$("#addressLine").width('100%');
	$("#phoneType").width('100%').kendoComboBox({
		dataSource: phoneTypes,
        dataValueField: 'phoneTypeID',
        dataTextField: 'phoneTypeName',		
        optionLabel: '',
	});
	$("#phoneNo").width('100%').kendoMaskedTextBox();
	$("#emailType").width('100%').kendoComboBox({
		dataSource: emailTypes,
        dataValueField: 'emailTypeID',
        dataTextField: 'emailTypeName',		
        optionLabel: '',
	});
	$("#emailAddress").width('100%').kendoMaskedTextBox();

    $('#tabs').kendoTabStrip();
}


//Populate the input fields for an update
function populateUI() {
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
function saveClient() {
    retrieveValues();
	
	clientModel.client = client;
	clientModel.clientAddress = clientAddress;
	clientModel.clientPhone = clientPhone;
	clientModel.clientEmail = clientEmail;
    
	saveToServer();
}

function saveClientData() {
	client.surName = $("#surName").data('kendoMaskedTextBox').value();
	client.otherNames = $("#otherNames").data('kendoMaskedTextBox').value();
    client.sex = $("#sex").data('kendoComboBox').value();
    client.maritalStatusID = $("#maritalStatus").data('kendoComboBox').value();
    client.DOB = $("#DOB").data('kendoDatePicker').value();
	client.branchID = $("#branch").data('kendoComboBox').value();
	client.idNo = $("#idNo").data('kendoMaskedTextBox').value();
	client.idNoTypeID = $("#idType").data('kendoComboBox').value();
	client.expriryDate = $("#expriryDate").data('kendoDatePicker').value();
	client.clientTypeID = $("#clientType").data('kendoComboBox').value();
	client.categoryID = $("#clientCategory").data('kendoComboBox').value();
}

function saveClientAddress() {
	clientAddress.addressTypeID = $("#addressType").data('kendoComboBox').value();
	clientAddress.addressLine1 = $("#addressLine").val();
	clientAddress.cityTown = $("#cityTown").data('kendoMaskedTextBox').value();
	
}

function saveClientPhone() {
	clientPhone.phoneTypeID = $("#phoneType").data('kendoComboBox').value();
	clientPhone.phoneNo = $("#phoneNo").data('kendoMaskedTextBox').value();
}

function saveClientEmail() {
	clientEmail.emailTypeID = $("#emailType").data('kendoComboBox').value();
	clientEmail.emailAddress = $("#emailAddress").data('kendoMaskedTextBox').value();
}

function retrieveValues() {
    
	//client data
	saveClientData();	
	
	//address data
	saveClientAddress();
	
	//phone data
	saveClientPhone();
	
	//email data
	saveClientEmail();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(clientModel),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + agencyAPI_Token);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        alert('Client Saved Successfuly','Success', function () { window.location = "/Agent/Dashboard/"; });
        //successDialog('Client Saved Successfuly', 'SUCCESS', function () { window.location = "/Agent/Dashboard/"; });

    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer









