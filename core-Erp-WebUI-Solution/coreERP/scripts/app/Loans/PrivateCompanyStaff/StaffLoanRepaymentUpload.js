"use strict";

var ui = {};
var fileName = "";
var saved = 0;
$(function () {// on page load
    displayLoadingDialog();
    ui = new uploadUI();
    ui.prepareUI();
});
var uploadUI = (function () {//user interface object
    function uploadUI() {
    }
    uploadUI.prototype.prepareUI = function () {
        $('#repaymentFile').kendoUpload({// uploag form element
            localization: {
                select: "Select the File",
            },
            async: {
                autoUpload: true,
                saveUrl: coreER_Url_Root + '/FileUpload/SaveLoanRepaymentList?token=' + authToken,
                removeUrl: coreER_Url_Root + '/FileUpload/RemoveLoanRepaymentList?token=' + authToken,
            },
            success: function (e) {
                if (e.operation == "upload") {
                    fileName = e.response;
                    successAlert("File uploaded successfuly. Ready to save repayment List.");
                }
            },
            multiple: false,
        });
        $('#save').click(function () {//trigger excel uploaded file processing.
            saveRepaymentData();
        });
        dismissLoadingDialog();
    };
    return uploadUI;
})();

function saveRepaymentData() {
	
	var file = {};
	file.fileName = fileName
    displayLoadingDialog();
    $.ajax({
        url: coreER_Url_Root + "/FileUpload/SaveUploadedLoanRepaymentList",
        type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify(file),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        dismissLoadingDialog();
        successAlert(JSON.stringify(data) + ' Loan repayment upload completed.');
        window.location = '/Asets/Asets';
    }).error(function (error) {
        errorDialog("Some records are not valid.<br>Please correct the file and try again.<br>", "Error While Processing the File.");
        dismissLoadingDialog();
    });
}