//******************************************//
//  			     INVOICE - JAVASCRIPT             		//
// 		CREATOR: EMMANUEL OWUSU(MAN)			//
//				 DATE:  												//
//******************************************//


//"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var invoiceApiUrl = coreERPAPI_URL_Root + "/crud/ArInvoice";
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/inventoryItem";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/unitOfMeasurement";
var paymentTermApiUrl = coreERPAPI_URL_Root + "/crud/paymentTerm";
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var salesOrderApiUrl = coreERPAPI_URL_Root + "/crud/salesOrder";
var jobCardApiUrl = coreERPAPI_URL_Root + "/crud/jobCard";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var companyProfileApiUrl = coreERPAPI_URL_Root + "/crud/companyProfile";



//Declaration of variables to store records retrieved from the database
var arInvoice = {}
var inventoryItems = {};
var locations = {};
var accounts = {};
var unitOfMeasurements = {};
var paymentTerms = {};
var customers = {};
var salesOrder = {};
var jobCard = {};
var currencies = {};
var companyProfile = {};
var subTotalAfterDiscount = 0;
var invoiceTotal = 0;
var vatNnhilApplied = false;
var withApplied = false;





//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//Declare a variable and store shrinkageBatch table ajax call in it
var invoiceAjax = $.ajax({
    url: invoiceApiUrl + '/Get/' + invoiceId,
    type: 'Get',
    contentType: 'application/json',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store location table ajax call in it
var inventoryItemsAjax = $.ajax({
    url: inventoryItemApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store unitOfMeasurement table ajax call in it
var locationsAjax = $.ajax({
    url: locationApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var accountsAjax = $.ajax({
    url: acctsApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store unitOfMeasurement table ajax call in it
var unitOfMeasurementsAjax = $.ajax({
    url: unitOfMeasurementApiUrl + '/Get',
    type: 'Get',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var paymentTermAjax = $.ajax({
    url: paymentTermApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var customersAjax = $.ajax({
    url: customerApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var salesOrderAjax = $.ajax({
    url: salesOrderApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var jobCardAjax = $.ajax({
    url: jobCardApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store currency table ajax call in it
var currencyAjax = $.ajax({
    url: currencyApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store currency table ajax call in it
var companyProfileAjax = $.ajax({
    url: companyProfileApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
    $.when(invoiceAjax, inventoryItemsAjax, locationsAjax, accountsAjax, unitOfMeasurementsAjax, paymentTermAjax, customersAjax, salesOrderAjax, jobCardAjax, currencyAjax, companyProfileAjax)
        .done(function (dataInvoice, dataInventoryItems, dataLocations, dataAccounts, dataUnitOfMeasurements, dataPaymentTerm, dataCustomers, dataSalesOrder, dataJobCard, dataCurrency, dataCompanyProfile) {
            arInvoice = dataInvoice[2].responseJSON;
			inventoryItems = dataInventoryItems[2].responseJSON;
            locations = dataLocations[2].responseJSON;
			accounts = dataAccounts[2].responseJSON;
            unitOfMeasurements = dataUnitOfMeasurements[2].responseJSON;
            paymentTerms = dataPaymentTerm[2].responseJSON;
            customers = dataCustomers[2].responseJSON;
            salesOrder = dataSalesOrder[2].responseJSON;
			jobCard = dataJobCard[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;	
            companyProfile = dataCompanyProfile[2].responseJSON;	

            //Prepares UI
            prepareUi();
        });
}


//Function to prepare user interface
function prepareUi() 
{
	$('#tabs').kendoTabStrip();
	$('#salesOrder1').click(function (event) 
	{
		//
		if(document.getElementById('salesOrder1').checked) 
		{
			//retrieve sales Orders set sales order &&  invoice Date controls 
			document.getElementById('salesOrderTitle').innerHTML ='Sales Order';
			document.getElementById('salesOrderInput').innerHTML ='<input type="text" id="salesOrder" class="form-control" required data-required-msg="Invalid" />';
			document.getElementById('invoiceDateTitle').innerHTML ='Invoice Date';
			document.getElementById('invoiceDateInput').innerHTML ='<input type="text" id="invoiceDate" class="form-control" required data-required-msg="Invalid" readonly />';
			
			document.getElementById("customer").readonly = true;	
			$("#salesOrder").width('70%')
			.kendoComboBox(
			{
				dataSource: salesOrder,
				dataValueField: 'salesOrderId',
				dataTextField: 'orderNumber',
				change: onSalesOrderChange,										
				filter: "contains",
				highlightFirst: true,
				suggest: true,
				ignoreCase: true,
				animation: {
					close: { effects: "fadeOut zoom:out", duration: 200 },
					open: { effects: "fadeIn zoom:in", duration: 200 }
				},
				optionLabel: ''
			});
			
			$("#invoiceDate").width('70%')
			.kendoDatePicker(
			{
				format: 'dd-MMM-yyyy',
				parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
				min: dat, // sets min date 	
				max: new Date(), // sets max date
				animation: {
					close: { effects: "fadeOut zoom:out", duration: 200 },
					open: { effects: "fadeIn zoom:in", duration: 200 }
				},
				value: 	new Date()
			});
			var dat = new Date();
			dat.setDate(dat.getDate() - 1);
		}	
	});

    $('#salesOrder2').click(function (event) {	
		
	if(document.getElementById('salesOrder2').checked) {
		// remove grid if any && disable taxes customer Control
		removeGrid(); 
		document.getElementById("customer").readonly = true;
		document.getElementById("isWith").disabled = false;
		document.getElementById("isVAT_NHIL").disabled = false;	

		//retrieve Job Cards set Job Card &&  invoice Date controls 
		document.getElementById('salesOrderTitle').innerHTML ='Job Card';
		document.getElementById('salesOrderInput').innerHTML ='<input type="text" id="salesOrder" class="form-control" required data-required-msg="Invalid" />';
		document.getElementById('invoiceDateTitle').innerHTML ='Invoice Date';
		document.getElementById('invoiceDateInput').innerHTML ='<input type="text" id="invoiceDate" class="form-control" required data-required-msg="Invalid" readonly />';

		//Reset other controls to empty if  any
		$("#customer").data("kendoComboBox").value("");
		$("#paymentTerm").data("kendoComboBox").value("");
		$("#currency").data("kendoComboBox").value("");
		
		$("#salesOrder").width('70%')
			.kendoComboBox({
				dataSource: jobCard,
				dataValueField: 'jobCardId',
				dataTextField: 'jobNumber',
				change: onjobCardChange,										
				filter: "contains",
				highlightFirst: true,
				suggest: true,
				ignoreCase: true,
				animation: {
					close: { effects: "fadeOut zoom:out", duration: 200 },
					open: { effects: "fadeIn zoom:in", duration: 200 }
				},
				optionLabel: ''
			});
	
		$("#invoiceDate").width('70%')
			.kendoDatePicker({
				format: 'dd-MMM-yyyy',
				parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
				min: dat, // sets min date 
				max: new Date(), // sets max date
				animation: {
					close: { effects: "fadeOut zoom:out", duration: 200 },
					open: { effects: "fadeIn zoom:in", duration: 200 }
				},
				value: 	new Date()
			});
			
		var dat = new Date();
		dat.setDate(dat.getDate() - 1);	
		}	
	});	
	
	$('#isVAT_NHIL').click(function (event) {
		var grid = $("#grid").data("kendoGrid");	
			
		if (grid) {
			var invoiceLineGridData = $("#grid").data().kendoGrid.dataSource.view();
			if(!vatNnhilApplied){
				var vatNnhilTotal =0;
				var vatNnhilRate = companyProfile[0].vat_rate + companyProfile[0].nhil_rate;
			
					// If subTotalAfterDiscount > 0, then subTotalAfterDiscount has been computed already, so we calculate VAT 
					if(subTotalAfterDiscount > 0){
						vatNnhilTotal = (vatNnhilRate * subTotalAfterDiscount).toFixed(2);	
					}// Else, subTotalAfterDiscount has not been computed, so we compute
					else{
						for(var i = 0;i < invoiceLineGridData.length;i++){
							subTotalAfterDiscount += invoiceLineGridData[i].netAmount;
						}
						//After computing, we calculate VAT of the Amount.
						vatNnhilTotal = (vatNnhilRate * subTotalAfterDiscount).toFixed(2);						
					}	
						
					// If  grid is available, we add a new row with VAT details
					if (grid) {
						var dataSource = grid.dataSource;
						var total = dataSource.data().length;					
						dataSource.insert(total, {
							lineNumber: total+1,
							description: "VAT & NHIL (" + (vatNnhilRate * 100) + "%)",
							unitPrice: vatNnhilTotal,
							quantity: 1,
							unitOfMeasurementId: "",
							discountAmount: 0,
							totalAmount: vatNnhilTotal,
							netAmount: vatNnhilTotal							
						});
					}
					vatNnhilApplied = true;
				
			}else{
			
				for(var i = 0;i < invoiceLineGridData.length;i++){
					if(grid.dataSource.data()[i].description.includes("VAT & NHIL"))
					{ 
						item = grid.dataSource.data()[i];					
						grid.dataSource.remove(item);
						vatNnhilApplied = false;
						
					}
				}
				if(!withApplied){subTotalAfterDiscount = 0;}
			}		
		}else{
			warningDialog("Please Select a Sales Order", "NOTE");
		}	
	});	


     
	$('#isWith').click(function (event) {
		var grid = $("#grid").data("kendoGrid");
		if (grid) {
			var invoiceLineGridData = $("#grid").data().kendoGrid.dataSource.view();     
			var withTotal = 0;
			var  withTotalRate = companyProfile[0].withh_rate;
			var vatNnhilTotal = 0;
			var vatNnhilRate = (companyProfile[0].vat_rate + companyProfile[0].nhil_rate);

			if(!withApplied){
					if(subTotalAfterDiscount > 0){
						withTotal = (withTotalRate * subTotalAfterDiscount).toFixed(2);
						subTotalAfterDiscount -= withTotal;
					}else{
						for(var i = 0;i < invoiceLineGridData.length;i++){
							subTotalAfterDiscount += invoiceLineGridData[i].netAmount;
						}	
						withTotal = (withTotalRate * subTotalAfterDiscount).toFixed(2);
						subTotalAfterDiscount -= withTotal;
					}
					$('#grid').data("kendoGrid").dataSource.read();

					var dataSource = grid.dataSource;
					var total = dataSource.data().length;		
					vatNnhilTotal = vatNnhilRate * subTotalAfterDiscount;		

					if(vatNnhilApplied){
						dataSource.insert(total, {
							description: "VAT & NHIL (" + (vatNnhilRate * 100) + "%)",
							unitPrice: vatNnhilTotal,
							quantity: 1,
							unitOfMeasurementId: "",
							discountAmount: 0,
							totalAmount: vatNnhilTotal,
							netAmount: vatNnhilTotal						
						});
						total++;
					}							
					dataSource.insert(total, {
						description: "WITHHOLDING TAX (" + (withTotalRate * 100) + "%)",
						unitPrice: withTotal,
						quantity: 1,
						unitOfMeasurementId: null,
						discountAmount: 0,
						totalAmount: withTotal,
						netAmount: withTotal							
					});

					withApplied = true;
			}else{
				var dataSource = grid.dataSource;
				var withHoldingAmount = 0.0;
				alert( invoiceLineGridData.length);
				for(var i = 0;i < invoiceLineGridData.length;i++){					
					if(dataSource.data()[i].description.includes("WITHHOLDING TAX"))
						{ 
							var item = grid.dataSource.data()[i];
							withHoldingAmount = item.unitPrice;
							//remove the with-holding tax item
							grid.dataSource.remove(item);							
							withApplied = false;
							
							subTotalAfterDiscount = parseFloat(withHoldingAmount) + parseFloat(subTotalAfterDiscount);
							
							if(!vatNnhilApplied){subTotalAfterDiscount = 0;}
							else{
								$('#grid').data("kendoGrid").dataSource.read();
								alert(vatNnhilRate);
								alert(subTotalAfterDiscount);
								vatNnhilTotal = vatNnhilRate * subTotalAfterDiscount;
								var total = dataSource.data().length;		
								
								alert(vatNnhilTotal);
								
								dataSource.insert(total, {
									description: "VAT & NHIL (" + (vatNnhilRate * 100) + "%)",
									unitPrice: vatNnhilTotal,
									quantity: 1,
									unitOfMeasurementId: "",
									discountAmount: 0,
									totalAmount: vatNnhilTotal,
									netAmount: vatNnhilTotal						
								});
							}	
							break;
						}
						
				
				}

			}	
		}else{warningDialog("Please Select a Sales Order", "NOTE");}	
	});
	
		
    //If arInvoiceId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (arInvoice.arInvoiceId > 0) {
        //renderControls();
        //populateUi();
        //renderGrid();
        dismissLoadingDialog();
    } else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        renderControls();
        dismissLoadingDialog();
    }

//Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {
	
        var validator = $("#myform").kendoValidator().data("kendoValidator");
				
		if(document.getElementById('salesOrder1').checked){
			// Remove the tax components so that we save the line items
			$('#grid').data("kendoGrid").dataSource.read();
			//Retrieve & save Grid data
			var invoiceLineGridData = $("#grid").data().kendoGrid.dataSource.view();
		
		
			for(var i = 0;i < invoiceLineGridData.length;i++){
				invoiceTotal += invoiceLineGridData[i].totalAmount;
				if(vatApplied){
					arInvoice.arInvoiceLines[i].isVat = true;
				}
				if(nhilApplied){
					arInvoice.arInvoiceLines[i].isNHIL = true;
				}
				if(withApplied){
					arInvoice.arInvoiceLines[i].isWith = true;
				}			
			}	
		}

        if (!validator.validate()) {
            smallerWarningDialog('One or More Fields has Invalid value', 'ERROR');
        } else {
			if(document.getElementById('salesOrder1').checked) {
				if (invoiceLineGridData.length > 0) {
					displayLoadingDialog();
					saveInvoice();
				}else {
					smallerWarningDialog('Please Add Shrinkage Details', 'NOTE');
				}			
			} 
			if(document.getElementById('salesOrder2').checked) {
				displayLoadingDialog();
				saveInvoice();
			}

        }
    });
}

//Apply kendo Style to the input fields
function renderControls() 
	{
		$("#customer").width('90%')
			.kendoComboBox({
			dataSource: customers,
			dataValueField: 'customerId',
			dataTextField: 'customerName',
			filter: "contains",
			highlightFirst: true,
			suggest: true,
			ignoreCase: true,
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 200 },
				open: { effects: "fadeIn zoom:in", duration: 200 }
			},
			optionLabel: ''
		});
		
		$("#paymentTerm").width('90%')
			.kendoComboBox({
			dataSource: paymentTerms,
			dataValueField: 'paymentTermID',
			dataTextField: 'paymentTerms',
			change: onPaymentTermChange,				
			filter: "contains",
			highlightFirst: true,
			suggest: true,
			ignoreCase: true,
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 200 },
				open: { effects: "fadeIn zoom:in", duration: 200 }
			},
			optionLabel: ''
		});	
		
		$("#currency").width('90%')
		.kendoComboBox({
			dataSource: currencies,
			dataValueField: 'currency_id',
			dataTextField: 'major_name',
			change: onCurrencyChange,		
			filter: "contains",
			highlightFirst: true,
			suggest: true,
			ignoreCase: true,
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 200 },
				open: { effects: "fadeIn zoom:in", duration: 200 }
			},
			optionLabel: ''
		});	
	}

var onPaymentTermChange = function() {

    var id = $("#paymentTerm").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < paymentTerms.length; i++) {
        if (paymentTerms[i].paymentTermID == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Payment Term', 'ERROR');
        $("#paymentTerm").data("kendoComboBox").value("");
    }
}

var onCurrencyChange = function() {
	//Retrieve value enter validate
    var id = $("#currency").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < currencies.length; i++) {
        if (currencies[i].currency_id == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Currency', 'ERROR');
        $("#currency").data("kendoComboBox").value("");
    }
}

var onjobCardChange = function() {
	//Retrieve value enter validate
    var id = $("#salesOrder").data("kendoComboBox").value();
	var customer = $("#customer").data("kendoComboBox");

    var exist = false;

    for (var i = 0; i < jobCard.length; i++) {
        if (jobCard[i].jobCardId == id) {
			customer.value(jobCard[i].customerId);
			invoiceTotal = jobCard[i].totalLabour + jobCard[i].totalMaterial;
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Job Card', 'ERROR');
        $("#salesOrder").data("kendoComboBox").value("");
    }
}


var onSalesOrderChange = function () {
	var selectedSalesOrder = {};
	var customer = $("#customer").data("kendoComboBox");
	var paymentTerm = $("#paymentTerm").data("kendoComboBox");
	var currency = $("#currency").data("kendoComboBox");
    
    var id = $("#salesOrder").data("kendoComboBox").value();
	var exist = false;

    for (var i = 0; i < salesOrder.length; i++) {
        if (salesOrder[i].salesOrderId == id) {
			selectedSalesOrder = salesOrder[i];
            exist = true;
            break;
        }
    }

    if (exist) {
	    displayLoadingDialog();

		$.ajax(
        {
            url: invoiceApiUrl + '/GetInvoiceBySalesOrderId/' + id,
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            arInvoice = data;
			renderGrid();
			document.getElementById("isWith").disabled = false;
			document.getElementById("isVAT_NHIL").disabled = false;
            dismissLoadingDialog();
        }).error(function (error) {
				alert(JSON.stringify(error));
        });		
	
				customer.value(selectedSalesOrder.customerId);
				paymentTerm.value(selectedSalesOrder.paymentTermId);
				currency.value(selectedSalesOrder.currencyId);
		
    }else{
        warningDialog('Invalid Sales Order', 'ERROR');
        $("#salesOrder").data("kendoComboBox").value("");
		customer.value("");
		paymentTerm.value("");
		currency.value("");			
	}
}


//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(arInvoice.arInvoiceLines);
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
                    id: 'arInvoiceLineId',
                    fields: {
                        arInvoiceId: { type: 'number', defaultValue: arInvoice.arInvoiceId },
                        arInvoiceLineId: { type: 'number', editable: false },
                        inventoryItemId: { type: 'number', editable: false },
                        description: { type: 'string', editable: false },
                        unitPrice: { type: 'number', format: "{0:c}", editable: false },
                        quantity: { type: 'number', editable: false },
                        unitOfMeasurementId: { type: 'number', editable: false },
                        discountPercentage: { type: 'number', editable: false },						
                        discountAmount: { type: 'number', editable: false },
                        totalAmount: { type: 'number', format: "{0:c}", editable: false },
                        netAmount: { type: 'number', format: "{0:c}", editable: false },
                    }, //fields
                }, //model
            }, //schema
        }, //datasource
        columns: [
            { field: 'description', title: 'Description', width: 180},
            { field: 'unitPrice', title: 'Unit Price', width: 80,format: "{0:0.00}" },
            { field: 'quantity', title: 'Quantity', width: 80,  },
            { field: 'unitOfMeasurementId', title: 'Unit Of Measurement', width: 150 ,template: '#= getUnitOfMeasurement(unitOfMeasurementId) #'},
            { field: 'discountAmount', title: 'Discount',format: "{0:0.00}" },
            { field: 'totalAmount', title: 'Line Total',format: "{0:0.00}"},
            { field: 'netAmount', title: 'Line Net Total',format: "{0:0.00}"}			
        ],
    });
}

function removeGrid(){
    document.getElementById('grid').innerHTML ='';

	subTotalAfterDiscount = 0;
	invoiceTotal = 0;
	vatNnhilApplied = false;
	withApplied = false;
}


//retrieve values from from Input Fields and save 
function saveInvoice() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
	if(document.getElementById('salesOrder1').checked) {
		arInvoice.salesOrderId = $('#salesOrder').data('kendoComboBox').value();
		if(document.getElementById('isVAT').checked){
			arInvoice.isVat = true;
			arInvoice.vatRate = companyProfile[0].vat_rate; 
		}
		if(document.getElementById('isNHIL').checked){		
			arInvoice.isWith = true;
			arInvoice.withRate = companyProfile[0].withh_rate;
		}
	}else if(document.getElementById('salesOrder2').checked) {
		arInvoice.jobCardId = $('#salesOrder').data('kendoComboBox').value();
		arInvoice.isVat = true;
		arInvoice.vatRate = companyProfile[0].vat_rate;
		arInvoice.isWith = true;
		arInvoice.withRate = companyProfile[0].withh_rate;
	}

    arInvoice.invoiceDate = $('#invoiceDate').data('kendoDatePicker').value();
	arInvoice.customerId = $('#customer').data('kendoComboBox').value();
	arInvoice.customerName = $('#customer').data('kendoComboBox').text();
	arInvoice.totalAmount = invoiceTotal;
	arInvoice.balance = invoiceTotal;

	arInvoice.paymentTermId  = $('#paymentTerm').data('kendoComboBox').value();
	arInvoice.currencyId   = $('#currency').data('kendoComboBox').value();
    }

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: invoiceApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(arInvoice),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Invoice Successfully Saved\n Invoice Number:'+data.invoiceNumber, 'SUCCESS', function () { window.location = "/ArInvoice/Invoices/"; });
        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer


//retrieve unit of Measurement value from the Grid pop and display on the Grid
function getUnitOfMeasurement(id) {
    for (var i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id)
            return unitOfMeasurements[i].unitOfMeasurementName;
    }
}



