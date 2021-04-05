<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MassRevReqWaitAll.aspx.cs" Inherits="Material_Evaluation.MassRevReqWaitAll" %>

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

        .InvalidCheckBox {
            outline-color: red;
            outline-style: solid;
            outline-width: 1px;
            padding: 0px;
        }

        .btn[disabled] {
            background-color: #808080;
        }

        .btn[disabled]:hover{
            background-color: #808080!important;
        }

    </style>

    <%--Data table style--%>
    <style>
        td.details-control {
            background: url('../../Images/details_open.png') no-repeat center center;
            cursor: pointer;
        }

        .details-control {
            padding: 0px 3px 0px 3px !important;
            width: 1% !important;
        }

        tr.shown td.details-control {
            background: url('../../Images/details_close.png') no-repeat center center;
        }

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

        .dt-button {
            padding: 0px 15px 0px 15px;
            float: right !important;
        }

        .dataTables_filter input {
            width: 300px !important;
        }

        table.table thead tr th {
            color: white;
            font-weight: lighter;
        }

        .table-bordered {
            border: 0 !important;
            border-collapse: collapse !important;
        }

        #TbData_wrapper {
            overflow-x: hidden;
        }

        #TbData_filter {
            float: left !important;
        }

        #TbData {
            width: 100% !important;
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
        var Popup, dataTableTbData;
        var currentPageTbData = 0;
        var mainUrl = "";
        var DataDet = [];
        var NewObjToExport = [];
        var NewObjToExportDetails = [];

        $(document).ready(function () {
            mainUrl = window.location.href.replace("MassRevReqWaitAll.aspx", "");
            SetUpSideBar();
            DatePitcker();
            DatePitcker2();
            GenerateTbData();
        });

        function preventInput(evnt) {
            if (evnt.which != 9) evnt.preventDefault();
        }

        $(window).load(function () {
            $('#loading').fadeOut("fast");
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

        function DatePitcker() {
            try {
                (function ($) {
                    $(".form_datetime").datetimepicker({
                        //format: "dd/mm/yyyy - hh:ii",
                        //startDate: new Date(),
                        fontAwesome: 'font-awesome',
                        format: "dd-mm-yyyy",
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
        function DatePitcker2() {
            try {
                (function ($) {
                    $(".form_datetime2").datetimepicker({
                        //format: "dd/mm/yyyy - hh:ii",
                        startDate: new Date(),
                        fontAwesome: 'font-awesome',
                        format: "dd-mm-yyyy",
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

        function DatePitckerAppr(id) {
            try {
                jQuery.noConflict();
                (function ($) {
                    $('#TxtNewResDueDate_' + id).datetimepicker({
                        fontAwesome: 'font-awesome',
                        format: "dd-mm-yyyy",
                        autoclose: true,
                        todayBtn: true,
                        todayHighlight: true,
                        startDate: new Date(),
                        minView: 2
                    });
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : DatePitcker(id)');
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
                document.getElementById("FlUpload").value = "";

                document.getElementById("ChcMatCost").checked = false;
                document.getElementById("ChcProcCost").checked = false;
                document.getElementById("ChcSubMat").checked = false;
                document.getElementById("ChcOthMat").checked = false;
                
                document.getElementById("TxtValidDate").value = "";
                document.getElementById("TxtDuenextRev").value = "";
                document.getElementById("TxtResDueDate").value = "";
                document.getElementById("DdlReason").selectedIndex = 0;
                document.getElementById("txtRem").value = "";
                document.getElementById("DvRemark").style.display = "none";
                
                dataTableBasicData.clear().draw();
                dataTableDuplicateWithExpiredReq.clear().draw();
                dataTableTbQuoteRefListInvalid.clear().draw();
                dataTableTbValidData.clear().draw();
                dataTableTbInValidData.clear().draw();
                dataTableTbValidDataReqTemp.clear().draw();
                dataTableTbInValidDataReqTemp.clear().draw();
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

    <%--open And Close Modal--%>
    <script type="text/javascript">
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

        function openModal(ReqNo,DueDate) {
            try {
                jQuery.noConflict();
                document.getElementById("TxtModalReqNo").value = ReqNo;
                document.getElementById("TxtModalDueDate").value = DueDate;
                $('#myModal').modal('show');
            }
            catch (err) {
                alert(err + ' : OpenModalSession');
            }
        }
        function closeModal() {
            try {
                jQuery.noConflict();
                $('#myModal').modal('hide');
            }
            catch (err) {
                alert(err + ' : OpenModalSession');
            }
        }
    </script>

    <%--GenerateTbData--%>
    <script type="text/javascript">
        function GenerateTbData() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableTbData = $("#TbData").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').prop('type', 'text');
                            $(".paginate_button").click(function () {
                                currentPageTbData = dataTableTbData.page.info().page;
                                ExpandOrColapseAllPageChange();
                            });
                        },
                        "deferRender": false,
                        "columns": [
                            {
                                "className": 'details-control',
                                "data": null,
                                "defaultContent": ''
                            },
                            {
                                "data": null, "sortable": true,
                                render: function (data, type, row, meta) {
                                    return meta.row + 1;
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
                            {
                                "data": null,
                                "render": function (data, type, row, meta) {
                                    if (type === 'display') {
                                        var DueDate = "";
                                        if (row.QuoteResponseDueDate != null) {
                                            DueDate = moment(row.QuoteResponseDueDate).format('DD-MM-YYYY');
                                        }

                                        var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
                                        var CurDuedate = new Date(DueDate.toString().replace(/\-/g, '.').replace(pattern, '$3-$2-$1'));
                                        var CurrDate = new Date();
                                        var Isdisbaled = "";
                                        if (CurrDate > CurDuedate) {
                                            Isdisbaled="disabled"
                                        }
                                        return '<button id="BtnUpdateDate_' + meta.row + '" class="btn btn-sm btn-primary btn-block" onclick="openModal(\'' + row.RequestNumber + '\',\'' + DueDate + '\');" ' + Isdisbaled + '>Update Date</button>' +
                                            '<button id="BtnRej_' + meta.row + '" class="btn btn-sm btn-danger btn-block" onclick="RejectQuotation(\'' + row.RequestNumber + '\');" ' + Isdisbaled + '>Reject</button>';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            }
                        ],
                        "ordering": true,
                        "paging": true,
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
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatablesTbData' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.RequestNumber;
                        }
                    });

                    $("#lcDatatablesTbData").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesTbData").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            var info = dataTableTbData.page.info();
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
                            dataTableTbData.page.len(1).draw();
                        } else {
                            dataTableTbData.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesTbData").change(function (e) {

                        var info = dataTableDuplicateWithExpiredReq.page.info();
                        var recordsTotal = info.recordsTotal;

                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                        else if ($(this).val() > recordsTotal) {
                            $(this).val(recordsTotal);
                        }
                    });

                    $('#TbData tbody').on('click', 'td.details-control', function () {
                        var tr = $(this).closest('tr');
                        var row = dataTableTbData.row(tr);
                        var data = dataTableTbData.row(tr).data();

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
                    alert("GenerateTbData() : " + e);
                }
            }
            else {
                alert('GenerateTbData : Browser Not Support')
            }
        }

        function LoadDataDetail(RequestNumber) {
            try {
                var data_filter = DataDet.filter(element => element.RequestNumber == RequestNumber)

                var htmltext = "";

                htmltext += '<table id="TB_' + RequestNumber + '" class="table table-responsive table-bordered nopadding" border="1" style="border-color:#DDDDDD; overflow-x:auto;"> ' +
                            '<thead>' +
                                '<tr style="background-color:#009DDD!important;">' +
                                    '<th>No</td>' +
                                    '<th>Quote No</td>' +
                                    '<th>Vendor Code</td>' +
                                    '<th>Vendor Name</td>' +
                                '</tr>' +
                            '</thead>';

                for (var r = 0; r < data_filter.length; r++) {
                    var VendorCode = data_filter[r].VendorCode === null ? '' : data_filter[r].VendorCode;
                    var VendorName = data_filter[r].VendorName === null ? '' : data_filter[r].VendorName;
                    var QuoteNo = data_filter[r].QuoteNo === null ? '' : data_filter[r].QuoteNo;
                    var url = mainUrl + "QQPReview.aspx?Number=" + QuoteNo;
                    htmltext += '<tr>' +
                                    '<td>' + (r + 1) + '</td>' +
                                    '<td><a onclick="openInNewTab2(\'' + url + '\')">' + QuoteNo + '</a></td>' +
                                    '<td>' + VendorCode + '</td>' +
                                    '<td>' + VendorName + '</td>' +
                                '</tr>';
                }
                htmltext += '</table>';

                return htmltext;
            } catch (err) {
                alert("LoadDataDetail(): " + err);
                return "";
            }
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
                                    "Dept"];

                    var config = {
                        filename: "eMET [Mass Revision All]",
                        sheetName: "Sheet1",
                    };

                    var data = [
                        {
                            "styles": { title: 17, messageTop: 0, messageBottom: 0, header: 22, footer: 0 },
                            "title": ["Report Name: e-MET [Mass Revision All] Main Data", "Report Generated: " + today + " "],
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
                                    "Vendor Code",
                                    "Vendor Name",
                                    "Quote No"];

                    var config = {
                        filename: "eMET [Mass Revision All] Pending Detail",
                        sheetName: "Sheet1",
                    };

                    var data = [
                        {
                            "styles": { title: 17, messageTop: 0, messageBottom: 0, header: 22, footer: 0 },
                            "title": ["Report Name: e-MET  [Mass Revision All]  [Main & Details] ", "Report Generated: " + today + " "],
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

    <%--ExpandOrColapseAllPageChange--%>
    <script type="text/javascript">
        function ExpandOrColapseAllPageChange() {
            try {
                ShowLoading();
                setTimeout(function () {
                    if (document.getElementById("imgExOrCol").title == "Open") {
                        dataTableTbData.rows().every(function (rowIdx, tableLoop, rowLoop) {
                            if (this.child.isShown()) {
                                $('td:first-child', this.node()).trigger('click');
                            }
                        });
                    }
                    else {
                        var table = document.getElementById("TbData");
                        for (var i = 1; i < table.rows.length; i++) {
                            if (table.rows[i].cells[1] != null) {
                                var RowNo = table.rows[i].cells[1].innerText;
                                var row = dataTableTbData.row(RowNo - 1);
                                var data = dataTableTbData.row(RowNo - 1).data();

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

    <%--ExpandOrColapseAll()--%>
    <script type="text/javascript">
        function ExpandOrColapseAll() {
            try {
                ShowLoading();
                setTimeout(function () {
                    if (document.getElementById("imgExOrCol").title == "Open") {

                        document.getElementById("imgExOrCol").src = mainUrl + "/Images/details_close.png";
                        document.getElementById("imgExOrCol").title = "Closed";

                        var table = document.getElementById("TbData");
                        for (var i = 1; i < table.rows.length; i++) {
                            if (table.rows[i].cells[1] != null) {
                                var RowNo = table.rows[i].cells[1].innerText;
                                var row = dataTableTbData.row(RowNo - 1);
                                var data = dataTableTbData.row(RowNo - 1).data();

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

                        dataTableTbData.rows().every(function (rowIdx, tableLoop, rowLoop) {
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
    </script>

    <%--GetData()--%>
    <script type="text/javascript">
        function GetData() {
            try {
                var MainData = [];
                DataDet = [];

                jQuery.noConflict();
                ShowLoading();
                if (typeof (Worker) !== "undefined") {
                    setTimeout(function () {
                        var url = mainUrl + "/EmetServices/MassRevisionReqWait/MyJSONMassRevisionALLReqWait.asmx/LoadData";
                        var _Plant = document.getElementById('TxtPlant').value;
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
                                Plant: _Plant, FltrDate: _FltrDate, From: _From, To: _To, FilterBy: _FilterBy, FilterValue: _FilterValue
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
                                    DataDet = data.DataDetail;
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
                var length = $("#lcDatatablesTbData").val();
                if (length == "" || length == "0") {
                    length = "1";
                    $("#lcDatatablesTbData").val("1");
                }

                SetupObjectToExportMain(MainData);
                SetupObjectToExportDet(DataDet);

                dataTableTbData.clear().draw();
                dataTableTbData.rows.add(MainData).draw();
                //length change input textbox
                dataTableTbData.page.len(length).draw();
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

                    SetUpObjToExportMain.push(
                        Plant, RequestNumber, NoQuote, RequestDate, QuoteResponseDueDate, Product, Material, MaterialDesc, CreatedBy
                        , UseDep
                        )

                    NewObjToExport.push(SetUpObjToExportMain);
                }
            } catch (e) {
                alert("SetupObjectToExportMain() :" + e)
            }
        }

        function SetupObjectToExportDet(DataDet) {
            debugger;
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

                    var VendorCode = DataDet[r].VendorCode === null ? '' : DataDet[r].VendorCode;
                    var VendorName = DataDet[r].VendorName === null ? '' : DataDet[r].VendorName;
                    var QuoteNo = DataDet[r].QuoteNo === null ? '' : DataDet[r].QuoteNo;

                    SetUpObjToExportDet.push(
                        Plant, RequestNumber, NoQuote, RequestDate, QuoteResponseDueDate, Product, Material, MaterialDesc, CreatedBy
                        , UseDep
                        , VendorCode, VendorName, QuoteNo
                    )
                    NewObjToExportDetails.push(SetUpObjToExportDet);
                }
            } catch (e) {
                alert("SetupObjectToExportDet() :" + e)
            }
        }
    </script>

    <%--RejectQuote--%>
    <script type="text/javascript">
        function RejectQuotation(RequestNo) {
            try {
                jQuery.noConflict();
                ShowLoading();
                if (typeof (Worker) !== "undefined") {
                    setTimeout(function () {
                        var url = mainUrl + "/EmetServices/MassRevisionReqWait/MyJSONMassRevisionALLReqWait.asmx/RejectQuote";
                        var UseId = document.getElementById('TxtuseID').value;
                        
                        $.ajax({
                            url: url,
                            cache: false,
                            type: "POST",
                            dataType: 'json',
                            timeout: 300000, // sets timeout to 5 minutes
                            //contentType: "application/json; charset=utf-8",
                            data: {
                                RequestNo: RequestNo, UseId: UseId
                            },
                            async: false,
                            beforeSend: function () {
                                ShowLoading();
                            },
                            complete: function () {
                                CloseLoading();
                                GetData();
                            },
                            success: function (data) {
                                if (data.success == true) {
                                    alert(data.message);
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
                    alert('RejectQuotation : Browser Not Support');
                    CloseLoading();
                }
            } catch (e) {
                alert("RejectQuotation : " + e);
                CloseLoading();
            }
        }
    </script>

    <%--UpdatedateQuotation--%>
    <script type="text/javascript">
        function UpdatedateQuotation() {
            try {
                jQuery.noConflict();
                ShowLoading();
                if (typeof (Worker) !== "undefined") {
                    setTimeout(function () {
                        var url = mainUrl + "/EmetServices/MassRevisionReqWait/MyJSONMassRevisionALLReqWait.asmx/UpdatedateQuotation";
                        var UseId = document.getElementById('TxtuseID').value;
                        var RequestNo = document.getElementById('TxtModalReqNo').value;
                        var NewDueDate = document.getElementById('TxtModalDueDate').value;
                        $.ajax({
                            url: url,
                            cache: false,
                            type: "POST",
                            dataType: 'json',
                            timeout: 300000, // sets timeout to 5 minutes
                            //contentType: "application/json; charset=utf-8",
                            data: {
                                RequestNo: RequestNo, NewDueDate: NewDueDate, UseId: UseId
                            },
                            async: false,
                            beforeSend: function () {
                                ShowLoading();
                            },
                            complete: function () {
                                CloseLoading();
                                GetData();
                                closeModal();
                            },
                            success: function (data) {
                                if (data.success == true) {
                                    alert(data.message);
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
                    alert('RejectQuotation : Browser Not Support');
                    CloseLoading();
                }
            } catch (e) {
                alert("RejectQuotation : " + e);
                CloseLoading();
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
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
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
        <asp:UpdatePanel runat="server" ID="UpsidebarToggle">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="col-lg-12" style="padding: 5px;">
                        <div class="row">
                            <div class="col-sm-10" style="padding-top: 5px;">
                                <a onclick="ShowLoading();" href="Home.aspx">
                                    <asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                                <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" ID="sidebarToggle" ><i class="fas fa-bars"></i> </asp:LinkButton>
                                <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                                <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-sm-2 fa-pull-right" style="background-color: #E9ECEF;">
                                <asp:Label ID="lblUser" runat="server" Width="147px"></asp:Label><br />
                                <asp:Label ID="lblplant" runat="server" Text=""></asp:Label>
                                <asp:LinkButton runat="server" ID="BtnLogOut" OnClientClick="LogOut()" Text="Logout"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="wrapper">
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

            <!-- Content -->
            <div id="content-wrapper">
                <div class="container-fluid">
                    <!-- Breadcrumbs-->
                    <%--<ol class="breadcrumb">
                        <li class="breadcrumb-item">
                            <a href="#">First Article Item</a>
                        </li>
                    </ol>--%>
                    <!-- Area Chart Example-->
                    <div class="card">
                        <div class="card-header">
                            <div class="card-header-content ">
                                <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION STATUS - <b>Quote Request With SAP Code Mass Revision All</b>
                            </div>
                        </div>
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-8" style="padding-bottom: 5px;">
                                            <asp:Label runat="server" ID="LbFilter" Text="Filter By :"></asp:Label>
                                        </div>
                                        <div class="col-sm-4 text-right" style="padding-bottom: 5px;">
                                            <asp:Button runat="server" ID="BtnReset" Text="Reset" CssClass="btn btn-sm btn-warning" autopostback="false"></asp:Button>
                                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger" autopostback="false" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:Panel runat="server" DefaultButton="btnSearch">
                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFltrDate">
                                                    <asp:ListItem Text="Request Date" Value="RequestDate"></asp:ListItem>
                                                    <asp:ListItem Text="Quote Response Due Date" Value="QuoteResponseDueDate"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-4">
                                                <div class="col-lg-6 nopadding">
                                                    <div class="group-main" style="padding: 0px 2px 5px 2px;">
                                                        <div class="SearchBox-txt">
                                                            <asp:TextBox ID="TxtFrom" OnclientClick="return false;" runat="server" placeholder="Date From"
                                                                ToolTip="Date From" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                            </asp:TextBox>
                                                        </div>
                                                        <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 3px 1px 1px;">
                                                            <a class="fa fa-calendar" style="color: #005496; padding: 1px 1px 1px 1px;" onclick="javascript: $('#TxtFrom').focus();"></a>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 nopadding">
                                                    <div class="group-main" style="padding: 0px 2px 5px 2px;">
                                                        <div class="SearchBox-txt">
                                                            <asp:TextBox ID="TxtTo" OnclientClick="return false;" runat="server" placeholder="Date To"
                                                                ToolTip="Date To" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                            </asp:TextBox>
                                                        </div>
                                                        <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 3px 1px 1px;">
                                                            <a class="fa fa-calendar" style="color: #005496; padding: 1px 1px 1px 1px;" onclick="javascript: $('#TxtTo').focus();"></a>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFilterBy" onchange="javascript:document.getElementById('txtFind').value = ''">
                                                    <asp:ListItem Text="Plant" Value="Plant"></asp:ListItem>
                                                    <asp:ListItem Text="Request Number" Value="RequestNumber"></asp:ListItem>
                                                    <asp:ListItem Text="Product" Value="Product"></asp:ListItem>
                                                    <asp:ListItem Text="Material" Value="Material"></asp:ListItem>
                                                    <asp:ListItem Text="Material Desc" Value="MaterialDesc"></asp:ListItem>
                                                    <asp:ListItem Text="Quote No" Value="QuoteNo"></asp:ListItem>
                                                    <asp:ListItem Text="Process Group ID" Value="ProcessGroup"></asp:ListItem>
                                                    <asp:ListItem Text="Process Group Desc" Value="ProcessGroupDesc"></asp:ListItem>
                                                    <asp:ListItem Text="SMN PIC" Value="CreatedBy"></asp:ListItem>
                                                    <asp:ListItem Text="Department" Value="UseDep"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:TextBox runat="server" ID="txtFind" Text=""></asp:TextBox>
                                            </div>

                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:LinkButton ID="btnSearch" CssClass="btn btn-sm btn-primary btn-sm btn-block my-btn-sm" runat="server"
                                                    autopostback="false" OnClientClick="ShowLoading();GetData();return false;"><i class="fa fa-search" aria-hidden="true" 
                                                            style="color:#F5F5F5;" ></i> Search </asp:LinkButton>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-md-12">
                                    <table id="TbData" class="table table-responsive table-striped table-bordered nopadding" style="width: 100%">
                                        <thead style="background-color: #006699!important">
                                            <tr>
                                                <th class="no-sort" style="vertical-align: middle; background-color:whitesmoke ;" onclick="ExpandOrColapseAll();">
                                                    <img src="images/details_open.png" id="imgExOrCol"  title="Open" class="nopadding no-sort" /></th>
                                                <th>No</th>
                                                <th>Plant</th>
                                                <th>Req No</th>
                                                <th>No. Que</th>
                                                <th class="no-sort">Req Date</th>
                                                <th class="no-sort">Response Date</th>
                                                <th>Product</th>
                                                <th>Material</th>
                                                <th>Material Desc</th>
                                                <th>SMN PIC</th>
                                                <th>Dept</th>
                                                <th class="no-sort">Q.Res.Due Dt</th>
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

        <!-- Scroll to Top Button-->
        <a class="scroll-to-top rounded" href="#page-top"><i class="fas fa-angle-up"></i>
        </a>

        <%--modal add/edit data--%>
        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Bootstrap Modal add data user Dialog -->
                <div class="modal fade" id="myModal" data-backdrop="static" tabindex="-1" role="dialog"
                    aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-lg">
                        <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="modal-content" style="background: linear-gradient(90deg, #F7F7F7, #ffffff, #F7F7F7); border-radius: 15px;">
                                    <div class="modal-header">
                                        <div class="row">
                                            <div class="col-sm-12 text-uppercase text-center" style="text-shadow: 1px 2px 1px white;">
                                                <asp:Label ID="LbModalHeader" runat="server" Text="Quote Respon Due Date"
                                                    ForeColor="#004080" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-body" style="background-color: white; padding-bottom: 0px;">
                                        <div class="row" style="padding-bottom: 0px;">
                                            <div class="col-sm-12" style="padding-bottom: 0px;">

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label11" runat="server" Text="Request Number"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtModalReqNo" runat="server" Enabled="false"></asp:TextBox>
                                                        <asp:TextBox ID="TxtMaterial" runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="lblUserName" runat="server" Text="Due Date"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <div class="group-main">
                                                            <div class="SearchBox-txt">
                                                                <asp:TextBox ID="TxtModalDueDate" OnclientClick="return false;" runat="server" CssClass="form_datetime2"
                                                                     AutoPostBack="false" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black">
                                                                </asp:TextBox>
                                                            </div>
                                                            <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 3px 1px 3px;">
                                                                <a class="fa fa-calendar" style="color: #005496; padding: 1px 3px 1px 3px;" onclick="javascript: $('#TxtModalDueDate').focus();"></a>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-12" style="padding-bottom: 0px;">
                                                        <div class="table table-responsive" style="padding-bottom: 0px;">
                                                            <asp:GridView ID="grdvendor" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                CssClass="table table-sm table-bordered table-nowrap">
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="VendorCode1" HeaderText="Vendor ID" />
                                                                    <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                                                                    <asp:BoundField DataField="QuoteNo" HeaderText="Quote No" />
                                                                </Columns>
                                                                <EditRowStyle BackColor="#999999" />
                                                                <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                                                <HeaderStyle BackColor="#4D94FF" ForeColor="White" />
                                                                <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#1a2e4c" />
                                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:UpdatePanel ID="UpInvalidRequest" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row" style="padding-bottom: 10px" runat="server" id="DvInvalidRequest" visible="false">
                                                            <div class="col-md-12">
                                                                <div class="col-md-12" style="background: #fa0606">
                                                                    <asp:Label ID="Label18" runat="server" Text="Faill to update Due Date , New Request has been created for below vendor"
                                                                        Visible="true" ForeColor="White" Font-Bold="true"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="table table-responsive table-sm">
                                                                    <asp:GridView ID="GvInvalidRequest" runat="server" AutoGenerateColumns="False"
                                                                        AllowPaging="false" PageSize="10"
                                                                        CssClass="table-sm table-bordered table-nowrap WrapCnt" Font-Bold="False" Width="100%">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="RequestNumber" HeaderText="Req. No" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="QuoteResponseDueDate" HeaderText="Res Due Date" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="QuoteNo" HeaderText="QuoteNo" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="Material" HeaderText="Material" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                        </Columns>
                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                        <PagerSettings PageButtonCount="10" />
                                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" BorderColor="White" />
                                                                        <RowStyle ForeColor="#000066" />
                                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer" style="background-color: #F5F5F5; border-bottom-right-radius: 15px; border-bottom-left-radius: 15px; border-top: 0px;">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class="col-sm-6" style="padding-bottom: 8px;">
                                                        <asp:Button ID="btnSubmit" CssClass="btn btn-sm btn-primary" OnClientClick="ShowLoading();UpdatedateQuotation();"  Width="100%"
                                                            Font-Size="14px" runat="server" Text="Save" />
                                                    </div>
                                                    <div class="col-sm-6 " style="padding-bottom: 8px;">
                                                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-default" Text="Close" Width="100%" autopostback="false"
                                                            Font-Size="14px" data-dismiss="modal" aria-hidden="true" OnClientClick="closeModal();return false" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Modal session expired -->
        <div class="modal fade" id="myModalSession" data-backdrop="static" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
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
                                    <asp:Button ID="StartTimer" runat="server" Text="Start"  CssClass="btn btn-sm btn-primary" />
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
                    <asp:TextBox runat="server" ID="TxtuseID" value=""></asp:TextBox>
                    <asp:TextBox runat="server" ID="TxtuserDept" value=""></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>

</body>
</html>
