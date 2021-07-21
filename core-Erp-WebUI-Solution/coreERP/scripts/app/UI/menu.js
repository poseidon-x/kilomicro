/*
UI Scripts for Loan Category Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var menuApiUrl = coreERPAPI_URL_Root + "/ui/UIAPI/GetMenus?authToken=" + authToken;

$(function () {
    var menuData = [];
    $.getJSON(menuApiUrl, '', function (data) {
        menuData = data;

        var ui = new menuUI();
        ui.renderMenu(menuData);
    });
});

var menuUI = (function () {
    function menuUI() {
    }
    menuUI.prototype.renderMenu = function (menuData) {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#ajaxMenu").kendoMenu({
            dataSource: menuData,
            openOnClick: true
        });
    };
    return menuUI;
})();
//# sourceMappingURL=menu.js.map
