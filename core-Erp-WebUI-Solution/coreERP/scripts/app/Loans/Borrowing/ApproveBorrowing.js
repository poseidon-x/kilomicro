//***********************************************************//
//  	       BORROWING - JAVASCRIPT                
// 			 CREATOR: EMMANUEL OWUSU(MAN)    	   
//		      WEEK: AUG(10TH - 14TH), 2015  		  
//*********************************************************//


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var borrowingApiUrl = coreERPAPI_URL_Root + "/crud/Borrowing";
var borrowingClientApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingClients";
var borrowingTypeApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingType";
var tenureTypeApiUrl = coreERPAPI_URL_Root + "/crud/TenureType";
var interestTypeApiUrl = coreERPAPI_URL_Root + "/crud/interestType";
var repaymentModeApiUrl = coreERPAPI_URL_Root + "/crud/repaymentMode";
var repaymentScheduleApiUrl = coreERPAPI_URL_Root + "/crud/Schedule";



//Declaration of variables to store records retrieved from the database
var borrowings = {};
var clients = {};
var borrowingTypes = {};
var tenureTypes = {};
var interestTypes = {};
var repaymentModes = {};
var selectedBorrowing = {};


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

var clientAjax = $.ajax({
    url: borrowingClientApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var borrowingTypeAjax = $.ajax({
    url: borrowingTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var tenureTypeAjax = $.ajax({
    url: tenureTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var interestTypeAjax = $.ajax({
    url: interestTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var repaymentModeAjax = $.ajax({
    url: repaymentModeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(clientAjax, borrowingTypeAjax, tenureTypeAjax, interestTypeAjax, repaymentModeAjax)
        .done(function (dataClient, dataBorrowingType, dataTenureType, dataInterestType, dataRepaymentMode) {
			clients = dataClient[2].responseJSON;
            borrowingTypes = dataBorrowingType[2].responseJSON;
			tenureTypes = dataTenureType[2].responseJSON;
            interestTypes = dataInterestType[2].responseJSON;
            repaymentModes = dataRepaymentMode[2].responseJSON;
			
            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() 
{
    renderControls();
    dismissLoadingDialog();
	
	
	//Validate to Check Empty/Null input Fields
    $('#viewSchedule').click(function (event) {
        var validator = $("#myform").kendoValidator().data("kendoValidator");
			
			
        if (!validator.validate()) {
            smallerWarningDialog('One or More Fields are Empty', 'ERROR');
        } else {
			displayLoadingDialog();
			//Retrieve & save Grid data
			viewSampleSchedule();
		}
	});
	
	//Validate to Check Empty/Null input Fields
    $('#approve').click(function (event) {
	
        var validator = $("#myform").kendoValidator().data("kendoValidator");
		
        if (!validator.validate()) {
            smallerWarningDialog('One or More Fields are Empty', 'ERROR');
        } else {
			displayLoadingDialog();
			//Retrieve & save Grid data
			saveBorrowing();
		}
	});
}

//Apply kendo Style to the input fields
function renderControls() {
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
		
    $("#borrowingNo").width("100%")
		.kendoComboBox({
		    dataSource: borrowings,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "borrowingId",
		    dataTextField: "borrowingNo",
		    change: onBorrowingChange,
		    optionLabel: ""
	});
	
    $("#borrowingTenure").width("100%")
		.kendoNumericTextBox({
			min: 1,
			format: "0 ''"
    });
	
    $("#tenureType").width("100%")
		.kendoComboBox({
			dataSource: tenureTypes,			
		    filter: "contains",
		    suggest: true,
			dataValueField: "tenureTypeID",
			dataTextField: "tenureTypeName",
			change: onTenureTypeChange,		
			optionLabel: ""
	});
	
    $("#borrowingType").width("100%")
		.kendoComboBox({
			dataSource: borrowingTypes,			
		    filter: "contains",
		    suggest: true,
			dataValueField: "borrowingTypeId",
			dataTextField: "borrowingTypeName",
			change: onBorrowingTypeChange,		
			optionLabel: ""
	});
	
    $("#interestType").width("100%")
		.kendoComboBox({
			dataSource: interestTypes,			
		    filter: "contains",
		    suggest: true,
			dataValueField: "interestTypeID",
			dataTextField: "interestTypeName",
			//change: onBorrowingTypeChange,		
			optionLabel: ""
	});
	
    $("#amountRequested").width('100%')
		.kendoNumericTextBox({
			min: 1
    });
	
    $("#interestRate").width('100%')
		.kendoNumericTextBox({
			min: 1,
			format: "0 \\%"
    });
	
    $("#repaymentMode").width('100%')
		.kendoComboBox({
			dataSource: repaymentModes,			
		    filter: "contains",
		    suggest: true,
			dataValueField: "repaymentModeID",
			dataTextField: "repaymentModeName",
			//change: onTenureTypeChange,		
			optionLabel: ""
	});
	
    $("#applicationFee").width('100%')
		.kendoNumericTextBox({
			min: 0
    });
	
    $("#processingFee").width('100%')
		.kendoNumericTextBox({
			min: 0
    });
	
    $("#commission").width('100%')
		.kendoNumericTextBox({
			min: 0
    });
	
    $("#amountApproved").width('100%')
		.kendoNumericTextBox({
			min: 0
    });
}

function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(selectedBorrowing.borrowingRepaymentSchedules);
                },
                create: function (entries) {
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
                    id: 'borrowingRepaymentScheduleId',
                    fields: {
                        borrowingRepaymentScheduleId: { type: 'number' },
                        borrowingId: { type: 'number', editable: false },
                        repaymentDate: { type: 'date' },
                        interestPayment: { type: 'number' },
                        principalPayment: { type: 'number' },
                        interestBalance: { type: 'number' },
                        principalBalance: { type: 'number' },
                        totalPayment: { type: 'number' },
                        totalBalance: { type: 'number' }
					} //fields
                } //model
            } //schema
        }, //datasource
        scrollable: false,		
        columns: [
			{ field: 'repaymentDate', title: 'Repayment Date', format: "{0:dd-MMM-yyyy}" },
            { field: 'interestPayment', title: 'Interest Payment', format: "{0: #,###.#0}" },
            { field: 'principalPayment', title: 'Principal Payment', format: "{0: #,###.#0}"  },
            { field: 'interestBalance', title: 'Interest Balance', format: "{0: #,###.#0}"  },
            { field: 'principalBalance', title: 'Principal Balance', format: "{0: #,###.#0}"  },
            { field: 'totalPayment', title: 'Total Payment', format: "{0: #,###.#0}"  },
            { field: 'totalBalance', title: 'Total Balance', format: "{0: #,###.#0}"  },
		]
});
    

}

function populateUi(){
	$("#borrowingTenure").data("kendoNumericTextBox").value(selectedBorrowing.borrowingTenure);
	$("#tenureType").data("kendoComboBox").value(selectedBorrowing.tenureTypeId);
	$("#borrowingType").data("kendoComboBox").value(selectedBorrowing.borrowingTypeId);
	$("#interestType").data("kendoComboBox").value(selectedBorrowing.interestTypeId);
	$("#amountRequested").data("kendoNumericTextBox").value(selectedBorrowing.amountRequested);
	$("#interestRate").data("kendoNumericTextBox").value(selectedBorrowing.interestRate);
	$("#repaymentMode").data("kendoComboBox").value(selectedBorrowing.repaymentModeId);
	$("#applicationFee").data("kendoNumericTextBox").value(selectedBorrowing.applicationFee);
	$("#processingFee").data("kendoNumericTextBox").value(selectedBorrowing.processingFee);
	$("#commission").data("kendoNumericTextBox").value(selectedBorrowing.commission);
}

var onClientChange = function() {
	var clientId = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == clientId) {
            exist = true;
			
			displayLoadingDialog();
			$.ajax({
				url: borrowingApiUrl + '/GetClientUnapprovedBrws?clientId=' + clientId,
				type: 'Get',
				beforeSend: function(req) {
					req.setRequestHeader('Authorization', "coreBearer " + authToken);
				}
			}).done(function (data) {
				borrowings = data;
				dismissLoadingDialog();
				
				//Set the returned data to loan datasource
				$("#borrowingNo").data("kendoComboBox").value("");
				$("#borrowingNo").data("kendoComboBox").setDataSource(borrowings);
				
			}).error(function (xhr, data, error) {
				dismissLoadingDialog();
				console.log("error retrieving data");
				warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
			});
			
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}


var onBorrowingChange = function() {
	var id = $("#borrowingNo").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < borrowings.length; i++) {
        if (borrowings[i].borrowingId == id) {
			selectedBorrowing = borrowings[i];
			populateUi();
            exist = true;
            break;
        }
    }	
	
    if (!exist) {
        warningDialog('Invalid Borrowing', 'ERROR');
        $("#borrowingNo").data("kendoComboBox").value("");
    }
}


var onTenureTypeChange = function() {
	var id = $("#tenureType").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < tenureTypes.length; i++) {
        if (tenureTypes[i].tenureTypeID == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Tenure Type', 'ERROR');
        $("#tenureType").data("kendoComboBox").value("");
    }
}

var onBorrowingTypeChange = function() {
	var id = $("#borrowingType").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < borrowingTypes.length; i++) {
        if (borrowingTypes[i].borrowingTypeId == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Borrowing Type', 'ERROR');
        $("#borrowingType").data("kendoComboBox").value("");
    }
}


//retrieve values from from Input Fields and save 
function saveBorrowing() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {	
	selectedBorrowing.amountApproved = $('#amountApproved').data('kendoNumericTextBox').value();
	selectedBorrowing.approvalComments = $('#approvalComment').val();
}

//view to schedule function
function viewSampleSchedule() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: repaymentScheduleApiUrl + '/GetSampleRepaySchedule',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(selectedBorrowing),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and render Grid
		
		for(var i=0;i<data.length;i++){
			data[i].totalPayment = data[i].principalPayment + data[i].interestPayment;
			data[i].totalBalance = data[i].principalBalance + data[i].interestBalance;
		}
		
		selectedBorrowing.borrowingRepaymentSchedules = data;
		
		renderGrid();
        dismissLoadingDialog();        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer   
   
//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: borrowingApiUrl + '/ApproveBorrowing',
        type: 'Put',
        contentType: 'application/json',
        data: JSON.stringify(selectedBorrowing),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        //successDialog('Borrowing Successfully Saved \n Borrowing Number:'+data.borrowingNo, 
		//'SUCCESS', function () { window.location = "/Borrowing/CreateBorrowing"; });
		
		successDialog('Borrowing Successfully Approved \n Borrowing Number:'+data.borrowingNo,
		'SUCCESS', function () { window.location = "/Borrowing/CreateBorrowing/"; });

        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

function specialityEditor(container, options) {
    $('<input type="text" id="speciality" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("100%")
    .kendoComboBox({
        dataSource: specialities,
        dataValueField: "specialityId",
        dataTextField: "specialityName",
        change: onSpecialityChange,
        optionLabel: ""
    });
}

function hourlyBillingRateEditor(container, options) {
    $('<input type="text" id="hourlyBillingRate" data-bind="value:' + options.field + '"/> ')
    .appendTo(container)
	.width("100%")
    .kendoNumericTextBox({
		format: "0:#.00 \\%",
		decimals: "2",
		min: 0,
        max: 100,
        step: 0.01
    });
}



function billableEditor(container, options) {
    $('<input type="checkbox" id="billable" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("100%");
}
                        
var onSpecialityChange = function() {
	//Retrieve value enter validate
    var id = $("#speciality").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < specialities.length; i++) {
        if ( specialities[i].specialityId == id) {
            exist = true;
            break;
        }
    }	
	
    if (!exist) {
        warningDialog('Invalid Speciality', 'ERROR');
        $("#speciality").data("kendoComboBox").value("");
    }
}

function getSpecialityIdEditor(id) {
    for (var i = 0; i < specialities.length; i++) {
        if (specialities[i].specialityId == id) 
        return specialities[i].specialityName;
    }
}

function getBillingRateEditor(rate) {
    return rate + "%";
}

