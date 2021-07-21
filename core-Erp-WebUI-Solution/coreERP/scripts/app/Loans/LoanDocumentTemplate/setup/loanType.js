/*
UI Scripts for Loan LoanType Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var LoanTypeApiUrl = coreERPAPI_URL_Root + "/crud/LoanType";
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

        accounts.push({
            acct_id: 0,
            acc_name: "Not Selected",
        });

        var ui = new LoanTypeUI();
        ui.renderGrid();
    });
});

var LoanTypeUI = (function () {
    function LoanTypeUI() {
    }
    LoanTypeUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: LoanTypeApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: LoanTypeApiUrl + "/Post",
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: LoanTypeApiUrl + "/Put",
                        type: "PUT",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: LoanTypeApiUrl + "/Delete",
                        type: "DELETE",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }
                },
                pageSize: 10,
                schema: {
                    model: {
                        id: "loanTypeID",
                        fields: 
                           {
                               loanTypeID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               loanTypeName: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               vaultAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               bankAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               writeOffAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               unearnedInterestAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               interestIncomeAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               unpaidCommissionAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               commissionAndFeesAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               accountsReceivableAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               unearnedExtraChargesAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               tagPrefix: {
                                   validation: {
                                       required: false,
                                   },
                                   defaultValue: 0,
                               },
                               incentiveAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               holdingAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               refundAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               withHoldingAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               apIncentiveAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               apCommissionAccountID: {
                                   validation: {
                                       required: true,
                                   }
                               },
                               commissionAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },
                               provisionExpenseAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               },                            
                               provisionsAccountID: {
                                   validation: {
                                       required: true,
                                   },
                                   defaultValue: 0,
                               }
                           }
                    }
                }
            },
            columns: [
                {
                    field: "loanTypeID",
                    title: "Loan Type ID",
                    width: 75,
                    filterable: false
                },
                { field: "loanTypeName", title: "Loan Type Name", width: 200 },
                {
                    field: "vaultAccountID",
                    title: "Vault Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(vaultAccountID) #",
                },
                {
                    field: "bankAccountID",
                    title: "Bank Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(bankAccountID ) #",
                },
                {
                    field: "unearnedInterestAccountID",
                    title: "Unearned Inerest Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(unearnedInterestAccountID ) #",
                },
                {
                    field: "interestIncomeAccountID",
                    title: "Interest Income Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(interestIncomeAccountID ) #",
                },
                {
                    field: "unpaidCommissionAccountID",
                    title: "Upaid Commision Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(unpaidCommissionAccountID ) #",
                }, 
                {
                    field: "commissionAndFeesAccountID",
                    title: "Commision and Fees Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(commissionAndFeesAccountID ) #",
                },
                {
                    field: "accountsReceivableAccountID",
                    title: "Account Receivable Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(accountsReceivableAccountID ) #",
                }, 
                {
                    field: "unearnedExtraChargesAccountID",
                    title: "Unearned Extra Charges Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(unearnedExtraChargesAccountID ) #",
                },
                {
                    field: "tagPrefix",
                    title: "Tag Prefix",
                    width: 160,
                },
                {
                    field: "incentiveAccountID",
                    title: "Incentive Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(incentiveAccountID ) #",
                },
                {
                    field: "holdingAccountID",
                    title: "Holding Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(holdingAccountID ) #",
                },
                {
                    field: "refundAccountID",
                    title: "Refund Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(refundAccountID ) #",
                },
                {
                    field: "withHoldingAccountID",
                    title: "With Holding Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(withHoldingAccountID ) #",
                },
                {
                    field: "apIncentiveAccountID",
                    title: "A/P Incentive Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(apIncentiveAccountID ) #",
                },
                {
                    field: "commissionAccountID",
                    title: "Commission Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(commissionAccountID ) #",
                },
                {
                    field: "provisionExpenseAccountID",
                    title: "Provision Expense Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(provisionExpenseAccountID ) #",
                },
                {
                    field: "provisionsAccountID",
                    title: "Provisions Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(provisionsAccountID ) #",
                },
                {
                    field: "writeOffAccountID",
                    title: "Write-Off Account",
                    width: 160,
                    editor: this.accountDropDownEditor,
                    template: "#= getAccountName(writeOffAccountID ) #",
                },
                {
                    command: ["edit", "destroy"],
                    width:100,
                },
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true,
            toolbar: [
                {
                    name: "create",
                    title: "Add New Loan Type",
                }
            ],
        });
    };

    LoanTypeUI.prototype.accountDropDownEditor = function (container, options) {
        try{
            $('<input required data-text-field="acc_name" data-value-field="acct_id" data-bind="value:' + options.field + '"/>')
                .width(160)
                .appendTo(container)
                .kendoDropDownList({
                    optionLabel: " ",
                    autoBind: false,
                    dataSource: accounts,
                    width: 160
                });
        }
        catch(e){}
    }

    return LoanTypeUI;
})();

function mandatoryCheckbox(container, options) {
    $('<input type="checkbox" data-bind ="checked:' + options.field + '" ></input>').appendTo(container);
}

function LoanTypeIDEditor(container, options) {
    $('<input type="number" data-bind ="value:' + options.field + '" ></input>').appendTo(container);
}

function getAccountName(accountID) {
    var accountName = "";

    try{
        for (var i = 0; i < accounts.length; i++) {
            if (accounts[i].acct_id === accountID) {
                accountName = accounts[i].acc_name;
            }
        }
    }
    catch (e) { }

    return accountName;
}
