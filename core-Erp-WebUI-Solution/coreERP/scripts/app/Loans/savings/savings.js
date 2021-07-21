/*
UI Scripts for Loan saving Management
Creator: man@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var savingApiUrl = coreERPAPI_URL_Root + "/crud/saving";
var savingTypeApiUrl = coreERPAPI_URL_Root + "/crud/savingType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var savingPlanIntervalApiUrl = coreERPAPI_URL_Root + "/crud/savingPlanInterval";
var depositRepaymentModeApiUrl = coreERPAPI_URL_Root + "/crud/DepositRepaymentMode";
var fieldAgentsApiUrl = coreERPAPI_URL_Root + "/crud/Agent";

var saving = {};
var savingTypes = {};
var clients = {};
var savingPlanIntervals = {};
var depositRepaymentModes = {};
var ralationOfficers = {};
var agents = {};

var savingAjax = $.ajax({
    url: savingApiUrl + '/GetSavingAccount/' + savingId,
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
    url: clientApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var savingPlanIntervalAjax = $.ajax({
    url: savingPlanIntervalApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var depositRepaymentModeAjax = $.ajax({
    url: depositRepaymentModeApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});/*
var relationshipOfficerAjax = $.ajax({
    url: relationshipOfficersApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});*/
var fieldAgentAjax = $.ajax({
    url: fieldAgentsApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
//Load page data
function loadData() {	
    $.when(savingAjax,savingTypeAjax,clientAjax,savingPlanIntervalAjax,depositRepaymentModeAjax,
	fieldAgentAjax)
        .done(function (dataSaving,dataSavingType,dataClient,dataSavingPlanInterval,dataDepositRepaymentMode,
		dataFieldAgent) {
            saving = dataSaving[2].responseJSON;
            savingTypes = dataSavingType[2].responseJSON;
            clients = dataClient[2].responseJSON;
            savingPlanIntervals = dataSavingPlanInterval[2].responseJSON;
            depositRepaymentModes = dataDepositRepaymentMode[2].responseJSON;
            agents = dataFieldAgent[2].responseJSON;
			
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
	if(saving.savingID>0){
		populateUi();
	}
    $('#save').click(function (event) {
		if (confirm("Are you sure, you want to save this account")) {
            displayLoadingDialog();
			saveAccount();				
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
		dataTextField: "clientName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});	
	$("#firstSavingDate").width("90%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
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
	
	$("#annualInterestRate").width("90%")
	.kendoNumericTextBox({
		format: "0.#0 '%'",
		min: 0,
		change: function(e) {
			onAnnualInterestRateChange();
		}
	});
	$("#monthlyInterestRate").width("90%")
	.kendoNumericTextBox({
		format: "0.#0 '%'",
		min: 0,
		change: function(e) {
			onMonthlyInterestRateChange();
		}
	});
	$("#savingPlan").width("90%")
	.kendoComboBox({
		dataSource: savingPlanIntervals,
		filter: "contains",
		suggest: true,
		dataValueField: "planDays",
		dataTextField: "planIntervalName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onPlanChange,
		optionLabel: ""
	});
	$("#savingPlanAmount").width("90%")
	.kendoNumericTextBox();
	$("#principalRepaymentMode").width("90%")
	.kendoComboBox({
		dataSource: depositRepaymentModes,
		filter: "contains",
		suggest: true,
		dataValueField: "repaymentModeDays",
		dataTextField: "repaymentModeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#interestRepaymentMode").width("90%")
	.kendoComboBox({
		dataSource: depositRepaymentModes,
		filter: "contains",
		suggest: true,
		dataValueField: "repaymentModeDays",
		dataTextField: "repaymentModeName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#agent").width("90%")
	.kendoComboBox({
		dataSource: agents,
		filter: "contains",
		suggest: true,
		dataValueField: "agentId",
		dataTextField: "agentNameWithNo",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
	});
	$("#staff").width("90%")
	.kendoComboBox({
		dataSource: ralationOfficers,
		filter: "contains",
		suggest: true,
		dataValueField: "staffId",
		dataTextField: "staffName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: "Not Applicable"
	});
}
var onPlanChange = function(){
	var planId = $("#savingPlan").data("kendoComboBox").value();
	var planAmt = $("#savingPlanAmount").data("kendoNumericTextBox");
	if(planId==0){
		planAmt.value(0);
		planAmt.enable(false);
	}else{
		planAmt.value("");
		planAmt.enable(true);
	}
}
function populateUi(){
	$("#client").data("kendoComboBox").value(saving.clientID);
	$("#savingType").data("kendoComboBox").value(saving.savingTypeID);
	//$("#annualInterestRate").data("kendoComboBox").value(saving.currencyID);
	$("#monthlyInterestRate").data("kendoNumericTextBox").value(saving.interestRate);
	//$("#depositPlan").data("kendoComboBox").value(savingAdditional.saving.availablePrincipalBalance);
	$("#interestBalance").data("kendoNumericTextBox").value(savingAdditional.saving.interestBalance);
	$("#availableInterestBalance").data("kendoNumericTextBox").value(savingAdditional.saving.availableInterestBalance);
}

function retrieveValues(){
	saving.clientID = $("#client").data("kendoComboBox").value();
	saving.firstSavingDate = $("#firstSavingDate").data("kendoDatePicker").value();
	saving.savingTypeID = $("#savingType").data("kendoComboBox").value();
	saving.interestRate = $("#monthlyInterestRate").data("kendoNumericTextBox").value();
	saving.interestRepaymentModeID = $("#interestRepaymentMode").data("kendoComboBox").value();
	saving.principalRepaymentModeID = $("#principalRepaymentMode").data("kendoComboBox").value();	
	saving.savingPlanID = $("#savingPlan").data("kendoComboBox").value();
	saving.savingPlanAmount = $("#savingPlanAmount").data("kendoNumericTextBox").value();
	saving.agentId = $("#agent").data("kendoComboBox").value();
}
function saveAccount() {
    retrieveValues();
    saveToServer();
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: savingApiUrl + '/PostSavings',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(saving),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Savings Account Successfully Saved', 'SUCCESS', function () { window.location = '/Saving/Savings'; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer

function onAnnualInterestRateChange(){
    var annualRate = $('#annualInterestRate').data('kendoNumericTextBox').value();

    if(annualRate != null && annualRate != 'undefined'){
        var monthylyRate = annualRate/12;
        $('#monthlyInterestRate').data('kendoNumericTextBox').value(monthylyRate);
    }
}

//Calculate Interest Rate
function onMonthlyInterestRateChange(){
    var monthlyRate = $('#monthlyInterestRate').data('kendoNumericTextBox').value();

    if(monthlyRate != null && monthlyRate != 'undefined'){
        var yearlyRate = monthlyRate*12;
        $('#annualInterestRate').data('kendoNumericTextBox').value(yearlyRate);
    }
}