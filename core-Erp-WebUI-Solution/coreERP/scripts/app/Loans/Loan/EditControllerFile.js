
/*---------------------------------------------------------------------------------------------------*/

/*
Creator: bmensa-bonsu@acsghana.com
*/

"use strict";


var conrollerApiUrl = coreERPAPI_URL_Root + "/crud/ControllerFile";
var repTypeApiUrl = coreERPAPI_URL_Root + "/crud/ControllerFile";
var remarksApiUrl = coreERPAPI_URL_Root + "/crud/ControllerFile";


var loanObj = {};
var controllerFiles = {};
var loan = [];
var fileID;
var myObj = {};
var controllerRepaymentType = {};
var remarks = {};
var loanNo = [];

var controllerAjax = $.ajax({
    url: conrollerApiUrl + '/GetControllerFile',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var repTypeAjax = $.ajax({
    url: repTypeApiUrl + '/GetContRepaymentType',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

var remarksAjax = $.ajax({
    url: remarksApiUrl + '/GetRemarks',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


function loadData() {
    $.when(controllerAjax, repTypeAjax, remarksAjax)
		.done(function (dataController, dataRepType, dataRemarks) {
		    controllerFiles = dataController[0];
		    controllerRepaymentType = dataRepType[0];
		    remarks = dataRemarks[0];
		    var ui = new oUI();
		    ui.prepareUI();
		    //Prepares UI

		}
	);
}




$(function () {
    //displayLoadingDialog();
    loadData();
    $("#saveid").hide();
    

});
var fileChange = function () {
    $("#retrieve").prop("disabled", true);
    var loan = [];
    fileID = $("#file").data("kendoDropDownList").value();
    console.log("fileID ", fileID);

    $.ajax({
        url: coreERPAPI_URL_Root + '/crud/ControllerFile/GetLoanNo/' + fileID,
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        loanNo = data;
        console.log("data is ", data)
        $("#retrieve").prop("disabled", false);

    });

}
console.log("loan object is", loanNo);

var oUI = (function () {
    function oUI() {
    }
    oUI.prototype.prepareUI = function () {

        $("#file").width("100%").kendoDropDownList({
            dataSource: controllerFiles,
            filter: "contains",
            suggest: true,
            dataValueField: "fileID",
            dataTextField: "fileName",
            highlightFirst: true,
            change: fileChange,
            ignoreCase: true,
            optionLabel: " ",

            activate: function () {
                $('#file').focus();
            }
        });


        $('#retrieve').kendoButton({
            click: function (e) {
                displayLoadingDialog();
                grid();
                $("#saveid").show();


            }

        });

    }



    return oUI;
})();


//make column uneditable
function nonEditor(container, options) {
    container.text(options.model[options.field]);
    container.removeClass("k-edit-cell");
}

function getContRepType(id) {
    if (id != null)
        for (var i = 0; i < controllerRepaymentType.length; i++) {
            if (controllerRepaymentType[i].controllerRepaymentTypeId == id) {
                return controllerRepaymentType[i].description;
            }
           
        }

    else if (id == null || id == undefined) {
        return "";
    }
}

function getRemarks(id) {
    if (id != null)
        for (var i = 0; i < remarks.length; i++) {
            if (remarks[i].controllerRemarksId == id) {
                return remarks[i].controllerRemarksId;
            }

        }

    else if (id == null || id == undefined) {
        return "";
    }
}

function getLoans(id) {
    if (id != null)
        for (var i = 0; i < loanNo.length; i++) {
            if (loanNo[i].loanNo == id) {
                return loanNo[i].loanNo;
            }

        }

    else if (id == null || id == undefined || id =="") {
        return "";
    }
}



function grid() {

    $.ajax({
        url: coreERPAPI_URL_Root + '/crud/ControllerFile/GetControllerFileDetails/' + fileID,
        type: 'Get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }
    }).done(function (data) {
        myObj = data;
        console.log("this is all data", data)
        dismissLoadingDialog();

         $("#grid").kendoGrid({
            dataSource: {
            transport: {
                read: function (entries) {
                    entries.success(myObj);
                },
                create: function (entries) {
                    entries.success(entries.data);
                },
                update: function (entries) {
                    for (var i = 0; i < myObj.length; i++) {
                        if (myObj[i].fileDetailID == entries.data.fileDetailID) {
                            if (entries.data.controllerRepaymentTypeId && entries.data.controllerRepaymentTypeId.controllerRepaymentTypeId) {
                                myObj[i].controllerRepaymentTypeId = entries.data.controllerRepaymentTypeId.controllerRepaymentTypeId;
                                entries.data.controllerRepaymentTypeId = entries.data.controllerRepaymentTypeId.controllerRepaymentTypeId;
                            }
                            else {
                                myObj[i].controllerRepaymentTypeId = entries.data.controllerRepaymentTypeId;
                            }

                            if (entries.data.remarks && entries.data.remarks.controllerRemarksId) {
                                myObj[i].remarks = entries.data.remarks.controllerRemarksId;
                                entries.data.remarks = entries.data.remarks.controllerRemarksId;
                            }
                            else {
                                myObj[i].remarks = entries.data.remarks;
                            }

                            if (entries.data.loanNo && entries.data.loanNo.loanNo) {
                                myObj[i].loanNo = entries.data.loanNo.loanNo;
                                entries.data.loanNo = entries.data.loanNo.loanNo;
                            }
                            else {
                                myObj[i].loanNo = entries.data.loanNo;
                            }
                            if (entries.data.overage && entries.data.overage.overage) {
                                myObj[i].overage = entries.data.overage.overage;
                                entries.data.overage = entries.data.overage.overage;
                            }
                            else {
                                myObj[i].overage = entries.data.overage;
                            }
                        }
                    }
                    entries.success(entries.data);
                    $("#grid").data('kendoGrid').dataSource.data(myObj);
                },
               
                destroy: function (entries) {
                    entries.success(entries.data);
                },
                parameterMap: function (data) { return JSON.stringify(data); },
            },
            schema: {
                model: {
                    id: "fileDetailID",
                    fields: {
                        fileDetailID: { editable: false, type: "number" },
                        staffID: {editable:true},
                        managementUnit: { editable: false },
                        employeeName: { editable: true},
                        origAmt: { editable: true },
                        monthlyDeduction: { editable: true },
                        remarks: { editable: true },
                        controllerRepaymentTypeId: { editable: true },
                        loanNo: { editable: true },


                    }
                }
            }
            },
            columns: [

                 {
                     field: 'staffID',
                     title: 'Staff ID',
                     filterable: true,
                     sortable: true,
                     editor: nonEditor,
                     width: "8%"

                 },


                {
                    field: 'managementUnit',
                    title: 'Management Unit',
                    filterable: true,
                    sortable: true,
                    editor: nonEditor,
                    width: "12%"

                },

                 {
                     field: 'employeeName',
                     title: 'Employee Name',
                     filterable: true,
                     sortable: true,
                     editor: nonEditor,
                    width: "10%"

                 },

                  {
                      field: 'origAmt',
                      title: 'Original Amount',
                      filterable: true,
                      sortable: true,
                      editor: nonEditor,
                      width: "9%"

                  },

                    {
                        field: 'monthlyDeduction',
                        title: 'Monthly Deduction',
                        filterable: true,
                        sortable: true,
                        editor: nonEditor,
                        width: "9%"

                    },

                     {
                         field: 'overage',
                         title: 'Overage',
                         filterable: true,
                         sortable: true,
                         width: "9%"

                     },

                     {
                         field: 'loanNo',
                         title: 'Loan No.',
                         filterable: true,
                         sortable: true,
                         width: "9%",
                         editor: loansEditor,
                         template: '#= getLoans(loanNo) #',

                     },

                     {
                         field: 'remarks',
                         title: 'Remarks',
                         filterable: true,
                         sortable: true,
                         width: "10%",
                         editor: remarksEditor,
                         template: '#= getRemarks(remarks) #',

                     },

                       {
                           field: 'controllerRepaymentTypeId',
                           title: 'Controller Repayment Type',
                           filterable: true,
                           sortable: true,
                           editor: contTypeEditor,
                           template: '#= getContRepType(controllerRepaymentTypeId) #',
                           width: "15%"

                       },


                    
                     { command: [{ name: "edit", text: "Edit" }] }

            ],

            toolbar: [
            { name: "pdf", text: "Export to PDF" },
            { name: "excel", text: "Export to Excel" },

            ],
            pdf: {
                landscape: true,
                paperSize: "A4",
                fileName: "ControllerFileDetail.pdf", allPages: true
            },
            excel: {
                fileName: "ControllerFileDetail.xlsx", allPages: true
            },
            filterable: true,
            sortable: {
                mode: "multiple",
            },
            editable: "inline",
      
            pageable: {
                pageSize: 50,
                pageSizes: [50, 150, 300, 600, 1000],
                previousNext: true,
                buttonCount: 5,
            },
            navigation: true,
            groupable: true,
            selectable: true,
            mobile: true,
            reorderable: true,
            resizable: true

        }).data("kendoGrid");

    });


    $('#savechanges').kendoButton({
        click: function (e) {
            saveChanges();
        }

    });

    function saveChanges() {

        retrieveValues();
       
            displayLoadingDialog();
            saveToServer();
        

    }

    function contTypeEditor(container, options) {
        $('<input type="text" data-text-field ="description" data-value-field="controllerRepaymentTypeId" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            dataSource: {
                data: controllerRepaymentType
            },
            dataValueField: 'controllerRepaymentTypeId',
            autoBind: true,
            dataTextField: 'description',
            optionLabel: " ",
        });
    }

    function remarksEditor(container, options) {
        $('<input type="text" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            dataSource: remarks,
            dataValueField: 'controllerRemarksId',
            dataTextField: 'controllerRemarksId',
            optionLabel: '',
        });
    }

    function loansEditor(container, options) {
        $('<input type="text" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            dataSource: loanNo,
            dataValueField: 'loanNo',
            dataTextField: 'loanNo',
            optionLabel: '',
        });
    }
   

    function retrieveValues() {
        loanObj.controllerFileDetails = $('#grid').data().kendoGrid.dataSource.view();
    }

    function saveToServer() {
        $.ajax({
            url: coreERPAPI_URL_Root + "/crud/ControllerFile/ControllerFileUpdate",
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(loanObj),
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            dismissLoadingDialog();
            successDialog('ControllerFile Updated Successfully.',
            'SUCCESS', function () { window.location = "/"; });
        }).error(function (xhr, data, error) {
            dismissLoadingDialog();
            alert(xhr.responseJSON.ExceptionMessage);
        });
    }


}