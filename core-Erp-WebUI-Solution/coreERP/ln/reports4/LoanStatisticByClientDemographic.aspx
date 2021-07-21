<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="LoanStatisticByClientDemographic.aspx.cs" Inherits="coreERP.ln.reports4.LoanStatisticByClientDemographic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loan Statistics Report
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
        <div class="col-md-2">Type</div>
        <div class="col-md-2"><input id="demoType"/></div>
        <div class="col-md-2">As At</div>
        <div class="col-md-2"><input id="monthEndDate"/></div>
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
            $('#demoType')
                .kendoComboBox({
                    dataSource: [{ type: 1, typeName: 'By Age Group' }, { type: 2, typeName: 'By Amount Disbursed' }],
                    dataTextField: 'typeName',
                    dataValueField: 'type',
                    value: 1,
                    change: function () {
                        loadReport();
                    }
                });
            $('#monthEndDate')
                .kendoDatePicker({
                    value: new Date(),
                    format: 'dd-MMM-yyyy',
                    change: function() {
                        loadReport();
                    }
                });
        });

        function loadReport() {
            $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
            $("#reportViewer1")
            .telerik_ReportViewer({
                serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
                
                reportSource: {
                    report: "coreData.Reports.BOG.LoanStatsByDemographic, coreData",
                    parameters: {
                        demographicType: $('#demoType').data('kendoComboBox').value(),
                        monthEndDate: $('#monthEndDate').data('kendoDatePicker').value()
                    },
                },
                persistSession: false,
            });
        }

    </script>
</asp:Content>
