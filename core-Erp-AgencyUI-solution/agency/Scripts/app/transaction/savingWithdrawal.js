/*
Creator: damien@acsghana.com
*/

// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = agencyAPI_Token;
var savingWithdrawalApiUrl = agencyAPI_URL_Root + "/Saving";
var savingTypeApiUrl = agencyAPI_URL_Root + "/SavingType";
var clientApiUrl = agencyAPI_URL_Root + "/AllClients";
var currencyApiUrl = agencyAPI_URL_Root + "/Currency";
var modeOfPaymentApiUrl = agencyAPI_URL_Root + "/PaymentMode";
var bankApiUrl = agencyAPI_URL_Root + "/Banks";

var savingWithdrawal = {};
var savingTypes = {};
var clients = {};
var currencies = {};
var modeOfpayments = {};
var banks = {};

var withdrawalTypes = [
		{value: 'I', name:'Interest'},
		{value: 'P', name:'Principal'},
		{value: 'B', name: 'Both'}
	];

var savingAjax = $.ajax({
    url: savingWithdrawalApiUrl + '/GetSavingWithdrawal/' + savingWithdrawalId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var savingTypeAjax = $.ajax({
    url: savingTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var clientAjax = $.ajax({
    url: clientApiUrl + '/GetSavingsClient',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var currencyAjax = $.ajax({
    url: currencyApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var paymentModeAjax = $.ajax({
    url: modeOfPaymentApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var bankAjax = $.ajax({
    url: bankApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
//Load page data
function loadData() {	
    $.when(savingAjax,savingTypeAjax,clientAjax,currencyAjax,paymentModeAjax,bankAjax)
        .done(function (dataSaving,dataSavingType,dataClient,dataCurrency,dataPaymentMode,dataBank) {
            savingWithdrawal = dataSaving[2].responseJSON;
            savingTypes = dataSavingType[2].responseJSON;
            clients = dataClient[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
            modeOfpayments = dataPaymentMode[2].responseJSON;
            banks = dataBank[2].responseJSON;
			
            //Prepares UI
            prepareUi();
			dismissLoadingDialog();

        }
	);
}
$(function () {
    displayLoadingDialog();
    loadData();
});

function prepareUi(){
	renderControls();
	if(savingWithdrawal.savingWithdrawalID>0){
		populateUi();
	}
    $('#save').click(function (event) {
		if (confirm("Are you sure you want Withdraw from savings account")) {
            displayLoadingDialog();
			saveWithdrawal();				
		} else
		{
            smallerWarningDialog('Please review and withdraw later', 'NOTE');
        }
	});
}
//Apply kendo Style to the input fields
function renderControls() {
    $("#client").width("90%").kendoComboBox({
		dataSource: clients,
		filter: "contains",
		suggest: true,
		dataValueField: "clientID",
		dataTextField: "clientNameWithAccountNO",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onClientChange,
		optionLabel: ""
	});	
	$("#savingType").width("90%").kendoComboBox({
		dataSource: savingTypes,
		filter: "contains",
		suggest: true,
		dataValueField: "savingTypeID",
		dataTextField: "savingTypeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#currency").width("90%").kendoComboBox({
		dataSource: currencies,
		filter: "contains",
		suggest: true,
		dataValueField: "currency_id",
		dataTextField: "major_name",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#principalBalance").width("90%").kendoNumericTextBox();
	$("#availablePrincipalBalance").width("90%").kendoNumericTextBox();
	$("#interestBalance").width("90%").kendoNumericTextBox();
	$("#availableInterestBalance").width("90%").kendoNumericTextBox();
	$("#withdrawalDate").width("90%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$("#withdrawalAmount").width("90%").kendoNumericTextBox();
	$("#paymentType").width("90%").kendoComboBox({
		dataSource: modeOfpayments,
		filter: "contains",
		suggest: true,
		dataValueField: "modeOfPaymentID",
		dataTextField: "modeOfPaymentName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onPaymentTypeChange,
		optionLabel: ""
	});
	$("#checkNo").width("90%").kendoMaskedTextBox();
	$("#bank").width("90%").kendoComboBox({
		dataSource: banks,
		filter: "contains",
		suggest: true,
		dataValueField: "bankId",
		dataTextField: "bankName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#withdrawalType").width("90%").kendoComboBox({
		dataSource: withdrawalTypes,
		dataValueField: "value",
		dataTextField: "name",
		optionLabel: "",
		change: function(e) {
			var amt = $("#withdrawalAmount").data("kendoNumericTextBox");
			var sv = savingWithdrawal.saving;
			
			if (this.value() == 'I') {
				amt.max(sv.availableInterestBalance);
				if(amt.value() > sv.availableInterestBalance)amt.value(sv.availableInterestBalance);
			}
			else if (this.value() == 'P') {
				amt.max(sv.availablePrincipalBalance);
			}
			else if (this.value() == 'B') {
				amt.max(sv.availablePrincipalBalance+sv.availableInterestBalance);
			}
        }
	});
	$("#narration").width("90%").kendoMaskedTextBox();
}
function populateUi(){
	$("#client").data("kendoComboBox").value(savingWithdrawal.saving.clientID);
	$("#savingType").data("kendoComboBox").value(savingWithdrawal.saving.savingTypeID);
	$("#currency").data("kendoComboBox").value(savingWithdrawal.saving.currencyID);
	$("#principalBalance").data("kendoNumericTextBox").value(savingWithdrawal.saving.principalBalance);
	$("#availablePrincipalBalance").data("kendoNumericTextBox").value(savingWithdrawal.saving.availablePrincipalBalance);
	$("#interestBalance").data("kendoNumericTextBox").value(savingWithdrawal.saving.interestBalance);
	$("#availableInterestBalance").data("kendoNumericTextBox").value(savingWithdrawal.saving.availableInterestBalance);
}
var onClientChange = function(){
	var clientId = $("#client").data("kendoComboBox").value();
	getClientSavingsAccount(clientId);
}
var onPaymentTypeChange = function(){
	var payTypeId = $("#paymentType").data("kendoComboBox").value();
	var exist = false;	
	//Retrieve value enter validate
    for (var i = 0; i < modeOfpayments.length; i++) {
        if (modeOfpayments[i].modeOfPaymentID == payTypeId) {            
			checkPaymentType(payTypeId);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Payment Type', 'ERROR');
        $("#paymentType").data("kendoComboBox").value("");
    }
}
function checkPaymentType(id){
	var chk = $("#checkNo").data("kendoMaskedTextBox");
	var bk = $("#bank").data("kendoComboBox");
	if(id == 1){
		chk.value("Not Applicable");
		bk.text("Not Applicable");
		chk.enable(false);
		bk.enable(false);
	}else{
		chk.value("");
		bk.text("");
		chk.enable(true);
		bk.enable(true);
	}
}
function getClientSavingsAccount(id){
	$.ajax({
        url: savingWithdrawalApiUrl + '/GetSavingAccount/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        savingWithdrawal.saving = data;
		populateUi();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
}
function retrieveValues(){
	var sav = savingWithdrawal.saving;
	savingWithdrawal.withdrawalDate = $("#withdrawalDate").data("kendoDatePicker").value();
	var withAmt = $("#withdrawalAmount").data("kendoNumericTextBox").value();
	savingWithdrawal.modeOfPaymentID = $("#paymentType").data("kendoComboBox").value();
	if(savingWithdrawal.modeOfPaymentID==2){
		savingWithdrawal.checkNo = $("#checkNo").data("kendoMaskedTextBox").value();
		savingWithdrawal.bankID = $("#bank").data("kendoComboBox").value();
	}
	savingWithdrawal.naration = $("#narration").data("kendoMaskedTextBox").value();
	if(withAmt>sav.availableInterestBalance){
		savingWithdrawal.interestWithdrawal = sav.availableInterestBalance;
		withAmt -= sav.availableInterestBalance;
	}
	savingWithdrawal.withType = $("#withdrawalType").data("kendoComboBox").value();
	savingWithdrawal.withdrawalAmount = savingWithdrawal.interestWithdrawal+savingWithdrawal.principalWithdrawal;
	savingWithdrawal.principalWithdrawal = withAmt;
	savingWithdrawal.principalBalance = sav.principalBalance  - savingWithdrawal.principalWithdrawal;
	savingWithdrawal.interestBalance = sav.interestBalance - savingWithdrawal.interestWithdrawal;

}
function saveWithdrawal() {
    retrieveValues();
    saveToServer();
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: savingWithdrawalApiUrl + '/PostSavingsWithdrawal',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(savingWithdrawal),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('saving withdrawal Successful', 'SUCCESS');        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer