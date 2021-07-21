var authToken = coreERPAPI_Token;

$(function () {

        renderControls();
    
});



function renderControls() {
    var today = new Date();
    var tenYearsAgo = new Date();
    tenYearsAgo.setFullYear(tenYearsAgo.getFullYear() - 10);
    $("#startDate")
        .width('90%')
        .kendoDatePicker({
            value: tenYearsAgo,
            format: "dd-MMM-yyyy",
            parseFormat: ["dd-MM-yy", "dd-MM-yyyy", "dd-MMM-yy", "dd/MM/yy", "dd/MM/yyyy"]
        });

    $("#endDate")
        .width('90%')
        .kendoDatePicker({
            value: today,
            format: "dd-MMM-yyyy",
            parseFormat: ["dd-MM-yy", "dd-MM-yyyy", "dd-MMM-yy", "dd/MM/yy", "dd/MM/yyyy"]
        });

    dismissLoadingDialog();


    $("#getReport").click(function () {
        var start = $("#startDate").data("kendoDatePicker").value();
        var end = $("#endDate").data("kendoDatePicker").value();

        loadReport(start, end);
    });

}

function loadReport(start, end) {
    $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "coreData.Reports.Loans.DisbursedLoansReport, coreData",
                parameters: {
                    startDate: start,
                    endDate: end,
                    repaymentInterval: 0
                }
            },
            persistSession: false
        });
}




