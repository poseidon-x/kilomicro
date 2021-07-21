'use strict';

var depositUpgradeApiUrl = coreERPAPI_URL_Root + "/crud/DepositUpgrade";
var clientsApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var depositApiUrl = coreERPAPI_URL_Root + "/crud/deposit";
var banksApiUrl = coreERPAPI_URL_Root + "/crud/banks";
var depositTypeApiUrl = coreERPAPI_URL_Root + "/crud/depositType"; 
var modeOfPaymentApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";

var depositUpgrade = {};
var deposit = {};
var clients = {};
var depositTypes = {};
var banks = {};
var modeOfpayments = {};
var depositPeriods = [
    { value:60, month:"60 Days" },
    { value:91, month:"91 Days" },
    { value:182, month:"182 Days" },	
    { value:365, month:"365 Days" }
];
var depositUpgradeAjax = $.ajax({
    url: depositUpgradeApiUrl + "/Get/" + -1,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var clientAjax = $.ajax({
    url: clientsApiUrl + "/GetDepositClient",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var depositAjax = $.ajax({
    url: depositApiUrl + "/GetDeposit/"+previousDepositId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var depositTypeAjax = $.ajax({
    url: depositTypeApiUrl + "/GetDepositUpgradeableDepositTypes/"+previousDepositId,
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
    url: banksApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
function loadData() {

    $.when(depositUpgradeAjax,depositAjax,clientAjax,depositTypeAjax,paymentModeAjax,bankAjax)
        .done(function (dataDepositUpgrade,dataDeposit,dataClient,dataDepositType,dataPaymentMode,dataBank) {
            depositUpgrade = dataDepositUpgrade[2].responseJSON;
            deposit = dataDeposit[2].responseJSON;			
            clients = dataClient[2].responseJSON;
            depositTypes = dataDepositType[2].responseJSON;
			modeOfpayments = dataPaymentMode[2].responseJSON;
            banks = dataBank[2].responseJSON;
            var ui = new oUI();
            //Prepares UI
            ui.prepareUI();
			if(deposit.depositID > 0){
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

var oUI = (function () {
    function oUI() {
    }
    oUI.prototype.prepareUI = function () {
				
        $('#deposit').width('90%').kendoComboBox({
            dataSource: clients,
            dataValueField: 'depositId',
            dataTextField: 'clientNameWithDepositNO',
            filter: "contains",
            highlightFirst: true,
            suggest: true,
            ignoreCase: true,
            animation: {
                close: { effects: "fadeOut zoom:out", duration: 200 },
                open: { effects: "fadeIn zoom:in", duration: 200 }
            },
			//change: onClientChange,
            optionLabel: ''
        });
        $('#upgradeDate').width('90%').kendoDatePicker({
            format: '{0:dd-MMM-yyyy}',
            parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
            change:function(e) {
                OnPeriodChange();
            } 
        });
        $('#balance').width('90%').kendoNumericTextBox();
        $('#topUpAmount').width('90%').kendoNumericTextBox({
			min:0,
			change:function(e) {
                calculateExpectedInterest();
            } 
		});
        $('#upgradeDepositType').width('90%').kendoComboBox({
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
        $('#annualInterestRate').width('90%').kendoNumericTextBox({
			format: "0.#0 '%'",
            min: 0,
			change: function(e) {
                onAnnualInterestRateChange();
            }
		});
		$('#interest').width('90%').kendoNumericTextBox();
        $('#maturitySum').width('90%').kendoNumericTextBox();		
		$('#period').width('90%').kendoComboBox({
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
		$("#topupPaymentMode").width("90%")
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
			change: onPaymentTypChange,
			optionLabel: ""
		});
		$("#checkNo").width("90%")
		.kendoMaskedTextBox();
		$("#bank").width("90%")
		.kendoComboBox({
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
		
        $('#save').click(function () {
            saveUpgradeAccount();
        });
    }    

    return oUI;
})();

var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            exist = true;
			//getClientPicture(id);
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}
var onPaymentTypChange = function () {
    var id = $("#topupPaymentMode").data("kendoComboBox").value();
    var exist = false;
	
	var cheque = $("#checkNo").data("kendoMaskedTextBox");
	var bank = $("#bank").data("kendoComboBox");

	//Retrieve value enter validate
    for (var i = 0; i < modeOfpayments.length; i++) {
        if (modeOfpayments[i].ID == id) {
            exist = true;
			if(id == 1){
				bank.value("");
				cheque.value("");
				bank.enable(false);
				cheque.enable(false);
			}else{
				bank.enable(true);
				cheque.enable(true);
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
var onProductChange = function () {
    var id = $("#upgradeDepositType").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < depositTypes.length; i++) {
        if (depositTypes[i].depositTypeID == id) {
            exist = true;
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Product', 'ERROR');
        $("#upgradeDepositType").data("kendoComboBox").value("");
    }
}
function populateUI() {
	getClientPicture(deposit.clientID);
    $('#deposit').data('kendoComboBox').value(deposit.depositID);
    $('#balance').data('kendoNumericTextBox').value(deposit.principalBalance+deposit.interestBalance);
}
function retrieveValues() {
    depositUpgrade.previousDepositId = $('#deposit').data('kendoComboBox').value();
    depositUpgrade.balanceCD = $('#balance').data('kendoNumericTextBox').value();
    depositUpgrade.topUpAmount = $('#topUpAmount').data('kendoNumericTextBox').value();	
    depositUpgrade.upgradeDate = $('#upgradeDate').data('kendoDatePicker').value();
    depositUpgrade.upgradeDepositTypeID = $('#upgradeDepositType').data('kendoComboBox').value();
    depositUpgrade.annualInterestRate = $('#annualInterestRate').data('kendoNumericTextBox').value();
    depositUpgrade.depositPeriodInDays = $('#period').data('kendoComboBox').value();
    depositUpgrade.maturityDate = $('#maturityDate').data('kendoDatePicker').value();
	depositUpgrade.topupPaymentModeId = $('#topupPaymentMode').data('kendoComboBox').value();
	if(depositUpgrade.topupPaymentModeId==2){
		depositUpgrade.topupCheckNo = $("#checkNo").data("kendoMaskedTextBox").value();
		depositUpgrade.topupBankId = $("#bank").data("kendoComboBox").value();
	}
    depositUpgrade.interestRate = depositUpgrade.annualInterestRate/12;
}
function saveUpgradeAccount() {

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
        url: depositUpgradeApiUrl + "/PostDepositUpgrade",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(depositUpgrade),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		successDialog('Investment Upgrade successfuly, New Account no: '+data.depositNo,
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

//Calculate Interest Rate
function onAnnualInterestRateChange(){
    var annualRate = $('#annualInterestRate').data('kendoNumericTextBox').value();

    if(annualRate != null && annualRate != 'undefined'){
		calculateExpectedInterest();
    }
}
function calculateExpectedInterest(){
    var rate = 0;
    var annualRate = $('#annualInterestRate').data('kendoNumericTextBox').value();
    var period = $('#period').data('kendoComboBox').value();
	var amount = 0;
    var bal = $('#balance').data('kendoNumericTextBox').value();
    var topUp = $('#topUpAmount').data('kendoNumericTextBox').value();
	if(bal != null && bal != 'undefined'){amount+=bal;}
	if(topUp != null && topUp != 'undefined'){amount+=topUp;}

	rate = annualRate;
	var periodInDays = 0;
	if(period != null || period != 'undefined')
	{periodInDays = period;}
			
	var InterestAmount = amount * (periodInDays / 365.0) * (rate/100.0);
	$('#interest').data('kendoNumericTextBox').value(InterestAmount);
	$('#maturitySum').data('kendoNumericTextBox').value(InterestAmount+amount);
}

function OnPeriodChange(){
	period = $('#period').data('kendoComboBox').value();
	period = parseInt(period);
	var d= $('#upgradeDate').data('kendoDatePicker').value();	
	
	var da = new Date(d.getFullYear(),d.getMonth(),d.getDate());
    if(da != null && da != 'undefined' && period != 'undefined' && period > 0){
        var maturityDate = new Date(da);
		maturityDate.setDate(maturityDate.getDate() + period);
        //maturityDate.setMonth(maturityDate.getMonth() + period);
        $('#maturityDate').data('kendoDatePicker').value(maturityDate);
        calculateExpectedInterest();
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
function displayClientImage(data){
	$("#clientImage").attr("src","data:image/png;base64,"+data);
}
