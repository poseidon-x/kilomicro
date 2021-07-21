/*
UI Scripts for Loan depositType Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var loanClosureReasonApiUrl = coreERPAPI_URL_Root + "/crud/loanClosureReason";

$(function() {
    var ui = new loanClosureReasonUi();
    ui.renderGrid();
});

var loanClosureReasonUi = (function () {
    function loanClosureReasonUi() {
    }
    loanClosureReasonUi.prototype.renderGrid = function () {
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: loanClosureReasonApiUrl + "/Get",
                        type: "POST",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: loanClosureReasonApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: loanClosureReasonApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: loanClosureReasonApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function(req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    // the array of repeating data elements (depositType)
                    data: "Data",
                    // the total count of records in the whole dataset. used
                    // for paging.
                    total: "Count",
                    model: {
                        id: "loanClosureReasonId",
                        fields:
                        {
                            loanClosureReasonId: {
                                validation: {
                                    required: true,
                                },
                                defaultValue: 0,
                            },
                            loanClosureReasonName: {
                                validation: {
                                    required: true,
                                }, 
                            }, 
                        }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },
            columns: [
                {
                    field: "loanClosureReasonName",
                    title: "Reason",
                    width: 300
                }, 
                {
                    command: ["edit", "destroy"],
                    width: 220,
                },
            ],
            toolbar: [
                {
                    name: "create",
                    text: "Add New Reason",
                },
                "pdf",
                "excel"
            ],
            excel: {
                fileName: "loanClosureReasons.xlsx"
            },
            pdf: {
                paperKind: "A3",
                landscape: true,
                fileName: "loanClosureReasons.pdf"
            },
            filterable: true,
            sortable: {
                mode: "multiple",
            },
            editable: "popup",
            pageable: {
                pageSize: 10,
                pageSizes: [10, 25, 50, 100, 1000],
                previousNext: true,
                buttonCount: 5,
            },
            groupable: true,
            selectable: true,
            edit: function(e) {
                var editWindow = this.editable.element.data("kendoWindow");
                editWindow.wrapper.css({ width: 700 });
                editWindow.title("Edit Loan Closure Reasons Data");
            },
            save: function(e) {
                $('.k-grid-update').css('display', 'none');
            },
            mobile: true,
            reorderable: true,
            resizable: true,
        });
    };
     
    return loanClosureReasonUi;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function depositTypeIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function getAccountName(accountID) {
    var accountName = "";

    try {
        for (var i = 0; i < accounts.length; i++) {
            if (accounts[i].acct_id === accountID) {
                accountName = accounts[i].acc_name;
            }
        }
    }
    catch (e) { }

    return accountName;
}

function getPlanName(planId) {
    var planName = "";

    try {
        for (var i = 0; i < plans.length; i++) {
            if (plans[i].planId === planId) {
                planName = plans[i].planName;
            }
        }
    }
    catch (e) { }

    return planName;
}

var longTextEditor = function (container, options) {
    $('<input class="input-control" data-bind="value:' + options.field + '" style="width:500px;"/>')
        .appendTo(container);
}

var checkBoxTextEditor = function (container, options) {
    $('<input type="checkbox" class="input-control" data-bind="checked:' + options.field + '" />')
        .appendTo(container);
}

var dateEditor = function (container, options) {
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
            decimals: 6,
            min: 0,
            format: '#,##0.#####0'
        });
};

function booleanCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

