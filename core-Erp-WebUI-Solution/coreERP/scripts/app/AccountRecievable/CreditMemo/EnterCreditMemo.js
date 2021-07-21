//******************************************//
//  			CREDIT MEMO - JAVASCRIPT             		//
// 		CREATOR: EMMANUEL OWUSU(MAN)			//
//				WEEK: JUNE(1ST - 5TH), 2015					//
//******************************************//


"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var creditMemoApiUrl = coreERPAPI_URL_Root + "/crud/ArCreditMemo";
var invoiceApiUrl = coreERPAPI_URL_Root + "/crud/ArInvoice";
var paymentApiUrl = coreERPAPI_URL_Root + "/crud/ArPayment";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var creditMemoReasonApiUrl = coreERPAPI_URL_Root + "/crud/ArCreditMemoReason";



//Declaration of variables to store records retrieved from the database
var arCreditMemo = {};
var arInvoices = {};
var arPayment = {};
var currencies = {};
var arCreditMemoReason = {};
var selectedInvoice ={};
var selectedPayment ={};

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


var creditMemoAjax = $.ajax({
    url: creditMemoApiUrl + '/Get/' + creditMemoId,
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

var creditMemoReasonAjax = $.ajax({
    url: creditMemoReasonApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(creditMemoAjax, currencyAjax, creditMemoReasonAjax)
        .done(function (dataCreditMemo, dataCurrency, dataCreditMemoReason) {
            arCreditMemo = dataCreditMemo[2].responseJSON;
			currencies = dataCurrency[2].responseJSON;
            arCreditMemoReason = dataCreditMemoReason[2].responseJSON;
			
            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() 
{

	$('#paymentMemo').click(function (event) 
	{
		if(document.getElementById('paymentMemo').checked) 
		{
			if(JSON.stringify(arPayment) === '{}'){
				displayLoadingDialog();
				var creditMemoReason = $("#creditMemoReason").data("kendoComboBox");

						$.ajax({
							url: paymentApiUrl + '/Get',
							type: 'Get',
							beforeSend: function(req) {
								req.setRequestHeader('Authorization', "coreBearer " + authToken);
							}
							}).done(function (data) {
								arPayment = data;
								prepareMemoTypeControl();
								creditMemoReason.enable(true);
								dismissLoadingDialog();
							}).error(function (error) {
								dismissLoadingDialog();
								alert(JSON.stringify(error));

							});
			}else{
				prepareMemoTypeControl();
				creditMemoReason.enable(true);
			}
		}	
	});

    $('#invoiceMemo').click(function (event) {
	if(document.getElementById('invoiceMemo').checked){
			if(JSON.stringify(arInvoices) === '{}'){
				var creditMemoReason = $("#creditMemoReason").data("kendoComboBox");

						displayLoadingDialog();
						$.ajax({
							url: invoiceApiUrl + '/Get',
							type: 'Get',
							beforeSend: function(req) {
								req.setRequestHeader('Authorization', "coreBearer " + authToken);
							}
							}).done(function (data) {
								arInvoices = data;
								prepareMemoTypeControl();
								creditMemoReason.enable(true);
								dismissLoadingDialog();
							}).error(function (error) {
								dismissLoadingDialog();
								alert(JSON.stringify(error));

							});
			}else{
				prepareMemoTypeControl();
				creditMemoReason.enable(true);
			}
		}	
	});	

	
		
    //If arInvoiceId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (arCreditMemo.creditMemoId > 0) {
        renderControls();
        //populateUi();
        dismissLoadingDialog();
    } else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        renderControls();
        dismissLoadingDialog();
    }

	//Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {
	
        var validator = $("#myform").kendoValidator().data("kendoValidator");
		
        if (!validator.validate()) {
            smallerWarningDialog('One or More Fields are Empty', 'ERROR');
        } else {
			if(document.getElementById('invoiceMemo').checked) {
				var memoLineGridData = $("#grid").data().kendoGrid.dataSource.view();					

				
				if (memoLineGridData.length > 0) {
					displayLoadingDialog();
					saveCreditMemoLineGridData(memoLineGridData);

				//Retrieve & save Grid data
					saveMemo();
                }else {
                        smallerWarningDialog('Please Add Shrinkage Details', 'NOTE');
                }
				
			}else if(document.getElementById('paymentMemo').checked){
				displayLoadingDialog();


				saveMemo();
			}
    //});
		}
	});
}

//Apply kendo Style to the input fields
function renderControls() {

		$("#currency").width("75%")
		.kendoComboBox({
			dataSource: currencies,
			dataValueField: "currency_id",
			dataTextField: "major_name",
			change: onCurrencyChange,		
			optionLabel: ""
		});
		
		$("#creditMemoReason").width('75%')
			.kendoComboBox({
			dataSource: arCreditMemoReason,
			dataValueField: 'creditMemoReasonId',
			dataTextField: 'creditMemoReasonName',
			change: onReasonChange,		
			optionLabel: ""
		});	
		
		$("#totalAmountReturned").width("75%")
		 .kendoNumericTextBox({
			format: "#,##0.00"
		});
		
		$("#totalBalanceLeft").width("75%")
		 .kendoNumericTextBox({
			format: "#,##0.00"
		});	
		
		//$('#tabs').kendoTabStrip();
}

function saveCreditMemoLineGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            arCreditMemo.creditMemoLines.push(data[i]);
        }
    }
    else {
	arCreditMemo.creditMemoLines.push(data[0]);
	}
}


function prepareMemoTypeControl() {
	if(document.getElementById('paymentMemo').checked){		
			document.getElementById('memoTypeTitle').innerHTML ='Payment Number';
			document.getElementById('memoTypeInput').innerHTML ='<input type="text" id="payment" class="form-control" required data-required-msg="Invalid" />';
			document.getElementById('memoDateTitle').innerHTML ='Memo Date';
			document.getElementById('memoDateInput').innerHTML ='<input type="text" id="memoDate" class="form-control" required data-required-msg="Invalid" readonly />';
			document.getElementById('grid').innerHTML ='';
			$("#currency").data("kendoComboBox").value("");
			$("#creditMemoReason").data("kendoComboBox").value("");
			$("#totalAmountReturned").data("kendoNumericTextBox").value(0);
			$("#totalBalanceLeft").data("kendoNumericTextBox").value(0);

			$("#payment").width('75%')
			.kendoComboBox({
				dataSource: arPayment,
				dataValueField: "arPaymentId",
				dataTextField: "paymentNumber",
				change: onPaymentChanged,				
				optionLabel: "",
			});
			
	}else if(document.getElementById('invoiceMemo').checked){
			document.getElementById('memoTypeTitle').innerHTML ='Invoice Number';
			document.getElementById('memoTypeInput').innerHTML ='<input type="text" id="invoice" class="form-control" required data-required-msg="Invalid" />';
			document.getElementById('memoDateTitle').innerHTML ='Memo Date';
			document.getElementById('memoDateInput').innerHTML ='<input type="text" id="memoDate" class="form-control" required data-required-msg="Invalid" readonly />';

			$("#currency").data("kendoComboBox").value("");
			$("#creditMemoReason").data("kendoComboBox").value("")
			$("#totalAmountReturned").data("kendoNumericTextBox").value(0);
			$("#totalBalanceLeft").data("kendoNumericTextBox").value(0);
			
			
			$("#invoice").width('75%')
			.kendoComboBox(
			{
				dataSource: arInvoices,
				dataValueField: "arInvoiceId",
				dataTextField: "invoiceNumber",
				change: onInvoiceChanged,
				optionLabel: "",
			});
	}
			
			$("#memoDate").width('75%')
			.kendoDatePicker({
				format: 'dd-MMM-yyyy',
				parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
				min: dat, // sets min date 	
				max: new Date(), // sets max date	
				value: 	new Date()
			});
			var dat = new Date();
			dat.setDate(dat.getDate() - 1);;
}

var onPaymentChanged = function() {
	//Retrieve value enter validate
    var id = $("#payment").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < arPayment.length; i++) {
        if (arPayment[i].arPaymentId == id) {
            exist = true;
			selectedPayment = arPayment[i];
			var amountCovered = 0;
			
			for(var i = 0; i < selectedPayment.arPaymentLines.length; i++){
				amountCovered +=  selectedPayment.arPaymentLines[i].amountPaid;
			}
			var overPaymentAmount = selectedPayment.totalAmountPaid - amountCovered;
			$("#currency").data("kendoComboBox").value(selectedPayment.currencyId);
			$("#totalAmountReturned").data("kendoNumericTextBox").value(overPaymentAmount);
			$("#totalBalanceLeft").data("kendoNumericTextBox").value(overPaymentAmount);
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Invoice', 'ERROR');
        $("#invoice").data("kendoComboBox").value("");
    }
}

var onInvoiceChanged = function() {
	//Retrieve value enter validate
    var id = $("#invoice").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < arInvoices.length; i++) {
        if (arInvoices[i].arInvoiceId == id) {
            exist = true;
			selectedInvoice = arInvoices[i];
			$("#currency").data("kendoComboBox").value(selectedInvoice.currencyId);
			renderGrid();
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Invoice', 'ERROR');
        $("#invoice").data("kendoComboBox").value("");
    }
}	
	
var onCurrencyChange = function() {
	//Retrieve value enter validate
    var id = $("#currency").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < currencies.length; i++) {
        if (currencies[i].currency_id == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Currency', 'ERROR');
        $("#currency").data("kendoComboBox").value("");
    }
}	

var onReasonChange = function() {

    var id = $("#creditMemoReason").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < arCreditMemoReason.length; i++) {
        if (arCreditMemoReason[i].creditMemoReasonId == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Memo Reason', 'ERROR');
        $("#creditMemoReason").data("kendoComboBox").value("");
    }
}


var unitPrice;
var memoAmount = 0;

//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(arCreditMemo.creditMemoLines);
                },
                create: function(entries) {
				var data = entries.data;
						if(!data.amountReturned > 0) {
							data.amountReturned = data.quantityReturned * unitPrice;
						   	memoAmount += data.amountReturned;
							$('#totalAmountReturned').data('kendoNumericTextBox').value(memoAmount);
							$('#totalBalanceLeft').data('kendoNumericTextBox').value(memoAmount);
						}
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
					alert("deleting");
                    entries.success();
                }
            }, //transport
            schema: {
                model: {
                    id: 'creditMemoLineId',
                    fields: {
                        creditMemoId: { type: 'number', defaultValue: arCreditMemo.creditMemoId },
                        creditMemoLineId: { type: 'number', editable: false },
                        arinvoiceLineId: { },
                        quantityReturned: { type: 'number',},						
                        amountReturned: { type: 'number',  format: "{0:c}", editable: false },
                    }, //fields
                }, //model
            }, //schema
        }, //datasource
		editable: 'popup',
        columns: [
			{ field: 'arinvoiceLineId', title: 'Description', editor: invoiceEditor, template: '#= getInvoiceEditor(arinvoiceLineId) #'},
            { field: 'quantityReturned', title: 'Quantity Returned',  editor: quantityReturnedEditor},
            { field: 'amountReturned', title: 'Amount',  format: "{0:#,##0.00}" },
			{ command: ['destroy'] , width: 110}			
       ],
		toolbar: [{ name: 'create', text: 'Add Order Line' }]	   
    });
}


//retrieve values from from Input Fields and save 
function saveMemo() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
	if(document.getElementById('paymentMemo').checked)
	{arCreditMemo.arPaymentId = $('#payment').data('kendoComboBox').value(); }
	if(document.getElementById('invoiceMemo').checked)
	{arCreditMemo.arInvoiceId = $('#invoice').data('kendoComboBox').value(); }
    arCreditMemo.memoDate = $('#memoDate').data('kendoDatePicker').value();
	arCreditMemo.currencyId =  $('#currency').data('kendoComboBox').value();
	arCreditMemo.creditMemoReasonId =  $('#creditMemoReason').data('kendoComboBox').value();
    arCreditMemo.totalAmountReturned = $('#totalAmountReturned').data('kendoNumericTextBox').value();
    arCreditMemo.totalBalanceLeft = $('#totalBalanceLeft').data('kendoNumericTextBox').value();
   }

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: creditMemoApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(arCreditMemo),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Memo Successfully Saved \n Credit Memo Number:'+data.memoNumber, 'SUCCESS'); //, function () { window.location = "/ArInvoice/Invoices/"; }
        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer



function invoiceEditor(container, options) {
    $('<input type="text" id="arInvoiceLines" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoComboBox({
        dataSource: selectedInvoice.arInvoiceLines,
        dataValueField: "arInvoiceLineId",
        dataTextField: "description",
        change: onInvoiceLineChange,
        optionLabel: ""
    });
}

var maxQuantityReturned;

function quantityReturnedEditor(container, options) {
    $('<input type="text" id="quantityReturned" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoNumericTextBox({
		format: "#,##0.00",
		min: 0,
		max: maxQuantityReturned
    });
}

function getInvoiceEditor(id) {
    for (var i = 0; i < selectedInvoice.arInvoiceLines.length; i++) {
        if (selectedInvoice.arInvoiceLines[i].arInvoiceLineId == id) 
        return selectedInvoice.arInvoiceLines[i].description;
    }
}

var onInvoiceLineChange = function() {
	//Retrieve value enter validate
    var id = $("#arInvoiceLines").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < selectedInvoice.arInvoiceLines.length; i++) {
        if ( selectedInvoice.arInvoiceLines[i].arInvoiceLineId == id) {
			unitPrice = selectedInvoice.arInvoiceLines[i].unitPrice;
			maxQuantityReturned = selectedInvoice.arInvoiceLines[i].quantity;
            exist = true;
            break;
        }
    }


var quantityRet = $("#quantityReturned").data("kendoNumericTextBox");


quantityRet.max(10)	
	
	
    if (!exist) {
        warningDialog('Invalid Item', 'ERROR');
        $("#arInvoiceLines").data("kendoComboBox").value("");
    }
}






