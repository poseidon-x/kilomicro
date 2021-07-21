
/*---------------------------------------------------------------------------------------------------*/

/*
Creator: bmensa-bonsu@acsghana.com
*/

"use strict";


var clientApiUrl = coreERPAPI_URL_Root + "/crud/ClientLookUp";
//var loanApiUrl = coreERPAPI_URL_Root + "/crud/LoanSchedule";

var editObj = {};
var loanObj = {};
var clients = {};
var loan = [];
var clientID;
var myObj = {};

var clientAjax = $.ajax({
    url: clientApiUrl + '/GetLoanClients',
    type: 'Get',
    beforeSend: function (req) {
        req.setRequestHeader('Authorization', "coreBearer " + authToken);
    }
});


function loadData() {
    $.when(clientAjax)
		.done(function (dataClient) {
		    clients = dataClient;
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
var clientChange = function () {
    console.log("Changed");
    var loan = [];
   clientID = $("#client").data("kendoDropDownList").value();
   

   $.ajax({
       url: coreERPAPI_URL_Root + '/crud/LoanSchedule/GetClientLoan/' + clientID,
       type: 'Get',
       beforeSend: function (req) {
           req.setRequestHeader('Authorization', "coreBearer " + authToken);
       }
   }).done(function (data) {
       console.log("loan data", data);
       loan = data;
       var loadDS = $("#loan").data("kendoDropDownList");
       loadDS.setDataSource(loan);
   });
  
}

var loanChange = function () {
    
    var loan = [];
    loanID = $("#loan").data("kendoDropDownList").value();
    console.log("load ID",loanID);

}

var oUI = (function () {
    function oUI() {
    }
    oUI.prototype.prepareUI = function () {

        $("#client").width("100%").kendoDropDownList({
            dataSource: clients,
            filter: "contains",
            suggest: true,
            dataValueField: "clientID",
            dataTextField: "clientName",
            highlightFirst: true,
            change: clientChange,
            ignoreCase: true,
            optionLabel: " ",

            activate: function () {
                $('#client').focus();
            }
        });

        
        $("#loan").width("100%").kendoDropDownList({
            dataSource: loan,
            filter: "contains",
            suggest: true,
            dataValueField: "loanID",
            dataTextField: "Description",
            change: loanChange,
            highlightFirst: true,
            ignoreCase: true,
            optionLabel: " ",
        });





        $('#retrieve').kendoButton({
            click: function (e) {
                grid();
                $("#saveid").show();


            }

        });

      
        

    }



    return oUI;
})();

var gridWidget;
//make column uneditable
function nonEditor(container, options) {
    container.text(options.model[options.field]);
    container.removeClass("k-edit-cell");
}




function grid() {
    $.ajax({
        url: coreERPAPI_URL_Root + '/crud/LoanSchedule/GetLoanSchedule/'+ loanID,
        type: "GET",
        contentType: "application/json",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        }

    }).done(function (data) {
        myObj = data;
        console.log("this is all data", data)


        gridWidget=  $("#grid").kendoGrid({
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
                    id: "repaymentScheduleID",
                    fields: {
                        repaymentScheduleID: { editable: false, type: "number" },
                        repaymentDate: {editable:false, nullable:true},
                        principalPayment: { editable: false },
                        interestPayment: { editable: true},
                        principalBalance: { editable: true },
                        interestBalance: { editable: true},
                    }
                }
            }
            },
            columns: [

                 {
                     field: 'repaymentDate',
                     title: 'Repayment Date',
                     template: "#= kendo.toString(kendo.parseDate(repaymentDate, 'yyyy-MM-dd'), 'dd/MM/yyyy') #",
                     filterable: true,
                     editor: nonEditor,
                     sortable: true,
                     width: "15%"

                 },


                {
                    field: 'principalPayment',
                    title: 'Principal Payment',
                    filterable: true,
                    editor: nonEditor,
                    sortable: true,
                    width: "15%"

                },

                 {
                     field: 'interestPayment',
                     title: 'Interest Payment',
                     filterable: true,
                     editor: nonEditor,
                     sortable: true,
                     width: "15%"

                 },

                  {
                      field: 'principalBalance',
                      title: 'Principal Balance',
                      filterable: true,
                      sortable: true,
                      width: "15%"

                  },

                    {
                        field: 'interestBalance',
                        title: 'Interest Balance',
                        filterable: true,
                        sortable: true,
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
                paperSize: "A3",
                fileName: "RepaymentSchedule.pdf", allPages: true
            },
            excel: {
                fileName: "RepaymentSchedule.xlsx", allPages: true
            },
            filterable: true,
            sortable: {
                mode: "multiple",
            },
            editable: "inline",
      
            pageable: {
                pageSize: 10,
                pageSizes: [10, 25, 50, 100, 1000],
                previousNext: true,
                buttonCount: 5,
            },
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
    function retrieveValues() {
        loanObj.repaymentSchedules = $('#grid').data().kendoGrid.dataSource.view();
    }

    function saveToServer() {
        $.ajax({
            url: coreERPAPI_URL_Root + "/crud/LoanSchedule/ScheduleUpdate",
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(loanObj),
            beforeSend: function (req) {
                req.setRequestHeader('Authorization', "coreBearer " + authToken);
            }
        }).done(function (data) {
            dismissLoadingDialog();
            successDialog('Schedule Updated Successfully.',
            'SUCCESS', function () { window.location = "/"; });
        }).error(function (xhr, data, error) {
            dismissLoadingDialog();
            alert(xhr.responseJSON.ExceptionMessage);
        });
    }


}