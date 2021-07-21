/*
UI Scripts for Loan savingType Management
Creator: man@acsghana.com
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
var currentDocumentContent='';
var currentDocumentContents=[];

var minClientDateOfBirth = new Date();
minClientDateOfBirth.setFullYear(minClientDateOfBirth.getFullYear() - 18 );
var minIdExpiryDate = new Date();
minIdExpiryDate.setMonth(minIdExpiryDate.getMonth() + 3 );


//Declare a variable and store client table ajax call in it
var clientAjax = $.ajax({
    url: clientApiUrl + '/Get/' + clientId,
    type: 'Get',
    contentType: 'application/json',
    beforeSend: function(req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

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
//Load page data
function loadData() {	
    $.when(clientAjax, maritalStatusAjax, idTypeAjax, clientTypeAjax, clientCategoryAjax, industryAjax, branchAjax, 
		addressTypeAjax, phoneTypeAjax, emailTypeAjax,sectorAjax)
        .done(function (dataClient, dataMaritalStatus, dataIdType, dataClientType, dataClientCategory, dataIndustry, dataBranch, 
			dataAddressType, dataPhoneType, dataEmailType, dataSector) {
            client = dataClient[2].responseJSON;
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

var clientUI = (function(){
	function clientUI(){	
	}
	clientUI.prototype.prepareUi = function(){
		this.renderControls();
		$('#save').click(function (event) {
			if (confirm("Are you sure you want to Save Client")) {
				displayLoadingDialog();
				saveClient();				
			} else
			{
				smallerWarningDialog('Please review and save later', 'NOTE');
			}			
		});	

		$('input[type=radio][name=sex]').change(function() {
        if (this.value == 'male') {
			client.sex = 'M';
        }
        else if (this.value == 'female') {
			client.sex = 'F';
        }
	});
	}
	clientUI.prototype.renderControls = function(){
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
		$("#city").width('90%').kendoMaskedTextBox();
		$("#landmark").width('90%').kendoMaskedTextBox();
		$("#mailingCity").width('90%').kendoMaskedTextBox();
		
		this.renderPhoneGrid();
		this.renderEmailGrid();
		this.renderDocumentsGrid();
	}
	clientUI.prototype.renderPhoneGrid = function(){				
		$('#phones').kendoGrid({
			dataSource: {
					transport: {
						read: function (entries) {
							entries.success(client.clientPhones);                    
						},
						create: function (entries) {
							entries.success(entries.data);
						},
						update: function (entries) {
							entries.success(entries.data);
						},
						destroy: function (entries) {
							entries.success(entries.data);
						},
						parameterMap: function (data) { return JSON.stringify(data); },
					},
					pageSize: 10,
					schema: {
						model: {
						id: 'clientId',
						fields: {
							clientId: { editable: false,type: 'number' },
							phoneId: { editable: false, validation: { required: true } },
							phoneTypeId: { editable: true, validation: { required: true } },
							phoneNumber: { editable: true, validation: { required: true } },
						} //fields
					},
				},
			},
			scrollable: false,
			sortable: true,
			columns: [  
				{ field: 'phoneTypeId', title: 'Phone. Type',editor: phoneTypeEditor,template: '#= getPhoneType(phoneTypeId) #' },  
				{ field: 'phoneNumer', title: 'Phone',editor: phoneEditor },          
				{ command: ["edit","destroy"],width:210 }
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5,
			},
			toolbar: [
				{
					name: "create",
					text: 'Add Phone',
				}
			],
			editable: "popup",
			edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
				editWindow.wrapper.css({ width: 500 });
                editWindow.title("Client Phone");
            },
			mobile: true
		});		
	}
	clientUI.prototype.renderEmailGrid = function(){				
		$('#email').kendoGrid({
			dataSource: {
					transport: {
						read: function (entries) {
							entries.success(client.clientEmails);                    
						},
						create: function (entries) {
							entries.success(entries.data);
						},
						update: function (entries) {
							entries.success(entries.data);
						},
						destroy: function (entries) {
							entries.success(entries.data);
						},
						parameterMap: function (data) { return JSON.stringify(data); },
					},
					pageSize: 10,
					schema: {
						model: {
						id: 'clientId',
						fields: {
							clientId: { editable: false,type: 'number' },
							emailId: { editable: false, validation: { required: true } },
							emailTypeId: { editable: true, validation: { required: true } },
							emailAddress: { editable: true, validation: { required: true } },
						} //fields
					},
				},
			},
			scrollable: false,
			sortable: true,
			columns: [  
				{ field: 'emailTypeId', title: 'Email Type',editor: emailTypeEditor,template: '#= getEmailType(emailTypeId) #' },  
				{ field: 'emailAddress', title: 'Email',editor: emailEditor },          
				{ command: ["edit","destroy"],width:210 }
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5,
			},
			toolbar: [
				{
					name: "create",
					text: 'Add Email',
				}
			],
			editable: "popup",
			edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
				editWindow.wrapper.css({ width: 500 });
                editWindow.title("Client Phone");
            },
			mobile: true
		});		
	}
	clientUI.prototype.renderDocumentsGrid = function(){				
		$('#documents').kendoGrid({
			dataSource: {
					transport: {
						read: function (entries) {
							entries.success(client.supportingDocuments);                    
						},
						create: function (entries) {
							entries.data = {
								document: currentDocumentContent,
								description: entries.data.description,
								fileName: currentModel.fileName,
								mimeType: currentModel.mimeType
							};
							currentDocumentContents.push(entries.data);
							entries.success(entries.data);
							client.supportingDocuments.push(entries.data);
						},
						update: function (entries) {
							entries.success(client.supportingDocuments);
						},
						destroy: function (entries) {
							entries.success(entries.data);
						},
						parameterMap: function (data) { return JSON.stringify(data); },
					},
					pageSize: 10,
					schema: {
						model: {
						id: 'documentId',
						fields: {
							documentId: { editable: false,type: 'number' },
							clientId: { editable: false, validation: { required: true } },
							description: { editable: true, validation: { required: true,
									validateTitle: function(input){
										if(input.is("[description]") && input.val().length > 255){
											input.attr("data-validateTitle-msg", "Max length 255 characters exceeded.");
											return false;
										} 
										else return true;
									}
                                },
                            },
							docdocumentum: { editable: true, validation: { required: true } },
							fileName: { editable: true, validation: { required: true } },
							mimeType: { editable: true, validation: { required: true } }
						} //fields
					},
				},
			},
			scrollable: false,
			sortable: true,
			columns: [  				
				{ field: "description",	title: "Description",editor: textEditor },  
				{ field: "docum", title: "Document",	editor: this.fileEditor,
					template: '#if(documentId > 0){# <a href="'+coreERPAPI_URL_Root+
					'/FileDownload/DownloadDocument/#: documentId #">#:fileName#</a>#} else {#<span>(Waiting to be saved...)</span>#}#',
				},    
				{ command: ["destroy"],width:150 }
			],
			pageable: {
				pageSize: 10,
				pageSizes: [10, 25, 50, 100, 1000],
				previousNext: true,
				buttonCount: 5,
			},
			toolbar: [
				{
					name: "create",
					className: 'addNewDocument',
					text: 'Add New Document',
				}
			],
			editable: "popup",
			edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
				editWindow.wrapper.css({ width: 500 });
                editWindow.title("Client Document");
            },
			mobile: true
		});		
	}
	clientUI.prototype.fileEditor = function(container, options) {//editor
        $('<input type="file" id="documentFile" name="documentFile" />')
            .appendTo(container)
            .kendoUpload({
				localization: {
					select: "Select the File",					
				},
				async: {
					autoUpload: true,
					saveUrl: clientApiUrl + '/SaveDocument?token=' + authToken,
					removeUrl: clientApiUrl + '/removeDocument?token=' + authToken,
				}, 
				upload: onDocumentUpload,
			});
        currentModel = options.model;
        var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
        tooltipElement.appendTo(container);
    }
	return clientUI;
})();

var onClientChange = function(){
	var clientId = $("#client").data("kendoComboBox").value();
	getClientPicture(clientId);
}

function dateEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        }
    });
}
function dateOfBirthEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoDatePicker({
        format: '{0:dd-MMM-yyyy}',
        parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
		max: minGuarantorDateOfBirth
    });
}

function textEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoMaskedTextBox();
}
function emailEditor(container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoMaskedTextBox();
}
function phoneEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoMaskedTextBox({
		mask: "(000) 000-0000",
	});
}
function numericEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoNumericTextBox({
		min:0
	});
}

function idTypeEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: idTypes,
        dataValueField: "idTypeId",
        dataTextField: "idTypeName",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		//change: onActionChanged,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function phoneTypeEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: phoneTypes,
        dataValueField: "phoneTypeID",
        dataTextField: "phoneTypeName",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		//change: onActionChanged,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function emailTypeEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: emailTypes,
        dataValueField: "emailTypeID",
        dataTextField: "emailTypeName",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		//change: onActionChanged,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function addressEditor(container, options) {
	$('<textarea class="form-control" id="addressLine" data-bind="value: ' + options.field + '" cols="3" rows="4"></textarea>')
        .appendTo(container)
		.width("100%");
}
function cityEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: cities,
        dataValueField: "cityId",
        dataTextField: "cityName",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		//change: onActionChanged,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function photoEditor(container, options) {//editor
	$('<input type="file" id="photoFile" name="photoFile" />')
		.appendTo(container)
		.kendoUpload({
			localization: {
				select: "Select the File",					
			},
			async: {
				autoUpload: true,
				saveUrl: loanApiUrl + '/SavePhoto?token=' + authToken,
				removeUrl: loanApiUrl + '/removePhoto?token=' + authToken,
			}, 
			upload: onPhotoUpload
		});
	//currentModel = options.model;
	var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
	tooltipElement.appendTo(container);
}
function cityEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: cities,
        dataValueField: "cityId",
        dataTextField: "cityName",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		//change: onActionChanged,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function onPhotoUpload(e) { //photo upload grabber
	if (!window.FileReader) {
	  return;
	}     
	$.each(e.files, function() {
	  var file = this.rawFile;
	  var reader = new FileReader();
	  reader.onload = (function() {
		var data = reader.result;
		currentPhotoContent=data.substr(data.indexOf('base64,')+7);
		currentModel.fileName = file.name;
		currentModel.mimeType = data.substr(0, data.indexOf ('base64'));
		currentModel.image = currentPhotoContent;


		var photoContent = {
			photo: currentPhotoContent,
			fileName: currentModel.fileName,
			mimeType: currentModel.mimeType
		}
		currentPhotoContents.push(photoContent);

	  });       
	  reader.readAsDataURL(file);
	  e.preventDefault();	  
	});
}
function onDocumentUpload(e) { //document upload grabber
        if (!window.FileReader) {
          return;
        }     
        $.each(e.files, function() {
          var file = this.rawFile;
          var reader = new FileReader();
          reader.onload = (function() {
            var data = reader.result;
			currentDocumentContent=data.substr(data.indexOf('base64,')+7);
            console.log(file.name, currentDocumentContent);
			currentModel.fileName = file.name;
			currentModel.mimeType = data.substr(0, data.indexOf ('base64'));
			currentModel.docum = currentDocumentContent;
          });       
          reader.readAsDataURL(file);
          e.preventDefault();	  
        });//todo: delete
    }

function collateralTypeEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: collateralTypes,
        dataValueField: "collateralTypeID",
        dataTextField: "collateralTypeName",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		//change: onActionChanged,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function legalOwnerEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: collateralOwnerships,
        dataValueField: "type",
        dataTextField: "type",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function financialTypeEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: financialTypes,
        dataValueField: "financialTypeID",
        dataTextField: "financialTypeName",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		//change: onActionChanged,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}
function frequencyEditor(container, options) {
    $('<input required data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .width("110%")
    .kendoComboBox({
        dataSource: repaymentModes,
        dataValueField: "repaymentModeID",
        dataTextField: "repaymentModeName",
        filter: "contains",
        highlightFirst: true,
        suggest: true,
		//change: onActionChanged,
        ignoreCase: true,
        animation: {
            close: { effects: "fadeOut zoom:out", duration: 300 },
            open: { effects: "fadeIn zoom:in", duration: 300 }
        },
        optionLabel: ''
    });
}

function getIdType(id) {
    for (var i = 0; i < idTypes.length; i++) {
        if (idTypes[i].idTypeId == id)
            return idTypes[i].idTypeName;
    }
}
function getPhoneType(id) {
    for (var i = 0; i < phoneTypes.length; i++) {
        if (phoneTypes[i].phoneTypeID == id)
		{return phoneTypes[i].phoneTypeName;}
    }
}
function getCity(id) {
    for (var i = 0; i < cities.length; i++) {
        if (cities[i].cityId == id)
		{return cities[i].cityName;}
    }
}
function getAddress(content) {
	if(typeof(content) == 'undefined')return"";
    var commentToShowOnGrid = content;
	if(content.length>8){
		commentToShowOnGrid = content.substr(0,7);
		return commentToShowOnGrid+"...";
	}
	return commentToShowOnGrid;
}
function getEmail(email) {
	if(typeof(email) == 'undefined')return"";
    var emailToShowOnGrid = email;
	if(email.length>5){
		emailToShowOnGrid = email.substr(0,5);
		return emailToShowOnGrid+"...";
	}
	return emailToShowOnGrid;
}
function getCollateralType(id)  {
    for (var i = 0; i < collateralTypes.length; i++) {
        if (collateralTypes[i].collateralTypeID == id)
		{return collateralTypes[i].collateralTypeName;}
    }
}
function getFinancialType(id)  {
    for (var i = 0; i < financialTypes.length; i++) {
        if (financialTypes[i].financialTypeID == id)
		{return financialTypes[i].financialTypeName;}
    }
}
function getFrequency(id)  {
    for (var i = 0; i < repaymentModes.length; i++) {
        if (repaymentModes[i].repaymentModeID == id)
		{return repaymentModes[i].repaymentModeName;}
    }
}
function getEmailType(id)  {
    for (var i = 0; i < emailTypes.length; i++) {
        if (emailTypes[i].emailTypeID == id)
		{return emailTypes[i].emailTypeName;}
    }
}
function saveClient() {
    retrieveValues();
	if(validateInput()){
		saveToServer();
	}else{
		warningDialog(errorMessage,'ERROR'); 
	}
    
}
function validateInput(){
	errorMessage = "";
	if((client.surName.length < 1 ) || client.otherNames.length < 1  || client.maritalStatusId < 1
		|| client.industryId < 1 || client.branchId < 1 || client.sectorId < 1
		|| client.clientTypeId < 1 || client.clientCategoryId < 0 || client.primaryIdTypeId < 1
		|| client.primaryIdNo.length < 1 || client.sex.length < 1){
		if(client.surName.length < 1 ){
			errorMessage = errorMessage + "Surname cannot be empty <br/>"
		}
		if(client.otherNames.length < 1){
			errorMessage = errorMessage + "otherNames cannot be empty <br/>"
		}
		if(client.maritalStatusId < 1){
			errorMessage = errorMessage + "Invalid marital status <br/>"
		}
		if(client.industryId < 1){
			errorMessage = errorMessage + "Invalid industry <br/>"
		}
		if(client.branchId < 1){
			errorMessage = errorMessage + "Invalid branch <br/>"
		}
		if(client.sectorId < 1){
			errorMessage = errorMessage + "Invalid sector <br/>"
		}		
		if(client.clientTypeId < 1){
			errorMessage = errorMessage + "Invalid client type <br/>"
		}
		if(client.clientCategoryId < 0){
			errorMessage = errorMessage + "Invalid client category <br/>"
		}
		if(client.primaryIdTypeId < 1){
			errorMessage = errorMessage + "Invalid primary ID type <br/>"
		}
		if(client.primaryIdNo.length < 1){
			errorMessage = errorMessage + "Primary ID Number cannot be empty <br/>"
		}
		if(client.sex.length < 1){
			errorMessage = errorMessage + "Client sex cannot be empty <br/>"
		}
		return false;
	}
	else return true;
}

function retrieveValues(){
	client.surName = $("#surName").data("kendoMaskedTextBox").value();
	client.otherNames = $("#otherNames").data("kendoMaskedTextBox").value();
	client.dateOfBirth = $("#DOB").data("kendoDatePicker").value();
	client.maritalStatusId = $("#maritalStatus").data("kendoComboBox").value();
	client.industryId = $("#industry").data("kendoComboBox").value();
	client.branchId = $("#branch").data("kendoComboBox").value();
	client.sectorId = $("#sector").data("kendoComboBox").value();
	client.clientTypeId = $("#clientType").data("kendoComboBox").value();
	client.clientCategoryId = $("#clientCategory").data("kendoComboBox").value();
	client.primaryIdTypeId = $('#primaryIdType').data("kendoComboBox").value();
	client.primaryIdNo = $('#primaryIdNo').data("kendoMaskedTextBox").value();
	client.primaryIdExpiry = $('#primaryIdExpiry').data("kendoDatePicker").value();
	client.secondaryIdTypeId = $("#secondaryIdType").data("kendoComboBox").value();
	client.secondaryIdNo = $("#secondaryIdNo").data("kendoMaskedTextBox").value();
	client.secondaryIdExpiry = $("#secondaryIdExpiry").data("kendoDatePicker").value();	
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
	client.clientPhones = $('#phones').data().kendoGrid.dataSource.view();
	client.clientEmails = $('#email').data().kendoGrid.dataSource.view();
	client.supportingDocuments = $('#documents').data().kendoGrid.dataSource.view();	
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(client),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		if(data == null){
			successDialog('Client Account Successfully Created', 'SUCCESS',
			function () { window.location = '/dash/home.aspx'; }); 
		}else{
			successDialog('Client Account Successfully Created. Account Number: '+data.accountNumber , 'SUCCESS',
			function () { window.location = '/dash/home.aspx'; }); 
		}       
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}//func saveToServer



function getClientPicture(id) {
    displayLoadingDialog();
    $.ajax({
        url: clientApiUrl + "/GetClientImage/"+id,
        type: 'GET',
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
		dismissLoadingDialog();
		displayClientImage(data);
    }).error(function (xhr, data, error) {
        dismissLoadingDialog();
		//warningDialog("Error while retrieving client Image", 'ERROR');
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}
function displayClientImage(data){
	$("#clientImage").attr("src","data:image/png;base64,"+data);
}


function getClients(input) {
	displayLoadingDialog();
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: clientApiUrl + '/GetSearchedClient',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(input),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		if(data.length>0){
			filteredClients = data;
			resetControls();
		}
		else{
			filteredClients = [];
			successDialog('No client found that match the searched criteria', 'SUCCESS');
		}        
    }).error(function (xhr, data, error) {
		dismissLoadingDialog();
		warningDialog(xhr.responseJSON.ExceptionMessage,'ERROR'); 
	});	
}
function resetControls(){
	//var clnt = $("#client").data("kendoComboBox");
	//clnt.setDataSource(filteredClients);
	var ui = new clientUI();
	ui.renderControls();
	$("#details").show();
}



