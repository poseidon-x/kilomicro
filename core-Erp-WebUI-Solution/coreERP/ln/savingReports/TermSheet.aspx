<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="TermSheet.aspx.cs" Inherits="coreERP.ln.savingReports.TermSheet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Investment Term Sheet
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
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Client Name:
            </div>
            <div class="subFormInput">
                <input type="text" id="client"/>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Investment Account:
            </div>
            <div class="subFormInput">
                <input type="text" id="saving"/>
            </div>
        </div>
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
            loadClients();
        });

        function loadReport(savingId) {
            $("#reportViewer1").replaceWith('<div id="reportViewer1" class="k-widget">');
            $("#reportViewer1")
            .telerik_ReportViewer({
                serviceUrl: coreERPAPI_URL_Root + "/Reporting/",
                
                reportSource: {
                    report: "coreData.Reports.Savings.SavingsTermSheet, coreData",
                    parameters: {
                        savingId: savingId,
                    },
                },
                persistSession: false,
            });
        }

        function loadClients() {
            $('#client').replaceWith('<input type="text" id="client"/>');
            $('#client').kendoComboBox({
                    dataSource: {
                        transport: {
                            read: {
                                url: coreERPAPI_URL_Root + "/crud/ClientLookUp/GetSavingsClients",
                                type: 'GET',
                                contentType: 'application/json',
                                beforeSend: function(req) {
                                    req.setRequestHeader('Authorization', "coreBearer " + authToken);
                                },
                            }
                        }
                    },
                    dataValueField: 'clientID',
                    dataTextField: 'clientName',
                    optionLabel: ' ',
                    change: function(e) {
                        loadSavings(this.value());
                    }
                });
        }

        function loadSavings(clientId) {
            $('#saving').replaceWith('<input type="text" id="saving"/>');
            $('#saving').kendoComboBox({
                    dataSource: {
                        transport: {
                            read: {
                                url: coreERPAPI_URL_Root + "/crud/Saving/Get/" + clientId,
                                type: 'GET',
                                contentType: 'application/json',
                                beforeSend: function(req) {
                                    req.setRequestHeader('Authorization', "coreBearer " + authToken);
                                },
                            }
                        }
                    },
                    dataValueField: 'savingID',
                    dataTextField: 'savingNo',
                    optionLabel: ' ',
                    change: function(e) {
                        loadReport(this.value());
                    }
                });
        }

    </script>
</asp:Content>
