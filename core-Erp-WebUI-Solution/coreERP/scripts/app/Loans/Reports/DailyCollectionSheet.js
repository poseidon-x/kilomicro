//***********************************************************//
//  	     LOAN DOCUMENT TEMPLATE - JAVASCRIPT                
// 		        CREATOR: EMMANUEL OWUSU(MAN)    	   
//		       WEEK: JULY(27TH - 31TH), 2015  		  
//*********************************************************//

"use strict";

var authToken = coreERPAPI_Token;
var branchApiUrl = coreERPAPI_URL_Root + "/crud/branch";

var branches = {};

$(function () {
displayLoadingDialog();
    loadForm();   
});

function loadForm() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: branchApiUrl + '/Get',
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
		branches = data;
        dismissLoadingDialog();
		prepareUi();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
}

function prepareUi() {
	renderControls();

	$("#getReport").click(function () {
        var branchId = $("#branch").data("kendoComboBox").value();
        var collectionDate = $("#collectionDate").data("kendoDatePicker").value();

        loadReport(branchId, collectionDate);
    });
}

function renderControls() {
    $("#branch").width('90%')
        .kendoComboBox({
			dataSource: branches,
			filter: "contains",
			suggest: true,
			dataValueField: "branchID",
			dataTextField: "branchName",
			filter: "contains",
			highlightFirst: true,
			suggest: true,
			ignoreCase: true,
			//change: onChargeTypeChange,
			optionLabel: ""
	});

    $("#collectionDate").width('90%')
        .kendoDatePicker({
		format: '{0:dd-MMM-yyyy}',
		parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"]
	});
	
}

function loadReport(branId, collDate) {
    $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "coreData.Reports.Loans.DailyCollectionSheet, coreData",
                parameters: {
                    branchId: branId,
                    collectionDate: collDate
                }
            },
            persistSession: false
        });
}




