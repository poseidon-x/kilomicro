//***********************************************************//
//  	     BORROWING ACCOUNT REPORT - JAVASCRIPT                
// 		        CREATOR: EMMANUEL OWUSU(MAN)    	   
//		       WEEK: JULY(17TH - 21ST), 2015  		  
//*********************************************************//

"use strict";

var authToken = coreERPAPI_Token;
//var borrowingApiUrl = coreERPAPI_URL_Root + "/crud/Borrowing";
var borrowingClientApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingClients";

var clients = {};

$(function () {
	
	$.ajax(
        {
            url: borrowingClientApiUrl + '/GetDisbursedBorrowingClient',
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            clients = data;
			renderControls();

            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
	
	$("#getReport").click(function () {
		var clientId = $("#client").data("kendoComboBox").value();
		
        if (clientId > 0) {
            loadReport(clientId);
        } else {
            warningDialog("Select Client", 'ERROR');
        }
    });
	
});



function renderControls() {

    $("#client").width("100%")
		.kendoComboBox({
		    dataSource: clients,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientID",
		    dataTextField: "clientName",
			change: onClientChange,
			optionLabel: ""
	});

    dismissLoadingDialog();
}

var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
			exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    } 
}

function loadReport(clientId) {
    $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "coreData.Reports.Borrowing.ClientBorrowingsSummary, coreData",
                parameters: {
                    clientId: clientId
                },
            },
            persistSession: false
        });
}

