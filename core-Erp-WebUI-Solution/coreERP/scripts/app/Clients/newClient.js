/*
//*******************************************
//***   NEW CLIENT JS              
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: AUG 28TH, 2015  	
// MODIFIED BY: SAMUEL WENDOLIN KETECHIE
// MODIFIED DATE: DEC 20, 2020
//*******************************************
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed

"use strict";
var authToken = coreERPAPI_Token;
var clientApiUrl = coreERPAPI_URL_Root + "/crud/client";
var maritalStatusApiUrl = coreERPAPI_URL_Root + "/crud/MaritalStatus";
var idTypeApiUrl = coreERPAPI_URL_Root + "/crud/IdType";
var clientTypeApiUrl = coreERPAPI_URL_Root + "/crud/ClientType";
var clientCategoryApiUrl = coreERPAPI_URL_Root + "/crud/ClientCategory";
var industryApiUrl = coreERPAPI_URL_Root + "/crud/Industry";
var industryApiUrl = coreERPAPI_URL_Root + "/crud/Industry";
var branchApiUrl = coreERPAPI_URL_Root + "/crud/Branch";
var addressTypeApiUrl = coreERPAPI_URL_Root + "/crud/AddressType";
var phoneTypeApiUrl = coreERPAPI_URL_Root + "/crud/PhoneType";
var emailTypeApiUrl = coreERPAPI_URL_Root + "/crud/EmailType";
var sectorApiUrl = coreERPAPI_URL_Root + "/crud/sector";
var loanGroupApiUrl = coreERPAPI_URL_Root + "/crud/LoanGroup";



var client = {};
var clientAddress = {};
var clientPhone = {};
var clientEmail = {};
var maritalStatus = {};
var idTypes = {};
var clientTypes = {};
var clientCategories = {};
var industries = {};
var branches = {};
var addressTypes = {};
var phoneTypes = {};
var emailTypes = {};
var errorMessage = "";
var sectors = {};
var currentModel = {};
var nokPicture = "";
var loanGroups = {};
var phones = [];

var minClientDateOfBirth = new Date();
minClientDateOfBirth.setFullYear(minClientDateOfBirth.getFullYear() - 18);
var minIdExpiryDate = new Date();
minIdExpiryDate.setMonth(minIdExpiryDate.getMonth() + 3);


////Declare a variable and store client table ajax call in it
//var clientAjax = $.ajax({
//    //url: clientApiUrl + '/Get/' + clientId,
//    //type: 'Get',
//    //contentType: 'application/json',
//    //beforeSend: function(req) {
//    //    req.setRequestHeader('Authorization', "coreBearer " + authToken);
//    //}
//});

//Declare a variable and store marital status table ajax call in it
var maritalStatusAjax = $.ajax({
    url: maritalStatusApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store id type table ajax call in it
var idTypeAjax = $.ajax({
    url: idTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store client type table ajax call in it
var clientTypeAjax = $.ajax({
    url: clientTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store client category table ajax call in it
var clientCategoryAjax = $.ajax({
    url: clientCategoryApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store industry table ajax call in it
var industryAjax = $.ajax({
    url: industryApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store branch table ajax call in it
var branchAjax = $.ajax({
    url: branchApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store address type table ajax call in it
var addressTypeAjax = $.ajax({
    url: addressTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store phone type table ajax call in it
var phoneTypeAjax = $.ajax({
    url: phoneTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store email type table ajax call in it
var emailTypeAjax = $.ajax({
    url: emailTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var sectorAjax = $.ajax({
    url: sectorApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//Declare a variable and store Loan Group table ajax call in it
var loanGroupAjax = $.ajax({
    url: loanGroupApiUrl + "/Get",
    type: "Get",
    beforeSend: function(req) {
        req.setRequestHeader("Authorization", "coreBearer " + authToken);
    }
});


//Load page data
function loadData() {
    $.when(maritalStatusAjax, idTypeAjax, clientTypeAjax, clientCategoryAjax, industryAjax, branchAjax,
		addressTypeAjax, phoneTypeAjax, emailTypeAjax, sectorAjax,loanGroupAjax)
        .done(function (dataMaritalStatus, dataIdType, dataClientType, dataClientCategory, dataIndustry, dataBranch,
			dataAddressType, dataPhoneType, dataEmailType, dataSector,dataLoanGroup) {
            maritalStatus = dataMaritalStatus[2].responseJSON;
            idTypes = dataIdType[2].responseJSON;
            clientTypes = dataClientType[2].responseJSON;
            clientCategories = dataClientCategory[2].responseJSON;
            industries = dataIndustry[2].responseJSON;
            branches = dataBranch[2].responseJSON;
            addressTypes = dataAddressType[2].responseJSON;
            phoneTypes = dataPhoneType[2].responseJSON;
            emailTypes = dataEmailType[2].responseJSON;
            sectors = dataSector[2].responseJSON;
            loanGroups = dataLoanGroup[2].responseJSON;
            dismissLoadingDialog();
            //Prepares UI
            var ui = new clientUI();
            ui.prepareUi();
        }
	);
}
$(function () {
    displayLoadingDialog();
    loadData();
});

var clientUI = (function () {
    function clientUI() {
    }
    clientUI.prototype.prepareUi = function () {
        this.renderControls();
    }
    clientUI.prototype.renderControls = function () {
        $("#tabs").kendoTabStrip();
        $("#surName").width('90%').kendoMaskedTextBox();
        $("#otherNames").width('90%').kendoMaskedTextBox();
        $("#DOB").width('90%').kendoDatePicker({
            format: '{0:dd-MMM-yyyy}',
            parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
            max: minClientDateOfBirth
        });
        $("#clientType").width("90%")
		.kendoComboBox({
		    dataSource: clientTypes,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientTypeId",
		    dataTextField: "clientTypeName",
		    highlightFirst: true,
		    ignoreCase: true,
		    //change: onClientChange,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});
        $("#maritalStatus").width("90%")
		.kendoComboBox({
		    dataSource: maritalStatus,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "maritalStatusID",
		    dataTextField: "maritalStatusName",
		    highlightFirst: true,
		    ignoreCase: true,
		    //change: onClientChange,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});
        $("#primaryIdType").width("90%")
		.kendoComboBox({
		    dataSource: idTypes,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "idTypeId",
		    dataTextField: "idTypeName",
		    highlightFirst: true,
		    ignoreCase: true,
		    //change: onClientChange,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});
        $("#primaryIdNo").width('90%').kendoMaskedTextBox();
        $("#primaryIdExpiry").width('90%').kendoDatePicker({
            format: '{0:dd-MMM-yyyy}',
            parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
            min: minIdExpiryDate
        });
        $("#secondaryIdType").width("90%")
		.kendoComboBox({
		    dataSource: idTypes,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "idTypeId",
		    dataTextField: "idTypeName",
		    highlightFirst: true,
		    ignoreCase: true,
		    //change: onClientChange,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});
        $("#secondaryIdNo").width('90%').kendoMaskedTextBox();
        $("#secondaryIdExpiry").width('90%').kendoDatePicker({
            format: '{0:dd-MMM-yyyy}',
            parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
            min: minIdExpiryDate
        });

        $("#branch").width("90%")
		.kendoComboBox({
		    dataSource: branches,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "branchID",
		    dataTextField: "branchName",
		    highlightFirst: true,
		    ignoreCase: true,
		    //change: onClientChange,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});
        $("#industry").width("90%")
		.kendoComboBox({
		    dataSource: industries,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "industryID",
		    dataTextField: "industryName",
		    highlightFirst: true,
		    ignoreCase: true,
		    //change: onClientChange,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});
        $("#clientCategory").width("90%")
		.kendoComboBox({
		    dataSource: clientCategories,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "categoryID",
		    dataTextField: "categoryName",
		    highlightFirst: true,
		    ignoreCase: true,
		    //change: onClientChange,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});
        $("#sector").width("90%")
		.kendoComboBox({
		    dataSource: sectors,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "sectorID",
		    dataTextField: "sectorName",
		    highlightFirst: true,
		    ignoreCase: true,
		    //change: onClientChange,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});

        //loan group dropdown
        $("#group").width("90%")
		.kendoComboBox({
		    dataSource: loanGroups,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "loanGroupId",
		    dataTextField: "loanGroupName",
		    highlightFirst: true,
		    ignoreCase: true,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});
        

        $("#city").width('90%').kendoMaskedTextBox();
        $("#landmark").width('90%').kendoMaskedTextBox();
        $("#mailingCity").width('90%').kendoMaskedTextBox();

        //NEXT OF KIN PRERENDER
        $("#nokIdType").width("90%")
		.kendoComboBox({
		    dataSource: idTypes,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "idTypeId",
		    dataTextField: "idTypeName",
		    highlightFirst: true,
		    ignoreCase: true,
		    //change: onClientChange,
		    optionLabel: "",
		    animation: {
		        close: { effects: "fadeOut zoom:out", duration: 300 },
		        open: { effects: "fadeIn zoom:in", duration: 300 }
		    }
		});

        $("#nokSurname").width('90%').kendoMaskedTextBox();
        $("#nokEmail").width('90%').kendoMaskedTextBox();
        $("#nokOthername").width('90%').kendoMaskedTextBox();
        $("#nokPhone").width('90%').kendoMaskedTextBox();
        $("#nokIdNumber").width('90%').kendoMaskedTextBox();
        $("#nokRelation").width('90%').kendoMaskedTextBox();

        //Client Phone
        $("#mobilePhone").width('90%').kendoMaskedTextBox();
        $("#workPhone").width('90%').kendoMaskedTextBox();
        $("#homePhone").width('90%').kendoMaskedTextBox();

        // Client Email
        $("#personalEmail").width('90%').kendoMaskedTextBox();
        $("#officeEmail").width('90%').kendoMaskedTextBox();

        //Supporting Docs


    }

    return clientUI;
})();




$("#save").click(function (e) {
    e.preventDefault();
    bootbox.confirm({
        title: "Confirm ?",
        message: "Are you sure you want to save this Client",
        callback: function (confirmed) {
            if (confirmed) {
                displayLoadingDialog();
                saveClient();
            }
        }
    });
    
});


function saveClient() {
    var file = document.querySelector("#nokPicture").files[0];
    if (file) {
        getBase64File(file).then(function (dataFile) {
            //Remove the "data:.."
            nokPicture = dataFile.substr(dataFile.indexOf(',') + 1);

            //retrieve other values
            retrieveValues();
            if (validateInput()) {
                saveToServer();
            } else {
                dismissLoadingDialog();
                ShowToast(errorMessage, "error");
            }

        });
    } else {
        retrieveValues();
        if (validateInput()) {
            saveToServer();

        } else {
            dismissLoadingDialog();
            ShowToast(errorMessage, "error");
        }
    }
    
    dismissLoadingDialog();

}



function retrieveValues() {
    //Basic Info
    client.surName = $("#surName").data("kendoMaskedTextBox").value();
    client.otherNames = $("#otherNames").data("kendoMaskedTextBox").value();
    client.dateOfBirth = $("#DOB").data("kendoDatePicker").value();
    client.maritalStatusId = parseInt($("#maritalStatus").val());
    client.industryId = parseInt($("#industry").val());
    client.branchId = parseInt($("#branch").val());
    client.sectorId = parseInt($("#sector").val());
    client.clientTypeId = parseInt($("#clientType").val());
    client.clientCategoryId = parseInt($("#clientCategory").val());
    client.primaryIdTypeId = parseInt($('#primaryIdType').val());
    client.primaryIdNo = $('#primaryIdNo').data("kendoMaskedTextBox").value();
    client.primaryIdExpiry = $('#primaryIdExpiry').data("kendoDatePicker").value();
    client.secondaryIdTypeId = parseInt($("#secondaryIdType").val());
    client.secondaryIdNo = $("#secondaryIdNo").data("kendoMaskedTextBox").value();
    client.secondaryIdExpiry = $("#secondaryIdExpiry").data("kendoDatePicker").value();
    client.loanGroupId =parseInt($("#group").val());
    

    var clnImage = $("#clientImage").attr("src");
    client.imageDataString = clnImage.substr(clnImage.indexOf(',') + 1);

    var selected = $("input[type='radio'][name='sex']:checked");
    client.sex = selected.val() == "male" ? "M" : (selected.val()=="female" ? "F" : "" );


    client.clientAddress = {
        addressLine: $("#physicaladdress").val(),
        landMark: $("#landmark").data("kendoMaskedTextBox").value(),
        city: $("#city").data("kendoMaskedTextBox").value(),
    };
    client.mailingAddress = {
        addressLine1: $("#mailingPhysicaladdress1").val(),
        addressLine2: $("#mailingPhysicaladdress2").val(),
        city: $("#mailingCity").data("kendoMaskedTextBox").value(),
    };
    //Phones
    client.clientPhones = retrievePhones();

    //Emails
    var emails = retrieveEmails();
    client.clientEmails = emails;

    //Next of Kin
    var nokInfo = retrieveNOKInfo();
    client.nextOfKin = nokInfo;


    //client.supportingDocuments = $('#documents').data().kendoGrid.dataSource.view();

    console.log(client);
}


function getBase64File(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () { resolve(reader.result) };
        reader.onerror = function (error)
        { reject(error) }
    });
}



function retrieveNOKInfo() {
    var nokSurname = $("#nokSurname").val();
    var nokEmail = $("#nokEmail").val();
    var nokIdTypeId =parseInt($("#nokIdType").val());
    var nokOthername = $("#nokOthername").val();
    var nokPhone = $("#nokPhone").val();
    var nokIdNumber = $("#nokIdNumber").val();
    var nokRelation = $("#nokRelation").val();
    
   

    
    var nextOfKin = {
        emailAddress: nokEmail,
        idNumber: nokIdNumber,
        idNoTypeID: nokIdTypeId,
        imageDataString: nokPicture,
        phoneNumber: nokPhone,
        otherNames: nokOthername,
        relationship: nokRelation,
        surName: nokSurname
    }

    return nextOfKin;
}


function validateInput() {
    errorMessage = "";
    
    if (!client.surName) {
        errorMessage = errorMessage + "Surname cannot be empty\n";
    }
    if (!client.otherNames) {
        errorMessage = errorMessage + "OtherNames cannot be empty\n";
    }

    if (!client.dateOfBirth) {
        errorMessage = errorMessage + "Date of Birth cannot be empty\n";
    }

    if (!client.maritalStatusId) {
        errorMessage = errorMessage + "Invalid marital status\n";
    }
    if (!client.industryId) {
        errorMessage = errorMessage + "Invalid industry\n";
    }
    if (!client.branchId) {
        errorMessage = errorMessage + "Invalid branch\n";
    }
    if (!client.sectorId) {
        errorMessage = errorMessage + "Invalid sector\n";
    }
    if (!client.clientTypeId) {
        errorMessage = errorMessage + "Invalid client type\n";
    }
    if (parseInt(client.clientCategoryId) < 0) {
        errorMessage = errorMessage + "Invalid client category \n";
    }
    if (!client.primaryIdTypeId) {
        errorMessage = errorMessage + "Invalid primary ID type\n";
    }
    if (!client.primaryIdNo) {
        errorMessage = errorMessage + "Primary ID Number cannot be empty\n";
    }
    if (!client.sex) {
        errorMessage = errorMessage + "Client gender(sex) cannot be empty\n";
    }

    if (!client.nextOfKin) {
        errorMessage = errorMessage + "Please provide Next Of Kin\n";
    }
    if (!client.nextOfKin.surName && !client.nextOfKin.otherNames) {
        errorMessage = errorMessage + "Next of Kin names cannot be empty\n";
    }


    if (!client.nextOfKin.idNoTypeID && !client.nextOfKin.idNumber) {
        //errorMessage = errorMessage + "Next of Kin ID info cannot be empty\n";
    }
    if (!client.nextOfKin.phoneNumber) {
        errorMessage = errorMessage + "Next of Kin Phone Number cannot be empty\n";
    }
    if (!client.nextOfKin.relationship) {
        errorMessage = errorMessage + "Next of Kin Relation cannot be empty\n";
    }
    if (!client.imageDataString) {
        //errorMessage = errorMessage + "Client Image cannot be empty\n";
    }

    if (!client.clientPhones) {
        errorMessage = errorMessage + "Please add a phone number\n";
    }

    if (client.clientPhones.length < 1) {
        errorMessage = errorMessage + "Please add a phone number\n";
    }

    

    if (!client.clientAddress) {
        errorMessage = errorMessage + "Please provide address information\n";
    }

    if (!client.clientAddress.city) {
        errorMessage = errorMessage + "Please provide city in address information\n";
    }

    if (!client.clientAddress.addressLine) {
        errorMessage = errorMessage + "Please provide residencial address information\n";
    }

    if (!client.clientAddress.landMark) {
        errorMessage = errorMessage + "Please provide a landmark in address information\n";
    }

    if (!client.loanGroupId) {
        errorMessage = errorMessage + "Please select a Loan Group\n";
    }

    if (phones.length < 1) {
        errorMessage = errorMessage + "Please add client phone number\n";
    }

    if (phones.length > 0) {
        for (var i = 0; i < phones.length ; i++) {
            if (!phones[i].phoneNumber) {
                errorMessage = errorMessage + "Please add client phone number\n";
            }

            if (phones[i].phoneNumber.length < 10 || phones[i].phoneNumber.length > 12) {
                errorMessage = errorMessage + "Client phone number must be 10 to 12 digits long (eg. 233xxxxxxxxx or 024xxxxxxx)\n";
            }
        }
    }

    if (client.nextOfKin.phoneNumber.length < 10 || client.nextOfKin.phoneNumber.length > 12) {
        errorMessage = errorMessage + "Next of Kin Phone Number must be 10 to 12 digits long (eg. 233xxxxxxxxx or 024xxxxxxx)\n";
    }
    //TODO: Validate phone numbers here

    if (errorMessage) {
        ShowToast(errorMessage, "error");
        return false;
    }
    else return true;

}


function retrieveEmails() {
    var emails = [];
    var personalEmail = $("#personalEmail").val();
    var officeEmail = $("#officeEmail").val();

    if (personalEmail) {
        emails.push({
            emailTypeId: 2,
            emailAddress: $("#personalEmail").val()
        });
    }

    if (officeEmail) {
        emails.push({
            emailTypeId: 1,
            emailAddress: $("#officeEmail").val()
        });
    }

    return emails;
}

function retrievePhones() {
    var workPhone = $("#workPhone").val();
    var mobilePhone = $("#mobilePhone").val();
    var homePhone = $("#homePhone").val();

    if (workPhone) {
        phones.push({
            phoneTypeId: 1,
            phoneNumber: $("#workPhone").val()
        });
    }

    if (mobilePhone) {
        phones.push({
            phoneTypeId: 2,
            phoneNumber: $("#mobilePhone").val()
        });
    }

    if (homePhone) {
        phones.push({
            phoneTypeId: 3,
            phoneNumber: $("#homePhone").val()
        });
    }

    return phones;

}


function saveToServer() {
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + "/SaveNewClient",
        type: "Post",
        contentType: "application/json",
        data: JSON.stringify(client),
        beforeSend: function (req) {
            req.setRequestHeader("Authorization", "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        if (data !== null) {
            ShowToast("Client Account Successfully Created.\n\nAccount Number: ".concat(data.accountNumber), "success");
            setTimeout(function () {
                window.location = "/ln/client/default.aspx?catID=".concat(data.categoryID);
            }, 6500);

        } else {
            ShowToast("Failed to save client data. Please check and try again.", "error");
            
        }
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        ShowToast(xhr.responseJSON.ExceptionMessage, "error");

       // warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}//func saveToServer


function resetControls() {
    //var clnt = $("#client").data("kendoComboBox");
    //clnt.setDataSource(filteredClients);
    var ui = new clientUI();
    ui.renderControls();
    $("#details").show();
}


//WebCam Image capture
var webcamElement = document.getElementById('webcam');
var canvasElement = document.getElementById('canvas');
var snapSoundElement = document.getElementById('snapSound');
var webcam = new Webcam(webcamElement, 'user', canvasElement, snapSoundElement);

var initialImage = "data:image/jpeg;base64," +
    "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";
$("#clientImage").attr("src", initialImage);

$("#useCam").click(function () {
    $("#snapCam").attr("disabled", false);
    $("#cameraFlip").attr("disabled", false);
    $("#clientImageChoose").attr("disabled", true);
    invokeWebCam();
});

$("#snapCam").click(function () {
    snapPicture();
});

function invokeWebCam() {
    webcam.start()
   .then(result => {
       console.log("webcam started");
   })
   .catch(err => {
       console.log(err);
   });

}


function snapPicture() {
    var picture = webcam.snap();
    $("#clientImage").attr("src", picture);
    webcam.stop();
    $("#clientImageChoose").attr("disabled", false);
    $("#clientImageChoose").val("");
}

$("#cameraFlip").click(function () {
    webcam.flip();
    webcam.start();
});

$("#clientImageChoose").click(function () {
    $("#snapCam").attr("disabled", true);
    $("#cameraFlip").attr("disabled", true);
});


$("#clientImageChoose").change(function (e) {

    var file = e.target.files[0];
    if (file) {
        getBase64File(file).then(function (dataFile) {
            $("#clientImage").attr("src", dataFile);
        });
    }

});




function showLoader() {
    var spinDialog = bootbox.dialog({
        message: '<p class="text-center mb-0"><i class="fa fa-spin fa-cog"></i> Loading... </p>',
        closeButton: false,
        centerVertical: true
    });

    return spinDialog;
}

