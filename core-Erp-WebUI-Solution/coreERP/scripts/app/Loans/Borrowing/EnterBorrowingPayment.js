//***********************************************************//
//  	     EnterBorrowingPayment - JAVASCRIPT                
// 				CREATOR: EMMANUEL OWUSU(MAN)    	   
//		        DATE: AUG(10TH - 14th), 2015  		  
//*********************************************************//

"use strict";

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var borrowingApiUrl = coreERPAPI_URL_Root + "/crud/Borrowing";
var borrowingClientApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingClients";
var modeOfPaymentApiUrl = coreERPAPI_URL_Root + "/crud/modeOfPayment";
var repaymentTypeApiUrl = coreERPAPI_URL_Root + "/crud/repaymentType";
var bankApiUrl = coreERPAPI_URL_Root + "/crud/Banks";


//Declaration of variables to store records retrieved from the database
var borrowing = {};
var clients = {};
var selectedBorrowing = {};
var modeOfPayments = {};
var repaymentTypes = {};
var banks = {};
var borrowingRepayment = {};

var borrowingAjax = $.ajax({
    url: borrowingApiUrl + '/Get/' + id,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var borrowinRepaymentgAjax = $.ajax({
    url: borrowingApiUrl + '/NewBorrowingPayment',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var clientAjax = $.ajax({
    url: borrowingClientApiUrl + '/GetApprovedBorrowingClient',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var modeOfPaymentAjax = $.ajax({
    url: modeOfPaymentApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var repaymentTypeAjax = $.ajax({
    url: repaymentTypeApiUrl + '/Get',
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

function loadForm() {	
	$.when(borrowingAjax, borrowinRepaymentgAjax, clientAjax, modeOfPaymentAjax, repaymentTypeAjax, bankAjax)
        .done(function (dataBorrowing, dataBorrowinRepayment, dataClient, dataModeOfPayment, dataRepaymentType, dataBank) {
            borrowing = dataBorrowing[2].responseJSON;
            borrowingRepayment = dataBorrowinRepayment[2].responseJSON;
			clients = dataClient[2].responseJSON;
            modeOfPayments = dataModeOfPayment[2].responseJSON;
            repaymentTypes = dataRepaymentType[2].responseJSON;
            banks = dataBank[2].responseJSON;
			
            //Prepares UI
            prepareUi();
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
    populateUi();

    $('#savePayment').click(function (event) {

        var validator = $("#myform").kendoValidator().data("kendoValidator");


		//if (!validator.validate()) {
        //    smallerWarningDialog('A form input is empty or has invalid value', 'ERROR');
    //}
    //else {
			if (confirm('Are you sure you want make this payment to borrowing Account?')) {
                displayLoadingDialog();

				savePayment();				
            } else {
                smallerWarningDialog('Please review and make payment later', 'NOTE');
            }
		//}
	});
}

//Apply kendo Style to the input fields
function renderControls() {
    $("#client").width("100%")
		.kendoComboBox({
		    dataSource: clients,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientID",
		    dataTextField: "clientName",
			//change: onClientChange,
			optionLabel: ""
	});
    
    $("#borrowing").width('100%')
		.kendoComboBox({
		    dataSource: borrowing,
		    dataValueField: "borrowingId",
		    dataTextField: "borrowingNo",
		    //change: onBorrowingChange,
		    optionLabel: ""
	});

    $("#repaymentType").width('100%')
		.kendoComboBox({
		    dataSource: repaymentTypes,
		    dataValueField: "repaymentTypeID",
		    dataTextField: "repaymentTypeName",
		    //change: onBorrowingChange,
		    optionLabel: ""
	});    
    
    $("#repaymentDate").width('100%')
	.kendoDatePicker({
		format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
        enable: false,
		value: new Date()
	});
    
    $("#modeOfPayment").width('100%')
		.kendoComboBox({
		    dataSource: modeOfPayments,
		    dataValueField: "ID",
		    dataTextField: "Description",
		    change: onModeOfPaymentChange,
		    optionLabel: ""
	});	
	
	$("#amountPaid").width('100%')
		.kendoNumericTextBox({
			min: 0,
			enable: false
    });	
}

function populateUi(){

    $("#client").data("kendoComboBox").value(borrowing.clientId);
    $("#borrowing").data("kendoComboBox").value(borrowing.borrowingNo);

}


var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
			setUpBorrowingControl(id);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Cliet', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    } 
}

var onModeOfPaymentChange = function () {
    var id = $("#modeOfPayment").data("kendoComboBox").value();
	var decs = $("#modeOfPayment").data("kendoComboBox").text();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < modeOfPayments.length; i++) {
        if (modeOfPayments[i].ID == id) {
			if(decs.toLowerCase().indexOf("cheque") >= 0){
				renderBankAndChequeControls();				
			}else{			
				document.getElementById("bankTitle").innerHTML = '';
				document.getElementById("bankInput").innerHTML = '';
				document.getElementById("chequeNumberTitle").innerHTML = '';
				document.getElementById("chequeNumberInput").innerHTML = '';
			}
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Cliet', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    } 
}

//retrieve values from from Input Fields and save 
function savePayment() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    borrowingRepayment.borrowingId = borrowing.borrowingId;
    borrowingRepayment.modeOfPaymentId = $('#modeOfPayment').data('kendoComboBox').value();
    borrowingRepayment.repayementDate = $('#repaymentDate').data('kendoDatePicker').value();
    borrowingRepayment.repaymentTypeId = $('#repaymentType').data('kendoComboBox').value();
    borrowingRepayment.amountPaid = $('#amountPaid').data('kendoNumericTextBox').value();

	var modeOfPay = $('#modeOfPayment').data('kendoComboBox').text();
	if(modeOfPay.toLowerCase().indexOf("cheque") >= 0){
		borrowingRepayment.bankId = $('#bank').data('kendoComboBox').value();
		borrowingRepayment.checkNo = $('#cheque').data('kendoMaskedTextBox').value();
	}

}

//Save to server function
function saveToServer() {
		
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: borrowingApiUrl + '/BorrowingPayment',
        type: 'Put',
        contentType: 'application/json',
        data: JSON.stringify(borrowingRepayment),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Borrowing Account Payment Received:', 'SUCCESS', function () { window.location = '/dash/home.aspx'; });        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

function setUpBorrowingControl(id) {
	displayLoadingDialog();
		$.ajax(
        {
            url: borrowingApiUrl + '/GetClientApprovedBrws/' + id,
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            borrowings = data;

            //Set the returned data to loan datasource
			$("#borrowing").data("kendoComboBox").value("");
            $("#borrowing").data("kendoComboBox").setDataSource(borrowings);
            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
}

function renderBankAndChequeControls(){
	document.getElementById("bankTitle").innerHTML = 'Bank';
	document.getElementById("bankInput").innerHTML = '<input type="text" id="bank" required data-required-msg="Invalid" />';
	document.getElementById("chequeNumberTitle").innerHTML = 'Cheque Number';
	document.getElementById("chequeNumberInput").innerHTML = '<input type="text" id="cheque" />';
	
	$("#bank").width('100%')
		.kendoComboBox({
		    dataSource: banks,
		    dataValueField: "bankId",
		    dataTextField: "bankName",
		    //change: onModeOfPaymentChange,
		    optionLabel: ""
	});	
	
	$("#cheque").width('100%')
		.kendoMaskedTextBox();
}


