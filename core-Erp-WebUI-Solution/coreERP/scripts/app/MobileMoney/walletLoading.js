var authToken = coreERPAPI_Token;
var walletApiUrl = coreERPAPI_URL_Root + "/crud/MomoWallet";
var walletPdfExportUrl = coreERPAPI_URL_Root + "/Export/MomoWallet/Pdf?token=" + authToken;
var providerApiUrl = coreERPAPI_URL_Root + "/crud/MomoProvider";
var accountApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var cashierApiUrl = coreERPAPI_URL_Root + "/crud/Cashier";
var bankApiUrl = coreERPAPI_URL_Root + "/crud/BankAccount";
var modeOfPaymentApiUrl = coreERPAPI_URL_Root + "/crud/ModeOfPayment";

var accounts = [];
var cashiers = [];
var providers = [];
var walletLoading = {};
var wallets = [];
var modeOfPayments = [];
var banks = [];

$(function () {

    $.ajax({
        url: walletApiUrl + "/NewLoading",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        walletLoading = data;

        $.ajax({
            url: walletApiUrl + "/WalletLookup",
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data2) {
            wallets = data2;
            renderDropDown("walletID", wallets);

            $.ajax({
                url: modeOfPaymentApiUrl + "/Get",
                beforeSend: function (req) {
                    req.setRequestHeader('Authorization', "coreBearer " + authToken);
                }
            }).done(function (data3) {
                modeOfPayments = data3;
                renderDropDown("modeOfPaymentID", modeOfPayments);

                $.ajax({
                    url: bankApiUrl + "/Get",
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                }).done(function (data4) {
                    banks = data4;
                    renderDropDown("bankID", banks);

                    $("#loadingDate").kendoDatePicker();
                    $("#amountLoaded").kendoNumericTextBox();
                    $("#save").kendoButton();
                });
            });
        });
    });
});

function renderDropDown(controlID, dataSource)
{
    $('#' + controlID) 
        .kendoDropDownList({
            autoBind: false,
            optionLabel: " ",
            dataSource: dataSource,
            dataTextField: "Description",
            dataValueField: "ID"
        });
}