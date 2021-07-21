//Declaration of variables to API controllers
var authToken = coreERPAPI_Token;
var documentTemplateApiUrl = coreERPAPI_URL_Root + "/crud/loanDocumentTemplate";

//Declaration of variables to store records retrieved from the database
var creditLine = {};
var clients = {};

$(function () {
    displayLoadingDialog();
    loadForm();
});

function loadForm() {
    $('#tabs').kendoTabStrip();
    renderGrid();
    dismissLoadingDialog();
}

function renderGrid() {
    $('#grid').kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: documentTemplateApiUrl + '/Get',
                    type: 'Post',
                    contentType: 'application/json',
                    beforeSend: function (req) {
                        req.setRequestHeader('Authorization', "coreBearer " + authToken);
                    }
                },
                create: function (entries) {
                    entries.success(entries.data);
                },
                
                destroy: {

                },
                parameterMap: function (data) {
                    return JSON.stringify(data);
                },

            }, //transport
            pageSize: 10,
            schema: {
                // the array of repeating data elements (depositType)
                data: "Data",
                // the total count of records in the whole dataset. used
                // for paging.
                total: "Count",
                model: {
                    id: 'loanDocumentTemplateId',
                    fields: {
                        loanDocumentTemplateId: { type: 'number', defaultValue: 0 },
                        templateName: { type: 'string', editable: false }
                    } //fields
                } //model
            }, //schema
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        }, //datasource
        columns: [
            { field: 'templateName', title: 'Name' },
            { command: [templateEditButton, templateDeleteButton] }
        ],
        toolbar: [
            {
                className: 'addNewDocument',
                text: 'Create New Document'
            },
            "pdf",
            "excel"
        ], //toolbar  


        excel: {
            fileName: "DocumentTemplates.xlsx"
        },
        pdf: {
            paperKind: "A3",
            landscape: true,
            fileName: "DocumentTemplates.pdf"
        },
        filterable: true,
        sortable: {
            mode: "multiple"
        },
        editable: "popup",
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5
        },
        groupable: true,
        selectable: true
    });

    $(".addNewDocument").click(function () {
        window.location = "/DocumentTemplate/CreateLoanDocumentTemplate";
    });
}


var templateEditButton = {
    name: "edit",
    text: "Edit",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);

        window.location = "/DocumentTemplate/CreateLoanDocumentTemplate/" + data.loanDocumentTemplateId.toString();

    },

};


var templateDeleteButton = {
    name: "delete",
    text: "Delete",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        var id = data.loanDocumentTemplateId;

        $.ajax({
            url: documentTemplateApiUrl + "/DeleteDocument/" + id,
            type: "Delete",
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).success(function (data) {
            //On success stop loading Dialog and alert a success message
            dismissLoadingDialog();
            successDialog('Loan Document Template Successfully Deleted', 'SUCCESS');
        }).error(function (xhr, data, error) {
            //On error stop loading Dialog and alert a the specific message
            dismissLoadingDialog();
            warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
        });


    },

};
