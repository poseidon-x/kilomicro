//"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var LoanClientsApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var LoanGroupDayApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupDay";
var staffApiUrl = coreERPAPI_URL_Root + "/crud/Staff";

//Declaration of variables to store records retrieved from the database
var loanGroup = {};
var clientsWithoutGroup = {};
var allClients = {};
var remainingClients = [];
var days = {};
var staff = {};

//serve as  datasource for client Leader combobox
var allGroupClients = [];

var loanGroupAjax = $.ajax({
    url: LoanGroupApiUrl + '/Get/' + loanGroupId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var clientAjax = $.ajax({
    url: LoanClientsApiUrl + '/GetClientWithoutGroup',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var clientAllAjax = $.ajax({
    url: LoanClientsApiUrl + '/GetAllGroupClients', 
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var daysAjax = $.ajax({
    url: LoanGroupDayApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var staffAjax = $.ajax({
    url: staffApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {
	$.when(loanGroupAjax, clientAjax, clientAllAjax, daysAjax,staffAjax)
        .done(function (dataLoanGroup, dataClient, dataAllClients, dataDay,dataStaff) {
			loanGroup = dataLoanGroup[2].responseJSON;                        
			allClients = dataAllClients[2].responseJSON;                        
			clientsWithoutGroup = dataClient[2].responseJSON;
			days = dataDay[2].responseJSON;
			staff = dataStaff[2].responseJSON;
			populateRemainingClients(allClients,clientsWithoutGroup, loanGroup.loanGroupClients);
			
			prepareUi();
			dismissLoadingDialog();
        });
}

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//Function to prepare user interface
function prepareUi() 
	{		
	$("#groupName").width("75%") .kendoMaskedTextBox();
	$("#groupDay").width("75%").kendoComboBox({
		dataSource: days,
		dataValueField: 'loanGroupDayId',
		dataTextField: 'dayName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onDayChange,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		optionLabel: ''
	});
	$("#relationsOfficer").width("75%").kendoComboBox({
		dataSource: staff,
		dataValueField: 'staffId',
		dataTextField: 'staffName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		//change: onDayChange,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		optionLabel: ''
	});
	$("#groupLeaderClient").width("75%").kendoComboBox({
	    dataSource: allGroupClients,
		dataValueField: 'clientID',
		dataTextField: 'clientNameWithAccountNO',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onDayChange,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		optionLabel: ''
	});
	$('#tabs').kendoTabStrip();

	if(loanGroup.loanGroupId > 0)
	{
		$('#groupName').data('kendoMaskedTextBox').value(loanGroup.loanGroupName);
		$('#groupDay').data('kendoComboBox').value(loanGroup.loanGroupDayId);
		$('#relationsOfficer').data('kendoComboBox').value(loanGroup.relationsOfficerStaffId);
		$('#groupLeaderClient').data('kendoComboBox').value(loanGroup.leaderClientId);
	}
		
	renderGrid();
	
	$('#save').click(function (event) {
	var validator = $("#myform").kendoValidator().data("kendoValidator");
		
    if (validator.validate()) {
		var gridData = $("#groupGrid").data().kendoGrid.dataSource.view();
		if(gridData.length > 4 && gridData.length < 31){
			if (confirm('Are you sure you want save this group?')) {
				displayLoadingDialog();
				SaveLoanGroup();				
			} else {
			   smallerWarningDialog('Please review and save later', 'NOTE');
			}
		}else{
			successDialog('Group clients cannot be less than five(5) or more than 30 ','Warning')
		}
	}else{smallerWarningDialog('One or more input is empty or invalid', 'WARNING');}		
	});
}

var onDayChange = function () {
    var id = $("#groupDay").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < days.length; i++) {
        if (days[i].loanGroupDayId == id) {
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Day', 'ERROR');
        $("#groupDay").data("kendoComboBox").value("");
    }
}

//render Grid
function renderGrid() {
    $('#groupGrid').kendoGrid({
            dataSource: {
                transport: {
					read: function (entries) {
						entries.success(loanGroup.loanGroupClients);					
					},
					create: function (entries) {
						var data = entries.data;
						removeClient(data.clientId);
						//addToGroupClient(data.clientId);
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
                        id: "loanGroupClientId",
                        fields: {
                            loanGroupClientId: { editable: false, type: "number" },
                            loanGroupId: { editable: false, type: "number"  },
							clientId: {editable: true, validation: {  required: true} }
                        }
                    }
                },
            },
			columns: [
				{ field: 'clientId', title: 'Client', editor: clientEditor, template: '#= getClient(clientId) #' },
				{ command: [ "edit", "destroy"], width: "180px"   }
			],
            toolbar: [
                { name: "create", text: "Add Client" },
            ],
            editable: "inline"
        });
}


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
        url: LoanGroupApiUrl + '/Post',
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
