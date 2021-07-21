var clientApiUrl = coreERPAPI_URL_Root + "/crud/allClients";
var depositApiUrl = coreERPAPI_URL_Root + "/crud/deposit";

var clients = {};
var deposits = {};

$(function () {
    displayLoadingDialog();
    loadData();
});

function loadData() {
	displayLoadingDialog();
    $.ajax({
        url: clientApiUrl + "/GetDepositClient",
        type: 'GET',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		clients = data;
		renderControls();
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}


function renderControls() {
	$("#client").width("90%")
	.kendoComboBox({
		dataSource: clients,
		filter: "contains",
		suggest: true,
		dataValueField: "clientID",
		dataTextField: "clientName",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onClientChange,
		optionLabel: ""
	});
    $("#deposit").width("90%")
	.kendoComboBox({
		dataSource: deposits,
		filter: "contains",
		suggest: true,
		dataValueField: "depositID",
		dataTextField: "depositNo",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onDepositChange,
		optionLabel: ""
	});
}
var onClientChange = function(){
	$("#deposit").data("kendoComboBox").value("");
	var clientId = $("#client").data("kendoComboBox").value();
	var exist = false;	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == clientId) {            
			GetClientDeposits(clientId);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
		$("#deposit").data("kendoComboBox").setDataSource([]);
    }
}
var onDepositChange = function(){
	var depId = $("#deposit").data("kendoComboBox").value();
	var exist = false;	
	//Retrieve value enter validate
    for (var i = 0; i < deposits.length; i++) {
        if (deposits[i].depositID == depId) {            
			GetReport(depId);
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Account', 'ERROR');
        $("#deposit").data("kendoComboBox").value("");
    }
}
function GetClientDeposits(id) {
	displayLoadingDialog();
	var dep = $("#deposit").data("kendoComboBox");
	$.ajax({
        url: depositApiUrl + "/GetClientDeposits/"+id,
        type: 'GET',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		deposits = data;
		dep.setDataSource(deposits);
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}
function GetReport(id) {
    displayLoadingDialog();
	
	if(id != null && id != "undefined"){
		$("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
		loadReport(id);
		dismissLoadingDialog();
	}
}

function loadReport(depId) {
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "coreData.Reports.Deposit.DepositCertificate, coreData",
                parameters: {
					depositId: depId
                },
            },
            //persistSession: false
        });
}

