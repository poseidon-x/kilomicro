'use strict';

var clientInvestmentApiUrl = coreERPAPI_URL_Root + "/crud/Deposit";
var clientInvestmentReceiptDetailApiUrl = coreERPAPI_URL_Root + "/crud/InvestmentReceipt";
var clientsApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var paymentModeApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var banksApiUrl = coreERPAPI_URL_Root + "/crud/banks";
var depositTypeApiUrl = coreERPAPI_URL_Root + "/crud/depositType"; 
var depositRepaymentModeApiUrl = coreERPAPI_URL_Root + "/crud/DepositRepaymentMode";
var relationshipOfficersApiUrl = coreERPAPI_URL_Root + "/crud/Staff";
var fieldAgentsApiUrl = coreERPAPI_URL_Root + "/crud/Agent";

var clientInvestmApplication = {};
var clientInvestmRecieptDet = {};
var clients = {};
var paymentModes = {};
var banks = {};
var depositTypes = {};
var depositRepaymentModes = {};
var staff = {};
var fieldAgents = {};
var depositPeriods = [
    { value:2, month:"60 Days",days:60 },
    { value:3, month:"91 Days",days:91 },
    { value:6, month:"182 Days",days:182 },	
    { value:12, month:"365 Days",days:365 }
];
var clientInvestmentAjax = $.ajax({
    url: clientInvestmentApiUrl + "/GetDeposit/" + depositId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var clientInvestmentRecieptDetAjax = $.ajax({
    url: clientInvestmentReceiptDetailApiUrl + "/GetInvestmentDetail/" + receiptDetailId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var clientAjax = $.ajax({
    url: clientsApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var paymentModeAjax = $.ajax({
    url: paymentModeApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var bankAjax = $.ajax({
    url: banksApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var depositTypeAjax = $.ajax({
    url: depositTypeApiUrl + "/Get",
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
});
var relationshipOfficerAjax = $.ajax({
    url: relationshipOfficersApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var fieldAgentAjax = $.ajax({
    url: fieldAgentsApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadData() {

    $.when(clientInvestmentAjax,clientAjax, paymentModeAjax, bankAjax, depositTypeAjax, depositRepaymentModeAjax, relationshipOfficerAjax, fieldAgentAjax,clientInvestmentRecieptDetAjax)
        .done(function (dataClientInvestment,dataClient, dataPaymentMode, dataBank, dataDeposiType, dataDepositRepaymentMode, dataRelationshipOfficer, dataFieldAgent, dataClientInvestmentRecieptDet) {
            clientInvestmApplication = dataClientInvestment[2].responseJSON;
			clientInvestmRecieptDet = dataClientInvestmentRecieptDet[2].responseJSON;
            clients = dataClient[2].responseJSON;
            paymentModes = dataPaymentMode[2].responseJSON;
            banks = dataBank[2].responseJSON;
            depositTypes = dataDeposiType[2].responseJSON;
            depositRepaymentModes = dataDepositRepaymentMode[2].responseJSON;
            staff = dataRelationshipOfficer[2].responseJSON;
            fieldAgents = dataFieldAgent[2].responseJSON;

            prepareUI();
			if(clientInvestmRecieptDet.clientInvestmentReceiptDetailId> 0){
				populateUI();
			}
            dismissLoadingDialog();
        }
	);
}


$(function () {
    displayLoadingDialog();
    loadData();
});

function prepareUI() {
				
	$('#client').width('90%').kendoComboBox({
		dataSource: clients,
		dataValueField: 'clientID',
		dataTextField: 'clientName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		change: onClientChange,
		optionLabel: ''
	});
	$('#amountInvested').width('90%').kendoNumericTextBox();
	$('#depositDate').width('90%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
		change:function(e) {
			OnPeriodChange();
		} 
	});
	$('#paymentType').width('90%').kendoComboBox({
		dataSource: paymentModes,
		dataValueField: 'ID',
		dataTextField: 'Description',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onPaymentTypChange,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
	});
	$('#checkNo').width('90%').kendoMaskedTextBox();
	$('#bank').width('90%').kendoComboBox({
		dataSource: banks,
		dataValueField: 'bankId',
		dataTextField: 'bankName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
	});
	$('#narration').width('90%').kendoMaskedTextBox();
	$('#investmentProduct').width('90%').kendoComboBox({
		dataSource: depositTypes,
		dataValueField: 'depositTypeID',
		dataTextField: 'depositTypeName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		change: onProductChange,
		optionLabel: ''
	});
	$('#relationshipOfficer').width('90%').kendoComboBox({
		dataSource: staff,
		dataValueField: 'staffId',
		dataTextField: 'staffName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
	});
	$('#fieldAgent').width('90%').kendoComboBox({
		dataSource: fieldAgents,
		dataValueField: 'agentId',
		dataTextField: 'agentNameWithNo',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
	});
	$('#depositPeriod').width('90%').kendoComboBox({
		dataSource: depositPeriods,
		dataValueField: 'value',
		dataTextField: 'month',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: '',
		change: function(e) {
			period = this.value();
			OnPeriodChange();
		}   
	});
	$('#maturityDate').width('90%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	$('#annualInterestRate').width('90%').kendoNumericTextBox({
		format: "0.#0 '%'",
		min: 0,
		change: function(e) {
			onAnnualInterestRateChange();
		}
	});
	$('#principalBalance').width('90%').kendoNumericTextBox();
	$('#interestExpected').width('90%').kendoNumericTextBox();
	$('#interestBalance').width('90%').kendoNumericTextBox();		
	$('#maturitySum').width('90%').kendoNumericTextBox();		
	dismisChequeDetails();	

	$('#save').click(function () {
		saveclientInvestmApplication();
	});	
}

var onPaymentTypChange = function () {
    var id = $("#paymentType").data("kendoComboBox").value();
    var exist = false;
	
	var cheque = $("#checkNo").data("kendoMaskedTextBox");
	var bank = $("#bank").data("kendoComboBox");

	//Retrieve value enter validate
    for (var i = 0; i < paymentModes.length; i++) {
        if (paymentModes[i].ID == id) {
            exist = true;
			if(id == 1){
				dismisChequeDetails();
			}else{
				displayChequeDetails();
			}
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid paymeny type', 'ERROR');
        $("#paymentType").data("kendoComboBox").value("");
		$("#checkNo").removeAttr('readonly');
		$("#bank").removeAttr('readonly');
    }
}
function dismisChequeDetails(){
	$("#chequeDet").hide();
	$("#checkNo").data("kendoMaskedTextBox").value("");
	$("#bank").data("kendoComboBox").value("");
}
function displayChequeDetails(){
	$("#checkNo").data("kendoMaskedTextBox").value("");
	$("#bank").data("kendoComboBox").value("");
	$("#chequeDet").show();
}
var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            exist = true;
			getClientPicture(id);
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}
var onProductChange = function () {
    var id = $("#investmentProduct").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < depositTypes.length; i++) {
        if (depositTypes[i].depositTypeID == id) {
            exist = true;
			setRepaymentMode();
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Product', 'ERROR');
        $("#investmentProduct").data("kendoComboBox").value("");
    }
}
function populateUI() {
    var cl = $('#client').data('kendoComboBox');
    var amt = $('#amountInvested').data('kendoNumericTextBox');
	var payTyp = $('#paymentType').data('kendoComboBox');
	var ban = $('#bank').data('kendoComboBox');
	var cheq = $('#checkNo').data('kendoMaskedTextBox');
	
	cl.value(clientInvestmRecieptDet.clientInvestmentReceipt.clientId);
	amt.value(clientInvestmRecieptDet.amountReceived);
	payTyp.value(clientInvestmRecieptDet.paymentModeId);
	ban.value(clientInvestmRecieptDet.bankId);
	cheq.value(clientInvestmRecieptDet.chequeNumber);
    getClientPicture(clientInvestmRecieptDet.clientInvestmentReceipt.clientId);
	
	cl.enable(false);
	amt.enable(false);
	payTyp.enable(false);
	ban.enable(false);
	cheq.enable(false);
}

function retrieveValues() {
    clientInvestmApplication.clientID = $('#client').data('kendoComboBox').value();
    clientInvestmApplication.amountInvested = $('#amountInvested').data('kendoNumericTextBox').value();
    clientInvestmApplication.firstDepositDate = $('#depositDate').data('kendoDatePicker').value();
    clientInvestmApplication.depositTypeID = $('#investmentProduct').data('kendoComboBox').value();
    clientInvestmApplication.principalBalance = $('#amountInvested').data('kendoNumericTextBox').value();
    clientInvestmApplication.annualInterestRate = $('#annualInterestRate').data('kendoNumericTextBox').value();	
    clientInvestmApplication.interestRate = (clientInvestmApplication.annualInterestRate/12);
    clientInvestmApplication.period = $('#depositPeriod').data('kendoComboBox').value();
    clientInvestmApplication.maturityDate = $('#maturityDate').data('kendoDatePicker').value();
    clientInvestmApplication.staffID = $('#relationshipOfficer').data('kendoComboBox').value();
    clientInvestmApplication.interestExpected = $('#interestExpected').data('kendoNumericTextBox').value();
	clientInvestmApplication.clientInvestmentReceiptDetailId = receiptDetailId;

	clientInvestmApplication.depositAdditionals = [];
	clientInvestmApplication.depositAdditionals.push({
		depositDate : $('#depositDate').data('kendoDatePicker').value(),
		depositAmount : $('#amountInvested').data('kendoNumericTextBox').value(),
		principalBalance : $('#amountInvested').data('kendoNumericTextBox').value(),
		bankID : $('#bank').data('kendoComboBox').value(),
		checkNo : $('#checkNo').data('kendoMaskedTextBox').value(),
		modeOfPaymentID : $('#paymentType').data('kendoComboBox').value(),
		fxRate : clientInvestmApplication.interestRate,
		naration : $('#narration').data('kendoMaskedTextBox').value(),
	});
}

function saveclientInvestmApplication() {

    var validator = $("#myform").kendoValidator().data("kendoValidator");
    if (!validator.validate()) {
        alert('One or More Fields are Empty');
    } else {
        displayLoadingDialog();
        retrieveValues();
        saveToServer();
    }
}

function saveToServer() {
    displayLoadingDialog();
    $.ajax({
        url: clientInvestmentApiUrl + "/PostDeposit",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(clientInvestmApplication),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		successDialog('Investment received successfuly.',
            'SUCCESS', function () { window.location = '/dash/home.aspx'; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

function getProduct(id){
    for (var i = 0; i < products.length; i++) {
        if (products[i].productId == id) {
            return products[i].productName;
        }
    } 
} 

var period = 0;
//Calculate Maturity Date
function OnPeriodChange(){
	period = $('#depositPeriod').data('kendoComboBox').value();
	
	var exist = false;
	for(var i = 0;i<depositPeriods.length;i++){
		if(depositPeriods[i].value == period){
			var d= $('#depositDate').data('kendoDatePicker').value();	
	
			var da = new Date(d.getFullYear(),d.getMonth(),d.getDate());
			if(da != null && da != 'undefined' && period > 0){
				var maturityDate = new Date(da);
				maturityDate.setDate(maturityDate.getDate() + depositPeriods[i].days);
				$('#maturityDate').data('kendoDatePicker').value(maturityDate);
				calculateExpectedInterest();
			}
			break;
		}
	}	
}

//Calculate Interest Rate
function onAnnualInterestRateChange(){
    var annualRate = $('#annualInterestRate').data('kendoNumericTextBox').value();

    if(annualRate != null && annualRate != 'undefined'){
        //var monthylyRate = annualRate/12;
        //$('#monthlyInterestRate').data('kendoNumericTextBox').value(monthylyRate);
        calculateExpectedInterest();
    }
}

//Calculate Interest Rate
function onMonthlyInterestRateChange(){
    var monthlyRate = $('#monthlyInterestRate').data('kendoNumericTextBox').value();

    if(monthlyRate != null && monthlyRate != 'undefined'){
        var yearlyRate = monthlyRate*12;
        $('#annualInterestRate').data('kendoNumericTextBox').value(yearlyRate);
        calculateExpectedInterest();
    }
}

//Calculate Interest Rate
function calculateExpectedInterest(){
    var rate = 0;
    //var monthlyRate = $('#monthlyInterestRate').data('kendoNumericTextBox').value();
    var annualRate = $('#annualInterestRate').data('kendoNumericTextBox').value();
    var period = $('#depositPeriod').data('kendoComboBox').value();
    var amount = $('#amountInvested').data('kendoNumericTextBox').value();

    if(amount!=null&&amount!='undefined'){
        if (annualRate > 0) {
            rate = annualRate;
        } 
		
		var periodInDays = 0;

		for(var i = 0;i<depositPeriods.length;i++){
			if(depositPeriods[i].value == period){
				periodInDays = depositPeriods[i].days;
				break;
			}
		}
                
        var interestAmount = amount * (periodInDays / 365.0) * (rate/100.0);
        $('#interestExpected').data('kendoNumericTextBox').value(interestAmount);
        $('#maturitySum').data('kendoNumericTextBox').value(interestAmount + amount);
		$('#principalBalance').data('kendoNumericTextBox').value(amount);
		$('#interestBalance').data('kendoNumericTextBox').value(0);
    }
}

function getClientPicture(id) {
    displayLoadingDialog();
    $.ajax({
        url: clientsApiUrl + "/GetClientImage/"+id,
        type: 'GET',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		displayClientImage(data);
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
		//warningDialog("Error while retrieving client Image", 'ERROR');
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

function setRepaymentMode(){

}
function displayClientImage(data){
	$("#clientImage").attr("src","data:image/png;base64,"+data);
}


