/*
UI Scripts for Loan Category Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var systemDateApiUrl = coreERPAPI_URL_Root + "/crud/SystemDate";
var walletPdfExportUrl = coreERPAPI_URL_Root + "/Export/MomoWallet/Pdf?token=" + authToken;
 
$(function () {

    var ui = new systemDateUI();
    ui.renderGrid(); 

});

var systemDateUI = (function () {
    function systemDateUI() {
    }
    systemDateUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: systemDateApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: systemDateApiUrl + "/Post",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: systemDateApiUrl + "/Put",
                        type: "PUT",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    parameterMap: function (data, type) {
                        return kendo.stringify(data);
                    },
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "systemDateID",
                        fields: {
                            systemDateID: {
                                editable: false,
                                type: "number"
                            }, 
                            loanSystemDate: {
                                editable: true, 
                                type: "date"
                            },
                            savingSystemDate: {
                                editable: true,
                                type: "date"
                            },
                            depositSystemDate: {
                                editable: true, 
                                type: "date"
                            },
                            investmentSystemDate: {
                                editable: true, 
                                type: "date"
                            },
                            susuSystemDate: {
                                editable: true, 
                                type: "date"
                            },
                            creditUnionSystemDate: {
                                editable: true, 
                                type: "date"
                            },
                            accountsSystemDate: {
                                editable: true, 
                                type: "date"
                            }, 
                        }
                    }
                }
            }, 
            columns: [ 
                {
                    field: "loanSystemDate", title: "Loan", editor: dateEditor,
                    format: "{0:dd-MMM-yyyy}"
                },
                {
                    field: "savingSystemDate", title: "Savings", editor: dateEditor,
                    format: "{0:dd-MMM-yyyy}"
                }, {
                    field: "depositSystemDate", title: "Investments", editor: dateEditor,
                    format: "{0:dd-MMM-yyyy}"
                }, {
                    field: "investmentSystemDate", title: "Out. Investments", editor: dateEditor,
                    format: "{0:dd-MMM-yyyy}"
                }, {
                    field: "susuSystemDate", title: "Susu", editor: dateEditor,
                    format: "{0:dd-MMM-yyyy}"
                }, {
                    field: "creditUnionSystemDate", title: "Credit Union", editor: dateEditor,
                    format: "{0:dd-MMM-yyyy}"
                }, {
                    field: "accountsSystemDate", title: "Accounting", editor: dateEditor,
                    format: "{0:dd-MMM-yyyy}"
                }, 
                { command: ["edit"] }
            ],
            toolbar: [ 
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true,
            edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
                editWindow.wrapper.css({ width: 700 });
            }
        });
    };
     
    return systemDateUI;
})();
 
var dateEditor = function (container, options){
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDatePicker({ 
            format: "dd-MMM-yyyy"
        });
}
