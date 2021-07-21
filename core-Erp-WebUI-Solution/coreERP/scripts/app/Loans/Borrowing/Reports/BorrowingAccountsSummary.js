//***********************************************************//
//  	     BORROWING ACCOUNTS SUMMARY REPORT - JAVASCRIPT                
// 		        CREATOR: EMMANUEL OWUSU(MAN)    	   
//		       WEEK: JULY(17TH - 21ST), 2015  		  
//*********************************************************//

"use strict";

var authToken = coreERPAPI_Token;
var borrowingClientApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingClients";


$(function () {	
	$("#getReport").click(function () {
		loadReport();
    });	
});

function loadReport() {
    $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "coreData.Reports.Borrowing.BorrowingOutstandingReport, coreData",
                parameters: {
                },
            },
            persistSession: false
        });
}

