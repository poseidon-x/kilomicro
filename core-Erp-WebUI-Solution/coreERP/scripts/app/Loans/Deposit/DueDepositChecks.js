
"use strict";

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var depositCheckApiUrl = coreERPAPI_URL_Root + "/crud/DepositCheck";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/DepositClient";
var bankApiUrl = coreERPAPI_URL_Root + "/crud/banks";
var bankAccountApiUrl = coreERPAPI_URL_Root + "/crud/bank";


//Declaration of variables to store records retrieved from the database
var clientChecks = {};
var clients = {};
var banks = {};
var bankAccounts = {};
var dueChecks = {};


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

var bankAccountAjax = $.ajax({
    url: bankAccountApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var dueChecksAjax = $.ajax({
    url: depositCheckApiUrl + '/GetAllDueChecks',
    type: 'Post',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

$(function () {
    displayLoadingDialog();
	loadData();	
});



function loadData() {
    $.when(clientAjax, bankAjax, bankAccountAjax, dueChecksAjax)
     .done(function (dataClient, dataBank, dataBankAccount, dataDueCheck) {
        clients = dataClient[2].responseJSON;
        banks = dataBank[2].responseJSON;
		bankAccounts = dataBankAccount[2].responseJSON;
		dueChecks = dataDueCheck[2].responseJSON;
		
		//Load Form
        loadForm();
    });
}

function loadForm() {
    $('#tabs').kendoTabStrip();
    renderGrid();
    dismissLoadingDialog();
	
	$('#save').click(function (event) {

        var gridData = $("#setupGrid").data().kendoGrid.dataSource.view();

		if (confirm('Are you sure you want save Changes?')) {
            displayLoadingDialog();
			clientChecks.clientCheckDetails = [];
            saveGridData(gridData);

			saveChecks();				
        } else {
            smallerWarningDialog('Please review and save changes later', 'NOTE');
        }
	});
}

function saveGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
			if(data[i].cashed){clientChecks.clientCheckDetails.push(data[i]);}
            
        }
    }
    else {
		if(data[i].cashed){clientChecks.clientCheckDetails.push(data[0]);}
	
	}
}

function renderGrid() {
    $('#setupGrid').kendoGrid({
		dataSource: {
            transport:  {
				read: function(entries) {
                    entries.success(dueChecks);
                },
                create: function(entries) {
					entries.data.sourceBankAccountId = source;
                    entries.success(entries.data);
                },
                update: function (entries) {
					//var data = entries.data;
					entries.data.sourceBankAccountId = source;
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
                        clientCheckId: { type: 'number', editable: false },
                        checkNumber: { type: 'string', editable: false },						
                        bankId: { type: 'number', editable: false },
                        checkAmount: { type: 'number', editable: false },
                        checkDate: { type: 'date', editable: false },
                        cashed: { type: 'boolean', editable: true   },
						sourceBankAccountId: { type: 'number' }
					}, //fields
                }, //model
            }, //schema
        }, //datasource
		editable: 'inline',
        columns: [
			{ field: 'checkNumber', title: 'Cheque Number'},
            { field: 'bankId', title: 'Bank', template: '#= getBank(bankId) #' },
            { field: 'checkAmount', title: 'Amount', format: "{0: #,###.#0}" },
            { field: 'checkDate', title: 'Due Date', format: "{0:dd-MMM-yyyy}" },
            //{ field: 'cashed', title: 'Mark as cashed', editor: cashedEditor, 
			//  template: '<input type="checkbox" disabled="disabled" data-bind="checked: cashed" #= cashed? checked="checked":"" #/>'
				//template: '<input type="checkbox" #= cashed ? \'checked="checked"\' : "" # class="chkbx" />'
			//}
			{ field: 'cashed',title: 'Mark as cashed', width: 120, filterable: { multi: true, dataSource: [{ cashed: false }, { cashed: true }]} },
			{ field: 'sourceBankAccountId', title: 'Deposit Account',  editor: bankAccountEditor, template: '#= getBankAccount(sourceBankAccountId) #' },
			{ command: ['edit'] }			
       ],
		toolbar: [{ className: 'addNewCheck', text: 'Enter New Investment Check' }]	   
    });
}

function getBank(id) {
    for (var i = 0; i < banks.length; i++) {
        if (banks[i].bankId == id) {
            return banks[i].bankName;
        }
    }
}


//Save to server function
function saveChecks() {
		
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: depositCheckApiUrl + '/ProcessClearedCheck',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(clientChecks),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Client Check(s) Processed Successfully:', 'SUCCESS', function () { window.location = '/dash/home.aspx'; });        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer


function bankAccountEditor(container, options) {
    $('<input type="text" id="sourceBankAccount" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoComboBox({
        dataSource: bankAccounts,
        dataValueField: "bank_acct_id",
        dataTextField: "bank_acct_desc",
		change: onBankAccountChanged,
        optionLabel: ""
    });
}

function getBankAccount(sourceBankAccountId) {
	if(sourceBankAccountId != null)
    for (var i = 0; i < bankAccounts.length; i++) {
        if (bankAccounts[i].bank_acct_id == sourceBankAccountId) {
            return bankAccounts[i].bank_acct_desc;
        }
    }
}

var source;

var onBankAccountChanged = function(){
	source = $('#sourceBankAccount').data('kendoComboBox').value();
}

