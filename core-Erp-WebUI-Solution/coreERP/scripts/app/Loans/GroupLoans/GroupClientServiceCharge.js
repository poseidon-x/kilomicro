//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: SAMUEL WENDOLIN KETECHIE    	   
//***   DATE: SEPT 18TH, 2018 	
//*******************************************


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var clientServiceChargeApiUrl = coreERPAPI_URL_Root + "/crud/ClientServiceCharge";
var chargeTypeApiUrl = coreERPAPI_URL_Root + "/crud/ClientChargeType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";


//Declaration of variables to store records retrieved from the database
var clientServiceCharge = {};
var chargeTypes = {};
var clients = {};
var paymentId = -1;
var loanGroups = {};
var _GroupId = {};


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//get loan groups
var groupLoanAjax = $.ajax({
    url: LoanGroupApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer" + authToken);
    }
});

//get charges
var clientServiceChargeAjax = $.ajax({
    url: clientServiceChargeApiUrl + '/GetGroupService',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//get all charge types
var chargeTypeAjax = $.ajax({
    url: chargeTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//get all clients
var clientAjax = $.ajax({
    url: clientApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Get All Deposit Clients
function loadForm() {
    $.when(groupLoanAjax, clientServiceChargeAjax, chargeTypeAjax, clientAjax)
        .done(function (dataGroupLoan, dataClientServiceCharge, dataChargeType, dataClient) {
            clientServiceCharge = dataClientServiceCharge[2].responseJSON;
            chargeTypes = dataChargeType[2].responseJSON;
            loanGroups = dataGroupLoan[2].responseJSON;
            clients = dataClient[2].responseJSON;

            prepareUi();
            dismissLoadingDialog();
        });
}

//Function to prepare user interface
function prepareUi() {
    $('#paymentDate').width('80%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        value: new Date()
    });
    $('#group').width('80%').kendoComboBox({
        dataSource: loanGroups,
        dataValueField: 'loanGroupId',
        dataTextField: 'loanGroupName',
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        ignoreCase: true,
        change: onGroupChange,
        optionLabel: '',
    });

    $('#save').click(function (event) {
        if (confirm("Are you sure you want to apply charges" )) {
            saveToServer();
        }
        else {
            smallerWarningDialog('Please review and apply later', 'NOTE');
        }
    });
}

var onGroupChange = function () {
    var id = $("#group").data("kendoComboBox").value();
    var exist = false;
    //Reset Grid to empty
    $('#paymentGrid').html('');

    //Retrieve value enter validate
    for (var i = 0; i < loanGroups.length; i++) {
        if (loanGroups[i].loanGroupId == id) {
            exist = true;
            getClientsForGroup(id);
            _GroupId = id;
            renderGrid();

            break;
        }

    }

    if (!exist) {
        warningDialog('Invalid Group', 'ERROR');
        $("#group").data("kendoComboBox").value("");
        $('#paymentGrid').html('');
    }
}

function getClientsForGroup(groupId) {
    $.ajax({
        url: clientApiUrl + "/GetClientForGroup?groupId=" + groupId,
        type: 'Get',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        clients = data;
    });
}

function renderGrid() {
    $('#tabs').kendoTabStrip({});
    $('#paymentGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(clientServiceCharge.payments);
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
                        chargeTypeId: { editable: true },
                        amount: { editable: true, type: "number" },
                        clientId: { editable: true }
                    }
                }
            },
            aggregate: [
                    { field: "amount", aggregate: "sum" }
            ]
        },
        columns: [
           { field: 'clientId', title: 'Client Name', editor: clientEditor, template: '#= getClient(clientId) #' },
			{ field: 'chargeTypeId', title: 'Charge Type', editor: chargeTypeEditor, template: '#= getChargeType(chargeTypeId) #' },
			{
			    field: 'amount',
			    title: 'Amount',
			    format: '{0: #,###.#0}',
			    footerTemplate: "Total Charges: #: sum #",
			    groupFooterTemplate: "Total Amount :  #= kendo.toString(sum, '0,000.00') #"
			},
			{ command: [{ name: "edit" }, { name: "destroy" }], width: 180 }
        ],

        toolbar: [
            { name: "create", text: "Add Payment" },
        ],
        editable: "popup"
    });
}

//get client name pulated in cell
function chargeTypeEditor(container, options) {//editor
    $('<input required data-text-field="chargeTypeName" data-value-field="chargeTypeID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            optionLabel: "",
            highlightFirst: true,
            suggest: true,
            dataSource: {
                data: chargeTypes,
            },
            dataValueField: "chargeTypeID",
            dataTextField: "chargeTypeName",
            template: '<span>#: chargeTypeName #</span>',
            filter: "contains",
        });
    var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}

function getChargeType(id) {
    for (var i = 0; i < chargeTypes.length; i++) {
        if (chargeTypes[i].chargeTypeID == id) {
            return chargeTypes[i].chargeTypeName;
        }
    }
}

//get client name pulated in cell
function getClient(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            return clients[i].clientName;
        }
    }
}

function clientEditor(container, options) {//editor
    $('<input required data-text-field="clientName" data-value-field="clientID" data-bind="value:' + options.field + '" id="client"/>')
        .appendTo(container)
        .kendoDropDownList({
            optionLabel: "--Select Client--",
            highlightFirst: true,
            suggest: true,
            dataSource: {
                data: clients,
            },
            dataValueField: "clientID",
            dataTextField: "clientName",
            template: '<span>#: clientName #</span>',
            filter: "contains",
        });
}

//Save to server function
function saveToServer() {
    clientServiceCharge.groupId = _GroupId;
    clientServiceCharge.chargeDate = new Date().toLocaleDateString();
    clientServiceCharge.payments = $("#paymentGrid").data().kendoGrid.dataSource.view();
    displayLoadingDialog();
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientServiceChargeApiUrl + '/PostGroupMulti',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(clientServiceCharge),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Charge successfully Applied:', 'SUCCESS',
		function () { window.location = '/ln/setup/postTill.aspx'; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });

}//func saveToServer

function getClients(input) {
    displayLoadingDialog();
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetClientForGroup?groupId=' + input,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        if (data.length > 0) {
            clients = data;
            //successDialog('Enter the values', 'SUCCESS');
        }
        else {
            //clients = {};
            warningDialog('No client found for the criteria', 'FAIL');
        }
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

