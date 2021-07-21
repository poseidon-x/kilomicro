                                                                //Declaration of variables 
var authToken = coreERPAPI_Token;

// crud is the route to the api controllers
var productApiUrl = coreERPAPI_URL_Root + "/crud/product";
var productSubCategoryApiUrl = coreERPAPI_URL_Root + "/crud/productSubCategory";
var inventoryMethodApiUrl = coreERPAPI_URL_Root + "/crud/inventoryMethod";
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/unitOfMeasurement";
var productMakeApiUrl = coreERPAPI_URL_Root + "/crud/productMake";
var productStatusApiUrl = coreERPAPI_URL_Root + "/crud/productStatus";

var products = {};
var productsList = [];
var productSubCategories = {};
var inventoryMethods = {};
var unitOfMeasurements = {};
var productMakes = {};
var productStatus = {};


$(function () {
    displayLoadingDialog();

    var productAjax = $.ajax({
        url: productApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store location table ajax call in it
    var productSubCategoryAjax = $.ajax({
        url: productSubCategoryApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store inventoryItem table ajax call in it
    var inventoryMethodAjax = $.ajax({
        url: inventoryMethodApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store unitOfMeasurement table ajax call in it
    var unitOfMeasurementAjax = $.ajax({
        url: unitOfMeasurementApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store product table ajax call in it
    var productMakeAjax = $.ajax({
        url: productMakeApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store brand table ajax call in it
    var productStatusAjax = $.ajax({
        url: productStatusApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });



    $.when(productAjax, productSubCategoryAjax, inventoryMethodAjax, unitOfMeasurementAjax, productMakeAjax, productStatusAjax)
        .done(function (dataProduct, dataProductSubCategory, dataInventoryMethod, dataUnitOfMeasurement, dataProductMake, dataProductStatus) {

            // the actual data is contained in the third position in the array hence accessing position index 2
            products = dataProduct[2].responseJSON;
            productSubCategories = dataProductSubCategory[2].responseJSON;
            inventoryMethods = dataInventoryMethod[2].responseJSON;
            unitOfMeasurements = dataUnitOfMeasurement[2].responseJSON;
            productMakes = dataProductMake[2].responseJSON;
            productStatus = dataProductStatus[2].responseJSON;
			
			dismissLoadingDialog();
			renderGrid();
        });


});


function renderGrid() {
	$('#tabs').kendoTabStrip();
    $('#productsGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
						entries.success(products);					
				},
                create: function (entries) {
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
                    entries.success();
                },
                parameterMap: function (data) {
                    return JSON.stringify(data);
                },
            }, //transport
            pageSize: 10,
            schema: {
                // the array of repeating data elements (depositType)
                //data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                //total: "Count",
                model: {
                    id: 'productId',
                    fields: {
                        productId: { type: 'number', defaultValue: products.productId },
                        productSubCategoryId: { type: 'number', editable: false, },
                        productCode: { type: 'string', editable: false, },
                        productName: { type: 'string', editable: false, },
                        inventoryMethodId: { type: 'number', editable: false, },
                        unitOfMeasurementId: { type: 'number', editable: false, },
                        productMakeId: { type: 'number', editable: false, },
                        productStatusId: { type: 'number', editable: false, },
                        //creator: { type: 'string', editable: false, },
                        //created: { type: 'date', editable: false, },



                    }, //fields
                }, //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }, //datasource
        columns: [
            { field: 'productSubCategoryId', title: 'Product Sub-Category', template: '#= getProductSubCategory(productSubCategoryId) #' },
            { field: 'productCode', title: 'Product Code' },
            { field: 'productName', title: 'Product Name' },
            { field: 'inventoryMethodId', title: 'Inventory Method', template: '#= getInventoryMethod(inventoryMethodId) #' },
            { field: 'unitOfMeasurementId', title: 'Unit Of Measurement', template: '#= getUnitOfMeasurement(unitOfMeasurementId) #' },
            { field: 'productMakeId', title: 'Product Make', template: '#= getProductMake(productMakeId) #' },
            { field: 'productStatusId', title: 'Product Status', template: '#= getProductStatus(productStatusId) #' },
            {
                command: [gridEditButton],
                width: 230

            },
        ],
        toolbar: [
            {
                className: 'addProduct',
                text: 'Add New Product',
            },
            //"pdf",
            //"excel"
        ], //toolbar  

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

    $(".addProduct").click(function () {
        window.location = "/ProductManagement/Product";
    });
}

function getProductSubCategory(id) {
    for (i = 0; i < productSubCategories.length; i++) {
        if (productSubCategories[i].productSubCategoryId == id) {
            return productSubCategories[i].productSubCategoryName;
        }
    }
}
function getInventoryMethod(id) {
    for (i = 0; i < inventoryMethods.length; i++) {
        if (inventoryMethods[i].inventoryMethodId == id) {
            return inventoryMethods[i].inventoryMethodName;
        }
    }
}
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
    name: "Edit Product",
    text: "Edit",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/ProductManagement/Product/" + data.productId.toString();
    },

};

var gridApproveButton = {
    name: "Approve Shrinkage",
    text: "Approve",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/InventoryShrinkage/ApproveShrinkage/" + data.shrinkageBatchId.toString();
    },

};

var gridPostButton = {
    name: "Post Shrinkage",
    text: "Post",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/InventoryShrinkage/PostShrinkage/" + data.shrinkageBatchId.toString();
    },

};
