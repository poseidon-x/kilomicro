<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="RegularSusuAccount.aspx.cs" Inherits="coreERP.ln.susu.TelerikReports.GroupSusuStatement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    
    <style>
        #reportViewer1 {
            position: absolute;
            left: 5px;
            right: 5px;
            top: 145px;
            bottom: 5px;
            font-family: 'segoe ui', 'ms sans serif';
            overflow: hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div class="row">
        <span>Client Name</span>
        <select id="cboClients" />
        <input type="button" class="btn" id="btnRunReport" onclick="loadReport();" value="Load Report For Selected Client" />
    </div>
    <div id="reportViewer1" class="k-widget">
</div>

    <script src="/Content/reporting/js/telerikReportViewer-14.1.20.618.min.js"></script>

    <script type="text/javascript">
    $.ajaxSetup({
        type: "POST",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        },
    });

    $.ajaxSetup({
        type: "GET",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        },
    });

    $.ajaxSetup({
        type: "OPTIONS",
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', "coreBearer " + authToken);
        },
    });

    $(function () {

        $.ajax({
            url: coreERPAPI_URL_Root + "/crud/ClientLookUp/GetNormalSusuClients",
            type: "GET",
        }).done(function (data) {
            var clients = data;

            $('#cboClients')
               .kendoComboBox({
                   optionLabel: " ",
                   autoBind: true,
                   dataSource: clients,
                   dataValueField: "clientID",
                   dataTextField: "clientName"
               });
        });

    });

    function loadReport() {
        $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
        $("#reportViewer1")
        .telerik_ReportViewer({
            serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
            
            reportSource: {
                report: "coreData.Reports.RegularSusuAccount, coreData",
                parameters: {
                    clientID: $("#cboClients").val(),
                    susuAccountID: null,
                    susuGroupID: null,
                    startDate: null,
                    endDate: null,
                },
            },
            persistSession: false,
        });
    }
    
    </script>
</asp:Content>
