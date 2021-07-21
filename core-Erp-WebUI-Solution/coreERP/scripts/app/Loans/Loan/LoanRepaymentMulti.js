//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: OCT 20TH, 2015  	
//*******************************************


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var loanRepaymentApiUrl = coreERPAPI_URL_Root + "/crud/LoanRepayment";
var chargeTypeApiUrl = coreERPAPI_URL_Root + "/crud/LoanRepaymentType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";





//Declaration of variables to store records retrieved from the database
var loanRepayment = {};
var chargeTypes = {};
var clients = {};
var clientLoans = {};
var paymentId = -1;


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

var loanRepaymentAjax = $.ajax({
    url: loanRepaymentApiUrl + '/GetMulti',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var chargeTypeAjax = $.ajax({
    url: chargeTypeApiUrl + '/GetOthers',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Get All Deposit Clients
function loadForm() {
	
    $.when(loanRepaymentAjax, chargeTypeAjax)
        .done(function (dataLoanRepayment, dataChargeType) {
            loanRepayment = dataLoanRepayment[2].responseJSON;
            chargeTypes = dataChargeType[2].responseJSON;          
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
		var cln = $('#client').data('kendoDropDownList').text();
		if (confirm("Are you sure you want to apply charge to "+cln)) {
			saveCharge();				
		} 
		else
		{
            smallerWarningDialog('Please review and apply later', 'NOTE');
        }
	});
}

//Apply kendo Style to the input fields
function renderControls() {
	$("#client").keyup(function(){
		var txt = $("#client").data("kendoMaskedTextBox").value();
		
		if(txt.length > 2){
			var input = {searchString:txt};
			getClients(input);
		}
	});
	
	$("#client").width("90%")
	.kendoMaskedTextBox();
	$("#paymentDate").width('90%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        value: new Date()
	});
	$("#loan").width('90%').kendoDropDownList({
	    dataSource: clientLoans,
	    filter: "contains",
	    suggest: true,
	    dataValueField: "loanId",
	    dataTextField: "loanNo",
	    highlightFirst: true,
	    optionLabel: ""
	});
	clientLoans
	renderGrid()
}

function renderGrid() {
    $('#tabs').kendoTabStrip({});
    $('#paymentGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(loanRepayment.payments);
                },
                create: function (entries) {
                    entries.data.paymentId = paymentId;
                    paymentId--;
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success(entries.data);
                },
                destroy: function (entries) {
                    entries.success(entries.data);
                },
                parameterMap: function (data) { return JSON.stringify(data); }
            },
            schema: {
                model: {
                    id: "paymentId",
                    fields: {
                        paymentId: { editable: true, type: "number" },
                        repaymentTypeID: { editable: true },
                        amount: { editable: true, type: "number" }
                    }
                }
            },
        },		
        columns: [
			{
			    field: 'repaymentTypeID',
			    title: 'Payment Type',
			    editor: chargeTypeEditor,
			    template: '#= getChargeType(repaymentTypeID) #'
			},
			{
			    field: 'amount',
			    title: 'Amount',
			    format: '{0: #,###.#0}'
			},
			{
			    command: [
                  { name: "edit" },
                  { name: "destroy" }
			    ],
			    width: 180
			}
        ],

        toolbar: [
            { name: "create", text: "Add Payment" },
        ],
        editable: "popup"
    });
}

function chargeTypeEditor(container, options) {//editor
    $('<input required data-text-field="repaymentTypeName" data-value-field="repaymentTypeID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            optionLabel: "",
            highlightFirst: true,
            suggest: true,
            dataSource: {
                data: chargeTypes,
            },
            dataValueField: "repaymentTypeID",
            dataTextField: "repaymentTypeName",
            template: '<span>#: repaymentTypeName #</span>',
            filter: "contains",
        });
    var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}

function getChargeType(id){
    for (var i = 0; i < chargeTypes.length; i++) {
        if (chargeTypes[i].repaymentTypeID == id) {
            return chargeTypes[i].repaymentTypeName;
        }
    } 
}

//retrieve values from from Input Fields and save 
function saveCharge() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    loanRepayment.loanId = $('#loan').data('kendoDropDownList').value();
    loanRepayment.paymentDate = $("#paymentDate").data('kendoDatePicker').value();
    loanRepayment.payments = $("#paymentGrid").data().kendoGrid.dataSource.view();
}

//Save to server function
function saveToServer() {
	displayLoadingDialog();	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: loanRepaymentApiUrl + '/PostMulti',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(loanRepayment),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Repayment successfully received:', 'SUCCESS', 
		function () { window.location = '/ln/setup/postTill.aspx'; });
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
	
}//func saveToServer

function getClients(input) {
	displayLoadingDialog();
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetAllClientsBySearch',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(input),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		if(data.length>0){
			clients = data;
			setUpCombox();
		}
		else{
			clients = {};
			warningDialog('No client found that match the searched criteria', 'NOTE');
		}        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
	});	
}

function getClientUndisbursedLoans(clientId) {
    displayLoadingDialog();
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetClientUndisbursedLoans/' + clientId,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        clientLoans = data;
        if (clientLoans.length > 0) {
            $('#loan').data('kendoDropDownList').value("");
            $('#loan').data('kendoDropDownList').setDataSource(clientLoans);
        }
        else{
            warningDialog("The selected client doesn't have any undisbursed", 'NOTE');
        }

    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

function setUpCombox(){
	$("#client")
	.kendoDropDownList({
		dataSource: clients,
		filter: "contains",
		suggest: true,
		dataValueField: "clientID",
		dataTextField: "clientNameWithAccountNO",
		highlightFirst: true,
		optionLabel: "----- Select Loan ------",
		change: onClientChange
	});
}

var onClientChange = function () {
    var id = $("#client").data("kendoDropDownList").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            getClientUndisbursedLoans(id);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Client', 'ERROR');
        $("#client").data("kendoDropDownList").value("");
    }
}