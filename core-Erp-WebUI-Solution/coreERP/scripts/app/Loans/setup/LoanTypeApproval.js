"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanTypeApiUrl = coreERPAPI_URL_Root + "/crud/LoanTypeApproval";
var ProfileTypeApiUrl = coreERPAPI_URL_Root + "/crud/ProfType";


//Declaration of variables to store records retrieved from the database
var loanTypes = {};
var loanType = {};
var roles = {};
var users = {};
var accessLevels = {}; 
var LoanTypeApprovals = {};
var approvalLevels = [
    {value:1,text:"First"},
    {value:2,text:"Second"},
    {value:3,text:"Third"},
    {value:4,text:"Fourth"},
    {value:5,text:"Fifth"}
];
var profileTypes = [
    {value:"R",text:"Role"},
    {value:"U",text:"User"},
    {value:"A",text:"Access Level"}
];

var loanTypeAjax = $.ajax({
    url: LoanTypeApiUrl + "/Get",
	type: 'Get',
	contentType: "application/json",
	beforeSend: function (req) {
		req.setRequestHeader('Authorization', "coreBearer " + authToken);
	}
});
var accessLevelAjax = $.ajax({
    url: ProfileTypeApiUrl + '/GetAccessLevels',
    type: 'Get',
    contentType: "application/json",
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var rolesAjax = $.ajax({
    url: ProfileTypeApiUrl + '/GetRoles',
    type: 'Get',
    contentType: "application/json",
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var usersAjax = $.ajax({
    url: ProfileTypeApiUrl + '/GetUsers',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var loanTypeApprovalAjax = $.ajax({
    url: LoanTypeApiUrl + '/GetApproval/' + loanTypeID,
    type: 'Post',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {
    $.when(loanTypeAjax, accessLevelAjax, rolesAjax, usersAjax, loanTypeApprovalAjax)
        .done(function (dataLoanType, dataAccessLevel, dataRole, dataUser, dataLoanTypeApproval) {
            loanTypes = dataLoanType[2].responseJSON;
            accessLevels = dataAccessLevel[2].responseJSON;
            roles = dataRole[2].responseJSON;
            users = dataUser[2].responseJSON;
            loanType = dataLoanTypeApproval[2].responseJSON;

            //Prepares UI
            prepareUi();
			renderGrid();
            dismissLoadingDialog();
        });
}

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

function prepareUi(){
    $('#loanType').width('100%').kendoComboBox({
            dataSource: loanTypes,
            dataValueField: 'loanTypeID',
            dataTextField: 'loanTypeName',
            filter: "contains",
            highlightFirst: true,
            suggest: true,
            ignoreCase: true,
            change: onLoanTypeChange,
            animation: {
                close: { effects: "fadeOut zoom:out", duration: 300 },
                open: { effects: "fadeIn zoom:in", duration: 300 }
            },
            optionLabel: ''
        });
}

var onLoanTypeChange = function(){
    var  id = $('#loanType').data('kendoComboBox').value();
    var exist = false;

    //Retrieve value enter validate
    for (var i = 0; i < loanTypes.length; i++) {
        if (loanTypes[i].loanTypeID == id) {
            exist = true;
            getLoanTypeApproval(id);
            break;
        }
    }
    
    if (!exist) {
        warningDialog('Invalid Product', 'ERROR');
        $("#loanType").data("kendoComboBox").value("");
    }

}

//render Grid
function renderGrid() {
	$('#tabs').kendoTabStrip();
    $('#approvalTypes').kendoGrid({
            dataSource: {
                transport: {
					read: function (entries) {
                        entries.success(loanType.loanApprovalStages);                    
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
                        id: "loanApprovalStageId",
                        fields: {
                            loanApprovalStageId: { editable: false, type: "number" },
							loanTypeId: {editable: false },
							name: {editable: true },                            
							isMandatory: { type: "boolean",editable: true },
							ordinal: { editable: true }                        }
                    }
                },
            },		
			columns: [
				{ field: 'name', title: 'Stage Name', editor: nameEditor },
				{ field: 'ordinal', title: 'Level',editor: ordinaryEditor, template: '#= getOrinal(ordinal) #' },
				{ field: 'isMandatory', title: 'Mandatory',template: '<input type="checkbox" #= isMandatory ? \'checked="isMandatory"\' : "" # disabled="disabled" />' },								
				{ command: ['edit','delete'], width: "180px" }
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5
			},
			toolbar: [
				{
					name: "create",
					className: 'addApprovalStage',
					text: 'Add New Stage',
				},
				{
					name: "save",
					className: 'saveChanges',
					text: 'Save Stage(s)',
				}
			],
			editable: "popup",
			//detailTemplate: 'Stage Officers: <div class="grid"></div>',
			//detailInit: grid_detailInit
			detailInit: lineInit,
			dataBound: function ()
			{
				this.expandRow(this.tbody.find("tr.k-master-row").first());
			}
        });
		$(".saveChanges").click(function () {
			saveApprovalStages();
		});
}

//function grid_detailInit(e) {
//    e.detailRow.find(".grid").kendoGrid({
function lineInit(e) {
        $("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
						if (typeof (e.data.loanApprovalStageOfficers) === "undefined") {
							e.data.loanApprovalStageOfficers = [];
						}
						entries.success(e.data.loanApprovalStageOfficers);
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
                pageSize: 10,
                schema: {
                    model: {
                        id: "loanApprovalStageOfficerId",
                        fields: {
                            loanApprovalStageOfficerId: { editable: false, type: "number" },
                            loanApprovalStageId: { editable: false, type: "number"  },
							profileType: { editable: true, type:"string"},
							profileValue: { editable: true, type:"string"},
						}
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
				{ field: 'profileType', title: 'Profile', editor: profileTypeEditor, template: '#= getProfileType(profileType) #' },
				{ field: 'profileValue', title: 'Value', editor: profileValueEditor},
				{ command: ['edit','delete'], width: "180px" }
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
					className: 'addApprovalStageOfficer',
					text: 'Add New Officer',
				}
			],
		editable: "popup",
		mobile: true
    }).data("kendoGrid");
}

var gridDeleteButton = {
    name: "destroy",
    text: "Delete"
};

function getClient(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id)
            return clients[i].clientNameWithAccountNO;
    }
}
function getDay(id) {
    for (var i = 0; i < days.length; i++) {
        if (days[i].loanGroupDayId == id) 
            return days[i].dayName;
    }
}

function getLoanTypeApproval(id){
    displayLoadingDialog();
    $.ajax({
        url: LoanTypeApiUrl + "/GetApproval/" + id,
        type: 'POST',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        loanType = data;
        dismissLoadingDialog();
        $('#approvalTypes').html("");
        renderGrid();
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

function ordinaryEditor(container, options) {
    $('<input id="ordinal" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: approvalLevels,
        dataValueField: "value",
        dataTextField: "text",
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
}
function nameEditor(container, options) {
    $('<input id="ordinal" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoMaskedTextBox();
}

function profileTypeEditor(container, options) {
    $('<input id="profileType" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("100%")
    .kendoComboBox({
        dataSource: profileTypes,
        dataValueField: "value",
        dataTextField: "text",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        ignoreCase: true,
		change: onProfileTypeChange,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function profileValueEditor(container, options) {	
	$('<input  id="profileValue" data-bind="value:' + options.field + '"/>')
	.appendTo(container)
	.width("100%")
	.kendoComboBox({
		dataSource: [],
		dataValueField: "value",
		dataTextField: "value",
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
}
function getOrinal(val) {
    for (var i = 0; i < approvalLevels.length; i++) {
        if (approvalLevels[i].value == val)
            return approvalLevels[i].text;
    }
}
function getProfileType(val) {
    for (var i = 0; i < profileTypes.length; i++) {
        if (profileTypes[i].value == val)
            return profileTypes[i].text;
    }
}
var onProfileTypeChange = function(){
	var id = $("#profileType").data("kendoComboBox").value();
	var profValue = $("#profileValue").data("kendoComboBox");
	
	if(id == "R"){
		profValue.setDataSource(roles);
	}else if(id == "A"){
		profValue.setDataSource(accessLevels);
	}else if(id == "U"){
		profValue.setDataSource(users);
	}
}

function saveApprovalStages() {
    loanType.loanApprovalStages = $("#approvalTypes").data().kendoGrid.dataSource.view();
    if (loanType.loanApprovalStages.length > 0) {
        displayLoadingDialog();
        saveToServer();
    } else {
        smallerWarningDialog('Please add Approvals', 'NOTE');
    }
}

function saveToServer() {
    $.ajax({
        url: LoanTypeApiUrl + "/Post",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(loanType),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        successDialog('Approval Stage(s) Saved successfully',
		'SUCCESS', function () { window.location = "/dash/home.aspx"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}

