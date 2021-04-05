<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MassRevisionAll.aspx.cs" Inherits="Material_Evaluation.MassRevisionAll" %>

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

        #TbBasicData_wrapper, #TbQuoteRefListInvalid_wrapper, #TbDuplicateWithExpiredReq_wrapper, #TbValidData_wrapper, #TbInValidData_wrapper, #TbValidDataReqTemp_wrapper, #TbInValidDataReqTemp_wrapper {
            overflow-x: hidden;
        }

        #TbBasicData_filter, #TbDuplicateWithExpiredReq_filter, #TbValidData_filter, #TbInValidData_filter, #TbValidDataReqTemp_filter, #TbInValidDataReqTemp_filter {
            float: left !important;
        }

        #TbBasicData, #TbDuplicateWithExpiredReq, #TbValidData, #TbInValidData, #TbValidDataReqTemp, #TbInValidDataReqTemp {
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
        var Popup, dataTableBasicData, dataTableDuplicateWithExpiredReq, dataTableTbQuoteRefListInvalid, dataTableTbValidData, dataTableTbInValidData, dataTableTbValidDataReqTemp, dataTableTbInValidDataReqTemp;
        var currentPageBasicData = 0, currentPageDuplicateWithExpiredReq = 0, currentPageInvalidReq = 0, currentPageValidData = 0, currentPageInValidData = 0, currentPageValidDataReqTemp = 0, currentPageInValidDataReqTemp = 0;
        var DataDetExpiredReq = [];
        var DataValidComp = [];
        var mainUrl = "";

        $(document).ready(function () {
            mainUrl = window.location.href.replace("MassRevisionAll.aspx", "");
            SetUpSideBar();
            DatePitcker();
            GetDdlReason();
            SetDueOnDate();
            GeneratedataTableBasicData();
            GenerateTbDuplicateWithExpiredReq();
            GenerateTbQuoteRefListInvalid();
            GenerateTbTbValidData();
            GenerateTbInValidData();
            GenerateTbValidDataReqTemp();
            GenerateTbInValidDataReqTemp();
            $('a#BtnTemplate').attr({ target: '_blank', href: mainUrl + '/files/LBS PIR for Mass Upload.xlsx' });
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

                document.getElementById("ChcMatCost").disabled = false;
                document.getElementById("ChcProcCost").disabled = false;
                document.getElementById("ChcSubMat").disabled = false;
                document.getElementById("ChcOthMat").disabled = false;

                document.getElementById("TxtValidDate").disabled = false;
                document.getElementById("TxtResDueDate").disabled = false;
                document.getElementById("DdlReason").disabled = false;
                document.getElementById("txtRem").disabled = false;
                
                document.getElementById("TxtValidDate").value = "";
                document.getElementById("TxtResDueDate").value = "";
                document.getElementById("DdlReason").selectedIndex = 0;
                document.getElementById("txtRem").value = "";
                document.getElementById("DvRemark").style.display = "none";

                document.getElementById("BtnProceed").disabled = false;
                document.getElementById("BtnCancelProceed").disabled = true;
                
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

        function DdlReasonchange(DdlReasonID, TxtOthReasonID, LblengtVCID) {
            try {

                var DdlReason = document.getElementById(DdlReasonID);
                var ReasonSelct = DdlReason.options[DdlReason.selectedIndex].value;
                var DdlReasonidx = $("#" + DdlReasonID)[0].selectedIndex;
                if (DdlReasonidx <= 0) {
                    document.getElementById(TxtOthReasonID).style.display = "none";
                    document.getElementById(LblengtVCID).style.display = "none";
                    document.getElementById(DdlReasonID).style.border = "1px solid #ff0000";
                }
                else if (ReasonSelct.toString() == "Others") {
                    document.getElementById(TxtOthReasonID).style.display = "block";
                    document.getElementById(LblengtVCID).style.display = "block";
                    document.getElementById(DdlReasonID).style.border = "1px solid #CCCCCC";

                    var TxtRem = $("#txtRem").val();
                    if (TxtRem == "") {
                        document.getElementById("txtRem").style.border = "1px solid #ff0000";
                    }
                    else {
                        document.getElementById("txtRem").style.border = "1px solid #CCCCCC";
                    }
                }
                else {
                    document.getElementById(TxtOthReasonID).style.display = "none";
                    document.getElementById(LblengtVCID).style.display = "none";
                    document.getElementById(DdlReasonID).style.border = "1px solid #CCCCCC";
                }
                //HideBtnSubmit();
            }
            catch (err) {
                alert(err + ": DdlReasonchange")
            }
        }

        function RemarkLght(TxtOthReasonID, LblengtVCID) {
            try {
                var MaxLength = 200;
                var a = document.getElementById(TxtOthReasonID).value;

                $("#" + TxtOthReasonID).keypress(function (e) {
                    if ($(this).val().length >= MaxLength) {
                        e.preventDefault();
                    }
                });

                a = document.getElementById(TxtOthReasonID).value;
                document.getElementById(LblengtVCID).innerHTML = ' ' + (200 - a.length) + ' character left'

                if (a.length > 200) {
                    a = a.slice(0, 200);
                    document.getElementById(LblengtVCID).innerHTML = ' ' + (200 - a.length) + ' character left';
                    $("#" + TxtOthReasonID).val(a);
                }
            }
            catch (err) {
                alert(err + ": RemarkLght(TxtOthReasonID, LblengtVCID)");
            }
        }
    </script>


    <%--get MDM Data--%>
    <script type="text/javascript">
        function GetDdlReason() {
            if (typeof (Worker) !== "undefined") {
                var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/GetDdlReason";
                $.ajax({
                    url: url,
                    cache: false,
                    type: "POST",
                    data: { ReasonType: "Creation" },
                    dataType: 'json',
                    async: false,
                    beforeSend: function () {
                        ShowLoading();
                    },
                    complete: function () {
                        CloseLoading();
                    },
                    success: function (data) {
                        if (data.success == true) {
                            if (data.MyData.length > 0) {
                                markup = "<option value='0' class='optionPlaceHolder'>-- Select --</option>";
                                for (var x = 0; x < data.MyData.length; x++) {
                                    markup += "<option value='" + data.MyData[x].Value + "'>" + data.MyData[x].Text + "</option>";
                                }
                            }
                            else {
                                markup += "<option value='0' class='optionPlaceHolder' >No Data Found</option>";
                            }
                        }
                        else {
                            markup += "<option value='0' class='optionPlaceHolder'>No Data Found</option>";
                        }
                        markup += "<option value='Others'>Others</option>";
                        $("#DdlReason").html(markup).show();
                    },
                    error: function (xhr, status, error) {
                        alert("GetDdlReason : " + error);
                    }
                },
                $('#DdlReason :nth-child(0)').prop('selected', true));
            }
            else {
                alert('GetDdlReason : Browser Not Support')
            }
        }

        function SetDueOnDate() {
            if (typeof (Worker) !== "undefined") {
                var url = mainUrl + "/EmetServices/MassRevision/MyJSONMassRevisionALL.asmx/SetDueOnDate";
                $.ajax({
                    url: url,
                    cache: false,
                    type: "POST",
                    data: { Plant: document.getElementById("TxtPlant").value },
                    dataType: 'json',
                    async: false,
                    beforeSend: function () {
                        ShowLoading();
                    },
                    complete: function () {
                        CloseLoading();
                    },
                    success: function (data) {
                        if (data.success == true) {
                            if (data.Enabled == true) {
                                document.getElementById("TxtDuenextRev").disabled = false;
                            }
                            else {
                                document.getElementById("TxtDuenextRev").disabled = true;
                            }
                            document.getElementById("TxtDuenextRev").value = data.message;
                        }
                        else {
                            alert("SetDueOnDate : " + data.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert("SetDueOnDate : " + error);
                    }
                });
            }
            else {
                alert('SetDueOnDate : Browser Not Support')
            }
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

        function OpenModalDuplicateExpired() {
            try {
                jQuery.noConflict();
                $("#MdDuplicateReq").modal({
                    backdrop: 'static',
                    keyboard: false
                });

                setTimeout(function () {
                    var table = $('#TbDuplicateWithExpiredReq').DataTable();
                    table.columns.adjust();
                }, 100);
            }
            catch (err) {
                alert(err + ' : OpenModalDuplicateExpired');
            }
        }

        function CloseModalDuplicateExpired() {
            try {
                jQuery.noConflict();
                $("#MdDuplicateReq").modal('hide');
            }
            catch (err) {
                alert(err + ' : OpenModalDuplicateExpired');
            }
        }
    </script>

    <%--valBeforeUpload()--%>
    <script type="text/javascript">
        function valBeforeUpload() {
            var err = "";
            err += "Please check field listed in below : \n";
            var iserr = false;
            var fu = document.getElementById("FlUpload");
            var val = $("#FlUpload").val().toLowerCase();
            var regex = new RegExp("(.*?)\.(xlsx)$");
            if (fu.value.length <= 0) {
                $("#FlUpload").css("border", "1px solid #ff0000");
                err = "Please select the file !";
                iserr = true;
            }
            else if (!(regex.test(val))) {
                $("#FlUpload").css("border", "1px solid #ff0000");
                err = "Invalid File. Please upload a File with (.xlsx)extension";
                $("#FlUpload").val("");
                iserr = true;
            }
            else {
                $("#FlUpload").css("border", "1px solid #F7F7F7");
            }

            var RevAll = document.getElementById("RbisMassRevisionAll").checked;
            var Rev = document.getElementById("RbisMassRevision").checked;
            if (RevAll == false && Rev == false) {
                iserr = true;
                err = "Please Select Mass revision Type";
                $("#FlUpload").css("border", "1px solid #F7F7F7");
            }

            if (iserr == true) {
                alert(err);
                return false;
            }
        }
    </script>

    <%--SetUpSessionFileUpload()--%>
    <script type="text/javascript">
        function SetUpSessionFileUpload() {
            var Sccs = false;
            var submitMsg = "";
            setTimeout(function () {
                try {
                    if (typeof (Worker) !== "undefined") {
                        var formdata = new FormData();
                        var MyFileName = "";
                        var MyFile;
                        var FU = $("#FlUpload");
                        if (FU != null) {
                            //var files = FU.files[0];
                            var files = FU.prop('files');
                            if (files != null) {
                                if (files[0] != null) {
                                    MyFile = files[0];
                                    MyFileName = files[0].name;
                                }
                            }
                        }
                        if (MyFile != null) {
                            formdata.append('MassFlAtc_' + document.getElementById("TxtuseID").value, MyFile);
                        }

                        $.ajax({
                            type: "POST",
                            url: mainUrl + "/EmetServices/MassRevision/HandlerFileUplad.ashx",
                            data: formdata,
                            cache: false,
                            async: false,
                            processData: false,
                            contentType: false,
                            beforeSend: function () {
                                ShowLoading();
                            },
                            complete: function () {
                                ColectDataFileUpload();
                            },
                            success: function (result) {
                                var Resutl = JSON.stringify(result);
                                if (Resutl.toString() == "\"OK\"") {
                                    Sccs = true;
                                }
                                else {
                                    submitMsg = JSON.stringify(result);
                                }
                            },
                            error: function (err) {
                                submitMsg = err.statusText;
                            }
                        });
                    }
                    else {
                        Sccs = false;
                        submitMsg = "SetUpSessionFileUpload : Browser Not Support";
                    }
                } catch (e) {
                    Sccs = false;
                    CloseLoading();
                    submitMsg = "SetUpSessionFileUpload : " + e;
                }

                if (Sccs == true) {
                    CloseLoading();
                    return true;
                }
                else {
                    alert(submitMsg);
                    CloseLoading();
                    return false;
                }
            }, 100);
        }
    </script>

    <%--ColectDataFileUpload--%>
    <script type="text/javascript">
        function ColectDataFileUpload() {
            try {
                jQuery.noConflict();
                if (typeof (Worker) !== "undefined") {
                    setTimeout(function () {
                        var url = mainUrl + "/EmetServices/MassRevision/MyJSONMassRevisionALL.asmx/ColectDataFileUpload";
                        var _UseID = document.getElementById('TxtuseID').value;

                        $.ajax({
                            url: url,
                            cache: false,
                            type: "POST",
                            dataType: 'json',
                            //contentType: "application/json; charset=utf-8",
                            data: {
                                UseID: _UseID
                            },
                            async: false,
                            beforeSend: function () {
                                ShowLoading();
                            },
                            complete: function () {
                                CloseLoading();
                            },
                            success: function (data) {
                                if (data.success == true) {
                                    var length = $("#lcDatatablesBasicData").val();
                                    if (length == "" || length == "0") {
                                        length = "1";
                                        $("#lcDatatablesBasicData").val("1");
                                    }

                                    dataTableBasicData.clear().draw();
                                    dataTableBasicData.rows.add(data.MyBasicData).draw();
                                    dataTableBasicData.page.len(length).draw();
                                }
                                else {
                                    alert("ColectDataFileUpload : " + data.message);
                                }
                            },
                            error: function (xhr, status, error) {
                                alert("ColectDataFileUpload : " + error);
                            }
                        });
                    }, 100);
                }
                else {
                    alert('ColectDataFileUpload : Browser Not Support');
                    CloseLoading();
                }
            } catch (e) {
                alert("ColectDataFileUpload : " + e);
                CloseLoading();
            }
        }

    </script>

    <%--@*GeneratedataTableBasicData()*@--%>
    <script type="text/javascript">
        function GeneratedataTableBasicData() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();

                    dataTableBasicData = $("#TbBasicData").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').prop('type', 'text');
                            $(".paginate_button").click(function () {
                                currentPageBasicData = dataTableBasicData.page.info().page;
                            });
                        },
                        "deferRender": true,
                        "columns": [
                        {
                            "data": null,
                            "sortable": true,
                            render: function (data, type, row, meta) {
                                return meta.row + 1;
                            }
                        },
                        { "data": "Plant", "autoWidth": true },
                        { "data": "PIRNo", "autoWidth": true },
                        { "data": "MaterialCode", "autoWidth": true },
                        { "data": "MaterialDesc", "autoWidth": true },
                        { "data": "VendorCode", "autoWidth": true },
                        { "data": "VendorName", "autoWidth": true },
                        { "data": "ProcessGroup", "autoWidth": true }
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
                                text: '<span class="glyphicon glyphicon-export"></span> Export',
                                className: "btn btn-sm btn-success"
                            }
                        ],
                        language: {
                            'emptytable': 'No data found',
                            'search': '',
                            'searchPlaceholder': 'Filter All Columns',
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatablesBasicData' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.RequestNumber;
                        }
                    });

                    $("#lcDatatablesBasicData").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesBasicData").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            var info = dataTableBasicData.page.info();
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
                            dataTableBasicData.page.len(1).draw();
                        } else {
                            dataTableBasicData.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesBasicData").change(function (e) {

                        var info = dataTableBasicData.page.info();
                        var recordsTotal = info.recordsTotal;

                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                        else if ($(this).val() > recordsTotal) {
                            $(this).val(recordsTotal);
                        }
                    });

                    $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                } catch (e) {
                    alert("GeneratedataTableBasicData() : " + e);
                }
            }
            else {
                alert('GeneratedataTableBasicData : Browser Not Support')
            }
        }
    </script>

    <%--GenerateTbDuplicateWithExpiredReq--%>
    <script type="text/javascript">
        function GenerateTbDuplicateWithExpiredReq() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableDuplicateWithExpiredReq = $("#TbDuplicateWithExpiredReq").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').prop('type', 'text');
                            $(".paginate_button").click(function () {
                                currentPage = dataTableDuplicateWithExpiredReq.page.info().page;
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
                            {
                                "data": "RequestDate",
                                "render": function (value) {
                                    if (value === null) return "";
                                    return moment(value).format('DD-MM-YYYY');
                                },
                                "autoWidth": true
                            },
                            { "data": "RequestNumber", "autoWidth": true },
                            {
                                "data": "QuoteResponseDueDate",
                                "render": function (value) {
                                    if (value === null) return "";
                                    return moment(value).format('DD-MM-YYYY');
                                },
                                "autoWidth": true
                            },
                            { "data": "Material", "autoWidth": true },
                            { "data": "MaterialDesc", "autoWidth": true },
                            {
                                "data": null,
                                "render": function (data, type, row, meta) {
                                    if (type === 'display') {
                                        return '<input type="radio" id="RbRej_' + meta.row + '" name="RejOrChgDate_' + row.RequestNumber + '" onclick="RbRejectExpReq(\'' + meta.row + '\');IsAllRadioChecked(\'REJ\');" />';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": null,
                                "render": function (data, type, row, meta) {
                                    if (type === 'display') {
                                        return '<input type="radio" id="RbchangeDate_' + meta.row + '" name="RejOrChgDate_' + row.RequestNumber + '" onclick="RbChangedateResDueDate(\'' + meta.row + '\');IsAllRadioChecked(\'CHGDATE\');" />';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": "QuoteResponseDueDate",
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        return '<input type="text" id="TxtNewResDueDate_' + meta.row + '" value="' + moment(data).format('DD-MM-YYYY') + '" disabled="disabled" />';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                        ],
                        "ordering": false,
                        "paging": false,
                        columnDefs: [{
                            orderable: false,
                            targets: "no-sort"
                        }],
                        "dom": "<'row'<'col-lg-12 col-sm-12 col-md-12 col-12'>>" +
                               "<'row'<'col-sm-12'tr>>" +
                               "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                        buttons: [
                        ],
                        language: {
                            'emptytable': 'No data found',
                            'search': '',
                            'searchPlaceholder': 'Filter All Columns',
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatablesExpiredReq' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
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

                    $("#lcDatatablesExpiredReq").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            var info = dataTableDuplicateWithExpiredReq.page.info();
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
                            dataTableDuplicateWithExpiredReq.page.len(1).draw();
                        } else {
                            dataTableDuplicateWithExpiredReq.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesExpiredReq").change(function (e) {

                        var info = dataTableDuplicateWithExpiredReq.page.info();
                        var recordsTotal = info.recordsTotal;

                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                        else if ($(this).val() > recordsTotal) {
                            $(this).val(recordsTotal);
                        }
                    });

                    $('#TbDuplicateWithExpiredReq tbody').on('click', 'td.details-control', function () {
                        var tr = $(this).closest('tr');
                        var row = dataTableDuplicateWithExpiredReq.row(tr);
                        var data = dataTableDuplicateWithExpiredReq.row(tr).data();

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
                    alert("GenerateTbDuplicateWithExpiredReq() : " + e);
                }
            }
            else {
                alert('GenerateTbDuplicateWithExpiredReq : Browser Not Support')
            }
        }
    </script>

    <%--Gen table For Vendor And Material in Progress--%>
    <script type="text/javascript">
        function GenerateTbQuoteRefListInvalid() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableTbQuoteRefListInvalid = $("#TbQuoteRefListInvalid").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $(".paginate_button").click(function () {
                                currentPageInvalidReq = dataTableTbQuoteRefListInvalid.page.info().page;
                            });
                        },
                        "deferRender": false,
                        "columns": [
                        {
                            "data": "RequestDate",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        { "data": "RequestNumber", "autoWidth": true },
                        {
                            "data": "QuoteResponseDueDate",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        { "data": "QuoteNo", "autoWidth": true },
                        { "data": "Material", "autoWidth": true },
                        { "data": "MaterialDesc", "autoWidth": true },
                        { "data": "VendorCode1", "autoWidth": true },
                        { "data": "VendorName", "autoWidth": true }
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
                                text: '<span class="glyphicon glyphicon-export"></span> Export',
                                className: "btn btn-sm btn-success"
                            }
                        ],
                        language: {
                            'emptytable': 'No data found',
                            'search': '',
                            'searchPlaceholder': 'Filter All Columns',
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatablesListInvalid' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.QuoteNo;
                        }
                    });

                    $("#lcDatatablesListInvalid").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesListInvalid").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTableTbQuoteRefListInvalid.page.len(1).draw();
                        } else {
                            dataTableTbQuoteRefListInvalid.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesListInvalid").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });

                } catch (e) {
                    alert("GenerateTbQuoteRefListInvalid() : " + e);
                }
            }
            else {
                alert('GenerateTbQuoteRefListInvalid : Browser Not Support')
            }
        }
    </script>

    <%--GenerateTb Invalid Data Due On Data Not Maintain In MDM--%>
    <script type="text/javascript">
        function GenerateTbInValidData() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableTbInValidData = $("#TbInValidData").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $(".paginate_button").click(function () {
                                currentPageInValidData = dataTableTbInValidData.page.info().page;
                            });
                        },
                        "deferRender": false,
                        "columns": [
                        {
                            "data": null,
                            "sortable": true,
                            render: function (data, type, row, meta) {
                                return meta.row + 1;
                            }
                        },
                        { "data": "Plant", "autoWidth": true },
                        { "data": "PIRNo", "autoWidth": true },
                        { "data": "MaterialCode", "autoWidth": true },
                        { "data": "MaterialDesc", "autoWidth": true },
                        { "data": "VendorCode", "autoWidth": true },
                        { "data": "VendorName", "autoWidth": true },
                        { "data": "ProcessGroup", "autoWidth": true },
                        { "data": "Remark", "autoWidth": true }
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
                                text: '<span class="glyphicon glyphicon-export"></span> Export',
                                className: "btn btn-sm btn-success"
                            }
                        ],
                        language: {
                            'emptytable': 'No data found',
                            'search': '',
                            'searchPlaceholder': 'Filter All Columns',
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatablesListInValidData' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.QuoteNo;
                        }
                    });

                    $("#lcDatatablesListInValidData").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesListInValidData").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTableTbInValidData.page.len(1).draw();
                        } else {
                            dataTableTbInValidData.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesListInValidData").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });

                    setTimeout(function () {
                        var table = $('#TbInValidData').DataTable();
                        table.columns.adjust();
                    }, 100);
                } catch (e) {
                    alert("GenerateTbInValidData() : " + e);
                }
            }
            else {
                alert('GenerateTbInValidData : Browser Not Support')
            }
        }
    </script>

    <%--gen Table Valid Data Can Create Request--%>
    <script type="text/javascript">
        function GenerateTbTbValidData() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableTbValidData = $("#TbValidData").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $(".paginate_button").click(function () {
                                currentPageValidData = dataTableTbValidData.page.info().page;
                            });
                        },
                        "deferRender": false,
                        "columns": [
                        {
                            "data": null,
                            "sortable": true,
                            render: function (data, type, row, meta) {
                                return meta.row + 1;
                            }
                        },
                        { "data": "Plant", "autoWidth": true },
                        { "data": "PIRNo", "autoWidth": true },
                        { "data": "MaterialCode", "autoWidth": true },
                        { "data": "MaterialDesc", "autoWidth": true },
                        { "data": "VendorCode", "autoWidth": true },
                        { "data": "VendorName", "autoWidth": true },
                        { "data": "ProcessGroup", "autoWidth": true }
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
                                text: '<span class="glyphicon glyphicon-export"></span> Export',
                                className: "btn btn-sm btn-success"
                            }
                        ],
                        language: {
                            'emptytable': 'No data found',
                            'search': '',
                            'searchPlaceholder': 'Filter All Columns',
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatablesListValidData' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.QuoteNo;
                        }
                    });

                    $("#lcDatatablesListValidData").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesListValidData").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTableTbValidData.page.len(1).draw();
                        } else {
                            dataTableTbValidData.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesListValidData").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });

                    setTimeout(function () {
                        var table = $('#TbValidData').DataTable();
                        table.columns.adjust();
                    }, 100);
                } catch (e) {
                    alert("GenerateTbQuoteRefListInvalid() : " + e);
                }
            }
            else {
                alert('GenerateTbQuoteRefListInvalid : Browser Not Support')
            }
        }
    </script>

    <%--GenerateTbInValidDataReqTemp--%>
    <script type="text/javascript">
        function GenerateTbInValidDataReqTemp() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableTbInValidDataReqTemp = $("#TbInValidDataReqTemp").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $(".paginate_button").click(function () {
                                currentPageInValidDataReqTemp = dataTableTbInValidDataReqTemp.page.info().page;
                            });
                        },
                        "deferRender": false,
                        "columns": [
                        {
                            "data": null,
                            "sortable": true,
                            render: function (data, type, row, meta) {
                                return meta.row + 1;
                            }
                        },
                        { "data": "Plant", "autoWidth": true },
                        { "data": "PIRNo", "autoWidth": true },
                        { "data": "MaterialCode", "autoWidth": true },
                        { "data": "MaterialDesc", "autoWidth": true },
                        { "data": "VendorCode", "autoWidth": true },
                        { "data": "VendorName", "autoWidth": true },
                        { "data": "ProcessGroup", "autoWidth": true }
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
                                text: '<span class="glyphicon glyphicon-export"></span> Export',
                                className: "btn btn-sm btn-success"
                            }
                        ],
                        language: {
                            'emptytable': 'No data found',
                            'search': '',
                            'searchPlaceholder': 'Filter All Columns',
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatablesListInValidDataReqTemp' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.QuoteNo;
                        }
                    });

                    $("#lcDatatablesListInValidDataReqTemp").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesListInValidDataReqTemp").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTableTbInValidDataReqTemp.page.len(1).draw();
                        } else {
                            dataTableTbInValidDataReqTemp.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesListInValidDataReqTemp").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });

                    setTimeout(function () {
                        var table = $('#TbInValidDataReqTemp').DataTable();
                        table.columns.adjust();
                    }, 100);
                } catch (e) {
                    alert("GenerateTbInValidDataReqTemp() : " + e);
                }
            }
            else {
                alert('GenerateTbInValidDataReqTemp : Browser Not Support')
            }
        }
    </script>

    <%--GenerateTbValidDataReqTemp--%>
    <script type="text/javascript">
        function GenerateTbValidDataReqTemp() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableTbValidDataReqTemp = $("#TbValidDataReqTemp").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $(".paginate_button").click(function () {
                                currentPageValidDataReqTemp = dataTableTbValidDataReqTemp.page.info().page;
                            });
                        },
                        "deferRender": false,
                        "columns": [
                        {
                            "data": null,
                            "sortable": true,
                            render: function (data, type, row, meta) {
                                return meta.row + 1;
                            }
                        },
                        { "data": "NewRequestNumber", "autoWidth": true },
                        { "data": "MaterialCode", "autoWidth": true },
                        { "data": "MaterialDesc", "autoWidth": true },
                        { "data": "CodeRef", "autoWidth": true },
                        { "data": "VendorCode", "autoWidth": true },
                        { "data": "VendorName", "autoWidth": true },
                        { "data": "ProcessGroup", "autoWidth": true },
                        {
                            "data": null,
                            "render": function (data, type, row, meta, value) {
                                var htmltext = "";
                                if (type === 'display') {
                                    var data_filter = DataValidComp.filter(element => element.NewRequestNumber == row.NewRequestNumber);
                                    htmltext += '<table id="TbComp_' + row.NewRequestNumber + '" class="table table-responsive table-bordered nopadding" border="1" style="border-color:#DDDDDD; overflow-x:auto;"> ' +
                                                '<thead>' +
                                                    '<tr style="background-color:#009DDD!important;">' +
                                                        '<th>Comp Material</td>' +
                                                        '<th>Comp Desc</td>' +
                                                        '<th>Amt Scur</td>' +
                                                        '<th>Selling Crcy</td>' +
                                                        '<th>Amount VCur</td>' +
                                                        '<th>Vendor Crcy</td>' +
                                                        '<th>Unit</td>' +
                                                        '<th>UOM</td>' +
                                                        '<th>Valid From</td>' +
                                                        '<th>Valid To</td>' +
                                                        '<th>Exch Rate</td>' +
                                                        '<th>Exch Rate Valid From</td>' +
                                                    '</tr>' +
                                                '</thead>';

                                    for (var r = 0; r < data_filter.length; r++) {
                                        var CompMaterial = data_filter[r].CompMaterial === null ? '' : data_filter[r].CompMaterial;
                                        var CompMaterialDesc = data_filter[r].CompMaterialDesc === null ? '' : data_filter[r].CompMaterialDesc;
                                        var AmtSCur = data_filter[r].AmtSCur === null ? '' : data_filter[r].AmtSCur;
                                        if (AmtSCur.toString().length > 0) {
                                            AmtSCur = parseFloat(AmtSCur).toFixed(2);
                                        }
                                        var SellingCrcy = data_filter[r].SellingCrcy === null ? '' : data_filter[r].SellingCrcy;
                                        var AmtVCur = data_filter[r].AmtVCur === null ? '' : data_filter[r].AmtVCur;
                                        if (AmtVCur.toString().length > 0) {
                                            AmtVCur = parseFloat(AmtVCur).toFixed(2);
                                        }
                                        var VendorCrcy = data_filter[r].VendorCrcy === null ? '' : data_filter[r].VendorCrcy;
                                        var Unit = data_filter[r].Unit === null ? '' : data_filter[r].Unit;
                                        var UOM = data_filter[r].UOM === null ? '' : data_filter[r].UOM;
                                        var CusMatValFrom = data_filter[r].CusMatValFrom === null ? '' : moment(data_filter[r].CusMatValFrom).format('DD-MM-YYYY');
                                        var CusMatValTo = data_filter[r].CusMatValTo === null ? '' : moment(data_filter[r].CusMatValTo).format('DD-MM-YYYY');
                                        var ExchRate = data_filter[r].ExchRate === null ? '' : data_filter[r].ExchRate;
                                        if (ExchRate.toString().length > 0) {
                                            ExchRate = parseFloat(ExchRate).toFixed(4);
                                        }
                                        var ExchRateValidFrom = data_filter[r].ExchRateValidFrom === null ? '' : moment(data_filter[r].ExchRateValidFrom).format('DD-MM-YYYY');
                                        htmltext += '<tr>' +
                                                        '<td>' + CompMaterial + '</td>' +
                                                        '<td>' + CompMaterialDesc + '</td>' +
                                                        '<td>' + AmtSCur + '</td>' +
                                                        '<td>' + SellingCrcy + '</td>' +
                                                        '<td>' + AmtVCur + '</td>' +
                                                        '<td>' + VendorCrcy + '</td>' +
                                                        '<td>' + Unit + '</td>' +
                                                        '<td>' + UOM + '</td>' +
                                                        '<td>' + CusMatValFrom + '</td>' +
                                                        '<td>' + CusMatValTo + '</td>' +
                                                        '<td>' + ExchRate + '</td>' +
                                                        '<td>' + ExchRateValidFrom + '</td>' +
                                                    '</tr>';
                                    }
                                    htmltext += '</table>';

                                    return htmltext;
                                }
                                return data;
                            },
                            "autoWidth": true
                        }
                        ],
                        "ordering": true,
                        columnDefs: [{
                            orderable: false,
                            targets: "no-sort"
                        }],
                        "dom": "<'row'<'col-lg-12 col-sm-12 col-md-12 col-12'l>>" +
                               "<'row'<'col-sm-12'tr>>" +
                               "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                        buttons: [
                            {
                                extend: "excel",
                                text: '<span class="glyphicon glyphicon-export"></span> Export',
                                className: "btn btn-sm btn-success"
                            }
                        ],
                        language: {
                            'emptytable': 'No data found',
                            'search': '',
                            'searchPlaceholder': 'Filter All Columns',
                            "lengthMenu": "Show <input class='' type='text' id='lcDatatablesListValidDataReqTemp' value='10' style='width:70px; display:unset;margin:0 10px;' type='number' min='1'/> entries &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.QuoteNo;
                        }
                    });

                    $("#lcDatatablesListValidDataReqTemp").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesListValidDataReqTemp").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTableTbValidDataReqTemp.page.len(1).draw();
                        } else {
                            dataTableTbValidDataReqTemp.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesListValidDataReqTemp").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });

                    setTimeout(function () {
                        var table = $('#TbValidDataReqTemp').DataTable();
                        table.columns.adjust();
                    }, 100);
                } catch (e) {
                    alert("GenerateTbValidDataReqTemp() : " + e);
                }
            }
            else {
                alert('GenerateTbValidDataReqTemp : Browser Not Support')
            }
        }
    </script>


    <%--ValidateProcess()--%>
    <script type="text/javascript">
        function ValidateProcess() {
            var OK = false;
            try {
                var Mytable = $('#TbBasicData').DataTable();
                var Mydata = Mytable.rows().data();
                if (Mydata.length > 0) {
                    OK = true;
                }
                else {
                    alert("No Data To Proceed");
                }
            } catch (e) {
                alert("ValidateProcess() : " + e)
            }

            return OK;
        }
    </script>

    <%--ValidateCreateReq()--%>
    <script type="text/javascript">
        function ValidateCreateReq() {
            var iserr = false;
            try {
                var Mytable = $('#TbValidData').DataTable();
                var Mydata = Mytable.rows().data();
                var err = "";
                err += "Please check field listed in below : \n";

                var ckMatCost = document.getElementById("ChcMatCost").checked;
                var ckProcCost = document.getElementById("ChcProcCost").checked;
                var ckSubMatCost = document.getElementById("ChcSubMat").checked;
                var ckOthCost = document.getElementById("ChcOthMat").checked;

                if (Mydata.length <= 0) {
                    iserr = true;
                    err += "No Data To Proceed  \n";
                }

                if (ckMatCost == false && ckProcCost == false && ckSubMatCost == false && ckOthCost == false) {
                    err += "check at least 1 Cost to revice  \n";
                    $("#ChcMatCost").addClass("InvalidCheckBox");
                    $("#ChcProcCost").addClass("InvalidCheckBox");
                    $("#ChcSubMat").addClass("InvalidCheckBox");
                    $("#ChcOthMat").addClass("InvalidCheckBox");
                    iserr = true;
                }
                else {
                    $("#ChcMatCost").removeClass("InvalidCheckBox");
                    $("#ChcProcCost").removeClass("InvalidCheckBox");
                    $("#ChcSubMat").removeClass("InvalidCheckBox");
                    $("#ChcOthMat").removeClass("InvalidCheckBox");
                }

                var DdlReasonIdx = $("#DdlReason")[0].selectedIndex;
                var ResDueDate = $("#TxtResDueDate").val();
                var ValDate = $("#TxtValidDate").val();
                var TxtDuenextRev = $("#TxtDuenextRev").val();

                if (DdlReasonIdx <= 0) {
                    document.getElementById("DdlReason").style.border = "1px solid #ff0000";
                    err += "Select Request Purpose \n";
                    iserr = true;
                }
                else {
                    var DdlReason = document.getElementById('DdlReason');
                    var ReasonSelct = DdlReason.options[DdlReason.selectedIndex].value;
                    if (ReasonSelct.toString() == "Others") {
                        var TxtRem = $("#txtRem").val();
                        if (TxtRem == "") {
                            document.getElementById("txtRem").style.border = "1px solid #ff0000";
                            err += "enter remark for Other Option \n";
                            iserr = true;
                        }
                        else {
                            document.getElementById("txtRem").style.border = "1px solid #CCCCCC";
                        }
                    }
                }

                if (ResDueDate == "") {
                    document.getElementById("TxtResDueDate").style.border = "1px solid #ff0000";
                    err += "Reponse due date cannot be empty \n";
                    iserr = true;
                }
                else {
                    document.getElementById("TxtResDueDate").style.border = "1px solid #CCCCCC";
                }

                if (ValDate == "") {
                    document.getElementById("TxtValidDate").style.border = "1px solid #ff0000";
                    err += "Effective Date Cannot be empty \n";
                    iserr = true;
                }
                else {
                    document.getElementById("TxtValidDate").style.border = "1px solid #CCCCCC";
                }

                if (TxtDuenextRev == "") {
                    document.getElementById("TxtDuenextRev").style.border = "1px solid #ff0000";
                    err += "Due Dt Next Rev date Cannot be empty \n";
                    iserr = true;
                }
                else {
                    document.getElementById("TxtDuenextRev").style.border = "1px solid #CCCCCC";
                }

                if (ValDate != "" && TxtDuenextRev != "") {
                    var ArrValDate = ValDate.split('/');
                    var NewFormatValDate = ArrValDate[2] + '-' + ArrValDate[1] + '-' + ArrValDate[0];
                    var DtArrValDate = new Date(NewFormatValDate)

                    var ArrDuenextRev = TxtDuenextRev.split('/');
                    var NewFormatDuenextRev = ArrDuenextRev[2] + '-' + ArrDuenextRev[1] + '-' + ArrDuenextRev[0];
                    var DtDuenextRev = new Date(NewFormatDuenextRev)

                    if (DtArrValDate >= DtDuenextRev) {
                        document.getElementById("TxtDuenextRev").style.border = "1px solid #ff0000";
                        err += "Effective Date should not be Less than Due date \n";
                        iserr = true;
                    }
                }

            } catch (e) {
                alert("ValidateCreateReq() : " + e)
            }

            if (iserr == true) {
                alert("ValidateCreateReq() : " + err);
                return false;
            }
            else {
                return true;
            }
        }

        function ChgEmptyFlColor() {
            try {
                var DdlReasonIdx = $("#DdlReason")[0].selectedIndex;
                var cMatCost = document.getElementById("ChcMatCost").checked;
                var cProcCost = document.getElementById("ChcProcCost").checked;
                var cSubMatCost = document.getElementById("ChcSubMat").checked;
                var cOthCost = document.getElementById("ChcOthMat").checked;
                var ResDueDate = $("#TxtResDueDate").val();
                var ValDate = $("#TxtValidDate").val();
                var TxtDuenextRev = $("#TxtDuenextRev").val();

                //var fu = document.getElementById("FlUpload");
                //var val = $("#FlUpload").val().toLowerCase();
                //var regex = new RegExp("(.*?)\.(xlsx)$");

                if (DdlReasonIdx <= 0) {
                    document.getElementById("DdlReason").style.border = "1px solid #ff0000";
                }
                else {
                    var DdlReason = document.getElementById('DdlReason');
                    var ReasonSelct = DdlReason.options[DdlReason.selectedIndex].value;
                    if (ReasonSelct.toString() == "Others") {
                        var TxtRem = $("#txtRem").val();
                        if (TxtRem == "") {
                            document.getElementById("txtRem").style.border = "1px solid #ff0000";
                        }
                        else {
                            document.getElementById("txtRem").style.border = "1px solid #CCCCCC";
                        }
                    }
                }

                if (cMatCost == false && cProcCost == false && cSubMatCost == false && cOthCost == false) {
                    $("#ChcMatCost").addClass("InvalidCheckBox");
                    $("#ChcProcCost").addClass("InvalidCheckBox");
                    $("#ChcSubMat").addClass("InvalidCheckBox");
                    $("#ChcOthMat").addClass("InvalidCheckBox");
                }
                else {
                    $("#ChcMatCost").removeClass("InvalidCheckBox");
                    $("#ChcProcCost").removeClass("InvalidCheckBox");
                    $("#ChcSubMat").removeClass("InvalidCheckBox");
                    $("#ChcOthMat").removeClass("InvalidCheckBox");
                }

                if (ResDueDate == "") {
                    document.getElementById("TxtResDueDate").style.border = "1px solid #ff0000";
                }
                else {
                    document.getElementById("TxtResDueDate").style.border = "1px solid #CCCCCC";
                }
                if (ValDate == "") {
                    document.getElementById("TxtValidDate").style.border = "1px solid #ff0000";
                }
                else {
                    document.getElementById("TxtValidDate").style.border = "1px solid #CCCCCC";
                }
                if (TxtDuenextRev == "") {
                    document.getElementById("TxtDuenextRev").style.border = "1px solid #ff0000";
                }
                else {
                    document.getElementById("TxtDuenextRev").style.border = "1px solid #CCCCCC";
                }
            }
            catch (err) {
                alert(err + ": ChgEmptyFlColor")
            }
        }
    </script>

    <%--Cek Vendor Vs material--%>
    <script type="text/javascript">
        function CekDuplicateWithExpiredReq() {
            var DtExpiredInvalidMain = [];
            DataDetExpiredReq = [];
            ShowLoading();
            setTimeout(function () {
                try {
                    if (typeof (Worker) !== "undefined") {
                        jQuery.noConflict();
                        var url = mainUrl + "/EmetServices/RevisionEMET/MyXml.asmx/CekVendorVsMaterialExpiredReq";
                        var Mytable = $('#TbBasicData').DataTable();
                        var Mydata = Mytable.rows().data();

                        var _VendVsMat = [];
                        var _VendVsMatOriginal = [];

                        for (var i = 0; i < Mydata.length; i++) {
                            var VendorCode = Mydata[i].VendorCode;
                            var MaterialCode = Mydata[i].MaterialCode;
                            _VendVsMat.push({
                                "Vendor": VendorCode,
                                "Material": MaterialCode
                            });
                        }

                        if (_VendVsMat.length <= 0) {
                            alert("CekDuplicateWithExpiredReq() : " + 'No Data To Process');
                            CloseLoading();
                            return false;
                        }
                        else {
                            _VendVsMatOriginal = _VendVsMat;
                            var IsOK = false;

                            while (_VendVsMat.length > 100 || _VendVsMat.length <= 100) {
                                var cek = true;
                                var filtered = [];
                                if (_VendVsMat.length > 100) {
                                    var filtered = _VendVsMat.slice(0, 100);
                                    _VendVsMat = _VendVsMat.slice(101, _VendVsMat.length);
                                }

                                var jsonText;
                                if (filtered.length == 0) {
                                    jsonText = JSON.stringify({ VendVsMat: _VendVsMat });
                                }
                                else {
                                    jsonText = JSON.stringify({ VendVsMat: filtered });
                                }

                                $.ajax({
                                    url: url,
                                    cache: false,
                                    type: "POST",
                                    dataType: 'json',
                                    contentType: "application/json; charset=utf-8",
                                    //data: { VendVsMat: _VendVsMat },
                                    data: jsonText,
                                    async: false,
                                    beforeSend: function () {
                                    },
                                    complete: function () {
                                    },
                                    success: function (xml, ajaxStatus) {
                                        var data = JSON.parse(xml.d.toString());
                                        if (DtExpiredInvalidMain == null) {
                                            //DtExpiredInvalid = data;
                                        }
                                        else {
                                            if (data.success == true && data.message == "Data Invalid Found") {
                                                //DtExpiredInvalid.push({ data })
                                                var InvalidMain = data.mainData;
                                                var dataInvalid = data.InvalidDataSelected;

                                                if (InvalidMain.length > 0) {

                                                    for (var i = 0; i < InvalidMain.length; i++) {
                                                        DtExpiredInvalidMain.push({
                                                            "Plant": InvalidMain[i].Plant,
                                                            "RequestNumber": InvalidMain[i].RequestNumber,
                                                            "RequestDate": InvalidMain[i].RequestDate,
                                                            "QuoteResponseDueDate": InvalidMain[i].QuoteResponseDueDate,
                                                            "Material": InvalidMain[i].Material,
                                                            "MaterialDesc": InvalidMain[i].MaterialDesc
                                                        });
                                                    }

                                                    for (var i = 0; i < dataInvalid.length; i++) {
                                                        DataDetExpiredReq.push({
                                                            "Plant": dataInvalid[i].Plant,
                                                            "RequestNumber": dataInvalid[i].RequestNumber,
                                                            "RequestDate": dataInvalid[i].RequestDate,
                                                            "QuoteResponseDueDate": dataInvalid[i].QuoteResponseDueDate,
                                                            "QuoteNo": dataInvalid[i].QuoteNo,
                                                            "Material": dataInvalid[i].Material,
                                                            "MaterialDesc": dataInvalid[i].MaterialDesc,
                                                            "VendorCode1": dataInvalid[i].VendorCode1,
                                                            "VendorName": dataInvalid[i].VendorName
                                                        });
                                                    }
                                                }

                                            }
                                        }
                                    },
                                    error: function (xhr, status, error) {
                                        alert("CekDuplicateWithExpiredReq() : " + error);
                                        CloseLoading();
                                        cek = false;
                                    }
                                });

                                if (cek == false) {
                                    break;
                                }
                                if (_VendVsMat.length <= 100) {
                                    break;
                                }
                            }

                            if (DtExpiredInvalidMain.length > 0) {
                                OpenModalDuplicateExpired();
                                setTimeout(function () {
                                    dataTableDuplicateWithExpiredReq.clear().draw();
                                    dataTableDuplicateWithExpiredReq.rows.add(DtExpiredInvalidMain).draw();
                                    dataTableDuplicateWithExpiredReq.columns.adjust().draw();
                                }, 500);
                                CloseLoading();
                                return false;
                            }
                            else {
                                if (CekVendorVsMaterial(_VendVsMatOriginal) == true) {
                                    return true;
                                }
                                else {
                                    return false;
                                }
                            }
                        }
                    }
                    else {
                        CloseLoading();
                        return false;
                    }
                } catch (e) {
                    alert("CekDuplicateWithExpiredReq : " + e);
                    CloseLoading();
                    return false;
                }
            }, 500);
        }

        function CekVendorVsMaterial(_VendVsMat) {
            var Ok = false;
            
            if (typeof (Worker) !== "undefined") {
                var DataInvlid = [];
                try {
                    if (typeof (Worker) !== "undefined") {
                        jQuery.noConflict();
                        var url = mainUrl + "/EmetServices/RevisionEMET/MyXml.asmx/CekVendorVsMaterial";
                        var _VendVsMatOriginal = [];

                        if (_VendVsMat.length <= 0) {
                            alert("CekVendorVsMaterial" + 'No Data To Process');
                            return false;
                        }
                        else {
                            _VendVsMatOriginal = _VendVsMat;
                            var IsOK = false;
                            while (_VendVsMat.length > 100 || _VendVsMat.length <= 100) {
                                var cek = true;
                                var filtered = [];
                                var LengtBeforeFilter = _VendVsMat.length;
                                if (_VendVsMat.length > 100) {
                                    var filtered = _VendVsMat.slice(0, 101);
                                    _VendVsMat = _VendVsMat.slice(101, _VendVsMat.length);
                                }

                                var jsonText;
                                if (filtered.length == 0) {
                                    jsonText = JSON.stringify({ VendVsMat: _VendVsMat });
                                }
                                else {
                                    jsonText = JSON.stringify({ VendVsMat: filtered });
                                }

                                $.ajax({
                                    url: url,
                                    cache: false,
                                    type: "POST",
                                    dataType: 'json',
                                    contentType: "application/json; charset=utf-8",
                                    //data: { VendVsMat: _VendVsMat },
                                    data: jsonText,
                                    async: false,
                                    beforeSend: function () {
                                    },
                                    complete: function () {
                                    },
                                    success: function (xml, ajaxStatus) {
                                        var data = JSON.parse(xml.d.toString());
                                        if (data.success == true) {
                                            if (data.InvalidDataSelected != null) {
                                                if (data.InvalidDataSelected.length > 0) {
                                                    //DataInvlid = data.InvalidDataSelected;
                                                    var Dta = data.InvalidDataSelected;
                                                    if (DataInvlid.length <= 0) {
                                                        DataInvlid = Dta
                                                    }
                                                    else {
                                                        if (Dta.lengh > 0) {
                                                            for (var i = 0; i < Dta.lengh; i++) {
                                                                DataInvlid.push(Dta[0]);
                                                            }
                                                        }
                                                    }
                                                    console.log(Dta);
                                                }
                                            }
                                        }
                                        else {
                                            alert("CekVendorVsMaterial " + data.message);
                                        }
                                    },
                                    error: function (xhr, status, error) {
                                        alert("CekVendorVsMaterial(_VendVsMat) : " + error)
                                    }
                                });
                                
                                if (cek == false) {
                                    break;
                                }
                                if (LengtBeforeFilter <= 100) {
                                    break;
                                }
                            }
                            Ok = true;
                        }
                    }
                } catch (e) {
                    alert("CekVendorVsMaterial(_VendVsMat) : " + e)
                }
            }
            else {
                alert('CekVendorVsMaterial : Browser Not Support')
            }
            
            if (Ok = true) {
                var Mytable = $('#TbBasicData').DataTable();
                var Mydata = Mytable.rows().data().toArray();
                var FinalData = [];
                if (DataInvlid.length > 0) {
                    dataTableTbQuoteRefListInvalid.clear().draw();
                    dataTableTbQuoteRefListInvalid.rows.add(DataInvlid).draw();
                    dataTableTbQuoteRefListInvalid.columns.adjust().draw();

                    var Dtfilter = [];

                    for (var i = 0; i < DataInvlid.length; i++) {
                        var V2 = DataInvlid[i].VendorCode1;
                        var M2 = DataInvlid[i].Material;
                        var VM2 = V2.concat('-', M2);
                        for (var v = 0; v < Mydata.length; v++) {
                            if (Mydata[v] != null) {
                                var V1 = Mydata[v].VendorCode;
                                var M1 = Mydata[v].MaterialCode;
                                var VM1 = V1.concat('-', M1);

                                if (VM1 === VM2) {
                                    delete Mydata[v];
                                    break;
                                }
                                else {
                                    //Dtfilter.push(_VendVsMatOriginal[v]);
                                }
                            }
                        }
                    }

                    for (var i = 0; i < Mydata.length; i++) {
                        if (Mydata[i] != null) {
                            Dtfilter.push(Mydata[i]);
                        }
                    }
                    FinalData = Dtfilter;
                    dataTableTbValidData.clear().draw();
                    dataTableTbValidData.rows.add(Dtfilter).draw();
                    dataTableTbValidData.columns.adjust().draw();
                    document.getElementById("BtnProceed").disabled = true;
                    document.getElementById("BtnCancelProceed").disabled = false;

                    document.getElementById("BtnCancelCreateReq").disabled = true;
                    document.getElementById("BtnCreateRequest").disabled = false;
                }
                else {
                    FinalData = Mydata;
                    dataTableTbValidData.clear().draw();
                    dataTableTbValidData.rows.add(Mydata).draw();
                    dataTableTbValidData.columns.adjust().draw();
                }

                if (CekVendorVsMaterialValid(FinalData) == true) {
                    CloseLoading();
                    document.getElementById("BtnProceed").disabled = true;
                    document.getElementById("BtnCancelProceed").disabled = false;
                    return true;
                }
                else {
                    CloseLoading();
                    document.getElementById("BtnProceed").disabled = true;
                    document.getElementById("BtnCancelProceed").disabled = false;
                    return false;
                }
            }
            else {
                CloseLoading();
                return false;
            }
        }

    </script>

    <%--Cek Material And Vendor Data To MDM--%>
    <script type="text/javascript">
        function CekVendorVsMaterialValid(_FinalData) {
            var OK = false;
            jQuery.noConflict();
            if (typeof (Worker) !== "undefined") {
                var DataInvlid = [];
                var DataValid = [];
                var IsConContinue = false;
                try {
                    if (typeof (Worker) !== "undefined") {

                        var url = mainUrl + "/EmetServices/MassRevision/MyXMLMassRevisionALL.asmx/CekVendorVsMaterialValid";
                        var _FinalDataOriginal = [];

                        if (_FinalData.length <= 0) {
                            alert('No Data To Process');
                            CloseLoading();
                        }
                        else {
                            _FinalDataOriginal = _FinalData;
                            while (_FinalData.length > 100 || _FinalData.length <= 100) {

                                var cek = true;
                                var filtered = [];
                                var LengtBeforeFilter = _FinalData.length;
                                var LengtAfterFilter = 0;
                                if (_FinalData.length > 100) {
                                    var filtered = _FinalData.slice(0, 101);
                                    _FinalData = _FinalData.slice(101, _FinalData.length);
                                }

                                var jsonText;
                                if (filtered.length == 0) {
                                    jsonText = JSON.stringify({ MassrevVendorVsMaterial: _FinalData });
                                }
                                else {
                                    jsonText = JSON.stringify({ MassrevVendorVsMaterial: filtered });
                                }

                                $.ajax({
                                    url: url,
                                    cache: false,
                                    type: "POST",
                                    dataType: 'json',
                                    timeout: 300000,
                                    //processData: false,
                                    //cache: false,
                                    async: false,
                                    contentType: "application/json; charset=utf-8",
                                    //data: { VendVsMat: _FinalData },
                                    data: jsonText,
                                    beforeSend: function () {

                                    },
                                    complete: function () {
                                        //console.log(DataValid);
                                        //console.log(DataInvlid);
                                    },
                                    success: function (xml, ajaxStatus) {
                                        var data = JSON.parse(xml.d.toString());
                                        if (data.success == true) {
                                            var Dta = data.InValidData;
                                            if (Dta.length > 0) {
                                                for (var i = 0; i < Dta.length; i++) {
                                                    DataInvlid.push(Dta[i]);
                                                }
                                            }

                                            var DtaV = data.ValidData;
                                            if (DtaV.length > 0) {
                                                for (var i = 0; i < DtaV.length; i++) {
                                                    DataValid.push(DtaV[i]);
                                                }
                                            }
                                        }
                                        else {
                                            cek = false;
                                            alert("CekVendorVsMaterialValid(_FinalData) : " + data.message);
                                        }
                                    },
                                    error: function (request, xhr, status, error) {
                                        cek = false;
                                        alert("CekVendorVsMaterialValid(_FinalData) : " + request.responseText)
                                    }
                                });

                                if (cek == false) {
                                    break;
                                }
                                if (LengtBeforeFilter <= 100) {
                                    break;
                                }
                            }
                            IsConContinue = true;
                        }
                    }
                } catch (e) {
                    alert("CekVendorVsMaterialValid(_FinalData) : " + e)
                }

                if (IsConContinue = true) {

                    dataTableTbInValidData.clear().draw();
                    if (DataInvlid.length > 0) {
                        dataTableTbInValidData.rows.add(DataInvlid).draw();
                    }
                    dataTableTbInValidData.columns.adjust().draw();

                    dataTableTbValidData.clear().draw();
                    if (DataValid.length > 0) {
                        dataTableTbValidData.rows.add(DataValid).draw();
                    }
                    dataTableTbValidData.columns.adjust().draw();
                    OK = true;
                }
                else {
                    dataTableTbInValidData.clear().draw();
                    dataTableTbInValidData.columns.adjust().draw();

                    dataTableTbValidData.clear().draw();
                    dataTableTbValidData.columns.adjust().draw();
                }
            }
            else {
                dataTableTbInValidData.clear().draw();
                dataTableTbInValidData.columns.adjust().draw();

                dataTableTbValidData.clear().draw();
                dataTableTbValidData.columns.adjust().draw();
                alert('CekVendorVsMaterialValid : Browser Not Support')
            }

            return OK;
        }
    </script>

    <%--Expired Data Function--%>
    <script type="text/javascript">
        function RbRejectExpReq(id) {
            try {
                jQuery.noConflict();

                var Mytable = $('#TbDuplicateWithExpiredReq').DataTable();
                var Mydata = Mytable.rows().data();
                var OldDueDate = moment(Mydata[id].QuoteResponseDueDate).format('DD-MM-YYYY');
                document.getElementById("TxtNewResDueDate_" + id).value = OldDueDate;
                document.getElementById("TxtNewResDueDate_" + id).disabled = true;
                //document.getElementById("GvDuplicateWithExpiredReq_IcnCalendarNewDueDate_" + id).disabled = true;
                //reversecheckfromitemToHeader('Reject');
            }
            catch (err) {
                alert(err + ' : RbRejectExpReq(id)');
            }
        }

        function IsAllRadioChecked(Source) {
            try {
                jQuery.noConflict();
                document.getElementById("RbAllReject").checked = false;
                document.getElementById("RbAllchangeDate").checked = false;
                var Mytable = $('#TbDuplicateWithExpiredReq').DataTable();
                var Mydata = Mytable.rows().data();

                var IsAllCheck = true;
                if (Source.toUpperCase() == "REJ") {
                    for (var i = 0; i < Mydata.length; i++) {
                        var RbRej = document.getElementById("RbRej_" + i);
                        if (RbRej.checked == false) {
                            IsAllCheck = false;
                            break;
                        }
                    }

                    if (IsAllCheck == true) {
                        document.getElementById("RbAllReject").checked = true;
                    }
                }
                else {
                    for (var i = 0; i < Mydata.length; i++) {
                        var RbchangeDate = document.getElementById("RbchangeDate_" + i);
                        if (RbchangeDate.checked == false) {
                            IsAllCheck = false;
                            break;
                        }
                    }

                    if (IsAllCheck == true) {
                        document.getElementById("RbAllchangeDate").checked = true;
                    }
                }
            } catch (e) {
                alert("IsAllRadioChecked" + e);
            }
        }

        function RbChangedateResDueDate(id) {
            try {
                document.getElementById("TxtNewResDueDate_" + id).disabled = false;
                //document.getElementById("GvDuplicateWithExpiredReq_IcnCalendarNewDueDate_" + id).disabled = false;
                DatePitckerAppr(id);
                //reversecheckfromitemToHeader('ChangeDate');
            }
            catch (err) {
                alert(err + ' : RbChangedateResDueDate(id)');
            }
        }

        function CheckAllRejOrChgDate(HeaderAction) {
            try {
                jQuery.noConflict();
                var Mytable = $('#TbDuplicateWithExpiredReq').DataTable();
                var Mydata = Mytable.rows().data();
                var RbRej, RbchangeDate, TxtNewResDueDate;

                if (HeaderAction == "Reject") {
                    for (var i = 0; i < Mydata.length; i++) {
                        var RqNo = Mydata[i].RequestNumber;
                        RbRej = document.getElementById("RbRej_" + i + "");
                        RbchangeDate = document.getElementById("RbchangeDate_" + i + "");
                        if (RbRej != null) {
                            RbRej.checked = true;
                            RbRejectExpReq(i);
                        }
                    }
                }
                else {
                    for (var i = 0; i < Mydata.length; i++) {
                        var RqNo = Mydata[i].RequestNumber;
                        RbRej = document.getElementById("RbRej_" + i + "");
                        RbchangeDate = document.getElementById("RbchangeDate_" + i + "");

                        if (RbchangeDate != null) {
                            RbchangeDate.checked = true;
                            RbChangedateResDueDate(i);
                        }
                    }
                }
            }
            catch (err) {
                alert(err + ": CheckAllRejOrChgDate(HeaderAction)");
                return false;
            }
        }

        function ValidateDuplicateReqList() {
            try {
                var Mytable = $('#TbDuplicateWithExpiredReq').DataTable();
                var Mydata = Mytable.rows().data();
                var RbRej, RbchangeDate, TxtNewResDueDate;
                var IsAllCheck = true;
                var IsValidNewResDueDate = true;
                var ReqNo = "";

                for (var i = 0; i < Mydata.length; i++) {
                    RbRej = document.getElementById("RbRej_" + i);
                    RbchangeDate = document.getElementById("RbchangeDate_" + i);

                    if (RbRej != null && RbchangeDate != null) {
                        if (RbRej.checked == false && RbchangeDate.checked == false) {
                            IsAllCheck = false;
                            ReqNo = Mydata[i].RequestNumber.toString();
                            break;
                        }
                    }
                }

                if (IsAllCheck == true) {
                    for (var i = 0; i < Mydata.length; i++) {
                        RbRej = document.getElementById("RbRej_" + i);
                        RbchangeDate = document.getElementById("RbchangeDate_" + i);
                        TxtNewResDueDate = document.getElementById("TxtNewResDueDate_" + i);

                        if (TxtNewResDueDate != null) {
                            if (RbchangeDate != null) {
                                if (RbchangeDate.checked == true) {
                                    if (TxtNewResDueDate.value == moment(Mydata[i].QuoteResponseDueDate).format('DD-MM-YYYY')) {
                                        IsValidNewResDueDate = false;
                                        ReqNo = Mydata[i].RequestNumber.toString();
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (IsValidNewResDueDate == false) {
                        alert("New Response Due Date can not be same with old Response Due Date, Please Check Request No : " + ReqNo)
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    alert("Please select decision for Request no : " + ReqNo)
                    return false;
                }
            }
            catch (err) {
                alert(err + ": ValidateDuplicateReqList()");
                return false;
            }
        }

        function SumbitDuplicateReqList() {
            try {
                jQuery.noConflict();
                var url = mainUrl + "/EmetServices/RevisionEMET/MyXML.asmx/SumbitDuplicateReqList";
                var Mytable = $('#TbDuplicateWithExpiredReq').DataTable();
                var Mydata = Mytable.rows().data();

                var _DuplicateReqListAction = [];
                for (var i = 0; i < Mydata.length; i++) {
                    if (document.getElementById("RbRej_" + i) != null && document.getElementById("TxtNewResDueDate_" + i) != null) {
                        var ActionRej = document.getElementById("RbRej_" + i).checked;
                        var NewResDueDate = document.getElementById("TxtNewResDueDate_" + i).value;
                        _DuplicateReqListAction.push({
                            "RequestNumber": Mydata[i].RequestNumber.toString(),
                            //"QuoteNo": Mydata[i].QuoteNo.toString(),
                            "ActionRej": ActionRej,
                            "NewResDueDate": NewResDueDate,
                        })
                    }
                }
                var jsonText = JSON.stringify({ DuplicateReqListAction: _DuplicateReqListAction });

                $.ajax({
                    url: url,
                    cache: false,
                    type: "POST",
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    data: jsonText,
                    async: false,
                    complete: function () {

                    },
                    success: function (xml, ajaxStatus) {
                        var Mydata = JSON.parse(xml.d.toString());
                        if (Mydata.success == true) {
                            alert("SumbitDuplicateReqList()" + Mydata.message);
                            CloseModalDuplicateExpired();
                            document.getElementById("BtnProceed").click();
                        }
                        else {
                            alert("SumbitDuplicateReqList()" + Mydata.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert("SumbitDuplicateReqList()" + error);
                    }
                });
            } catch (e) {
                alert("SumbitDuplicateReqList()" + e)
            }
        }

        function ExpandOrColapseAll() {
            try {
                ShowLoading();
                setTimeout(function () {
                    if (document.getElementById("imgExOrCol").title == "Open") {

                        document.getElementById("imgExOrCol").src = mainUrl + "/Images/details_close.png";
                        document.getElementById("imgExOrCol").title = "Closed";

                        var table = document.getElementById("TbDuplicateWithExpiredReq");
                        for (var i = 1; i < table.rows.length; i++) {
                            if (table.rows[i].cells[1] != null) {
                                var RowNo = table.rows[i].cells[1].innerText;
                                var row = dataTableDuplicateWithExpiredReq.row(RowNo - 1);
                                var data = dataTableDuplicateWithExpiredReq.row(RowNo - 1).data();

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

                        dataTableDuplicateWithExpiredReq.rows().every(function (rowIdx, tableLoop, rowLoop) {
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
                        dataTableDuplicateWithExpiredReq.rows().every(function (rowIdx, tableLoop, rowLoop) {
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
                                var row = dataTableDuplicateWithExpiredReq.row(RowNo - 1);
                                var data = dataTableDuplicateWithExpiredReq.row(RowNo - 1).data();

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

        function LoadDataDetail(RequestNumber) {
            try {
                var data_filter = DataDetExpiredReq.filter(element => element.RequestNumber == RequestNumber)

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
                    var VendorCode = data_filter[r].VendorCode1 === null ? '' : data_filter[r].VendorCode1;
                    var VendorName = data_filter[r].VendorName === null ? '' : data_filter[r].VendorName;
                    var QuoteNo = data_filter[r].QuoteNo === null ? '' : data_filter[r].QuoteNo;
                    var url = mainUrl+ "QQPReview.aspx?Number=" + QuoteNo;
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

    <%--Process Create request Temp--%>
    <script type="text/javascript">
        function DeleteNonRequest() {
            var OK = false;
            if (typeof (Worker) !== "undefined") {
                var url = mainUrl + "/EmetServices/MassRevision/MyJSONMassRevisionALL.asmx/DeleteNonRequest";
                $.ajax({
                    url: url,
                    cache: false,
                    type: "POST",
                    data: { UseID: document.getElementById("TxtuseID").value },
                    dataType: 'json',
                    async: false,
                    beforeSend: function () {
                        ShowLoading();
                    },
                    complete: function () {
                        CloseLoading();
                    },
                    success: function (data) {
                        if (data.success == true) {
                            OK = true;
                        }
                        else {
                            alert("DeleteNonRequest : " + data.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert("DeleteNonRequest : " + error);
                    }
                });
            }
            else {
                alert('DeleteNonRequest : Browser Not Support')
            }

            return OK;
        }

        function CreateReqTemp() {
            ShowLoading();
            setTimeout(function () {
                if (DeleteNonRequest() == true) {
                    try {
                        if (typeof (Worker) !== "undefined") {
                            jQuery.noConflict();
                            var DataInvlid = [];
                            var DataValid = [];
                            DataValidComp = [];
                            var IsConContinue = true;

                            var url = mainUrl + "/EmetServices/MassRevision/MyXMLMassRevisionALL.asmx/CreateReqTemp";
                            var Mytable = $('#TbValidData').DataTable();
                            var Mydata = Mytable.rows().data().toArray();

                            var _IsMatCost = document.getElementById("ChcMatCost").checked;
                            var _IsProcCost = document.getElementById("ChcProcCost").checked;
                            var _IsSubMatCost = document.getElementById("ChcSubMat").checked;
                            var _IsOthCost = document.getElementById("ChcOthMat").checked;

                            var _EffDate = document.getElementById("TxtValidDate").value;
                            var _DueDatenextRev = document.getElementById("TxtDuenextRev").value;
                            var _RespDueDate = document.getElementById("TxtResDueDate").value;
                            var _RePurposeReason = document.getElementById("DdlReason").value;
                            var _RePurposeRemark = document.getElementById("txtRem").value;

                            var _UserId = document.getElementById("TxtuseID").value;
                            var _SMNPicDept = document.getElementById("TxtuserDept").value;

                            while (Mydata.length > 10 || Mydata.length <= 10) {
                                var cek = true;
                                var filtered = [];
                                var LengtBeforeFilter = Mydata.length;
                                var LengtAfterFilter = 0;
                                if (Mydata.length > 10) {
                                    filtered = Mydata.slice(0, 11);
                                    Mydata = Mydata.slice(11, Mydata.length);
                                }

                                var jsonText;
                                if (filtered.length == 0) {
                                    jsonText = JSON.stringify({ IsMatCost: _IsMatCost, IsProcCost: _IsProcCost, IsSubMatCost: _IsSubMatCost, IsOthCost: _IsOthCost, EffDate: _EffDate, DueDatenextRev: _DueDatenextRev, RespDueDate: _RespDueDate, RePurposeReason: _RePurposeReason, RePurposeRemark: _RePurposeRemark, UserId: _UserId, SMNPicDept: _SMNPicDept, MassrevVendorVsMaterial: Mydata });
                                }
                                else {
                                    jsonText = JSON.stringify({ IsMatCost: _IsMatCost, IsProcCost: _IsProcCost, IsSubMatCost: _IsSubMatCost, IsOthCost: _IsOthCost, EffDate: _EffDate, DueDatenextRev: _DueDatenextRev, RespDueDate: _RespDueDate, RePurposeReason: _RePurposeReason, RePurposeRemark: _RePurposeRemark, UserId: _UserId, SMNPicDept: _SMNPicDept, MassrevVendorVsMaterial: filtered });
                                }

                                $.ajax({
                                    url: url,
                                    cache: false,
                                    type: "POST",
                                    dataType: 'json',
                                    timeout: 300000,
                                    //processData: false,
                                    //cache: false,
                                    async: false,
                                    contentType: "application/json; charset=utf-8",
                                    //data: { VendVsMat: _FinalData },
                                    data: jsonText,
                                    beforeSend: function () {

                                    },
                                    complete: function () {
                                        //console.log(DataValid);
                                        //console.log(DataInvlid);
                                    },
                                    success: function (xml, ajaxStatus) {
                                        var data = JSON.parse(xml.d.toString());
                                        if (data.success == true) {
                                            var Dta = data.ValidDataMain;
                                            if (Dta.length > 0) {
                                                for (var i = 0; i < Dta.length; i++) {
                                                    DataValid.push(Dta[i]);
                                                }
                                            }

                                            var DtaV = data.ValidDataMainComponent;
                                            if (DtaV.length > 0) {
                                                for (var i = 0; i < DtaV.length; i++) {
                                                    DataValidComp.push(DtaV[i]);
                                                }
                                            }

                                            var DtaInValid = data.InValidData;
                                            if (DtaInValid.length > 0) {
                                                for (var i = 0; i < DtaInValid.length; i++) {
                                                    DataInvlid.push(DtaInValid[i]);
                                                }
                                            }
                                        }
                                        else {
                                            cek = false;
                                            alert("CreateReqTemp() : " + data.message);
                                        }
                                    },
                                    error: function (request, xhr, status, error) {
                                        cek = false;
                                        alert("CreateReqTemp() : " + request.responseText)
                                    }
                                });

                                if (cek == false) {
                                    IsConContinue = false;
                                    break;
                                }
                                if (LengtBeforeFilter <= 10) {
                                    break;
                                }
                            }

                            if (IsConContinue = true) {
                                dataTableTbValidDataReqTemp.clear().draw();
                                if (DataValid.length > 0) {
                                    dataTableTbValidDataReqTemp.rows.add(DataValid).draw();
                                }
                                dataTableTbValidDataReqTemp.columns.adjust().draw();

                                dataTableTbInValidDataReqTemp.clear().draw();
                                if (DataInvlid.length > 0) {
                                    dataTableTbInValidDataReqTemp.rows.add(DataInvlid).draw();
                                }
                                dataTableTbInValidDataReqTemp.columns.adjust().draw();

                                CloseLoading();
                                document.getElementById("ChcMatCost").disabled = true;
                                document.getElementById("ChcProcCost").disabled = true;
                                document.getElementById("ChcSubMat").disabled = true;
                                document.getElementById("ChcOthMat").disabled = true;
                                document.getElementById("TxtValidDate").disabled = true;
                                document.getElementById("TxtDuenextRev").disabled = true;
                                document.getElementById("TxtResDueDate").disabled = true;
                                document.getElementById("DdlReason").disabled = true;
                                document.getElementById("DdlReason").disabled = true;
                                document.getElementById("txtRem").disabled = true;
                                document.getElementById("BtnCreateRequest").disabled = true;

                                document.getElementById("BtnCancelCreateReq").disabled = false;
                            }
                            else {
                                dataTableTbValidDataReqTemp.clear().draw();
                                dataTableTbValidDataReqTemp.columns.adjust().draw();

                                dataTableTbInValidDataReqTemp.clear().draw();
                                dataTableTbInValidDataReqTemp.columns.adjust().draw();
                                CloseLoading();
                            }
                        }
                        else {
                            dataTableTbValidDataReqTemp.clear().draw();
                            dataTableTbValidDataReqTemp.columns.adjust().draw();

                            dataTableTbInValidDataReqTemp.clear().draw();
                            dataTableTbInValidDataReqTemp.columns.adjust().draw();

                            CloseLoading();
                            return false;
                        }
                    } catch (e) {
                        dataTableTbValidDataReqTemp.clear().draw();
                        dataTableTbValidDataReqTemp.columns.adjust().draw();

                        dataTableTbInValidDataReqTemp.clear().draw();
                        dataTableTbInValidDataReqTemp.columns.adjust().draw();

                        alert("CreateReqTemp : " + e);
                        CloseLoading();
                        return false;
                    }
                }
                else {
                    CloseLoading();
                    return false;
                }
            }, 500);
        }
    </script>

    <script type="text/javascript">
        function CancelProceed() {
            try {
                document.getElementById("BtnCreateRequest").disabled = true;
                document.getElementById("BtnCancelCreateReq").disabled = true;

                document.getElementById("BtnProceed").disabled = false;
                document.getElementById("BtnCancelProceed").disabled = true;

                dataTableBasicData.clear().draw();
                dataTableDuplicateWithExpiredReq.clear().draw();
                dataTableTbQuoteRefListInvalid.clear().draw();
                dataTableTbValidData.clear().draw();
                dataTableTbInValidData.clear().draw();
                dataTableTbValidDataReqTemp.clear().draw();
                dataTableTbInValidDataReqTemp.clear().draw();
            } catch (e) {
                alert("CancelCreateReq : " + e)
            }
        }
    </script>

    <script type="text/javascript">
        function CancelCreateReq() {
            try {
                document.getElementById("ChcMatCost").disabled = false;
                document.getElementById("ChcProcCost").disabled = false;
                document.getElementById("ChcSubMat").disabled = false;
                document.getElementById("ChcOthMat").disabled = false;
                document.getElementById("TxtValidDate").disabled = false;
                document.getElementById("TxtDuenextRev").disabled = false;
                document.getElementById("TxtResDueDate").disabled = false;
                document.getElementById("DdlReason").disabled = false;
                document.getElementById("DdlReason").disabled = false;
                document.getElementById("txtRem").disabled = false;
                document.getElementById("BtnCreateRequest").disabled = false;
                document.getElementById("BtnCancelCreateReq").disabled = true;

                dataTableTbValidDataReqTemp.clear().draw();
                dataTableTbInValidDataReqTemp.clear().draw();
            } catch (e) {
                alert("CancelCreateReq : " + e)
            }
        }
    </script>

    <%--ValidateSubmit()--%>
    <script type="text/javascript">
        function ValidateSubmit() {
            var OK = false;
            try {
                var Mytable = $('#TbValidDataReqTemp').DataTable();
                var Mydata = Mytable.rows().data();
                if (Mydata.length > 0) {
                    OK = true;
                }
                else {
                    alert("No Data To Submit");
                }
            } catch (e) {
                alert("ValidateProcess() : " + e)
            }

            return OK;
        }

        function ProceedSubmitRequest() {
            ShowLoading();
            setTimeout(function () {
                try {
                    if (typeof (Worker) !== "undefined") {
                        jQuery.noConflict();
                        var IsConContinue = true;

                        var url = mainUrl + "/EmetServices/MassRevision/MyXMLMassRevisionALL.asmx/ProceedSubmitRequest";
                        //var Mytable = $('#TbValidDataReqTemp').DataTable();
                        //var Mydata = Mytable.rows().data().toArray();
                        var Mydata = DataValidComp;

                        var _isMassRevisionAll = document.getElementById("RbisMassRevisionAll").checked;
                        var _IsMatCost = document.getElementById("ChcMatCost").checked;
                        var _IsProcCost = document.getElementById("ChcProcCost").checked;
                        var _IsSubMatCost = document.getElementById("ChcSubMat").checked;
                        var _IsOthCost = document.getElementById("ChcOthMat").checked;

                        var _EffDate = document.getElementById("TxtValidDate").value;
                        var _DueDatenextRev = document.getElementById("TxtDuenextRev").value;
                        var _RespDueDate = document.getElementById("TxtResDueDate").value;
                        var _RePurposeReason = document.getElementById("DdlReason").value;
                        var _RePurposeRemark = document.getElementById("txtRem").value;

                        var _UserId = document.getElementById("TxtuseID").value;
                        var _SMNPicDept = document.getElementById("TxtuserDept").value;

                        while (Mydata.length > 10 || Mydata.length <= 10) {
                            var cek = true;
                            var filtered = [];
                            var LengtBeforeFilter = Mydata.length;
                            var LengtAfterFilter = 0;
                            if (Mydata.length > 10) {
                                filtered = Mydata.slice(0, 11);
                                Mydata = Mydata.slice(11, Mydata.length);
                            }

                            var jsonText;
                            if (filtered.length == 0) {
                                for (var i = 0 ; i < Mydata.length ; i++) {
                                    if (Mydata[i].MassUpdateDate != null) {
                                        Mydata[i].MassUpdateDate = moment(Mydata[i].MassUpdateDate).format('YYYY-MM-DD');
                                    }
                                    if (Mydata[i].CusMatValFrom != null) {
                                        Mydata[i].CusMatValFrom = moment(Mydata[i].CusMatValFrom).format('YYYY-MM-DD');
                                    }
                                    if (Mydata[i].CusMatValTo != null) {
                                        Mydata[i].CusMatValTo = moment(Mydata[i].CusMatValTo).format('YYYY-MM-DD');
                                    }
                                    if (Mydata[i].ExchRateValidFrom != null) {
                                        Mydata[i].ExchRateValidFrom = moment(Mydata[i].ExchRateValidFrom).format('YYYY-MM-DD');
                                    }
                                }

                                jsonText = JSON.stringify({
                                    isMassRevisionAll:_isMassRevisionAll, IsMatCost: _IsMatCost, IsProcCost: _IsProcCost, IsSubMatCost: _IsSubMatCost, IsOthCost: _IsOthCost,
                                    EffDate: _EffDate, DueDatenextRev: _DueDatenextRev, RespDueDate: _RespDueDate, RePurposeReason: _RePurposeReason, RePurposeRemark: _RePurposeRemark,
                                    UserId: _UserId, SMNPicDept: _SMNPicDept,
                                    MainAndCompData: Mydata
                                });
                            }
                            else {
                                for (var i = 0 ; i < filtered.length ; i++) {
                                    if (filtered[i].MassUpdateDate != null) {
                                        filtered[i].MassUpdateDate = moment(filtered[i].MassUpdateDate).format('YYYY-MM-DD');
                                    }
                                    if (filtered[i].CusMatValFrom != null) {
                                        filtered[i].CusMatValFrom = moment(filtered[i].CusMatValFrom).format('YYYY-MM-DD');
                                    }
                                    if (filtered[i].CusMatValTo != null) {
                                        filtered[i].CusMatValTo = moment(filtered[i].CusMatValTo).format('YYYY-MM-DD');
                                    }
                                    if (filtered[i].ExchRateValidFrom != null) {
                                        filtered[i].ExchRateValidFrom = moment(filtered[i].ExchRateValidFrom).format('YYYY-MM-DD');
                                    }
                                }
                                jsonText = JSON.stringify({
                                    isMassRevisionAll: _isMassRevisionAll, IsMatCost: _IsMatCost, IsProcCost: _IsProcCost, IsSubMatCost: _IsSubMatCost, IsOthCost: _IsOthCost,
                                    EffDate: _EffDate, DueDatenextRev: _DueDatenextRev, RespDueDate: _RespDueDate, RePurposeReason: _RePurposeReason, RePurposeRemark: _RePurposeRemark,
                                    UserId: _UserId, SMNPicDept: _SMNPicDept, MainAndCompData: filtered
                                });
                            }
                            $.ajax({
                                url: url,
                                cache: false,
                                type: "POST",
                                dataType: 'json',
                                timeout: 300000,
                                //processData: false,
                                //cache: false,
                                async: false,
                                contentType: "application/json; charset=utf-8",
                                //data: { VendVsMat: _FinalData },
                                data: jsonText,
                                beforeSend: function () {

                                },
                                complete: function () {
                                    //console.log(DataValid);
                                    //console.log(DataInvlid);
                                },
                                success: function (xml, ajaxStatus) {
                                    var data = JSON.parse(xml.d.toString());
                                    if (data.success == true) {
                                        
                                    }
                                    else {
                                        cek = false;
                                        alert("ProceedSubmitRequest() : " + data.message);
                                    }
                                },
                                error: function (request, xhr, status, error) {
                                    cek = false;
                                    alert("ProceedSubmitRequest() : " + request.responseText)
                                }
                            });

                            if (cek == false) {
                                IsConContinue = false;
                                break;
                            }
                            if (LengtBeforeFilter <= 10) {
                                break;
                            }
                        }

                        if (IsConContinue = true) {
                            alert("Data Submitted")
                            window.location = mainUrl + "/Home.aspx";
                        }
                        else {
                            CloseLoading();
                            return false;
                        }
                    }
                    else {

                        CloseLoading();
                        return false;
                    }
                } catch (e) {

                    alert("ProceedSubmitRequest : " + e);
                    CloseLoading();
                    return false;
                }
            }, 500);
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
<body>
    <form id="form1" runat="server" autopostback="false">
        <asp:ScriptManager ID="scriptmanager1" runat="server" AsyncPostBackTimeout="36000"></asp:ScriptManager>
        <div class="col-md-12" id="DvMsgErr" runat="server" visible="false">
            <asp:Label runat="server" ID="LbMsgErr" Font-Bold="true" Visible="true"></asp:Label>
        </div>
        <div class="row">
            <div id="loading" class="col-md-12" style="padding-top: 200px;">
                <img id="loading-image" src="images/loading.gif" alt="Loading..." />
                <div class="col-md-12" style="text-align: center; opacity: 1;">
                    <asp:UpdatePanel runat="server" ID="UpLoading" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lbLoading" runat="server" Text="please Wait..." Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
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
                                <asp:LinkButton runat="server" OnClientClick="SidebarMenu();return false;" class="btn btn-link btn-sm text-white order-1 order-sm-0" ID="sidebarToggle"><i class="fas fa-bars"></i> </asp:LinkButton>
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

            <div id="content-wrapper" style="background-color: white;">
                <div class="container-fluid">
                    <div class="row" style="padding-bottom: 10px;">
                        <div class="col-md-12">
                            <div class="col-md-12 card" style="padding: 10px; background-color: white;">
                                <div class="col-md-12 card-body Padding-Nol">
                                    <div class="col-md-12" style="background-color: white;">
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" autopostback="false">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-sm-4" style="padding-bottom: 5px;">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <asp:RadioButton runat="server" AutoPostBack="false" ID="RbisMassRevision" GroupName="MassRevisn" Text="&nbsp;Mass Revision" Font-Size="Medium"  />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:RadioButton runat="server" AutoPostBack="false" ID="RbisMassRevisionAll" GroupName="MassRevisn" Text="&nbsp;Mass Revision All" Font-Size="Medium" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-8 text-right" style="padding-bottom: 5px;">
                                                        <asp:Button runat="server" ID="BtnReset" Text="Reset" CssClass="btn btn-sm btn-warning" OnClientClick="BtnReset_Click();return false;" autopostback="false"></asp:Button>
                                                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="ShowLoading();Close_Click()" CssClass="btn btn-sm btn-danger" autopostback="false" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12" style="padding-bottom: 10px;">
                                                        <div class="col-md-12" style="border-bottom: 2px solid #006EB7"></div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <asp:FileUpload runat="server" ID="FlUpload" autopostback="false" />
                                                    </div>
                                                    <div class="col-md-6 text-right">
                                                        <asp:LinkButton runat="server" ID="BtnUpload" CssClass="btn btn-primary btn-sm"
                                                            OnClientClick="if(valBeforeUpload()==false) return false;SetUpSessionFileUpload(); return false"><span class="fa fa-upload"></span> Upload  </asp:LinkButton>
                                                        <a id="BtnTemplate" class="btn btn-success btn-sm"><span class="fa fa-download"></span> Download Template</a>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <%--All data from file upload--%>
                                                <div class="row" runat="server" id="DvAllDataUpload" >
                                                    <div class="col-md-12" style="padding-bottom: 0px;">
                                                        <div class="row" style="padding-bottom: 5px;">
                                                            <div class="col-md-12">
                                                                <div class="col-md-12" style="background-color:#464646; margin-bottom:5px;">
                                                                    <asp:Label ID="LbTitleAllData" runat="server" Text="All Data From File Upload"
                                                                 Visible="true" ForeColor="White"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-12" style="padding: 0px; ">
                                                            <table id="TbBasicData" class="table table-responsive table-bordered nopadding" >
                                                                <thead style="background-color:#464646!important;">
                                                                    <tr>
                                                                        <th>No.</th>
                                                                        <th>Plant</th>
                                                                        <th>PIR No</th>
                                                                        <th>Material Code</th>
                                                                        <th>Material Desc</th>
                                                                        <th>Vendor Code</th>
                                                                        <th>Vendor Name</th>
                                                                        <th>Process Group</th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px; padding-top: 5px;" runat="server" id="DvProceed">
                                                    <div class="col-md-6">
                                                    </div>
                                                    <div class="col-md-3">
                                                        <button id="BtnCancelProceed" class="btn btn-danger btn-sm btn-block" disabled="disabled" onclick="CancelProceed();return false;">Cancel Proceed</button>
                                                    </div>
                                                    <div class="col-md-3 text-right">
                                                        <asp:Button runat="server" ID="BtnProceed" CssClass="btn btn-primary btn-sm btn-block"
                                                            Text="Proceed" OnClientClick="if(ValidateProcess()==false)return false;CekDuplicateWithExpiredReq();return false" />
                                                    </div>
                                                </div>

                                                <%--vendor with material is in progress--%>
                                                <div class="row" id="DvInvalidRequest" style="padding-bottom:10px;">
                                                    <div class="col-md-12">
                                                    <div class="col-md-12" style="background-color:#D93702; margin-bottom:5px;">
                                                        <asp:Label ID="Label8" runat="server" Text="Below vendor with material is in progress"
                                                             Visible="true" ForeColor="White"></asp:Label>
                                                    </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <table id="TbQuoteRefListInvalid" class="table table-responsive table-striped table-bordered nopadding" style="width:100%">
                                                            <thead>
                                                                <tr>
                                                                    <th style="background-color:#D93702!important">Request Date</th>
                                                                    <th style="background-color:#D93702!important">Req. No</th>
                                                                    <th style="background-color:#D93702!important">Res Due Date</th>
                                                                    <th style="background-color:#D93702!important">QuoteNo</th>
                                                                    <th style="background-color:#D93702!important">Material</th>
                                                                    <th style="background-color:#D93702!important">Material Desc</th>
                                                                    <th style="background-color:#D93702!important">Vendor Code</th>
                                                                    <th style="background-color:#D93702!important">Vendor Name</th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div>
                                                </div>

                                                <%--Invalid data can not process to create request--%>
                                                <div class="row" id="DvInValidData" style="padding-bottom:10px;">
                                                    <div class="col-md-12">
                                                    <div class="col-md-12" style="background-color:#bf0303; margin-bottom:5px;">
                                                        <asp:Label ID="Label6" runat="server" Text="Invalid Data"
                                                             Visible="true" ForeColor="White" ></asp:Label>
                                                    </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <table id="TbInValidData" class="table table-responsive table-bordered nopadding" >
                                                                <thead style="background-color:#bf0303!important;">
                                                                    <tr>
                                                                        <th>No.</th>
                                                                        <th>Plant</th>
                                                                        <th>PIR No</th>
                                                                        <th>Material Code</th>
                                                                        <th>Material Desc</th>
                                                                        <th>Vendor Code</th>
                                                                        <th>Vendor Name</th>
                                                                        <th>Process Group</th>
                                                                        <th>Remark</th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                    </div>
                                                </div>

                                                <%--valid data can process to create request--%>
                                                <div class="row" id="DvValidData" style="padding-bottom:10px;">
                                                    <div class="col-md-12">
                                                    <div class="col-md-12" style="background-color:#027604; margin-bottom:5px;">
                                                        <asp:Label ID="Label1" runat="server" Text="Valid Data Can Create Request"
                                                             Visible="true" ForeColor="White"></asp:Label>
                                                    </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <table id="TbValidData" class="table table-responsive table-bordered nopadding" >
                                                                <thead style="background-color:#027604!important;">
                                                                    <tr>
                                                                        <th>No.</th>
                                                                        <th>Plant</th>
                                                                        <th>PIR No</th>
                                                                        <th>Material Code</th>
                                                                        <th>Material Desc</th>
                                                                        <th>Vendor Code</th>
                                                                        <th>Vendor Name</th>
                                                                        <th>Process Group</th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                    </div>
                                                </div>

                                                <%--Form control--%>
                                                <div class="row">
                                                    <div class="col-md-12 " runat="server" id="DvFormContrl2" style="padding-bottom: 5px; padding-top: 10px;">
                                                        <fieldset style="border-color:#CED4DA!important">
                                                            <legend>Please Complete Below Info</legend>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            Revison For
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:CheckBox runat="server" ID="ChcMatCost" Text="Material Cost" Width="100px" AutoPostBack="false" onchange="return false" />
                                                                    <asp:CheckBox runat="server" ID="ChcProcCost" Text="Process Cost" Width="100px" AutoPostBack="false" onchange="return false" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:CheckBox runat="server" ID="ChcSubMat" Text="Sub Mat Cost" Width="100px" AutoPostBack="false" onchange="return false" />
                                                                    <asp:CheckBox runat="server" ID="ChcOthMat" Text="Others Cost" Width="100px" AutoPostBack="false" onchange="return false" />
                                                                </div>
                                                            </div>
                                                            <div class="row" style="padding-bottom: 5px;">
                                                                <div class="col-md-6">
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <asp:Label runat="server" ID="Label4" Text="Effective Date"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="group-main">
                                                                                <div style="padding: 0px; width: 100%">
                                                                                    <asp:TextBox ID="TxtValidDate" OnclientClick="return false;"
                                                                                        onkeydown="javascript:preventInput(event);" CssClass="form_datetime"
                                                                                        autocomplete="off" AutoCompleteType="Disabled" onchange="ChgEmptyFlColor();"
                                                                                        runat="server" ForeColor="Black">
                                                                                    </asp:TextBox>
                                                                                </div>
                                                                                <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 1px 0px 1px;">
                                                                                    <a class="fa fa-calendar" runat="server" id="Span1" style="color: #005496; padding: 1px 3px 0px 1px;"
                                                                                        onclick="javascript: $('#TxtValidDate').focus();"></a>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <asp:Label runat="server" ID="Label2" Text="Due Dt Next Rev"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="group-main">
                                                                                <div style="padding: 0px; width: 100%">
                                                                                    <asp:TextBox ID="TxtDuenextRev" OnclientClick="return false;" CssClass="form_datetime"
                                                                                        onkeydown="javascript:preventInput(event);"
                                                                                        autocomplete="off" AutoCompleteType="Disabled" onchange="ChgEmptyFlColor();"
                                                                                        runat="server" ForeColor="Black">
                                                                                    </asp:TextBox>
                                                                                </div>
                                                                                <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 1px 0px 1px;">
                                                                                    <a class="fa fa-calendar" runat="server" id="A1" style="color: #005496; padding: 1px 3px 0px 1px;"
                                                                                        onclick="javascript: $('#TxtDuenextRev').focus();"></a>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <asp:Label runat="server" ID="Label3" Text="Response Due Date"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="group-main">
                                                                                <div style="padding: 0px; width: 100%">
                                                                                    <asp:TextBox ID="TxtResDueDate" OnclientClick="return false;"
                                                                                        onkeydown="javascript:preventInput(event);" onchange="ChgEmptyFlColor();" CssClass="form_datetime"
                                                                                        autocomplete="off" AutoCompleteType="Disabled"
                                                                                        runat="server" ForeColor="Black">
                                                                                    </asp:TextBox>
                                                                                </div>
                                                                                <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 1px 0px 1px;">
                                                                                    <a class="fa fa-calendar" runat="server" id="IcnCalResduedate" style="color: #005496; padding: 1px 3px 0px 1px;"
                                                                                        onclick="javascript: $('#TxtResDueDate').focus();"></a>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <asp:Label runat="server" ID="Label5" Text="Request Purpose"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:DropDownList runat="server" ID="DdlReason" onchange="DdlReasonchange('DdlReason', 'DvRemark', 'LblengtVC')"></asp:DropDownList>
                                                                            <div class="row" runat="server" id="DvRemark" style="display: none;">
                                                                                <div class="col-md-12" style="padding-top: 5px;">
                                                                                    <asp:TextBox ID="txtRem" runat="server" placeholder="Maximum 200 character" ToolTip="Maximum 200 character" onchange="ChgEmptyFlColor();"
                                                                                        TextMode="MultiLine" Width="100%" onkeyup="RemarkLght('txtRem','LblengtVC')"></asp:TextBox>
                                                                                    <asp:Label ID="LblengtVC" runat="server" Text="200 Character left" Font-Size="12px" CssClass="fa pull-right" Font-Bold="false" Font-Names="calibri"></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px; padding-top: 5px;" runat="server" id="DvCreateReq">
                                                    <div class="col-md-6">
                                                    </div>
                                                    <div class="col-md-3">
                                                        <button id="BtnCancelCreateReq" class="btn btn-danger btn-sm btn-block" disabled="disabled" onclick="CancelCreateReq();return false;">Cancel Request</button>

                                                    </div>
                                                    <div class="col-md-3 text-right">
                                                        <asp:Button runat="server" ID="BtnCreateRequest" CssClass="btn btn-primary btn-sm btn-block"
                                                            OnClientClick="if(ValidateCreateReq()==false) return false;CreateReqTemp();return false;" Text="Create Request" />
                                                    </div>
                                                </div>

                                                <%--list invalid data request quote--%>
                                                <div class="row" runat="server" id="DvInvalidListRequest">
                                                    <div class="col-md-12" style="padding-bottom: 5px;">
                                                        <div class="col-md-12" style="background-color:#D93702; margin-bottom:5px;">
                                                                <asp:Label runat="server" ID="LbInvalidListRequest" Text="Invalid Request Due On Component Effective is Expired" ForeColor="White"></asp:Label>
                                                            </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-lg-12" style="padding: 0px;">
                                                            <table id="TbInValidDataReqTemp" class="table table-responsive table-bordered nopadding" >
                                                                <thead style="background-color:#D93702!important;">
                                                                    <tr>
                                                                        <th>No.</th>
                                                                        <th>Plant</th>
                                                                        <th>PIR No</th>
                                                                        <th>Material Code</th>
                                                                        <th>Material Desc</th>
                                                                        <th>Vendor Code</th>
                                                                        <th>Vendor Name</th>
                                                                        <th>Process Group</th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>

                                                <%--Valid Data Can Create request--%>
                                                <div class="row" runat="server" id="DvGvReqList">
                                                    <div class="col-md-12" style="padding-bottom: 5px; padding-top: 5px;">
                                                       <div class="col-md-12" style="background-color:#005496; margin-bottom:5px;">
                                                                <asp:Label runat="server" ID="LbQuoteMsg" Text="Request can be Created only for below Vendors" 
                                                                    ForeColor="White" ></asp:Label>
                                                            </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-lg-12" style="padding: 0px;">
                                                            <table id="TbValidDataReqTemp" class="table table-responsive table-bordered nopadding" >
                                                                <thead style="background-color:#005496!important;">
                                                                    <tr>
                                                                        <th>No.</th>
                                                                        <th>Req No</th>
                                                                        <th>Material Code</th>
                                                                        <th>Material Desc</th>
                                                                        <th>Code Ref</th>
                                                                        <th>Vendor Code</th>
                                                                        <th>Vendor Name</th>
                                                                        <th>Proc Group</th>
                                                                        <th></th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px; padding-top: 10px;" runat="server" id="DvSubmit">
                                                    <div class="col-md-9">
                                                    </div>
                                                    <div class="col-md-3">
                                                        <a class="btn btn-primary btn-sm btn-block" id="BtnSubmit" onclick="if(ValidateSubmit()==false) return false;ProceedSubmitRequest();return false;"> Submit</a>
                                                    </div>
                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
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
                <div class="col-md-12" style="padding: 5px; align-content: center; text-align: center">
                    <span style="font: bold 13px calibri, calibri">Copyright © ShimanoDT 2018</span>
                </div>
            </div>
        </div>

        <a class="scroll-to-top rounded" href="#page-top"><i class="fas fa-angle-up"></i></a>

        <!-- Modal duplicate reuest with expired request-->
        <div class="modal fade" id="MdDuplicateReq" data-backdrop="static" tabindex="-1" role="dialog" 
        aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false" >
            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
            <ContentTemplate>
            <div class="modal-dialog" style="width:95%; position: absolute; margin-top:0px; top:0px; margin-left:2%; ">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel21" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row"  style="background-color:#F7F7F7; padding-bottom:10px; padding-left:10px; padding-right:10px; box-shadow: 1px 1px 1000px 1px;
                                          border-top-left-radius:10px; border-top-right-radius:10px;
                                          border-bottom-left-radius:10px; border-bottom-right-radius:10px;">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-lg-12 text-center" style="padding-top:10px; padding-bottom:10px;">
                                            <!-- header -->
                                        </div>
                                    </div>

                                    <div class="row" style="padding-top:10px; background-color:white;">
                                        <div class="col-xs-12" style="padding:10px">
                                            <asp:Label runat="server" ID="Label47" Text="Below Vendor with material code selected have old request pending and expired, 
                                                please select action for update the date or reject to closed old request before create new request." />
                                        </div>
                                    </div>

                                    <div class="row" style="padding-top:10px; background-color:white;">
                                        <div class="col-md-12">
                                        <table id="TbDuplicateWithExpiredReq" class="table table-responsive table-bordered nopadding">
                                            <thead style="background-color:#006699!important;"> 
                                                <tr>
                                                    <th style="vertical-align: middle; background-color:whitesmoke ;" onclick="ExpandOrColapseAll();">
                                                    <img src="images/details_open.png" id="imgExOrCol"  title="Open" class="nopadding no-sort" /></th>
                                                    <th class="no-sort">No. </th>
                                                    <th class="no-sort">Request Date</th>
                                                    <th class="no-sort">Req. No</th>
                                                    <th class="no-sort">Res Due Date</th> 
                                                    <th class="no-sort">Material</th>
                                                    <th class="no-sort">Material Desc</th>
                                                    <th class="no-sort"><input type="radio" id="RbAllReject" onclick="CheckAllRejOrChgDate('Reject')" name="RejOrChgDateHeader" /> <label id="LbRbAllReject" onclick="CheckAllRejOrChgDate('Reject')" for="RbAllReject"> Reject </label></th>
                                                    <th class="no-sort"><input type="radio" id="RbAllchangeDate" onclick="CheckAllRejOrChgDate('Changedate')" name="RejOrChgDateHeader" /> <label id="LbRbAllchangeDate" onclick="CheckAllRejOrChgDate('Changedate')" for="RbAllchangeDate"> Change Date </label></th>
                                                    <th class="no-sort">New Response Due Date</th>
                                                </tr>
                                            </thead>
                                        </table>
                                        </div>
                                    </div>

                                    <div class="row pull-right" style="padding-top:10px;">	
                                        <asp:Button ID="BtnSubmitProcDuplicateReg" runat="server" Text="Submit" OnClientClick="if(ValidateDuplicateReqList()==false) return false;SumbitDuplicateReqList();return false;" autopostback="false"   CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="14px"/>
                                        <asp:Button ID="BtnCancelMdDuplicate" runat="server" Text="Cancel" OnClientClick="CloseModalDuplicateExpired();return false;" autopostback="false" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="14px"/>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <!-- Modal session expired -->
        <div class="modal fade" id="myModalSession" data-backdrop="static" tabindex="-1" role="dialog"
                        aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
            <ContentTemplate>
            <div class="modal-dialog">
                    <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5);border-bottom-left-radius: 15px;border-bottom-right-radius: 15px;">
                        <div class="col-md-12 Padding-Nol" style="font:bold 22px calibri, calibri; text-align:center; align-content:center;"> Your Session Is About To Expire !!  </div>
                      <h4></h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-2" style="padding:10px">
                                        <asp:Image ID="ImagWarning" runat="server" class="responsive" ImageUrl="~/js/jsextendsession/images/timeout-icon.png"/>
                                    </div>
                                    <div class="col-xs-10" style="padding:10px">
                                        <asp:Timer ID="TimerCntDown" runat="server" Interval="1000" Enabled="false"></asp:Timer>
                                        You will be logged out in : <asp:Label ID="countdown" runat="server" Font-Bold="true" ForeColor="Red" Text="30"></asp:Label> seconds<br />
                                        do u want to stay Sign In?
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5);border-top-left-radius: 15px;border-top-right-radius: 15px;">
                        <asp:Button ID="BtnRefresh" runat="server" Text="Yes, Keep me Sign In" CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="18px"/>
                        <asp:Button ID="CtnCloseMdl" runat="server" Text="No, Sign Me Out" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="18px"/>
                        <div style="display:none;"><asp:Button ID="StartTimer" runat="server" Text="Start" CssClass="btn btn-sm btn-primary" /></div>
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
