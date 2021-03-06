<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewReq_changes.aspx.cs"
    Inherits="Material_Evaluation.NewReq_changes" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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

    <!-- Page level plugin CSS-->
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" />

    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet" />

    <link href="Styles/NewStyle/NewStyle.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link href="js/BootstrapDatePcr/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <style type="text/css">
        .SideBarMenu {
            width: 300px;
        }

        td {
            font-size: 14px;
            border: 1px solid #DDDDDD;
        }

        .btn-info {
            background-color: #008fb3 !important;
            border: 1px solid #008fb3;
        }

            .btn-info:hover {
                background-color: #007a99 !important;
                border: 1px solid #007a99;
            }

        .lbattachpad {
            padding: 2px;
        }

            .lbattachpad:hover {
                padding: 2px;
            }

        .WrapCnt td, th {
            white-space: normal !important;
            /*word-wrap: break-word;*/
            font-size: 14px !important;
        }

        .WrapCnt a {
            padding: 0px;
        }

        select[disabled] {
            background-color: #EBEBE4;
        }

        .blink {
          animation: blink-animation 1s steps(5, start) infinite;
          -webkit-animation: blink-animation 1s steps(5, start) infinite;
        }

        table input[type='text'][disabled] {
			background-color: #f6f6f6!important;
            color:black;
            font-weight:bolder;
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
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="Scripts/stickycolumandheaderplugin/tableHeadFixer.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/locales/bootstrap-datetimepicker.fr.js"></script>

    <%--Global Script--%>
    <script type="text/javascript">
        var CellscountGlobal = 0;

        $(document).ready(function () {
            $('#HdnFirsLoad').val("1");


            //if ($('#hdnRevOfEmet').val("1")) {
            //    if ($('#HdnNetProfnDisc').val() != "" ) {
            //        var a = $('#txtNetProfit\\(\\%\\)0');
            //        if (a != null) {
            //            $('#txtNetProfit\\(\\%\\)0').val($('#HdnNetProfnDisc').val());
            //        }
            //    }
            //}

            var a = $('#txtNetProfit\\(\\%\\)0').val();
            if (a != null) {
                if (a.toString().replace(" %", "") == "NaN") {
                    a = "0.0 %"
                    $('#txtNetProfit\\(\\%\\)0').val(a);
                }
            }

            freezeheader();
            DatePitcker();

            Layout7Condition();
            CekFollowBaseQtyMatcostCondition();

            RevisionOfEmetConditionForBOM();
            CekIsuseToolAmor();
            playAudio();

            var chkIsUseToolAmor = document.getElementById("GdvQuoreReqRevice_chkIsUseToolAmortize_0");
            var chkIsUseMacAmor = document.getElementById("GdvQuoreReqRevice_chkIsUseMachineAmortize_0");
            var chkAcsTabSubMatCost = document.getElementById("GdvQuoreReqRevice_chkSubMatRef_0");
            var TxtSubMatcostTot = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0");
            var TxtUnitSubMatcostTot = document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0");
            var QuoteRef = $("#hdnQuoteNoRef").val();

            if (QuoteRef != "") {
                submatlcost();
            }
            
            if (chkIsUseToolAmor != null || chkIsUseMacAmor != null || chkAcsTabSubMatCost != null) {
                if (QuoteRef != "" && (chkAcsTabSubMatCost.checked == true || chkIsUseToolAmor.checked == true || chkIsUseMacAmor.checked == true)) {
                    //submatlcost();
                    if (TxtSubMatcostTot != null && TxtUnitSubMatcostTot != null) {
                        TxtSubMatcostTot.value = $("#GdvQuoreReqRevice_chkSubMatRef_0").parent().text().trim();
                        TxtUnitSubMatcostTot.value = $("#GdvQuoreReqRevice_chkSubMatRef_0").parent().text().trim();
                    }
                }
            }

            if (QuoteRef != "" && (chkIsUseToolAmor.checked == true || chkIsUseMacAmor.checked == true)) {
                var txttotsubmatcosttable = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0");
                if (txttotsubmatcosttable != null) {
                    document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value = "";
                }
            }
        });

        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });

        $(document).on('keypress', '#TextBox1', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });

        $(document).on('keypress', '#txtfinal', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });

        $(document).on('keyup', '#txtProfit\\(\\%\\)0', function () {
            var trunval = $(this).val();
            var hdnVendorType = $("#hdnVendorType").val();

            if (trunval == '' || trunval == null) {
                $('#txtDiscount\\(\\%\\)0').prop("disabled", false);
                trunval = 0;
            }
            else {
                $('#txtDiscount\\(\\%\\)0').prop("disabled", true);
            }

            if (hdnVendorType == "TeamShimano") {
                CalculationFinalCost();
            }
            else {
                var gtc = document.getElementById("txtGrandTotalCost/pcs0").value;
                var ttlProfit = document.getElementById("txtProfit(%)0").value;
                var ttlFinalProfit = (gtc * ((100 + (+ttlProfit)) / 100));

                //add by celindo : hafiz
                if (ttlProfit.toString().length = 0) {
                    ttlProfit = 0
                }

                var TotMatCostFn = parseFloat(document.getElementById("txtTotalMaterialCost/pcs-0").value)
                var TotProCostFn = parseFloat(document.getElementById("txtFinalQuotePrice/pcs1").value)
                var TotSubMatCostFn = parseFloat(document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value)
                var TotOthrCostFn = parseFloat(document.getElementById("txtTotalOtherItemCost/pcs-0").value)
                var TotGtCostFn = parseFloat(document.getElementById("txtGrandTotalCost/pcs0").value)

                var TtlProcesCost = parseFloat(document.getElementById("txtTotalProcessesCost/pcs-0").value);
                var FinalVal = TtlProcesCost + (TtlProcesCost * (ttlProfit / 100));
                document.getElementById("txtFinalQuotePrice/pcs1").value = fixedDecml(FinalVal, 4);

                var Finalresult = TotMatCostFn + FinalVal + TotSubMatCostFn + TotOthrCostFn
                document.getElementById("txtFinalQuotePrice/pcs4").value = fixedDecml(Finalresult, 4);

                var GtValue = parseFloat(document.getElementById("txtGrandTotalCost/pcs0").value);
                var txtFinalValue = parseFloat(document.getElementById("txtFinalQuotePrice/pcs4").value);
                var NetProfDisc = (((txtFinalValue - GtValue) / txtFinalValue) * 100);

                var CheckValNetProfDisc = NetProfDisc.toString().toUpperCase();
                if (CheckValNetProfDisc == "NAN" || CheckValNetProfDisc == "INFINITY") {
                    NetProfDisc = 0;
                }
                document.getElementById("txtNetProfit(%)0").value = NetProfDisc.toFixed(1) + ' %';
                document.getElementById("HdnNetProfnDisc").value = NetProfDisc.toFixed(1);
                //

                var ttFinalCost = parseFloat(ttlFinalProfit.toFixed(5));

                //disabled by celindo : hafiz
                //document.getElementById("txtFinalQuotePrice/pcs0").value = ttFinalCost;

                $("#hdnTFinalQPrice").val(ttFinalCost);
                $("#hdnProfit").val(ttlProfit);
                $("#hdnDiscount").val(0);
            }
        });

        $(document).on('keyup', '#txtDiscount\\(\\%\\)0', function () {

            var trunval = $(this).val();
            if (trunval == '' || trunval == null) {
                $('#txtProfit\\(\\%\\)0').prop("disabled", false);
                trunval = 0;
            }
            else {
                $('#txtProfit\\(\\%\\)0').prop("disabled", true);
            }

            var gtcDisc = document.getElementById("txtGrandTotalCost/pcs0").value;
            var ttDisc = document.getElementById("txtDiscount(%)0").value;

            var TT = (100 + (-ttDisc));

            var TTDiv = TT / 100;


            var ttlFinaldiscount = (gtcDisc * TTDiv);

            var ttFinaldiscCost = fixedDecml(ttlFinaldiscount, 4).toString();

            //add by celindo : hafiz
            if (ttDisc.toString().length = 0) {
                ttDisc = 0
            }

            var TotMatCostFn = parseFloat(document.getElementById("txtTotalMaterialCost/pcs-0").value)
            var TotProCostFn = parseFloat(document.getElementById("txtFinalQuotePrice/pcs1").value)
            var TotSubMatCostFn = parseFloat(document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value)
            var TotOthrCostFn = parseFloat(document.getElementById("txtTotalOtherItemCost/pcs-0").value)
            var TotGtCostFn = parseFloat(document.getElementById("txtGrandTotalCost/pcs0").value)

            var TtlProcesCost = parseFloat(document.getElementById("txtTotalProcessesCost/pcs-0").value);
            var FinalVal = TtlProcesCost - (TtlProcesCost * (ttDisc / 100));
            document.getElementById("txtFinalQuotePrice/pcs1").value = fixedDecml(FinalVal, 4);

            var Finalresult = TotMatCostFn + FinalVal + TotSubMatCostFn + TotOthrCostFn
            document.getElementById("txtFinalQuotePrice/pcs4").value = fixedDecml(Finalresult, 4);

            var GtValue = parseFloat(document.getElementById("txtGrandTotalCost/pcs0").value);
            var txtFinalValue = parseFloat(document.getElementById("txtFinalQuotePrice/pcs4").value);
            var NetProfDisc = (((txtFinalValue - GtValue) / txtFinalValue) * 100);

            var CheckValNetProfDisc = NetProfDisc.toString().toUpperCase();
            if (CheckValNetProfDisc == "NAN" || CheckValNetProfDisc == "INFINITY") {
                NetProfDisc = 0;
            }
            document.getElementById("txtNetProfit(%)0").value = NetProfDisc.toFixed(1) + ' %';
            document.getElementById("HdnNetProfnDisc").value = NetProfDisc.toFixed(1);
            //
            //disabled by celindo - hafiz
            //document.getElementById("txtFinalQuotePrice/pcs0").value = ttFinaldiscCost;

            $("#hdnTFinalQPrice").val(ttFinaldiscCost);

            $("#hdnProfit").val(0);
            $("#hdnDiscount").val(ttDisc);

            //alert($("#hdnTFinalQPrice").val());
        });

        $(document).on('keyup', '#GA\\(\\%\\)0', function () {
            CalculationFinalCost();
        });

        function truncator(numToTruncate, intDecimalPlaces) {
            // alert(numToTruncate, intDecimalPlaces);
            var numPower = Math.pow(10, intDecimalPlaces); // "numPowerConverter" might be better
            // alert(numPower);
            return ~ ~(numToTruncate * numPower) / numPower;
        }

        function ShowLoading() {
            $('#loading').show();
        }

        function CloseLoading() {
            $('#loading').fadeOut("fast");
        }

        function openInNewTab(url) {
            var QuNoRef = document.getElementById('LblQuNoRef').innerHTML;
            var fullUrl = url + "?Number=" + QuNoRef.replace(": ", "");
            var win = window.open(fullUrl, '_blank');
            win.focus();
        }

        function openInNewTabDynamic(url, id) {

            var QuNoRef = document.getElementById(id).innerHTML;
            var fullUrl = url + "?Number=" + QuNoRef.replace(": ", "");
            var win = window.open(fullUrl, '_blank');
            win.focus();
        }

        function freezeheader() {
            try {

                (function ($) {
                    var Table1 = $("#Table1");
                    if (Table1 != null) {
                        $("#Table1").tableHeadFixer({ 'left': 1 });
                    }
                    var TablePC = $("#TablePC");
                    if (TablePC != null) {
                        $("#TablePC").tableHeadFixer({ 'left': 1 });
                    }
                    var TableSMC = $("#TableSMC");
                    if (TableSMC != null) {
                        $("#TableSMC").tableHeadFixer({ 'left': 1 });
                    }
                    var TableOthers = $("#TableOthers");
                    if (TableOthers != null) {
                        $("#TableOthers").tableHeadFixer({ 'left': 1 });
                    }
                    var TableUnit = $("#TableOthers");
                    if (TableUnit != null) {
                        $("#TableUnit").tableHeadFixer({ 'left': 1 });
                    }
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : freezeheader');
            }
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
    </script>

    <%--Layout7Condition()--%>
    <script type="text/javascript">
        function Layout7Condition() {
            try {
                if ($('#hdnLayoutScreen').val() == "Layout7") {
                    $('#DvProcessCostPart').hide();
                    $('#DvSubMatPart').hide();
                    if (document.getElementById("txtProfit(%)0") != null) {
                        document.getElementById("txtProfit(%)0").disabled = true;
                    }

                    if (document.getElementById("txtDiscount(%)0") != null) {
                        document.getElementById("txtDiscount(%)0").disabled = true;
                    }

                    if (document.getElementById('GA(%)0') != null) {
                        document.getElementById("GA(%)0").disabled = true;
                    }
                }
            } catch (err) {
                alert("Layout7Condition : " + err)
            }
        }
    </script>

    <%--RevisionOfEmetConditionForBOM--%>
    <script type="text/javascript">
        function RevisionOfEmetConditionForBOM() {
            try {
                var CellsCount = $("#Table1").find('tr')[0].cells.length;
                var varProcessGroup = $("#hdnLayoutScreen").val();
                var varhdnQuoteNoRef = $("#hdnQuoteNoRef").val();

                if (varhdnQuoteNoRef.toString() != "") {
                    if ((varProcessGroup != "Layout2") || (varProcessGroup != "Layout7")) {
                        var rawmaterialCode = "";
                        var rawmaterialDesc = "";
                        var rawmaterialCost = "";
                        var rawmaterialUOM = "";
                        var rawmaterialUOMTot = "";
                        var BOMData = document.getElementById("GvSMNBomEffctvDate");
                        if (BOMData != null) {
                            var rowcount = $('#GvSMNBomEffctvDate tr').length;
                            if (rowcount > 1) {
                                for (var i = 0; i < (rowcount - 1) ; i++) {
                                    rawmaterialCode = document.getElementById("GvSMNBomEffctvDate").rows[i + 1].cells[0].innerHTML;
                                    rawmaterialDesc = document.getElementById("GvSMNBomEffctvDate").rows[i + 1].cells[1].innerHTML;
                                    rawmaterialCost = document.getElementById("GvSMNBomEffctvDate").rows[i + 1].cells[4].innerHTML;
                                    rawmaterialUOM = document.getElementById("GvSMNBomEffctvDate").rows[i + 1].cells[7].innerHTML;
                                    if (rawmaterialUOM.toUpperCase() == "KG") {
                                        rawmaterialUOMTot = "G";
                                    }
                                    else {
                                        rawmaterialUOMTot = rawmaterialUOM;
                                    }
                                    rawmaterialCost = parseFloat(rawmaterialCost / 1000).toFixed(3);

                                    var txtMaterialSAPCode = document.getElementById("txtMaterialSAPCode" + i + "");
                                    var txtMaterialDescription = document.getElementById("txtMaterialDescription" + i + "");
                                    var txtRawMaterialCostkg = document.getElementById("txtRawMaterialCost/kg" + i + "");
                                    var txtRawMaterialUOM = document.getElementById("ddlRawMaterialCostUOM" + i + "");
                                    var txtRawMaterialUOMTot = document.getElementById("txtTotRawMaterialCostUOM" + i + "");
                                    var txtMaterialCostpcs = document.getElementById("txtMaterialCost/pcs" + i + "");
                                    var txtTotalMaterialCostpcs0 = document.getElementById("txtTotalMaterialCost/pcs0");

                                    if (txtMaterialSAPCode != null) {
                                        document.getElementById("txtMaterialSAPCode" + i + "").value = rawmaterialCode;
                                    }
                                    if (txtMaterialDescription != null) {
                                        document.getElementById("txtMaterialDescription" + i + "").value = rawmaterialDesc;
                                    }
                                    if (txtRawMaterialCostkg != null) {
                                        document.getElementById("txtRawMaterialCost/kg" + i + "").value = rawmaterialCost;
                                    }
                                    if (txtRawMaterialUOM != null) {
                                        document.getElementById("ddlRawMaterialCostUOM" + i + "").value = rawmaterialUOM;
                                    }
                                    if (txtRawMaterialUOMTot != null) {
                                        document.getElementById("txtTotRawMaterialCostUOM" + i + "").value = rawmaterialUOMTot;
                                    }

                                    if (document.getElementById("HdnAcsTabMatCost").value == "True") {
                                        if (txtMaterialCostpcs != null) {
                                            document.getElementById("txtMaterialCost/pcs" + i + "").value = "";
                                        }
                                        if (txtTotalMaterialCostpcs0 != null) {
                                            document.getElementById("txtTotalMaterialCost/pcs0").value = "";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (err) {
                alert("RevisionOfEmetConditionForBOM : " + err)
            }
        }
    </script>

    <%--script alert unred announcement --%>
    <script type="text/javascript">
        function playAudio() {
            if (document.getElementById('LiUnReadAnn').style.display == "block") {
                var x = document.getElementById("myAlertAudio");
                //x.loop = true;
                x.play();
            }
        }
    </script>

    <%--Amortize Script--%>
    <script type="text/javascript">
        function CekIsuseToolAmor(){
            try {
                var IsUseToolAmor = $("#HdnIsUseToolAmortize").val();
                if (IsUseToolAmor != null) {
                    if (IsUseToolAmor == "0" || IsUseToolAmor == "") {
                        document.getElementById("TxtIsUseToolAmor").value = "NO";
                        //document.getElementById("DvToolAmortize").style.display = "none";
                    }
                    else {
                        document.getElementById("TxtIsUseToolAmor").value = "YES";
                    }
                }
                else {
                    document.getElementById("TxtIsUseToolAmor").value = "NO";
                }
            } catch (err) {
                alert("CekValueInputByUser : " + err);
            }
        }

        function CekIshaveOldToolAmor() {
            try {
                var IshaveOldToolAmor = false;
                var table = $("#TableSMC");
                if (table != null) {
                    var CellsCount = $("#TableSMC").find('tr')[0].cells.length;
                    for (var i = 0; i < CellsCount - 1; i++) {
                        var e = document.getElementById("LbToolOrMachineAmor" + i);
                        if (e != null) {
                            if (e.innerHTML == "Old Tool Amortize") {
                                var btndel = document.getElementById("BtnDelSubMatCost" + i);
                                if (btndel != null) {
                                    IshaveOldToolAmor = true;
                                    btndel.click();
                                }
                            }
                        }
                    }

                    if (IshaveOldToolAmor == false) {
                        submatlcost();
                    }
                }
            } catch (e) {
                alert("CekIshaveOldToolAmor : " + e);
            }
        }
    </script>

    <%--ComntByVendorLght--%>
    <script type="text/javascript">
        function ComntByVendorLght() {
            try {
                var MaxLength = 150;
                var a = document.getElementById('TxtComntByVendor').value;

                $('#TxtComntByVendor').keypress(function (e) {
                    if ($(this).val().length >= MaxLength) {
                        e.preventDefault();
                    }
                });

                a = document.getElementById('TxtComntByVendor').value
                document.getElementById('LblengtVC').innerHTML = ' ' + (150 - a.length) + ' character left'

                if (a.length > 150) {
                    a = a.slice(0, 150);
                    document.getElementById('LblengtVC').innerHTML = ' ' + (150 - a.length) + ' character left';
                    $("#TxtComntByVendor").val(a);
                }
            } catch (err) {
                alert("ComntByVendorLght() : " + err);
            }
        }
    </script>

    <%--session script--%>
    <script type="text/javascript">
        <%--script alert and extend session--%>
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

    </script>

    <%--script modal UOM--%>
    <script type="text/javascript">
        function GenerateTaleUOM() {
            try {
                $('#DvUOM').html("");
                var CellsCount = $("#Table1").find('tr')[0].cells.length;
                var markup = "";
                var UOMList = [];
                var myddlUOM = document.getElementById('ddlRawMaterialCostUOM0');
                if (myddlUOM != null) {
                    for (var i = 0; i < myddlUOM.length; i++) {
                        UOMList.push(myddlUOM.options[i].value);
                    }
                }
                if (CellsCount > 0) {
                    var varProcessGroup = $("#hdnLayoutScreen").val();
                    markup = '<table id="TbUomMatCost" class="table table-bordered table-responsive" style="font-weight:lighter!important">' +
                                '<thead style="background-color:#1A2E4C; font-weight:lighter!important">' +
                                    '<th> <font color="white" style="font-weight:lighter!important">No.</font> </th>' +
                                    '<th><font color="white" style="font-weight:lighter!important">Raw Mat SAP Code</font> </th>' +
                                    '<th><font color="white" style="font-weight:lighter!important">Raw Mat Desc</font> </th>' +
                                    '<th><font color="white" style="font-weight:lighter!important">Raw Mat Cost</font> </th>';

                    if (varProcessGroup.toString().toUpperCase() != "LAYOUT7") {
                        markup += '<th><font color="white" style="font-weight:lighter!important">Ttl Raw Mat Cost</font> </th>';
                    }
                    
                    markup += ' <th><font color="white" style="font-weight:lighter!important">Raw Mat UOM</font> </th>' +
                             '</thead>'
                    for (var i = 0; i < CellsCount - 1; i++) {
                        var RawMatCode = "";
                        var RawMatDesc = "";
                        var RawMatCost = "";
                        var TotRawMatCost = "";

                        var TxtRawMatCode = document.getElementById("txtMaterialSAPCode" + i + "");
                        var TxtRawMatDesc = document.getElementById("txtMaterialDescription" + i + "");
                        var TxtRawMatCost = document.getElementById("txtRawMaterialCost/kg" + i + "");
                        var TotRawMatCost = document.getElementById("txtTotalRawMaterialCost/g" + i + "");

                        if (TxtRawMatCode != null) {
                            RawMatCode = TxtRawMatCode.value;
                        }
                        if (TxtRawMatDesc != null) {
                            RawMatDesc = TxtRawMatDesc.value;
                        }
                        if (TxtRawMatCost != null) {
                            RawMatCost = TxtRawMatCost.value;
                        }
                        if (TotRawMatCost != null) {
                            TotRawMatCost = TotRawMatCost.value;
                        }


                        markup += ' <tr> ' +
                                    '<td>' + (i + 1) + '</td>' +
                                    '<td>' + RawMatCode + '</td>' +
                                    '<td>' + RawMatDesc + '</td>' +
                                    '<td>' + RawMatCost + '</td>';
                        if (varProcessGroup.toString().toUpperCase() != "LAYOUT7") {
                            markup += '<td>' + TotRawMatCost + '</td>';
                        }

                        markup += '<td>' +
                                  '<select id="DdlModalUom_' + i + '">';
                        if (UOMList.length > 0) {
                            for (var u = 0; u < UOMList.length; u++) {
                                var Isselected = "";
                                var isDisabled = "";
                                if (varProcessGroup.toString().toUpperCase() != "LAYOUT7") {
                                    if (parseFloat(RawMatCost).toFixed(5) == parseFloat(TotRawMatCost).toFixed(5)) {
                                        if (UOMList[u].toString().toUpperCase() == "G") {
                                            isDisabled = "disabled"
                                        }
                                    }
                                    else {
                                        if (UOMList[u].toString().toUpperCase() == "G") {
                                            Isselected = "selected"
                                        }
                                        else {
                                            isDisabled = "disabled"
                                        }
                                    }
                                }
                                markup += '<option value="' + UOMList[u] + '" ' + Isselected + '  ' + isDisabled + '>' + UOMList[u] + '</option>';
                            }
                        }
                        markup += '</select>' +
                             '</td>' +
                             ' </tr> ';
                    }
                    markup += ' </table>'
                }

                $('#DvUOM').html(markup);
            }
            catch (err) {
                alert(err + ' : GenerateTaleUOM');
            }
        }

        function ValidateSubmitModalUOM() {
            var OK = false;
            try {
                var Tble = document.getElementById("TbUomMatCost");
                if (Tble != null) {
                    var totalRowCount = Tble.rows.length;
                    if (totalRowCount > 0) {
                        for (var i = 1; i < totalRowCount; i++) {
                            var ddlUOM = document.getElementById("DdlModalUom_" + (i - 1) + "");
                            if (ddlUOM != null) {
                                if (!ddlUOM.value.includes("SELECT UOM")) {
                                    OK = true;
                                }
                                else {
                                    alert("Please Select UOM for Row No : " + i);
                                    OK = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (err) {
                alert(err + ' : ValidateSubmitModalUOM');
            }
            return OK;
        }

        function SubmitModalUOM() {
            try {
                var Tble = document.getElementById("TbUomMatCost");
                if (Tble != null) {
                    var totalRowCount = Tble.rows.length;
                    if (totalRowCount > 0) {
                        for (var i = 1; i < totalRowCount; i++) {
                            var ddlUOM = document.getElementById("DdlModalUom_" + (i - 1) + "");
                            var ddlUOMTbMatcost = document.getElementById("ddlRawMaterialCostUOM" + (i - 1) + "");
                            if (ddlUOM != null) {
                                if (ddlUOMTbMatcost != null) {
                                    ddlUOMTbMatcost.value = ddlUOM.value;
                                }
                            }
                        }
                    }
                }
            }
            catch (err) {
                alert(err + ' : ValidateSubmitModalUOM');
            }
            CloseModalUOM();
        }

        function OpenModalUOM() {
            try {
                jQuery.noConflict();
                $("#MyModalUOM").modal({
                    backdrop: 'static',
                    keyboard: false
                });

                GenerateTaleUOM();
            }
            catch (err) {
                alert(err + ' : OpenModalUOM');
            }
        }

        function CloseModalUOM() {
            try {
                jQuery.noConflict();
                $("#MyModalUOM").modal('hide');
            }
            catch (err) {
                alert(err + ' : CloseModalUOM');
            }
        }
    </script>

    <%--material cost table script--%>
    <script type="text/javascript">
        function ClickAddMaterial() {
            document.getElementById('btnAddColumns').click();
        }

        function DelMatDetail(ColumnNo, TtlField) {
            var Cfrm = confirm("Are you sure you want to delete this item?");
            if (Cfrm == true) {
                MCDataStore();
                var Data = $("#hdnMCTableValues").val();
                var CN = ColumnNo;
                var TtlField = TtlField;
                var temp = new Array();
                temp = Data.split(",");

                var DataFinal = ""
                for (var i = 0; i < temp.length; i++) {
                    if (i >= ((CN * TtlField) - TtlField) && i < (CN * TtlField)) {

                    }
                    else {
                        DataFinal += temp[i] + ",";
                    }
                }

                $("#hdnMCTableValues").val(DataFinal);


                // raw material UOM
                var DataUOM = $("#hdnMCTableRawMatUom").val();
                var tempUOM = new Array();
                tempUOM = DataUOM.split(",");

                var DataFinalUOM = ""
                for (var i = 0; i < tempUOM.length; i++) {
                    if ((i + 1) == ColumnNo) {

                    }
                    else {
                        DataFinalUOM += tempUOM[i] + ",";
                    }
                }

                $("#hdnMCTableRawMatUom").val(DataFinalUOM);
                //

                document.getElementById('BtnDelMaterial').click();
            }
        }

        // Calculate Material Cost Table
        function MatlCalculation() {
            try {
                var CellsCount = $("#Table1").find('tr')[0].cells.length;
                var Validate = true;
                var varProcessGroup = $("#hdnLayoutScreen").val();

                for (var i = 0; i < CellsCount - 1; i++) {

                    if (varProcessGroup == "Layout2") {
                        document.getElementById("txtMaterialCost/pcs" + i).value = 0;
                    }
                    else if (varProcessGroup == "Layout7") {
                        var RawMaterialCost = document.getElementById("txtRawMaterialCost/kg" + i).value;
                        if (RawMaterialCost == '') {
                            RawMaterialCost = 0;
                        }
                        document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(RawMaterialCost).toFixed(3);
                        processcost();
                        submatlcost();
                    }
                    else {
                        if (Validate === false) {
                            $('#txtScrapRebate\\/pcs' + i).val("");
                            break;
                        }


                        var rawmaterial = document.getElementById("txtRawMaterialCost/kg" + i).value;
                        var BOMData = document.getElementById("GvSMNBomEffctvDate");
                        if (BOMData != null) {
                            var rowcount = $('#GvSMNBomEffctvDate tr').length;
                            if (rowcount > 1) {
                                if (i < (rowcount - 1)) {
                                    rawmaterial = document.getElementById("GvSMNBomEffctvDate").rows[i + 1].cells[4].innerHTML;
                                    document.getElementById("txtRawMaterialCost/kg" + i).value = parseFloat(rawmaterial / 1000).toFixed(3);
                                    rawmaterial = document.getElementById("txtRawMaterialCost/kg" + i).value;

                                }
                            }
                        }
                        else {
                            rawmaterial = document.getElementById("txtRawMaterialCost/kg" + i).value;
                        }

                        if (rawmaterial == "") {
                            rawmaterial = 0;
                        }
                        if (isNaN(rawmaterial) == true) {
                            rawmaterial = 0;
                        }
                        var raw = parseFloat(rawmaterial).toFixed(6);
                        var rawmatl_Grams = raw / 1000;

                        var totrawmatl_Grams = parseFloat(rawmatl_Grams).toFixed(6);

                        var TxtRawMatCost = document.getElementById("txtRawMaterialCost/kg" + i);
                        var TxtTotRawMatCost = document.getElementById("txtTotalRawMaterialCost/g" + i);
                        var TxtRawMatUOM = document.getElementById("ddlRawMaterialCostUOM" + i);

                        if (TxtRawMatUOM != null && TxtRawMatCost != null && TxtTotRawMatCost != null) {

                            var RawMatCost = TxtRawMatCost.value;
                            var RawMatUOM = TxtRawMatUOM.value;
                            if (RawMatCost.toString() != "") {
                                if (RawMatUOM.toString() != "") {
                                    if (RawMatUOM.toString().toUpperCase() == "KG") {
                                        document.getElementById("txtTotalRawMaterialCost/g" + i).value = totrawmatl_Grams;
                                    }
                                    else {
                                        document.getElementById("txtTotalRawMaterialCost/g" + i).value = RawMatCost;
                                    }
                                }
                            }
                        }

                        var partunit = document.getElementById("txtPartNetUnitWeight(g)0").value;
                        var cavity = document.getElementById("txtCavity0").value;
                        var matlyeildpercent = document.getElementById("txtMaterialYield/MeltingLoss(%)0").value;
                        if (varProcessGroup == "Layout5")  // Layout5 - ST
                        {
                            partunit = document.getElementById("txtPartNetUnitWeight(g)" + i).value;
                            cavity = document.getElementById("txtCavity" + i).value;
                            matlyeildpercent = document.getElementById("txtMaterialYield/MeltingLoss(%)" + i).value;
                        }

                        var tot = (partunit * cavity);
                        var recycleratio = 0;
                        var runnerweight_Percent = 0;

                        if (varProcessGroup == "Layout1") {  // Layout1 - IM

                            var runnerweight = document.getElementById("txt~RunnerWeight/shot(g)0").value;

                            if (isNaN(runnerweight) == true || runnerweight == "undefined" || runnerweight == "" || runnerweight == null) {

                                tot = (partunit * cavity);

                                var virginmatl = tot

                            }
                            else {

                                //var runnerweight_Percent = ((runnerweight / (partunit * cavity)) * 100);
                                runnerweight_Percent = runnerweight / (parseFloat((partunit * cavity)) + parseFloat(runnerweight)) * 100;
                                if (runnerweight_Percent.toString() == 'NaN') {
                                    runnerweight_Percent = 0;
                                }

                                tot = (partunit * cavity);

                                recycleratio = document.getElementById("txt~RecycleMaterialRatio(%)0").value;
                                // alert(runnerweight_Percent);
                                //var only4digit = runnerweight_Percent.toString().slice(0, 4);
                                //alert(only4digit);
                                document.getElementById("txt~RunnerRatio/pcs(%)0").value = parseInt(runnerweight_Percent);
                            }
                        }

                        if (varProcessGroup == "Layout3") { // Layout3 - CA
                            var CStotshotweight = Number(tot);
                            //alert("CStotshotweight");
                            //alert(CStotshotweight);
                            var CSvirginmatl = CStotshotweight;
                            // var CSgross_shot_weight = (CSvirginmatl * (100 + Number(matlyeildpercent)) / 100);
                            var CSgross_shot_weight = (CSvirginmatl * (100 + Number(matlyeildpercent)) / 100);
                            //alert("CSgross_shot_weight");
                            //alert(CSgross_shot_weight);
                            // var CSGross_shottemptest = (CSvirginmatl * (CSvirginmatl + Number(matlyeildpercent)) / 100);  // As per excel - For CA 1 + matlyeildpercent

                            var CSgross = truncator(CSgross_shot_weight, 4).toString();
                            // var CSgross_weight = (CSgross / cavity); // As per Excel do not 
                            var CSgross_weight = (CSgross / cavity);
                            //alert("CSgross_weight");
                            //alert(CSgross_weight);
                            var CSMaterialCost = CSgross_weight * totrawmatl_Grams;

                            var CheckValCSgross_weight = CSgross_weight.toString().toUpperCase();
                            var CheckValCSMaterialCost = CSMaterialCost.toString().toUpperCase();
                            if (CheckValCSgross_weight == "NAN" || CheckValCSgross_weight == "INFINITY") {
                                CSgross_weight = 0;
                            }
                            if (CheckValCSMaterialCost == "NAN" || CheckValCSMaterialCost == "INFINITY") {
                                CSMaterialCost = 0;
                            }
                            document.getElementById("txtMaterialGrossWeight/pc(g)0").value = parseFloat(CSgross_weight).toFixed(6);
                            document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(CSMaterialCost).toFixed(6);


                        }

                        else if (varProcessGroup == "Layout1") {// Layout1 - IM
                            var totshotweight = (Number(tot) + Number(runnerweight));
                            var virginmatl = 0;
                            
                            var TxtMainRecycleRatio = document.getElementById("TxtImRcylRatio");
                            if (TxtMainRecycleRatio != null) {
                                var MainRecycleRatio = TxtMainRecycleRatio.value;
                                if (MainRecycleRatio.toString().toUpperCase() == "FOLLOW RUNNER RATIO") {
                                    recycleratio = parseFloat(runnerweight_Percent).toFixed(4);
                                }
                            }

                            if (recycleratio == 0) {
                                virginmatl = totshotweight;
                            }
                            else if (recycleratio > 0 && recycleratio < 100) {
                                virginmatl = (totshotweight * ((100 - recycleratio) / 100));
                            }
                            else {
                                virginmatl = tot;
                            }
                            
                            var gross_shot_weight = (virginmatl * (100 + Number(matlyeildpercent)) / 100);
                            var gross = truncator(gross_shot_weight, 4).toString();
                            var gross_weight = (gross / cavity);

                            var CheckValgross_weight = gross_weight.toString().toUpperCase();
                            if (CheckValgross_weight == "NAN" || CheckValgross_weight == "INFINITY") {
                                gross_weight = 0;
                            }
                            document.getElementById("txtMaterialGrossWeight/pc(g)0").value = gross_weight;
                            var matl_Cost = (gross_weight * totrawmatl_Grams);
                            document.getElementById("txtMaterialGrossWeight/pc(g)0").value = parseFloat(gross_weight).toFixed(6);
                            document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(matl_Cost).toFixed(6);
                        }

                        else if (varProcessGroup == "Layout5") { // Layout5 - ST
                            // alert("IN");
                            var width = document.getElementById("txt~~Width(mm)" + i).value;
                            var thicknes = document.getElementById("txt~~Thickness(mm)" + i).value;
                            var pitch = document.getElementById("txt~~Pitch(mm)" + i).value;
                            var density = document.getElementById("txt~MaterialDensity" + i).value;


                            var scraploss = document.getElementById("txtScrapLossAllowance(%)" + i).value;
                            var scrapprice = document.getElementById("txtScrapPrice/kg" + i).value;

                            var cal1 = ((width * thicknes * pitch) / cavity);
                            var cal11 = parseFloat(cal1).toFixed(6);
                            var cal2 = (density / 1000);
                            var cal3 = ((100 + Number(matlyeildpercent)) / 100);
                            var cal33 = fixedDecml(cal3, 4).toString();
                            var gwFixed = (cal11 * cal2 * cal33);
                            var gros_weight = parseFloat(gwFixed).toFixed(6);

                            var CheckValgros_weight = gros_weight.toString().toUpperCase();
                            if (CheckValgros_weight == "NAN" || CheckValgros_weight == "INFINITY") {
                                gros_weight = 0;
                            }

                            document.getElementById("txtMaterialGrossWeight/pc(g)" + i).value = gros_weight;
                            var scrapweight = parseFloat(gros_weight) - parseFloat(partunit);
                            document.getElementById("txtMaterialScrapWeight(g)" + i).value = parseFloat(scrapweight).toFixed(4);

                            //var scrap = fixedDecml(((100 - Number(scraploss)) / 100), 4).toString();
                            var scrap = parseFloat(((100 - Number(scraploss)) / 100).toFixed(6));

                            //var scrabweightpc = fixedDecml((scrapweight * scrap), 4).toString();
                            var scrabweightpc = parseFloat((scrapweight * scrap).toFixed(6));

                            var scraprebate = parseFloat((scrabweightpc * (scrapprice / 1000))).toFixed(6);
                            //var scraprebate = parseFloat((scrabweightpc * (scrapprice / 1000)).toFixed(6));

                            document.getElementById("txtScrapRebate/pcs" + i).value = scraprebate;

                            //var matlcost1 = fixedDecml((gros_weight * rawmatl_Grams),4).toString();
                            var matlcost1 = parseFloat((gros_weight * rawmatl_Grams).toFixed(6));

                            //var matlcost2 = fixedDecml((gros_weight * rawmatl_Grams) + (-scraprebate), 4).toString();
                            //var matlcost2 = parseFloat((gros_weight * rawmatl_Grams) + (-scraprebate).toFixed(6));
                            var matlcost2 = (gros_weight * rawmatl_Grams) - (scraprebate);

                            document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(matlcost2).toFixed(6);
                        }

                        else if (varProcessGroup == "Layout4" || varProcessGroup == "Layout6") {  // Layout4 - MS , Layout6 - SPR

                            //subash - new formula
                            var CStotshotweight = Number(tot);
                            var CSvirginmatl = CStotshotweight;
                            var CSgross_shot_weight = (CSvirginmatl * (100 + Number(matlyeildpercent)) / 100);
                            var CSgross = truncator(CSgross_shot_weight, 4).toString();
                            var CSgross_weight = (CSgross / cavity);

                            var CheckValCSgross_weight = CSgross_weight.toString().toUpperCase();
                            if (CheckValCSgross_weight == "NAN" || CheckValCSgross_weight == "INFINITY") {
                                CSgross_weight = 0;
                            }
                            document.getElementById("txtMaterialGrossWeight/pc(g)0").value = parseFloat(CSgross_weight).toFixed(6);

                            var CSMaterialCost = CSgross_weight * totrawmatl_Grams;
                            document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(CSMaterialCost).toFixed(6);
                            //end subash

                            //alert('layout6');
                            //var CStotshotweight = Number(tot);
                            //var CSvirginmatl = CStotshotweight;
                            //var CSgross_shot_weight = (CSvirginmatl * (100 + Number(matlyeildpercent)) / 100); /// Only for CA  and that also should come 1 . not 100 +

                            //var CSGross_shot_weight_MS = (CSvirginmatl * (CSvirginmatl + Number(matlyeildpercent)) / 100);

                            ////var CSgross = truncator(CSGross_shot_weight_MS, 4).toString();
                            //var CSgross_weight = (CSGross_shot_weight_MS);

                            //var CSMaterialCost = CSgross_weight * totrawmatl_Grams;

                            ////var CSMaterial_GrosWeight = truncator(CSgross_weight, 4).toString();
                            //var CSMaterial_GrosWeight = parseFloat(CSgross_weight.toFixed(6));
                            ////50002077
                            ////alert("CSMaterial_GrosWeight");
                            //document.getElementById("txtMaterialGrossWeight/pc(g)0").value = CSMaterial_GrosWeight;


                            //// common


                            ////var matlcost_PC = fixedDecml(CSMaterialCost, 4).toString();

                            //document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(CSMaterialCost.toFixed(6));

                        }

                    }
                }

                if (Validate == true) {
                    var InputId = "";
                    var r = 0;
                    var CellsCount = $("#Table1").find('tr')[0].cells.length;
                    var table = document.getElementById('Table1');

                    $('#Table1 tr').each(function () {
                        if (r > 0) {
                            for (var c = 1; c < CellsCount; c++) {
                                InputId += $(table.rows[r].cells[c]).find("input").attr("id") + ",";
                            }
                        }
                        r++;
                    });
                    var ArrInputId = InputId.split(',');
                    for (var t = 0 ; t < (ArrInputId.length - 1) ; t++) {
                        var id = ArrInputId[t].toString();
                        if (id !== undefined) {
                            if (id.toString() !== "undefined") {
                                var idVal = document.getElementById(id).value;
                                if (idVal.toString() == "") {
                                    idVal = 0;
                                }
                                if (id.includes('(g)') || id.includes('/g')) {
                                    document.getElementById(id).value = parseFloat(idVal).toFixed(4);
                                }
                                else if (id.includes('/kg')) {
                                    document.getElementById(id).value = parseFloat(idVal).toFixed(3);
                                }
                            }
                        }
                    }

                    var totalmatlcal = 0;
                    for (var i = 0; i < CellsCount - 1; i++) {
                        var ttlmtlcost = document.getElementById("txtMaterialCost/pcs" + i).value;

                        totalmatlcal = ((+totalmatlcal) + (+ttlmtlcost));

                    }
                    //var newtotalmtl = truncator(totalmatlcal, 4).toString();
                    var CheckValTot = totalmatlcal.toString().toUpperCase();
                    if (CheckValTot == "NAN" || CheckValTot == "INFINITY") {
                        totalmatlcal = 0;
                    }
                    document.getElementById("txtTotalMaterialCost/pcs0").value = parseFloat(totalmatlcal).toFixed(5);
                    document.getElementById("txtTotalMaterialCost/pcs-0").value = parseFloat(totalmatlcal).toFixed(5);
                    MCDataStore();

                    ReCalculate();
                    //*
                }
                //return false;
            } catch (err) {
                alert("MatlCalculation() : " + err);
            }

        }

        //this script used for page :Review req mass, new request change mass, new request changes, new request change mass
        function SetTotaRawMatCostAndUOM(Id) {
            try {
                var TxtRawMatCost = document.getElementById("txtRawMaterialCost/kg" + Id);
                var TxtTotRawMatCost = document.getElementById("txtTotalRawMaterialCost/g" + Id);
                var TxtRawMatUOM = document.getElementById("ddlRawMaterialCostUOM" + Id);

                if (TxtRawMatUOM != null && TxtRawMatCost != null && TxtTotRawMatCost != null) {
                    var RawMatUOM = TxtRawMatUOM.value;
                    if (document.getElementById("txtTotRawMaterialCostUOM" + Id) != null) {
                        if (RawMatUOM.toString().toUpperCase() == "KG") {
                            document.getElementById("txtTotRawMaterialCostUOM" + Id).value = "G";
                        }
                        else {
                            document.getElementById("txtTotRawMaterialCostUOM" + Id).value = RawMatUOM;
                        }
                    }


                    if (RawMatUOM == "--SELECT UOM--" || RawMatUOM == "" || RawMatUOM.value == "UOM NOT EXIST") {
                        document.getElementById("txtTotalMaterialCost/pcs0").value = "";
                        document.getElementById("txtTotalRawMaterialCost/g" + Id).value = "";
                        document.getElementById("txtTotRawMaterialCostUOM" + Id).value = "";
                    }
                    else {
                        var RawMatCost = TxtRawMatCost.value;
                        if (RawMatCost.toString() != "") {
                            if (RawMatUOM.toString() != "") {
                                if (RawMatUOM.toString().toUpperCase() == "KG") {
                                    document.getElementById("txtTotalRawMaterialCost/g" + Id).value = (parseFloat(RawMatCost) / 1000).toFixed(4);
                                }
                                else {
                                    document.getElementById("txtTotalRawMaterialCost/g" + Id).value = RawMatCost;
                                }
                            }
                        }
                    }
                }
            } catch (err) {
                alert("SetTotaRawMatCostAndUOM : " + err);
            }
        }

        //Session Maintain for Material Cost Table //this script used for page :Review req mass, new request change mass, new request changes, new request change mass
        function MCDataStore() {
            try {
                //  alert("IN");

                var hdnvaltemp = "";
                var hdnvaltempRawMatUom = "";
                var hdnvaltempST = "";

                var CellsCount = $("#Table1").find('tr')[0].cells.length;
                var varProcessGroup = $("#hdnLayoutScreen").val();

                for (var i = 0; i < CellsCount - 1; i++) {
                    var txtRawMatCostUOM = document.getElementById("ddlRawMaterialCostUOM" + i);
                    var RawMaterialUOM = "";
                    if (txtRawMatCostUOM != null) {
                        RawMaterialUOM = txtRawMatCostUOM.value;
                    }

                    if (varProcessGroup == "Layout2") {
                        var txtSapcode = document.getElementById("txtMaterialSAPCode" + i).value;
                        var cavity = document.getElementById("txtCavity" + 0).value;
                        var partunit = document.getElementById("txtPartNetUnitWeight(g)" + 0).value;
                        var txtMcost = document.getElementById("txtMaterialCost/pcs" + i).value;
                        var txtTMcost = document.getElementById("txtTotalMaterialCost/pcs0").value;

                        hdnvaltemp += (+ "," + txtSapcode + "," + partunit + "," + cavity + "," + txtMcost + "," + txtTMcost + ",");
                    }
                    else if (varProcessGroup == "Layout7") {
                        var txtSapcode = document.getElementById("txtMaterialSAPCode" + i).value;
                        var txtMaterialDescription = document.getElementById("txtMaterialDescription" + i).value;
                        var txtRawMaterialCostKG = document.getElementById("txtRawMaterialCost/kg" + i).value;
                        var txtMcost = document.getElementById("txtMaterialCost/pcs" + i).value;
                        var txtTMcost = document.getElementById("txtTotalMaterialCost/pcs0").value;
                        hdnvaltemp += (+ "," + txtSapcode + "," + txtMaterialDescription + "," + txtRawMaterialCostKG + "," + txtMcost + "," + txtTMcost + ",");
                    }
                    else {
                        var txtSapcode = document.getElementById("txtMaterialSAPCode" + i).value;
                        var txtSapDesc = document.getElementById("txtMaterialDescription" + i).value;
                        var txtrawmaterial = document.getElementById("txtRawMaterialCost/kg" + i).value;

                        var txttotalRawMaterial = document.getElementById("txtTotalRawMaterialCost/g" + i).value;

                        if (varProcessGroup == "Layout5") //stamping- ST
                        {
                            var partunit = document.getElementById("txtPartNetUnitWeight(g)" + i).value;
                            var cavity = document.getElementById("txtCavity" + i).value;
                            var matlyeildpercent = document.getElementById("txtMaterialYield/MeltingLoss(%)" + i).value;
                            var txtSTmgweight = document.getElementById("txtMaterialGrossWeight/pc(g)" + i).value;
                        }
                        else {
                            var partunit = document.getElementById("txtPartNetUnitWeight(g)0").value;
                            var cavity = document.getElementById("txtCavity0").value;
                            var matlyeildpercent = document.getElementById("txtMaterialYield/MeltingLoss(%)0").value;
                            var txtSTmgweight = document.getElementById("txtMaterialGrossWeight/pc(g)0").value;
                        }

                        var txtMcost = document.getElementById("txtMaterialCost/pcs" + i).value;
                        var txtTMcost = document.getElementById("txtTotalMaterialCost/pcs0").value;

                        // alert(varProcessGroup);
                        // var varProcessGroup = $("#txtprocs").val();

                        //hdnvaltemp += (+ "," + txtSapcode + "," + txtSapDesc + "," + txtrawmaterial + "," + txttotalRawMaterial + "," + partunit + ",");
                        //implement raw material UOM 
                        hdnvaltemp += (+ "," + txtSapcode + "," + txtSapDesc + "," + txtrawmaterial + "," + txttotalRawMaterial + "," + partunit + ",");

                        //alert(varProcessGroup);
                        if (varProcessGroup == "Layout1") { // Layout1 - IM

                            var runnerweight = document.getElementById("txt~RunnerWeight/shot(g)0").value;
                            var txtRunnerRatio = document.getElementById("txt~RunnerRatio/pcs(%)0").value;
                            var recycleratio = document.getElementById("txt~RecycleMaterialRatio(%)0").value;

                            hdnvaltemp += (cavity + "," + runnerweight + "," + txtRunnerRatio + "," + recycleratio + ",");
                            hdnvaltemp += (matlyeildpercent + "," + txtSTmgweight + "," + txtMcost + "," + txtTMcost + ",");
                        }

                        else if (varProcessGroup == "Layout5") { // Layout5 - ST
                            var thicknes = document.getElementById("txt~~Thickness(mm)" + i).value;
                            var width = document.getElementById("txt~~Width(mm)" + i).value;

                            var pitch = document.getElementById("txt~~Pitch(mm)" + i).value;
                            var density = document.getElementById("txt~MaterialDensity" + i).value;

                            var scrapweight = document.getElementById("txtMaterialScrapWeight(g)" + i).value;
                            var scraploss = document.getElementById("txtScrapLossAllowance(%)" + i).value;
                            var scrapprice = document.getElementById("txtScrapPrice/kg" + i).value;
                            var txtScraprepate = document.getElementById("txtScrapRebate/pcs" + i).value;

                            hdnvaltemp += (thicknes + "," + width + "," + pitch + "," + density + "," + cavity + "," + matlyeildpercent + "," + txtSTmgweight + "," + scrapweight + "," + scraploss + "," + scrapprice + "," + txtScraprepate + "," + txtMcost + "," + txtTMcost + ",");
                            // alert(hdnvaltemp);
                        }

                        else if (varProcessGroup == "Layout4") { // Layout4 - MS
                            //var diameterId = document.getElementById("txt~~DiameterID(mm)" + i).value;
                            //var diameterOD = document.getElementById("txt~~DiameterOD(mm)" + i).value;
                            //var widthMS = document.getElementById("txt~~Width(mm)" + i).value;
                            //hdnvaltemp += (diameterId + "," + diameterOD + "," + widthMS + ",");
                        }

                        if (varProcessGroup != "Layout5" && varProcessGroup != "Layout1") {
                            hdnvaltemp += (cavity + "," + matlyeildpercent + "," + txtSTmgweight + "," + txtMcost + "," + txtTMcost + ",");
                        }
                    }

                    if (varProcessGroup != "Layout2") {
                        hdnvaltempRawMatUom += (RawMaterialUOM + ",");
                    }

                    $("#hdnMCTableCount").val(i);
                }

                // alert(hdnvaltemp);
                $("#hdnMCTableValues").val(hdnvaltemp);
                $("#hdnMCTableRawMatUom").val(hdnvaltempRawMatUom);
                //return false;
            } catch (err) {
                alert("MCDataStore() : " + err);
            }
        }
    </script>

    <%--process group table script--%>
    <script type="text/javascript">
        function ClickAddProces() {
            document.getElementById('btnaddProcessCost').click();
        }

        function DelProces(ColumnNo, TtlField) {
            var Cfrm = confirm("Are you sure you want to delete this item?");
            if (Cfrm == true) {
                ProcessCostDataStore();
                var Data = $("#hdnProcessValues").val();
                var CN = ColumnNo;
                var TtlField = TtlField;
                var temp = new Array();
                temp = Data.split(",");

                var DataFinal = ""
                for (var i = 0; i < temp.length; i++) {
                    if (i >= ((CN * TtlField) - TtlField) && i < (CN * TtlField)) {

                    }
                    else {
                        DataFinal += temp[i] + ",";
                    }
                }

                $("#hdnProcessValues").val(DataFinal);
                document.getElementById('BtnDelProcess').click();
            }
        }

        // Calculate Process Cost Table
        function processcost() {
            try {

                var CellsCount = $("#TablePC").find('tr')[0].cells.length;
                var CellsCountMatCost = $("#Table1").find('tr')[0].cells.length;
                var varProcessGroup = $("#hdnLayoutScreen").val();
                var Validate = true;
                //  alert(CellsCount);
                var Hr_Rate;
                if (varProcessGroup != "Layout7") {
                    for (var i = 0; i < CellsCount - 1; i++) {
                        if (Validate === false) {
                            $('#txtProcessCost\\/pc' + i).val("");
                            $('#txtTotalProcessesCost\\/pcs0').val("");
                            break;
                        }

                        var PrGroup = $('#dynamicddlProcess' + i + ' option:selected').text();
                        var SubPrGroup = $('#dynamicddlSubProcess' + i + ' option:selected').text();
                        //if (PrGroup.trim() == '--Select--') {
                        //    Validate = false;
                        //    alert('please Select Process Group at column ' + (i + 1) + ' !');
                        //    break;
                        //}

                        if (PrGroup.trim() != '--Select--') {
                            if (SubPrGroup.trim() == '--Select--') {
                                Validate = false;
                                alert('please Select Sub process at column ' + (i + 1) + ' !');
                                break;
                            }
                        }

                        var Hr_Rate;
                        var VdNameIfTurnKey = document.getElementById("txtIfTurnkey-VendorName" + i).value;

                        if (VdNameIfTurnKey != '') {
                        }
                        else {
                            var stdhrdate = document.getElementById("txtStandardRate/HR" + i).value;
                            var vendrate = document.getElementById("txtVendorRate" + i).value;
                            if (stdhrdate == vendrate) {
                                Hr_Rate = stdhrdate;

                            }
                            else {
                                Hr_Rate = vendrate;
                            }
                            var cavity = document.getElementById("txtCavity0").value;
                            var partunit = document.getElementById("txtPartNetUnitWeight(g)0").value;
                            var prosuom = document.getElementById("txtProcessUOM" + i).value;
                            var base = document.getElementById("txtBaseqty" + i).value;
                            if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "CYCLETIMEINSEC/SHOT") {
                                if (cavity != "") {
                                    base = cavity;
                                }
                            }
                            else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "CYCLETIMEINSEC/PC") {
                                base = 1;
                            }
                            else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "PCS/LOAD" || prosuom.toString().replace(/\s/g, '').toUpperCase() == "PC" || prosuom.toString().replace(/\s/g, '').toUpperCase() == "CYCLETIMEINSEC") {

                                base = document.getElementById("txtBaseqty" + i).value;
                            }
                            else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "KG/LOAD" || prosuom.toString().replace(/\s/g, '').toUpperCase() == "KG") {
                                if (varProcessGroup == "Layout5") { // Layout5 - ST
                                    if (CellsCountMatCost > 2) {
                                        if (CellsCountMatCost == CellsCount) {
                                            partunit = $("#txtPartNetUnitWeight\\(g\\)" + i).val();
                                            base = parseInt(1000 / partunit);
                                            $('#txtBaseqty' + i).prop("disabled", true);
                                            document.getElementById("txtBaseqty" + i).value = base;
                                        }
                                        else {
                                            //partunit = $("#txtPartNetUnitWeight\\(g\\)" + (CellsCountMatCost - 2)).val();
                                            //$('#txtBaseqty' + i).prop("disabled", false);
                                            //base = parseInt(1000 / partunit);
                                            //document.getElementById("txtBaseqty" + i).value = base;

                                            partunit = 0;
                                            $('#txtBaseqty' + i).prop("disabled", false);
                                            //document.getElementById("txtBaseqty" + i).value = "";
                                        }
                                    }
                                    else {
                                        partunit = document.getElementById("txtPartNetUnitWeight(g)" + 0).value;
                                        base = parseInt(1000 / partunit);
                                        document.getElementById("txtBaseqty" + i).value = base;
                                    }
                                }
                                else {
                                    partunit = document.getElementById("txtPartNetUnitWeight(g)" + 0).value;
                                    base = parseInt(1000 / partunit);
                                    $('#txtBaseqty' + i).prop("disabled", true);
                                    document.getElementById("txtBaseqty" + i).value = base;
                                }

                            }

                            var duration = document.getElementById("txtDurationperProcessUOM(Sec)" + i).value;
                            var efficiencyeild = document.getElementById("txtEfficiency/ProcessYield(%)" + i).value;
                            var ratecost = Hr_Rate / 3600;

                            var rate = truncator(ratecost, 4).toString();
                            var durationcost = (duration / base)


                            var durtion_cost = truncator(durationcost, 4).toString();
                            var effcirntcost = duration * ((100 + 100 - efficiencyeild) / 100) / base;
                            var processcost = Hr_Rate / (3600 / effcirntcost);

                            var CheckValprocesscost = processcost.toString().toUpperCase();
                            if (CheckValprocesscost == "NAN" || CheckValprocesscost == "INFINITY") {
                                processcost = 0;
                            }
                            document.getElementById("txtProcessCost/pc" + i).value = parseFloat(processcost).toFixed(6);

                            // document.getElementById("txtTotalProcessesCost/pcs" + i).value = process_Cost;

                            if ($("#dynamicddlSubvendorname" + i).prop('selectedIndex') > 0) {
                                var txtTurnkeyCost_pc = parseFloat($('#txtTurnkeyCost\\/pc' + i).val());
                                var txtTurnkeyProfit = $('#txtTurnkeyProfit' + i).val().replace("%", "");
                                var txtTurnkeyProfitNew = parseFloat(txtTurnkeyProfit) / 100;
                                var txtProcessCost = txtTurnkeyCost_pc + (txtTurnkeyCost_pc * txtTurnkeyProfitNew);

                                var CheckValtxtProcessCost = processcost.toString().toUpperCase();
                                if (CheckValtxtProcessCost == "NAN" || CheckValtxtProcessCost == "INFINITY") {
                                    txtProcessCost = 0;
                                }
                                document.getElementById("txtProcessCost/pc" + i).value = parseFloat(txtProcessCost).toFixed(6);
                                //document.getElementById("txtProcessCost/pc" + i).value = parseFloat(processcost.toFixed(6));
                            }
                        }

                        var BsQtyVal = $('#txtBaseqty' + i).val();
                        var DurProcUom = $('#txtDurationperProcessUOM\\(Sec\\)' + i).val();
                        var EfficiencyYield = $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val();
                        var txtIfTurnkeyVendorName = $('#txtIfTurnkey-VendorName' + i).val();
                        var txtIfTurnkeySubVN = document.getElementById('dynamicddlSubvendorname' + i).selectedIndex;

                        if (PrGroup.trim() != '--Select--') {
                            if (txtIfTurnkeyVendorName == "") {
                                if (txtIfTurnkeySubVN <= 0) {
                                    var ProcUomOri = prosuom.split('-');
                                    if (BsQtyVal.toString() === "" && ProcUomOri[0].toString().toUpperCase().replace(/\s/g, '') === "STROKES/MIN") {
                                        Validate = false;
                                        alert("please Enter base qty !!");
                                        $('#txtProcessCost\\/pc' + i).val("");
                                        document.getElementById("txtBaseqty" + i).focus();
                                    }
                                    else if (BsQtyVal.toString() === "" && ProcUomOri[0].toString().toUpperCase().replace(/\s/g, '') === "KG/LOAD") {
                                        Validate = false;
                                        alert("please Enter base qty !!");
                                        $('#txtProcessCost\\/pc' + i).val("");
                                        document.getElementById("txtBaseqty" + i).focus();
                                    }

                                    if (DurProcUom == "") {
                                        Validate = false;
                                        alert("Please Enter Duration per Process UOM (Sec) !!");
                                        document.getElementById("txtDurationperProcessUOM(Sec)" + i).focus();
                                    }

                                    if (EfficiencyYield == "" || EfficiencyYield == 0 || parseInt(EfficiencyYield) > 100) {
                                        Validate = false;
                                        alert("Please Enter Efficiency/Process Yield (%) between 1 to 100 !!");
                                        document.getElementById("txtEfficiency/ProcessYield(%)" + i).focus();
                                        $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val("100");
                                    }

                                    if ($("#txtVendorRate" + i).val() == "") {
                                        Validate = false;
                                        alert("Vendor rate cannot be empty !!");
                                        document.getElementById("txtVendorRate" + i).focus();
                                        $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val("100");
                                    }
                                }
                                else {
                                    var TurnkeyCost = $('#txtTurnkeyCost\\/pc' + i).val();
                                    if (TurnkeyCost == "") {
                                        if (txtIfTurnkeySubVN > 0) {
                                            Validate = false;
                                            alert("Please Enter Turn Key Cost !!");
                                            $('#txtTurnkeyCost\\/pc' + i).focus();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                var totalprocesscal = 0;

                if (Validate == true) {
                    for (var i = 0; i < CellsCount - 1; i++) {
                        var ttlprocesscost = document.getElementById("txtProcessCost/pc" + i).value;

                        totalprocesscal = ((+totalprocesscal) + (+ttlprocesscost));
                    }
                    //add by celindo - hafiz
                    var CheckValTot = totalprocesscal.toString().toUpperCase();
                    if (CheckValTot == "NAN" || CheckValTot == "INFINITY") {
                        totalprocesscal = 0;
                    }

                    if (document.getElementById("txtTotalProcessesCost/pcs0") != null) {
                        document.getElementById("txtTotalProcessesCost/pcs0").value = parseFloat(totalprocesscal).toFixed(5);
                    }

                    document.getElementById("txtTotalProcessesCost/pcs-0").value = parseFloat(totalprocesscal).toFixed(5);
                    //*

                    ProcessCostDataStore();
                    ReCalculate();
                }
                return false;
            } catch (err) {
                alert("processcost() : " + err);
                return false;
            }
        }

        //Session Maintain for Process Cost Table
        function ProcessCostDataStore() {

            var CellsCount = $("#TablePC").find('tr')[0].cells.length;
            var hdnvaltemp = "";
            for (var i = 0; i < CellsCount - 1; i++) {

                var txtProcessGroup = document.getElementById("dynamicddlProcess" + i).options[document.getElementById("dynamicddlProcess" + i).selectedIndex].text;
                var txtSubProcess = document.getElementById("dynamicddlSubProcess" + i).value;

                var txtifVendorName = document.getElementById("txtIfTurnkey-VendorName" + i).value;
                var txtdynamicddlSubvendorname = "";
                var indxDdlSubvnd = $("#dynamicddlSubvendorname" + i + " option:selected").index()
                if (indxDdlSubvnd == 0) {
                    txtdynamicddlSubvendorname = "";
                }
                else {
                    txtdynamicddlSubvendorname = document.getElementById("dynamicddlSubvendorname" + i).value;
                }

                var txtMachineLabor = document.getElementById("dynamicddlMachineLabor" + i).value;

                var txtMachineLaborconditionval = "";
                // alert(txtMachineLabor);
                if (txtMachineLabor == "Machine") {
                    //txtMachineLaborconditionval = document.getElementById("dynamicddlMachine" + i).value;
                    var SAPCode = $("#txtpartdesc").val();
                    var IsSAPCode = $("#hdnIsSAPCode").val();
                    //if (SAPCode.replace('-', '').trim().toString() == "") {
                    if (IsSAPCode.toString() == "False") {
                        var ItemdynamicddlMachine = $("#dynamicddlMachine" + i + " option").length;
                        if (ItemdynamicddlMachine == 0) {
                            txtMachineLaborconditionval = $("#txtMachineId" + i).val();
                        }
                        else {
                            txtMachineLaborconditionval = $('#dynamicddlMachine' + i + ' :selected').text();
                        }
                    }
                    else {
                        txtMachineLaborconditionval = $('#dynamicddlMachine' + i + ' :selected').text();
                    }
                }
                else {
                    txtMachineLaborconditionval = document.getElementById("txtMachineId" + i).value;
                }

                var txtStandardRate = document.getElementById("txtStandardRate/HR" + i).value;
                var txtVendorRate = document.getElementById("txtVendorRate" + i).value;
                var txtPUom = document.getElementById("txtProcessUOM" + i).value;
                var txtBaseQ = document.getElementById("txtBaseqty" + i).value;
                var txtdurationUOM = document.getElementById("txtDurationperProcessUOM(Sec)" + i).value;
                var txtEffPro = document.getElementById("txtEfficiency/ProcessYield(%)" + i).value;
                var txtTurnkeyCost_pc = document.getElementById("txtTurnkeyCost/pc" + i).value;
                var txtTurnkeyProfit = document.getElementById("txtTurnkeyProfit" + i).value;
                var txtProCost = document.getElementById("txtProcessCost/pc" + i).value;

                var txttotalcost = document.getElementById("txtTotalProcessesCost/pcs0").value;

                hdnvaltemp += (+ "," + txtProcessGroup + "," + txtSubProcess + "," + txtifVendorName + "," + txtdynamicddlSubvendorname + "," + txtMachineLabor + "," + txtMachineLaborconditionval + "," + txtStandardRate + "," + txtVendorRate + "," + txtPUom + "," + txtBaseQ + "," + txtdurationUOM + "," + txtEffPro + "," + txtTurnkeyCost_pc + "," + txtTurnkeyProfit + "," + txtProCost + "," + txttotalcost + ",");

                $("#hdnProcessTableCount").val(i);
                //alert($("#hdnProcessValues").val());
            }

            $("#hdnProcessValues").val(hdnvaltemp);
        }

        //this script used for page :Review req mass, new request change mass, new request changes, new request change mass
        function GetRunnerRatioPrcentagePerPiece() {
            try {
                var varProcessGroup = $("#hdnLayoutScreen").val();

                if (varProcessGroup == "Layout1") {  // Layout1 - IM

                    var runnerweight = document.getElementById("txt~RunnerWeight/shot(g)0").value;
                    if (runnerweight == '') {
                        runnerweight = 0;
                    }
                    var partunit = document.getElementById("txtPartNetUnitWeight(g)0").value;
                    if (partunit == '') {
                        partunit = 0;
                    }
                    var cavity = document.getElementById("txtCavity0").value;
                    if (cavity == '') {
                        cavity = 0;
                    }
                    var runnerweight_Percent = runnerweight / (parseFloat((partunit * cavity)) + parseFloat(runnerweight)) * 100;
                    if (runnerweight_Percent.toString() == 'NaN') {
                        runnerweight_Percent = 0;
                    }
                    document.getElementById("txt~RunnerRatio/pcs(%)0").value = parseInt(runnerweight_Percent);
                    document.getElementById("txtTotalMaterialCost/pcs0").value = "";

                    var TxtMainRecycleRatio = document.getElementById("TxtImRcylRatio");
                    if (TxtMainRecycleRatio != null) {
                        var MainRecycleRatio = TxtMainRecycleRatio.value;
                        if (MainRecycleRatio.toString().toUpperCase() == "FOLLOW RUNNER RATIO") {
                            if (document.getElementById("txt~RecycleMaterialRatio(%)0") != null) {
                                document.getElementById("txt~RecycleMaterialRatio(%)0").value = parseInt(runnerweight_Percent);
                            }
                        }
                        else {
                            if (document.getElementById("txt~RecycleMaterialRatio(%)0") != null) {
                                if (document.getElementById("txt~RunnerRatio/pcs(%)0") != null) {
                                    if (parseInt(runnerweight_Percent) > parseInt(MainRecycleRatio)) {
                                        document.getElementById("txt~RecycleMaterialRatio(%)0").value = MainRecycleRatio;
                                    }
                                    else {
                                        document.getElementById("txt~RecycleMaterialRatio(%)0").value = parseInt(runnerweight_Percent);
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (err) {
                alert("GetRunnerRatioPrcentagePerPiece : " + err);
            }
        }

        function dynamicddlMachineLaborMethod(seltext, i) {
            //var selectedText = $(this).find("option:selected").text();
            //var selectedValue = $(this).val();
            //
            //subash
            //$("#txtStandardRate\\/HR"+i).prop("disabled", false);
            $("#txtVendorRate" + i).prop("disabled", false);

            if (seltext.toString().toUpperCase() == "LABOR") {
                $("#dynamicddlMachine" + i).hide();
                $("#txtMachineId" + i).show();


                $('#grdLaborlisthidden tr').each(function () {
                    var checkval = 0;
                    $(this).find('td').each(function () {

                        if (checkval == 0) {
                            $("#txtStandardRate\\/HR" + i).val($(this).text());
                            $("#txtVendorRate" + i).val($(this).text());
                            checkval = 1;
                        }

                        if ($(this).html() == "Y") {
                            $("#txtVendorRate" + i).prop("disabled", true);
                            $("#hdnVendorActivity").val($(this).html());
                        }
                        else if ($(this).html() == "N") {
                            $("#txtVendorRate" + i).prop("disabled", false);
                            $("#hdnVendorActivity").val($(this).html());
                        }

                    })
                });

            }
            else {
                $("#dynamicddlMachine" + i).show();
                $("#txtMachineId" + i).hide();


                $('#grdMachinelisthidden tr').each(function () {
                    var checkval = 0;
                    $(this).find('td').each(function () {

                        // alert($(this).text());
                        if (checkval == 1) {
                            $("#txtStandardRate\\/HR" + i).val($(this).text());
                            $("#txtVendorRate" + i).val($(this).text());
                            checkval++;
                        }

                        if ($(this).text().trim().trimLeft().trimRight() == $("#dynamicddlMachine" + i).find("option:selected").text().trim().trimLeft().trimRight()) {
                            checkval++;
                        }

                        if (checkval == 2) {
                            if ($(this).html() == "Y") {
                                $("#txtVendorRate" + i).prop("disabled", true);
                                $("#hdnVendorActivity").val($(this).html());
                            }
                            else if ($(this).html() == "N") {
                                $("#txtVendorRate" + i).prop("disabled", false);
                                $("#hdnVendorActivity").val($(this).html());
                            }

                        }

                    })
                });

            }
            $("#txtStandardRate\\/HR" + i).prop("disabled", true);
            $("#txtVendorRate" + i).prop("disabled", true);
        }

        function dynamicddlMachineMethod(i) {
            $('#grdMachinelisthidden tr').each(function () {
                var checkval = 0;
                $(this).find('td').each(function () {
                    //  alert($(this).text());
                    if (checkval == 1) {
                        $("#txtStandardRate\\/HR" + i).val($(this).text());
                        $("#txtVendorRate" + i).val($(this).text());
                        checkval++;
                    }

                    if ($(this).text().trim().trimLeft().trimRight() == $("#dynamicddlMachine" + i).find("option:selected").text().trim().trimLeft().trimRight()) {
                        checkval++;
                    }

                    if (checkval == 2) {
                        if ($(this).html() == "Y") {
                            $("#txtVendorRate" + i).prop("disabled", true);
                            $("#hdnVendorActivity").val($(this).html());
                        }
                        else if ($(this).html() == "N") {
                            $("#txtVendorRate" + i).prop("disabled", false);
                            $("#hdnVendorActivity").val($(this).html());
                        }
                    }
                })
            });
            Baseqtyupdatebyuom(i);
        }

        function DisAndEmptyFormControlIfTurnKey(i) {

            $('#dynamicddlProcess' + i).prop("disabled", true);
            $('#dynamicddlSubProcess' + i).prop("disabled", true);
            $('#dynamicddlMachineLabor' + i).prop("disabled", true);
            $('#dynamicddlMachine' + i).prop("disabled", true);

            $('#dynamicddlMachineLabor' + i).hide();
            $('#dynamicddlMachine' + i).hide();
            $('#dynamicddlHideMachineLabor' + i).show();
            $('#txtMachineId' + i).hide();

            //$("#dynamicddlMachineLabor" + i).prop('selectedIndex', -1);
            //$("#dynamicddlMachine" + i).prop('selectedIndex', -1);

            $('#txtMachineId' + i).prop("disabled", true);
            $('#txtStandardRate\\/HR' + i).prop("disabled", true);
            $('#txtVendorRate' + i).prop("disabled", true);
            $('#txtProcessUOM' + i).prop("disabled", true);
            $('#txtBaseqty' + i).prop("disabled", true);
            $('#txtDurationperProcessUOM\\(Sec\\)' + i).prop("disabled", true);
            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).prop("disabled", true);


            $('#txtProcessCost\\/pc' + i).prop("disabled", false);

            $('#txtStandardRate\\/HR' + i).val("");
            $('#txtVendorRate' + i).val("");
            $('#txtProcessUOM' + i).val("");
            //$('#txtBaseqty' + i).val("");
            $('#txtDurationperProcessUOM\\(Sec\\)' + i).val("");
            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val("");
            $('#txtStandardRate\\/HR' + i).val("");

            $('#dynamicddlSubvendorname' + i).prop("disabled", true);
            $('#txtTurnkeyCost\\/pc' + i).val("");
            $('#txtTurnkeyProfit' + i).val("");
            $('#txtTurnkeyCost\\/pc' + IdNo).prop("disabled", true);
            $('#txtTurnkeyProfit' + IdNo).prop("disabled", true);
        }

        function TurnKeyVendorUpdate(i) {
            var trunval = $('#txtIfTurnkey-VendorName' + i).val();
            // alert(turnval);
            if (trunval == '' || trunval == null) {

                $("#dynamicddlProcess" + i).prop("disabled", false);
                $("#dynamicddlSubProcess" + i).prop("disabled", false);
                $('#dynamicddlSubvendorname' + i).prop("disabled", false);
                $('#dynamicddlMachineLabor' + i).prop("disabled", false);
                $('#dynamicddlMachine' + i).prop("disabled", false);

                $('#txtProcessCost\\/pc' + i).prop("disabled", true);

                $("#dynamicddlMachine" + i).prop('selectedIndex', -1);

                $('#dynamicddlMachineLabor' + i).show();
                $('#dynamicddlHideMachineLabor' + i).hide();
                $('#txtMachineId' + i).show();
                $('#dynamicddlHideMachineLabor' + i).hide();
                $('#txtMachineId' + i).show();

                $('#txtHide' + i).hide();
                $('#ddlHideMachine' + i).hide();

                document.getElementById("txtStandardRate/HR" + i).value = "";
                document.getElementById("txtVendorRate" + i).value = "";
                document.getElementById("txtProcessUOM" + i).value = "";
                document.getElementById("txtBaseqty" + i).value = "";
                document.getElementById("txtTurnkeyCost/pc" + i).value = "";
                document.getElementById("txtTurnkeyProfit" + i).value = "";
                document.getElementById("txtProcessCost/pc" + i).value = "";
                document.getElementById("txtTotalProcessesCost/pcs" + 0).value = "";

                var MachineLabortxt1 = $('#dynamicddlMachineLabor' + i).find("option:selected").text();
                if (MachineLabortxt1.toString().toUpperCase() == "MACHINE") {
                    $('#txtMachineId' + i).hide();
                    $('#dynamicddlMachine' + i).show();
                }
                else {
                    $('#dynamicddlMachine' + i).hide();
                    $('#txtMachineId' + i).show();
                }

                var SubProcGrp = document.getElementById('dynamicddlSubProcess' + i).selectedIndex;
                if (SubProcGrp > -1) {
                    var SubProcGroup = $('#dynamicddlSubProcess' + i).val();
                    $('#hdnSubProcGroup').val(SubProcGroup.toString());
                }

                $('#hdnColumTblProcNo').val(i.toString());
                document.getElementById('BtnFndVndMachineVsProcUom').click();
            }
            else {
                document.getElementById("DvBaseqty" + i).style.display = "none";
                $("#dynamicddlSubvendorname" + i).prop('selectedIndex', 0);
                $('#dynamicddlSubvendorname' + i).prop("disabled", true);

                $('#dynamicddlMachineLabor' + i).prop("disabled", true);
                $('#dynamicddlMachine' + i).prop("disabled", true);

                $('#dynamicddlProcess' + i).prop("disabled", true);
                $('#dynamicddlSubProcess' + i).prop("disabled", true);
                $('#txtMachineId' + i).prop("disabled", true);

                $("#dynamicddlMachine" + i).prop('selectedIndex', -1);

                $('#dynamicddlMachineLabor' + i).hide();
                $('#dynamicddlHideMachineLabor' + i).show();
                $('#dynamicddlMachine' + i).hide();
                $('#txtMachineId' + i).show();
                $('#dynamicddlHideMachineLabor' + i).show();

                $('#ddlHideMachine' + i).hide();
                $('#txtHide' + i).hide();

                $('#hdnSTDRate').val($('#txtStandardRate\\/HR' + i).val());
                $('#hdnVendorRate').val($('#txtVendorRate' + i).val());
                $('#hdnUOM').val($('#txtProcessUOM' + i).val());

                $('#txtMachineId' + i).val("");
                $('#txtStandardRate\\/HR' + i).val("");
                $('#txtVendorRate' + i).val("");
                $('#txtProcessUOM' + i).val("");
                $('#txtBaseqty' + i).val("");
                $('#txtDurationperProcessUOM\\(Sec\\)' + i).val("");
                $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val("");

                $('#txtVendorRate' + i).prop("disabled", true);
                $('#txtBaseqty' + i).prop("disabled", true);


                $('#txtDurationperProcessUOM\\(Sec\\)' + i).prop("disabled", true);

                $('#txtStandardRate\\/HR' + i).prop("disabled", true);

                $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).prop("disabled", true);
                $('#txtProcessCost\\/pc' + i).prop("disabled", false);
                // $('#txtProcessCost\\/pc'+i).text("");
                //$('#txtProcessCost\\/pc'+i).html("");
                $('#txtProcessCost\\/pc' + i).val("");

                $('#txtTurnkeyCost\\/pc' + i).prop("disabled", true);
                $('#txtTurnkeyProfit' + i).prop("disabled", true);
            }
        }

        function Baseqtyupdatebyuom(i) {

            var varProcessGroup = $("#hdnLayoutScreen").val();
            var cavity = document.getElementById("txtCavity0").value;
            var partunit = document.getElementById("txtPartNetUnitWeight(g)0").value;
            var prosuom = document.getElementById("txtProcessUOM" + i).value;

            var CellsCountMatCost = $("#Table1").find('tr')[0].cells.length;
            var CellsCountProcCost = $("#TablePC").find('tr')[0].cells.length;

            if (varProcessGroup == "Layout5") { // Layout5 - ST
                if (CellsCountMatCost > 2) {
                    if (CellsCountMatCost == CellsCountProcCost) {
                        cavity = $("#txtCavity" + i).val();
                        partunit = $("#txtPartNetUnitWeight\\(g\\)" + i).val();
                    }
                    else {
                        cavity = $("#txtCavity" + (CellsCountMatCost - 2)).val();
                        partunit = $("#txtPartNetUnitWeight\\(g\\)" + (CellsCountMatCost - 2)).val();
                    }
                    prosuom = $("#txtProcessUOM" + i).val();
                }
            }

            if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "CYCLETIMEINSEC/SHOT") {
                if (varProcessGroup == "Layout5") {
                    if (CellsCountMatCost > 2) {
                        $('#txtBaseqty' + i).prop("disabled", false);
                        $('#txtBaseqty' + i).val("");
                    }
                    else {
                        $('#txtBaseqty' + i).val(cavity);
                        $('#txtBaseqty' + i).prop("disabled", true);
                    }
                }
                else {
                    cavity = document.getElementById("txtCavity0").value;
                    $('#txtBaseqty' + i).val(cavity);
                    $('#txtBaseqty' + i).prop("disabled", true);
                }
            }
            else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "CYCLETIMEINSEC/PC") {
                $('#txtBaseqty' + i).val(1);
                $('#txtBaseqty' + i).prop("disabled", true);
            }
            else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "PCS/LOAD" || prosuom.toString().replace(/\s/g, '').toUpperCase() == "PC" || prosuom.toString().replace(/\s/g, '').toUpperCase() == "CYCLETIMEINSEC") {
                $('#txtBaseqty' + i).prop("disabled", false);
                $('#txtBaseqty' + i).val("");
            }//subash
            else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "KG/LOAD" || prosuom.toString().replace(/\s/g, '').toUpperCase() == "KG") {
                if (CellsCountMatCost > 2) {
                    if (CellsCountMatCost == CellsCountProcCost) {
                        partunit = document.getElementById("txtPartNetUnitWeight(g)" + i).value;
                        var base11 = 1000 / partunit;
                        $('#txtBaseqty' + i).prop("disabled", true);
                        $('#txtBaseqty' + i).val(parseInt(base11));
                    }
                    else {
                        partunit = 0;
                        $('#txtBaseqty' + i).prop("disabled", false);
                        document.getElementById("txtBaseqty" + i).value = "";
                        //partunit = document.getElementById("txtPartNetUnitWeight(g)" + (CellsCountMatCost - 2)).value;
                    }
                }
                else {
                    partunit = document.getElementById("txtPartNetUnitWeight(g)" + 0).value;
                    var base11 = 1000 / partunit;
                    $('#txtBaseqty' + i).prop("disabled", true);
                    $('#txtBaseqty' + i).val(parseInt(base11));
                }
            }
            else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "STROKES/MIN") {
                if (varProcessGroup == "Layout5") {
                    if (CellsCountMatCost > 2) {
                        $('#txtBaseqty' + i).val("");
                        $('#txtBaseqty' + i).prop("disabled", false);
                        $('#txtBaseqty' + i).focus();
                    }
                    else {
                        $('#txtBaseqty' + i).val(document.getElementById("txtCavity0").value);
                        $('#txtBaseqty' + i).prop("disabled", true);
                    }
                }
                else {
                    $('#txtBaseqty' + i).val(document.getElementById("txtCavity0").value);
                    $('#txtBaseqty' + i).prop("disabled", true);
                }
            }
            debugger;
            var ArrProsuom = prosuom.toString().replace(/\s/g, '').toUpperCase().split('-');
            if (ArrProsuom.length > 0) {
                prosuom = ArrProsuom[0].toString().trim();
            }
            if (prosuom.toString().replace(/\s/g, '').toUpperCase() == ("STROKES/MIN")) {
                var PrcGrpCodeFull = $('#dynamicddlProcess' + i + ' :selected').text();
                var DataMachine = $('option:selected', "#dynamicddlMachine" + i + "").attr('mac-id');

                var ArrPrcGrpCode = PrcGrpCodeFull.split("-");

                var PrcGrpCode = "";
                var MachineID = "";
                var MachineType = "";
                var MacTonnage = "";

                if (ArrPrcGrpCode.length == 2) {
                    PrcGrpCode = ArrPrcGrpCode[0].toString();
                }

                var dynamicddlMachineLabor = document.getElementById('dynamicddlMachineLabor' + i).selectedIndex;
                if (dynamicddlMachineLabor == 0) {
                    $('#txtDurationperProcessUOM\\(Sec\\)' + i).prop("disabled", true);
                    $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).prop("disabled", true);

                    $('#hdnColumTblProcNo').val(i.toString());
                    $('#hdnProcGroup').val(PrcGrpCode);
                    $('#hdnMachineId').val(DataMachine);
                    //$('#hdnMachineId').val(MachineID.trim());
                    //$('#hdnMachineType').val(MachineType.trim());
                    //$('#hdnTonnage').val(MacTonnage.trim().replace("T", ""));
                    document.getElementById('btnClickCalcPrcUOMStkMin').click();
                }
                else {
                    $('#txtDurationperProcessUOM\\(Sec\\)' + i).prop("disabled", false);
                    $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).prop("disabled", false);
                    $('#txtDurationperProcessUOM\\(Sec\\)' + i).val("");
                    $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val("");
                }

            }
            else {
                $('#txtDurationperProcessUOM\\(Sec\\)' + i).prop("disabled", false);
                $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).prop("disabled", false);
                $('#txtDurationperProcessUOM\\(Sec\\)' + i).val("");
                $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val("");
            }

            var ValSAPSpProcType = $('#HdnSAPSpProcType').val();
            var ValAcsTabMatCost = $('#HdnAcsTabMatCost').val();
            if (ValSAPSpProcType == "30") {
                if (ValAcsTabMatCost == "False") {
                    document.getElementById("txtBaseqty" + i).value = "1";
                    $("#txtBaseqty" + i).prop("disabled", false);
                }
            }

            cekProcGroupSelected(i);
            ReturnBaseQtyProcCheckStats();
        };

        function dynamicddlProcessMethod(seltext, i) {
            $("#dynamicddlSubvendorname" + i).prop('selectedIndex', 0);
            $('#dynamicddlSubvendorname' + i).prop("disabled", false);

            $("#dynamicddlMachineLabor" + i).prop('selectedIndex', 0);
            $('#txtIfTurnkey-VendorName' + i).prop("disabled", false);

            $('#dynamicddlMachineLabor' + i).show();
            $('#ddlHideMachine' + i).hide();
            $('#txtMachineId' + i).hide();

            $('#dynamicddlMachine' + i).show();
            $('#dynamicddlHideMachineLabor' + i).hide();

            $('#txtDurationperProcessUOM\\(Sec\\)' + i).prop("disabled", false);
            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).prop("disabled", false);

            var selectedText = seltext.toString();
            $("#dynamicddlSubProcess" + i).empty();

            $('#dynamicddlMachine' + i).empty();

            document.getElementById("txtStandardRate/HR" + i).value = "";
            document.getElementById("txtVendorRate" + i).value = "";
            document.getElementById("txtProcessUOM" + i).value = "";
            document.getElementById("txtBaseqty" + i).value = "";

            var firsttime = 0;
            $('#grdProcessGrphidden tr').each(function () {
                var checkval = 0;

                $(this).find('td').each(function () {

                    if (checkval == 0) {
                        if ($(this).html().toString().toUpperCase() == selectedText.toString().toUpperCase()) {
                            checkval = 1;
                        }
                    }
                    else if (checkval == 1) {
                        var mySelect = $("#dynamicddlSubProcess" + i);
                        mySelect.append($('<option></option>').val($(this).html()).html($(this).html()));

                        checkval = 2;
                    }
                    else if (checkval == 2 && firsttime == 0) {
                        // alert($(this).text() + "al");
                        document.getElementById("txtProcessUOM" + i).value = $(this).text();
                        //Baseqtyupdatebyuom(i);
                        checkval = 0;
                        firsttime = 1;
                    }
                })
            });

            // $('#dynamicddlMachine'+i)


            $('#grdMachinelisthidden tr').each(function () {
                //  
                var processval = $(this).find("td:eq(4)").text();
                //   alert(processval );

                var substringseltext = selectedText.toString().substring(0, 4);

                if (substringseltext.indexOf("-") != -1) {
                    substringseltext = substringseltext.toString().substring(0, 2);
                }

                // alert("sel" + substringseltext);
                if (processval != "") {
                    if (processval.toString().toUpperCase() == substringseltext.toString().toUpperCase()) {
                        // alert("seltext" + substringseltext);

                        var mySelect = $("#dynamicddlMachine" + i);
                        var MacId = $(this).find("td:eq(5)").text();

                        //mySelect.append($('<option></option>').val($(this).find("td:eq(0)").text()).attr('Mac-Id', MacId).html($(this).find("td:eq(0)").text()));
                        mySelect.append($('<option></option>').val($(this).find("td:eq(0)").text()).html($(this).find("td:eq(0)").text()));
                    }
                    else {

                    }
                }
            });

            dynamicddlMachineMethod(i);
        }

        function dynamicddlSubprocessMethod(seltext, i) {
            $("#dynamicddlSubvendorname" + i).prop('selectedIndex', 0);
            $('#dynamicddlSubvendorname' + i).prop("disabled", false);

            $("#dynamicddlMachineLabor" + i).prop('selectedIndex', 0);
            $('#txtIfTurnkey-VendorName' + i).prop("disabled", false);

            $('#dynamicddlMachineLabor' + i).show();
            $('#ddlHideMachine' + i).hide();
            $('#txtMachineId' + i).hide();

            $('#dynamicddlMachine' + i).show();
            $('#dynamicddlHideMachineLabor' + i).hide();

            var selectedText = seltext;
            $('#grdProcessGrphidden tr').each(function () {
                var checkval = 0;
                $(this).find('td').each(function () {

                    if (checkval == 0) {
                        if ($(this).html().toString().toUpperCase() == selectedText.toString().toUpperCase()) {
                            checkval = 1;
                        }
                    }
                    else if (checkval == 1) {
                        document.getElementById("txtProcessUOM" + i).value = $(this).text();
                        checkval = 0;

                        Baseqtyupdatebyuom(i);
                    }
                })
            });
        }

        function ProcGrpChange(IdNo) {
            try {
                var x = document.getElementById('dynamicddlProcess' + IdNo).selectedIndex;
                $("#txtMachineId" + IdNo).val("");
                if (x > 0) {
                    $("#dynamicddlSubProcess" + IdNo).prop("disabled", false);

                    $("#dynamicddlSubvendorname" + IdNo).prop('selectedIndex', 0);
                    $('#dynamicddlSubvendorname' + IdNo).prop("disabled", false);

                    $("#dynamicddlMachineLabor" + IdNo).prop('selectedIndex', 0);
                    $('#txtIfTurnkey-VendorName' + IdNo).prop("disabled", false);
                    $('#dynamicddlMachineLabor' + IdNo).prop("disabled", false);
                    $('#dynamicddlMachine' + IdNo).prop("disabled", false);

                    $('#dynamicddlMachineLabor' + IdNo).show();
                    $('#ddlHideMachine' + IdNo).hide();
                    $('#txtMachineId' + IdNo).hide();

                    $('#dynamicddlMachine' + IdNo).show();
                    $('#dynamicddlHideMachineLabor' + IdNo).hide();

                    $('#txtDurationperProcessUOM\\(Sec\\)' + IdNo).prop("disabled", false);
                    $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + IdNo).prop("disabled", false);

                    $("#dynamicddlSubProcess" + IdNo).empty();

                    $('#dynamicddlMachine' + IdNo).empty();

                    document.getElementById("txtStandardRate/HR" + IdNo).value = "";
                    document.getElementById("txtVendorRate" + IdNo).value = "";
                    document.getElementById("txtProcessUOM" + IdNo).value = "";
                    document.getElementById("txtBaseqty" + IdNo).value = "";
                    document.getElementById("txtTurnkeyCost/pc" + IdNo).value = "";
                    document.getElementById("txtTurnkeyProfit" + IdNo).value = "";
                    document.getElementById("txtProcessCost/pc" + IdNo).value = "";
                    document.getElementById("txtTotalProcessesCost/pcs" + 0).value = "";



                    if (x > 0) {
                        var ProcGroup = $('#dynamicddlProcess' + IdNo).val();
                        var ArrProcGroup = ProcGroup.split('-');
                        var procGrpCode = ArrProcGroup[0].toString().trim();
                        $('#hdnProcGroup').val(procGrpCode.toString());
                        $('#hdnColumTblProcNo').val(IdNo);
                        document.getElementById('BtnFndVndMachine').click();
                    }
                    $("#txtEfficiency\\/ProcessYield\\(\\%\\)" + IdNo).val("100");
                }
                else {
                    $("#dynamicddlSubProcess" + IdNo).prop("disabled", true);
                    $("#txtIfTurnkey-VendorName" + IdNo).prop("disabled", true);
                    $("#dynamicddlSubvendorname" + IdNo).prop("disabled", true);
                    $("#dynamicddlMachineLabor" + IdNo).prop("disabled", true);
                    $("#dynamicddlMachine" + IdNo).prop("disabled", true);
                    $("#txtStandardRate\\/HR" + IdNo).prop("disabled", true);
                    $("#txtVendorRate" + IdNo).prop("disabled", true);
                    $("#txtProcessUOM" + IdNo).prop("disabled", true);
                    $("#txtBaseqty" + IdNo).prop("disabled", true);
                    $("#txtDurationperProcessUOM\\(Sec\\)" + IdNo).prop("disabled", true);
                    $("#txtEfficiency\\/ProcessYield\\(\\%\\)" + IdNo).prop("disabled", true);
                    $("#txtTurnkeyCost\\/pc" + IdNo).prop("disabled", true);
                    $("#txtTurnkeyProfit" + IdNo).prop("disabled", true);
                    $("#txtProcessCost\\/pc" + IdNo).prop("disabled", true);
                    $("#txtTotalProcessesCost\\/pcs" + IdNo).prop("disabled", true);

                    $("#dynamicddlSubProcess" + IdNo).prop('selectedIndex', -1);
                    $("#dynamicddlSubvendorname" + IdNo).prop('selectedIndex', 0);
                    $("#dynamicddlMachineLabor" + IdNo).prop('selectedIndex', 0);

                    document.getElementById("txtIfTurnkey-VendorName" + IdNo).value = "";
                    document.getElementById("txtStandardRate/HR" + IdNo).value = "";
                    document.getElementById("txtVendorRate" + IdNo).value = "";
                    document.getElementById("txtProcessUOM" + IdNo).value = "";
                    document.getElementById("txtBaseqty" + IdNo).value = "";
                    document.getElementById("txtDurationperProcessUOM(Sec)" + IdNo).value = "";
                    document.getElementById("txtEfficiency/ProcessYield(%)" + IdNo).value = "";
                    document.getElementById("txtTurnkeyCost/pc" + IdNo).value = "";
                    document.getElementById("txtTurnkeyProfit" + IdNo).value = "";
                    document.getElementById("txtProcessCost/pc" + IdNo).value = "";
                    document.getElementById("txtTotalProcessesCost/pcs" + 0).value = "";
                }
            } catch (err) {
                alert("ProcGrpChange() : " + err);
            }
        }

        function GetLayoutProcGrp(DdlProcessGroup) {
            try {

                var table = document.getElementById('GvProcessGrpVsLayout');
                var RowsCounttable = $('#GvProcessGrpVsLayout tr').length;
                var Layout = "";
                for (var r = 1; r < RowsCounttable; r++) {
                    var TbProcessGroup = table.rows[r].cells[0].innerHTML;
                    if (DdlProcessGroup == TbProcessGroup) {
                        Layout = table.rows[r].cells[1].innerHTML;
                        break;
                    }
                }
                return Layout;
            } catch (err) {
                alert("GetLayoutProcGrp : " + err);
                return Layout;
            }
        }

        function cekProcGroupSelected(IdNo) {
            try {

                var CellsCountTabMatCost = $("#Table1").find('tr')[0].cells.length;

                var DdlProcGroup = document.getElementById("dynamicddlProcess" + IdNo);
                var ProcGroup = DdlProcGroup.options[DdlProcGroup.selectedIndex].value;
                var MainLayout = $("#hdnLayoutScreen").val();
                var Layout = GetLayoutProcGrp(ProcGroup);
                //var value = DdlProcGroup.options[e.selectedIndex].value;
                //var text = DdlProcGroup.options[e.selectedIndex].text;

                var ValSAPSpProcType = $('#HdnSAPSpProcType').val();
                var ValAcsTabMatCost = $('#HdnAcsTabMatCost').val();

                var IfSubcon = document.getElementById("txtIfTurnkey-VendorName" + IdNo).value;
                var SubvendorIdx = document.getElementById('dynamicddlSubvendorname' + IdNo).selectedIndex;
                var ProcessUOM = document.getElementById('txtProcessUOM' + IdNo).value.replace(/\s/g, '').toUpperCase();

                
                if (MainLayout == "Layout5") {
                    if (CellsCountTabMatCost == 2) {
                        if (Layout == "Layout5" || Layout == "Layout1") {
                            if ((IfSubcon == "") && (SubvendorIdx <= 0)) {
                                if (ProcessUOM != "CYCLETIMEINSEC/PC" && ProcessUOM != "CYCLETIMEINSEC/SHOT" && ProcessUOM != "PCS/LOAD") {
                                    //document.getElementById("DvBaseqty" + IdNo).style.display = "block";
                                    document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                                }
                                else {
                                    document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                                }
                            }
                            else {
                                document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                            }
                        }
                        else {
                            document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                        }
                    }
                    else {
                        document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                    }
                }
                else {
                    if (Layout == "Layout5" || Layout == "Layout1") {
                        if ((IfSubcon == "") && (SubvendorIdx <= 0)) {
                            if (CellsCountTabMatCost == 2) {
                                if (ProcessUOM != "CYCLETIMEINSEC/PC" && ProcessUOM != "CYCLETIMEINSEC/SHOT" && ProcessUOM != "PCS/LOAD") {
                                    //document.getElementById("DvBaseqty" + IdNo).style.display = "block";
                                    document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                                }
                                else {
                                    document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                                }
                            }
                            else {
                                document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                            }
                        }
                        else {
                            document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                        }
                    }
                    else {
                        document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                    }
                }

                var AcsTabProcCost = $("#HdnAcsTabProcCost").val();
                if (AcsTabProcCost == "False") {
                    document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                }
                else if (ValSAPSpProcType == "30" && ValAcsTabMatCost == "False") {
                    document.getElementById("DvBaseqty" + IdNo).style.display = "none";
                }

            } catch (err) {
                alert("cekProcGroupSelected : " + err);
            }
        }

        function CekFollowBaseQtyMatcostCondition() {
            try {
                var CellsCountTabProcCost = $("#TablePC").find('tr')[0].cells.length;
                //var AcsTabProcCost = $("#HdnAcsTabProcCost").val();
                if ($("#TablePC") != null) {
                    if (CellsCountTabProcCost >= 1) {
                        for (var i = 1; i < CellsCountTabProcCost; i++) {
                            //if (AcsTabProcCost == "False") {
                            //    document.getElementById("DvBaseqty" + (i - 1)).style.display = "none";
                            //}
                            cekProcGroupSelected(i - 1);
                        }
                    }
                }
            } catch (err) {
                alert("CekFollowBaseQtyMatcostCondition : " + err);
            }
        }

        function ChkBaseQtyProcessChange(IdNo) {
            try {
                var ChkBaseqty = document.getElementById("ChkBaseqty" + IdNo);

                if (ChkBaseqty.checked == true) {
                    $("#txtBaseqty" + IdNo).prop("disabled", true);
                    $("#txtBaseqty" + IdNo).val($("#txtCavity" + 0).val());
                }
                else {
                    $("#txtBaseqty" + IdNo).prop("disabled", false);
                }

            } catch (err) {
                alert("ChkBaseQtyProcessChange : " + err);
            }
        }

        function ReturnBaseQtyProcCheckStats() {
            try {
                var MainLayout = $("#hdnLayoutScreen").val();

                var TabMatCost = document.getElementById("Table1");
                var TabProcCost = document.getElementById("TablePC");

                var CellsCountTabMatCost = $("#Table1").find('tr')[0].cells.length;
                var CellsCountTabProcCost = $("#TablePC").find('tr')[0].cells.length;

                var ValSAPSpProcType = $('#HdnSAPSpProcType').val();
                var ValAcsTabMatCost = $('#HdnAcsTabMatCost').val();

                //if (ValSAPSpProcType != "30" && ValAcsTabMatCost != "False") {

                //}
                if (MainLayout == "Layout5") {
                    if (CellsCountTabMatCost == 2) {
                        for (var i = 2; i <= CellsCountTabProcCost; i++) {
                            var DdlProcGroup = document.getElementById("dynamicddlProcess" + (i - 2));
                            var ProcGroup = DdlProcGroup.options[DdlProcGroup.selectedIndex].value;
                            var Layout = GetLayoutProcGrp(ProcGroup);

                            var IfSubcon = document.getElementById("txtIfTurnkey-VendorName" + (i - 2)).value;
                            var SubvendorIdx = document.getElementById('dynamicddlSubvendorname' + (i - 2)).selectedIndex;
                            var ProcessUOM = document.getElementById('txtProcessUOM' + (i - 2)).value.replace(/\s/g, '').toUpperCase();

                            if (Layout == "Layout5" || Layout == "Layout1") {
                                if ((IfSubcon == "") && (SubvendorIdx <= 0)) {
                                    if (ProcessUOM != "CYCLETIMEINSEC/PC" && ProcessUOM != "CYCLETIMEINSEC/SHOT" && ProcessUOM != "PCS/LOAD") {
                                        //document.getElementById("DvBaseqty" + (i - 2)).style.display = "block";
                                        document.getElementById("DvBaseqty" + (i - 2)).style.display = "none";
                                    }
                                    else {
                                        document.getElementById("DvBaseqty" + (i - 2)).style.display = "none";
                                    }
                                    if ($("#txtBaseqty" + (i - 2)).val() == $("#txtCavity" + 0).val()) {
                                        var ChkBaseqty = document.getElementById("ChkBaseqty" + (i - 2));
                                        ChkBaseqty.checked = true;
                                        $("#txtBaseqty" + (i - 2)).prop("disabled", true);
                                    }
                                    else {
                                        var ChkBaseqty = document.getElementById("ChkBaseqty" + (i - 2));
                                        ChkBaseqty.checked = false;
                                        $("#txtBaseqty" + (i - 2)).prop("disabled", false);
                                    }
                                }
                                else {
                                    document.getElementById("DvBaseqty" + (i - 2)).style.display = "none";
                                }
                            }
                            else {
                                document.getElementById("DvBaseqty" + (i - 2)).style.display = "none";
                            }
                        }
                    }
                    else {
                        for (var i = 2; i <= CellsCountTabProcCost; i++) {
                            var DdlProcGroup = document.getElementById("dynamicddlProcess" + (i - 2));
                            var ProcGroup = DdlProcGroup.options[DdlProcGroup.selectedIndex].value;
                            var Layout = GetLayoutProcGrp(ProcGroup);
                            if (Layout == "Layout5" || Layout == "Layout1") {
                                $("#txtBaseqty" + (i - 2)).prop("disabled", false);
                                document.getElementById("DvBaseqty" + (i - 2)).style.display = "none";
                            }
                        }
                    }
                }
                else {
                    for (var i = 2; i <= CellsCountTabProcCost; i++) {
                        var DdlProcGroup = document.getElementById("dynamicddlProcess" + (i - 2));
                        var ProcGroup = DdlProcGroup.options[DdlProcGroup.selectedIndex].value;
                        var Layout = GetLayoutProcGrp(ProcGroup);

                        var IfSubcon = document.getElementById("txtIfTurnkey-VendorName" + (i - 2)).value;
                        var SubvendorIdx = document.getElementById('dynamicddlSubvendorname' + (i - 2)).selectedIndex;
                        var ProcessUOM = document.getElementById('txtProcessUOM' + (i - 2)).value.replace(/\s/g, '').toUpperCase();

                        if (Layout == "Layout5" || Layout == "Layout1") {
                            if ((IfSubcon == "") && (SubvendorIdx <= 0) && ProcessUOM != "CYCLETIMEINSEC/PC" && ProcessUOM != "PCS/LOAD") {
                                if (ProcessUOM != "CYCLETIMEINSEC/PC" && ProcessUOM != "CYCLETIMEINSEC/SHOT" && ProcessUOM != "PCS/LOAD") {
                                    //document.getElementById("DvBaseqty" + (i - 2)).style.display = "block";
                                    document.getElementById("DvBaseqty" + (i - 2)).style.display = "none";
                                }
                                else {
                                    document.getElementById("DvBaseqty" + (i - 2)).style.display = "none";
                                }

                                if ($("#txtBaseqty" + (i - 2)).val() == $("#txtCavity" + 0).val()) {
                                    var ChkBaseqty = document.getElementById("ChkBaseqty" + (i - 2));
                                    ChkBaseqty.checked = true;
                                    $("#txtBaseqty" + (i - 2)).prop("disabled", true);
                                }
                                else {
                                    var ChkBaseqty = document.getElementById("ChkBaseqty" + (i - 2));
                                    ChkBaseqty.checked = false;
                                    $("#txtBaseqty" + (i - 2)).prop("disabled", false);
                                }
                            }
                            else {
                                document.getElementById("DvBaseqty" + (i - 2)).style.display = "none";
                            }
                        }
                        else {
                            document.getElementById("DvBaseqty" + (i - 2)).style.display = "none";
                        }
                    }
                }
            } catch (err) {
                alert("ReturnBaseQtyProcCheckStats : " + err);
            }
        }

        function ReturnBaseQtyProcBaseQtyValue() {
            try {
                var TabMatCost = document.getElementById("Table1");
                var TabProcCost = document.getElementById("TablePC");

                var CellsCountTabMatCost = $("#Table1").find('tr')[0].cells.length;
                var CellsCountTabProcCost = $("#TablePC").find('tr')[0].cells.length;

                var ValSAPSpProcType = $('#HdnSAPSpProcType').val();
                var ValAcsTabMatCost = $('#HdnAcsTabMatCost').val();

                //if (ValSAPSpProcType != "30" && ValAcsTabMatCost != "False")
                //{

                //}
                var MainLayout = $("#hdnLayoutScreen").val();
                if (MainLayout == "Layout5") {
                    if (CellsCountTabMatCost == 2) {
                        for (var i = 2; i <= CellsCountTabProcCost; i++) {
                            var DdlProcGroup = document.getElementById("dynamicddlProcess" + (i - 2));
                            var ProcGroup = DdlProcGroup.options[DdlProcGroup.selectedIndex].value;
                            var Layout = GetLayoutProcGrp(ProcGroup);
                            var ProcessUOM = document.getElementById('txtProcessUOM' + (i - 2)).value.replace(/\s/g, '').toUpperCase();

                            if (Layout == "Layout5" || Layout == "Layout1") {
                                var ChkBaseqty = document.getElementById("ChkBaseqty" + (i - 2));
                                if (ChkBaseqty.checked == true) {
                                    if (ProcessUOM != "CYCLETIMEINSEC/PC") {
                                        $("#txtBaseqty" + (i - 2)).val($("#txtCavity" + 0).val());
                                        $("#txtTotalProcessesCost\\/pcs0").val("");
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    if (CellsCountTabMatCost == 2) {
                        for (var i = 2; i <= CellsCountTabProcCost; i++) {
                            var DdlProcGroup = document.getElementById("dynamicddlProcess" + (i - 2));
                            var ProcGroup = DdlProcGroup.options[DdlProcGroup.selectedIndex].value;
                            var Layout = GetLayoutProcGrp(ProcGroup);
                            var ProcessUOM = document.getElementById('txtProcessUOM' + (i - 2)).value.replace(/\s/g, '').toUpperCase();

                            if (Layout == "Layout5" || Layout == "Layout1") {
                                var ChkBaseqty = document.getElementById("ChkBaseqty" + (i - 2));
                                if (ChkBaseqty.checked == true) {
                                    if (ProcessUOM != "CYCLETIMEINSEC/PC") {
                                        $("#txtBaseqty" + (i - 2)).val($("#txtCavity" + 0).val());
                                        $("#txtTotalProcessesCost\\/pcs0").val("");
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (err) {
                alert("ChkBaseQtyProcessChange : " + err);
            }
        }

        function SubProcgroupChnge(IdNo) {
            try {
                $("#txtMachineId" + IdNo).val("");
                var DdlMacLabIdx = document.getElementById('dynamicddlMachineLabor' + IdNo).selectedIndex;
                var IsSAPCode = $("#hdnIsSAPCode").val();
                if (IsSAPCode.toString() == "False") {
                    if (DdlMacLabIdx == 0) {
                        $("#txtMachineId" + IdNo).prop("disabled", true);
                        var ItemdynamicddlMachine = $('#dynamicddlMachine' + IdNo + ' option').length;
                        if (ItemdynamicddlMachine == 0) {
                            $("#txtMachineId" + IdNo).show();
                            $("#dynamicddlMachine" + IdNo).hide();
                        }
                        else {
                            $("#txtMachineId" + IdNo).hide();
                            $("#dynamicddlMachine" + IdNo).show();
                            document.getElementById('dynamicddlMachine' + IdNo).selectedIndex = 0;
                            MachineChange(IdNo);
                        }
                    }
                    else {
                        $("#dynamicddlMachine" + IdNo).hide();
                        $("#txtMachineId" + IdNo).show();
                        $("#txtMachineId" + IdNo).val("");
                        var tbl = document.getElementById('grdLaborlisthidden');
                        var tbl_row = tbl.rows[1];
                        var tbl_CellVndRate = tbl_row.cells[0];
                        var valueVndRate = tbl_CellVndRate.innerHTML.toString();
                        var tbl_Cell_Is_Std = tbl_row.cells[1];
                        var valueIs_Std = tbl_Cell_Is_Std.innerHTML.toString();
                        $("#txtStandardRate\\/HR" + IdNo).val(valueVndRate);
                        $("#txtVendorRate" + IdNo).val(valueVndRate);
                        $("#txtMachineId" + IdNo).prop("disabled", true);
                        if (valueIs_Std == "Y") {
                            if (valueVndRate == "") {
                                $("#txtVendorRate" + IdNo).prop("disabled", false);
                            }
                            else {
                                $("#txtVendorRate" + IdNo).prop("disabled", true);
                            }
                        }
                        else {
                            $("#txtVendorRate" + IdNo).prop("disabled", false);
                        }
                    }
                }
                else {
                    $("#dynamicddlSubvendorname" + IdNo).prop('selectedIndex', 0);
                    $('#dynamicddlSubvendorname' + IdNo).prop("disabled", false);

                    //$("#dynamicddlMachineLabor" + IdNo).prop('selectedIndex', 0);
                    $('#txtIfTurnkey-VendorName' + IdNo).prop("disabled", false);

                    if (DdlMacLabIdx == 0) {
                        $("#txtMachineId" + IdNo).prop("disabled", true);
                        var ItemdynamicddlMachine = $('#dynamicddlMachine' + IdNo + ' option').length;
                        if (ItemdynamicddlMachine == 0) {
                            $("#txtMachineId" + IdNo).show();
                            $("#dynamicddlMachine" + IdNo).hide();
                        }
                        else {
                            $("#txtMachineId" + IdNo).hide();
                            $("#dynamicddlMachine" + IdNo).show();
                            document.getElementById('dynamicddlMachine' + IdNo).selectedIndex = 0;
                            MachineChange(IdNo);
                        }
                    }
                    else {
                        $("#dynamicddlMachine" + IdNo).hide();
                        $("#txtMachineId" + IdNo).show();
                        $("#txtMachineId" + IdNo).val("");
                        var tbl = document.getElementById('grdLaborlisthidden');
                        var tbl_row = tbl.rows[1];
                        var tbl_CellVndRate = tbl_row.cells[0];
                        var valueVndRate = tbl_CellVndRate.innerHTML.toString();
                        var tbl_Cell_Is_Std = tbl_row.cells[1];
                        var valueIs_Std = tbl_Cell_Is_Std.innerHTML.toString();
                        $("#txtStandardRate\\/HR" + IdNo).val(valueVndRate);
                        $("#txtVendorRate" + IdNo).val(valueVndRate);
                        $("#txtMachineId" + IdNo).prop("disabled", true);
                        if (valueIs_Std == "Y") {
                            if (valueVndRate == "") {
                                $("#txtVendorRate" + IdNo).prop("disabled", false);
                            }
                            else {
                                $("#txtVendorRate" + IdNo).prop("disabled", true);
                            }
                        }
                        else {
                            $("#txtVendorRate" + IdNo).prop("disabled", false);
                        }
                    }

                    //$('#dynamicddlMachineLabor' + IdNo).show();
                    //$('#ddlHideMachine' + IdNo).hide();
                    //$('#txtMachineId' + IdNo).hide();

                    //$('#dynamicddlMachine' + IdNo).show();
                    //$('#dynamicddlHideMachineLabor' + IdNo).hide();

                    document.getElementById("txtTurnkeyCost/pc" + IdNo).value = "";
                    document.getElementById("txtTurnkeyProfit" + IdNo).value = "";
                    document.getElementById("txtProcessCost/pc" + IdNo).value = "";
                    document.getElementById("txtTotalProcessesCost/pcs" + 0).value = "";
                }
                GetProcUom(IdNo);
            } catch (err) {
                alert("SubProcgroupChnge() : " + err);
            }
        }

        function StoreSubprocGroup(IdNo) {
            try {
                document.getElementById("dynamicddlSubProcess" + IdNo).options.length = 0;

                var x = document.getElementById("grdSubProcessGrphidden").rows.length;
                for (i = 1; i < x; i++) {
                    var a = document.getElementById("grdSubProcessGrphidden").rows[i].cells[0].innerHTML;
                    //mySelect.append(a.toString());
                    $('#dynamicddlSubProcess' + IdNo).append($('<option></option>').val(a.toString()).html(a.toString()));
                }

                GetProcUom(IdNo);
            } catch (err) {
                alert("StoreSubprocGroup() : " + err);
            }
        }

        function MachineChange(IdNo) {
            GetVndRate(IdNo);
            Baseqtyupdatebyuom(IdNo);
        }

        function DdlMachineLaborChange(IdNo) {
            try {

                //var selectedText = $(this).find("option:selected").text();
                //dynamicddlMachineLaborMethod(selectedText, 0);

                $("#txtStandardRate\\/HR" + IdNo).val("");
                $("#txtVendorRate" + IdNo).val("");
                $("#txtVendorRate" + IdNo).prop("disabled", false);
                $("#txtMachineId" + IdNo).prop("disabled", true);

                var DdlMacLabIdx = document.getElementById('dynamicddlMachineLabor' + IdNo).selectedIndex;
                var vendorName = $("#lblVName").text();
                var SAPCode = $("#txtpartdesc").val();
                var IsSAPCode = $("#hdnIsSAPCode").val();
                //if (SAPCode.replace('-', '').trim().toString() == "") {
                if (IsSAPCode.toString() == "False") {
                    if (DdlMacLabIdx == 0) {
                        var ItemdynamicddlMachine = $('#dynamicddlMachine' + IdNo + ' option').length;
                        if (ItemdynamicddlMachine == 0) {
                            $("#txtMachineId" + IdNo).show();
                            $("#dynamicddlMachine" + IdNo).hide();
                            $("#txtMachineId" + IdNo).prop("disabled", false);
                        }
                        else {
                            $("#txtMachineId" + IdNo).hide();
                            $("#dynamicddlMachine" + IdNo).show();
                            document.getElementById('dynamicddlMachine' + IdNo).selectedIndex = 0;
                            MachineChange(IdNo);
                        }
                    }
                    else {
                        $("#dynamicddlMachine" + IdNo).hide();
                        $("#txtMachineId" + IdNo).show();
                        $("#txtMachineId" + IdNo).val("");
                        var tbl = document.getElementById('grdLaborlisthidden');
                        var tbl_row = tbl.rows[1];
                        var tbl_CellVndRate = tbl_row.cells[0];
                        var valueVndRate = tbl_CellVndRate.innerHTML.toString();
                        var tbl_Cell_Is_Std = tbl_row.cells[1];
                        var valueIs_Std = tbl_Cell_Is_Std.innerHTML.toString();
                        $("#txtStandardRate\\/HR" + IdNo).val(valueVndRate);
                        $("#txtVendorRate" + IdNo).val(valueVndRate);
                        if (valueIs_Std == "Y") {
                            if (valueVndRate == "") {
                                $("#txtVendorRate" + IdNo).prop("disabled", false);
                            }
                            else {
                                $("#txtVendorRate" + IdNo).prop("disabled", true);
                            }
                        }
                        else {
                            $("#txtVendorRate" + IdNo).prop("disabled", false);
                        }
                    }
                }
                else {
                    if (DdlMacLabIdx == 0) {
                        var x = document.getElementById('dynamicddlProcess' + IdNo).selectedIndex;
                        if (x > 0) {
                            var ProcGroup = $('#dynamicddlProcess' + IdNo).val();
                            var ArrProcGroup = ProcGroup.split('-');
                            var procGrpCode = ArrProcGroup[0].toString().trim();
                            $('#hdnProcGroup').val(procGrpCode.toString());
                            $('#hdnColumTblProcNo').val(IdNo);
                            document.getElementById('BtnMacList').click();


                            var Grid = document.getElementById("grdMachinelisthidden").rows.length;
                            if (Grid <= 1) {
                                alert('no machine list maintained for vendor : ' + vendorName + ' for this process group');
                            }

                            $("#dynamicddlMachine" + IdNo).show();
                            $("#txtMachineId" + IdNo).hide();

                            $('#grdMachinelisthidden tr').each(function () {
                                var checkval = 0;
                                $(this).find('td').each(function () {
                                    // alert($(this).text());
                                    if (checkval == 1) {
                                        $("#txtStandardRate\\/HR" + i).val($(this).text());
                                        $("#txtVendorRate" + i).val($(this).text());
                                        checkval++;
                                    }

                                    if ($(this).text().trim().trimLeft().trimRight() == $("#dynamicddlMachine" + i).find("option:selected").text().trim().trimLeft().trimRight()) {
                                        checkval++;
                                    }

                                    if (checkval == 2) {
                                        if ($(this).html() == "Y") {
                                            $("#txtVendorRate" + i).prop("disabled", true);
                                            $("#hdnVendorActivity").val($(this).html());
                                        }
                                        else if ($(this).html() == "N") {
                                            $("#txtVendorRate" + i).prop("disabled", false);
                                            $("#hdnVendorActivity").val($(this).html());
                                        }

                                    }

                                })
                            });
                        }
                    }
                    else {
                        $("#dynamicddlMachine" + IdNo).hide();
                        $("#txtMachineId" + IdNo).show();

                        var tbl = document.getElementById('grdLaborlisthidden');
                        var tbl_row = tbl.rows[1];
                        var tbl_CellVndRate = tbl_row.cells[0];
                        var valueVndRate = tbl_CellVndRate.innerHTML.toString();
                        var tbl_Cell_Is_Std = tbl_row.cells[1];
                        var valueIs_Std = tbl_Cell_Is_Std.innerHTML.toString();
                        $("#txtStandardRate\\/HR" + IdNo).val(valueVndRate);
                        $("#txtVendorRate" + IdNo).val(valueVndRate);
                        if (valueIs_Std == "Y") {
                            $("#txtVendorRate" + IdNo).prop("disabled", true);
                        }
                        else {
                            $("#txtVendorRate" + IdNo).prop("disabled", false);
                        }
                    }
                }
                $("#txtStandardRate\\/HR" + IdNo).prop("disabled", true);
            }
            catch (err) {
                alert("DdlMachineLaborChange : " + err);
            }
        }

        function GetProcUom(IdNo) {
            try {
                var SubProcGrp = document.getElementById('dynamicddlSubProcess' + IdNo).selectedIndex;
                if (SubProcGrp > -1) {
                    var ProcGrp = $('#dynamicddlProcess' + IdNo).val();
                    $('#hdnProcGroup').val(ProcGrp);

                    $('#hdnColumTblProcNo').val(IdNo);

                    var SubProcGroup = $('#dynamicddlSubProcess' + IdNo).val();
                    $('#hdnSubProcGroup').val(SubProcGroup.toString());
                    document.getElementById('BtnFndProcUom').click();
                }
            } catch (err) {
                alert("GetProcUom() : " + err);
            }
        }

        function StoreProcUOM(IdNo) {
            try {
                var ProcUom = $("#hdnProcUOM").val();
                $('#txtProcessUOM' + IdNo).val(ProcUom);
                Baseqtyupdatebyuom(IdNo);
            } catch (err) {
                alert("StoreProcUOM() : " + err);
            }
        }

        function StoreVnderMachineList(IdNo) {
            try {
                document.getElementById("dynamicddlMachine" + IdNo).options.length = 0;
                var x = document.getElementById("grdMachinelisthidden").rows.length;
                if (x > 1) {
                    for (i = 1; i < x; i++) {
                        var a = document.getElementById("grdMachinelisthidden").rows[i].cells[0].innerHTML;
                        var MacId = document.getElementById("grdMachinelisthidden").rows[i].cells[5].innerHTML;
                        //mySelect.append(a.toString());
                        $('#dynamicddlMachine' + IdNo).append($('<option></option>').val(a.toString()).attr('Mac-Id', MacId).html(a.toString()));
                    }
                    GetVndRate(IdNo);
                }
                else {
                    $("#dynamicddlMachineLabor" + IdNo).prop('selectedIndex', 1);

                    $("#dynamicddlMachine" + IdNo).hide();
                    $("#txtMachineId" + IdNo).show();
                    $("#txtMachineId" + IdNo).prop("disabled", true);

                    var GridgrdLaborlisthiddenLenght = document.getElementById("grdLaborlisthidden").rows.length;
                    if (GridgrdLaborlisthiddenLenght > 1) {
                        var tbl = document.getElementById('grdLaborlisthidden');
                        var tbl_row = tbl.rows[1];
                        var tbl_CellVndRate = tbl_row.cells[0];
                        var valueVndRate = tbl_CellVndRate.innerHTML.toString();
                        var tbl_Cell_Is_Std = tbl_row.cells[1];
                        var valueIs_Std = tbl_Cell_Is_Std.innerHTML.toString();
                        $("#txtStandardRate\\/HR" + IdNo).val(valueVndRate);
                        $("#txtVendorRate" + IdNo).val(valueVndRate);
                        if (valueIs_Std == "Y") {
                            $("#txtVendorRate" + IdNo).prop("disabled", true);
                        }
                        else {
                            $("#txtVendorRate" + IdNo).prop("disabled", false);
                        }
                    }
                    else {
                        $("#txtVendorRate" + IdNo).prop("disabled", false);
                    }
                }
            } catch (err) {
                alert("StoreVnderMachineList() : " + err);
            }
        }

        function GetVndRate(IdNo) {
            try {
                var MachSelIndx = document.getElementById('dynamicddlMachine' + IdNo).selectedIndex;
                if (MachSelIndx > -1) {
                    var ProcGroup = $('#dynamicddlProcess' + IdNo).val();
                    var ArrProcGroup = ProcGroup.split('-');
                    var procGrpCode = ArrProcGroup[0].toString().trim();

                    //var MacIdDdl = $('#dynamicddlMachine' + IdNo).val();
                    var MacIdDdl = $("#dynamicddlMachine" + IdNo + " option:selected").text();
                    var ArrMacId = MacIdDdl.split('-');
                    var MacId = ArrMacId[0].toString().trim();

                    $('#hdnProcGroup').val(procGrpCode.toString());
                    //$('#hdnMachineId').val(MacId.toString());
                    var SelMacID = $('option:selected', "#dynamicddlMachine" + IdNo + "").attr('mac-id');
                    $('#hdnMachineId').val(SelMacID);
                    $('#hdnColumTblProcNo').val(IdNo);
                    document.getElementById('BtnFndVndRate').click();
                }
            } catch (err) {
                alert("GetVndRate() : " + err);
            }
        }

        function StoreVndRate(IdNo) {
            try {
                var StdRate = $("#hdnStdrRate").val();
                $('#txtStandardRate\\/HR' + IdNo).val(StdRate);
                $('#txtVendorRate' + IdNo).val(StdRate);
                var FollowStdRate = $('#hdnFollowStdRate').val();
                if (FollowStdRate == "N") {
                    $('#txtVendorRate' + IdNo).prop("disabled", false);
                }
                else {
                    $('#txtVendorRate' + IdNo).prop("disabled", true);
                }
                var x = document.getElementById('dynamicddlProcess' + IdNo).selectedIndex;
                var y = document.getElementById('dynamicddlSubProcess' + IdNo).selectedIndex;
                if (x > -1 && x > -1) {
                    GetProcUom(IdNo);
                }
            } catch (err) {
                alert("StoreVndRate() : " + err);
            }
        }

        function SubVendorData(IdNo) {
            try {
                $('#txtMachineId' + IdNo).hide();
                $('#hdnColumTblProcNo').val(IdNo.toString());

                var x = document.getElementById('dynamicddlSubvendorname' + IdNo).selectedIndex;
                if (x > 0) {
                    document.getElementById("DvBaseqty" + IdNo).style.display = "none";

                    $('#txtTurnkeyProfit' + IdNo).val($('#hdnHidenProfit').val());

                    $('#txtIfTurnkey-VendorName' + IdNo).prop("disabled", true);

                    $('#dynamicddlHideMachineLabor' + IdNo).show();
                    $('#ddlHideMachine' + IdNo).show();
                    $('#dynamicddlMachineLabor' + IdNo).hide();
                    $('#dynamicddlMachine' + IdNo).hide();


                    //$('#txtStandardRate\\/HR' + IdNo).prop("disabled", true);
                    $('#txtMachineId' + IdNo).prop("disabled", true);
                    $('#txtVendorRate' + IdNo).prop("disabled", true);
                    $('#txtProcessUOM' + IdNo).prop("disabled", true);
                    $('#txtBaseqty' + IdNo).prop("disabled", true);
                    $('#txtDurationperProcessUOM\\(Sec\\)' + IdNo).prop("disabled", true);
                    $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + IdNo).prop("disabled", true);
                    $('#txtTurnkeyCost\\/pc' + IdNo).prop("disabled", false);
                    $('#txtTurnkeyProfit' + IdNo).prop("disabled", true);
                    $('#txtProcessCost\\/pc' + IdNo).prop("disabled", true);

                    $('#txtMachineId' + IdNo).val("");
                    $('#txtIfTurnkey-VendorName' + IdNo).val("");
                    $('#txtStandardRate\\/HR' + IdNo).val("");
                    $('#txtVendorRate' + IdNo).val("");
                    $('#txtProcessUOM' + IdNo).val("");
                    $('#txtBaseqty' + IdNo).val("");
                    $('#txtDurationperProcessUOM\\(Sec\\)' + IdNo).val("");
                    $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + IdNo).val("");

                    $("#dynamicddlHideMachineLabor" + IdNo).show();
                    $("#dynamicddlMachineLabor" + IdNo).hide();
                    $("#ddlHideMachine" + IdNo).hide();
                    $("#txtMachineId" + IdNo).show();
                }
                else {

                    $("#dynamicddlSubProcess" + IdNo).prop("disabled", false);
                    $("#txtIfTurnkey-VendorName" + IdNo).prop("disabled", false);
                    $("#dynamicddlSubvendorname" + IdNo).prop("disabled", false);
                    $("#dynamicddlMachineLabor" + IdNo).prop("disabled", false);
                    $("#dynamicddlMachine" + IdNo).prop("disabled", false);
                    $("#txtStandardRate\\/HR" + IdNo).prop("disabled", true);
                    $("#txtVendorRate" + IdNo).prop("disabled", true);
                    $("#txtProcessUOM" + IdNo).prop("disabled", true);
                    $("#txtBaseqty" + IdNo).prop("disabled", true);
                    $("#txtDurationperProcessUOM\\(Sec\\)" + IdNo).prop("disabled", true);
                    $("#txtEfficiency\\/ProcessYield\\(\\%\\)" + IdNo).prop("disabled", true);
                    $("#txtTurnkeyCost\\/pc" + IdNo).prop("disabled", true);
                    $("#txtTurnkeyProfit" + IdNo).prop("disabled", true);
                    $("#txtProcessCost\\/pc" + IdNo).prop("disabled", true);
                    $("#txtTotalProcessesCost\\/pcs" + IdNo).prop("disabled", true);

                    $("#dynamicddlSubvendorname" + IdNo).prop('selectedIndex', 0);
                    $("#dynamicddlMachineLabor" + IdNo).prop('selectedIndex', 0);


                    document.getElementById("txtIfTurnkey-VendorName" + IdNo).value = "";
                    document.getElementById("txtStandardRate/HR" + IdNo).value = "";
                    document.getElementById("txtVendorRate" + IdNo).value = "";
                    document.getElementById("txtProcessUOM" + IdNo).value = "";
                    document.getElementById("txtBaseqty" + IdNo).value = "";
                    document.getElementById("txtDurationperProcessUOM(Sec)" + IdNo).value = "";
                    document.getElementById("txtEfficiency/ProcessYield(%)" + IdNo).value = "";
                    document.getElementById("txtTurnkeyCost/pc" + IdNo).value = "";
                    document.getElementById("txtTurnkeyProfit" + IdNo).value = "";
                    document.getElementById("txtProcessCost/pc" + IdNo).value = "";
                    document.getElementById("txtTotalProcessesCost/pcs" + 0).value = "";

                    $("#ddlHideMachine" + IdNo).hide();
                    $("#dynamicddlHideMachineLabor" + IdNo).hide();
                    $("#txtMachineId" + IdNo).hide();
                    $("#dynamicddlMachineLabor" + IdNo).show();

                    var MachineLabortxt1 = $('#dynamicddlMachineLabor' + IdNo).find("option:selected").text();
                    if (MachineLabortxt1.toString().toUpperCase() == "MACHINE") {
                        $('#txtMachineId' + IdNo).hide();
                        $('#dynamicddlMachine' + IdNo).show();
                    }
                    else {
                        $('#dynamicddlMachine' + IdNo).hide();
                        $('#txtMachineId' + IdNo).show();
                    }

                    var SubProcGrp = document.getElementById('dynamicddlSubProcess' + IdNo).selectedIndex;
                    if (SubProcGrp > -1) {
                        var SubProcGroup = $('#dynamicddlSubProcess' + IdNo).val();
                        $('#hdnSubProcGroup').val(SubProcGroup.toString());
                    }

                    document.getElementById('BtnFndVndMachineVsProcUom').click();
                }
            } catch (err) {
                alert("SubVendorData() : " + err);
            }
        }

        function PrcGrpVsStokes_Min(ColumTblProcNo, Stk_Min, efficiency) {
            //var DurProcUom = parseInt(efficiency) / parseInt(Stk_Min);
            var DurProcUom = parseInt(60) / parseInt(Stk_Min);
            $('#txtDurationperProcessUOM\\(Sec\\)' + ColumTblProcNo).val(parseFloat(DurProcUom).toFixed(4));
            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + ColumTblProcNo).val(efficiency);
            $('#txtProcessUOM' + ColumTblProcNo).val("STROKES/MIN" + "-" + Stk_Min);

            $('#txtDurationperProcessUOM\\(Sec\\)' + ColumTblProcNo).prop("disabled", true);
            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + ColumTblProcNo).prop("disabled", true);
        }

        function NoProcessgrpVsProcUom(ColumTblProcNo) {

            $('#txtDurationperProcessUOM\\(Sec\\)' + ColumTblProcNo).prop("disabled", false);
            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + ColumTblProcNo).prop("disabled", false);
            $('#txtDurationperProcessUOM\\(Sec\\)' + ColumTblProcNo).val("");
            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + ColumTblProcNo).val("");
        }
    </script>

    <%--sub mat cost script--%>
    <script type="text/javascript">
        function DelSubMatCost(ColumnNo, TtlField) {
            var cntinue = false;
            var BtnDel = document.getElementById("BtnDelSubMatCost" + ColumnNo + "");
            if (BtnDel != null) {
                if (BtnDel.style.display == "none") {
                    cntinue = true;
                }
                else {
                    var Cfrm = confirm("Are you sure you want to delete this item?");
                    if (Cfrm == true) {
                        cntinue = true;
                    }
                }
            }

            if (cntinue == true) {
                submatlcost();
                var Data = $("#hdnSMCTableValues").val();
                var CN = ColumnNo;
                var TtlField = TtlField;
                var temp = new Array();
                temp = Data.split(",");

                var DataFinal = ""
                for (var i = 0; i < temp.length; i++) {
                    if (i >= ((CN * TtlField) - TtlField) && i < (CN * TtlField)) {

                    }
                    else {
                        DataFinal += temp[i] + ",";
                    }
                }

                $("#hdnSMCTableValues").val(DataFinal);
                document.getElementById('BtnDelSubMatCost').click();
            }
        }

        function submatlcost() {
            try {
                var CellsCount = $("#TableSMC").find('tr')[0].cells.length;
                for (var i = 0; i < CellsCount - 1; i++) {
                    // alert("3");
                    //var submatDesc = document.getElementById("txtSub-Mat/T&JDescription" + i).value;
                    var lbTxt = document.getElementById("LbToolOrMachineAmor" + (i + 1));
                    var BtnDel = document.getElementById("BtnDelSubMatCost" + (i + 1));
                    var ok = false;

                    if (lbTxt != null) {
                        if (lbTxt.innerHTML != "Old Tool Amortize" && lbTxt.innerHTML != "Old Machine Amortize") {
                            ok = true;
                        }
                    }
                    else if (BtnDel != null) {
                        ok = true;
                    }

                    if (ok == true) {
                        var e = document.getElementById("dynamicddlSubMat" + i);
                        var submatDesc = "";
                        if (e != null) {
                            submatDesc = e.options[e.selectedIndex].value;
                        }

                        var submatl = document.getElementById("txtSub-Mat/T&JCost" + i).value;
                        var consumption = document.getElementById("txtConsumption(pcs)" + i).value;
                        var submatpc = submatl / consumption;

                        var CheckValsubmatpc = submatpc.toString().toUpperCase();
                        if (CheckValsubmatpc == "NAN" || CheckValsubmatpc == "INFINITY") {
                            submatpc = 0;
                        }
                        document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value = parseFloat(submatpc).toFixed(6);
                    }
                }

                var totalsubcal = 0;
                for (var i = 0; i < CellsCount - 1; i++) {
                    var lbTxt = document.getElementById("LbToolOrMachineAmor" + (i + 1));
                    var BtnDel = document.getElementById("BtnDelSubMatCost" + (i + 1));
                    var ok = false;
                    if (lbTxt != null) {
                        if (lbTxt.innerHTML != "Old Tool Amortize" && lbTxt.innerHTML != "Old Machine Amortize") {
                            ok = true;
                        }
                    }
                    else if (BtnDel != null) {
                        ok = true;
                    }

                    if (ok == true) {
                        var ttlSubprocesscost = document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value;
                        totalsubcal = ((+totalsubcal) + (+ttlSubprocesscost));
                    }
                }

                var CheckValTot = totalsubcal.toString().toUpperCase();
                if (CheckValTot == "NAN" || CheckValTot == "INFINITY") {
                    totalsubcal = 0;
                }
                document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value = parseFloat(totalsubcal).toFixed(5);
                document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value = parseFloat(totalsubcal).toFixed(5);

                submatlCostDataStore();
                ReCalculate();
            } catch (e) {
                alert("submatlcost : " + e)
            }
        }

        function submatlCostDataStore() {
            var CellsCount = $("#TableSMC").find('tr')[0].cells.length;

            var hdnvaltemp = "";

            for (var i = 0; i < CellsCount - 1; i++) {
                var lbTxt = document.getElementById("LbToolOrMachineAmor" + (i + 1));
                var BtnDel = document.getElementById("BtnDelSubMatCost" + (i + 1));
                var ok = false;
                if (lbTxt != null) {
                    if (lbTxt.innerHTML != "Old Tool Amortize" && lbTxt.innerHTML != "Old Machine Amortize") {
                        ok = true;
                    }
                    else {
                        ok = false;
                    }
                }
                else if (BtnDel != null) {
                    ok = true;
                }

                if (ok == true) {
                    //var txtSubMatDesc = document.getElementById("txtSub-Mat/T&JDescription" + i).value;
                    var ddlSubMatDesc = document.getElementById("dynamicddlSubMat" + i);
                    var txtSubMatCost = document.getElementById("txtSub-Mat/T&JCost" + i);
                    var txtConsup = document.getElementById("txtConsumption(pcs)" + i);
                    var txtCostPcs = document.getElementById("txtSub-Mat/T&JCost/pcs" + i);
                    var txttotalcost = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0");

                    var ddlSubMatDescVal = "";
                    var txtSubMatCostVal = "";
                    var txtConsupVal = "";
                    var txtCostPcsVal = "";
                    var txttotalcostVal = "";

                    if (ddlSubMatDesc != null) {
                        ddlSubMatDescVal = ddlSubMatDesc.options[ddlSubMatDesc.selectedIndex].value;
                    }
                    if (txtSubMatCost != null) {
                        txtSubMatCostVal = document.getElementById("txtSub-Mat/T&JCost" + i).value;
                    }
                    if (txtConsup != null) {
                        txtConsupVal = document.getElementById("txtConsumption(pcs)" + i).value;
                    }
                    if (txtCostPcs != null) {
                        txtCostPcsVal = document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value;
                    }
                    if (txttotalcost != null) {
                        txttotalcostVal = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;
                    }

                    hdnvaltemp += (+ "," + ddlSubMatDescVal + "," + txtSubMatCostVal + "," + txtConsupVal + "," + txtCostPcsVal + "," + txttotalcostVal + ",");


                    $("#hdnSMCTableCount").val(i);
                }
            }
            $("#hdnSMCTableValues").val(hdnvaltemp);
        }

        function submatlCostAddColumn() {
            var table = document.getElementById('TableSMC');
            var rowscount = table.rows.length;
            var CurrSMCTable = $("#hdnSMCTableValues").val();
            var hdnvaltemp = "";

            for (var i = 0; i < rowscount - 1; i++) {
                hdnvaltemp += ("" + ",");
            }
            $("#hdnSMCTableValues").val(CurrSMCTable + hdnvaltemp);
        }

        function submatlCostDataStoreWhitoutMachineAmor() {
            var tableSMC = document.getElementById('TableSMC');
            var rowscountSMC = (tableSMC.rows.length - 1);
            var ColumnSMC = $("#TableSMC").find('tr')[0].cells.length;

            var tableMacAmor = document.getElementById('GvMachineAmor');
            var rowsMacAmor = 0;
            if (tableMacAmor != null) {
                if ($("#HdnIsUseMachineAmor").val() == "1") {
                    rowsMacAmor = (tableMacAmor.rows.length - 1);
                }
            }

            var ToolAmortizeCount = 0;
            if ($("#HdnIsUseToolAmortize").val() != "") {
                ToolAmortizeCount = $("#HdnIsUseToolAmortize").val();
            }

            var hdnvaltemp = "";

            for (var i = 0; i < ColumnSMC - 1; i++) {
                var ddlSubMatDesc = document.getElementById("dynamicddlSubMat" + i);
                var txtSubMatCost = document.getElementById("txtSub-Mat/T&JCost" + i);
                var txtConsup = document.getElementById("txtConsumption(pcs)" + i);
                var txtCostPcs = document.getElementById("txtSub-Mat/T&JCost/pcs" + i);
                var txttotalcost = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0");

                var ddlSubMatDescVal = "";
                var txtSubMatCostVal = "";
                var txtConsupVal = "";
                var txtCostPcsVal = "";
                var txttotalcostVal = "";
                if (i >= ToolAmortizeCount) {
                    if (i < (parseInt(ToolAmortizeCount) + parseInt(rowsMacAmor))) {
                        var a = 0;
                    }
                    else {
                        if (ddlSubMatDesc != null) {
                            ddlSubMatDescVal = ddlSubMatDesc.options[ddlSubMatDesc.selectedIndex].value;
                        }
                        if (txtSubMatCost != null) {
                            txtSubMatCostVal = document.getElementById("txtSub-Mat/T&JCost" + i).value;
                        }
                        if (txtConsup != null) {
                            txtConsupVal = document.getElementById("txtConsumption(pcs)" + i).value;
                        }
                        if (txtCostPcs != null) {
                            txtCostPcsVal = document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value;
                        }
                        if (txttotalcost != null) {
                            txttotalcostVal = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;
                        }

                        hdnvaltemp += (+ "," + ddlSubMatDescVal + "," + txtSubMatCostVal + "," + txtConsupVal + "," + txtCostPcsVal + "," + txttotalcostVal + ",");
                    }
                }
                else {
                    if (ddlSubMatDesc != null) {
                        ddlSubMatDescVal = ddlSubMatDesc.options[ddlSubMatDesc.selectedIndex].value;
                    }
                    if (txtSubMatCost != null) {
                        txtSubMatCostVal = document.getElementById("txtSub-Mat/T&JCost" + i).value;
                    }
                    if (txtConsup != null) {
                        txtConsupVal = document.getElementById("txtConsumption(pcs)" + i).value;
                    }
                    if (txtCostPcs != null) {
                        txtCostPcsVal = document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value;
                    }
                    if (txttotalcost != null) {
                        txttotalcostVal = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;
                    }

                    hdnvaltemp += (+ "," + ddlSubMatDescVal + "," + txtSubMatCostVal + "," + txtConsupVal + "," + txtCostPcsVal + "," + txttotalcostVal + ",");
                }
            }
            $("#hdnSMCTableValues").val(hdnvaltemp);
        }
    </script>

    <%--other cost table script--%>
    <script type="text/javascript">
        function DelOthCost(ColumnNo, TtlField) {
            var Cfrm = confirm("Are you sure you want to delete this item?");
            if (Cfrm == true) {
                OthersCostDataStore();
                var Data = $("#hdnOtherValues").val();
                var CN = ColumnNo;
                var TtlField = TtlField;
                var temp = new Array();
                temp = Data.split(",");

                var DataFinal = ""
                for (var i = 0; i < temp.length; i++) {
                    if (i >= ((CN * TtlField) - TtlField) && i < (CN * TtlField)) {

                    }
                    else {
                        DataFinal += temp[i] + ",";
                    }
                }

                $("#hdnOtherValues").val(DataFinal);
                document.getElementById('BtnDelOthCost').click();
            }
        }

        // Calculate Other Cost Table
        function othercost() {
            try {
                var CellsCount = $("#TableOthers").find('tr')[0].cells.length;
                // alert("Others");


                var totalothercal = 0;
                for (var i = 0; i < CellsCount - 1; i++) {

                    var Otheritem = document.getElementById("txtOtherItemCost/pcs" + i).value;
                    var ttlothercost = Otheritem;

                    totalothercal = ((+totalothercal) + (+ttlothercost));
                }
                /// var newtotalOther = fixedDecml(totalothercal, 6).toString();
                var CheckValTot = totalothercal.toString().toUpperCase();
                if (CheckValTot == "NAN" || CheckValTot == "INFINITY") {
                    totalothercal = 0;
                }
                document.getElementById("txtTotalOtherItemCost/pcs0").value = parseFloat(totalothercal).toFixed(5);
                document.getElementById("txtTotalOtherItemCost/pcs-0").value = parseFloat(totalothercal).toFixed(5);

                var ttlmtlcostFinal = document.getElementById("txtTotalMaterialCost/pcs0").value;
                var ttlprocostFinal = document.getElementById("txtTotalProcessesCost/pcs0").value;
                var ttlSubcostFinal = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;
                var ttlOthercostFinal = document.getElementById("txtTotalOtherItemCost/pcs0").value;

                document.getElementById("txtTotalMaterialCost/pcs0").value = ttlmtlcostFinal;

                document.getElementById("txtTotalOtherItemCost/pcs0").value = ttlOthercostFinal;

                var GrandTotalCost = ((+ttlmtlcostFinal) + (+ttlprocostFinal) + (+ttlSubcostFinal) + (+ttlOthercostFinal));

                var GrandTotalCostFinal = parseFloat(GrandTotalCost).toFixed(6);

                //var ttlProfit = document.getElementById("txtProfit(%)0").value;
                //var ttlDiscount = document.getElementById("txtDiscount(%)0").value;

                //var ttlFinalProfit = GrandTotalCost * ((100 + ttlProfit) / 100)
                //var ttlFinalDiscount = GrandTotalCost * ((100 - ttlDiscount) / 100)


                //var ttlfinalprofittrun = parseFloat(ttlFinalProfit.toFixed(4));
                //var ttlfinaldiscounttrun = parseFloat(ttlFinalDiscount.toFixed(4));

                $("#hdnTMatCost").val(parseFloat(ttlmtlcostFinal).toFixed(5).toString());
                $("#hdnTSumMatCost").val(parseFloat(ttlSubcostFinal).toFixed(5).toString());
                $("#hdnTOtherCost").val(parseFloat(ttlOthercostFinal).toFixed(5).toString());
                $("#hdnTGTotal").val(parseFloat(GrandTotalCostFinal).toFixed(5).toString());

                OthersCostDataStore();
                ReCalculate();
            } catch (err) {
                alert("othercost() : " + err);
            }
        }

        //Session Maintain for Other Cost Table
        function OthersCostDataStore() {
            try {
                var CellsCount = $("#TableOthers").find('tr')[0].cells.length;

                var hdnvaltemp = "";

                for (var i = 0; i < CellsCount - 1; i++) {

                    //var txtOthersDesc = document.getElementById("txtItemsDescription" + i).value;
                    var e = document.getElementById("dynamicddlOthCost" + i);
                    var txtOthersDesc = e.options[e.selectedIndex].value;

                    var txtOthersCost = document.getElementById("txtOtherItemCost/pcs" + i).value;
                    var txttotalcost = document.getElementById("txtTotalOtherItemCost/pcs0").value;


                    hdnvaltemp += (+ "," + txtOthersDesc + "," + txtOthersCost + "," + txttotalcost + ",");

                    $("#hdnOthersTableCount").val(i);
                }

                $("#hdnOtherValues").val(hdnvaltemp);
            } catch (err) {
                alert("OthersCostDataStore() : " + err);
            }
        }
    </script>

    <%--table unit cost script--%>
    <script type="text/javascript">

        function returnProfDiscGA() {
            try {
                var txtDisc = $("#txtDiscount\\(\\%\\)0");
                var txtProf = $("#txtProfit\\(\\%\\)0");
                var txtGA = $("#GA\\(\\%\\)0");

                var HdnDisc = $("#hdnDiscount").val().replace("NaN", "");
                var HdnProf = $("#hdnProfit").val().replace("NaN", "");
                var HdnGA = $("#hdnGA").val().replace("NaN", "");

                if (txtDisc != null) {
                    if (HdnDisc == "0") {
                        HdnDisc = "";
                    }
                    txtDisc.val(HdnDisc);
                }
                if (txtProf != null) {
                    if (txtProf == "0") {
                        txtProf = "";
                    }
                    txtProf.val(HdnProf);
                }
                if (txtGA != null) {
                    if (txtGA == "0") {
                        txtGA = "";
                    }
                    txtGA.val(HdnGA);
                }
            }
            catch (err) {
                alert(err + ": returnProfDiscGA")
            }
        }

        function RestoreDataTbaleUnit() {
            try {

                var hdnVendorType = $("#hdnVendorType").val();
                var hdnMassRevision = $("#hdnMassRevision").val();

                if (hdnVendorType == "TeamShimano") {
                    var TotalOtherItemCost = document.getElementById('txtTotalOtherItemCost/pcs0').value;
                    document.getElementById('txtTotalOtherItemCost/pcs-0').value = TotalOtherItemCost;

                    var TxtProfit = document.getElementById('txtProfit(%)0');
                    var TxtGA = document.getElementById('GA(%)0');

                    //if ($('#btnAddOtherCost').css('display') == 'none' || $('#btnAddOtherCost').css("visibility") == "hidden") {
                    //    TxtProfit.disabled = true;
                    //    TxtGA.disabled = true;
                    //}
                    //else {
                    //    TxtProfit.disabled = false;
                    //    TxtGA.disabled = false;
                    //}

                    TxtProfit.disabled = true;
                    TxtGA.disabled = true;
                }
                else {
                    var txtProfit = parseFloat(document.getElementById('txtProfit(%)0').value);
                    var txtDiscount = parseFloat(document.getElementById('txtDiscount(%)0').value)
                    if (txtProfit.toString() == "NaN") {
                        txtProfit = 0;
                    }
                    if (txtDiscount.toString() == "NaN") {
                        txtDiscount = 0;
                    }
                    if (document.getElementById("txtTotalProcessesCost/pcs0") != null && document.getElementById("txtTotalSub-Mat/T&JCost/pcs0") != null) {
                        document.getElementById("txtTotalProcessesCost/pcs-0").value = document.getElementById("txtTotalProcessesCost/pcs0").value;
                        document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;
                    }

                    document.getElementById("txtFinalQuotePrice/pcs0").value = $("#hdnTMatCost").val();
                    document.getElementById("txtFinalQuotePrice/pcs1").value = $("#hdnTProCost").val();
                    document.getElementById("txtFinalQuotePrice/pcs2").value = $("#hdnTSumMatCost").val();
                    document.getElementById("txtFinalQuotePrice/pcs3").value = $("#hdnTOtherCost").val();
                    document.getElementById("txtFinalQuotePrice/pcs4").value = $("#hdnTGTotal").val();
                    document.getElementById("txtFinalQuotePrice/pcs4").value = $("#hdnTFinalQPrice").val();

                    var GtValue = parseFloat(document.getElementById("txtGrandTotalCost/pcs0").value);
                    var FinalValue = parseFloat(document.getElementById("txtFinalQuotePrice/pcs4").value);
                    var NetProfDisc = (((FinalValue - GtValue) / FinalValue) * 100);
                    if (NetProfDisc == "NaN") {
                        NetProfDisc = 0
                    }
                    document.getElementById("txtNetProfit(%)0").value = NetProfDisc.toFixed(1) + ' %';
                    document.getElementById("HdnNetProfnDisc").value = NetProfDisc.toFixed(1);

                    var textbox = document.getElementById('txtProfit(%)0');
                    var textbox2 = document.getElementById('txtDiscount(%)0');

                    var QuoteRef = $("#hdnQuoteNoRef").val();
                    if (QuoteRef.toString() == "") {

                        var profit = document.getElementById('txtProfit(%)0').value;
                        var disc = document.getElementById('txtDiscount(%)0').value;
                        if (profit == "" && disc == "") {
                            if ($('#btnaddProcessCost').css('display') == 'none' || $('#btnaddProcessCost').css("visibility") == "hidden") {
                                textbox.disabled = true;
                                textbox2.disabled = true;
                            }
                            else {
                                textbox.disabled = false;
                                textbox2.disabled = false;
                            }
                        }
                        else if (profit != "") {
                            if ($('#btnaddProcessCost').css('display') == 'none' || $('#btnaddProcessCost').css("visibility") == "hidden") {
                                textbox.disabled = true;
                                textbox2.disabled = true;
                            }
                            else {
                                if (profit == "" && disc == "") {
                                    textbox.disabled = false;
                                    textbox2.disabled = false;
                                }
                                else if (profit != "") {
                                    textbox.disabled = false;
                                    textbox2.disabled = true;
                                }
                                else {
                                    textbox.disabled = true;
                                    textbox2.disabled = false;
                                }
                            }
                        }
                        else {
                            if ($('#btnaddProcessCost').css('display') == 'none' || $('#btnaddProcessCost').css("visibility") == "hidden") {
                                textbox.disabled = true;
                                textbox2.disabled = true;
                            }
                            else {
                                if (profit == "" && disc == "") {
                                    textbox.disabled = false;
                                    textbox2.disabled = false;
                                }
                                else if (profit != "") {
                                    textbox.disabled = false;
                                    textbox2.disabled = true;
                                }
                                else {
                                    textbox.disabled = true;
                                    textbox2.disabled = false;
                                }
                            }
                        }
                    }
                    else {
                        var AcsTabProcCost = $("#HdnAcsTabProcCost").val();
                        if (AcsTabProcCost == "True") {
                            var profit = document.getElementById('txtProfit(%)0').value;
                            var disc = document.getElementById('txtDiscount(%)0').value;
                            if (profit == "" && disc == "") {
                                if ($('#btnaddProcessCost').css('display') == 'none' || $('#btnaddProcessCost').css("visibility") == "hidden") {
                                    textbox.disabled = true;
                                    textbox2.disabled = true;
                                }
                                else {
                                    textbox.disabled = false;
                                    textbox2.disabled = false;
                                }
                            }
                            else if (profit != "") {
                                if ($('#btnaddProcessCost').css('display') == 'none' || $('#btnaddProcessCost').css("visibility") == "hidden") {
                                    textbox.disabled = true;
                                    textbox2.disabled = true;
                                }
                                else {
                                    if (profit == "" && disc == "") {
                                        textbox.disabled = false;
                                        textbox2.disabled = false;
                                    }
                                    else if (profit != "") {
                                        textbox.disabled = false;
                                        textbox2.disabled = true;
                                    }
                                    else {
                                        textbox.disabled = true;
                                        textbox2.disabled = false;
                                    }
                                }
                            }
                            else {
                                if ($('#btnaddProcessCost').css('display') == 'none' || $('#btnaddProcessCost').css("visibility") == "hidden") {
                                    textbox.disabled = true;
                                    textbox2.disabled = true;
                                }
                                else {
                                    if (profit == "" && disc == "") {
                                        textbox.disabled = false;
                                        textbox2.disabled = false;
                                    }
                                    else if (profit != "") {
                                        textbox.disabled = false;
                                        textbox2.disabled = true;
                                    }
                                    else {
                                        textbox.disabled = true;
                                        textbox2.disabled = false;
                                    }
                                }
                            }
                        }
                        else {
                            textbox.disabled = true;
                            textbox2.disabled = true;
                        }
                    }
                }

                if (hdnMassRevision != "") {
                    if (hdnVendorType == "TeamShimano") {
                        var txtGA = $("#GA\\(\\%\\)0");
                        var txtprof = document.getElementById('txtProfit(%)0');
                        txtprof.disabled = true;
                        txtGA.disabled = true;
                    }
                    else {
                        var txtprof = document.getElementById('txtProfit(%)0');
                        var txtdisc = document.getElementById('txtDiscount(%)0');
                        txtprof.disabled = true;
                        txtdisc.disabled = true;
                    }
                }
            }
            catch (err) {
                alert(err + ':RestoreDataTbaleUnit')
            }
        }

        function AllRecalculate() {
            try {
                var hdnMassRevision = $("#hdnMassRevision").val();
                if (hdnMassRevision != "") {
                    var TotMatOld = $("#HdnMAssTotMatCost").val();
                    var TotMatNew = $("#txtTotalMaterialCost\\/pcs0").val();
                    //if (TotMatOld == TotMatNew) {
                    //    MCDataStore();
                    //}
                    //else {
                    //    $("#Button2").click();
                    //}
                    MCDataStore();
                    ProcessCostDataStore();
                    submatlCostDataStore();
                    OthersCostDataStore();
                }
                else {
                    //$("#Button2").click();
                    //$("#Button6").click();
                    //$("#Button4").click();
                    //$("#Button5").click();
                    MCDataStore();
                    ProcessCostDataStore();
                    submatlCostDataStore();
                    OthersCostDataStore();
                }
            } catch (err) {
                alert("AllRecalculate() : " + err);
            }
        }

        function SaveAsDraftData() {
            MCDataStore();
            ProcessCostDataStore();
            submatlCostDataStore();
            OthersCostDataStore();
        }

        function ReCalculate() {
            try {


                var TotMatCost = parseFloat(document.getElementById('txtTotalMaterialCost/pcs-0').value);
                if (TotMatCost.toString() == "" || TotMatCost.toString() == "NaN") {
                    TotMatCost = 0;
                }
                var TotProCost = 0;
                var TotSubMatCost = 0;
                if (document.getElementById("txtTotalProcessesCost/pcs0") != null && document.getElementById("txtTotalSub-Mat/T&JCost/pcs0") != null) {
                    document.getElementById("txtTotalProcessesCost/pcs-0").value = document.getElementById("txtTotalProcessesCost/pcs0").value;
                    document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;

                    TotProCost = parseFloat(document.getElementById('txtTotalProcessesCost/pcs-0').value)
                    if (TotProCost.toString() == "" || TotProCost.toString() == "NaN") {
                        TotProCost = 0;
                    }
                    TotSubMatCost = parseFloat(document.getElementById('txtTotalSub-Mat/T&JCost/pcs-0').value);
                    if (TotSubMatCost.toString() == "" || TotProCost.toString() == "NaN") {
                        TotSubMatCost = 0;
                    }

                }
                var TotOthrCost = parseFloat(document.getElementById('txtTotalOtherItemCost/pcs-0').value)
                if (TotOthrCost.toString() == "" || TotProCost.toString() == "NaN") {
                    TotOthrCost = 0;
                }
                var ValueAfterProforDisc = parseFloat(document.getElementById('txtFinalQuotePrice/pcs1').value)
                if (ValueAfterProforDisc.toString() == "" || ValueAfterProforDisc.toString() == "NaN") {
                    ValueAfterProforDisc = 0;
                }
                var hdnVendorType = $("#hdnVendorType").val();
                var txtProfit = "";
                var txtDiscount = "";
                var GA = "";
                if (hdnVendorType == "TeamShimano") {
                    if (document.getElementById('GA(%)0').value == "") {
                        GA = 0;
                    }
                    else {
                        GA = parseFloat(document.getElementById('GA(%)0').value);
                    }
                    if (document.getElementById('txtProfit(%)0').value == "") {
                        txtProfit = 0;
                    }
                    else {
                        txtProfit = parseFloat(document.getElementById('txtProfit(%)0').value);
                    }

                    if (document.getElementById('txtTotalProcessesCost/pcs-0').value != "") {
                        document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(TotProCost).toFixed(5);
                    }
                }
                else {
                    txtProfit = document.getElementById('txtProfit(%)0').value;
                    txtDiscount = document.getElementById('txtDiscount(%)0').value;
                    if (txtProfit == "") {
                        txtProfit = 0;
                    }
                    else {
                        txtProfit = parseFloat(txtProfit);
                    }
                    if (txtDiscount == "") {
                        txtDiscount = 0;
                    }
                    else {
                        txtDiscount = parseFloat(txtDiscount);
                    }

                    var textbox = document.getElementById('txtProfit(%)0');
                    var textbox2 = document.getElementById('txtDiscount(%)0');

                    if (txtProfit == 0 && txtDiscount == 0) {
                        if ($('#btnaddProcessCost').css('display') == 'none' || $('#btnaddProcessCost').css("visibility") == "hidden") {
                            textbox.disabled = true;
                            textbox2.disabled = true;
                        }
                        else {
                            textbox.disabled = false;
                            textbox2.disabled = false;
                        }
                    }
                    else if (txtProfit != 0 && txtDiscount == 0) {
                        if ($('#btnaddProcessCost').css('display') == 'none' || $('#btnaddProcessCost').css("visibility") == "hidden") {
                            textbox.disabled = true;
                            textbox2.disabled = true;
                        }
                        else {
                            textbox.disabled = false;
                            textbox2.disabled = true;
                        }
                    }
                    else {
                        if ($('#btnaddProcessCost').css('display') == 'none' || $('#btnaddProcessCost').css("visibility") == "hidden") {
                            textbox.disabled = true;
                            textbox2.disabled = true;
                        }
                        else {
                            textbox.disabled = true;
                            textbox2.disabled = false;
                        }
                    }


                    if (textbox.disabled == false && textbox2.disabled == false) {
                        if (document.getElementById('txtTotalProcessesCost/pcs-0').value != "") {
                            document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(TotProCost).toFixed(5);
                        }
                    }
                    else {
                        if (textbox.disabled == true && textbox.value == "") {
                            //final value when Discount change
                            var WithDisc = parseFloat(TotProCost) - (parseFloat(TotProCost) * (parseFloat(txtDiscount) / 100));

                            var CheckValWithDisc = WithDisc.toString().toUpperCase();
                            if (CheckValWithDisc == "NAN" || CheckValWithDisc == "INFINITY") {
                                WithDisc = 0;
                            }

                            if (document.getElementById('txtTotalProcessesCost/pcs-0').value != "") {
                                if (WithDisc == 0) {
                                    document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(TotProCost).toFixed(5);
                                }
                                else {
                                    document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(WithDisc).toFixed(5);
                                }
                            }
                        }
                        else {
                            //final value whit profit

                            var WithProfit = parseFloat(TotProCost) + (parseFloat(TotProCost) * (parseFloat(txtProfit) / 100));

                            var CheckValWithProfit = WithProfit.toString().toUpperCase();
                            if (CheckValWithProfit == "NAN" || CheckValWithProfit == "INFINITY") {
                                WithProfit == 0;
                            }

                            if (document.getElementById('txtTotalProcessesCost/pcs-0').value != "") {
                                if (WithProfit == 0) {
                                    document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(TotProCost).toFixed(5);
                                }
                                else {
                                    document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(WithProfit).toFixed(5);
                                }
                            }
                        }
                    }
                }

                if (document.getElementById('txtTotalMaterialCost/pcs-0').value != "") {
                    document.getElementById("txtFinalQuotePrice/pcs0").value = parseFloat(TotMatCost).toFixed(5);
                }

                var GtValue1 = (TotMatCost) + (parseFloat(document.getElementById('txtTotalProcessesCost/pcs-0').value)) + (parseFloat(document.getElementById('txtTotalSub-Mat/T&JCost/pcs-0').value)) + (parseFloat(document.getElementById('txtTotalOtherItemCost/pcs-0').value));
                var CheckValGtValue1 = GtValue1.toString().toUpperCase();
                if (CheckValGtValue1 == "NAN" || CheckValGtValue1 == "INFINITY") {
                    GtValue1 = 0;
                }
                document.getElementById("txtGrandTotalCost/pcs0").value = parseFloat(GtValue1).toFixed(5);
                document.getElementById("txtFinalQuotePrice/pcs2").value = document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value;
                document.getElementById("txtFinalQuotePrice/pcs3").value = document.getElementById("txtTotalOtherItemCost/pcs-0").value;


                var a = parseFloat(document.getElementById('txtFinalQuotePrice/pcs0').value)
                var b = parseFloat(document.getElementById('txtFinalQuotePrice/pcs1').value)
                var c = parseFloat(document.getElementById('txtFinalQuotePrice/pcs2').value)
                var d = parseFloat(document.getElementById('txtFinalQuotePrice/pcs3').value)
                var SumValFinal = a + b + c + d

                var CheckValSumValFinal = SumValFinal.toString().toUpperCase();
                if (CheckValSumValFinal == "NAN" || CheckValSumValFinal == "INFINITY") {
                    SumValFinal = 0;
                }
                document.getElementById("txtFinalQuotePrice/pcs4").value = parseFloat(SumValFinal).toFixed(5);

                var GtValue = "";
                var FinalValue = "";
                if (hdnVendorType == "TeamShimano") {
                    CalculationFinalCost();
                    FinalValue = $("#txtFinalQuotePrice\\/pcs4").val();
                }
                else {
                    GtValue = parseFloat(document.getElementById("txtGrandTotalCost/pcs0").value);
                    FinalValue = parseFloat(document.getElementById("txtFinalQuotePrice/pcs4").value);
                    var NetProfDisc = (((FinalValue - GtValue) / FinalValue) * 100);

                    var CheckValNetProfDisc = NetProfDisc.toString().toUpperCase();
                    if (CheckValNetProfDisc == "NAN" || CheckValNetProfDisc == "INFINITY") {
                        NetProfDisc = 0;
                    }
                    document.getElementById("txtNetProfit(%)0").value = NetProfDisc.toFixed(1) + ' %';
                    document.getElementById("HdnNetProfnDisc").value = NetProfDisc.toFixed(1);
                }

                var hdnMassRevision = $("#hdnMassRevision").val();
                if (hdnMassRevision != "") {
                    var TotMat = $("#txtTotalMaterialCost\\/pcs0").val();
                    var TotProc = $("#HdnMAssTotProcCost").val();
                    var TotSubMat = $("#HdnMAssTotSubMatCost").val();
                    var TotOth = $("#HdnMAssTotOthCost").val();
                    document.getElementById('txtTotalMaterialCost/pcs0').value = TotMat;
                    document.getElementById('txtTotalProcessesCost/pcs0').value = TotProc;
                    document.getElementById('txtTotalSub-Mat/T&JCost/pcs0').value = TotSubMat;
                    document.getElementById('txtTotalOtherItemCost/pcs0').value = TotOth;

                    document.getElementById('txtTotalMaterialCost/pcs-0').value = TotMat;
                    document.getElementById('txtTotalProcessesCost/pcs-0').value = TotProc;
                    document.getElementById('txtTotalSub-Mat/T&JCost/pcs-0').value = TotSubMat;
                    document.getElementById('txtTotalOtherItemCost/pcs-0').value = TotOth;

                    if (document.getElementById('txtTotalMaterialCost/pcs-0').value != "") {
                        document.getElementById('txtFinalQuotePrice/pcs0').value = TotMat;
                    }

                    if (document.getElementById('txtTotalProcessesCost/pcs-0').value != "") {
                        document.getElementById('txtFinalQuotePrice/pcs1').value = TotProc;
                    }


                    document.getElementById('txtFinalQuotePrice/pcs2').value = TotSubMat;
                    document.getElementById('txtFinalQuotePrice/pcs3').value = TotOth;

                    var GrTot = parseFloat(TotMat) + parseFloat(TotProc) + parseFloat(TotSubMat) + parseFloat(TotOth);
                    var FinTot = parseFloat(TotMat) + parseFloat(TotProc) + parseFloat(TotSubMat) + parseFloat(TotOth);
                    document.getElementById('txtGrandTotalCost/pcs0').value = parseFloat(GrTot).toFixed(5);
                    document.getElementById('txtFinalQuotePrice/pcs4').value = parseFloat(FinTot).toFixed(5);

                    if (hdnVendorType == "TeamShimano") {
                        var txtGA = $("#GA\\(\\%\\)0");
                        var txtprof = document.getElementById('txtProfit(%)0');
                        txtprof.disabled = true;
                        txtGA.disabled = true;
                    }
                    else {
                        var txtprof = document.getElementById('txtProfit(%)0');
                        var txtdisc = document.getElementById('txtDiscount(%)0');
                        txtprof.disabled = true;
                        txtdisc.disabled = true;
                    }

                    MCDataStore();
                    ProcessCostDataStore();
                    submatlCostDataStore();
                    OthersCostDataStore();
                }

                $("#hdnTMatCost").val(document.getElementById("txtFinalQuotePrice/pcs0").value);
                $("#hdnTProCost").val(document.getElementById("txtFinalQuotePrice/pcs1").value);
                $("#hdnTSumMatCost").val(document.getElementById("txtFinalQuotePrice/pcs2").value);
                $("#hdnTOtherCost").val(document.getElementById("txtFinalQuotePrice/pcs3").value);
                $("#hdnTGTotal").val(document.getElementById("txtGrandTotalCost/pcs0").value);
                if (txtProfit == 0) {
                    txtProfit = "";
                }
                if (txtDiscount == 0) {
                    txtDiscount = "";
                }
                $("#hdnProfit").val(txtProfit);
                $("#hdnDiscount").val(txtDiscount);
                $("#hdnTFinalQPrice").val(FinalValue);

                Layout7Condition();

            }
            catch (err) {
                alert(err + 'ReCalculate')
            }
        }

        function CalculationFinalCost() {
            try {

                var TotMatCost = $("#txtTotalMaterialCost\\/pcs-0").val();
                if (TotMatCost.toString() == "" || TotMatCost.toString() == "NaN") {
                    TotMatCost = 0;
                }
                var TotProcCost = $("#txtTotalProcessesCost\\/pcs-0").val();
                if (TotProcCost.toString() == "" || TotProcCost.toString() == "NaN") {
                    TotProcCost = 0;
                }
                var TotSubMatCost = $("#txtTotalSub-Mat\\/T\\&JCost\\/pcs-0").val();
                if (TotSubMatCost.toString() == "" || TotSubMatCost.toString() == "NaN") {
                    TotSubMatCost = 0;
                }
                var TotOtherCost = $("#txtTotalOtherItemCost\\/pcs-0").val();
                if (TotOtherCost.toString() == "" || TotOtherCost.toString() == "NaN") {
                    TotOtherCost = 0;
                }
                var GtCost = $("#txtGrandTotalCost\\/pcs0").val();
                if (GtCost.toString() == "" || GtCost.toString() == "NaN") {
                    GtCost = 0;
                }
                var GA = parseFloat(document.getElementById('GA(%)0').value);
                if (GA.toString() == "" || GA.toString() == "NaN") {
                    GA = 0;
                }
                var Profit = parseFloat(document.getElementById('txtProfit(%)0').value);
                if (Profit.toString() == "" || Profit.toString() == "NaN") {
                    Profit = 0;
                }

                var CostAfterGA = parseFloat(GtCost) + (parseFloat(GtCost) * (parseFloat(GA) / 100));
                var CostAfterProfit = parseFloat(CostAfterGA) + (parseFloat(CostAfterGA) * (parseFloat(Profit) / 100));
                var FinalTotOtherCost = parseFloat(TotOtherCost) + (parseFloat(CostAfterProfit) - parseFloat(GtCost));
                var FinalGTCost = parseFloat(TotMatCost) + parseFloat(TotProcCost) + parseFloat(TotSubMatCost) + parseFloat(FinalTotOtherCost);

                document.getElementById("txtFinalQuotePrice/pcs0").value = parseFloat(TotMatCost).toFixed(5).toString();
                document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(TotProcCost).toFixed(5).toString();
                document.getElementById("txtFinalQuotePrice/pcs2").value = parseFloat(TotSubMatCost).toFixed(5).toString();
                document.getElementById("txtFinalQuotePrice/pcs3").value = parseFloat(FinalTotOtherCost).toFixed(5).toString();
                document.getElementById("txtFinalQuotePrice/pcs4").value = parseFloat(FinalGTCost).toFixed(5).toString();
                $("#hdnGA").val(GA);
            }
            catch (err) {
                alert("CalculationFinalCost" + err);
            }
        }

    </script>

    <%--DatePitcker--%>
    <script type="text/javascript">
        function DatePitcker() {
            try {
                (function ($) {
                    $(".form_datetime").datetimepicker({
                        //format: "dd-MM-yyyy - hh:ii",
                        fontAwesome: 'font-awesome',
                        startDate: new Date(),
                        format: "dd-mm-yyyy",
                        autoclose: true,
                        todayBtn: true,
                        todayHighlight: true,
                        minView: 2
                    });
                })(jQuery);
            } catch (e) {
                alert("DatePitcker : " + e);
            }
        }
    </script>

    <%--CompareDueDateandDefaultDueDate()--%>
    <script type="text/javascript">
        function CompareDueDateandDefaultDueDate() {
            try {

                var StrDueDate = document.getElementById("txtfinal").value.toString().replace("-", ".");
                var strDefaultDueDate = document.getElementById("TxtDefaultDueDate").value.toString().replace("-", ".");
                if (strDefaultDueDate != "") {
                    var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
                    var dtDueDate = new Date(StrDueDate.replace(pattern, '$3-$2-$1'));
                    var dtDefaultDueDate = new Date(strDefaultDueDate.replace(pattern, '$3-$2-$1'));

                    if (dtDueDate < dtDefaultDueDate) {
                        alert("Due Dt Next Rev cannot be small than Quote Next Revison Date");
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return true;
                }
            }
            catch (err) {
                alert(err + ": CompareDueDateandDefaultDueDate");
                return false;
            }
        }
    </script>

    <%--attachment script--%>
    <script type="text/javascript">
        function ClcBtnFlUpload() {
            try {
                $('#FlAttachment').trigger('click');
            }
            catch (err) {
                alert(": ClcBtnFlUpload" + err)
            }
        }

        function CheckFileSize() {
            try {
                //1 mb = 1024kb
                //1 kb = 1024 byte
                //1 mb = 1024 * 1024 = 1048576
                //3 mb = 3 * 1048576 = 3145728
                var fileSize = document.getElementById("FlAttachment").files[0].size;
                if (fileSize <= 3145728) {
                    debugger;
                    var format = /[!@#$%^&*()+\=\[\]{};':"\\|,.<>\/?]/;
                    var objRE = new RegExp(/([^\/\\]+)$/);
                    var FlName = objRE.exec(document.getElementById('FlAttachment').value);
                    var ArrFilename = FlName[0].toString().split(".");
                    var Ext = ArrFilename[ArrFilename.length-1];
                    var NewFlName = FlName[0].replace('.' + Ext + '', '');
                    if (format.test(NewFlName)) {
                        alert("Invalid File Name. \n File name cannot contain below character : \n [ ! @ # $ % ^ & * ( ) + \ = \ [ \ ] { } ; ' : \" \\ | , . < > \ / ? ] ");
                        $("#FlAttachment").val("");
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    var MB = fileSize / 1048576;
                    document.getElementById("FlAttachment").value = '';
                    alert("File is too large, Maximum file size 3 Mb. File  Size: " + MB.toFixed(1) + " Mb");
                    return false;
                }
            }
            catch (err) {
                alert(err + ": CheckFileSize");
                return false;
            }
        }

        function CheckFileUpload() {
            try {
                var FL = document.getElementById("LbFlName").innerHTML;
                if (FL == "No File") {
                    alert('No File to download');
                    return false;
                }
                else {
                    return true;
                }
            }
            catch (err) {
                alert("CheckFileUpload" + err)
            }
        }

        function UploadFile() {
            try {
                if (CheckFileSize() == true) {
                    var txtDisc = $("#txtDiscount\\(\\%\\)0");
                    var txtProf = $("#txtProfit\\(\\%\\)0");
                    var txtGA = $("#GA\\(\\%\\)0");

                    if (txtDisc != null) {
                        $("#hdnDiscount").val($("#txtDiscount\\(\\%\\)0").val());
                    }
                    if (txtProf != null) {
                        $("#hdnProfit").val($("#txtProfit\\(\\%\\)0").val());
                    }
                    if (txtGA != null) {
                        $("#hdnGA").val($("#GA\\(\\%\\)0").val());
                    }

                    $("#HdnFirsLoad").val("2");
                    send();
                }
                //$("#BtnUpload").click();

            } catch (err) {
                alert("UploadFile() : " + err);
            }
        }

        function send() {
            try {
                var fd = new FormData();
                fd.append("fileToUpload", document.getElementById('FlAttachment').files[0]);
                var xhr = new XMLHttpRequest();
                xhr.open("POST", "SaveFileUploadAttachment.aspx");
                xhr.send(fd);

                var flupload = document.getElementById('FlAttachment').files[0];
                var flname = flupload.name;

                $('#LbFlName').text(flname);
            } catch (err) {
                alert("UploadFile() : " + err);
            }
        }

        function DelAttachment() {
            try {
                document.getElementById("FlAttachment").value = '';
                $('#LbFlName').text("No File");

                var fd = new FormData();
                fd.append("fileToUpload", document.getElementById('FlAttachment').files[0]);
                var xhr = new XMLHttpRequest();
                xhr.open("POST", "SaveFileUploadAttachment.aspx");
                xhr.send(fd);
            } catch (err) {
                alert("DelAttachment() : " + err);
            }
        }

        function Downloadfile() {
            try {
                var flupload = document.getElementById('FlAttachment').files[0];
                if (flupload != null) {
                    var flname = flupload.name;
                    var flType = flupload.type;
                    //------
                    var fr = new FileReader();
                    fr.readAsDataURL(flupload);

                    var blob = new Blob([flupload], { type: flType });

                    var objectURL = window.URL.createObjectURL(blob);
                    //console.log(objectURL);

                    if (navigator.appVersion.toString().indexOf('.NET') > 0) {
                        window.navigator.msSaveOrOpenBlob(blob, flname);
                    } else {
                        var link = document.createElement('a');
                        link.href = objectURL;
                        link.download = flname;
                        document.body.appendChild(link);
                        link.click();
                        link.remove();
                    }
                }
                else {
                    alert("No File Attached !!");
                }
            } catch (err) {
                alert("Downloadfile() : " + err);
            }
        }
    </script>

    <%--CekMachineAmortizeFromProcCostTable(IsFromCalculteSubMat)--%>
    <script type="text/javascript">
        function CekMachineAmortizeFromProcCostTable(IsFromCalculteSubMat)
        {
            try {
                var IsFromCalculteSubMat = IsFromCalculteSubMat;
                //document.getElementById("HdnMachinListprocCost").value;
                if ($("#HdnAcsTabSubMatCost").val() == "False" && $("#HdnAcsTabSubMatCost").val() == "0") {
                    return false;
                }
                else {

                    var tablePC = $("#TablePC");
                    var tableMacAmor = document.getElementById('GvMachineAmor');
                    var UseMacAmor = false;
                    var MachineList = [];
                    if (tablePC != null && tableMacAmor != null) {
                        var columncounttablePC = tablePC.find('tr')[0].cells.length;
                        var rowscounttableMacAmor = tableMacAmor.rows.length;

                        for (var i = 1; i < columncounttablePC ; i++) {
                            var dynamicddlMachineLabor = $("#dynamicddlMachineLabor" + (i - 1) + "");
                            if (dynamicddlMachineLabor != null) {
                                if (dynamicddlMachineLabor.val() == "Machine") {
                                    var macidddl = $("#dynamicddlMachine" + (i - 1) + "").find(':selected').attr('mac-id');
                                    var ProcGrpddl = $("#dynamicddlProcess" + (i - 1) + "").find(':selected').val();

                                    for (var m = 1; m < rowscounttableMacAmor ; m++) {
                                        var MacIdgrid = document.getElementById("GvMachineAmor").rows[m].cells[4].innerHTML;
                                        var ProcGrpCodegrid = document.getElementById("GvMachineAmor").rows[m].cells[3].innerHTML;
                                        if (macidddl == MacIdgrid && ProcGrpddl == ProcGrpCodegrid) {
                                            MachineList.push(macidddl);
                                            UseMacAmor = true;
                                            //break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (MachineList.length > 0) {
                        document.getElementById("HdnMachinListprocCost").value = MachineList;
                    }
                    else
                    {
                        document.getElementById("HdnMachinListprocCost").value = "";
                    }

                    if (UseMacAmor == true) {
                        $("#HdnIsUseMachineAmor").val("1");
                    }
                    else {
                        $("#HdnIsUseMachineAmor").val("0");
                    }
                    submatlcost();
                    submatlCostDataStore();
                    document.getElementById('BtnProcMachineAmor').click();

                    //if (UseMacAmor == true) {
                    //    $("#HdnIsUseMachineAmor").val("1");
                    //    if (IsFromCalculteSubMat == false) {
                    //        submatlcost();
                    //        document.getElementById('BtnProcMachineAmor').click();
                    //    }

                    //}
                    //else {
                    //    $("#HdnIsUseMachineAmor").val("0");
                    //    if (IsFromCalculteSubMat == false) {
                    //        submatlCostDataStoreWhitoutMachineAmor();
                    //        document.getElementById('BtnProcMachineAmor').click();
                    //    }
                    //}
                    return true;
                }
            } catch (e) {
                alert("CekMachineAmortizeFromProcCostTable() : " + e);
                return false;
            }
        }

        function CekMachineAmortizeFromProcCostTable2(IsFromCalculteSubMat) {
            try {
                var IsFromCalculteSubMat = IsFromCalculteSubMat;
                //document.getElementById("HdnMachinListprocCost").value;
                if ($("#HdnAcsTabSubMatCost").val() == "False" && $("#HdnAcsTabSubMatCost").val() == "0") {
                    return false;
                }
                else {

                    var tablePC = $("#TablePC");
                    var tableMacAmor = document.getElementById('GvMachineAmor');
                    var UseMacAmor = false;
                    var MachineList = [];
                    if (tablePC != null && tableMacAmor != null) {
                        var columncounttablePC = tablePC.find('tr')[0].cells.length;
                        var rowscounttableMacAmor = tableMacAmor.rows.length;

                        for (var i = 1; i < columncounttablePC ; i++) {
                            var dynamicddlMachineLabor = $("#dynamicddlMachineLabor" + (i - 1) + "");
                            if (dynamicddlMachineLabor != null) {
                                if (dynamicddlMachineLabor.val() == "Machine") {
                                    var macidddl = $("#dynamicddlMachine" + (i - 1) + "").find(':selected').attr('mac-id');
                                    var ProcGrpddl = $("#dynamicddlProcess" + (i - 1) + "").find(':selected').val();

                                    for (var m = 1; m < rowscounttableMacAmor ; m++) {
                                        var MacIdgrid = document.getElementById("GvMachineAmor").rows[m].cells[4].innerHTML;
                                        var ProcGrpCodegrid = document.getElementById("GvMachineAmor").rows[m].cells[3].innerHTML;
                                        if (macidddl == MacIdgrid && ProcGrpddl == ProcGrpCodegrid) {
                                            MachineList.push(macidddl);
                                            UseMacAmor = true;
                                            //break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (MachineList.length > 0) {
                        document.getElementById("HdnMachinListprocCost").value = MachineList;
                    }
                    else {
                        document.getElementById("HdnMachinListprocCost").value = "";
                    }

                    if (UseMacAmor == true) {
                        $("#HdnIsUseMachineAmor").val("1");
                    }
                    else {
                        $("#HdnIsUseMachineAmor").val("0");
                    }
                    submatlcost();
                    submatlCostDataStore();
                    submatlCostAddColumn();
                    return true;
                }
            } catch (e) {
                alert("CekMachineAmortizeFromProcCostTable() : " + e);
                return false;
            }
        }
    </script>

    <%--validation script--%>
    <script type="text/javascript">
        function appendUSDoll(strValue) {
            return ("$" + fixedDecml(parseFloat(CheckNull(strValue))));
        }

        function fixedDecml(numValue) {
            return numValue.toFixed(5);
        }

        function isincludecomma(txtID) {
            try {

                var text = document.getElementById(txtID).value;
                if (text.includes(',')) {
                    text = text.replace(/\,/g, '');
                    alert("cannot used character comma (,) !!! ");
                }
                document.getElementById(txtID).value = text;
            } catch (err) {
                alert("isincludecomma :" + err);
            }
        }

        function IsInteger(evt) {
            var chr = String.fromCharCode(evt.which);
            if (!(/[0-9]/.test(chr))) {
                evt.preventDefault();
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function validateNumber(txtID) {
            try {

                var validNumber = new RegExp(/^\d*\.?\d*$/);
                var lastValid = document.getElementById(txtID).value;
                if (validNumber.test(document.getElementById(txtID).value)) {
                    var ArrlastValid = lastValid.split('.');
                    if (ArrlastValid.length > 1) {
                        var lastValid0 = ArrlastValid[0].toString().trim();
                        if (lastValid0[0] == "" || lastValid0[0] == null) {
                            document.getElementById(txtID).value = '0' + lastValid;
                        }
                    }
                } else {
                    lastValid = lastValid.toString().substring(0, lastValid.length - 1);
                    document.getElementById(txtID).value = lastValid;
                }
            }
            catch (err) {
                alert(err + " : validateNumber(txtID)")
            }
        }

        function ValOnlyNo(txtID, Source) {
            //page related : review_reqmass,newrewwsapgp,newrequest,newreqchangemass,newreq_changes,reciew_req
            try {
                var regex = /^[-+]?\d*\.?\d*$/;
                var fullval = document.getElementById(txtID).value;
                var val = parseFloat(document.getElementById(txtID).value);
                if (txtID.includes('txtTurnkeyProfit')) {
                    fullval = document.getElementById(txtID).value.replace('%', '');
                    val = parseFloat(document.getElementById(txtID).value.replace('%', ''));
                }
                var IsValid = false;
                if (document.getElementById(txtID).value != "") {
                    if (!(regex.test(fullval))) {
                        if (txtID.includes('txtDurationperProcessUOM')) {
                            if (Source == "RedirectTxt") {
                                alert("Only Allow Integer !");
                            }
                            document.getElementById(txtID).focus();
                            IsValid = false;
                        }
                        else {
                            if (Source == "RedirectTxt") {
                                alert("Only Allow Decimal Number!");
                            }
                            document.getElementById(txtID).focus();
                            IsValid = false;
                        }
                    }
                    else if (txtID.includes('txtDurationperProcessUOM') && fullval.includes(".")) {
                        IsValid = false;
                    }
                    else {
                        IsValid = true;
                    }
                }
                else {
                    IsValid = true;
                }

                if (IsValid == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (err) {
                alert(err + " : ValOnlyNo(txtID)");
                return false;
            }
        }


        function ValidateMatCalc() {
            //page related : review_reqmass,newrewwsapgp,newrequest,newreqchangemass,newreq_changes,reciew_req
            try {
                var CellsCount = $("#Table1").find('tr')[0].cells.length;
                var Validate = true;
                var varProcessGroup = $("#hdnLayoutScreen").val();

                var MaterialSAPCode = null;
                var MaterialDescription = null;
                var RawMaterialCost = null;
                var RawMaterialUOM = null;
                var PartNetUnitWeight = null;
                var RunnerWeightShot = null;
                var RunnerRatio_pc = null;
                var RecycleMaterialRatio = null;
                var Thickness = null;
                var Width = null;
                var Pitch = null;
                var MaterialDensity = null;
                var Cavity = null;
                var MaterialYieldOrMeltingLoss = null;
                var MaterialScrapWeight = null;
                var ScrapLossAllowance = null;
                var ScrapPrice = null;
                var ValSAPSpProcType = HdnSAPSpProcType.value;
                var ValHdnAcsTabMatCost = HdnAcsTabMatCost.value;

                if (ValSAPSpProcType == "30" && ValHdnAcsTabMatCost == "False") {
                    return true;
                }
                else {
                    for (var i = 0; i < CellsCount; i++) {
                        MaterialSAPCode = null;
                        MaterialDescription = null;
                        RawMaterialCost = null;
                        RawMaterialUOM = null;
                        PartNetUnitWeight = null;
                        RunnerWeightShot = null;
                        RunnerRatio_pc = null;
                        RecycleMaterialRatio = null;
                        Thickness = null;
                        Width = null;
                        Pitch = null;
                        MaterialDensity = null;
                        Cavity = null;
                        MaterialYieldOrMeltingLoss = null;
                        MaterialScrapWeight = null;
                        ScrapLossAllowance = null;
                        ScrapPrice = null;

                        if (document.getElementById("txtMaterialSAPCode" + i) != null) {
                            MaterialSAPCode = document.getElementById("txtMaterialSAPCode" + i);
                        }
                        if (document.getElementById("txtMaterialDescription" + i) != null) {
                            MaterialDescription = document.getElementById("txtMaterialDescription" + i);
                        }
                        if (document.getElementById("txtRawMaterialCost/kg" + i) != null) {
                            RawMaterialCost = document.getElementById("txtRawMaterialCost/kg" + i);
                        }
                        if (document.getElementById("ddlRawMaterialCostUOM" + i) != null) {
                            RawMaterialUOM = document.getElementById("ddlRawMaterialCostUOM" + i);
                        }
                        if (document.getElementById("txtPartNetUnitWeight(g)" + i) != null) {
                            PartNetUnitWeight = document.getElementById("txtPartNetUnitWeight(g)" + i);
                        }
                        if (document.getElementById("txt~RunnerWeight/shot(g)" + i) != null) {
                            RunnerWeightShot = document.getElementById("txt~RunnerWeight/shot(g)" + i);
                        }
                        if (document.getElementById("txt~RunnerRatio/pcs(%)" + i) != null) {
                            RunnerRatio_pc = document.getElementById("txt~RunnerRatio/pcs(%)" + i);
                        }
                        if (document.getElementById("txt~RecycleMaterialRatio(%)" + i) != null) {
                            RecycleMaterialRatio = document.getElementById("txt~RecycleMaterialRatio(%)" + i);
                        }
                        if (document.getElementById("txt~~Thickness(mm)" + i) != null) {
                            Thickness = document.getElementById("txt~~Thickness(mm)" + i);
                        }
                        if (document.getElementById("txt~~Width(mm)" + i) != null) {
                            Width = document.getElementById("txt~~Width(mm)" + i);
                        }
                        if (document.getElementById("txt~~Pitch(mm)" + i) != null) {
                            Pitch = document.getElementById("txt~~Pitch(mm)" + i);
                        }
                        if (document.getElementById("txt~MaterialDensity" + i) != null) {
                            MaterialDensity = document.getElementById("txt~MaterialDensity" + i);
                        }
                        if (document.getElementById("txtCavity" + i) != null) {
                            Cavity = document.getElementById("txtCavity" + i);
                        }
                        if (document.getElementById("txtMaterialYield/MeltingLoss(%)" + i) != null) {
                            MaterialYieldOrMeltingLoss = document.getElementById("txtMaterialYield/MeltingLoss(%)" + i);
                        }
                        if (document.getElementById("txtMaterialScrapWeight(g)" + i) != null) {
                            MaterialScrapWeight = document.getElementById("txtMaterialScrapWeight(g)" + i);
                        }
                        if (document.getElementById("txtScrapLossAllowance(%)" + i) != null) {
                            ScrapLossAllowance = document.getElementById("txtScrapLossAllowance(%)" + i);
                        }
                        if (document.getElementById("txtScrapPrice/kg" + i) != null) {
                            ScrapPrice = document.getElementById("txtScrapPrice/kg" + i);
                        }

                        if (varProcessGroup != "Layout2") {
                            if (MaterialSAPCode != null && MaterialDescription != null) {
                                if (MaterialSAPCode.value.trim() == "" && MaterialDescription.value.trim() == "") {
                                    MaterialSAPCode.focus();
                                    alert('Please enter Material SAP Code or Material Description !');
                                    return false;
                                }
                            }
                        }
                        else {
                            if (MaterialSAPCode != null) {
                                if (MaterialSAPCode.value.trim() == "") {
                                    MaterialSAPCode.focus();
                                    alert('Please enter Material Code !');
                                    return false;
                                }
                            }
                        }

                        if (RawMaterialCost != null) {
                            if (RawMaterialCost.value == "" || RawMaterialCost.value == 0) {
                                RawMaterialCost.focus();
                                alert('Raw Material Cost/ kg cannot be Empty or 0 !');
                                return false;
                            }
                            else if (RawMaterialCost.value != "") {
                                if (ValOnlyNo("txtRawMaterialCost/kg" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Raw Material Cost value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (RawMaterialUOM != null) {
                            var QuoteRef = hdnQuoteNoRef.value;
                            var AcsTabMatCost = HdnAcsTabMatCost.value;
                            if (AcsTabMatCost.toString().toUpperCase() == "FALSE") {
                                if (RawMaterialUOM.value == "" || RawMaterialUOM.value == "--SELECT UOM--" || RawMaterialUOM.value == "UOM NOT EXIST" || RawMaterialUOM.value.length > 40) {
                                    OpenModalUOM();
                                    return false;
                                }
                            }
                            else {
                                if (RawMaterialUOM.value == "") {
                                    RawMaterialUOM.focus();
                                    alert('Raw Material UOM cannot be empty !');
                                    return false;
                                }
                                else if (RawMaterialUOM.value == "--SELECT UOM--") {
                                    RawMaterialUOM.focus();
                                    alert('Please Select Raw material UOM')
                                    return false;
                                }
                                else if (RawMaterialUOM.value == "UOM NOT EXIST") {
                                    RawMaterialUOM.focus();
                                    alert('Raw material UOM NOT EXIST, Please Contact Administrator');
                                    return false;
                                }
                                else if (RawMaterialUOM.value.length > 40) {
                                    RawMaterialUOM.focus();
                                    alert('Raw Material UOM cannot be more than 40 Char !');
                                    return false;
                                }
                            }
                        }

                        if (PartNetUnitWeight != null) {
                            if (PartNetUnitWeight.value == "" || PartNetUnitWeight.value == 0) {
                                PartNetUnitWeight.focus();
                                alert('Part Net Unit Weight (g) cannot be Empty or 0 !');
                                return false;
                            }
                            else if (PartNetUnitWeight.value != "") {
                                if (ValOnlyNo("txtPartNetUnitWeight(g)" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Part Net Unit Weight value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (RunnerWeightShot != null) {
                            if (RunnerWeightShot.value == "" || RunnerWeightShot.value == 0) {
                                RunnerWeightShot.focus();
                                alert('Runner Weight/shot (g) cannot be Empty or 0 !');
                                return false;
                            }
                            else if (RunnerWeightShot.value != "") {
                                if (ValOnlyNo("txt~RunnerWeight/shot(g)" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Runner Weight Shot value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (RecycleMaterialRatio != null) {
                            if (RecycleMaterialRatio.value == "") {
                                RecycleMaterialRatio.focus();
                                alert('Recycle Material Ratio (%) cannot be Empty !');
                                return false;
                            }
                            else if (RecycleMaterialRatio.value != "") {
                                if (ValOnlyNo("txt~RecycleMaterialRatio(%)" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Recycle Material Ratio (%) value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (Thickness != null) {
                            if (Thickness.value == "" || Thickness.value == 0) {
                                Thickness.focus();
                                alert('Thickness (mm) cannot be Empty or 0 !');
                                return false;
                            }
                            else if (Thickness.value != "") {
                                if (ValOnlyNo("txt~~Thickness(mm)" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Thickness (mm) value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (Width != null) {
                            if (Width.value == "" || Width.value == 0) {
                                Width.focus();
                                alert('Width (mm) cannot be Empty or 0 !');
                                return false;
                            }
                            else if (Width.value != "") {
                                if (ValOnlyNo("txt~~Width(mm)" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Width (mm) value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (Pitch != null) {
                            if (Pitch.value == "" || Pitch.value == 0) {
                                Pitch.focus();
                                alert('Pitch (mm) cannot be Empty or 0 !');
                                return false;
                            }
                            else if (Pitch.value != "") {
                                if (ValOnlyNo("txt~~Pitch(mm)" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Pitch (mm) value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (MaterialDensity != null) {
                            if (MaterialDensity.value == "" || MaterialDensity.value == 0) {
                                MaterialDensity.focus();
                                alert('Material Density cannot be Empty or 0 !');
                                return false;
                            }
                            else if (MaterialDensity.value != "") {
                                if (ValOnlyNo("txt~MaterialDensity" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Material Density value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (Cavity != null) {
                            if (Cavity.value == "" || Cavity.value == 0) {
                                Cavity.focus();
                                alert('Base Qty / Cavity cannot be Empty or 0 !');
                                return false;
                            }
                            else if (Cavity.value != "") {
                                if (ValOnlyNo("txtCavity" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Cavity value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (MaterialYieldOrMeltingLoss != null) {
                            if (MaterialYieldOrMeltingLoss.value == "") {
                                MaterialYieldOrMeltingLoss.focus();
                                alert('Material/Melting Loss (%) cannot be Empty !');
                                return false;
                            }
                            else if (MaterialYieldOrMeltingLoss.value != "") {
                                if (ValOnlyNo("txtMaterialYield/MeltingLoss(%)" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Material/Melting Loss (%) value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        //if (MaterialScrapWeight != null) {
                        //    if (MaterialScrapWeight.value == "") {
                        //        MaterialScrapWeight.focus();
                        //        alert('Material Scrap Weight (g) cannot be Empty !');
                        //        return false;
                        //    }
                        //}

                        if (ScrapLossAllowance != null) {
                            if (ScrapLossAllowance.value == "") {
                                ScrapLossAllowance.focus();
                                alert('Scrap Loss Allowance (%) cannot be Empty or 0 !');
                                return false;
                            }
                            else if (ScrapLossAllowance.value != "") {
                                if (ValOnlyNo("txtScrapLossAllowance(%)" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Scrap Loss Allowance (%) value at column ' + (i + 1) + ' !');
                                    return false;
                                }
                            }
                        }

                        if (MaterialScrapWeight == null) {
                            if (MaterialScrapWeight != null) {
                                if (MaterialScrapWeight.value == "" || MaterialScrapWeight.value == 0) {
                                    MaterialScrapWeight.focus();
                                    alert('Material Scrap Weight cannot be Empty or 0 !');
                                    return false;
                                }
                                else if (MaterialScrapWeight.value != "") {
                                    if (ValOnlyNo("txtMaterialScrapWeight(g)" + i, "BtnCalculte") == false) {
                                        alert('Only allow decimal number, Please check Material Scrap Weight value at column ' + (i + 1) + ' !');
                                        return false;
                                    }
                                }
                            }
                        }
                        else {
                            if (ScrapPrice != null) {
                                if (ScrapPrice.value == "") {
                                    ScrapPrice.focus();
                                    alert('Scrap Price/kg cannot be Empty or 0 !');
                                    return false;
                                }
                                else if (ScrapPrice.value != "") {
                                    if (ValOnlyNo("txtScrapPrice/kg" + i, "BtnCalculte") == false) {
                                        alert('Only allow decimal number, Please check Scrap Price/kg value at column ' + (i + 1) + ' !');
                                        return false;
                                    }
                                }
                            }
                        }

                        if (MaterialScrapWeight != null && ScrapPrice != null) {
                            if (MaterialScrapWeight.value > 0 && ScrapPrice.value == 0) {
                                ScrapPrice.focus();
                                alert('Scrap Price/kg cannot be 0 when Material Scrap Weight (g) more than 0 !');
                                return false;
                            }
                        }

                        if (ScrapPrice != null && RawMaterialCost != null) {
                            var ScrapPriceVal = parseFloat(ScrapPrice.value);
                            var RawMaterialCostVal = parseFloat(RawMaterialCost.value);
                            if (ScrapPriceVal > RawMaterialCostVal) {
                                alert('Scrap Price/kg cannot more than Raw Material Cost/kg');
                                $('#txtScrapPrice\\/kg' + i).val("");
                                $('#txtMaterialCost\\/pcs' + i).val("");
                                $('#txtScrapRebate\\/pcs' + i).val("");
                                $('#txtTotalMaterialCost\\/pcs0').val("");
                                ScrapPrice.focus();
                                return false;
                            }
                        }

                        if (RecycleMaterialRatio != null && RunnerRatio_pc != null) {
                            var RecycleMaterialRatioVal = parseFloat(RecycleMaterialRatio.value);
                            var RunnerRatio_pcVal = parseFloat(RunnerRatio_pc.value);
                            if (RecycleMaterialRatioVal > RunnerRatio_pcVal) {
                                alert('The Recycle Material Ratio shall not > Runner Ratio');
                                $('#txtMaterialGrossWeight\\/pc\\(g\\)' + i).val("");
                                $('#txtMaterialCost\\/pcs' + i).val("");
                                $('#txtTotalMaterialCost\\/pcs0').val("");
                                $('#txt\\~RecycleMaterialRatio\\(\\%\\)' + i).val("");
                                $('#txtTotalMaterialCost\\/pcs0').val("");
                                RecycleMaterialRatio.focus();
                                return false;
                            }
                        }

                    }
                    return true;
                }
            } catch (err) {
                alert('ValidateMatCalc() : ' + err);
                return false;
            }
        }

        function ValidateProcCost() {
            //page related : review_reqmass,newrewwsapgp,newrequest,newreqchangemass,newreq_changes,reciew_req
            try {
                var CellsCount = $("#TablePC").find('tr')[0].cells.length;
                var Validate = true;
                var varProcessGroup = $("#hdnLayoutScreen").val();

                var VendorRate = null;
                var Baseqty = null;
                var DurationperProcessUOM = null;
                var Efficiency = null;
                var TurnkeyCostPerpc = null;
                var TurnkeyFees = null;
                var ProcessCostPc = null;
                var txtIfTurnkeyVendorName = null;
                var ddlSubvendorname = null;

                if (varProcessGroup != "Layout7") {
                    for (var i = 0; i < (CellsCount - 1) ; i++) {

                        var PrGroup = $('#dynamicddlProcess' + i + ' option:selected').text();
                        var SubPrGroup = $('#dynamicddlSubProcess' + i + ' option:selected').text();

                        var Hr_Rate;
                        var VdNameIfTurnKey = document.getElementById("txtIfTurnkey-VendorName" + i).value;
                        var BsQtyVal = $('#txtBaseqty' + i).val();
                        var prosuom = document.getElementById("txtProcessUOM" + i).value;
                        var DurProcUom = $('#txtDurationperProcessUOM\\(Sec\\)' + i).val();
                        var EfficiencyYield = $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val();
                        txtIfTurnkeyVendorName = $('#txtIfTurnkey-VendorName' + i).val();
                        var txtIfTurnkeySubVN = document.getElementById('dynamicddlSubvendorname' + i).selectedIndex;
                        var MachOrLabIdx = document.getElementById('dynamicddlMachineLabor' + i).selectedIndex;
                        var MachineSlected = document.getElementById('dynamicddlMachine' + i).value;

                        if (CellsCount == 2 && PrGroup.trim() == '--Select--') {
                        }
                        else {
                            if (CellsCount > 2 && PrGroup.trim() == '--Select--') {
                                Validate = false;
                                alert('please Select process Group at column ' + (i + 1) + ' !');
                                break;
                            }

                            if (PrGroup.trim() != '--Select--') {
                                if (SubPrGroup.trim() == '--Select--') {
                                    Validate = false;
                                    alert('please Select Sub process at column ' + (i + 1) + ' !');
                                    break;
                                }

                                if (txtIfTurnkeyVendorName == "") {
                                    if (txtIfTurnkeySubVN <= 0) {
                                        if (MachOrLabIdx == 0) {
                                            if (MachineSlected == "") {
                                                Validate = false;
                                                alert("Please Select Machine For Column No " + (i + 1) + " !!");
                                                break;
                                            }
                                        }

                                        var ProcUomOri = prosuom.split('-');
                                        if (BsQtyVal.toString() === "" && ProcUomOri[0].toString().toUpperCase().replace(/\s/g, '') === "STROKES/MIN") {
                                            Validate = false;
                                            alert("please Enter base qty !!");
                                            $('#txtProcessCost\\/pc' + i).val("");
                                            document.getElementById("txtBaseqty" + i).focus();
                                            break;
                                        }
                                        else if (BsQtyVal.toString() === "" && ProcUomOri[0].toString().toUpperCase().replace(/\s/g, '') === "KG/LOAD") {
                                            Validate = false;
                                            alert("please Enter base qty !!");
                                            $('#txtProcessCost\\/pc' + i).val("");
                                            document.getElementById("txtBaseqty" + i).focus();
                                            break;
                                        }

                                        if (DurProcUom == "") {
                                            Validate = false;
                                            alert("Please Enter Duration per Process UOM (Sec) !!");
                                            document.getElementById("txtDurationperProcessUOM(Sec)" + i).focus();
                                            break;
                                        }

                                        if (EfficiencyYield == "" || EfficiencyYield == 0 || parseInt(EfficiencyYield) > 100) {
                                            Validate = false;
                                            alert("Please Enter Efficiency/Process Yield (%) between 1 to 100 !!");
                                            document.getElementById("txtEfficiency/ProcessYield(%)" + i).focus();
                                            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val("100");
                                            break;
                                        }

                                        if ($("#txtVendorRate" + i).val() == "") {
                                            Validate = false;
                                            alert("Vendor rate cannot be empty !!");
                                            document.getElementById("txtVendorRate" + i).focus();
                                            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val("100");
                                            break;
                                        }
                                    }
                                    else {
                                        var TurnkeyCost = $('#txtTurnkeyCost\\/pc' + i).val();
                                        if (TurnkeyCost == "") {
                                            if (txtIfTurnkeySubVN > 0) {
                                                Validate = false;
                                                alert("Please Enter Turn Key Cost !!");
                                                $('#txtTurnkeyCost\\/pc' + i).focus();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (Validate == true) {
                    for (var i = 0; i < (CellsCount - 1) ; i++) {
                        VendorRate = null;
                        Baseqty = null;
                        DurationperProcessUOM = null;
                        Efficiency = null;
                        TurnkeyCostPerpc = null;
                        TurnkeyFees = null;
                        ProcessCostPc = null;
                        ddlSubvendorname = null;
                        txtIfTurnkeyVendorName = "";

                        //txtIfTurnkeyVendorName--------------------------------------------------------------------------------------------------
                        txtIfTurnkeyVendorName = $('#txtIfTurnkey-VendorName' + i).val();

                        //ddlSubvendorname--------------------------------------------------------------------------------------------------
                        if (document.getElementById("dynamicddlSubvendorname" + i) != null) {
                            ddlSubvendorname = document.getElementById("dynamicddlSubvendorname" + i).value;
                            if (ddlSubvendorname == "Select" || ddlSubvendorname == "") {
                                ddlSubvendorname = "";
                            }
                        }
                        else {
                            ddlSubvendorname = "";
                        }

                        if (ddlSubvendorname == "" && txtIfTurnkeyVendorName == "") {
                            //VendorRate--------------------------------------------------------------------------------------------------
                            if (document.getElementById("txtVendorRate" + i) != null) {
                                VendorRate = document.getElementById("txtVendorRate" + i);
                            }

                            if (VendorRate != null) {
                                if (VendorRate.value != "") {
                                    if (ValOnlyNo("txtVendorRate" + i, "BtnCalculte") == false) {
                                        alert('Only allow decimal number, Please check Vendor Rate value at column ' + (i + 1) + ' !');

                                        document.getElementById("txtTotalProcessesCost/pcs0").value = "";
                                        Validate = false;
                                        break;
                                    }
                                }
                            }
                            //--------------------------------------------------------------------------------------------------


                            //Baseqty--------------------------------------------------------------------------------------------------
                            if (document.getElementById("txtBaseqty" + i) != null) {
                                Baseqty = document.getElementById("txtBaseqty" + i);
                            }

                            if (Baseqty != null) {
                                if (Baseqty.value != "" && Baseqty.value != "0") {
                                    if (ValOnlyNo("txtBaseqty" + i, "BtnCalculte") == false) {
                                        var Layout = $("#hdnLayoutScreen").val();
                                        if (Layout != "Layout7") {
                                            alert('Only allow decimal number, Please check Base qty value at column ' + (i + 1) + ' !');
                                            document.getElementById("txtTotalProcessesCost/pcs0").value = "";
                                            Validate = false;
                                            break;
                                        }
                                    }
                                }
                                else if (Baseqty.value == "0" || Baseqty.value == "") {
                                    var Layout = $("#hdnLayoutScreen").val();
                                    if (Layout != "Layout7") {
                                        alert('Please check Base qty value at column ' + (i + 1) + ' cannot be empty or 0 !');
                                        Validate = false;
                                        break;
                                    }
                                }
                            }
                            //--------------------------------------------------------------------------------------------------


                            //DurationperProcessUOM--------------------------------------------------------------------------------------------------
                            if (document.getElementById("txtDurationperProcessUOM(Sec)" + i) != null) {
                                DurationperProcessUOM = document.getElementById("txtDurationperProcessUOM(Sec)" + i);
                            }

                            if (DurationperProcessUOM != null) {
                                if (DurationperProcessUOM.value != "") {
                                    if (ValOnlyNo("txtDurationperProcessUOM(Sec)" + i, "BtnCalculte") == false) {
                                        if (document.getElementById("txtDurationperProcessUOM(Sec)" + i).disabled == false) {
                                            //alert('Only allow number, Please check Duration Process UOM value at column ' + (i + 1) + ' !');
                                            alert('Duration per Process UOM (sec) only accept whole number (avoid decimal place), check column ' + (i + 1) + ' !');

                                            document.getElementById("txtTotalProcessesCost/pcs0").value = "";
                                            Validate = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            //--------------------------------------------------------------------------------------------------


                            //Efficiency--------------------------------------------------------------------------------------------------
                            if (document.getElementById("txtEfficiency/ProcessYield(%)" + i) != null) {
                                Efficiency = document.getElementById("txtEfficiency/ProcessYield(%)" + i);
                            }

                            if (Efficiency != null) {
                                if (Efficiency.value != "") {
                                    if (ValOnlyNo("txtEfficiency/ProcessYield(%)" + i, "BtnCalculte") == false) {
                                        alert('Only allow decimal number, Please check Efficiency value at column ' + (i + 1) + ' !');

                                        document.getElementById("txtTotalProcessesCost/pcs0").value = "";
                                        Validate = false;
                                        break;
                                    }
                                }
                            }
                            //--------------------------------------------------------------------------------------------------
                        }
                        else if (txtIfTurnkeyVendorName != "") {
                            if (document.getElementById("txtProcessCost/pc" + i) != null) {
                                if (document.getElementById("txtProcessCost/pc" + i).value == "") {
                                    alert('Please enter Process Cost/pc at column ' + (i + 1) + ' !');
                                    Validate = false;
                                    break;
                                }
                                else if (txtIfTurnkeyVendorName.trim() == "") {
                                    alert('If Subcon - Subcon Name do not have value at column ' + (i + 1) + ' !');
                                    Validate = false;
                                    break;
                                }
                            }
                        }


                        //TurnkeyCostPerpc--------------------------------------------------------------------------------------------------
                        if (document.getElementById("txtTurnkeyCost/pc" + i) != null) {
                            TurnkeyCostPerpc = document.getElementById("txtTurnkeyCost/pc" + i);
                        }

                        if (TurnkeyCostPerpc != null) {
                            if (TurnkeyCostPerpc.value != "") {
                                if (ValOnlyNo("txtTurnkeyCost/pc" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Turnkey Cost/pc value at column ' + (i + 1) + ' !');

                                    document.getElementById("txtTotalProcessesCost/pcs0").value = "";
                                    Validate = false;
                                    break;
                                }
                            }
                        }
                        //--------------------------------------------------------------------------------------------------


                        //TurnkeyFees--------------------------------------------------------------------------------------------------
                        if (document.getElementById("txtTurnkeyProfit" + i) != null) {
                            TurnkeyFees = document.getElementById("txtTurnkeyProfit" + i);
                        }

                        if (TurnkeyFees != null) {
                            if (TurnkeyFees.value != "") {
                                if (ValOnlyNo("txtTurnkeyProfit" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Turn key Fees value at column ' + (i + 1) + ' !');

                                    document.getElementById("txtTotalProcessesCost/pcs0").value = "";
                                    Validate = false;
                                    break;
                                }
                            }
                        }
                        //--------------------------------------------------------------------------------------------------


                        //ProcessCostPc--------------------------------------------------------------------------------------------------
                        if (document.getElementById("txtProcessCost/pc" + i) != null) {
                            ProcessCostPc = document.getElementById("txtProcessCost/pc" + i);
                        }

                        if (ProcessCostPc != null) {
                            if (ProcessCostPc.value != "") {
                                if (ValOnlyNo("txtProcessCost/pc" + i, "BtnCalculte") == false) {
                                    alert('Only allow decimal number, Please check Process Cost Pc value at column ' + (i + 1) + ' !');

                                    document.getElementById("txtTotalProcessesCost/pcs0").value = "";
                                    Validate = false;
                                    break;
                                }
                            }
                        }
                        //--------------------------------------------------------------------------------------------------
                    }
                }

                if (Validate == true) {
                    return true;
                }
                else {
                    return false;
                }

            } catch (err) {
                alert("ValidateProcCost : " + err);
                return false;
            }
        }

        function ValidateSubMat() {
            //page related : review_reqmass,newrewwsapgp,newrequest,newreqchangemass,newreq_changes,reciew_req
            try {
                var CellsCount = $("#TableSMC").find('tr')[0].cells.length;
                var Validate = true;
                var varProcessGroup = $("#hdnLayoutScreen").val();

                var SubMatorTnJCost = null;
                var Consumption = null;

                for (var i = 0; i < CellsCount; i++) {
                    SubMatorTnJCost = null;
                    Consumption = null;

                    if (document.getElementById("txtSub-Mat/T&JCost" + i) != null) {
                        SubMatorTnJCost = document.getElementById("txtSub-Mat/T&JCost" + i);
                    }

                    if (SubMatorTnJCost != null) {
                        if (SubMatorTnJCost.value != "") {
                            if (ValOnlyNo("txtSub-Mat/T&JCost" + i, "BtnCalculte") == false) {
                                alert('Only allow decimal number, Please check Sub-Mat/T&J Cost value at column ' + (i + 1) + ' !');

                                document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value = "";
                                document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value = "";
                                Validate = false;
                                break;
                            }
                        }
                    }

                    if (document.getElementById("txtConsumption(pcs)" + i) != null) {
                        Consumption = document.getElementById("txtConsumption(pcs)" + i);
                    }

                    if (Consumption != null) {
                        if (Consumption.value != "") {
                            if (ValOnlyNo("txtConsumption(pcs)" + i, "BtnCalculte") == false) {
                                alert('Only allow decimal number, Please check Consumption (pc) value at column ' + (i + 1) + ' !');

                                document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value = "";
                                document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value = "";
                                Validate = false;
                                break;
                            }
                        }
                    }
                }

                if (Validate == true) {
                    return true;
                }
                else {
                    return false;
                }

            } catch (err) {
                alert("ValidateSubMat : " + err);
                return false;
            }
        }

        function ValidateOthCost() {
            //page related : review_reqmass,newrewwsapgp,newrequest,newreqchangemass,newreq_changes,reciew_req
            try {
                var CellsCount = $("#TableOthers").find('tr')[0].cells.length;
                var Validate = true;
                var varProcessGroup = $("#hdnLayoutScreen").val();

                var OtherItemCostpc = null;

                for (var i = 0; i < CellsCount; i++) {
                    OtherItemCostpc = null;
                    if (document.getElementById("txtOtherItemCost/pcs" + i) != null) {
                        OtherItemCostpc = document.getElementById("txtOtherItemCost/pcs" + i);
                    }

                    if (OtherItemCostpc != null) {
                        if (OtherItemCostpc.value != "") {
                            if (ValOnlyNo("txtOtherItemCost/pcs" + i, "BtnCalculte") == false) {
                                alert('Only allow decimal number, Please check Other Item Cost value at column ' + (i + 1) + ' !');
                                document.getElementById("txtTotalOtherItemCost/pcs0").value = "";
                                Validate = false;
                            }
                        }
                    }
                }

                if (Validate == true) {
                    return true;
                }
                else {
                    return false;
                }

            } catch (err) {
                alert("ValidateOthCost : " + err);
                return false;
            }
        }

        function ValidateUnit() {
            //page related : review_reqmass,newrewwsapgp,newrequest,newreqchangemass,newreq_changes,reciew_req
            try {
                var Validate = true;
                var Disc = null;
                var Profit = null;
                var GA = null;

                if (document.getElementById("txtProfit(%)0") != null) {
                    Profit = document.getElementById("txtProfit(%)0");
                }

                if (document.getElementById("txtDiscount(%)0") != null) {
                    Disc = document.getElementById("txtDiscount(%)0");
                }

                if (document.getElementById("GA(%)0") != null) {
                    GA = document.getElementById("GA(%)0");
                }

                if (Profit != null) {
                    if (Profit.value != "") {
                        if (ValOnlyNo("txtProfit(%)0", "BtnCalculte") == false) {
                            alert('Only allow decimal number, Please check Profit value !');
                            Validate = false;
                            return false;
                        }
                    }
                }

                if (Disc != null) {
                    if (Disc.value != "") {
                        if (ValOnlyNo("txtDiscount(%)0", "BtnCalculte") == false) {
                            alert('Only allow decimal number, Please check Disc value !');
                            Validate = false;
                            return false;
                        }
                    }
                }

                if (GA != null) {
                    if (GA.value != "") {
                        if (ValOnlyNo("GA(%)0", "BtnCalculte") == false) {
                            alert('Only allow decimal number, Please check GA value !');
                            Validate = false;
                            return false;
                        }
                    }
                }

                if (Validate == true) {
                    return true;
                }
                else {
                    return false;
                }

            } catch (err) {
                alert("ValidateUnit : " + err);
                return false;
            }
        }

        function Validate(IdNo, txt) {
            if (txt == 'MATYIELD') {
                var txtMaterialYield = parseInt($('#txtMaterialYield\\/MeltingLoss\\(\\%\\)' + IdNo).val());
                if (txtMaterialYield > 100) {
                    alert("value cannot bigger than 100%");
                    $('#txtMaterialYield\\/MeltingLoss\\(\\%\\)' + IdNo).val("");
                }
            }
            else if (txt == 'SCRAPALLOWENCE') {
                var txtScrapLossAllowance = parseInt($('#txtScrapLossAllowance\\(\\%\\)' + IdNo).val());
                if (txtScrapLossAllowance > 100) {
                    alert("value cannot bigger than 100%");
                    $('#txtScrapLossAllowance\\(\\%\\)' + IdNo).val("");
                }
            }
        }

        function validateTotalValue() {
            //var totmatcost = $('#txtTotalMaterialCost\\/pcs0').val();
            //var totprocost = $('#txtTotalSub-Mat\\/T\\&JCost\\/pcs0').val();
            //var totsubmatcost = $('#txtTotalSub-Mat\\/T\\&JCost\\/pcs0').val();
            //var totothcost = $('#txtTotalOtherItemCost\\/pcs0').val();
            var totmatcost = document.getElementById('txtTotalMaterialCost/pcs0').value;
            var totprocost = document.getElementById('txtTotalProcessesCost/pcs0').value;
            var totsubmatcost = document.getElementById('txtTotalSub-Mat/T&JCost/pcs0').value;
            var totothcost = document.getElementById('txtTotalOtherItemCost/pcs0').value;
            var net = "";
            var hdnVendorType = $("#hdnVendorType").val();
            if (hdnVendorType == "TeamShimano") {
                net = "";
            }
            else {
                var net = document.getElementById('txtNetProfit(%)0').value
            }
            if (totmatcost.toString().toUpperCase().includes("NAN") || totmatcost.toString().toUpperCase().includes("INFINITY") || totmatcost.toString() == "") {
                alert("No Material Cost Calculation, Please press Calculate Material Cost Button.. !!");
                return false;
            }
            else if (totprocost.toString().toUpperCase().includes("NAN") || totprocost.toString().toUpperCase().includes("INFINITY") || totprocost.toString() == "") {
                alert("No Process Cost Calculation, Please press Calculate Process Cost Calculation Button.. !!");
                return false;
            }
            else if (totsubmatcost.toString().toUpperCase().includes("NAN") || totsubmatcost.toString().toUpperCase().includes("INFINITY") || totsubmatcost.toString() == "") {
                alert("No Sub Material Cost Calculation, Please press Calculate Sub Material Cost Button.. !!");
                return false;
            }
            else if (totothcost.toString().toUpperCase().includes("NAN") || totothcost.toString().toUpperCase().includes("INFINITY") || totothcost.toString() == "") {
                alert("No Other Cost Calculation, Please press Calculate Other Cost Button.. !!");
                return false;
            }

            if (hdnVendorType != "TeamShimano") {
                if (net.toString().replace(" %", "").toUpperCase().includes("NAN") || totothcost.toString().toUpperCase().includes("INFINITY")) {
                    alert("No Other Cost Calculation, Please press Calculate Other Cost Button.. !!");
                    return false;
                }
            }
            return true;
        }

        function cekIsHaveOldToolOrmachineAmortize(Idraft) {
            var ok = true;
            try {
                var CellsCount = $("#TableSMC").find('tr')[0].cells.length;
                var chkIsUseToolAmor = document.getElementById("GdvQuoreReqRevice_chkIsUseToolAmortize_0");
                var chkIsUseMacAmor = document.getElementById("GdvQuoreReqRevice_chkIsUseMachineAmortize_0");
                var IsHaveMachineAmor = false;
                for (var i = 0; i < CellsCount - 1; i++) {
                    var lbTxt = document.getElementById("LbToolOrMachineAmor" + (i + 1));
                    var BtnDel = document.getElementById("BtnDelSubMatCost" + (i + 1));

                    if (lbTxt != null) {
                        if (lbTxt.innerHTML == "Old Tool Amortize") {
                            ok = false;
                            alert("Please press calculate Sub Material Cost to Remove Old Tool Amortize");
                            break;
                        }
                        else if (lbTxt.innerHTML == "Old Machine Amortize") {
                            ok = false;
                            alert("Please press calculate Sub Material Cost to Remove Old Machine Amortize");
                            break;
                        }
                        else if (lbTxt.innerHTML == "Machine Amortize") {
                            IsHaveMachineAmor = true;
                        }
                    }
                }

                if (Idraft == false && ok == true && chkIsUseToolAmor != null ) {
                    if (IsHaveMachineAmor == false && chkIsUseMacAmor.checked == true) {
                        ok = false;
                        alert("No Machine Amortize Selected");
                    }
                }
                
            } catch (e) {
                ok = false;
                alert("cekIsHaveOldToolOrmachineAmortize : " + e);
            }
            return ok;
        }
    </script>
</head>
<body id="page-top">
    <form id="form1" runat="server">
        <div id="header">
            <asp:ScriptManager ID="scriptmanager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
            <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"></asp:ToolkitScriptManager>--%>
        </div>
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

        <div class="row text-center">
            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="PanelHideShow" runat="server">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <div id="loading" class="col-sm-12" style="padding-top: 200px;">
                                    <img id="loading-image" src="images/loading.gif" alt="Loading..." />
                                    <div class="col-sm-12" style="text-align: center; opacity: 1;">
                                        <asp:Label ID="lbLoading0" runat="server" Font-Bold="true" ForeColor="#0000ff" Text="please Wait..."></asp:Label>
                                    </div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <!-- Header -->
        <asp:UpdatePanel runat="server" ID="UpsidebarToggle">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="col-lg-12" style="padding: 5px;">
                        <div class="row">
                            <div class="col-sm-10" style="padding-top: 5px;">
                                <a onclick="ShowLoading();" href="Emet_author_V.aspx?num=15">
                                    <asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                                <%--<button class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" href="#"><i class="fas fa-bars"></i></button>--%>
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
                    <!-- Area Chart Example-->
                    <div class="card" style="background-color: white">
                        <div class="card-body" style="padding-top: 0px;">
                            <%--button reset--%>
                            <div class="col-md-12" style="background-color: white; padding-top: 10px;">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="lbTitle" runat="server" Text="New" Font-Bold="true" Font-Size="Large" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12" style="padding-top: 5px; background-color: white;">
                                <div class="col-md-12" style="border-bottom: 2px solid #006EB7"></div>
                            </div>

                            <%--entrydata--%>
                            <div class="col-md-12" style="background-color: white;">
                                <div class="row" style="padding-bottom: 10px; padding-top: 10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label9" runat="server" Text="VENDOR DETAIL"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label21" runat="server" Text="New Quote No"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:Label ID="lblreqst" runat="server" Text=": "></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdnQuoteNo" />
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="DvQuoteRef">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label22" runat="server" Text="Old Quote No"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <a onclick="openInNewTab('VViewRequest.aspx');">
                                                    <asp:Label ID="LblQuNoRef" runat="server" Text=": "></asp:Label></a>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label23" runat="server" Text="Currency"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:Label ID="lblCurrency" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label34" runat="server" Text="City"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label ID="lblVName" runat="server" Text="" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label10" runat="server" Text="Updated By"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:Label ID="Label14" runat="server" Text="" Visible="true"></asp:Label>
                                                <asp:Label ID="Label15" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="Label11" runat="server" Text="" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label26" runat="server" Text="SHIMANO DETAIL"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label17" runat="server" ForeColor="Black" Text="SMN PIC"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtsmnpic" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_addres" runat="server" ForeColor="Black" Text="Email"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtemail" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_cntact" runat="server" ForeColor="Black" Text="Plant & Department"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="TxtPlant" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="TxtDepartment" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Labelquote" runat="server" ForeColor="Black" Text="Vendor Response Due Date"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtquotationDueDate" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" BackColor="#E6E6E6" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label39" runat="server" ForeColor="Black" Text="Request Date"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="TxtRequestDate" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label2" runat="server" Text="PART I: QUOTED PART INFO"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_partdesc" runat="server" ForeColor="Black" Text="Part Code & Desc"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtpartdesc" Enabled="false" runat="server" Height="60px" TextMode="MultiLine"
                                                    ForeColor="Black" CssClass="form-control-sm" Font-Size="14px" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row" runat="server" id="DvProduct">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_cntact0" runat="server" ForeColor="Black" Text="Product"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtprod" Enabled="false" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="DvReqPlant" style="display: none;">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label40" runat="server" ForeColor="Black" Text="GP Request Plant"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="TxtReqPlant" Enabled="false" runat="server" Height="55px"
                                                    TextMode="MultiLine" ForeColor="Black" Width="100%" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;" runat="server" id="DvSAPPIR">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_cntact4" runat="server" ForeColor="Black" Text="SAP PIR JOB TYPE & Desc"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtSAPJobType" Enabled="false" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_PIR" runat="server" ForeColor="Black" Text="PIR Type & Desc"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtPIRtype" Enabled="false" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_partDRG" runat="server" ForeColor="Black" Text="PART DRG"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtdrawng" Enabled="false" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_proces" runat="server" ForeColor="Black" Text="Process Group"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtprocs" Enabled="false" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="txtPartUnit" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label27" runat="server" ForeColor="Black" Text="BaseUOM: "></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtBaseUOM" Enabled="false" runat="server" CssClass="form-control-sm" Font-Size="14px" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <%--  <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label28" runat="server"  ForeColor="Black" Text="Country Of Origin"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                              <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlpirjtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlpirjtype_SelectedIndexChanged" Font-Bold="False" ForeColor="Black"  Width="281px" Height="30px">
                                                </asp:DropDownList>
                                                </ContentTemplate>
                                              </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label16" runat="server" Text="Net Weight:"
                                                    ForeColor="Black"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:UpdatePanel ID="UpdatePanel111" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="txtunitweight" Enabled="false" runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="txtUOM" Enabled="false" runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="txtpartdesc" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label20" runat="server" Text="Request Purpose" Enabled="false"
                                                    ForeColor="Black"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtRem" Enabled="false" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                    </ContentTemplate>

                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label19" runat="server" Text="Mnth.Est.Qty & UOM:"
                                                    ForeColor="Black"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="txtMQty" Enabled="false" runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="txtBaseUOM1" Enabled="false" runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="txtpartdesc" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label24" runat="server" Text="FA Date & Qty" ForeColor="Black"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="TxtFADate" OnclientClick="return false;" Enabled="false" runat="server" ForeColor="Black"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="TxtFAQty" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label25" runat="server" Text="1<sup>st</sup> Delivery Date & Qty" ForeColor="Black"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="TxtDelDate" runat="server" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="TxtDelQty" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- start whitout code gp field -->
                                <div runat="server" id="DvWhitoutCodeGpField" style="display: none;">
                                    <div class="row" style="padding-bottom: 10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label32" runat="server" Text="Incoterms"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TxtIncoterms" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                        </div>
                                    </div>
                                    <div class="row" style="padding-bottom: 10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label29" runat="server" Text="Packing Requirements" Enabled="false"
                                                        ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TxtPckRequirement" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label30" runat="server" Text="Others Requirements" Enabled="false" ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TxtOthRequirement" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- end whitout code gp field -->

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row" runat="server" id="DvPlatingType">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label43" runat="server" ForeColor="Black" Text="Plating Type"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="TxtPlatingType" Enabled="false" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row" runat="server" id="DvImRcylRatio" visible="false">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label37" runat="server" ForeColor="Black" Text="Recycle Ratio (%)"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="TxtImRcylRatio" Enabled="false" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;" runat="server" id="DvToolAmortize">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label44" runat="server" ForeColor="Black" Text="Use Tool Amortize"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="TxtIsUseToolAmor" Enabled="false" runat="server" CssClass="form-control-sm" Font-Size="14px"
                                                    ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>

                                <!-- BOM DATA Before Effective Date -->
                                <div class="row" style="padding-bottom: 10px; display: block;">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12" style="padding-bottom: 5px;">
                                                <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid blue">
                                                    <asp:Label ID="lbl_proces0" runat="server" ForeColor="Black" Font-Bold="true" Text="SMN BOM & Material Cost Details Before Effective Date"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="table-responsive table-sm">
                                                    <asp:GridView ID="GVBomListBefEffdate" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false"
                                                        EmptyDataText="No records Found" BackColor="White" CssClass="table-responsive table-sm" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="material" HeaderText="Raw Material Code" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Raw Material Desc" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="AmtSCur" HeaderText="Amt SCur" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="SellingCrcy" HeaderText="Selling Crcy" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="AmtVCur" HeaderText="Amt VCur" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="VendorCrcy" HeaderText="Vendor Crcy" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="Unit" HeaderText="Unit" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="ValidFrom" HeaderText="Valid From" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="center" />
                                                            <asp:BoundField DataField="ValidTo" HeaderText="Valid To" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="center" />
                                                            <asp:BoundField DataField="ExchRate" HeaderText="Exch Rate" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="ExchValFrom" HeaderText="Exch Rate Valid From" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                        </Columns>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#006699" ForeColor="White" />
                                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                        <RowStyle ForeColor="#000066" />
                                                        <SelectedRowStyle BackColor="#669999" ForeColor="White" />
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

                                <!-- BOM DATA Based On Effective Date-->
                                <div class="row" style="padding-bottom: 10px; display: block;">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12" style="padding-bottom: 5px;">
                                                <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid blue">
                                                    <asp:Label ID="Label33" runat="server" ForeColor="#ff3300" Font-Bold="true"
                                                        Text="New SMN BOM & Material Cost Details Based On Effective Date"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="table-responsive table-sm">
                                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="GvSMNBomEffctvDate" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false"
                                                                EmptyDataText="No records Found" BackColor="White" CssClass="table-responsive table-sm" Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField DataField="material" HeaderText="Raw Material Code" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="MaterialDesc" HeaderText="Raw Material Desc" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="AmtSCur" HeaderText="Amt SCur" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="SellingCrcy" HeaderText="Selling Crcy" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="AmtVCur" HeaderText="Amt VCur" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="VendorCrcy" HeaderText="Vendor Crcy" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="Unit" HeaderText="Unit" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField DataField="ValidFrom" HeaderText="Valid From" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="center" />
                                                                    <asp:BoundField DataField="ValidTo" HeaderText="Valid To" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="center" />
                                                                    <asp:BoundField DataField="ExchRate" HeaderText="Exch Rate" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="ExchValFrom" HeaderText="Exch Rate Valid From" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right" />
                                                                </Columns>
                                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                <HeaderStyle BackColor="#006699" ForeColor="White" />
                                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                                <RowStyle ForeColor="#000066" />
                                                                <SelectedRowStyle BackColor="#669999" ForeColor="White" />
                                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!--PIR old Quote Mass revision-->
                                <div class="row" style="padding-bottom: 10px;" runat="server" id="DvGvPIROldQuoteMass" visible="false">
                                    <div class="col-md-12">
                                        <div class="row" style="padding-top: 5px;">
                                            <div class="col-md-12" style="padding-bottom: 5px;">
                                                <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid blue">
                                                    <asp:Label ID="Label36" runat="server" ForeColor="Black" Text="Old Data From PIR"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="table-responsive table-sm">
                                                    <asp:GridView ID="GvQuoteDataPIR" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false"
                                                        EmptyDataText="No records Found" BackColor="White" CssClass="table-responsive table-sm" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="PIRNo" HeaderText="PIR No" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                            <asp:BoundField DataField="OldTotMatCost" HeaderText="Tot Mat Cost" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                            <asp:BoundField DataField="OldTotProCost" HeaderText="Tot Proc Cost" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                            <asp:BoundField DataField="OldTotSubMatCost" HeaderText="Tot Sub Mat Cost" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                            <asp:BoundField DataField="OldTotOthCost" HeaderText="Tot Oth Cost" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                            <asp:BoundField DataField="MassUpdateDate" HeaderText="Mass Update Date" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#006699" ForeColor="White" />
                                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                        <RowStyle ForeColor="#000066" />
                                                        <SelectedRowStyle BackColor="#669999" ForeColor="White" />
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

                                <%--Old Request Revice--%>
                                <div class="row" runat="server" id="DvQuoreReqRevice" style="padding-bottom: 10px; display: block;">
                                    <div class="col-md-12" style="padding-bottom: 5px;">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label35" runat="server" Text="Old Request Quotation"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-lg-12 table table-responsive" style="padding: 0px; display: block;">
                                            <asp:UpdatePanel ID="UpdatePanel23" runat="server" ChildrenAsTriggers="true"
                                                UpdateMode="Conditional" RenderMode="Block">
                                                <ContentTemplate>
                                                    <asp:GridView ID="GdvQuoreReqRevice" runat="server" AutoGenerateColumns="False"
                                                        OnRowDataBound="GdvQuoreReqRevice_RowDataBound" OnRowCommand="GdvQuoreReqRevice_RowCommand"
                                                        CssClass="table-responsive  table-sm table-bordered table-nowrap  Padding-Nol WrapCnt">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Old Quote No" HeaderStyle-CssClass="text-center ">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Text='<%# Eval("QuoteNo") %>' ItemStyle-Width="150px" runat="server" ID="LbGvQNo" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:BoundField DataField="vendor" HeaderText="Vendor" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="Product" HeaderText="Product" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialClass" HeaderText="Material Class" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="Material" HeaderText="Material" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="ProcessGroup" HeaderText="Process Group" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center "></asp:BoundField>--%>
                                                            <asp:TemplateField HeaderText="Mat Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                <ItemStyle Width="80px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMatRef" runat="server" Text='<%# Eval("TotalMaterialCost") %>' onclick="return false;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Proc Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                <ItemStyle Width="80px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkProcRef" runat="server" Text='<%# Eval("TotalProcessCost") %>' onclick="return false;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sub Mat Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                <ItemStyle Width="90px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSubMatRef" runat="server" Text='<%# Eval("TotalSubMaterialCost") %>' onclick="return false;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Others Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                <ItemStyle Width="80px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkOthRef" runat="server" Text='<%# Eval("TotalOtheritemsCost") %>' onclick="return false;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="FinalQuotePrice" HeaderText="Final Price" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Tool Amortize" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                <ItemStyle Width="80px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <div style="display:none;">
                                                                    <asp:CheckBox ID="chkIsUseToolAmortize" runat="server" Text="" onclick="return false;" ForeColor="Transparent"/>
                                                                    </div>
                                                                    <asp:Label Text='<%# Eval("ActionToolAmortize") %>' ItemStyle-Width="150px" runat="server" ID="LbToolAction" Font-Bold="true" ForeColor="Black" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Machine Amortize" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                <ItemStyle Width="80px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <div style="display:none;">
                                                                    <asp:CheckBox ID="chkIsUseMachineAmortize" runat="server" Text="" onclick="return false;" ForeColor="Transparent" />
                                                                    </div>
                                                                    <asp:Label Text='<%# Eval("ActionMachineAmortize") %>' ItemStyle-Width="150px" runat="server" ID="LbMacAction" Font-Bold="true" ForeColor="Black" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
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
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label18" runat="server" Text="To Be Filled By Vendor"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <asp:Label ID="Label12" runat="server" ForeColor="Black" Text="Effective Date"></asp:Label>
                                            </div>
                                            <div class="col-lg-7">
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <asp:TextBox ID="TextBox1" OnclientClick="return false;"
                                                            onkeydown="javascript:preventInput(event);"
                                                            autocomplete="off" AutoCompleteType="Disabled"
                                                            runat="server" ForeColor="Black" Enabled="false" CssClass="form_datetime">
                                                        </asp:TextBox>
                                                    </div>
                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 3px 4px 0px 3px;">
                                                        <i class="fa fa-calendar" style="color: #005496;" onclick="javascript: if(document.getElementById('TextBox1').disabled == false){$('#TextBox1').focus();}"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <asp:Label ID="Label13" runat="server" ForeColor="Black" Text="Due Dt Next Rev"></asp:Label>
                                            </div>
                                            <div class="col-lg-7">
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <asp:TextBox ID="txtfinal" OnclientClick="return false;" runat="server" onpaste="return false;"
                                                            onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                        </asp:TextBox>
                                                    </div>
                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 3px 4px 0px 3px;">
                                                        <i class="fa fa-calendar" style="color: #005496;" onclick="javascript: if(document.getElementById('txtfinal').disabled == false){$('#txtfinal').focus();}"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="DivDefaultDueOnDate" style="padding-top: 5px; display: none;">
                                            <div class="col-lg-5">
                                                <asp:Label ID="Label38" runat="server" ForeColor="Black" Text="Default Due Dt Next Rev"></asp:Label>
                                            </div>
                                            <div class="col-lg-7">
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <asp:TextBox ID="TxtDefaultDueDate" OnclientClick="return false;" runat="server" ForeColor="#ff3300" Enabled="false"
                                                            Font-Bold="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <asp:Label ID="Label28" runat="server" ForeColor="Black" Text="Country Of Origin"></asp:Label>
                                            </div>
                                            <div class="col-lg-7">
                                                <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlpirjtype" runat="server" AutoPostBack="true" CssClass=""
                                                            OnSelectedIndexChanged="ddlpirjtype_SelectedIndexChanged" Font-Bold="False" ForeColor="Black" Width="100%" Height="30px">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <asp:Label ID="Label31" runat="server" ForeColor="Black" Text="Attachment (Max Size: 3MB)"></asp:Label>
                                            </div>
                                            <div class="col-lg-7">
                                                <asp:UpdatePanel ID="UpdatePanel22" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:LinkButton runat="server" ID="LbAttach" OnClientClick="ClcBtnFlUpload();return false;" CssClass="lbattachpad">
                                                            <span class="glyphicon glyphicon-paperclip" style="padding: 0px;" runat="server" id="SpFileName"></span>
                                                        </asp:LinkButton>
                                                        <a><asp:Label ID="LbFlName" runat="server" ForeColor="Black" Text="No File" onclick="ClcBtnFlUpload();"></asp:Label></a>
                                                        <asp:LinkButton runat="server" ID="BtnDelAttachment" OnClientClick="DelAttachment();return false;" autopostback="false"
                                                            CssClass="lbattachpad pull-right">
                                                            <span class="glyphicon glyphicon-trash" style="padding: 0px; color: #fb0808;" runat="server" id="Span2"></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="BtnPreview" OnClientClick="Downloadfile();return false;"  autopostback="false"
                                                            CssClass="lbattachpad pull-right">
                                                            <span class="glyphicon glyphicon-download" style="padding: 0px; color: #0bd409;" runat="server" id="Span1"></span>
                                                        </asp:LinkButton>

                                                        <div style="display: none">
                                                            <asp:FileUpload ID="FlAttachment" runat="server" onchange="UploadFile();" autopostback="false" />
                                                            <asp:Button runat="server" ID="BtnUpload" Text="upload" OnClientClick="return CheckFileSize(); return false;" autopostback="false"  />
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <%--<asp:PostBackTrigger ControlID="BtnUpload" />--%>
                                                        <%--<asp:PostBackTrigger ControlID="BtnPreview" />--%>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label1" runat="server" Text="Vendor Detail"
                                                Font-Size="18px" ForeColor="#2153a5" Visible="false"></asp:Label>
                                            <asp:Label ID="lblmatlcost" runat="server" Text="PART II: MATERIAL COST"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <%--button add and delete material--%>
                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAddColumns" onkeypress="return false" runat="server"
                                                    Text="Add Material" OnClientClick="MCDataStore()" OnClick="btnAddColumns_Click" Width="250px" CssClass="btn btn-success" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAddColumns" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        <%--button delete material--%>
                                        <div style="display: none">
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="BtnDelMaterial" onkeypress="return false" runat="server"
                                                        Text="Delete Material" OnClick="BtnDelMaterial_Click" CssClass="Login-button" />
                                                    <asp:Label ID="LbColumnNo" runat="server" Text="1"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BtnDelMaterial" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                                <%--table material cost--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
                                        <div class="table-responsive table-sm Padding-Nol">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Table ID="Table1" runat="server" class="table-bordered table-nowrap"></asp:Table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddColumns" />
                                                </Triggers>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BtnDelMaterial" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="col-md-12" id="DvOldMatCost" runat="server" visible="false">
                                        <asp:Label runat="server" ID="txtOldMatcost" Text="Old Total Material Cost/pc :"></asp:Label>
                                    </div>
                                </div>

                                <%--button calcultae mat cost--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button2" runat="server" Text="Calculate Material Cost" Width="250px"
                                            OnClientClick="if(ValidateMatCalc()==false) return false;MatlCalculation(); return false;" UseSubmitBehavior="false"
                                            CssClass="btn btn-info " OnClick="Button2_Click" />
                                        <asp:Label ID="LbNoteMaterial" runat="server" Text="<b><font color='red' size='4'>*</font></b> If Material Cost have only one column and if, Base Qty/Cavity, is changed, after the Process Cost calculation, Please reselect the Process Group again & recalculate the Process Cost"></asp:Label>
                                    </div>
                                </div>

                                <div id="DvProcessCostPart" style="display: block;">
                                    <%--label part III--%>
                                    <div class="row" style="padding-bottom: 10px;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label4" runat="server" Text="PART III: PROCESS COST"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <%--button add and delete process cost--%>
                                    <div class="row" style="padding-bottom: 10px">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnaddProcessCost" runat="server" Text="Add Process" Width="250px"
                                                        CssClass="btn btn-success" OnClientClick="ProcessCostDataStore()" OnClick="btnaddProcessCost_Click" />
                                                    <div style="display: none;">
                                                        <asp:Button ID="btnClickCalcPrcUOMStkMin" runat="server" OnClick="btnClickCalcPrcUOMStkMin_Click" />
                                                        <asp:Button ID="BtnFndVndMachine" runat="server" OnClick="BtnFndVndMachine_Click" />
                                                        <asp:LinkButton ID="BtnFndVndMachineVsProcUom" runat="server" OnClick="BtnFndVndMachineVsProcUom_Click" />
                                                        <asp:LinkButton ID="BtnFndProcUom" runat="server" OnClick="BtnFndProcUom_Click" />
                                                        <asp:LinkButton ID="BtnFndVndRate" runat="server" OnClick="BtnFndVndRate_Click" />
                                                        <asp:Button ID="BtnMacList" runat="server" OnClick="BtnMacList_Click" />
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnaddProcessCost" />
                                                </Triggers>
                                            </asp:UpdatePanel>

                                            <%--button delete process--%>
                                            <div style="display: none">
                                                <asp:UpdatePanel ID="UpdatePanel13" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="BtnDelProcess" onkeypress="return false" runat="server"
                                                            Text="Delete Process" OnClick="BtnDelProcess_Click" CssClass="Login-button" />
                                                        <asp:Label ID="Label5" runat="server" Text="1"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnDelProcess" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>

                                    <%--table process cost--%>
                                    <div class="row" style="padding-bottom: 0px;">
                                        <div class="col-md-12">
                                            <div class="table table-responsive table-sm">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Table ID="TablePC" runat="server" CssClass="table-bordered table-nowrap"></asp:Table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnaddProcessCost" />
                                                    </Triggers>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnDelProcess" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>

                                    <%--button calcultae process cost--%>
                                    <div class="row" style="padding-bottom: 10px;">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="upCalculateBtnProcCost" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate></ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:Button ID="Button6" runat="server" Text="Calculate Process Cost" OnClientClick="if(ValidateProcCost()==false) return false;processcost();
                                                            if(CekMachineAmortizeFromProcCostTable(false) == false) return false; return false"
                                                            CssClass="btn btn-info" UseSubmitBehavior="False" Width="250px" />
                                        </div>
                                    </div>
                                </div>

                                <div id="DvSubMatPart" style="display: block;">
                                    <%--label part IV--%>
                                    <div class="row" style="padding-bottom: 10px;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label3" runat="server" Text="PART IV: SUB-MAT/T&amp;J COST"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <%--button add and delete SUB-MAT/T&J COST--%>
                                    <div class="row" style="padding-bottom: 10px">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnAddSubProcessCost" runat="server" Text="Add SUB-MAT Cost" Width="250px"
                                                        CssClass="btn btn-success" OnClientClick="if(CekMachineAmortizeFromProcCostTable2(false) == false) return false;" 
                                                        OnClick="btnAddSubProcessCost_Click" />
                                                    <div style="display:none">
                                                        <asp:Button ID="BtnProcMachineAmor" runat="server" OnClick="BtnProcMachineAmor_Click" />
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddSubProcessCost" />
                                                </Triggers>
                                            </asp:UpdatePanel>

                                            <%--btndel submat cost--%>
                                            <div style="display: none">
                                                <asp:UpdatePanel ID="UpdatePanel14" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="BtnDelSubMatCost" onkeypress="return false" runat="server"
                                                            Text="Delete Material" OnClick="BtnDelSubMatCost_Click" CssClass="Login-button" />
                                                        <asp:Label ID="Label8" runat="server" Text="1"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnDelSubMatCost" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>

                                    <%--table SUB-MAT/T&J COST--%>
                                    <div class="row" style="padding-bottom: 0px;">
                                        <div class="col-md-12">
                                            <div class="table table-responsive table-sm">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                    <ContentTemplate>

                                                        <asp:Table ID="TableSMC" runat="server" class="table-bordered table-nowrap"></asp:Table>

                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddSubProcessCost" />
                                                    </Triggers>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnDelSubMatCost" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>

                                    <%--button calcultae SUB-MAT/T&J COST--%>
                                    <div class="row" style="padding-bottom: 10px;">
                                        <div class="col-md-12">
                                            <asp:Button ID="Button4" runat="server" Text="Calculate Sub Material Cost"
                                                OnClientClick="if(ValidateSubMat()==false) return false;
                                                if(CekMachineAmortizeFromProcCostTable(false) == false) return false;
                                                return false" UseSubmitBehavior="false"
                                                CssClass="btn btn-info" Width="250px" />
                                        </div>
                                    </div>
                                </div>

                                <%--label part V--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label6" runat="server" Text="PART V: OTHER COST"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <%--button add and delete OTHER COST--%>
                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAddOtherCost" runat="server" Text="Add Others" Width="250px"
                                                    CssClass="btn btn-success" OnClientClick="OthersCostDataStore()" OnClick="btnAddOtherCost_Click" />

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAddOtherCost" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        <%--button delete other--%>
                                        <div style="display: none">
                                            <asp:UpdatePanel ID="UpdatePanel15" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="BtnDelOthCost" onkeypress="return false" runat="server"
                                                        Text="Delete Other Cost" OnClick="BtnDelOthCost_Click" CssClass="Login-button" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BtnDelMaterial" />
                                                </Triggers>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BtnDelOthCost" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                                <%--table OTHER COST--%>
                                <div class="row" style="padding-bottom: 0px;">
                                    <div class="col-md-12">
                                        <div class="table table-responsive table-sm">
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Table ID="TableOthers" runat="server" class="table-bordered table-nowrap"></asp:Table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddOtherCost" />
                                                </Triggers>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BtnDelOthCost" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                                <%--button calcultae OTHER COST--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button5" runat="server" Text="Calculate Other Cost"
                                            OnClientClick="if(ValidateOthCost()==false) return false;othercost(); return false" Width="250px"
                                            CssClass="btn btn-info" UseSubmitBehavior="false" />
                                    </div>
                                </div>

                                <%--label part VI PART UNIT PRICE--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label7" runat="server" Text="PART VI: PART UNIT PRICE"></asp:Label>
                                            <asp:Label ID="lblcry" runat="server" ForeColor="Yellow"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <%--table PART UNIT PRICE--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
                                        <div class="table table-responsive table-sm">
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnAddPart" Visible="false" runat="server" Text="Add PART UNIT"
                                                        CssClass="Login-button" />

                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddPart" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Table ID="TableUnit" runat="server" class="table-bordered table-nowrap">
                                                    </asp:Table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddPart" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                                <%--comment by vendor--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:Label ID="lbComnt" runat="server" Text="Comment By Vendor"></asp:Label>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="TxtComntByVendor" Enabled="true" runat="server" Height="50px" TextMode="MultiLine" onkeyup="ComntByVendorLght()" CssClass="form-control"></asp:TextBox>
                                                <div style="text-align: right">
                                                    <asp:Label ID="LblengtVC" runat="server" Text="150 character left " Font-Size="11px"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%--button submit and save as draft--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnSaveDraft" runat="server" Text="Save As Draft"
                                                    CssClass="btn btn-primary" OnClick="BtnSaveDraft_Click" OnClientClick="if(cekIsHaveOldToolOrmachineAmortize(true) == false) return false;ReCalculate();SaveAsDraftData();ShowLoading();" />
                                                <asp:Button ID="Button1" runat="server" Text="Submit" CssClass="btn btn-primary"
                                                    OnClientClick="if(cekIsHaveOldToolOrmachineAmortize(false) == false) return false;if(validateTotalValue()==false) return false;if(ValidateMatCalc()==false) return false;MatlCalculation();if(ValidateProcCost()==false) return false;processcost();if(ValidateSubMat()==false) return false;submatlcost();if(ValidateOthCost()==false) return false;othercost();if(ValidateUnit()==false) return false;ReCalculate();ShowLoading();" 
                                                    OnClick="Button1_Click1" />
                                                <asp:Label ID="lblcreateuser" runat="server" Visible="False"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="Button1" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                            </div>

                            <asp:GridView ID="grdVendrDet" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                <Columns>
                                    <asp:BoundField DataField="REQUESTDATE" HeaderText="Request Date" />
                                    <asp:BoundField DataField="QUOTENO" HeaderText="Quote No" />
                                    <asp:BoundField DataField="Description" HeaderText="Vendor Name" />
                                    <asp:BoundField DataField="Crcy" HeaderText=" Quote Currency" />
                                    <asp:BoundField DataField="PICName" HeaderText="PIC Name" />
                                    <asp:BoundField DataField="PICemail" HeaderText="PIC Email" />
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                <HeaderStyle BackColor="#1a2e4c" ForeColor="White" />
                                <PagerStyle BackColor="#1a2e4c" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>

                            <asp:GridView ID="grdProcessGrphidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                <Columns>

                                    <asp:BoundField DataField="ProcessGrpCode" HeaderText="Process Grp Code" />
                                    <asp:BoundField DataField="SubProcessName" HeaderText="Sub Process Name" />
                                    <asp:BoundField DataField="ProcessUomDescription" HeaderText="Process UOM Description" />
                                    <asp:BoundField DataField="ProcessUOM" HeaderText="Process UOM" />
                                    <asp:BoundField DataField="ProcessGrpCode" HeaderText="ProcessGroup" />



                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                <HeaderStyle BackColor="#1a2e4c" ForeColor="White" />
                                <PagerStyle BackColor="#1a2e4c" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>

                            <asp:UpdatePanel runat="server" ID="UpgrdSubProcessGrphidden">
                                <ContentTemplate>
                                    <asp:GridView ID="grdSubProcessGrphidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                        Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                        <Columns>
                                            <asp:BoundField DataField="SubProcessName" HeaderText="Sub Process Name" />
                                            <asp:BoundField DataField="ProcessUomDescription" HeaderText="Process UOM Description" />
                                            <asp:BoundField DataField="ProcessUOM" HeaderText="Process UOM" />
                                            <asp:BoundField DataField="ProcessGrpCode" HeaderText="ProcessGroup" />
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                        <HeaderStyle BackColor="#1a2e4c" ForeColor="White" />
                                        <PagerStyle BackColor="#1a2e4c" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div style="display: none">
                                <asp:UpdatePanel runat="server" ID="UpgrdMachinelisthidden">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdMachinelisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                            ShowHeaderWhenEmpty="true" Style="color: #333333; border-collapse: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                            <Columns>
                                                <asp:BoundField DataField="Machine" HeaderText="Machine" />
                                                <asp:BoundField DataField="SMNStdrateHr" HeaderText="SMNStdrateHr" />
                                                <asp:BoundField DataField="FollowStdRate" HeaderText="FollowStdRate" />
                                                <asp:BoundField DataField="Currency" HeaderText="CURR" />
                                                <asp:BoundField DataField="ProcessGrp" HeaderText="ProcessGroup" />
                                                <asp:BoundField DataField="MacId" HeaderText="MacId" />

                                            </Columns>

                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                            <HeaderStyle BackColor="#1a2e4c" ForeColor="White" />
                                            <PagerStyle BackColor="#1a2e4c" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div style="display: none;">
                                <asp:GridView ID="grdLaborlisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4" ShowHeaderWhenEmpty="true"
                                    Style="color: #333333;">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />

                                    <Columns>
                                        <asp:BoundField DataField="StdLabourRateHr" HeaderText="StdLabourRateHr" />
                                        <asp:BoundField DataField="FollowStdRate" HeaderText="FollowStdRate" />
                                        <asp:BoundField DataField="Currency" HeaderText="CURR" />

                                    </Columns>

                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                    <HeaderStyle BackColor="#1a2e4c" ForeColor="White" />
                                    <PagerStyle BackColor="#1a2e4c" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                            </div>

                            <div style="display: none;">
                                <asp:GridView ID="GvProcessGrpVsLayout" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                    Style="color: #333333;" OnRowDataBound="GvProcessGrpVsLayout_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                    <Columns>
                                        <asp:BoundField DataField="ProcessGrp" HeaderText="ProcessGrp" />
                                        <asp:BoundField DataField="ScreenLayout" HeaderText="ScreenLayout" />
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div style="display: none">
                                <asp:UpdatePanel runat="server" ID="UpMachineAmor">
                                    <ContentTemplate>
                                        <div class="table table-sm table-responsive">
                                        <asp:GridView ID="GvMachineAmor" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                            ShowHeaderWhenEmpty="true" Style="color: #333333; border-collapse: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                            <Columns>
                                                <asp:BoundField DataField="Plant" HeaderText="Plant" />
                                                <asp:BoundField DataField="VendorCode" HeaderText="VendorCode" />
                                                <asp:BoundField DataField="VendorCurrency" HeaderText="VendorCurrency" />
                                                <asp:BoundField DataField="Process_Grp_code" HeaderText="Process_Grp_code" />
                                                <asp:BoundField DataField="Vend_MachineID" HeaderText="Vend_MachineID" />
                                                <asp:BoundField DataField="AmortizeCost" HeaderText="AmortizeCost" />
                                                <asp:BoundField DataField="AmortizeCurrency" HeaderText="AmortizeCurrency" />
                                                <asp:BoundField DataField="ExchangeRate" HeaderText="ExchangeRate" />
                                                <asp:BoundField DataField="AmortizeCost_Vend_Curr" HeaderText="AmortizeCost_Vend_Curr" />
                                                <asp:BoundField DataField="AmortizePeriod" HeaderText="AmortizePeriod" />
                                                <asp:BoundField DataField="AmortizePeriodUOM" HeaderText="AmortizePeriodUOM" />
                                                <asp:BoundField DataField="TotalAmortizeQty" HeaderText="TotalAmortizeQty" />
                                                <asp:BoundField DataField="QtyUOM" HeaderText="QtyUOM" />
                                                <asp:BoundField DataField="AmortizeCost_Pc_Vend_Curr" HeaderText="AmortizeCost_Pc_Vend_Curr" />
                                                <asp:BoundField DataField="EffectiveFrom" HeaderText="EffectiveFrom" />
                                                <asp:BoundField DataField="DueDate" HeaderText="DueDate" />
                                            </Columns>
                                        </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <asp:HiddenField ID="ddlSubprocess" runat="server" Value="" />
                            <asp:HiddenField ID="ddlUom" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTProCost" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTProCost1" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTSumMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTOtherCost" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTGTotal" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTFinalQPrice" runat="server" Value="" />
                            <asp:HiddenField ID="hdnProfit" runat="server" Value="" />
                            <asp:HiddenField ID="hdnDiscount" runat="server" Value="" />
                            <asp:HiddenField ID="hdnGA" runat="server" Value="" />

                            <asp:HiddenField ID="hdnSTDRate" runat="server" Value="" />
                            <asp:HiddenField ID="hdnVendorRate" runat="server" Value="" />
                            <asp:HiddenField ID="hdnUOM" runat="server" Value="" />
                            <asp:HiddenField ID="hdnVendorActivity" runat="server" Value="" />


                            <asp:HiddenField ID="hdnSMCTable" runat="server" Value="" />

                            <asp:HiddenField ID="hdnSMCTableValues" runat="server" Value="" />
                            <asp:HiddenField ID="hdnSMCTableCount" runat="server" Value="" />


                            <asp:HiddenField ID="hdnOtherValues" runat="server" Value="" />
                            <asp:HiddenField ID="hdnOthersTableCount" runat="server" Value="" />
                            <asp:HiddenField ID="hdnProcessValues" runat="server" Value="" />
                            <asp:HiddenField ID="hdnProcessTableCount" runat="server" Value="" />
                            <asp:HiddenField ID="hdnMCTableValues" runat="server" Value="" />
                            <asp:HiddenField ID="hdnMCTableCount" runat="server" Value="" />

                            <asp:HiddenField ID="hdnMCTableRawMatUom" runat="server" Value="" />

                            <asp:HiddenField ID="hdnLayoutScreen" runat="server" Value="" />
                            <asp:HiddenField ID="hdnHidenProfit" runat="server" Value="" />
                            <asp:HiddenField ID="HdnNetProfnDisc" runat="server" Value="" />

                            <asp:HiddenField ID="hdnColumTblProcNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnProcGroup" runat="server" Value="" />
                            <asp:HiddenField ID="hdnSubProcGroup" runat="server" Value="" />
                            <asp:HiddenField ID="hdnUnitValues" runat="server" Value="" />
                            <asp:UpdatePanel runat="server" ID="uphdnStdrRate">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hdnStdrRate" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnFollowStdRate" runat="server" Value="" />
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel runat="server" ID="UphdnProcUOM">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hdnProcUOM" runat="server" Value="" />
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:HiddenField ID="hdnMachineId" runat="server" Value="" />
                            <asp:HiddenField ID="hdnMachineType" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTonnage" runat="server" Value="" />
                            <asp:HiddenField ID="HdnFirsLoad" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnVendorType" runat="server" Value="" />
                            <asp:HiddenField ID="hdnQuoteNoRef" runat="server" Value="" />
                            <asp:HiddenField ID="hdnIsSAPCode" runat="server" Value="" />

                            <asp:HiddenField ID="hdnRevOfEmet" runat="server" Value="" />
                            <asp:HiddenField ID="hdnMassRevision" runat="server" Value="" />
                            <asp:HiddenField ID="HdnMAssTotMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnMAssTotProcCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnMAssTotSubMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnMAssTotOthCost" runat="server" Value="" />

                            <asp:HiddenField ID="HdnSAPSpProcType" runat="server" Value="" />
                            <asp:HiddenField ID="HdnAcsTabMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnAcsTabProcCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnAcsTabSubMatCost" runat="server" Value="" />
                            
                            <asp:HiddenField ID="HdnIsUseToolAmortize" runat="server" Value="" />
                            <asp:HiddenField ID="HdnIsUseMachineAmor" runat="server" Value="0" />
                            <asp:HiddenField ID="HdnMachinListprocCost" runat="server" Value="" />
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

        <!-- Modal MyModalUOM -->
        <div class="modal fade" id="MyModalUOM" data-backdrop="static" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;">
                                <div class="col-lg-12 Padding-Nol" style="font: bold 22px calibri, calibri; text-align: center; align-content: center;">Please Select UOM For Below Material </div>
                                <h4></h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div id="DvUOM">
                                                <%--<table id="TbUomMatCost" class="table table-bordered table-responsive" style="font-weight:lighter!important">
                                                    <thead style="background-color:#1A2E4C; font-weight:lighter!important">
                                                        <th> <font color="white" style="font-weight:lighter!important">No.</font> </th>
                                                        <th><font color="white" style="font-weight:lighter!important">Raw Mat SAP Code</font> </th>
                                                        <th><font color="white" style="font-weight:lighter!important">Raw Mat Desc</font> </th>
                                                        <th><font color="white" style="font-weight:lighter!important">Raw Mat Cost</font> </th>
                                                        <th><font color="white" style="font-weight:lighter!important">Ttl Raw Mat Cost</font> </th>
                                                        <th><font color="white" style="font-weight:lighter!important">Raw Mat UOM</font> </th>
                                                    </thead>
                                                    <tr>
                                                        <td>1</td>
                                                        <td>TEST</td>
                                                        <td>TEST</td>
                                                        <td>0.0000</td>
                                                        <td>0.0000</td>
                                                        <td>
                                                            <select id="DdlModalUom_0">
                                                                <option value="0">--Select--</option>
                                                                <option value="KG">KG</option>
                                                                <option value="G">G</option>
                                                                <option value="PC">PC</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                </table>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-top-left-radius: 15px; border-top-right-radius: 15px;">
                                <a class="btn btn-sm btn-primary" id="BtnSubmitMdUOM" onclick="if(ValidateSubmitModalUOM()==false)return false;SubmitModalUOM()">Submit</a>
                                <a class="btn btn-sm btn-default" id="BtnCancelMdUOM" onclick="CloseModalUOM()">Cancel</a>
                            </div>
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
    </form>
</body>
</html>


