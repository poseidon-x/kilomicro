/*
UI Scripts for Loan savingType Management
Creator: man@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed

"use strict";
var authToken = coreERPAPI_Token;
var loanApiUrl = coreERPAPI_URL_Root + "/crud/loan";
var loanTypeApiUrl = coreERPAPI_URL_Root + "/crud/lnType";
var repaymentModeApiUrl = coreERPAPI_URL_Root + "/crud/repaymentMode";
var interestTypeApiUrl = coreERPAPI_URL_Root + "/crud/interestType";
var staffApiUrl = coreERPAPI_URL_Root + "/crud/staff";
var agentApiUrl = coreERPAPI_URL_Root + "/crud/agent";
var clientApiUrl = coreERPAPI_URL_Root + "/crud/AllClients";
var idTypeApiUrl = coreERPAPI_URL_Root + "/crud/idType";
var phoneTypeApiUrl = coreERPAPI_URL_Root + "/crud/phoneType";
var cityApiUrl = coreERPAPI_URL_Root + "/crud/city";
var collateralTypeApiUrl = coreERPAPI_URL_Root + "/crud/collateralType";
var financialTypeApiUrl = coreERPAPI_URL_Root + "/crud/financialType";

var loan = {};
var loanTypes = {}; 
var repaymentModes = {};
var interestTypes = {};
var staff = {};
var agents = {};
var idTypes = {};
var cities = {};
var phoneTypes = {};
var collateralTypes = {};
var financialTypes = {};
var filteredClients = {};
var currentModel = {}; 
var currentDocumentContent='';
var currentDocumentContents=[];
var currentPhotoContent='';
var currentPhotoContents=[];
var collateralOwnerships = [
	{ id: 1, type: "Guarantor" },
	{ id: 2, type: "Applicant" }
]
var errorMessage = "";
var minGuarantorDateOfBirth = new Date();
minGuarantorDateOfBirth.setFullYear(minGuarantorDateOfBirth.getFullYear() - 18 );

var loanAjax = $.ajax({
    url: loanApiUrl + '/GetLoanAccount/'+id ,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var loanTypeAjax = $.ajax({
    url: loanTypeApiUrl + '/GetViewModel',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var repaymentModeAjax = $.ajax({
    url: repaymentModeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var interestTypeAjax = $.ajax({
    url: interestTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var staffAjax = $.ajax({
    url: staffApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var agentAjax = $.ajax({
    url: agentApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var idTypeAjax = $.ajax({
    url: idTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var collateralTypeAjax = $.ajax({
    url: collateralTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var financialTypeAjax = $.ajax({
    url: financialTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var phoneTypeAjax = $.ajax({
    url: phoneTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
var cityAjax = $.ajax({
    url: cityApiUrl + '/GetViewModel',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
//Load page data
function loadData() {	
    $.when(loanAjax,loanTypeAjax,repaymentModeAjax,interestTypeAjax,staffAjax,agentAjax,
		idTypeAjax,phoneTypeAjax,cityAjax,collateralTypeAjax,financialTypeAjax)
        .done(function (dataLoan,dataLoanType,dataRepaymentMode,dataInterestType,dataStaff,dataAgent,
			dataIdType,dataPhoneType,dataCity,dataCollateralType,dataFinancialType) {
            loan = dataLoan[2].responseJSON;
            loanTypes = dataLoanType[2].responseJSON;
            repaymentModes = dataRepaymentMode[2].responseJSON;
            interestTypes = dataInterestType[2].responseJSON;
            staff = dataStaff[2].responseJSON;
            agents = dataAgent[2].responseJSON;
            idTypes = dataIdType[2].responseJSON;
            cities = dataCity[2].responseJSON;
            phoneTypes = dataPhoneType[2].responseJSON;
            collateralTypes = dataCollateralType[2].responseJSON;
            financialTypes = dataFinancialType[2].responseJSON;
		
			dismissLoadingDialog();
			//Prepares UI
			var ui = new loanUI();
			ui.prepareUi();
        }
	);
}
$(function () {
    displayLoadingDialog();
    loadData();
});

var loanUI = (function(){
	function loanUI(){	
	}
	loanUI.prototype.prepareUi = function(){
		$("#details").hide();
		$("#searchBtn").click(function(){
			var searchInput = $("#searchInput").val();
			if(searchInput.length>2){
				var input = {searchString:searchInput};
				getClients(input);
			}else{
				warningDialog("Minimum of three(3) characters required","Note");
			}
		});
		$('#save').click(function (event) {
			if (confirm("Are you sure you want Save Loan")) {
				displayLoadingDialog();
				saveLoan();				
			} else
			{
				smallerWarningDialog('Please review and save later', 'NOTE');
			}			
		});		
	}
	loanUI.prototype.renderControls = function(){
		$("#tabs").kendoTabStrip();
		$("#client").width("90%")
		.kendoComboBox({
			dataSource: filteredClients,
			filter: "contains",
			suggest: true,
			dataValueField: "clientID",
			dataTextField: "clientNameWithAccountNO",
			highlightFirst: true,
			ignoreCase: true,
			change: onClientChange,
			optionLabel: "",
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 300 },
				open: { effects: "fadeIn zoom:in", duration: 300 }
			}
		});
		$("#amountRequested").width("90%")
		.kendoNumericTextBox({
			min: 1
		});		
		$("#loanType").width("90%")
		.kendoComboBox({
			dataSource: loanTypes,
			filter: "contains",
			suggest: true,
			dataValueField: "loanTypeId",
			dataTextField: "loanTypeName",
			highlightFirst: true,
			ignoreCase: true,
			//change: onClientChange,
			optionLabel: "",
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 300 },
				open: { effects: "fadeIn zoom:in", duration: 300 }
			}
		});
		$("#tenure").width("90%")
		.kendoNumericTextBox({
			min: 1,
			format: "0 ' Month(s)'"
		});	
		$("#applicationDate").width("90%").kendoDatePicker({
			format: '{0:dd-MMM-yyyy}',
			parseFormats: ["yyyy-MM-dd", "yy-MM-dd", "yyyy-MMM-dd"],
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 300 },
				open: { effects: "fadeIn zoom:in", duration: 300 }
			},
			value: new Date()
		});
		$("#repaymentMode").width("90%")
		.kendoComboBox({
			dataSource: repaymentModes,
			filter: "contains",
			suggest: true,
			dataValueField: "repaymentModeID",
			dataTextField: "repaymentModeName",
			highlightFirst: true,
			ignoreCase: true,
			//change: onClientChange,
			optionLabel: "",
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 300 },
				open: { effects: "fadeIn zoom:in", duration: 300 }
			}
		});
		$("#interestRate").width("90%")
		.kendoNumericTextBox({
			min: 1,
			max: 100,
			format: "0 ' %'"
		});
		$("#interestType").width("90%")
		.kendoComboBox({
			dataSource: interestTypes,
			filter: "contains",
			suggest: true,
			dataValueField: "interestTypeID",
			dataTextField: "interestTypeName",
			highlightFirst: true,
			ignoreCase: true,
			//change: onClientChange,
			optionLabel: "",
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 300 },
				open: { effects: "fadeIn zoom:in", duration: 300 }
			}
		});
		$("#staff").width("90%")
		.kendoComboBox({
			dataSource: staff,
			filter: "contains",
			suggest: true,
			dataValueField: "staffId",
			dataTextField: "staffNameWithStaffNo",
			highlightFirst: true,
			ignoreCase: true,
			//change: onClientChange,
			optionLabel: "",
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 300 },
				open: { effects: "fadeIn zoom:in", duration: 300 }
			}
		});
		$("#agent").width("90%")
		.kendoComboBox({
			dataSource: agents,
			filter: "contains",
			suggest: true,
			dataValueField: "agentId",
			dataTextField: "agentNameWithNo",
			highlightFirst: true,
			ignoreCase: true,
			//change: onClientChange,
			optionLabel: "",
			animation: {
				close: { effects: "fadeOut zoom:out", duration: 300 },
				open: { effects: "fadeIn zoom:in", duration: 300 }
			}
		});	
		this.renderGuarantorGrid();
		this.renderCollateralGrid();
		this.renderFinancialsGrid();
		this.renderDocumentsGrid();
	}
	loanUI.prototype.renderGuarantorGrid = function(){				
		$('#guarantors').kendoGrid({
			dataSource: {
					transport: {
						read: function (entries) {
							entries.success(loan.lnGurantors);                    
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
						id: 'loanGurantorID',
						fields: {
							loanGurantorID: { editable: false,type: 'number' },
							loanID: { editable: false, validation: { required: true } },
							surName: { editable: true, validation: { required: true } },
							otherNames: { editable: true, validation: { required: true } },
							DOB: { type: "date", editable: true, validation: { required: true } },
							idTypeId: { editable: true, validation: { required: true } },
							IdNumber: { editable: true, validation: { required: true } },
							phoneTypeId: { editable: true, validation: { required: true } },							
							addressLine: { editable: true, validation: { required: true } },
							cityId: { editable: true, validation: { required: true } },
							phoneNumer: { editable: true, validation: { required: true } },
							email: { editable:true,validation: { required: false } },
						} //fields
					},
				},
			},
			scrollable: false,
			sortable: true,
			columns: [
				{ field: 'surName',title: 'Surname',editor: textEditor },
				{ field: 'otherNames', title: 'OtherNames', editor: textEditor },  
				{ field: 'DOB', title: 'Date of Birth',editor: dateOfBirthEditor, format: "{0: dd-MMM-yyyy}" }, 
				{ field: 'IdNumber', title: 'ID. No.',editor: textEditor },  				
				{ field: 'idTypeId', title: 'ID. Type',editor: idTypeEditor,template: '#= getIdType(idTypeId) #' },  
				{ field: 'phoneNumer', title: 'Phone',editor: phoneEditor },
				{ field: 'phoneTypeId', title: 'Phone. Type',editor: phoneTypeEditor,template: '#= getPhoneType(phoneTypeId) #' },  				
				{ field: 'addressLine', title: 'Addrs.Line',editor: addressEditor, template: '#= getAddress(addressLine) #' }, 
				{ field: 'cityId', title: 'City',editor: cityEditor,template: '#= getCity(cityId) #' },            
				{ field: 'email', title: 'Email',editor: textEditor, template: '#= getEmail(email) #' },            
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
					//className: 'addGurantor',
					text: 'Add Gurantor',
				}
			],
			editable: "popup",
			edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
				editWindow.wrapper.css({ width: 500 });
                editWindow.title("Loan Gurantor");
            },
			detailTemplate: 'Gurantor Photo: <div class="grid"></div>',
			detailInit: gurantorGrid_detailInit,
			mobile: true
		});		
	}
	loanUI.prototype.renderCollateralGrid = function(){				
		$('#collaterals').kendoGrid({
			dataSource: {
					transport: {
						read: function (entries) {
							entries.success(loan.lnCollateral);                    
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
						id: 'loanCollateralID',
						fields: {
							loanCollateralID: { editable: false,type: 'number' },
							loanID: { editable: false, validation: { required: true } },
							collateralTypeID: { editable: true, validation: { required: true } },
							fairValue: { editable: true, validation: { required: true } },
							legalOwner: { editable: true, validation: { required: true } },
							collateralDescription: { editable: true, validation: { required: true } }
						} //fields
					},
				},
			},
			scrollable: false,
			sortable: true,
			columns: [  				
				{ field: 'collateralTypeID', title: 'Collateral Type',editor: collateralTypeEditor,template: '#= getCollateralType(collateralTypeID) #' },  
				{ field: 'fairValue', title: 'Fair Value',editor: numericEditor },
				{ field: 'legalOwner', title: 'Legal Owner', editor: legalOwnerEditor },  				
				{ field: 'collateralDescription', title: 'Description',editor: addressEditor, template: '#= getAddress(collateralDescription) #' },           
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
					className: 'addGurantor',
					text: 'Add Collateral',
				}
			],
			editable: "popup",
			edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
				editWindow.wrapper.css({ width: 500 });
                editWindow.title("Loan Collateral");
            },
			detailTemplate: 'Collateral Photoes: <div class="grid"></div>',
			detailInit: collateralGrid_detailInit,
			mobile: true
		});		
	}
	loanUI.prototype.renderFinancialsGrid = function(){				
		$('#financials').kendoGrid({
			dataSource: {
					transport: {
						read: function (entries) {
							entries.success(loan.loanFinancials);                    
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
						id: 'loanFinancialID',
						fields: {
							loanFinancialID: { editable: false,type: 'number' },
							loanID: { editable: false, validation: { required: true } },
							financialTypeID: { editable: true, validation: { required: true } },
							revenue: { editable: true, validation: { required: true } },
							expenses: { editable: true, validation: { required: true } },
							otherCosts: { editable: true, validation: { required: true } },
							frequencyID: { editable: true, validation: { required: true } }
						} //fields
					},
				},
			},
			scrollable: false,
			sortable: true,
			columns: [  				
				{ field: 'financialTypeID', title: 'Financial Type',editor: financialTypeEditor,template: '#= getFinancialType(financialTypeID) #' },  
				{ field: 'revenue', title: 'Revenue',editor: numericEditor },
				{ field: 'expenses', title: 'Expense', editor: numericEditor },  				
				{ field: 'otherCosts', title: 'otherCosts',editor: numericEditor },    
				{ field: 'frequencyID', title: 'frequency',editor: frequencyEditor, template: '#= getFrequency(frequencyID) #' },           				
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
					className: 'addFinancial',
					text: 'Add Financial',
				}
			],
			editable: "popup",
			edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
				editWindow.wrapper.css({ width: 500 });
                editWindow.title("Loan Financial");
            },
			//detailTemplate: 'Gurantor Photo: <div class="grid"></div>',
			//detailInit: grid_detailInit,
			mobile: true
		});		
	}
	loanUI.prototype.renderDocumentsGrid = function(){				
		$('#documents').kendoGrid({
			dataSource: {
					transport: {
						read: function (entries) {
							entries.success(loan.lnDocuments);                    
						},
						create: function (entries) {
							entries.data = {
								docum: currentDocumentContent,
								description: entries.data.description,
								fileName: currentModel.fileName,
								mimeType: currentModel.mimeType
							};
							currentDocumentContents.push(entries.data);
							entries.success(entries.data);
							loan.lnDocuments.push(entries.data);
						},
						update: function (entries) {
							entries.success(loan.lnDocuments);
						},
						destroy: function (entries) {
							entries.success(entries.data);
						},
						parameterMap: function (data) { return JSON.stringify(data); },
					},
					pageSize: 10,
					schema: {
						model: {
						id: 'loanDocumentID',
						fields: {
							loanDocumentID: { editable: false,type: 'number' },
							loanID: { editable: false, validation: { required: true } },
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
							docum: { editable: true, validation: { required: true } },
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
					template: '#if(loanDocumentID > 0){# <a href="'+coreERPAPI_URL_Root+
					'/FileDownload/DownloadDocument/#: loanDocumentID #">#:fileName#</a>#} else {#<span>(Waiting to be saved...)</span>#}#',
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
                editWindow.title("Loan Document");
            },
			//detailTemplate: 'Gurantor Photo: <div class="grid"></div>',
			//detailInit: grid_detailInit,
			mobile: true
		});		
	}
	loanUI.prototype.fileEditor = function(container, options) {//editor
        $('<input type="file" id="documentFile" name="documentFile" />')
            .appendTo(container)
            .kendoUpload({
				localization: {
					select: "Select the File",					
				},
				async: {
					autoUpload: true,
					saveUrl: loanApiUrl + '/SaveDocument?token=' + authToken,
					removeUrl: loanApiUrl + '/removeDocument?token=' + authToken,
				}, 
				upload: onDocumentUpload,
			});
        currentModel = options.model;
        var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
        tooltipElement.appendTo(container);
    }
	return loanUI;
})();

function gurantorGrid_detailInit(e) {
    e.detailRow.find(".grid").kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
						if (typeof (e.data.gurantorPhotos) === "undefined") {
							e.data.gurantorPhotos = [];
						}
						entries.success(e.data.gurantorPhotos);
					},
					create: function (entries) {
						entries.data = {
							photo:currentPhotoContent,
							fileName: currentModel.fileName,
							mimeType: currentModel.mimeType
						}
						//currentPhotoContents.push(entries.data);
						entries.success(entries.data);
						e.data.gurantorPhotos.push(entries.data);
					},
					update: function (entries) {
						entries.success(e.data.gurantorPhotos);
					},/*
					destroy: function (entries) {
					    for (var i = 0; i < employee.employeePhotoes.length; i++) {
					        if (employee.employeePhotoes[i].employeePhotoId == entries.data.employeePhotoId) {
					            employee.employeePhotoes.splice(i, 1);
					        }
					    }
					    for (var i = 0; i < currentPhotoContents.length; i++) {
					        if (currentPhotoContents[i].employeePhotoId == entries.data.employeePhotoId) {
					            currentPhotoContents.splice(i, 1);
					        }
					    }
						entries.success(employee.employeePhotoes);	
					},*/
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'imageID',
                    fields: {
						imageID: { editable: false, type: 'number' },						
						loanGurantorID: { editable: false, type: 'number' },
						photo: {  validation: { required: true } },
						fileName: { editable: false, validation: { required: true } },
						mimeType: { validation: { required: true } },
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
			{ field: 'photo', title: 'photo',editor: photoEditor, template: '<img src="#:mimeType#base64,#:photo#" height="40"/>' },
            { field: 'fileName', title: 'File Name' },
            {
                command: [
                    { name: "destroy", text: "Delete" }
                ],
                width: 150
            },
		],
		toolbar: [
                { name: "create", text: "Add New Photo" },
            ],
		editable: "popup",
		selectable: true,
		edit: function (e) {
			var editWindow = this.editable.element.data("kendoWindow");
			editWindow.title("Edit Photo Data");
		},
		save: function (e) {
			$('.k-grid-update').css('display', 'none');
			
		},
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		mobile: true
    }).data("kendoGrid");
}
function collateralGrid_detailInit(e) {
    e.detailRow.find(".grid").kendoGrid({
        dataSource: {
                transport: {
					read: function (entries) {
						if (typeof (e.data.collateralPhotos) === "undefined") {
							e.data.collateralPhotos = [];
						}
						entries.success(e.data.collateralPhotos);
					},
					create: function (entries) {
						entries.data = {
							photo:currentPhotoContent,
							fileName: currentModel.fileName,
							mimeType: currentModel.mimeType
						}
						//currentPhotoContents.push(entries.data);
						entries.success(entries.data);
						e.data.collateralPhotos.push(entries.data);
					},
					update: function (entries) {
						entries.success(e.data.collateralPhotos);
					},/*
					destroy: function (entries) {
					    for (var i = 0; i < employee.employeePhotoes.length; i++) {
					        if (employee.employeePhotoes[i].employeePhotoId == entries.data.employeePhotoId) {
					            employee.employeePhotoes.splice(i, 1);
					        }
					    }
					    for (var i = 0; i < currentPhotoContents.length; i++) {
					        if (currentPhotoContents[i].employeePhotoId == entries.data.employeePhotoId) {
					            currentPhotoContents.splice(i, 1);
					        }
					    }
						entries.success(employee.employeePhotoes);	
					},*/
					parameterMap: function (data) { return JSON.stringify(data); },
				},
                pageSize: 10,
                schema: {
                    model: {
                    id: 'collateralImageID',
                    fields: {
						collateralImageID: { editable: false, type: 'number' },						
						loanCollateralID: { editable: false, type: 'number' },
						photo: {  validation: { required: true } },
						fileName: { editable: false, validation: { required: true } },
						mimeType: { validation: { required: true } },
					} //fields
                },
			},
		},
        scrollable: false,
        sortable: true,
        columns: [
			{ field: 'photo', title: 'photo',editor: photoEditor, template: '<img src="#:mimeType#base64,#:photo#" height="40"/>' },
            { field: 'fileName', title: 'File Name' },
            {
                command: [
                    { name: "destroy", text: "Delete" }
                ],
                width: 150
            },
		],
		toolbar: [
                { name: "create", text: "Add Collateral Photo" },
            ],
		editable: "popup",
		selectable: true,
		edit: function (e) {
			var editWindow = this.editable.element.data("kendoWindow");
			editWindow.title("Edit Photo Data");
		},
		save: function (e) {
			$('.k-grid-update').css('display', 'none');
			
		},
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
		mobile: true
    }).data("kendoGrid");
}

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
function saveLoan() {
    retrieveValues();
	if(validateInput()){
		saveToServer();
	}else{
		warningDialog(errorMessage,'ERROR'); 
	}
    
}
function validateInput(){
	errorMessage = "";
	if((loan.clientID < 1 ) || loan.amountRequested < 1 || loan.loanTypeID < 1
		|| loan.loanTenure < 1 || loan.repaymentModeID < 1 || loan.iterestRate < 1
		|| loan.interestTypeID < 1 ){
		if(loan.clientID < 1 ){
			errorMessage = errorMessage + "Invalid client selected <br/>"
		}
		if(loan.amountRequested < 1){
			errorMessage = errorMessage + "Amount requested cannot be less than one(1) <br/>"
		}
		if(loan.loanTypeID){
			errorMessage = errorMessage + "Invalid loan type selected <br/>"
		}
		if(loan.loanTenure < 1){
			errorMessage = errorMessage + "Loan tenure loan cannot be less than one(1) <br/>"
		}
		if(loan.repaymentModeID < 1){
			errorMessage = errorMessage + "Invalid Repayment mode selected <br/>"
		}
		if(loan.iterestRate < 1){
			errorMessage = errorMessage + "Interest rate cannot be less than one(1) <br/>"
		}
		if(loan.interestTypeID < 1){
			errorMessage = errorMessage + "Invalid interest type selected <br/>"
		}
		return false;
	}
	else return true;
}

function retrieveValues(){
	loan.clientID = $("#client").data("kendoComboBox").value();
	loan.amountRequested = $("#amountRequested").data("kendoNumericTextBox").value();
	loan.loanTypeID = $("#loanType").data("kendoComboBox").value();
	loan.loanTenure = $("#tenure").data("kendoNumericTextBox").value();
	loan.applicationDate = $("#applicationDate").data("kendoDatePicker").value();
	loan.repaymentModeID = $("#repaymentMode").data("kendoComboBox").value();
	loan.interestRate = $("#interestRate").data("kendoNumericTextBox").value();
	loan.interestTypeID = $("#interestType").data("kendoComboBox").value();
	loan.staffID = $("#staff").data("kendoComboBox").value();
	loan.agentID = $("#agent").data("kendoComboBox").value();
	loan.lnGurantors = $('#guarantors').data().kendoGrid.dataSource.view();
	loan.lnCollateral = $('#collaterals').data().kendoGrid.dataSource.view();
	loan.loanFinancials = $('#financials').data().kendoGrid.dataSource.view();	
	loan.lnDocuments = $('#documents').data().kendoGrid.dataSource.view();	
}
function saveToServer() {	
    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: loanApiUrl + '/Post',
        type: 'Post',
        contentType: 'application/json',
        data: JSON.stringify(loan),
        beforeSend: function(req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
		if(data == null){
			successDialog('Loan Successfully Saved ', 'SUCCESS',
			function () { window.location = '/dash/home.aspx'; }); 
		}else{
			successDialog('Loan Successfully Saved, loan Number '+data.loanNo , 'SUCCESS',
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
	var ui = new loanUI();
	ui.renderControls();
	$("#details").show();
}



