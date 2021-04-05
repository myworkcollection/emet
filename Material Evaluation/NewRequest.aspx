<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewRequest.aspx.cs" Inherits="Material_Evaluation.NewRequest" EnableEventValidation="false" %>

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
    <link href="js/select2-4.0.13/dist/css/select2.min.css" rel="stylesheet" />
    <style type="text/css">
        .SideBarMenu {
            width: 300px;
        }

        .WrapCnt td, th {
            white-space: normal !important;
            /*word-wrap: break-word;*/
            font-size: 14px !important;
        }

        .invalid-Form {
            border-color: #dc3545 !important;
            /*padding-right: calc(1.3em + 0.75rem);
          background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' fill='%23dc3545' viewBox='-2 -2 7 7'%3e%3cpath stroke='%23dc3545' d='M0 0l3 3m0-3L0 3'/%3e%3ccircle r='.5'/%3e%3ccircle cx='3' r='.5'/%3e%3ccircle cy='3' r='.5'/%3e%3ccircle cx='3' cy='3' r='.5'/%3e%3c/svg%3E");
          background-repeat: no-repeat;
          background-position: center right calc(0.375em + 0.1875rem);
          background-size: calc(0.75em + 0.375rem) calc(0.75em + 0.375rem);*/
        }

    </style>

    

    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>

    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>

    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/locales/bootstrap-datetimepicker.fr.js"></script>
    <script type="text/javascript" src="js/select2-4.0.13/dist/js/select2.min.js"></script>

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

    <script type="text/javascript" language="javascript">
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
            if (flag)
                HeaderCheckBox.checked = CheckBox.checked
        }

    </script>

    <script type="text/javascript">

        function initdropdownMatList() {
            var Myjson = JSON.parse($("#TxtDataJson").val());

            $('#DdlMatCodeList').select2({
                minimumInputLength: 2,
                placeholder: '-- Enter SAP Mat Code Or Desc--',
                delay: 600,
                data: Myjson,
                templateSelection: function (data, container) {
                    // Add custom attributes to the <option> tag for the selected option
                    $(data.element).attr('data-custom-attributes', data.PlantStatus);
                    return data.text;
                }
            }).on('select2:select', function (evt) {
                ShowLoading();
                var data = $("#DdlMatCodeList").select2('data')[0];
                var data_filter = Myjson.filter(element => element.id == data.id)

                var plantstts = data_filter[0].PlantStatus;
                if(plantstts != null){
                    if (plantstts.toUpperCase() == "Z9" || plantstts.toUpperCase() == "Z4") {
                        alert("Cannot Use Material with Plant Status : " + plantstts)
                        return false;
                    }
                    else {
                        $("#txtpartdesc").val(data.id);
                    }
                }
                else
                {
                    $("#txtpartdesc").val(data.id);
                }
                
            }).on('select2:clearing', function (evt) {
            });

            //var Myjson = [];
            //$("#DdlMatCodeList option").each(function () {
            //    Myjson.push({
            //        "id": $(this).val(),
            //        "text": $(this).text()
            //    });
            //    //json[$(this).text()] = $(this).val();
            //});

            //$('#TxtMatCodeList').select2({
            //    minimumInputLength: 2,
            //    placeholder: '-- Enter SAP Mat Code Or Desc--',
            //    delay: 600,
            //    data: Myjson
            //}).on('select2:select', function (evt) {
            //    ShowLoading();
            //    var data = $("#TxtMatCodeList").select2('data')[0];
            //    $("#txtpartdesc").val(data.id);
            //}).on('select2:clearing', function (evt) {
            //});
        }


        function initdropdownProduct() {
            $('#DdlProduct').select2({
                minimumInputLength: 0,
                placeholder: '-- Enter Product Or Desc--',
                delay: 1000
            }).on('select2:select', function (evt) {
                var data = $("#DdlProduct").select2('data')[0];
                ShowLoading();
            }).on('select2:clearing', function (evt) {
            });;
        }

    </script>


    <%--script loading page--%>
    <script language="javascript" type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });

        $(document).ready(function () {
            DatePitcker();
            setupUseToolAmorOrNot();
            initdropdownMatList();
            initdropdownProduct();
            ChangeEmptyFieldColor();
        });
    </script>

    <script type="text/javascript">
        function setupUseToolAmorOrNot() {
            if (document.getElementById("article").checked == true) {
                document.getElementById("DvIsuseToolAmor").style.display = "block";
            }
            else {
                document.getElementById("DvIsuseToolAmor").style.display = "none";
                document.getElementById("DdlToolAmortize").value = "NO";
            }
        }

        function ValOnlyNo(txtID, Source) {
            //page related : review_reqmass,newrewwsapgp,newrequest,newreqchangemass,newreq_changes,reciew_req
            try {
                var regex = /^[-+]?\d*\.?\d*$/;
                var fullval = document.getElementById(txtID).value;
                var val = parseFloat(document.getElementById(txtID).value);
                var IsValid = false;
                if (document.getElementById(txtID).value != "") {
                    if (!(regex.test(fullval))) {
                        if (Source == 'txt') {
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

        function ConfirmMsgSAPSpProcType() {
            try {
                var chck = document.getElementById("ChcDisMatCost");
                if (chck != null) {
                    if (chck.checked == true) {
                        document.getElementById("LbMessage").innerHTML = "Current SAP Sp Proc Type selected is 30, and Material Cost is <b><font color='red'> Disabled </b></font>. Do you want to Continue ???";
                    }
                    else {
                        document.getElementById("LbMessage").innerHTML = "Current SAP Sp Proc Type selected is 30, and Material Cost is <b><font color='#77b300'> Enabled </b></font>. Do you want to Continue ???";
                    }
                    OpenModalConfirm();
                    return false;
                }
                else {
                    ShowLoading();
                    $("#BtnSubmitReq").click();
                    return true;
                }
            } catch (err) {
                alert(err + " : ConfirmSAPSpProcType()");
            }
        }

        function validateSubmitReq() {
            try {
                var err = "";
                var iserr = false;

                if ($("#Table1") == null) {
                    err += "no request list yet, Please click button create request  \n";
                    iserr = true;
                }
                else {
                    var RowCount = $('#Table1 tr').length;
                    if (RowCount <= 1) {
                        err += "no request list yet, Please click button create request  \n";
                        iserr = true;
                    }
                }

                if (iserr == true) {
                    alert(err);
                    return false;
                }
            }
            catch (err) {
                alert(err + ": validateSubmitReq");
            }
        }

        function EmptyRequestList() {
            try {
                if ($("#Table1") != null && $("#Button1") != null) {
                    $("#Table1").hide();
                    $("#Button1").hide();
                    $("#Label2").hide();
                }
            }
            catch (err) {
                alert(err + ": EmptyRequestList");
            }
        }

        function ChangeEmptyFieldColor() {
            try {
                (function ($) {
                    var a = document.getElementById("article").checked;
                    if (a == true) {
                        if ($("#ddlmatltype")[0].selectedIndex == 0) {
                            $("#ddlmatltype").css("border", "1px solid #ff0000");
                        }

                        var plantstatusitm = $("#ddlplantstatus option").length;
                        if (a == true) {
                            if (plantstatusitm > 1) {
                                if ($("#ddlplantstatus")[0].selectedIndex < 0) {
                                    $("#ddlplantstatus").css("border", "1px solid #ff0000");
                                }
                            }
                            else if (plantstatusitm <= 0) {
                                $("#ddlplantstatus").css("border", "1px solid #ff0000");
                            }
                        }
                        else {
                            $('#ddlplantstatus').attr('disabled', true);
                            $("#ddlplantstatus").css("backgroundColor", "#EBEBE4");
                        }

                        if ($("#ddlproctype")[0].selectedIndex == 0) {
                            $("#ddlproctype").css("border", "1px solid #ff0000");
                        }
                        if ($("#ddlsplproctype")[0].selectedIndex <= 0) {
                            $("#ddlsplproctype").css("border", "1px solid #ff0000");
                        }
                        var pirtype = $("#ddlpirtype option").length;
                        if (pirtype > 1) {
                            if ($("#ddlpirtype")[0].selectedIndex == 0) {
                                $("#ddlpirtype").css("border", "1px solid #ff0000");
                            }
                        }
                        else if (pirtype <= 0) {
                            $("#ddlpirtype").css("border", "1px solid #ff0000");
                        }

                        if ($("#txtPIRDesc").val() == "") {
                            $("#txtPIRDesc").css("border", "1px solid #ff0000");
                        }

                        if ($("#TxtEffectiveDate").val() == "") {
                            $("#TxtEffectiveDate").css("border", "1px solid #ff0000");
                        }

                        if ($("#DdlMatCodeList").val() == "") {
                            $("body").find("[aria-labelledby='select2-DdlMatCodeList-container']").addClass("invalid-Form");
                        }

                        //if ($("#TxtMatCodeList").val() == "") {
                        //    $("body").find("[aria-labelledby='select2-TxtMatCodeList-container']").addClass("invalid-Form");
                        //}
                    }


                    if ($("#DdlProduct").val() == "") {
                        $("#DdlProduct").css("border", "1px solid #ff0000");
                        $("body").find("[aria-labelledby='select2-DdlProduct-container']").addClass("invalid-Form");
                    }
                    if ($("#txtpartdesc").val() == "") {
                        $("#txtpartdesc").css("border", "1px solid #ff0000");
                    }
                    else {
                        $("#txtpartdesc").css("border", "1px solid #CCCCCC");
                    }

                    if ($("#txtpartdescription").val() == "") {
                        $("#txtpartdescription").css("border", "1px solid #ff0000");
                    }
                    else {
                        $("#txtpartdescription").css("border", "1px solid #CCCCCC");
                    }

                    if ($("#txtunitweight").val() == "") {
                        $("#txtunitweight").css("border", "1px solid #ff0000");
                    }
                    else {
                        $("#txtunitweight").css("border", "1px solid #CCCCCC");
                    }

                    if ($("#txtUOM").val() == "") {
                        $("#txtUOM").css("border", "1px solid #ff0000");
                    }
                    else {
                        $("#txtUOM").css("border", "1px solid #CCCCCC");
                    }

                    if ($("#txtMQty").val() == "") {
                        $("#txtMQty").css("border", "1px solid #ff0000");
                    }
                    else {
                        $("#txtMQty").css("border", "1px solid #CCCCCC");
                    }

                    if ($("#txtBaseUOM1").val() == "") {
                        $("#txtBaseUOM1").css("border", "1px solid #ff0000");
                    }
                    else {
                        $("#txtBaseUOM1").css("border", "1px solid #CCCCCC");
                    }

                    if ($("#txtDate").val() == "") {
                        $("#txtDate").css("border", "1px solid #ff0000");
                    }

                    if ($("#DdlMatClass")[0].selectedIndex <= 0) {
                        $("#DdlMatClass").css("border", "1px solid #ff0000");
                    }

                    if ($("#DdlReason")[0].selectedIndex == 0) {
                        $("#DdlReason").css("border", "1px solid #ff0000");
                    }
                    if ($("#ddlprocess")[0].selectedIndex == 0) {
                        $("#ddlprocess").css("border", "1px solid #ff0000");
                    }

                    var e = document.getElementById("ddlprocess");
                    var value = e.options[e.selectedIndex].value;
                    var LayoutID = document.getElementById("HdnLayoutId").value;

                    if (LayoutID.toString().toUpperCase() == "LAYOUT1") {
                        var ddl = document.getElementById("DdlImRcylRatio");
                        var ddlvalue = ddl.options[ddl.selectedIndex].value;
                        if (ddlvalue == "SELECT") {
                            $("#DdlImRcylRatio").css("border", "1px solid #ff0000");
                        }
                        else if (ddlvalue == "NO DATA") {
                            $("#DdlImRcylRatio").css("border", "1px solid #ff0000");
                        }
                    }

                    var pirjtypeitem = $("#ddlpirjtype option").length;
                    if (pirjtypeitem > 1) {
                        if ($("#ddlpirjtype")[0].selectedIndex == 0) {
                            $("#ddlpirjtype").css("border", "1px solid #ff0000");
                        }
                    }
                    else if (pirjtypeitem <= 0) {
                        $("#ddlpirjtype").css("border", "1px solid #ff0000");
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
                })(jQuery);
            } catch (e) {
                alert(e);
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
                //if (fu.value.length <= 0 && lb == "") {
                //    $("#FileUpload1").css("border", "1px solid #ff0000");
                //    err = "Please select the file !";
                //    iserr = true;
                //}
                //else if (!(regex.test(val)) && fu.value.length > 0) {
                //    $("#FileUpload1").css("border", "1px solid #ff0000");
                //    err = "Invalid File. Please upload a File with (.pdf)extension";
                //    $("#FileUpload1").val("");
                //    iserr = true;
                //}
                if (fu.value.length > 0 && lb == "") {
                    $("#FileUpload1").css("border", "1px solid #ff0000");
                    err = "Please Upload the file !";
                    iserr = true;
                }
                else if (lb == "") {
                    $("#FileUpload1").css("border", "1px solid #ff0000");
                    err = "Please select the file !";
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

        function validateCreateReq() {
            //ResetBackColorField();
            var err = "";
            err += "Please check field listed in below : \n";
            var iserr = false;
            var a = document.getElementById("article").checked;
            if (a == true) {
                if ($("#ddlmatltype")[0].selectedIndex == 0) {
                    $("#ddlmatltype").css("border", "1px solid #ff0000");
                    err += "Please select Material Type !! \n";
                    iserr = true;
                }

                var plantstatusitm = $("#ddlplantstatus option").length;
                if (a == true) {
                    if (plantstatusitm > 1) {
                        if ($("#ddlplantstatus")[0].selectedIndex < 0) {
                            $("#ddlplantstatus").css("border", "1px solid #ff0000");
                            err += "Please select Plant Status !! \n";
                            iserr = true;
                        }
                    }
                    else if (plantstatusitm <= 0) {
                        $("#ddlplantstatus").css("border", "1px solid #ff0000");
                        err += "Plant Status not exist, please contack administrator ! \n";
                        iserr = true;
                    }
                    else if ($("#TxtEffectiveDate").val() == "") {
                        $("#TxtEffectiveDate").css("border", "1px solid #ff0000");
                        err += "Please select Effective Date! \n";
                        iserr = true;
                    }
                }
                var SAPProcType = $("#ddlproctype option:selected").text();
                if ($("#ddlproctype")[0].selectedIndex == 0) {
                    $("#ddlproctype").css("border", "1px solid #ff0000");
                    err += "Please select SAP Proc Type !! \n";
                    iserr = true;
                }
                else if (SAPProcType == "E") {
                    $("#ddlproctype").css("border", "1px solid #ff0000");
                    err += "Procurement Type E no need PIR and hence, so no need Quotation Request ! !! \n";
                    iserr = true;
                }

                if ($("#ddlsplproctype")[0].selectedIndex <= 0) {
                    $("#ddlsplproctype").css("border", "1px solid #ff0000");
                    err += "Please select SAP Sp Proc Type !! \n";
                    iserr = true;
                }

                var pirtype = $("#ddlpirtype option").length;
                if (pirtype > 1) {
                    if ($("#ddlpirtype")[0].selectedIndex < 0) {
                        $("#ddlpirtype").css("border", "1px solid #ff0000");
                        err += "Please select PIR Type \n";
                        iserr = true;
                    }
                }
                else if (pirtype <= 0) {
                    $("#ddlpirtype").css("border", "1px solid #ff0000");
                    err += "PIR Type not exist please contact Administrator \n";
                    iserr = true;
                }
                if ($("#txtPIRDesc").val() == "") {
                    $("#txtPIRDesc").css("border", "1px solid #ff0000");
                    err += "Please enter PIR Description! \n";
                    iserr = true;
                }

                var TxtEffectiveDate = $("#TxtEffectiveDate");
                if (TxtEffectiveDate != null) {
                    if ($("#TxtEffectiveDate").val() == "") {
                        $("#TxtEffectiveDate").css("border", "1px solid #ff0000");
                        err += "Please select Effective Date! \n";
                        iserr = true;
                    }
                }
            }


            if ($("#DdlProduct").val() == "") {
                $("#DdlProduct").css("border", "1px solid #ff0000");
                err += "Please enter Product! \n";
                iserr = true;
            }
            if ($("#DdlMatClass")[0].selectedIndex <= 0) {
                $("#DdlMatClass").css("border", "1px solid #ff0000");
                err += "Please select Material Class Desc. \n";
                iserr = true;
            }
            if ($("#txtpartdesc").val() == "") {
                $("#txtpartdesc").css("border", "1px solid #ff0000");
                err += "Please enter SAP Part Code! \n";
                iserr = true;
            }
            if ($("#txtpartdescription").val() == "") {
                $("#txtpartdescription").css("border", "1px solid #ff0000");
                err += "Please enter Part Code Description! \n";
                iserr = true;
            }

            if ($("#txtunitweight").val() == "") {
                $("#txtunitweight").css("border", "1px solid #ff0000");
                err += "Please enter Net unit weigtht \n";
                iserr = true;
            }
            if ($("#txtunitweight").val() != "") {
                if (ValOnlyNo('txtunitweight', 'btn') == false) {
                    err += "Net Weight Decimal No Only \n";
                    $("#txtunitweight").css("border", "1px solid #ff0000");
                    var iserr = true;
                }
            }
            if ($("#txtUOM").val() == "") {
                $("#txtUOM").css("border", "1px solid #ff0000");
                err += "please enter UOM fro net unit weight! \n";
                iserr = true;
            }
            if ($("#DdlReason")[0].selectedIndex == 0) {
                $("#DdlReason").css("border", "1px solid #ff0000");
                err += "Please Request Purpose! \n";
                iserr = true;
            }
            if ($("#txtMQty").val() == "") {
                $("#txtMQty").css("border", "1px solid #ff0000");
                err += "Please enter  Mnth.Est.Qty! \n";
                iserr = true;
            }
            if ($("#txtMQty").val() != "") {
                if (ValOnlyNo('txtMQty', 'btn') == false) {
                    err += "Mnth.Est.Qty Decimal No Only \n";
                    $("#txtMQty").css("border", "1px solid #ff0000");
                    var iserr = true;
                }
            }
            if ($("#txtBaseUOM1").val() == "") {
                $("#txtBaseUOM1").css("border", "1px solid #ff0000");
                err += "Please enter  Mnth.Est.Qty Base UOM! \n";
                iserr = true;
            }
            if ($("#lblMessage").text() == "") {
                $("#FileUpload1").css("border", "1px solid #ff0000");
                err += "Drawing Number should not be null! \n";
                iserr = true;
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
                iserr = true;
            }

            var e = document.getElementById("ddlprocess");
            var value = e.options[e.selectedIndex].value;
            var LayoutID = document.getElementById("HdnLayoutId").value;

            if (LayoutID == "LAYOUT1") {
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

            var pirjtypeitem = $("#ddlpirjtype option").length;
            if (pirjtypeitem > 1) {
                if ($("#ddlpirjtype")[0].selectedIndex == 0) {
                    $("#ddlpirjtype").css("border", "1px solid #ff0000");
                    err += "Please select PIR Job Type & Desc. ! \n";
                    iserr = true;
                }
            }
            else if (pirjtypeitem <= 0) {
                $("#ddlpirjtype").css("border", "1px solid #ff0000");
                err += "PIR Job Type not exist for this process group \n";
                iserr = true;
            }

            if ($("#DdlToolAmortize")[0].selectedIndex <= 0) {
                $("#DdlToolAmortize").css("border", "1px solid #ff0000");
                err += "Please select Use Tool Amortize Condition. ! \n";
                iserr = true;
            }

            if ($("#txtDate").val() == "") {
                $("#txtDate").css("border", "1px solid #ff0000");
                err += "Please select Quotation response due date! \n";
                iserr = true;
            }

            var SBM = document.getElementById("RbTeamShimano").checked;
            if (SBM == true && $("#DdlToolAmortize").val() == "NO") {
                var DdlVendor = $("#DdlVendor option").length;
                var VndStatus = $("#DdlVendor option:selected").text();
                if (VndStatus == "--Select Vendor--") {
                    err += "Please Select Vendor \n";
                    $("#DdlVendor").css("border", "1px solid #ff0000");
                    iserr = true;
                }
                else if (VndStatus == "--Vendor Not Exist--") {
                    err += "Vendor not Exist for this process group, please contact administrator \n";
                    $("#DdlVendor").css("border", "1px solid #ff0000");
                    iserr = true;
                }
            }
            else {
                var TableVendorList;
                if ($("#DdlToolAmortize").val() == "YES") {
                    TableVendorList = $("#GvVndToolAmortize");
                }
                else {
                    TableVendorList = $("#grdvendor");
                }

                if (TableVendorList != null) {
                    var checkedBoxesCount = 0;
                    if ($("#DdlToolAmortize").val() == "YES") {
                        checkedBoxesCount = $("#GvVndToolAmortize").find("input:checkbox:checked").length;
                    }
                    else {
                        checkedBoxesCount = $("#grdvendor").find("input:checkbox:checked").length;
                    }

                    if (checkedBoxesCount == 0) {
                        err += "Please select at least 1 vendor ! \n";
                        iserr = true;
                    }
                }
            }

            if (iserr == true) {
                alert(err);
                return false;
            }
        }

        function ResetFiledColor(txtID) {
            if (txtID == "FileUpload1") {
                document.getElementById(txtID).style.border = "1px solid #CCCCCC";
            }
            else {
                document.getElementById(txtID).style.border = "1px solid #CCCCCC";
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

        function OpenModalConfirm() {
            try {
                jQuery.noConflict();
                $("#MdConfirm").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            catch (err) {
                alert(err + ' : OpenModalConfirm');
            }
        }

        function CloseModalConfirm() {
            try {
                jQuery.noConflict();
                $("#MdConfirm").modal('hide');
            }
            catch (err) {
                alert(err + ' : CloseModalConfirm');
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

        function DatePitcker() {
            try {
                (function ($) {
                    $(".form_datetime").datetimepicker({
                        //format: "dd-mm-yyyy - hh:ii",
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
                alert(e);
            }
        }

        function DatePitckerAppr(id) {
            try {
                (function ($) {
                    $('#GvDuplicateWithExpiredReq_TxtNewDueDate_' + id).datetimepicker({
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

        function CheckAllRejOrChgDate(HeaderAction) {
            try {
                var RowsCountGv = $('#GvDuplicateWithExpiredReq tr').length;

                if (HeaderAction == "Reject") {
                    for (var i = 1; i < RowsCountGv; i++) {
                        if (document.getElementById("GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "") != null) {
                            document.getElementById("GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "").checked = true;
                            RbRejectExpReq(i - 1);
                        }
                    }
                }
                else {
                    for (var i = 1; i < RowsCountGv; i++) {
                        if (document.getElementById("GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "") != null) {
                            document.getElementById("GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "").checked = true;
                            RbChangedateResDueDate(i - 1);
                        }
                    }
                }
            }
            catch (err) {
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
                        if (document.getElementById("GvDuplicateWithExpiredReq_RbReject_" + (i - 1) + "") != null) {
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
                        if (document.getElementById("GvDuplicateWithExpiredReq_RbChangeDate_" + (i - 1) + "") != null) {
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

        function IsDatePitckerEnabled(txtid) {
            try {
                if (document.getElementById(txtid).disabled == false) {
                    $('#' + txtid).focus();
                }
            }
            catch (err) {
                alert(err + ": IsDatePitckerEnabled(iconid,txtid)")
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


        $(document).on('keypress', '#txtDate', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });

        $(document).on('keypress', '#TxtEffectiveDate', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });


    </script>

    <script type="text/javascript">
        function send() {
            try {
                var fd = new FormData();
                fd.append("fileToUpload", document.getElementById('FileUpload1').files[0]);
                var xhr = new XMLHttpRequest();
                xhr.open("POST", "SaveFileUploadAttachment.aspx?Source=NewReq");
                xhr.send(fd);

                var flupload = document.getElementById('FileUpload1').files[0];
                var flname = flupload.name;

                $('#lblMessage').text(flname);
            } catch (err) {
                alert("UploadFile() : " + err);
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
                                                <asp:RadioButton ID="article" runat="server" Text="&nbsp; With SAP Code" onclick="document.location.href='NewRequest.aspx?num=1'"
                                                    GroupName="RegularMenu" TextAlign="Right" AutoPostBack="true" Font-Bold="false"
                                                    OnCheckedChanged="article_CheckedChanged" />
                                            </div>
                                            <div class="col-md-4" style="padding-top: 5px; padding-bottom: 3px;">
                                                <asp:RadioButton ID="RbWithouSAPCode" runat="server" Text="&nbsp; Without SAP Code" onclick="document.location.href='NewRequest.aspx?num=2'"
                                                    GroupName="RegularMenu" TextAlign="Right" AutoPostBack="true" Font-Bold="false"
                                                    OnCheckedChanged="RbWithouSAPCode_CheckedChanged" />
                                            </div>
                                            <div class="col-md-4" style="padding-top: 5px; padding-bottom: 3px;">
                                                <asp:RadioButton ID="RbWithouSAPGp" runat="server" Text="&nbsp; Without SAP Code (GP)" Font-Bold="false"
                                                    GroupName="RegularMenu" TextAlign="Right" AutoPostBack="true"
                                                    OnCheckedChanged="RbWithouSAPGp_CheckedChanged" />
                                            </div>
                                            <div class="col-md-4" style="display: none">
                                                <asp:RadioButton ID="draftcost" runat="server" Text=""
                                                    GroupName="RegularMenu" TextAlign="Left" AutoPostBack="true"
                                                    OnCheckedChanged="draftcost_CheckedChanged" />
                                                <asp:Label ID="Label13" Text="Draft and Cost Planning" runat="server" Visible="False"></asp:Label>
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
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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

                                                <div class="row" style="padding-bottom: 10px">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label1" runat="server" Text="Material Type:" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="ddlmatltype" runat="server" Width="100%" ForeColor="Black" onchange="EmptyRequestList();"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlmatltype_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label4" runat="server" Text="Plant Status" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="ddlplantstatus" runat="server" AutoPostBack="true" Width="100%" onchange="EmptyRequestList();"
                                                                    OnSelectedIndexChanged="ddlplantstatus_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label7" runat="server" Text="SAP Proc Type" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="ddlproctype" runat="server" AutoPostBack="true" Width="100%" onchange="EmptyRequestList();"
                                                                    ForeColor="Black" OnSelectedIndexChanged="ddlproctype_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label10" runat="server" Text="SAP Sp Proc Type" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="ddlsplproctype" runat="server" AutoPostBack="true" Width="100%" onchange="EmptyRequestList();" ForeColor="Black"
                                                                    OnSelectedIndexChanged="ddlsplproctype_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:CheckBox runat="server" Checked="true" ID="ChcDisMatCost" Visible="false" Enabled="true" Text="&nbsp; Disabled Mat Cost" ForeColor="#000099"></asp:CheckBox>
                                                                <asp:TextBox ID="txtsplproc" runat="server" Width="100%" Visible="false"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label5" runat="server" Text="Product" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="DdlProduct" runat="server" CssClass="select2-size-sm form-control" AutoPostBack="true" onchange="EmptyRequestList();" OnSelectedIndexChanged="DdlProduct_SelectedIndexChanged"></asp:DropDownList>
                                                                <%--<asp:TextBox ID="txtprodID" runat="server" TextMode="MultiLine" AutoPostBack="true" onchange="EmptyRequestList();" OnTextChanged="txtprodID_TextChanged" Width="100%"></asp:TextBox>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="lblmatldesc" runat="server" Text="Material Class Desc."
                                                                    ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="DdlMatClass" runat="server" AutoPostBack="true" onchange="EmptyRequestList();" OnSelectedIndexChanged="DdlMatClass_SelectedIndexChanged"></asp:DropDownList>
                                                                <%--<asp:TextBox ID="txtmatlclass" runat="server" Width="100%" TextMode="MultiLine" AutoPostBack="true" onchange="EmptyRequestList();" OnTextChanged="txtmatlclass_TextChanged" Visible="False"></asp:TextBox>--%>
                                                                <%--<asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="1" CompletionListElementID="pnlPrdCode" EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1" ServiceMethod="getMatlClass" TargetControlID="txtmatlclass" UseContextKey="True">
                                                                </asp:AutoCompleteExtender>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px; display: block" runat="server" id="DvDdlSAP">
                                                    <div class="col-md-10">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                SAP Part Code & Desc
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div style="display:none">
                                                                    
                                                                    <asp:TextBox runat="server" ID="TxtDataJson" Text="" CssClass="select2-size-sm form-control" AutoPostBack="false"></asp:TextBox>
                                                                </div>

                                                                <asp:DropDownList ID="DdlMatCodeList" runat="server" CssClass="select2-size-sm form-control" AutoPostBack="true" onchange="ShowLoading();EmptyRequestList();"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;" runat="server" id="DvTxtSAPCode">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="LbPartCode" runat="server" Text="SAP Part Code" Enabled="false"
                                                                    ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="txtpartdesc" runat="server" onchange="EmptyRequestList();"
                                                                    AutoPostBack="true" TextMode="MultiLine" OnTextChanged="txtpartdesc_TextChanged1" Width="100%"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label6" runat="server" Text="Part Code Description" Enabled="false" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="txtpartdescription" Width="100%" TextMode="MultiLine" onchange="EmptyRequestList();ChangeEmptyFieldColor();"
                                                                    runat="server" AutoPostBack="false" ForeColor="Black" OnTextChanged="txtpartdescription_TextChanged1"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label9" runat="server" Text="PIR Type" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <asp:DropDownList ID="ddlpirtype" runat="server" AutoPostBack="true" onchange="EmptyRequestList();"
                                                                            ForeColor="Black" Width="100%"
                                                                            OnSelectedIndexChanged="ddlpirtype_SelectedIndexChanged1">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox ID="txtPIRDesc" runat="server" Width="100%"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label11" runat="server" Text="Net Weight:" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox ID="txtunitweight" runat="server" AutoPostBack="false" Width="100%" onchange="EmptyRequestList();ValOnlyNo('txtunitweight','txt');ChangeEmptyFieldColor();"
                                                                            oninput="validateNumber('txtunitweight')" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox ID="txtUOM" runat="server" AutoPostBack="false" Width="100%" onchange="EmptyRequestList();ChangeEmptyFieldColor();"
                                                                            AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                                                    </div>
                                                                </div>
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
                                                                        <asp:DropDownList ID="DdlReason" runat="server" AutoPostBack="true" onchange="EmptyRequestList();"
                                                                            OnSelectedIndexChanged="DdlReason_SelectedIndexChanged">
                                                                        </asp:DropDownList>
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
                                                                <asp:Label ID="Label19" runat="server" Text="Mnth.Est.Qty & Base UOM:" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox ID="txtMQty" runat="server" AutoPostBack="false" Width="100%" onchange="EmptyRequestList();ValOnlyNo('txtMQty','txt');ChangeEmptyFieldColor();"
                                                                            oninput="validateNumber('txtMQty')" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:TextBox ID="txtBaseUOM1" runat="server" AutoPostBack="false" Width="100%" onchange="EmptyRequestList();ChangeEmptyFieldColor();"
                                                                            AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                                                    </div>
                                                                </div>
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
                                                                <asp:Label ID="Label26" runat="server" Text="1<sup>st</sup> Delivery Date & Qty" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
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
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <div class="row">
                                                                <div class="col-md-6" style="border: double; height: 50px; padding: 5px;">
                                                                    <asp:Label ID="lbldrawng" runat="server" Text="Drawing No:" ForeColor="Black"></asp:Label>
                                                                    <asp:Label ID="lblMessage" runat="server" Enabled="False" ForeColor="Black" Text="lblMessage" Font-Bold="true"></asp:Label>
                                                                </div>
                                                                <div class="col-md-6" style="border: double; height: 50px; padding: 10px 5px 5px 5px;">
                                                                    <asp:FileUpload ID="FileUpload1" ToolTip="" runat="server" EnableViewState="true" CssClass="form-control-sm" Font-Size="14px" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="row">
                                                                <div class="col-md-6" style="border: double; vertical-align: central; height: 50px; padding: 5px;">
                                                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClientClick="if(validateBtnUpload()==false) return false;send();"
                                                                        OnClick="btnUpload_Click" CssClass="btn btn-sm btn-primary" Width="100%" Font-Size="14px" />
                                                                </div>
                                                                <div class="col-md-6" style="border: double; height: 50px; padding: 5px; vertical-align: central;">
                                                                    <asp:LinkButton ID="btnViewDownPDF" OnClick="btnViewDownPDF_Click" runat="server"
                                                                        OnClientClick="if(validatebtnViewDownPDF()==false) return false;"
                                                                        Text=" Download & View" CssClass="btn btn-link" ForeColor="Blue"></asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <br />
                                                    <div class="col-md-6" runat="server" id="DvEffectiveDate">
                                                        <div class="row">
                                                            <div class="col-lg-5">
                                                                <asp:Label ID="Label3" runat="server" ForeColor="Black" Text="Effective Date"></asp:Label>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <div class="group-main">
                                                                    <div class="SearchBox-txt">
                                                                        <asp:TextBox ID="TxtEffectiveDate" OnclientClick="return false;" onchange="ResetFiledColor('TxtEffectiveDate');EmptyRequestList();" Text=""
                                                                            onkeydown="javascript:preventInput(event);" CssClass="form_datetime" OnTextChanged="TxtEffectiveDate_TextChanged" AutoPostBack="true"
                                                                            autocomplete="off" AutoCompleteType="Disabled"
                                                                            runat="server" ForeColor="Black">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 5px 5px 3px 3px;">
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
                                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 5px 5px 3px 3px;">
                                                                        <i class="fa fa-calendar" style="color: #005496;" onclick="IsDatePitckerEnabled('TxtDuenextRev');"></i>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-lg-5">
                                                                <asp:Label ID="Label22" runat="server" ForeColor="Black" Text="Quotation response due date"></asp:Label>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <div class="group-main">
                                                                    <div class="SearchBox-txt">
                                                                        <asp:TextBox ID="txtDate" OnclientClick="return false;" onchange="ResetFiledColor('txtDate');EmptyRequestList();" CssClass="form_datetime"
                                                                            onkeydown="javascript:preventInput(event);"
                                                                            autocomplete="off" AutoCompleteType="Disabled"
                                                                            runat="server" ForeColor="Black">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 5px 5px 3px 3px">
                                                                        <i class="fa fa-calendar" style="color: #005496;" onclick="javascript: $('#txtDate').focus();"></i>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row" id="DvIsuseToolAmor">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label30" runat="server" ForeColor="Black" Text="Use Tool Amortize"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="DdlToolAmortize" runat="server" AutoPostBack="true" Width="100%" onchange="EmptyRequestList();"
                                                                    ForeColor="Black" OnSelectedIndexChanged="DdlToolAmortize_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0"> -- Select Use Tool Amortize Condition -- </asp:ListItem>
                                                                    <asp:ListItem Value="YES"> YES </asp:ListItem>
                                                                    <asp:ListItem Value="NO"> NO </asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="lbl_cntact2" runat="server" ForeColor="Black" Text="Process Group"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="ddlprocess" runat="server" AutoPostBack="true" Width="100%" onchange="EmptyRequestList();"
                                                                    ForeColor="Black" OnSelectedIndexChanged="ddlprocess_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="lbljobtypedesc" runat="server" Text="PIR Job Type & Desc." ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:DropDownList ID="ddlpirjtype" runat="server" AutoPostBack="true" Width="100%" onchange="EmptyRequestList();"
                                                                    ForeColor="Black" OnSelectedIndexChanged="ddlpirjtype_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtjobtypedesc" runat="server" AutoPostBack="true" Width="100%"
                                                                    OnTextChanged="txtjobtypedesc_TextChanged" Visible="False"></asp:TextBox>
                                                                <%--<asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="1"
                                                                    CompletionListElementID="pnlPrdCode" EnableCaching="true" FirstRowSelected="true"
                                                                    MinimumPrefixLength="1" ServiceMethod="getSAPJOB" TargetControlID="txtjobtypedesc"
                                                                    UseContextKey="True">
                                                                </asp:AutoCompleteExtender>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5">
                                                                <asp:Label ID="Label14" runat="server" Text="Plating Type" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7">
                                                                <asp:TextBox ID="txtplatingtype" runat="server" AutoPostBack="true" autocomplete="off" Width="100%"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5" style="padding-top: 5px; padding-bottom: 3px;">
                                                                <asp:Label ID="Label8" runat="server" Text="Vendor Type" ForeColor="Black"></asp:Label>
                                                            </div>
                                                            <div class="col-md-7" style="padding-top: 5px; padding-bottom: 3px;">
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
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="UpVendorType" runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px;" id="DvImRcylRatio" visible="false" runat="server">
                                                    <div class="col-md-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5">
                                                                <asp:Label ID="Label27" runat="server" ForeColor="Black" Text="Recycle Ratio (%)"></asp:Label>
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
                                                                                            <asp:Label ID="LbCheck" runat="server" AutoPostBack="false" Text="SE" ForeColor="Transparent" />
                                                                                            <asp:CheckBox ID="chkSelectAllVndToolAmor" runat="server" AutoPostBack="false" Visible="false" OnCheckedChanged="chkSelectAllVndToolAmor_CheckedChanged" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkVndToolAmor" runat="server" onclick="ShowLoading();" AutoPostBack="true" Text='<%# Eval("VendorCode") %>' ForeColor="Transparent" Width="0px" OnCheckedChanged="chkVndToolAmor_CheckedChanged" />
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
                                                                        <asp:DropDownList runat="server" ID="DdlVendor" onchange="EmptyRequestList();"
                                                                            OnSelectedIndexChanged="DdlVendor_SelectedIndexChanged" AutoPostBack="true">
                                                                        </asp:DropDownList>
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
                                                    <div class="col-md-6">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row">
                                                            <div class="col-md-5"></div>
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
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="UpInvalidRequest" runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px" runat="server" id="DvInvalidRequest" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="background: #fa0606">
                                                            <asp:Label ID="Label18" runat="server" Text="Request Can not be created due on : vendor selected still Under Processing in below request "
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


                                        <asp:UpdatePanel ID="UpOldReqAutoRej" runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px" id="DvOldReqAutoRej" runat="server" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="background: #cf8603">
                                                            <asp:Label ID="LbAutorejectText" runat="server" Text="Below Request Will Auto Rej due on : New Request Creation , Response Date is Expired And Vendor Not Response" ForeColor="White" Font-Bold="true"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="table table-responsive table-sm Padding-Nol" style="padding-top: 10px; background-color: white;">
                                                            <asp:GridView ID="GvOldReqAutoRej" runat="server" AutoGenerateColumns="False"
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

                                        <asp:UpdatePanel ID="UpUpdateOldReq" runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px" id="DvUpdateOldReq" runat="server" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="background: #b502bd">
                                                            <asp:Label ID="Label25" runat="server" Text="New Request Can not be create due On : the Request for Below Vendor still Under processing, and vendor not in the list will be add into below request" ForeColor="White" Font-Bold="true"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="table table-responsive table-sm Padding-Nol" style="padding-top: 10px; background-color: white;">
                                                            <asp:GridView ID="GvUpdateOldReq" runat="server" AutoGenerateColumns="False"
                                                                AllowPaging="false" PageSize="10" OnRowDataBound="GvUpdateOldReq_RowDataBound"
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
                                                                    <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-CssClass="text-center "></asp:BoundField>
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

                                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px" runat="server" id="DvInvalidRequestHasApprv" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="background: #f2318f">
                                                            <asp:Label ID="Label29" runat="server" Text="Request Can not be created due on : vendor and material selected has been approved "
                                                                Visible="true" ForeColor="White" Font-Bold="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="table table-responsive table-sm">
                                                            <asp:GridView ID="GvInvalidRequestHasApprv" runat="server" AutoGenerateColumns="False"
                                                                AllowPaging="false" PageSize="10" OnRowDataBound="GvInvalidRequestHasApprv_RowDataBound"
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

                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="padding-bottom: 10px" runat="server" id="DvValidRequest" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="background: #259404">
                                                            <asp:Label ID="Label2" runat="server" Text="Quote Request can be Created only for below Vendors"
                                                                Visible="false" ForeColor="White"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div class="table table-responsive table-sm table-nowrap">
                                                            <asp:Table ID="Table1" runat="server" CssClass="table-sm table-bordered"></asp:Table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="Table1" />
                                            </Triggers>
                                        </asp:UpdatePanel>


                                        <div class="row" style="padding-bottom: 10px">
                                            <div class="col-md-6">
                                            </div>
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label15" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="Label16" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="Label17" runat="server" Visible="False"></asp:Label>
                                                        <asp:HiddenField ID="hdnReqNo" runat="server" />
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <div style="padding-bottom: 10px" runat="server" id="DvConfirmForSubmit" visible="false">
                                                                    <asp:Button ID="Button1" runat="server" CssClass="Login-button"
                                                                        OnClientClick="if(validateSubmitReq()==false) return false;if(ConfirmMsgSAPSpProcType()==false) return false;"
                                                                        Text="Submit" Visible="false" Width="100%" OnClick="Button1_Click" />
                                                                    <div style="display: none;">
                                                                        <asp:Button ID="BtnSubmitReq" runat="server" CssClass="Login-button"
                                                                            Text="Submit" Visible="false" Width="100%" OnClick="Button1_Click" />
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
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

        <!-- Modal confirmation-->
        <div class="modal fade" id="MdConfirm" data-backdrop="static" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 5px; background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;">
                                <div class="col-md-12 Padding-Nol" style="font: bold 22px calibri, calibri; text-align: center; align-content: center;">
                                    <span class="glyphicon glyphicon-question-sign" style="color: #005496; font-size: 24px;"></span>
                                    Confirmation 
                                </div>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-xs-12" style="padding: 10px">
                                                <asp:Label runat="server" ID="LbMessage" Text="Current SAP Sp Proc Type selected is 30, and Material Cost is Disabled. Do you want to Continue ???" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer" style="padding: 5px; background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-top-left-radius: 15px; border-top-right-radius: 15px;">
                                <asp:Button ID="BtnYes" runat="server" Text="Yes" OnClientClick="CloseModalConfirm();ShowLoading();" OnClick="Button1_Click" CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="14px" />
                                <asp:Button ID="BtnNo" runat="server" Text="No" OnClientClick="CloseModalConfirm();return false;" autopostback="false" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="14px" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

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
                                                                        <asp:RadioButton ID="RbAllReject" runat="server" Text=" &nbsp; Reject" GroupName="RbActionHeader" onchange="CheckAllRejOrChgDate('Reject')" />
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
                                    <asp:HiddenField runat="server" ID="HdnLayoutId" />
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
