<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exportPage.aspx.cs" Inherits="coreERP.dash.exportPage" %>

<!DOCTYPE >
<html>
<head>
    <title>Export Page</title>  
    <link href="/Content/bootstrap.min.css" rel="stylesheet" /> 
    <link href="/Content/kendo/styles/kendo.common.min.css" rel="stylesheet" /> 
    <link href="/Content/kendo/styles/kendo.bootstrap.min.css" rel="stylesheet" />  
    <link href="/Content/coreThemes/coreTheme.bootstrap.css" rel="stylesheet" /> 
    <link href="/Content/font-awesome/css/font-awesome.min.css" rel="stylesheet" />  
  
    <script src="/scripts/ajax/jquery-2.1.1.js"></script>
    <script src="/Content/kendo/js/kendo.all.min.js"></script>
    <script src="/scripts/libs/jszip.js"></script> 
</head>
<body>
    <form id="form1" runat="server">
         
    </form>
</body>
    
    <script type="text/javascript">
        document.ready = function() {
            var exportType = getParameterByName('exportType');
            if (exportType === 'pdf') {
                exportPdfKendo();
            } else if (exportType === 'img') {
                exportImageKendo();
            } else if (exportType === 'svg') {
                exportSvgKendo();
            }
        };
         
        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        function doneExporting() {
            window.close();
        }

        function exportPdfKendo() {
            // Convert the DOM element to a drawing using kendo.drawing.drawDOM
            kendo.drawing.drawDOM($("#mainContent", window.opener.document))
                .then(function(group) {
                    // Render the result as a PDF file
                    return kendo.drawing.exportPDF(group, {
                        paperSize: "auto",
                        margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
                    });
                })
                .done(function(data) {
                    // Save the PDF file
                kendo.saveAs({
                    dataURI: data,
                    fileName: getParameterByName('pageTitle') + ".pdf",
                    proxyURL: "http://demos.telerik.com/kendo-ui/service/export"
                });
                window.close();
            });
        }

        function exportImageKendo() {
            // Convert the DOM element to a drawing using kendo.drawing.drawDOM
            kendo.drawing.drawDOM($("#mainContent", window.opener.document))
                .then(function(group) {
                    // Render the result as a PNG image
                    return kendo.drawing.exportImage(group);
                })
                .done(function(data) {
                    // Save the image file
                kendo.saveAs({
                    dataURI: data,
                    fileName: getParameterByName('pageTitle') + ".png",
                    proxyURL: "http://demos.telerik.com/kendo-ui/service/export"
                });
                window.close();
            });
        }

        function exportSvgKendo() {
            // Convert the DOM element to a drawing using kendo.drawing.drawDOM
            kendo.drawing.drawDOM($("#mainContent", window.opener.document))
                .then(function(group) {
                    // Render the result as a SVG document
                    return kendo.drawing.exportSVG(group);
                })
                .done(function(data) {
                    // Save the SVG document
                kendo.saveAs({
                    dataURI: data,
                    fileName: getParameterByName('pageTitle') + ".svg",
                    proxyURL: "http://demos.telerik.com/kendo-ui/service/export"
                });
                window.close(); 
                });
        }
    </script>

</html>
