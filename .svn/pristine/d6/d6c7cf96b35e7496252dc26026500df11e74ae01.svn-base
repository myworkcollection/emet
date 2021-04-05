<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClosedStatus.aspx.cs" Inherits="Material_Evaluation.ClosedStatus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>eMET</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <!-- Bootstrap core CSS-->
    <%--<link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="Styles/bootstrap-3.4.1-dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" ty

    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet" />

    <link href="Styles/NewStyle/NewStyle.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link href="js/BootstrapDatePcr/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="Scripts/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <style type="text/css">
        .ui-datepicker .ui-datepicker-next {
            background-color: white !important;
        }

        .ui-datepicker .ui-datepicker-prev {
            background-color: white !important;
        }

            .ui-datepicker .ui-datepicker-prev span, .ui-datepicker .ui-datepicker-next span {
                background-color: white;
            }

        .SideBarMenu {
            width: 300px;
        }
    </style>

    <%--Data table style--%>
    <style>
        .WrapCnt td, th {
            white-space: normal !important;
            /*word-wrap: break-word;*/
            font-size: 14px !important;
        }

        .WrapCnt a {
            padding: 0px;
        }

        table.table tr td {
            font: 14px calibri;
        }

        #TableReport_wrapper {
            overflow-x: hidden;
        }

        .dt-button {
            padding: 0px 15px 0px 15px;
        }

        #TableReport_filter {
            float: left !important;
        }

        .dt-buttons {
            float: right !important;
        }

        .dataTables_filter input {
            width: 300px !important;
        }

        table.table thead tr th {
            color: white;
        }

        .Myth th {
            background: url('../../Images/details_open.png') no-repeat center center;
            cursor: pointer;
        }

        td.details-control {
            background: url('../../Images/details_open.png') no-repeat center center;
            cursor: pointer;
        }
        tr.shown td.details-control {
            background: url('../../Images/details_close.png') no-repeat center center;
        }

        #TbMainAllReq_wrapper {
        overflow-x:hidden;
        }
        #TbMainAllReq {
        width:100%!important;
        }

        .details-control {
            padding:0px 3px 0px 3px!important;
            width:1%!important;
        }

        .table-bordered {
        border:0!important;
        border-collapse:collapse!important;
        }
    </style>

    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/moment.min.js"></script>
    
    <script type="text/javascript" src="Scripts/datatables/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/jszip.min.js"></script>

    <%--<script type="text/javascript" src="Scripts/datatables/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/buttons.html5.min.js"></script>--%>

    <script type="text/javascript" src="Scripts/datatables/dataTables.buttons.js"></script>
    <script type="text/javascript" src="Scripts/datatables/buttons.html5.js"></script>


    <script type="text/javascript" src="Scripts/datatables/buttons.flash.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/buttons.print.min.js"></script>

    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/locales/bootstrap-datetimepicker.fr.js"></script>
    <script src="Scripts/Worker-helper.js"></script>

    <script src="Scripts/JavaScript-JSON-Data-Excel-XLSX/jhxlsx.lib.min.js"></script>
    <script src="Scripts/JavaScript-JSON-Data-Excel-XLSX/jszip.min.js"></script>


    <script type="text/javascript">
        var Popup, dataTable;
        var currentPage = 0;
        var DataDet = [];
        var NewObjToExport = [];
        var NewObjToExportDetails = [];
        var mainUrl = "";
        

        $(document).ready(function () {
            mainUrl = window.location.href.replace("ClosedStatus.aspx", "");
            SetUpSideBar();
            DatePitcker();
            GenerateTableReport();
        });

        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });

        $(document).on('keydown', '#TxtFrom', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });

        $(document).on('keydown', '#TxtTo', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });

        function SetUpSideBar() {
            if (sessionStorage.getItem("SidebarMenu") == null) {
                SideBarMenu.style.display = "block";
            }
            else {
                var Display = sessionStorage.getItem("SidebarMenu");
                SideBarMenu.style.display = Display;
            }
        }

        function SidebarMenu() {
            var SideBarMenu = document.getElementById("SideBarMenu");
            if (SideBarMenu.style.display === "none") {
                SideBarMenu.style.display = "block";
                sessionStorage.setItem("SidebarMenu", "block");
            } else {
                //$("#SideBarMenu").toggle(500, "easeOutQuint");
                SideBarMenu.style.display = "none";
                sessionStorage.setItem("SidebarMenu", "none");
            }
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        }

        function OpenModalSession() {
            try {
                jQuery.noConflict();
                $("#myModalSession").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            catch (err) {
                alert(err + ' : OpenModalSession');
            }
        }

        function DatePitcker() {
            try {
                (function ($) {
                    $(".form_datetime").datetimepicker({
                        //format: "dd/mm/yyyy - hh:ii",
                        fontAwesome: 'font-awesome',
                        format: "dd/mm/yyyy",
                        autoclose: true,
                        todayBtn: true,
                        todayHighlight: true,
                        minView: 2
                    });
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : DatePitcker');
            }
        }

        function ShowLoading() {
            try {
                $('#loading').show();
            }
            catch (err) {
                alert(err + ' : ShowLoading');
            }
        }

        function CloseLoading() {
            $('#loading').fadeOut("fast");
        }
        
        function openInNewTab2(url) {
            var win = window.open(url, '_blank');
            win.focus();
        }

        function BtnReset_Click() {
            try {
                document.getElementById("DdlReqType").selectedIndex = 0;
                document.getElementById("DdlSMNStatus").selectedIndex = 0;
                document.getElementById("DdlFltrDate").selectedIndex = 0;
                document.getElementById("DdlFilterBy").selectedIndex = 0;
                document.getElementById("TxtFrom").value = "";
                document.getElementById("TxtTo").value = "";
                document.getElementById("txtFind").value = "";

                var length = $("#lcDatatables").val();
                if (length == "" || length == "0") {
                    length = "1";
                    $("#lcDatatables").val("1");
                }

                dataTable.clear().draw();
                dataTable.page.len(length).draw();
            } catch (e) {
                alert("BtnReset_Click : " + e)
            }
        }

        function Close_Click() {
            try {
                window.location = mainUrl + "/Home.aspx";
            } catch (e) {
                alert("Close_Click : " + e)
            }
        }

        function LogOut() {
            var url = mainUrl + "/EmetServices/LogInOrLogout.asmx/Logout";
            $.ajax({
                url: url,
                cache: false,
                type: "POST",
                dataType: 'json',
                async: false,
                complete: function () {

                },
                success: function (data) {
                    if (data.success == true) {
                        window.location = mainUrl + "/Login.aspx";
                    }
                },
                error: function (reponse) {
                }
            });
        }
    </script>

    <%--@*GenerateTbMainAllReq()*@--%>
    <script type="text/javascript">
        function GenerateTableReport() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();

                    dataTable = $("#TbMainAllReq").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "processing": "<span class='fa-stack fa-lg'>\n\
                            <i class='fa fa-spinner fa-spin fa-stack-2x fa-fw'></i>\n\
                       </span>&emsp;Processing ...",
                        "drawCallback": function () {
                            //$('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $('div.dataTables_filter input').prop('type', 'text');
                            $(".paginate_button").click(function () {
                                currentPage = dataTable.page.info().page;
                                ExpandOrColapseAllPageChange();
                            });
                        },
                        "deferRender": true,
                        "columns": [
                            {
                                "className": 'details-control',
                                "data": null,
                                "defaultContent": ''
                            },
                            {
                                "data": null, "sortable": true,
                                render: function (data, type, row, meta) {
                                    return meta.row +  1;
                                }
                            },
                        { "data": "Plant", "autoWidth": true },
                        { "data": "RequestNumber", "autoWidth": true },
                        { "data": "NoQuote", "autoWidth": true },
                        {
                            "data": "RequestDate",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "QuoteResponseDueDate",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        { "data": "Product", "autoWidth": true },
                        { "data": "Material", "autoWidth": true },
                        { "data": "MaterialDesc", "autoWidth": true },
                        { "data": "CreatedBy", "autoWidth": true },
                        { "data": "UseDep", "autoWidth": true },
                        { "data": "ReqType", "autoWidth": true }
                        ],
                        "ordering": true,
                        columnDefs: [{
                            orderable: false,
                            targets: "no-sort"
                        }],
                        "dom": "<'row'<'col-lg-12 col-sm-12 col-md-12 col-12'lB>>" +
                               "<'row'<'col-sm-12'tr>>" +
                               "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                        buttons: [
                            {
                                extend: "excel",
                                text: '<span class="glyphicon glyphicon-export"></span> Export [Main Data]',
                                className: "btn btn-sm btn-success",
                                name: "btnExcelExport",
                                action: function (e, dt, node, config) {
                                    var that = this;
                                    ShowLoading();
                                    if (typeof (Worker) !== "undefined") {
                                        setTimeout(function () {
                                            //var myWorker = new Worker('Scripts/Worker-helper.js');
                                            //myWorker.postMessage(newObj);
                                            //myWorker.onmessage = function (e) {
                                            //    console.log(e);
                                            //}
                                            ExportData();
                                            //$.fn.dataTable.ext.buttons.excelHtml5.action.call(that, e, dt, node, config)
                                            CloseLoading();
                                        }, 50);
                                    }
                                    else {
                                        alert("Browser Not Support");
                                    }
                                }
                            },
                            {
                                extend: "excel",
                                text: '<span class="glyphicon glyphicon-export"></span> Export [Main Data & Details]',
                                className: "btn btn-sm btn-success",
                                name: "btnExcelExportDetails",
                                action: function (e, dt, node, config) {
                                    var that = this;
                                    ShowLoading();
                                    if (typeof (Worker) !== "undefined") {
                                        setTimeout(function () {
                                            //var myWorker = new Worker('Scripts/Worker-helper.js');
                                            //myWorker.postMessage(newObj);
                                            //myWorker.onmessage = function (e) {
                                            //    console.log(e);
                                            //}
                                            ExportDataDetails();
                                            //$.fn.dataTable.ext.buttons.excelHtml5.action.call(that, e, dt, node, config)
                                            CloseLoading();
                                        }, 50);
                                    }
                                    else {
                                        alert("Browser Not Support");
                                    }
                                }
                            }
                        ],
                        language: {
                            'emptytable': 'No data found',
                            'search': '',
                            'searchPlaceholder': 'Filter All Columns',
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatables' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.RequestNumber;
                        }
                    });

                    $("#lcDatatables").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatables").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            var info = dataTable.page.info();
                            var recordsTotal = info.recordsTotal;

                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                            else if (res > recordsTotal) {
                                $(this).val(recordsTotal)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTable.page.len(1).draw();
                        } else {
                            dataTable.page.len(length).draw();
                        }
                    });

                    $("#lcDatatables").change(function (e) {
                        
                        var info = dataTable.page.info();
                        var recordsTotal = info.recordsTotal;

                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                        else if ($(this).val() > recordsTotal) {
                            $(this).val(recordsTotal);
                        }
                    });


                    $('#TbMainAllReq tbody').on('click', 'td.details-control', function () {
                        var tr = $(this).closest('tr');
                        var row = dataTable.row(tr);
                        var data = dataTable.row(tr).data();

                        if (row.child.isShown()) {
                            // This row is already open - close it
                            row.child.hide();
                            tr.removeClass('shown');
                        }
                        else {
                            // Open this row
                            var RequestNumber = data.RequestNumber;
                            row.child(LoadDataDetail(RequestNumber)).show();
                            tr.addClass('shown');
                        }

                        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                    });

                } catch (e) {
                    alert("GenerateTableReport() : " + e);
                }
            }
            else {
                alert('GenerateTableReport : Browser Not Support')
            }
        }
    </script>

    <%--@*LoadDataDetail*@--%>
    <script type="text/javascript">
        function LoadDataDetail(RequestNumber) {
            try {
                var data_filter = DataDet.filter(element => element.RequestNumber == RequestNumber)

                var htmltext = "";
                
                htmltext += '<table id="TB_' + RequestNumber + '" class="table table-responsive table-bordered nopadding" border="1" style="border-color:#DDDDDD; overflow-x:auto;"> ' +
                            '<thead>' +
                                '<tr style="background-color:#009DDD!important;">' +
                                    '<th rowspan="2">No</td>' +
                                    '<th colspan="2">Vendor</td>' +
                                    '<th rowspan="2">Quote No</td>' +
                                    '<th colspan="6">Total Cost</td>' +
                                    '<th rowspan="2">Vnd.Res. Status</td>' +
                                    '<th colspan="4">SMN DIR</td>' +
                                '</tr>' +
                                '<tr style="background-color:#009DDD!important;">' +
                                    '<th>ID</td>' +
                                    '<th>Name</td>' +

                                    '<th>Material</td>' +
                                    '<th>Process</td>' +
                                    '<th>SubMat</td>' +
                                    '<th>Other</td>' +
                                    '<th>Grand</td>' +
                                    '<th>Final</td>' +

                                    '<th>Decision</td>' +
                                    '<th>Date</td>' +
                                    '<th>Name</td>' +
                                    '<th>Comment</td>' +
                                '</tr>' +
                            '</thead>';
                        
                for (var r = 0; r < data_filter.length; r++) {
                    var VendorCode = data_filter[r].VendorCode === null ? '' : data_filter[r].VendorCode;
                    var VendorName = data_filter[r].VendorName === null ? '' : data_filter[r].VendorName;
                    var QuoteNo = data_filter[r].QuoteNo === null ? '' : data_filter[r].QuoteNo;
                    var TotalMaterialCost = data_filter[r].TotalMaterialCost === null ? '' : data_filter[r].TotalMaterialCost;
                    var TotalProcessCost = data_filter[r].TotalProcessCost === null ? '' : data_filter[r].TotalProcessCost;
                    var TotalSubMaterialCost = data_filter[r].TotalSubMaterialCost === null ? '' : data_filter[r].TotalSubMaterialCost;
                    var TotalOtheritemsCost = data_filter[r].TotalOtheritemsCost === null ? '' : data_filter[r].TotalOtheritemsCost;
                    var GrandTotalCost = data_filter[r].GrandTotalCost === null ? '' : data_filter[r].GrandTotalCost;
                    var FinalQuotePrice = data_filter[r].FinalQuotePrice === null ? '' : data_filter[r].FinalQuotePrice;
                    var ResponseStatus = data_filter[r].ResponseStatus === null ? '' : data_filter[r].ResponseStatus;
                    
                    var DDecision = data_filter[r].DDecision === null ? '' : data_filter[r].DDecision;
                    var DAprRejDt = data_filter[r].DAprRejDt === null ? '' : moment(data_filter[r].DAprRejDt).format('DD-MM-YYYY');
                    var Dname = data_filter[r].Dname === null ? '' : data_filter[r].Dname;
                    var DComment = data_filter[r].DComment === null ? '' : data_filter[r].DComment;
                    var url = "QQPReview.aspx?Number=" + QuoteNo;
                    htmltext += '<tr>' +
                                    '<td>'+ (r+1) +'</td>' +
                                    '<td>' + VendorCode + '</td>' +
                                    '<td>' + VendorName + '</td>' +
                                    '<td><a onclick="openInNewTab2(\'' + url + '\')">' + QuoteNo + '</a></td>' +
                                    '<td>' + TotalMaterialCost + '</td>' +
                                    '<td>' + TotalProcessCost + '</td>' +
                                    '<td>' + TotalSubMaterialCost + '</td>' +
                                    '<td>' + TotalOtheritemsCost + '</td>' +
                                    '<td>' + GrandTotalCost + '</td>' +
                                    '<td>' + FinalQuotePrice + '</td>' +
                                    '<td>' + ResponseStatus + '</td>' +
                                    
                                    '<td>' + DDecision + '</td>' +
                                    '<td>' + DAprRejDt + '</td>' +
                                    '<td>' + Dname + '</td>' +
                                    '<td>' + DComment + '</td>' +
                                '</tr>';
                }
                htmltext += '</table>';

                return htmltext;
            } catch (err) {
                alert("LoadDataDetail(): " + err);
                return "";
            }
        }

        function SetupTableDetail(RequestNumber) {
            try {
                var data_filter = DataDet.filter(element => element.RequestNumber == RequestNumber);
                var currentPageDet = 0;

                var dataTableDet = $("#TB_" + RequestNumber + "").DataTable({
                    "bDestroy": true,
                    "language": {
                        "emptytable": "No data found"
                    },
                    "processing": "<span class='fa-stack fa-lg'>\n\
                                <i class='fa fa-spinner fa-spin fa-stack-2x fa-fw'></i>\n\
                                </span>&emsp;Processing ...",
                    "drawCallback": function () {
                        //$('div.dataTables_filter input').addClass('form-control form-control-sm');
                        $('div.dataTables_filter input').prop('type', 'text');
                        $(".paginate_button").click(function () {
                            currentPageDet = dataTableDet.page.info().page;
                        });
                    },
                    "deferRender": true,
                    "columns": [
                    {
                        "data": null, "sortable": false,
                        render: function (data, type, row, meta) {
                            return meta.row + 1;
                        }
                    },
                    { "data": "VendorCode", "autoWidth": true },
                    { "data": "VendorName", "autoWidth": true },
                    { "data": "QuoteNo", "autoWidth": true },
                    { "data": "TotalMaterialCost", "autoWidth": true },
                    { "data": "TotalProcessCost", "autoWidth": true },
                    { "data": "TotalSubMaterialCost", "autoWidth": true },
                    { "data": "TotalOtheritemsCost", "autoWidth": true },
                    { "data": "GrandTotalCost", "autoWidth": true },
                    { "data": "FinalQuotePrice", "autoWidth": true },
                    { "data": "ResponseStatus", "autoWidth": true },
                    { "data": "MDecision", "autoWidth": true },
                    {
                        "data": "MAprRejDt",
                        "render": function (value) {
                            if (value === null) return "";
                            return moment(value).format('dd-mm-yyyy hh:mm:ss');
                        },
                        "autowidth": true
                    },
                    { "data": "Mname", "autoWidth": true },
                    { "data": "MComment", "autoWidth": true },
                    { "data": "DDecision", "autoWidth": true },
                    {
                        "data": "DAprRejDt",
                        "render": function (value) {
                            if (value === null) return "";
                            return moment(value).format('DD-MM-YYYY HH:mm:ss');
                        },
                        "autoWidth": true
                    },
                    { "data": "Dname", "autoWidth": true },
                    { "data": "DComment", "autoWidth": true }
                    ],
                    "ordering": false,
                    "dom": "<'row'<'col-lg-12 col-sm-12 col-md-12 col-12'lfB>>" +
                           "<'row'<'col-sm-12'tr>>" +
                           "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                    buttons: [
                        {
                            extend: "excel",
                            text: '<span class="glyphicon glyphicon-export"></span> Export to Excel',
                            className: "btn btn-sm btn-success",
                            name: "btnExcelExport",
                            action: function (e, dt, node, config) {
                                var that = this;
                                ShowLoading();
                                if (typeof (Worker) !== "undefined") {
                                    setTimeout(function () {
                                        //var myWorker = new Worker('Scripts/Worker-helper.js');
                                        //myWorker.postMessage(newObj);
                                        //myWorker.onmessage = function (e) {
                                        //    console.log(e);
                                        //}
                                        ExportData();
                                        //$.fn.dataTable.ext.buttons.excelHtml5.action.call(that, e, dt, node, config)
                                        CloseLoading();
                                    }, 50);
                                }
                                else {
                                    alert("Browser Not Support");
                                }
                            }
                        }
                    ],
                    language: {
                        'emptytable': 'No data found',
                        'search': '',
                        'searchPlaceholder': 'Filter All Columns',
                        "lengthMenu": "Show <input class='' type='text' id='lcDatatables" + RequestNumber + "' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                    },
                    "scrollX": true,
                    rowId: function (a) {
                        return "id_" + a.QuoteNo;
                    }
                });

                $("#lcDatatables" + RequestNumber + "").keydown(function (e) {
                    if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                        e.preventDefault();
                    }
                });

                $("#lcDatatables" + RequestNumber + "").on("input", function (e) {
                    var length = $(this).val();
                    var res = length.charAt(0);

                    if (length.length > 1) {
                        if (res == "0") {
                            length = length.substring(1);
                            $(this).val(length)
                        }
                    }

                    if ($(this).val() == "" || $(this).val() == "0") {
                        dataTableDet.page.len(1).draw();
                    } else {
                        dataTableDet.page.len(length).draw();
                    }
                });

                $("#lcDatatables" + RequestNumber + "").change(function (e) {
                    if ($(this).val() == "" || $(this).val() == "0") {
                        $(this).val("1");
                    }
                });

                var lenght = $("#lcDatatables" + RequestNumber + "").val();
                dataTableDet.clear().draw();
                dataTableDet.rows.add(data_filter).draw();
                dataTableDet.page.len(lenght).draw();
            } catch (e) {
                alert("SetupTableDetail : " +e);
            }
        }
    </script>

    <%--Load data--%>
    <script type="text/javascript">
        function GetReportData() {
            try {
                var MainData = [];
                DataDet = [];

                jQuery.noConflict();
                ShowLoading();
                if (typeof (Worker) !== "undefined") {
                    setTimeout(function () {
                        var url = mainUrl + "/EmetServices/CompledRequest/MyJson.asmx/LoadData";
                        var _Plant = document.getElementById('TxtPlant').value;
                        var _Status = "ALL";
                        var _SMNStatus = document.getElementById('DdlSMNStatus').value;
                        var _ReqType = document.getElementById('DdlReqType').value;
                        var _ReqStatus = "Closed";
                        var _FltrDate = document.getElementById('DdlFltrDate').value;
                        var _From = document.getElementById('TxtFrom').value;
                        var _To = document.getElementById('TxtTo').value;

                        var _FilterBy = document.getElementById('DdlFilterBy').value;
                        var _FilterValue = document.getElementById('txtFind').value;

                        $.ajax({
                            url: url,
                            cache: false,
                            type: "POST",
                            dataType: 'json',
                            timeout: 300000, // sets timeout to 5 minutes
                            //contentType: "application/json; charset=utf-8",
                            data: {
                                Plant: _Plant, Status: _Status, SMNStatus: _SMNStatus, ReqType: _ReqType, ReqStatus: _ReqStatus,
                                FltrDate: _FltrDate, From: _From, To: _To, FilterBy: _FilterBy, FilterValue: _FilterValue,VendorCode : ""
                            },
                            async: false,
                            beforeSend: function () {
                                ShowLoading();
                            },
                            complete: function () {
                                CloseLoading();
                                SetupAndLoadData(MainData, DataDet);
                            },
                            success: function (data) {
                                if (data.success == true) {
                                    var length = $("#lcDatatables").val();
                                    if (length == "" || length == "0") {
                                        length = "1";
                                        $("#lcDatatables").val("1");
                                    }
                                    MainData = data.MainData;
                                    DataDet = data.AllRequestData;
                                }
                                else {
                                    alert(data.message);
                                }
                            },
                            error: function (xhr, status, error) {
                                alert(error);
                            }
                        });
                    }, 0);
                }
                else {
                    alert('GetReportData : Browser Not Support');
                    CloseLoading();
                }
            } catch (e) {
                alert("GetReportData : " + e);
                CloseLoading();
            }
        }

        function SetupAndLoadData(MainData, DataDet) {
            try {
                jQuery.noConflict();
                var length = $("#lcDatatables").val();
                if (length == "" || length == "0") {
                    length = "1";
                    $("#lcDatatables").val("1");
                }

                SetupObjectToExportMain(MainData);
                SetupObjectToExportDet(DataDet);

                dataTable.clear().draw();
                dataTable.rows.add(MainData).draw();
                //length change input textbox
                dataTable.page.len(length).draw();
            } catch (e) {
                alert("SetupAndLoadData : " + e)
            }
        }

        function SetupObjectToExportMain(MainData) {
            try {
                NewObjToExport = [];
                for (var r = 0; r < MainData.length; r++) {
                    var SetUpObjToExportMain = [];
                    var SetUpObjToExportDet = [];
                    var Plant = MainData[r].Plant === null ? '' : MainData[r].Plant;
                    var RequestNumber = MainData[r].RequestNumber === null ? '' : MainData[r].RequestNumber;
                    var NoQuote = MainData[r].NoQuote === null ? '' : MainData[r].NoQuote;
                    var RequestDate = MainData[r].RequestDate === null ? '' : moment(MainData[r].RequestDate).format('DD-MM-YYYY');
                    var QuoteResponseDueDate = MainData[r].QuoteResponseDueDate === null ? '' : moment(MainData[r].QuoteResponseDueDate).format('DD-MM-YYYY');
                    var Product = MainData[r].Product === null ? '' : MainData[r].Product;
                    var Material = MainData[r].Material === null ? '' : MainData[r].Material;
                    var MaterialDesc = MainData[r].MaterialDesc === null ? '' : MainData[r].MaterialDesc;
                    var CreatedBy = MainData[r].CreatedBy === null ? '' : MainData[r].CreatedBy;
                    var UseDep = MainData[r].UseDep === null ? '' : MainData[r].UseDep;
                    var ReqType = MainData[r].ReqType === null ? '' : MainData[r].ReqType;

                    SetUpObjToExportMain.push(
                        Plant, RequestNumber, NoQuote, RequestDate, QuoteResponseDueDate, Product, Material, MaterialDesc, CreatedBy
                        , UseDep, ReqType
                        )

                    NewObjToExport.push(SetUpObjToExportMain);
                }
            } catch (e) {
                alert("SetupObjectToExportMain() :" + e)
            }
        }

        function SetupObjectToExportDet(DataDet) {
            try {
                NewObjToExportDetails = [];
                for (var r = 0; r < DataDet.length; r++) {
                    var SetUpObjToExportMain = [];
                    var SetUpObjToExportDet = [];
                    var Plant = DataDet[r].Plant === null ? '' : DataDet[r].Plant;
                    var RequestNumber = DataDet[r].RequestNumber === null ? '' : DataDet[r].RequestNumber;
                    var NoQuote = DataDet[r].NoQuote === null ? '' : DataDet[r].NoQuote;
                    var RequestDate = DataDet[r].RequestDate === null ? '' : moment(DataDet[r].RequestDate).format('DD-MM-YYYY');
                    var QuoteResponseDueDate = DataDet[r].QuoteResponseDueDate === null ? '' : moment(DataDet[r].QuoteResponseDueDate).format('DD-MM-YYYY');
                    var Product = DataDet[r].Product === null ? '' : DataDet[r].Product;
                    var Material = DataDet[r].Material === null ? '' : DataDet[r].Material;
                    var MaterialDesc = DataDet[r].MaterialDesc === null ? '' : DataDet[r].MaterialDesc;
                    var CreatedBy = DataDet[r].CreatedBy === null ? '' : DataDet[r].CreatedBy;
                    var UseDep = DataDet[r].UseDep === null ? '' : DataDet[r].UseDep;
                    var ReqType = DataDet[r].ReqType === null ? '' : DataDet[r].ReqType;

                    var VendorCode = DataDet[r].VendorCode === null ? '' : DataDet[r].VendorCode;
                    var VendorName = DataDet[r].VendorName === null ? '' : DataDet[r].VendorName;
                    var QuoteNo = DataDet[r].QuoteNo === null ? '' : DataDet[r].QuoteNo;
                    var TotalMaterialCost = DataDet[r].TotalMaterialCost === null ? '' : DataDet[r].TotalMaterialCost;
                    var TotalProcessCost = DataDet[r].TotalProcessCost === null ? '' : DataDet[r].TotalProcessCost;
                    var TotalSubMaterialCost = DataDet[r].TotalSubMaterialCost === null ? '' : DataDet[r].TotalSubMaterialCost;
                    var TotalOtheritemsCost = DataDet[r].TotalOtheritemsCost === null ? '' : DataDet[r].TotalOtheritemsCost;
                    var GrandTotalCost = DataDet[r].GrandTotalCost === null ? '' : DataDet[r].GrandTotalCost;
                    var FinalQuotePrice = DataDet[r].FinalQuotePrice === null ? '' : DataDet[r].FinalQuotePrice;
                    var ResponseStatus = DataDet[r].ResponseStatus === null ? '' : DataDet[r].ResponseStatus;
                    
                    var DDecision = DataDet[r].DDecision === null ? '' : DataDet[r].DDecision;
                    var DAprRejDt = DataDet[r].DAprRejDt === null ? '' : moment(DataDet[r].DAprRejDt).format('DD-MM-YYYY');
                    var Dname = DataDet[r].Dname === null ? '' : DataDet[r].Dname;
                    var DComment = DataDet[r].DComment === null ? '' : DataDet[r].DComment;

                    SetUpObjToExportDet.push(
                        Plant, RequestNumber, NoQuote, RequestDate, QuoteResponseDueDate, Product, Material, MaterialDesc, CreatedBy
                        , UseDep, ReqType
                        , VendorCode, VendorName, QuoteNo, TotalMaterialCost, TotalProcessCost, TotalSubMaterialCost
                        , TotalOtheritemsCost, GrandTotalCost, FinalQuotePrice, ResponseStatus, DDecision, DAprRejDt, Dname, DComment
                    )
                    NewObjToExportDetails.push(SetUpObjToExportDet);
                }
            } catch (e) {
                alert("SetupObjectToExportDet() :" + e)
            }
        }
    </script>

    <%--ExpandOrColapseAll--%>
    <script type="text/javascript">
        function ExpandOrColapseAll() {
            try {
                ShowLoading();
                setTimeout(function () {
                    if (document.getElementById("imgExOrCol").title == "Open") {

                        document.getElementById("imgExOrCol").src = mainUrl + "/Images/details_close.png";
                        document.getElementById("imgExOrCol").title = "Closed";

                        var table = document.getElementById("TbMainAllReq");
                        for (var i = 1; i < table.rows.length; i++) {
                            if (table.rows[i].cells[1] != null) {
                                var RowNo = table.rows[i].cells[1].innerText;
                                var row = dataTable.row(RowNo - 1);
                                var data = dataTable.row(RowNo - 1).data();

                                if (row.child.isShown()) {
                                    // This row is already open - close it
                                    //row.child.hide();
                                    //$('#id_' + RequestNumber + '').removeClass('shown');
                                }
                                else {
                                    // Open this row
                                    var RequestNumber = data.RequestNumber;
                                    if (RequestNumber == null) {
                                        break;
                                    }
                                    else {
                                        row.child(LoadDataDetail(RequestNumber)).show();
                                        $('#id_' + RequestNumber + '').addClass('shown');
                                    }

                                    //taking time to long if using datatable for detail
                                    //SetupTableDetail(RequestNumber);


                                }
                            }
                        }


                        //dataTable.rows().every(function (rowIdx, tableLoop, rowLoop) {
                        //    if (!this.child.isShown()) {
                        //        $('td:first-child', this.node()).trigger('click');
                        //    }
                        //});
                    }
                    else {
                        document.getElementById("imgExOrCol").src = mainUrl + "/Images/details_open.png";
                        document.getElementById("imgExOrCol").title = "Open";

                        dataTable.rows().every(function (rowIdx, tableLoop, rowLoop) {
                            if (this.child.isShown()) {
                                $('td:first-child', this.node()).trigger('click');
                            }
                        });
                    }
                    $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                }, 50);
            } catch (err) {
                alert("ExpandOrColapseAll(): " + err);
            }
            CloseLoading();
        }

        function ExpandOrColapseAllPageChange() {
            try {
                ShowLoading();
                setTimeout(function () {
                    if (document.getElementById("imgExOrCol").title == "Open") {
                        dataTable.rows().every(function (rowIdx, tableLoop, rowLoop) {
                            if (this.child.isShown()) {
                                $('td:first-child', this.node()).trigger('click');
                            }
                        });
                    }
                    else {
                        var table = document.getElementById("TbMainAllReq");
                        for (var i = 1; i < table.rows.length; i++) {
                            if (table.rows[i].cells[1] != null) {
                                var RowNo = table.rows[i].cells[1].innerText;
                                var row = dataTable.row(RowNo - 1);
                                var data = dataTable.row(RowNo - 1).data();

                                if (row.child.isShown()) {
                                    // This row is already open - close it
                                    //row.child.hide();
                                    //$('#id_' + RequestNumber + '').removeClass('shown');
                                }
                                else {
                                    // Open this row
                                    var RequestNumber = data.RequestNumber;
                                    if (RequestNumber == null) {
                                        break;
                                    }
                                    else {
                                        row.child(LoadDataDetail(RequestNumber)).show();
                                        $('#id_' + RequestNumber + '').addClass('shown');
                                    }

                                    //taking time to long if using datatable for detail
                                    //SetupTableDetail(RequestNumber);


                                }
                            }
                        }


                    }

                    $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                }, 50);
            } catch (err) {
                alert("ExpandOrColapseAll(): " + err);
            }
            CloseLoading();
        }
    </script>

    <%--Export--%>
    <script type="text/javascript">
        function ExportData() {
            try {
                if (NewObjToExport.length > 0) {
                    var today = new Date();
                    var dd = today.getDate();

                    var mm = today.getMonth() + 1;
                    var yyyy = today.getFullYear();
                    if (dd < 10) {
                        dd = '0' + dd;
                    }

                    if (mm < 10) {
                        mm = '0' + mm;
                    }
                    today = dd + '-' + mm + '-' + yyyy;

                    var MyHeader = ["Plant",
                                    "Req. No",
                                    "No. Que",
                                    "Req. Date",
                                    "Response Date",
                                    "Product",
                                    "Material",
                                    "Material Desc",
                                    "SMN PIC",
                                    "Dept",
                                    "Req Type"];

                    var config = {
                        filename: "eMET Completed Requests",
                        sheetName: "Sheet1",
                    };

                    var data = [
                        {
                            "styles": { title: 17, messageTop: 0, messageBottom: 0, header: 22, footer: 0 },
                            "title": ["Report Name: e-MET Completed Requests Main Data", "Report Generated: " + today + " "],
                            "messageTop": null, "messageBottom": null,
                            "header": MyHeader,
                            "body": NewObjToExport
                        }

                        //second table
                        //,
                        //{ "title": null, "messageTop": null, "messageBottom": null, "header": MyHeader, "footer": MyHeader, "body": [NewObjToExport] },
                    ];

                    generateXLSX(config, data);
                }
                else {
                    alert("No Available Data To Export")
                }
            } catch (e) {
                alert("ExportData : " + e)
            }
        }

        function ExportDataDetails() {
            try {
                if (NewObjToExportDetails.length > 0) {
                    var today = new Date();
                    var dd = today.getDate();

                    var mm = today.getMonth() + 1;
                    var yyyy = today.getFullYear();
                    if (dd < 10) {
                        dd = '0' + dd;
                    }

                    if (mm < 10) {
                        mm = '0' + mm;
                    }
                    today = dd + '-' + mm + '-' + yyyy;

                    var MyHeader = ["Plant",
                                    "Req. No",
                                    "No. Que",
                                    "Req. Date",
                                    "Response Date",
                                    "Product",
                                    "Material",
                                    "Material Desc",
                                    "SMN PIC",
                                    "Dept",
                                    "Req Type",
                                    "Vendor Code",
                                    "Vendor Name",
                                    "Quote No",
                                    "Material Cost",
                                    "Process Cost",
                                    "SubMat Cost",
                                    "Other Cost",
                                    "Grand Ttl Cost",
                                    "Final Cost",
                                    "Vnd.Res. Status",
                                    
                                    "DIR Decision",
                                    "DIR Date",
                                    "DIR Name",
                                    "DIR Comment"];

                    var config = {
                        filename: "eMET All Request",
                        sheetName: "Sheet1",
                    };

                    var data = [
                        {
                            "styles": { title: 17, messageTop: 0, messageBottom: 0, header: 22, footer: 0 },
                            "title": ["Report Name: e-MET Completed Requests [Main & Details] ", "Report Generated: " + today + " "],
                            "messageTop": null, "messageBottom": null,
                            "header": MyHeader,
                            "body": NewObjToExportDetails
                        }

                        //second table
                        //,
                        //{ "title": null, "messageTop": null, "messageBottom": null, "header": MyHeader, "footer": MyHeader, "body": [NewObjToExport] },
                    ];

                    generateXLSX(config, data);
                }
                else {
                    alert("No Available Data To Export")
                }
            } catch (e) {
                alert("ExportDataDetails : " + e)
            }
        }
    </script>

    <%--script alert and extend session--%>
    <script type="text/javascript">
        try {
            //jQuery.noConflict();
            //$(function () {
            //    jQuery.noConflict();
            //    var timeout = 570000;
            //    $(document).bind("idle.idleTimer", function () {
            //        // function you want to fire when the user goes idle
            //        var x = document.getElementById("loading");
            //        if (window.getComputedStyle(x).display === "none") {
            //            //console.log('hide');
            //        }
            //        else {

            //            OpenModalSession();
            //            $("#StartTimer").click();
            //        }
            //        //$.timeoutDialog({ timeout: 0.25, countdown: 15, logout_redirect_url: 'Login.aspx', restart_on_yes: true });
            //    });
            //    $(document).bind("active.idleTimer", function () {
            //        // function you want to fire when the user becomes active again
            //    });
            //    $.idleTimer(timeout);
            //});
        }
        catch (err) {
            alert(err + ' : alert and extend session');
        }
    </script>


</head>
<body id="page-top">
    <form id="form1" runat="server" autopostback="false">
        <asp:ScriptManager ID="scriptmanager1" runat="server" AsyncPostBackTimeout="36000"></asp:ScriptManager>
        <div class="col-md-12" id="DvMsgErr" runat="server" visible="false">
            <asp:Label runat="server" ID="LbMsgErr" Font-Bold="true" Visible="true"></asp:Label>
        </div>

        <div class="row">
            <div id="loading" class="col-sm-12" style="padding-top: 200px;">
                <img id="loading-image" src="images/loading.gif" alt="Loading..." />
                <div class="col-sm-12" style="text-align: center; opacity: 1;">
                    <asp:Label ID="lbLoading" runat="server" Text="please Wait..." Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Header -->
        <asp:UpdatePanel runat="server" ID="UpsidebarToggle" autopostback="false">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="col-lg-12" style="padding: 5px;">
                        <div class="row">
                            <div class="col-sm-10" style="padding-top: 5px;">
                                <a onclick="ShowLoading();" href="Home.aspx">
                                    <asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                                <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" ID="sidebarToggle"><i class="fas fa-bars"></i> </asp:LinkButton>
                                <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                                <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-sm-2 fa-pull-right" style="background-color: #E9ECEF;">
                                <asp:Label ID="lblUser" runat="server" Width="147px"></asp:Label><br />
                                <asp:Label ID="lblplant" runat="server" Text=""></asp:Label>
                                <asp:LinkButton runat="server" ID="BtnLogOut" autopostback="false" OnClientClick="LogOut()" Text="Logout"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="wrapper" class="bg-white">
            <!-- Sidebar -->
            <div id="SideBarMenu" style="width: 300px;" runat="server" class="SideBarMenu">
                <ul class="sidebar">
                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=1">
                            <i class="fas fa-fw fa-tachometer-alt"></i>
                            <span>Home</span>
                        </a>
                    </li>


                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=2">
                            <i class="fas fa-fw fa-newspaper"></i>
                            <span>Create Request</span></a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Revision.aspx">
                            <i class="fas fa-fw fa-table"></i>
                            <span>Revision of MET</span></a>

                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="MassRevision.aspx">
                            <i class="fas fa-fw fa-chart-area"></i>
                            <span>Mass Revision</span></a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=16">
                            <i class="fas fa-fw fa-table"></i>
                            <span>PIR Generation</span></a>

                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="PIRGenMassRev.aspx">
                            <i class="fas fa-fw fa-table"></i>
                            <span>PIR Generation Mass Revision</span></a>

                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=18">
                            <i class="fas fa-fw fa-table"></i>
                            <span style="">Log Vendor Password Changes</span></a>

                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="EMasterData.aspx">
                            <i class="fas fa-fw fa-table"></i>
                            <span>Master Data</span>
                        </a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="SmnReport.aspx">
                            <i class="fas fa-fw fa-book-open"></i>
                            <span>Report</span>
                        </a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="RealTimeVendInvSMN.aspx">
                            <i class="fas fa-fw fa-book-open"></i>
                            <span>Vendor Real Time Inventory</span>
                        </a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="aboutemet.aspx">
                            <i class="fas  fa-fw fa-info"></i>
                            <span>About</span></a>

                    </li>
                </ul>
            </div>

            <div id="content-wrapper">
                <div class="container-fluid">
                    <div class="card">
                        <div class="card-header">
                            <div class="card-header-content ">
                                <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION STATUS - <b>Completed Requests</b>
                            </div>
                        </div>
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server" autopostback="false">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-8" style="padding-bottom: 5px;">
                                            <asp:Label runat="server" ID="LbFilter" Text="Filter By :"></asp:Label>
                                        </div>
                                        <div class="col-sm-4 text-right" style="padding-bottom: 5px;">
                                            <asp:Button runat="server" ID="BtnReset" Text="Reset" CssClass="btn btn-sm btn-warning" OnClientClick="BtnReset_Click();return false;" autopostback="false"></asp:Button>
                                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="ShowLoading();Close_Click()" CssClass="btn btn-sm btn-danger" autopostback="false" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" autopostback="false">
                                <ContentTemplate>
                                    <asp:Panel runat="server" DefaultButton="btnSearch">
                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <asp:Label runat="server" ID="Label5" Text="Req. Type"></asp:Label>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:DropDownList runat="server" ID="DdlReqType" autopostback="false">
                                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                            <asp:ListItem Text="New" Value="WithSAPCode"></asp:ListItem>
                                                            <asp:ListItem Text="Revision" Value="WithSAPCodeRevision"></asp:ListItem>
                                                            <asp:ListItem Text="Mass Revision" Value="WithSAPCodeMassRevision"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFltrDate">
                                                    <asp:ListItem Text="Request Date" Value="RequestDate"></asp:ListItem>
                                                    <asp:ListItem Text="Quote Response Due Date" Value="QuoteResponseDueDate" ></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <div class="row">
                                                    <div class="col-lg-6" style="padding-bottom: 5px;">
                                                    <div class="group-main">
                                                        <div class="SearchBox-txt">
                                                            <asp:TextBox ID="TxtFrom" OnclientClick="return false;" runat="server" placeholder="Date From" autopostback="false"
                                                                ToolTip="Date From" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                            </asp:TextBox>
                                                        </div>
                                                        <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 2px 3px 1px 3px;">
                                                            <a class="fa fa-calendar" style="color: #005496; padding: 2px 3px 1px 3px;" onclick="javascript: $('#TxtFrom').focus();"></a>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6" style="padding-bottom: 5px;">
                                                    <div class="group-main">
                                                        <div class="SearchBox-txt">
                                                            <asp:TextBox ID="TxtTo" OnclientClick="return false;" runat="server" placeholder="Date To" autopostback="false"
                                                                ToolTip="Date To" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                            </asp:TextBox>
                                                        </div>
                                                        <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 2px 3px 1px 3px;">
                                                            <a class="fa fa-calendar" style="color: #005496; padding: 2px 3px 1px 3px;" onclick="javascript: $('#TxtTo').focus();"></a>
                                                        </span>
                                                    </div>
                                                </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <asp:Label runat="server" ID="Label4" Text="Dir. Status"></asp:Label>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:DropDownList runat="server" ID="DdlSMNStatus" autopostback="false">
                                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                            <asp:ListItem Text="Approved" Value="DApproved"></asp:ListItem>
                                                            <asp:ListItem Text="Rejected" Value="DRejected"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFilterBy" autopostback="false">
                                                    <asp:ListItem Text="Plant" Value="Plant"></asp:ListItem>
                                                    <asp:ListItem Text="Request Number" Value="RequestNumber"></asp:ListItem>
                                                    <asp:ListItem Text="Product" Value="Product"></asp:ListItem>
                                                    <asp:ListItem Text="Material" Value="Material"></asp:ListItem>
                                                    <asp:ListItem Text="Material Desc" Value="MaterialDesc"></asp:ListItem>
                                                    <asp:ListItem Text="Quote No" Value="QuoteNo"></asp:ListItem>
                                                    <asp:ListItem Text="Vendor Code" Value="VendorCode1"></asp:ListItem>
                                                    <asp:ListItem Text="Vendor Name" Value="VendorName"></asp:ListItem>
                                                    <asp:ListItem Text="Process Group ID" Value="ProcessGroup"></asp:ListItem>
                                                    <asp:ListItem Text="Process Group Desc" Value="ProcessGroupDesc"></asp:ListItem>
                                                    <asp:ListItem Text="SMN PIC" Value="CreatedBy"></asp:ListItem>
                                                    <asp:ListItem Text="Department" Value="UseDep"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:TextBox runat="server" ID="txtFind" Text=""></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row">
                                            
                                            
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-4"></div>
                                            <div class="col-lg-4"></div>
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:LinkButton ID="btnSearch" CssClass="btn btn-sm btn-primary btn-block my-btn-sm" runat="server"
                                                    autopostback="false" OnClientClick="ShowLoading();GetReportData();return false;"><i class="fa fa-search" aria-hidden="true" 
                                                            style="color:#F5F5F5;" ></i> Search </asp:LinkButton>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-md-12">
                                    <table id="TbMainAllReq" class="table table-responsive table-bordered nopadding" >
                                    <thead style="background-color:#006699!important;">
                                        <tr>
                                            <th style="vertical-align: middle; background-color:whitesmoke ;" onclick="ExpandOrColapseAll();">
                                                <img src="images/details_open.png" id="imgExOrCol"  title="Open" class="nopadding no-sort" /></th>
                                            <th>No.</th>
                                            <th>Plant</th>
                                            <th>Req. No</th>
                                            <th>No. Que</th>
                                            <th class="no-sort">Req. Date</th>
                                            <th class="no-sort">Response Date</th>
                                            <th>Product</th>
                                            <th>Material</th>
                                            <th>Material Desc</th>
                                            <th>SMN PIC</th>
                                            <th>Dept</th>
                                            <th>Req Type</th>
                                        </tr>
                                    </thead>
                                </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Footer -->
        <div class="container-fluid" style="background-color: #F5F5F5">
            <div class="row">
                <div class="col-lg-12" style="padding: 5px; align-content: center; text-align: center">
                    <span style="font: bold 13px calibri, calibri">Copyright © ShimanoDT 2018</span>
                </div>
            </div>
        </div>

        <a class="scroll-to-top rounded" href="#page-top"><i class="fas fa-angle-up"></i></a>

        <!-- Modal session expired -->
        <div class="modal fade" id="myModalSession" data-backdrop="static" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel20" runat="server" autopostback="false">
                <ContentTemplate>
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;">
                                <div class="col-lg-12 Padding-Nol" style="font: bold 22px calibri, calibri; text-align: center; align-content: center;">Your Session Is About To Expire !!  </div>
                                <h4></h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="col-xs-2" style="padding: 10px">
                                                <asp:Image ID="ImagWarning" runat="server" class="responsive" ImageUrl="~/js/jsextendsession/images/timeout-icon.png" />
                                            </div>
                                            <div class="col-xs-10" style="padding: 10px">
                                                <asp:Timer ID="TimerCntDown" runat="server" Interval="1000" Enabled="false"></asp:Timer>
                                                You will be logged out in :
                                                <asp:Label ID="countdown" runat="server" Font-Bold="true" ForeColor="Red" Text="30"></asp:Label>
                                                seconds<br />
                                                do u want to stay Sign In?
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-top-left-radius: 15px; border-top-right-radius: 15px;">
                                <asp:Button ID="BtnRefresh" runat="server" Text="Yes, Keep me Sign In" CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="18px" />
                                <asp:Button ID="CtnCloseMdl" runat="server" Text="No, Sign Me Out" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="18px" />
                                <div style="display: none;">
                                    <asp:Button ID="StartTimer" runat="server" Text="Start" CssClass="btn btn-sm btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="DvHdnField" runat="server" style="display: none">
            <asp:UpdatePanel runat="server" ID="UpdatePanel2" autopostback="false">
                <ContentTemplate>
                    <asp:TextBox runat="server" ID="TxtPlant" value=""></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>

</body>
</html>