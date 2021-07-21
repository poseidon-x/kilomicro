//***********************************************************//
//  	     LOAN DOCUMENT TEMPLATE - JAVASCRIPT                
// 		        CREATOR: EMMANUEL OWUSU(MAN)    	   
//		       WEEK: JULY(27TH - 31TH), 2015  		  
//*********************************************************//

"use strict";

var authToken = coreERPAPI_Token;
var documentTemplateApiUrl = coreERPAPI_URL_Root + "/crud/loanDocumentTemplate";
var loanDocumentPlaceHolderTypeApiUrl = coreERPAPI_URL_Root + "/crud/loanDocumentPlaceHolderType";
var loanClientsApiUrl = coreERPAPI_URL_Root + "/crud/loanClient";
var clientLoansApiUrl = coreERPAPI_URL_Root + "/crud/clientLoan";

var documentTemplate = {};
var loanClients = {};
var clientLoans = {};


var documentTemplateAjax = $.ajax({
    url: documentTemplateApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var loanClientsAjax = $.ajax({
    url: loanClientsApiUrl + '/GetAllLoanClients',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


$(function () {
displayLoadingDialog();
    loadForm();   
});

function loadForm() {
    $.when(documentTemplateAjax, loanClientsAjax)
        .done(function (dataDocumentTemplate, dataLoanClients) {
            documentTemplate = dataDocumentTemplate[2].responseJSON;
            loanClients = dataLoanClients[2].responseJSON;

            //Prepares UI
            prepareUi();
        }
	);
}

function prepareUi() {
	renderControls();
    dismissLoadingDialog();

	$("#getReport").click(function () {
        var loanId = $("#loan").data("kendoComboBox").value();
        var documentId = $("#document").data("kendoComboBox").value();

        loadReport(loanId, documentId);
    });
}

function renderControls() {
    $("#document")
		.width('90%')
        .kendoComboBox({
			dataSource: documentTemplate,
			dataValueField: "loanDocumentTemplateId",
			dataTextField: "templateName",
			change: onDocumentChange,		
			optionLabel: ""
	});

    $("#client")
        .width('90%')
        .kendoComboBox({
			dataSource: loanClients,
			filter: "contains",
		    suggest: true,
			dataValueField: "clientID",
			dataTextField: "clientName",
			change: onClientChange,		
			optionLabel: ""
	});
	
	$("#loan")
        .width('90%')
        .kendoComboBox({
			dataSource: clientLoans,
			dataValueField: "loanID",
			dataTextField: "loanNo",
			change: onLoanChange,		
			optionLabel: ""
	});
}

var onDocumentChange = function() {

    var id = $("#document").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < documentTemplate.length; i++) {
        if (documentTemplate[i].loanDocumentTemplateId == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Document', 'ERROR');
        $("#document").data("kendoComboBox").value("");
    }
}

var onClientChange = function() {

    var id = $("#client").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < loanClients.length; i++) {
        if (loanClients[i].clientID == id) {
            exist = true;
			displayLoadingDialog();
			
			$.ajax({
				url: clientLoansApiUrl + '/GetClientLoans/' + id,
				type: 'Get',
				beforeSend: function(req) {
					req.setRequestHeader('Authorization', "coreBearer " + authToken);
				}
			}).done(function (data) {
				clientLoans = data;
				dismissLoadingDialog();
				
				//Set the returned data to loan datasource
				//$("#loan").data("kendoComboBox").value("");
				$("#loan").data("kendoComboBox").setDataSource(clientLoans);
				
			}).error(function (xhr, data, error) {
				dismissLoadingDialog();
				warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
			});
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    }
}

var onLoanChange = function() {
    var id = $("#loan").data("kendoComboBox").value();
    var exist = false;

    for (var i = 0; i < clientLoans.length; i++) {
        if (clientLoans[i].loanID == id) {
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Loan', 'ERROR');
        $("#loan").data("kendoComboBox").value("");
    }
}

function loadReport(loanId, documentId) {
    $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "coreData.Reports.Loans.LoanDocument, coreData",
                parameters: {
                    loanId: loanId,
                    documentId: documentId,
                    repaymentInterval: 0
                }
            },
            persistSession: false
        });
}




