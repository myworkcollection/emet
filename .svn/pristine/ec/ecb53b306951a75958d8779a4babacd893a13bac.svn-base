<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewReq_changesTmShmn.aspx.cs" Inherits="Material_Evaluation.NewReq_changesTmShmn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <title>eMET</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <!-- Bootstrap core CSS-->
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />

    <!-- Page level plugin CSS-->
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" />

    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet" />

    <link href="Styles/NewStyle/Style.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />

    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>

    <style type="text/css">
        .group-main {
          display: flex;
        }

        .SearchBox-txt {
          flex: 1 0 8em;
        }

        .SearchBox-btn {
          /* Never shrink or grow */
          flex: 0 0 auto;
          padding-top:6px;
          padding-left:5px;
          padding-right:6px;
          border-top-right-radius: 4px;
          border-bottom-right-radius: 4px;
        }

        #loading {
           width: 100%;
           height: 100%;
           top: 0;
           left: 0;
           position: fixed;
           display: block;
           opacity:0.6;
           background-color: whitesmoke;
           z-index: 99999;
           text-align: center;
        }

        #loading-image {
              position: center;
              z-index: 99999;
              opacity:1;
            }

        .table-nowrap th,td{
            white-space: nowrap;
            font-size:14px;
        }
    </style>


    <script type="text/javascript">
        function validateNumber(txtID) {
            
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

        $(function () {
            $("[id*=TextBox1]").datepicker({
                //showOn: 'button',
                buttonImageOnly: true,
                dateFormat: "dd/mm/yy",
                minDate: 0,
                buttonImage: 'images/calendar.png'
            });
        });
        $(function () {
            $("[id*=txtfinal]").datepicker({
                //showOn: 'button',
                buttonImageOnly: true,
                dateFormat: "dd/mm/yy",
                minDate: 0,
                buttonImage: 'images/calendar.png'
            });
        });

        var CellscountGlobal = 0;

        $(document).ready(function () {
            // alert("ready");
            $(function () {


                var CellsCount = $("#TablePC").find('tr')[0].cells.length;
                CellscountGlobal = CellsCount;
                // alert("Cells" + CellscountGlobal);
                for (var i = 0; i < CellsCount - 1; i++) {

                    $("#dynamicddlMachine" + i).show();
                    $("#txtMachineId" + i).hide();

                }
            });

        });


        function dynamicddlMachineLaborMethod(seltext,i)
        {
            $("#txtStandardRate\\/HR" + i).val("");
            $("#txtVendorRate" + i).val("");

            //var selectedText = $(this).find("option:selected").text();
            //var selectedValue = $(this).val();
            //
            //subash
            //$("#txtStandardRate\\/HR"+i).prop("disabled", false);
            $("#txtVendorRate"+i).prop("disabled", false);

            if (seltext.toString().toUpperCase() == "LABOR") {
                $("#dynamicddlMachine"+i).hide();
                $("#txtMachineId"+i).show();


                $('#grdLaborlisthidden tr').each(function () {
                    var checkval = 0;
                    $(this).find('td').each(function () {

                        if (checkval == 0) {
                            $("#txtStandardRate\\/HR"+i).val($(this).text());
                            $("#txtVendorRate"+i).val($(this).text());
                            checkval = 1;
                        }

                        if ($(this).html() == "Y") {
                            $("#txtVendorRate"+i).prop("disabled", true);
                            $("#hdnVendorActivity").val($(this).html());
                        }
                        else if ($(this).html() == "N") {
                            $("#txtVendorRate"+i).prop("disabled", false);
                            $("#hdnVendorActivity").val($(this).html());
                        }

                    })
                });

            }
            else {
                $("#dynamicddlMachine"+i).show();
                $("#txtMachineId"+i).hide();


                $('#grdMachinelisthidden tr').each(function () {
                    var checkval = 0;
                    $(this).find('td').each(function () {

                       // alert($(this).text());
                        if (checkval == 1) {
                            $("#txtStandardRate\\/HR"+i).val($(this).text());
                            $("#txtVendorRate"+i).val($(this).text());
                            checkval++;
                        }

                        if ($(this).text().trim().trimLeft().trimRight() == $("#dynamicddlMachine"+i).find("option:selected").text().trim().trimLeft().trimRight()) {
                            checkval++;
                        }

                        if (checkval == 2) {
                            if ($(this).html() == "Y") {
                                $("#txtVendorRate"+i).prop("disabled", true);
                                $("#hdnVendorActivity").val($(this).html());
                            }
                            else if ($(this).html() == "N") {
                                $("#txtVendorRate"+i).prop("disabled", false);
                                $("#hdnVendorActivity").val($(this).html());
                            }

                        }

                    })
                });

            }
            $("#txtStandardRate\\/HR"+i).prop("disabled", true);
            $("#txtVendorRate"+i).prop("disabled", true);
        }


        function dynamicddlMachineMethod(i)
        {
            $('#grdMachinelisthidden tr').each(function () {
                var checkval = 0;
                $(this).find('td').each(function () {
                  //  alert($(this).text());
                    if (checkval == 1) {
                        $("#txtStandardRate\\/HR"+i).val($(this).text());
                        $("#txtVendorRate"+i).val($(this).text());
                        checkval++;
                    }

                    if ($(this).text().trim().trimLeft().trimRight() == $("#dynamicddlMachine"+i).find("option:selected").text().trim().trimLeft().trimRight()) {
                        checkval++;
                    }

                    if (checkval == 2) {
                        if ($(this).html() == "Y") {
                            $("#txtVendorRate"+i).prop("disabled", true);
                            $("#hdnVendorActivity").val($(this).html());
                        }
                        else if ($(this).html() == "N") {
                            $("#txtVendorRate"+i).prop("disabled", false);
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

        function TurnKeyVendorUpdate(turnv, i) {

            var trunval = turnv;
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

                document.getElementById('BtnFndVndMachineVsProcUom').click();
            }
            else {
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


        $(document).on('keyup', '#txtIfTurnkey-VendorName0', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 0);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName1', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 1);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName2', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 2);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName3', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 3);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName4', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 4);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName5', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 5);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName6', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 6);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName7', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 7);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName8', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 8);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName9', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 9);

        });

        $(document).on('keyup', '#txtIfTurnkey-VendorName10', function () {

            var trunval = $(this).val();
            TurnKeyVendorUpdate(trunval, 10);

        });


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
            else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "PCS/LOAD" || prosuom.toString().replace(/\s/g, '').toUpperCase() == "PC") {
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
                else
                {
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

            var ArrProsuom = prosuom.toString().replace(/\s/g, '').toUpperCase().split('-');
            if (ArrProsuom.length > 0)
            {
                prosuom = ArrProsuom[0].toString().trim();
            }
            if (prosuom.toString().replace(/\s/g, '').toUpperCase() == ("STROKES/MIN")) {
                var PrcGrpCodeFull = $('#dynamicddlProcess' + i + ' :selected').text();
                var DataMachine = $('#dynamicddlMachine' + i + ' :selected').text();

                var ArrPrcGrpCode = PrcGrpCodeFull.split("-");
                var ArrDataMachine = DataMachine.split("-");

                var PrcGrpCode = "";
                var MachineID = "";
                var MachineType = "";
                var MacTonnage = "";

                if (ArrPrcGrpCode.length == 2) {
                    PrcGrpCode = ArrPrcGrpCode[0].toString();
                }

                if (ArrDataMachine.length > 0) {
                    MachineID = ArrDataMachine[0].toString();

                    //MachineType = ArrDataMachine[3].toString();
                    //MacTonnage = ArrDataMachine[2].toString();
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

        };

        function dynamicddlProcessMethod(seltext , i)
        {
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
                        var mySelect = $("#dynamicddlSubProcess"+i);
                        mySelect.append($('<option></option>').val($(this).html()).html($(this).html()));

                        checkval = 2;
                    }
                    else if (checkval == 2 && firsttime == 0) {
                       // alert($(this).text() + "al");
                        document.getElementById("txtProcessUOM"+i).value = $(this).text();
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

                if (substringseltext.indexOf("-") != -1)
                {
                    substringseltext = substringseltext.toString().substring(0, 2);
                }

               // alert("sel" + substringseltext);
                if (processval != "") {
                    if (processval.toString().toUpperCase() == substringseltext.toString().toUpperCase()) {
                       // alert("seltext" + substringseltext);
                       
                        var mySelect = $("#dynamicddlMachine" + i);
                        mySelect.append($('<option></option>').val($(this).find("td:eq(0)").text()).html($(this).find("td:eq(0)").text()));

                    }
                    else
                    {
                       
                    }
                }
            });

            dynamicddlMachineMethod(i);
        }

        function dynamicddlSubprocessMethod(seltext,i)
        {
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
                        document.getElementById("txtProcessUOM"+i).value = $(this).text();
                        checkval = 0;

                        Baseqtyupdatebyuom(i);
                    }
                })
            });
        }

        //disabled by hafiz : function onchange added from server side to make dynamic ddl

        //$(document).on('change', '#dynamicddlProcess0', function () {
           
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 0);
            
        //});
        

        //$(document).on('change', '#dynamicddlSubProcess0', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 0);
        //});

        //$(document).on('change', '#dynamicddlProcess1', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 1);
        //});

        //$(document).on('change', '#dynamicddlSubProcess1', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 1);

        //});

        //$(document).on('change', '#dynamicddlProcess2', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 2);
        //});

        //$(document).on('change', '#dynamicddlSubProcess2', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 2);

        //});

        //$(document).on('change', '#dynamicddlProcess3', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 3);
        //});

        //$(document).on('change', '#dynamicddlSubProcess3', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 3);

        //});

        //$(document).on('change', '#dynamicddlProcess4', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 4);
        //});

        //$(document).on('change', '#dynamicddlSubProcess4', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 4);

        //});

        //$(document).on('change', '#dynamicddlProcess5', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 5);
        //});

        //$(document).on('change', '#dynamicddlSubProcess5', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 5);

        //});

        //$(document).on('change', '#dynamicddlProcess6', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 6);
        //});

        //$(document).on('change', '#dynamicddlSubProcess6', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 6);

        //});

        //$(document).on('change', '#dynamicddlProcess7', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 7);
        //});

        //$(document).on('change', '#dynamicddlSubProcess7', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 7);

        //});

        //$(document).on('change', '#dynamicddlProcess8', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 8);
        //});

        //$(document).on('change', '#dynamicddlSubProcess8', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 8);

        //});

        //$(document).on('change', '#dynamicddlProcess9', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlProcessMethod(selectedText, 9);
        //});

        //$(document).on('change', '#dynamicddlSubProcess9', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();

        //    dynamicddlSubprocessMethod(selectedText, 9);

        //});


        //$(document).on('change', '#dynamicddlMachineLabor0', function () {
            //var selectedText = $(this).find("option:selected").text();
            //dynamicddlMachineLaborMethod(selectedText, 0);


            //var selectedValue = $(this).val();

            //$("#txtStandardRate\\/HR0").prop("disabled", false);
            //$("#txtVendorRate0").prop("disabled", false);

            //if (selectedText == "Labor") {
            //    $("#dynamicddlMachine0").hide();
            //    $("#txtMachineId0").show();


            //    $('#grdLaborlisthidden tr').each(function () {
            //        var checkval = 0;
            //        $(this).find('td').each(function () {

            //            if (checkval == 0) {
            //                $("#txtStandardRate\\/HR0").val($(this).text());
            //                $("#txtVendorRate0").val($(this).text());
            //                //document.getElementById("txtStandardRate/HR0").value = $(this).text();
            //                //document.getElementById("txtVendorRate0").value = $(this).text();
            //                checkval = 1;
            //            }

            //            if ($(this).html() == "Y") {
            //                $("#txtVendorRate0").prop("disabled", true);
            //                $("#hdnVendorActivity").val($(this).html());
            //            }
            //            else if ($(this).html() == "N") {
            //                $("#txtVendorRate0").prop("disabled", false);
            //                $("#hdnVendorActivity").val($(this).html());
            //            }

            //        })
            //    });

            //}
            //else {
            //    $("#dynamicddlMachine0").show();
            //    $("#txtMachineId0").hide();


            //    $('#grdMachinelisthidden tr').each(function () {
            //        var checkval = 0;
            //        $(this).find('td').each(function () {

            //          //  alert($(this).text());
            //            if (checkval == 1) {
            //                $("#txtStandardRate\\/HR0").val($(this).text());
            //                $("#txtVendorRate0").val($(this).text());
            //                checkval++;
            //            }

            //            if ($(this).text().trim().trimLeft().trimRight() == $("#dynamicddlMachine0").find("option:selected").text().trim().trimLeft().trimRight()) {
            //                checkval++;
            //            }

            //            if (checkval == 2) {
            //                if ($(this).html() == "Y") {
            //                    $("#txtVendorRate0").prop("disabled", true);
            //                    $("#hdnVendorActivity").val($(this).html());
            //                }
            //                else if ($(this).html() == "N") {
            //                    $("#txtVendorRate0").prop("disabled", false);
            //                    $("#hdnVendorActivity").val($(this).html());
            //                }

            //            }

            //        })
            //    });

            //}
            //$("#txtStandardRate\\/HR0").prop("disabled", true);
            //$("#txtVendorRate0").prop("disabled", true);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor1', function () {
        //    var selectedText = $(this).find("option:selected").text();
        //    //alert("test");
        //    dynamicddlMachineLaborMethod(selectedText, 1);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor2', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    //var selectedValue = $(this).val();

        //    dynamicddlMachineLaborMethod(selectedText, 2);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor3', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    dynamicddlMachineLaborMethod(selectedText, 3);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor4', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    dynamicddlMachineLaborMethod(selectedText, 4);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor5', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    dynamicddlMachineLaborMethod(selectedText, 5);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor6', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    dynamicddlMachineLaborMethod(selectedText, 6);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor7', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();


        //    dynamicddlMachineLaborMethod(selectedText, 7);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor8', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();


        //    dynamicddlMachineLaborMethod(selectedText, 8);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor9', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();


        //    dynamicddlMachineLaborMethod(selectedText, 9);

        //});

        //$(document).on('change', '#dynamicddlMachineLabor10', function () {

        //    var selectedText = $(this).find("option:selected").text();
        //    var selectedValue = $(this).val();


        //    dynamicddlMachineLaborMethod(selectedText, 10);

        //});


        //$(document).on('change', '#dynamicddlMachine0', function () {

        //    dynamicddlMachineMethod(0);


        //});

        //$(document).on('change', '#dynamicddlMachine1', function () {


        //    dynamicddlMachineMethod(1);

        //});

        //$(document).on('change', '#dynamicddlMachine2', function () {


        //    dynamicddlMachineMethod(2);

        //});

        //$(document).on('change', '#dynamicddlMachine3', function () {


        //    //var seltext = $("#dynamicddlMachine3").find("option:selected").text().trim();

        //    dynamicddlMachineMethod(3);

        //});

        //$(document).on('change', '#dynamicddlMachine4', function () {


        //    dynamicddlMachineMethod(4);
        //});

        //$(document).on('change', '#dynamicddlMachine5', function () {


        //    dynamicddlMachineMethod(5);

        //});

        //$(document).on('change', '#dynamicddlMachine6', function () {

        //    dynamicddlMachineMethod(6);

        //});

        //$(document).on('change', '#dynamicddlMachine7', function () {

        //    dynamicddlMachineMethod(7);

        //});

        //$(document).on('change', '#dynamicddlMachine8', function () {

        //    dynamicddlMachineMethod(8);

        //});

        //$(document).on('change', '#dynamicddlMachine9', function () {

        //    dynamicddlMachineMethod(9);

        //});

        //$(document).on('change', '#dynamicddlMachine10', function () {

        //    dynamicddlMachineMethod(10);

        //});

        //end disabled by hafiz


        $(document).on('keyup', '#txtProfit\\(\\%\\)0', function () {
            var trunval = $(this).val();
            var hdnVendorType = $("#hdnTGTotal").val();
            if (hdnVendorType != null || hdnVendorType != "" || hdnVendorType != "External") {
                if (trunval == '' || trunval == null) {
                    alert('empty');
                }
                else {
                    alert('ok');
                }
            }
            else {
                if (trunval == '' || trunval == null) {
                    $('#txtDiscount\\(\\%\\)0').prop("disabled", false);
                }
                else {
                    $('#txtDiscount\\(\\%\\)0').prop("disabled", true);
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
                    //

                    var ttFinalCost = parseFloat(ttlFinalProfit.toFixed(4));

                    //disabled by celindo : hafiz
                    //document.getElementById("txtFinalQuotePrice/pcs0").value = ttFinalCost;

                    $("#hdnTFinalQPrice").val(ttFinalCost);
                    $("#hdnProfit").val(ttlProfit);
                    $("#hdnDiscount").val(0);


                }
            }
        });

        $(document).on('keyup', '#txtDiscount\\(\\%\\)0', function () {

            var trunval = $(this).val();
            if (trunval == '' || trunval == null) {

                $('#txtProfit\\(\\%\\)0').prop("disabled", false);
                //disabled by celindo - hafiz
                //document.getElementById("txtFinalQuotePrice/pcs0").value = document.getElementById("txtGrandTotalCost/pcs0").value;
            }
            else {
                $('#txtProfit\\(\\%\\)0').prop("disabled", true);
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
                //
                //disabled by celindo - hafiz
                //document.getElementById("txtFinalQuotePrice/pcs0").value = ttFinaldiscCost;

                $("#hdnTFinalQPrice").val(ttFinaldiscCost);

                $("#hdnProfit").val(0);
                $("#hdnDiscount").val(ttDisc);

                //alert($("#hdnTFinalQPrice").val());

            }
        });

    </script>

    <script type="text/javascript" >
        function appendUSDoll(strValue) {
            return ("$" + fixedDecml(parseFloat(CheckNull(strValue))));
        }

        function fixedDecml(numValue) {
            return numValue.toFixed(4);
        }
       
        function submatlcost() {
            var CellsCount = $("#TableSMC").find('tr')[0].cells.length;
            for (var i = 0; i < CellsCount - 1; i++) {
                // alert("3");
                var submatDesc = document.getElementById("txtSub-Mat/T&JDescription" + i).value;
                var submatl = document.getElementById("txtSub-Mat/T&JCost" + i).value;
                var consumption = document.getElementById("txtConsumption(pcs)" + i).value;
                var submatpc = submatl / consumption;

                var CheckValsubmatpc = submatpc.toString().toUpperCase();
                if (CheckValsubmatpc == "NAN" || CheckValsubmatpc == "INFINITY") {
                    submatpc = 0;
                }
                document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value = parseFloat(submatpc.toFixed(6));
            }

            var totalsubcal = 0;
            for (var i = 0; i < CellsCount - 1; i++) {
                var ttlSubprocesscost = document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value;

                totalsubcal = ((+totalsubcal) + (+ttlSubprocesscost));

            }
            //var newtotalsub = truncator(totalsubcal, 4).toString();
            //document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value = totalsubcal;

            //add by celindo - hafiz
            var CheckValTot = totalsubcal.toString().toUpperCase();
            if (CheckValTot == "NAN" || CheckValTot == "INFINITY") {
                totalsubcal = 0;
            }
            document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value = parseFloat(totalsubcal.toFixed(4));
            document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value = parseFloat(totalsubcal.toFixed(4));
            //*

            /// for Dynamic Data Store 
            for (var i = 0; i < CellsCount - 1; i++) {
                // alert("IN")
                var rowCount111;
                rowCount111 = $('#hdngrdSubMatCost tr').length;
                // alert(rowCount111);
                if ($('#hdngrdSubMatCost tr').length > 0) {
                    // alert("OC");
                    $('#hdngrdSubMatCost tr:last').after('<tr><td>' + rowCount111 + '</td>' +
                        '<td>' + document.getElementById("txtSub-Mat/T&JDescription" + i).value + '</td>' +
                        '<td>' + document.getElementById("txtSub-Mat/T&JCost" + i).value + '</td>' +
                        '<td>' + document.getElementById("txtConsumption(pcs)" + i).value + '</td>' +
                '<td>' + document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value + '</td>' +
                '<td>' + document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value + '</td>' + '</tr>');
                }
            }

            submatlCostDataStore();
            ReCalculate();
        }

        function submatlCostDataStore() {

            var CellsCount = $("#TableSMC").find('tr')[0].cells.length;
           // $("#hdngrdSubMatCost").find("tr:not(:nth-child(1)):not(:nth-child(2))").remove();

            var hdnvaltemp = "";

            for (var i = 0; i < CellsCount - 1; i++) {

                var txtSubMatDesc = document.getElementById("txtSub-Mat/T&JDescription" + i).value;
                var txtSubMatCost = document.getElementById("txtSub-Mat/T&JCost" + i).value;
                var txtConsup = document.getElementById("txtConsumption(pcs)" + i).value;
                var txtCostPcs = document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value;
                var txttotalcost = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;

                    hdnvaltemp += ( + "," + txtSubMatDesc + ","+txtSubMatCost + "," + txtConsup + "," + txtCostPcs + "," + txttotalcost + ",");


                    $("#hdnSMCTableCount").val(i);
                //alert("IN")
                //var rowCount111;
                //rowCount111 = $('#hdngrdSubMatCost tr').length;
                
                //if ($('#hdngrdSubMatCost tr').length > 0) {
                //    alert("OC");
                //    $('#hdngrdSubMatCost tr:last').after('<tr><td>' + (rowCount111 - 1) + '</td>' +
                //        '<td>' + document.getElementById("txtSub-Mat/T&JDescription" + i).value + '</td>' +
                //        '<td>' + document.getElementById("txtSub-Mat/T&JCost" + i).value + '</td>' +
                //        '<td>' + document.getElementById("txtConsumption(pcs)" + i).value + '</td>' +
                //'<td>' + document.getElementById("txtSub-Mat/T&JCost/pcs" + i).value + '</td>' +
                //'<td>' + document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value + '</td>' + '</tr>');
            //}

           

            }
            $("#hdnSMCTableValues").val(hdnvaltemp);
        }

        // Calculate Process Cost Table

        function processcost() {
            
            var CellsCount = $("#TablePC").find('tr')[0].cells.length;
            var CellsCountMatCost = $("#Table1").find('tr')[0].cells.length;
            var varProcessGroup = $("#hdnLayoutScreen").val();
            var Validate = true;
            //  alert(CellsCount);
            var Hr_Rate;
            for (var i = 0; i < CellsCount - 1; i++) {
                if (Validate === false) {
                    $('#txtProcessCost\\/pc' + i).val("");
                    $('#txtTotalProcessesCost\\/pcs0').val("");
                    break;
                }

                var PrGroup = $('#dynamicddlProcess' + i + ' option:selected').text();
                var SubPrGroup = $('#dynamicddlSubProcess' + i + ' option:selected').text();
                if(PrGroup.trim() == '--Select--')
                {
                    Validate = false;
                    alert('Pleasee Select Process Group at column ' + (i + 1) + ' !');
                    break;
                }
                if (SubPrGroup.trim() == '--Select--') {
                    Validate = false;
                    alert('Pleasee Select Sub process at column ' + (i + 1) + ' !');
                    break;
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
                        base = cavity;
                    }
                    else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "CYCLETIMEINSEC/PC") {
                        base = 1;
                    }
                    else if (prosuom.toString().replace(/\s/g, '').toUpperCase() == "PCS/LOAD" || prosuom.toString().replace(/\s/g, '').toUpperCase() == "PC") {

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
                            else
                            {
                                partunit = document.getElementById("txtPartNetUnitWeight(g)" + 0).value;
                                base = parseInt(1000 / partunit);
                                document.getElementById("txtBaseqty" + i).value = base;
                            }
                        }
                        else
                        {
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
                    document.getElementById("txtProcessCost/pc" + i).value = parseFloat(processcost.toFixed(6));

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
                        document.getElementById("txtProcessCost/pc" + i).value = parseFloat(txtProcessCost.toFixed(6));
                        //document.getElementById("txtProcessCost/pc" + i).value = parseFloat(processcost.toFixed(6));
                    }
                }
                
                var BsQtyVal = $('#txtBaseqty' + i).val();
                var DurProcUom = $('#txtDurationperProcessUOM\\(Sec\\)' + i).val();
                var EfficiencyYield = $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + i).val();
                var txtIfTurnkeyVendorName = $('#txtIfTurnkey-VendorName' + i).val();
                var txtIfTurnkeySubVN = document.getElementById('dynamicddlSubvendorname' + i).selectedIndex;
                if (txtIfTurnkeyVendorName == "")
                {
                    if (txtIfTurnkeySubVN <= 0 ) {
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
                    else
                    {
                        var TurnkeyCost = $('#txtTurnkeyCost\\/pc' + i).val();
                        if (TurnkeyCost == "")
                        {
                            if (txtIfTurnkeySubVN > 0)
                            {
                                Validate = false;
                                alert("Please Enter Turn Key Cost !!");
                                $('#txtTurnkeyCost\\/pc' + i).focus();
                            }
                        }
                    }
                }
            }

            var totalprocesscal = 0;

            if (Validate == true)
            {
                for (var i = 0; i < CellsCount - 1; i++) {
                    var ttlprocesscost = document.getElementById("txtProcessCost/pc" + i).value;

                    totalprocesscal = ((+totalprocesscal) + (+ttlprocesscost));
                }
                //add by celindo - hafiz
                var CheckValTot = totalprocesscal.toString().toUpperCase();
                if (CheckValTot == "NAN" || CheckValTot == "INFINITY") {
                    totalprocesscal = 0;
                }
                document.getElementById("txtTotalProcessesCost/pcs0").value = parseFloat(totalprocesscal.toFixed(4));
                document.getElementById("txtTotalProcessesCost/pcs-0").value = parseFloat(totalprocesscal.toFixed(4));
                //*

                ProcessCostDataStore();
                ReCalculate();
            }
            return false;
        }

        //Session Maintain for Process Cost Table
        function ProcessCostDataStore() {

            var CellsCount = $("#TablePC").find('tr')[0].cells.length;
            var hdnvaltemp = "";
            for (var i = 0; i < CellsCount - 1; i++) {

                var txtProcessGroup = document.getElementById("dynamicddlProcess" + i).value;
                var txtSubProcess = document.getElementById("dynamicddlSubProcess" + i).value;

                var txtifVendorName = document.getElementById("txtIfTurnkey-VendorName" + i).value;
                var txtdynamicddlSubvendorname = "";
                var indxDdlSubvnd = $("#dynamicddlSubvendorname"+ i +" option:selected").index()
                if (indxDdlSubvnd == 0)
                {
                    txtdynamicddlSubvendorname = "";
                }
                else
                {
                    txtdynamicddlSubvendorname = document.getElementById("dynamicddlSubvendorname" + i).value;
                }
                
                var txtMachineLabor = document.getElementById("dynamicddlMachineLabor" + i).value;

                var txtMachineLaborconditionval = "";
               // alert(txtMachineLabor);
                if (txtMachineLabor == "Machine") {
                    //txtMachineLaborconditionval = document.getElementById("dynamicddlMachine" + i).value;
                    var SAPCode = $("#txtpartdesc").val();
                    if (SAPCode.replace('-', '').trim().toString() == "") {
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

        // Calculate Material Cost Table
        function MatlCalculation() {
            
            var CellsCount = $("#Table1").find('tr')[0].cells.length;
            var Validate = true;
            var varProcessGroup = $("#hdnLayoutScreen").val();

            for (var i = 0; i < CellsCount - 1; i++) {

                if (varProcessGroup == "Layout2") {
                    document.getElementById("txtMaterialCost/pcs" + i).value = 0;
                }
                else
                {
                    if (Validate === false) {
                        $('#txtScrapRebate\\/pcs' + i).val("");
                        break;
                    }
                    var rawmaterial = document.getElementById("txtRawMaterialCost/kg" + i).value;
                    if (isNaN(rawmaterial) == true) {
                        rawmaterial = 0;
                    }
                    var raw = rawmaterial;
                    var rawmatl_Grams = raw / 1000;

                    var totrawmatl_Grams = parseFloat(rawmatl_Grams).toFixed(6);
                    document.getElementById("txtTotalRawMaterialCost/g" + i).value = totrawmatl_Grams;
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

                    if (varProcessGroup == "Layout1") {  // Layout1 - IM

                        var runnerweight = document.getElementById("txt~RunnerWeight/shot(g)0").value;

                        if (isNaN(runnerweight) == true || runnerweight == "undefined" || runnerweight == "" || runnerweight == null) {

                            tot = (partunit * cavity);

                            var virginmatl = tot

                        }
                        else {

                            var runnerweight_Percent = ((runnerweight / (partunit * cavity)) * 100);

                            tot = (partunit * cavity);

                            var recycleratio = document.getElementById("txt~RecycleMaterialRatio(%)0").value;
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
                        document.getElementById("txtMaterialGrossWeight/pc(g)0").value = parseFloat(CSgross_weight.toFixed(6));
                        document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(CSMaterialCost.toFixed(6));


                    }

                    else if (varProcessGroup == "Layout1") {// Layout1 - IM
                        var totshotweight = (Number(tot) + Number(runnerweight));
                        var virginmatl = (totshotweight * ((100 - recycleratio) / 100));
                        var gross_shot_weight = (virginmatl * (100 + Number(matlyeildpercent)) / 100);
                        var gross = truncator(gross_shot_weight, 4).toString();
                        var gross_weight = (gross / cavity);

                        var CheckValgross_weight = gross_weight.toString().toUpperCase();
                        if (CheckValgross_weight == "NAN" || CheckValgross_weight == "INFINITY") {
                            gross_weight = 0;
                        }
                        document.getElementById("txtMaterialGrossWeight/pc(g)0").value = gross_weight;
                        var matl_Cost = (gross_weight * totrawmatl_Grams);
                        document.getElementById("txtMaterialGrossWeight/pc(g)0").value = parseFloat(gross_weight.toFixed(6));
                        document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(matl_Cost.toFixed(6));
                    }

                    else if (varProcessGroup == "Layout5") { // Layout5 - ST
                        // alert("IN");
                        var width = document.getElementById("txt~~Width(mm)" + i).value;
                        var thicknes = document.getElementById("txt~~Thickness(mm)" + i).value;
                        var pitch = document.getElementById("txt~~Pitch(mm)" + i).value;
                        var density = document.getElementById("txt~MaterialDensity" + i).value;

                        var scrapweight = document.getElementById("txtMaterialScrapWeight(g)" + i).value;
                        var scraploss = document.getElementById("txtScrapLossAllowance(%)" + i).value;
                        var scrapprice = document.getElementById("txtScrapPrice/kg" + i).value;

                        var cal1 = ((width * thicknes * pitch) / cavity);
                        var cal11 = parseFloat(cal1.toFixed(6));
                        var cal2 = (density / 1000);
                        var cal3 = ((100 + Number(matlyeildpercent)) / 100);
                        var cal33 = fixedDecml(cal3, 4).toString();
                        var gwFixed = (cal11 * cal2 * cal33);
                        var gros_weight = parseFloat(gwFixed.toFixed(6));

                        var CheckValgros_weight = gros_weight.toString().toUpperCase();
                        if (CheckValgros_weight == "NAN" || CheckValgros_weight == "INFINITY") {
                            gros_weight = 0;
                        }
                        document.getElementById("txtMaterialGrossWeight/pc(g)" + i).value = gros_weight;

                        //var scrap = fixedDecml(((100 - Number(scraploss)) / 100), 4).toString();
                        var scrap = parseFloat(((100 - Number(scraploss)) / 100).toFixed(6));

                        //var scrabweightpc = fixedDecml((scrapweight * scrap), 4).toString();
                        var scrabweightpc = parseFloat((scrapweight * scrap).toFixed(6));

                        var scraprebate = parseFloat((scrabweightpc * (scrapprice / 1000)).toFixed(6));
                        //var scraprebate = parseFloat((scrabweightpc * (scrapprice / 1000)).toFixed(6));

                        document.getElementById("txtScrapRebate/pcs" + i).value = scraprebate;

                        //var matlcost1 = fixedDecml((gros_weight * rawmatl_Grams),4).toString();
                        var matlcost1 = parseFloat((gros_weight * rawmatl_Grams).toFixed(6));

                        //var matlcost2 = fixedDecml((gros_weight * rawmatl_Grams) + (-scraprebate), 4).toString();
                        //var matlcost2 = parseFloat((gros_weight * rawmatl_Grams) + (-scraprebate).toFixed(6));
                        var matlcost2 = (gros_weight * rawmatl_Grams) - (scraprebate);

                        document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(matlcost2.toFixed(6));
                    }

                    else if (varProcessGroup == "Layout4" || varProcessGroup == "Layout6") {  // Layout4 - MS , Layout6 - SPR
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
                        document.getElementById("txtMaterialGrossWeight/pc(g)0").value = parseFloat(CSgross_weight.toFixed(6));

                        var CSMaterialCost = CSgross_weight * totrawmatl_Grams;
                        document.getElementById("txtMaterialCost/pcs" + i).value = parseFloat(CSMaterialCost.toFixed(6));
                        //end subash

                    }

                    var txtScrapPrice = parseFloat($('#txtScrapPrice\\/kg' + i).val());
                    var txtTotalRawMaterialCost = parseFloat($('#txtRawMaterialCost\\/kg' + i).val());
                    if (txtScrapPrice > txtTotalRawMaterialCost) {
                        alert('Scrap Price/kg cannot more than Raw Material Cost/kg');
                        $('#txtScrapPrice\\/kg' + i).val("");
                        $('#txtMaterialCost\\/pcs' + i).val("");
                        $('#txtScrapRebate\\/pcs' + i).val("");
                        $('#txtScrapPrice\\/kg' + i).focus();
                        Validate = false;
                    }
                }
            }

            if (Validate == true) {
                var totalmatlcal = 0;
                for (var i = 0; i < CellsCount - 1; i++) {
                    var ttlmtlcost = document.getElementById("txtMaterialCost/pcs" + i).value;

                    totalmatlcal = ((+totalmatlcal) + (+ttlmtlcost));

                }
                //var newtotalmtl = truncator(totalmatlcal, 4).toString();
                var CheckValTot = totalmatlcal.toString().toUpperCase();
                if (CheckValTot == "NAN" || CheckValTot == "INFINITY")
                {
                    totalmatlcal = 0;
                }
                document.getElementById("txtTotalMaterialCost/pcs0").value = parseFloat(totalmatlcal.toFixed(4));
                document.getElementById("txtTotalMaterialCost/pcs-0").value = parseFloat(totalmatlcal.toFixed(4));
                MCDataStore();

                ReCalculate();
                //*
            }
            return false;
        }

        //Session Maintain for Material Cost Table
        function MCDataStore() {
            var hdnvaltemp = "";

            var hdnvaltempST = "";

            var CellsCount = $("#Table1").find('tr')[0].cells.length;
            var varProcessGroup = $("#hdnLayoutScreen").val();

            for (var i = 0; i < CellsCount - 1; i++) {
                if (varProcessGroup == "Layout2") {
                    var txtSapcode = document.getElementById("txtMaterialSAPCode" + i).value;
                    var cavity = document.getElementById("txtCavity" + 0).value;
                    var partunit = document.getElementById("txtPartNetUnitWeight(g)" + 0).value;
                    var txtMcost = document.getElementById("txtMaterialCost/pcs" + i).value;
                    var txtTMcost = document.getElementById("txtTotalMaterialCost/pcs0").value;
                    hdnvaltemp += (+ "," + txtSapcode + "," + partunit + "," + cavity + "," + txtMcost + "," + txtTMcost + ",");
                }
                else
                {
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

                    hdnvaltemp += (+ "," + txtSapcode + "," + txtSapDesc + "," + txtrawmaterial + "," + txttotalRawMaterial + "," + partunit + ",");

                    //alert(varProcessGroup);
                    if (varProcessGroup == "Layout1") { // Layout1 - IM

                        var runnerweight = document.getElementById("txt~RunnerWeight/shot(g)0").value;
                        var txtRunnerRatio = document.getElementById("txt~RunnerRatio/pcs(%)0").value;
                        var recycleratio = document.getElementById("txt~RecycleMaterialRatio(%)0").value;

                        hdnvaltemp += (runnerweight + "," + txtRunnerRatio + "," + recycleratio + ",");

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
                    if (varProcessGroup != "Layout5")
                        hdnvaltemp += (cavity + "," + matlyeildpercent + "," + txtSTmgweight + "," + txtMcost + "," + txtTMcost + ",");
                }
                
                $("#hdnMCTableCount").val(i);
            }

            // alert(hdnvaltemp);
            $("#hdnMCTableValues").val(hdnvaltemp);
            return false;
        }

        function truncator(numToTruncate, intDecimalPlaces) {
            // alert(numToTruncate, intDecimalPlaces);
            var numPower = Math.pow(10, intDecimalPlaces); // "numPowerConverter" might be better
            // alert(numPower);
            return ~ ~(numToTruncate * numPower) / numPower;
        }

        // Calculate Other Cost Table
        function othercost() {

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
            document.getElementById("txtTotalOtherItemCost/pcs0").value = parseFloat(totalothercal.toFixed(4));
            document.getElementById("txtTotalOtherItemCost/pcs-0").value = parseFloat(totalothercal.toFixed(4));

            var ttlmtlcostFinal = document.getElementById("txtTotalMaterialCost/pcs0").value;
            var ttlprocostFinal = document.getElementById("txtTotalProcessesCost/pcs0").value;
            var ttlSubcostFinal = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;
            var ttlOthercostFinal = document.getElementById("txtTotalOtherItemCost/pcs0").value;

            document.getElementById("txtTotalMaterialCost/pcs0").value = ttlmtlcostFinal;

            document.getElementById("txtTotalOtherItemCost/pcs0").value = ttlOthercostFinal;

            var GrandTotalCost = ((+ttlmtlcostFinal) + (+ttlprocostFinal) + (+ttlSubcostFinal) + (+ttlOthercostFinal));

            var GrandTotalCostFinal = parseFloat(GrandTotalCost.toFixed(6));
            
            //var ttlProfit = document.getElementById("txtProfit(%)0").value;
            //var ttlDiscount = document.getElementById("txtDiscount(%)0").value;

            //var ttlFinalProfit = GrandTotalCost * ((100 + ttlProfit) / 100)
            //var ttlFinalDiscount = GrandTotalCost * ((100 - ttlDiscount) / 100)


            //var ttlfinalprofittrun = parseFloat(ttlFinalProfit.toFixed(4));
            //var ttlfinaldiscounttrun = parseFloat(ttlFinalDiscount.toFixed(4));

            $("#hdnTMatCost").val(ttlmtlcostFinal);
            $("#hdnTSumMatCost").val(ttlSubcostFinal);
            $("#hdnTOtherCost").val(ttlOthercostFinal);
            $("#hdnTGTotal").val(GrandTotalCostFinal);

            OthersCostDataStore();
            ReCalculate();
        }

        //Session Maintain for Other Cost Table
        function OthersCostDataStore() {

            var CellsCount = $("#TableOthers").find('tr')[0].cells.length;

            var hdnvaltemp = "";

            for (var i = 0; i < CellsCount - 1; i++) {

                var txtOthersDesc = document.getElementById("txtItemsDescription" + i).value;
                var txtOthersCost = document.getElementById("txtOtherItemCost/pcs" + i).value;

                var txttotalcost = document.getElementById("txtTotalOtherItemCost/pcs0").value;


                hdnvaltemp += (+ "," + txtOthersDesc + "," + txtOthersCost + "," + txttotalcost + ",");

                $("#hdnOthersTableCount").val(i);
            }

            $("#hdnOtherValues").val(hdnvaltemp);
        }

    </script>

    <script type="text/javascript">
        function ReCalculate() {
            try
            {
                document.getElementById("txtTotalProcessesCost/pcs-0").value = document.getElementById("txtTotalProcessesCost/pcs0").value;
                document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;

                var TotMatCost = parseFloat(document.getElementById('txtTotalMaterialCost/pcs-0').value)
                var TotProCost = parseFloat(document.getElementById('txtTotalProcessesCost/pcs-0').value)
                var TotSubMatCost = parseFloat(document.getElementById('txtTotalSub-Mat/T&JCost/pcs-0').value)
                var TotOthrCost = parseFloat(document.getElementById('txtTotalOtherItemCost/pcs-0').value)
                var ValueAfterProforDisc = parseFloat(document.getElementById('txtFinalQuotePrice/pcs1').value)
                var hdnVendorType = $("#hdnTGTotal").val();
                var txtProfit = "";
                var txtDiscount = "";
                var GA = "";
                if (hdnVendorType != null || hdnVendorType != "" || hdnVendorType != "External") {
                    txtProfit = parseFloat(document.getElementById('txtProfit(%)0').value);
                    GA = parseFloat(document.getElementById('GA(%)0').value);
                    document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(TotProCost).toFixed(4);
                }
                else {
                    txtProfit = parseFloat(document.getElementById('txtProfit(%)0').value);
                    txtDiscount = parseFloat(document.getElementById('txtDiscount(%)0').value);

                    var textbox = document.getElementById('txtProfit(%)0');
                    var textbox2 = document.getElementById('txtDiscount(%)0');

                    if (txtProfit.toString().toUpperCase().includes("NAN") && txtDiscount.toString().toUpperCase().includes("NAN")) {
                        textbox.disabled = false;
                        textbox2.disabled = false;
                    }
                    else {
                        if (txtProfit.toString().replace("NaN", "").length > 0) {
                            textbox.disabled = false;
                            textbox2.disabled = true;
                        }
                        else {
                            textbox.disabled = true;
                            textbox2.disabled = false;
                        }
                    }
                    if (textbox.disabled == false && textbox2.disabled == false) {
                        var DiscProf = TotProCost + (TotProCost * (0 / 100));
                        var CheckValDiscProf = DiscProf.toString().toUpperCase();
                        if (CheckValDiscProf == "NAN" || CheckValDiscProf == "INFINITY") {
                            DiscProf = 0;
                        }
                        document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(DiscProf).toFixed(4);
                    }
                    else {
                        if (textbox.disabled) {
                            //final value when Discount change
                            if (txtDiscount.toString().length = 0) {
                                txtDiscount = 0;
                            }
                            var txtDiscount = parseFloat(document.getElementById('txtDiscount(%)0').value)
                            var WithDisc = TotProCost - (TotProCost * (txtDiscount / 100));

                            var CheckValWithDisc = WithDisc.toString().toUpperCase();
                            if (CheckValWithDisc == "NAN" || CheckValWithDisc == "INFINITY") {
                                WithDisc = 0;
                            }
                            document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(WithDisc).toFixed(4);
                        }
                        else {
                            //final value whit profit
                            if (txtProfit.toString().length = 0) {
                                txtProfit = 0;
                            }
                            var txtProfit = parseFloat(document.getElementById('txtProfit(%)0').value)
                            var WithProfit = TotProCost + (TotProCost * (txtProfit / 100));

                            var CheckValWithProfit = WithProfit.toString().toUpperCase();
                            if (CheckValWithProfit == "NAN" || CheckValWithProfit == "INFINITY") {
                                WithProfit = 0;
                            }
                            document.getElementById("txtFinalQuotePrice/pcs1").value = parseFloat(WithProfit).toFixed(4);
                        }
                    }
                }

                document.getElementById("txtFinalQuotePrice/pcs0").value = TotMatCost;
                var GtValue1 = (TotMatCost) + (parseFloat(document.getElementById('txtTotalProcessesCost/pcs-0').value)) + (parseFloat(document.getElementById('txtTotalSub-Mat/T&JCost/pcs-0').value)) + (parseFloat(document.getElementById('txtTotalOtherItemCost/pcs-0').value));
                var CheckValGtValue1 = GtValue1.toString().toUpperCase();
                if (CheckValGtValue1 == "NAN" || CheckValGtValue1 == "INFINITY") {
                    GtValue1 = 0;
                }
                document.getElementById("txtGrandTotalCost/pcs0").value = parseFloat(GtValue1).toFixed(4);
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
                document.getElementById("txtFinalQuotePrice/pcs4").value = parseFloat(SumValFinal).toFixed(4);

                var GtValue = "";
                var FinalValue = "";
                if (hdnVendorType != null || hdnVendorType != "" || hdnVendorType != "External") {
                    GtValue = parseFloat(document.getElementById("txtGrandTotalCost/pcs0").value);
                    FinalValue = parseFloat(document.getElementById("txtFinalQuotePrice/pcs4").value);
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
                }

                $("#hdnTMatCost").val(document.getElementById("txtFinalQuotePrice/pcs0").value);
                $("#hdnTProCost").val(document.getElementById("txtFinalQuotePrice/pcs1").value);
                $("#hdnTSumMatCost").val(document.getElementById("txtFinalQuotePrice/pcs2").value);
                $("#hdnTOtherCost").val(document.getElementById("txtFinalQuotePrice/pcs3").value);
                $("#hdnTGTotal").val(document.getElementById("txtGrandTotalCost/pcs0").value);
                $("#hdnProfit").val(txtProfit);
                $("#hdnDiscount").val(txtDiscount);
                $("#hdnTFinalQPrice").val(FinalValue);
            }
            catch(err)
            {
                alert(err + 'ReCalculate')
            }
        }

        function CalculationFinalCost()
        {
            try
            {
                var GtCost = $("#txtGrandTotalCost\\/pcs0").val();
                var GA = $("#GA\\(\\%\\)0").val();
                var Profit = $("txtProfit\\(\\%\\)0").val();
                var CostAfterGA = parseFloat(GtCost).toFixed(4) + (parseFloat(GtCost).toFixed(4) * parseFloat(GA).toFixed(2) / 100);
                var CostAfterProfit = parseFloat(CostAfterGA).toFixed(4) + (parseFloat(CostAfterGA).toFixed(4) * parseFloat(Profit).toFixed(2) / 100)
            }
            catch(err)
            {
                alert(err);
            }
        }
    </script>

    <script type="text/javascript">
        function ClickAddMaterial()
        {
            document.getElementById('btnAddColumns').click();
        }
        function ClickAddProces() {
            document.getElementById('btnaddProcessCost').click();
        }
        function DelMatDetail(ColumnNo, TtlField) {
            var Cfrm = confirm("Are you sure you want to delete this item?");
            if (Cfrm == true) {
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
                document.getElementById('BtnDelMaterial').click();
            }
        }

        function DelProces(ColumnNo, TtlField) {
            var Cfrm = confirm("Are you sure you want to delete this item?");
            if (Cfrm == true) {
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
        function DelSubMatCost(ColumnNo, TtlField) {
            var Cfrm = confirm("Are you sure you want to delete this item?");
            if (Cfrm == true) {
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
        function DelOthCost(ColumnNo, TtlField) {
            var Cfrm = confirm("Are you sure you want to delete this item?");
            if (Cfrm == true) {
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

        function ComntByVendorLght() {
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
        }

        function ProcGrpChange(IdNo) {
            var x = document.getElementById('dynamicddlProcess' + IdNo).selectedIndex;
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
        }

        function SubProcgroupChnge(IdNo) {
            
            $("#dynamicddlSubvendorname" + IdNo).prop('selectedIndex', 0);
            $('#dynamicddlSubvendorname' + IdNo).prop("disabled", false);

            //$("#dynamicddlMachineLabor" + IdNo).prop('selectedIndex', 0);
            $('#txtIfTurnkey-VendorName' + IdNo).prop("disabled", false);

            $('#dynamicddlMachineLabor' + IdNo).show();
            $('#ddlHideMachine' + IdNo).hide();
            $('#txtMachineId' + IdNo).hide();

            $('#dynamicddlMachine' + IdNo).show();
            $('#dynamicddlHideMachineLabor' + IdNo).hide();

            document.getElementById("txtTurnkeyCost/pc" + IdNo).value = "";
            document.getElementById("txtTurnkeyProfit" + IdNo).value = "";
            document.getElementById("txtProcessCost/pc" + IdNo).value = "";
            document.getElementById("txtTotalProcessesCost/pcs" + 0).value = "";

            GetProcUom(IdNo);
        }

        function StoreSubprocGroup(IdNo) {
            
            document.getElementById("dynamicddlSubProcess" + IdNo).options.length = 0;

            var x = document.getElementById("grdSubProcessGrphidden").rows.length;
            for (i = 1; i < x; i++) {
                var a = document.getElementById("grdSubProcessGrphidden").rows[i].cells[0].innerHTML;
                //mySelect.append(a.toString());
                $('#dynamicddlSubProcess' + IdNo).append($('<option></option>').val(a.toString()).html(a.toString()));
            }

            GetProcUom(IdNo);
        }

        function MachineChange(IdNo)
        {
            GetVndRate(IdNo);
            Baseqtyupdatebyuom(IdNo);
        }


        function DdlMachineLaborChange(IdNo)
        {
            try {
                //var selectedText = $(this).find("option:selected").text();
                //dynamicddlMachineLaborMethod(selectedText, 0);

                $("#txtStandardRate\\/HR" + IdNo).val("");
                $("#txtVendorRate" + IdNo).val("");
                $("#txtVendorRate" + IdNo).prop("disabled", false);

                var DdlMacLabIdx = document.getElementById('dynamicddlMachineLabor' + IdNo).selectedIndex;
                var vendorName = $("#lblVName").text();
                var SAPCode = $("#txtpartdesc").val();
                if (SAPCode.replace('-', '').trim().toString() == "") {
                    if (DdlMacLabIdx == 0) {
                        $("#txtMachineId" + IdNo).prop("disabled", false);
                        var ItemdynamicddlMachine = $('#dynamicddlMachine0 option').length;
                        if (ItemdynamicddlMachine == 0) {
                            //$("#txtStandardRate\\/HR" + IdNo).prop("disabled", false);
                        }
                        else {
                            //$("#txtStandardRate\\/HR" + IdNo).prop("disabled", true);
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
            catch (err)
            {
                alert(err);
            }
        }

        function GetProcUom(IdNo) {
            
            var SubProcGrp = document.getElementById('dynamicddlSubProcess' + IdNo).selectedIndex;
            if (SubProcGrp > -1) {
                var SubProcGroup = $('#dynamicddlSubProcess' + IdNo).val();
                $('#hdnSubProcGroup').val(SubProcGroup.toString());
                document.getElementById('BtnFndProcUom').click();
            }
        }

        function StoreProcUOM(IdNo) {
            var ProcUom = $("#hdnProcUOM").val();
            $('#txtProcessUOM' + IdNo).val(ProcUom);
            Baseqtyupdatebyuom(IdNo);
        }

        function StoreVnderMachineList(IdNo) {
            
            document.getElementById("dynamicddlMachine" + IdNo).options.length = 0;
            var x = document.getElementById("grdMachinelisthidden").rows.length;
            if (x > 1) {
                for (i = 1; i < x; i++) {
                    var a = document.getElementById("grdMachinelisthidden").rows[i].cells[0].innerHTML;
                    //mySelect.append(a.toString());
                    $('#dynamicddlMachine' + IdNo).append($('<option></option>').val(a.toString()).html(a.toString()));
                }
                GetVndRate(IdNo);
            }
            else
            {
                $("#dynamicddlMachineLabor" + IdNo).prop('selectedIndex', 1);

                $("#dynamicddlMachine" + IdNo).hide();
                $("#txtMachineId" + IdNo).show();
                $("#txtMachineId" + IdNo).prop("disabled", true);

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

        function GetVndRate(IdNo) {
            var MachSelIndx = document.getElementById('dynamicddlMachine' + IdNo).selectedIndex;
            if (MachSelIndx > -1) {
                var ProcGroup = $('#dynamicddlProcess' + IdNo).val();
                var ArrProcGroup = ProcGroup.split('-');
                var procGrpCode = ArrProcGroup[0].toString().trim();

                var MacIdDdl = $('#dynamicddlMachine' + IdNo).val();
                var ArrMacId = MacIdDdl.split('-');
                var MacId = ArrMacId[0].toString().trim();

                $('#hdnProcGroup').val(procGrpCode.toString());
                //$('#hdnMachineId').val(MacId.toString());
                $('#hdnMachineId').val(MacIdDdl);
                $('#hdnColumTblProcNo').val(IdNo);
                document.getElementById('BtnFndVndRate').click();
            }
        }
        
        function StoreVndRate(IdNo) {
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
            if (x > -1 && y > -1)
            {
                GetProcUom(IdNo);
            }
        }

        function SubVendorData(IdNo) {
            $('#txtMachineId' + IdNo).hide();
            var x = document.getElementById('dynamicddlSubvendorname' + IdNo).selectedIndex;
            if (x > 0) {
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
        }

        function PrcGrpVsStokes_Min(ColumTblProcNo, Stk_Min, efficiency) {
            var DurProcUom = parseInt(efficiency) / parseInt(Stk_Min);
            $('#txtDurationperProcessUOM\\(Sec\\)' + ColumTblProcNo).val(DurProcUom);
            $('#txtEfficiency\\/ProcessYield\\(\\%\\)' + ColumTblProcNo).val(efficiency);
            $('#txtProcessUOM' + ColumTblProcNo).val("STROKES/MIN" + "-" + Stk_Min);
        }

        function Validate(IdNo,txt)
        {
            if (txt == 'MATYIELD')
            {
                var txtMaterialYield = parseInt($('#txtMaterialYield\\/MeltingLoss\\(\\%\\)' + IdNo).val());
                if (txtMaterialYield > 100) {
                    alert("value cannot bigger than 100%");
                    $('#txtMaterialYield\\/MeltingLoss\\(\\%\\)' + IdNo).val("");
                }
            }
            else if (txt == 'SCRAPALLOWENCE')
            {
                var txtScrapLossAllowance = parseInt($('#txtScrapLossAllowance\\(\\%\\)' + IdNo).val());
                if (txtScrapLossAllowance > 100) {
                    alert("value cannot bigger than 100%");
                    $('#txtScrapLossAllowance\\(\\%\\)' + IdNo).val("");
                }
            }
        }

        function validateTotalValue()
        {
            //var totmatcost = $('#txtTotalMaterialCost\\/pcs0').val();
            //var totprocost = $('#txtTotalSub-Mat\\/T\\&JCost\\/pcs0').val();
            //var totsubmatcost = $('#txtTotalSub-Mat\\/T\\&JCost\\/pcs0').val();
            //var totothcost = $('#txtTotalOtherItemCost\\/pcs0').val();
            var totmatcost = document.getElementById('txtTotalMaterialCost/pcs0').value;
            var totprocost = document.getElementById('txtTotalProcessesCost/pcs0').value;
            var totsubmatcost = document.getElementById('txtTotalSub-Mat/T&JCost/pcs0').value;
            var totothcost = document.getElementById('txtTotalOtherItemCost/pcs0').value;
            var net = document.getElementById('txtNetProfit(%)0').value;
            if (totmatcost.toString().toUpperCase().includes("NAN") || totmatcost.toString().toUpperCase().includes("INFINITY") || totmatcost.toString()== "")
            {
                alert("No Material Cost Calculation, Please press Calculate Material Cost Button.. !!");
                return false;
            }
            else if (totprocost.toString().toUpperCase().includes("NAN") || totprocost.toString().toUpperCase().includes("INFINITY") || totprocost.toString()== "")
            {
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
            else if (net.toString().replace(" %","").toUpperCase().includes("NAN") || totothcost.toString().toUpperCase().includes("INFINITY")) {
                alert("No Other Cost Calculation, Please press Calculate Other Cost Button.. !!");
                return false;
            }
            return true;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
            {
                return false;
            }
            return true;
        }
    </script>

</head>
<body id="page-top">
    <form id="form1" runat="server">
        <div id="header">
        <asp:ScriptManager ID="scriptmanager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
            <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"></asp:ToolkitScriptManager>--%>
        </div>

        <div class="row">
            <div id="loading" class="col-sm-12" style="padding-top:200px;" >
                <img id="loading-image" src="images/loading.gif" alt="Loading..."/>
                <div class="col-sm-12" style="text-align:center; opacity:1;">
                    <asp:Label ID="lbLoading" runat="server" Text="Please Wait..." Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                </div>
            </div>
        </div>

        <div class="row text-center">
                <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="PanelHideShow" runat="server" >
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" >
                                <ProgressTemplate>
                                    <div id="loading" class="col-sm-12" style="padding-top:200px;">
                                    <img id="loading-image" src="images/loading.gif" alt="Loading..."/>
                                    <div class="col-sm-12" style="text-align:center; opacity:1;">
                                        <asp:Label ID="lbLoading0" runat="server" Text="Please Wait..." Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                                    </div>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
     
        <!-- Header -->
        <div class="container-fluid">
            <div class="col-lg-12" style="padding:5px;">
            <div class="row">
                <div class="col-sm-10" style="padding-top:5px;">
                    <a onclick="ShowLoading();" href="Vendor.aspx"><asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                    <button class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" href="#"><i class="fas fa-bars"></i></button>
                    <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                    <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-2 fa-pull-right" style="background-color:#E9ECEF;">
                    <asp:Label ID="lblUser"  runat="server" Width="147px"></asp:Label><br />
                    <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
                    <a href="login.aspx">Logout</a>
                </div>
            </div>
            </div>
        </div>

        <div id="wrapper">
            <!-- Sidebar -->
            <ul class="sidebar navbar-nav">
            <li class="nav-item active">
              <a class="nav-link" onclick="ShowLoading();" href="Emet_author_V.aspx?num=15">
                <i class="fas fa-fw fa-tachometer-alt"></i>
                <span>Home</span>
              </a>
            </li>
            <li class="nav-item active">
              <a class="nav-link" onclick="ShowLoading();" href="Emet_author_V.aspx?num=16">
                <i class="fas fa-fw fa-table"></i>
                <span>Master Data</span>
              </a>
            </li>
            <li class="nav-item active">
              <a class="nav-link" onclick="ShowLoading();" href="Emet_author_V.aspx?num=21">
                <i class="fas fa-fw fa-key"></i>
                <span>Change Password</span>
              </a>
            </li>
            <!-- <li class="nav-item active">
                  <a class="nav-link" onclick="ShowLoading();" href="aboutemet.aspx?">
                    <i class="fas  fa-fw fa-info"></i>
                    <span> About</span>
                  </a>
                </li> -->
          </ul>

            <div id="content-wrapper">
                <div class="container-fluid">
                    <!-- Breadcrumbs-->
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item"><a href="#">New</a> </li>
                    </ol>
                    <!-- Icon Cards-->
                    <div class="row">
                    </div>
                    <!-- Area Chart Example-->
                    <div class="card mb-3">
                        <%-- <div class="card-header">
                            <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION
                        </div>--%>
                        <div class="card-body">
                            <!--<canvas id="myAreaChart" width="100%" height="30"></canvas>-->
                            <%--  <table class="stylebody" >--%>                            
                            <%--button reset--%>
                            <div class="col-md-12" style="background-color:rgba(0,0,0,.03);padding-top:10px;">
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="Login-button" OnClick="btnReset_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="Login-button" PostBackUrl="Request_Waiting_vendor.aspx"  />
		                    </div>

                            <%--entrydata--%>
                            <div class="col-md-12" style="background-color:rgba(0,0,0,.03);">
                                <div class="row" style="padding-bottom:10px; padding-top:10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label10" runat="server" Text="VENDOR DETAIL" ></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px">
                                    <div class="col-md-3">
                                        <asp:Label ID="lblreqst" runat="server" Text="Quote No:"></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdnQuoteNo" />
                                    </div>
                                    <div class="col-md-5">
                                        <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lblVName" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblCurrency" runat="server" Text=""></asp:Label>
                                        
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label26" runat="server" Text="SHIMANO DETAIL" ></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label17" runat="server"  ForeColor="Black" Text="SMN PIC"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtsmnpic" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_addres" runat="server"  ForeColor="Black" Text="Email"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtemail" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_cntact" runat="server"  ForeColor="Black" Text="Plant & Department"></asp:Label>
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
                                                 <asp:Label ID="Labelquote" runat="server" ForeColor="Black" Text="Vendor Response Due Date:"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtquotationDueDate" runat="server" ForeColor="Black" BackColor="#E6E6E6" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label2" runat="server" Text="PART I: QUOTED PART INFO"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_cntact0" runat="server"  
                                                    ForeColor="Black" Text="Product"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtprod" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_partdesc" runat="server"  ForeColor="Black" Text="Part Code & Desc"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtpartdesc" Enabled="false" runat="server" Height="55px" 
                                                    TextMode="MultiLine"  ForeColor="Black" Width="100%" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_cntact4" runat="server"  ForeColor="Black" Text="SAP PIR JOB TYPE & Desc"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtSAPJobType" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_PIR" runat="server"  ForeColor="Black" Text="PIR Type & Desc"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtPIRtype" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_partDRG" runat="server"  ForeColor="Black" Text="PART DRG"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtdrawng" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_proces" runat="server"  ForeColor="Black" Text="Proces Group"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtprocs" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="txtPartUnit" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label27" runat="server"  ForeColor="Black" Text="BaseUOM: "></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtBaseUOM" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>



                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label11" runat="server" Text="Net Weight:" 
                                            ForeColor="Black"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:UpdatePanel ID="UpdatePanel111" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="txtunitweight"  Enabled="false"  runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off" ></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="txtUOM"  Enabled="false"  runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off" ></asp:TextBox>
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

                                <div class="row" style="padding-bottom:10px;">
                                      <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label20" runat="server" Text="Remarks(Max 250 char)"  Enabled="false"
                                            ForeColor="Black"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                    <ContentTemplate>
                                                       <asp:TextBox ID="txtRem"  Enabled="false"  runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
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
                                                                <asp:TextBox ID="txtMQty"  Enabled="false"  runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off" ></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6">
                                                               <asp:TextBox ID="txtBaseUOM1"  Enabled="false"  runat="server" AutoPostBack="true"  Width="100%" AutoCompleteType="Disabled" autocomplete="off" ></asp:TextBox>
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

                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12" style="padding-bottom:5px;">
                                                <div class="col-md-12 Padding-Nol" style="border-bottom:1px solid blue">
                                                    <asp:Label ID="lbl_proces0" runat="server"  ForeColor="Black" Text="SMN BOM & Material Cost details"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-12 Padding-Nol">
                                                <div class="table table-responsive table-sm">
                                                    <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="false" 
                                                            ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" 
                                                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Width="100%">
                                                            <Columns>
                                                                <asp:BoundField DataField="material" HeaderText="Material" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MaterialDesc" HeaderText="MaterialDesc" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Selling_Crcy" HeaderText="Selling_Crcy" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>

                                                                  <asp:BoundField DataField="OAmount" HeaderText="Amt_SCur" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                  
                                                                <asp:BoundField DataField="Venor_Crcy" HeaderText="Vendor_Crcy" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Amount" HeaderText="Amt_VCur" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#006699"  ForeColor="White" />
                                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                        <RowStyle ForeColor="#000066" />
                                                        <SelectedRowStyle BackColor="#669999"  ForeColor="White" />
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

                                 <div class="row" style="padding-bottom:10px">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label14" runat="server" Text="To Be Filled By Vendor"></asp:Label>
                                           
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <asp:Label ID="Label12" runat="server"  ForeColor="Black" Text="SMN Quote Effective Date"></asp:Label>
                                            </div>
                                            <div class="col-lg-7">
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <asp:TextBox ID="TextBox1" OnclientClick="return false;" 
                                                            onkeydown="javascript:preventInput(event);" 
                                                            autocomplete="off" AutoCompleteType="Disabled"
                                                            runat="server"  ForeColor="Black" >
                                                        </asp:TextBox>
                                                    </div>
                                                    <span class="SearchBox-btn" style="background-color:#E9ECEF;">
                                                        <i class="fa fa-calendar" style="color:#005496;" onclick="javascript: $('#TextBox1').focus();" ></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <asp:Label ID="Label13" runat="server"  ForeColor="Black" Text="SMN Due Date for Next Revision"></asp:Label>
                                            </div>
                                            <div class="col-lg-7">
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <asp:TextBox ID="txtfinal" OnclientClick="return false;" runat="server" 
                                                            onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" >
                                                        </asp:TextBox>
                                                    </div>
                                                    <span class="SearchBox-btn" style="background-color:#E9ECEF;">
                                                        <i class="fa fa-calendar" style="color:#005496;" onclick="javascript: $('#txtfinal').focus();" ></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                  <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-lg-5">
                                               <asp:Label ID="Label28" runat="server"  ForeColor="Black" Text="Country Of Origin"></asp:Label> 
                                            </div>
                                            <div class="col-lg-7">
                                                <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                <ContentTemplate>
                                                <asp:DropDownList ID="ddlpirjtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlpirjtype_SelectedIndexChanged" Font-Bold="False" ForeColor="Black"  Width="100%" Height="30px">
                                                </asp:DropDownList>
                                                </ContentTemplate>
                                              </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                             
                                            </div>
                                            <div class="col-md-7">
                                              
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="row" style="padding-bottom:10px">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="lblmatlcost" runat="server" Text="PART II: Material Cost"></asp:Label>
                                            <asp:Label ID="Label1" runat="server" Text="Vendor Details" Font-Size="18px" ForeColor="#2153a5" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <%--button add and delete material--%>
                                <div class="row" style="padding-bottom:10px">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnAddColumns" runat="server" Text="Add Material" 
                                                        OnClientClick="MCDataStore();" OnClick="btnAddColumns_Click"
                                                        CssClass="Login-button" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddColumns" />
                                                </Triggers>
                                            </asp:UpdatePanel>

                                            <%--button delete material--%>
                                            <div style="display:none"> 
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="BtnDelMaterial" onkeypress="return false"  runat="server"
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
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="table table-responsive table-sm">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Table ID="Table1" runat="server" CssClass="table-bordered table-nowrap">
                                                    </asp:Table>
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
                                </div>
                                
                                <%--button calcultae mat cost--%>
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button2" runat="server" Text="Calculate Material Cost"
                                                OnClientClick="MatlCalculation(); return false" UseSubmitBehavior="false"
                                                CssClass="Login-button" OnClick="Button2_Click" />
                                    </div>
                                </div>

                                <%--label part III--%>
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label4" runat="server" Text="PART III: PROCESS COST"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <%--button add and delete process cost--%>
                                <div class="row" style="padding-bottom:10px">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnaddProcessCost" runat="server" Text="Add Process"
                                                    CssClass="Login-button" OnClientClick="ProcessCostDataStore()"  OnClick="btnaddProcessCost_Click" />
                                                <div style="display:none;">
                                                    <asp:Button ID="btnClickCalcPrcUOMStkMin" runat="server" OnClick="btnClickCalcPrcUOMStkMin_Click"/>
                                                    <asp:Button ID="BtnFndVndMachine" runat="server" OnClick="BtnFndVndMachine_Click"/>
                                                    <asp:Button ID="BtnFndVndMachineVsProcUom" runat="server" OnClick="BtnFndVndMachineVsProcUom_Click"/>
                                                    <asp:Button ID="BtnFndProcUom" runat="server" OnClick="BtnFndProcUom_Click"/>
                                                    <asp:Button ID="BtnFndVndRate" runat="server" OnClick="BtnFndVndRate_Click"/>
                                                    <asp:Button ID="BtnMacList" runat="server" OnClick="BtnMacList_Click"/>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnaddProcessCost" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        <%--button delete process--%>
                                            <div style="display:none"> 
                                            <asp:UpdatePanel ID="UpdatePanel13" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="BtnDelProcess" onkeypress="return false"  runat="server"
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
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="table table-responsive table-sm">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Table ID="TablePC" runat="server" CssClass="table-bordered table-nowrap">
                                                    </asp:Table>

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
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button6" runat="server" Text="Calculate Process Cost" OnClientClick="processcost();return false"
                                            CssClass="Login-button" UseSubmitBehavior="False" />
                                    </div>
                                </div>

                                <%--label part IV--%>
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label3" runat="server" Text="PART IV: SUB-MAT/T&amp;J COST"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <%--button add and delete SUB-MAT/T&J COST--%>
                                <div class="row" style="padding-bottom:10px">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAddSubProcessCost" runat="server" Text="Add SUB-MAT Cost"
                                                    CssClass="Login-button" OnClientClick="submatlCostDataStore()"  OnClick="btnAddSubProcessCost_Click" />

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAddSubProcessCost" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        <%--btndel submat cost--%>
                                        <div style="display:none"> 
                                            <asp:UpdatePanel ID="UpdatePanel16" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="BtnDelSubMatCost" onkeypress="return false"  runat="server"
                                                        Text="Delete Material" OnClick="BtnDelSubMatCost_Click" CssClass="Login-button" />
                                                    <asp:Label ID="Label9" runat="server" Text="1"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BtnDelSubMatCost" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                       </div>
                                    </div>
                                </div>

                                <%--table SUB-MAT/T&J COST--%>
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="table table-responsive table-sm">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                    <asp:Table ID="TableSMC" runat="server" CssClass="table-bordered table-nowrap">
                                                    </asp:Table>
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
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button4" runat="server" Text="Calculate Sub Material Cost"
                                            OnClientClick="submatlcost();return false" UseSubmitBehavior="false"
                                            CssClass="Login-button" />
                                    </div>
                                </div>

                                <%--label part V--%>
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label6" runat="server" Text="PART V: OTHER COST"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <%--button add and delete OTHER COST--%>
                                <div class="row" style="padding-bottom:10px">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAddOtherCost" runat="server" Text="Add Others"
                                                    CssClass="Login-button" OnClientClick="OthersCostDataStore()"  OnClick="btnAddOtherCost_Click" />

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAddOtherCost" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        <%--button delete other--%>
                                            <div style="display:none"> 
                                            <asp:UpdatePanel ID="UpdatePanel15" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="BtnDelOthCost" onkeypress="return false"  runat="server"
                                                        Text="Delete Other Cost" OnClick="BtnDelOthCost_Click" CssClass="Login-button table-nowrap" />
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
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="table table-responsive table-sm">
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>

                                                    <asp:Table ID="TableOthers" runat="server" CssClass="table-bordered table-nowrap">
                                                    </asp:Table>

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
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button5" runat="server" Text="Calculate Other Cost"
                                            OnClientClick="othercost(); return false"
                                            CssClass="Login-button" UseSubmitBehavior="false" />
                                    </div>
                                </div>

                                <%--label part VI PART UNIT PRICE--%>
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label7" runat="server" Text="PART VI: PART UNIT PRICE"></asp:Label>
                                            <asp:Label ID="lblcry" runat="server" ForeColor="Yellow"></asp:Label>
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnAddPart" Visible="false" runat="server" Text="Add PART UNIT"
                                                        CssClass="Login-button" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddPart" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                                <%--table PART UNIT PRICE--%>
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="table table-responsive table-sm">
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Table ID="TableUnit" runat="server" CssClass="table-bordered table-nowrap">
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
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:Label ID="lbComnt" runat="server" Text ="Comment By Vendor"></asp:Label>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="TxtComntByVendor" Enabled="true" runat="server" Height="55px" CssClass="form-control" onkeyup="ComntByVendorLght()"
                                                     TextMode="MultiLine" Width="100%" placeholder="Max 150 character" ></asp:TextBox>
                                                <div style="text-align:right"><asp:Label ID="LblengtVC" runat="server" Text ="150 character left"></asp:Label></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%--button submit and save as draft--%>
                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnSaveDraft" runat="server" Text="Save As Draft" 
                                                    CssClass="Login-button" OnClick="BtnSaveDraft_Click" OnClientClick="ReCalculate();if(validateTotalValue()==false) return false;" />
                                                <asp:Button ID="Button1" runat="server" Text="Submit" CssClass="Login-button" OnClientClick="ReCalculate();if(validateTotalValue()==false) return false;" OnClick="Button1_Click1" />
                                                <asp:Label ID="lblcreateuser" runat="server" Visible="False"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="Button1" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                            </div>

                            <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="grdVendrDet" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="REQUESTDATE" HeaderText="Request Date" />
                                                <asp:BoundField DataField="QUOTENO" HeaderText="Quote No" />
                                                <asp:BoundField DataField="Description" HeaderText="Vendor Name" />
                                                <asp:BoundField DataField="Crcy" HeaderText=" Quote Currency" />
                                                <asp:BoundField DataField="PICName" HeaderText="PIC Name" />
                                                <asp:BoundField DataField="PICemail" HeaderText="PIC Email" />
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                        </asp:GridView>

                                        <asp:GridView ID="grdProcessGrphidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                            Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                 
                                                <asp:BoundField DataField="ProcessGrpCode" HeaderText="Process Grp Code" />
                                                <asp:BoundField DataField="SubProcessName" HeaderText="Sub Process Name" />
                                                <asp:BoundField DataField="ProcessUomDescription" HeaderText="Process UOM Description" />
                                                <asp:BoundField DataField="ProcessUOM" HeaderText="Process UOM" />
                                                 <asp:BoundField DataField="ProcessGrpCode" HeaderText="ProcessGroup" />

                                                

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                        </asp:GridView>

                                        <asp:UpdatePanel runat="server" ID="UpgrdSubProcessGrphidden">
                                        <ContentTemplate>
                                        <asp:GridView ID="grdSubProcessGrphidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                            Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="SubProcessName" HeaderText="Sub Process Name" />
                                                <asp:BoundField DataField="ProcessUomDescription" HeaderText="Process UOM Description" />
                                                <asp:BoundField DataField="ProcessUOM" HeaderText="Process UOM" />
                                                 <asp:BoundField DataField="ProcessGrpCode" HeaderText="ProcessGroup" />
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                        </asp:GridView>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <div style="display:none">
                                        <asp:UpdatePanel runat="server" ID="UpgrdMachinelisthidden">
                                            <ContentTemplate>
                                        <asp:GridView ID="grdMachinelisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                            ShowHeaderWhenEmpty="true" Style="color: #333333; border-collapse: collapse; ">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Machine" HeaderText="Machine" />
                                                <asp:BoundField DataField="SMNStdrateHr" HeaderText="SMNStdrateHr" />
                                                <asp:BoundField DataField="FollowStdRate" HeaderText="FollowStdRate" />
                                                <asp:BoundField DataField="Currency" HeaderText="Currency" />
                                                <asp:BoundField DataField="ProcessGrp" HeaderText="ProcessGroup" />

                                            </Columns>

                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                        </asp:GridView>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                        </div>
                                        <asp:GridView ID="grdLaborlisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                            Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                                            <Columns>
                                                <asp:BoundField DataField="StdLabourRateHr" HeaderText="StdLabourRateHr" />
                                                <asp:BoundField DataField="FollowStdRate" HeaderText="FollowStdRate" />
                                                <asp:BoundField DataField="Currency" HeaderText="Currency" />

                                            </Columns>

                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
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

                             
                            <asp:HiddenField ID="hdnLayoutScreen" runat="server" Value="" />
                            <asp:HiddenField ID="hdnHidenProfit" runat="server" Value="" />

                            <asp:HiddenField ID="hdnColumTblProcNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnProcGroup" runat="server" Value="" />
                            <asp:HiddenField ID="hdnSubProcGroup" runat="server" Value="" />

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
                            <asp:HiddenField ID="hdnVendorType" runat="server" Value="" />
                        </div>
                    </div>
                </div>
                <!-- /.content-wrapper -->
            </div>
        </div>
        
        <!-- Footer -->
        <div class="container-fluid" style="background-color:#F5F5F5">
            <div class="row">
                <div class="col-lg-12" style="padding:5px; align-content:center; text-align:center">
                    <span style="font:bold 13px calibri, calibri">Copyright © ShimanoDT 2018</span>
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
                    <div class="modal-header" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5);border-bottom-left-radius: 15px;border-bottom-right-radius: 15px;">
                        <div class="col-lg-12 Padding-Nol" style="font:bold 22px calibri, calibri; text-align:center; align-content:center;"> Your Session Is About To Expire !!  </div>
                      <h4></h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="row">
                                    <div class="col-xs-2" style="padding:10px">
                                        <asp:Image ID="ImagWarning" runat="server" class="responsive" ImageUrl="~/js/jsextendsession/images/timeout-icon.png"/>
                                    </div>
                                    <div class="col-xs-10" style="padding:10px">
                                        <asp:Timer ID="TimerCntDown" runat="server" Interval="1000" OnTick="TimerCntDown_Tick" Enabled="false"></asp:Timer>
                                        You will be logged out in : <asp:Label ID="countdown" runat="server" Font-Bold="true" ForeColor="Red" Text="30"></asp:Label> seconds<br />
                                        do u want to stay Sign In?
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5);border-top-left-radius: 15px;border-top-right-radius: 15px;">
                        <asp:Button ID="BtnRefresh" runat="server" Text="Yes, Keep me Sign In" OnClick="BtnRefresh_Click" CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="18px"/>
                        <asp:Button ID="CtnCloseMdl" runat="server" Text="No, Sign Me Out" OnClick="CtnCloseMdl_Click" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="18px"/>
                        <div style="display:none;"><asp:Button ID="StartTimer" runat="server" Text="Start" OnClick="StartTimer_Click" CssClass="btn btn-sm btn-primary" /></div>
                    </div>
                </div>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>

    <%--script loading page--%>
        <script language="javascript" type="text/javascript">
            $(window).load(function () {
                $('#loading').fadeOut("fast");
          });
        </script>
        <script type="text/javascript">
            function ShowLoading() {
                $('#loading').show();
            }
            function CloseLoading() {
                $('#loading').fadeOut("fast");
            }
        </script>

        <%--script open modal--%>
        <script type="text/javascript">
                function OpenModalSession() {
                    $("#myModalSession").modal({
                    backdrop: 'static',
                    keyboard: false
                });
                }

                function DatePitcker() {
                    $('#TextBox1').datepicker({
                        buttonImageOnly: true,
                        dateFormat: "dd/mm/yy"
                    });

                    $('#txtfinal').datepicker({
                        buttonImageOnly: true,
                        dateFormat: "dd/mm/yy"
                    });
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
</body>
</html>
