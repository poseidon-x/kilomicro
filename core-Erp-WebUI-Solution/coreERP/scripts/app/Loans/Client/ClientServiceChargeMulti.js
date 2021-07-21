//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: EMMANUEL OWUSU(MAN)  AND MODIFIED BY SAMUEL WENDOLIN KETECHIE   
//***   WEEK: OCT 20TH, 2015  	
//***   MODIFICATION DATE: JAN 13, 2020
//*******************************************


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var clientServiceChargeApiUrl = coreERPAPI_URL_Root + "/crud/ClientServiceCharge";
var chargeTypeApiUrl = coreERPAPI_URL_Root + "/crud/ClientChargeType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";


//Declaration of variables to store records retrieved from the database
var clientServiceCharge = {};
var chargeTypes = {};
var clients = {};
var paymentId = -1;


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

var clientServiceChargeAjax = $.ajax({
    url: clientServiceChargeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var chargeTypeAjax = $.ajax({
    url: chargeTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Get All Deposit Clients
function loadForm() {
	
    $.when(clientServiceChargeAjax,chargeTypeAjax)
        .done(function (dataClientServiceCharge, dataChargeType) {
            clientServiceCharge = dataClientServiceCharge[2].responseJSON;
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
		var cln = $('#client').data('kendoComboBox').text();
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
	$("#chargeDate").width('90%').kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        value: new Date()
	});
	renderGrid()
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
                        amount: { editable: true, type: "number" }
                    }
                }
            },
        },		
        columns: [
			{ field: 'chargeTypeId', title: 'Charge Type', editor: chargeTypeEditor, template: '#= getChargeType(chargeTypeId) #' },
			{ field: 'amount', title: 'Amount', format: '{0: #,###.#0}' },
			{ command: [{ name: "edit" },{ name: "destroy" }],width:180}
        ],

        toolbar: [
            { name: "create", text: "Add Payment" },
        ],
        editable: "popup"
    });
}

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

function getChargeType(id){
    for (var i = 0; i < chargeTypes.length; i++) {
        if (chargeTypes[i].chargeTypeID == id) {
            return chargeTypes[i].chargeTypeName;
        }
    } 
}
/*
var onChargeTypeChange = function () {
    var id = $("#chargeType").data("kendoComboBox").value();
	var amt = $("#amount").data("kendoNumericTextBox");
	amt.value("");
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < chargeTypes.length; i++) {
        if (chargeTypes[i].chargeTypeID == id) {
			if(chargeTypes[i].fixed){
				amt.value(chargeTypes[i].chargeDefaultAmount);
			}
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Charge Type', 'ERROR');
        $("#chargeType").data("kendoComboBox").value("");
    }
}*/


//retrieve values from from Input Fields and save 
function saveCharge() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    clientServiceCharge.clientId = $('#client').data('kendoComboBox').value();
    clientServiceCharge.chargeDate = $("#chargeDate").data('kendoDatePicker').value();
    clientServiceCharge.payments = $("#paymentGrid").data().kendoGrid.dataSource.view();
}

//Save to server function
function saveToServer() {
	displayLoadingDialog();	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientServiceChargeApiUrl + '/PostMulti',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(clientServiceCharge),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Charge successfully Applied:', 'SUCCESS', 
		function () { window.location = '/dash/home.aspx'; });        
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
			successDialog('No client found that match the searched criteria', 'SUCCESS');
		}        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}

function setUpCombox(){
	$("#client")
	.kendoComboBox({
		dataSource: clients,
		filter: "contains",
		suggest: true,
		dataValueField: "clientID",
		dataTextField: "clientNameWithAccountNO",
		highlightFirst: true,
		optionLabel: ""
	});
}
