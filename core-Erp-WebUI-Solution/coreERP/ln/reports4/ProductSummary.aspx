<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="ProductSummary.aspx.cs" Inherits="coreERP.ln.reports4.ProductSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Product Summary Report
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
            loadReport();
        });

        function loadReport() {
            $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
            $("#reportViewer1")
            .telerik_ReportViewer({
                serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
                
                reportSource: {
                    report: "coreData.Reports.ProductSummary, coreData",
                    parameters: {
                        endDate: null, 
                    },
                },
                persistSession: false,
            });
        }

    </script>
</asp:Content>
