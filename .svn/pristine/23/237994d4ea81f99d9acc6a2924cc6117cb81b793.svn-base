<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VViewRequest.aspx.cs" Inherits="Material_Evaluation.VViewRequest" %>


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

    <%--script loading page--%>
    <script language="javascript" type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });
    </script>


    <script type="text/javascript">
        function MassRevForBOMWhenHaveQuteRef() {
            try {
                if ($("#HdnStatus").val() == "") {
                    var CellsCount = $("#Table1").find('tr')[0].cells.length;
                    var varProcessGroup = $("#hdnLayoutScreen").val();
                    var varhdnQuoteNoRef = $("#hdnQuoteNoRefmassRev").val();

                    if ((varProcessGroup != "Layout2") || (varProcessGroup != "Layout7")) {
                        var rawmaterialCode = "";
                        var rawmaterialDesc = "";
                        var rawmaterialCost = "";
                        var TotrawmaterialCost = "";
                        var rawmaterialUOM = "";
                        var rawmaterialUOMTot = "";
                        var BOMData = document.getElementById("GvSMNBomEffctvDate");
                        var matCostTbale = document.getElementById("Table1");
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
                                    if (rawmaterialUOM.toUpperCase() == "KG") {
                                        TotrawmaterialCost = parseFloat(rawmaterialCost / 1000).toFixed(4);
                                    }
                                    else {
                                        TotrawmaterialCost = rawmaterialCost;
                                    }

                                    document.getElementById("Table1").rows[1].cells[i + 1].innerHTML = rawmaterialCode;
                                    document.getElementById("Table1").rows[2].cells[i + 1].innerHTML = rawmaterialDesc
                                    document.getElementById("Table1").rows[3].cells[i + 1].innerHTML = rawmaterialCost + ' / ' + rawmaterialUOM;
                                    document.getElementById("Table1").rows[4].cells[i + 1].innerHTML = TotrawmaterialCost + ' / ' + rawmaterialUOMTot;
                                }
                            }
                        }

                        if (varhdnQuoteNoRef.toString() != "") {
                            if (matCostTbale != null) {
                                var rowC = $('#Table1 tr').length;
                                for (var i = 1; i < CellsCount; i++) {
                                    document.getElementById("Table1").rows[rowC - 1].cells[1].innerHTML = "";
                                    document.getElementById("Table1").rows[rowC - 2].cells[i].innerHTML = "";
                                }
                            }
                        }
                    }
                }
            } catch (err) {
                alert("RevisionOfEmetConditionForBOM : " + err)
            }
        }

        //page related : quote qost plant - vviewrequestmass  - vviewrequest
        function CekValueInputByUser() {
            try {
                
                var Layout = $("#hdnLayoutScreen").val();

                var TabBOM = document.getElementById("GvSMNBomEffctvDate");
                var rowscountTabBOM = 0;
                if (TabBOM != null) {
                    rowscountTabBOM = TabBOM.rows.length;
                }

                var TabMatCost = document.getElementById("Table1");
                var rowscountTabMatCost = TabMatCost.rows.length;
                var CellsCountTabMatCost = $("#Table1").find('tr')[0].cells.length;
                var ValHdnAcsTabMatCost = HdnAcsTabMatCost.value;

                var TabProcCost = document.getElementById("TablePC");
                var rowscountTabProCost = TabProcCost.rows.length;
                var CellsCountTabProcCost = $("#TablePC").find('tr')[0].cells.length;
                var ValHdnAcsTabProcCost = HdnAcsTabProCost.value;

                var TabSubMatCost = document.getElementById("TableSMC");
                var rowscountTabSubMatCost = TabSubMatCost.rows.length;
                var CellsCountTabSubMatCost = $("#TableSMC").find('tr')[0].cells.length;
                var ValHdnAcsTabSubMatCost = HdnAcsTabSubMatCost.value;

                var TabOthCost = document.getElementById("TableOthers");
                var rowscountTabOthCost = TabOthCost.rows.length;
                var CellsCountTabOthCost = $("#TableOthers").find('tr')[0].cells.length;
                var ValHdnAcsTabOthCost = HdnAcsTabOthCost.value;

                var TabUnitCost = document.getElementById("TableUnit");
                var rowscountTabUnitCost = TabOthCost.rows.length;
                var CellsCountTabUnitCost = $("#TableUnit").find('tr')[0].cells.length;

                var ValSAPSpProcType = HdnSAPSpProcType.value;

                //table material cost
                if (ValHdnAcsTabMatCost == "True") {
                    if (Layout == "Layout1") {
                        if (rowscountTabBOM <= 1) {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML;
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Runner Weight/shot (g)" || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Material/Melting Loss (%)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    if (FieldValue != 5) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                            $(td).attr('title', "Default : 5");
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                var td = tr.cells[z];
                                                if (td != null) {
                                                    td.style.color = "blue";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Runner Weight/shot (g)" || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Material/Melting Loss (%)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    if (FieldValue != 5) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                            $(td).attr('title', "Default : 5");
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost") {

                                                    if (z > (rowscountTabBOM - 1)) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                                else {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Layout == "Layout2") {
                        if (rowscountTabBOM <= 1) {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Part Net Unit Weight (g)"
                                     || FieldName == "Base Qty / Cavity") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                var td = tr.cells[z];
                                                if (td != null) {
                                                    td.style.color = "blue";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Layout == "Layout3") {
                        if (rowscountTabBOM <= 1) {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Material/Melting Loss (%)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    if (FieldValue != 10) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                            $(td).attr('title', "Default : 10");
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                var td = tr.cells[z];
                                                if (td != null) {
                                                    td.style.color = "blue";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Material/Melting Loss (%)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    if (FieldValue != 10) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                            $(td).attr('title', "Default : 10");
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost") {

                                                    if (z > (rowscountTabBOM - 1)) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                                else {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Layout == "Layout4") {
                        if (rowscountTabBOM <= 1) {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                var td = tr.cells[z];
                                                if (td != null) {
                                                    td.style.color = "blue";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost") {

                                                    if (z > (rowscountTabBOM - 1)) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                                else {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Layout == "Layout5") {
                        if (rowscountTabBOM <= 1) {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                debugger;
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Thickness (mm)" || FieldName == "Width (mm)" || FieldName == "Pitch (mm)" || FieldName == "Material Density"
                                    || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)" || FieldName == "Scrap Loss Allowance (%)" || FieldName == "Scrap Price/kg") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Material/Melting Loss (%)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    if (FieldValue != 2) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                            $(td).attr('title', "Default : 2");
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Material Density") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    if (FieldValue != 7.86) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                            $(td).attr('title', "Default : 7.86");
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Thickness (mm)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Width (mm)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Pitch (mm)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Scrap Loss Allowance (%)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                            else {
                                                var td = tr.cells[z];
                                                if (td != null) {
                                                    td.style.color = "blue";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Thickness (mm)" || FieldName == "Width (mm)" || FieldName == "Pitch (mm)" || FieldName == "Material Density"
                                    || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)" || FieldName == "Scrap Loss Allowance (%)" || FieldName == "Scrap Price/kg") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Material/Melting Loss (%)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    if (FieldValue != 5) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                            $(td).attr('title', "Default : 5");
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Material Density") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    if (FieldValue != 7.86) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                            $(td).attr('title', "Default : 7.86");
                                                        }
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Thickness (mm)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Width (mm)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Pitch (mm)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                            else if (FieldName == "Scrap Loss Allowance (%)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                            else {
                                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost") {

                                                    if (z > (rowscountTabBOM - 1)) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                                else {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Layout == "Layout6") {
                        if (rowscountTabBOM <= 1) {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                var td = tr.cells[z];
                                                if (td != null) {
                                                    td.style.color = "blue";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost" || FieldName == "Part Net Unit Weight (g)"
                                    || FieldName == "Base Qty / Cavity" || FieldName == "Material/Melting Loss (%)") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Part Net Unit Weight (g)") {
                                                if (TabMatCost.rows[i].cells[z] != null) {
                                                    var FieldValue = TabMatCost.rows[i].cells[z].innerHTML;
                                                    var PartNetUnitWeight = $("#txtunitweight");
                                                    if (PartNetUnitWeight != null) {
                                                        var PartNetUnitWeightVal = PartNetUnitWeight.val();
                                                        if (PartNetUnitWeightVal != "") {
                                                            if (parseFloat(PartNetUnitWeightVal).toFixed(4) != FieldValue) {
                                                                var td = tr.cells[z];
                                                                if (td != null) {
                                                                    td.style.color = "blue";
                                                                    $(td).attr('title', "Default : " + parseFloat(PartNetUnitWeightVal).toFixed(4));
                                                                }
                                                            }
                                                        }
                                                        else {
                                                            var td = tr.cells[z];
                                                            if (td != null) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost") {

                                                    if (z > (rowscountTabBOM - 1)) {
                                                        var td = tr.cells[z];
                                                        if (td != null) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                                else {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Layout == "Layout7") {
                        if (rowscountTabBOM <= 1) {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            var td = tr.cells[z];
                                            if (td != null) {
                                                td.style.color = "blue";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            for (var i = 1; i < rowscountTabMatCost; i++) {
                                var FieldName = TabMatCost.rows[i].cells[0].innerHTML.trim();
                                if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost") {
                                    var tr = TabMatCost.getElementsByTagName("tr")[i];
                                    if (tr != null) {
                                        for (var z = 1; z < CellsCountTabMatCost; z++) {
                                            if (FieldName == "Raw Material SAP Code" || FieldName == "Raw Material Description" || FieldName == "Raw Material Cost") {
                                                if (z > (rowscountTabBOM - 1)) {
                                                    var td = tr.cells[z];
                                                    if (td != null) {
                                                        td.style.color = "blue";
                                                    }
                                                }
                                            }
                                            else {
                                                var td = tr.cells[z];
                                                if (td != null) {
                                                    td.style.color = "blue";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //table Sub material cost
                if (ValHdnAcsTabSubMatCost == "True") {
                    var ToolAmorCount = 0;
                    var MachineAmorCount = 0;
                    if ($("#HdnIsUseToolAmortize").val() != "") {
                        ToolAmorCount = $("#HdnIsUseToolAmortize").val();
                    }

                    if ($("#HdnIsUseMachineAmor").val() != "") {
                        MachineAmorCount = $("#HdnIsUseMachineAmor").val();
                    }
                    AmorCount = parseInt(ToolAmorCount) + parseInt(MachineAmorCount);

                    for (var i = 1; i < rowscountTabSubMatCost; i++) {
                        var FieldName = TabSubMatCost.rows[i].cells[0].innerHTML.trim().replace("&amp;", "&");
                        if (FieldName == "Sub-Mat/T&J Description" || FieldName == "Sub-Mat/T&J Cost" || FieldName == "Consumption (pc)") {
                            var tr = TabSubMatCost.getElementsByTagName("tr")[i];
                            if (tr != null) {
                                for (var z = 1; z < CellsCountTabSubMatCost; z++) {
                                    var td = tr.cells[z];
                                    if (td != null) {
                                        if (z > AmorCount) {
                                            td.style.color = "blue";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //table Others cost
                if (ValHdnAcsTabOthCost == "True") {
                    for (var i = 1; i < rowscountTabOthCost; i++) {
                        var FieldName = TabOthCost.rows[i].cells[0].innerHTML.trim();
                        if (FieldName == "Items Description" || FieldName == "Other Item Cost/pc" || FieldName == "Other Item Cost/UOM") {
                            var tr = TabOthCost.getElementsByTagName("tr")[i];
                            if (tr != null) {
                                for (var z = 1; z < CellsCountTabOthCost; z++) {
                                    var td = tr.cells[z];
                                    if (td != null) {
                                        td.style.color = "blue";
                                    }
                                }
                            }
                        }
                    }
                }

                //table Unit
                for (var i = 1; i < rowscountTabUnitCost; i++) {
                    var tr = TabUnitCost.getElementsByTagName("tr")[i];
                    if (tr != null) {
                        for (var z = 1; z < CellsCountTabUnitCost; z++) {
                            if (z == 2 || z == 3) {
                                var td = tr.cells[z];
                                if (td != null) {
                                    td.style.color = "blue";
                                }
                            }
                        }
                    }
                }

                //table Process cost
                if (ValHdnAcsTabProcCost == "True") {
                    var Subcon = "";
                    var Subvendor = "";

                    for (var i = 1; i < rowscountTabProCost; i++) {
                        var FieldName = TabProcCost.rows[i].cells[0].innerHTML.trim();
                        if (FieldName == "Process Grp Code" || FieldName == "Sub Process" || FieldName == "If Subcon - Subcon Name" || FieldName == "If Turnkey- Sub vendor name"
                            || FieldName == "Machine /Labor" || FieldName == "Machine" || FieldName == "Vendor Rate/HR"
                            || FieldName == "Base qty" || FieldName == "Duration per Process UOM (Sec)" || FieldName == "Efficiency"
                            || FieldName == "Turnkey Cost/pc" || FieldName == "Turnkey Fees" || FieldName == "Process Cost/pc") {
                            var tr = TabProcCost.getElementsByTagName("tr")[i];
                            if (tr != null) {
                                for (var z = 1; z < CellsCountTabProcCost; z++) {
                                    var td = tr.cells[z];
                                    if (td != null) {

                                        Subcon = TabProcCost.rows[3].cells[z].innerHTML;
                                        Subvendor = TabProcCost.rows[4].cells[z].innerHTML;

                                        if (FieldName == "Process Grp Code") {
                                            td.style.color = "blue";
                                        }
                                        else if (FieldName == "Sub Process") {
                                            td.style.color = "blue";
                                        }
                                        else if (FieldName == "If Subcon - Subcon Name") {
                                            //Subcon = TabProcCost.rows[i].cells[z].innerHTML;
                                            if (Subcon != "") {
                                                td.style.color = "blue";
                                            }
                                        }
                                        else if (FieldName == "If Turnkey- Sub vendor name") {
                                            //Subvendor = TabProcCost.rows[i].cells[z].innerHTML;
                                            if (Subvendor != "") {
                                                td.style.color = "blue";
                                            }
                                        }
                                        else if (FieldName == "Machine /Labor") {
                                            if (Subcon == "" & Subvendor == "") {
                                                td.style.color = "blue";
                                            }
                                        }
                                        else if (FieldName == "Machine") {
                                            if (Subcon == "" & Subvendor == "") {
                                                var MachineOrLabor = TabProcCost.rows[i - 1].cells[z].innerHTML;
                                                if (MachineOrLabor == "Machine") {
                                                    td.style.color = "blue";
                                                }
                                            }
                                        }
                                        else if (FieldName == "Vendor Rate/HR") {
                                            
                                            if (Subcon == "" & Subvendor == "") {
                                                var StdRate = TabProcCost.rows[i].cells[z].innerHTML;
                                                var VndRate = TabProcCost.rows[i - 1].cells[z].innerHTML;
                                                if (StdRate != VndRate) {
                                                    td.style.color = "blue";
                                                }
                                            }
                                        }
                                        else if (FieldName == "Base qty") {
                                            if (Subcon == "" & Subvendor == "") {
                                                var ProcUOM = TabProcCost.rows[i - 1].cells[z].innerHTML.trim().toUpperCase().replace(/\s/g, '');
                                                var BaseQtyTabProcCost = TabProcCost.rows[i - 1].cells[z].innerHTML;
                                                var BaseQtyTabMatCost = "";
                                                for (var m = 1; m < rowscountTabMatCost; m++) {
                                                    var FieldNameMatCost = TabMatCost.rows[m].cells[0].innerHTML.trim();
                                                    if (FieldNameMatCost == "Base Qty / Cavity") {
                                                        BaseQtyTabMatCost = TabMatCost.rows[m].cells[1].innerHTML.trim();
                                                    }
                                                }

                                                if (ProcUOM == "PCS/LOAD") {
                                                    td.style.color = "blue";
                                                }
                                                else if (Layout == "Layout5") {
                                                    if (ProcUOM != "CYCLETIMEINSEC/PC") {
                                                        if (CellsCountTabMatCost != 2) {
                                                            td.style.color = "blue";
                                                        }
                                                        else {
                                                            if (BaseQtyTabProcCost != BaseQtyTabMatCost) {
                                                                td.style.color = "blue";
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (ProcUOM != "CYCLETIMEINSEC/PC") {
                                                    if (rowscountTabMatCost > 2) {
                                                        td.style.color = "blue";
                                                    }
                                                    else {
                                                        if (BaseQtyTabProcCost != BaseQtyTabMatCost) {
                                                            td.style.color = "blue";
                                                        }
                                                    }
                                                }
                                                /////
                                            }
                                        }
                                        else if (FieldName == "Duration per Process UOM (Sec)") {
                                            if (Subcon == "" & Subvendor == "") {
                                                var ProcUOM = TabProcCost.rows[i - 2].cells[z].innerHTML.trim().toUpperCase().replace(/\s/g, '');
                                                if (ProcUOM.includes("STROKES/MIN-")) {
                                                    //td.style.color = "black";
                                                }
                                                else {
                                                    td.style.color = "blue";
                                                }
                                            }
                                            /////

                                        }
                                        else if (FieldName == "Efficiency") {
                                            if (Subcon == "" & Subvendor == "") {
                                                var ProcUOM = TabProcCost.rows[i - 3].cells[z].innerHTML.trim().toUpperCase().replace(/\s/g, '');
                                                if (ProcUOM.includes("STROKES/MIN-")) {
                                                    //td.style.color = "black";
                                                }
                                                else {
                                                    td.style.color = "blue";
                                                }
                                            }
                                            /////

                                        }
                                        else if (FieldName == "Turnkey Cost/pc") {
                                            if (Subcon == "" & Subvendor == "") {
                                            }
                                            else if (Subcon == "" & Subvendor != "") {
                                                td.style.color = "blue";
                                            }
                                            /////

                                        }
                                        else if (FieldName == "Turnkey Fees") {
                                            if (Subcon == "" & Subvendor == "") {
                                            }
                                            /////

                                        }
                                        else if (FieldName == "Process Cost/pc") {

                                            if (Subcon != "" & Subvendor == "") {
                                                td.style.color = "blue";
                                            }
                                            /////
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (err) {
                alert("CekValueInputByUser : " + err);
            }
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

        function freezeheader() {
            try {
                (function ($) {
                    $("#Table1").tableHeadFixer({ 'left': 1 });
                    $("#TablePC").tableHeadFixer({ 'left': 1 });
                    $("#TableSMC").tableHeadFixer({ 'left': 1 });
                    $("#TableOthers").tableHeadFixer({ 'left': 1 });
                    $("#TableUnit").tableHeadFixer({ 'left': 1 });
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : freezeheader');
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
                alert(err + ": CheckFileUpload")
            }
        }

        function CalculateTabUnit() {
            try {
                
                var hdnMCTableValues = $("#hdnMCTableValues").val();
                var hdnProcessValues = $("#hdnProcessValues").val();
                var hdnSMCTableValues = $("#hdnSMCTableValues").val();
                var hdnOtherValues = $("#hdnOtherValues").val();
                var vndType = $("#hdnVendorType").val();

                var hdnMassRevision = $("#hdnMassRevision").val();
                var TotMat = $("#HdnMAssTotMatCost").val();
                var TotProc = $("#HdnMAssTotProcCost").val();
                var TotSubMat = $("#HdnMAssTotSubMatCost").val();
                var TotOth = $("#HdnMAssTotOthCost").val();

                if (hdnMassRevision != "") {
                    if (vndType == "External") {
                        if (hdnProcessValues == "" && hdnSMCTableValues == "" && hdnOtherValues == "") {
                            if (document.getElementById("hdnUnitValues").value != "") {
                                var Tot = parseFloat(TotMat) + parseFloat(TotProc) + parseFloat(TotSubMat) + parseFloat(TotOth);
                                document.getElementById("TableUnit").rows[5].cells[1].innerHTML = parseFloat(Tot).toFixed(5);
                                document.getElementById("TableUnit").rows[5].cells[4].innerHTML = parseFloat(Tot).toFixed(5);
                            }
                        }
                    }
                }
            }
            catch (err) {
                alert("Test" + err)
            }
        }

        function Layout7Condition() {
            try {
                if ($('#hdnLayoutScreen').val() == "Layout7") {
                    $('#DvProcessCostPart').hide();
                    $('#DvSubMatPart').hide();
                }
            } catch (err) {
                alert("Layout7Condition : " + err)
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

    <%--additional script--%>
    <script type="text/javascript">
        $(document).ready(function () {
            try {
                CalculateTabUnit();
                $(function () {
                    if (document.getElementById("hdnUnitValues").value != "") {
                        var hdnVendorType = $("#hdnVendorType").val();
                        if (hdnVendorType == "TeamShimano") {
                            var x = document.getElementById("TableOthers").rows.length;
                            var y = document.getElementById("TableOthers").rows[x - 1].cells[1].innerHTML;
                            document.getElementById("TableUnit").rows[4].cells[1].innerHTML = y;
                        }
                        else {
                            var x = document.getElementById("TablePC").rows.length;
                            var y = document.getElementById("TablePC").rows[x - 1].cells[1].innerHTML;
                            document.getElementById("TableUnit").rows[2].cells[1].innerHTML = y;
                            var Grantot = document.getElementById("TableUnit").rows[5].cells[1].innerHTML;
                            var FinalGrantot = document.getElementById("TableUnit").rows[5].cells[4].innerHTML;
                            var NetprofDisc = null;
                            if (Grantot == "" && FinalGrantot == "") {
                                NetprofDisc = 0.0;
                            }
                            else {
                                NetprofDisc = (((FinalGrantot - Grantot) / FinalGrantot) * 100);
                            }
                            document.getElementById("TableUnit").rows[5].cells[5].innerHTML = NetprofDisc.toFixed(1) + ' %';
                        }
                    }
                });

                freezeheader();
                Layout7Condition();
                CekValueInputByUser();
                MassRevForBOMWhenHaveQuteRef();
                CekIsuseToolAmor()
                playAudio();
            }
            catch (err) {
                alert(err);
            }
        });
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
            } catch (err) {
                alert("CekValueInputByUser : " + err);
            }
        }
    </script>
</head>
<body id="page-top">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
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
                <div class="container-fluid" style="display:none">
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

        <div id="wrapper">
            <!-- Sidebar -->
            <div id="SideBarMenu" style="width: 300px; display:none;" runat="server" class="SideBarMenu">
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
                    <div class="card" style="background-color: white">
                        <div class="card-body">
                            <%--button reset--%>
                            <div class="col-md-12" style="background-color: white; padding-top: 10px;">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="lbTitle" runat="server" Text="Review" Font-Bold="true" Font-Size="Large" />
                                        <asp:Label ID="LbStatus" runat="server" Text="(N.A)" Font-Bold="true" Font-Size="Large" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12" style="padding-top: 5px; background-color: white;">
                                <div class="col-md-12" style="border-bottom: 2px solid #006EB7"></div>
                            </div>

                            <div class="col-md-12" style="background-color: white;">
                                <div class="row" style="padding-bottom: 10px; padding-top: 10px;">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="Label8" runat="server" Text="VENDOR DETAIL"></asp:Label>
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

                                        <div class="row" runat="server" id="DVSmnSubmit" visible="false">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label37" runat="server" Text="SMN Submit By"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:Label ID="LbSmnSubmitBy" runat="server" Text="Waiting For submission"></asp:Label>
                                                <asp:Label ID="LbSmnSubmitOn" runat="server" Text=""></asp:Label>
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
                                                <asp:TextBox ID="txtsmnpic" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_addres" runat="server" ForeColor="Black" Text="Email"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtemail" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
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
                                                <asp:Label ID="Labelquote" runat="server" ForeColor="Black" Text="Quotation Due Date"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtquotationDueDate" runat="server" ForeColor="Black" BackColor="#E6E6E6" Enabled="false"></asp:TextBox>
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

                                <div class="row" style="padding-bottom: 10px;">
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
                                                <asp:TextBox ID="txtpartdesc" Enabled="false" runat="server" Height="55px"
                                                    TextMode="MultiLine" ForeColor="Black" Width="100%" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row" runat="server" id="DvProduct">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_cntact0" runat="server" ForeColor="Black" Text="Product"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtprod" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="DvReqPlant" style="display: none;">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label16" runat="server" ForeColor="Black" Text="GP Request Plant"></asp:Label>
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
                                                <asp:TextBox ID="txtSAPJobType" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_PIR" runat="server" ForeColor="Black" Text="PIR Type & Desc"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtPIRtype" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
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
                                                <asp:TextBox ID="txtdrawng" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbl_proces" runat="server" ForeColor="Black" Text="Process Group"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtprocs" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="txtPartUnit" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label27" runat="server" ForeColor="Black" Text="Base UOM: "></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtBaseUOM" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <%--     <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label5" runat="server"  ForeColor="Black" Text="Country Of Origin"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtCorigin" Enabled="false" runat="server" Height="27px" Width="100%" 
                                            ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label5" runat="server" Text="Net Weight:"
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
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                                                    <asp:Label ID="Label38" runat="server" Text="Incoterms"></asp:Label>
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

                                    <div class="row" style="padding-bottom: 10px;">
                                        <%--<div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label31" runat="server" Text="GP Request Plant"  ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TxtPlantRequestor" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                                <!-- end whitout code gp field -->


                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label12" runat="server" ForeColor="Black" Text="Effective Date"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="TextBox1" runat="server" Enabled="false" ForeColor="Black"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label13" runat="server" ForeColor="Black" Text="Due Dt Next Rev"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtfinal" runat="server" Enabled="false" ForeColor="Black"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;" runat="server" id="DvEffDateChngByMng" visible="false">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label40" runat="server" ForeColor="Black" Text="New Effective Date"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="TxtEffDateByMng" runat="server" Enabled="false" ForeColor="Black"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label41" runat="server" ForeColor="Black" Text="New Due Dt Next Rev"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="TxtDueDateByMng" runat="server" Enabled="false" ForeColor="Black"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="Label28" runat="server" ForeColor="Black" Text="Country Of Origin"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtCorigin" Enabled="false" runat="server" Height="27px" Width="100%" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <asp:Label ID="Label31" runat="server" ForeColor="Black" Text="Attachment"></asp:Label>
                                            </div>
                                            <div class="col-lg-7">
                                                <asp:UpdatePanel ID="UpdatePanel22" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="LbFlName" runat="server" ForeColor="Black" Text="No File" onclick="ClcBtnFlUpload();"></asp:Label>
                                                        <asp:LinkButton runat="server" ID="BtnPreview" OnClientClick="return CheckFileUpload();" OnClick="BtnPreview_Click"
                                                            CssClass="lbattachpad pull-right">
                                                            <span class="glyphicon glyphicon-download" style="padding: 0px; color: #0bd409;" runat="server" id="Span1"></span>
                                                        </asp:LinkButton>
                                                        <div style="display: none">
                                                            <asp:Label ID="LbFlNameOri" runat="server" ForeColor="Black" Text="No File"></asp:Label>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="BtnPreview" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

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
                                                <asp:Label ID="Label9" runat="server" ForeColor="Black" Text="Recycle Ratio (%)"></asp:Label>
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
                                <div class="row" style="padding-bottom: 10px;" runat="server" id="DvGvPIROldQuoteMass">
                                    <div class="col-md-12">
                                        <div class="row" style="padding-top: 5px;">
                                            <div class="col-md-12" style="padding-bottom: 5px;">
                                                <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid blue">
                                                    <asp:Label ID="Label36" runat="server" ForeColor="Black" Text="Old Data From PIR"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="table-responsive table-sm">
                                                    <asp:GridView ID="GvQuoteDataPIR" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" OnRowCommand="GvQuoteDataPIR_RowCommand"
                                                        EmptyDataText="No records Found" BackColor="White" CssClass="table-responsive table-sm" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="PIRNo" HeaderText="PIR No" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Quote Ref" HeaderStyle-CssClass="text-center ">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Text='<%# Eval("MassRevQutoteRef") %>' runat="server" CommandName="LinktoRedirect" ID="LbMassRevQutoteRef"
                                                                                CommandArgument="  <%# ((GridViewRow) Container).RowIndex %>" OnClientClick="ShowLoading();" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
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
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true"
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
                                                                <ItemStyle Width="70px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMatRef" runat="server" Text='<%# Eval("TotalMaterialCost") %>' onclick="return false;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Proc Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                <ItemStyle Width="70px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkProcRef" runat="server" Text='<%# Eval("TotalProcessCost") %>' onclick="return false;"/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sub Mat Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                <ItemStyle Width="70px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSubMatRef" runat="server" Text='<%# Eval("TotalSubMaterialCost") %>' onclick="return false;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Others Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                <ItemStyle Width="70px"></ItemStyle>
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
                                        <span class="glyphicon glyphicon-stop" style="color: blue"></span>Value Input by user 
                                            <span class="glyphicon glyphicon-stop" style="color: black"></span>Value from system 
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-md-12">
                                        <div class="col-md-12 title">
                                            <asp:Label ID="lblmatlcost" runat="server" Text="PART II: Material Cost"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <%--table material cost--%>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="row" style="padding-bottom: 10px;">
                                            <div class="col-md-12">
                                                <div class="table-responsive">
                                                    <asp:Table ID="Table1" runat="server" CssClass="table-bordered table-sm table-nowrap">
                                                    </asp:Table>
                                                </div>
                                            </div>
                                            <div class="col-md-12" id="DvOldMatCost" runat="server" visible="false">
                                                <asp:Label runat="server" ID="txtOldMatcost" Text="Old Total Material Cost/pc :"></asp:Label>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <%--NoteMaterial--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
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

                                    <%--table process cost--%>
                                    <div class="row" style="padding-bottom: 10px;">
                                        <div class="col-md-12">
                                            <div class="table table-responsive table-sm">
                                                <asp:Table ID="TablePC" runat="server" CssClass="table-bordered table-nowrap"></asp:Table>
                                            </div>
                                        </div>
                                        <div class="col-md-12" id="DvOldOldProccost" runat="server" visible="false">
                                            <asp:Label runat="server" ID="txtOldProccost" Text="Old Total Process Cost/pc :"></asp:Label>
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

                                    <%--table SUB-MAT/T&J COST--%>
                                    <div class="row" style="padding-bottom: 10px;">
                                        <div class="col-md-12">
                                            <div class="table table-responsive table-sm">
                                                <asp:Table ID="TableSMC" runat="server" CssClass="table-bordered table-nowrap ">
                                                </asp:Table>
                                            </div>
                                        </div>
                                        <div class="col-md-12" id="DvOldSubMat" runat="server" visible="false">
                                            <asp:Label runat="server" ID="txtOldSubMat" Text="Old Total SUB-MAT/T&J COST Cost/pc :"></asp:Label>
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

                                <%--table OTHER COST--%>
                                <div class="row" style="padding-bottom: 10px;">
                                    <div class="col-md-12">
                                        <div class="table table-responsive table-sm">
                                            <asp:Table ID="TableOthers" runat="server" CssClass="table-bordered table-nowrap ">
                                            </asp:Table>
                                        </div>
                                    </div>
                                    <div class="col-md-12" id="DvOldOthCost" runat="server" visible="true">
                                            <asp:Label runat="server" ID="txtOldOthCost" Text="Old Total Oth Cost/pc :"></asp:Label>
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
                                            <asp:Table ID="TableUnit" runat="server" CssClass="table-bordered table-nowrap">
                                            </asp:Table>
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
                                                <asp:TextBox ID="TxtComntByVendor" Enabled="false" runat="server" Height="55px" TextMode="MultiLine" MaxLength="150" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%--back to Home--%>
                                <div class="row" style="padding-bottom: 10px; display:none">
                                    <div class="col-md-12 text-right">
                                        <asp:Button ID="Button1" runat="server" PostBackUrl="~/Vendor.aspx" Text="Back to Home" CssClass="btn btn-primary" />
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

                            <asp:GridView ID="grdMachinelisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />

                                <Columns>
                                    <asp:BoundField DataField="Machine" HeaderText="Machine" />
                                    <asp:BoundField DataField="SMNStdrateHr" HeaderText="SMNStdrateHr" />
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

                            <asp:GridView ID="grdLaborlisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                Style="color: #333333; border-collapse: collapse; visibility: collapse;">
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

                            <asp:Label ID="Label1" runat="server" Text="Vendor Details" ForeColor="#2153a5" Visible="false"></asp:Label>
                            <asp:HiddenField ID="ddlSubprocess" runat="server" Value="" />
                            <asp:HiddenField ID="ddlUom" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="hdnTProCost" runat="server" Value="" />
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

                            <asp:HiddenField ID="hdnOtherValues" runat="server" Value="" />
                            <asp:HiddenField ID="hdnProcessValues" runat="server" Value="" />
                            <asp:HiddenField ID="hdnMCTableValues" runat="server" Value="" />

                            <asp:HiddenField ID="hdnUnitValues" runat="server" Value="" />
                            <asp:HiddenField ID="hdnLayoutScreen" runat="server" Value="" />
                            <asp:HiddenField ID="hdnVendorType" runat="server" Value="" />
                            <asp:HiddenField ID="hdnQuoteNoRef" runat="server" Value="" />

                            <asp:HiddenField ID="hdnQuoteNoRefmassRev" runat="server" Value="" />
                            <asp:HiddenField ID="HdnStatus" runat="server" Value="" />

                            <asp:HiddenField ID="hdnMassRevision" runat="server" Value="" />
                            <asp:HiddenField ID="HdnMAssTotMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnMAssTotProcCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnMAssTotSubMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnMAssTotOthCost" runat="server" Value="" />


                            <asp:HiddenField ID="HdnSAPSpProcType" runat="server" Value="" />
                            <asp:HiddenField ID="HdnAcsTabMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnAcsTabProCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnAcsTabSubMatCost" runat="server" Value="" />
                            <asp:HiddenField ID="HdnAcsTabOthCost" runat="server" Value="" />

                            <asp:HiddenField ID="HdnIsUseToolAmortize" runat="server" Value="" />
                            <asp:HiddenField ID="HdnIsUseMachineAmor" runat="server" Value="0" />
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
                                <div class="col-lg-12 Padding-Nol" style="font: bold 22px calibri, calibri; text-align: center; align-content: center;">Access For This Page is About To End !!  </div>
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
                                                This Page will closed in :
                                                <asp:Label ID="countdown" runat="server" Font-Bold="true" ForeColor="Red" Text="30"></asp:Label>
                                                seconds<br />
                                                do u want to keep this page open?
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-top-left-radius: 15px; border-top-right-radius: 15px;">
                                <asp:Button ID="BtnRefresh" runat="server" Text="Yes, Keep Open" OnClick="BtnRefresh_Click" CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="18px" />
                                <asp:Button ID="CtnCloseMdl" runat="server" Text="No, Close This Page" OnClick="CtnCloseMdl_Click" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="18px" />
                                <div style="display: none;">
                                    <asp:Button ID="StartTimer" runat="server" Text="Start" OnClick="StartTimer_Click" CssClass="btn btn-sm btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <!-- Modal session expired -->
        <%--<div class="modal fade" id="myModalSession" data-backdrop="static" tabindex="-1" role="dialog"
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
        </div>--%>
    </form>
</body>
</html>

