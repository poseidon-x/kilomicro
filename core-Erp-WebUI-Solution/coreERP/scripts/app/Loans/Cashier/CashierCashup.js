﻿//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: OCT 20TH, 2015  	
//*******************************************


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var cashierCashupApiUrl = coreERPAPI_URL_Root + "/crud/cashierCashup";
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/CashierTill";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var bankAccountApiUrl = coreERPAPI_URL_Root + "/crud/BankGLAccount";
var transferTypeApiUrl = coreERPAPI_URL_Root + "/crud/CashierTransferType";


//Declaration of variables to store records retrieved from the database
var cashiersCashup = {};
var cashierTills = {};
var accts = {};
var bankGLAcctIds = {};
var bankAccts = [];
var cashAccts = [];
var transferTypes = {};

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


var cashierCashupAjax = $.ajax({
    url: cashierCashupApiUrl + '/Get/' + cashierCashupId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var cashierTillAjax = $.ajax({
    url: cashierTillApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var acctAjax = $.ajax({
    url: acctsApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var transferTypeAjax = $.ajax({
    url: transferTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var bankAccountAjax = $.ajax({
    url: bankAccountApiUrl + '/GetGlAcctIds',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Get All Deposit Clients
function loadForm() {
	
    $.when(cashierCashupAjax,cashierTillAjax,acctAjax,transferTypeAjax,bankAccountAjax)
        .done(function (dataCashierCashup, dataCashierTill,dataAcct,dataTransferType,dataBankAccount) {
            cashiersCashup = dataCashierCashup[2].responseJSON;
            cashierTills = dataCashierTill[2].responseJSON;
            accts = dataAcct[2].responseJSON;
            transferTypes = dataTransferType[2].responseJSON;
            bankGLAcctIds = dataBankAccount[2].responseJSON;
			getBankGlAccounts(bankGLAcctIds);
			getCashGlAccounts();
			
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
		var cashier = $('#cashierTill').data('kendoComboBox').text();
		if (confirm("Are you sure you want cashup "+cashier+"\'s Till?")) {
            displayLoadingDialog();
			savecashiersCashup();				
		} else
		{
            smallerWarningDialog('Please review and cashup later', 'NOTE');
        }
	});
}

//Apply kendo Style to the input fields
function renderControls() {
	
	
    $("#cashierTill").width("100%")
	.kendoComboBox({
		dataSource: cashierTills,
		filter: "contains",
		suggest: true,
		dataValueField: "cashiersTillID",
		dataTextField: "userName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onCashierTillChange,
		optionLabel: ""
	});	
	$("#cashupDate").width('100%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$("#transferType").width("100%")
	.kendoComboBox({
		dataSource: transferTypes,
		filter: "contains",
		suggest: true,
		dataValueField: "cashierTransferTypeId",
		dataTextField: "transferTypeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onTransferTypeChange,
		optionLabel: ""
	});
	$("#account").width("100%")
	.kendoComboBox({
		//dataSource: accts,
		filter: "contains",
		suggest: true,
		dataValueField: "acct_id",
		dataTextField: "acc_name",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#transferAmount").width("100%")
	.kendoNumericTextBox();

	

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
var onCashierTillChange = function () {
    var id = $("#cashierTill").data("kendoComboBox").value();
	var amount = $("#transferAmount").data("kendoNumericTextBox");
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < transferTypes.length; i++) {
        if (cashierTills[i].cashiersTillID == id) {
            amount.value(cashierTills[i].currentBalance);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Cashier Till', 'ERROR');
        $("#cashierTill").data("kendoComboBox").value("");
    }
}

//retrieve values from from Input Fields and save 
function savecashiersCashup() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    cashiersCashup.cashierTillId = $('#cashierTill').data('kendoComboBox').value();
    cashiersCashup.transferTypeId = $('#transferType').data('kendoComboBox').value();
    cashiersCashup.transferAmount = $("#transferAmount").data('kendoNumericTextBox').value();
	cashiersCashup.cashupDate = $("#cashupDate").data('kendoDatePicker').value();
	if(cashiersCashup.transferTypeId == 1){
		cashiersCashup.cashInVaultId = $("#account").data('kendoComboBox').value();
	}else if(cashiersCashup.transferTypeId == 2){
		cashiersCashup.bankAccountId = $("#account").data('kendoComboBox').value();
	}
}

//Save to server function
function saveToServer() {
		
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: cashierCashupApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(cashiersCashup),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Cashup Successful:', 'SUCCESS', function () { window.location = '/Cashier/CashierCashup'; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
	
}//func saveToServer

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

