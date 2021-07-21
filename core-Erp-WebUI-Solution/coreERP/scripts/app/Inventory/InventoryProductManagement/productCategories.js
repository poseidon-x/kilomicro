                                                                //Declaration of variables 
var authToken = coreERPAPI_Token;

// crud is the route to the api controllers
//var productApiUrl = coreERPAPI_URL_Root + "/crud/product";
var productCategoryApiUrl = coreERPAPI_URL_Root + "/crud/productCategory";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";

var productCategories = {};
var accts = {};

$(function () {
    displayLoadingDialog();

    //Declare a variable and store location table ajax call in it
    var productCategoryAjax = $.ajax({
        url: productCategoryApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store inventoryItem table ajax call in it
    var accountAjax = $.ajax({
		url: acctsApiUrl + '/Get',
		type: 'Get',
		beforeSend: function (req) {
			req.setRequestHeader('Authorization', "coreBearer " + authToken);
		}
	});

    $.when(productCategoryAjax, accountAjax)
        .done(function (dataProductCategory, dataAccount) {

            // the actual data is contained in the third position in the array hence accessing position index 2
            productCategories = dataProductCategory[2].responseJSON;
            accts = dataAccount[2].responseJSON;
			
			dismissLoadingDialog();
			renderGrid();
        });
});


function renderGrid() {
	$('#tabs').kendoTabStrip();
    $('#productCategoryGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
						entries.success(productCategories);					
				},
                create: function (entries) {
                    entries.success(entries.data);
                },
                parameterMap: function (data) {
                    return JSON.stringify(data);
                },
            }, //transport
            pageSize: 10,
            schema: {
                model: {
                    id: 'productCategoryId',
                    fields: {
                        productCategoryId: { type: 'number' },
                        productCategoryName: { type: 'string', editable: false, },
                        cogsAccountId: { type: 'string', editable: false, },
                        inventoryAccountId: { type: 'string', editable: false, },
                        apAccountId: { type: 'number', editable: false, },
                        arAccountId: { type: 'number', editable: false, },
                        incomeAccountId: { type: 'number', editable: false, },
                        expenseAccountId: { type: 'number', editable: false, }
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }, //datasource
        columns: [
            { field: 'productCategoryName', title: 'Category'},// template: '#= getProductSubCategory(productSubCategoryId) #' },
            { field: 'cogsAccountId', title: 'COGS Acct.', template: '#= getAccountName(cogsAccountId) #' },
            { field: 'inventoryAccountId', title: 'Inventory Acct.', template: '#= getAccountName(inventoryAccountId) #' },
            { field: 'apAccountId', title: 'Acct. Payable', template: '#= getAccountName(apAccountId) #' },
            { field: 'arAccountId', title: 'Acct. Receivable', template: '#= getAccountName(arAccountId) #' },
            { field: 'incomeAccountId', title: 'Income Acct.', template: '#= getAccountName(incomeAccountId) #' },
            { field: 'expenseAccountId', title: 'Expense Acct.', template: '#= getAccountName(expenseAccountId) #' },

            {
                command: [gridEditButton],
                width: 130
            },
        ],
        toolbar: [
            {
                className: 'addProduct',
                text: 'Add New Category',
            },
        ], 
        filterable: true,
        sortable: {
            mode: "multiple",
        },
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
        groupable: true,
        selectable: true,
		detailInit: lineInit,
		dataBound: function ()
		{
			this.expandRow(this.tbody.find("tr.k-master-row").first());
		},
    });

    $(".addProduct").click(function () {
        window.location = "ProductManagement/ProductCategory";
    });
}


function lineInit(e) {
    $("<div/>").appendTo(e.detailCell).kendoGrid({
		dataSource: {
            transport: {
                read: function (entries) {
                        if (typeof (e.data.productSubCategories) === "undefined") {
                            e.data.productSubCategories = [];
                        }
                        entries.success(e.data.productSubCategories); 
				},
                create: function (entries) {
                    entries.success(entries.data);
                },
                parameterMap: function (data) {
                    return JSON.stringify(data);
                },
            }, //transport
            pageSize: 10,
            schema: {
                model: {
                    id: 'productSubCategoryId',
                    fields: {
                        productSubCategoryId: { type: 'number',editable: false},
                        productCategoryId: { type: 'number', editable: false },
                        productSubCategoryName: { type: 'string', editable: false, }
                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }, //datasource
        columns: [
            { field: 'productSubCategoryName', title: 'Product Sub-Cat Name' }
        ],
        
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
    });
}
/*
function getProductSubCategory(id) {
    for (i = 0; i < productSubCategories.length; i++) {
        if (productSubCategories[i].productSubCategoryId == id) {
            return productSubCategories[i].productSubCategoryName;
        }
    }
}*/
function getAccountName(id) {
    for (i = 0; i < accts.length; i++) {
        if (accts[i].acct_id == id) {
            return accts[i].acc_name;
        }
    }
}
var gridEditButton = {
    name: "editCategory",
    text: "Edit Category",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/ProductManagement/ProductCategory/" + data.productCategoryId.toString();
    },

};
/*
function getUnitOfMeasurement(id) {
    for (i = 0; i < unitOfMeasurements.length; i++) {
        if (unitOfMeasurements[i].unitOfMeasurementId == id) {
            return unitOfMeasurements[i].unitOfMeasurementName;
        }
    }
}
function getProductMake(id) {
    for (i = 0; i < productMakes.length; i++) {
        if (productMakes[i].productMakeId == id) {
            return productMakes[i].productMakeName;
        }
    }
}
function getProductStatus(id) {
    for (i = 0; i < productStatus.length; i++) {
        if (productStatus[i].productStatusId == id) {
            return productStatus[i].productStatusName;
        }
    }
}

function getDateOnly(date) {
    var newDate = date.toDateString();
    return newDate;
}

var gridEditButton = {
    name: "Edit Category",
    text: "Edit",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/ProductManagement/ProductCategory/" + data.productCategoryId.toString();
    },

};
*/