//"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var privateCompanyStaffApiUrl = coreERPAPI_URL_Root + "/crud/privateCompanyStaff";
var employerApiUrl = coreERPAPI_URL_Root + "/crud/employer";
var loanClientsApiUrl = coreERPAPI_URL_Root + "/crud/allClients";
var employeeContractTypeApiUrl = coreERPAPI_URL_Root + "/crud/employeeContractType";
var addressTypeApiUrl = coreERPAPI_URL_Root + "/crud/addressType";
var cityApiUrl = coreERPAPI_URL_Root + "/crud/city";
var employeeDepartmentApiUrl = coreERPAPI_URL_Root + "/crud/employeeDepartment";




//Declaration of variables to store records retrieved from the database
var privateCompanyStaff = {};
var employers = {};
var clients = {};
var employeeContractTypes = {};
var addressTypes = {};
var cities = {};
var employeeDepartments = {};

//serve as  datasource for client Leader combobox

var privateCompanyStaffAjax = $.ajax({
    url: privateCompanyStaffApiUrl + '/Get/' + id,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var employerAjax = $.ajax({
    url: employerApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var clientAjax = $.ajax({
    url: loanClientsApiUrl + '/GetLoanTypeClients', 
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var employeeContractTypeAjax = $.ajax({
    url: employeeContractTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var addressTypeAjax = $.ajax({
    url: addressTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var cityAjax = $.ajax({
    url: cityApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var employeeDepartmentAjax = $.ajax({
    url: employeeDepartmentApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {
	$.when(privateCompanyStaffAjax, employerAjax, clientAjax, employeeContractTypeAjax,
	addressTypeAjax,cityAjax,employeeDepartmentAjax)
        .done(function (dataPrivateCompanyStaff, dataEmployer, dataClient, dataEmployeeContractType,
		dataAddressType,dataCity,dataEmployeeDepartment) {
			privateCompanyStaff = dataPrivateCompanyStaff[2].responseJSON;                        
			employers = dataEmployer[2].responseJSON;                        
			clients = dataClient[2].responseJSON;
			employeeContractTypes = dataEmployeeContractType[2].responseJSON;
			addressTypes = dataAddressType[2].responseJSON;
			cities = dataCity[2].responseJSON;
			employeeDepartments = dataEmployeeDepartment[2].responseJSON;
			
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
	$("#client").width("90%").kendoComboBox({
		dataSource: clients,
		dataValueField: 'clientID',
		dataTextField: 'clientNameWithAccountNO',
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
	$("#employer").width("90%").kendoComboBox({
		dataSource: employers,
		dataValueField: 'employerID',
		dataTextField: 'employerName',
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
	$("#employeeNumber").width("90%").kendoMaskedTextBox();
	$("#employeeContractType").width("90%").kendoComboBox({
	    dataSource: employeeContractTypes,
		dataValueField: 'employeeContractTypeID',
		dataTextField: 'employeeContractTypeName',
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
	$("#employmentStartDate").width("90%").kendoDatePicker({
	    format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 300 },
			open: { effects: "fadeIn zoom:in", duration: 300 }
		},
		max: new Date()
	});
	$("#socialSecurityNumber").width("90%").kendoMaskedTextBox();
	$("#position").width("90%").kendoMaskedTextBox();
		
	$('#tabs').kendoTabStrip();
	/*
	if(loanGroup.loanGroupId > 0)
	{
		$('#groupName').data('kendoMaskedTextBox').value(loanGroup.loanGroupName);
		$('#groupDay').data('kendoComboBox').value(loanGroup.loanGroupDayId);
		$('#relationsOfficer').data('kendoComboBox').value(loanGroup.relationsOfficerStaffId);
		$('#groupLeaderClient').data('kendoComboBox').value(loanGroup.leaderClientId);
	}
	*/	
	renderGrid();
	
	$('#save').click(function (event) {
	var validator = $("#myform").kendoValidator().data("kendoValidator");
		
    if (validator.validate()) {
		if (confirm('Are you sure you want save Staff?')) {
			saveSatff();				
		} else {
		   smallerWarningDialog('Please review and save later', 'NOTE');
		}	
	}else{smallerWarningDialog('One or more required fields is/are empty', 'WARNING');}		
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
    $('#addressGrid').kendoGrid({
		dataSource: {
			transport: {
				read: function (entries) {
					entries.success(privateCompanyStaff.privateCompanyStaffAddresses);					
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
				parameterMap: function (data) { return JSON.stringify(data); }
			},
			schema: {
				model: {
					id: "privateCompanyStaffAddressId",
					fields: {
						privateCompanyStaffAddressId: { editable: false, type: "number" },
						privateCompanyStaffId: { editable: false, type: "number"  },
						addressTypeId: {editable: true, validation: {  required: true} },
						cityId: { editable: true },
						addressLine: {editable: true, validation: {  required: true} }
					}
				}
			},
		},
		columns: [
			{ field: 'addressTypeId', title: 'Address Type', editor: addressTypeEditor, template: '#= getAddressType(addressTypeId) #' },
			{ field: 'cityId', title: 'City', editor: cityEditor, template: '#= getCity(cityId) #' },
			{ field: 'addressLine', title: 'Address', editor: addressEditor, template: '#= getAddress(addressLine) #' },
			{ command: [ "edit", 'destroy'], width: "180px"   }
		],
		pageable: {
			pageSize: 30,
			pageSizes: [30, 50, 100, 500, 1000],
			previousNext: true,
			buttonCount: 5
		},
		sortable: {
			mode: "multiple",
			allowUnsort: true
		},
		edit: function (e) {
			var editWindow = this.editable.element.data("kendoWindow");
			editWindow.wrapper.css({ width: 500 });
			editWindow.title("Edit Data");
		},
		toolbar: [
			{ name: "create", text: "Add Address" },
		],
		editable: "popup",
	});
	
    $('#verificationGrid').kendoGrid({
		dataSource: {
			transport: {
				read: function (entries) {
					entries.success(privateCompanyStaff.privateCompanyStaffVerifications);					
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
				parameterMap: function (data) { return JSON.stringify(data); }
			},
			schema: {
				model: {
					id: "privateCompanyStaffVerificationId",
					fields: {
						privateCompanyStaffVerificationId: { editable: false, type: "number" },
						privateCompanyStaffId: { editable: false, type: "number"  },
						contactPersonName: {editable: true, validation: {  required: true} },
						contactPersonPosition: { editable: true,  },
						departmentId: {editable: true, validation: {  required: true} },
						email: {editable: true, validation: {  required: true} },
						phone: {editable: true, validation: {  required: true} }						
					}
				}
			},
		},
		columns: [
			{ field: 'contactPersonName', title: 'Contact Person', editor: textEditor },
			{ field: 'contactPersonPosition', title: 'Position', editor: textEditor },
			{ field: 'departmentId', title: 'Department', editor: departmentEditor, template: '#= getDepartment(departmentId) #' },
			{ field: 'email', title: 'Email', editor: textEditor },
			{ field: 'phone', title: 'Phone', editor: phoneNumberEditor },
			{ command: [ "edit", 'destroy'], width: "180px"   }
		],
		pageable: {
			pageSize: 30,
			pageSizes: [30, 50, 100, 500, 1000],
			previousNext: true,
			buttonCount: 5
		},
		sortable: {
			mode: "multiple",
			allowUnsort: true
		},
		edit: function (e) {
			var editWindow = this.editable.element.data("kendoWindow");
			editWindow.wrapper.css({ width: 500 });
			editWindow.title("Edit Data");
		},
		toolbar: [
			{ name: "create", text: "Add Address" },
		],
		editable: "popup",
	});
}


function addressTypeEditor(container, options) {
    $('<input required  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: addressTypes,
		filter: "contains",
		dataValueField: "addressTypeID",
		dataTextField: "addressTypeName",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
    });
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function cityEditor(container, options) {
    $('<input required  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: cities,
		filter: "contains",
		dataValueField: "city_id",
		dataTextField: "city_name",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
    });
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}
function departmentEditor(container, options) {
    $('<input required  data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: employeeDepartments,
		filter: "contains",
		dataValueField: "employeeDepartmentId",
		dataTextField: "dapartmentName",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		optionLabel: ""
    });
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
function addressEditor(container, options) {
	$('<textarea class="form-control" id="addressLine" data-bind="value: ' + options.field + '" cols="3" rows="4"></textarea>')
        .appendTo(container)
		.width("100%");
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

function getAddressType(id) {
    for (var i = 0; i < addressTypes.length; i++) {
        if (addressTypes[i].addressTypeID == id)
            return addressTypes[i].addressTypeName;
    }
}
function getCity(id) {
    for (var i = 0; i < cities.length; i++) {
        if (cities[i].city_id == id)
            return cities[i].city_name;
    }
}
function getAddress(content) {
    var commentToShowOnGrid = content.substr(0,7);
	return commentToShowOnGrid+"...";
}
function getDepartment(id) {
    for (var i = 0; i < employeeDepartments.length; i++) {
        if (employeeDepartments[i].employeeDepartmentId == id)
            return employeeDepartments[i].dapartmentName;
    }
}

//retrieve values from from Input Fields and save 
function saveSatff() {
    retrieveValues();
    saveToServer();
}
function retrieveValues() {
    privateCompanyStaff.employerId = $('#employer').data('kendoComboBox').value();
    privateCompanyStaff.employeeNumber = $('#employeeNumber').data('kendoMaskedTextBox').value();
    privateCompanyStaff.clientId = $('#client').data('kendoComboBox').value();
    privateCompanyStaff.employeeContractTypeId = $('#employeeContractType').data('kendoComboBox').value();
	privateCompanyStaff.employmentStartDate = $('#employmentStartDate').data('kendoDatePicker').value();
    privateCompanyStaff.socialSecurityNumber = $('#socialSecurityNumber').data('kendoMaskedTextBox').value();
    privateCompanyStaff.position = $('#position').data('kendoMaskedTextBox').value();
	privateCompanyStaff.privateCompanyStaffAddresses = $("#addressGrid").data().kendoGrid.dataSource.view();
	privateCompanyStaff.privateCompanyStaffVerifications = $("#verificationGrid").data().kendoGrid.dataSource.view();
}
//Save to server function
function saveToServer() {
	displayLoadingDialog();
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: privateCompanyStaffApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(privateCompanyStaff),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Staff Saved Successfully',
            'SUCCESS', function () { window.location = '/dash/home.aspx'; });
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

function gridDeleteFunction(e){
	var clnId = e.model.clientId;
	addClientBack(clnId); 
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
function addClientBack(id) {
	var clientLeader = $("#groupLeaderClient").data("kendoComboBox");
    for (var i = 0; i < clientsWithoutGroup.length; i++) {
        if (clientsWithoutGroup[i].clientID == id) {
            remainingClients.push(clientsWithoutGroup[i]);
            allGroupClients.splice(i,1);
            clientLeader.setDataSource(allGroupClients);
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
