<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VndReport.aspx.cs" Inherits="Material_Evaluation.VndReport" %>

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

    <%--unread announcement blink text--%>
    <style>
        .blink {
          animation: blink-animation 1s steps(5, start) infinite;
          -webkit-animation: blink-animation 1s steps(5, start) infinite;
        }
        @keyframes blink-animation {
          to {
            visibility: hidden;
          }
        }
        @-webkit-keyframes blink-animation {
          to {
            visibility: hidden;
          }
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
        var dataTableReport;
        var currentPage = 0;
        var NewObjToExport = [];
        var mainUrl = "";

        $(document).ready(function () {
            mainUrl = window.location.href.replace("VndReport.aspx", "");
            SetUpSideBar();
            DatePitcker();
            SetUpSmnResponseDdl();
            GenerateTableReport();
            playAudio();
        });

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

        function Generate() {
            try {
                var LastId = $("#LastId").val();
                LastId = parseInt(LastId) + 1;
                $("#LastId").val(LastId);

                var DvDynamic = $("#DvDynamic");
                DvDynamic.append(
                    '<div class="col-md-4" style="padding-bottom:5px;" id="DvSubFilter' + LastId + '">' +
                        '<div class="row">' +
                            '<div class="col-md-5">' +
                                '<select id="ddl' + LastId + '">' +
                                '<option value="TQ.Plant">Plant</option>' +
                                    '<option value="TQ.RequestNumber">Request Number</option>' +
                                    '<option value="TQ.Product">Product</option>' +
                                    '<option value="TQ.Material">Material</option>' +
                                    '<option value="TQ.MaterialDesc">Material Desc</option>' +
                                    '<option value="TQ.QuoteNo">Quote No</option>' +
                                    '<option value="TQ.VendorCode1">Vendor Code</option>' +
                                    '<option value="TQ.VendorName">Vendor Name</option>' +
                                    '<option value="TQ.ProcessGroup">Process Group ID</option>' +
                                    '<option value="ProcessGroupDesc">Process Group Desc</option>' +
                                    '<option value="CreatedBy">SMN PIC</option>' +
                                    '<option value="UseDep">Department</option>' +
                                '</select>' +
                            '</div>' +
                            '<div class="col-md-7">' +
                                '<div class="group-main">' +
                                '<div class="SearchBox-txt">' +
                                '<input type="text" id="Txt' + LastId + '" onkeyup="ValidateExtraFilter(\'Txt' + LastId + '\')" onpaste="ValidateExtraFilter(\'Txt' + LastId + '\')" >' +
                                '</div>' +
                                '<span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 2px 5px 1px 5px;">' +
                                '<a onclick="RemoveDiv(' + LastId + ');" id="BtnDelSubFltr' + LastId + '" class="Padding-Nol"><i class="fa fa-trash-alt" aria-hidden="true" style="color:#D9534F;"></i></a>' +
                                '</span>' +
                                '</div>' +
                            '</div>' +
                        '</div>' +
                    '</div>'
                );
            }
            catch (err) {
                alert(err);
            }
        }

        function GenerateMulti(Tot) {
            try {
                $("#LastId").val(Tot);
                var DvDynamic = $("#DvDynamic");
                for (var i = 0; i < Tot; i++) {
                    DvDynamic.append(
                        '<div class="col-md-4" style="padding-bottom:5px;" id="DvSubFilter' + i + '">' +
                            '<div class="row">' +
                                '<div class="col-md-5">' +
                                    '<select id="ddl' + i + '">' +
                                    '<option value="TQ.Plant">Plant</option>' +
                                    '<option value="TQ.RequestNumber">Request Number</option>' +
                                    '<option value="TQ.Product">Product</option>' +
                                    '<option value="TQ.Material">Material</option>' +
                                    '<option value="TQ.MaterialDesc">Material Desc</option>' +
                                    '<option value="TQ.QuoteNo">Quote No</option>' +
                                    '<option value="TQ.VendorCode1">Vendor Code</option>' +
                                    '<option value="TQ.VendorName">Vendor Name</option>' +
                                    '<option value="TQ.ProcessGroup">Process Group ID</option>' +
                                    '<option value="ProcessGroupDesc">Process Group Desc</option>' +
                                    '<option value="CreatedBy">SMN PIC</option>' +
                                    '<option value="UseDep">Department</option>' +
                                    '</select>' +
                                '</div>' +
                                '<div class="col-md-7">' +
                                    '<div class="group-main">' +
                                    '<div class="SearchBox-txt">' +
                                    '<input type="text" id="Txt' + i + '" onkeyup="ValidateExtraFilter(\'Txt' + i + '\')" onpaste="ValidateExtraFilter(\'Txt' + i + '\')" >' +
                                    '</div>' +
                                    '<span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 2px 5px 1px 5px;">' +
                                    '<a onclick="RemoveDiv(' + i + ');" id="BtnDelSubFltr' + i + '" class="Padding-Nol"><i class="fa fa-trash-alt" aria-hidden="true" style="color:#D9534F;"></i></a>' +
                                    '</span>' +
                                    '</div>' +
                                '</div>' +
                            '</div>' +
                        '</div>'
                    );
                }
            }
            catch (err) {
                alert("GenerateMulti : " + err);
            }
        }

        function RemoveDiv(Id) {
            try {
                document.getElementById("DvSubFilter" + Id).remove();

                //var LastId = $("#LastId").val();
                //LastId = parseInt(LastId) - 1;
                //$("#LastId").val(LastId);
            }
            catch (err) {
                alert(err);
            }
        }

        function RemoveAllDiv() {
            try {
                var LastId = $("#LastId").val();
                for (var i = 0; i <= LastId; i++) {
                    if (document.getElementById("DvSubFilter" + i) != null) {
                        document.getElementById("DvSubFilter" + i).remove();
                    }
                }
                $("#LastId").val(0);
                $("#TxtExtraFilter").val("");
            }
            catch (err) {
                alert(err);
            }
        }

        function GetAllExtraFilterData() {
            try {
                var LastId = $("#LastId").val();
                var ExtraFilter = "";
                for (var i = 0; i <= LastId; i++) {
                    if (document.getElementById("DvSubFilter" + i) != null) {
                        var ddlSelected = document.getElementById("ddl" + i).options[document.getElementById("ddl" + i).selectedIndex].value;
                        var Txtused = document.getElementById("Txt" + i).value;
                        ExtraFilter += ddlSelected + ":" + Txtused + "|";
                    }
                }
                if (ExtraFilter != "") {
                    ExtraFilter = ExtraFilter.substring(0, ExtraFilter.length - 1)
                }
                $("#TxtExtraFilter").val(ExtraFilter);
            } catch (e) {
                alert("GetAllExtraFilterData : " + e)
            }
        }

        function ReturnExtraFiltercondition() {
            try {
                var ExtraFilter = $("#TxtExtraFilter").val();
                var LastId = $("#LastId").val();
                if (ExtraFilter != "" && LastId != 0) {
                    var ArrExtraFilter = ExtraFilter.split('|');
                    for (var i = 0; i < ArrExtraFilter.length; i++) {
                        var ExtraFilterDet = ArrExtraFilter[i].toString().split(':');
                        debugger;
                        var ddl = document.getElementById('ddl' + i);
                        var Txt = document.getElementById('Txt' + i);
                        if (ddl != null) {
                            var opts = ddl.options.length;
                            for (var d = 0; d < opts; d++) {
                                if (ddl.options[d].value == ExtraFilterDet[0].toString()) {
                                    ddl.options[d].selected = true;
                                    break;
                                }
                            }
                        }

                        if (Txt != null) {
                            Txt.value = ExtraFilterDet[1].toString();
                        }
                    }
                }
                //LastId = parseInt(LastId) - 1;
                //$("#LastId").val(LastId);
            }
            catch (err) {
                alert("ReturnExtraFiltercondition : " + err);
            }
        }

        function ValidateExtraFilter(id) {
            try {
                debugger;
                var fieldText = $("#" + id).val();
                if (fieldText.includes("|")) {
                    alert('Cannot allow character " | " in extra filter search box !');
                    $("#" + id).val("");
                }
                if (fieldText.includes(":")) {
                    alert('Cannot allow character " : " in extra filter search box !');
                    $("#" + id).val("");
                }
                else {

                }
            } catch (e) {
                alert("ValidateExtraFilter : " + e);
                return false;
            }
        }

        function BtnReset_Click() {
            try {
                document.getElementById("DdlReqType").selectedIndex = 0;
                document.getElementById("DdlStatus").selectedIndex = 0;
                document.getElementById("DdlSMNStatus").selectedIndex = 0;
                document.getElementById("DdlFltrDate").selectedIndex = 0;
                document.getElementById("DdlReqStatus").selectedIndex = 0;
                document.getElementById("DdlFilterBy").selectedIndex = 0;
                document.getElementById("TxtFrom").value = "";
                document.getElementById("TxtTo").value = "";
                document.getElementById("txtFind").value = "";

                RemoveAllDiv();
                var length = $("#lcDatatablesTableReport").val();
                if (length == "" || length == "0") {
                    length = "1";
                    $("#lcDatatablesTableReport").val("1");
                }
                dataTableReport.clear().draw();
                dataTableReport.page.len(length).draw();
            } catch (e) {
                alert("BtnReset_Click : " + e)
            }
        }

        function Close_Click() {
            try {
                window.location = mainUrl + "/vendor.aspx";
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

    <%--SetUpSmnResponseDdl--%>
    <script type="text/javascript">
        function SetUpSmnResponseDdl() {
            try {
                $("#DdlSMNStatus").empty();
                var VendorType = document.getElementById("TxtVendorType").value;
                var markup = '';
                markup += '<option value="All">All</option>';
                markup += '<option value="Waiting">Waiting Vend. Submission</option>';
                markup += '<option value="MPending">Mgr. Pending</option>';
                if (VendorType == "TeamShimano") {
                    markup += '<option value="MResubmit">Mgr Request Resubmit</option>';
                }
                markup += '<option value="MApproved">Mgr. Approved</option>';
                markup += '<option value="MRejected">Mgr. Rejected</option>';
                markup += '<option value="DApproved">Dir. Approved</option>';
                markup += '<option value="DRejected">Dir. Rejected</option>';
                $("#DdlSMNStatus").html(markup).show();
            } catch (e) {
                alert("SetUpSmnResponseDdl :" + e);
            }
        }
    </script>

    <script type="text/javascript">
        //Generate data table
        function GenerateTableReport() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableReport = $("#TableReport").DataTable({
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
                                currentPage = dataTableReport.page.info().page;
                            });
                        },
                        "deferRender": true,
                        "columns": [
                        {
                            "data": null,
                            render: function (data, type, row, meta) {
                                return meta.row + meta.settings._iDisplayStart + 1;
                            }
                        },
                        { "data": "Request_Number", "autoWidth": true },
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
                        {
                            "data": "Quote_No",
                            "render": function (data, type, row, meta) {
                                if (type === 'display') {
                                    var url = "QQPReview.aspx?Number=" + data;
                                    return '<a id="LbQSel_' + row.QuoteNo + '" onclick="openInNewTab2(\'' + url + '\')">' + data + '</a>';
                                }
                                return data;
                            },
                            "autoWidth": true
                        },
                        { "data": "Plant", "autoWidth": true },
                        { "data": "GP_Request_Plant", "autoWidth": true },
                        { "data": "Req_Type", "autoWidth": true },
                        { "data": "Req_Status", "autoWidth": true },
                        { "data": "VndRes_Status", "autoWidth": true },
                        { "data": "SMNResStatus", "autoWidth": true },
                        { "data": "SMN_PIC_Dept", "autoWidth": true },
                        { "data": "SMN_PIC", "autoWidth": true },
                        { "data": "SMN_PIC_Email", "autoWidth": true },
                        { "data": "Product", "autoWidth": true },
                        { "data": "Material_Type", "autoWidth": true },
                        { "data": "Material_Class", "autoWidth": true },
                        { "data": "Material", "autoWidth": true },
                        { "data": "Material_Desc", "autoWidth": true },
                        { "data": "Base_UOM", "autoWidth": true },
                        { "data": "Plant_Status", "autoWidth": true },
                        { "data": "Drawing_No", "autoWidth": true },
                        { "data": "SAP_Proc_Type", "autoWidth": true },
                        { "data": "SAP_Sp_Proc_Type", "autoWidth": true },
                        { "data": "PIR_Type", "autoWidth": true },
                        { "data": "PIR_Job_Type", "autoWidth": true },
                        { "data": "Plating_Type", "autoWidth": true },
                        { "data": "Process_Group", "autoWidth": true },
                        { "data": "Req_Recycle_Ratio", "autoWidth": true },
                        { "data": "Request_Purpose", "autoWidth": true },
                        { "data": "MnthEstQty", "autoWidth": true },
                        { "data": "MnthEstQty_UOM", "autoWidth": true },
                        {
                            "data": "FA_Date",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        { "data": "FA_Qty", "autoWidth": true },
                        {
                            "data": "Delivery_Date",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        { "data": "Delivery_Qty", "autoWidth": true },
                        {
                            "data": "Effective_Date",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "Due_Dt_Next_Rev",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "New_Effective_Date",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "New_Due_Dt_Next_Rev",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        { "data": "Previous_Quote_No", "autoWidth": true },
                        { "data": "Incoterm", "autoWidth": true },
                        { "data": "Packing_Req", "autoWidth": true },
                        { "data": "Others_Req", "autoWidth": true },
                        { "data": "Vendor_Code", "autoWidth": true },
                        { "data": "Vendor_Name", "autoWidth": true },
                        { "data": "Vendor_Country_Code", "autoWidth": true },
                        { "data": "Vendor_Currency", "autoWidth": true },
                        { "data": "Vendor_PIC", "autoWidth": true },
                        { "data": "Vendor_PIC_Email", "autoWidth": true },
                        { "data": "Country_Org", "autoWidth": true },
                        { "data": "Raw_Material_SAP_Code", "autoWidth": true },
                        { "data": "Raw_Material_Desc", "autoWidth": true },
                        { "data": "Raw_Material_Cost", "autoWidth": true },
                        { "data": "Raw_Material_Cost_UOM", "autoWidth": true },
                        { "data": "Part_Net_Weight", "autoWidth": true },
                        { "data": "Part_Net_Weight_UOM", "autoWidth": true },
                        { "data": "Part_Unit_Weight", "autoWidth": true },
                        { "data": "Thickness", "autoWidth": true },
                        { "data": "Width", "autoWidth": true },
                        { "data": "Pitch", "autoWidth": true },
                        { "data": "Material_Density", "autoWidth": true },
                        { "data": "Runner_Weightshot", "autoWidth": true },
                        { "data": "Runner_Ratiopcs", "autoWidth": true },
                        { "data": "Recycle_Material_Ratio", "autoWidth": true },
                        { "data": "Cavity", "autoWidth": true },
                        { "data": "MaterialMelting_Loss", "autoWidth": true },
                        { "data": "Material_Gross_Weightpc", "autoWidth": true },
                        { "data": "Material_Scrap_Weight", "autoWidth": true },
                        { "data": "Scrap_Loss_Allowance", "autoWidth": true },
                        { "data": "Scrap_Pricekg", "autoWidth": true },
                        { "data": "Scrap_Rebate_pcs", "autoWidth": true },
                        { "data": "Material_Costpcs", "autoWidth": true },
                        { "data": "Total_Material_Costpcs", "autoWidth": true },
                        { "data": "Process_Grp_Code", "autoWidth": true },
                        { "data": "Sub_Process", "autoWidth": true },
                        { "data": "If_Subcon_Subcon_Name", "autoWidth": true },
                        { "data": "If_TurnkeySub_vendor_name", "autoWidth": true },
                        { "data": "Machine_Labor", "autoWidth": true },
                        { "data": "Machine", "autoWidth": true },
                        { "data": "Standard_RateHR", "autoWidth": true },
                        { "data": "Vendor_Rate_HR", "autoWidth": true },
                        { "data": "Process_UOM", "autoWidth": true },
                        { "data": "Base_Qty", "autoWidth": true },
                        { "data": "Duration_per_Process_UOM", "autoWidth": true },
                        { "data": "Efficiency", "autoWidth": true },
                        { "data": "Turnkey_Costpc", "autoWidth": true },
                        { "data": "Turnkey_Fees", "autoWidth": true },
                        { "data": "Process_Costpc", "autoWidth": true },
                        { "data": "Total_Processes_Costpcs", "autoWidth": true },
                        { "data": "SubMat_TJ_Description", "autoWidth": true },
                        { "data": "SubMat_TJ_Cost", "autoWidth": true },
                        { "data": "Consumption", "autoWidth": true },
                        { "data": "SubMat_TJ_Costpcs", "autoWidth": true },
                        { "data": "Total_SubMat_TJ_Costpcs", "autoWidth": true },
                        { "data": "Items_Description", "autoWidth": true },
                        { "data": "Other_Item_Costpcs", "autoWidth": true },
                        { "data": "Total_Other_Item_Costpcs", "autoWidth": true },
                        { "data": "Total_Material_Cost_pc", "autoWidth": true },
                        { "data": "Total_Process_Cost_pc", "autoWidth": true },
                        { "data": "Total_Sub_Material_Cost_pc", "autoWidth": true },
                        { "data": "Total_Other_items_Cost_pc", "autoWidth": true },
                        { "data": "Grand_Total_Cost__pc", "autoWidth": true },
                        { "data": "Final_Quote_Price_pc", "autoWidth": true },
                        { "data": "Net_ProfitDiscount", "autoWidth": true },
                        { "data": "GA", "autoWidth": true },
                        { "data": "profit", "autoWidth": true },
                        { "data": "discount", "autoWidth": true },
                        { "data": "Comment_By_Vendor", "autoWidth": true },
                        { "data": "Mgr_Decision", "autoWidth": true },
                        { "data": "Mgr_Comment", "autoWidth": true },
                        { "data": "Mgr_Name", "autoWidth": true },
                        {
                            "data": "Mgr_AprRej_Date",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        { "data": "DIR_Decision", "autoWidth": true, "ordering": true },
                        { "data": "DIR_Name", "autoWidth": true, "ordering": false },
                        {
                            "data": "DIRAprRejDate",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        { "data": "DIR_Comment", "autoWidth": true },
                        ],
                        "ordering": true,
                        columnDefs: [{
                            orderable: false,
                            targets: "no-sort"
                        }],
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
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatablesTableReport' value='100' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.Quote_No;
                        }
                    });

                    $("#lcDatatablesTableReport").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesTableReport").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTableReport.page.len(1).draw();
                        } else {
                            dataTableReport.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesTableReport").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });

                } catch (e) {
                    alert("GenerateTableReport() : " + e);
                }
            }
            else {
                alert('GenerateTableReport : Browser Not Support')
            }
        }

        function GetReportData() {
            try {
                jQuery.noConflict();

                var SMNReportDataQuoteDetail = [];
                var SMNReportDataMatCost = [];
                var SMNReportDataProcCost = [];
                var SMNReportDataSubMatCost = [];
                var SMNReportDataOthCost = [];

                ShowLoading();
                if (typeof (Worker) !== "undefined") {
                    setTimeout(function () {
                        var url = mainUrl + "/EmetServices/SMNReport/MyJson.asmx/LoadDataReport";
                        var _Plant = document.getElementById('TxtPlant').value;
                        var _VndCode = document.getElementById('TxtVndCode').value;
                        var _Status = document.getElementById('DdlStatus').value;
                        var _SMNStatus = document.getElementById('DdlSMNStatus').value;
                        var _ReqType = document.getElementById('DdlReqType').value;
                        var _ReqStatus = document.getElementById('DdlReqStatus').value;
                        var _FltrDate = document.getElementById('DdlFltrDate').value;
                        var _From = document.getElementById('TxtFrom').value;
                        var _To = document.getElementById('TxtTo').value;

                        var _FilterBy = document.getElementById('DdlFilterBy').value;
                        var _FilterValue = document.getElementById('txtFind').value;
                        var _ExtraFilter = document.getElementById('TxtExtraFilter').value;

                        $.ajax({
                            url: url,
                            cache: false,
                            type: "POST",
                            dataType: 'json',
                            //contentType: "application/json; charset=utf-8",
                            data: {
                                Plant: _Plant, Status: _Status, SMNStatus: _SMNStatus, ReqType: _ReqType, ReqStatus: _ReqStatus,
                                FltrDate: _FltrDate, From: _From, To: _To, FilterBy: _FilterBy, FilterValue: _FilterValue,
                                ExtraFilter: _ExtraFilter, VendorCode : _VndCode
                            },
                            async: false,
                            beforeSend: function () {
                                ShowLoading();
                            },
                            complete: function () {
                                CloseLoading();
                                SetupAndLoadData(SMNReportDataQuoteDetail, SMNReportDataMatCost, SMNReportDataProcCost, SMNReportDataSubMatCost, SMNReportDataOthCost);
                            },
                            success: function (data) {
                                if (data.success == true) {
                                    var length = $("#lcDatatables").val();
                                    if (length == "" || length == "0") {
                                        length = "1";
                                        $("#lcDatatables").val("1");
                                    }
                                    SMNReportDataQuoteDetail = data.SMNReportDataQuoteDetail;
                                    SMNReportDataMatCost = data.SMNReportDataMatCost;
                                    SMNReportDataProcCost = data.SMNReportDataProcCost;
                                    SMNReportDataSubMatCost = data.SMNReportDataSubMatCost;
                                    SMNReportDataOthCost = data.SMNReportDataOthCost;
                                }
                                else {
                                    alert(data.message);
                                }
                            },
                            error: function (xhr, status, error) {
                                alert(error);
                            }
                        });
                    }, 100);
                }
                else {
                    alert('GetReportData : Browser Not Support')
                }
            } catch (e) {
                alert("GetReportData : " + e);
                CloseLoading();
            }
        }

        function SetupAndLoadData(SMNReportDataQuoteDetail, SMNReportDataMatCost, SMNReportDataProcCost, SMNReportDataSubMatCost, SMNReportDataOthCost) {
            try {
                jQuery.noConflict();
                var newObj = [];
                NewObjToExport = [];
                for (var i = 0; i < SMNReportDataQuoteDetail.length; i++) {
                    var MainQuoteNo = SMNReportDataQuoteDetail[i].Quote_No;
                    var MatCostfiltered = SMNReportDataMatCost.filter(a => a.QuoteNo == MainQuoteNo);
                    var ProcCostfiltered = SMNReportDataProcCost.filter(a => a.QuoteNo == MainQuoteNo);
                    var SubMatCostfiltered = SMNReportDataSubMatCost.filter(a => a.QuoteNo == MainQuoteNo);
                    var OthCostfiltered = SMNReportDataOthCost.filter(a => a.QuoteNo == MainQuoteNo);

                    var HighNoRow = Math.max(MatCostfiltered.length, ProcCostfiltered.length, SubMatCostfiltered.length, OthCostfiltered.length);
                    for (var h = 0; h < HighNoRow; h++) {
                        var Raw_Material_SAP_Code = "";
                        var Raw_Material_Desc = "";
                        var Raw_Material_Cost = "";
                        var Raw_Material_Cost_UOM = "";
                        var Part_Net_Weight = "";
                        var Part_Net_Weight_UOM = "";
                        var Part_Unit_Weight = "";
                        var Thickness = "";
                        var Width = "";
                        var Pitch = "";
                        var Material_Density = "";
                        var Runner_Weightshot = "";
                        var Runner_Ratiopcs = "";
                        var Recycle_Material_Ratio = "";
                        var Cavity = "";
                        var MaterialMelting_Loss = "";
                        var Material_Gross_Weightpc = "";
                        var Material_Scrap_Weight = "";
                        var Scrap_Loss_Allowance = "";
                        var Scrap_Pricekg = "";
                        var Scrap_Rebate_pcs = "";
                        var Material_Costpcs = "";
                        var Total_Material_Costpcs = "";

                        var Process_Grp_Code = "";
                        var Sub_Process = "";
                        var If_Subcon_Subcon_Name = "";
                        var If_TurnkeySub_vendor_name = "";
                        var Machine_Labor = "";
                        var Machine = "";
                        var Standard_RateHR = "";
                        var Vendor_Rate_HR = "";
                        var Process_UOM = "";
                        var Base_Qty = "";
                        var Duration_per_Process_UOM = "";
                        var Efficiency = "";
                        var Turnkey_Costpc = "";
                        var Turnkey_Fees = "";
                        var Process_Costpc = "";
                        var Total_Processes_Costpcs = "";

                        var SubMat_TJ_Description = "";
                        var SubMat_TJ_Cost = "";
                        var Consumption = "";
                        var SubMat_TJ_Costpcs = "";
                        var Total_SubMat_TJ_Costpcs = "";

                        var Items_Description = "";
                        var Other_Item_Costpcs = "";
                        var Total_Other_Item_Costpcs = "";

                        if (MatCostfiltered[h] != null) {
                            Raw_Material_SAP_Code = MatCostfiltered[h].Raw_Material_SAP_Code;
                            Raw_Material_Desc = MatCostfiltered[h].Raw_Material_Desc;
                            Raw_Material_Cost = MatCostfiltered[h].Raw_Material_Cost;
                            Raw_Material_Cost_UOM = MatCostfiltered[h].Raw_Material_Cost_UOM;
                            Part_Net_Weight = MatCostfiltered[h].Part_Net_Weight;
                            Part_Net_Weight_UOM = MatCostfiltered[h].Part_Net_Weight_UOM;
                            Part_Unit_Weight = MatCostfiltered[h].Part_Unit_Weight;
                            Thickness = MatCostfiltered[h].Thickness;
                            Width = MatCostfiltered[h].Width;
                            Pitch = MatCostfiltered[h].Pitch;
                            Material_Density = MatCostfiltered[h].Material_Density;
                            Runner_Weightshot = MatCostfiltered[h].Runner_Weightshot;
                            Runner_Ratiopcs = MatCostfiltered[h].Runner_Ratiopcs;
                            Recycle_Material_Ratio = MatCostfiltered[h].Recycle_Material_Ratio;
                            Cavity = MatCostfiltered[h].Cavity;
                            MaterialMelting_Loss = MatCostfiltered[h].MaterialMelting_Loss;
                            Material_Gross_Weightpc = MatCostfiltered[h].Material_Gross_Weightpc;
                            Material_Scrap_Weight = MatCostfiltered[h].Material_Scrap_Weight;
                            Scrap_Loss_Allowance = MatCostfiltered[h].Scrap_Loss_Allowance;
                            Scrap_Pricekg = MatCostfiltered[h].Scrap_Pricekg;
                            Scrap_Rebate_pcs = MatCostfiltered[h].Scrap_Rebate_pcs;
                            Material_Costpcs = MatCostfiltered[h].Material_Costpcs;
                            Total_Material_Costpcs = MatCostfiltered[h].Total_Material_Costpcs;
                        }

                        if (ProcCostfiltered[h] != null) {
                            Process_Grp_Code = ProcCostfiltered[h].Process_Grp_Code;
                            Sub_Process = ProcCostfiltered[h].Sub_Process;
                            If_Subcon_Subcon_Name = ProcCostfiltered[h].If_Subcon_Subcon_Name;
                            If_TurnkeySub_vendor_name = ProcCostfiltered[h].If_TurnkeySub_vendor_name;
                            Machine_Labor = ProcCostfiltered[h].Machine_Labor;
                            Machine = ProcCostfiltered[h].Machine;
                            Standard_RateHR = ProcCostfiltered[h].Standard_RateHR;
                            Vendor_Rate_HR = ProcCostfiltered[h].Vendor_Rate_HR;
                            Process_UOM = ProcCostfiltered[h].Process_UOM;
                            Base_Qty = ProcCostfiltered[h].Base_Qty;
                            Duration_per_Process_UOM = ProcCostfiltered[h].Duration_per_Process_UOM;
                            Efficiency = ProcCostfiltered[h].Efficiency;
                            Turnkey_Costpc = ProcCostfiltered[h].Turnkey_Costpc;
                            Turnkey_Fees = ProcCostfiltered[h].Turnkey_Fees;
                            Process_Costpc = ProcCostfiltered[h].Process_Costpc;
                            Total_Processes_Costpcs = ProcCostfiltered[h].Total_Processes_Costpcs;
                        }

                        if (SubMatCostfiltered[h] != null) {
                            SubMat_TJ_Description = SubMatCostfiltered[h].SubMat_TJ_Description;
                            SubMat_TJ_Cost = SubMatCostfiltered[h].SubMat_TJ_Cost;
                            Consumption = SubMatCostfiltered[h].Consumption;
                            SubMat_TJ_Costpcs = SubMatCostfiltered[h].SubMat_TJ_Costpcs;
                            Total_SubMat_TJ_Costpcs = SubMatCostfiltered[h].Total_SubMat_TJ_Costpcs;
                        }

                        if (OthCostfiltered[h] != null) {
                            Items_Description = OthCostfiltered[h].Items_Description;
                            Other_Item_Costpcs = OthCostfiltered[h].Other_Item_Costpcs;
                            Total_Other_Item_Costpcs = OthCostfiltered[h].Total_Other_Item_Costpcs;
                        }

                        newObj.push({
                            "Request_Number": SMNReportDataQuoteDetail[i].Request_Number,
                            "RequestDate": SMNReportDataQuoteDetail[i].RequestDate,
                            "QuoteResponseDueDate": SMNReportDataQuoteDetail[i].QuoteResponseDueDate,
                            "Quote_No": SMNReportDataQuoteDetail[i].Quote_No,
                            "Plant": SMNReportDataQuoteDetail[i].Plant,
                            "GP_Request_Plant": SMNReportDataQuoteDetail[i].GP_Request_Plant,
                            "Req_Type": SMNReportDataQuoteDetail[i].Req_Type,
                            "Req_Status": SMNReportDataQuoteDetail[i].Req_Status,
                            "VndRes_Status": SMNReportDataQuoteDetail[i].VndRes_Status,
                            "SMNResStatus": SMNReportDataQuoteDetail[i].SMNResStatus,
                            "SMN_PIC_Dept": SMNReportDataQuoteDetail[i].SMN_PIC_Dept,
                            "SMN_PIC": SMNReportDataQuoteDetail[i].SMN_PIC,
                            "SMN_PIC_Email": SMNReportDataQuoteDetail[i].SMN_PIC_Email,
                            "Product": SMNReportDataQuoteDetail[i].Product,
                            "Material_Type": SMNReportDataQuoteDetail[i].Material_Type,
                            "Material_Class": SMNReportDataQuoteDetail[i].Material_Class,
                            "Material": SMNReportDataQuoteDetail[i].Material,
                            "Material_Desc": SMNReportDataQuoteDetail[i].Material_Desc,
                            "Base_UOM": SMNReportDataQuoteDetail[i].Base_UOM,
                            "Plant_Status": SMNReportDataQuoteDetail[i].Plant_Status,
                            "Drawing_No": SMNReportDataQuoteDetail[i].Drawing_No,
                            "SAP_Proc_Type": SMNReportDataQuoteDetail[i].SAP_Proc_Type,
                            "SAP_Sp_Proc_Type": SMNReportDataQuoteDetail[i].SAP_Sp_Proc_Type,
                            "PIR_Type": SMNReportDataQuoteDetail[i].PIR_Type,
                            "PIR_Job_Type": SMNReportDataQuoteDetail[i].PIR_Job_Type,
                            "Plating_Type": SMNReportDataQuoteDetail[i].Plating_Type,
                            "Process_Group": SMNReportDataQuoteDetail[i].Process_Group,
                            "Req_Recycle_Ratio": SMNReportDataQuoteDetail[i].Req_Recycle_Ratio,
                            "Request_Purpose": SMNReportDataQuoteDetail[i].Request_Purpose,
                            "MnthEstQty": SMNReportDataQuoteDetail[i].MnthEstQty,
                            "MnthEstQty_UOM": SMNReportDataQuoteDetail[i].MnthEstQty_UOM,
                            "FA_Date": SMNReportDataQuoteDetail[i].FA_Date,
                            "FA_Qty": SMNReportDataQuoteDetail[i].FA_Qty,
                            "Delivery_Date": SMNReportDataQuoteDetail[i].Delivery_Date,
                            "Delivery_Qty": SMNReportDataQuoteDetail[i].Delivery_Qty,
                            "Effective_Date": SMNReportDataQuoteDetail[i].Effective_Date,
                            "Due_Dt_Next_Rev": SMNReportDataQuoteDetail[i].Due_Dt_Next_Rev,
                            "New_Effective_Date": SMNReportDataQuoteDetail[i].New_Effective_Date,
                            "New_Due_Dt_Next_Rev": SMNReportDataQuoteDetail[i].New_Due_Dt_Next_Rev,
                            "Previous_Quote_No": SMNReportDataQuoteDetail[i].Previous_Quote_No,
                            "Incoterm": SMNReportDataQuoteDetail[i].Incoterm,
                            "Packing_Req": SMNReportDataQuoteDetail[i].Packing_Req,
                            "Others_Req": SMNReportDataQuoteDetail[i].Others_Req,
                            "Vendor_Code": SMNReportDataQuoteDetail[i].Vendor_Code,
                            "Vendor_Name": SMNReportDataQuoteDetail[i].Vendor_Name,
                            "Vendor_Country_Code": SMNReportDataQuoteDetail[i].Vendor_Country_Code,
                            "Vendor_Currency": SMNReportDataQuoteDetail[i].Vendor_Currency,
                            "Vendor_PIC": SMNReportDataQuoteDetail[i].Vendor_PIC,
                            "Vendor_PIC_Email": SMNReportDataQuoteDetail[i].Vendor_PIC_Email,
                            "Country_Org": SMNReportDataQuoteDetail[i].Country_Org,
                            "Raw_Material_SAP_Code": Raw_Material_SAP_Code,
                            "Raw_Material_Desc": Raw_Material_Desc,
                            "Raw_Material_Cost": Raw_Material_Cost,
                            "Raw_Material_Cost_UOM": Raw_Material_Cost_UOM,
                            "Part_Net_Weight": Part_Net_Weight,
                            "Part_Net_Weight_UOM": Part_Net_Weight_UOM,
                            "Part_Unit_Weight": Part_Unit_Weight,
                            "Thickness": Thickness,
                            "Width": Width,
                            "Pitch": Pitch,
                            "Material_Density": Material_Density,
                            "Runner_Weightshot": Runner_Weightshot,
                            "Runner_Ratiopcs": Runner_Ratiopcs,
                            "Recycle_Material_Ratio": Recycle_Material_Ratio,
                            "Cavity": Cavity,
                            "MaterialMelting_Loss": MaterialMelting_Loss,
                            "Material_Gross_Weightpc": Material_Gross_Weightpc,
                            "Material_Scrap_Weight": Material_Scrap_Weight,
                            "Scrap_Loss_Allowance": Scrap_Loss_Allowance,
                            "Scrap_Pricekg": Scrap_Pricekg,
                            "Scrap_Rebate_pcs": Scrap_Rebate_pcs,
                            "Material_Costpcs": Material_Costpcs,
                            "Total_Material_Costpcs": Total_Material_Costpcs,
                            "Process_Grp_Code": Process_Grp_Code,
                            "Sub_Process": Sub_Process,
                            "If_Subcon_Subcon_Name": If_Subcon_Subcon_Name,
                            "If_TurnkeySub_vendor_name": If_TurnkeySub_vendor_name,
                            "Machine_Labor": Machine_Labor,
                            "Machine": Machine,
                            "Standard_RateHR": Standard_RateHR,
                            "Vendor_Rate_HR": Vendor_Rate_HR,
                            "Process_UOM": Process_UOM,
                            "Base_Qty": Base_Qty,
                            "Duration_per_Process_UOM": Duration_per_Process_UOM,
                            "Efficiency": Efficiency,
                            "Turnkey_Costpc": Turnkey_Costpc,
                            "Turnkey_Fees": Turnkey_Fees,
                            "Process_Costpc": Process_Costpc,
                            "Total_Processes_Costpcs": Total_Processes_Costpcs,
                            "SubMat_TJ_Description": SubMat_TJ_Description,
                            "SubMat_TJ_Cost": SubMat_TJ_Cost,
                            "Consumption": Consumption,
                            "SubMat_TJ_Costpcs": SubMat_TJ_Costpcs,
                            "Total_SubMat_TJ_Costpcs": Total_SubMat_TJ_Costpcs,
                            "Items_Description": Items_Description,
                            "Other_Item_Costpcs": Other_Item_Costpcs,
                            "Total_Other_Item_Costpcs": Total_Other_Item_Costpcs,
                            "Total_Material_Cost_pc": SMNReportDataQuoteDetail[i].Total_Material_Cost_pc,
                            "Total_Process_Cost_pc": SMNReportDataQuoteDetail[i].Total_Process_Cost_pc,
                            "Total_Sub_Material_Cost_pc": SMNReportDataQuoteDetail[i].Total_Sub_Material_Cost_pc,
                            "Total_Other_items_Cost_pc": SMNReportDataQuoteDetail[i].Total_Other_items_Cost_pc,
                            "Grand_Total_Cost__pc": SMNReportDataQuoteDetail[i].Grand_Total_Cost__pc,
                            "Final_Quote_Price_pc": SMNReportDataQuoteDetail[i].Final_Quote_Price_pc,
                            "Net_ProfitDiscount": SMNReportDataQuoteDetail[i].Net_ProfitDiscount,
                            "GA": SMNReportDataQuoteDetail[i].GA,
                            "profit": SMNReportDataQuoteDetail[i].profit,
                            "discount": SMNReportDataQuoteDetail[i].discount,
                            "Comment_By_Vendor": SMNReportDataQuoteDetail[i].Comment_By_Vendor,
                            "Mgr_Decision": SMNReportDataQuoteDetail[i].Mgr_Decision,
                            "Mgr_Comment": SMNReportDataQuoteDetail[i].Mgr_Comment,
                            "Mgr_Name": SMNReportDataQuoteDetail[i].Mgr_Name,
                            "Mgr_AprRej_Date": SMNReportDataQuoteDetail[i].Mgr_AprRej_Date,
                            "DIR_Decision": SMNReportDataQuoteDetail[i].DIR_Decision,
                            "DIR_Name": SMNReportDataQuoteDetail[i].DIR_Name,
                            "DIRAprRejDate": SMNReportDataQuoteDetail[i].DIRAprRejDate,
                            "DIR_Comment": SMNReportDataQuoteDetail[i].DIR_Comment
                        });

                        var SetUpObjToExport = [];
                        SetUpObjToExport.push(

                            SMNReportDataQuoteDetail[i].Request_Number.toString(),
                            SMNReportDataQuoteDetail[i].RequestDate === null ? '' : moment(SMNReportDataQuoteDetail[i].RequestDate).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].QuoteResponseDueDate === null ? '' : moment(SMNReportDataQuoteDetail[i].QuoteResponseDueDate).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].Quote_No,
                            SMNReportDataQuoteDetail[i].Plant,
                            SMNReportDataQuoteDetail[i].GP_Request_Plant,
                            SMNReportDataQuoteDetail[i].Req_Type,
                            SMNReportDataQuoteDetail[i].Req_Status,
                            SMNReportDataQuoteDetail[i].VndRes_Status,
                            SMNReportDataQuoteDetail[i].SMNResStatus,
                            SMNReportDataQuoteDetail[i].SMN_PIC_Dept,
                            SMNReportDataQuoteDetail[i].SMN_PIC,
                            SMNReportDataQuoteDetail[i].SMN_PIC_Email,
                            SMNReportDataQuoteDetail[i].Product,
                            SMNReportDataQuoteDetail[i].Material_Type,
                            SMNReportDataQuoteDetail[i].Material_Class,
                            SMNReportDataQuoteDetail[i].Material,
                            SMNReportDataQuoteDetail[i].Material_Desc,
                            SMNReportDataQuoteDetail[i].Base_UOM,
                            SMNReportDataQuoteDetail[i].Plant_Status,
                            SMNReportDataQuoteDetail[i].Drawing_No,
                            SMNReportDataQuoteDetail[i].SAP_Proc_Type,
                            SMNReportDataQuoteDetail[i].SAP_Sp_Proc_Type,
                            SMNReportDataQuoteDetail[i].PIR_Type,
                            SMNReportDataQuoteDetail[i].PIR_Job_Type,
                            SMNReportDataQuoteDetail[i].Plating_Type,
                            SMNReportDataQuoteDetail[i].Process_Group,
                            SMNReportDataQuoteDetail[i].Req_Recycle_Ratio,
                            SMNReportDataQuoteDetail[i].Request_Purpose,
                            SMNReportDataQuoteDetail[i].MnthEstQty,
                            SMNReportDataQuoteDetail[i].MnthEstQty_UOM,
                            SMNReportDataQuoteDetail[i].FA_Date === null ? '' : moment(SMNReportDataQuoteDetail[i].FA_Date).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].FA_Qty,
                            SMNReportDataQuoteDetail[i].Delivery_Date === null ? '' : moment(SMNReportDataQuoteDetail[i].Delivery_Date).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].Delivery_Qty,
                            SMNReportDataQuoteDetail[i].Effective_Date === null ? '' : moment(SMNReportDataQuoteDetail[i].Effective_Date).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].Due_Dt_Next_Rev === null ? '' : moment(SMNReportDataQuoteDetail[i].Due_Dt_Next_Rev).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].New_Effective_Date === null ? '' : moment(SMNReportDataQuoteDetail[i].New_Effective_Date).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].New_Due_Dt_Next_Rev === null ? '' : moment(SMNReportDataQuoteDetail[i].New_Due_Dt_Next_Rev).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].Previous_Quote_No,
                            SMNReportDataQuoteDetail[i].Incoterm,
                            SMNReportDataQuoteDetail[i].Packing_Req,
                            SMNReportDataQuoteDetail[i].Others_Req,
                            SMNReportDataQuoteDetail[i].Vendor_Code,
                            SMNReportDataQuoteDetail[i].Vendor_Name,
                            SMNReportDataQuoteDetail[i].Vendor_Country_Code,
                            SMNReportDataQuoteDetail[i].Vendor_Currency,
                            SMNReportDataQuoteDetail[i].Vendor_PIC,
                            SMNReportDataQuoteDetail[i].Vendor_PIC_Email,
                            SMNReportDataQuoteDetail[i].Country_Org,
                            Raw_Material_SAP_Code,
                            Raw_Material_Desc,
                            Raw_Material_Cost,
                            Raw_Material_Cost_UOM,
                            Part_Net_Weight,
                            Part_Net_Weight_UOM,
                            Part_Unit_Weight,
                            Thickness,
                            Width,
                            Pitch,
                            Material_Density,
                            Runner_Weightshot,
                            Runner_Ratiopcs,
                            Recycle_Material_Ratio,
                            Cavity,
                            MaterialMelting_Loss,
                            Material_Gross_Weightpc,
                            Material_Scrap_Weight,
                            Scrap_Loss_Allowance,
                            Scrap_Pricekg,
                            Scrap_Rebate_pcs,
                            Material_Costpcs,
                            Total_Material_Costpcs,
                            Process_Grp_Code,
                            Sub_Process,
                            If_Subcon_Subcon_Name,
                            If_TurnkeySub_vendor_name,
                            Machine_Labor,
                            Machine,
                            Standard_RateHR,
                            Vendor_Rate_HR,
                            Process_UOM,
                            Base_Qty,
                            Duration_per_Process_UOM,
                            Efficiency,
                            Turnkey_Costpc,
                            Turnkey_Fees,
                            Process_Costpc,
                            Total_Processes_Costpcs,
                            SubMat_TJ_Description,
                            SubMat_TJ_Cost,
                            Consumption,
                            SubMat_TJ_Costpcs,
                            Total_SubMat_TJ_Costpcs,
                            Items_Description,
                            Other_Item_Costpcs,
                            Total_Other_Item_Costpcs,
                            SMNReportDataQuoteDetail[i].Total_Material_Cost_pc,
                            SMNReportDataQuoteDetail[i].Total_Process_Cost_pc,
                            SMNReportDataQuoteDetail[i].Total_Sub_Material_Cost_pc,
                            SMNReportDataQuoteDetail[i].Total_Other_items_Cost_pc,
                            SMNReportDataQuoteDetail[i].Grand_Total_Cost__pc,
                            SMNReportDataQuoteDetail[i].Final_Quote_Price_pc,
                            SMNReportDataQuoteDetail[i].Net_ProfitDiscount,
                            SMNReportDataQuoteDetail[i].GA,
                            SMNReportDataQuoteDetail[i].profit,
                            SMNReportDataQuoteDetail[i].discount,
                            SMNReportDataQuoteDetail[i].Comment_By_Vendor,
                            SMNReportDataQuoteDetail[i].Mgr_Decision,
                            SMNReportDataQuoteDetail[i].Mgr_Comment,
                            SMNReportDataQuoteDetail[i].Mgr_Name,
                            SMNReportDataQuoteDetail[i].Mgr_AprRej_Date === null ? '' : moment(SMNReportDataQuoteDetail[i].Mgr_AprRej_Date).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].DIR_Decision,
                            SMNReportDataQuoteDetail[i].DIR_Name,
                            SMNReportDataQuoteDetail[i].DIRAprRejDate === null ? '' : moment(SMNReportDataQuoteDetail[i].DIRAprRejDate).format('DD-MM-YYYY'),
                            SMNReportDataQuoteDetail[i].DIR_Comment)

                        NewObjToExport.push(SetUpObjToExport);
                    }
                }

                var length = $("#lcDatatablesTableReport").val();
                if (length == "" || length == "0") {
                    length = "1";
                    $("#lcDatatablesTableReport").val("1");
                }
                dataTableReport.clear().draw();
                dataTableReport.rows.add(newObj).draw();
                //length change input textbox
                dataTableReport.page.len(length).draw();
            } catch (e) {
                alert("SetupAndLoadData : " + e)
            }
        }

        function openInNewTab2(url) {
            var win = window.open(url, '_blank');
            win.focus();
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

                    var MyHeader = ["Request Number",
                                    "Request Date",
                                    "Response Due Date",
                                    "Quote No",
                                    "Plant",
                                    "GP Request Plant",
                                    "Req Type",
                                    "Req Status",
                                    "Vnd.Res. Status",
                                    "SMN.Res.Status",
                                    "SMN PIC Dept",
                                    "SMN PIC",
                                    "SMN PIC Email",
                                    "Product",
                                    "Material Type",
                                    "Material Class",
                                    "Material",
                                    "Material Desc",
                                    "Base UOM",
                                    "Plant Status",
                                    "Drawing No",
                                    "SAP Proc Type",
                                    "SAP Sp Proc Type",
                                    "PIR Type",
                                    "PIR Job Type",
                                    "Plating Type",
                                    "Process Group",
                                    "Req Recycle Ratio (%)",
                                    "Request Purpose",
                                    "Mnth.Est.Qty",
                                    "Mnth.Est.Qty UOM",
                                    "FA Date",
                                    "FA Qty",
                                    "1st Delivery Date",
                                    "1st Delivery Qty",
                                    "Effective Date",
                                    "Due Dt Next Rev",
                                    "New Effective Date",
                                    "New Due Dt Next Rev",
                                    "Previous Quote No.",
                                    "Incoterm",
                                    "Packing Req",
                                    "Others Req",
                                    "Vendor Code",
                                    "Vendor Name",
                                    "Vendor Country Code",
                                    "Vendor Currency",
                                    "Vendor PIC",
                                    "Vendor PIC Email",
                                    "Country Org",
                                    "Raw Material SAP Code",
                                    "Raw Material Desc",
                                    "Raw Material Cost",
                                    "Raw Material Cost UOM",
                                    "Part Net Weight",
                                    "Part Net Weight UOM",
                                    "Part Unit Weight (g)",
                                    "Thickness (mm)",
                                    "Width (mm)",
                                    "Pitch (mm)",
                                    "Material Density",
                                    "Runner Weight/shot (g)",
                                    "Runner Ratio/pcs (%)",
                                    "Recycle Material Ratio (%)",
                                    "Cavity",
                                    "Material/Melting Loss (%)",
                                    "Material Gross Weight/pc (g)",
                                    "Material Scrap Weight (g)",
                                    "Scrap Loss Allowance (%)",
                                    "Scrap Price/kg",
                                    "Scrap Rebate / pcs",
                                    "Material Cost/pcs",
                                    "Total Material Cost/pcs",
                                    "Process Grp Code",
                                    "Sub Process",
                                    "If Subcon - Subcon Name",
                                    "If Turnkey- Sub vendor name",
                                    "Machine / Labor",
                                    "Machine",
                                    "Standard Rate/HR",
                                    "Vendor Rate / HR",
                                    "Process UOM",
                                    "Base Qty",
                                    "Duration per Process UOM (Sec)",
                                    "Efficiency",
                                    "Turnkey Cost/pc",
                                    "Turnkey Fees",
                                    "Process Cost/pc",
                                    "Total Processes Cost/pcs",
                                    "Sub-Mat / T&J Description",
                                    "Sub-Mat / T&J Cost",
                                    "Consumption (pcs)",
                                    "Sub-Mat / T&J Cost/pcs",
                                    "Total Sub-Mat / T&J Cost/pcs",
                                    "Items Description",
                                    "Other Item Cost/pcs",
                                    "Total Other Item Cost/pcs",
                                    "Total Material Cost / pc",
                                    "Total Process Cost / pc",
                                    "Total Sub Material Cost / pc",
                                    "Total Other items Cost / pc",
                                    "Grand Total Cost / pc",
                                    "Final Quote Price / pc",
                                    "Net Profit/Discount (%)",
                                    "GA (%)",
                                    "profit (%)",
                                    "discount (%)",
                                    "Comment By Vendor",
                                    "Mgr Decision",
                                    "Mgr Comment",
                                    "Mgr Name",
                                    "Mgr Apr/Rej Date",
                                    "DIR Decision",
                                    "DIR Name",
                                    "DIR Apr/Rej Date",
                                    "DIR Comment"];

                    var config = {
                        filename: "eMET",
                        sheetName: "Sheet1",
                    };

                    var data = [
                        {
                            "styles": { title: 17, messageTop: 0, messageBottom: 0, header: 22, footer: 0 },
                            "title": ["Report Name: e-MET", "Report Generated: " + today + " "],
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
    </script>

    <%--script alert and extend session--%>
    <script type="text/javascript">
        //try {
        //    $(function () {
        //        var timeout = 570000;
        //        $(document).bind("idle.idleTimer", function () {
        //            // function you want to fire when the user goes idle
        //            var x = document.getElementById("loading");
        //            if (window.getComputedStyle(x).display === "none") {
        //                //console.log('hide');
        //            }
        //            else {

        //                OpenModalSession();
        //                $("#StartTimer").click();
        //            }
        //            //$.timeoutDialog({ timeout: 0.25, countdown: 15, logout_redirect_url: 'Login.aspx', restart_on_yes: true });
        //        });
        //        $(document).bind("active.idleTimer", function () {
        //            // function you want to fire when the user becomes active again
        //        });
        //        $.idleTimer(timeout);
        //    });
        //}
        //catch (err) {
        //    alert(err + ' : alert and extend session');
        //}
    </script>

    <%--script alert unred announcement --%>
    <script type="text/javascript">
        function playAudio() {
            try {
                jQuery.noConflict();
                if (document.getElementById('LiUnReadAnn').style.display == "block") {
                    var x = document.getElementById("myAlertAudio");
                    //x.loop = true;
                    x.play();
                }
            } catch (e) {

            }
        }
    </script>
</head>
<body id="page-top">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server" AsyncPostBackTimeout="360000"></asp:ScriptManager>
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
                                <a onclick="ShowLoading();" href="Vendor.aspx">
                                    <asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                                <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" ID="sidebarToggle" ><i class="fas fa-bars"></i> </asp:LinkButton>
                                <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                                <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-sm-2 fa-pull-right" style="background-color: #E9ECEF;">
                                <asp:Label ID="lblUser" runat="server" Width="147px"></asp:Label><br />
                                <asp:Label ID="lblplant" runat="server" Text=""></asp:Label>
                                <asp:LinkButton runat="server" ID="BtnLogOut" Text="Logout" OnClientClick="LogOut()"></asp:LinkButton>
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
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author_V.aspx?num=15">
                            <i class="fas fa-fw fa-tachometer-alt"></i>
                            <span>Home</span>
                        </a>
                    </li>
                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author_V.aspx?num=16">
                            <i class="fas fa-fw fa-table"></i>
                            <span>Master Data</span>
                        </a>
                    </li>
                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="ChangePwd.aspx?num=15">
                            <i class="fas fa-fw fa-key"></i>
                            <span>Change Password</span>
                        </a>
                    </li>
                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="VndReport.aspx">
                            <i class="fas fa-fw fa-book-open"></i>
                            <span>Report</span>
                        </a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="RealTimeVendInv.aspx">
                            <i class="fas fa-fw fa-book-open"></i>
                            <span>Real Time Inventory</span>
                        </a>
                    </li>

                    <li class="sideMenu" runat="server" id="LiUnReadAnn" style="display:block">
                        <a onclick="ShowLoading();" href="Emet_author_V.aspx?num=14" class="MyLink">
                            <asp:Label runat="server" ID="lbUnreadAnn" CssClass="blink" Text="You Have Unread Announcements" ForeColor="Red"
                                 Font-Bold="true" Font-Size="20px"></asp:Label>
                        </a>
                        <audio id="myAlertAudio" controls="controls" style="display:none" allow="autoplay">
                          <source src="Styles/alert1.wav" type="audio/wav"/>
                          Your browser does not support the audio element.
                        </audio>
                    </li>
                </ul>
            </div>

            <div id="content-wrapper">
                <div class="container-fluid">
                    <div class="card">
                        <div class="card-header">
                            <div class="card-header-content ">
                                <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION STATUS - <b>Report</b>
                            </div>
                        </div>
                        <div class="card-body">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-2" style="padding-bottom: 5px;">
                                            <asp:Label runat="server" ID="LbFilter" Text="Filter By :"></asp:Label>
                                        </div>
                                        <div class="col-sm-10 text-right" style="padding-bottom: 5px;">
                                            <div style="display: none">
                                                <asp:TextBox runat="server" ID="LastId" Text="0"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="TxtExtraFilter" Text=""></asp:TextBox>
                                            </div>
                                            <a class="btn btn-sm btn-primary btn-sm" onclick="Generate();"><i class="fa fa-plus-square"></i>Add More Filter </a>
                                            <a class="btn btn-sm btn-danger btn-sm" onclick="RemoveAllDiv();"><i class="fa fa-trash-alt"></i>Delete All Extra Filter </a>
                                            <asp:Button runat="server" ID="BtnReset" Text="Reset" CssClass="btn btn-sm btn-warning" OnClientClick="BtnReset_Click();return false;" AutoPostBack="false"></asp:Button>
                                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger" OnClientClick="Close_Click()" AutoPostBack="false" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-lg-4" style="padding-bottom: 5px;">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label runat="server" ID="Label5" Text="Req. Type"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:DropDownList runat="server" ID="DdlReqType" AutoPostBack="false">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="New" Value="WithSAPCode"></asp:ListItem>
                                                        <asp:ListItem Text="Revision" Value="WithSAPCodeRevision"></asp:ListItem>
                                                        <asp:ListItem Text="Without SAP Code" Value="WithoutSAPCode"></asp:ListItem>
                                                        <asp:ListItem Text="Without SAP Code (GP)" Value="WithoutSAPCodeGP"></asp:ListItem>
                                                        <asp:ListItem Text="Mass Revision" Value="WithSAPCodeMassRevision"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4" style="padding-bottom: 5px;">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label runat="server" ID="Label3" Text="Vnd.Res. Status"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:DropDownList runat="server" ID="DdlStatus" AutoPostBack="false">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="Vendor Pending" Value="Pending"></asp:ListItem>
                                                        <asp:ListItem Text="Vendor Completed" Value="Completed"></asp:ListItem>
                                                        <asp:ListItem Text="Auto Completed By SMN" Value="Auto"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4" style="padding-bottom: 5px;">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label runat="server" ID="Label4" Text="SMN.Res. Status"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:DropDownList runat="server" ID="DdlSMNStatus" AutoPostBack="false">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="Waiting Vend. Submission" Value="Waiting"></asp:ListItem>
                                                        <asp:ListItem Text="Mgr. Pending" Value="MPending"></asp:ListItem>
                                                        <asp:ListItem Text="Mgr Request Resubmit" Value="MResubmit"></asp:ListItem>
                                                        <asp:ListItem Text="Mgr. Approved" Value="MApproved"></asp:ListItem>
                                                        <asp:ListItem Text="Mgr. Rejected" Value="MRejected"></asp:ListItem>
                                                        <asp:ListItem Text="Dir. Approved" Value="DApproved"></asp:ListItem>
                                                        <asp:ListItem Text="Dir. Rejected" Value="DRejected"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-lg-4" style="padding-bottom: 5px;">
                                            <asp:DropDownList runat="server" ID="DdlFltrDate" AutoPostBack="false">
                                                <asp:ListItem Text="Request Date" Value="RequestDate"></asp:ListItem>
                                                <asp:ListItem Text="Quote Response Due Date" Value="QuoteResponseDueDate"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-lg-4" style="padding-bottom: 5px;">
                                            <div class="group-main">
                                                <div class="SearchBox-txt">
                                                    <asp:TextBox ID="TxtFrom" OnclientClick="return false;" runat="server" placeholder="Date From" AutoPostBack="false"
                                                        ToolTip="Date From" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                    </asp:TextBox>
                                                </div>
                                                <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 2px 3px 1px 3px;">
                                                    <a class="fa fa-calendar" style="color: #005496; padding: 2px 3px 1px 3px;" onclick="javascript: $('#TxtFrom').focus();"></a>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-lg-4" style="padding-bottom: 5px;">
                                            <div class="group-main">
                                                <div class="SearchBox-txt">
                                                    <asp:TextBox ID="TxtTo" OnclientClick="return false;" runat="server" placeholder="Date To" AutoPostBack="false"
                                                        ToolTip="Date To" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                    </asp:TextBox>
                                                </div>
                                                <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 2px 3px 1px 3px;">
                                                    <a class="fa fa-calendar" style="color: #005496; padding: 2px 3px 1px 3px;" onclick="javascript: $('#TxtTo').focus();"></a>
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-lg-4" style="padding-bottom: 5px;">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label runat="server" ID="Label6" Text="Req. Status"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:DropDownList runat="server" ID="DdlReqStatus" AutoPostBack="false">
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                                                        <asp:ListItem Text="In Progress" Value="InProgress"></asp:ListItem>
                                                        <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4" style="padding-bottom: 5px;">
                                            <asp:DropDownList runat="server" ID="DdlFilterBy" AutoPostBack="false">
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

                                    <div class="row" id="DvDynamic">
                                    </div>

                                    <div class="row" style="padding-bottom: 5px;">
                                        <div class="col-md-4">
                                        </div>
                                        <div class="col-md-4">
                                        </div>
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="btnSearch" CssClass="btn btn-sm btn-info btn-block" runat="server"
                                                autopostback="false" OnClientClick="GetAllExtraFilterData();ShowLoading();GetReportData();return false;"><i class="fa fa-search" aria-hidden="true"
                                                    style="color:#F5F5F5;" ></i> VIEW</asp:LinkButton>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>



                            <div class="row">
                                <div class="col-md-12 table table-responsive">
                                    <table id="TableReport" class="table table-responsive table-striped table-bordered nopadding" style="width: 100%;">
                                        <thead>
                                            <tr style="color: white;">
                                                <th style="background-color: #337AB7!important;">No.</th>
                                                <th style="background-color: #337AB7!important">Request Number</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">Request Date</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">Response Due Date</th>
                                                <th style="background-color: #337AB7!important">Quote No</th>
                                                <th style="background-color: #337AB7!important">Plant</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">GP Request Plant</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">Req Type</th>
                                                <th style="background-color: #337AB7!important">Req Status</th>
                                                <th style="background-color: #337AB7!important">Vnd.Res. Status</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">SMN.Res.Status</th>
                                                <th style="background-color: #337AB7!important">SMN PIC Dept</th>
                                                <th style="background-color: #337AB7!important">SMN PIC</th>
                                                <th style="background-color: #337AB7!important">SMN PIC Email</th>
                                                <th style="background-color: #337AB7!important">Product</th>
                                                <th style="background-color: #337AB7!important">Material Type</th>
                                                <th style="background-color: #337AB7!important">Material Class</th>
                                                <th style="background-color: #337AB7!important">Material</th>
                                                <th style="background-color: #337AB7!important">Material Desc</th>
                                                <th style="background-color: #337AB7!important">Base UOM</th>
                                                <th style="background-color: #337AB7!important">Plant Status</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">Drawing No</th>
                                                <th style="background-color: #337AB7!important">SAP Proc Type</th>
                                                <th style="background-color: #337AB7!important">SAP Sp Proc Type</th>
                                                <th style="background-color: #337AB7!important">PIR Type</th>
                                                <th style="background-color: #337AB7!important">PIR Job Type</th>
                                                <th style="background-color: #337AB7!important">Plating Type</th>
                                                <th style="background-color: #337AB7!important">Process Group</th>
                                                <th style="background-color: #337AB7!important">Req Recycle Ratio (%)</th>
                                                <th style="background-color: #337AB7!important">Request Purpose</th>
                                                <th style="background-color: #337AB7!important">Mnth.Est.Qty</th>
                                                <th style="background-color: #337AB7!important">Mnth.Est.Qty UOM</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">FA Date</th>
                                                <th style="background-color: #337AB7!important">FA Qty</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">1st Delivery Date</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">1st Delivery Qty</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">Effective Date</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">Due Dt Next Rev</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">New Effective Date</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">New Due Dt Next Rev</th>
                                                <th style="background-color: #337AB7!important">Previous Quote No.</th>
                                                <th style="background-color: #337AB7!important">Incoterm</th>
                                                <th style="background-color: #337AB7!important">Packing Req</th>
                                                <th style="background-color: #337AB7!important">Others Req</th>
                                                <th style="background-color: #337AB7!important">Vendor Code</th>
                                                <th style="background-color: #337AB7!important">Vendor Name</th>
                                                <th style="background-color: #337AB7!important">Vendor Country Code</th>
                                                <th style="background-color: #337AB7!important">Vendor Currency</th>
                                                <th style="background-color: #337AB7!important">Vendor PIC</th>
                                                <th style="background-color: #337AB7!important">Vendor PIC Email</th>
                                                <th style="background-color: #337AB7!important">Country Org</th>
                                                <th style="background-color: #3ea707!important">Raw Material SAP Code</th>
                                                <th style="background-color: #3ea707!important">Raw Material Desc</th>
                                                <th style="background-color: #3ea707!important">Raw Material Cost</th>
                                                <th style="background-color: #3ea707!important">Raw Material Cost UOM</th>
                                                <th style="background-color: #3ea707!important">Part Net Weight</th>
                                                <th style="background-color: #3ea707!important">Part Net Weight UOM</th>
                                                <th style="background-color: #3ea707!important">Part Unit Weight (g)</th>
                                                <th style="background-color: #3ea707!important">Thickness (mm)</th>
                                                <th style="background-color: #3ea707!important">Width (mm)</th>
                                                <th style="background-color: #3ea707!important">Pitch (mm)</th>
                                                <th style="background-color: #3ea707!important">Material Density</th>
                                                <th style="background-color: #3ea707!important">Runner Weight/shot (g)</th>
                                                <th style="background-color: #3ea707!important">Runner Ratio/pcs (%)</th>
                                                <th style="background-color: #3ea707!important">Recycle Material Ratio (%)</th>
                                                <th style="background-color: #3ea707!important">Cavity</th>
                                                <th style="background-color: #3ea707!important">Material/Melting Loss (%)</th>
                                                <th style="background-color: #3ea707!important">Material Gross Weight/pc (g)</th>
                                                <th style="background-color: #3ea707!important">Material Scrap Weight (g)</th>
                                                <th style="background-color: #3ea707!important">Scrap Loss Allowance (%)</th>
                                                <th style="background-color: #3ea707!important">Scrap Price/kg</th>
                                                <th style="background-color: #3ea707!important">Scrap Rebate / pcs</th>
                                                <th style="background-color: #3ea707!important">Material Cost/pcs</th>
                                                <th style="background-color: #3ea707!important">Total Material Cost/pcs</th>

                                                <th style="background-color: #ff6a00!important">Process Grp Code</th>
                                                <th style="background-color: #ff6a00!important">Sub Process</th>
                                                <th style="background-color: #ff6a00!important">If Subcon - Subcon Name</th>
                                                <th style="background-color: #ff6a00!important">If Turnkey- Sub vendor name</th>
                                                <th style="background-color: #ff6a00!important">Machine / Labor</th>
                                                <th style="background-color: #ff6a00!important">Machine</th>
                                                <th style="background-color: #ff6a00!important">Standard Rate/HR</th>
                                                <th style="background-color: #ff6a00!important">Vendor Rate / HR</th>
                                                <th style="background-color: #ff6a00!important">Process UOM</th>
                                                <th style="background-color: #ff6a00!important">Base Qty</th>
                                                <th style="background-color: #ff6a00!important">Duration per Process UOM (Sec)</th>
                                                <th style="background-color: #ff6a00!important">Efficiency</th>
                                                <th style="background-color: #ff6a00!important">Turnkey Cost/pc</th>
                                                <th style="background-color: #ff6a00!important">Turnkey Fees</th>
                                                <th style="background-color: #ff6a00!important">Process Cost/pc</th>
                                                <th style="background-color: #ff6a00!important">Total Processes Cost/pcs</th>

                                                <th style="background-color: #d59208!important">Sub-Mat / T&J Description</th>
                                                <th style="background-color: #d59208!important">Sub-Mat / T&J Cost</th>
                                                <th style="background-color: #d59208!important">Consumption (pcs)</th>
                                                <th style="background-color: #d59208!important">Sub-Mat / T&J Cost/pcs</th>
                                                <th style="background-color: #d59208!important">Total Sub-Mat / T&J Cost/pcs</th>

                                                <th style="background-color: #c10ac3!important">Items Description</th>
                                                <th style="background-color: #c10ac3!important">Other Item Cost/pcs</th>
                                                <th style="background-color: #c10ac3!important">Total Other Item Cost/pcs</th>

                                                <th style="background-color: #337AB7!important">Total Material Cost / pc</th>
                                                <th style="background-color: #337AB7!important">Total Process Cost / pc</th>
                                                <th style="background-color: #337AB7!important">Total Sub Material Cost / pc</th>
                                                <th style="background-color: #337AB7!important">Total Other items Cost / pc</th>
                                                <th style="background-color: #337AB7!important">Grand Total Cost / pc</th>
                                                <th style="background-color: #337AB7!important">Final Quote Price / pc</th>
                                                <th style="background-color: #337AB7!important">Net Profit/Discount (%)</th>
                                                <th style="background-color: #337AB7!important">GA (%)</th>
                                                <th style="background-color: #337AB7!important">profit (%)</th>
                                                <th style="background-color: #337AB7!important">discount (%)</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">Comment By Vendor</th>
                                                <th style="background-color: #337AB7!important">Mgr Decision</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">Mgr Comment</th>
                                                <th style="background-color: #337AB7!important">Mgr Name</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">Mgr Apr/Rej Date</th>
                                                <th style="background-color: #337AB7!important">DIR Decision</th>
                                                <th style="background-color: #337AB7!important">DIR Name</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">DIR Apr/Rej Date</th>
                                                <th style="background-color: #337AB7!important" class="no-sort">DIR Comment</th>
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
                                                <asp:Timer ID="TimerCntDown" runat="server" Interval="1000"  Enabled="false"></asp:Timer>
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
                                    <asp:Button ID="StartTimer" runat="server" Text="Start" autopostback="false" CssClass="btn btn-sm btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="DvHdnField" runat="server" style="display: none">
            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                <ContentTemplate>
                    <asp:TextBox runat="server" ID="TxtmainUrl" value=""></asp:TextBox>
                    <asp:TextBox runat="server" ID="TxtPlant" value=""></asp:TextBox>
                    <asp:TextBox runat="server" ID="TxtVndCode" value=""></asp:TextBox>
                    <asp:TextBox runat="server" ID="TxtVendorType" value=""></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>

</body>
</html>
