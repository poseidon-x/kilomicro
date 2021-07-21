//*******************************************
//***   CLIENT DEPOSIT RECEIPT JAVASCRIPT                
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: NOV 17TH, 2015  	
//*******************************************

//"use strict";

var investmentUpgradeApiUrl = coreERPAPI_URL_Root + "/crud/DepositUpgrade";
var clientsApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var depositTypeApiUrl = coreERPAPI_URL_Root + "/crud/depositType";
var depositApiUrl = coreERPAPI_URL_Root + "/crud/deposit";

var clients = {};
var depositTypes = {};
var deposits = {};
var depositSearch = {};
var depositPeriods = [
    { value:2, month:"60 Days" },
    { value:3, month:"91 Days" },
    { value:6, month:"182 Days" },	
    { value:12, month:"365 Days" }
];
var upgradeableDeposits = {};
var creteriaIndex;
var clientAjax = $.ajax({
    url: clientsApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var depositTypeAjax = $.ajax({
    url: depositTypeApiUrl + "/Get",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var depositAjax = $.ajax({
    url: investmentUpgradeApiUrl + "/GetRunningDeposit",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var depositSearchModelAjax = $.ajax({
    url: investmentUpgradeApiUrl + "/GetDepositSearchModel",
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
$(function () {
	displayLoadingDialog();
	loadData();	
});

function loadData() {
    $.when(clientAjax, depositTypeAjax, depositAjax,depositSearchModelAjax)
        .done(function (dataClient, dataDeposiType, dataDeposit,dataDepositSearchModel) {
            clients = dataClient[2].responseJSON;
            depositTypes = dataDeposiType[2].responseJSON;
			deposits = dataDeposit[2].responseJSON;
			depositSearch = dataDepositSearchModel[2].responseJSON;

            prepareUi();
            dismissLoadingDialog();
        }
	);
}

//Function to prepare user interface
function prepareUi() 
{	
    $("#tabs").width("100%").kendoTabStrip();
	$('#search').width('100%').kendoMaskedTextBox();
	
	$('input[type=radio][name=searchCriteria]').change(function() {
		$('#grid').html('');
		//$('#search').data('kendoComboBox').value("");
		if (this.value == 'product') {
			creteriaIndex = 'P'
			$('#search').width('100%').kendoComboBox({
				dataSource: depositTypes,
				dataValueField: 'depositTypeID',
				dataTextField: 'depositTypeName',
				filter: "contains",
				highlightFirst: true,
				suggest: true,
				ignoreCase: true,
				animation: {
					close: { effects: "fadeOut zoom:out", duration: 200 },
					open: { effects: "fadeIn zoom:in", duration: 200 }
				},
				change: onChange,
				optionLabel: '' 
			});
        }
        else if (this.value == 'depositNo') {
			creteriaIndex = 'D'
			$('#search').width('100%').kendoComboBox({
				dataSource: deposits,
				dataValueField: 'depositID',
				dataTextField: 'depositNo',
				filter: "contains",
				highlightFirst: true,
				suggest: true,
				ignoreCase: true,
				animation: {
					close: { effects: "fadeOut zoom:out", duration: 200 },
					open: { effects: "fadeIn zoom:in", duration: 200 }
				},
				change: onChange,
				optionLabel: '' 
			});
        }
		else if (this.value == 'clientName') {
			creteriaIndex = 'C'
			$('#search').width('100%').kendoComboBox({
				dataSource: clients,
				dataValueField: 'clientID',
				dataTextField: 'clientName',
				filter: "contains",
				highlightFirst: true,
				suggest: true,
				ignoreCase: true,
				animation: {
					close: { effects: "fadeOut zoom:out", duration: 200 },
					open: { effects: "fadeIn zoom:in", duration: 200 }
				},
				change: onChange,
				optionLabel: '' 
			});
        }
    });
	
	$('#searchButton').click(function (event) {
		var serh = $('#search').data('kendoComboBox').value();

		if (serh!=null && serh!="undefined") {
            displayLoadingDialog();
			upgradeableDeposits = {};
			getDeposits();				
		}else
		{
            smallerWarningDialog('Please add search creteria', 'NOTE');
        }
	});
	
    //renderGrid();
}

var onChange = function(){
	$('#grid').html('');
}

//render Grid
function renderGrid() {	
    $('#grid').kendoGrid({
        dataSource: {
            transport: {
					read: function (entries) {
						entries.success(upgradeableDeposits);					
					},
					parameterMap: function (data) { return JSON.stringify(data); }
			}, //transport
            schema: {
                    model: {
                    id: 'depositID',
                    fields: {
						depositID: { type: 'number' },
						clientID: { type: 'number' },
						depositNo: { type: 'string' },
						depositTypeID: { type: 'number' },
						depositID: { type: 'number' },
						interestRate: { type: 'number' },
						period: { type: 'number' },
						maturityDate: { type: 'date' }
						} //fields
					},
				}, //schema
        }, //datasource
        sortable: true,
        columns: [		
			{ field: 'clientID', title: 'Client',template: '#= getClient(clientID) #' },		
            { field: 'depositNo', title: 'Invest. No.' },
            { field: 'depositTypeID', title: 'Product', template: '#= getDepositType(depositTypeID) #' },			
            { field: 'depositID', title: 'Current Balance', format: "{0: #,###.#0}",template: '#= getBalance(principalBalance,interestBalance) #' },
            { field: 'interestRate', title: 'Interest rate', format: "{0: ##.#0}" },
            { field: 'period', title: 'Period(Days)', template: '#= getPeriod(period) #' },
			{ field: 'maturityDate', title: 'Maturity Date', format: "{0:dd-MMM-yyyy}" },
            { command: [applyInvestmentButton],width:100 }
		],
	    //toolbar: [{ name: "create", className: 'createNew', text: 'Receive New Invesment..'}],
		pageable: {
			pageSize: 10,
			pageSizes: [10, 25, 50, 100, 1000],
			previousNext: true,
			buttonCount: 5
		},
	});
	
	$(".createNew").click(function () {
        window.location = "/Deposit/InvestmentReceipt";
    });
}


var applyInvestmentButton = {
	name: "gridapply",
	text: "Upgrade..",
	click: function(e) {
		var tr = $(e.target).closest("tr"); // get the current table row (tr)
		var data = this.dataItem(tr);
		window.location = "/Deposit/DepositUpgrade/"+data.depositID.toString();
	}
};

function getClient(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            return clients[i].clientName;
        }
    }
}
function getDepositType(id) {
    for (var i = 0; i < depositTypes.length; i++) {
        if (depositTypes[i].depositTypeID == id) {
            return depositTypes[i].depositTypeName;
        }
    }
}
function getPeriod(id) {
    for (var i = 0; i < depositPeriods.length; i++) {
        if (depositPeriods[i].value == id) {
            return depositPeriods[i].month;
        }
    }
}
function getBalance(prinBal,intBal) {
    return (prinBal+intBal).toFixed(2);
}

function getDeposits() {
	
	if(creteriaIndex == 'C'){
		depositSearch.criteria = creteriaIndex;
		depositSearch.clientId = $('#search').data('kendoComboBox').value();
	}else if(creteriaIndex == 'D'){
		depositSearch.criteria = creteriaIndex;
		depositSearch.depositId = $('#search').data('kendoComboBox').value();
	}else if(creteriaIndex == 'P'){
		depositSearch.criteria = creteriaIndex;
		depositSearch.depositTypeId = $('#search').data('kendoComboBox').value();
	}
		
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: investmentUpgradeApiUrl + '/GetUpgradeableDepositsBySearch',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(depositSearch),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
		upgradeableDeposits = data;
        dismissLoadingDialog();
        if(upgradeableDeposits.length>0)renderGrid();        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});
	
}