/*
UI Scripts for Credit Union Membership Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var chapterApiUrl = coreERPAPI_URL_Root + "/crud/CreditUnionChapter";
var memberApiUrl = coreERPAPI_URL_Root + "/crud/CreditUnionMember";
var memberPdfExportUrl = coreERPAPI_URL_Root + "/Export/CreditUnionMember/Pdf?token=" + authToken; 
var clientApiUrl = coreERPAPI_URL_Root + "/crud/Client";

var accounts = [];
var chapters = [];
var clients = [];

$(function () {
    $.ajax({
        url: chapterApiUrl + "/ChapterLookUp",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        chapters = data;
        $.ajax({
            url: clientApiUrl + "/Get",
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            clients = data;

            var ui = new memberUI();
            ui.renderGrid();
            $("#toolbar").click(exportToPdf);

        });
    });
});

var memberUI = (function () {
    function memberUI() {
    }
    memberUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: memberApiUrl + "/Get",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    create: {
                        url: memberApiUrl + "/Post",
                        type: "POST",
                        contentType: "application/json",
                        accepts: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    update: {
                        url: memberApiUrl + "/Put",
                        type: "PUT",
                        contentType: "application/json",
                        accepts: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    destroy: {
                        url: memberApiUrl + "/Delete",
                        type: "DELETE",
                        contentType: "application/json",
                        accepts: "application/json",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                    parameterMap: function (data, type) {
                        return JSON.stringify(data);
                    }
                },
                pageSize: 20,
                schema: {
                    model: {
                        id: "creditUnionMemberID",
                        fields: {
                            creditUnionMemberID: {
                                editable: false,
                                type: "number"
                            }, 
                            creditUnionChapterID: {
                                editable: true,
                                validation: {
                                    required: true
                                },
                                type: "number"
                            },
                            joinedDate: {
                                editable: true,
                                validation: {
                                    required: true
                                },
                                type:"date"
                            },
                            clientID: {
                                editable: true,
                                validation: {
                                    required: true
                                }
                            },
                            sharesBalance: {
                                editable: false,
                                validation: {
                                    required: true
                                },
                                type: "number",
                                defaultValue: 0,
                            }, 
                        }
                    }
                }
            }, 
            columns: [ 
                {
                    field: "clientID", title: "Client Name", width: 450, editor: this.clientDropDownEditor,
                    template: "#= getClientName(clientID) #"
                },
                {
                    field: "creditUnionChapterID", title: "Chapter Name", editor: this.chapterDropDownEditor, width: 400,
                    template: "#= getChapterName(creditUnionChapterID) #"
                },
                { field: "joinedDate", title: "Date Joined", editor: dateEditor, format: "{0:dd-MMM-yyyy}" },
                { field: "sharesBalance", title: "Balance of Shares", format: "{0:#,##0.#0}" },  
                { command: ["edit"] }
            ],
            toolbar: [
                { name: "create", text: "Add Member" },
                {
                    name: "pdf",
                    text: "Export to PDF",
                    imageClass: "k-icon k-i-custom",
                    imageUrl: "/images/pdf.jpg",
                    url: memberPdfExportUrl,
                    template: kendo.template($("#toolbar-icon-pdf").html())
                }
            ],
            sortable: true,
            filterable: true,
            editable: "popup",
            pageable: true,
            edit: function (e) {
                var editWindow = this.editable.element.data("kendoWindow");
                editWindow.wrapper.css({ width: 700 });
                editWindow.title("Credit Union Membership Manager");
            }
        });
    };
     
    memberUI.prototype.chapterDropDownEditor = function (container, options) {
        $('<input required data-text-field="Description" data-value-field="ID" data-bind="value:' + options.field + '"/>')
            .width(450)
            .appendTo(container)
            .kendoDropDownList({
                optionLabel: " ",
                autoBind: false,
                dataSource: chapters
            });
    }

    memberUI.prototype.clientDropDownEditor = function (container, options) {
        $('<input required data-text-field="Description" data-value-field="ID" data-bind="value:' + options.field + '"/>')
            .width(450)
            .appendTo(container)
            .kendoDropDownList({
                optionLabel: " ",
                autoBind: false,
                dataSource: clients
            });
    }

    return memberUI;
})();

var longTextEditor = function (container, options) {
    $('<input data-bind="value:' + options.field + '" style="width:500px;"/>')
        .appendTo(container);
}

var dateEditor = function (container, options){
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDatePicker({ 
        format: "dd-MMM-yyyy"
    });
}

var numericEditor = function (container, options) {
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            decimals: 2,
            min: 0,
            format: '#,##0.#0'
        });
};

function exportToPdf(e) { 
    window.open(memberPdfExportUrl, "_blank");
    return false;
} 

function getChapterName(chapterID) {
    var chapterName = "";

    for (var i = 0; i < chapters.length; i++) {
        if (chapters[i].ID == chapterID) {
            chapterName = chapters[i].Description;
        }
    }

    return chapterName;
}

function getClientName(clientID) {
    var clientName = "";

    for (var i = 0; i < clients.length; i++) {
        if (clients[i].ID == clientID) {
            clientName = clients[i].Description;
        }
    }

    return clientName;
}
