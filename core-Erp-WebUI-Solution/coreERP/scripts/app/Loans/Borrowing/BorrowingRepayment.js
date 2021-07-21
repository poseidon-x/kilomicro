//***********************************************************//
//  	     Borrowing Repayment - JAVASCRIPT                
// 		      CREATOR: EMMANUEL OWUSU(MAN)    	   
//		      DATE: AUG(10TH - 14th), 2015  		  
//*********************************************************//


"use strict";

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var borrowingApiUrl = coreERPAPI_URL_Root + "/crud/Borrowing";
var borrowingClientApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingClients";
var borrowingTypeApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingType";


//Declaration of variables to store records retrieved from the database
var borrowings = {};
var clients = {};
var selectedBorrowing = {};
var borrowingTypes = {};

var clientAjax = $.ajax({
	url: borrowingClientApiUrl + '/GetDisbursedBorrowingClient',
	type: 'Get',
	beforeSend: function(req){
		req.setRequestHeader('Authorization', 'coreBearer' + authToken);
	}
});

var borrowingTypeAjax = $.ajax({
	url: borrowingTypeApiUrl + '/Get',
	type: 'Get',
	beforeSend: function(req){
		req.setRequestHeader('Authorization', 'coreBearer' + authToken);
	}
});

function loadForm() {
	$.when(clientAjax, borrowingTypeAjax)
     .done(function (dataClient, dataBorrowingType) {
		clients = dataClient[2].responseJSON;
        borrowingTypes = dataBorrowingType[2].responseJSON;
					
        //Prepares UI
        prepareUi();
    });
}

//Function to call load form function
$(function () {
    //displayLoadingDialog();
    loadForm();
});

//Function to prepare user interface
function prepareUi() 
{		
    renderControls();
	dismissLoadingDialog

    $('#save').click(function (event) {

        var validator = $("#myform").kendoValidator().data("kendoValidator");
        var gridData = $("#grid").data().kendoGrid.dataSource.view();


		//if (!validator.validate()) {
        //    smallerWarningDialog('A form input is empty or has invalid value', 'ERROR');
    //}
    //else {
			if (confirm('Are you sure you want Save Repayment Schedule?')) {
                displayLoadingDialog();
				selectedBorrowing.borrowingRepaymentSchedules = [];
                saveGridData(gridData);

				saveSchedule();				
            } else {
                smallerWarningDialog('Please review and save later', 'NOTE');
            }
		//}
	});
}

//Apply kendo Style to the input fields
function renderControls() {
	$("#tabs").kendoTabStrip();
	
    $("#client").width("100%")
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
            getClientBorrowings(id);
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
                    entries.success(borrowings);
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
                    id: 'borrowingId',
                    fields: {
                        borrowingId: { type: 'number', editable: false },
                        borrowingNo: { type: 'string' },
                        borrowingTypeId: { type: 'number' },
                        clientId: { type: 'number' },
                        applicationDate: { type: 'date' },
                        aprovalDate: { type: 'date' },
                        disbursedDate: { type: 'date' },
                        amountRequested: { type: 'number' },
                        amountApproved: { type: 'number' },
                        amountDisbursed: { type: 'number' }
					} //fields
                } //model
            } //schema
        }, //datasource
        scrollable: false,
        columns: [
			{ field: 'borrowingNo', title: 'Borrowing Number' },
            { field: 'borrowingTypeId', title: 'Product', template: '#= getBorrowingType(borrowingTypeId) #' },
            { field: 'clientId', title: 'Borrowing Client', template: '#= getClient(clientId) #' },
            { field: 'applicationDate', title: 'Application Date', format: "{0: dd-MMM-yyy}" },
            { field: 'aprovalDate', title: 'Approval Date', format: "{0: dd-MMM-yyyy}" },
            { field: 'disbursedDate', title: 'Disbursed Date', format: "{0: dd-MMM-yyyy}" },
            { field: 'amountRequested', title: 'Amount Requested', format: "{0: #,###.#0}" },
            { field: 'amountApproved', title: 'Amount Approved', format: "{0: #,###.#0}" },
            { field: 'amountDisbursed', title: 'Amount Disbursed', format: "{0: #,###.#0}" },
            { command: [ makePaymentButton]}
		]		
        //toolbar: [{ name: 'create', text: 'Add New Schedule' }]
    });
}

var makePaymentButton = {
    name: "payment",
    text: "Payment",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        
        window.location = "/Borrowing/EnterBorrowingPayment/" + data.borrowingId.toString();
    },
};


function getClientBorrowings(id) {
    displayLoadingDialog();
    $.ajax(
        {
            url: borrowingApiUrl + '/GetClientApprovedBrws/' + id,
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            borrowings = data;
            document.getElementById('grid').innerHTML = '';

            //Set the returned data to loan datasource
            renderGrid();

            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
}

function getClient(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id)
            return clients[i].clientName;
    }
}



function getBorrowingType(id){
	for(var i = 0; i < borrowingTypes.length;i++){
		if(borrowingTypes[i].borrowingTypeId = id){
			return borrowingTypes[i].borrowingTypeName;
		}
	}	
};

