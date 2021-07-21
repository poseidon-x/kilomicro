//***********************************************************//
//  	     CREDIT MEMO - JAVASCRIPT                //
// 		CREATOR: EMMANUEL OWUSU(MAN)    	   //
//		      WEEK: JUNE(8TH - 12TH), 2015  		  //
//*********************************************************//


//"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var clientApiUrl = coreERPAPI_URL_Root + "/crud/LoanClient";
var loansApiUrl = coreERPAPI_URL_Root + "/crud/ClientLoan";
var loanRepmtApiUrl = coreERPAPI_URL_Root + "/crud/LoanRepayment";
var loanRestcApiUrl = coreERPAPI_URL_Root + "/crud/LoanRestructure";
var bankAccountApiUrl = coreERPAPI_URL_Root + "/crud/Bank";
var modeOfPaymentApiUrl = coreERPAPI_URL_Root + "/crud/LoanModeOfPayment";




//Declaration of variables to store records retrieved from the database
var clients = {};
var loans = {};
var selLoan = {};
var selLoanRep = {};
var newSchds= {};
var bankAccounts= {};
var modeOfPayments= {};
var id = -1

var restructureAjax = $.ajax({
    url: loanRestcApiUrl + '/Get/' + id,
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

var bankAccountAjax = $.ajax({
    url: bankAccountApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store currency table ajax call in it
var modeOfPaymentAjax = $.ajax({
    url: modeOfPaymentApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {
    $.when(restructureAjax, clientAjax, bankAccountAjax, modeOfPaymentAjax)
        .done(function (dataRestructure, dataClient, dataBankAccount, dataModeOfPayment) {
			selLoanRep = dataRestructure[2].responseJSON;
            clients = dataClient[2].responseJSON;
            bankAccounts = dataBankAccount[2].responseJSON;
            modeOfPayments = dataModeOfPayment[2].responseJSON;
			
			prepareUi();
			dismissLoadingDialog();
        });
}


//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});




//Function to prepare user interface
function prepareUi() 
{		
    renderControls();

    $('#save').click(function (event) {

        var validator = $("#myform").kendoValidator().data("kendoValidator");


		//if (!validator.validate()) {
        //    smallerWarningDialog('A form input is empty or has invalid value', 'ERROR');
    //}
    //else {
			if (confirm('Are you sure you want Restructure this Loan?')) {
                displayLoadingDialog();
				selLoanRep.saveChanges = true;
				SaveAdditionLoan();				
            } else {
                smallerWarningDialog('Please review and Restructure later', 'NOTE');
            }
		//}
	});
	
	$('#viewSchedule').click(function (event) {
	//if (!validator.validate()) {
      //      smallerWarningDialog('A form input is empty or has invalid value', 'ERROR');
        //} else {
            displayLoadingDialog();
			ViewAdditionLoanSchd();
        //}   
	});
}

//Apply kendo Style to the input fields
function renderControls() {
    

    $("#client").width("75%")
		.kendoComboBox({
		    dataSource: clients,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientID",
		    dataTextField: "clientName",
			change: onClientChange,
			optionLabel: ""
	});
		
    $("#loan").width('75%')
		.kendoComboBox({
		    dataSource: loans,
		    dataValueField: "loanID",
		    dataTextField: "loanNo",
		    change: onLoanChange,
		    optionLabel: ""
	});

    $("#originalprincipal").width('75%')
        .kendoNumericTextBox({
        format: "#,##0.00"
    });

    $("#originalinterest").width("75%")
        .kendoNumericTextBox({
		     format: "#,##0.00"
        });

    $("#remainingprincipal").width("75%")
        .kendoNumericTextBox({
            format: "#,##0.00"
        });

    $("#remaininginterest").width("75%")
        .kendoNumericTextBox({
            format: "#,##0.00"
        });

    $("#addedPenalty").width("75%")
    .kendoNumericTextBox({
        format: "#,##0.00"
    });

    $("#remainingPenalty").width("75%")
        .kendoNumericTextBox({
            format: "#,##0.00"
        });

    $("#totalBalanceBd").width("75%")
        .kendoNumericTextBox({
            format: "#,##0.00"
        });

    $("#originalTenure").width("75%")
        .kendoNumericTextBox({
            format: "0 'Months'"
        });
		
	$("#interestRate").width("75%")
        .kendoNumericTextBox({
            format: "# \\%"
        });

    $("#disbursementDate").width("75%")
        .kendoDatePicker({
        format: 'dd-MMM-yyyy',
        parseFormats: ['dd-MM-yyyy', 'yyyy-MM-dd', 'yyyy-MMM-dd'],
    });

    $("#additionalTenure").width("75%")
        .kendoNumericTextBox({
            format: "0 'Months'"
        });

    $("#extraDisbursementAmount").width("75%")
        .kendoNumericTextBox({
            format: "#,##0.00"
        });
		
    $("#paymentMode").width("75%")
        .kendoComboBox({
		    dataSource: modeOfPayments,
		    dataValueField: "modeOfPaymentID",
		    dataTextField: "modeOfPaymentName",
		    //change: onLoanChange,
		    optionLabel: ""
	});
		
		$("#bank").width("75%")
        .kendoComboBox({
		    dataSource: bankAccounts,
		    dataValueField: "bank_acct_id",
		    dataTextField: "bank_acct_desc",
		    //change: onLoanChange,
		    optionLabel: ""
	});

    $("#checkNo").width("75%")
        .kendoMaskedTextBox();		
		
}


var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Cliet', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    } else {
        displayLoadingDialog();
				
		
        $.ajax(
        {
            url: loansApiUrl + '/GetClientLoans/' + id,
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            loans = data;
			document.getElementById('grid').innerHTML ='';

            //Set the returned data to loan datasource
			$("#loan").data("kendoComboBox").value("");
            $("#loan").data("kendoComboBox").setDataSource(loans);
            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
    }
}

var onLoanChange = function () {
    var id = $("#loan").data("kendoComboBox").value();
    var exist = false;
	var lnAmt = 0;
	var principalBalance = 0;
	var interestBalance = 0
	

    //Retrieve value enter validate
    for (var i = 0; i < loans.length; i++) {
        if (loans[i].loanID == id) {
			selLoan = loans[i];
			lnAmt = loans[i].amountDisbursed;
			
			 displayLoadingDialog();
			 $.ajax(
				{
					url: loanRepmtApiUrl + '/Get/' + id,
					type: 'Get',
					beforeSend: function (req) {
						req.setRequestHeader('Authorization', "coreBearer " + authToken);
					}
				}).done(function (data) {
					selLoanRep = data;
					
					var totalInterestRemaining = 0;
					var totalPrincipalRemaining = 0;
					
					totalInterestRemaining = selLoanRep.loanTotalInterest - selLoanRep.totalIntrPaid;
					totalPrincipalRemaining = selLoanRep.loanTotalPrincipal - selLoanRep.totalPrinPaid;
						
					$("#originalprincipal").data("kendoNumericTextBox").value(selLoanRep.loanTotalPrincipal);
					$("#originalinterest").data("kendoNumericTextBox").value(selLoanRep.loanTotalInterest);
					$("#remainingprincipal").data("kendoNumericTextBox").value(totalPrincipalRemaining);
					$("#remaininginterest").data("kendoNumericTextBox").value(totalInterestRemaining);							
					$("#addedPenalty").data("kendoNumericTextBox").value(selLoanRep.totalyPenalties);
					$("#remainingPenalty").data("kendoNumericTextBox").value(selLoanRep.penaltyBalance);
					$("#totalBalanceBd").data("kendoNumericTextBox").value(selLoanRep.loanBalance);
					$("#originalTenure").data("kendoNumericTextBox").value(selLoan.loanTenure);
					$("#additionalTenure").data("kendoNumericTextBox").value(0);
					$("#extraDisbursementAmount").data("kendoNumericTextBox").value(0);
					$("#interestRate").data("kendoNumericTextBox").value(selLoan.interestRate);
					$("#disbursementDate").data("kendoDatePicker").value(selLoan.disbursementDate);


					
					dismissLoadingDialog();
				}).error(function (error) {
					alert(JSON.stringify(error));
				});
			 
			
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Loan', 'ERROR');
        $("#loan").data("kendoComboBox").value("");
    }
}

//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(newSchds);
                },
                create: function(entries) {
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
                    entries.success();
                }
            }, //transport
            schema: {
                model: {
                    id: 'repaymentScheduleID',
                    fields: {
                        repaymentScheduleID: { type: 'number', defaultValue: newSchds.repaymentScheduleID },
                        repaymentDate: { type: 'date', editable: false },
                        interestPayment: { type: 'number', editable: false },						
                        principalPayment: { type: 'number', editable: false },
                        interestBalance: { type: 'number', editable: false  },
                        principalBalance: { type: 'number', editable: false }
						}, //fields
                }, //model
            }, //schema
        }, //datasource
        columns: [
			{ field: 'repaymentDate', title: 'Date', format: '{0:dd-MMM-yyyy}' },
            { field: 'interestPayment', title: 'Interest Payment', format: '{0:#,##0.00}' },
            { field: 'principalPayment', title: 'Principal Payment', format: '{0:#,##0.00}' },
            { field: 'interestBalance', title: 'Interest Balance', format: '{0:#,##0.00}' },
            { field: 'principalBalance', title: 'Principal Balance', format: '{0:#,##0.00}' },
       ],
    });
}


//retrieve values from from Input Fields and save 
function SaveAdditionLoan() {
    retrieveValues();
    saveToServer();
}

function ViewAdditionLoanSchd() {
    retrieveValues();
	
	$.ajax({
        url: loanRestcApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(selLoanRep),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		newSchds = data;
		//warningDialog(JSON.stringify(newSchds));
		renderGrid();
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
	
}


function retrieveValues() {
    selLoanRep.loan = selLoan;
    selLoanRep.additionalTenure =  $('#additionalTenure').data('kendoNumericTextBox').value();
    selLoanRep.additionalPrincipal =  $('#extraDisbursementAmount').data('kendoNumericTextBox').value();
    selLoanRep.interestRate =  $('#interestRate').data('kendoNumericTextBox').value();
    selLoanRep.paymentMode =  $('#paymentMode').data('kendoComboBox').value();	
    selLoanRep.bank =  $('#bank').data('kendoComboBox').value();
    selLoanRep.checkNo =  $('#checkNo').data('kendoMaskedTextBox').value();

}

//Save to server function
function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: loanRestcApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(selLoanRep),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Loan Successfully Restructured:', 'SUCCESS', function () { window.location = '/dash/home.aspx'; });        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer
