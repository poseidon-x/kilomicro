/*
UI Scripts for Loan Category Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var transactionApiUrl = coreERPAPI_URL_Root + "/crud/CreditUnionTransaction";
var providerPdfExportUrl = coreERPAPI_URL_Root + "/Export/MomoProvider/Pdf?token=" + authToken;
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
        url: memberApiUrl + "/MemberLookUp",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data2) {
        members = data2;

        $.ajax({
            url: modeOfPaymentApiUrl + "/Get",
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data3) {
            modeOfPayments = data3;

            $.ajax({
                url: bankApiUrl + "/Get",
                beforeSend: function (req) {
                    req.setRequestHeader('Authorization', "coreBearer " + authToken);
                }
            }).done(function (data4) {
                banks = data4;

                var ui = new transactionUI();
                ui.renderGrid();
                $("#toolbar").click(exportToPdf);
            });
        });
    });
});

var transactionUI = (function () {
    function transactionUI() {
    }
    transactionUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: transactionApiUrl +"/Pending",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                },
                pageSize: 20,
                schema: {
                    model: {
                        id: "creditUnionShareTransactionID",
                        fields: {
                            creditUnionShareTransactionID: {
                                editable: false,
                                type: "number"
                            }, 
                            creditUnionMemberID: {
                                editable: true,
                                validation: {
                                    required: true
                                },
                                type:"number"
                            },
                            transactionDate: {
                                editable: true,
                                validation: {
                                    required: true
                                },
                                type: "date"
                            },
                            transactionType: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            }, 
                            modeOfPaymentID: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            checkNumber: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            bankID: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            numberOfShares: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            enteredBy: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            }
                        }
                    }
                }
            }, 
            columns: [ 
                {
                    field: "creditUnionMemberID", title: "Member Name", width: 200,
                    template: "#= getMemberName(creditUnionMemberID) #"
                },
                {
                    field: "transactionDate", title: "Tran Date",
                    format: "{0:dd-MMM-yyyy}"
                },
                {
                    field: "transactionType", title: "Tran Type",
                    template: "#= getTransactionType(transactionType) #"
                },
                {
                    field: "modeOfPaymentID", title: "Pmt Mode",
                    template: "#= getModeOfPayment(modeOfPaymentID) #"
                },
                {
                    field: "checkNumber", title: "Cheque #" 
                },
                {
                    field: "bankID", title: "Bank",
                    template: "#= getBankName(bankID) #"
                },
                {
                    field: "numberOfShares", title: "No of Shares",
                    format: "{0:#,##0.#0}"
                },
                {
                    field: "enteredBy", title: "Entered By", 
                },
                { command: [{ text: "Post", click: approve }] }
            ], 
            sortable: true,
            filterable: true, 
            pageable: true
        });
    };
      
    return transactionUI;
})();

function booleanCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function categoryIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function exportToPdf(e) { 
    window.open(providerPdfExportUrl, "_blank");
    return false;
} 

function getMemberName(memberID) {
    var memberName = "";

    for (var i = 0; i < members.length; i++) {
        if (members[i].ID == memberID) {
            memberName = members[i].Description;
        }
    }

    return memberName;
}

function getModeOfPayment(mpID) {
    var name = "";

    for (var i = 0; i < modeOfPayments.length; i++) {
        if (modeOfPayments[i].ID == mpID) {
            name = modeOfPayments[i].Description;
        }
    }

    return name;
}

function getTransactionType(tranType) {
    var typeName = "";

    for (var i = 0; i < transactionTypes.length; i++) {
        if (transactionTypes[i].ID == tranType) {
            typeName = transactionTypes[i].Description;
        }
    }

    return typeName;
}

function getBankName(bankID) {
    var bankName = "";

    for (var i = 0; i < banks.length; i++) {
        if (banks[i].ID == bankID) {
            bankName = banks[i].Description;
        }
    }

    return bankName;
}

function approve(e) {
    e.preventDefault();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var creditUnionShareTransactionID = dataItem.creditUnionShareTransactionID;
    $.ajax({
        url: transactionApiUrl + "/Approve/" + creditUnionShareTransactionID,
        type: "POST",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        if (data != null) {
            alert("Shares Transaction Successfully Posted.");
            $('#setupGrid').data('kendoGrid').dataSource.read();
        }
    });
}