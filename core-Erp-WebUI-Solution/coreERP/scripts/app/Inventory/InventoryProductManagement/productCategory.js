var authToken = coreERPAPI_Token;
var productCategoryApiUrl = coreERPAPI_URL_Root + "/crud/productCategory";
var acctsApiUrl = coreERPAPI_URL_Root + "/crud/GLAccount";

var productCategory = {};
var accts = {};

$(function () {
    displayLoadingDialog();
    loadForm();
});

var productCategoryAjax = $.ajax({
    url: productCategoryApiUrl + '/Get/' + productCategoryId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var accountAjax = $.ajax({
    url: acctsApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

function loadForm() {
	$.when(productCategoryAjax, accountAjax)
        .done(function (dataProductCategory, dataAccount) {
            productCategory = dataProductCategory[2].responseJSON;
            accts = dataAccount[2].responseJSON;
            			
			prepareUi();
        });	
}



function prepareUi() {
    if (productCategory.productCategoryId > 0) {
        renderControls();
        renderGrid();
        populateUi();
        dismissLoadingDialog();
    } else {
        renderControls();
        renderGrid();
        dismissLoadingDialog();
    }

    $('#save').click(function () {
        var entityGrid = $('#productSubCategoryGrid').kendoGrid();
        var data = [entityGrid.dataSource.data()];
        var totalNumber = data.length;

        for(var i = 0; i < totalNumber; i++) {
            var currentDataItem = data[i];
            alert(JSON.Stringfy(currentDataItem));
        }

        if ($("#productCategoryName").val() == "" || $("#productCategoryName").val() == null) {
            alert("Please Fill out Product Category Name");
            $("#productCategoryName").style.borderColor.red();
        } else if ($("#cogsAccount").val() == "" || $("#cogsAccount").val() == null) {
            alert("Please Select COGS Account");
        } else if ($("#inventoryAccount").val() == "" || $("#inventoryAccount").val() == null) {
            alert("Please Select an Inventory Account");
        } else if ($("#apAccount").val() == "" || $("#apAccount").val() == null) {
            alert("Please Select an Account Payable");
        } else if ($("#arAccount").val() == "" || $("#arAccount").val() == null) {
            alert("Please Select an Account Recieveable");
        } else if ($("#incomeAccount").val() == "" || $("#incomeAccount").val() == null) {
            alert("Please Select a Income Account ");
        } else if ($("#expenseAccount").val() == "" || $("#expenseAccount").val() == null) {
            alert("Please Select an Expense Account");
        } else {
            displayLoadingDialog();
            saveProductCategory();
        }
    });
}


function renderControls() {
	$('#tabs').kendoTabStrip();
    $('#productCategoryName').width('90%').kendoMaskedTextBox();
    $('#cogsAccount').width('90%').kendoComboBox({
        dataSource: accts,
        dataValueField: 'acct_id',
        dataTextField: 'acc_name',
        filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
    });
    
    $('#inventoryAccount').width('90%').kendoComboBox({
        dataSource: accts,
        dataValueField: 'acct_id',
        dataTextField: 'acc_name',
        filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
    });
    $('#apAccount').width('90%').kendoComboBox({
        dataSource: accts,
        dataValueField: 'acct_id',
        dataTextField: 'acc_name',
        filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
    });
    $('#arAccount').width('90%').kendoComboBox({
        dataSource: accts,
        dataValueField: 'acct_id',
        dataTextField: 'acc_name',
        filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
    });
    $('#incomeAccount').width('90%').kendoComboBox({
        dataSource: accts,
        dataValueField: 'acct_id',
        dataTextField: 'acc_name',
        filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
    });
    $('#expenseAccount').width('90%').kendoComboBox({
        dataSource: accts,
        dataValueField: 'acct_id',
        dataTextField: 'acc_name',
        filter: "contains",
		highlightFirst: true,
		suggest: true,
		ignoreCase: true,
		animation: {
			close: { effects: "fadeOut zoom:out", duration: 200 },
			open: { effects: "fadeIn zoom:in", duration: 200 }
		},
		optionLabel: ''
    });
}


function renderGrid() {
    $('#productSubCategoryGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(productCategory.productSubCategories);
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
            },//transport
            schema: {
                model: {
                    id: 'productSubCategoryId',
                    fields: {
                        productCategoryId: { type: 'number', defaultValue: productCategory.productCategoryId, },
                        productSubCategoryId: { type: 'number', editTable: false, },
                        productSubCategoryName: { type: 'stirng', },
                    },//fields
                },//model
            },//schema
        },//datasource
        editable: 'popup',
        columns: [
            { field: 'productSubCategoryName', title: 'Product Sub-Cat Name', },

            { command: ['edit', 'delete'], width:200 },
        ],
        toolbar: [{ name: 'add', text: 'Add Product Sub-Category' }],
    })//#productSubCategory
}

function populateUi() {
    $('#productCategoryName').data('kendoMaskedTextBox').value(productCategory.productCategoryName);
    $('#cogsAccount').data('kendoComboBox').value(productCategory.cogsAccountId);
    $('#inventoryAccount').data('kendoComboBox').value(productCategory.inventoryAccountId);
    $('#apAccount').data('kendoComboBox').value(productCategory.apAccountId);
    $('#arAccount').data('kendoComboBox').value(productCategory.arAccountId);
    $('#incomeAccount').data('kendoComboBox').value(productCategory.incomeAccountId);
    $('#expenseAccount').data('kendoComboBox').value(productCategory.expenseAccountId);
}

function saveProductCategory() {
   retrieveValues();
    var error = validate();
    if (error != '') {
        alert('unable to save Invoice, there is an error ');
        alert(error);
    }
    else {
        retrieveValues();
        saveToServer();
    }
}

function retrieveValues() {
    productCategory.productCategoryName = $('#productCategoryName').data('kendoMaskedTextBox').value();
    productCategory.cogsAccountId = $('#cogsAccount').data('kendoComboBox').value();
    productCategory.inventoryAccountId = $('#inventoryAccount').data('kendoComboBox').value();
    productCategory.apAccountId = $('#apAccount').data('kendoComboBox').value();
    productCategory.arAccountId = $('#arAccount').data('kendoComboBox').value();
    productCategory.incomeAccountId = $('#incomeAccount').data('kendoComboBox').value();
    productCategory.expenseAccountId = $('#expenseAccount').data('kendoComboBox').value();
	productCategory.productSubCategories = $("#productSubCategoryGrid").data().kendoGrid.dataSource.view();
}

function validate() {
    return '';
}

function saveToServer() {
    displayLoadingDialog();
    $.ajax({
        url: productCategoryApiUrl + '/Post',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(productCategory),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function(data) {
        dismissLoadingDialog();
        successDialog('Product sub-Category Saved Successfuly', 'SUCCESS', function () { window.location = "/ProductManagement/ProductCategories/"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}//func saveToServer

function onProductCategoryDataEntered(data) {
    if (data.length > 1) {
        for (i = 0; i < data.length; i++) {
            var found = false;
            for (j = 0; j < productCategory.productSubCategories.length; j++) {
                if (items[i].productSubCategoryName == productCategory.productSubCategories[j].productSubCategoryName) {
                    found = true;
                    break;
                }
                if (found == false) {
                    productCategory.productSubCategories.push(items[i]);
                }

            }
        }
    }
    else {
        inventoryTransfer.inventoryTransferDetails.push(data);
    }



}
