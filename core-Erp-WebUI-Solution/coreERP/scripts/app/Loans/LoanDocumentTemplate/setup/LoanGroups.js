"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var LoanClientsApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var LoanGroupDayApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupDay";
var staffApiUrl = coreERPAPI_URL_Root + "/crud/Staff";


//Declaration of variables to store records retrieved from the database
var clients = {};
var days = {};
var staff = {};


var clientAjax = $.ajax({
    url: LoanClientsApiUrl + '/GetAllGroupClients',   //GetGroupClients',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var daysAjax = $.ajax({
    url: LoanGroupDayApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var staffAjax = $.ajax({
    url: staffApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {
    $.when(clientAjax, daysAjax, staffAjax)
        .done(function (dataClient, dataDay, dataStaff) {
			clients = dataClient[2].responseJSON;
			days = dataDay[2].responseJSON;
			staff = dataStaff[2].responseJSON;
			
			renderGrid();
			dismissLoadingDialog();
        });
}

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//render Grid
function renderGrid() {
	$('#tabs').kendoTabStrip();
    $('#groupGrid').kendoGrid({
            dataSource: {
                transport: {
					read: {
						url: LoanGroupApiUrl + '/Get',
						type: 'Get',
						contentType: "application/json",						
						beforeSend: function (req) {
							req.setRequestHeader('Authorization', "coreBearer " + authToken);
						}
					},
					destroy: {
					    url: LoanGroupApiUrl + "/Delete",
						type: 'DELETE',
						contentType: "application/json",
						beforeSend: function (req) {
							req.setRequestHeader('Authorization', "coreBearer " + authToken);
						} 
					},
					parameterMap: function (data) { return JSON.stringify(data); }
				},
                schema: {
                    model: {
                        id: "loanGroupId",
                        fields: {
                            loanGroupId: { editable: false, type: "number" },
							loanGroupName: {editable: false },
							loanGroupNumber: {editable: false },                            
							loanGroupDayId: { editable: false },
							leaderClientId: { editable: false },
							relationsOfficerStaffId: { editable: false }
                        }
                    }
                },
            },		
			columns: [
				{ field: 'loanGroupName', title: 'Group Name' },
				{ field: 'loanGroupNumber', title: 'Group Number' },				
				{ field: 'loanGroupDayId', title: 'Group Day', template: '#= getDay(loanGroupDayId) #' },
				{ field: 'leaderClientId', title: 'Group Leader', template: '#= getClient(leaderClientId) #' },
				{ field: 'relationsOfficerStaffId', title: 'Relations Officer', template: '#= getStaff(relationsOfficerStaffId) #' },
				{ command: [gridEditButton], width: "90px" },
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5
			},
			toolbar: [
				{
					className: 'addGroup',
					text: 'Add New Group',
				}
			],
			detailTemplate: 'Group Clients: <div class="grid"></div>',
			detailInit: grid_detailInit
        });
		
		$(".addGroup").click(function () {
			window.location = "/LoanSetup/CreateLoanGroup";
		});
}

function grid_detailInit(e) {
    e.detailRow.find(".grid").kendoGrid({
        dataSource: {
                transport: {
                read: function (entries) {
					if (typeof (e.data.loanGroupClients) === "undefined") {
						e.data.loanGroupClients = [];
					}
					entries.success(e.data.loanGroupClients);
				},
                parameterMap: function (data) { return JSON.stringify(data); }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "loanGroupClientId",
                        fields: {
                            loanGroupClientId: { editable: false, type: "number" },
                            loanGroupId: { editable: false, type: "number"  },
							clientId: { editable: false, type:"string"},
					}
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
				{ field: 'clientId', title: 'Client', template: '#= getClient(clientId) #' },
		],
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		mobile: true
    }).data("kendoGrid");
}

var gridEditButton = {
    name: "edit",
    text: "Edit",
    click: function(e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/LoanSetup/CreateLoanGroup/" + data.loanGroupId.toString();
    },
};

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
function getStaff(id) {
    for (var i = 0; i < staff.length; i++) {
        if (staff[i].staffId == id)
            return staff[i].staffName;
    }
}


