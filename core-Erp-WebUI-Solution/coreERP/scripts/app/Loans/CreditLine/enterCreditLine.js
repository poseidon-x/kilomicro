//***********************************************************//
//  	     CREDIT MEMO - JAVASCRIPT                //
// 		CREATOR: EMMANUEL OWUSU(MAN)    	   //
//		      WEEK: JUNE(8TH - 12TH), 2015  		  //
//*********************************************************//


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


function loadForm() {
    $.when(creditLineAjax, clientAjax)
        .done(function (dataCreditLine, dataClient) {
            creditLine = dataCreditLine[2].responseJSON;		
            clients = dataClient[2].responseJSON;
			
			prepareUi() ;
			dismissLoadingDialog();
        });
}


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//Function to prepare user interface
function prepareUi() 
{		
    renderControls();

    $('#save').click(function (event) {
			if (confirm('Are you sure you want save this credit line?')) {
                displayLoadingDialog();
				saveCreditLine();				
            } else {
                smallerWarningDialog('Please review and save later', 'NOTE');
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
		min: new Date(),
    });

    $("#amountRequested").width('75%')
        .kendoNumericTextBox({
        format: "GHS #,##0.00"
    });

    $("#tenure").width("75%")
        .kendoNumericTextBox({
            format: "0 'Months'"
    });		
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
        warningDialog('Invalid Client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}

//retrieve values from from Input Fields and save 
function saveCreditLine() {
    retrieveValues();
    saveToServer();
}



function retrieveValues() {
    creditLine.clientId = $('#client').data('kendoComboBox').value();
    creditLine.applicationDate =  $('#applicationDate').data('kendoDatePicker').value();	
    creditLine.amountRequested =  $('#amountRequested').data('kendoNumericTextBox').value();	
    creditLine.tenure =  $('#tenure').data('kendoNumericTextBox').value();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: creditLineApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(creditLine),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Credit Line Successfully Saved \n Credit Line Number:' + data.creditLineNumber, 'SUCCESS',
            function () { window.location = '/CreditLine/CreditLines'; });
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer
