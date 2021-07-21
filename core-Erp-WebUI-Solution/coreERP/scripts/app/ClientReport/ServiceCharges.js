var authToken = coreERPAPI_Token;
var cashierTillApiUrl = coreERPAPI_URL_Root + "/crud/CashierTill";
var chargeTypeApiUrl = coreERPAPI_URL_Root + "/crud/ClientChargeType";


var chargeTypes = {};
var cashiers = {};
var reportParam = {
    startDate: new Date(),
    endDate: new Date(),
    chargeTypeId: 0,
    cashier: ""
};

var cashierTillAjax = $.ajax({
    url: cashierTillApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var chargeTypeAjax = $.ajax({
    url: chargeTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {
    $.when(chargeTypeAjax, cashierTillAjax)
        .done(function (dataChargeType, dataCashierTill) {
            chargeTypes = dataChargeType[2].responseJSON;
            cashierTills = dataCashierTill[2].responseJSON;           

            //Prepares UI
            prepareUi();
            dismissLoadingDialog();
        });
}


$(function () {
    loadForm();
});

function prepareUi() {
    $("#startDate").width('70%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        value: new Date()
    });
    $("#endDate").width('70%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        value: new Date()
    });
    $("#chargeType").width('70%')
      .kendoDropDownList({
        dataSource: chargeTypes,
        dataValueField: "chargeTypeID",
        dataTextField: "chargeTypeName",
        optionLabel: '-----Select Charge Type------',
        filter: "contains",
        suggest: true,
        ignoreCase: true,
      });
    $("#cashier").width('70%')
      .kendoDropDownList({
          dataSource: cashierTills,
          dataValueField: "cashiersTillID",
          dataTextField: "userName",
          optionLabel: '-----Select Cashier------',
          filter: "contains",
          suggest: true,
          ignoreCase: true
      });
    $('#view').click(function (event) {
        saveCharge();
    });
}

function saveCharge() {
    $("#reportViewer1").html('');
    retrieveData();
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/Reporting/templates/telerikReportViewerTemplate-FA.html',
            reportSource: {
                report: "coreData.Reports.Client.ClientServiceCharge, coreData",
                parameters: {
                    cashier: reportParam.cashier,
                    chargeTypeId: reportParam.chargeTypeId,
                    endDate:reportParam.endDate,
                    startDate: reportParam.startDate
                },
            },
            persistSession: false
        });
}

function retrieveData() {
    var cashier = $("#cashier").data("kendoDropDownList").value();
    var chargeType = $("#chargeType").data("kendoDropDownList").value();
    var startDate = $("#startDate").data("kendoDatePicker").value();
    var endDate = $("#endDate").data("kendoDatePicker").value();

    if (cashier != "") reportParam.cashier = cashier;
    if (chargeType != "") reportParam.chargeTypeId = chargeType;
    if (endDate != "") reportParam.endDate = endDate;
    if (startDate != "") reportParam.startDate = startDate;
}

