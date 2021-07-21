//***********************************************************//
//  	     CREDIT MEMO - JAVASCRIPT                
// 		CREATOR: EMMANUEL OWUSU(MAN)    	   
//		      DATE: AUG(10TH - 14th), 2015  		  
//*********************************************************//


//"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var borrowingApiUrl = coreERPAPI_URL_Root + "/crud/Borrowing";
var borrowingClientApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingClients";


//Declaration of variables to store records retrieved from the database
var borrowings = {};
var clients = {};
var selectedBorrowing = {};


function loadForm() {
    displayLoadingDialog();
	$.ajax({
		url: borrowingClientApiUrl + '/GetApprovedBorrowingClient',
		type: 'Get',
		beforeSend: function(req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}
	}).done(function (data) {
		clients = data;
		prepareUi();
		
		dismissLoadingDialog();
			
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

//Function to prepare user interface
function prepareUi() 
{		
    renderControls();

    $('#save').click(function (event) {

        var validator = $("#myform").kendoValidator().data("kendoValidator");
        var gridData = $("#grid").data().kendoGrid.dataSource.view();


		//if (!validator.validate()) {
        //    smallerWarningDialog('A form input is empty or has invalid value', 'ERROR');
    //}
    //else {
			if (confirm('Are you sure you want Save Repayment Schedule?')) {
                displayLoadingDialog();
				selectedBorrowing.borrowingRepaymentSchedules = [];
                saveGridData(gridData);

				saveSchedule();				
            } else {
                smallerWarningDialog('Please review and save later', 'NOTE');
            }
		//}
	});
}

function saveGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            selectedBorrowing.borrowingRepaymentSchedules.push(data[i]);
        }
    }
    else {
		selectedBorrowing.borrowingRepaymentSchedules.push(data[0]);
	}
}

//Apply kendo Style to the input fields
function renderControls() {
    $("#client").width("100%")
		.kendoComboBox({
		    dataSource: clients,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientID",
		    dataTextField: "clientName",
			change: onClientChange,
			optionLabel: ""
	});
		
    $("#borrowing").width('100%')
		.kendoComboBox({
		    dataSource: borrowings,
		    dataValueField: "borrowingId",
		    dataTextField: "borrowingNo",
		    change: onBorrowingChange,
		    optionLabel: ""
	});		
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
            url: borrowingApiUrl + '/GetClientApprovedBrws/' + id,
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            borrowings = data;
			document.getElementById('grid').innerHTML ='';

            //Set the returned data to loan datasource
			$("#borrowing").data("kendoComboBox").value("");
            $("#borrowing").data("kendoComboBox").setDataSource(borrowings);
            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
    }
}

var onBorrowingChange = function () {
    var id = $("#borrowing").data("kendoComboBox").value();
    var exist = false;	

    //Retrieve value enter validate
    for (var i = 0; i < borrowings.length; i++) {
        if (borrowings[i].borrowingId == id) {
			
			selectedBorrowing = borrowings[i];
			$('#tabs').kendoTabStrip();
			renderGrid();
		    
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Borrowing', 'ERROR');
        $("#borrowing").data("kendoComboBox").value("");
    }
}

//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(selectedBorrowing.borrowingRepaymentSchedules);
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
                    id: 'borrowingRepaymentScheduleId',
                    fields: {
                        borrowingRepaymentScheduleId: { type: 'number' },
                        borrowingId: { type: 'number', editable: false },
                        repaymentDate: { type: 'date' },
                        interestPayment: { type: 'number' },
                        principalPayment: { type: 'number' },
                        interestBalance: { type: 'number', editable: false },
                        principalBalance: { type: 'number', editable: false },
                        totalPayment: { type: 'number', editable: false },
                        totalBalance: { type: 'number', editable: false }
					} //fields
                } //model
            } //schema
        }, //datasource
		editable: 'popup',
        scrollable: false,
        columns: [
			{ field: 'repaymentDate', title: 'Repayment Date', format: "{0:dd-MMM-yyyy}" },
            { field: 'interestPayment', title: 'Interest Payment', format: "{0: #,###.#0}" },
            { field: 'principalPayment', title: 'Principal Payment', format: "{0: #,###.#0}"  },
            { field: 'interestBalance', title: 'Interest Balance', format: "{0: #,###.#0}"  },
            { field: 'principalBalance', title: 'Principal Balance', format: "{0: #,###.#0}"   },
            { field: 'totalPayment', title: 'Total Payment', format: "{0: #,###.#0}"  },
            { field: 'totalBalance', title: 'Total Balance', format: "{0: #,###.#0}"  },
            { command: ['edit','destroy']  }
		],		
        edit: function (e) {
            var editWindow = this.editable.element.data("kendoWindow");
            editWindow.wrapper.css({ width: 400 });
            editWindow.title("Edit Page");
        },
		toolbar: [{ name: 'create', text: 'Add New Schedule' }]
    });
}


//retrieve values from from Input Fields and save 
function saveSchedule() {
    //retrieveValues();
    saveToServer();
}


function retrieveValues() {
    //loanAdditionalInfo.loanId = $('#loan').data('kendoComboBox').value();
}

//Save to server function
function saveToServer() {
		
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: borrowingApiUrl + '/Put',
        type: 'Put',
        contentType: 'application/json',
        data: JSON.stringify(selectedBorrowing),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Borrowing Repayment Saved Successfully:', 'SUCCESS', function () { window.location = '/dash/home.aspx'; });        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

