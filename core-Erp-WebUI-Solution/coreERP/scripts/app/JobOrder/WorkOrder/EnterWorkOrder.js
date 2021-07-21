//***********************************************************//
//  	     CREDIT MEMO - JAVASCRIPT                //
// 		CREATOR: EMMANUEL OWUSU(MAN)    	   //
//		      WEEK: JUNE(8TH - 12TH), 2015  		  //
//*********************************************************//


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var workOrderApiUrl = coreERPAPI_URL_Root + "/crud/WorkOrder";
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var specialityApiUrl = coreERPAPI_URL_Root + "/crud/Speciality";


//Declaration of variables to store records retrieved from the database
var workOrder = {};
var customers = {};
var specialities = {};

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


var workOrderAjax = $.ajax({
    url: workOrderApiUrl + '/Get/' + workOrderId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var customerAjax = $.ajax({
    url: customerApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var specialityAjax = $.ajax({
    url: specialityApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(workOrderAjax, customerAjax, specialityAjax)
        .done(function (dataWorkOrder, dataCustomer, dataSpeciality) {
            workOrder = dataWorkOrder[2].responseJSON;
			customers = dataCustomer[2].responseJSON;
            specialities = dataSpeciality[2].responseJSON;
			
            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() 
{		
    //If arInvoiceId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (workOrder.workOrderId > 0) {
        renderControls();
        //populateUi();
        dismissLoadingDialog();
    } else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        renderControls();
        dismissLoadingDialog();
    }

	//Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {
	
        var validator = $("#myform").kendoValidator().data("kendoValidator");
		
        if (!validator.validate()) {
            smallerWarningDialog('One or More Fields are Empty', 'ERROR');
        } else {
				var activitiesGridData = $("#grid").data().kendoGrid.dataSource.view();					
				
				if (activitiesGridData.length > 0) {
					displayLoadingDialog();
					saveAcitivityGridData(activitiesGridData);
					//Retrieve & save Grid data
					saveWorkOrder();
                }else {
                    smallerWarningDialog('Please Add Order Activities Details', 'NOTE');
				}
		}
	});
}

//Apply kendo Style to the input fields
function renderControls() {
	$("#customer").width("75%")
		.kendoComboBox({
			dataSource: customers,
			dataValueField: "customerId",
			dataTextField: "customerName",
			change: onCustomerChange,		
			optionLabel: ""
	});
		
	$("#workOrderDate").width('75%')
		.kendoDatePicker({
			format: 'dd-MMM-yyyy',
			parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
			value: 	new Date()
    });
		
}

function saveAcitivityGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            workOrder.workOrderActivities.push(data[i]);
        }
    }
    else {
	workOrder.workOrderActivities.push(data[0]);
	}
}

var onCustomerChange = function() {
	var id = $("#customer").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < customers.length; i++) {
        if (customers[i].customerId == id) {
			renderGrid();
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Customer', 'ERROR');
        $("#customer").data("kendoComboBox").value("");
    }
}

//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(workOrder.workOrderActivities);
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
                    id: 'workOrderActivityId',
                    fields: {
                        workOrderId: { type: 'number', defaultValue: workOrder.workOrderId },
                        workOrderActivityId: { type: 'number', editable: false },
                        specialityId: {},						
                        hourlyBillingRate: { validation: { min: 0, max: 100} },
                        hourlyCost: { type: 'number',  format: "{0:c}",  validation: { min: 0}  },
                        markup: { validation: { min: 0, max: 100} },
                        billable: { type: 'boolean'  }

						}, //fields
                }, //model
            }, //schema
        }, //datasource
		editable: 'popup',
        columns: [
			{ field: 'specialityId', title: 'Description', editor: specialityEditor, template: '#= getSpecialityIdEditor(specialityId) #'},
            { field: 'hourlyBillingRate', title: 'Hourly Rate', editor: hourlyBillingRateEditor, template: '#= getBillingRateEditor(hourlyBillingRate) #'},
            { field: 'hourlyCost', title: 'Hourly Cost',  format: "{0:#,##0.00}" },
            { field: 'markup', title: 'Markup',  editor: hourlyBillingRateEditor, template: '#= getBillingRateEditor(markup) #' },
            { field: 'billable', title: 'billable', editor: billableEditor, 
				template: '<input type="checkbox" disabled="disabled" id="billable" data-bind="checked: billable" #= billable? checked="checked":"" #/>'},
			{ command: ['destroy'] , width: 110}			
       ],
		toolbar: [{ name: 'create', text: 'Add Activities' }]	   
    });
}


//retrieve values from from Input Fields and save 
function saveWorkOrder() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    workOrder.customerId = $('#customer').data('kendoComboBox').value();
	workOrder.workOrderDate =  $('#workOrderDate').data('kendoDatePicker').value();
   }

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: workOrderApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(workOrder),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Work Order Successfully Saved \n Work Order Number:'+data.workOrderNumber, 'SUCCESS'); //, function () { window.location = "/ArInvoice/Invoices/"; }
        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

function specialityEditor(container, options) {
    $('<input type="text" id="speciality" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
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
	.width("75%")
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
	.width("75%");
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

