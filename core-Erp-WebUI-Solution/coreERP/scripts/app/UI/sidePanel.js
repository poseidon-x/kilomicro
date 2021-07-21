
function getHeaders(menuType) {
    var sideMenuApiUrl = coreERPAPI_URL_Root + "/ui/UIAPI/GetMenuHeaders?authToken=" + authToken + "&menuType=" + menuType;
    
    var headerItems = new kendo.data.DataSource({
        transport: {
            read: {
                accept: "application/json",
                type: "GET",
                url: sideMenuApiUrl
            },
        },
        schema: {
            data: function (response) { 

                return response;
            },
            model: {
                id: "itemId",
                fields: {
                    itemId: { type: "number" },
                    text: { type: "string" },
                    encoded: { type: "boolean" },
                    expanded: { type: "boolean" },
                    content: { type: "string" }, 
                    menuType: { type: "number"}
                }
            },
        },
    });

    return dataSource;
}

function loadPanel(panel, menuType) {
    var sideMenuApiUrl = coreERPAPI_URL_Root + "/ui/UIAPI/GetMenuHeaders?authToken=" + authToken + "&menuType=" + menuType;
    var dataSource = [];
    $.getJSON(sideMenuApiUrl, function (data) {
        dataSource = data;
        var panelbar = $("#" + panel).kendoPanelBar({
            expandMode: "single",
            dataSource: dataSource, //need to convert data to JSON array
        });
    });
}

function onSelect(e) {
    var detailItem = $(e.item);  // current panelbar item

    var itemId = that.headerItems .data()[detailItem.index()].itemId;
    var sideMenuApiUrl = coreERPAPI_URL_Root + "/ui/UIAPI/GetMenuDetails?authToken="
        + authToken + "&menuType=" + menuType + "&itemId=" + itemId; // load id from datasource
    $.ajax({
        url: sideMenuApiUrl,
        type: "GET",
        accept: "application/json",
        success: function (item) {
            var detailTemplate = kendo.template($("#sideMenuDetailTemplate")); // load detail template
            var compiledHTML = detailTemplate(detailData); // fill template with data
            detailItem.find(".panelbar-item-content").html(compiledHTML); //add filled detail template to panelbar item.
        },
        error: function (result) {
            //alert('error');
        }
    });
}