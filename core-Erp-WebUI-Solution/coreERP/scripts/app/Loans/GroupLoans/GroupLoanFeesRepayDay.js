//*******************************************
//***   GROUP FEES REPAYMENT BY DAY GROUP JAVASCRIPT                
//***   CREATOR: SAMUEL WENDOLIN KETECHIE    	   
//***   DATE: JAN 17TH, 2019 	
//*******************************************


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var loanRepaymentApiUrl = coreERPAPI_URL_Root + "/crud/LoanRepayment";
var chargeTypeApiUrl = coreERPAPI_URL_Root + "/crud/LoanRepaymentType";
var LoanGroupDayApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupDay";


//Declaration of variables to store records retrieved from the database
var loanRepayment = {};
var chargeTypes = {};
var clientLoans = {};
  var days = {};

var repaymentTypes = [{
    repaymentTypeID: 6,
    repaymentTypeName:'Processing Fees'
},
{
    repaymentTypeID: 8,
    repaymentTypeName: 'Insurance'
}
];
//Function to call load form function
$(function () {
    displayLoadingDialog();
    document.querySelector("#snack_bar_warning").style.display = "none";
    document.querySelector("#snack_bar_success").style.display = "none";
    document.querySelector("#save").style.display = "none";
    loadForm();
});



function loadForm() {
    getTodayRepaymentFees();       
}

$("#save").click(function (event) {
    event.preventDefault();
    if (confirm("Are you sure you want to receive the fees payment ?")) {
        saveToServer();
    }
    else {
        smallerWarningDialog('Please review and apply later', 'NOTE');
    }
});

function getTodayRepaymentFees() {
    let errMess = "There are no fees to be repaid today.";
    var payModel = {
        repaymentDate:new Date().toLocaleString()
    }
    $.ajax({
        url: loanRepaymentApiUrl + "/GetTodayRepaymentFees",
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(payModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        if (data != null && data.repaymentFees.length > 0) {
            loanRepayment = data;
            renderGrid();
            document.querySelector("#save").style.display = "block";
        } else {
            showSnackBarWarning(errMess);
            dismissLoadingDialog();
            //Reset Grid to empty
            $('#paymentGrid').html('');
        }

    }).error(function (xhr, data, error) {
        //warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
        showSnackBarWarning(errMess);
        dismissLoadingDialog();
    });
}

function renderGrid() {
    $('#tabs').kendoTabStrip({});
    $('#paymentGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.data = loanRepayment.repaymentFees;
                    entries.success(entries.data);
                },
                create: function (entries) {
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success(entries.data);
                },
                //parameterMap: function(options, operation) {
                //    if (operation !== "read" && options.models) {
                //        return {models: kendo.stringify(options.models)};
                //    }
                //}
                parameterMap: function (data) {
                    return JSON.stringify(data);
                }
            },
            schema: {
                model: {
                    id: "paymentId",
                    fields: {
                        paymentId: { editable: true, type: "number" },
                        clientId: { editable: false },
                        clientName: { editable: false },
                        groupName: { editable: false },
                        loanNo: { editable: false },
                        insuranceAmount: { editable: false, type: "number" },
                        processingFeeAmount: { editable: false, type: "number" },
                        paid: { type: "boolean", editable: true, validation: { required: true } }
                    }
                }
            },
            aggregate: [
                    { field: "insuranceAmount", aggregate: "sum" },
                    { field: "processingFeeAmount", aggregate: "sum" }
            ],
            group: [
                {
                    field: "groupName",
                    aggregates: [
                      { field: "insuranceAmount", aggregate: "sum" },
                    { field: "processingFeeAmount", aggregate: "sum" }
                    ]
                },
            ]
        },
        columns: [
			{
			    field: 'clientName', title: 'Name',
			},
           {
               field: 'groupName',
               title: 'Group'
           },
            {
                field: 'loanNo',
                title: 'Loan No.'
            },
			
            {
                field: 'processingFeeAmount',
                title: 'Processing Fee',
                format: '{0: ##.#0}',
                footerTemplate: "Overall Total Fee: #: sum #",
                groupFooterTemplate: "Group Total Fee :  #= kendo.toString(sum, '00.00') #"
            },
            {
                field: 'insuranceAmount',
                title: 'Insurance Amt',
                format: '{0: ##.#0}',
                footerTemplate: "Overall Total Insurance: #: sum #",
                groupFooterTemplate: "Group Total Insurance :  #= kendo.toString(sum, '00.00') #"
            },
            {
                field: 'paid',
                title: 'Paid ?',
                template: '<input type="checkbox" #= paid ? \'checked="paid"\' : "" # disabled="disabled" />',
                width: 70
            },

			{
			    command: [
                    { name: "edit", text: "Check" }
			    ],
			    width: 180
			}
        ],
        resizable: true,        
        editable: "inline",
        dataBound: function (e) {
            var grid = this;
            $(".k-grouping-row").each(function (e) {
                grid.collapseGroup(this);
            });
        }
    });
    dismissLoadingDialog();
}


//Save to server function
function saveToServer() {
    displayLoadingDialog();
    let grid = $("#paymentGrid").data("kendoGrid"),
        actualData = grid.dataSource.data();
    loanRepayment.repaymentFees = actualData;
    $.ajax({
        url: loanRepaymentApiUrl + '/PostRepaymentFeesForDay',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(loanRepayment),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        dismissLoadingDialog();
        if (data === true) {
            let mess = "Charge payment successfully received.";
            showSnackBarSuccess(mess);
        } else {
            showSnackBarWarning("Charge was not saved. Kindly check and Save again.");

        }
        
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });

}

function showSnackBarWarning(message) {
    $("#snack_bar_warning").html(message);
    let x = document.getElementById("snack_bar_warning");
    x.className = "show";
    setTimeout(function () {
        x.className = x.className.replace("show", "");
    }, 9050);

}
function showSnackBarSuccess(message) {
    $("#snack_bar_success").html(message);
    let x = document.getElementById("snack_bar_success");
    x.className = "show alert-success";
    setTimeout(function () {
        x.className = x.className.replace("alert-success show", "");
        window.location = "/ln/setup/postTill.aspx";
    }, 9500);

}
