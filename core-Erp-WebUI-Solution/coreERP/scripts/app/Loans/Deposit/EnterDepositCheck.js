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
var bankApiUrl = coreERPAPI_URL_Root + "/crud/banks";


//Declaration of variables to store records retrieved from the database
var clientChecks = {};
var clients = {};
var banks = {};

var minCheckDate = new Date();
    minCheckDate.setDate(minCheckDate.getDate() + 1);

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


var clientAjax = $.ajax({
    url: clientApiUrl + '/GetAllDepositClients',
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

//Get All Deposit Clients
function loadForm() {
	
	$.when(clientAjax, bankAjax)
        .done(function (dataClient, dataBank) {
            clients = dataClient[2].responseJSON;
            banks = dataBank[2].responseJSON;

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

        var gridData = $("#grid").data().kendoGrid.dataSource.view();

		if (confirm('Are you sure you want save check(s)?')) {
            displayLoadingDialog();
			clientChecks.clientCheckDetails = [];
            saveGridData(gridData);

			saveClientChecks();				
            } else {
                smallerWarningDialog('Please review and save later', 'NOTE');
            }
	});
}

function saveGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            clientChecks.clientCheckDetails.push(data[i]);
        }
    }
    else {
	clientChecks.clientCheckDetails.push(data[0]);
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
			change: onClientChange,
			optionLabel: ""
	});	
}


var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            getClientChecks(id);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Cliet', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}

//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(clientChecks.clientCheckDetails);
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
                        clientCheckDetailId: { type: 'number' },
                        clientCheckId: { type: 'number' },
                        checkNumber: { type: 'string' },
                        bankId: {  },
                        checkAmount: { type: 'number', validation: { min: 1 } },
                        checkDate: { type: 'date' , validation: { min: minCheckDate }}
						} //fields
                } //model
            } //schema
        }, //datasource
		editable: 'popup',
        columns: [
			{ field: 'checkNumber', title: 'Cheque Number' },
            { field: 'bankId', title: 'Bank', editor: bankEditor, template: '#= getBank(bankId) #' },
            { field: 'checkAmount', title: 'Amount', format: "{0: #,###.#0}" },
            { field: 'checkDate', title: 'Due Date', format: "{0:dd-MMM-yyyy}"},
            { command: ['edit','destroy'] }
       ],
	    toolbar: [{ name: 'create', text: 'Add New Cheque' }]

    });
}


//retrieve values from from Input Fields and save 
function saveClientChecks() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    clientChecks.clientId = $('#client').data('kendoComboBox').value();
}

//Save to server function
function saveToServer() {
		
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: depositCheckApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(clientChecks),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Client Check(s) Saved Successfully:', 'SUCCESS', function () { window.location = '/dash/home.aspx'; });        
    }).error(function (error) {
		dismissLoadingDialog();
		successDialog('Please Continue', 'SUCCESS', function () { window.location = '/dash/home.aspx'; }); 
	});
	
}//func saveToServer

function bankEditor(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoComboBox({
        dataSource: banks,
        dataValueField: "bankId",
        dataTextField: "bankName",
        optionLabel: ""
    });
}

function getBank(id) {
    for (var i = 0; i < banks.length; i++) {
        if (banks[i].bankId == id) {
            return banks[i].bankName;
        }
    }
}

function getClientChecks(id) {
    displayLoadingDialog();

    $.ajax(
    {
        url: depositCheckApiUrl + '/Get/' + id,
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

