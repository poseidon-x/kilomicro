var authToken = coreERPAPI_Token;
var repaymentInterval;
var borrowingApiUrl = coreERPAPI_URL_Root + "/crud/Borrowing";
var borrowingClientApiUrl = coreERPAPI_URL_Root + "/crud/BorrowingClients";

var clients = {};
var borrowing = {};

$(function () {
	
	$.ajax(
        {
            url: borrowingClientApiUrl + '/GetDisbursedBorrowingClient',
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            clients = data;
			renderControls();

            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
	
	$("#getReport").click(function () {
		var borrowingId = $("#borrowing").data("kendoComboBox").value();
		
        if (borrowingId > 0) {
            loadReport(borrowingId);
        } else {
            warningDialog("Select Borrowing Account", 'ERROR');
        }
    });
	
});



function renderControls() {

    $("#client").width("100%")
		.kendoComboBox({
		    dataSource: clients,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientID",
		    dataTextField: "clientName",
			change: onClientChange,
			optionLabel: ""
	});

    $("#borrowing").width('100%')
		.kendoComboBox({
		    dataSource: borrowing,
		    dataValueField: "borrowingId",
		    dataTextField: "borrowingNo",
		    change: onBorrowingChange,
		    optionLabel: ""
	});
	
    dismissLoadingDialog();
}

var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
			getClientBorrowings(id);
			$("#borrowing").data("kendoComboBox").value("");
			exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Client', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    } 
}

var onBorrowingChange = function () {
    var id = $("#borrowing").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < borrowings.length; i++) {
        if (borrowings[i].borrowingId == id) {
			exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Borrowing', 'ERROR');
        $("#borrowing").data("kendoComboBox").value("");
    } 
}

function loadReport(borrowingId) {
    $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
    $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            templateUrl: '/Content/reporting/templates/telerikReportViewerTemplate-9.1.15.624.html',
            reportSource: {
                report: "coreData.Reports.Borrowing.ClientBorrowingReport, coreData",
                parameters: {
                    borrowingId: borrowingId
                },
            },
            persistSession: false
        });
}

function getClientBorrowings(id){
	
	$.ajax(
        {
            url: borrowingApiUrl + '/GetClientDisbursedBrws/' + id,
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            borrowings = data;
			$("#borrowing").data("kendoComboBox").setDataSource(borrowings);
			
            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
}
