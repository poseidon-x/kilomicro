//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: OCT 20TH, 2015  	
//*******************************************

"use strict";

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/CashierTillDay";
var cashierApiUrl = coreERPAPI_URL_Root + "/crud/Cashier";


//Declaration of variables to store records retrieved from the database
var cashierTillRange = {};
var cashiers = {};

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

var cashierTillAjax = $.ajax({
    url: cashierTillApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var cashierAjax = $.ajax({
    url: cashierApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


//Get All Deposit Clients
function loadForm() {
	
    $.when(cashierTillAjax,cashierAjax)
        .done(function (dataCashierTill, dataCashier) {
            cashierTillRange = dataCashierTill[2].responseJSON;
            cashiers = dataCashier[2].responseJSON;
            
            //Prepares UI
            prepareUi();
			dismissLoadingDialog();
        });
}

//Function to prepare user interface
function prepareUi() 
{	
    renderControls();

    $('#save').click(function (event) {
		var cashier = $('#currentCashier').data('kendoComboBox').text();
		if (confirm("Are you sure you want to open till for "+cashier)) {
			saveCashierTill();				
		} else
		{
            smallerWarningDialog('Please review and save later', 'NOTE');
        }
	});
	
	$('#close').click(function (event) {
		var cashier = $('#currentCashier').data('kendoComboBox').text();
		if (confirm("Are you sure you want to close till for "+cashier)) {
			closeCashierTill();				
		} else
		{
            smallerWarningDialog('Please review and save later', 'NOTE');
        }
	});
}

//Apply kendo Style to the input fields
function renderControls() {
    $("#currentCashier").width("100%")
	.kendoComboBox({
		dataSource: cashiers,
		dataValueField: "ID",
		dataTextField: "Description",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		optionLabel: ""
	});	
	$("#startDate").width('100%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		}
	});
	$("#endDate").width('100%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		}
	});
}

var onTransferTypeChange = function () {
    var id = $("#transferType").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < transferTypes.length; i++) {
        if (transferTypes[i].cashierTransferTypeId == id) {
            setUpAccountComboBox(id);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Transfer Type', 'ERROR');
        $("#transferType").data("kendoComboBox").value("");
    }
}

//retrieve values from from Input Fields and save 
function saveCashierTill() {
    retrieveValues();
    saveToServer();
}

function closeCashierTill() {
    retrieveValues();
    saveTillClosure();
}

function retrieveValues() {
    cashierTillRange.cashierTillId = $('#currentCashier').data('kendoComboBox').value();
    cashierTillRange.startDate = $('#startDate').data('kendoDatePicker').value();
    cashierTillRange.endDate = $("#endDate").data('kendoDatePicker').value();
}

//Save to server function
function saveToServer() {		
    //Save
	displayLoadingDialog();
    $.ajax({
        url: cashierTillApiUrl + '/OpenTillRange',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(cashierTillRange),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		successDialog('Till for range successfully open', 'SUCCESS', 
			function () { window.location = '/dash/home.aspx'; });
                
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer

//Save to server function
function saveTillClosure() {		
    //Save
	displayLoadingDialog();
    $.ajax({
        url: cashierTillApiUrl + '/CloseTillRange',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(cashierTillRange),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Till Successfully Close:', 'SUCCESS', 
		function () { window.location = '/dash/home.aspx'; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer
