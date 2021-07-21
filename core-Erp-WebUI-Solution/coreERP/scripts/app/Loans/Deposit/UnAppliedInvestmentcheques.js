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
    url: investmentReceiptApiUrl + '/Get' ,
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
    $("#tabs").width("100%")
        .kendoTabStrip();
    renderGrid();
}


//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport: {
					read: {
						url: investmentReceiptApiUrl + '/GetUnAppliedChecks',
						type: 'Get',
						contentType: "application/json",						
						beforeSend: function (req) {
							req.setRequestHeader('Authorization', "coreBearer " + authToken);
						}
					},
                    parameterMap: function (data) { return JSON.stringify(data); }
			}, //transport
            schema: {
                    model: {
                    id: 'clientCheckDetailId',
                    fields: {
						clientInvestmentReceiptDetailId: { type: 'number' },
						clientInvestmentReceiptId: { type: 'number' },
						clientId: { type: 'number', validation: { required: true } },
						receiptDate: { type: 'date', validation: { required: true } },
						amountReceived: { type: 'number', validation: { required: true } },
						paymentModeId: { validation: { required: true } },
						bankId: { validation: { required: false } },
						chequeNumber: { type: 'string' }
						} //fields
					},
				}, //schema
        }, //datasource
        sortable: true,
        columns: [
		
			{ field: 'clientId', title: 'Client',template: '#= getClient(clientId) #' },		
			{ field: 'receiptDate', title: 'Receipt Date', format: "{0:dd-MMM-yyyy}" },
            { field: 'amountReceived', title: 'Amount', format: "{0: #,###.#0}" },
            { field: 'paymentModeId', title: 'Payment Mode', template: '#= getPaymentMode(paymentModeId) #' },
            { field: 'bankId', title: 'Bank', template: '#= getBank(bankId) #' },
            { field: 'chequeNumber', title: 'Cheque No.' },
            { command: [applyInvestmentButton] }
		],
	    toolbar: [{ name: "create", className: 'createNew', text: 'Receive New Invesment..'}],
		pageable: {
			pageSize: 10,
			pageSizes: [10, 25, 50, 100, 1000],
			previousNext: true,
			buttonCount: 5
		},
	});
	
	$(".createNew").click(function () {
        window.location = "/Deposit/InvestmentReceipt";
    });
}


var applyInvestmentButton = {
	name: "gridapply",
	text: "Apply..",
	click: function(e) {
		var tr = $(e.target).closest("tr"); // get the current table row (tr)
		var data = this.dataItem(tr);
		window.location = "/Deposit/ApplyInvestment/?Id=-1&receiptDetailId="+data.clientInvestmentReceiptDetailId.toString();
	}
};

function getBank(id) {
    for (var i = 0; i < banks.length; i++) {
        if (banks[i].bankId == id) {
            return banks[i].bankName;
        }
    }
}
function getClient(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            return clients[i].clientNameWithAccountNO;
        }
    }
}
function getPaymentMode(id) {
    for (var i = 0; i < PaymentModes.length; i++) {
        if (PaymentModes[i].ID == id) {
            return PaymentModes[i].Description;
        }
    }
}
function getClient(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            return clients[i].clientNameWithAccountNO;
        }
    }
}
