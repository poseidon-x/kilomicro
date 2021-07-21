/*
UI Scripts for Loan Category Management
Creator: kofi@acsghana.com
*/
// jQuery ready function ensures DOM is loaded before the code inside is executed
var authToken = coreERPAPI_Token;
var flagApiUrl = coreERPAPI_URL_Root + "/crud/SavingsPlanFlag";
var savingTypeApiUrl = coreERPAPI_URL_Root + "/crud/SavingType";

var savingTypes = [];

$(function () {
    $.ajax({
        url: savingTypeApiUrl + "/Get",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data2) {
        savingTypes = data2;

        var ui = new flagUI();
        ui.renderGrid();

        grid = $("#setupGrid").data("kendoGrid");
        grid.table.on("click", ".checkbox", selectRow);
        $("#hcheckbox").click(function () {
            selectAllRows(this.checked);
        });


        $("#approveSelected").bind("click", function () {
            for (var i = 0; i < checkedIds.length; i++) {
                var item = checkedIds[i];
                if (item.checked === true) {
                    $.ajax({
                        url: flagApiUrl + "/Apply/" + item.savingPlanFlagId,
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }).done(function (data) {
                        if (data != null) {
                            if (!alerted) {
                                alerted = true;
                            }
                        }
                    });
                }
            }

            setTimeout(function () {
                if (alerted) {
                    $("#setupGrid").data("kendoGrid").dataSource.read();
                    alert("Savings Account Plan Migration Sucessuly Applied.");
                }
            }, 3000);
        });

        $("#rejectSelected").bind("click", function () {
            for (var i = 0; i < checkedIds.length; i++) {
                var item = checkedIds[i];
                if (item.checked === true) {
                    $.ajax({
                        url: flagApiUrl + "/Reject/" + item.savingPlanFlagId,
                        type: "POST",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    }).done(function (data) {
                        if (data != null) {
                            if (!alerted) {
                                alerted = true;
                            }
                        }
                    });
                }
            }

            setTimeout(function () {
                if (alerted) {
                    $("#setupGrid").data("kendoGrid").dataSource.read();
                    alert("Savings Account Plan Migration Successfully Rejected.");
                }
            }, 3000);
        });

    });
});
var flagUI = (function () {
    function flagUI() {
    }
    flagUI.prototype.renderGrid = function () {
        // use jQuery a selector to get the div with an id of peopleGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        var grid  = $("#setupGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: flagApiUrl + "/GetUnApplied",
                        beforeSend: function (req) {
                            req.setRequestHeader('Authorization', "coreBearer " + authToken);
                        }
                    },
                },
                pageSize: 20,
                schema: {
                    model: {
                        id: "savingPlanFlagID",
                        fields: {
                            savingPlanFlagID: {
                                editable: false,
                                type: "number"
                            }, 
                            saving: {
                                editable: false, 
                            },
                            flagDate: {
                                editable: false, 
                                type: "date"
                            },
                            currentPlanId: {
                                editable: false,
                                type: "number"
                            }, 
                            proposedPlanId: {
                                editable: true,
                                type: "number"
                            },  
                        }
                    }
                }
            }, 
            columns: [
                {
                    template: "<input type='checkbox' class='checkbox' />",
                    headerTemplate: "<input type='checkbox' id='hcheckbox' />",
                    width: 30
                },
                {
                    field: "savingPlanFlagID", title: "Client Name",
                    template: "#= saving.client.surName + ', ' + saving.client.otherNames #"
                },
                {
                    field: "savingPlanFlagID", title: "Account No",
                    template: "#= saving.savingNo #",
                    width: 100
                },
                {
                    field: "flagDate", title: "Flagged Date",
                    format: "{0:dd-MMM-yyyy}",
                    width: 130
                },
                {
                    field: "currentPlanId", title: "Current Product",
                    template: "#= getSavingType(currentPlanId) #"
                },
                {
                    field: "proposedPlanId", title: "Proposed Product",
                    template: "#= getSavingType(proposedPlanId) #"
                },
                {
                    command: [{ text: "Apply", click: apply }, { text: "Reject", click: reject }],width:170,
                }
            ], 
            sortable: true,
            filterable: true, 
            pageable: true,
            selectable: "multiple"
        });
    };

    return flagUI;
})();
 
var alerted = false;
var checkedIds = [];

function selectAllRows(flag) {
    grid = $("#setupGrid").data("kendoGrid");
    if (flag) {
        grid.select(grid.tbody.find(">tr"));
    }
    else {
        grid.clearSelection();
    }

    var i = 0;
    grid.tbody.find("tr").each(function () {
        var $this = $(this),
            ckbox;
        ckbox = $this.find(".checkbox");
        ckbox.prop("checked", flag);
        checkedIds.push({
            savingPlanFlagId: grid.dataItem($this).savingPlanFlagID,
            checked: flag,
        });
        i = i + 1;
    });

}

//on click of the checkbox:
function selectRow() {
    var flag = this.checked,
    row = $(this).closest("tr"),
    grid = $("#setupGrid").data("kendoGrid"),
    dataItem = grid.dataItem(row);

    var savingPlanFlagId = dataItem.savingPlanFlagID;
    var found = false;
    for (var item2 in checkedIds) {
        if (item2.savingPlanFlagId === savingPlanFlagId) {
            item2.checked = flag;
            found = true;
            break;
        }
    }
    if (found === false) {
        checkedIds.push({
            savingPlanFlagId: savingPlanFlagId,
            checked: flag,
        });
    }
    if (flag) {
        //-select the row
        row.addClass("k-state-selected");
    } else {
        //-remove selection
        row.removeClass("k-state-selected");
    }
}

function getSavingType(savingTypeId) {
    var name = "";

    for (var i = 0; i < savingTypes.length; i++) {
        if (savingTypes[i].savingTypeID == savingTypeId) {
            name = savingTypes[i].savingTypeName;
        }
    }

    return name;
}
 
function apply(e) {
    e.preventDefault();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var savingPlanFlagId = dataItem.savingPlanFlagID;
    $.ajax({
        url: flagApiUrl + "/Apply/" + savingPlanFlagId,
        type: "POST",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        if (data != null) {
            alert("Savings Account Plan Migration Successfully Applied.");
            $('#setupGrid').data('kendoGrid').dataSource.read();
        }
    }).error(function (err) {
        if (err != null) {
            alert("Error: " + err.toString());
            $('#setupGrid').data('kendoGrid').dataSource.read();
        }
    });
}
function reject(e) {
    e.preventDefault();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var savingPlanFlagId = dataItem.savingPlanFlagID;
    $.ajax({
        url: flagApiUrl + "/Reject/" + savingPlanFlagId,
        type: "POST",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        if (data != null) {
            alert("Savings Account Plan Migration Successfully Rejected.");
            $('#setupGrid').data('kendoGrid').dataSource.read();
        }
    }).error(function (err) {
        if (err != null) {
            alert("Error: " + err.toString());
            $('#setupGrid').data('kendoGrid').dataSource.read();
        }
    });
}