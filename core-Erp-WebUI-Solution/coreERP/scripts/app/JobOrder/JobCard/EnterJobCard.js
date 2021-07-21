//**********************************************//
//  			CREDIT MEMO - JAVASCRIPT        //
// 		CREATOR: EMMANUEL OWUSU(MAN)			//
//		WEEK: JUNE(8TH - 12TH), 2015 			//
//**********************************************//


"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var jobCardApiUrl = coreERPAPI_URL_Root + "/crud/jobCard";
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var workOrderApiUrl = coreERPAPI_URL_Root + "/crud/workOrder";
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/inventoryItem";
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/unitOfMeasurement";
var companyProfileApiUrl = coreERPAPI_URL_Root + "/crud/companyProfile";

//Declaration of variables to store records retrieved from the database
var jobCard = {};
var customers = {};
var workOrders = {};
var inventoryItems = {};
var unitOfMeasurements = {};
var selectedWorkOrder = {};
var selWorkOrdActivity = {};
var companyProfile = {};

var totalLabour = 0;
var totalMaterial = 0;
var standardHourlyBillingRate = 0;
var standardMarkUpRate = 0;
var vatRate;
var nhilRate;
var vatAmount = 0;
var nhilAmount = 0;


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


var jobCardAjax = $.ajax({
    url: jobCardApiUrl + '/Get/' + jobCardId,
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

var workOrderAjax = $.ajax({
    url: workOrderApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var inventoryItemAjax = $.ajax({
    url: inventoryItemApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var unitOfMeasurementAjax = $.ajax({
    url: unitOfMeasurementApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var companyProfileAjax = $.ajax({
    url: companyProfileApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {

    $.when(jobCardAjax, customerAjax, workOrderAjax, inventoryItemAjax, unitOfMeasurementAjax, companyProfileAjax)
        .done(function (dataJobCard, dataCustomer, dataWorkOrder, dataInventoryItem, dataUnitOfMeasurement, dataCompanyProfile) {
            jobCard = dataJobCard[2].responseJSON;
			customers = dataCustomer[2].responseJSON;
            workOrders = dataWorkOrder[2].responseJSON;
            inventoryItems = dataInventoryItem[2].responseJSON;
            unitOfMeasurements = dataUnitOfMeasurement[2].responseJSON;
            companyProfile = dataCompanyProfile[2].responseJSON;	
			
			vatRate = companyProfile[0].vat_rate;
			nhilRate = companyProfile[0].nhil_rate;
			
            //Prepares UI
            prepareUi();
        });
}

//Function to prepare user interface
function prepareUi() 
{
		
    //If arInvoiceId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (jobCard.jobCardId > 0) {
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
			var materialDetailGridData = $("#materialDetailGrid").data().kendoGrid.dataSource.view();					
			var labourDetailGridData = $("#labourDetailGrid").data().kendoGrid.dataSource.view();					
				
			if (materialDetailGridData.length > 0 && labourDetailGridData.length > 0) {
				displayLoadingDialog();
				saveMaterialDetailGridData(materialDetailGridData);
				saveLabourDetailGridData(labourDetailGridData);

				//Retrieve & save Grid data
				saveJobCard();
            }else {
                smallerWarningDialog('One or More Details grid is/are empty', 'NOTE');
            }
    //});
		}
	});
}

//variable to check that labour date is not ealier than job starting daye
var date = new Date();


//Apply kendo Style to the input fields
function renderControls() {
		if(jobCard.length > 0){date = jobCard.jobDate}
		

		$("#jobDate").width('75%')
			.kendoDatePicker({
				format: 'dd-MMM-yyyy',
				parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
				enable: false,
				value: date
		});
		
		$("#orderStartingDate").width('75%')
			.kendoDatePicker({
				format: 'dd-MMM-yyyy',
				parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
				min: date,
		});
		
		$("#customer").width("75%")
		.kendoComboBox({
			dataSource: customers,
			dataValueField: "customerId",
			dataTextField: "customerName",
			change: onCustomerChange,		
			optionLabel: ""
		});
		
		$("#workOrderNumber").width('75%')
			.kendoComboBox({
			dataSource: workOrders,
			dataValueField: 'workOrderNumber',
			dataTextField: 'workOrderNumber',
			change: onWorkOrderChange,		
			optionLabel: ""
		});	
		
		$("#standardMarkUpRate").width("75%")
		 .kendoNumericTextBox({
			format: "0 '%'",
			value: standardMarkUpRate,
			change: onStandardMarkUpRateChanged,	
			min: 0
		});
		
		$("#standardHourlyBillingRate").width("75%")
		 .kendoNumericTextBox({
			format: "#,##0.00",
			value: standardHourlyBillingRate,
			change: onStandardHourlyBillingRateChanged,
			min: 0			
		});	
		
		$("#totalLabour").width("75%")
		 .kendoNumericTextBox({
			format: "#,##0.00",
		});
		
		$("#totalMaterial").width("75%")
		 .kendoNumericTextBox({
			format: "#,##0.00",
		});	
		
		$("#vatNNhil").width("75%")
		 .kendoNumericTextBox({
			format: "#,##0.00"
		});			
		
		$('#tabs').kendoTabStrip();
		
		
}

function saveMaterialDetailGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            jobCard.jobCardMaterialDetails.push(data[i]);
        }
    }
    else {
	jobCard.jobCardMaterialDetails.push(data[0]);
	}
}

function saveLabourDetailGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            jobCard.jobCardLabourDetails.push(data[i]);
        }
    }
    else {
	jobCard.jobCardLabourDetails.push(data[0]);
	}
}


var onCustomerChange = function() {
	//Retrieve value enter validate
    var id = $("#customer").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < customers.length; i++) {
        if (customers[i].customerId == id) {
            exist = true;
			break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Customer', 'ERROR');
        $("#customer").data("kendoComboBox").value("");
    }
}

var onWorkOrderChange = function() {
	//Retrieve value enter validate
    var order = $("#workOrderNumber").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < workOrders.length; i++) {
        if (workOrders[i].workOrderNumber == order) {
            exist = true;
			selectedWorkOrder = workOrders[i];
			$("#customer").data("kendoComboBox").value(selectedWorkOrder.customerId);
			renderLabourGrid();
			renderMaterialGrid();
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Work Order', 'ERROR');
        $("#workOrderNumber").data("kendoComboBox").value("");
    }
}

var onStandardHourlyBillingRateChanged = function() {
	standardHourlyBillingRate = $("#standardHourlyBillingRate").data("kendoNumericTextBox").value();
}

var onStandardMarkUpRateChanged = function() {
	standardMarkUpRate = $("#standardMarkUpRate").data("kendoNumericTextBox").value();
}

	
	

function renderLabourGrid() {
    $('#labourDetailGrid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(jobCard.jobCardLabourDetails);
                },
                create: function(entries) {
				var data = entries.data;
						if(!data.billingRate > 0) {
							data.billingRate = standardHourlyBillingRate;
							data.billingValue = $('#billableValue').data('kendoNumericTextBox').value();
							data.totalHours = $('#totalHours').data('kendoNumericTextBox').value();
							totalLabour += data.billingValue;
							updateTotalLabour(totalLabour);						
						}
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
                    id: 'jobCardLabourDetailId',
                    fields: {
                        jobCardId: { type: 'number', defaultValue: jobCard.jobCardId, editable: false },
                        jobCardLabourDetailId: { type: 'number', editable: false },
                        labourDate: { type: 'date', editable: false },
                        productionLineDescription: { type: 'string', validation: { required: true } },						
                        starTime: { type: 'date',validation: { required: true } },
						endTime: { type: 'date', validation: { required: true } },
                        activityCode: { type: 'string', validation: { required: true } },						
                        totalHours: { type: 'number', validation: { required: true } },
                        billable: { type: 'bool', validation: { required: true }, defaultValue: false },
						billableHours: { type: 'number' },
                        billingValue: { type: 'number' },						
                    }, //fields
                }, //model
            }, //schema
        }, //datasource
		editable: 'popup',
        columns: [
			{ field: 'labourDate', title: 'Date', format: '{0:dd-MMM-yyyy}' },
			{ field: 'activityCode', title: 'Activity Code', editor: activityCodeEditor },
            { field: 'productionLineDescription', title: 'Description', editor: descriptionEditor},
            { field: 'starTime', title: 'Start Time', format: '{0:hh:mm tt}', editor: startTimeEditor },
            { field: 'endTime', title: 'End Time', format: '{0:hh:mm tt}', editor: endTimeEditor },
            { field: 'totalHours', title: 'Total Hours', editor: totalHoursEditor },
            { field: 'billable', title: 'Billable', editor: billableEditor, 
				template: '<input type="checkbox" disabled="disabled" id="billable" data-bind="checked: billable" #= billable? checked="checked":"" #/>'},
            { field: 'billableHours', title: 'Billable Hours', format: "{0:#,##0.00}", editor: billableHoursEditor },
            { field: 'billingValue', title: 'Bill Value', format: "{0:#,##0.00}", editor: billingValueEditor },
			{ command: ['edit', 'destroy'] , width: 110}			
       ],
		toolbar: [{ name: 'create', text: 'Add Labour Detail' }]	   
    });
}

function renderMaterialGrid() {
	$('#materialDetailGrid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(jobCard.jobCardMaterialDetails);
                },
                create: function(entries) {
				var data = entries.data;
						if(!data.markup > 0) {
							data.markup = standardMarkUpRate;
							data.unitCost = $('#unitCost').data('kendoNumericTextBox').value();
							data.materialCost = $('#materialCost').data('kendoNumericTextBox').value();
							data.materialCharge = $('#materialCharge').data('kendoNumericTextBox').value();
							totalMaterial += data.materialCharge;	
							updateTotalMaterial(totalMaterial);																				
						}				
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
                    entries.success();
                }
            },//transport
            schema: {
                model: {
                    id: 'jobCardMaterialDetailId',
                    fields: {
                        jobCardId: { type: 'number', defaultValue: jobCard.jobCardId, editable: false },
                        jobCardMaterialDetailId: { type: 'number', editable: false },
                        serialNumber: { type: 'string', validation: { required: true } },
                        inventoryItemId: { validation: { required: true } },						
                        materialDescription: { type: 'string', validation: { required: true } },						
						partNumber: { type: 'string', validation: { required: true } },
                        quantity: { type: 'number', validation: { required: true } },						
                        unitOfMeasurementId: { validation: { required: true } },
                        unitCost: { type: 'number', validation: { required: true } },
						materialCost: { type: 'number', validation: { required: true } },
                        materialCharge: { type: 'number', validation: { required: true } },						
                    }, //fields
                }, //model
            }, //schema
        }, //datasource
		editable: 'popup',
        columns: [
            { field: 'inventoryItemId', title: 'Inventory Item',  editor: inventoryItemEditor, template: '#= getInventoryItem(inventoryItemId) #'},
			{ field: 'serialNumber', title: 'Serial Number'},
            { field: 'materialDescription', title: 'Description'},
            { field: 'partNumber', title: 'Part Number'},
            { field: 'quantity', title: 'Quantity', editor: quantityEditor},
            { field: 'unitOfMeasurementId', title: 'Measurement Unit', editor: unitOfMeasurementEditor, template: '#= getUnitOfMeasurement(unitOfMeasurementId) #' },
            { field: 'unitCost', title: 'Unit Cost', format: "{0:#,##0.00}",  editor: unitCostEditor },
            { field: 'materialCost', title: 'Material Cost',  format: "{0:#,##0.00}", editor: materialCostEditor },
            { field: 'materialCharge', title: 'Material Charge',  format: "{0:#,##0.00}", editor: materialChargeEditor },
			{ command: ['edit', 'destroy'] , width: 110}			
       ],
		toolbar: [{ name: 'create', text: 'Add Material Detail' }]	   
    });
}

//retrieve values from from Input Fields and save 
function saveJobCard() {
    retrieveValues();
    saveToServer();
}

function retrieveValues() {
    jobCard.workOrderNumber = $('#workOrderNumber').data('kendoComboBox').value();
	jobCard.jobDate =  $('#jobDate').data('kendoDatePicker').value();
	jobCard.orderStartingDate =  $('#orderStartingDate').data('kendoDatePicker').value();
    jobCard.customerId = $('#customer').data('kendoComboBox').value();
    jobCard.standardMarkUpRate = $('#standardMarkUpRate').data('kendoNumericTextBox').value();
    jobCard.standardHourlyBillingRate = $('#standardHourlyBillingRate').data('kendoNumericTextBox').value();
    jobCard.totalLabour = $('#totalLabour').data('kendoNumericTextBox').value();
    jobCard.totalMaterial = $('#totalMaterial').data('kendoNumericTextBox').value();
    jobCard.vat = vatAmount;
    jobCard.nhil = nhilAmount;

	}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: jobCardApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(jobCard),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Job Card Successfully Saved \n Job Card Number:' + data.jobNumber,
            'SUCCESS', function() { window.location = "/JobCard/JobCards/"; });

    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

function labourDateEditor(container, options) {
	var labourMinDate =$("#orderStartingDate").data(kendoDatePicker).value();

    $('<input type="text" id="labourDate" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoDatePicker({
		format: 'dd-MMM-yyyy',
		parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
		min: labourMinDate
    });
}

function descriptionEditor(container, options) {
    $('<input type="text" id="description" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoMaskedTextBox();
}

function activityCodeEditor(container, options) {
    $('<input type="text" id="activity" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoComboBox({
        dataSource: selectedWorkOrder.workOrderActivities,
        dataValueField: "activityCode",
        dataTextField: "activityCode",
        change: onActivityCodeChange,
        optionLabel: ""
    });
}

function startTimeEditor(container, options) {
    $('<input type="text" id="startTimePicker" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoTimePicker({
		change: onStartTimeChange
	});
}

function endTimeEditor(container, options) {
    $('<input type="text" id="endTimePicker" disabled="disabled" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoTimePicker({
		change: onEndTimePickerChange
	});
}

function inventoryItemEditor(container, options) {
    $('<input type="text" id="inventoryItem" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoComboBox({
        dataSource: inventoryItems,
        dataValueField: "inventoryItemId",
        dataTextField: "inventoryItemName",
        change: onInventoryItemChange,
        optionLabel: ""
    });
}

function unitOfMeasurementEditor(container, options) {
    $('<input type="text" id="unitOfMeasurement" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoComboBox({
        dataSource: unitOfMeasurements,
        dataValueField: "unitOfMeasurementId",
        dataTextField: "unitOfMeasurementName",
        change: onUnitOfMeasurementChange,
        optionLabel: ""
    });
}

function billableEditor(container, options) {
    $('<input type="checkbox" id="billable" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%");
}

function totalHoursEditor(container, options) {
    $('<input type="text" id="totalHours" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoNumericTextBox({
        min: 0
    });
}

function billableHoursEditor(container, options) {
    $('<input type="text" id="billableHours" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoNumericTextBox({
		change: onBillingRateChange,	
        min: 0
    });
}

function billingRateEditor(container, options) {
    $('<input type="text" id="billingRate" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoNumericTextBox({
		change: onBillableHoursChange,
        min: 0
    });
}

function billingValueEditor(container, options) {
    $('<input type="text" id="billableValue" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoNumericTextBox({
        min: 0
    });
}

function unitCostEditor(container, options) {
    $('<input type="text" id="unitCost" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoNumericTextBox({
        min: 0
    });
}

function materialCostEditor(container, options) {
    $('<input type="text" id="materialCost" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoNumericTextBox({
        min: 0
    });
}

function materialChargeEditor(container, options) {
    $('<input type="text" id="materialCharge" readonly data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoNumericTextBox({
        min: 0
    });
}

function quantityEditor(container, options) {
    $('<input type="text" id="quantity" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoNumericTextBox({
        min: 0,
		change: onQuantityChanged
    });
}

function getInventoryItem(id) {
    for (var i = 0; i < inventoryItems.length; i++) {
        if (inventoryItems[i].inventoryItemId == id) 
        return inventoryItems[i].inventoryItemName;
    }
}

function getUnitOfMeasurement(id) {
    for (var i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id) 
        return unitOfMeasurements[i].unitOfMeasurementName;
    }
}


var onInventoryItemChange = function() {
	//Retrieve value and validate
    var id = $("#inventoryItem").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < inventoryItems.length; i++) {
        if ( inventoryItems[i].inventoryItemId == id) {
			$("#unitCost").data("kendoNumericTextBox").value(inventoryItems[i].unitPrice);
            exist = true;
            break;
        }
    }	
	
    if (!exist) {
        warningDialog('Invalid Inventory Item', 'ERROR');
        $("#inventoryItem").data("kendoComboBox").value("");
    }
}

var onUnitOfMeasurementChange = function() {
	//Retrieve value and validate
    var id = $("#unitOfMeasurement").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < unitOfMeasurements.length; i++) {
        if ( unitOfMeasurements[i].unitOfMeasurementId == id) {
            exist = true;
            break;
        }
    }	
	
    if (!exist) {
        warningDialog('Invalid Unit of Measurement', 'ERROR');
        $("#unitOfMeasurement").data("kendoComboBox").value("");
    }
}

var onActivityCodeChange = function() {
	//Retrieve value and validate
    var code = $("#activity").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < selectedWorkOrder.workOrderActivities.length; i++) {
        if ( selectedWorkOrder.workOrderActivities[i].activityCode == code) {
            selWorkOrdActivity = selectedWorkOrder.workOrderActivities[i];
			exist = true;
            break;
        }
    }	
	
    if (!exist) {
        warningDialog('Invalid Activity', 'ERROR');
        $("#activity").data("kendoComboBox").value("");
    }
}

var onStartTimeChange = function() {
    var start = $("#startTimePicker").data("kendoTimePicker").value();
    var end = $("#endTimePicker").data("kendoTimePicker");
    end.enable(true);
	
	end.min(start);
	if(end.value() <  start ){ end.value(start);}
}

var onEndTimePickerChange = function() {
    var start = $("#startTimePicker").data("kendoTimePicker").value();
    var end = $("#endTimePicker").data("kendoTimePicker").value();
	var hourDiff = parseInt(end.getHours() - start.getHours());    

    $("#totalHours").data("kendoNumericTextBox").value(hourDiff);
    $("#billableHours").data("kendoNumericTextBox").max(hourDiff);
}

var onBillableHoursChange = function() {
    var billHours = $("#billableHours").data("kendoNumericTextBox").value();
    var billAmount = billHours * standardHourlyBillingRate;
	
    $("#billableValue").data("kendoNumericTextBox").value(billAmount);
}

var onBillingRateChange = function() {
    var billHours = $("#billableHours").data("kendoNumericTextBox").value();
    var billAmount = billHours * standardHourlyBillingRate;
	
    $("#billableValue").data("kendoNumericTextBox").value(billAmount);
}

var onQuantityChanged = function() {
    var quan = $("#quantity").data("kendoNumericTextBox").value();
    var unitCost = $("#unitCost").data("kendoNumericTextBox").value();

    var matCost = quan * unitCost;
    var matCharge = matCost + (matCost * (standardMarkUpRate/100));
	
    $("#materialCost").data("kendoNumericTextBox").value(matCost);
    $("#materialCharge").data("kendoNumericTextBox").value(matCharge);
}

function updateTotalLabour(currentTotal){
	$('#totalLabour').data('kendoNumericTextBox').value(currentTotal);

	var lab = $('#totalLabour').data('kendoNumericTextBox').value();
    var mat = $('#totalMaterial').data('kendoNumericTextBox').value();
		
	vatAmount = (mat+lab) * vatRate;
	nhilAmount = (mat+lab) * nhilRate;

	$('#vatNNhil').data('kendoNumericTextBox').value(vatAmount + nhilAmount);
}

function updateTotalMaterial(currentMaterial){
    $('#totalMaterial').data('kendoNumericTextBox').value(currentMaterial);
	var lab = $('#totalLabour').data('kendoNumericTextBox').value();
    var mat = $('#totalMaterial').data('kendoNumericTextBox').value();
		
	vatAmount = (mat+lab) * vatRate;
	nhilAmount = (mat+lab) * nhilRate;

	$('#vatNNhil').data('kendoNumericTextBox').value(vatAmount + nhilAmount);
}





