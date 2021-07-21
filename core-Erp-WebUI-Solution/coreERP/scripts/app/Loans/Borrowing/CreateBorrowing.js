//***********************************************************//
//  	       BORROWING - JAVASCRIPT                
// 			 CREATOR: EMMANUEL OWUSU(MAN)    	   
//		      WEEK: AUG(3RD - 7TH), 2015  		  
//*********************************************************//


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var borrowingApiUrl = coreERPAPI_URL_Root + "/crud/Borrowing";
var loanClientApiUrl = coreERPAPI_URL_Root + "/crud/LoanClient";
var borrowingTypeApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingType";
var tenureTypeApiUrl = coreERPAPI_URL_Root + "/crud/TenureType";
var interestTypeApiUrl = coreERPAPI_URL_Root + "/crud/interestType";
var repaymentModeApiUrl = coreERPAPI_URL_Root + "/crud/repaymentMode";


//Declaration of variables to store records retrieved from the database
var borrowing = {};
var clients = {};
var borrowingTypes = {};
var tenureTypes = {};
var interestTypes = {};
var repaymentModes = {};


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

var borrowingAjax = $.ajax({
    url: borrowingApiUrl + '/Get/' + borrowingId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var clientAjax = $.ajax({
    url: loanClientApiUrl + '/GetAllLoanClients',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var borrowingTypeAjax = $.ajax({
    url: borrowingTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var tenureTypeAjax = $.ajax({
    url: tenureTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var interestTypeAjax = $.ajax({
    url: interestTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var repaymentModeAjax = $.ajax({
    url: repaymentModeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(borrowingAjax, clientAjax, borrowingTypeAjax, tenureTypeAjax, interestTypeAjax, repaymentModeAjax)
        .done(function (dataBorrowing, dataClient, dataBorrowingType, dataTenureType, dataInterestType, dataRepaymentMode) {
            borrowing = dataBorrowing[2].responseJSON;
			clients = dataClient[2].responseJSON;
            borrowingTypes = dataBorrowingType[2].responseJSON;
			tenureTypes = dataTenureType[2].responseJSON;
            interestTypes = dataInterestType[2].responseJSON;
            repaymentModes = dataRepaymentMode[2].responseJSON;
			
            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() 
{		
    //If arInvoiceId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (borrowing.borrowingId > 0) {
        renderControls();
        //populateUi();
        dismissLoadingDialog();
    } else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        renderControls();
        dismissLoadingDialog();
    }

	//Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {
	
        var validator = $("#myform").kendoValidator().data("kendoValidator");
		
        if (!validator.validate()) {
            smallerWarningDialog('One or More Fields are Empty', 'ERROR');
        } else {
			displayLoadingDialog();
			//Retrieve & save Grid data
			saveBorrowing();
		}
	});
}

//Apply kendo Style to the input fields
function renderControls() {
    $("#client").width("100%")
		.kendoComboBox({
			dataSource: clients,			
		    filter: "contains",
		    suggest: true,
			dataValueField: "clientID",
			dataTextField: "clientName",
			change: onClientChange,		
			optionLabel: ""
	});
		
    $("#applicationDate").width('100%')
		.kendoDatePicker({
			format: 'dd-MMM-yyyy',
			parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
			value: 	new Date(),
			max: new Date()
    });
	
    $("#borrowingTenure").width('100%')
		.kendoNumericTextBox({
			min: 1,
			format: "0 ''"
    });
	
    $("#tenureType").width("100%")
		.kendoComboBox({
			dataSource: tenureTypes,			
		    filter: "contains",
		    suggest: true,
			dataValueField: "tenureTypeID",
			dataTextField: "tenureTypeName",
			change: onTenureTypeChange,		
			optionLabel: ""
	});
	
    $("#borrowingType").width("100%")
		.kendoComboBox({
			dataSource: borrowingTypes,			
		    filter: "contains",
		    suggest: true,
			dataValueField: "borrowingTypeId",
			dataTextField: "borrowingTypeName",
			change: onBorrowingTypeChange,		
			optionLabel: ""
	});
	
    $("#interestType").width("100%")
		.kendoComboBox({
			dataSource: interestTypes,			
		    filter: "contains",
		    suggest: true,
			dataValueField: "interestTypeID",
			dataTextField: "interestTypeName",
			//change: onBorrowingTypeChange,		
			optionLabel: ""
	});
	
    $("#amountRequested").width('100%')
		.kendoNumericTextBox({
			min: 1
    });
	
    $("#interestRate").width('100%')
		.kendoNumericTextBox({
			min: 1,
			format: "0 \\%"
    });
	
    $("#repaymentMode").width('100%')
		.kendoComboBox({
			dataSource: repaymentModes,			
		    filter: "contains",
		    suggest: true,
			dataValueField: "repaymentModeID",
			dataTextField: "repaymentModeName",
			//change: onTenureTypeChange,		
			optionLabel: ""
	});
	
    $("#applicationFee").width('100%')
		.kendoNumericTextBox({
			min: 1
    });
	
    $("#processingFee").width('100%')
		.kendoNumericTextBox({
			min: 1
    });
	
    $("#commission").width('100%')
		.kendoNumericTextBox({
			min: 1
    });
}

var onClientChange = function() {
	var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}

var onTenureTypeChange = function() {
	var id = $("#tenureType").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < tenureTypes.length; i++) {
        if (tenureTypes[i].tenureTypeID == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Tenure Type', 'ERROR');
        $("#tenureType").data("kendoComboBox").value("");
    }
}

var onBorrowingTypeChange = function() {
	var id = $("#borrowingType").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < borrowingTypes.length; i++) {
        if (borrowingTypes[i].borrowingTypeId == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Borrowing Type', 'ERROR');
        $("#borrowingType").data("kendoComboBox").value("");
    }
}


//retrieve values from from Input Fields and save 
function saveBorrowing() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    borrowing.clientId = $('#client').data('kendoComboBox').value();
	borrowing.borrowingTypeId =  $('#borrowingType').data('kendoComboBox').value();
	borrowing.borrowingTenure =  $('#borrowingTenure').data('kendoNumericTextBox').value();
	borrowing.tenureTypeId =  $('#tenureType').data('kendoComboBox').value();
	borrowing.amountRequested =  $('#amountRequested').data('kendoNumericTextBox').value();
	borrowing.applicationDate =  $('#applicationDate').data('kendoDatePicker').value();
	borrowing.interestTypeId =  $('#interestType').data('kendoComboBox').value();
	borrowing.interestRate =  $('#interestRate').data('kendoNumericTextBox').value();
	borrowing.repaymentModeId =  $('#repaymentMode').data('kendoComboBox').value();
	borrowing.applicationFee =  $('#applicationFee').data('kendoNumericTextBox').value();
	borrowing.processingFee =  $('#processingFee').data('kendoNumericTextBox').value();
	borrowing.commission =  $('#commission').data('kendoNumericTextBox').value();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: borrowingApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(borrowing),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        //successDialog('Borrowing Successfully Saved \n Borrowing Number:'+data.borrowingNo, 
		//'SUCCESS', function () { window.location = "/Borrowing/CreateBorrowing"; });
		
		successDialog('Borrowing Successfully Saved \n Borrowing Number:'+data.borrowingNo,
		'SUCCESS', function () { window.location = "/Borrowing/CreateBorrowing/"; });

        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

function specialityEditor(container, options) {
    $('<input type="text" id="speciality" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("90%")
    .kendoComboBox({
        dataSource: specialities,
        dataValueField: "specialityId",
        dataTextField: "specialityName",
        change: onSpecialityChange,
        optionLabel: ""
    });
}

function hourlyBillingRateEditor(container, options) {
    $('<input type="text" id="hourlyBillingRate" data-bind="value:' + options.field + '"/> ')
    .appendTo(container)
	.width("90%")
    .kendoNumericTextBox({
		format: "0:#.00 \\%",
		decimals: "2",
		min: 0,
        max: 100,
        step: 0.01
    });
}



function billableEditor(container, options) {
    $('<input type="checkbox" id="billable" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("90%");
}
                        
var onSpecialityChange = function() {
	//Retrieve value enter validate
    var id = $("#speciality").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < specialities.length; i++) {
        if ( specialities[i].specialityId == id) {
            exist = true;
            break;
        }
    }	
	
    if (!exist) {
        warningDialog('Invalid Speciality', 'ERROR');
        $("#speciality").data("kendoComboBox").value("");
    }
}

function getSpecialityIdEditor(id) {
    for (var i = 0; i < specialities.length; i++) {
        if (specialities[i].specialityId == id) 
        return specialities[i].specialityName;
    }
}

function getBillingRateEditor(rate) {
    return rate + "%";
}

