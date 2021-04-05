<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MassRevision.aspx.cs" Inherits="Material_Evaluation.NewRequestUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

    <!-- Page level plugin CSS-->
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" />

    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet" />

    <link href="Styles/NewStyle/NewStyle.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link rel="stylesheet" href="Scripts/jquery-ui-1.12.1/jquery-ui.css" />
    <link rel="stylesheet" href="js/jsextendsession/css/timeout-dialog.css" />
    <link href="js/BootstrapDatePcr/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <style type="text/css">
        .SideBarMenu {
            width: 300px;
        }

        .lbattachpad {
            padding: 2px 2px 0px 2px;
        }

            .lbattachpad:hover {
                padding: 2px;
            }

        .lbPreview {
            padding-top: 1px;
        }

            .lbPreview:hover {
                padding-top: 1px;
            }

        select[disabled] {
            background-color: #EBEBE4;
        }

        .WrapCnt td, th {
            white-space: normal !important;
            /*word-wrap: break-word;*/
            font-size: 14px !important;
        }

        .WrapCnt a {
            padding: 0px;
        }

        .selectedCell {
            background-color: lightblue;
        }

        .unselectedCell {
            background-color: white;
        }

        .modalextendseason-position {
            z-index: 999999 !important;
            position: fixed !important;
        }
    </style>

    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>

    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script src="Scripts/stickycolumandheaderplugin/tableHeadFixer.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/locales/bootstrap-datetimepicker.fr.js"></script>

    <%--script loading page--%>
    <script lang="javascript" type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });

        $(document).ready(function () {
            DatePitcker()
            ChgEmptyFlColor();
        });
    </script>
    <script type="text/javascript">
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

        function DatePitcker() {
            try {
                (function ($) {
                    $(".form_datetime").datetimepicker({
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
                alert(err + ": DatePitcker(txtID)");
            }
        }

        function OpenModalInvalidData() {
            try {
                jQuery.noConflict();
                $("#MdInvalidData").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            catch (err) {
                alert(err + ' : OpenModalInvalidData');
            }
        }

        function CloseModalInvalidData() {
            try {
                jQuery.noConflict();
                $("#MdInvalidData").modal('hide');
            }
            catch (err) {
                alert(err + ' : CloseModalQuoteRef');
            }
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
                HideBtnSubmit();
            }
            catch (err) {
                alert(err + ": DdlReasonchange")
            }
        }

        function HideBtnSubmit() {
            try {
                var Btn = document.getElementById("DvSubmit");
                var Table = document.getElementById("TbReqList");
                if (Btn != null) {
                    document.getElementById("DvSubmit").style.display = "none";
                }
                if (Table != null) {
                    document.getElementById("TbReqList").style.display = "none";
                }
            }
            catch (err) {
                alert(err + ":HideBtnSubmit")
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

        function ChgEmptyFlColor() {
            try {

                var FControl1 = document.getElementById("DvFormContrl1");
                var FControl2 = document.getElementById("DvFormContrl2");

                if (FControl1 != null && FControl2 != null) {
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
                        $("#ChcMatCost").css("outline-color", "red");
                        $("#ChcMatCost").css("outline-style", "solid");
                        $("#ChcMatCost").css("outline-width", "1px");
                        $("#ChcMatCost").css("padding", "0px");

                        $("#ChcProcCost").css("outline-color", "red");
                        $("#ChcProcCost").css("outline-style", "solid");
                        $("#ChcProcCost").css("outline-width", "1px");
                        $("#ChcProcCost").css("padding", "0px");

                        $("#ChcSubMat").css("outline-color", "red");
                        $("#ChcSubMat").css("outline-style", "solid");
                        $("#ChcSubMat").css("outline-width", "1px");
                        $("#ChcSubMat").css("padding", "0px");

                        $("#ChcOthMat").css("outline-color", "red");
                        $("#ChcOthMat").css("outline-style", "solid");
                        $("#ChcOthMat").css("outline-width", "1px");
                        $("#ChcOthMat").css("padding", "0px");
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

                    //if (fu.value.length <= 0) {
                    //    $("#FlUpload").css("border", "1px solid #ff0000");
                    //}
                    //else if (!(regex.test(val))) {
                    //    $("#FlUpload").css("border", "1px solid #ff0000");
                    //    $("#FlUpload").val("");
                    //}
                    //else {
                    //    $("#FlUpload").css("border", "1px solid #F7F7F7");
                    //}
                }
            }
            catch (err) {
                alert(err + ": ChgEmptyFlColor")
            }
        }

        function valBeforeUpload() {
            var err = "";
            err += "Please check field listed in below : \n";
            var iserr = false;

            //var DdlReasonIdx = $("#DdlReason")[0].selectedIndex;
            //var cMatCost = document.getElementById("ChcMatCost").checked;
            //var cProcCost = document.getElementById("ChcProcCost").checked;
            //var cSubMatCost = document.getElementById("ChcSubMat").checked;
            //var cOthCost = document.getElementById("ChcOthMat").checked;
            //var ResDueDate = $("#TxtResDueDate").val();
            //var ValDate = $("#TxtValidDate").val();

            var fu = document.getElementById("FlUpload");
            var val = $("#FlUpload").val().toLowerCase();
            var regex = new RegExp("(.*?)\.(xlsx)$");


            //if (DdlReasonIdx <= 0) {
            //    document.getElementById("DdlReason").style.border = "1px solid #ff0000";
            //    err += "Select Request Purpose \n";
            //    iserr = true;
            //}
            //else {
            //    var DdlReason = document.getElementById('DdlReason');
            //    var ReasonSelct = DdlReason.options[DdlReason.selectedIndex].value;
            //    if (ReasonSelct.toString() == "Others") {
            //        var TxtRem = $("#txtRem").val();
            //        if (TxtRem == "") {
            //            document.getElementById("txtRem").style.border = "1px solid #ff0000";
            //            err += "enter remark for Other Option \n";
            //            iserr = true;
            //        }
            //        else {
            //            document.getElementById("txtRem").style.border = "1px solid #CCCCCC";
            //        }
            //    }
            //}

            //if (cMatCost == false && cProcCost == false && cSubMatCost == false && cOthCost == false) {
            //    $("#ChcMatCost").css("outline-color", "red");
            //    $("#ChcMatCost").css("outline-style", "solid");
            //    $("#ChcMatCost").css("outline-width", "1px");
            //    $("#ChcMatCost").css("padding", "0px");

            //    $("#ChcProcCost").css("outline-color", "red");
            //    $("#ChcProcCost").css("outline-style", "solid");
            //    $("#ChcProcCost").css("outline-width", "1px");
            //    $("#ChcProcCost").css("padding", "0px");

            //    $("#ChcSubMat").css("outline-color", "red");
            //    $("#ChcSubMat").css("outline-style", "solid");
            //    $("#ChcSubMat").css("outline-width", "1px");
            //    $("#ChcSubMat").css("padding", "0px");

            //    $("#ChcOthMat").css("outline-color", "red");
            //    $("#ChcOthMat").css("outline-style", "solid");
            //    $("#ChcOthMat").css("outline-width", "1px");
            //    $("#ChcOthMat").css("padding", "0px");

            //    err += "check at least 1 Cost to revice";
            //    iserr = true;
            //}
            //if (ResDueDate == "") {
            //    document.getElementById("TxtResDueDate").style.border = "1px solid #ff0000";
            //    err += "Reponse due date cannot be empty \n";
            //    iserr = true;
            //}
            //else {
            //    document.getElementById("TxtResDueDate").style.border = "1px solid #CCCCCC";
            //}
            //if (ValDate == "") {
            //    document.getElementById("TxtValidDate").style.border = "1px solid #ff0000";
            //    err += "Valid date Cannot be empty \n";
            //    iserr = true;
            //}
            //else {
            //    document.getElementById("TxtValidDate").style.border = "1px solid #CCCCCC";
            //}

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

            if (iserr == true) {
                alert(err);
                return false;
            }
        }

        function valCreateReq() {
            var table = document.getElementById('GvValidData');
            var err = "";
            err += "Please check field listed in below : \n";
            var iserr = false;

            if (table != null) {
                var FControl1 = document.getElementById("DvFormContrl1");
                var FControl2 = document.getElementById("DvFormContrl2");

                if (FControl1 != null && FControl2 != null) {
                    var DdlReasonIdx = $("#DdlReason")[0].selectedIndex;
                    var cMatCost = document.getElementById("ChcMatCost").checked;
                    var cProcCost = document.getElementById("ChcProcCost").checked;
                    var cSubMatCost = document.getElementById("ChcSubMat").checked;
                    var cOthCost = document.getElementById("ChcOthMat").checked;
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

                    if (cMatCost == false && cProcCost == false && cSubMatCost == false && cOthCost == false) {
                        $("#ChcMatCost").css("outline-color", "red");
                        $("#ChcMatCost").css("outline-style", "solid");
                        $("#ChcMatCost").css("outline-width", "1px");
                        $("#ChcMatCost").css("padding", "0px");

                        $("#ChcProcCost").css("outline-color", "red");
                        $("#ChcProcCost").css("outline-style", "solid");
                        $("#ChcProcCost").css("outline-width", "1px");
                        $("#ChcProcCost").css("padding", "0px");

                        $("#ChcSubMat").css("outline-color", "red");
                        $("#ChcSubMat").css("outline-style", "solid");
                        $("#ChcSubMat").css("outline-width", "1px");
                        $("#ChcSubMat").css("padding", "0px");

                        $("#ChcOthMat").css("outline-color", "red");
                        $("#ChcOthMat").css("outline-style", "solid");
                        $("#ChcOthMat").css("outline-width", "1px");
                        $("#ChcOthMat").css("padding", "0px");

                        err += "check at least 1 Cost to revice";
                        iserr = true;
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
                        err += "Valid date Cannot be empty \n";
                        iserr = true;
                    }
                    else {
                        document.getElementById("TxtValidDate").style.border = "1px solid #CCCCCC";
                    }
                    if (TxtDuenextRev == "") {
                        document.getElementById("TxtDuenextRev").style.border = "1px solid #ff0000";
                        err += "Valid date Cannot be empty \n";
                        iserr = true;
                    }
                    else {
                        document.getElementById("TxtDuenextRev").style.border = "1px solid #CCCCCC";
                    }

                    if (ValDate != "" && TxtDuenextRev != "") {
                        debugger;
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
                }
            }
            else {
                err += "No valid data to create request \n";
                iserr = true;
            }

            if (iserr == true) {
                alert(err);
                return false;
            }
        }

        function ValidateSubmit() {
            try {
                ShowLoading();
                var table = document.getElementById('GvReqList');
                //table = null;
                if (table == null) {
                    alert('No Data To Submit, Please Create Request !');
                    CloseLoading();
                    return false;

                    //alert('^_^ sorry .... under construction');
                    //CloseLoading();
                    //return false;
                }
                else {
                    var RowsCount = $('#GvReqList tr').length;
                    if (RowsCount > 1) {
                        return true;
                    }
                    else {
                        CloseLoading();
                        alert('No Data To Submit, Please Create Request !');
                        return false;
                    }
                }
            }
            catch (err) {
                alert(err + ": ValidateSubmit()");
            }
        }

        function preventInput(evnt) {
            if (evnt.which != 9) evnt.preventDefault();
        }

        function validateBtnUpload() {
            try {

                var iserr = false;
                var fu = document.getElementById("FlUpload");
                var val = $("#FlUpload").val().toLowerCase();
                var regex = new RegExp("(.*?)\.(xlsx)$");
                var err = "";
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

                if (iserr == true) {
                    alert(err);
                    return false;
                }
            }
            catch (err) {
                return false;
                alert(err + ": validateBtnUpload()");
            }
        }

        function ExportInvalidData() {
            $("#BExportInvalidData").click();
        }

        function ExportInvalidDataListReq() {
            $("#BExportInvalidDataListRequest").click();
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

        function OpenModalDuplicateExpired() {
            try {
                jQuery.noConflict();
                $("#MdDuplicateReq").modal({
                    backdrop: 'static',
                    keyboard: false
                });
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

        function DatePitckerAppr(id) {
            try {
                (function ($) {
                    $('#GvDuplicateWithExpiredReq_TxtNewDueDate_' + id).datetimepicker({
                        fontAwesome: 'font-awesome',
                        format: "dd/mm/yyyy",
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

        function RbChangedateResDueDate(id) {
            try {
                (function ($) {
                    //document.getElementById("GvDuplicateWithExpiredReq_TxtNewDueDate_" + id).style.display = "block";
                    //document.getElementById("GvDuplicateWithExpiredReq_IcnCalendarNewDueDate_" + id).style.display = "block";
                    document.getElementById("GvDuplicateWithExpiredReq_TxtNewDueDate_" + id).disabled = false;
                    document.getElementById("GvDuplicateWithExpiredReq_IcnCalendarNewDueDate_" + id).disabled = false;
                    DatePitckerAppr(id);
                    reversecheckfromitemToHeader('ChangeDate');
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : DatePitcker(id)');
            }
        }

        function RbRejectExpReq(id) {
            try {
                (function ($) {
                    var table = document.getElementById('GvDuplicateWithExpiredReq');
                    var OldDueDate = table.rows[id + 1].cells[2].innerHTML;
                    document.getElementById("GvDuplicateWithExpiredReq_TxtNewDueDate_" + id).value = OldDueDate;
                    //document.getElementById("GvDuplicateWithExpiredReq_TxtNewDueDate_" + id).style.display = "none";
                    //document.getElementById("GvDuplicateWithExpiredReq_IcnCalendarNewDueDate_" + id).style.display = "none";
                    document.getElementById("GvDuplicateWithExpiredReq_TxtNewDueDate_" + id).disabled = true;
                    document.getElementById("GvDuplicateWithExpiredReq_IcnCalendarNewDueDate_" + id).disabled = true;
                    reversecheckfromitemToHeader('Reject');
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : DatePitcker(id)');
            }
        }

        function FocusToTxt(id, txtid) {
            try {
                (function ($) {
                    if (document.getElementById("GvDuplicateWithExpiredReq_TxtNewDueDate_" + id).disabled == false) {
                        DatePitckerAppr(id);
                        $('#' + txtid).focus();
                    }
                })(jQuery);
            }
            catch (err) {
                alert(err + ": FocusToTxt(iconid,txtid)")
            }
        }

        function ValidateDuplicateReqList() {
            try {
                var RowsCountGv = $('#GvDuplicateWithExpiredReq tr').length;
                var IsAllCheck = true;
                var IsValidNewResDueDate = true;
                var ReqNo = "";

                for (var i = 1; i < RowsCountGv; i++) {
                    if ($("#GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "") != null && $("#GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "") != null) {
                        if ($("#GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "").prop("checked") == false && $("#GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "").prop("checked") == false) {
                            IsAllCheck = false;
                            ReqNo = document.getElementById('GvDuplicateWithExpiredReq').rows[i].cells[1].innerHTML;
                            break;
                        }
                    }
                }

                if (IsAllCheck == true) {
                    for (var i = 1; i < RowsCountGv; i++) {
                        if ($("#GvDuplicateWithExpiredReq_TxtNewDueDate_" + (i - 1) + "") != null) {
                            if ($("#GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "") != null) {
                                if ($("#GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "").prop("checked") == true) {
                                    if (document.getElementById("GvDuplicateWithExpiredReq_TxtNewDueDate_" + (i - 1)).value == document.getElementById('GvDuplicateWithExpiredReq').rows[i].cells[2].innerHTML) {
                                        IsValidNewResDueDate = false;
                                        ReqNo = document.getElementById('GvDuplicateWithExpiredReq').rows[i].cells[1].innerHTML;
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
                alert(err + ": FocusToTxt(iconid,txtid)");
                return false;
            }
        }

        function ShowProgres() {
            document.getElementById("DvProgress").style.display = "block";
        }

        function HideProgres() {
            document.getElementById("DvProgress").style.display = "none";
        }

        function CheckAllRejOrChgDate(HeaderAction) {
            try {
                var RowsCountGv = $('#GvDuplicateWithExpiredReq tr').length;
                var w = 0;
                if (HeaderAction == "Reject") {
                    for (var i = 1; i < RowsCountGv; i++) {
                        if ($("#GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "") != null) {
                            document.getElementById("GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "").checked = true;
                            RbRejectExpReq(i - 1);
                        }
                    }
                }
                else {
                    for (var i = 1; i < RowsCountGv; i++) {
                        if ($("#GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "") != null) {
                            document.getElementById("GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "").checked = true;
                            RbChangedateResDueDate(i - 1);
                        }
                    }
                }
            }
            catch (err) {
                document.getElementById("DvProgress").style.display = "none";
                alert(err + ": CheckAllRejOrChgDate(HeaderAction)");
                return false;
            }
        }

        function reversecheckfromitemToHeader(Action) {
            try {
                var RowsCountGv = $('#GvDuplicateWithExpiredReq tr').length;
                var CountRealData = 0;
                var countChecked = 0;
                if (Action == "Reject") {
                    for (var i = 1; i < RowsCountGv; i++) {
                        if ($("#GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "") != null) {
                            CountRealData++;
                            if (document.getElementById("GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "").checked == true) {
                                countChecked++;
                            }
                        }
                    }
                    if (CountRealData == countChecked) {
                        document.getElementById("GvDuplicateWithExpiredReq_RbAllReject").checked = true;
                    }
                    else {
                        document.getElementById("GvDuplicateWithExpiredReq_RbAllReject").checked = false;
                        document.getElementById("GvDuplicateWithExpiredReq_RbAllchangeDate").checked = false;
                    }
                }
                else {
                    for (var i = 1; i < RowsCountGv; i++) {
                        if ($("#GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "") != null) {
                            CountRealData++;
                            if (document.getElementById("GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "").checked == true) {
                                countChecked++;
                            }
                        }
                    }
                    if (CountRealData == countChecked) {
                        document.getElementById("GvDuplicateWithExpiredReq_RbAllchangeDate").checked = true;
                    }
                    else {
                        document.getElementById("GvDuplicateWithExpiredReq_RbAllReject").checked = false;
                        document.getElementById("GvDuplicateWithExpiredReq_RbAllchangeDate").checked = false;
                    }
                }

            } catch (err) {
                alert(err + ": reversecheckfromitemToHeader(HeaderAction)");
                return false;
            }
        }
    </script>
    <%--script alert and extend session--%>
    <script type="text/javascript">
        $(function () {
            var timeout = 570000;
            $(document).bind("idle.idleTimer", function () {
                // function you want to fire when the user goes idle
                OpenModalSession();
                $("#StartTimer").click();
                //$.timeoutDialog({ timeout: 0.25, countdown: 15, logout_redirect_url: 'Login.aspx', restart_on_yes: true });
            });
            $(document).bind("active.idleTimer", function () {
                // function you want to fire when the user becomes active again
            });
            $.idleTimer(timeout);
        }); (jQuery)


        $(document).on('keypress', '#txtDate', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
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
        <asp:UpdatePanel runat="server" ID="UpsidebarToggle">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="col-md-12" style="padding: 5px;">
                        <div class="row">
                            <div class="col-md-10" style="padding-top: 5px;">
                                <a onclick="ShowLoading();" href="Home.aspx">
                                    <asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                                <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" ID="sidebarToggle" OnClick="sidebarToggle_Click"><i class="fas fa-bars"></i> 
                                </asp:LinkButton>
                                <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                                <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-md-2 fa-pull-right" style="background-color: #E9ECEF;">
                                <asp:Label ID="lbluser1" runat="server" Width="147px"></asp:Label><br />
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

            <div id="content-wrapper" style="background-color: white;">
                <div class="container-fluid">
                    <div class="row" style="padding-bottom: 10px;">
                        <div class="col-md-12">
                            <div class="col-md-12 card" style="padding: 10px; background-color: white;">
                                <div class="col-md-12 card-body Padding-Nol">
                                    <div class="col-md-12" style="background-color: white;">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                            <ContentTemplate>
                                                <div class="row" style="padding-top: 5px; padding-bottom: 5px;">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lbTitle" runat="server" Text="Mass Revision" Font-Bold="true" Font-Size="Large" />
                                                    </div>
                                                    <div class="col-md-6 text-right">
                                                        <asp:LinkButton runat="server" ID="BtnReset" CssClass="btn btn-warning btn-sm"
                                                            OnClick="BtnReset_Click">Reset</asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="BtnClose" CssClass="btn btn-danger btn-sm"
                                                            OnClick="BtnClose_Click">Close</asp:LinkButton>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="border-bottom: 2px solid #006EB7"></div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <div class="row" runat="server" id="DvFormContrl1" style="padding-top: 5px;">
                                                    <div class="col-md-2">
                                                        <asp:Label runat="server" ID="Label2" Text="Revision For"></asp:Label>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <asp:CheckBox runat="server" ID="ChcMatCost" Text="Material Cost" Enabled="false" Width="100px" Checked="true" />
                                                        <asp:CheckBox runat="server" ID="ChcProcCost" Text="Process Cost" Enabled="false" Width="100px" />
                                                        <asp:CheckBox runat="server" ID="ChcSubMat" Text="Sub Mat Cost" Enabled="false" Width="100px" />
                                                        <asp:CheckBox runat="server" ID="ChcOthMat" Text="Others Cost" Enabled="false" Width="100px" />
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <asp:FileUpload runat="server" ID="FlUpload" />
                                                    </div>
                                                    <div class="col-md-6 text-right">
                                                        <asp:LinkButton runat="server" ID="BtnUpload" CssClass="btn btn-primary btn-sm"
                                                            OnClientClick="if(valBeforeUpload()==false) return false;ShowLoading();"
                                                            OnClick="BtnUpload_Click"><span class="fa fa-upload"></span> Upload  </asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="BtnTemplate" CssClass="btn btn-success btn-sm"
                                                            OnClick="BtnTemplate_Click"><span class="fa fa-download"></span> Download Template</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="BtnUpload" />
                                                <asp:PostBackTrigger ControlID="BtnTemplate" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <%--All data from file upload--%>
                                                <div class="row" runat="server" id="DvAllDataUpload" visible="false">
                                                    <div class="col-md-12" style="padding-bottom: 0px;">
                                                        <div class="row" style="padding-bottom: 5px;">
                                                            <div class="col-md-8">
                                                                <asp:Label runat="server" ID="LbTitleAllData" Text="All Data" ForeColor="Black" Font-Bold="true"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <asp:Label runat="server" ID="Label13" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                                                <asp:TextBox runat="server" ID="TxtShowEntryAllData" Text="5" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                                    Width="50px" CssClass="fa-pull-right" Style="text-align: center"
                                                                    OnTextChanged="TxtShowEntryAllData_TextChanged" AutoPostBack="true">
                                                                </asp:TextBox>
                                                                <asp:Label runat="server" ID="Label14" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-12 table-sm table-responsive" style="padding: 0px;">
                                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="GdvAllData" runat="server" ShowHeaderWhenEmpty="true" Width="100%" BackColor="White"
                                                                        AllowPaging="True" PageSize="5" OnPageIndexChanging="GdvAllData_PageIndexChanging"
                                                                        AutoGenerateColumns="False" OnRowDataBound="GdvAllData_RowDataBound"
                                                                        AllowSorting="True" OnSorting="GdvAllData_Sorting" OnRowCreated="GdvAllData_RowCreated"
                                                                        CssClass="table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="No.">
                                                                                <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="PIRNo" HeaderText="PIR No" SortExpression="PIRNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialCode" HeaderText="Material Code" SortExpression="MaterialCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" SortExpression="VendorCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="ProcessGroup" HeaderText="Proc Group" SortExpression="ProcessGroup" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                        </Columns>
                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                        <PagerSettings PageButtonCount="10" />
                                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px; padding-top: 5px;" runat="server" id="DvProceed" visible="false">
                                                    <div class="col-md-10">
                                                    </div>
                                                    <div class="col-md-2 text-right">
                                                        <asp:Button runat="server" ID="BtnProceed" CssClass="btn btn-primary btn-sm btn-block"
                                                            Text="Proceed" OnClick="BtnProceed_Click" OnClientClick="ShowLoading();" />
                                                    </div>
                                                </div>

                                                <%--Invalid data from file upload--%>
                                                <div class="row" runat="server" id="DvInvalidData" visible="false">
                                                    <div class="col-md-12" style="padding-bottom: 0px;">
                                                        <div class="row" style="padding-bottom: 5px;">
                                                            <div class="col-md-8">
                                                                <asp:Label runat="server" ID="LbTitleInvalidData" Text="Invalid Data" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                                <asp:LinkButton runat="server" ID="BtnExportInvalidData" OnClientClick="ExportInvalidData();return false;" OnClick="BtnExportInvalidData_Click">
                                                        <span class="fa fa-file-excel" style="font-size:15px; color:darkgreen;"> Export </span>
                                                                </asp:LinkButton>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <asp:Label runat="server" ID="Label6" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                                                <asp:TextBox runat="server" ID="TxtShowEntryInvalid" Text="5" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                                    Width="50px" CssClass="fa-pull-right" Style="text-align: center"
                                                                    OnTextChanged="TxtShowEntryInvalid_TextChanged" AutoPostBack="true">
                                                                </asp:TextBox>
                                                                <asp:Label runat="server" ID="Label10" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-12 table-sm table-responsive" style="padding: 0px;">
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="GvInvalid" runat="server" ShowHeaderWhenEmpty="true" Width="100%" BackColor="White"
                                                                        AllowPaging="True" PageSize="5" OnPageIndexChanging="GvInvalid_PageIndexChanging"
                                                                        AutoGenerateColumns="False" OnRowDataBound="GvInvalid_RowDataBound"
                                                                        AllowSorting="True" OnSorting="GvInvalid_Sorting" OnRowCreated="GvInvalid_RowCreated"
                                                                        CssClass="table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="No.">
                                                                                <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="PIRNo" HeaderText="PIR No" SortExpression="PIRNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialCode" HeaderText="Material Code" SortExpression="MaterialCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" SortExpression="VendorCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="ProcessGroup" HeaderText="Proc Group" SortExpression="ProcessGroup" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <%--<asp:BoundField DataField="ProcDesc" HeaderText="Proc Group Desc" SortExpression="ProcDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                                            <asp:BoundField DataField="InvalidDesc" HeaderText="Invalid Desc" SortExpression="InvalidDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                        </Columns>
                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                        <PagerSettings PageButtonCount="10" />
                                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>

                                                <%--valid data from file upload--%>
                                                <div class="row" runat="server" id="DvValidData" visible="false">
                                                    <div class="col-md-12" style="padding-bottom: 0px;">
                                                        <div class="row" style="padding-bottom: 5px; padding-top: 10px;">
                                                            <div class="col-md-8">
                                                                <asp:Label runat="server" ID="LbTitleValidData" Text="Valid Data" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <asp:Label runat="server" ID="Label7" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                                                <asp:TextBox runat="server" ID="TxtShowEntryValid" Text="5" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                                    Width="50px" CssClass="fa-pull-right" Style="text-align: center"
                                                                    OnTextChanged="TxtShowEntryValid_TextChanged" AutoPostBack="true">
                                                                </asp:TextBox>
                                                                <asp:Label runat="server" ID="Label8" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-12 table-sm table-responsive" style="padding: 0px;">
                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="GvValidData" runat="server" ShowHeaderWhenEmpty="true" Width="100%" BackColor="White"
                                                                        AllowPaging="True" PageSize="5" OnPageIndexChanging="GvValidData_PageIndexChanging"
                                                                        AutoGenerateColumns="False" OnRowDataBound="GvValidData_RowDataBound"
                                                                        AllowSorting="True" OnSorting="GvValidData_Sorting" OnRowCreated="GvValidData_RowCreated"
                                                                        CssClass="table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="No.">
                                                                                <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="PIRNo" HeaderText="PIR No" SortExpression="PIRNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialCode" HeaderText="Material Code" SortExpression="MaterialCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" SortExpression="VendorCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="ProcessGroup" HeaderText="Proc Group" SortExpression="ProcessGroup" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="ProcDesc" HeaderText="Proc Group Desc" SortExpression="ProcDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                        </Columns>
                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                        <PagerSettings PageButtonCount="10" />
                                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 " runat="server" id="DvFormContrl2" style="padding-bottom: 5px; padding-top: 10px;">
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
                                                                        <asp:Label runat="server" ID="Label1" Text="Due Dt Next Rev"></asp:Label>
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
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px; padding-top: 5px;" runat="server" id="DvCreateReq" visible="false">
                                                    <div class="col-md-10">
                                                    </div>
                                                    <div class="col-md-2 text-right">
                                                        <asp:Button runat="server" ID="BtnCreateRequest" CssClass="btn btn-primary btn-sm btn-block"
                                                            OnClientClick="if(valCreateReq()==false) return false;ShowLoading();" Text="Create Request" OnClick="BtnCreateRequest_Click" />
                                                    </div>
                                                </div>

                                                <%--list invalid data request quote--%>
                                                <div class="row" runat="server" id="DvInvalidListRequest" visible="false">
                                                    <div class="col-md-12" style="padding-bottom: 5px;">
                                                        <div class="row">
                                                            <div class="col-md-8">
                                                                <asp:Label runat="server" ID="LbInvalidListRequest" Text="Invalid Request Total Record : 0" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                                <asp:LinkButton runat="server" ID="BtnExportInvalidListRequest" OnClientClick="ExportInvalidDataListReq();return false;" OnClick="BtnExportInvalidListRequest_Click">
                                                        <span class="fa fa-file-excel" style="font-size:15px; color:darkgreen;"> Export </span>
                                                                </asp:LinkButton>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <asp:Label runat="server" ID="Label15" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                                                <asp:TextBox runat="server" ID="TxtShowEntryInvalidReqList" Text="5" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                                    Width="50px" CssClass="fa-pull-right" Style="text-align: center"
                                                                    OnTextChanged="TxtShowEntryInvalidReqList_TextChanged" AutoPostBack="true">
                                                                </asp:TextBox>
                                                                <asp:Label runat="server" ID="Label16" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-lg-12 table-sm table-responsive" style="padding: 0px;">
                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="GdvInvalidRequest" runat="server" ShowHeaderWhenEmpty="true" Width="100%" BackColor="White"
                                                                        AllowPaging="True" PageSize="5" OnPageIndexChanging="GdvInvalidRequest_PageIndexChanging"
                                                                        AutoGenerateColumns="False" OnRowDataBound="GdvInvalidRequest_RowDataBound"
                                                                        AllowSorting="True" OnSorting="GdvInvalidRequest_Sorting" OnRowCreated="GdvInvalidRequest_RowCreated"
                                                                        CssClass="table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="No.">
                                                                                <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <%--<asp:BoundField DataField="PIRNo" HeaderText="PIR No" SortExpression="PIRNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                                                                    <asp:BoundField DataField="MaterialCode"  HeaderText="Material Code" SortExpression="MaterialCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                    <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                    <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" SortExpression="VendorCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                    <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                    <asp:BoundField DataField="ProcessGroup" HeaderText="Proc Group" SortExpression="ProcessGroup" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                    <asp:BoundField DataField="ProcDesc" HeaderText="Proc Group Desc" SortExpression="ProcDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                                            <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="PIRNo" HeaderText="PIR No" SortExpression="PIRNo" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialCode" HeaderText="Material Code" SortExpression="MaterialCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <%--<asp:BoundField DataField="CodeRef" HeaderText="Code Ref" SortExpression="CodeRef" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                                            <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" SortExpression="VendorCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="ProcessGroup" HeaderText="Proc Group" SortExpression="ProcessGroup" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <%--Gridview CompMaterial info--%>
                                                                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                                                <ItemTemplate>
                                                                                    <asp:Panel ID="pnlDet" runat="server" Style="display: block">
                                                                                        <asp:GridView ID="GvComMaterialInfoInvalid" runat="server" AutoGenerateColumns="false"
                                                                                            OnRowDataBound="GvComMaterialInfoInvalid_RowDataBound" Width="100%"
                                                                                            CssClass="table-hover Padding-Nol table-bordered" DataKeyNames="material">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="material" HeaderText="Raw Material Code" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="MaterialDesc" HeaderText="Raw Material Desc" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="OAmount" HeaderText="Amt SCur" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                                                <asp:BoundField DataField="Selling_Crcy" HeaderText="Selling Crcy" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="Amount" HeaderText="Amt VCur" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                                                <asp:BoundField DataField="Venor_Crcy" HeaderText="Venor Crcy" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="Unit" HeaderText="Unit" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="CusMatValFrom" HeaderText="Valid From" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="CusMatValTo" HeaderText="Valid To" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="ExchRate" HeaderText="Exch Rate" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                                                <asp:BoundField DataField="ValidFrom" HeaderText="Exch Rate Valid From" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="center" />
                                                                                            </Columns>
                                                                                            <HeaderStyle BackColor="#4d94ff" Font-Bold="True" ForeColor="White" />
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                        <PagerSettings PageButtonCount="10" />
                                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>

                                                <%--list data request quote--%>
                                                <div class="row" runat="server" id="DvGvReqList" visible="false">
                                                    <div class="col-md-12" style="padding-bottom: 5px; padding-top: 5px;">
                                                        <div class="row">
                                                            <div class="col-md-8">
                                                                <asp:Label runat="server" ID="LbQuoteMsg" Text="Quote Request can be Created only for below Vendors"
                                                                    Visible="false" ForeColor="DarkGreen" Font-Bold="true"></asp:Label>
                                                                <asp:Label runat="server" ID="LbReqlistTotRecord" Text="Total Record : 0" ForeColor="DarkGreen" Font-Bold="true"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <asp:Label runat="server" ID="Label9" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                                                <asp:TextBox runat="server" ID="TxtShowEntryReqList" Text="5" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                                    Width="50px" CssClass="fa-pull-right" Style="text-align: center"
                                                                    OnTextChanged="TxtShowEntryReqList_TextChanged" AutoPostBack="true">
                                                                </asp:TextBox>
                                                                <asp:Label runat="server" ID="Label11" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-lg-12 table-sm table-responsive" style="padding: 0px;">
                                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="GvReqList" runat="server" ShowHeaderWhenEmpty="true" Width="100%" BackColor="White"
                                                                        AllowPaging="True" PageSize="5" OnPageIndexChanging="GvReqList_PageIndexChanging"
                                                                        AutoGenerateColumns="False" OnRowDataBound="GvReqList_RowDataBound"
                                                                        AllowSorting="True" OnSorting="GvReqList_Sorting" OnRowCreated="GvReqList_RowCreated"
                                                                        CssClass="table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="No.">
                                                                                <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <%--<asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                                            <%--<asp:BoundField DataField="PIRNo" HeaderText="PIR No" SortExpression="PIRNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center" ></asp:BoundField>--%>
                                                                            <asp:BoundField DataField="ReqNo" HeaderText="Req No" SortExpression="ReqNo" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialCode" HeaderText="Material Code" SortExpression="MaterialCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="CodeRef" HeaderText="Code Ref" SortExpression="CodeRef" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" SortExpression="VendorCode" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <asp:BoundField DataField="ProcessGroup" HeaderText="Proc Group" SortExpression="ProcessGroup" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                            <%--Gridview CompMaterial info--%>
                                                                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                                                <ItemTemplate>
                                                                                    <asp:Panel ID="pnlDet" runat="server" Style="display: block">
                                                                                        <asp:GridView ID="GvComMaterialInfo" runat="server" AutoGenerateColumns="false"
                                                                                            OnRowDataBound="GvComMaterialInfo_RowDataBound" Width="100%"
                                                                                            CssClass="table-hover Padding-Nol table-bordered" DataKeyNames="material">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="material" HeaderText="Comp Material" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="MaterialDesc" HeaderText="Comp Material Desc" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="OAmount" HeaderText="Amt SCur" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                                                <asp:BoundField DataField="Selling_Crcy" HeaderText="Selling Crcy" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="Amount" HeaderText="Amt VCur" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                                                <asp:BoundField DataField="Venor_Crcy" HeaderText="Venor Crcy" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="Unit" HeaderText="Unit" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="CusMatValFrom" HeaderText="Valid From" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="CusMatValTo" HeaderText="Valid To" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                                                <asp:BoundField DataField="ExchRate" HeaderText="Exch Rate" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                                                <asp:BoundField DataField="ValidFrom" HeaderText="Exch Rate Valid From" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="center" />
                                                                                            </Columns>
                                                                                            <HeaderStyle BackColor="#4d94ff" Font-Bold="True" ForeColor="White" />
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                        <PagerSettings PageButtonCount="10" />
                                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px; padding-top: 10px;" runat="server" id="DvSubmit" visible="false">
                                                    <div class="col-md-10">
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Button runat="server" ID="BtnSubmit" CssClass="btn btn-primary btn-sm btn-block" OnClientClick="return ValidateSubmit();return false;"
                                                            Text="Submit" OnClick="BtnSubmit_Click" />
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

        <div style="display: none;">
            <asp:Button runat="server" ID="BExportInvalidData" OnClick="BtnExportInvalidData_Click" />
            <asp:Button runat="server" ID="BExportInvalidDataListRequest" OnClick="BtnExportInvalidListRequest_Click" />
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
            aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog" style="width: 95%; position: absolute; margin-top: 0px; top: 0px; margin-left: 2%;">
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel21" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row" style="background-color: #F7F7F7; padding-bottom: 10px; padding-left: 10px; padding-right: 10px; box-shadow: 1px 1px 1000px 1px; border-top-left-radius: 10px; border-top-right-radius: 10px; border-bottom-left-radius: 10px; border-bottom-right-radius: 10px;">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-lg-12 text-center" style="padding-top: 10px; padding-bottom: 10px;">
                                                    <!-- header -->
                                                </div>
                                            </div>

                                            <div class="row" style="padding-top: 10px; background-color: white;">
                                                <div class="col-xs-12" style="padding: 10px">
                                                    <asp:Label runat="server" ID="Label21" Text="Below Vendor with material code selected have old request pending and expired, 
                                                please select action for update the date or reject to closed old request before create new request." />
                                                </div>
                                            </div>

                                            <div class="row" style="padding-top: 10px; background-color: white; display: none;" id="DvProgress">
                                                <div class="col-md-12">
                                                    <div class="progress" style="margin-bottom: 0px !important;" id="ProgresCheckHeader">
                                                        <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="100"
                                                            aria-valuemin="0" aria-valuemax="100" style="width: 100%; padding: 0px!important;">
                                                            Please Waitt
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row" style="padding-top: 10px; background-color: white;">
                                                <div class="col-md-12">
                                                    <div class="table table-responsive table-sm">
                                                        <asp:GridView ID="GvDuplicateWithExpiredReq" runat="server" AutoGenerateColumns="False"
                                                            AllowPaging="false" PageSize="10" OnRowDataBound="GvDuplicateWithExpiredReq_RowDataBound"
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
                                                                <asp:TemplateField HeaderText="Reject" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <HeaderTemplate>
                                                                        <asp:RadioButton ID="RbAllReject" runat="server" Text=" &nbsp; Reject" GroupName="RbActionHeader" onchange="CheckAllRejOrChgDate('Reject');" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="RbReject" GroupName="RbAction" runat="server"></asp:RadioButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Change Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <HeaderTemplate>
                                                                        <asp:RadioButton ID="RbAllchangeDate" runat="server" Text=" &nbsp; Change Date" GroupName="RbActionHeader" onchange="CheckAllRejOrChgDate('Changedate')" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="RbChangeDate" GroupName="RbAction" runat="server"></asp:RadioButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="New Response Due Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <div class="group-main">
                                                                            <asp:TextBox ID="TxtNewDueDate" OnclientClick="return false;" runat="server" Text='<%# Eval("QuoteResponseDueDate") %>' Width="100%" Enabled="false"
                                                                                CssClass="TxtGrid" ToolTip="New Response Due Date" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black">
                                                                            </asp:TextBox>
                                                                            <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 2px 3px 2px 3px;">
                                                                                <span class="fa fa-calendar" style="color: #005496;" id="IcnCalendarNewDueDate" runat="server"></span>
                                                                            </span>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
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

                                            <div class="row pull-right" style="padding-top: 10px;">
                                                <asp:Button ID="BtnSubmitProcDuplicateReg" runat="server" Text="Submit" OnClientClick="if(ValidateDuplicateReqList()==false) return false;CloseModalDuplicateExpired();ShowLoading();" OnClick="BtnSubmitProcDuplicateReg_Click" CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="14px" />
                                                <asp:Button ID="BtnCancelMdDuplicate" runat="server" Text="Cancel" OnClientClick="CloseModalDuplicateExpired();return false;" autopostback="false" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="14px" />
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

        <!-- Modal Invalid Data Deatail-->
        <div class="modal fade modalextendseason-position" id="MdInvalidData" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="myModalLabecl" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog" style="width: 95%; position: absolute; margin-top: 0px; top: 0px; margin-left: 2%;">
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel14" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row" style="background-color: #F7F7F7; padding-bottom: 10px; padding-left: 10px; padding-right: 10px; box-shadow: 1px 1px 1000px 1px; border-top-left-radius: 10px; border-top-right-radius: 10px; border-bottom-left-radius: 10px; border-bottom-right-radius: 10px;">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-lg-12 text-center" style="padding-top: 10px; padding-bottom: 10px;">
                                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="LbTitileInvalid" runat="server" Text="Invalid Data Detail" Font-Size="Large"
                                                                Font-Bold="true"></asp:Label>
                                                            <asp:Button runat="server" ID="BtnCloseModal" Text="X" OnClientClick="CloseModalInvalidData();return false;"
                                                                CssClass="btn btn-sm btn-danger fa-pull-right" Font-Names="calibri" Font-Size="10px" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="col-lg-12" style="border-bottom: 2px solid #006EB7"></div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12" style="background-color: white">
                                                    <asp:Label runat="server" ID="LbMessage" Text="" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <!-- Modal session expired -->
        <div class="modal fade modalextendseason-position" id="myModalSession" data-backdrop="static" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;">
                                <div class="col-md-12 Padding-Nol" style="font: bold 22px calibri, calibri; text-align: center; align-content: center;">Your Session Is About To Expire !!  </div>
                                <h4></h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
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

    </form>
</body>
</html>
