//***********************************************************//
//  	     LOAN DOCUMENT TEMPLATE - JAVASCRIPT
// 		        CREATOR: EMMANUEL OWUSU(MAN)
//		       WEEK: JULY(27TH - 31TH), 2015
//*********************************************************//

"use strict";

var authToken = coreERPAPI_Token;

$(function () {
displayLoadingDialog();
    prepareUi();
});

function prepareUi() {
	renderControls();
    dismissLoadingDialog();

	$("#getReport").click(function () {
        var selectedMonth = $("#month").data("kendoDatePicker").value();

        loadReport(selectedMonth);
    });
}

function renderControls() {
    $("#month").width('90%').kendoDatePicker({
        start: "year",
        depth: "year",
		format: '{0:MMM-yyyy}',
		parseFormats: ["yyyy-MM", "yy-MM", "yyyy-MMM"]
	});
}

function loadReport(month) {
    $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-FA-9.2.15.1105.html',
            reportSource: {
                report: "coreData.Reports.BOG.DepositFromPublic, coreData",
                parameters: {
                    monthEndDate: month
                }
            },
            persistSession: false
        });
}
