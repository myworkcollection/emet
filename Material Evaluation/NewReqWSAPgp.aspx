<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewReqWSAPgp.aspx.cs" Inherits="Material_Evaluation.NewReqWSAPgp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />

    <!-- Page level plugin CSS-->
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" />

    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet" />

    <link href="Styles/NewStyle/NewStyle.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link rel="stylesheet" href="Scripts/jquery-ui-1.12.1/jquery-ui.css" />
    <link rel="stylesheet" href="js/jsextendsession/css/timeout-dialog.css" />
    <link rel="stylesheet" href="Scripts/multiselect/bootstrap-multiselect.css" />
    <link href="js/BootstrapDatePcr/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <style type="text/css">
        .Unwrap th {
            white-space: pre-wrap;
            font-size: 14px;
        }

        label {
            font-weight: normal !important;
        }

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

        .rbfont {
            font-weight: bold;
        }

        .blockddlslc {
            padding: 2px 5px;
            font-size: 14px;
            width: 100%;
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
    <script type="text/javascript" src="Scripts/multiselect/bootstrap-multiselect.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/locales/bootstrap-datetimepicker.fr.js"></script>

    <%--script loading page--%>
    <script lang="javascript" type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });

        $(document).ready(function () {
            CloseLoading();
            ChangeEmptyFieldColor();
            multiselectDropDown();
            DatePitcker();
        });
    </script>
    <script type="text/javascript">
        function ValOnlyNo(txtID, source) {
            //page related : review_reqmass,newrewwsapgp,newrequest,newreqchangemass,newreq_changes,reciew_req
            try {
                var regex = /^[-+]?\d*\.?\d*$/;
                var fullval = document.getElementById(txtID).value;
                var val = parseFloat(document.getElementById(txtID).value);
                var IsValid = false;
                if (document.getElementById(txtID).value != "") {
                    if (!(regex.test(fullval))) {
                        if (source == 'txt') {
                            alert("Only Allow Decimal Number!");
                        }
                        document.getElementById(txtID).focus();
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



        function EmptyRequestList() {
            try {
                if ($("#Table1") != null && $("#Button1") != null) {
                    $("#Table1").hide();
                    $("#Button1").hide();
                    ChangeEmptyFieldColor();
                }
            }
            catch (err) {
                alert(err + ": EmptyRequestList");
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
                        //format: "dd-mm-yyyy - hh:ii",
                        startDate: new Date(),
                        format: "dd-mm-yyyy",
                        autoclose: true,
                        todayBtn: true,
                        todayHighlight: true,
                        minView: 2
                    });
                })(jQuery);
            } catch (e) {
                alert(e);
            }
        }

        function CharlgtCode(id) {
            var MaxLength = 50;
            var a = document.getElementById(id).value;
            $('#' + id).keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });

            a = document.getElementById(id).value;

            if (a.length > MaxLength) {
                a = a.slice(0, MaxLength);
                $("#" + id).val(a);
            }
        }

        function CharlgtDesc(id) {
            var MaxLength = 100;
            var a = document.getElementById(id).value;
            $('#' + id).keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });

            a = document.getElementById(id).value;

            if (a.length > MaxLength) {
                a = a.slice(0, MaxLength);
                $("#" + id).val(a);
            }
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

        function ResetFiledColor(txtID) {
            if (txtID == "FileUpload1") {
                document.getElementById(txtID).style.border = "1px solid #CCCCCC";
            }
            else if (txtID == "DvPlantRequestor") {
                document.getElementById(txtID).style.border = "0px solid #CCCCCC";
            }
            else {
                document.getElementById(txtID).style.border = "1px solid #CCCCCC";
            }
        }

        function ResetBackColorField() {
            try {
                $("#txtpartdesc").css("backgroundColor", "white");
                $("#txtpartdescription").css("backgroundColor", "white");
                $("#txtMQty").css("backgroundColor", "white");
                $("#txtBaseUOM1").css("backgroundColor", "white");
                $("#txtunitweight").css("backgroundColor", "white");
                $("#txtUOM").css("backgroundColor", "white");
                $("#TxtFADate").css("backgroundColor", "white");
                $("#TxtFAQty").css("backgroundColor", "white");
                $("#TxtDelDate").css("backgroundColor", "white");
                $("#TxtDelQty").css("backgroundColor", "white");
                $("#DdlReason").css("backgroundColor", "white");
                $("#DdlIncoterms").css("backgroundColor", "white");
                $("#TxtPckRequirement").css("backgroundColor", "white");
                $("#TxtOthRequirement").css("backgroundColor", "white");
                $("#DdlPlantRequestor").css("backgroundColor", "white");
                $("#txtplatingtype").css("backgroundColor", "white");
                $("#FileUpload1").css("backgroundColor", "#F7F7F7");
                $("#ddlprocess").css("backgroundColor", "white");
                $("#txtDate").css("backgroundColor", "white");
            }
            catch (err) {
                alert(err + ":ResetBackColorField()")
            }
        }

        function ChangeEmptyFieldColor() {
            try {
                $(function () {
                    if ($("#txtpartdesc").val() == "") {
                        $("#txtpartdesc").css("border", "1px solid #ff0000");
                    }
                    if ($("#txtpartdescription").val() == "") {
                        $("#txtpartdescription").css("border", "1px solid #ff0000");
                    }
                    if ($("#txtMQty").val() == "") {
                        $("#txtMQty").css("border", "1px solid #ff0000");
                    }
                    if ($("#txtBaseUOM1").val() == "") {
                        $("#txtBaseUOM1").css("border", "1px solid #ff0000");
                    }
                    //if ($("#txtunitweight").val() == "") {
                    //    $("#txtunitweight").css("border", "1px solid #ff0000");
                    //}
                    //if ($("#txtUOM").val() == "") {
                    //    $("#txtUOM").css("border", "1px solid #ff0000");
                    //}
                    if ($("#txtDate").val() == "") {
                        $("#txtDate").css("border", "1px solid #ff0000");
                    }
                    if ($("#DdlReason")[0].selectedIndex == 0) {
                        $("#DdlReason").css("border", "1px solid #ff0000");
                    }
                    if ($("#DdlIncoterms")[0].selectedIndex == 0) {
                        $("#DdlIncoterms").css("border", "1px solid #ff0000");
                    }

                    if ($("#ddlprocess")[0].selectedIndex == 0) {
                        $("#ddlprocess").css("border", "1px solid #ff0000");
                    }

                    var e = document.getElementById("ddlprocess");
                    var value = e.options[e.selectedIndex].value;
                    if (value == "IM") {
                        if (ddl != null) {
                            var ddl = document.getElementById("DdlImRcylRatio");
                            var ddlvalue = ddl.options[ddl.selectedIndex].value;
                            if (ddlvalue == "SELECT") {
                                $("#DdlImRcylRatio").css("border", "1px solid #ff0000");
                            }
                            else if (ddlvalue == "NO DATA") {
                                $("#DdlImRcylRatio").css("border", "1px solid #ff0000");
                            }
                        }
                    }

                    if ($("#lblMessage").text() == "") {
                        $("#FileUpload1").css("border", "1px solid #ff0000");
                    }

                    if ($("#DdlToolAmortize")[0].selectedIndex == 0) {
                        $("#DdlToolAmortize").css("border", "1px solid #ff0000");
                    }

                    var SBM = document.getElementById("RbTeamShimano").checked;
                    if (SBM == true) {
                        if ($("#DdlToolAmortize").val() == "NO" || $("#DdlToolAmortize")[0].selectedIndex == 0) {
                            var DdlVendor = $("#DdlVendor option").length;
                            var VndStatus = $("#DdlVendor option:selected").text();
                            if (VndStatus == "--Select Vendor--") {
                                $("#DdlVendor").css("border", "1px solid #ff0000");
                            }
                            else if (VndStatus == "--Vendor Not Exist--") {
                                $("#DdlVendor").css("border", "1px solid #ff0000");
                            }
                        }
                    }

                    //if ($("#DdlPlantRequestor")[0].selectedIndex == 0) {
                    //    $("#DdlPlantRequestor").css("border", "1px solid #ff0000");
                    //}
                    PlantRequestor = document.getElementById('LbSPlantRequestor');
                    var IsSelect = false;
                    for (var i = 0; i < PlantRequestor.length; i++) {
                        if (PlantRequestor.options[i].selected) {
                            IsSelect = true;
                        }
                    }
                    if (IsSelect == false) {
                        $("#DvPlantRequestor").css("border", "1px solid #ff0000");
                        $("#DvPlantRequestor").css("border-radius", "4px");
                    }

                }); (jQuery);
            }
            catch (err) {
                alert(err + ": ChangeEmptyFieldColor");
            }
        }

        function validateCreateReq() {
            ResetBackColorField();
            var err = "";
            err += "Please check field listed in below : \n";
            var iserr = false;

            PlantRequestor = document.getElementById('LbSPlantRequestor');
            var IsSelect = false;
            if (PlantRequestor.length > 0) {
                for (var i = 0; i < PlantRequestor.length; i++) {
                    if (PlantRequestor.options[i].selected) {
                        IsSelect = true;
                    }
                }
                if (IsSelect == false) {
                    $("#DvPlantRequestor").css("border", "1px solid #ff0000");
                    $("#DvPlantRequestor").css("border-radius", "4px");
                    err += "Please select GP Request Plant \n";
                    var iserr = true;
                }
            }
            else {
                $("#DvPlantRequestor").css("border", "1px solid #ff0000");
                $("#DvPlantRequestor").css("border-radius", "4px");
                err += "GP Request Plant not exist, Please contact administrator \n";
                var iserr = true;
            }

            if ($("#txtpartdesc").val() == "") {
                err += "Part Code cannot be empty \n";
                $("#txtpartdesc").css("border", "1px solid #ff0000");
                var iserr = true;
            }
            if ($("#txtpartdescription").val() == "") {
                err += "Part Code Description cannot be empty \n";
                $("#txtpartdescription").css("border", "1px solid #ff0000");
                var iserr = true;
            }
            if ($("#txtMQty").val() == "") {
                err += "Mnth.Est.Qty cannot be empty \n";
                $("#txtMQty").css("border", "1px solid #ff0000");
                var iserr = true;
            }
            if ($("#txtMQty").val() != "") {
                if (ValOnlyNo('txtMQty', 'btn') == false) {
                    err += "Mnth.Est.Qty Decimal No Only \n";
                    $("#txtMQty").css("border", "1px solid #ff0000");
                    var iserr = true;
                }
            }
            if ($("#txtBaseUOM1").val() == "") {
                err += "Base UOM cannot be empty \n";
                $("#txtBaseUOM1").css("border", "1px solid #ff0000");
                var iserr = true;
            }
            if ($("#txtunitweight").val() != "") {
                if (ValOnlyNo('txtunitweight', 'btn') == false) {
                    err += "Net Weight Decimal No Only \n";
                    $("#txtunitweight").css("border", "1px solid #ff0000");
                    var iserr = true;
                }
            }
            if ($("#TxtFAQty").val() != "") {
                if (ValOnlyNo('TxtFAQty', 'btn') == false) {
                    err += "FA Qty Decimal No Only \n";
                    $("#TxtFAQty").css("border", "1px solid #ff0000");
                    var iserr = true;
                }
            }
            if ($("#TxtDelQty").val() != "") {
                if (ValOnlyNo('TxtDelQty', 'btn') == false) {
                    err += "Del Qty Decimal No Only \n";
                    $("#TxtDelQty").css("border", "1px solid #ff0000");
                    var iserr = true;
                }
            }

            //if ($("#txtUOM").val() == "") {
            //    err += "Net Weight UOM cannot be empty \n";
            //    $("#txtUOM").css("border", "1px solid #ff0000");
            //    var iserr = true;
            //}
            if ($("#txtDate").val() == "") {
                err += "Please select Quotation response due date \n";
                $("#txtDate").css("border", "1px solid #ff0000");
                var iserr = true;
            }
            if ($("#DdlReason")[0].selectedIndex == 0) {
                var Reason = $("#DdlReason option:selected").text();
                if (Reason == "--Select Request Purpose--") {
                    err += "Please select Request Purpose \n";
                }
                else if (Reason == "--Request Purpose Not Exist--") {
                    err += "Request Purpose not Exist, please contact Administrator \n";
                }
                else {
                    err += "Invalid Reason \n";
                }
                $("#DdlReason").css("border", "1px solid #ff0000");
                var iserr = true;
            }
            if ($("#DdlIncoterms")[0].selectedIndex == 0) {
                var Reason = $("#DdlIncoterms option:selected").text();
                if (Reason == "--Select Incoterm--") {
                    err += "Please select Incoterm \n";
                }
                else if (Reason == "--Incoterm Not Exist--") {
                    err += "Incoterm not Exist, please contact Administrator \n";
                }
                else {
                    err += "Invalid Incoterm \n";
                }
                $("#DdlIncoterms").css("border", "1px solid #ff0000");
                var iserr = true;
            }


            if ($("#lblMessage").text() == "") {
                err += "Drawing Number should not be null! \n";
                $("#FileUpload1").css("border", "1px solid #ff0000");
                var iserr = true;
            }
            if ($("#ddlprocess")[0].selectedIndex == 0) {
                var Reason = $("#ddlprocess option:selected").text();
                if (Reason == "-- Select Process --") {
                    err += "Please Select Process \n";
                }
                else if (Reason == "--Process Not Exist--") {
                    err += "Process Group not Exist, please contact Administrator \n";
                }
                else {
                    err += "Invalid Process Group \n";
                }
                $("#ddlprocess").css("border", "1px solid #ff0000");
                var iserr = true;
            }

            var e = document.getElementById("ddlprocess");
            var value = e.options[e.selectedIndex].value;
            if (value == "IM") {
                var ddl = document.getElementById("DdlImRcylRatio");
                var ddlvalue = ddl.options[ddl.selectedIndex].value;
                if (ddlvalue == "SELECT") {
                    $("#DdlImRcylRatio").css("border", "1px solid #ff0000");
                    err += "Please Select Im Recycle Ratio! \n";
                    iserr = true;
                }
                else if (ddlvalue == "NO DATA") {
                    $("#DdlImRcylRatio").css("border", "1px solid #ff0000");
                    err += "Im Recycle Ratio not exist, Please contact Administrator! \n";
                    iserr = true;
                }
            }

            if ($("#DdlToolAmortize")[0].selectedIndex <= 0) {
                $("#DdlToolAmortize").css("border", "1px solid #ff0000");
                err += "Please select Use Tool Amortize Condition. ! \n";
                iserr = true;
            }

            if (iserr == true) {
                alert(err);
                return false;
            }
        }

        function validateBtnUpload() {
            try {

                var iserr = false;
                var fu = document.getElementById("FileUpload1");
                var val = $("#FileUpload1").val().toLowerCase();
                //var regex = new RegExp("(.*?)\.(pdf)$");
                var regex = new RegExp("\.(pdf)$");

                var err = "";
                if (fu.value.length <= 0) {
                    $("#FileUpload1").css("border", "1px solid #ff0000");
                    err = "Please select the file !";
                    iserr = true;
                }
                else {
                    if (!(regex.test(val))) {
                        $("#FileUpload1").css("border", "1px solid #ff0000");
                        err = "Invalid File. Please upload a File with (.pdf)extension";
                        $("#FileUpload1").val("");
                        iserr = true;
                    }
                    else {
                        var fileSize = fu.files[0].size;
                        if (fileSize <= 3145728) {
                            var format = /[!@#$%^&*()+\=\[\]{};':"\\|,.<>\/?]/;
                            var objRE = new RegExp(/([^\/\\]+)$/);
                            var FlName = objRE.exec(document.getElementById('FileUpload1').value);
                            var NewFlName = FlName[0].replace('.pdf', '');
                            if (format.test(NewFlName)) {
                                $("#FileUpload1").css("border", "1px solid #ff0000");
                                err = "Invalid File Name. \n File name cannot contain below character : \n [ ! @ # $ % ^ & * ( ) + \ = \ [ \ ] { } ; ' : \" \\ | , . < > \ / ? ] ";
                                $("#FileUpload1").val("");
                                iserr = true;
                            }
                        }
                        else {
                            var MB = fileSize / 1048576;
                            $("#FileUpload1").css("border", "1px solid #ff0000");
                            err = "File is too large, Maximum file size 3 Mb. File  Size: " + MB.toFixed(1) + " Mb";
                            $("#FileUpload1").val("");
                            iserr = true;
                        }
                    }
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

        function validatebtnViewDownPDF() {
            try {
                var iserr = false;
                var err = "";
                var fu = document.getElementById("FileUpload1");
                var lb = $("#lblMessage").text();
                var val = $("#FileUpload1").val().toLowerCase();
                var regex = new RegExp("(.*?)\.(pdf)$");
                if (fu.value.length <= 0 && lb == "") {
                    $("#FileUpload1").css("border", "1px solid #ff0000");
                    err = "Please select the file !";
                    iserr = true;
                }
                else if (!(regex.test(val)) && fu.value.length > 0) {
                    $("#FileUpload1").css("border", "1px solid #ff0000");
                    err = "Invalid File. Please upload a File with (.pdf)extension";
                    $("#FileUpload1").val("");
                    iserr = true;
                }
                else if (!(regex.test(val)) && lb == "") {
                    $("#FileUpload1").css("border", "1px solid #ff0000");
                    err = "Invalid File. Please upload a File with (.pdf)extension";
                    $("#FileUpload1").val("");
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

        function multiselectDropDown() {
            try {
                $(function () {
                    $('[id*=LbSPlantRequestor]').multiselect({
                        includeSelectAllOption: true
                    });
                }); (jQuery);
            }
            catch (err) {
                alert(err + ": multiselectDropDown");
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
        });
    </script>

    <script type="text/javascript">

        function preventInput(evnt) {

            if (evnt.which != 9) evnt.preventDefault();

        }

        function checkAll(objRef) {

            var GridView = document.getElementById("<%=grdvendor.ClientID %>");

            var inputList = GridView.getElementsByTagName("chkheader");

            //   alert(inputList);

            for (var i = 0; i < inputList.length; i++) {

                //Get the Cell To find out ColumnIndex

                var row = inputList[i].parentNode.parentNode;

                //  alert(row);

                //  alert("row");

                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                    if (objRef.checked) {

                        //If the header checkbox is checked

                        //check all checkboxes

                        //and highlight all rows

                        row.style.backgroundColor = "aqua";

                        inputList[i].checked = true;

                    }

                    else {

                        //If the header checkbox is checked

                        //uncheck all checkboxes

                        //and change rowcolor back to original

                        if (row.rowIndex % 2 == 0) {

                            //Alternating Row Color

                            row.style.backgroundColor = "#C2D69B";

                        }

                        else {

                            row.style.backgroundColor = "white";

                        }

                        inputList[i].checked = false;

                    }

                }

            }

        }

        function Check_Click(objRef) {

            // alert("welcome");

            //Get the Row based on checkbox

            var row = objRef.parentNode.parentNode;

            if (objRef.checked) {

                //If checked change color to Aqua

                row.style.backgroundColor = "aqua";

            }

            else {

                //If not checked change back to original color

                if (row.rowIndex % 2 == 0) {

                    //Alternating Row Color

                    row.style.backgroundColor = "#C2D69B";

                }

                else {

                    row.style.backgroundColor = "white";

                }

            }



            //Get the reference of GridView

            var GridView = row.parentNode;



            //Get all input elements in Gridview

            var inputList = GridView.getElementsByTagName("chkchild");



            for (var i = 0; i < inputList.length; i++) {

                //The First element is the Header Checkbox

                var headerCheckBox = inputList[0];



                //Based on all or none checkboxes

                //are checked check/uncheck Header Checkbox

                var checked = true;

                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {

                    if (!inputList[i].checked) {

                        checked = false;

                        break;

                    }

                }

            }

            headerCheckBox.checked = checked;

        }

        function SelectAll(id) {

            var grid = document.getElementById("<%= grdvendor.ClientID %>");
            //  alert(grid);
            var cell;

            // alert("muthu");

            if (grid.rows.length > 0) {

                for (i = 1; i < grdvendor.rows.length; i++) {



                    cell = grid.rows[i].cells[2];

                    alert(cell);


                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {

                            cell.childNodes[j].checked = id.checked;
                        }
                    }
                }
            }
        }


        function checkAll(ele) {
            var checkboxes = document.getElementsByTagName('input');
            if (ele.checked) {
                for (var i = 0; i < checkboxes.length; i++) {
                    if (checkboxes[i].type == 'checkbox') {
                        checkboxes[i].checked = true;
                    }
                }
            } else {
                for (var i = 0; i < checkboxes.length; i++) {
                    console.log(i)
                    if (checkboxes[i].type == 'checkbox') {
                        checkboxes[i].checked = false;
                    }
                }
            }

        }

        function ShowDialog() {
            $("#dialog1").dialog();
        }

        function MarkCheckBox(cntrl) {


            var gvWorkSpace = document.getElementById('<%=grdvendor.ClientID %>');

            // alert("first");
            var gvRow = gvWorkSpace.getElementsByTagName('tr');
            for (var i = 1; i < gvRow.length; i++) {
                var gvCntrl = gvRow[i].getElementsByTagName('chkheader');
                for (var j = 0; j < gvCntrl.length; j++) {
                    if (gvCntrl[j].type == "checkbox") {

                        //  alert("Hi");

                        if (CmdtType == 'Delete') {
                            if (gvCntrl[j].disabled != true) {
                                gvCntrl[j].checked = cntrl.checked;
                                break;
                            }
                        }

                        else if (CmdtType == 'UpdateDate') {
                            gvCntrl[j + 1].checked = cntrl.checked;
                            break;
                        }
                    }
                }
            }
        }

        function CheckFile(Cntrl) {
            var file = document.getElementById(Cntrl.name);
            var len = file.value.length;
            var ext = file.value;
            if (ext.substr(len - 3, len) != "pdf") {
                alert("Please select a doc or pdf file ");
                return false;
            }
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function SelectAll(CheckBox) {
            TotalChkBx = parseInt('<%= this.grdvendor.Rows.Count %>');
            var TargetBaseControl = document.getElementById('<%= this.grdvendor.ClientID %>');
            var TargetChildControl = "chkSelect";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var iCount = 0; iCount < Inputs.length; ++iCount) {
                if (Inputs[iCount].type == 'checkbox' && Inputs[iCount].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[iCount].checked = CheckBox.checked;
            }
        }

        function SelectDeSelectHeader(CheckBox) {
            TotalChkBx = parseInt('<%= this.grdvendor.Rows.Count %>');
            var TargetBaseControl = document.getElementById('<%= this.grdvendor.ClientID %>');
            var TargetChildControl = "chkSelect";
            var TargetHeaderControl = "chkSelectAll";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var flag = false;
            var HeaderCheckBox;
            for (var iCount = 0; iCount < Inputs.length; ++iCount) {
                if (Inputs[iCount].type == 'checkbox' && Inputs[iCount].id.indexOf(TargetHeaderControl, 0) >= 0)
                    HeaderCheckBox = Inputs[iCount];
                if (Inputs[iCount] != CheckBox && Inputs[iCount].type == 'checkbox' && Inputs[iCount].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[iCount].id.indexOf(TargetHeaderControl, 0) == -1) {
                    if (CheckBox.checked) {
                        if (!Inputs[iCount].checked) {
                            flag = false;
                            HeaderCheckBox.checked = false;
                            return;
                        }
                        else
                            flag = true;
                    }
                    else if (!CheckBox.checked)
                        HeaderCheckBox.checked = false;
                }
            }
            if (flag) {
                HeaderCheckBox.checked = CheckBox.checked;
            }
        }

        function CompareDueDateandDefaultDueDate() {
            try {
                var StrEffDate = "";
                var strDueOn = "";
                if (document.getElementById("TxtEffectiveDate") != null && document.getElementById("TxtDuenextRev") != null) {
                    StrEffDate = document.getElementById("TxtEffectiveDate").value.toString().replace(/\-/g, '.');
                    strDueOn = document.getElementById("TxtDuenextRev").value.toString().replace(/\-/g, '.');
                }

                if (StrEffDate != "" && strDueOn != "") {
                    var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
                    var dtEffDate = new Date(StrEffDate.replace(pattern, '$3-$2-$1'));
                    var dtDueOn = new Date(strDueOn.replace(pattern, '$3-$2-$1'));

                    if (dtEffDate > dtDueOn) {
                        alert("Due Dt Next Rev cannot be small than Effective date");
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
                            <div class="col-md-12 card" style="padding: 10px;">
                                <div class="col-md-12 Padding-Nol">
                                    <div class="row" style="padding-top: 5px; padding-bottom: 5px;">
                                        <div class="col-md-6">
                                            <asp:Label ID="lbTitle" runat="server" Text="Create Request" Font-Bold="true" Font-Size="Large" />
                                        </div>
                                        <div class="col-md-6 text-right">
                                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-sm btn-warning" OnClick="btnReset_Click" />
                                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger" PostBackUrl="Home.aspx" />
                                            <asp:Label ID="lbluser" runat="server" Text="Label" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12" style="padding-bottom: 5px;">
                                            <div class="col-md-12" style="border-bottom: 2px solid #006EB7"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12" style="background-color: white;">
                                    <asp:UpdatePanel runat="server" ID="UpRegularMenu">
                                        <ContentTemplate>
                                            <div class="col-md-4" style="padding-top: 5px; padding-bottom: 3px;">
                                                <asp:RadioButton ID="article" runat="server" Text="&nbsp;<b> With SAP Code </b>" onclick="document.location.href='NewRequest.aspx?num=1'"
                                                    GroupName="RegularMenu" TextAlign="Right" AutoPostBack="true"
                                                    OnCheckedChanged="article_CheckedChanged" />
                                            </div>
                                            <div class="col-md-4" style="padding-top: 5px; padding-bottom: 3px;">
                                                <asp:RadioButton ID="RbWithouSAPCode" runat="server" Text="&nbsp; <b> Without SAP Code </b>" onclick="document.location.href='NewRequest.aspx?num=2'"
                                                    GroupName="RegularMenu" TextAlign="Right" AutoPostBack="true"
                                                    OnCheckedChanged="RbWithouSAPCode_CheckedChanged" />
                                            </div>
                                            <div class="col-md-4" style="padding-top: 5px; padding-bottom: 3px;">
                                                <asp:RadioButton ID="RbWithouSAPGp" runat="server" Text="&nbsp; <b> Without SAP Code (GP)</b>"
                                                    GroupName="RegularMenu" TextAlign="Right" AutoPostBack="true"
                                                    OnCheckedChanged="RbWithouSAPCode_CheckedChanged" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row" style="padding-bottom: 10px;">
                        <div class="col-md-12">
                            <div class="col-md-12 card" style="padding: 10px; background-color: white;">
                                <div class="col-md-12 card-body Padding-Nol">
                                    <%--entrydata--%>
                                    <div class="col-md-12" style="background-color: white;">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px; padding-top: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="lbl_date" runat="server" Text="Date" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="txtReqDate" runat="server" AutoCompleteType="Disabled" autocomplete="off" Width="100%"
                                                                    onkeydown="javascript:preventInput(event);"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label12" runat="server" Text="Plant" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="txtplant" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label9" runat="server" Text="GP Request Plant" Enabled="false"
                                                                    ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <%--<div style="display:none;"><asp:DropDownList ID="DdlPlantRequestor" runat="server" AutoPostBack="true"></asp:DropDownList></div>--%>

                                                                <div class="col-md-12 Padding-Nol" id="DvPlantRequestor">
                                                                    <asp:ListBox ID="LbSPlantRequestor" runat="server" SelectionMode="Multiple" onchange="ResetFiledColor('DvPlantRequestor');EmptyRequestList();"></asp:ListBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label14" runat="server" Text="Plating Type" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="txtplatingtype" runat="server"
                                                                    MaxLength="100" ToolTip="max 100 character" AutoCompleteType="Disabled" autocomplete="off"
                                                                    Width="100%" onkeydown="ResetFiledColor('txtplatingtype');return (event.keyCode!=13);"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="LbPartCode" runat="server" Text="Part Code" Enabled="false"
                                                                    ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="txtpartdesc" runat="server" onchange="EmptyRequestList();"
                                                                    MaxLength="50" ToolTip="max 50 character" AutoCompleteType="Disabled" autocomplete="off" onkeyup="CharlgtCode('txtpartdesc');"
                                                                    TextMode="MultiLine" Width="100%" onkeydown="ResetFiledColor('txtpartdesc');return (event.keyCode!=13);"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label6" runat="server" Text="Part Code Description" Enabled="false" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="txtpartdescription" Width="100%" TextMode="MultiLine" onkeyup="CharlgtDesc('txtpartdescription');"
                                                                    onkeydown="ResetFiledColor('txtpartdescription');return (event.keyCode!=13);" onchange="EmptyRequestList();"
                                                                    MaxLength="100" ToolTip="max 100 character" AutoCompleteType="Disabled" autocomplete="off"
                                                                    runat="server" ForeColor="Black"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label19" runat="server" Text="Mnth.Est.Qty & Base UOM:" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox ID="txtMQty" runat="server" Width="100%" oninput="validateNumber('txtMQty')"
                                                                            onkeydown="ResetFiledColor('txtMQty');return (event.keyCode!=13);"
                                                                            AutoCompleteType="Disabled" autocomplete="off" onchange="EmptyRequestList();ValOnlyNo('txtMQty','txt');"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox ID="txtBaseUOM1" runat="server" Width="100%" MaxLength="10" ToolTip="max 10 character" onchange="EmptyRequestList();"
                                                                            AutoCompleteType="Disabled" autocomplete="off" onkeydown="ResetFiledColor('txtBaseUOM1');return (event.keyCode!=13);"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label11" runat="server" Text="Net Weight & Base UOM:" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:UpdatePanel ID="UpdatePanel111" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row">
                                                                            <div class="col-md-6">
                                                                                <asp:TextBox ID="txtunitweight" runat="server" Width="100%" oninput="validateNumber('txtunitweight')"
                                                                                    onchange="EmptyRequestList();ValOnlyNo('txtunitweight','txt');"
                                                                                    onkeydown="return (event.keyCode!=13);" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:TextBox ID="txtUOM" runat="server" Width="100%" onchange="EmptyRequestList();"
                                                                                    MaxLength="50" ToolTip="max 50 character" AutoCompleteType="Disabled" autocomplete="off"
                                                                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label1" runat="server" Text="FA Date & Qty" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <div class="group-main">
                                                                            <div class="SearchBox-txt">
                                                                                <asp:TextBox ID="TxtFADate" OnclientClick="return false;" onchange="ResetFiledColor('TxtFADate');EmptyRequestList();"
                                                                                    onkeydown="javascript:preventInput(event);" CssClass="form_datetime"
                                                                                    autocomplete="off" AutoCompleteType="Disabled"
                                                                                    runat="server" ForeColor="Black">
                                                                                </asp:TextBox>
                                                                            </div>
                                                                            <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 2px 5px 0px 3px">
                                                                                <i class="fa fa-calendar" style="color: #005496;" onclick="javascript: $('#TxtFADate').focus();"></i>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox ID="TxtFAQty" runat="server" Width="100%" AutoCompleteType="Disabled" onkeydown="ResetFiledColor('TxtFAQty');return (event.keyCode!=13);"
                                                                            oninput="validateNumber('TxtFAQty')" autocomplete="off" onchange="EmptyRequestList();ValOnlyNo('TxtFAQty','txt');"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label3" runat="server" Text="1<sup>st</sup> Delivery Date & Qty" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row">
                                                                            <div class="col-md-6">
                                                                                <div class="group-main">
                                                                                    <div class="SearchBox-txt">
                                                                                        <asp:TextBox ID="TxtDelDate" OnclientClick="return false;" onchange="ResetFiledColor('TxtDelDate');EmptyRequestList();"
                                                                                            onkeydown="javascript:preventInput(event);" CssClass="form_datetime"
                                                                                            autocomplete="off" AutoCompleteType="Disabled"
                                                                                            runat="server" ForeColor="Black">
                                                                                        </asp:TextBox>
                                                                                    </div>
                                                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 2px 5px 0px 3px">
                                                                                        <i class="fa fa-calendar" style="color: #005496;" onclick="javascript: $('#TxtDelDate').focus();"></i>
                                                                                    </span>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:TextBox ID="TxtDelQty" runat="server" Width="100%" AutoCompleteType="Disabled" onkeydown="ResetFiledColor('TxtDelQty');return (event.keyCode!=13);"
                                                                                    oninput="validateNumber('TxtDelQty')" autocomplete="off" onchange="EmptyRequestList();ValOnlyNo('TxtDelQty','txt');"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
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
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <asp:DropDownList ID="DdlReason" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DdlReason_SelectedIndexChanged" onchange="EmptyRequestList();"></asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-12" style="padding-top: 5px;">
                                                                        <asp:TextBox ID="txtRem" runat="server" placeholder="Maximum 200 character" ToolTip="Maximum 200 character" onchange="EmptyRequestList();"
                                                                            TextMode="MultiLine" Width="100%" Visible="false"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label5" runat="server" Text="Incoterms" Enabled="false"
                                                                    ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="DdlIncoterms" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DdlReason_SelectedIndexChanged" onchange="EmptyRequestList();"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label7" runat="server" Text="Packing Requirements" Enabled="false"
                                                                    ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="TxtPckRequirement" runat="server" onchange="EmptyRequestList();"
                                                                    MaxLength="100" ToolTip="max 100 character" AutoCompleteType="Disabled" autocomplete="off"
                                                                    onkeydown="ResetFiledColor('TxtPckRequirement');return (event.keyCode!=13);"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label4" runat="server" Text="Others Requirements" Enabled="false" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="TxtOthRequirement" runat="server" onchange="EmptyRequestList();"
                                                                    MaxLength="100" ToolTip="max 100 character" AutoCompleteType="Disabled" autocomplete="off"
                                                                    onkeydown="ResetFiledColor('TxtOthRequirement');return (event.keyCode!=13);"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <!-- Upload control-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-md-6" style="border: double; height: 50px; padding: 5px;">
                                                            <asp:Label ID="lbldrawng" runat="server" Text="Drawing No:" ForeColor="Black"></asp:Label>
                                                            <asp:UpdatePanel ID="UpdatePanel171" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblMessage" runat="server" Enabled="False" ForeColor="Black" Text="lblMessage" Font-Bold="true"></asp:Label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-md-6" style="border: double; height: 50px; padding: 10px 5px 5px 5px;">
                                                            <asp:FileUpload ID="FileUpload1" ToolTip="" runat="server" onchange="ResetFiledColor('FileUpload1');"
                                                                EnableViewState="true" CssClass="form-control-sm" Font-Size="14px" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-md-6" style="border: double; vertical-align: central; height: 50px; padding: 5px;">
                                                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClientClick="if(validateBtnUpload()==false) return false;"
                                                                OnClick="btnUpload_Click" CssClass="btn btn-sm btn-primary" Width="100%" Font-Size="14px" />
                                                        </div>
                                                        <div class="col-md-6" style="border: double; height: 50px; padding: 5px; vertical-align: central;">
                                                            <asp:LinkButton ID="btnViewDownPDF" OnClick="btnViewDownPDF_Click" runat="server" OnClientClick="if(validatebtnViewDownPDF()==false) return false;"
                                                                Text=" Download & View" CssClass="btn btn-link" ForeColor="Blue"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end upload control-->

                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <br />
                                                                <asp:Label ID="lbl_cntact2" runat="server" ForeColor="Black" Text="Process Group"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <br />
                                                                <asp:DropDownList ID="ddlprocess" runat="server" AutoPostBack="true" Width="100%" onchange="EmptyRequestList();"
                                                                    ForeColor="Black" OnSelectedIndexChanged="ddlprocess_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <br />
                                                            <div class="col-lg-5">
                                                                <asp:Label ID="Label22" runat="server" ForeColor="Black" Text="Quotation response due date"></asp:Label>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <div class="group-main">
                                                                    <div class="SearchBox-txt">
                                                                        <asp:TextBox ID="txtDate" OnclientClick="return false;" onchange="ResetFiledColor('txtDate');EmptyRequestList();"
                                                                            onkeydown="javascript:preventInput(event);" CssClass="form_datetime"
                                                                            autocomplete="off" AutoCompleteType="Disabled"
                                                                            runat="server" ForeColor="Black">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 2px 5px 0px 3px">
                                                                        <i class="fa fa-calendar" style="color: #005496;" onclick="javascript: $('#txtDate').focus();"></i>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6" runat="server" id="DvEffectiveDate">
                                                        <div class="row">
                                                            <div class="col-lg-5">
                                                                <asp:Label ID="Label10" runat="server" ForeColor="Black" Text="Effective Date"></asp:Label>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <div class="group-main">
                                                                    <div class="SearchBox-txt">
                                                                        <asp:TextBox ID="TxtEffectiveDate" OnclientClick="return false;" onchange="ResetFiledColor('TxtEffectiveDate');EmptyRequestList();" Text=""
                                                                            onkeydown="javascript:preventInput(event);" CssClass="form_datetime" OnTextChanged="TxtEffectiveDate_TextChanged"
                                                                            autocomplete="off" AutoCompleteType="Disabled" AutoPostBack="true"
                                                                            runat="server" ForeColor="Black">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 2px 5px 0px 3px;">
                                                                        <i class="fa fa-calendar" style="color: #005496;" onclick="javascript: $('#TxtEffectiveDate').focus();"></i>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6" runat="server" id="DvDueDateNextRev">
                                                        <div class="row">
                                                            <div class="col-lg-5">
                                                                <asp:Label ID="Label28" runat="server" ForeColor="Black" Text="Due Dt Next Rev"></asp:Label>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <div class="group-main">
                                                                    <div class="SearchBox-txt">
                                                                        <asp:TextBox ID="TxtDuenextRev" OnclientClick="return false;" onchange="ResetFiledColor('TxtDuenextRev');"
                                                                            onkeydown="javascript:preventInput(event);" CssClass="form_datetime"
                                                                            autocomplete="off" AutoCompleteType="Disabled"
                                                                            runat="server" ForeColor="Black">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 2px 5px 0px 3px;">
                                                                        <i class="fa fa-calendar" style="color: #005496;" onclick="IsDatePitckerEnabled('TxtDuenextRev');"></i>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="UpVendorType" runat="server">
                                            <ContentTemplate>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label8" runat="server" Text="Vendor Type" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <div class="row">
                                                                    <div class="col-sm-5">
                                                                        <asp:RadioButton ID="RbExternal" runat="server" Text="&nbsp; External"
                                                                            GroupName="RbVendorType" TextAlign="Right" AutoPostBack="true" Checked="true"
                                                                            OnCheckedChanged="RbExternal_CheckedChanged" />
                                                                    </div>
                                                                    <div class="col-sm-7">
                                                                        <asp:RadioButton ID="RbTeamShimano" runat="server" Text="&nbsp; SBM"
                                                                            GroupName="RbVendorType" TextAlign="Right" AutoPostBack="true"
                                                                            OnCheckedChanged="RbTeamShimano_CheckedChanged" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="row" style="display:none">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label30" runat="server" ForeColor="Black" Text="Use Tool Amortize"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:UpdatePanel ID="UpToolAmortize" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:DropDownList ID="DdlToolAmortize" runat="server" AutoPostBack="true" Width="100%" onchange="EmptyRequestList();"
                                                                            ForeColor="Black" OnSelectedIndexChanged="DdlToolAmortize_SelectedIndexChanged">
                                                                            <asp:ListItem Value="0"> -- Select Use Tool Amortize Condition -- </asp:ListItem>
                                                                            <asp:ListItem Value="YES"> YES </asp:ListItem>
                                                                            <asp:ListItem Value="NO" Selected="True"> NO </asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="ddlprocess" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" runat="server" id="DvImRcylRatio" visible="false" style="padding-bottom:10px;">
                                                    <div class="col-md-6" >
                                                        <div class="row">
                                                            <div class="col-lg-5">
                                                                <asp:Label ID="Label27" runat="server" ForeColor="Black" Text="IM Recycle Ratio (%)"></asp:Label>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <asp:DropDownList ID="DdlImRcylRatio" onchange="ResetFiledColor('DdlImRcylRatio');" runat="server" AutoPostBack="false" Width="100%" ForeColor="Black">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;" id="DvVndToolAmortize" runat="server" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <asp:Label ID="Label31" runat="server" Text="Select Vendors" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="table table-responsive table-sm">
                                                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="GvVndToolAmortize" runat="server" AutoGenerateColumns="False" CellPadding="4" Width="100%"
                                                                                GridLines="both" ForeColor="#333333" CssClass="table-nowrap" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" BackColor="White">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderStyle-BackColor="#FBDCA3" HeaderText="SELECT TO ACCESS">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="LbCheck" runat="server" AutoPostBack="false"  Text="SE" ForeColor="Transparent"/>
                                                                                            <asp:CheckBox ID="chkSelectAllVndToolAmor" runat="server" AutoPostBack="false" Visible="false" OnCheckedChanged="chkSelectAllVndToolAmor_CheckedChanged"  />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkVndToolAmor" runat="server" onclick="ShowLoading();" AutoPostBack="true" Text='<%# Eval("VendorCode") %>' ForeColor="Transparent" Width="0px"  OnCheckedChanged="chkVndToolAmor_CheckedChanged" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="20px" />
                                                                                    </asp:TemplateField>

                                                                                    <asp:BoundField DataField="VendorCode" HeaderText="Vendor ID" />
                                                                                    <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                                                                                    <asp:BoundField DataField="SearchTerm" HeaderText="Search Term" />

                                                                                    <asp:BoundField DataField="ToolTypeID" HeaderText="Tool Type" />
                                                                                    <asp:BoundField DataField="Amortize_Tool_ID" HeaderText="Tool Amortize ID" />
                                                                                    <asp:BoundField DataField="Amortize_Tool_Desc" HeaderText="Tool Amortize Desc" />
                                                                                    <asp:BoundField DataField="AmortizeCost" HeaderText="Amortize Cost" />
                                                                                    <asp:BoundField DataField="AmortizeCurrency" HeaderText="Amortize Currency" />
                                                                                    <asp:BoundField DataField="ExchangeRate" HeaderText="Exch Rate" />
                                                                                    <asp:BoundField DataField="AmortizePeriod" HeaderText="Period" />
                                                                                    <asp:BoundField DataField="AmortizePeriodUOM" HeaderText="Period UOM" />
                                                                                    <asp:BoundField DataField="TotalAmortizeQty" HeaderText="Tot Amortize Qty" />
                                                                                    <asp:BoundField DataField="QtyUOM" HeaderText="Qty UOM" />
                                                                                    <asp:BoundField DataField="AmortizeCost_Vend_Curr" HeaderText="Amortize Cost Vnd Curr" />
                                                                                    <asp:BoundField DataField="AmortizeCost_Pc_Vend_Curr" HeaderText="Amortize Cost Pc Vnd Curr" />
                                                                                    <asp:BoundField DataField="EeffDt" HeaderText="Effective From" />
                                                                                    <asp:BoundField DataField="DuDate" HeaderText="Due On" />
                                                                                </Columns>
                                                                                <EditRowStyle BackColor="#999999" />
                                                                                <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                                                                <HeaderStyle BackColor="#e6f2ff" ForeColor="Black" />
                                                                                <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#1a2e4c" />
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
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;" runat="server" id="DvTeamShmnVendor" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="border: double black; padding: 5px 0px 5px 0px;">
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="col-md-5">
                                                                        <asp:Label ID="LbSelectPlant" runat="server" Text="Select Vendor" ForeColor="Black"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-7">
                                                                        <asp:DropDownList runat="server" ID="DdlVendor" OnSelectedIndexChanged="DdlVendor_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="col-md-5">
                                                                        <asp:Label ID="Label23" runat="server" Text="Plant" ForeColor="Black"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-7">
                                                                        <div class="row">
                                                                            <div class="col-md-6">
                                                                                <asp:TextBox runat="server" ID="TxtPlantVendor" Enabled="false"></asp:TextBox>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:TextBox runat="server" ID="TxtPlantVendorDesc" Enabled="false"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-4" style="display: none;">
                                                                    <asp:TextBox runat="server" ID="TxtVendorDesc" Enabled="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="TxtSrcTerm" Enabled="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;" runat="server" id="DvExternal" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="border: double black; padding: 5px 10px 5px 10px;">
                                                            <div class="row">
                                                                <div class="col-md-2">
                                                                    <asp:Label ID="lbl_vendrName" runat="server" Text="Select Vendors" ForeColor="Black"></asp:Label>
                                                                </div>
                                                                <div class="col-md-10">
                                                                    <div class="table table-responsive table-sm">
                                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:GridView ID="grdvendor" runat="server" AutoGenerateColumns="False" CellPadding="4" Width="100%"
                                                                                    GridLines="both" ForeColor="#333333" CssClass="table-nowrap">
                                                                                    <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderStyle-BackColor="#FBDCA3"
                                                                                            HeaderText="SELECT TO ACCESS">
                                                                                            <HeaderTemplate>
                                                                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chk" runat="server" EnableViewState="true" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>


                                                                                        <asp:BoundField DataField="VendorCode" HeaderText="Vendor ID" />
                                                                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                                                                                        <asp:BoundField DataField="SearchTerm" HeaderText="Search Term" />

                                                                                    </Columns>
                                                                                    <EditRowStyle BackColor="#999999" />
                                                                                    <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                                                                    <HeaderStyle BackColor="#e6f2ff" ForeColor="Black" />
                                                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#1a2e4c" />
                                                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                                                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                                                </asp:GridView>


                                                                                <asp:GridView ID="grdvendor1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                                    GridLines="both" ForeColor="#333333" Visible="False">
                                                                                    <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="Vendor Code" HeaderText="Vendor ID" />
                                                                                        <asp:BoundField DataField="Vendor Name" HeaderText="Vendor Name" />
                                                                                        <asp:BoundField DataField="SearchTerm" HeaderText="Search Term" />

                                                                                    </Columns>
                                                                                    <EditRowStyle BackColor="#999999" />
                                                                                    <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                                                                    <HeaderStyle BackColor="#e6f2ff" ForeColor="Black" />
                                                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#1a2e4c" />
                                                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                                                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                                                </asp:GridView>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="grdvendor" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px" runat="server" id="DvCreateRequest" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label2" runat="server" Text="Quote Request can be Created only for below Vendors"
                                                                    Visible="false" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="row">
                                                                    <div class="col-md-5">
                                                                    </div>
                                                                    <div class="col-md-7">
                                                                        <asp:Button ID="btnsave" runat="server" CssClass="Login-button"
                                                                            OnClientClick="if(validateCreateReq()==false) return false;if(CompareDueDateandDefaultDueDate()==false) return false;ShowLoading();" Text="Create Request"
                                                                            Width="100%" OnClick="btnsave_Click" />
                                                                        <asp:Button ID="btnsubmit" runat="server" CssClass="Login-button" PostBackUrl="vendor_Quotation.aspx"
                                                                            Text="Submit" Visible="false" Width="100px" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <div class="row" style="padding-bottom: 10px">
                                            <div class="col-md-12">
                                                <div class="table table-responsive table-sm table-nowrap">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Table ID="Table1" runat="server" CssClass="table-sm table-bordered">
                                                            </asp:Table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Table1" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" style="padding-bottom: 10px">
                                            <div class="col-md-12">
                                                <asp:HiddenField ID="hdnReqNo" runat="server" />
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="row">
                                                                    <div class="col-md-5">
                                                                    </div>
                                                                    <div class="col-md-7">
                                                                        <asp:Button ID="Button1" runat="server" CssClass="Login-button" OnClientClick="ShowLoading();"
                                                                            Text="Submit" Visible="false" Width="100%" OnClick="Button1_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:Label ID="Label15" runat="server" Visible="False"></asp:Label>
                                                <asp:Label ID="Label16" runat="server" Visible="False"></asp:Label>
                                                <asp:Label ID="Label17" runat="server" Visible="False"></asp:Label>
                                            </div>
                                        </div>

                                        <div id="dialog1" title="Full Image View" style="display: none">
                                            <asp:Image ID="img1" runat="server" />
                                        </div>
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
