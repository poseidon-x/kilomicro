//***********************************************************//
//  	     CREDIT MEMO - JAVASCRIPT                //
// 		CREATOR: EMMANUEL OWUSU(MAN)    	   //
//		      WEEK: JUNE(8TH - 12TH), 2015  		  //
//*********************************************************//


"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var depositTypeTenureApiUrl = coreERPAPI_URL_Root + "/crud/depositTypeAllowedTenure";
var depositTypeApiUrl = coreERPAPI_URL_Root + "/crud/depositType";
var tenureTypeApiUrl = coreERPAPI_URL_Root + "/crud/tenureType";


//Declaration of variables to store records retrieved from the database
var depositTypeTenure = {};
var depositType = {};
var tenureType = {};

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

var depositTypeTenureAjax = $.ajax({
    url: depositTypeTenureApiUrl + "/Get/" + depositTypeAllowedTenureId,
    type: "GET",
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var depositTypeAjax = $.ajax({
    url: depositTypeApiUrl + '/Get',
    type: 'GET',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var tenureTypeAjax = $.ajax({
    url: tenureTypeApiUrl + '/Get',
    type: 'GET',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(depositTypeTenureAjax, depositTypeAjax, tenureTypeAjax)
        .done(function (dataDepositTypeTenure, dataDepositType, dataTenureType) {
            depositTypeTenure = dataDepositTypeTenure[2].responseJSON;
            depositType = dataDepositType[2].responseJSON;
            tenureType = dataTenureType[2].responseJSON;
			
            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() 
{		
    //If depositTypeAllowedTenureId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    //if (depositTypeTenure.depositTypeAllowedTenureId > 0) {
       // renderControls();
        //populateUi();
       // dismissLoadingDialog();
    //} else //Else its a Post/Create, Hence render empty UI for new Entry
    //{
        renderControls();
        dismissLoadingDialog();
    //}

	//Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {
	
        var validator = $("#myform").kendoValidator().data("kendoValidator");
		
        if (!validator.validate()) {
            smallerWarningDialog('One or More Fields are Empty', 'ERROR');
        } else {
            saveDepositTypeTenure();
		}
	});
}

//Apply kendo Style to the input fields
function renderControls() {
    $("#depositType").width("100%")
	.kendoComboBox({
		dataSource: depositType,
		filter: "contains",
		suggest: true,
		dataValueField: "depositTypeID",
		dataTextField: "depositTypeName",
		//change: onDepositTypeChange,
		optionLabel: ""
	});

    $("#tenureType").width("100%")
	.kendoComboBox({
		dataSource: tenureType,
		filter: "contains",
		suggest: true,
		dataValueField: "tenureTypeID",
		dataTextField: "tenureTypeName",
		change: onTenureTypeChange,
		optionLabel: ""
	});	
	$("#minTenure").width("100%").kendoNumericTextBox();
	$("#maxTenure").width("100%").kendoNumericTextBox();
}

var onTenureTypeChange = function () {
    var id = $("#tenureType").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < tenureType.length; i++) {
        if (tenureType[i].tenureTypeID == id) {
            renderOtherControls(tenureType[i].tenureTypeName);
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Tenure', 'ERROR');
        $("#tenureType").data("kendoComboBox").value("");
    }
}

//render Grid
function renderOtherControls(tenureTypeName) {

    $("#minTenure").width('75%')
		.kendoNumericTextBox({
		    format: "0 " + tenureTypeName
		});

    $("#maxTenure").width('75%')
		.kendoNumericTextBox({
		    format: "0 " + tenureTypeName
		});
    
}


//retrieve values from from Input Fields and save 
function saveDepositTypeTenure() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    depositTypeTenure.depositTypeId = $('#depositType').data('kendoComboBox').value();
    depositTypeTenure.tenureTypeId = $('#tenureType').data('kendoComboBox').value();
    depositTypeTenure.minTenure = $('#minTenure').data('kendoNumericTextBox').value();
    depositTypeTenure.maxTenure = $('#maxTenure').data('kendoNumericTextBox').value();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: depositTypeTenureApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(depositTypeTenure),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Product Tenure Successfully Saved', 'SUCCESS', function() { window.location = "/dash/home.aspx"; });

    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

