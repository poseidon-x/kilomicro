//***********************************************************//
//  	     LOAN DOCUMENT TEMPLATE - JAVASCRIPT                
// 		        CREATOR: EMMANUEL OWUSU(MAN)    	   
//		       WEEK: JULY(27TH - 31TH), 2015  		  
//*********************************************************//


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var loanDocumentPlaceHolderTypeApiUrl = coreERPAPI_URL_Root + "/crud/loanDocumentPlaceHolderType";



//Declaration of variables to store records retrieved from the database
var loanDocumentPlaceHolderType = {};

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.ajax(
        {
            url: loanDocumentPlaceHolderTypeApiUrl + '/Get/' + loanDocumentPlaceHolderTypeId,
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            loanDocumentPlaceHolderType = data;
            prepareUi();
        }).error(function (error) {
            alert("error calling controller");
            alert(JSON.stringify(error));
        });
}


//Function to prepare user interface
function prepareUi() 
{		
    //If arInvoiceId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (loanDocumentPlaceHolderType.loanDocumentPlaceHolderTypeId > 0) {
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
			//Retrieve & save data
			saveDocument();            
		}
	});
}

//Apply kendo Style to the input fields
function renderControls() {

    $("#placeHolderTypeCode").width("75%")
		.kendoMaskedTextBox();

    $("#entityTypeCode").width("75%")
		.kendoMaskedTextBox();
}

//retrieve values from from Input Fields and save 
function saveDocument() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    loanDocumentPlaceHolderType.placeHolderTypeCode = $('#placeHolderTypeCode').data('kendoMaskedTextBox').value();
    loanDocumentPlaceHolderType.entityTypeCode = $('#entityTypeCode').data('kendoMaskedTextBox').value();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: loanDocumentPlaceHolderTypeApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(loanDocumentPlaceHolderType),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Document Place Holder Successfully Saved', 'SUCCESS',
         function () { window.location = "/DocumentTemplate/LoanDocumentTemplates/"; });        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer
