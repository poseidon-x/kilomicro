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
var companyProfile = {};

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});


var jobCardAjax = $.ajax({
    url: jobCardApiUrl + '/Get/' + id,
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
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var unitOfMeasurementAjax = $.ajax({
    url: unitOfMeasurementApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
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

            //Prepares UI
            prepareUi();
        });
}

//Function to prepare user interface
function prepareUi() {

    //If jobCardId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (jobCard.jobCardId > 0) {
        renderControls();
        populateUi();
		renderLabourGrid();
		renderMaterialGrid();
        dismissLoadingDialog();
    }else{
        warningDialog("No Job Card Specified", 'SORRY');		
	}

    //Validate to Check Empty/Null input Fields
    $('#approve').click(function (event) {
        //CHECK IF INVOICE IS NOT POSTED
        if (!jobCard.approved) {
            if (confirm('Are you sure you want Approve this Job Card?')) {
                displayLoadingDialog();
                saveApproval();
            } else {
                smallerWarningDialog('Job card approved already', 'NOTE');
            }
        }
    });
}



//Apply kendo Style to the input fields
function renderControls() {

    $("#jobDate").width('75%')
        .kendoDatePicker({
            format: 'dd-MMM-yyyy',
            parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
            enable: false
        });

    $("#orderStartingDate").width('75%')
        .kendoDatePicker({
            format: 'dd-MMM-yyyy',
            parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
        });

    $("#customer").width("75%")
    .kendoComboBox({
        dataSource: customers,
        dataValueField: "customerId",
        dataTextField: "customerName",
        optionLabel: ""
    });

    $("#workOrderNumber").width('75%')
        .kendoComboBox({
            dataSource: workOrders,
            dataValueField: 'workOrderNumber',
            dataTextField: 'workOrderNumber',
            optionLabel: ""
        });

    $("#standardMarkUpRate").width("75%")
     .kendoNumericTextBox({
         format: "0 '%'",
     });

    $("#standardHourlyBillingRate").width("75%")
     .kendoNumericTextBox({
         format: "#,##0.00"     });

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
         format: "#,##0.00",
     });

    $('#tabs').kendoTabStrip();
}

function populateUi() {
	
	$('#workOrderNumber').data('kendoComboBox').value(jobCard.workOrderNumber);
    $('#jobDate').data('kendoDatePicker').value(jobCard.jobDate);
    $('#orderStartingDate').data('kendoDatePicker').value(jobCard.orderStartingDate);	
    $('#customer').data('kendoComboBox').value(jobCard.customerId);
    $('#standardMarkUpRate').data('kendoNumericTextBox').value(jobCard.standardMarkUpRate);
    $('#standardHourlyBillingRate').data('kendoNumericTextBox').value(jobCard.standardHourlyBillingRate);
	$('#totalLabour').data('kendoNumericTextBox').value(jobCard.totalLabour);
    $('#totalMaterial').data('kendoNumericTextBox').value(jobCard.totalMaterial);
	var vatNNhil = jobCard.vat + jobCard.nhil;
    $('#vatNNhil').data('kendoNumericTextBox').value(vatNNhil);
}

function renderLabourGrid() {
    $('#labourDetailGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(jobCard.jobCardLabourDetails);
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
                    id: 'jobCardLabourDetailId',
                    fields: {
                        jobCardId: { type: 'number', defaultValue: jobCard.jobCardId, editable: false },
                        jobCardLabourDetailId: { type: 'number', editable: false },
                        labourDate: { type: 'date', editable: false },
                        productionLineDescription: { type: 'string', editable: false },
                        starTime: { type: 'date', editable: false },
                        endTime: { type: 'date', editable: false },
                        activityCode: { type: 'string', editable: false },
                        totalHours: { type: 'number',  editable: false },
                        billable: { type: 'bool', editable: false },
                        billableHours: { type: 'number', editable: false },
                        billingValue: { type: 'number', editable: false },
                    }, //fields
                }, //model
            }, //schema
        }, //datasource
        columns: [
			{ field: 'labourDate', title: 'Date', format: '{0:dd-MMM-yyyy}' },
			{ field: 'activityCode', title: 'Activity Code' },
            { field: 'productionLineDescription', title: 'Description' },
            { field: 'starTime', title: 'Start Time', format: '{0:hh:mm tt}' },
            { field: 'endTime', title: 'End Time', format: '{0:hh:mm tt}' },
            { field: 'totalHours', title: 'Total Hours', },
            {
                field: 'billable', title: 'Billable',
                template: '<input type="checkbox" disabled="disabled" id="billable" data-bind="checked: billable" #= billable? checked="checked":"" #/>'
            },
            { field: 'billableHours', title: 'Billable Hours', format: '{0:#,##0.00}' },
            { field: 'billingValue', title: 'Bill Value', format: '{0:#,##0.00}' },
        ],
    });
}

function renderMaterialGrid() {
    $('#materialDetailGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(jobCard.jobCardMaterialDetails);
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
            },//transport
            schema: {
                model: {
                    id: 'jobCardMaterialDetailId',
                    fields: {
                        jobCardId: { type: 'number', defaultValue: jobCard.jobCardId, editable: false },
                        jobCardMaterialDetailId: { type: 'number', editable: false },
                        serialNumber: { type: 'string', editable: false },
                        inventoryItemId: { editable: false },
                        materialDescription: { type: 'string', editable: false },
                        partNumber: { type: 'string', editable: false },
                        quantity: { type: 'number', editable: false },
                        unitOfMeasurementId: { editable: false },
                        unitCost: { type: 'number', editable: false },
                        materialCost: { type: 'number', editable: false },
                        materialCharge: { type: 'number', editable: false },
                    }, //fields
                }, //model
            }, //schema
        }, //datasource
        columns: [
            { field: 'inventoryItemId', title: 'Inventory Item', template: '#= getInventoryItem(inventoryItemId) #' },
			{ field: 'serialNumber', title: 'Serial Number' },
            { field: 'materialDescription', title: 'Description' },
            { field: 'partNumber', title: 'Part Number' },
            { field: 'quantity', title: 'Quantity' },
            { field: 'unitOfMeasurementId', title: 'Measurement Unit', template: '#= getUnitOfMeasurement(unitOfMeasurementId) #' },
            { field: 'unitCost', title: 'Unit Cost', format: "{0:#,##0.00}" },
            { field: 'materialCost', title: 'Material Cost', format: "{0:#,##0.00}" },
            { field: 'materialCharge', title: 'Material Charge', format: "{0:#,##0.00}" },
        ],
    });
}

//retrieve values from from Input Fields and save 
function saveApproval() {
    saveToServer();
}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: jobCardApiUrl + '/ApproveJobCard',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(jobCard),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Job Card Successfully Approved \n Job Card Number:' + data.jobNumber, 'SUCCESS', function () { window.location = "/JobCard/JobCards/";}); 

    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}//func saveToServer


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
