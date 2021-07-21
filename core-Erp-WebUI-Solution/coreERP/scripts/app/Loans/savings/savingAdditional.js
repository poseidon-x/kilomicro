/*
UI Scripts for Loan savingType Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var savingAdditionalApiUrl = coreERPAPI_URL_Root + "/crud/saving";
var savingTypeApiUrl = coreERPAPI_URL_Root + "/crud/savingType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var modeOfPaymentApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var bankApiUrl = coreERPAPI_URL_Root + "/crud/banks";

var savingAdditional = {};
var savingTypes = {};
var clients = {};
var currencies = {};
var modeOfpayments = {};
var banks = {};

var savingAjax = $.ajax({
    url: savingAdditionalApiUrl + '/GetSavingAdditional/' + savingAdditionalId,
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
            savingAdditional = dataSaving[2].responseJSON;
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
	if(savingAdditional.savingAdditionalID>0){
		populateUi();
	}
    $('#save').click(function (event) {
		if (confirm("Are you sure you want save deposit")) {
            displayLoadingDialog();
			saveAdditional();				
		} else
		{
            smallerWarningDialog('Please review and save later', 'NOTE');
        }
	});
}
//Apply kendo Style to the input fields
function renderControls() {
    $("#client").width("90%")
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
		change: onClientChange,
		optionLabel: ""
	});	
	$("#savingType").width("90%")
	.kendoComboBox({
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
	$("#currency").width("90%")
	.kendoComboBox({
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
	$("#principalBalance").width("90%")
	.kendoNumericTextBox();
	$("#availablePrincipalBalance").width("90%")
	.kendoNumericTextBox();
	$("#interestBalance").width("90%")
	.kendoNumericTextBox();
	$("#availableInterestBalance").width("90%")
	.kendoNumericTextBox();
	$("#depositDate").width("90%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$("#depositAmount").width("90%")
	.kendoNumericTextBox();
	$("#paymentType").width("90%")
	.kendoComboBox({
		dataSource: modeOfpayments,
		filter: "contains",
		suggest: true,
		dataValueField: "ID",
		dataTextField: "Description",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onPaymentTypeChange,
		optionLabel: ""
	});
	$("#checkNo").width("90%")
	.kendoMaskedTextBox();
	$("#bank").width("90%")
	.kendoComboBox({
		dataSource: banks,
		filter: "contains",
		suggest: true,
		dataValueField: "bank_id",
		dataTextField: "bank_name",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#interestRate").width("90%")
	.kendoNumericTextBox();
	$("#naration").width("90%")
	.kendoMaskedTextBox();
}
function populateUi(){
	$("#client").data("kendoComboBox").value(savingAdditional.saving.clientID);
	$("#savingType").data("kendoComboBox").value(savingAdditional.saving.savingTypeID);
	$("#currency").data("kendoComboBox").value(savingAdditional.saving.currencyID);
	$("#principalBalance").data("kendoNumericTextBox").value(savingAdditional.saving.principalBalance);
	$("#availablePrincipalBalance").data("kendoNumericTextBox").value(savingAdditional.saving.availablePrincipalBalance);
	$("#interestBalance").data("kendoNumericTextBox").value(savingAdditional.saving.interestBalance);
	$("#availableInterestBalance").data("kendoNumericTextBox").value(savingAdditional.saving.availableInterestBalance);
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
        if (modeOfpayments[i].ID == payTypeId) {            
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
        url: savingAdditionalApiUrl + '/GetSavingAccount/'+id,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        savingAdditional.saving = data;
		populateUi();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
}
function retrieveValues(){
	savingAdditional.savingDate = $("#depositDate").data("kendoDatePicker").value();
	savingAdditional.savingAmount = $("#depositAmount").data("kendoNumericTextBox").value();
	savingAdditional.principalBalance = $("#depositAmount").data("kendoNumericTextBox").value();
	savingAdditional.modeOfPaymentID = $("#paymentType").data("kendoComboBox").value();
	if(savingAdditional.modeOfPaymentID==2){
		savingAdditional.checkNo = $("#checkNo").data("kendoMaskedTextBox").value();
		savingAdditional.bankID = $("#bank").data("kendoComboBox").value();
	}
	savingAdditional.naration = $("#naration").data("kendoMaskedTextBox").value();

}
function saveAdditional() {
    retrieveValues();
    saveToServer();
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: savingAdditionalApiUrl + '/PostSavingsAddtional',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(savingAdditional),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Deposit Successfully received', 'SUCCESS', function () { window.location = '/dash/home.aspx'; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer