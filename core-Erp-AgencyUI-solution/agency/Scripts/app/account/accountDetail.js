/*
Creator: damien@acsghana.com
*/

var authToken = agencyAPI_Token;
var clientApiUrl = agencyAPI_URL_Root + "/AllClients";

var CSModel = {};
var accountDetails = {};

var searchCriterias = [
		{value: 'accountnumber', name:'Account Number'},
		{value: 'accountname', name: 'Account Name'},
	];

$(function () {
    displayLoadingDialog();
    $.ajax(
    {   //Fetch data/record(s) from customer and assign to customer variable      
        url: clientApiUrl + '/GetSavingsClient',
		type: 'Get',
		beforeSend: function (req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}
    }).done(function (data) {
        //clients = data;
        renderControls();
		dismissLoadingDialog();
    }).error(function (error) {
		dismissLoadingDialog();
        warningDialog(JSON.stringify(error),'ERROR');
    });//customer ajax call
	
	var ui = new oUI();
	ui.prepareUI();
});

function renderControls() {
    $("#searchCriteria").width("95%").kendoComboBox({
		dataSource: searchCriterias,
		dataValueField: "value",
		dataTextField: "name",
		highlightFirst: true,
		change: function(e) {
			if (this.value() == 'accountnumber') {
				enInputAccNo()
			}
			else if (this.value() == 'accountname') {
				enInputAName()
			}
        }
	});
	$(".searchData").width("95%").kendoMaskedTextBox();
	
	$('#submit').click(function () {
		searchClient();
	});
}

function enInputAccNo(){
	$("#accNoDiv").show();
	$("#accNameDiv").hide();
	$("#accName").removeAttr('required');
}
function enInputAName(){
	$("#accNameDiv").show();
	$("#accNoDiv").hide();
	$("#accNo").removeAttr('required');
}

var oUI = (function () {
    function oUI() {		
    }  
	oUI.prototype.prepareUI = function (){
		$('#tabs').kendoTabStrip({});
		
		//this.renderAccountDetailGrid();
	}	
	oUI.prototype.renderAccountDetailGrid = function() {
        // use jQuery a selector to get the div with an id of accountDetailGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#accountDetailGrid").kendoGrid({
            dataSource: {
                transport: {
					read: function (entries) {
						entries.success(accountDetails);                    
					},
					parameterMap: function (data) { return JSON.stringify(data); }
				},
                schema: {
                    model: {

                        fields: {
                            clientNameWithAccountNO: { editable: false},
                            savingNo: { editable: false},
                            branch: { editable: false},
                            savingTypeName: { editable: false},
                            firstDepositDate: { editable: false, type: 'date'},
                            currentBalance: { editable: false},
                        }
                    }
                },
            },
		
			scrollable: false,
			sortable: true,
			pageable: true,
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5,
			},
			columns: [
				{ field: 'clientNameWithAccountNO', title: 'Account Name + CL Number' },
				{ field: 'savingNo', title: 'Savings Account Number' },
				{ field: 'branch', title: 'Branch' },
				{ field: 'savingTypeName', title: 'Saving Type' },
				{ field: 'firstDepositDate', title: 'First Deposit Date', format: "{0:dd-MM-yyyy}", attributes:{style:"text-align:center;"} },
				{ field: 'currentBalance', title: 'Current Balance', format: "{0: #,###.#0}", attributes:{style:"text-align:right;"} },
			]    
        });
    }

	return oUI;
})();

function searchClient() {
    //var validator = $("#myform").kendoValidator().data("kendoValidator");
    if (!validator.validate()) {
		warningDialog('One or More Fields are Empty','ERROR'); 
    } else {
		displayLoadingDialog();
		retrieveValues();
		sendRequestToServer();
	}
}

function retrieveValues(){
	
	var sAccNo = $('#accNo').data('kendoMaskedTextBox').value();
	var sAccName = $('#accName').data('kendoMaskedTextBox').value();

	CSModel.searchCriteria = $('#searchCriteria').data('kendoComboBox').value();
	
	if(sAccNo != ""){
		CSModel.searchData = sAccNo;
	}
	else if(sAccName != ""){
		CSModel.searchData = sAccName;
	}
}

function sendRequestToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/SearchClient',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(CSModel),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        //successDialog('Client Record Found', 'SUCCESS');
		accountDetails = data;
		var uI = new oUI;
		uI.renderAccountDetailGrid();
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}