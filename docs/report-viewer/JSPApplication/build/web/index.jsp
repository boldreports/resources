<%-- 
    Document   : index
    Created on : Apr 21, 2020, 4:21:55 PM
    Author     : LingarajS
--%>

<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
<link href="https://cdn.boldreports.com/2.2.28/content/material/bold.reports.all.min.css"  rel="stylesheet" />
<script src="https://cdn.boldreports.com/external/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="https://cdn.boldreports.com/2.2.28/scripts/common/bold.reports.common.min.js"></script>
<script src="https://cdn.boldreports.com/2.2.28/scripts/common/bold.reports.widgets.min.js"></script>

<!--Used to render the chart item. Add this script only if your report contains the chart report item.-->
<script src="https://cdn.boldreports.com/2.2.28/scripts/data-visualization/ej.chart.min.js"></script>

<!--Used to render the gauge item. Add this script only if your report contains the gauge report item. -->
<script src="https://cdn.boldreports.com/2.2.28/scripts/data-visualization/ej.lineargauge.min.js"></script>
<script src="https://cdn.boldreports.com/2.2.28/scripts/data-visualization/ej.circulargauge.min.js"></script>

<!--Used to render the map item. Add this script only if your report contains the map report item.-->
<script src="https://cdn.boldreports.com/2.2.28/scripts/data-visualization/ej.map.min.js"></script>

<!-- Report Viewer component script-->
<script src="https://cdn.boldreports.com/2.2.28/scripts/bold.report-viewer.min.js"></script>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>JSP Page</title>
    </head>
<div style="height: 600px; width: 950px;">
    <!-- Creating a div tag which will act as a container for boldReportViewer widget.-->
    <div style="height: 600px; width: 950px; min-height: 400px;" id="viewer"></div>
    <!-- Setting property and initializing boldReportViewer widget.-->
    <script type="text/javascript">
        $(function () {
             $("#viewer").boldReportViewer({
                reportServiceUrl: "https://demos.boldreports.com/services/api/ReportViewer",
                reportPath: '~/Resources/docs/sales-order-detail.rdl'
            });
        });
    </script>
</div>
</html>
