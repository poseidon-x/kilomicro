//"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanProvisionApiUrl = coreERPAPI_URL_Root + "/crud/LoanProvision";
var LoanApiUrl = coreERPAPI_URL_Root + "/crud/Loans";
var LoanClientsApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";

//Declaration of variables to store records retrieved from the database
var loanProvisions = {};
var loans = {};
var clients = {};
var provisionBatch = {};
var prov = {};


/*
function loadForm() {
	$.ajax({
        url: LoanClientsApiUrl + '/GetRunningLoanClient',
		type: 'Get',
		beforeSend: function (req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}
    }).success(function (data) {
		clients = data;
		
		prepareUi();
		dismissLoadingDialog();
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
	
}*/

//Function to call load form function
$(function () {
    displayLoadingDialog();
    prepareUi();
});


//Function to prepare user interface
function prepareUi() {	
	$('#tabs').kendoTabStrip();
	$("#year").width('80%').kendoDatePicker({
		format: '{0:yyyy}',
		parseFormats: ["yyyy", "yy"],
		depth: "year",
		start: "year",
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		//change: OnMonthChange
    });
	
	$("#month").width('80%').kendoDatePicker({
		format: '{0:MMM}',
		parseFormats: ["MMM", "MM"],
		depth: "month",
		start: "month",
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		//change: OnMonthChange
    });
	dismissLoadingDialog();
	
	$('#initialize').click(function (event) {	
	    displayLoadingDialog();
	
		var mont = $("#month").data("kendoDatePicker").value();
		var year = $("#year").data("kendoDatePicker").value();
		
		prov.provisionDate = new Date(year.getFullYear(),(mont.getMonth()),mont.getDate());
		
		$.ajax({
			url: LoanProvisionApiUrl + '/InitializeProvision',
			type: 'Post',
			contentType: 'application/json',
			data: JSON.stringify(prov),
			beforeSend: function(req) {
				req.setRequestHeader('Authorization', "coreBearer " + authToken);
			}
		}).done(function (data) {
			provisionBatch = data;
			getBatchLoansAndClients(provisionBatch.provisionBatchId);
		}).error(function (xhr, data, error) {
			//On error stop loading Dialog and alert a the specific message
			dismissLoadingDialog();
			warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
		});
	});
}

var request = {
	sort : []
};
//render Grid
function renderGrid() {
	
	prov.provisionDate = provisionBatch.provisionDate;
	request.take = 10;
	request.skip = 0;
	request.page = 10;
	request.provisionDate = provisionBatch.provisionDate;
	
	
    $('#grid').kendoGrid({
            dataSource: {
                transport: {
					read: {
                        url: LoanProvisionApiUrl + "/GetBatchProvision",
                        type: "POST",
						contentType: 'application/json',
						dataType: 'json',
						//data: JSON.stringify(request),
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
					update: {
						url: LoanProvisionApiUrl + "/Put",
						type: "PUT",
						contentType: "application/json",
						dataType: "json",
						beforeSend: function(req) {
							req.setRequestHeader('Authorization', "coreBearer " + authToken);
						}
					},
					parameterMap: function (data, operation) {
						//var data = null;           
						if(operation == 'read')
						{
						  data.provisionDate = provisionBatch.provisionDate;
						  //data = request;
						}
						  return JSON.stringify(data);
                    }
				},
				pageSize: 10,				
                schema: {
					data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "loanProvisionID",
                        fields: {
                            loanProvisionID: { editable: false },
                            loanID: { editable: false  },
							typeOfSecurity: {editable: false },
							principalBalance: {editable: false },
							daysDue: {editable: false },
							proposedAmount: {editable: true },
							securityValue: {editable: false }							
                        }
                    }
                },
				serverPaging: true,
				serverFiltering: true,
				serverSorting: true,
            },
			columns: [
				//{ field: 'loanID', title: 'Account Number', template: '#= getClientAccount(loanID) #' },
				{ field: 'loanID', title: 'Client', template: '#= getClientName(loanID) #' },
				{ field: 'loanID', title: 'Loan', template: '#= getLoan(loanID) #' },
				{ field: 'typeOfSecurity', title: 'Type Of Security' },
				{ field: 'principalBalance', title: 'Outstanding Principal',format:'{0: #,##0.#0}' },
				{ field: 'daysDue', title: 'Days Due' },
				{ field: 'proposedAmount', title: 'Provision', editor: AmountEditor,format:'{0: #,##0.#0}' },
				{ field: 'securityValue', title: 'Security Value',format:'{0: #,##0.#0}' },

				{ command: [ "edit"], width: "100px"   }
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5
			},
			sortable: {
				mode: "multiple",
				allowUnsort: true
            },
            editable: "inline",
        });
}
	


var deleteButton = {
		name: "destroy",
		text: "Delete",
		click: gridDeleteFunction
};
//retrieve values from from Input Fields and save 
function SaveLoanGroup() {
    retrieveValues();
    saveToServer();
}
function retrieveValues() {
    loanGroup.loanGroupName = $('#groupName').data('kendoMaskedTextBox').value();
    loanGroup.loanGroupDayId = $('#groupDay').data('kendoComboBox').value();
    loanGroup.relationsOfficerStaffId = $('#relationsOfficer').data('kendoComboBox').value();
    loanGroup.leaderClientId = $('#groupLeaderClient').data('kendoComboBox').value();
	retrieveGridData();
}
function retrieveGridData() {
	loanGroup.loanGroupClients = $("#groupGrid").data().kendoGrid.dataSource.view();
}


//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: LoanGroupPostApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(loanGroup),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Loan Group Information Saved Successfully',// \n Group Number:' + data.loanGroupNumber,
            'SUCCESS', function () { window.location = '/LoanSetup/LoanGroups'; });
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

function clientEditor(container, options) {
    $('<input id="remainingClient" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("100%")
    .kendoComboBox({
        dataSource: remainingClients,
        dataValueField: "clientID",
        dataTextField: "clientNameWithAccountNO",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onClientChange,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		optionLabel: ''
    });
}

function AmountEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("100%")
    .kendoNumericTextBox({
        min: 0
     });
}



var onClientChange = function () {
    var id = $("#remainingClient").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < remainingClients.length; i++) {
        if (remainingClients[i].clientID == id) {
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Client', 'ERROR');
        $("#remainingClient").data("kendoComboBox").value("");
    }
}

function gridDeleteFunction(e){
	var clnId = e.model.clientId;
	addClientBack(clnId); 
}

function getClientName(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].loanId == id){
			return clients[i].clientNameWithAccountNO;
		}            
    }
}
/*
function getClientAccount(id) {
    for (var i = 0; i < loans.length; i++) {
        if (loans[i].loanID == id){
			return getClientAccountNo(loans[i].clientID);
		}			
    }
}
*/
function getLoan(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].loanId == id){
			return clients[i].loanNo;
		}			
    }
}
/*
function getloan(id) {
    for (var i = 0; i < loans.length; i++) {
		if (loans[i].loanID == id){
			return loans[i].loanNo;
		}			
    }
}
*/



function getClient(id) {
    for (var i = 0; i < allClients.length; i++) {
        if (allClients[i].clientID == id)
            return allClients[i].clientNameWithAccountNO;
    }
}
function removeClient(id) {
	var clientLeader = $("#groupLeaderClient").data("kendoComboBox");
    for (var i = 0; i < remainingClients.length; i++) {
        if (remainingClients[i].clientID == id) {
            allGroupClients.push(remainingClients[i]);
            clientLeader.setDataSource(allGroupClients);
			remainingClients.splice(i,1);
			break;
		}
    }
}
function addClientBack(id) {
	//var clientLeader = $("#groupLeaderClient").data("kendoComboBox");
    for (var i = 0; i < clientsWithoutGroup.length; i++) {
        if (clientsWithoutGroup[i].clientID == id) {
            remainingClients.push(clientsWithoutGroup[i]);
			break;
		}
    }
}
function populateRemainingClients(clients, clientsWithoutGrp, groupClients) {
    for (var i = 0; i < clientsWithoutGrp.length; i++) {
        remainingClients.push(clientsWithoutGrp[i]);
    }



    var groupClientsIds = [];
	for (var i = 0; i < groupClients.length; i++) {
		groupClientsIds.push(groupClients[i].clientId);
    }


    for (var i = 0; i < clients.length; i++) {
        var ind = groupClientsIds.indexOf(clients[i].clientID) > -1;
        if (ind) {
            allGroupClients.push(clients[i]);
        }
    }
}

function getBatchLoansAndClients(batchId){
	var loanAjax = $.ajax({
		url: LoanClientsApiUrl + '/GetProvisionLoans/'+batchId,
		type: 'Get',
		beforeSend: function (req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}
	});
	var clientAjax = $.ajax({
		url: LoanClientsApiUrl + '/GetProvisionClients/'+batchId,
		type: 'Get',
		beforeSend: function (req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}
	});
	
	$.when(loanAjax, clientAjax)
        .done(function (dataLoan, dataClient) {
			loans = dataLoan[2].responseJSON;                        
			clients = dataClient[2].responseJSON;                        
						
			renderGrid();
			dismissLoadingDialog();
        });
}
