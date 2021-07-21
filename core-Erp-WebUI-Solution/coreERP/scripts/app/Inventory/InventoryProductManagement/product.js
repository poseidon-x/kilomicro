
//Declaration of variables 
var authToken = coreERPAPI_Token;
var productApiUrl = coreERPAPI_URL_Root + "/crud/product";
var productSubCategoryApiUrl = coreERPAPI_URL_Root + "/crud/productSubCategory";
var inventoryMethodApiUrl = coreERPAPI_URL_Root + "/crud/inventoryMethod";
var unitOfMeasurementApiUrl = coreERPAPI_URL_Root + "/crud/unitOfMeasurement";
var productMakeApiUrl = coreERPAPI_URL_Root + "/crud/productMake";
var productStatusApiUrl = coreERPAPI_URL_Root + "/crud/productStatus";

var productSubCategory = {};
var product = {};
var inventoryMethod = {};
var unitOfMeasurement = {};
var productMake = {};
var productStatus = {};

//Function to call load form function
$(function () {
    displayLoadingDialog();

    loadForm();
});

function loadForm() {
    //Ajax to fetch information from the Database through the Api controllers
    $.ajax(
    {   //Fetch data/record(s) from product and assign to product variable      
        url: productApiUrl + '/Get/' + productId,
        type: 'Get',
        contentType: 'application/json',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        product = data;
        //Fetch data/record(s) from productSubCategory table and assign to productSubCategory variable
        $.ajax({
            url: productSubCategoryApiUrl + '/Get',
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            productSubCategory = data;
            //Fetch data/record(s) from inventoryMethod table and assign to inventoryMethod variable
            $.ajax({
                url: inventoryMethodApiUrl + '/Get',
                type: 'Get',
                beforeSend: function (req) {
                    req.setRequestHeader('Authorization', "coreBearer " + authToken);
                }
            }).done(function (data) {
                inventoryMethod = data;
                //Fetch data/record(s) from unitOfMeasurement table and assign to unitOfMeasurement variable
                $.ajax({
                    url: unitOfMeasurementApiUrl + '/Get',
                    type: 'Get',
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                }).done(function (data) {
                    unitOfMeasurement = data;
                    //Fetch data/record(s) from productMake table and assign to productMake variable
                    $.ajax({
                        url: productMakeApiUrl + '/Get',
                        type: 'Get',
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }).done(function (data) {
                        productMake = data;
                        //Fetch data/record(s) from productStatus table and assign to productStatus variable
                        $.ajax({
                            url: productStatusApiUrl + '/Get',
                            type: 'Get',
                            beforeSend: function (req) {
                                req.setRequestHeader('Authorization', "coreBearer " + authToken);
                            }
                        }).done(function (data) {
                            productStatus = data;
                            //Prepare UI with our retrievd data 
                            prepareUi();

                        }).error(function (error) {
                            alert(JSON.stringify(error));
                        });//productStatus ajax call

                    }).error(function (error) {
                        alert(JSON.stringify(error));
                    });//productMake ajax call

                }).error(function (error) {
                    alert(JSON.stringify(error));
                });//unitOfMeasurement ajax call

            }).error(function (error) {
                alert(JSON.stringify(error));
            });//inventoryMethod ajax call

        }).error(function (error) {
            alert(JSON.stringify(error));
        });//productSubCategory ajax call

    }).error(function (error) {
        alert(JSON.stringify(error));
    });//product ajax call
}



function prepareUi() {
    //If productID > 0, Its an Update
    if (product.productId > 0) {
        //Call the form with controlls
        renderControls();
        //Populate the form with the specified Id ex  
        populateUi();
    } else {
        renderControls();
    }
    dismissLoadingDialog();
    //Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {
        if ($("#productSubCategoryId").val() == "" || $("#productSubCategoryId").val() == null) {
            $("#productSubCategoryId").css("border", "1px solid red");
            if ($("#productCode").val() == "" || $("#productCode").val() == null) {
                $("#productCode").css("border", "solid 1px #ff0000");
                if ($("#productName").val() == "" || $("#productName").val() == null) {
                    $("#productName").css("border", "solid 1px #ff0000");
                    if ($("#productDescription").val() == "" || $("#productDescription").val() == null) {
                        $("#productDescription").css("border", "solid 1px #ff0000");
                        if ($("#inventoryMethodId").val() == "" || $("#inventoryMethodId").val() == null) {
                            $("#inventoryMethodId").css("border", "solid 2px #ff0000");
                            if ($("#unitOfMeasurementId").val() == "" || $("#unitOfMeasurementId").val() == null) {
                                $("#unitOfMeasurementId").css("border", "solid 2px #ff0000");
                                if ($("#productMakeId").val() == "" || $("#productMakeId").val() == null) {
                                    $("#productMakeId").css("border", "solid 2px #ff0000");
                                    if ($("#productStatusId").val() == "" || $("#productStatusId").val() == null) {
                                        $("#productStatusId").css("border", "solid 2px #ff0000");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        } else {
            displayLoadingDialog();
            saveProduct();
        }
    });
}



function renderControls() {
    $("#productSubCategoryId").kendoComboBox({
        dataSource: productSubCategory,
        dataValueField: 'productSubCategoryId',
        dataTextField: 'productSubCategoryName',
        optionLabel: '',
    });

    $("#productCode").kendoMaskedTextBox();
    $("#productName").kendoMaskedTextBox();
    $("#productDescription").kendoMaskedTextBox();

    $('#inventoryMethodId').kendoComboBox({
        dataSource: inventoryMethod,
        dataValueField: 'inventoryMethodId',
        dataTextField: 'inventoryMethodName',
        optionLabel: '',
    });

    $('#unitOfMeasurementId').kendoComboBox({
        dataSource: unitOfMeasurement,
        dataValueField: 'unitOfMeasurementId',
        dataTextField: 'unitOfMeasurementName',
        optionLabel: '',
    });

    $('#productMakeId').kendoComboBox({
        dataSource: productMake,
        dataValueField: 'productMakeId',
        dataTextField: 'productMakeName',
        optionLabel: '',
    });

    $('#productStatusId').kendoComboBox({
        dataSource: productStatus,
        dataValueField: 'productStatusId',
        dataTextField: 'productStatusName',
        optionLabel: '',
    });
}


function populateUi() {

    $('#productSubCategoryId').data('kendoComboBox').value(product.productSubCategoryId);
    $('#productCode').data('kendoMaskedTextBox').value(product.productCode);
    $('#productName').data('kendoMaskedTextBox').value(product.productName);
    $('#productDescription').data('kendoMaskedTextBox').value(product.productDescription);
    $('#inventoryMethodId').data('kendoComboBox').value(product.inventoryMethodId);
    $('#unitOfMeasurementId').data('kendoComboBox').value(product.unitOfMeasurementId);
    $('#productMakeId').data('kendoComboBox').value(product.productMakeId);
    $('#productStatusId').data('kendoComboBox').value(product.productStatusId);
}


function saveProduct() {
    var error = validate();
    if (error != '') {
        alert('unable to save Product, there was an error ');
        alert(error);
    }
    else {
        retrieveValues();
        saveToServer();
    }
}

function retrieveValues() {
    product.productSubCategoryId = $('#productSubCategoryId').data('kendoComboBox').value();
    product.productCode = $('#productCode').data('kendoMaskedTextBox').value();
    product.productName = $('#productName').data('kendoMaskedTextBox').value();
    product.productDescription = $('#productDescription').data('kendoMaskedTextBox').value();
    product.inventoryMethodId = $('#inventoryMethodId').data('kendoComboBox').value();
    product.unitOfMeasurementId = $('#unitOfMeasurementId').data('kendoComboBox').value();
    product.productMakeId = $('#productMakeId').data('kendoComboBox').value();
    product.productStatusId = $('#productStatusId').data('kendoComboBox').value();

}

function validate() {
    return '';
}

function saveToServer() {
    $.ajax({
        url: productApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(product),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        successDialog('Product Saved Successfuly', 'SUCCESS', function () { window.location = "/ProductManagement/Products/"; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}//func saveToServer

