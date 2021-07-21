//***********************************************************//
//  	        CREDIT MEMO - JAVASCRIPT                
// 		      CREATOR: EMMANUEL OWUSU(MAN)    	   
//		      WEEK: JUNE(20TH - 24TH), 2015  		  
//**********************************************************//


//"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var creditLineApiUrl = coreERPAPI_URL_Root + "/crud/CreditLine";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";

//Declaration of variables to store records retrieved from the database
var creditLine = {};
var clients = {};

//Declare a variable and store creditLine table ajax call in it
var creditLineAjax = $.ajax({
    url: creditLineApiUrl + '/Get/' + creditLineId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store client table ajax call in it
var clientAjax = $.ajax({
    url: clientApiUrl + '/Get',
           type: 'Get',
           beforeSend: function (req) {
               req.setRequestHeader('Authorization', "coreBearer " + authToken);
           }
});

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

function loadForm() {
    $.when(creditLineAjax, clientAjax)
        .done(function (dataCreditLine, dataClient) {
            creditLine = dataCreditLine[2].responseJSON;		
            clients = dataClient[2].responseJSON;
			
			prepareUi() ;
        });
}




//Function to prepare user interface
function prepareUi() 
{
    if (creditLine.creditLineId > 0) {
        renderControls();
        populateUi();
        dismissLoadingDialog();
    } else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        dismissLoadingDialog();
		successDialog('Please select credit to approve', 'NOTE', function () { window.location = '/LoanSetup/CreditLines'; });         
	}


    $('#approve').click(function (event) {
			if (confirm('Are you sure you want Approve this credit line?')) {
                displayLoadingDialog();
				saveCreditLine();				
            } else {
                smallerWarningDialog('Please review and Approve later', 'NOTE');
            }
	});
	
}

//Apply kendo Style to the input fields
function renderControls() {
    
    $("#client").width("75%")
		.kendoComboBox({
		    dataSource: clients,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientID",
		    dataTextField: "clientName",
			change: onClientChange,
			optionLabel: ""
	});
	
    $("#applicationDate").width("75%")
        .kendoDatePicker({
        format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
		min: new Date()
    });

    $("#amountRequested").width('75%')
        .kendoNumericTextBox({
        format: "GHS #,##0.00",
		min: 0		
    });

    $("#tenure").width("75%")
        .kendoNumericTextBox({
            format: "0 'Months'",
		min: 0			
    });	

	$("#amountApproved").width('75%')
        .kendoNumericTextBox({
        format: "GHS #,##0.00",
		min: 0
    });	
}

function populateUi() {
    $('#client').data('kendoComboBox').value(creditLine.clientId);
    $('#applicationDate').data('kendoDatePicker').value(creditLine.applicationDate);
    $('#amountRequested').data('kendoNumericTextBox').value(creditLine.amountRequested);
    $('#tenure').data('kendoNumericTextBox').value(creditLine.tenure);
}


var onClientChange = function () {
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
        warningDialog('Invalid Cliet', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}

//retrieve values from from Input Fields and save 
function saveCreditLine() {
    retrieveValues();
    saveToServer();
}



function retrieveValues() {
    creditLine.amountApproved =  $('#amountApproved').data('kendoNumericTextBox').value();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: creditLineApiUrl + '/ApproveCreditLine',
        type: 'Put',
        contentType: 'application/json',
        data: JSON.stringify(creditLine),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Credit Line Number:' + data.creditLineNumber + ' Successfully Approved', 'SUCCESS',
            function () { window.location = '/CreditLine/CreditLines'; });
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer
