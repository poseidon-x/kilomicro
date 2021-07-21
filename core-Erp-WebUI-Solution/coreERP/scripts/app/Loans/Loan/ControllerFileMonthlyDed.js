
/*---------------------------------------------------------------------------------------------------*/

/*
Creator: bmensa-bonsu@acsghana.com
*/

"use strict";


var conrollerApiUrl = coreERPAPI_URL_Root + "/crud/ControllerFile";
//var repTypeApiUrl = coreERPAPI_URL_Root + "/crud/ControllerFile";
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
    url: conrollerApiUrl + '/GetAllControllerFile',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});

//var repTypeAjax = $.ajax({
//    url: repTypeApiUrl + '/GetContRepaymentType',
//    type: 'Get',
//    beforeSend: function (req) {
//        req.setRequestHeader('Authorization', "coreBearer " + authToken);
//    }
//});

var remarksAjax = $.ajax({
    url: remarksApiUrl + '/GetRemarks',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


function loadData() {
    $.when(controllerAjax,  remarksAjax)
		.done(function (dataController, dataRemarks) {
		    controllerFiles = dataController[0];
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

//function getContRepType(id) {
//    if (id != null)
//        for (var i = 0; i < controllerRepaymentType.length; i++) {
//            if (controllerRepaymentType[i].controllerRepaymentTypeId == id) {
//                return controllerRepaymentType[i].description;
//            }
           
//        }

//    else if (id == null || id == undefined) {
//        return "";
//    }
//}

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
        url: coreERPAPI_URL_Root + '/crud/ControllerFile/GetControllerFileOutstanding/' + fileID,
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
                    entries.success(entries.data);
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
                        loanNo: { editable: true },
                        amountDisbursed: { editable: true },



                    }
                }
            },
                group: [
                { field: "staffID" },
         ]
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
                      field: 'disbursementDate',
                      title: 'Disbursement Date',
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
                        width: "9%",
                        editor: nonEditor,

                    },

                     {
                         field: 'overage',
                         title: 'Overage',
                         filterable: true,
                         sortable: true,
                         editor: nonEditor,
                         width: "9%"

                     },

                     {
                         field: 'loanNo',
                         title: 'Loan No.',
                         filterable: true,
                         sortable: true,
                         width: "9%",
                         editor: nonEditor,
                         template: '#= getLoans(loanNo) #',

                     },

                     {
                         field: 'remarks',
                         title: 'Remarks',
                         filterable: true,
                         sortable: true,
                         width: "10%",
                         editor: nonEditor,
                         template: '#= getRemarks(remarks) #',

                     },

                      {
                          field: 'amountDisbursed',
                          title: 'Amount Disbursed',
                          filterable: true,
                          sortable: true,
                          width: "10%",

                      },

                      
                    
                   //  { command: [{ name: "edit", text: "Edit" }] }

            ],

            toolbar: [
            { name: "pdf", text: "Export to PDF" },
            { name: "excel", text: "Export to Excel" },

            ],
            pdf: {
                landscape: true,
                paperSize: "A3",
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

    //function contTypeEditor(container, options) {
    //    $('<input type="text" data-text-field ="description" data-value-field="controllerRepaymentTypeId" data-bind="value:' + options.field + '"/>')
    //    .appendTo(container)
    //    .kendoComboBox({
    //        dataSource: {
    //            data: controllerRepaymentType
    //        },
    //        dataValueField: 'controllerRepaymentTypeId',
    //        autoBind: true,
    //        dataTextField: 'description',
    //        optionLabel: " ",
    //    });
    //}

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
            url: coreERPAPI_URL_Root + "/crud/ControllerFile/ControllerFileOutstandingUpdate",
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(loanObj),
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            dismissLoadingDialog();
            successDialog('Updated Successfully.',
            'SUCCESS', function () { window.location = "/"; });
        }).error(function (xhr, data, error) {
            dismissLoadingDialog();
            alert(xhr.responseJSON.ExceptionMessage);
        });
    }


}