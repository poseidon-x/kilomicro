$(function () {
    loadReport();
});

function loadReport() {
    $('#report').telerik_ReportViewer({
        serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
        templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
        reportSource: {
            report: "coreData.Reports.Inventory.StockMaster, coreData",
            parameters: {

            },
        },
    });
}

