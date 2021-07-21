//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: OCT 20TH, 2015  	
//*******************************************


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var investmentReceiptApiUrl = coreERPAPI_URL_Root + "/crud/InvestmentReceipt";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var paymentModeApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var bankApiUrl = coreERPAPI_URL_Root + "/crud/banks";


//Declaration of variables to store records retrieved from the database
var investmentReceipt = {};
var clients = {};
var PaymentModes = {};
var banks = {};

var minCheckDate = new Date();
    minCheckDate.setDate(minCheckDate.getDate() + 1);

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


var clientAjax = $.ajax({
    url: clientApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var investmentReceiptAjax = $.ajax({
    url: investmentReceiptApiUrl + '/Get/' + investmentReceiptId,
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
var paymentModeAjax = $.ajax({
    url: paymentModeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Get All Deposit Clients
function loadForm() {
	
    $.when(clientAjax,investmentReceiptAjax,bankAjax,paymentModeAjax)
        .done(function (dataClient, dataInvestmentReceipt,dataBank,dataPaymentMode) {
            clients = dataClient[2].responseJSON;
            investmentReceipt = dataInvestmentReceipt[2].responseJSON;
            banks = dataBank[2].responseJSON;
            PaymentModes = dataPaymentMode[2].responseJSON;            

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
		if (confirm('Are you sure you want save Receipts(s)?')) {
            displayLoadingDialog();
			saveInvestmentReceipts();				
		} else
		{
            smallerWarningDialog('Please review and save later', 'NOTE');
        }
	});
}

//Apply kendo Style to the input fields
function renderControls() {
    $("#tabs").width("100%")
        .kendoTabStrip();

    $("#client").width("100%")
		.kendoComboBox({
		    dataSource: clients,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientID",
		    dataTextField: "clientNameWithAccountNO",
			change: onClientChange,
			filter: "contains",
			highlightFirst: true,
			suggest: true,
			ignoreCase: true,
			optionLabel: ""
	});	
    
}


var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
    $("#receiptGrid").html('');
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            getClientInvestmentReceipt(id);
            //renderGrid();
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}

//render Grid
function renderGrid() {
    $('#receiptGrid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(investmentReceipt.clientInvestmentReceiptDetails);
                },
                create: function(entries) {
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
                    id: 'clientCheckDetailId',
                    fields: {
                            clientInvestmentReceiptDetailId: { type: 'number' },
                            clientInvestmentReceiptId: { type: 'number' },
                            receiptDate: { type: 'date', validation: { required: true } },
                            amountReceived: { type: 'number', validation: { required: true } },
                            paymentModeId: { validation: { required: true } },
                            bankId: { validation: { required: false } },
                            chequeNumber: { type: 'string' }
						} //fields
                } //model
            } //schema
        }, //datasource
		editable: 'popup',
        columns: [
			{ field: 'receiptDate', title: 'Receipt Date', format: "{0:dd-MMM-yyyy}" },
            { field: 'amountReceived', title: 'Amount', format: "{0: #,###.#0}" },
            { field: 'paymentModeId', title: 'Payment Mode', editor: paymentModeEditor, template: '#= getPaymentMode(paymentModeId) #' },
            { field: 'bankId', title: 'Bank', editor: bankEditor, template: '#= getBank(bankId) #' },
            { field: 'chequeNumber', title: 'Cheque No.', editor: chequeEditor },
            { command: ['edit','destroy'] }
       ],
	   pageable: {
			pageSize: 10,
			pageSizes: [10, 25, 50, 100, 1000],
			previousNext: true,
			buttonCount: 5,
		},
	    toolbar: [{ name: 'create', text: 'Add New Placement' }]

    });
}


//retrieve values from from Input Fields and save 
function saveInvestmentReceipts() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    investmentReceipt.clientInvestmentReceiptDetails = [];
    investmentReceipt.clientId = $('#client').data('kendoComboBox').value();
    investmentReceipt.clientInvestmentReceiptDetails = $("#receiptGrid").data().kendoGrid.dataSource.view();    
}

//Save to server function
function saveToServer() {
		
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: investmentReceiptApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(investmentReceipt),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Client Investment Receipt Saved Successfully:', 'SUCCESS', function () { window.location = '/Deposit/ClientInvestmentReceipts'; });        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
	
}//func saveToServer

function bankEditor(container, options) {
    $('<input id="bank" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoComboBox({
        dataSource: banks,
        dataValueField: "bankId",
        dataTextField: "bankName",
        optionLabel: ""
    });
}
function chequeEditor(container, options) {
    $('<input id="chequeNo" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoMaskedTextBox();
}
function getBank(id) {
    var exist = false;
    for (var i = 0; i < banks.length; i++) {
        if (banks[i].bankId == id) {
            exist = true;
            return banks[i].bankName;
        }
    }
    return "";
}

function paymentModeEditor(container, options) {
    $('<input id="paymentMode" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoComboBox({
        dataSource: PaymentModes,
        dataValueField: "ID",
        dataTextField: "Description",
		change: onPayModeChange,
        optionLabel: ""
    });
}

function getPaymentMode(id) {
    for (var i = 0; i < PaymentModes.length; i++) {
        if (PaymentModes[i].ID == id) {
            return PaymentModes[i].Description;
        }
    }
}

function getClientInvestmentReceipt(id) {
    displayLoadingDialog();

    $.ajax(
    {
        url: investmentReceiptApiUrl + '/GetClientReceipts/' + id,
        type: 'POST',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        investmentReceipt.clientInvestmentReceiptDetails = data;
        //Clear grid content
        renderGrid();

        dismissLoadingDialog();
    }).error(function (error) {
        dismissLoadingDialog();
        alert(JSON.stringify(error));
    });
}

var onPayModeChange = function(){
	var id = $("#paymentMode").data("kendoComboBox").value();
    var exist = false;
	
	var cheque = $("#chequeNo").data("kendoMaskedTextBox");
	var bank = $("#bank").data("kendoComboBox");

	
	//Retrieve value enter validate
    for (var i = 0; i < PaymentModes.length; i++) {
        if (PaymentModes[i].ID == id) {
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
        $("#paymentMode").data("kendoComboBox").value("");
		bank.enable(true);
		cheque.enable(true);
    }
}

