//Declaration of variables 
var authToken = coreERPAPI_Token;
// crud is the route to the api controllers
var customerApiUrl = coreERPAPI_URL_Root + "/crud/customer";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var citiesApiUrl = coreERPAPI_URL_Root + "/crud/city";
var paymentTermApiUrl = coreERPAPI_URL_Root + "/crud/paymentTerm";



var customers = {};
var cities = {};
var accounts = {};
var currencies = {};
var paymentTerms = {};

var customerAjax = $.ajax({
    url: customerApiUrl + '/Get',
	type: 'Get',
	contentType: 'application/json',
	beforeSend: function(req) {
		req.setRequestHeader('Authorization', "coreBearer " + authToken);
	}
});

var accountAjax = $.ajax({
    url: acctsApiUrl + '/GetByCategory?categoryId=1',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store unitOfMeasurement table ajax call in it
var citiesAjax = $.ajax({
    url: citiesApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var currencyAjax = $.ajax({
    url: currencyApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var paymentTermAjax = $.ajax({
    url: paymentTermApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

$(function () {
    displayLoadingDialog();
    loadForm();
});

//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {

    $.when(customerAjax,accountAjax, citiesAjax, currencyAjax, paymentTermAjax)
        .done(function (dataCustomer,dataAccount, dataCities, dataCurrency, dataPaymentTerm) {
            customers = dataCustomer[2].responseJSON;
			accounts = dataAccount[2].responseJSON;
            cities = dataCities[2].responseJSON;
            currencies = dataCurrency[2].responseJSON;
            paymentTerms = dataPaymentTerm[2].responseJSON;			
            //prepare UI
            prepareUi();
    });
}

//Function to prepare user interface
function prepareUi() {
    $('#tabs').kendoTabStrip();
    renderGrid();
    dismissLoadingDialog();     
}



function renderGrid() {
    $('#customersGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
					entries.success(customers);
                },
                create: function(entries) {
                    entries.success(entries.data);
                },
                update: function(entries) {
                    entries.success();
                },
                destroy: function(entries) {
                    entries.success();
                },
                parameterMap: function(data) {
                    return JSON.stringify(data);
                },

            }, //transport
            pageSize: 10,
            schema: {
                model: {
                    id: 'customerId',
                    fields: {
                        customerId: { type: 'number' },
                        customerNumber: { type: 'string', editable: false },
                        customerName: { type: 'string', editable: false },
                        paymentTermID: { type: 'string', editable: false },
                        currencyId: { type: 'string', editable: false },
                        glAccountId: { type: 'string', editable: false },
                        balance: { type: 'number', editable: false },
                        locationId: { type: 'string', editable: false }                        
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        }, //datasource
        columns: [
            { field: 'customerNumber', title: 'Customer Number' },
            { field: 'customerName', title: 'Name' },
            { field: 'paymentTermID', title: 'Payment Term', template: '#= getPaymentTerm(paymentTermID) #' },
            { field: 'currencyId', title: 'Currency', template: '#= getCurrency(currencyId) #' },
            { field: 'glAccountId', title: 'Account', template: '#= getGlAccount(glAccountId) #' },
            { field: 'balance', title: 'Account Balance' },
            { field: 'locationId', title: 'City', template: '#= getLocation(locationId) #' },
            {
                command: [gridEditButton], width:100
            }
        ],
        toolbar: [
            {
                className: 'addCustomer',
                text: 'Add New Customer'
            },
        ], //toolbar  
        filterable: true,
        sortable: {
            mode: "multiple"
        },
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5
        },
        groupable: true,
        selectable: true
    });

    $(".addCustomer").click(function () {
        window.location = "/Customers/EnterCustomer";
    });
}

function getPaymentTerm(id) {
    for (i = 0; i < paymentTerms.length; i++) {
        if (paymentTerms[i].paymentTermID == id) {
            return paymentTerms[i].paymentTerms;
        }
    }
}

function getCurrency(id) {
    for (i = 0; i < currencies.length; i++) {
        if (currencies[i].currency_id == id) {
            return currencies[i].major_name;
        }
    }
}

function getGlAccount(id) {
    for (i = 0; i < accounts.length; i++) {
        if (accounts[i].acct_id == id) {
            return accounts[i].acc_name;
        }
    }
}


function getLocation(id) {
    for (i = 0; i < cities.length; i++) {
        if (cities[i].city_id == id) {
            return cities[i].city_name;
        }
    }
}


var gridEditButton = {
    name: "Edit Customer",
    text: "Edit",
    click: function(e) {
        var tr = $(e.target).closest("tr"); //get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/Customers/EnterCustomer/" + data.customerId.toString();
    }

};
