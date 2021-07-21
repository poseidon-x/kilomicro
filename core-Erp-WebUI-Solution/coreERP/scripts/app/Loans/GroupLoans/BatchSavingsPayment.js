"use strict"

//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var LoanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";
var LoanGroupBatchRepaymentApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroupBatchRepayment";

//Declaration of variables to store records retrieved from the database
var loanGroups = {};
var dueGroupRepayments = [];
var repaymentModel = {};

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {
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
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});
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
	$('#tabs').kendoTabStrip();
}
var onGroupChange = function () {
    var id = $("#group").data("kendoComboBox").value();
    var exist = true;
    //Reseet Grid to empty
    $('#repaymentGrid').html('');
	
    
			getSavings();
       
	
    if (!exist) {
        warningDialog('Invalid Group', 'ERROR');
        $("#groupName").data("kendoComboBox").value("");
        $("#day").data("kendoComboBox").value("");
		//day.enable() = true;
    }
}

//make column uneditable
function nonEditor(container, options) {
    container.text(options.model[options.field]);
    container.removeClass("k-edit-cell");
}

function getSavings() {
    repaymentModel.groupId = $("#group").data("kendoComboBox").value();
    repaymentModel.repaymentDate = $("#paymentDate").data("kendoDatePicker").value();
    
	displayLoadingDialog();	
	$.ajax({
	    url: LoanGroupBatchRepaymentApiUrl + "/GetSavings",
		type: 'POST',
        contentType: "application/json",
		data: JSON.stringify(repaymentModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }      
    }).success(function (data) {
		dismissLoadingDialog();
		if(data.length > 0)
		{
		    dueGroupRepayments = data;
            console.log("grid", dueGroupRepayments)
			renderGrid();				
		}
		else{
			//groups.setDataSource(loanGroups);
			warningDialog('There is no Loans for the selected Group ','ERROR');
			//groups.enable(false);
		}

	}).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}




//render Grid
function renderGrid() {
    $('#savingsGrid').kendoGrid({
		dataSource: {
			transport: {
				read: function(entries) {
                    entries.success(dueGroupRepayments);
                },
                update: function (entries) {
                    entries.success();
                },
				parameterMap: function(options, operation) {
					if (operation !== "read" && options.models) {
						return {models: kendo.stringify(options.models)};
					}
				}
			},
			schema: {
				model: {
					id: "savingID",
					fields: {
					    savingID: { editable: true, validation: { required: true } },
					    savingNo: { editable: false },
					    principalBalance: { editable: false },
					    clientID: { editable: false, type: "number" },
					    paid: { type: "boolean", editable: true, validation: { required: true } },
					    savingPlanAmount: { editable: true },
					    interestBalance: { editable: false },
					    Branch: { editable: false }
					}
				}
			},
			
			group: [
                { field: "Branch" },
                { field: "savingNo" }
			]
		},		
		columns: [
			{
			    field: 'clientAccNum', title: 'Client Acc. No.', editor: nonEditor},
			    { field: 'clientName', title: 'Client Name.', editor: nonEditor },
			   { field: 'savingNo', title: 'Savings No.', editor: nonEditor },
			{ field: 'principalBalance', title: 'Principal Balance', editor: nonEditor },

			{ field: 'savingPlanAmount', title: 'Amount Paid' },
                        { field: 'paid', template: '<input type="checkbox" #= paid ? \'checked="paid"\' : "" # disabled="disabled" />' },



			{ command: [{ name: "edit", text: "Edit" }]}
		],
		navigatable: true,
		groupable: false,
		pageable: {
			pageSize: 50,
			pageSizes: [50, 150, 300, 600, 1000],
			previousNext: true,
			buttonCount: 5,
		},
		toolbar: [				
				{
					name: "save",
					className: 'saveChanges',
					text: 'Save Amount',
				}
			],
		editable: "inline"
	});
	$(".saveChanges").click(function () {
		saveRepayment();
	});
}
function saveRepayment() {
	var validator = $("#myform").kendoValidator().data("kendoValidator");
		
	if (validator.validate()) {
	    var grid = $('#savingsGrid').data('kendoGrid');
	    grid.dataSource.group([]);
	    var repayments = $("#savingsGrid").data().kendoGrid.dataSource.view();
		if (repayments.length > 0 ) {
			displayLoadingDialog();
			saveToServer(repayments);
		} else {
			smallerWarningDialog('Please add Schedule to pay for', 'NOTE');
		}
	}else{smallerWarningDialog('One or more input is empty or invalid', 'WARNING');}
}
function saveToServer(repaymentData) {
    var postModel = {
        savings: repaymentData,
        paymentDate: $('#paymentDate').data('kendoDatePicker').value()
    }
    $.ajax({
        url: LoanGroupBatchRepaymentApiUrl + "/PostNewSavings",
		type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(postModel),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }      
    }).done(function (data) {
        dismissLoadingDialog();
        console.log("newdata", data);
        var message = "";
        if (data.successfulSavings.length) {
            message += data.successfulSavings.length + " successful savings\n";
            successDialog(message,
       'SUCCESS', function () { window.location = "/ln/setup/postTill.aspx"; });
        }
        if (data.failedSavings.length) {
            message += data.failedSavings.length + " failed savings\n";
            errorDialog(message,
		'FAILED', function () { window.location = "/dash/home.aspx"; });
        }
        //successDialog(message,
		//'', function () { window.location = "/dash/home.aspx"; });
	}).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}


