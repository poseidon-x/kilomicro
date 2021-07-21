//***********************************************************//
//  	     CREDIT MEMO - JAVASCRIPT                
// 		CREATOR: EMMANUEL OWUSU(MAN)    	   
//		      DATE: AUG(4TH), 2015  		  
//*********************************************************//


//"use strict"


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var loanAdditionalInfoApiUrl = coreERPAPI_URL_Root + "/crud/LoanAdditionalInfo";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/LoanClient";
var loansApiUrl = coreERPAPI_URL_Root + "/crud/ClientLoan";
var metaDataTypeApiUrl = coreERPAPI_URL_Root + "/crud/MetaDataType";




//Declaration of variables to store records retrieved from the database
var loanAdditionalInfo = {};
var clients = {};
var loans = {};
var metaDataTypes = {};




var clientAjax = $.ajax({
    url: clientApiUrl + '/GetAllLoanClients',
        type: 'Get',
        beforeSend: function (req) {
           req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
});

var metaDataTypeAjax = $.ajax({
    url: metaDataTypeApiUrl + '/Get',
        type: 'Get',
        beforeSend: function (req) {
           req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
});

function loadForm() {
    $.when(clientAjax, metaDataTypeAjax)
        .done(function (dataClient, dataMetaDataType) {
            clients = dataClient[2].responseJSON;
            metaDataTypes = dataMetaDataType[2].responseJSON;
			
			prepareUi();
			dismissLoadingDialog();
        });
}

//Function to call load form function
$(function () {
    displayLoadingDialog();
    loadForm();
});

//Function to prepare user interface
function prepareUi() 
{		
    renderControls();

    $('#save').click(function (event) {

        var validator = $("#myform").kendoValidator().data("kendoValidator");
        var gridData = $("#grid").data().kendoGrid.dataSource.view();


		//if (!validator.validate()) {
        //    smallerWarningDialog('A form input is empty or has invalid value', 'ERROR');
    //}
    //else {
			if (confirm('Are you sure you want Save Loan Additional Info?')) {
                displayLoadingDialog();
				loanAdditionalInfo.loanMetaDatas = [];
                saveGridData(gridData);

				SaveLoanAdditional();				
            } else {
                smallerWarningDialog('Please review and save later', 'NOTE');
            }
		//}
	});
}

function saveGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            loanAdditionalInfo.loanMetaDatas.push(data[i]);
        }
    }
    else {
	loanAdditionalInfo.loanMetaDatas.push(data[0]);
	}
}

//Apply kendo Style to the input fields
function renderControls() {
    $("#client").width("75%")
		.kendoComboBox({
		    dataSource: clients,
		    filter: "contains",
		    suggest: true,
		    dataValueField: "clientID",
		    dataTextField: "clientName",
			change: onClientChange,
			optionLabel: ""
	});
		
    $("#loan").width('75%')
		.kendoComboBox({
		    dataSource: loans,
		    dataValueField: "loanID",
		    dataTextField: "loanNo",
		    change: onLoanChange,
		    optionLabel: ""
	});		
}


var onClientChange = function () {
    var id = $("#client").data("kendoComboBox").value();
    var exist = false;
	
	//Retrieve value enter validate
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].clientID == id) {
            exist = true;
            break;
        }
    }
	
    if (!exist) {
        warningDialog('Invalid Cliet', 'ERROR');
        $("#client").data("kendoComboBox").value("");
    } else {
        displayLoadingDialog();
				
		
        $.ajax(
        {
            url: loansApiUrl + '/GetClientLoans/' + id,
            type: 'Get',
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            loans = data;
			document.getElementById('grid').innerHTML ='';

            //Set the returned data to loan datasource
			$("#loan").data("kendoComboBox").value("");
            $("#loan").data("kendoComboBox").setDataSource(loans);
            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
    }
}

var onLoanChange = function () {
    var id = $("#loan").data("kendoComboBox").value();
    var exist = false;	

    //Retrieve value enter validate
    for (var i = 0; i < loans.length; i++) {
        if (loans[i].loanID == id) {	
		
		displayLoadingDialog();
		$.ajax(
        {
            url: loanAdditionalInfoApiUrl + '/Get/' + id,
			type: 'Get',
			beforeSend: function (req) {
				req.setRequestHeader('Authorization', "coreBearer " + authToken);
			}
        }).done(function (data) {
            loanAdditionalInfo = data;
			
			$('#tabs').kendoTabStrip();
			renderGrid();
	
            dismissLoadingDialog();
        }).error(function (error) {
            alert(JSON.stringify(error));
        });
		    
            exist = true;
            break;
        }
    }

    if (!exist) {
        warningDialog('Invalid Loan', 'ERROR');
        $("#loan").data("kendoComboBox").value("");
    }
}

//render Grid
function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport:  {
                read: function(entries) {
                    entries.success(loanAdditionalInfo.loanMetaDatas);
                },
                create: function(entries) {
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
                    entries.success();
                }
            }, //transport
            schema: {
                model: {
                    id: 'loanMetaDataId',
                    fields: {
                        loanAdditionalInfoId: { type: 'number', defaultValue: loanAdditionalInfo.loanAdditionalInfoId },
                        metaDataTypeId: { editable: true  },
                        content: { type: 'string' }
						}, //fields
                }, //model
            }, //schema
        }, //datasource
		editable: 'popup',
        columns: [
			{ field: 'metaDataTypeId', title: 'Description', editor: metaDataTypeEditor, template: '#= getMetaDataType(metaDataTypeId) #'  },
            { field: 'content', title: 'Value', editor: contentEditor },
            { command: ['edit'] }
       ],
	    toolbar: [{ name: 'create', text: 'Add New Info' }]

    });
}


//retrieve values from from Input Fields and save 
function SaveLoanAdditional() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    loanAdditionalInfo.loanId = $('#loan').data('kendoComboBox').value();
}

//Save to server function
function saveToServer() {
	var type = "";
	if(loanAdditionalInfo.loanAdditionalInfoId > 0){type = "Put";}
	else {type="Post";}
	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: loanAdditionalInfoApiUrl + '/' + type,
        type: type,
        contentType: 'application/json',
        data: JSON.stringify(loanAdditionalInfo),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Loan Additional Information Saved Successfully:', 'SUCCESS', function () { window.location = '/dash/home.aspx'; });        
    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR');
    });
}//func saveToServer

function metaDataTypeEditor(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoComboBox({
        dataSource: metaDataTypes,
        dataValueField: "metaDataTypeId",
        dataTextField: "name",
        optionLabel: ""
    });
}

function contentEditor(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("75%")
    .kendoMaskedTextBox();
}

function getMetaDataType(id) {
    for (var i = 0; i < metaDataTypes.length; i++) {
        if (metaDataTypes[i].metaDataTypeId == id)
            return metaDataTypes[i].name;
    }
}
