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
						url: investmentReceiptApiUrl + '/Get',
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
                    id: 'clientInvestmentReceiptId',
                    fields: {
                            clientInvestmentReceiptId: { type: 'number' },
                            clientId: { type: 'number' }
						} //fields
                } //model
            } //schema
        }, //datasource
        columns: [
			{ field: 'clientId', title: 'Client', template: '#= getClient(clientId) #' }
		],
	    toolbar: [{ name: "create", className: 'createNew', text: 'Receive New Invesment..'}],
		pageable: {
			pageSize: 10,
			pageSizes: [10, 25, 50, 100, 1000],
			previousNext: true,
			buttonCount: 5
		},
		detailTemplate: 'Receipt Details: <div class="grid"></div>',
		detailInit: grid_detailInit
	});
	$(".createNew").click(function () {
        window.location = "/Deposit/InvestmentReceipt";
    });
}
function grid_detailInit(e) {
    e.detailRow.find(".grid").kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
						if (typeof (e.data.clientInvestmentReceiptDetails) === "undefined") {
							e.data.clientInvestmentReceiptDetails = [];
						}
						entries.success(e.data.clientInvestmentReceiptDetails);
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'clientInvestmentReceiptDetailId',
                    fields: {
						clientInvestmentReceiptDetailId: { type: 'number' },
						clientInvestmentReceiptId: { type: 'number' },
						receiptDate: { type: 'date', validation: { required: true } },
						amountReceived: { type: 'number', validation: { required: true } },
						paymentModeId: { validation: { required: true } },
						bankId: { validation: { required: false } },
						chequeNumber: { type: 'string' }
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
			{ field: 'receiptDate', title: 'Receipt Date', format: "{0:dd-MMM-yyyy}" },
            { field: 'amountReceived', title: 'Amount', format: "{0: #,###.#0}" },
            { field: 'paymentModeId', title: 'Payment Mode', template: '#= getPaymentMode(paymentModeId) #' },
            { field: 'bankId', title: 'Bank', template: '#= getBank(bankId) #' },
            { field: 'chequeNumber', title: 'Cheque No.' },
            { command: [applyInvestmentButton] }
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		mobile: true
    }).data("kendoGrid");
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
	if(id==null){return "";}
	var exist = false;
    for (var i = 0; i < banks.length; i++) {
		var b = banks[i];
        if (banks[i].bankId == id){ 
			exist = true;
            return banks[i].bankName;
        }
	}
	return "";
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
