/*
Creator: damien@acsghana.com
*/

var authToken = agencyAPI_Token;
var clientApiUrl = agencyAPI_URL_Root + "/AllClients";

var clients = {};
var reportModel = {};

$(function () {
    displayLoadingDialog();
    $.ajax(
    {   //Fetch data/record(s) from customer and assign to customer variable      
        url: clientApiUrl + '/GetSavingsClient',
		type: 'Get',
		beforeSend: function (req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}
    }).done(function (data) {
        clients = data;
        renderControls();
		dismissLoadingDialog();
    }).error(function (error) {
		dismissLoadingDialog();
        warningDialog(JSON.stringify(error),'ERROR');
    });//customer ajax call
});


function renderControls() {
    $("#saving").width("90%")
	.kendoComboBox({
		dataSource: clients,
		filter: "contains",
		suggest: true,
		dataValueField: "savingId",
		dataTextField: "clientNameWithAccountNO",
		filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		change: onClientChange,
		optionLabel: ""
	});
	$("#startDate").width("90%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
		change: onDateChanged
	});
	$("#endDate").width("90%").kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
		change: onDateChanged		
	});
}
var onClientChange = function(){
	$("#startDate").data("kendoDatePicker").value("");
	$("#endDate").data("kendoDatePicker").value("");
	var savId = $("#saving").data("kendoComboBox").value();
	var exist = false;	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].savingId == savId) {            
			onDateChanged;
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Account', 'ERROR');
        $("#saving").data("kendoComboBox").value("");
    }
}
var onDateChanged = function () {
    //displayLoadingDialog();
	var savingsId = $("#saving").data("kendoComboBox").value();
    var startDate = $("#startDate").data("kendoDatePicker").value();
    var endDate = $("#endDate").data("kendoDatePicker").value();
	
	if(startDate != null && startDate != "undefined"){
		$("#endDate").data("kendoDatePicker").min(endDate);
	}
	
	if(savingsId != null && savingsId != "undefined" && startDate != null && startDate != "undefined"
	&& endDate != null && endDate != "undefined"){
		$("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
		loadReport(savingsId,startDate,endDate);
	}
}

function loadReport(savId,start,end) {
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: agencyAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "agencyData.Report.Savings.ClientStatement, agencyData",
                parameters: {
					savingId: savId,
                    startDate: start,
					endDate: end
                },
            },
            persistSession: false
        });
}

