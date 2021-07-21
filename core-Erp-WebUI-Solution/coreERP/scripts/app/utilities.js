/*
  This is a set of utility functions used in rendering the ui components
*/

// called before loading a blocking ui component to display a progress dialog
function displayLoadingDialog() {
    kendo.ui.progress($("body"), true);
}

// called after loading of ui component is done
function dismissLoadingDialog() {
    kendo.ui.progress($("body"), false);
}
 
// called for error alert
function errorAlert(errorMsg) {
    $('<span id="alert" />')
        .kendoNotification({
            stacking: "down",
            show: onShowNotification,
            button: true
        }) 
        .data('kendoNotification')
        .error(errorMsg);
}

// called for Warning alert
function warningAlert(warningMsg) {
    $('<span id="alert" />') 
        .kendoNotification({
            stacking: "down",
            show: onShowNotification,
            button: true
        }) 
        .data('kendoNotification')
        .warning(warningMsg);
}

// called for success alert
function successAlert(successMsg) {
    $('<span id="alert" />') 
        .kendoNotification({
            stacking: "down",
            show: onShowNotification,
            button: true
        }) 
        .data('kendoNotification')
        .success(successMsg);
}
 
function onShowNotification(e) {
    if (!$("." + e.sender._guid)[1]) {
        var element = e.element.parent(),
            eWidth = element.width(),
            eHeight = element.height(),
            wWidth = $(window).width(),
            wHeight = $(window).height();

        var newLeft = Math.floor(wWidth / 2 - eWidth / 2);
        var newTop = Math.floor(wHeight / 2 - eHeight / 2);

        e.element.parent().css({ top: newTop, left: newLeft });
    }
}
 
function errorDialog(errorMsg, errorTitle) {
    var strContent = '<div><img src="/images/errors/error.jpg"><span>' + errorMsg + '</span></div>';
    $('<span id="win" />')
        .kendoWindow({ 
            actions: ["Close"], 
            title: errorTitle,
            draggable: false,
            pinned: false,
            modal: true,
            width: '400px',
            height: '100px'
        }) 
        .getKendoWindow()
        .content(strContent)
        .center()
        .open() ; 
}

function successDialog(successMsg, successTitle, callback) {
    var strContent = '<div><img src="/images/errors/success.png"><span>' + successMsg + '</span></div>';
    $('<span id="win" />') 
        .kendoWindow({  
            title: successTitle,
            modal: true,
            width: '400px',
            height: '100px',  
            close: function() { 
                if (typeof callback !== 'undefined') {
                    callback();
                }
            }
        })
        .getKendoWindow()
        .content(strContent)
        .center()
        .open();
}

function smallerSuccessDialog(successMsg, successTitle, callback) {
    var strContent = '<div><img src="/images/errors/success.png"><span>' + successMsg + '</span></div>';
    $('<span id="win" />')
        .kendoWindow({
            title: successTitle,
            modal: true,
            width: '300px',
            height: '50px',
            close: function () {
                if (typeof callback !== 'undefined') {
                    callback();
                }
            }
        })
        .getKendoWindow()
        .content(strContent)
        .center()
        .open();
}
 
function warningDialog(warningMsg, warningTitle) {
    var strContent = '<div><img src="/images/errors/warning.png"><span>' + warningMsg + '</span></div>';
    $('<span id="win" />')
        .kendoWindow({ 
            title: warningTitle,
            modal: true, 
            width: '400px',
            height: '100px',

        })
        .getKendoWindow()
        .content(strContent)
        .center()
        .open();
}

function smallerWarningDialog(warningMsg, warningTitle) {
    var strContent = '<div><span>' + warningMsg + '</span></div>';
    $('<span id="win" />')
        .kendoWindow({
            title: warningTitle,
            modal: true,
            width: '300px',
            height: '50px',

        })
        .getKendoWindow()
        .content(strContent)
        .center()
        .open();
}


/**
 * 
 * @param {string} message
 * @param {string} type
 */

function ShowToast(message, type) {

    if (!type) {
        type = "error";
    }
   
    swal({
        title: type === "error" ? "Warning!" : "Successful!",
        text: message,
        icon: type === "error" ? "warning" : "success",
        dangerMode: type === "error" ? true : false
    });
}
