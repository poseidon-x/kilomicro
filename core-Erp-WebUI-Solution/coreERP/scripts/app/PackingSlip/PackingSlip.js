var authToken = coreERPAPI_Token;
var invoiceApiUrl = coreERPAPI_URL_Root + "/crud/ArInvoice";

var invoices = {};

$(function () {

    $.ajax(
    {   //Fetch data/record(s) from customer and assign to customer variable      
        url: invoiceApiUrl + '/Get',
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        invoices = data;
        renderControls();
    }).error(function (error) {
        alert(JSON.stringify(error));
    });//customer ajax call



});

function renderControls() {
    $("#customerId").width('55%')
      .kendoComboBox({
          dataSource: invoices,
        dataValueField: 'customerId',
        dataTextField: 'customerName',
        change: onCustomerId,
        optionLabel: ''
    });
}

var onCustomerId = function () {
    displayLoadingDialog();
    var id = $("#customerId").data("kendoComboBox").value();
    $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
    loadReport(id);
    dismissLoadingDialog();
}

function loadReport(id) {
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "coreData.Reports.PackingSlip.PackingSlipReport, coreData",
                parameters: {
                    customerId: id
                },
            },
            persistSession: false
        });
}

