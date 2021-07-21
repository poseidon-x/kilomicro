'use strict';

var depositCertificateConfigApiUrl = coreERPAPI_URL_Root + "/crud/depositCertificateConfig";


var depositCertConfig = {};

function loadData() {
	displayLoadingDialog();
    $.ajax({
        url: depositCertificateConfigApiUrl + "/Get/"+id,
        type: 'GET',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		depositCertConfig = data;
		var ui = new oUI();
		ui.prepareUI();
		if(depositCertConfig.depositCertificateConfigId>0)
		{
			ui.populateUi();
		}
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}


$(function () {
    displayLoadingDialog();
    loadData();
});

var oUI = (function () {
    function oUI() {
    }
    oUI.prototype.prepareUI = function () {
				
        $('#earlyRedemption').width('90%').kendoEditor({ resizable: {
			content: true,
			toolbar: true
		}});
        $('#trust').width('90%').kendoEditor({ resizable: {
			content: true,
			toolbar: true
		}});
        $('#authority').width('90%').kendoEditor({ resizable: {
			content: true,
			toolbar: true
		}});
        $('#riskDisclosure').width('90%').kendoEditor({ resizable: {
			content: true,
			toolbar: true
		}});
		
        $('#save').click(function () {
            saveConfig();
        });
    } 
	
	oUI.prototype.populateUi = function () {
				
        $('#earlyRedemption').data("kendoEditor").value(depositCertConfig.earlyRedemptionText);
        $('#trust').data("kendoEditor").value(depositCertConfig.trustText);
        $('#authority').data("kendoEditor").value(depositCertConfig.authorityText);
        $('#riskDisclosure').data("kendoEditor").value(depositCertConfig.riskDisclosureText);
    } 

    return oUI;
})();


function populateUI() {
    $('#deposit').data('kendoComboBox').value(deposit.depositID);
    $('#balance').data('kendoNumericTextBox').value(deposit.principalBalance+deposit.interestBalance);
}
function retrieveValues() {
    depositCertConfig.earlyRedemptionText = $('#earlyRedemption').data('kendoEditor').value();
    depositCertConfig.trustText = $('#trust').data('kendoEditor').value();
    depositCertConfig.authorityText = $('#authority').data('kendoEditor').value();	
    depositCertConfig.riskDisclosureText = $('#riskDisclosure').data('kendoEditor').value();
}

function saveConfig() {

    var validator = $("#myform").kendoValidator().data("kendoValidator");
    if (!validator.validate()) {
        alert('One or More Fields are Empty');
    } else {
        displayLoadingDialog();
        retrieveValues();
        saveToServer();
    }
}

function saveToServer() {
    displayLoadingDialog();
    $.ajax({
        url: depositCertificateConfigApiUrl + "/Post",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(depositCertConfig),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		successDialog('Configuration successfully saved',
            'SUCCESS', function () { window.location = '/dash/home.aspx'; });
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}




