var authToken = coreERPAPI_Token;
var paymentApiUrl = coreERPAPI_URL_Root + "/crud/ArPayment";

//var invoices = {};
var arPayment = {};

$(function () {

    $.ajax(
    {   //Fetch data/record(s) from customer and assign to customer variable      
        url: paymentApiUrl + '/GetPaymentCustomers',
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        arPayment = data;
        renderControls();
    }).error(function (error) {
        alert(JSON.stringify(error));
    });//customer ajax call
});



function renderControls() {
    $("#customer").width('55%')
      .kendoComboBox({
          dataSource: arPayment,
        dataValueField: 'customerId',
        dataTextField: 'customerName',
        change: onCustomer,
        optionLabel: ''
    });
}

var onCustomer = function () {
    displayLoadingDialog();
    var id = $("#customer").data("kendoComboBox").value();
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
                report: "coreData.Reports.Payment.PaymentReport, coreData",
                parameters: {
                    customerId: id
                },
            },
            persistSession: false
        });
}

