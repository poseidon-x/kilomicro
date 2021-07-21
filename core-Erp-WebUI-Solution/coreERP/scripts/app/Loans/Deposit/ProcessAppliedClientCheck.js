//*******************************************
//***   CLIENT DEPOSIT CHECK JAVASCRIPT                
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: AUG 28TH, 2015  	
//*******************************************

"use strict";

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var depositCheckApiUrl = coreERPAPI_URL_Root + "/crud/DepositCheck";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/DepositClient";
var depositApiUrl = coreERPAPI_URL_Root + "/crud/deposit";



//Declaration of variables to store records retrieved from the database
var clientCheckDetail = {};
var clients = {};
var deposits = {};

//variable to track ckeck balance as and when applied
var balanceOnCheck;

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

var clientCheckDetailAjax = $.ajax({
    url: depositCheckApiUrl + '/GetClientCashedCheck/' + clientCheck,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var clientAjax = $.ajax({
    url: clientApiUrl + '/GetAllDepositClients',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var depositAjax = $.ajax({
    url: depositApiUrl + '/GetClientDeposit/' + clientId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Get All Deposit Clients
function loadForm() {
	
	$.when(clientCheckDetailAjax, clientAjax, depositAjax)
        .done(function (dataClientCheckDetailAjax, dataClient, dataDeposit) {
            clientCheckDetail = dataClientCheckDetailAjax[2].responseJSON;			
            clients = dataClient[2].responseJSON;
            deposits = dataDeposit[2].responseJSON;

            //Prepares UI
            prepareUi();
			dismissLoadingDialog();

        });
}

//Function to prepare user interface
function prepareUi() 
{	
    renderControls();
	populateUi();
	renderGrid();

    $('#save').click(function (event) {

        var gridData = $("#grid").data().kendoGrid.dataSource.view();

		if (confirm('Are you sure you want save check(s)?')) {
            displayLoadingDialog();
			clientCheckDetail.checkApplies = [];
            saveGridData(gridData);
			saveCheckApplication();				
            } else {
                smallerWarningDialog('Please review and save later', 'NOTE');
            }
	});
}

function saveGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            clientCheckDetail.checkApplies.push(data[i]);
        }
    }
    else {
	clientCheckDetail.checkApplies.push(data[0]);
	}
}

//Apply kendo Style to the input fields
function renderControls() {
    $("#tabs").width("75%")
        .kendoTabStrip();

    $("#client").width("75%")
		.kendoComboBox({
		    dataSource: clients,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientID",
		    dataTextField: "clientName",
			optionLabel: ""
	});

	$("#checkNo").width("75%")
		.kendoMaskedTextBox();
		
	$("#balance").width("75%")
		.kendoNumericTextBox();	
}

//Apply kendo Style to the input fields
function populateUi() {
	//warningDialog(JSON.stringify(clientCheckDetail), "Data");
	$('#client').data('kendoComboBox').value(clientCheckDetail.clientCheck.clientId);	
	$('#checkNo').data('kendoMaskedTextBox').value(clientCheckDetail.checkNumber);
	$('#balance').data('kendoNumericTextBox').value(clientCheckDetail.balance);
	balanceOnCheck = clientCheckDetail.balance;
}

//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(clientCheckDetail.checkApplies);
                },
                create: function(entries) {
					var data = entries.data;
					//if()
     				//	balanceOnCheck -= data.amountApplied;
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
                    id: 'checkApplyId',
                    fields: {
                        checkApplyId: { type: 'number' },
                        clientCheckDetailId: { type: 'number' },
                        depositId: { type: 'string' },
                        amountApplied: { type: 'number', validation:{ min:1 } },
						} //fields
                } //model
            } //schema
        }, //datasource
		editable: 'inline',
        columns: [
			{ field: 'depositId', title: 'Investment', editor: depositEditor, template: '#= getDeposit(depositId) #' },
            { field: 'amountApplied', title: 'Amount', editor: amountEditor, format: '{0: #,###.#0}' },
            { command: ['edit','destroy'] }
       ],
	    toolbar: [{ name: 'create', text: 'Apply to Investment' }]
    });
}


//retrieve values from from Input Fields and save 
function saveCheckApplication() {
    saveToServer();
}

//Save to server function
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: depositCheckApiUrl + '/ProcessAppliedChecks',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(clientCheckDetail),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Client Check Application Saved Successfully:', 'SUCCESS', function () { window.location = '/dash/home.aspx'; });        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

function depositEditor(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("100%")
    .kendoComboBox({
        dataSource: deposits,
        dataValueField: "depositID",
        dataTextField: "depositNo",
        optionLabel: ""
    });
}

function amountEditor(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("100%")
    .kendoNumericTextBox({
        min: 1,
		max: balanceOnCheck
    });
}

function getDeposit(id) {
    for (var i = 0; i < deposits.length; i++) {
        if (deposits[i].depositID == id) {
            return deposits[i].depositNo;
        }
    }
}

function getClientChecks(id) {
    displayLoadingDialog();

    $.ajax(
    {
        url: depositCheckApiUrl + '/Get/' + clientId,
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        clientChecks = data;
        //Clear grid content
        document.getElementById('grid').innerHTML = '';
        renderGrid();

        dismissLoadingDialog();
    }).error(function (error) {
        dismissLoadingDialog();
        alert(JSON.stringify(error));
    });
}

