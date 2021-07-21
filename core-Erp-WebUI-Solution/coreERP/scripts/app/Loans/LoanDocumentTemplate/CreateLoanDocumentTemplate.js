//***********************************************************//
//  	     LOAN DOCUMENT TEMPLATE - JAVASCRIPT                
// 		        CREATOR: EMMANUEL OWUSU(MAN)    	   
//		       WEEK: JULY(27TH - 31TH), 2015  		  
//*********************************************************//


"use strict";


//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var documentTemplateApiUrl = coreERPAPI_URL_Root + "/crud/loanDocumentTemplate";
var loanDocumentPlaceHolderTypeApiUrl = coreERPAPI_URL_Root + "/crud/loanDocumentPlaceHolderType";
var clientLoansApiUrl = coreERPAPI_URL_Root + "/crud/clientLoan";


//Declaration of variables to store records retrieved from the database
var documentTemplate = {};
var loanDocumentPlaceHolderTypes = {};
var currentPagePlaceHolders = [];

var pageNum = 1;

$(function () {
    displayLoadingDialog();
    loadForm();
});

var documentTemplateAjax = $.ajax({
    url: documentTemplateApiUrl + '/Get/' + documentTemplateId,
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var loanDocumentPlaceHolderTypeAjax = $.ajax({
    url: loanDocumentPlaceHolderTypeApiUrl + '/Get',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});
//Function to perform all ajax call at once and retrieve them into the respective array variables
function loadForm() {

    $.when(documentTemplateAjax, loanDocumentPlaceHolderTypeAjax)
        .done(function (dataDocumentTemplate, dataLoanDocumentPlaceHolderType) {
            documentTemplate = dataDocumentTemplate[2].responseJSON;
            loanDocumentPlaceHolderTypes = dataLoanDocumentPlaceHolderType[2].responseJSON;

            //Prepares UI
            prepareUi();
        });

}

//Function to prepare user interface
function prepareUi() {
    //If loanDocumentTemplateId > 0, Its an Update/Put, Hence render UI with retrieved existing data
    if (documentTemplate.loanDocumentTemplateId > 0) {
        renderControls();
        populateUi();
        renderGrid();
        dismissLoadingDialog();
    } else //Else its a Post/Create, Hence render empty UI for new Entry
    {
        renderControls();
        renderGrid();
        dismissLoadingDialog();
    }

    //Validate to Check Empty/Null input Fields
    $('#save').click(function (event) {

        var validator = $("#myform").kendoValidator().data("kendoValidator");

        if (!validator.validate()) {
            smallerWarningDialog('One or More Fields are Empty', 'ERROR');
        } else {
            var pagesGridData = $("#pageGrid").data().kendoGrid.dataSource.view();

            if (pagesGridData.length > 0) {
                displayLoadingDialog();
                saveDocumentPageGridData(pagesGridData);
                //Retrieve & save Grid data
                saveDocument();
            } else {
                smallerWarningDialog('Please add pages to document', 'NOTE');
            }
        }
    });
}

//Apply kendo Style to the input fields
function renderControls() {
    $('#tabs').kendoTabStrip();

    $("#templateName").width("75%")
		.kendoMaskedTextBox();
}

function populateUi() {
    $('#templateName').data('kendoMaskedTextBox').value(documentTemplate.templateName);
}

function saveDocumentPageGridData(data) {
    if (data.length > 1) {
        for (var i = 0; i < data.length; i++) {
            documentTemplate.loanDocumentTemplatePages.push(data[i]);
        }
    }
    else {
        documentTemplate.loanDocumentTemplatePages.push(data[0]);
    }
}

//render Grid
function renderGrid() {
    $('#pageGrid').kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(documentTemplate.loanDocumentTemplatePages);
                },
                create: function (entries) {
                    var data = entries.data;

                    //Check to if page does not have placeHolders & assign that of current page to it.
                    if (data.loanDocumentTemplatePagePlaceHolders.length < 1 && data.isNewPage !== false) {
                        data.isNewPage = false;
                        data.loanDocumentTemplatePagePlaceHolders = currentPagePlaceHolders;
                        currentPagePlaceHolders = [];
                    }
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
                    id: 'loanDocumentTemplatePageId',
                    fields: {
                        loanDocumentTemplateId: { type: 'number', defaultValue: documentTemplate.loanDocumentTemplateId },
                        loanDocumentTemplatePageId: { type: 'number', editable: false },
                        pageNumber: { type: 'number', validation: { min: 1 } },
                        isNewPage: { type: 'boolean', defaultValue: true },
                        content: { type: 'string' },
                    } //fields
                } //model
            } //schema
        }, //datasource
        editable: 'popup',
        columns: [
			{ field: 'pageNumber', title: 'Page Number' },
            { field: 'content', title: 'Content', editor: contentEditor, template: '#= getContent(content) #' },

            { command: ['destroy'] }
        ],
        edit: function (e) {
            var editWindow = this.editable.element.data("kendoWindow");
            editWindow.wrapper.css({ width: 800 });
            editWindow.title("Edit Page");
        },
        toolbar: [{ name: 'create', text: 'Add New Page' }],
        detailTemplate: 'Place Holders: <div class="grid"></div>',
        detailInit: grid_detailInit,
        dataBound: function () {
            this.expandRow(this.tbody.find("tr.k-master-row").first());
        }


    });


}

function grid_detailInit(e) {

    e.detailRow.find(".grid").kendoGrid({
        dataSource: {
            transport: {
                read: function (entries) {
                    if (typeof (e.data.loanDocumentTemplatePagePlaceHolders) === "undefined") {
                        e.data.loanDocumentTemplatePagePlaceHolders = [];
                    }
                    entries.success(e.data.loanDocumentTemplatePagePlaceHolders);
                },
                create: function (entries) {
                    entries.success(entries.data);
                },
                update: function (entries) {
                    entries.success();
                },
                destroy: function (entries) {
                    entries.success();
                }
            },
            schema: {
                model: {
                    id: 'loanDocumentTemplatePagePlaceHolderId',
                    fields: {
                        loanDocumentTemplatePagePlaceHolderId: { type: 'number', defaultValue: e.data.loanDocumentTemplatePagePlaceHolderId },
                        loanDocumentTemplatePageId: { type: 'number', editable: false },
                        placeHolderTypeId: { validation: { required: true } }
                    } //fields
                } //model
            } //schema
        },
        scrollable: false,
        sortable: true,
        pageable: true,
        editable: "popup",
        columns: [
            { field: 'placeHolderTypeId', title: 'Place Holder', template: '#= getPlaceHolderType(placeHolderTypeId) #' },
        ],
    }).data("kendoGrid");
}



//retrieve values from from Input Fields and save 
function saveDocument() {
    retrieveValues();
    saveToServer();
}


function retrieveValues() {
    documentTemplate.templateName = $('#templateName').data('kendoMaskedTextBox').value();
}

//Save to server function
function saveToServer() {

    var type = '';

    if (documentTemplate.loanDocumentTemplateId > 0) {
        type = 'Put';
    } else {
        type = 'Post';
    }

    //Perform an ajax call and pass the retrieved data to the Post function
    $.ajax({
        url: documentTemplateApiUrl + '/' + type,
        type: type,
        contentType: 'application/json',
        data: JSON.stringify(documentTemplate),
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).success(function (data) {
        //On success stop loading Dialog and alert a success message
        dismissLoadingDialog();
        successDialog('Loan Document Template Successfully Saved', 'SUCCESS',
         function () { window.location = "/DocumentTemplate/LoanDocumentTemplates/"; });

    }).error(function (xhr, data, error) {
        //On error stop loading Dialog and alert a the specific message
        dismissLoadingDialog();
        warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
    });
}//func saveToServer

function contentEditor(container, options) {
    $('<textarea id="contentText" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
	.width("250%")
    .kendoEditor();
}

function getPlaceHolderType(id) {
    for (var i = 0; i < loanDocumentPlaceHolderTypes.length; i++) {
        if (loanDocumentPlaceHolderTypes[i].loanDocumentPlaceHolderTypeId == id)
            return loanDocumentPlaceHolderTypes[i].entityTypeCode;
    }
}

function getContent(content) {
    //call retrieve placeHolders function
    retrievePlaceHoldersNames(content);

    //retrieve first 70 characters of content to display on grid
    var contentToShow = content.substr(0, 400);
    return contentToShow;
}

function retrievePlaceHoldersNames(content) {
    //retrieve placeHolders on page
    var pagePlaceHolders = retrievePlaceHolders(content);

    //Check if placeHolder exist & attach to page Place Holders
    if (pagePlaceHolders.length > 0) {
        currentPagePlaceHolders = [];
        for (var i = 0; i < pagePlaceHolders.length; i++) {
            var id = -1;
            for (var j = 0; j < loanDocumentPlaceHolderTypes.length; j++) {
                if (loanDocumentPlaceHolderTypes[j].placeHolderTypeCode == pagePlaceHolders[i])
                    id = loanDocumentPlaceHolderTypes[j].loanDocumentPlaceHolderTypeId;
            }

            if (id >= 0) {
                var pph = {
                    loanDocumentTemplatePagePlaceHolderId: 0,
                    loanDocumentTemplatePageId: 0,
                    placeHolderTypeId: id
                };
                if (currentPagePlaceHolders.indexOf(pph) < 0)
                { currentPagePlaceHolders.push(pph); }
            }
        }
    } else { currentPagePlaceHolders = []; }

}

function retrievePlaceHolders(content) {
    var placeHolders = [];
    var placeHolder;

    var loop = true;
    var startIndexOfFirstPH = -1;
    var endIndexOfFirstPH = -1;

    //retrieve  placeholders on current page
    do {
        startIndexOfFirstPH = content.indexOf("$");
        endIndexOfFirstPH = content.indexOf("$", startIndexOfFirstPH + 2);

        if (startIndexOfFirstPH >= 0 && endIndexOfFirstPH >= 0) {
            placeHolder = content.substr((startIndexOfFirstPH + 2), (endIndexOfFirstPH - startIndexOfFirstPH) - 2);
            if (placeHolders.indexOf(placeHolder) < 0)
            { placeHolders.push(placeHolder); }
            content = content.substring(endIndexOfFirstPH + 2);
        } else loop = false;
    } while (loop);

    return placeHolders;
}

















