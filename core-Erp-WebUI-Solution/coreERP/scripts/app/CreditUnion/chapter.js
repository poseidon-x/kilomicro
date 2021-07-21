/*
UI Scripts for Loan Category Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var chapterApiUrl = coreERPAPI_URL_Root + "/crud/CreditUnionChapter";
var chapterPdfExportUrl = coreERPAPI_URL_Root + "/Export/CreditUnionChapter/Pdf?token=" + authToken; 
var accountApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount"; 

var accounts = []; 

$(function () {

    $.ajax({
        url: accountApiUrl + "/Get",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        accounts = data;


        var ui = new chapterUI();
        ui.renderGrid();
        $("#toolbar").click(exportToPdf);

    });
});

var chapterUI = (function () {
    function chapterUI() {
    }
    chapterUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: chapterApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: chapterApiUrl + "/Post",
                        type: "POST",
                        contentType: "application/json",
                        accepts: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: chapterApiUrl + "/Put",
                        type: "PUT",
                        contentType: "application/json",
                        accepts: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: chapterApiUrl + "/Delete",
                        type: "DELETE",
                        contentType: "application/json",
                        accepts: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    parameterMap: function (data, type) {
                        return JSON.stringify(data);
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "creditUnionChapterID",
                        fields: {
                            creditUnionChapterID: {
                                editable: false,
                                type: "number"
                            }, 
                            chapterName: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            dateFormed: {
                                editable: true,
                                validation: {
                                    required: true
                                },
                                type:"date"
                            },
                            town: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            docRegistrationNumber: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            postalAddress: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            emailAddress: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            telePhoneNumber: {
                                editable: true,
                                validation: {
                                    required: true
                                } 
                            },
                            pricePerShare: {
                                editable: true,
                                validation: {
                                    required: true
                                } 
                            },
                            membersEquityAccountID: {
                                editable: true,
                                validation: {
                                    required: true
                                } 
                            },
                            vaultAccountID: {
                                editable: true,
                                validation: {
                                    required: true
                                } 
                            },
                            dividendsExpenseAccountID: {
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
                { field: "chapterName", title: "Chapter Name", width: 200, editor:longTextEditor },
                { field: "dateFormed", title: "Date Formed", editor: dateEditor, format: "{0:dd-MMM-yyyy}" },
                { field: "postalAddress", title: "Address", editor: longTextEditor },
                { field: "town", title: "Town" },
                { field: "docRegistrationNumber", title: "Reg. Number" },
                { field: "emailAddress", title: "Email", editor: longTextEditor },
                { field: "telePhoneNumber", title: "Tel. #" },
                { field: "pricePerShare", title: "Price per Share", editor: numericEditor, format:"{0:#,##0.#0}" },
                {
                    field: "membersEquityAccountID", title: "Equity Account", width: 200,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(membersEquityAccountID) #"
                },
                {
                    field: "vaultAccountID", title: "Vault Account", width: 200,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(vaultAccountID) #"
                },
                {
                    field: "dividendsExpenseAccountID", title: "Dividends Expense Account", width: 200,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(dividendsExpenseAccountID) #"
                },
                { command: ["edit"] }
            ],
            toolbar: [
                { name: "create", text: "Add Chapter" },
                {
                    name: "pdf",
                    text: "Export to PDF",
                    imageClass: "k-icon k-i-custom",
                    imageUrl: "/images/pdf.jpg",
                    url: chapterPdfExportUrl,
                    template: kendo.template($("#toolbar-icon-pdf").html())
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true,
            edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
                editWindow.wrapper.css({ width: 700 });
                editWindow.title("Credit Union Chapter Manager");
            }
        });
    };
     
    chapterUI.prototype.accountDropDownEditor = function (container, options) {
        $('<input required data-text-field="acc_name" data-value-field="acct_id" data-bind="value:' + options.field + '"/>')
            .width(450)
            .appendTo(container)
            .kendoDropDownList({
                optionLabel: " ",
                autoBind: false,
                dataSource: accounts
            });
    }
     
    return chapterUI;
})();

var longTextEditor = function (container, options) {
    $('<input data-bind="value:' + options.field + '" style="width:500px;"/>')
        .appendTo(container);
}

var dateEditor = function (container, options){
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDatePicker({ 
        format: "dd-MMM-yyyy"
    });
}

var numericEditor = function (container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            decimals: 2,
            min: 0,
            format: '#,##0.#0'
        });
};

function exportToPdf(e) { 
    window.open(chapterPdfExportUrl, "_blank");
    return false;
} 

function getAccountName(accountID) {
    var accountName = "";

    for (var i = 0; i < accounts.length; i++) {
        if (accounts[i].acct_id == accountID) {
            accountName = accounts[i].acc_name;
        }
    }

    return accountName;
}
 