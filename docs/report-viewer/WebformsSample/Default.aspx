<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Webforms_sample._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height: 650px;width: 950px;min-height:404px;">
        <Bold:ReportViewer runat="server" ID="viewer" ReportPath="~/Resources/company-sales(json).rdl"
            ReportServiceUrl="/api/ReportViewer">
        </Bold:ReportViewer>
    </div>
    <link href="Content/bold-reports/material/bold.reports.all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--Render the gauge item. Add these scripts only if your report contains the gauge report item.-->
    <script src="Scripts/bold-reports/common/ej2-base.min.js"></script>
    <script src="Scripts/bold-reports/common/ej2-data.min.js"></script>
    <script src="Scripts/bold-reports/common/ej2-pdf-export.min.js"></script>
    <script src="Scripts/bold-reports/common/ej2-svg-base.min.js"></script>
    <script src="Scripts/bold-reports/data-visualization/ej2-circulargauge.min.js"></script>
    <script src="Scripts/bold-reports/data-visualization/ej2-lineargauge.min.js"></script>

    <!--Renders the map item. Add this script only if your report contains the map report item.-->
    <script src="Scripts/bold-reports/data-visualization/ej2-maps.min.js"></script>
    <script src="Scripts/bold-reports/common/bold.reports.common.min.js"></script>
    <script src="Scripts/bold-reports/common/bold.reports.widgets.min.js"></script>

    <!--Renders the chart item. Add this script only if your report contains the chart report item.-->
    <script src="Scripts/bold-reports/data-visualization/ej.chart.min.js"></script>

    <!-- Report Viewer component script-->
    <script src="Scripts/bold-reports/bold.report-viewer.min.js"></script>
</asp:Content>
