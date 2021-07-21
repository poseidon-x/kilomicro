var authToken = coreERPAPI_Token;
var transactionApiUrl = coreERPAPI_URL_Root + "/crud/CreditUnionTransaction";
var transactionPdfExportUrl = coreERPAPI_URL_Root + "/Export/CreditUnionTransaction/Pdf?token=" + authToken;
var memberApiUrl = coreERPAPI_URL_Root + "/crud/CreditUnionMember";  
var bankApiUrl = coreERPAPI_URL_Root + "/crud/BankAccount";
var modeOfPaymentApiUrl = coreERPAPI_URL_Root + "/crud/ModeOfPayment";

var members = [];
var transaction = {}; 
var modeOfPayments = [];
var banks = [];
var transactionTypes = [{ ID: "O", Description: "Openning Balance" },
{ ID: "C", Description: "Buy Shares" },
{ ID: "D", Description: "Sell Shares" }
];
$(function () {

    $.ajax({
        url: transactionApiUrl + "/NewTransaction",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        transaction = data;

        $.ajax({
            url: memberApiUrl + "/MemberLookUp",
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data2) {
            members = data2;
            renderDropDown("creditUnionMemberID", members, function (e) {
                getPrice();
            });

            $.ajax({
                url: modeOfPaymentApiUrl + "/Get",
                beforeSend: function (req) {
                    req.setRequestHeader('Authorization', "coreBearer " + authToken);
                }
            }).done(function (data3) {
                modeOfPayments = data3;
                renderDropDown("modeOfPaymentID", modeOfPayments, null);

                $.ajax({
                    url: bankApiUrl + "/Get",
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                }).done(function (data4) {
                    banks = data4;
                    renderDropDown("bankID", banks, null);

                    $("#transactionDate").kendoDatePicker({
                        format: "dd-MMM-yyyy"
                    });
                    $("#numberOfShares").kendoNumericTextBox({
                        change: function (e) {
                            getPrice();
                        }
                    });
                    $("#sharePrice").val(transaction.sharePrice);
                    $("#sharePrice").kendoNumericTextBox();
                    $("#amount").kendoNumericTextBox();
                    $("#save").kendoButton();
                    renderDropDown("transactionType", transactionTypes);
                    $("#save").click(function (e) {
                        save();
                    });
                });
            });
        });
    });
});

function getPrice() {
    $.ajax({
        url: memberApiUrl + "/Get/" + $("#creditUnionMemberID").val(),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        var member = data;
        $("#sharePrice").data("kendoNumericTextBox").value(member.creditUnionChapter.pricePerShare);
        $("#amount").data("kendoNumericTextBox").value(member.creditUnionChapter.pricePerShare * $("#numberOfShares").data("kendoNumericTextBox").value());
    });
}

function save() {
    transaction.transactionDate = $("#transactionDate").data("kendoDatePicker").value();
    transaction.numberOfShares = $("#numberOfShares").val();
    transaction.bankID = $("#bankID").val();
    transaction.modeOfPaymentID = $("#modeOfPaymentID").val();
    transaction.transactionType = $("#transactionType").val();
    transaction.creditUnionMemberID = $("#creditUnionMemberID").val();
    transaction.checkNumber = $("#chequeNumber").val();

    $.ajax({
        url: transactionApiUrl + "/Post",
        type: "POST",
        data: JSON.stringify(transaction),
        contentType: "application/json",
        accepts: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        },
        error: function (e){
            "Could not Save Transaction";
        }
    }).done(function (data) {
        alert("Transaction Saved Successfully");
        document.location = "/";
    });
}

function renderDropDown(controlID, dataSource, changeFunction)
{
    $('#' + controlID) 
        .kendoDropDownList({
            autoBind: false,
            optionLabel: " ",
            dataSource: dataSource,
            dataTextField: "Description",
            dataValueField: "ID",
            change: changeFunction
        });
}