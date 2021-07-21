var authToken = coreERPAPI_Token;
var inventoryItemApiUrl = coreERPAPI_URL_Root + "/crud/inventoryItem";
var productApiUrl = coreERPAPI_URL_Root + "/crud/product";
var locationApiUrl = coreERPAPI_URL_Root + "/crud/location";
var brandApiUrl = coreERPAPI_URL_Root + "/crud/brand";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";
var inventoryMethodApiUrl = coreERPAPI_URL_Root + "/crud/inventoryMethod";
var currencyApiUrl = coreERPAPI_URL_Root + "/crud/currency";

var inventoryItems = {};
var products = {};
var locations = {};
var brands = {};
var assetAccounts = {};
var expenseAccounts = {};
var inventoryMethods = {};
var currencies = {};
var id;

$(function () {
    displayLoadingDialog();
    loadForm();
});

function loadForm() {
   //Declare a variable and store inventoryItem table ajax call in it
    var inventoryItemAjax = $.ajax({
        url: inventoryItemApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store location table ajax call in it
    var productAjax = $.ajax({
        url: productApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store location table ajax call in it
    var locationAjax = $.ajax({
        url: locationApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    //Declare a variable and store location table ajax call in it
    var brandAjax = $.ajax({
        url: brandApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    var assetAccountAjax = $.ajax({
        url: acctsApiUrl + '/GetByCategory?categoryId=1',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    var expenseAccountAjax = $.ajax({
        url: acctsApiUrl + '/GetByCategory?categoryId=6',
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    });

    var inventoryMethodAjax = $.ajax({
        url: inventoryMethodApiUrl + '/Get',
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

    $.when(inventoryItemAjax, productAjax, locationAjax, brandAjax, assetAccountAjax, expenseAccountAjax, inventoryMethodAjax, currencyAjax)
        .done(function (dataInventoryItem, dataProduct, dataLocation, dataBrand, dataAssetAccount, dataExpenseAccount, datainventoryMethod, datacurrency) {
            inventoryItems = dataInventoryItem[2].responseJSON;
            products = dataProduct[2].responseJSON;
            locations = dataLocation[2].responseJSON;
            brands = dataBrand[2].responseJSON;
            assetAccounts = dataAssetAccount[2].responseJSON;
            expenseAccounts = dataExpenseAccount[2].responseJSON;
            inventoryMethods = datainventoryMethod[2].responseJSON;
            currencies = datacurrency[2].responseJSON;

            //Prepares UI
            renderControl();
            dismissLoadingDialog();
        });
}

function renderControl() {
    $("#product")
    .width('50%')
    .kendoComboBox({
        dataSource: products,
        dataValueField: 'productId',
        dataTextField: 'productName',
        change: onProductChange,
        optionLabel: '------Please Select Product------',
    });


}

var onProductChange = function () {
    displayLoadingDialog();
    id = $("#product").data("kendoComboBox").value();
    renderGrid(id);
    dismissLoadingDialog();
}

function renderGrid(id) {
    $("#setupGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: inventoryItemApiUrl + "/Get/"+ id,
                    type: "POST",
                    contentType: "application/json",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                create: {
                    url: inventoryItemApiUrl + "/Post",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                update: {
                    url: inventoryItemApiUrl + "/Put",
                    type: "PUT",
                    contentType: "application/json",
                    dataType: "json",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                destroy: {
                    url: inventoryItemApiUrl + "/Delete",
                    type: "DELETE",
                    contentType: "application/json",
                    dataType: "json",
                    beforeSend: function(req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                parameterMap: function (data) {
                    return JSON.stringify(data);
                },

            },
            //transport

            pageSize: 10,
            schema: {
                // the array of repeating data elements (depositType)
                data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                total: "Count",
                model: {
                    id: "inventoryItemId",
                    fields:
                    {
                        itemNumber: { type: 'string', validation: { required: true }, editable: false, },
                        inventoryItemId: { editable: false },
                        productId: { defaultValue: id, editable: false, },
                        locationId: { type: 'number', validation: { required: true } },
                        brandId: { type: 'number', validation: { required: true } },
                        inventoryItemName: { type: 'string', validation: { required: true } },
                        unitPrice: { type: 'number', validation: { required: true, min: 1 } },
                        safetyStockLevel: { type: 'number', validation: { required: true, min: 1 } },
                        reorderPoint: { type: 'number', validation: { required: true, min: 1 } },
                        accountId: { type: 'number', validation: { required: true } },
                        shrinkageAccountId: {  type: 'number',validation: { required: true } },
                        inventoryMethodId: { type: 'number', validation: { required: true } },
                        currencyId: { type: 'number', validation: { required: true } }
                    } //fields
                } //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }, //datasource
        columns: [
            { field: "itemNumber", title: "Item Number"},

            {
                field: "locationId", title: "Location", editor: locationEditor,
                template: '#= getLocation(locationId) #'
            },
            {
                field: "brandId", title: "Brand", editor: brandEditor,
                template: '#= getBrand(brandId) #'
            },
            { field: "inventoryItemName", title: "Item Name" },
            { field: "unitPrice", title: "Unit Price"},
            { field: "safetyStockLevel", title: "Safety Stock Level"},
            { field: "reorderPoint", title: "Reorder level"},
            {
                field: "accountId", title: "Inventory Account", editor: assetAccountEditor,
                template: '#= getAssetAccount(accountId) #'
            },
            {
                field: "shrinkageAccountId", title: "Shrinkage Account", editor: expenseAccountEditor,
                template: '#= getExpenseAccount(shrinkageAccountId) #'
            },
            {
                field: "inventoryMethodId", title: "Inventory Method", editor: inventoryMethodEditor,
                template: '#= getInventoryMethod(inventoryMethodId) #'
            },
            {
                field: "currencyId", title: "Currency", editor: currencyEditor,
                template: '#= getCurrency(currencyId) #'
            },

            {command: ["edit", "destroy"],width: 170,}, //col4
        ], //columns
        toolbar: [
            {
                name: "create",
                text: "Add New Inventory Item",
            }, //tool1
            "pdf",
            "excel"
        ], //toolbar
        excel: {
            fileName: "inventoryItem.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "inventoryItem.pdf"
        },
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
        edit: function(e) {
            var editWindow = this.editable.element.data("kendoWindow");
            editWindow.wrapper.css({ width: 500 });
            editWindow.title("Edit Inventory Item");
        },
        save: function(e) {
            $('.k-grid-update').css('display', 'none');
           // saveBrand();
        },

        mobile: true,
        reorderable: true,
        resizable: true,
    }); // brandUI.prototype.renderGrid = function() 
}

function productEditor(container, options) {
    $('<input type="text" id="productId" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: products,
        dataValueField: "productId",
        dataTextField: "productName",
        optionLabel: ' '
    });
}

function locationEditor(container, options) {
    $('<input type="text" id="locationId" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: locations,
        dataValueField: "locationId",
        dataTextField: "locationName",
        optionLabel: ' '
    });
}

function brandEditor(container, options) {
    $('<input type="text" id="brandId" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: brands,
        dataValueField: "brandId",
        dataTextField: "brandName",
        optionLabel: ' '
    });
}

function assetAccountEditor(container, options) {
    $('<input type="text" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: assetAccounts,
        dataValueField: "acct_id",
        dataTextField: "acc_name",
        optionLabel: ' '
    });
}

function expenseAccountEditor(container, options) {
    $('<input type="text" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: expenseAccounts,
        dataValueField: "acct_id",
        dataTextField: "acc_name",
        optionLabel: ' '
    });
}

function inventoryMethodEditor(container, options) {
    $('<input type="text" id="inventoryMethodId" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: inventoryMethods,
        dataValueField: "inventoryMethodId",
        dataTextField: "inventoryMethodName",
        optionLabel: ' '
    });
}

function currencyEditor(container, options) {
    $('<input type="text" id="icurrencyId" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoDropDownList({
        dataSource: currencies,
        dataValueField: "currency_id",
        dataTextField: "major_name",
        optionLabel: ' '
    });
}

function getProduct(id) {
    for (i = 0; i < products.length; i++) {
        if (products[i].productId == id)
            return products[i].productName;
    }
}

//retrieve inventory Item value from the Grid pop and display on the Grid
function getBrand(id) {
    for (i = 0; i < brands.length; i++) {
        if (brands[i].brandId == id)
            return brands[i].brandName;
    }
}

function getLocation(id) {
    for (i = 0; i < locations.length; i++) {
        if (locations[i].locationId == id)
            return locations[i].locationName;
    }
}

function getInventoryMethod(id) {
    for (i = 0; i < inventoryMethods.length; i++) {
        if (inventoryMethods[i].inventoryMethodId == id)
            return inventoryMethods[i].inventoryMethodName;
    }
}

function getCurrency(id) {
    for (i = 0; i < currencies.length; i++) {
        if (currencies[i].currency_id == id)
            return currencies[i].major_name;
    }
}

function getAssetAccount(id) {
    for (i = 0; i < assetAccounts.length; i++) {
        if (assetAccounts[i].acct_id == id)
            return assetAccounts[i].acc_name;
    }
}

function getExpenseAccount(id) {
    for (i = 0; i < expenseAccounts.length; i++) {
        if (expenseAccounts[i].acct_id == id)
            return expenseAccounts[i].acc_name;
    }
}









  