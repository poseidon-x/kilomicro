var authToken = coreERPAPI_Token;
var repaymentInterval;

$(function () {
    renderControls();


    $("#daily").click(function (event) {
        if (document.getElementById("daily").checked) {
            repaymentInterval = 1;
        }
    });

    $("#weekly").click(function (event) {

        if (document.getElementById("weekly").checked) {
            repaymentInterval = 2;
        }
    });


    $("#getReport").click(function () {
        if (repaymentInterval > 0) {
            var start = $("#startDate").data("kendoDatePicker").value();
            var end = $("#endDate").data("kendoDatePicker").value();

            loadReport(start, end, repaymentInterval);
        } else {
            warningDialog("Select Repayment Type", 'ERROR');
        }
    });

});



function renderControls() {
    var today = new Date();
    var tenYearsAgo = new Date();

    tenYearsAgo.setFullYear(tenYearsAgo.getFullYear() - 10);
    $("#startDate")
        .width("100%")
        .kendoDatePicker({
            value: tenYearsAgo,
            format: "dd-MMM-yyyy",
            parseFormat: ["dd-MM-yy", "dd-MM-yyyy", "dd-MMM-yy", "dd/MM/yy", "dd/MM/yyyy"]
        });

    $("#endDate")
        .width("100%")
        .kendoDatePicker({
            value: today,
            format: "dd-MMM-yyyy",
            parseFormat: ["dd-MM-yy", "dd-MM-yyyy", "dd-MMM-yy", "dd/MM/yy", "dd/MM/yyyy"]
        });

    dismissLoadingDialog();



}

function loadReport(startDate, endDate) {
    $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "coreData.Reports.Loans.LoanDetailsReport, coreData",
                parameters: {
                    endDate: endDate,
                    startDate: startDate,
                    repaymentInterval: repaymentInterval
                },
            },
            persistSession: false
        });
}

