//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: SAMUEL WENDOLIN KETECHIE    	   
//***   DATE: DEC 4TH, 2018 	
//*******************************************


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var loanRepaymentApiUrl = coreERPAPI_URL_Root + "/crud/LoanRepayment";
var chargeTypeApiUrl = coreERPAPI_URL_Root + "/crud/LoanRepaymentType";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";


//Declaration of variables to store records retrieved from the database
var loanRepayment = {};
var chargeTypes = {};
var clients = {};
var clientLoans = {};
var paymentId = -1;
var loanGroups = {};
var loanGroupClients = {};
var groupLoans = {};
var loanNumber = {};
var _loanID = {};

//Function to call load form function
$(function () {
    displayLoadingDialog();
    document.querySelector("#snack_bar").style.display = "none";
    loadForm();
});

var loanDataSource = new kendo.data.DataSource({
    transport: {
        read: function (request) {
            var data = clientLoans;
            request.success(data);
        }
    }
});
var loanRepaymentAjax = $.ajax({
    url: loanRepaymentApiUrl + '/GetGroupLoanMulti',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var loanAjax = $.ajax({
    url: loanRepaymentApiUrl + '/GetLoanMulti',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var chargeTypeAjax = $.ajax({
    url: chargeTypeApiUrl + '/GetOthers',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var clientAjax = $.ajax({
    url: clientApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


function loadForm() {
    $.when(loanRepaymentAjax, chargeTypeAjax, clientAjax, loanAjax)
       .done(function (dataLoanRepayment, dataChargeType, dataClient, dataLoan) {
           loanRepayment = dataLoanRepayment[2].responseJSON;
           chargeTypes = dataChargeType[2].responseJSON;
           clients = dataClient[2].responseJSON;
           clientLoans = dataLoan[2].responseJSON;

           $.ajax({
               url: LoanGroupApiUrl + '/Get',
               type: 'Get',
               beforeSend: function (req) {
                   req.setRequestHeader('Authorization', "coreBearer " + authToken);
               }
           }).done(function (data) {
               dismissLoadingDialog();
               loanGroups = data;
               prepareUi();
           }).error(function (xhr, data, error) {
               dismissLoadingDialog();
               warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
           });



       });
}

function prepareUi() {
    $('#paymentDate').width('80%').kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        value: new Date()
    });
    $('#group').width('80%').kendoComboBox({
        dataSource: loanGroups,
        dataValueField: 'loanGroupId',
        dataTextField: 'loanGroupName',
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        ignoreCase: true,
        change: onGroupChange,
        optionLabel: '',
    });

    $("#save").click(function (event) {
        event.preventDefault();
        if (confirm("Are you sure you want to receive the fees payment ?")) {
            loanRepayment.groupId = $("#group").data("kendoComboBox").value();
            loanRepayment.paymentDate = $("#paymentDate").data("kendoDatePicker").value();
            saveToServer();
        }
        else {
            smallerWarningDialog('Please review and apply later', 'NOTE');
        }
    });
    $('#tabs').kendoTabStrip();
}
var onGroupChange = function () {
    var id = $("#group").data("kendoComboBox").value();
    var exist = false;
    //Reset Grid to empty
    $('#paymentGrid').html('');
    //Retrieve value enter validate
    for (var i = 0; i < loanGroups.length; i++) {
        if (loanGroups[i].loanGroupId == id) {
            exist = true;
            getClientsForGroup(id);
            renderGrid();
            break;
        }

    }
    if (!exist) {
        warningDialog('Invalid Group', 'ERROR');
        $("#group").data("kendoComboBox").value("");
        $('#paymentGrid').html('');
    }
}

function getClientsForGroup(groupId) {
    $.ajax({
        url: clientApiUrl + "/GetClientForGroup?groupId=" + groupId,
        type: 'Get',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        clients = data;
    });
}

function renderGrid() {
    $('#tabs').kendoTabStrip({});
    $('#paymentGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(loanRepayment.payments);
                },
                create: function (entries) {
                    entries.data.paymentId = paymentId;
                    paymentId--;
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success(entries.data);
                },
                destroy: function (entries) {
                    entries.success(entries.data);
                },
                parameterMap: function (data) { return JSON.stringify(data); }
            },
            schema: {
                model: {
                    id: "paymentId",
                    fields: {
                        paymentId: { editable: true, type: "number" },
                        clientID: { editable: true },
                        loanId: { editable: true },
                        repaymentTypeID: { editable: true },
                        amount: { editable: true, type: "number" }
                    }
                }
            },
            aggregate: [
                    { field: "amount", aggregate: "sum" }
            ]
        },
        columns: [
			{ field: 'clientID', title: 'Client Name', editor: clientEditor, template: '#= getClient(clientID) #' },
            { field: 'repaymentTypeID', title: 'Payment Type', editor: chargeTypeEditor, template: '#= getChargeType(repaymentTypeID) #' },
           {
               field: 'loanId',
               title: 'Loan Number',
               editor: loanEditor,
               template: '#= getLoan(clientID) #'
           },
			{
			    field: 'amount',
			    title: 'Amount',
			    format: '{0: #,###.#0}',
			    footerTemplate: "Total Amount: #: sum #",
			    groupFooterTemplate: "Total Amount :  #= kendo.toString(sum, '0,000.00') #"
			},
			{
			    command: [
                    { name: "edit" },
                  { name: "destroy" }
			    ],
			    width: 180
			}
        ],

        toolbar: [
            { name: "create", text: "Add Payment" },
        ],
        editable: "popup"
    });
}

function chargeTypeEditor(container, options) {
    $('<input required data-text-field="repaymentTypeName" data-value-field="repaymentTypeID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            optionLabel: "",
            highlightFirst: true,
            suggest: true,
            dataSource: {
                data: chargeTypes,
            },
            dataValueField: "repaymentTypeID",
            dataTextField: "repaymentTypeName",
            template: '<span>#: repaymentTypeName #</span>',
            filter: "contains",
        });
    var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
    tooltipElement.appendTo(container);
}

function getChargeType(id) {
    for (var i = 0; i < chargeTypes.length; i++) {
        if (chargeTypes[i].repaymentTypeID == id) {
            return chargeTypes[i].repaymentTypeName;
        }
    }
}

function getClient(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            return clients[i].clientName;
        }
    }
}

function clientEditor(container, options) {//editor
    $('<input required data-text-field="clientName" data-value-field="clientID" data-bind="value:' + options.field + '" id="client" />')
        .appendTo(container)
        .kendoDropDownList({
            optionLabel: "--Select Client--",
            highlightFirst: true,
            suggest: true,
            dataSource: {
                data: clients,
            },
            dataValueField: "clientID",
            dataTextField: "clientName",
            template: '<span>#: clientName #</span>',
            filter: "contains",
            change: onClientChange
        });
}

var onClientChange = function () {
    var id = $("#client").data("kendoDropDownList").value();
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            getClientUndisbursedLoans(id);
            return clients[i].clientName;
        }
    }


}

function getLoan(id) {
    var exist = false;    
    if (clientLoans.length != 0) {        
        for (var i = 0; i < clientLoans.length; i++) {
            //getClientUndisbursedLoans(id);
            _loanID = clientLoans[i].loanId;
            exist = true;
            if (exist == true) {
                loanNumber = clientLoans[i].loanNo;
            }
            return loanNumber;
        }
    }
}

function loanEditor(container, options) {//editor
    $('<input required data-text-field="loanNo" data-value-field="loanId" data-bind="value:' + options.field + '" id="loan" />')
        .appendTo(container)
        .kendoDropDownList({
            optionLabel: "",
            highlightFirst: true,
            suggest: true,
            dataSource: loanDataSource,
            dataValueField: "loanId",
            dataTextField: "loanNo",
            template: '<span>#: loanNo #</span>',
            filter: "contains",
        });
}

//Save to server function
function saveToServer() {
    displayLoadingDialog();
    //loanRepayment.loanId = $("loan").data('kendoDropDownList').value();
    loanRepayment.loanId = _loanID;
    loanRepayment.payments = $("#paymentGrid").data().kendoGrid.dataSource.view();

    $.ajax({
        url: loanRepaymentApiUrl + '/PostGroupMulti',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(loanRepayment),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        successDialog('Charge payment successfully received:', 'SUCCESS',
		function () { window.location = '/ln/setup/postTill.aspx'; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });

}

function getClientUndisbursedLoans(clientId) {
    displayLoadingDialog();
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetClientUndisbursedLoans/' + clientId,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        clientLoans = data;
        //var dropdown = $("#loan").data().kendoDropDownList;   
        loanDataSource.read();
        if (data.length <= 0) {
            showSnackBarWarning();
            dismissLoadingDialog();
        }
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}
function showSnackBarWarning() {
    let x = document.getElementById("snack_bar");
    x.className = "show";
    setTimeout(function () {
        x.className = x.className.replace("show", "");
    }, 10050);

}