<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealTimeVendInvSMN.aspx.cs" Inherits="Material_Evaluation.RealTimeVendInvSMN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />

    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet" />

    <link href="Styles/NewStyle/NewStyle.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link rel="stylesheet" href="Scripts/jquery-ui-1.12.1/jquery-ui.css" />
    <link rel="stylesheet" href="js/jsextendsession/css/timeout-dialog.css" />
    <link href="js/BootstrapDatePcr/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="Scripts/datatables/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="Scripts/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <style type="text/css">
        .SideBarMenu {
            width: 300px;
        }

        .lblpadding30 {
            padding-right:30px;
        }

        .linkLastWeek:hover {
            text-decoration: underline;
        }

        .dataTables_wrapper {
            overflow-x: unset !important;
        }


        #TbMainData td {
            text-align: center;
        }

        .WrapCnt td, th {
            white-space: nowrap !important;
            /*word-wrap: break-word;*/
            font-size: 14px !important;
        }

        .WrapCnt a {
            padding: 0px;
        }

        table.table thead tr th {
            color: white;
        }

        #TbData_wrapper {
            overflow-x: hidden;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button {
            background: linear-gradient(to bottom, #fff 0%, #dcdc 100%) !important;
        }

            .dataTables_wrapper .dataTables_paginate .paginate_button:not(.disabled):hover {
                background: #006699 !important;
                color: white !important;
            }

            .dataTables_wrapper .dataTables_paginate .paginate_button.current {
                background: #006699 !important;
                color: white !important;
            }
    </style>



    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Scripts/moment.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/jquery.dataTables.min.js"></script>

    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="Scripts/datatables/jszip.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/buttons.html5.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/buttons.flash.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/buttons.print.min.js"></script>

    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/locales/bootstrap-datetimepicker.fr.js"></script>


    <script type="text/javascript">
        var dataTable, dataTableMainData, currentPage, currentPageMainData, res;
        var selectedData = [];
        //$.noConflict();

        $(document).ready(function () {
            DatePitcker();
            GenerateTbData();
            GenerateNewMainTable();
        });

        function disabledbtnShowdetail() {
            $("#btnShowDetail").prop("disabled", true);
        }

        function enabledbtnShowdetail() {
            $("#btnShowDetail").prop("disabled", false);
        } 

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

        $(window).load(function () {
            $('#loading').fadeOut("fast");

            if ($('#IsFirstLoad').val("1")) {
                $('#BtnCekLatestNested').click();
            }
        });

        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "images/minus1.png");

            ////var ImageID = $(this).attr('ID');
            ////var ArrImageID = ImageID.split('_');
            ////$('#GridView1_BtnNstIdPls_' + ArrImageID[2]).click();
        });

        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "images/plus1.png");
            $(this).closest("tr").next().remove();

            //var ImageID = $(this).attr('ID');
            //var ArrImageID = ImageID.split('_');
            //$('#GridView1_BtnNstIdMin_' + ArrImageID[2]).click();
        });
    </script>

    <%--script open modal--%>
    <script type="text/javascript">
        //Prevent input date
        function preventInput(evnt) {
            if (evnt.which != 9) evnt.preventDefault();
        }

        function showDetailDiv() {
            $("#divDetailTable").css("display", "");
            $("#divFilterDetail").css("display", "");
            $("#divMainTable").css("display", "none");
        }

        function showMainPage() {
            $("#divDetailTable").css("display", "none");
            $("#divFilterDetail").css("display", "none");
            $("#divMainTable").css("display", "");
            $("#DdlFilterBy").val("VendorCode");
            $("#txtDateUpdated").val("");
            $("#txtFind").val("");
            $("#TxtFrom").val("");
            $("#TxtTo").val("");
        }

        function SidebarMenu() {
            var SideBarMenu = document.getElementById("SideBarMenu");
            if (SideBarMenu.style.display === "none") {
                SideBarMenu.style.display = "block";
                //$("#SideBarMenu").toggle(500, "easeOutQuint");
            } else {
                //$("#SideBarMenu").toggle(500, "easeOutQuint");
                SideBarMenu.style.display = "none";
            }
        }

        function exportTable() {
            $('#mainTableExport').click();
        }

        function ExpandAll() {
            try {
                $(function () {
                    var table = document.getElementById('GridView1');
                    if (table != null) {
                        var count = $('#GridView1 tr').length;
                        for (var c = 0; c < count; c++) {
                            var ImgUrl = $("#GridView1_Image1_" + c).attr('src');
                            if (ImgUrl != null) {
                                if (ImgUrl.toString() == "images/plus1.png") {
                                    $("#GridView1_Image1_" + c).attr("src", "images/minus1.png");
                                    $("#GridView1_Image1_" + c).closest("tr").after("<tr><td></td><td colspan = '999'>" + $("#GridView1_Image1_" + c).next().html() + "</td></tr>");
                                    var panel = document.getElementById("GridView1_pnlDet_" + c + "");
                                    //if (panel != null) {
                                    //    document.getElementById("GridView1_pnlDet_" + c + "").remove();
                                    //}
                                }
                            }
                        }
                    }
                }); (jQuery)
            }
            catch (err) {
                alert(err + ":ExpandAll")
            }
        }
        function ColapsAll() {
            try {
                $(function () {
                    var table = document.getElementById('GridView1');
                    if (table != null) {
                        var count = $('#GridView1 tr').length;
                        for (var c = 0; c < count; c++) {
                            var ImgUrl = $("#GridView1_Image1_" + c).attr('src');
                            if (ImgUrl != null) {
                                if (ImgUrl.toString() == "images/minus1.png") {
                                    $("#GridView1_Image1_" + c).attr("src", "images/plus1.png");
                                    $("#GridView1_Image1_" + c).closest("tr").next().remove();
                                }
                            }
                        }
                    }
                }); (jQuery)
            }
            catch (err) {
                alert(err + ":expandgrid")
            }
        }

        function openModalLatestSummary() {
            try {
                jQuery.noConflict();
                $('#modalLatestUpdated').modal('show');
            }
            catch (err) {
                alert(err + ' : OpenModalSession');
            }
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

                    $("#TxtModalDueDate").datetimepicker({
                        //format: "dd/mm/yyyy - hh:ii",
                        fontAwesome: 'font-awesome',
                        startDate: new Date(),
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
        function openModal() {
            try {
                jQuery.noConflict();
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
        function loadDatawithDate() {
            if ($("#TxtFrom").val() != "" && $("#TxtTo").val() != "") {
                ShowLoading();
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
        function TriggerNested(IdVsSts) {
            try {
                (function ($) {
                    var ArrIdVsSts = IdVsSts.split('-');
                    var Id = ArrIdVsSts[0].toString();
                    var Status = ArrIdVsSts[1].toString();
                    if (Status == "Ex") {
                        $("#GridView1_Image1_" + Id).closest("tr").after("<tr><td></td><td colspan = '999'>" + $("#GridView1_Image1_" + Id).next().html() + "</td></tr>")
                        $("#GridView1_Image1_" + Id).attr("src", "images/minus1.png");
                    }
                    else {
                        $("#GridView1_Image1_" + Id).attr("src", "images/plus1.png");
                    }
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : TriggerNested');
            }
        }

        function openInNewTab(url) {
            var win = window.open(url, '_blank');
            win.focus();
        }

        function sortingShowLoading() {
            $("th.headerSorting").children("a").on("click", function () {
                ShowLoading();
            });

            $(".pagination-sm td table tbody tr td a").on("click", function () {
                ShowLoading();
            });
        }
    </script>

    <%--script alert and extend session--%>
    <script type="text/javascript">
        try {
            $(function () {
                var timeout = 570000;
                $(document).bind("idle.idleTimer", function () {
                    // function you want to fire when the user goes idle
                    CloseModalCompare();
                    OpenModalSession();
                    $("#StartTimer").click();
                    //$.timeoutDialog({ timeout: 0.25, countdown: 15, logout_redirect_url: 'Login.aspx', restart_on_yes: true });
                });
                $(document).bind("active.idleTimer", function () {
                    // function you want to fire when the user becomes active again
                });
                $.idleTimer(timeout);
            });
        }
        catch (err) {
            alert(err + ' : alert and extend session');
        }
    </script>


    <%--GenerateTbData--%>
    <script type="text/javascript">
        function GenerateSearchByColumn() {
            // Setup - add a text input to each footer cell
            $('#TbData thead tr:eq(1) th').each(function () {
                var title = $(this).text();
                if (title != "") {
                    $(this).html('<input type="text" placeholder="Filter ' + title + '" class="column_search" style="color:black;"/>');
                }
            });

            // Apply the search
            $('#TbData thead').on('keyup', ".column_search", function () {
                dataTable
                    .column($(this).parent().index())
                    .search(this.value)
                    .draw();
            });

            $('#TbMainData thead tr:eq(1) th').each(function () {
                var title = $(this).text();
                if (title != "") {
                    $(this).html('<input type="text" placeholder="Filter ' + title + '" class="column_search" style="color:black;"/>');
                }
            });

            // Apply the search
            $('#TbMainData thead').on('keyup', ".column_search", function () {
                dataTableMainData
                    .column($(this).parent().index())
                    .search(this.value)
                    .draw();
            });
        }

        function formatDate(date) {
            var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2)
                month = '0' + month;
            if (day.length < 2)
                day = '0' + day;

            return [day, month, year].join('/');
        }

        function checkSelectedData() {
            if ($("input.cbbSelectDataCurrentWeek:checked").length == 0) {
                //alert("Please Select at Least 1 Data");
                showDetailDiv();
                return true;
            } else {
                var getDate = new Date();
                var currentDate = formatDate(getDate);
                $("#txtDateUpdated").val(currentDate);
                ShowLoading();
                return true;
            }
        }

        function checkAllCurrentWeek() {
            var checked = $("#chkAllCurrentWeek").prop("checked");
            dataTableMainData.column(10).nodes().to$().each(function (index) {
                var disabled = $(this).find('.chkCurrentWeek').prop('disabled');
                if (checked) {
                    if (disabled == false) {
                        $(this).find('.chkCurrentWeek').prop('checked', 'checked').trigger("change");
                    }
                } else {
                    $(this).find('.chkCurrentWeek').prop('checked', false).trigger("change");
                }
            });
        }

        function GenerateNewMainTable() {
            try {
                jQuery.noConflict();
                dataTableMainData = $("#TbMainData").DataTable({
                    "bDestroy": true,
                    "drawCallback": function () {
                        //$('div.dataTables_filter input').addClass('form-control form-control-sm');
                        $('div.dataTables_filter input').prop('type', 'text');
                        $("#TbMainData_paginate a.paginate_button").click(function () {
                            currentPageMainData = dataTableMainData.page.info().page;
                        });

                        $("a.linkLastWeek").click(function () {
                            var VendorCode = $(this).attr("VendorCode");
                            var CustomerNo = $(this).attr("CustomerNo");
                            var DateUpdated = $(this).attr("DateUpdated");
                            $("#txtDetailVendorCode").val(VendorCode);
                            $("#txtDetailCustomerNo").val(CustomerNo);
                            $("#txtDateUpdated").val(DateUpdated);
                            if (VendorCode != "") {
                                $("#txtFind").val(VendorCode);
                                $("#DdlFilterBy").val("VendorCode");
                            } else {
                                $("#txtFind").val(CustomerNo);
                                $("#DdlFilterBy").val("CustomerNo");
                            }

                            ShowLoading();
                            $("#clickByRow").click();
                        });

                        $("input.cbbSelectDataCurrentWeek").on("change", function () {
                            if ($(this).prop("checked") == true) {
                                var VendorCode = $(this).attr("VendorCode");
                                var CustomerNo = $(this).attr("CustomerNo");
                                var DateUpdated = $(this).attr("DateUpdated");
                                var obj = { VendorCode: VendorCode, CustomerNo: CustomerNo, DateUpdated: DateUpdated }
                                selectedData.push(obj);
                                
                                var result = selectedData.reduce((unique, o) => {
                                    if (!unique.some(obj => obj.VendorCode === o.VendorCode && obj.CustomerNo === o.CustomerNo)) {
                                        unique.push(o);
                                    }
                                    return unique;
                                }, []);

                                selectedData = result;
                                $("#txtSelectedVendorAndCustomer").val(JSON.stringify(selectedData));
                            } else {
                                var VendorCode = $(this).attr("VendorCode");
                                var CustomerNo = $(this).attr("CustomerNo");
                                var DateUpdated = $(this).attr("DateUpdated");

                                if (VendorCode != "") {
                                    $.each(selectedData, function (i, el) {
                                        if (this.VendorCode == VendorCode) {
                                            selectedData.splice(i, 1);
                                        }
                                    });
                                } else if (CustomerNo != "") {
                                    $.each(selectedData, function (i, el) {
                                        if (this.VendorCode == VendorCode) {
                                            selectedData.splice(i, 1);
                                        }
                                    });
                                }
                            }
                        });
                    },
                    "rowCallback": function (row, data, index) {
                        if (data.IsOld == "true") {
                            //Highlight the row
                            $(row).css("background-color", "#F4F1F0");
                        }
                        $(".dataTables_scrollHeadInner").css("width", "100% !important");
                        $(".dataTables_scrollHeadInner table").css("width", "100% !important");
                    },
                    "columns": [
                        //{
                        //    "className": 'details-control',
                        //    "orderable": false,
                        //    "data": null,
                        //    "defaultContent": ''
                        //},
                    {
                        "data": "",
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1 + ".";
                        }
                    },
                    { "data": "Plant", "autoWidth": true },
                    { "data": "VendorCode", "autoWidth": true },
                    { "data": "VendorDesc", "autoWidth": true },
                    { "data": "CustomerNo", "autoWidth": true },
                    { "data": "CustomerDesc", "autoWidth": true },
                    {
                        "data": "LastWeek_3",
                        "render": function (data, type, row, meta) {
                            var currentWeek = row.CurrentWeekNumber;
                            var VendorCode = row.VendorCode;
                            var CustomerNo = row.CustomerNo;
                            var DateUpdated = moment(data).format('DD/MM/YYYY');
                            $("#last3Week").html("Updated Week " + (currentWeek - 3));
                            if (data === null) return "";
                            return '<a VendorCode="' + VendorCode + '" CustomerNo="' + CustomerNo + '" DateUpdated="' + DateUpdated + '" class="linkLastWeek">' + moment(data).format('DD-MM-YYYY') + '</a>';
                        },
                        "autoWidth": true
                    },
                    {
                        "data": "LastWeek_2",
                        "render": function (data, type, row, meta) {
                            var currentWeek = row.CurrentWeekNumber;
                            var VendorCode = row.VendorCode;
                            var CustomerNo = row.CustomerNo;
                            var DateUpdated = moment(data).format('DD/MM/YYYY');
                            $("#last2Week").html("Updated Week " + (currentWeek - 2));
                            if (data === null) return "";
                            return '<a VendorCode="' + VendorCode + '" CustomerNo="' + CustomerNo + '" DateUpdated="' + DateUpdated + '" class="linkLastWeek">' + moment(data).format('DD-MM-YYYY') + '</a>';
                        },
                        "autoWidth": true
                    },
                    {
                        "data": "LastWeek_1",
                        "render": function (data, type, row, meta) {
                            var currentWeek = row.CurrentWeekNumber;
                            var VendorCode = row.VendorCode;
                            var CustomerNo = row.CustomerNo;
                            var DateUpdated = moment(data).format('DD/MM/YYYY');
                            $("#last1Week").html("Updated Week " + (currentWeek - 1));
                            if (data === null) return "";
                            return '<a VendorCode="' + VendorCode + '" CustomerNo="' + CustomerNo + '" DateUpdated="' + DateUpdated + '" class="linkLastWeek">' + moment(data).format('DD-MM-YYYY') + '</a>';
                        },
                        "autoWidth": true
                    },
                    {
                        "data": "CurrentWeek",
                        "render": function (data, type, row, meta) {
                            var currentWeek = row.CurrentWeekNumber;
                            $("#currentWeek").html("Updated Week " + (currentWeek) + " Current Week ");
                            if (data === null) return "";
                            var VendorCode = row.VendorCode;
                            var CustomerNo = row.CustomerNo;
                            var DateUpdated = moment(data).format('DD/MM/YYYY');
                            return '<a VendorCode="' + VendorCode + '" CustomerNo="' + CustomerNo + '" DateUpdated="' + DateUpdated + '" class="linkLastWeek">' + moment(data).format('DD-MM-YYYY') + '</a>';
                        },
                        "autoWidth": true
                    },
                    {
                        "data": "CurrentWeekNumber",
                        "render": function (data, type, row, meta) {
                            var currentWeekDate = row.CurrentWeek;
                            var disabled = "";
                            var currentWeek = row.CurrentWeekNumber;
                            var VendorCode = row.VendorCode;
                            var CustomerNo = row.CustomerNo;
                            var DateUpdated = moment(currentWeekDate).format('DD/MM/YYYY');
                            if (currentWeekDate === null) {
                                disabled = "disabled";
                            }
                            $("#currentWeek").html("Updated Week " + (currentWeek) + " Current Week ");
                            return '<input type="checkbox" style="width: 20px; height: 20px;" class="cbbSelectDataCurrentWeek chkCurrentWeek" VendorCode="' + VendorCode + '" CustomerNo="' + CustomerNo + '" DateUpdated="' + DateUpdated + '" class="linkLastWeek" ' + disabled + '>';
                        },
                        "autoWidth": true
                    }
                    ],
                    columnDefs: [
                            //{ "visible": false, "targets": [20] },
                            //{ "searchable": false, "targets": [20] }
                    ],
                    scrollX: true,
                    scrollY: 430,
                    scrollCollapse: true,
                    orderCellsTop: true,
                    dom: "<'row'<'col-lg-4 col-sm-12 col-md-12'B>>" +
                           "<'row'<'col-lg-12 col-sm-12 col-md-12 col-12 datatableleft'lf<'dtToolbar'>>>" +
                           "<'row'<'col-sm-12'tr>>" +
                           "<'row'<'col-sm-5 col-md-5 col-12'i><'col-sm-7 col-md-7 col-12'p>>",
                    buttons: [
                        {
                            extend: 'excel',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
                                orthogonal: 'export'
                            },
                            attr: {
                                id: 'mainTableExport'
                            },
                        }
                    ],
                    language: {
                        'emptytable': 'No data found',
                        'search': '',
                        'searchPlaceholder': 'Filter All Columns',
                        "lengthMenu": "Show <input class='' id='lcdataTableMainData' value='100' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries"
                    },
                    "ordering": false
                });

                $("#lcdataTableMainData").keydown(function (e) {
                    if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                        e.preventDefault();
                    }
                });

                $("#lcdataTableMainData").on("input", function (e) {
                    var length = $(this).val();
                    var res = length.charAt(0);

                    if (length.length > 1) {
                        if (res == "0") {
                            length = length.substring(1);
                            $(this).val(length)
                        }
                    }

                    if ($(this).val() == "" || $(this).val() == "0") {
                        dataTableMainData.page.len(1).draw();
                    } else {
                        dataTableMainData.page.len(length).draw();
                    }
                });

                $("#lcdataTableMainData").change(function (e) {
                    if ($(this).val() == "" || $(this).val() == "0") {
                        $(this).val("1");
                    }
                });

                var Mydata = [];
                if ($("#TxtDataMainJson").val() != "") {
                    Mydata = JSON.parse($("#TxtDataMainJson").val());
                }
                var length = $("#lcdataTableMainData").val();
                if (length == "" || length == "0") {
                    length = "1";
                    $("#lcdataTableMainData").val("1");
                }
                dataTableMainData.clear().draw();
                dataTableMainData.rows.add(Mydata).draw();
                //length change input textbox
                dataTableMainData.page.len(length).draw();
                $('.dt-buttons').css("display", "none");

                var totalEnableCbb = dataTableMainData.column(10).nodes().to$().find('.chkCurrentWeek:enabled').length;
                if (totalEnableCbb == 0) {
                    $("#chkAllCurrentWeek").prop("disabled", true);
                } else {
                    $("#chkAllCurrentWeek").prop("disabled", false);
                }
            } catch (e) {
                alert("GenerateNewMainTable(): " + e);
            }
        }

        function GenerateTbData() {
            try {

                jQuery.noConflict();
                dataTable = $("#TbData").DataTable({
                    "bDestroy": true,
                    "drawCallback": function () {
                        //$('div.dataTables_filter input').addClass('form-control form-control-sm');
                        $('div.dataTables_filter input').prop('type', 'text');
                        $("#TbData_paginate a.paginate_button").click(function () {
                            currentPage = dataTable.page.info().page;
                        });
                    },
                    "rowCallback": function (row, data, index) {
                        if (data.IsOld == "true") {
                            //Highlight the row
                            $(row).css("background-color", "#F4F1F0");
                        }
                        $(".dataTables_scrollHeadInner").css("width", "100% !important");
                        $(".dataTables_scrollHeadInner table").css("width", "100% !important");
                    },
                    "columns": [
                        //{
                        //    "className": 'details-control',
                        //    "orderable": false,
                        //    "data": null,
                        //    "defaultContent": ''
                        //},
                    {
                        "data": "",
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1 + ".";
                        }
                    },
                    { "data": "Plant", "autoWidth": true },
                    { "data": "VendorCode", "autoWidth": true },
                    { "data": "VndName", "autoWidth": true },
                    { "data": "CustomerNo", "autoWidth": true },
                    { "data": "CustomerDesc", "autoWidth": true },
                    { "data": "SAPCode", "autoWidth": true },
                    { "data": "MaterialDesc", "autoWidth": true },
                    { "data": "Stock", "autoWidth": true },
                    { "data": "UOM", "autoWidth": true },
                    { "data": "Remark", "autoWidth": true },
                    { "data": "MaterialType", "autoWidth": true },
                    { "data": "PirNo", "autoWidth": true },
                    { "data": "PIRDelFlag", "autoWidth": true },
                    { "data": "PlantStatus", "autoWidth": true },
                    { "data": "CreatedBy", "autoWidth": true },
                    {
                        "data": "CreatedDateOri",
                        "render": function (value) {
                            if (value === null) return "";
                            return moment(value).format('DD-MM-YYYY');
                        }, "autoWidth": true
                    },
                    { "data": "UpdatedBy", "autoWidth": true },
                    {
                        "data": "UpdatedDate",
                        "render": function (value) {
                            if (value === null) return "";
                            return moment(value).format('DD-MM-YYYY');
                        }, "autoWidth": true
                    },
                    { "data": "IsOld", "autoWidth": true }
                    ],
                    columnDefs: [
                            { "visible": false, "targets": [19] },
                            { "searchable": false, "targets": [19] }
                    ],
                    scrollX: true,
                    scrollY: 430,
                    scrollCollapse: true,
                    orderCellsTop: true,
                    dom: "<'row'<'col-lg-4 col-sm-12 col-md-12'B>>" +
                           "<'row'<'col-lg-12 col-sm-12 col-md-12 col-12 datatableleft'lf<'dtToolbar'>>>" +
                           "<'row'<'col-sm-12'tr>>" +
                           "<'row'<'col-sm-5 col-md-5 col-12'i><'col-sm-7 col-md-7 col-12'p>>",
                    buttons: [
                        {
                            extend: 'excel',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18],
                                orthogonal: 'export'
                            },
                            attr: {
                                id: 'mainTableExport'
                            },
                        }
                    ],
                    language: {
                        'emptytable': 'No data found',
                        'search': '',
                        'searchPlaceholder': 'Filter All Columns',
                        "lengthMenu": "Show <input class='' id='lcDatatables' value='100' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries"
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
                        if (res == "0") {
                            length = length.substring(1);
                            $(this).val(length)
                        }
                    }

                    if ($(this).val() == "" || $(this).val() == "0") {
                        dataTable.page.len(1).draw();
                    } else {
                        dataTable.page.len(length).draw();
                    }
                });

                $("#lcDatatables").change(function (e) {
                    if ($(this).val() == "" || $(this).val() == "0") {
                        $(this).val("1");
                    }
                });

                var Mydata = [];
                if ($("#TxtDataJson").val() != "") {
                    Mydata = JSON.parse($("#TxtDataJson").val());
                }
                var length = $("#lcDatatables").val();
                if (length == "" || length == "0") {
                    length = "1";
                    $("#lcDatatables").val("1");
                }
                dataTable.clear().draw();
                dataTable.rows.add(Mydata).draw();
                //length change input textbox
                dataTable.page.len(length).draw();

                window.setTimeout(function () {
                    dataTable.columns.adjust().draw();
                }, 500);
                $('.dt-buttons').css("display", "none");
            } catch (e) {
                alert("GenerateTbData(): " + e);
            }
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
                                <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" ID="sidebarToggle" OnClick="sidebarToggle_Click"><i class="fas fa-bars"></i> </asp:LinkButton>
                                <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                                <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-sm-2 fa-pull-right" style="background-color: #E9ECEF;">
                                <asp:Label ID="lblUser" runat="server" Width="147px"></asp:Label><br />
                                <asp:Label ID="lblplant" runat="server" Text=""></asp:Label>
                                <asp:LinkButton runat="server" ID="BtnLogOut" OnClick="BtnLogOut_Click" Text="Logout"></asp:LinkButton>
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
                                <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. Vendor Inventory
                            </div>
                        </div>
                        <div class="card-body">

                            <div id="divFilterDetail" style="display: none;">
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-8" style="padding-bottom: 5px;">
                                                <asp:Label runat="server" ID="LbFilter" Text="Filter By :"></asp:Label>
                                            </div>
                                            <div class="col-sm-4 text-right" style="padding-bottom: 5px;">
                                                <asp:Button Visible="false" ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-sm btn-success" PostBackUrl="Home.aspx" />
                                                <asp:Button ID="btnTemplate" runat="server" Text="Export" CssClass="btn btn-sm btn-primary" OnClientClick="exportTable();" />
                                                <asp:Button runat="server" ID="BtnReset" Text="Reset" CssClass="btn btn-sm btn-warning" OnClick="BtnReset_Click" autopostback="true"></asp:Button>
                                                <asp:Button ID="btnclose" runat="server" Text="Back to Main Page" CssClass="btn btn-sm btn-danger" OnClientClick="showMainPage();" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" DefaultButton="btnSearch">
                                            <div class="row">
                                                <div class="col-lg-4" style="padding-bottom: 5px;">
                                                    <asp:DropDownList runat="server" ID="DdlFilterBy">
                                                        <asp:ListItem Text="Vendor Code" Value="VendorCode"></asp:ListItem>
                                                        <asp:ListItem Text="Vendor Description" Value="VendorDesc"></asp:ListItem>
                                                        <asp:ListItem Text="Customer No" Value="CustomerNo"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Description" Value="CustomerDesc"></asp:ListItem>
                                                        <asp:ListItem Text="SAP Code" Value="Material"></asp:ListItem>
                                                        <asp:ListItem Text="SAP Desc" Value="MaterialDesc"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-8" style="padding-bottom: 5px;">
                                                    <div class="group-main">
                                                        <div class="SearchBox-txt">
                                                            <asp:TextBox runat="server" ID="txtFind" Text=""></asp:TextBox>
                                                        </div>
                                                        <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 4px 3px 1px 3px;">
                                                            <asp:LinkButton ID="btnSearch" CssClass="Padding-Nol" runat="server" autopostback="true" OnClientClick="ShowLoading();" OnClick="btnSearch_Click"><i class="fa fa-search" aria-hidden="true" 
                                                            style="color:#005496;" ></i></asp:LinkButton>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-4" style="padding-bottom: 5px;">
                                                    <asp:DropDownList runat="server" ID="DdlFltrDate" OnSelectedIndexChanged="DdlFltrDate_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="Created Date" Value="CreatedDate"></asp:ListItem>
                                                        <asp:ListItem Text="Updated Date" Value="UpdatedDate"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4" style="padding-bottom: 5px;">
                                                    <div class="group-main">
                                                        <div class="SearchBox-txt">
                                                            <asp:TextBox ID="TxtFrom" runat="server" placeholder="Date From" OnTextChanged="TxtFrom_TextChanged"
                                                                ToolTip="Date From" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                            </asp:TextBox>
                                                        </div>
                                                        <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 3px 2px 1px 1px;">
                                                            <a class="fa fa-calendar" style="color: #005496; padding: 1px 3px 1px 3px;" onclick="javascript: $('#TxtFrom').focus();"></a>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4" style="padding-bottom: 5px;">
                                                    <div class="group-main">
                                                        <div class="SearchBox-txt">
                                                            <asp:TextBox ID="TxtTo" runat="server" placeholder="Date To" OnTextChanged="TxtTo_TextChanged"
                                                                ToolTip="Date To" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                            </asp:TextBox>
                                                        </div>
                                                        <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 3px 2px 1px 1px;">
                                                            <a class="fa fa-calendar" style="color: #005496; padding: 1px 3px 1px 3px;" onclick="javascript: $('#TxtTo').focus();"></a>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <asp:UpdatePanel ID="UpForm" runat="server">
                                <ContentTemplate>
                                    <div class="col-lg-12" style="padding: 0px; display: none;">
                                        <asp:TextBox runat="server" ID="TxtDataJson" Text=""></asp:TextBox>
                                        <asp:TextBox runat="server" ID="TxtDataMainJson" Text=""></asp:TextBox>
                                    </div>

                                    <asp:Panel runat="server" DefaultButton="btnSearch">
                                        <div class="col-lg-12" style="padding: 0;" id="divMainTable">

                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div style="display:none;">                                                        
                                                        <asp:Button ID="clickByRow" runat="server" Text="Show Detail" CssClass="btn btn-sm btn-success" OnClick="btnShowDetail_Click"/>
                                                    </div>
                                                    <div class="col-sm-12 text-right" style="padding-bottom: 5px;">
                                                <asp:Button ID="btnShowDetail" runat="server" Text="Show Detail" CssClass="btn btn-sm btn-success" OnClick="btnShowDetail_Click" OnClientClick="if(checkSelectedData()==false) return false;"/>
                                                        <asp:Button ID="Button7" runat="server" Text="Close" CssClass="btn btn-sm btn-danger" PostBackUrl="Home.aspx" />
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                            <table id="TbMainData" class="table table-bordered Padding-Nol WrapCnt" style="width: 100%; background-color: #006699;">
                                                <thead>
                                                    <tr>
                                                        <th>No</th>
                                                        <th>Plant</th>
                                                        <th>Vendor Code</th>
                                                        <th>Vendor Name</th>
                                                        <th>Customer No</th>
                                                        <th>Customer Description</th>
                                                        <th id="last3Week" style="white-space: normal!important;">Updated Week -</th>
                                                        <th id="last2Week" style="white-space: normal!important;">Updated Week -</th>
                                                        <th id="last1Week" style="white-space: normal!important;">Updated Week -</th>
                                                        <th id="currentWeek" style="white-space: normal!important;">Updated Week - Current Week</th>
                                                        <th style="white-space: normal!important;min-width:70px;">
                                                            <label class="currentWeek" for="chkAllCurrentWeek"> Select All - View Current Week </label>
                                                            <input type="checkbox" id="chkAllCurrentWeek" style="width: 20px; height: 20px;" onclick="checkAllCurrentWeek()"/> 
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="padding: 5px;"></th>
                                                        <th style="padding: 5px;">Plant</th>
                                                        <th style="padding: 5px;">Vendor Code</th>
                                                        <th style="padding: 5px;">Vendor Name</th>
                                                        <th style="padding: 5px;">Customer No</th>
                                                        <th style="padding: 5px;">Customer Description</th>
                                                        <th style="padding: 5px;" class="last3Week">Updated Week -</th>
                                                        <th style="padding: 5px;" class="last2Week">Updated Week -</th>
                                                        <th style="padding: 5px;" class="last1Week">Updated Week -</th>
                                                        <th style="padding: 5px;" class="currentWeek">Updated Week - Current Week</th>
                                                        <th style="padding: 5px;"></th>
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div class="row">
                                                <div class="col-lg-12" style="margin-top:10px;">
                                                    <asp:Label ID="lblPercentageSubmission" runat="server" Text="% Submission = 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                                    <asp:Label ID="lblSubmitted" runat="server" Text="Vendor Submitted = 0" Font-Bold="true" CssClass="fa-pull-right lblpadding30"></asp:Label>
                                                    <asp:Label ID="lblTotalVendor" runat="server" Text="Total No. of Vendors for Inventory = 0" Font-Bold="true" CssClass="fa-pull-right lblpadding30"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-12" style="padding: 0; display: none;" id="divDetailTable">
                                            <table id="TbData" class="table table-bordered nowrap Padding-Nol WrapCnt" style="width: 100%; background-color: #006699;">
                                                <thead>
                                                    <tr>
                                                        <th>No</th>
                                                        <th>Plant</th>
                                                        <th>Vendor Code</th>
                                                        <th>Vendor Name</th>
                                                        <th>Customer No</th>
                                                        <th>Customer Description</th>
                                                        <th>Material</th>
                                                        <th>Material Description</th>
                                                        <th>Stock</th>
                                                        <th>UOM</th>
                                                        <th>Remark</th>
                                                        <th>Material Type</th>
                                                        <th>Pir No</th>
                                                        <th>PIR Del Flag</th>
                                                        <th>Plant Status</th>
                                                        <th>Created By</th>
                                                        <th>Created Date</th>
                                                        <th>Updated By</th>
                                                        <th>Updated Date</th>
                                                        <th>isOld</th>
                                                    </tr>
                                                    <tr>
                                                        <th style="padding: 5px;"></th>
                                                        <th style="padding: 5px;">Plant</th>
                                                        <th style="padding: 5px;">Vendor Code</th>
                                                        <th style="padding: 5px;">Vendor Name</th>
                                                        <th style="padding: 5px;">Customer No</th>
                                                        <th style="padding: 5px;">Customer Description</th>
                                                        <th style="padding: 5px;">Material</th>
                                                        <th style="padding: 5px;">Material Description</th>
                                                        <th style="padding: 5px;">Stock</th>
                                                        <th style="padding: 5px;">UOM</th>
                                                        <th style="padding: 5px;">Remark</th>
                                                        <th style="padding: 5px;">Material Type</th>
                                                        <th style="padding: 5px;">Pir No</th>
                                                        <th style="padding: 5px;">PIR Del Flag</th>
                                                        <th style="padding: 5px;">Plant Status</th>
                                                        <th style="padding: 5px;">Created By</th>
                                                        <th style="padding: 5px;">Created Date</th>
                                                        <th style="padding: 5px;">Updated By</th>
                                                        <th style="padding: 5px;">Updated Date</th>
                                                        <th>isOld</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div>
                                    </asp:Panel>


                                    <div class="row" style="padding-top: 10px; padding-bottom: 10px;">
                                        <div class="col-sm-6 ">
                                            <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:LinkButton runat="server" ID="BtnExpandAll" CssClass="btn btn-sm btn-primary btn-sm" Font-Size="14px"
                                                        OnClientClick="ExpandAll();return false;" autopostback="false"><i class="glyphicon glyphicon-collapse-down"></i> Expand All </asp:LinkButton>
                                                    <asp:LinkButton runat="server" ID="BtnColapsAll" CssClass="btn btn-sm btn-info btn-sm" Font-Size="14px"
                                                        OnClientClick="ColapsAll();return false;" autopostback="false"><i class="glyphicon glyphicon-collapse-up"></i> Collapse All </asp:LinkButton>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                        </div>
                                        <%--<div class="col-sm-6 ">
                                            <asp:Label runat="server" ID="Label1" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                            <asp:TextBox runat="server" ID="TxtShowEntry" Text="10" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                Width="50px" CssClass="fa-pull-right" Style="text-align: center" OnTextChanged="TxtShowEntry_TextChanged" AutoPostBack="true" OnChange="javascript:ShowLoading();"></asp:TextBox>
                                            <asp:Label runat="server" ID="Label2" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                        </div>--%>
                                    </div>

                                    <%--<div class="col-lg-12 table table-responsive" style="padding: 0px;display:none;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true"
                                            UpdateMode="Conditional" RenderMode="Block">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridView1" runat="server"
                                                    AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                    AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand"
                                                    AllowSorting="True" OnSorting="GridView1_Sorting"
                                                    OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound"
                                                    CssClass="table-responsive  table-sm table-bordered table-nowrap  Padding-Nol WrapCnt">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="No.">
                                                            <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                            <ItemStyle Width="3%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDateOri" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="PlantName" HeaderText="Plant Name" SortExpression="PlantName" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" SortExpression="VendorCode" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="VndName" HeaderText="Vendor Name" SortExpression="VndName" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="SAPCode" HeaderText="SAP Code" SortExpression="SAPCode" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="MaterialDesc" HeaderText="SAP Description" SortExpression="MaterialDesc" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="UOM" HeaderText="UOM" SortExpression="UOM" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="PirNo" HeaderText="Pir No." SortExpression="PirNo" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="Stock" HeaderText="Stock" SortExpression="Stock" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" HeaderStyle-CssClass="text-center headerSorting" HtmlEncode="false"></asp:BoundField>
                                                        <asp:BoundField DataField="PIRDelFlag" HeaderText="PIR DelFlag" SortExpression="PIRDelFlag" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="PlantStatus" HeaderText="Plant Status" SortExpression="PlantStatus" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" HeaderStyle-CssClass="text-center headerSorting"></asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                    <PagerSettings PageButtonCount="10" />
                                                    <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                    <RowStyle ForeColor="#000066" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>--%>

                                    <%--<div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="LbTtlRecords" runat="server" Text="Total Record : 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                        </div>
                                    </div>--%>

                                    <div class="row" style="display: none;">
                                        <asp:TextBox runat="server" ID="IsFirstLoad" Text="1"></asp:TextBox>
                                        <asp:Button runat="server" ID="BtnCekLatestNested" Text="reset" OnClick="BtnCekLatestNested_Click" autopostback="true"></asp:Button>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="lblhdnreason" runat="server" Style="visibility: collapse;"></asp:TextBox>
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
                                                <asp:Label ID="LbModalHeader" runat="server" Text="Edit Data Real Time Inventory"
                                                    ForeColor="#004080" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-body" style="background-color: white; padding-bottom: 0px;">
                                        <div class="row" style="padding-bottom: 0px;">
                                            <div class="col-sm-12" style="padding-bottom: 0px;">

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label11" runat="server" Text="Plant"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtPlant" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label3" runat="server" Text="Plant Name"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtPlantName" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label4" runat="server" Text="Vendor"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtVendor" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label5" runat="server" Text="Vendor Name"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtCvendorName" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label6" runat="server" Text="SAP Code"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtSAPCode" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label7" runat="server" Text="SAP Desc"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtSAPDesc" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label8" runat="server" Text="UOM"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtUOM" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label9" runat="server" Text="Stock"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtStock" runat="server" Enabled="true"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label10" runat="server" Text="Remark"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtRemark" runat="server" Enabled="true"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <%--<div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="lblUserName" runat="server" Text="Due Date"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <div class="group-main">
                                                            <div class="SearchBox-txt">
                                                                <asp:TextBox ID="TxtModalDueDate" OnclientClick="return false;" runat="server"
                                                                    OnTextChanged="TxtFrom_TextChanged" AutoPostBack="true" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black">
                                                                </asp:TextBox>
                                                            </div>
                                                            <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 3px 1px 3px;">
                                                                <a class="fa fa-calendar" style="color: #005496; padding: 1px 3px 1px 3px;" onclick="javascript: $('#TxtModalDueDate').focus();"></a>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>--%>

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
                                                                        AllowPaging="false" PageSize="10" OnRowDataBound="GvInvalidRequest_RowDataBound"
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
                                                        <asp:Button ID="btnSubmit" CssClass="btn btn-sm btn-primary" OnClientClick="ShowLoading();" OnClick="btnSubmit_Click" Width="100%"
                                                            Font-Size="14px" runat="server" Text="Save" />
                                                    </div>
                                                    <div class="col-sm-6 " style="padding-bottom: 8px;">
                                                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-default" Text="Close" Width="100%"
                                                            Font-Size="14px" data-dismiss="modal" aria-hidden="true" />
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

        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal fade" id="modalLatestUpdated" data-backdrop="static" tabindex="-1" role="dialog"
                    aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-lg" style="width: 1190px !important;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="modal-content" style="background: linear-gradient(90deg, #F7F7F7, #ffffff, #F7F7F7); border-radius: 15px; max-width: 100%;">
                                    <div class="modal-header">
                                        <div class="row">
                                            <div class="col-sm-12 text-uppercase text-center" style="text-shadow: 1px 2px 1px white;">
                                                <asp:Label ID="Label13" runat="server" Text="Latest Updated Summary"
                                                    ForeColor="#004080" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-body" style="background-color: white; padding-bottom: 0px;">
                                        <div class="row" style="padding-bottom: 0px;">
                                            <div class="col-sm-12" style="padding-bottom: 0px;">
                                                <div class="row" style="padding-bottom: 10px" runat="server" id="Div1">
                                                    <div class="col-md-12">
                                                        <div class="table table-responsive table-sm">
                                                            <asp:GridView ID="GDVLatestUpdate" runat="server" AutoGenerateColumns="False"
                                                                AllowPaging="false" Visible="true"
                                                                CssClass="table-sm table-bordered table-nowrap WrapCnt" Font-Bold="False" Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Plant" HeaderText="Plant" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="VendorCode" HeaderText="Vendor" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="Description" HeaderText="Vendor Descriptoin" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="CustomerNo" HeaderText="Customer No" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="Customerdescription" HeaderText="Customer Description" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="LatestUpdated" HeaderText="Latest Updated" HeaderStyle-CssClass="text-center "></asp:BoundField>
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
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer" style="background-color: #F5F5F5; border-bottom-right-radius: 15px; border-bottom-left-radius: 15px; border-top: 0px;">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class="col-sm-6 " style="padding-bottom: 8px; float: right;">
                                                        <asp:Button ID="Button4" runat="server" CssClass="btn btn-sm btn-default" Text="Close" Width="100%"
                                                            Font-Size="14px" data-dismiss="modal" aria-hidden="true" />
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
                                                <asp:Timer ID="TimerCntDown" runat="server" Interval="1000" OnTick="TimerCntDown_Tick" Enabled="false"></asp:Timer>
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
                                <asp:Button ID="BtnRefresh" runat="server" Text="Yes, Keep me Sign In" OnClick="BtnRefresh_Click" CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="18px" />
                                <asp:Button ID="CtnCloseMdl" runat="server" Text="No, Sign Me Out" OnClick="CtnCloseMdl_Click" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="18px" />
                                <div style="display: none;">
                                    <asp:Button ID="StartTimer" runat="server" Text="Start" OnClick="StartTimer_Click" CssClass="btn btn-sm btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div style="display: none">
            <asp:LinkButton runat="server" ID="BtnDownExport" CssClass="btn btn-sm btn-success btn-sm" Font-Size="14px"
                autopostback="true" OnClick="BtnDownExport_Click"><i class="glyphicon glyphicon-export"></i> Export </asp:LinkButton>
            <asp:TextBox ID="txtDetailVendorCode" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtDetailCustomerNo" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtDateUpdated" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtSelectedVendorAndCustomer" runat="server"></asp:TextBox>
        </div>
    </form>

</body>
</html>
