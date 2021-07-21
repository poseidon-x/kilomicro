//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: OCT 20TH, 2015  	
//*******************************************


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var clientServiceChargeApiUrl = coreERPAPI_URL_Root + "/crud/ClientServiceCharge";
var chargeTypeApiUrl = coreERPAPI_URL_Root + "/crud/ClientChargeType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";


//Declaration of variables to store records retrieved from the database
var clientServiceCharge = {};
var chargeTypes = {};
var clients = {};

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


var clientServiceChargeAjax = $.ajax({
    url: clientServiceChargeApiUrl + '/Get/' + id,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var chargeTypeAjax = $.ajax({
    url: chargeTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Get All Deposit Clients
function loadForm() {
	
    $.when(clientServiceChargeAjax,chargeTypeAjax)
        .done(function (dataClientServiceCharge, dataChargeType) {
            clientServiceCharge = dataClientServiceCharge[2].responseJSON;
            chargeTypes = dataChargeType[2].responseJSON;
          
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
		var cln = $('#client').data('kendoComboBox').text();
		if (confirm("Are you sure you want to apply charge to "+cln)) {
			saveCharge();				
		} else
		{
            smallerWarningDialog('Please review and apply later', 'NOTE');
        }
	});
}

//Apply kendo Style to the input fields
function renderControls() {
	$("#client").keyup(function(){
		var txt = $("#client").data("kendoMaskedTextBox").value();
		
		if(txt.length > 2){
			var input = {searchString:txt};
			getClients(input);
		}
	});
	
	$("#client").width("90%")
	.kendoMaskedTextBox();
    $("#chargeType").width("90%")
	.kendoComboBox({
		dataSource: chargeTypes,
		filter: "contains",
		suggest: true,
		dataValueField: "chargeTypeID",
		dataTextField: "chargeTypeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onChargeTypeChange,
		optionLabel: ""
	});	
	$("#chargeDate").width('90%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$("#amount").width("90%")
	.kendoNumericTextBox();
}

var onChargeTypeChange = function () {
    var id = $("#chargeType").data("kendoComboBox").value();
	var amt = $("#amount").data("kendoNumericTextBox");
	amt.value("");
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < chargeTypes.length; i++) {
        if (chargeTypes[i].chargeTypeID == id) {
			if(chargeTypes[i].fixed){
				amt.value(chargeTypes[i].chargeDefaultAmount);
			}
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Charge Type', 'ERROR');
        $("#chargeType").data("kendoComboBox").value("");
    }
}


//retrieve values from from Input Fields and save 
function saveCharge() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    clientServiceCharge.clientId = $('#client').data('kendoComboBox').value();
    clientServiceCharge.chargeTypeId = $('#chargeType').data('kendoComboBox').value();
    clientServiceCharge.chargeAmount = $("#amount").data('kendoNumericTextBox').value();
	clientServiceCharge.chargeDate = $("#chargeDate").data('kendoDatePicker').value();
}

//Save to server function
function saveToServer() {
	displayLoadingDialog();	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientServiceChargeApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(clientServiceCharge),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Charge successfully Applied:', 'SUCCESS', 
		function () { window.location = '/dash/home.aspx'; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
	
}//func saveToServer

function getClients(input) {
	displayLoadingDialog();
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetAllClientsBySearch',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(input),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		if(data.length>0){
			clients = data;
			setUpCombox();
		}
		else{
			clients = {};
			successDialog('No client found that match the searched criteria', 'SUCCESS');
		}        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}

function setUpCombox(){
	$("#client")
	.kendoComboBox({
		dataSource: clients,
		filter: "contains",
		suggest: true,
		dataValueField: "clientID",
		dataTextField: "clientNameWithAccountNO",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
}

function getBankGlAccounts(data){
	for(var i =0;i<data.length;i++){
		for(var j =0;j<accts.length;j++){
			if(accts[j].acct_id == data[i]) 
			{
				bankAccts.push(accts[j]);
				break;
			}
		}
	}
}

function getCashGlAccounts(){
	for(var i =0;i<accts.length;i++){
		var str = accts[i].acc_name;
		if(str.search(/cash in vault/i)>-1) 
		{
			cashAccts.push(accts[i]); 
		}
	}
}

function setUpAccountComboBox(id){
	var transType = $("#account").data("kendoComboBox");
	if(id == 1){
		transType.setDataSource(cashAccts);
	}else if(id == 2){
		transType.setDataSource(bankAccts)
	}
}

