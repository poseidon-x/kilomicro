'use strict';

var depositApiUrl = coreERPAPI_URL_Root + "/crud/Deposit";
var clientsApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var paymentModeApiUrl = coreERPAPI_URL_Root + "/crud/modeOfpayment";
var banksApiUrl = coreERPAPI_URL_Root + "/crud/banks";
var depositTypeApiUrl = coreERPAPI_URL_Root + "/crud/depositType"; 
var depositRepaymentModeApiUrl = coreERPAPI_URL_Root + "/crud/DepositRepaymentMode";
var relationshipOfficersApiUrl = coreERPAPI_URL_Root + "/crud/Staff";
var fieldAgentsApiUrl = coreERPAPI_URL_Root + "/crud/Agent";
var idTypeApiUrl = coreERPAPI_URL_Root + "/crud/idType";
var relationshipTypeApiUrl = coreERPAPI_URL_Root + "/crud/relationshipType";
var depositPeriodInDaysApiUrl = coreERPAPI_URL_Root + "/crud/depositPeriodInDays";

var deposit = {};
var clients = {};
var depositTypes = {};
var staff = {};
var fieldAgents = {};
var idTypes = {};
var relationTypes = {};
var depositPeriods = {};
var currentModel = {}; 
var currentPhotoContent='';
var currentPhotoContents=[];

var imageId = -1;

var depositAjax = $.ajax({
    url: depositApiUrl + "/GetNewDeposit/",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var depositPeriodInDaysAjax = $.ajax({
    url: depositPeriodInDaysApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var relationshipTypeAjax = $.ajax({
    url: relationshipTypeApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var idTypeAjax = $.ajax({
    url: idTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var depositTypeAjax = $.ajax({
    url: depositTypeApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var relationshipOfficerAjax = $.ajax({
    url: relationshipOfficersApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var fieldAgentAjax = $.ajax({
    url: fieldAgentsApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
function loadData() {

    $.when(depositAjax, depositTypeAjax,depositPeriodInDaysAjax, relationshipOfficerAjax, 
	fieldAgentAjax,relationshipTypeAjax,idTypeAjax)
        .done(function (dataDeposit, dataDeposiType, dataDepositPeriodInDays, dataRelationshipOfficer,
		dataFieldAgent,dataRelationshipType,dataIdType) {
            deposit = dataDeposit[2].responseJSON;
            depositTypes = dataDeposiType[2].responseJSON;
			depositPeriods = dataDepositPeriodInDays[2].responseJSON;
            staff = dataRelationshipOfficer[2].responseJSON;
            fieldAgents = dataFieldAgent[2].responseJSON;
			relationTypes = dataRelationshipType[2].responseJSON;
            idTypes = dataIdType[2].responseJSON;

            dismissLoadingDialog();
			prepareUI();
        }
	);
}


$(function () {
    displayLoadingDialog();
    loadData();
});

function prepareUI() {
	$("#details").hide();
	renderControls();
	$("#searchBtn").click(function(){
		var searchInput = $("#searchInput").val();
		if(searchInput.length>2){
			var input = {searchString:searchInput};
			getClients(input);
		}else{
			warningDialog("Minimum of three(3) characters required","Note");
		}
	});
	$('#save').click(function (event) {
		if (confirm("Are you sure you want create investment account")) {
			displayLoadingDialog();
			savedeposit();				
		} else
		{
			smallerWarningDialog('Please review and create later', 'NOTE');
		}			
	});
		
}

function renderControls(){
	$("#tabs").kendoTabStrip();
	$("#client").width("90%").kendoComboBox({
		dataSource: clients,
		filter: "contains",
		suggest: true,
		dataValueField: "clientID",
		dataTextField: "clientNameWithAccountNO",
		highlightFirst: true,
		ignoreCase: true,
		change: onClientChange,
		optionLabel: "",
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		}
	});		
	$('#investmentProduct').width('90%').kendoComboBox({
		dataSource: depositTypes,
		dataValueField: 'depositTypeID',
		dataTextField: 'depositTypeName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		change: onProductChange,
		optionLabel: ''
	});
	$('#relationshipOfficer').width('90%').kendoComboBox({
		dataSource: staff,
		dataValueField: 'staffId',
		dataTextField: 'staffName',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		optionLabel: ''
	});
	$('#fieldAgent').width('90%').kendoComboBox({
		dataSource: fieldAgents,
		dataValueField: 'agentId',
		dataTextField: 'agentNameWithNo',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		optionLabel: ''
	});
	$('#depositPeriod').width('90%').kendoComboBox({
		dataSource: depositPeriods,
		dataValueField: 'periodInDays',
		dataTextField: 'period',
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		optionLabel: ''
	});
	$('#annualInterestRate').width('90%').kendoNumericTextBox({
		format: "0.#0 '%'",
		min: 0
	});
	renderGrid();
}
function renderGrid(){
	$('#nextOfKinGrid').kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
						entries.success(deposit.depositNextOfKins);                    
					},
					create: function (entries) {
						entries.success(entries.data);
					},
					update: function (entries) {
						entries.success(entries.data);
					},
					destroy: function (entries) {
						entries.success(entries.data);
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'depositNextOfKinId',
                    fields: {
						depositNextOfKinId: { type: 'number',editable: false },
						depositId: { type: 'number',editable: false },
						surName: { editable: true, validation: { required: true } },
						otherNames: { editable: true, validation: { required: true, min: 1 } },
						dateOfBirth: { type: 'date', editable:true,validation: { required: true } },
						relationshipTypeId: {editable:true,validation: { required: true } },
						idTypeId: { editable:true,validation: { required: true } },						
						idNumber: { editable:true,validation: { required: true } },
						phoneNumber: { editable:true,validation: { required: true } },
						percentageAllocated: { editable:true,validation: { required: true,max: 100,min:1 } }						
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'surName',title: 'Surname', editor: textEditor },
            { field: 'otherNames', title: 'Other Names', editor: textEditor },  
            { field: 'dateOfBirth', title: 'Date of Birth', editor: dateEditor, format: "{0:dd-MMM-yyyy}" },            			
            { field: 'relationshipTypeId', title: 'Relation Type', editor: relationTypeEditor,template: '#= getRelationType(relationshipTypeId) #' },            
            { field: 'idTypeId', title: 'ID Type',editor: idTypeEditor, template: '#= getIdType(idTypeId) #' },  
            { field: 'idNumber', title: 'ID No.', editor: textEditor },            			
            { field: 'phoneNumber', title: 'Phone', editor: phoneNumberEditor },            
            { field: 'percentageAllocated', title: 'Percentage',editor: percentageEditor, template: '#= getPercentage(percentageAllocated) #' },			
            { command: ['edit','destroy'],width:200 }
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		toolbar: [
			{
				name: "create",
				text: 'Add next of kin',
			}
		],
		editable: "popup",
		edit: function(e) {
			var editWindow = this.editable.element.data("kendoWindow");
			editWindow.wrapper.css({ width: 500 });
			editWindow.title("Next of kin");
		  },
		mobile: true
    });
	
	$('#signatoriesGrid').kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
						entries.success(deposit.depositSignitoriesModel);
					},
					create: function (entries) {
						entries.success(entries.data);
					},
					update: function (entries) {
						entries.success(entries.data);
					},/*
					destroy: function (entries) {
					    for (var i = 0; i < employee.employeePhotoes.length; i++) {
					        if (employee.employeePhotoes[i].employeePhotoId == entries.data.employeePhotoId) {
					            employee.employeePhotoes.splice(i, 1);
					        }
					    }
					    for (var i = 0; i < currentPhotoContents.length; i++) {
					        if (currentPhotoContents[i].employeePhotoId == entries.data.employeePhotoId) {
					            currentPhotoContents.splice(i, 1);
					        }
					    }
						entries.success(employee.employeePhotoes);	
					},*/
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'depositSignatoryID',
                    fields: {
						depositSignatoryID: { editable: false, type: 'number' },												
						depositID: { editable: false, type: 'number' },						
						signatoryName: { editable: true, validation: { required: true } },
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
            { field: 'signatoryName', title: 'Full Name', editor: textEditor },
            {
                command: [
                    { name: "destroy", text: "Delete" }
                ],
                width: 150
            },
		],
		toolbar: [
                { name: "create", text: "Add New Signatory" },
            ],
		editable: "popup",
		selectable: true,
		edit: function (e) {
			var editWindow = this.editable.element.data("kendoWindow");
			editWindow.wrapper.css({ width: 500 });
			editWindow.title("Edit Photo Data");
		},
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		detailTemplate: 'Signature: <div class="grid"></div>',
		detailInit: signatoryGrid_detailInit,
		mobile: true
    });
}

function signatoryGrid_detailInit(e) {
    e.detailRow.find(".grid").kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
						if (typeof (e.data.signatures) === "undefined") {
							e.data.signatures = [];
						}
						entries.success(e.data.signatures);
					},
					create: function (entries) {
						entries.data = {
							imageId : imageId,
							image:currentPhotoContent,
							fileName: currentModel.fileName,
							mimeType: currentModel.mimeType
						}
						imageId --;
						entries.success(entries.data);
						e.data.signatures.push(entries.data);
					},
					update: function (entries) {
						entries.success(e.data.signatures);
					},
					destroy: function (entries) {
					    for (var i = 0; i < e.data.signatures.length; i++) {
					        if (e.data.signatures[i].imageId == entries.data.imageId) {
					            e.data.signatures.splice(i, 1);
					        }
					    }
					    for (var i = 0; i < currentPhotoContents.length; i++) {
					        if (currentPhotoContents[i].imageId == entries.data.imageId) {
					            currentPhotoContents.splice(i, 1);
					        }
					    }
						entries.success(e.data.signatures);	
					},
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'imageId',
                    fields: {
						imageId: { editable: false, type: 'number' },						
						image: {  validation: { required: true } },
						fileName: { editable: false, validation: { required: true } },
						mimeType: { validation: { required: true } },
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
			{ field: 'image', title: 'photo',editor: photoEditor, template: '<img src="#:mimeType#base64,#:image#" height="40"/>' },
            { field: 'fileName', title: 'File Name' },
            {
                command: [
                    { name: "destroy", text: "Delete" }
                ],
                width: 150
            },
		],
		toolbar: [
                { name: "create", text: "Add New Image" },
            ],
		editable: "popup",
		selectable: true,
		edit: function (e) {
			var editWindow = this.editable.element.data("kendoWindow");
			editWindow.title("Edit Image Data");
		},
		save: function (e) {
			$('.k-grid-update').css('display', 'none');
			
		},
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		mobile: true
    }).data("kendoGrid");
}
function photoEditor(container, options) {//editor
	$('<input type="file" id="photoFile" name="photoFile" />')
		.appendTo(container)
		.kendoUpload({
			localization: {
				select: "Select the File",					
			},
			async: {
				autoUpload: true,
				saveUrl: depositApiUrl + '/SavePhoto?token=' + authToken,
				removeUrl: depositApiUrl + '/removePhoto?token=' + authToken,
			}, 
			upload: onPhotoUpload
		});
	//currentModel = options.model;
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
	tooltipElement.appendTo(container);
}
function textEditor(container, options) {
    $('<input required  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoMaskedTextBox();
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function dateEditor(container, options) {
    $('<input required  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
		max: new Date()
	});
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function relationTypeEditor(container, options) {
    $('<input required  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: relationTypes,
		filter: "contains",
		dataValueField: "relationshipTypeId",
		dataTextField: "relationshipTypeName",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
    });
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function idTypeEditor(container, options) {
    $('<input required  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: idTypes,
		filter: "contains",
		dataValueField: "idTypeId",
		dataTextField: "idTypeName",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
    });
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function phoneNumberEditor(container, options) {
    $('<input required  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoMaskedTextBox({
		 mask: "(000) 000-0000"
	});
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function percentageEditor(container, options) {
    $('<input required  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoNumericTextBox({
		format: "0.#0 '%'",
		max: 100,
		min: 1
	});
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function onPhotoUpload(e) { //photo upload grabber
	if (!window.FileReader) {
	  return;
	}     
	$.each(e.files, function() {
	  var file = this.rawFile;
	  var reader = new FileReader();
	  reader.onload = (function() {
		var data = reader.result;
		currentPhotoContent = data.substr(data.indexOf('base64,')+7);
		currentModel.fileName = file.name;
		currentModel.mimeType = data.substr(0, data.indexOf ('base64'));
		currentModel.image = currentPhotoContent;


		var photoContent = {
			image: currentPhotoContent,
			fileName: currentModel.fileName,
			mimeType: currentModel.mimeType
		}
		currentPhotoContents.push(photoContent);

	  });       
	  reader.readAsDataURL(file);
	  e.preventDefault();	  
	});
}


function getRelationType(id) {
    for (var i = 0; i < relationTypes.length; i++) {
        if (relationTypes[i].relationshipTypeId == id)
            return relationTypes[i].relationshipTypeName;
    }
}
function getIdType(id) {
    for (var i = 0; i < idTypes.length; i++) {
        if (idTypes[i].idTypeId == id)
            return idTypes[i].idTypeName;
    }
}

function getPercentage(percentage) {
    return percentage+' %';    
}

var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            exist = true;
			getClientPicture(id);
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}
var onProductChange = function () {
    var id = $("#investmentProduct").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < depositTypes.length; i++) {
        if (depositTypes[i].depositTypeID == id) {
            exist = true;
			break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Product', 'ERROR');
        $("#investmentProduct").data("kendoComboBox").value("");
    }
}


function retrieveValues() {
    deposit.clientID = $('#client').data('kendoComboBox').value();
    deposit.depositTypeID = $('#investmentProduct').data('kendoComboBox').value();
    deposit.period = $('#depositPeriod').data('kendoComboBox').value();
    deposit.annualInterestRate = $('#annualInterestRate').data('kendoNumericTextBox').value();
    deposit.staffID = $('#relationshipOfficer').data('kendoComboBox').value();
	deposit.agentId = $('#fieldAgent').data('kendoComboBox').value();
	deposit.depositPeriodInDays = $('#depositPeriod').data('kendoComboBox').value();
	deposit.depositNextOfKins = $('#nextOfKinGrid').data().kendoGrid.dataSource.view();
	deposit.depositSignitoriesModel = $('#signatoriesGrid').data().kendoGrid.dataSource.view();

}

function savedeposit() {
    var validator = $("#myform").kendoValidator().data("kendoValidator");
    if (!validator.validate()) {
        warningDialog('One or More Fields are Empty','WARNING');
    } else {
        retrieveValues();
        saveToServer();
    }
}

function saveToServer() {
    displayLoadingDialog();
    $.ajax({
        url: depositApiUrl + "/PostNewDeposit",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(deposit),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		successDialog('Investment Account successfully created. Account No: '+data.depositNo ,
            'SUCCESS', function () { window.location = '/dash/home.aspx'; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}
function getClientPicture(id) {
    displayLoadingDialog();
    $.ajax({
        url: clientsApiUrl + "/GetClientImage/"+id,
        type: 'GET',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		displayClientImage(data);
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
		//warningDialog("Error while retrieving client Image", 'ERROR');
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}
function getClients(input) {
	var cln = $("#client").data("kendoComboBox");
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientsApiUrl + '/GetAllClientsBySearch',
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
			cln.setDataSource(clients);
			$("#details").show();
		}
		else{
			clients = {};
			$("#details").hide();
			successDialog('No client found that match the searched criteria', 'SUCCESS');
		}        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}
function displayClientImage(data){
	$("#clientImage").attr("src","data:image/png;base64,"+data);
}
