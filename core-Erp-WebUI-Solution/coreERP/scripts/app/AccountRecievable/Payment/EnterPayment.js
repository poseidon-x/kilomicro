//******************************************//
//  			AR PAYMENT - JAVASCRIPT             		//
// 		CREATOR: EMMANUEL OWUSU(MAN)			//
//				WEEK: MAY(18TH - 22ND), 2015	    		//
//******************************************//

"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var paymentApiUrl = coreERPAPI_URL_Root + "/crud/ArPayment";
var invoiceApiUrl = coreERPAPI_URL_Root + "/crud/ArInvoice";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var customerApiUrl = coreERPAPI_URL_Root + "/crud/ArInvoice";//fetch all invoice customers
var paymentMethodApiUrl = coreERPAPI_URL_Root + "/crud/paymentMethod";
var creditMemoApiUrl = coreERPAPI_URL_Root + "/crud/ArCreditMemo";




//Declaration of variables to store records retrieved from the database
var arPayment = {};
var arInvoices = {};
var currencies = {};
var customers = {};
var paymentMethods = {};
var creditMemoes = {};
var currentInvoiceBalance = 0;
var amount;
var remainingAmount;
var paymentCreditBal = 0;
var creditMemoName = "Credit Memo";


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//Declare a variable and store shrinkageBatch table ajax call in it
var paymentAjax = $.ajax({
    url: paymentApiUrl + '/Get/' + arPaymentId,
    type: 'Get',
    contentType: 'application/json',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var paymentMethodAjax = $.ajax({
    url: paymentMethodApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var customersAjax = $.ajax({
    url: customerApiUrl + '/GetInvoiceCustomers',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store currency table ajax call in it
var currencyAjax = $.ajax({
    url: currencyApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(paymentAjax, currencyAjax, customersAjax, paymentMethodAjax)
        .done(function (dataPayment, dataCurrency, dataCustomer, dataPaymentMethod) {
            arPayment = dataPayment[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
            customers = dataCustomer[2].responseJSON;
            paymentMethods = dataPaymentMethod[2].responseJSON;

            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() 
{
	document.getElementById('payment').innerHTML ='<input type="text" id="paymentMethodNumber" class="form-control" required data-required-msg="Invalid" />';


    renderControls();

    //If arInvoiceId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (arPayment.arPaymentId > 0) {
        populateUi();
    }
    dismissLoadingDialog();

    //Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {
	
        var validator = $("#myform").kendoValidator().data("kendoValidator");
		
        //Retrieve & save Grid data
        var paymentLineGridData = $("#grid").data().kendoGrid.dataSource.view();		

        if (!validator.validate()) {
            smallerWarningDialog('One or More Fields has Invalid value', 'ERROR');
        } else {
            if (paymentLineGridData.length > 0) {
                displayLoadingDialog();
				savePaymentLinGridData(paymentLineGridData);
                savePayment();
                    }
                    else {
                        smallerWarningDialog('Please Add Payment Details', 'NOTE');
                    }
        }
    });
}


function savePaymentLinGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            arPayment.arPaymentLines.push(data[i]);
			paymentCreditBal += data[i].amountPaid;
        }
    }
    else arPayment.arPaymentLines.push(data[0]);
}


//Apply kendo Style to the input fields
function renderControls() {

    $("#customer").width("75%")
    .kendoComboBox({
        dataSource: customers,
        dataValueField: "customerId",
        dataTextField: "customerName",
		change: onCustomerChange,
        optionLabel: ""
    });
	
		
    $("#paymentDate").width("75%")
	    .kendoDatePicker({
	        format: "dd-MMM-yyyy",
	        parseFormats: ["dd-MM-yyyy", "yyyy-MM-dd", "yyyy-MMM-dd"]
	    });

    $("#currency").width("75%")
    .kendoComboBox({
        dataSource: currencies,
        dataValueField: "currency_id",
        dataTextField: "major_name",
        optionLabel: ""
    });

    $("#paymentMethod").width("75%")
	    .kendoComboBox({
	        dataSource: paymentMethods,
	        dataValueField: "paymentMethodID",
	        dataTextField: "paymentMethodName",
			change: onPaymentChanged,
	        optionLabel: ""
	});


    $("#paymentMethodNumber").width("75%")
        .kendoMaskedTextBox();

    $("#totalAmountPaid").width("75%")
     .kendoNumericTextBox({
		format: "#,##0.00",
		change: onTotalAmountChange,
		downArrowText: "Less",
		upArrowText: "More"
	});
	
	var currency = $("#currency").data("kendoComboBox");
	currency.enable(false);
	var paymentMethod = $("#paymentMethod").data("kendoComboBox");
	paymentMethod.enable(false);
	var paymentMethod = $("#paymentMethod").data("kendoComboBox");
	paymentMethod.enable(false);
	var paymentMethodNumber = $("#paymentMethodNumber").data("kendoMaskedTextBox");
	paymentMethodNumber.enable(false);
	var totalAmountPaid = $("#totalAmountPaid").data("kendoNumericTextBox");
	totalAmountPaid.enable(false);

}


var onTotalAmountChange = function() {
    amount = $("#totalAmountPaid").data("kendoNumericTextBox").value();
	remainingAmount = amount;

	document.getElementById("id") == "";
	renderGrid();
	
	
	//$('#grid').data('kendoGrid').dataSource.read();

}

var onCustomerChange = function() {
	var currentDate = new Date();	
	var currency = $("#currency").data("kendoComboBox");
	var paymentMethod = $("#paymentMethod").data("kendoComboBox");
	var totalAmountPaid = $("#totalAmountPaid").data("kendoNumericTextBox");
	

    var id = $("#customer").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < customers.length; i++) {
        if (customers[i].customerId == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Customer', 'ERROR');
        $("#customer").data("kendoComboBox").value("");
    }else
	{
		displayLoadingDialog();
		var customerId = $("#customer").data("kendoComboBox").value();
				
		$.ajax({
        url: invoiceApiUrl + '/GetInvoiceByCustomer/?custId=' + customerId,
        type: 'Get',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
		}).done(function (data) {
			 $('#paymentDate').data("kendoDatePicker").value(currentDate);
		    	arInvoices = data;

			
			creditMemoes = {};
			document.getElementById('payment').innerHTML ='<input type="text" id="paymentMethodNumber" class="form-control" required data-required-msg="Invalid" />';
			
			$("#paymentMethodNumber").width("75%")
				.kendoMaskedTextBox();
				
			var paymentMethodNumber = $("#paymentMethodNumber").data("kendoMaskedTextBox");
			currency.enable(true);
			paymentMethod.enable(true);
			paymentMethod.enable(true);
			paymentMethodNumber.enable(true);
			totalAmountPaid.enable(true);
			
			currentInvoiceBalance = 0;
			amount = 0;
			remainingAmount = 0;

			dismissLoadingDialog();
        }).error(function (error) {
			dismissLoadingDialog();
            alert(JSON.stringify(error));

        });
	}
}

var onPaymentChanged = function() {
	var paymentMethodNumber = $("#paymentMethodNumber").data("kendoMaskedTextBox");
	var currency = $("#currency").data("kendoComboBox");

	
	
	var paymentId = $("#paymentMethod").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < paymentMethods.length; i++) {
        if (paymentMethods[i].paymentMethodID == paymentId) {		
            exist = true;
			if(paymentMethods[i].paymentMethodName == creditMemoName)
			{
				setUpCreditMemoControl();
			}			
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Payment Method', 'ERROR');
        $("#paymentMethod").data("kendoComboBox").value("");
    }

}

	


//render Grid
function renderGrid() {
    //arPayment.arPaymentLines.NewField = "invoiceBalance";
    $("#grid").kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(arPayment.arPaymentLines);
                },
                create: function(entries) {
				var data = entries.data;
					data.balanceLeft = currentInvoiceBalance - data.amountPaid;						
                    amount - data.amountPaid;
					//updateInvoiceList(data.arInvoiceId);
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
                    entries.success();
                }
            }, //transport
            schema: {
                model: {
                    id: "arPaymentLineId",
                    fields: {
                        arPaymentId: { type: 'number', defaultValue: arPayment.arPaymentId },
                        arPaymentLineId: { type: 'number', editable: false },
                        arInvoiceId: { editable: true},
                        amountPaid: { type: 'number' },
                        balanceLeft: {editable: false, validation: { min: 0} }
                    } //fields
                } //model
            } //schema
        }, //datasource
		editable: 'popup',
        columns: [
			{ field: 'arInvoiceId', title: 'Invoice #', editor: invoiceEditor, template: '#= getInvoiceEditor(arInvoiceId) #' },
            { field: 'amountPaid', title: 'Amount Paid', format: "{0:0.00}", editor: amountPaidEditor },
            { field: 'balanceLeft', title: 'Balance After Payment', format: "{0:0.00}"},
			{ command: ['edit', 'destroy'], width: '70' }
        ],
        toolbar: [{ name: 'create', text: 'Add Memo Details', }],
    });
}


//retrieve values from from Input Fields and save 
function savePayment() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
	arPayment.customerId = $("#customer").data("kendoComboBox").value();
    arPayment.paymentDate = $("#paymentDate").data("kendoDatePicker").value();
    arPayment.totalAmountPaid = $("#totalAmountPaid").data("kendoNumericTextBox").value();
    arPayment.paymentMethodId = $("#paymentMethod").data("kendoComboBox").value();

    if ($("#paymentMethod").data('kendoComboBox').text() == "Cheque") {
        arPayment.checkNumber = $("#paymentMethodNumber").data("kendoMaskedTextBox").value();
        arPayment.creditCardNumber = "";
        arPayment.mobileMoneyNumber = "";
    } else if ($("#paymentMethod").data('kendoComboBox').value() == "Credit/Debit Card") {
        arPayment.checkNumber = "";
        arPayment.creditCardNumber = $("#paymentMethodNumber").data("kendoMaskedTextBox").value();
        arPayment.mobileMoneyNumber = "";
    } else if ($("#paymentMethod").data('kendoComboBox').value() == "Mobile Money") {
        arPayment.checkNumber = "";
        arPayment.creditCardNumber = "";
        arPayment.mobileMoneyNumber = $("#paymentMethodNumber").data("kendoMaskedTextBox").value();
    }
    arPayment.currencyId = $("#currency").data("kendoComboBox").value();
	arPayment.paymentCreditBalance = paymentCreditBal;
	
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: paymentApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(arPayment),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Payment Successfully Saved', 'SUCCESS', function () { window.location = "/ArPayment/Payments/"; });
        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer


//retrieve unit of Measurement value from the Grid pop and display on the Grid
function getUnitOfMeasurement(id) {
    for (i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id)
            return unitOfMeasurements[i].unitOfMeasurementName;
    }
}



function invoiceEditor(container, options) {
    $('<input type="text" id="arInvoices" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("95%")
    .kendoComboBox({
        dataSource: arInvoices,
        dataValueField: "arInvoiceId",
        dataTextField: "invoiceNumberNBalance",
        change: onArInvoicesChange,
        optionLabel: ""
    });
}

function amountPaidEditor(container, options) {
    $('<input type="text" id="amountPaid" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("95%")
    .kendoNumericTextBox({
		format: "#,##0.00",
		//change: onTotalAmountChange,
		downArrowText: "Less",
		upArrowText: "More",
		min: remainingAmount
    });
	
	var invoiceAmount = $("#amountPaid").data("kendoNumericTextBox");
	invoiceAmount.readonly();
}

function getInvoiceEditor(id) {
    for (i = 0; i < arInvoices.length; i++) {
        if (arInvoices[i].arInvoiceId == id) 
        return arInvoices[i].invoiceNumberNBalance;
    }
}

function getCustomers(id) {
    for (i = 0; i < arInvoices.length; i++) {
        if (arInvoices[i].arInvoiceId == id) 
        return arInvoices[i].invoiceNumber;
    }
}

function customerExist(id) {
    var exist = false;
	var idNum = JSON.stringify(id);
	alert(idNum);
	alert(currentCustomers.length);
	
	if(currentCustomers.length > 0){
		for (i = 0; i < currentCustomers.length; i++)
		{
			alert("Checking");
			alert(i);
			alert(currentCustomers[i]);
			if (currentCustomers[i].customerId == idNum) {
							
				exist = true;
				alert("Exist");
				break;
			}
		}
	}

    if (!exist) {
        currentCustomers.push(customers[idNum]);
		alert(customers[idNum]);
			alert("Push first");
    }
}

var onArInvoicesChange = function() {

    var id = $("#arInvoices").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < arInvoices.length; i++) {
        if (arInvoices[i].arInvoiceId == id) {
		currentInvoiceBalance = arInvoices[i].balance
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Invoice', 'ERROR');
        $("#arInvoices").data("kendoComboBox").value("");
    }else
	{
		var invoiceAmount = $("#amountPaid").data("kendoNumericTextBox");
		invoiceAmount.value(currentInvoiceBalance);
		
		if(currentInvoiceBalance < amount){
			//remainingAmount = currentInvoiceBalance;
			invoiceAmount.value(currentInvoiceBalance);
		}else{
			invoiceAmount.value(currentInvoiceBalance);
		}
	}
}

function updateInvoiceList(id) {
	var invoice =[];
	
    for (i = 0; i < arInvoices.length; i++) {
        if (arInvoices[i].arInvoiceId != id){ 
        invoice.Add(arInvoices[i]);
		break;
		}
    }
	
	arInvoices = invoice;
	$("#arInvoices").data("kendoComboBox").dataSource.read();
}

function setUpCreditMemoControl() {
	var id = $("#customer").data("kendoComboBox").value();
	var currency = $("#currency").data("kendoComboBox");
	
	displayLoadingDialog();
	$.ajax({
		url: creditMemoApiUrl + '/GetMemoByCustomer/' + id,
		type: 'Get',
		beforeSend: function(req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}
	}).done(function (data) {
		creditMemoes = data;
		if(!creditMemoes.length > 0)
		{
			warningDialog("The Current customer does not have a Memo","NOTE");
		}
		else{
		warningDialog(JSON.stringify(creditMemoes),"DATA");
		document.getElementById('payment').innerHTML ='';
		document.getElementById('payment').innerHTML ='<input type="text" id="paymentMethodNumber" class="form-control" required data-required-msg="Invalid" />';
		
			$("#paymentMethodNumber").width("75%")
				.kendoComboBox({
					dataSource: creditMemoes,
					dataValueField: "creditMemoId",
					dataTextField: "memoNumber",
					optionLabel: ""
				});
									
				currency.enable(false);
			}
		dismissLoadingDialog();
	}).error(function (error) {
		dismissLoadingDialog();
		alert(JSON.stringify(error));
	});
}



