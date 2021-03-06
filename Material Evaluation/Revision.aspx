<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Revision.aspx.cs" Inherits="Material_Evaluation.Revision" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
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
    <link href="Scripts/datatables/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="Scripts/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <style type="text/css">
        .tbdisabled {
        pointer-events: none;
        cursor:no-drop!important;
        background:#787878!important;
        }

        .btn-success:disabled {
        background-color:#787878!important;
        }

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

        .selectedCell {
            background-color: lightblue;
        }

        .unselectedCell {
            background-color: white;
        }

        table.table thead tr th {
            color: white;
            background-color: #005496;
        }

        table.table tr td {
            font: 14px calibri;
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

        .no-footer {
        overflow-x:hidden!important;
        }

        .form-control-sm {
            font-size:15px!important;
            height:20px!important;
        }

        .optionPlaceHolder {
            color:gray!important;
        }

        
    </style>

    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Scripts/moment.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/jszip.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/buttons.html5.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/buttons.flash.min.js"></script>
    <script type="text/javascript" src="Scripts/datatables/buttons.print.min.js"></script>
    
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script src="Scripts/stickycolumandheaderplugin/tableHeadFixer.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript"  src="js/BootstrapDatePcr/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript"  src="js/BootstrapDatePcr/js/locales/bootstrap-datetimepicker.fr.js"></script>

    <%--script loading page--%>
    <script lang="javascript" type="text/javascript">
        var DataListVndAmor = [];
        var dataTableTbQuoteRefList, dataTableDuplicateWithExpiredReq, dataTableTbQuoteRefListSelected,dataTableTbQuoteRefListInvalid, dataTableTbCreateReqTemp;
        var DtReqPurpose, DtRecycleRatio, DateDuenextRev;
        var res;
        var currentPage = 0;
        var DataValidRequest = [];
        var test11 = [];
        var mainUrl = "";

        $(document).on('keypress', '#txtDate', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });

        $(window).load(function () {
            $('#loading').fadeOut("fast");
            //ChgEmptyFlColor();
        });

        $(document).ready(function () {
            if (typeof (Worker) !== "undefined") {

                mainUrl = window.location.href.replace("Revision.aspx", "");

                //ChgEmptyFlColor();
                GetDdlProduct();
                GetDdlProcGroup();
                GetDdlReason();
                GetImRecycleRatio();
                GetDateDuenextRev();

                GenerateTbQuoteRefList();
                GenerateTbDuplicateWithExpiredReq();
                GenerateTbQuoteRefListInvalid();
                GenerateTbQuoteRefListSelected();
                GenerateTbCreateReqTemp();

                DatePitcker();
                CloseLoading();

                $("div.toolbar").html('<label id="LbTitleDisabled" style="color:red;font-weight:500; display:none">(Data selected can not be changed until the pending request is canceled)</label> ');
            } else {
                alert('Sorry! No Web Worker support..');
                window.location = mainUrl + "/Home.aspx";
            }
        });

        $(document).on('change', '.effectiveDate', function () {
            var row = $(this).attr("attrrow");
            var id = $(this).attr("id");
            var selected = dataTableTbQuoteRefListSelected.column(23).nodes().to$().find('#DdlToolAmorRefSelected_' + row).val();
            var effectiveDate = dataTableTbQuoteRefListSelected.column(33).nodes().to$().find('#TxtEffDate_' + row).val();

            if (selected == "ADD" && effectiveDate != "") {
                if ($("#tableAmortize_" + row).length > 0) {
                    $("#tableAmortize_" + row).remove();
                }
                var tableAmor = '<table id="tableAmortize_' + row + '">' +
                                    '<thead>' +
                                        '<tr>' +
                                            '<th>Select</th>' +
                                            '<th>Tool Type</th>' +
                                            '<th>Tool Amortize ID</th>' +
                                            '<th>Tool Amortize Desc</th>' +
                                            '<th>Amortize Cost</th>' +
                                            '<th>Amortize Currency</th>' +
                                            '<th>Exch Rate</th>' +
                                            '<th>Period</th>' +
                                            '<th>Period UOM</th>' +
                                            '<th>Tot Amortize Qty</th>' +
                                            '<th>Qty UOM</th>' +
                                            '<th>Amortize Cost Vnd Curr</th>' +
                                            '<th>Amortize Cost Pc Vnd Curr</th>' +
                                            '<th>Effective From</th>' +
                                            '<th>Due On</th>' +
                                        '</tr>' +
                                    '</thead>' +
                               '</table>';
                $("#divToolAmortize_" + row).html(tableAmor);

                dataTableTbQuoteRefListSelected.columns.adjust().draw();
                var rowData = dataTableTbQuoteRefListSelected.row($(this).parents('tr')).data();
                var dtTable = $("#tableAmortize_" + row).DataTable({
                    "bDestroy": true,
                    "language": {
                        "emptytable": "No data found"
                    },
                    "drawCallback": function () {
                        $('div.dataTables_filter input').addClass('form-control form-control-sm');
                        $(".paginate_button").click(function () {
                            currentPage = dataTable.page.info().page;
                        });
                    },
                    "deferRender": false,
                    "filter": false,
                    "paging": false,
                    "columns": [
                        {
                            "data": "",
                            "render": function (data, type, row, meta) {
                                var amortizetoolid = row.Amortize_Tool_ID;
                                if (type === 'display') {
                                    return '<input type="radio" name="rb_' + rowData.QuoteNo + '" value="' + amortizetoolid + '">';
                                }
                            },
                            "autoWidth": true
                        },
                        { "data": "ToolTypeID" },
                        { "data": "Amortize_Tool_ID" },
                        { "data": "Amortize_Tool_Desc" },
                        { "data": "AmortizeCost" },
                        { "data": "AmortizeCurrency" },
                        { "data": "ExchangeRate" },
                        { "data": "AmortizePeriod" },
                        { "data": "AmortizePeriodUOM" },
                        { "data": "TotalAmortizeQty" },
                        { "data": "QtyUOM" },
                        { "data": "AmortizeCost_Vend_Curr" },
                        { "data": "AmortizeCost_Pc_Vend_Curr" },
                        {
                            "data": "EffectiveFrom",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "DueDate",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                    ],
                    "ordering": false,
                    language: {
                        "lengthMenu": "Show <input class='form-control form-control-sm' id='lcDatatables' value='10' style='width:70px; display:unset; text-align:center;' type='number' min='1'/> entries &nbsp; &nbsp;"
                    },
                    "scrollX": true,
                });

                var obj = { QuoteNo: rowData.QuoteNo, table: dtTable };
                test11.push(obj);

                var VendorCode = "";
                var processgrp = "";
                var EffectiveDate = "";
                var Material = "";
                var dataTableToolAmor = $("#tableAmortize_" + row).DataTable();
                GetVendorToolAmor(rowData.Vendor, rowData.ProcessGroup, effectiveDate, rowData.Material, rowData.QuoteNo);
            }
        });
        
    </script>

    <%--no longer use--%>
    <script type="text/javascript">
        function CheckorUncheckAllGvRefList() {
            try {
                var IsLY7InTheList = $("#TxtIsLY7InTheList").val();
                if (IsLY7InTheList == "true") {
                    alert('Cannot Check All Cost when have Raw Material Layout in the List !');
                    document.getElementById("GvQuoteRefList_chkAllRefHd").checked = false;
                }
                else {
                    var ChkAll = document.getElementById("GvQuoteRefList_chkAllRefHd").checked;
                    if (ChkAll == true) {
                        $("#GvQuoteRefList").find("[type='checkbox']").prop('checked', true);
                    }
                    else {
                        $("#GvQuoteRefList").find("[type='checkbox']").prop('checked', false);
                    }
                }

                CheckSubAndProcCostChecked();
            }
            catch (err) {
                alert(err + ": CheckorUncheckGvRefList")
            }
        }

        function IsAllGvHdRefListCheck(All, Mat, Proc, SubMat, Oth, ToolAmor, MachineAmor) {
            try {
                var CAll = document.getElementById(All);
                var CMat = document.getElementById(Mat);
                var CProc = document.getElementById(Proc);
                var CSubMat = document.getElementById(SubMat);
                var COth = document.getElementById(Oth);

                var CToolAmor = document.getElementById(ToolAmor);
                var CMachineAmor = document.getElementById(MachineAmor);

                if (CMat.checked == true && CProc.checked == true && CSubMat.checked == true && COth.checked == true && CToolAmor.checked == true && CMachineAmor.checked == true) {
                    CAll.checked = true;
                }
                else {
                    CAll.checked = false;
                }

                if (CAll.checked == true) {
                    $("#GvQuoteRefList").find("[type='checkbox']").prop('checked', true);
                }
            }
            catch (err) {
                alert(err + ": CheckorUncheckGvRefList")
            }
        }

        function CheckSubAndProcCostChecked() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    for (var i = 1; i < RowsCountGv; i++) {
                        var CSubMat = document.getElementById("GvQuoteRefList_chkSubMatRef_" + (i - 1) + "");
                        var CProc = document.getElementById("GvQuoteRefList_chkProcRef_" + (i - 1) + "");

                        var CToolAmor = document.getElementById("GvQuoteRefList_chkToolAmortizeRef_" + (i - 1) + "");
                        var CMachineAmor = document.getElementById("GvQuoteRefList_chkMachineAmortizeRef_" + (i - 1) + "");

                        var DToolAmor = document.getElementById("GvQuoteRefList_DdlToolAmortizeRef_" + (i - 1) + "");
                        var DMachineAmor = document.getElementById("GvQuoteRefList_DdlMachineAmortizeRef_" + (i - 1) + "");

                        if (CSubMat.checked == true) {
                            DToolAmor.removeAttribute('disabled');
                        }
                        else {
                            DToolAmor.setAttribute("disabled", "disabled");
                            DToolAmor.value = "REMOVE";
                            CToolAmor.checked = false;
                        }

                        if (CProc.checked == true && CSubMat.checked == true) {
                            DMachineAmor.removeAttribute('disabled');
                        }
                        else {
                            DMachineAmor.setAttribute("disabled", "disabled");
                            DMachineAmor.value = "REMOVE";
                            CMachineAmor.checked = false;
                        }
                    }
                }
            }
            catch (err) {
                alert(err + ": CheckSubAndProcCostChecked")
            }
        }

        function IsAllCostRowChecked() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var c = 1;
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    for (var i = 1; i < RowsCountGv; i++) {
                        var All = document.getElementById("GvQuoteRefList_chkAllRefRw_" + (i - 1) + "");
                        if (All.checked == true) {
                            c++;
                        }
                    }
                    if (c == RowsCountGv) {
                        document.getElementById("GvQuoteRefList_chkAllRefHd").checked = true;
                    }
                    else {
                        document.getElementById("GvQuoteRefList_chkAllRefHd").checked = false;
                    }
                }
            }
            catch (err) {
                alert(err + ": IsAllCostRowChecked")
            }
        }

        function IsAllMatCostRefRowChecked() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var c = 1;
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    for (var i = 1; i < RowsCountGv; i++) {
                        var All = document.getElementById("GvQuoteRefList_chkMatRef_" + (i - 1) + "");
                        if (All.checked == true) {
                            c++;
                        }
                    }
                    if (c == RowsCountGv) {
                        document.getElementById("GvQuoteRefList_chkAllMatRef").checked = true;
                    }
                    else {
                        document.getElementById("GvQuoteRefList_chkAllMatRef").checked = false;
                    }
                }
            }
            catch (err) {
                alert(err + ": IsAllMatCostRefRowChecked")
            }
        }

        function IsAllProCostRefRowChecked() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var c = 1;
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    for (var i = 1; i < RowsCountGv; i++) {
                        var All = document.getElementById("GvQuoteRefList_chkProcRef_" + (i - 1) + "");
                        if (All.checked == true) {
                            c++;
                        }
                    }
                    if (c == RowsCountGv) {
                        document.getElementById("GvQuoteRefList_chkAllProcRef").checked = true;
                    }
                    else {
                        document.getElementById("GvQuoteRefList_chkAllProcRef").checked = false;
                    }
                }
            }
            catch (err) {
                alert(err + ": IsAllProCostRefRowChecked")
            }
        }

        function IsAllSubMatCostRefRowChecked() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var c = 1;
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    for (var i = 1; i < RowsCountGv; i++) {
                        var All = document.getElementById("GvQuoteRefList_chkSubMatRef_" + (i - 1) + "");
                        if (All.checked == true) {
                            c++;
                        }
                    }
                    if (c == RowsCountGv) {
                        document.getElementById("GvQuoteRefList_chkAllSubMatRef").checked = true;
                    }
                    else {
                        document.getElementById("GvQuoteRefList_chkAllSubMatRef").checked = false;
                    }
                }
            }
            catch (err) {
                alert(err + ": IsAllProCostRefRowChecked")
            }
        }

        function IsAllOthCostRefRowChecked() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var c = 1;
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    for (var i = 1; i < RowsCountGv; i++) {
                        var All = document.getElementById("GvQuoteRefList_chkOthRef_" + (i - 1) + "");
                        if (All.checked == true) {
                            c++;
                        }
                    }
                    if (c == RowsCountGv) {
                        document.getElementById("GvQuoteRefList_chkAllOthRef").checked = true;
                    }
                    else {
                        document.getElementById("GvQuoteRefList_chkAllOthRef").checked = false;
                    }
                }
            }
            catch (err) {
                alert(err + ": IsAllProCostRefRowChecked")
            }
        }

        function IsAllToolAmorRefRowChecked() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var c = 1;
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    for (var i = 1; i < RowsCountGv; i++) {
                        var All = document.getElementById("GvQuoteRefList_chkToolAmortizeRef_" + (i - 1) + "");
                        if (All.checked == true) {
                            c++;
                        }
                    }
                    if (c == RowsCountGv) {
                        document.getElementById("GvQuoteRefList_chkAllToolAmortizeRef").checked = true;
                    }
                    else {
                        document.getElementById("GvQuoteRefList_chkAllToolAmortizeRef").checked = false;
                    }
                }
            }
            catch (err) {
                alert(err + ": IsAllToolAmorRefRowChecked")
            }
        }

        function IsAllMachineAmorRefRowChecked() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var c = 1;
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    for (var i = 1; i < RowsCountGv; i++) {
                        var All = document.getElementById("GvQuoteRefList_chkMachineAmortizeRef_" + (i - 1) + "");
                        if (All.checked == true) {
                            c++;
                        }
                    }
                    if (c == RowsCountGv) {
                        document.getElementById("GvQuoteRefList_chkAllMachineAmortizeRef").checked = true;
                    }
                    else {
                        document.getElementById("GvQuoteRefList_chkAllMachineAmortizeRef").checked = false;
                    }
                }
            }
            catch (err) {
                alert(err + ": IsAllMachineAmorRefRowChecked")
            }
        }

        function IsAllGvRowRefListCheck(All, Mat, Proc, SubMat, Oth, ToolAmor, MacAmor, DdlToolAmor, DdlMacAmor) {
            try {
                var CAll = document.getElementById(All);
                var CMat = document.getElementById(Mat);
                var CProc = document.getElementById(Proc);
                var CSubMat = document.getElementById(SubMat);
                var COth = document.getElementById(Oth);

                var CToolAmor = document.getElementById(ToolAmor);
                var CMachineAmor = document.getElementById(MacAmor);

                if (CMat.checked == true && CProc.checked == true && CSubMat.checked == true && COth.checked == true && CToolAmor.checked == true && CMachineAmor.checked == true) {
                    CAll.checked = true;
                }
                else {
                    CAll.checked = false;
                }

                var DToolAmor = document.getElementById(DdlToolAmor);
                var DMachineAmor = document.getElementById(DdlMacAmor);

                if (CSubMat.checked == true) {
                    DToolAmor.removeAttribute('disabled');
                }
                else {
                    DToolAmor.setAttribute("disabled", "disabled");
                    DToolAmor.value = "REMOVE";
                    CToolAmor.checked = false;
                }

                if (CProc.checked == true && CSubMat.checked == true) {
                    DMachineAmor.removeAttribute('disabled');
                }
                else {
                    DMachineAmor.setAttribute("disabled", "disabled");
                    DMachineAmor.value = "REMOVE";
                    CMachineAmor.checked = false;
                }

                IsAllCostRowChecked();
            }
            catch (err) {
                alert(err + ": CheckorUncheckGvRefList")
            }
        }

        function DdlToolAmortizeRefChange(All, Mat, Proc, SubMat, Oth, ToolAmor, MacAmor, DdlToolAmor, DdlMacAmor) {
            try {
                var CAll = document.getElementById(All);
                var CMat = document.getElementById(Mat);
                var CProc = document.getElementById(Proc);
                var CSubMat = document.getElementById(SubMat);
                var COth = document.getElementById(Oth);

                var CToolAmor = document.getElementById(ToolAmor);
                var CMachineAmor = document.getElementById(MacAmor);

                var DToolAmor = document.getElementById(DdlToolAmor);
                var DMachineAmor = document.getElementById(DdlMacAmor);

                if (DToolAmor.value == "ADD") {
                    CToolAmor.checked = true;
                }
                else {
                    CToolAmor.checked = false;
                }
            }
            catch (err) {
                alert(err + ": DdlToolAmortizeRefChange")
            }
        }

        function DdlMachineAmortizeRefChange(All, Mat, Proc, SubMat, Oth, ToolAmor, MacAmor, DdlToolAmor, DdlMacAmor) {
            try {
                var CAll = document.getElementById(All);
                var CMat = document.getElementById(Mat);
                var CProc = document.getElementById(Proc);
                var CSubMat = document.getElementById(SubMat);
                var COth = document.getElementById(Oth);

                var CToolAmor = document.getElementById(ToolAmor);
                var CMachineAmor = document.getElementById(MacAmor);

                var DToolAmor = document.getElementById(DdlToolAmor);
                var DMachineAmor = document.getElementById(DdlMacAmor);

                if (DMachineAmor.value == "ADD") {
                    CMachineAmor.checked = true;
                }
                else {
                    CMachineAmor.checked = false;
                }
            }
            catch (err) {
                alert(err + ": DdlMachineAmortizeRefChange")
            }
        }

        function chkAllRowRefClick(All, Mat, Proc, SubMat, Oth, ToolAmor, MacAmor, DdlToolAmor, DdlMacAmor) {
            try {
                var CAll = document.getElementById(All);
                var CMat = document.getElementById(Mat);
                var CProc = document.getElementById(Proc);
                var CSubMat = document.getElementById(SubMat);
                var COth = document.getElementById(Oth);

                var CToolAmor = document.getElementById(ToolAmor);
                var CMachineAmor = document.getElementById(MacAmor);

                if (CAll.checked == true) {
                    CMat.checked = true; CProc.checked = true; CSubMat.checked = true; COth.checked = true;
                    CToolAmor.checked = true; CMachineAmor.checked = true;
                }
                else {
                    CMat.checked = false; CProc.checked = false; CSubMat.checked = false; COth.checked = false;
                    CToolAmor.checked = false; CMachineAmor.checked = false;
                }

                var DToolAmor = document.getElementById(DdlToolAmor);
                var DMachineAmor = document.getElementById(DdlMacAmor);
                if (CSubMat.checked == true) {
                    DToolAmor.removeAttribute('disabled');
                }
                else {
                    DToolAmor.setAttribute("disabled", "disabled");
                    DToolAmor.value = "REMOVE";
                    CToolAmor.checked = false;
                }

                if (CProc.checked == true && CSubMat.checked == true) {
                    DMachineAmor.removeAttribute('disabled');
                }
                else {
                    DMachineAmor.setAttribute("disabled", "disabled");
                    DMachineAmor.value = "REMOVE";
                    CMachineAmor.checked = false;
                }

                IsAllMatCostRefRowChecked();
                IsAllProCostRefRowChecked();
                IsAllSubMatCostRefRowChecked();
                IsAllOthCostRefRowChecked();
                IsAllToolAmorRefRowChecked();
                IsAllMachineAmorRefRowChecked();
            }
            catch (err) {
                alert(err + ": chkAllRowRefClick")
            }
        }

        function CheckAllMatCostRef() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    var All = document.getElementById("GvQuoteRefList_chkAllMatRef");
                    if (All.checked == true) {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkMatRef_" + (i - 1) + "").checked = true;
                        }
                    }
                    else {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkMatRef_" + (i - 1) + "").checked = false;
                            document.getElementById("GvQuoteRefList_chkAllRefRw_" + (i - 1) + "").checked = false;
                        }
                    }
                }
                IsAllGvRowRefCheckFromHeaderClick();
            }
            catch (err) {
                alert(err + ": CheckAllMatCostRef")
            }
        }

        function CheckAllProcCostRef() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    var All = document.getElementById("GvQuoteRefList_chkAllProcRef");
                    if (All.checked == true) {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkProcRef_" + (i - 1) + "").checked = true;

                            var CSubMat = document.getElementById("GvQuoteRefList_chkSubMatRef_" + (i - 1) + "");
                            var CProc = document.getElementById("GvQuoteRefList_chkProcRef_" + (i - 1) + "");
                            var CToolAmor = document.getElementById("GvQuoteRefList_chkToolAmortizeRef_" + (i - 1) + "");
                            var CMachineAmor = document.getElementById("GvQuoteRefList_chkMachineAmortizeRef_" + (i - 1) + "");
                            var DToolAmor = document.getElementById("GvQuoteRefList_DdlToolAmortizeRef_" + (i - 1) + "");
                            var DMachineAmor = document.getElementById("GvQuoteRefList_DdlMachineAmortizeRef_" + (i - 1) + "");

                            if (CSubMat.checked == true) {
                                DToolAmor.removeAttribute('disabled');
                            }
                            else {
                                DToolAmor.setAttribute("disabled", "disabled");
                                DToolAmor.value = "REMOVE";
                                CToolAmor.checked = false;
                            }

                            if (CProc.checked == true && CSubMat.checked == true) {
                                DMachineAmor.removeAttribute('disabled');
                            }
                            else {
                                DMachineAmor.setAttribute("disabled", "disabled");
                                DMachineAmor.value = "REMOVE";
                                CMachineAmor.checked = false;
                            }
                        }
                    }
                    else {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkProcRef_" + (i - 1) + "").checked = false;
                            document.getElementById("GvQuoteRefList_chkAllRefRw_" + (i - 1) + "").checked = false;

                            var CSubMat = document.getElementById("GvQuoteRefList_chkSubMatRef_" + (i - 1) + "");
                            var CProc = document.getElementById("GvQuoteRefList_chkProcRef_" + (i - 1) + "");
                            var CToolAmor = document.getElementById("GvQuoteRefList_chkToolAmortizeRef_" + (i - 1) + "");
                            var CMachineAmor = document.getElementById("GvQuoteRefList_chkMachineAmortizeRef_" + (i - 1) + "");
                            var DToolAmor = document.getElementById("GvQuoteRefList_DdlToolAmortizeRef_" + (i - 1) + "");
                            var DMachineAmor = document.getElementById("GvQuoteRefList_DdlMachineAmortizeRef_" + (i - 1) + "");

                            if (CSubMat.checked == true) {
                                DToolAmor.removeAttribute('disabled');
                            }
                            else {
                                DToolAmor.setAttribute("disabled", "disabled");
                                DToolAmor.value = "REMOVE";
                                CToolAmor.checked = false;
                            }

                            if (CProc.checked == true && CSubMat.checked == true) {
                                DMachineAmor.removeAttribute('disabled');
                            }
                            else {
                                DMachineAmor.setAttribute("disabled", "disabled");
                                DMachineAmor.value = "REMOVE";
                                CMachineAmor.checked = false;
                            }
                        }
                    }
                    IsAllGvRowRefCheckFromHeaderClick();
                }
            }
            catch (err) {
                alert(err + ": CheckAllProcCostRef")
            }
        }

        function CheckAllSubMatCostRef() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    var All = document.getElementById("GvQuoteRefList_chkAllSubMatRef");
                    if (All.checked == true) {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkSubMatRef_" + (i - 1) + "").checked = true;

                            var CSubMat = document.getElementById("GvQuoteRefList_chkSubMatRef_" + (i - 1) + "");
                            var CProc = document.getElementById("GvQuoteRefList_chkProcRef_" + (i - 1) + "");
                            var CToolAmor = document.getElementById("GvQuoteRefList_chkToolAmortizeRef_" + (i - 1) + "");
                            var CMachineAmor = document.getElementById("GvQuoteRefList_chkMachineAmortizeRef_" + (i - 1) + "");
                            var DToolAmor = document.getElementById("GvQuoteRefList_DdlToolAmortizeRef_" + (i - 1) + "");
                            var DMachineAmor = document.getElementById("GvQuoteRefList_DdlMachineAmortizeRef_" + (i - 1) + "");

                            if (CSubMat.checked == true) {
                                DToolAmor.removeAttribute('disabled');
                            }
                            else {
                                DToolAmor.setAttribute("disabled", "disabled");
                                DToolAmor.value = "REMOVE";
                                CToolAmor.checked = false;
                            }

                            if (CProc.checked == true && CSubMat.checked == true) {
                                DMachineAmor.removeAttribute('disabled');
                            }
                            else {
                                DMachineAmor.setAttribute("disabled", "disabled");
                                DMachineAmor.value = "REMOVE";
                                CMachineAmor.checked = false;
                            }
                        }
                    }
                    else {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkSubMatRef_" + (i - 1) + "").checked = false;
                            document.getElementById("GvQuoteRefList_chkAllRefRw_" + (i - 1) + "").checked = false;

                            var CSubMat = document.getElementById("GvQuoteRefList_chkSubMatRef_" + (i - 1) + "");
                            var CProc = document.getElementById("GvQuoteRefList_chkProcRef_" + (i - 1) + "");
                            var CToolAmor = document.getElementById("GvQuoteRefList_chkToolAmortizeRef_" + (i - 1) + "");
                            var CMachineAmor = document.getElementById("GvQuoteRefList_chkMachineAmortizeRef_" + (i - 1) + "");
                            var DToolAmor = document.getElementById("GvQuoteRefList_DdlToolAmortizeRef_" + (i - 1) + "");
                            var DMachineAmor = document.getElementById("GvQuoteRefList_DdlMachineAmortizeRef_" + (i - 1) + "");

                            if (CSubMat.checked == true) {
                                DToolAmor.removeAttribute('disabled');
                            }
                            else {
                                DToolAmor.setAttribute("disabled", "disabled");
                                DToolAmor.value = "REMOVE";
                                CToolAmor.checked = false;
                            }

                            if (CProc.checked == true && CSubMat.checked == true) {
                                DMachineAmor.removeAttribute('disabled');
                            }
                            else {
                                DMachineAmor.setAttribute("disabled", "disabled");
                                DMachineAmor.value = "REMOVE";
                                CMachineAmor.checked = false;
                            }
                        }
                    }
                    IsAllGvRowRefCheckFromHeaderClick();
                }
            }
            catch (err) {
                alert(err + ": CheckAllSubMatCostRef")
            }
        }

        function CheckAllOthCostRef() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    var All = document.getElementById("GvQuoteRefList_chkAllOthRef");
                    if (All.checked == true) {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkOthRef_" + (i - 1) + "").checked = true;
                        }
                    }
                    else {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkOthRef_" + (i - 1) + "").checked = false;
                            document.getElementById("GvQuoteRefList_chkAllRefRw_" + (i - 1) + "").checked = false;
                        }
                    }
                    IsAllGvRowRefCheckFromHeaderClick();
                }
            }
            catch (err) {
                alert(err + ": CheckAllOthCostRef")
            }
        }

        function CheckAllToolAmorRef() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    var All = document.getElementById("GvQuoteRefList_chkAllToolAmortizeRef");
                    if (All.checked == true) {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkToolAmortizeRef_" + (i - 1) + "").checked = true;
                        }
                    }
                    else {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkToolAmortizeRef_" + (i - 1) + "").checked = false;
                            document.getElementById("GvQuoteRefList_chkAllRefRw_" + (i - 1) + "").checked = false;
                        }
                    }
                    IsAllGvRowRefCheckFromHeaderClick();
                }
            }
            catch (err) {
                alert(err + ": CheckAllToolAmorRef")
            }
        }

        function CheckAllmachineAmorRef() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    var All = document.getElementById("GvQuoteRefList_chkAllMachineAmortizeRef");
                    if (All.checked == true) {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkMachineAmortizeRef_" + (i - 1) + "").checked = true;
                        }
                    }
                    else {
                        for (var i = 1; i < RowsCountGv; i++) {
                            document.getElementById("GvQuoteRefList_chkMachineAmortizeRef_" + (i - 1) + "").checked = false;
                            document.getElementById("GvQuoteRefList_chkAllRefRw_" + (i - 1) + "").checked = false;
                        }
                    }
                    IsAllGvRowRefCheckFromHeaderClick();
                }
            }
            catch (err) {
                alert(err + ": CheckAllmachineAmorRef")
            }
        }

        function IsAllGvRowRefCheckFromHeaderClick() {
            try {
                var table = document.getElementById("GvQuoteRefList");
                if (table != null) {
                    var RowsCountGv = $('#GvQuoteRefList tr').length;
                    for (var i = 1; i < RowsCountGv; i++) {
                        var CAll = document.getElementById("GvQuoteRefList_chkAllRefRw_" + (i - 1) + "");
                        var CMat = document.getElementById("GvQuoteRefList_chkMatRef_" + (i - 1) + "");
                        var CProc = document.getElementById("GvQuoteRefList_chkProcRef_" + (i - 1) + "");
                        var CSubMat = document.getElementById("GvQuoteRefList_chkSubMatRef_" + (i - 1) + "");
                        var COth = document.getElementById("GvQuoteRefList_chkOthRef_" + (i - 1) + "");
                        if (CMat.checked == true && CProc.checked == true && CSubMat.checked == true && COth.checked == true) {
                            CAll.checked = true;
                        }
                        else {
                            CAll.checked = false;
                        }
                    }
                }
            }
            catch (err) {
                alert(err + ": IsAllGvRowRefCheckFromHeaderClick()")
            }
        }

        function HideBtnSubmit() {
            try {
                var Btn = document.getElementById("BtnSubmitRequest");
                var Table = document.getElementById("GvCreateReqTemp");
                if (Btn != null) {
                    document.getElementById("BtnSubmitRequest").style.display = "none";
                }
                if (Table != null) {
                    document.getElementById("GvCreateReqTemp").style.display = "none";
                }
            }
            catch (err) {
                alert(err + ":HideBtnSubmit")
            }
        }

        function chkAllCostClick(chkAllCost, ChkMatCost, chkProcCost, chkSubMatCost, chkOthCost, ToolAmor, MacAmor, DdlToolAmortizeID, DdlMachineAmortizeID) {
            try {

                var Allcost = document.getElementById(chkAllCost).checked;
                var DdlToolAmortize = document.getElementById(DdlToolAmortizeID);
                var DdlMachineAmortize = document.getElementById(DdlMachineAmortizeID);

                if (Allcost == true) {
                    document.getElementById(ChkMatCost).checked = true;
                    document.getElementById(chkProcCost).checked = true;
                    document.getElementById(chkSubMatCost).checked = true;
                    document.getElementById(chkOthCost).checked = true;

                    document.getElementById(ToolAmor).checked = true;
                    document.getElementById(MacAmor).checked = true;

                    DdlToolAmortize.removeAttribute('disabled');
                    DdlMachineAmortize.removeAttribute('disabled');

                    $("#" + ChkMatCost + "").css("outline-color", "#CCCCCC");
                    $("#" + ChkMatCost + "").css("outline-style", "solid");
                    $("#" + ChkMatCost + "").css("outline-width", "0px");

                    $("#" + chkProcCost + "").css("outline-color", "#CCCCCC");
                    $("#" + chkProcCost + "").css("outline-style", "solid");
                    $("#" + chkProcCost + "").css("outline-width", "0px");

                    $("#" + chkSubMatCost + "").css("outline-color", "#CCCCCC");
                    $("#" + chkSubMatCost + "").css("outline-style", "solid");
                    $("#" + chkSubMatCost + "").css("outline-width", "0px");

                    $("#" + chkOthCost + "").css("outline-color", "#CCCCCC");
                    $("#" + chkOthCost + "").css("outline-style", "solid");
                    $("#" + chkOthCost + "").css("outline-width", "0px");

                    $("#" + ToolAmor + "").css("outline-color", "#CCCCCC");
                    $("#" + ToolAmor + "").css("outline-style", "solid");
                    $("#" + ToolAmor + "").css("outline-width", "0px");

                    $("#" + MacAmor + "").css("outline-color", "#CCCCCC");
                    $("#" + MacAmor + "").css("outline-style", "solid");
                    $("#" + MacAmor + "").css("outline-width", "0px");
                }
                else if (Allcost == false) {
                    document.getElementById(ChkMatCost).checked = false;
                    document.getElementById(chkProcCost).checked = false;
                    document.getElementById(chkSubMatCost).checked = false;
                    document.getElementById(chkOthCost).checked = false;

                    document.getElementById(ToolAmor).checked = false;
                    document.getElementById(MacAmor).checked = false;

                    DdlToolAmortize.setAttribute("disabled", "disabled");
                    DdlMachineAmortize.setAttribute("disabled", "disabled");
                    DdlToolAmortize.value = "REMOVE";
                    DdlMachineAmortize.value = "REMOVE";

                    $("#" + ChkMatCost + "").css("outline-color", "#ff0000");
                    $("#" + ChkMatCost + "").css("outline-style", "solid");
                    $("#" + ChkMatCost + "").css("outline-width", "1px");
                    $("#" + ChkMatCost + "").css("padding", "0px");

                    $("#" + chkProcCost + "").css("outline-color", "#ff0000");
                    $("#" + chkProcCost + "").css("outline-style", "solid");
                    $("#" + chkProcCost + "").css("outline-width", "1px");
                    $("#" + chkProcCost + "").css("padding", "0px");

                    $("#" + chkSubMatCost + "").css("outline-color", "#ff0000");
                    $("#" + chkSubMatCost + "").css("outline-style", "solid");
                    $("#" + chkSubMatCost + "").css("outline-width", "1px");
                    $("#" + chkSubMatCost + "").css("padding", "0px");

                    $("#" + chkOthCost + "").css("outline-color", "#ff0000");
                    $("#" + chkOthCost + "").css("outline-style", "solid");
                    $("#" + chkOthCost + "").css("outline-width", "1px");
                    $("#" + chkOthCost + "").css("padding", "0px");

                    $("#" + ToolAmor + "").css("outline-color", "#ff0000");
                    $("#" + ToolAmor + "").css("outline-style", "solid");
                    $("#" + ToolAmor + "").css("outline-width", "1px");
                    $("#" + ToolAmor + "").css("padding", "0px");

                    $("#" + MacAmor + "").css("outline-color", "#ff0000");
                    $("#" + MacAmor + "").css("outline-style", "solid");
                    $("#" + MacAmor + "").css("outline-width", "1px");
                    $("#" + MacAmor + "").css("padding", "0px");

                }

                //GetVndToolSelected();
                HideBtnSubmit();
            }
            catch (err) {
                alert(err + ": chkAllCostClick")
            }
        }

        function chkCostClick(chkAllCost, ChkMatCost, chkProcCost, chkSubMatCost, chkOthCost, chkToolAmor, chkMacAmor, DdlToolAmortizeID, DdlMachineAmortizeID) {
            try {

                var MatCost = document.getElementById(ChkMatCost).checked;
                var ProcCost = document.getElementById(chkProcCost).checked;
                var SubMatCost = document.getElementById(chkSubMatCost).checked;
                var OthCost = document.getElementById(chkOthCost).checked;

                var ToolAmor = document.getElementById(chkToolAmor).checked;
                var MacAmor = document.getElementById(chkMacAmor).checked;

                var DdlToolAmortize = document.getElementById(DdlToolAmortizeID);
                var DdlMachineAmortize = document.getElementById(DdlMachineAmortizeID);

                if (MatCost == true && ProcCost == true && SubMatCost == true && OthCost == true && ToolAmor == true && MacAmor == true) {
                    document.getElementById(chkAllCost).checked = true;
                }
                else {
                    document.getElementById(chkAllCost).checked = false;
                }

                if (MatCost == true || ProcCost == true || SubMatCost == true || OthCost == true || ToolAmor == true || MacAmor == true) {
                    $("#" + ChkMatCost + "").css("outline-color", "#CCCCCC");
                    $("#" + ChkMatCost + "").css("outline-style", "solid");
                    $("#" + ChkMatCost + "").css("outline-width", "0px");

                    $("#" + chkProcCost + "").css("outline-color", "#CCCCCC");
                    $("#" + chkProcCost + "").css("outline-style", "solid");
                    $("#" + chkProcCost + "").css("outline-width", "0px");

                    $("#" + chkSubMatCost + "").css("outline-color", "#CCCCCC");
                    $("#" + chkSubMatCost + "").css("outline-style", "solid");
                    $("#" + chkSubMatCost + "").css("outline-width", "0px");

                    $("#" + chkOthCost + "").css("outline-color", "#CCCCCC");
                    $("#" + chkOthCost + "").css("outline-style", "solid");
                    $("#" + chkOthCost + "").css("outline-width", "0px");

                    $("#" + chkToolAmor + "").css("outline-color", "#CCCCCC");
                    $("#" + chkToolAmor + "").css("outline-style", "solid");
                    $("#" + chkToolAmor + "").css("outline-width", "0px");

                    $("#" + chkMacAmor + "").css("outline-color", "#CCCCCC");
                    $("#" + chkMacAmor + "").css("outline-style", "solid");
                    $("#" + chkMacAmor + "").css("outline-width", "0px");
                }
                else {
                    $("#" + ChkMatCost + "").css("outline-color", "#ff0000");
                    $("#" + ChkMatCost + "").css("outline-style", "solid");
                    $("#" + ChkMatCost + "").css("outline-width", "1px");
                    $("#" + ChkMatCost + "").css("padding", "0px");

                    $("#" + chkProcCost + "").css("outline-color", "#ff0000");
                    $("#" + chkProcCost + "").css("outline-style", "solid");
                    $("#" + chkProcCost + "").css("outline-width", "1px");
                    $("#" + chkProcCost + "").css("padding", "0px");

                    $("#" + chkSubMatCost + "").css("outline-color", "#ff0000");
                    $("#" + chkSubMatCost + "").css("outline-style", "solid");
                    $("#" + chkSubMatCost + "").css("outline-width", "1px");
                    $("#" + chkSubMatCost + "").css("padding", "0px");

                    $("#" + chkOthCost + "").css("outline-color", "#ff0000");
                    $("#" + chkOthCost + "").css("outline-style", "solid");
                    $("#" + chkOthCost + "").css("outline-width", "1px");
                    $("#" + chkOthCost + "").css("padding", "0px");

                    $("#" + chkToolAmor + "").css("outline-color", "#ff0000");
                    $("#" + chkToolAmor + "").css("outline-style", "solid");
                    $("#" + chkToolAmor + "").css("outline-width", "1px");
                    $("#" + chkToolAmor + "").css("padding", "0px");

                    $("#" + chkMacAmor + "").css("outline-color", "#ff0000");
                    $("#" + chkMacAmor + "").css("outline-style", "solid");
                    $("#" + chkMacAmor + "").css("outline-width", "1px");
                    $("#" + chkMacAmor + "").css("padding", "0px");
                }

                if (SubMatCost == true) {
                    DdlToolAmortize.removeAttribute('disabled');
                    if (ProcCost == true) {
                        DdlMachineAmortize.removeAttribute('disabled');
                    }
                    else {
                        DdlMachineAmortize.setAttribute("disabled", "disabled");
                        document.getElementById(chkMacAmor).checked = false;
                    }
                }
                else {
                    DdlToolAmortize.setAttribute("disabled", "disabled");
                    DdlMachineAmortize.setAttribute("disabled", "disabled");
                    DdlToolAmortize.value = "REMOVE";
                    DdlMachineAmortize.value = "REMOVE";
                    document.getElementById(chkToolAmor).checked = false;
                    document.getElementById(chkMacAmor).checked = false;
                }

                GetVndToolSelected();
                HideBtnSubmit();
            }
            catch (err) {
                alert(err + ": chkCostClick")
            }
        }

        function DdlAmortizeChange(chkAllCost, ChkMatCost, chkProcCost, chkSubMatCost, chkOthCost, chkToolAmor, chkMacAmor, DdlToolAmortizeID, DdlMachineAmortizeID, TxtMQtyID) {
            try {

                var CToolAmor = document.getElementById(chkToolAmor);
                var CMacAmor = document.getElementById(chkMacAmor);

                var DdlToolAmortize = document.getElementById(DdlToolAmortizeID);
                var DdlMachineAmortize = document.getElementById(DdlMachineAmortizeID);

                if (DdlToolAmortize.value == "ADD") {
                    CToolAmor.checked = true;
                    SetMyFocusID(TxtMQtyID.toString());
                }
                else {
                    CToolAmor.checked = false;
                }

                if (DdlMachineAmortize.value == "ADD") {
                    CMacAmor.checked = true;
                }
                else {
                    CMacAmor.checked = false;
                }

                GetVndToolSelected();
                HideBtnSubmit();
            }
            catch (err) {
                alert(err + ": DdlAmortizeChange")
            }
        }

        function valCreateReq() {
            var table = document.getElementById('GvTemp');
            var err = "";
            err += "Please check field listed in below : \n";
            var iserr = false;

            if (table != null) {
                var RowsGvTempC = $('#TxtGvTempLeng').val();
                for (var r = 1; r <= RowsGvTempC; r++) {

                    var fileName = document.getElementById("GvTemp_lbFileName_" + (r - 1) + "").innerHTML;
                    var DdlReasonIdx = $("#GvTemp_DdlReason_" + (r - 1) + "")[0].selectedIndex;
                    var cMatCost = document.getElementById("GvTemp_chkMatCost_" + (r - 1) + "").checked;
                    var cProcCost = document.getElementById("GvTemp_chkProcCost_" + (r - 1) + "").checked;
                    var cSubMatCost = document.getElementById("GvTemp_chkSubMatCost_" + (r - 1) + "").checked;
                    var cOthCost = document.getElementById("GvTemp_chkOthCost_" + (r - 1) + "").checked;

                    var cToolAmor = document.getElementById("GvTemp_chkToolAmortize_" + (r - 1) + "").checked;
                    var cmachineAmor = document.getElementById("GvTemp_chkMachineAmortize_" + (r - 1) + "").checked;

                    var MQty = $("#GvTemp_TxtMQty_" + (r - 1) + "").val();
                    var BaseUOM = $("#GvTemp_TxtBaseUOM_" + (r - 1) + "").val();
                    var ResDueDate = $("#GvTemp_TxtResDueDate_" + (r - 1) + "").val();
                    var EffectiveDate = $("#GvTemp_TxtEffectiveDate_" + (r - 1) + "").val();
                    var DueDateNextRev = $("#GvTemp_TxtDueDateNextRev_" + (r - 1) + "").val();
                    //var QuoteNo = table.rows[r].cells[1].innerHTML;
                    var QuoteNo = document.getElementById("GvTemp_LbQNModal_" + (r - 1) + "").innerHTML;

                    if (cMatCost == false && cProcCost == false && cSubMatCost == false && cOthCost == false && cToolAmor == false && cmachineAmor == false) {
                        err += "Please select at least one cost for Quote No : " + QuoteNo + " !! \n";
                        $("#GvTemp_chkMatCost_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkMatCost_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkMatCost_" + (r - 1) + "").css("outline-width", "1px");

                        $("#GvTemp_chkProcCost_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkProcCost_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkProcCost_" + (r - 1) + "").css("outline-width", "1px");

                        $("#GvTemp_chkSubMatCost_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkSubMatCost_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkSubMatCost_" + (r - 1) + "").css("outline-width", "1px");

                        $("#GvTemp_chkOthCost_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkOthCost_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkOthCost_" + (r - 1) + "").css("outline-width", "1px");

                        $("#GvTemp_chkToolAmortize_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkToolAmortize_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkToolAmortize_" + (r - 1) + "").css("outline-width", "1px");
                        $("#GvTemp_chkToolAmortize_" + (r - 1) + "").css("padding", "0px");

                        $("#GvTemp_chkMachineAmortize_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkMachineAmortize_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkMachineAmortize_" + (r - 1) + "").css("outline-width", "1px");
                        $("#GvTemp_chkMachineAmortize_" + (r - 1) + "").css("padding", "0px");

                        iserr = true;
                    }

                    if (cToolAmor == true) {
                        var tableToolAmor = document.getElementById("GvTemp_GvDetToolAmor_" + (r - 1) + "");
                        if (tableToolAmor != null) {
                            var rowscount = tableToolAmor.rows.length;
                            if (rowscount > 1) {
                                var found = false;
                                for (var v = 0; v < rowscount; v++) {
                                    var chk = document.getElementById("GvTemp_GvDetToolAmor_" + (r - 1) + "_chkVndToolAmor_" + v + "");
                                    if (chk != null) {
                                        if (chk.checked == true) {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                                if (found == false) {
                                    err += "Please Select Tool Amortize for Quote No : " + QuoteNo + "  !! \n";
                                    iserr = true;
                                }
                            }
                            else {
                                err += "No Tool Amortize Found for Quote No : " + QuoteNo + " !! \n ";
                                iserr = true;
                            }
                        }
                        else {
                            err += "No Tool Amortize Found for Quote No : " + QuoteNo + " !! \n ";
                            iserr = true;
                        }
                    }

                    if (DdlReasonIdx <= 0) {
                        err += "Please select Reason for Quote No : " + QuoteNo + " !! \n";
                        document.getElementById("GvTemp_DdlReason_" + (r - 1) + "").style.border = "1px solid #ff0000";
                        iserr = true;
                    }


                    if (MQty == "") {
                        err += "Please enter Mnth.Est.Qty for Quote No : " + QuoteNo + " !! \n";
                        document.getElementById("GvTemp_TxtMQty_" + (r - 1) + "").style.border = "1px solid #ff0000";
                        iserr = true;
                    }

                    if (BaseUOM == "") {
                        err += "Please enter Base UOM for Quote No : " + QuoteNo + " !! \n";
                        document.getElementById("GvTemp_TxtBaseUOM_" + (r - 1) + "").style.border = "1px solid #ff0000";
                        iserr = true;
                    }

                    if (ResDueDate == "") {
                        err += "please select response due date for Quote No : " + QuoteNo + " !! \n";
                        document.getElementById("GvTemp_TxtResDueDate_" + (r - 1) + "").style.border = "1px solid #ff0000";
                        iserr = true;
                    }

                    if (EffectiveDate == "") {
                        err += "please select Effective Date for Quote No : " + QuoteNo + " !! \n";
                        document.getElementById("GvTemp_TxtEffectiveDate_" + (r - 1) + "").style.border = "1px solid #ff0000";
                        iserr = true;
                    }

                    if (DueDateNextRev == "") {
                        err += "please select Due date Next Rev for Quote No : " + QuoteNo + " !! \n";
                        document.getElementById("GvTemp_TxtDueDateNextRev_" + (r - 1) + "").style.border = "1px solid #ff0000";
                        iserr = true;
                    }

                    if (EffectiveDate != "" && DueDateNextRev != "") {
                        var StrEffDate = EffectiveDate.toString().replace(/\-/g, '.');
                        var strDueOn = DueDateNextRev.toString().replace(/\-/g, '.');

                        var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
                        var dtEffDate = new Date(StrEffDate.replace(pattern, '$3-$2-$1'));
                        var dtDueOn = new Date(strDueOn.replace(pattern, '$3-$2-$1'));

                        if (dtEffDate > dtDueOn) {
                            err += " Due Dt Next Rev cannot be small than Effective date for Quote No : " + QuoteNo + " !! \n";
                            document.getElementById("GvTemp_TxtDueDateNextRev_" + (r - 1) + "").style.border = "1px solid #ff0000";
                            document.getElementById("GvTemp_TxtEffectiveDate_" + (r - 1) + "").style.border = "1px solid #ff0000";
                            iserr = true;
                        }
                    }

                    var PG = table.rows[r].cells[6].innerHTML;;
                    var layout = "";
                    var GvLL = document.getElementById('GvlayoutList');
                    var rowscount = GvLL.rows.length;
                    for (var l = 1; l <= rowscount; l++) {
                        var GvLLPrcGrp = GvLL.rows[l].cells[0].innerHTML;
                        if (GvLLPrcGrp.toString().toUpperCase() == PG.toString().toUpperCase()) {
                            layout = GvLL.rows[l].cells[1].innerHTML;
                            break;
                        }
                    }
                    if (layout.toString().toUpperCase() == "LAYOUT1") {
                        var DdlRR = document.getElementById("GvTemp_DdlRecycleRatio_" + (r - 1) + "");
                        var DdlRRVal = DdlRR.options[DdlRR.selectedIndex].value;
                        if (DdlRRVal == "00") {
                            err += "Please select Recycle Ratio for Quote No : " + QuoteNo + " !! \n";
                            document.getElementById("GvTemp_DdlRecycleRatio_" + (r - 1) + "").style.border = "1px solid #ff0000";
                            iserr = true;
                        }
                        else if (DdlRRVal == "NO DATA") {
                            err += "Please Contact Administrator to maintain Recycle Ratio !! \n";
                            document.getElementById("GvTemp_DdlRecycleRatio_" + (r - 1) + "").style.border = "1px solid #ff0000";
                            iserr = true;
                        }
                    }


                    //if (fileName == "" || fileName == "No File") {
                    //    err += "No Attachment, please select file for Quote No : " + QuoteNo + " !! \n";
                    //    document.getElementById("GvTemp_lbFileName_" + (r - 1) + "").style.border = "1px solid #ff0000";
                    //    iserr = true;
                    //}
                }
            }

            if (iserr == true) {
                alert(err);
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


        function TrigerFlUploadClick(BtnID) {
            try {
                $('#' + BtnID).click();
            }
            catch (err) {
                alert(err + ": FocusToTxt(iconid,txtid)")
            }
        }

        function ChgEmptyFlColor() {
            var table = document.getElementById('GvTemp');
            if (table != null) {
                var RowsGvTempC = $('#TxtGvTempLeng').val();
                for (var r = 1; r <= RowsGvTempC; r++) {
                    var fileName = document.getElementById("GvTemp_lbFileName_" + (r - 1) + "").innerHTML;
                    var DdlReasonIdx = $("#GvTemp_DdlReason_" + (r - 1) + "")[0].selectedIndex;
                    var cMatCost = document.getElementById("GvTemp_chkMatCost_" + (r - 1) + "").checked;
                    var cProcCost = document.getElementById("GvTemp_chkProcCost_" + (r - 1) + "").checked;
                    var cSubMatCost = document.getElementById("GvTemp_chkSubMatCost_" + (r - 1) + "").checked;
                    var cOthCost = document.getElementById("GvTemp_chkOthCost_" + (r - 1) + "").checked;

                    var cToolAmor = document.getElementById("GvTemp_chkToolAmortize_" + (r - 1) + "").checked;
                    var cmachineAmor = document.getElementById("GvTemp_chkMachineAmortize_" + (r - 1) + "").checked;

                    var MQty = $("#GvTemp_TxtMQty_" + (r - 1) + "").val();
                    var BaseUOM = $("#GvTemp_TxtBaseUOM_" + (r - 1) + "").val();
                    var ResDueDate = $("#GvTemp_TxtResDueDate_" + (r - 1) + "").val();
                    var EffectiveDate = $("#GvTemp_TxtEffectiveDate_" + (r - 1) + "").val();
                    var DueDateNextRev = $("#GvTemp_TxtDueDateNextRev_" + (r - 1) + "").val();
                    //if (fileName == "" || fileName == "No File") {
                    //    document.getElementById("GvTemp_lbFileName_" + (r - 1) + "").style.border = "1px solid #ff0000";
                    //}

                    if (DdlReasonIdx <= 0) {
                        document.getElementById("GvTemp_DdlReason_" + (r - 1) + "").style.border = "1px solid #ff0000";
                    }

                    if (cMatCost == false && cProcCost == false && cSubMatCost == false && cOthCost == false && cToolAmor == false && cmachineAmor == false) {
                        $("#GvTemp_chkMatCost_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkMatCost_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkMatCost_" + (r - 1) + "").css("outline-width", "1px");
                        $("#GvTemp_chkMatCost_" + (r - 1) + "").css("padding", "0px");

                        $("#GvTemp_chkProcCost_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkProcCost_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkProcCost_" + (r - 1) + "").css("outline-width", "1px");
                        $("#GvTemp_chkProcCost_" + (r - 1) + "").css("padding", "0px");

                        $("#GvTemp_chkSubMatCost_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkSubMatCost_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkSubMatCost_" + (r - 1) + "").css("outline-width", "1px");
                        $("#GvTemp_chkSubMatCost_" + (r - 1) + "").css("padding", "0px");

                        $("#GvTemp_chkOthCost_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkOthCost_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkOthCost_" + (r - 1) + "").css("outline-width", "1px");
                        $("#GvTemp_chkOthCost_" + (r - 1) + "").css("padding", "0px");

                        $("#GvTemp_chkToolAmortize_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkToolAmortize_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkToolAmortize_" + (r - 1) + "").css("outline-width", "1px");
                        $("#GvTemp_chkToolAmortize_" + (r - 1) + "").css("padding", "0px");

                        $("#GvTemp_chkMachineAmortize_" + (r - 1) + "").css("outline-color", "red");
                        $("#GvTemp_chkMachineAmortize_" + (r - 1) + "").css("outline-style", "solid");
                        $("#GvTemp_chkMachineAmortize_" + (r - 1) + "").css("outline-width", "1px");
                        $("#GvTemp_chkMachineAmortize_" + (r - 1) + "").css("padding", "0px");
                    }

                    if (MQty == "") {
                        document.getElementById("GvTemp_TxtMQty_" + (r - 1) + "").style.border = "1px solid #ff0000";
                    }

                    if (BaseUOM == "") {
                        document.getElementById("GvTemp_TxtBaseUOM_" + (r - 1) + "").style.border = "1px solid #ff0000";
                    }

                    if (ResDueDate == "") {
                        document.getElementById("GvTemp_TxtResDueDate_" + (r - 1) + "").style.border = "1px solid #ff0000";
                    }

                    if (EffectiveDate == "") {
                        document.getElementById("GvTemp_TxtEffectiveDate_" + (r - 1) + "").style.border = "1px solid #ff0000";
                    }

                    if (DueDateNextRev == "") {
                        document.getElementById("GvTemp_TxtDueDateNextRev_" + (r - 1) + "").style.border = "1px solid #ff0000";
                    }

                    var PG = table.rows[r].cells[6].innerHTML;;
                    var layout = "";
                    var GvLL = document.getElementById('GvlayoutList');
                    var rowscount = GvLL.rows.length;
                    for (var l = 1; l <= rowscount; l++) {
                        var GvLLPrcGrp = GvLL.rows[l].cells[0].innerHTML;
                        if (GvLLPrcGrp.toString().toUpperCase() == PG.toString().toUpperCase()) {
                            layout = GvLL.rows[l].cells[1].innerHTML;
                            break;
                        }
                    }
                    if (layout.toString().toUpperCase() == "LAYOUT1") {
                        var DdlRR = document.getElementById("GvTemp_DdlRecycleRatio_" + (r - 1) + "");
                        var DdlRRVal = DdlRR.options[DdlRR.selectedIndex].value;
                        if (DdlRRVal == "00") {
                            document.getElementById("GvTemp_DdlRecycleRatio_" + (r - 1) + "").style.border = "1px solid #ff0000";
                        }
                        else if (DdlRRVal == "NO DATA") {
                            document.getElementById("GvTemp_DdlRecycleRatio_" + (r - 1) + "").style.border = "1px solid #ff0000";
                        }
                    }
                }
            }
        }

        function UploadFile() {
            $("#BtnUpload").click();
        }

        function ValidatePreviewFile(lbFileNameID, LbPreviewID) {
            try {
                var fileName = document.getElementById(lbFileNameID).innerHTML;
                if (fileName == "" || fileName == "No File") {
                    alert('No Attachment, please select file !!');
                    return false;
                }
                else {
                    //$("#BtnPreview").click();
                    return true;
                }
            }
            catch (err) {
                alert(err + ":PreviewUplFile()")
            }
        }

        function ConfirmAddList() {
            try {
                var c = confirm("Add More Data into the List ?");
                if (c == false) {
                    CloseModalQuoteRef();
                }
            }
            catch (err) {
                alert(err + 'ConfirmAddList')
            }
        }

        function ConfirmChangeVendorType() {
            try {
                var c = confirm("Change Vendor Type will reset data in the list, do you want to continue ?");
                if (c == true) {

                }
                else {

                }
            }
            catch (err) {
                alert(err + 'ConfirmChangeVendorType')
            }
        }

        function OpenMdQuoteCostPlant() {
            try {
                jQuery.noConflict();
                $("#MdQuoteCostPlant").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            catch (err) {
                alert(err + ' : OpenMdQuoteCostPlant');
            }
        }

        function CloseMdQuoteCostPlant() {
            try {
                jQuery.noConflict();
                $("#MdQuoteCostPlant").modal('hide');
            }
            catch (err) {
                alert(err + ' : CloseMdQuoteCostPlant');
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
        
        function ClcBtnFlUpload() {
            try {
                $('#FlAttachment').trigger('click');
            }
            catch (err) {
                alert(err + ": ClcBtnFlUpload")
            }
        }
        
        function HiglightGrid() {
            try {
                $(function () {
                    $(document).on('click', '#GvQuoteRefList td', function () {
                        var color = $(this).css("background-color").toString();
                        //alert(color);
                        if (color == "rgb(242, 242, 242)") {
                            $(this).css({ background: "#ffff4d" });
                        }
                        else if (color == "rgb(255, 255, 77)") {
                            $(this).css({ background: "#ffffff" });
                        }
                        //$("#GvQuoteRefList tr").removeClass('selectCell');
                    });
                });
            }
            catch (err) {
                alert(err + ": HiglightGrid")
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
        
        function TrigerPreviewClick() {
            try {
                $("#BtnPreview").click();
            }
            catch (err) {
                alert(err + ' : freezeheader');
            }
        }
        
        function FocusToTxtt(id, txtid) {
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
        
        function FocusToTxt(iconid, txtid) {
            try {
                $('#' + txtid).focus();
            }
            catch (err) {
                alert(err + ": FocusToTxt(iconid,txtid)")
            }
        }

        

        function SetMyFocusID(id) {
            try {
                document.getElementById("TxtDefFocus").value = id;
            } catch (e) {
                alert("SetMyFocusID : " + e);
            }
        }

        function SetMyFocus() {
            try {
                var id = document.getElementById("TxtDefFocus").value;
                if (id != null || id != "") {
                    var ctrl = document.getElementById(id);
                    if (ctrl != null) {
                        ctrl.focus();
                        document.getElementById("TxtDefFocus").value = "";
                    }
                }
                RestoreVndToolSelected();

            } catch (e) {
                alert("SetMyFocus : " + e);
            }
        }
        
    </script>


    <script type="text/javascript">
        //$(function () {
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
        //}); (jQuery)
    </script>


    <%--Generate DataTable--%>
    <script type="text/javascript">
        function checkPlantStatusAndLayout() {
            if (typeof (Worker) !== "undefined") {
                var checkPlantStatusAndLayout = dataTableTbQuoteRefList.data();
                var LAYOUT7Found = false;
                var InActiveMat = false;

                for (var i = 0; i < checkPlantStatusAndLayout.length; i++) {
                    if (dataTableTbQuoteRefList.data()[i].MMPlantStatus.toString() == "Z4" || dataTableTbQuoteRefList.data()[i].MMPlantStatus.toString() == "Z9") {
                        InActiveMat = true;
                        break;
                    }

                    if (dataTableTbQuoteRefList.data()[i].Layout.toString() == "LAYOUT7") {
                        LAYOUT7Found = true;
                        break;
                    }
                }

                if (InActiveMat == true || LAYOUT7Found == true) {
                    $("#chkAllRefHd").attr("disabled", true);
                    $("#chkAllMatRef").attr("disabled", true);
                    $("#chkAllProcRef").attr("disabled", true);
                    $("#chkAllSubMatRef").attr("disabled", true);
                    $("#chkAllOthRef").attr("disabled", true);
                }
                else {
                    $("#chkAllRefHd").attr("disabled", false);
                    $("#chkAllMatRef").attr("disabled", false);
                    $("#chkAllProcRef").attr("disabled", false);
                    $("#chkAllSubMatRef").attr("disabled", false);
                    $("#chkAllOthRef").attr("disabled", false);
                }
            }
            else {
                alert('checkPlantStatusAndLayout : Browser Not Support')
            }
        }

        function GenerateTbQuoteRefList() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableTbQuoteRefList = $("#TbQuoteRefList").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $(".paginate_button").click(function () {
                                currentPage = dataTableTbQuoteRefList.page.info().page;
                            });

                            $(".ToolAmortizeDropdown").each(function (index) {
                                var selectedvalue = $(this).val();
                                $(this).attr("defaultValue", selectedvalue);
                            });

                            $(".MachineAmortizeDropdown").each(function (index) {
                                var selectedvalue = $(this).val();
                                $(this).attr("defaultValue", selectedvalue);
                            });

                        },
                        "deferRender": false,
                        "columns": [
                        {
                            "data": "QuoteNo",
                            "render": function (data, type, row, meta) {
                                if (type === 'display') {
                                    var url = "QQPReview.aspx?Number=" + data;
                                    return '<a id="LbQRef_' + row.QuoteNo + '" onclick="openInNewTab2(\'' + url + '\')">' + data + '</a>';
                                }
                                return data;
                            },
                            "autoWidth": true
                        },
                        { "data": "VendorCode1", "autoWidth": true },
                        { "data": "Product", "autoWidth": true },
                        { "data": "MaterialClass", "autoWidth": true },
                        { "data": "Material", "autoWidth": true },
                        { "data": "MaterialDesc", "autoWidth": true },
                        { "data": "ProcessGroup", "autoWidth": true },
                        { "data": "PrcGrpDesc", "autoWidth": true },
                        {
                            "data": null,
                            "render": function (data, type, row, meta) {
                                if (type === 'display') {
                                    var disabledCbb = "";
                                    if (row.MMPlantStatus == "Z4" || row.MMPlantStatus == "Z9" || row.Layout == "LAYOUT7") {
                                        disabledCbb = "disabled";
                                    }
                                    return '<input type="checkbox" class="checkAllRefRw" attrrow="' + meta.row + '" id="chkAllRefRw_' + row.QuoteNo + '" onclick="chkAllRefRw_Click(\'' + row.QuoteNo + '\')" ' + disabledCbb + '/>';
                                }
                                return data;
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "TotalMaterialCost",
                            "render": function (data, type, row, meta) {
                                var disabledCbb = "";
                                if (row.MMPlantStatus == "Z4" || row.MMPlantStatus == "Z9") {
                                    disabledCbb = "disabled";
                                }
                                return ' <input type="checkbox" class="checkAllMaterialCost" attrrow="' + meta.row + '" id="chkMatRef_' + row.QuoteNo + '"  onclick="chkDet_Click(\'' + row.QuoteNo + '\');CheckBasedOnColumn(\'chkMatRef_' + row.QuoteNo + '\');" ' + disabledCbb + '/><label for="chkMatRef_' + row.QuoteNo + '" id="LbchkMatRef_' + row.QuoteNo + '"> ' + "&nbsp;" + data + '  </label> '
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "TotalProcessCost",
                            "render": function (data, type, row, meta) {
                                var disabledCbb = "";
                                if (row.MMPlantStatus == "Z4" || row.MMPlantStatus == "Z9" || row.Layout == "LAYOUT7") {
                                    disabledCbb = "disabled";
                                }
                                return ' <input type="checkbox" class="checkAllProcessCost" attrrow="' + meta.row + '" id="chkProcRef_' + row.QuoteNo + '" onclick="chkDet_Click(\'' + row.QuoteNo + '\'); CheckBasedOnColumn(\'chkProcRef_' + row.QuoteNo + '\');" ' + disabledCbb + '/><label for="chkProcRef_' + row.QuoteNo + '" id="LbchkProcRef_' + row.QuoteNo + '"> ' + "&nbsp;" + data + '  </label> '
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "TotalSubMaterialCost",
                            "render": function (data, type, row, meta) {
                                var disabledCbb = "";
                                if (row.MMPlantStatus == "Z4" || row.MMPlantStatus == "Z9" || row.Layout == "LAYOUT7") {
                                    disabledCbb = "disabled";
                                }
                                return ' <input type="checkbox" class="checkAllSubMaterialCost" attrrow="' + meta.row + '" id="chkSubMatRef_' + row.QuoteNo + '" onclick="chkDet_Click(\'' + row.QuoteNo + '\');CheckBasedOnColumn(\'chkSubMatRef_' + row.QuoteNo + '\');" ' + disabledCbb + ' /><label for="chkSubMatRef_' + row.QuoteNo + '" id="LbchkSubMatRef_' + row.QuoteNo + '"> ' + "&nbsp;" + data + '  </label> '
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "TotalOtheritemsCost",
                            "render": function (data, type, row, meta) {
                                var disabledCbb = "";
                                if (row.MMPlantStatus == "Z4" || row.MMPlantStatus == "Z9") {
                                    disabledCbb = "disabled";
                                }
                                return ' <input type="checkbox" class="checkAllOtheritemsCost" attrrow="' + meta.row + '" id="chkOthRef_' + row.QuoteNo + '" onclick="chkDet_Click(\'' + row.QuoteNo + '\'); CheckBasedOnColumn(\'chkOthRef_' + row.QuoteNo + '\');" ' + disabledCbb + ' /><label for="chkOthRef_' + row.QuoteNo + '" id="LbchkOthRef_' + row.QuoteNo + '"> ' + "&nbsp;" + data + '  </label> '
                            },
                            "autoWidth": true
                        },
                        {
                            "data": null,
                            "render": function (data, type, row, meta) {
                                var ToolAmorExist = row.ToolAmorExist;
                                var ToolAmorExpired = row.ToolAmorExpired;
                                if (ToolAmorExist == false) {
                                    return ' <select id="DdlToolAmorRef_' + row.QuoteNo + '" disabled="true" class="ToolAmortizeDropdown"><option value="NO CHANGE">NO CHANGE</option> <option value="ADD">ADD</option><option value="REMOVE" disabled="disabled">REMOVE</option></select>'
                                }
                                else if (ToolAmorExpired == true) {
                                    return ' <select id="DdlToolAmorRef_' + row.QuoteNo + '" disabled="true" class="ToolAmortizeDropdown"></option><option value="NO CHANGE" disabled="disabled">NO CHANGE</option> <option value="ADD">ADD</option><option value="REMOVE" selected>REMOVE</option></select>'
                                }
                                else {
                                    return ' <select id="DdlToolAmorRef_' + row.QuoteNo + '" disabled="true" class="ToolAmortizeDropdown"><option value="NO CHANGE">NO CHANGE</option> <option value="ADD">ADD</option><option value="REMOVE">REMOVE</option></select>'
                                }
                            },
                            "autoWidth": true
                        },
                        {
                            "data": null,
                            "render": function (data, type, row, meta) {

                                var MacAmorExist = row.MacAmorExist;
                                var MacAmorExpired = row.MacAmorExpired;
                                //if (MacAmorExist == false) {
                                //    return ' <select id="DdlMachineAmortizeRef_' + row.QuoteNo + '" disabled="true" class="MachineAmortizeDropdown"><option value="NO CHANGE">NO CHANGE</option> <option value="ADD">ADD</option><option value="REMOVE" disabled="disabled">REMOVE</option></select>'
                                //}
                                //else if (MacAmorExpired == true) {
                                //    return ' <select id="DdlMachineAmortizeRef_' + row.QuoteNo + '" disabled="true" class="MachineAmortizeDropdown"></option><option value="NO CHANGE" disabled="disabled">NO CHANGE</option> <option value="ADD">ADD</option><option value="REMOVE" selected>REMOVE</option></select>'
                                //}
                                //else {
                                //    return ' <select id="DdlMachineAmortizeRef_' + row.QuoteNo + '" disabled="true" class="MachineAmortizeDropdown"><option value="NO CHANGE">NO CHANGE</option> <option value="ADD">ADD</option><option value="REMOVE">REMOVE</option></select>'
                                //}
                                if (MacAmorExist == false) {
                                    return ' <select id="DdlMachineAmortizeRef_' + row.QuoteNo + '" disabled="true" class="MachineAmortizeDropdown"><option value="NO CHANGE">NO CHANGE</option> <option value="REMOVE" disabled="disabled">REMOVE</option></select>'
                                }
                                else if (MacAmorExpired == true) {
                                    return ' <select id="DdlMachineAmortizeRef_' + row.QuoteNo + '" disabled="true" class="MachineAmortizeDropdown"></option><option value="NO CHANGE" disabled="disabled">NO CHANGE</option> <option value="REMOVE" selected>REMOVE</option></select>'
                                }
                                else {
                                    return ' <select id="DdlMachineAmortizeRef_' + row.QuoteNo + '" disabled="true" class="MachineAmortizeDropdown"><option value="NO CHANGE">NO CHANGE</option><option value="REMOVE">REMOVE</option></select>'
                                }
                            },
                            "autoWidth": true
                        },
                        { "data": "FinalQuotePrice", "autoWidth": true },
                        { "data": "ReqType", "autoWidth": true },
                        { "data": "MMPlantStatus", "autoWidth": true },

                        { "data": "ToolAmorExist", "autoWidth": true },
                        { "data": "ToolAmorExpired", "autoWidth": true },
                        { "data": "MacAmorExist", "autoWidth": true },
                        { "data": "MacAmorExpired", "autoWidth": true },
                        { "data": "Layout", "autoWidth": true },
                        ],
                        //paging: false,
                        //dom: 'lBfrtip',
                        //buttons: [
                        //    'copy', 'excel', 'print'
                        //],
                        "ordering": false,
                        language: {
                            "lengthMenu": "Show <input class='form-control form-control-sm' id='lcDatatables' value='10' style='width:70px; display:unset; text-align:center;' type='number' min='1'/> entries &nbsp; &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.QuoteNo;
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
                            dataTableTbQuoteRefList.page.len(1).draw();
                        } else {
                            dataTableTbQuoteRefList.page.len(length).draw();
                        }
                    });

                    $("#lcDatatables").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });


                    dataTableTbQuoteRefList.column(18).visible(false);
                    dataTableTbQuoteRefList.column(19).visible(false);
                    dataTableTbQuoteRefList.column(20).visible(false);
                    dataTableTbQuoteRefList.column(21).visible(false);
                    dataTableTbQuoteRefList.column(22).visible(false);
                } catch (e) {
                    alert("GenerateTbQuoteRefList() : " + e);
                }
            }
            else {
                alert('GenerateTbQuoteRefList : Browser Not Support')
            }
        }

        function proceedAddToList(_VendVsMat) {
            if (typeof (Worker) !== "undefined") {
                try {
                    //var length = $("#lcdatatables").val();
                    //if (length == "" || length == "0") {
                    //    length = "1";
                    //    $("#lcdatatables").val("1");
                    //}

                    var Mytable = $('#TbQuoteRefListSelected').DataTable();
                    var Mydata = Mytable.rows().data();
                    if (Mydata.length > 0) {
                        dataTableTbQuoteRefListSelected.rows.add(_VendVsMat).draw();
                        dataTableTbQuoteRefListSelected.columns.adjust().draw();
                    }
                    else {
                        dataTableTbQuoteRefListSelected.clear().draw();
                        dataTableTbQuoteRefListSelected.rows.add(_VendVsMat).draw();
                        dataTableTbQuoteRefListSelected.columns.adjust().draw();
                    }
                    //length change input textbox
                    //dataTableDuplicateWithExpiredReq.page.len(length).draw();

                    DatePitcker();
                } catch (e) {
                    alert(e);
                }
            }
            else {
                alert('proceedAddToList : Browser Not Support')
            }
        }

        function proceedAddToListInvalid(_VendVsMat) {
            if (typeof (Worker) !== "undefined") {
                try {
                    document.getElementById("DvInvalidRequest").style.display = "block";

                    var Mytable = $('#TbQuoteRefListInvalid').DataTable();
                    var Mydata = Mytable.rows().data();
                    if (Mydata.length > 0) {
                        dataTableTbQuoteRefListInvalid.rows.add(_VendVsMat).draw();
                        dataTableTbQuoteRefListInvalid.columns.adjust().draw();
                    }
                    else {
                        dataTableTbQuoteRefListInvalid.clear().draw();
                        dataTableTbQuoteRefListInvalid.rows.add(_VendVsMat).draw();
                        dataTableTbQuoteRefListInvalid.columns.adjust().draw();
                    }

                } catch (e) {
                    alert("proceedAddToListInvalid" + e);
                }
            }
            else {
                alert('proceedAddToListInvalid : Browser Not Support')
            }
        }

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
                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $(".paginate_button").click(function () {
                                currentPage = dataTableDuplicateWithExpiredReq.page.info().page;
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
                        { "data": "VendorName", "autoWidth": true },
                        {
                            "data": null,
                            "render": function (data, type, row, meta) {
                                if (type === 'display') {
                                    return '<input type="radio" id="RbRej_' + row.QuoteNo + '" name="RejOrChgDate_' + row.QuoteNo + '" onclick="RbRejectExpReq(\'' + row.QuoteNo + '\');IsAllRadioChecked(\'REJ\');" />';
                                }
                                return data;
                            },
                            "autoWidth": true
                        },
                        {
                            "data": null,
                            "render": function (data, type, row, meta) {
                                if (type === 'display') {
                                    return '<input type="radio" id="RbchangeDate_' + row.QuoteNo + '" name="RejOrChgDate_' + row.QuoteNo + '" onclick="RbChangedateResDueDate(\'' + row.QuoteNo + '\');IsAllRadioChecked(\'CHGDATE\');" />';
                                }
                                return data;
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "QuoteResponseDueDate",
                            "render": function (data, type, row, meta, value) {
                                if (type === 'display') {
                                    return '<input type="text" id="TxtNewResDueDate_' + row.QuoteNo + '" value="' + moment(data).format('DD-MM-YYYY') + '" disabled="disabled" />';
                                }
                                return data;
                            },
                            "autoWidth": true
                        },
                        ],
                        //dom: 'lBfrtip',
                        //buttons: [
                        //    'copy', 'excel', 'print'
                        //],
                        "ordering": false,
                        language: {
                            "lengthMenu": "Show <input class='form-control form-control-sm' id='lcDatatablesTbDuplicateWithExpiredReq' value='10' style='width:70px; display:unset; text-align:center;' type='number' min='1'/> entries &nbsp; &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.QuoteNo;
                        }
                    });

                    $("#TbDuplicateWithExpiredReq").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#TbDuplicateWithExpiredReq").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTableDuplicateWithExpiredReq.page.len(1).draw();
                        } else {
                            dataTableDuplicateWithExpiredReq.page.len(length).draw();
                        }
                    });

                    $("#TbDuplicateWithExpiredReq").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });

                } catch (e) {
                    alert("GenerateTbQuoteRefList() : " + e);
                }
            }
            else {
                alert('GenerateTbDuplicateWithExpiredReq : Browser Not Support')
            }
        }

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
                                currentPage = dataTableTbQuoteRefListInvalid.page.info().page;
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
                        //dom: 'lBfrtip',
                        //buttons: [
                        //    'copy', 'excel', 'print'
                        //],
                        "ordering": false,
                        language: {
                            "lengthMenu": "Show <input class='form-control form-control-sm' id='lcDatatablesListInvalid' value='10' style='width:70px; display:unset; text-align:center;' type='number' min='1'/> entries &nbsp; &nbsp;"
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

        function GenerateTbQuoteRefListSelected() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableTbQuoteRefListSelected = $("#TbQuoteRefListSelected").DataTable({
                        "bDestroy": true,
                        //"dom": '<"toolbar">frtip',
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function (data, type, row, meta) {
                            DatePitcker();
                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $(".paginate_button").click(function () {
                                currentPage = dataTableTbQuoteRefListSelected.page.info().page;
                            });

                            $('.ddlClassToolAmortize').on("change", function () {
                                var row = $(this).attr("attrrow");
                                var id = $(this).attr("id");
                                var selected = $(this).val();
                                var effectiveDate = dataTableTbQuoteRefListSelected.column(33).nodes().to$().find('#TxtEffDate_' + row).val();

                                if (selected == "ADD" && effectiveDate != "") {
                                    if ($("#tableAmortize_" + row).length > 0) {
                                        $("#tableAmortize_" + row).remove();
                                    }
                                    var tableAmor = '<table id="tableAmortize_' + row + '">' +
                                                        '<thead>' +
                                                            '<tr>' +
                                                                '<th>Select</th>' +
                                                                '<th>Tool Type</th>' +
                                                                '<th>Tool Amortize ID</th>' +
                                                                '<th>Tool Amortize Desc</th>' +
                                                                '<th>Amortize Cost</th>' +
                                                                '<th>Amortize Currency</th>' +
                                                                '<th>Exch Rate</th>' +
                                                                '<th>Period</th>' +
                                                                '<th>Period UOM</th>' +
                                                                '<th>Tot Amortize Qty</th>' +
                                                                '<th>Qty UOM</th>' +
                                                                '<th>Amortize Cost Vnd Curr</th>' +
                                                                '<th>Amortize Cost Pc Vnd Curr</th>' +
                                                                '<th>Effective From</th>' +
                                                                '<th>Due On</th>' +
                                                            '</tr>' +
                                                        '</thead>' +
                                                   '</table>';
                                    $("#divToolAmortize_" + row).html(tableAmor);

                                    var rowData = dataTableTbQuoteRefListSelected.row($(this).parents('tr')).data();
                                    var dtTable = $("#tableAmortize_" + row).DataTable({
                                        "bDestroy": true,
                                        "language": {
                                            "emptytable": "No data found"
                                        },
                                        "drawCallback": function () {
                                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                                            $(".paginate_button").click(function () {
                                                currentPage = dataTable.page.info().page;
                                            });
                                        },
                                        "deferRender": false,
                                        "filter": false,
                                        "paging": false,
                                        "columns": [
                                            {
                                                "data": "",
                                                "render": function (data, type, row, meta) {
                                                    var amortizetoolid = row.Amortize_Tool_ID;
                                                    if (type === 'display') {
                                                        return '<input type="radio" name="rb_' + rowData.QuoteNo + '" value="' + amortizetoolid + '">';
                                                    }
                                                },
                                                "autoWidth": true
                                            },
                                            { "data": "ToolTypeID" },
                                            { "data": "Amortize_Tool_ID" },
                                            { "data": "Amortize_Tool_Desc" },
                                            { "data": "AmortizeCost" },
                                            { "data": "AmortizeCurrency" },
                                            { "data": "ExchangeRate" },
                                            { "data": "AmortizePeriod" },
                                            { "data": "AmortizePeriodUOM" },
                                            { "data": "TotalAmortizeQty" },
                                            { "data": "QtyUOM" },
                                            { "data": "AmortizeCost_Vend_Curr" },
                                            { "data": "AmortizeCost_Pc_Vend_Curr" },
                                            {
                                                "data": "EffectiveFrom",
                                                "render": function (value) {
                                                    if (value === null) return "";
                                                    return moment(value).format('DD-MM-YYYY');
                                                },
                                                "autoWidth": true
                                            },
                                            {
                                                "data": "DueDate",
                                                "render": function (value) {
                                                    if (value === null) return "";
                                                    return moment(value).format('DD-MM-YYYY');
                                                },
                                                "autoWidth": true
                                            },
                                        ],
                                        "ordering": false,
                                        language: {
                                            "lengthMenu": "Show <input class='form-control form-control-sm' id='lcDatatables' value='10' style='width:70px; display:unset; text-align:center;' type='number' min='1'/> entries &nbsp; &nbsp;"
                                        },
                                        "scrollX": true,
                                    });

                                    var obj = { QuoteNo: rowData.QuoteNo, table: dtTable };
                                    test11.push(obj);


                                    //test11[row] = $("#tableAmortize_" + row).DataTable({
                                    //    "bDestroy": true,
                                    //    "language": {
                                    //        "emptytable": "No data found"
                                    //    },
                                    //    "drawCallback": function () {
                                    //        $('div.dataTables_filter input').addClass('form-control form-control-sm');
                                    //        $(".paginate_button").click(function () {
                                    //            currentPage = dataTable.page.info().page;
                                    //        });
                                    //    },
                                    //    "deferRender": false,
                                    //    "columns": [
                                    //        { "data": "ToolTypeID" },
                                    //        { "data": "Amortize_Tool_ID" },
                                    //        { "data": "Amortize_Tool_Desc" },
                                    //        { "data": "AmortizeCost" },
                                    //        { "data": "AmortizeCurrency" },
                                    //        { "data": "ExchangeRate" },
                                    //        { "data": "AmortizePeriod" },
                                    //        { "data": "AmortizePeriodUOM" },
                                    //        { "data": "TotalAmortizeQty" },
                                    //        { "data": "QtyUOM" },
                                    //        { "data": "AmortizeCost_Vend_Curr" },
                                    //        { "data": "AmortizeCost_Pc_Vend_Curr" },
                                    //        {
                                    //            "data": "EffectiveFrom",
                                    //            "render": function (value) {
                                    //                if (value === null) return "";
                                    //                return moment(value).format('DD-MM-YYYY');
                                    //            },
                                    //            "autoWidth": true
                                    //        },
                                    //        {
                                    //            "data": "DueDate",
                                    //            "render": function (value) {
                                    //                if (value === null) return "";
                                    //                return moment(value).format('DD-MM-YYYY');
                                    //            },
                                    //            "autoWidth": true
                                    //        },
                                    //    ],
                                    //    "ordering": false,
                                    //    language: {
                                    //        "lengthMenu": "Show <input class='form-control form-control-sm' id='lcDatatables' value='10' style='width:70px; display:unset; text-align:center;' type='number' min='1'/> entries &nbsp; &nbsp;"
                                    //    },
                                    //    "scrollX": true,
                                    //});

                                    var VendorCode = "";
                                    var processgrp = "";
                                    var EffectiveDate = "";
                                    var Material = "";
                                    var dataTableToolAmor = $("#tableAmortize_" + row).DataTable();
                                    GetVendorToolAmor(rowData.Vendor, rowData.ProcessGroup, effectiveDate, rowData.Material, rowData.QuoteNo);
                                    dataTableTbQuoteRefListSelected.columns.adjust().draw();
                                }
                                else if (selected == "REMOVE") {
                                    var rowData = dataTableTbQuoteRefListSelected.row($(this).parents('tr')).data();
                                    GetVendorToolAmorOld(rowData.QuoteNo);
                                }else if (selected == null || selected == "0" || selected == "NO CHANGE") {
                                    $("#tableAmortize_" + row).DataTable().destroy();
                                    dataTableTbQuoteRefListSelected.columns.adjust().draw();
                                    $("#divToolAmortize_" + row).html("No Data");
                                }
                            });
                        },
                        "deferRender": false,
                        "columns": [
                            {
                                "data": null,
                                render: function (data, type, row, meta) {
                                    return meta.row + meta.settings._iDisplayStart + 1;
                                }
                            },
                            {
                                "data": "QuoteNo",
                                "render": function (data, type, row, meta) {
                                    if (type === 'display') {
                                        var url = "QQPReview.aspx?Number=" + data;
                                        return '<a id="LbQSel_' + row.QuoteNo + '" onclick="openInNewTab2(\'' + url + '\')">' + data + '</a>';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            { "data": "Vendor", "autoWidth": true },
                            { "data": "VendorName", "autoWidth": true },
                            { "data": "Material", "autoWidth": true },
                            { "data": "MaterialDesc", "autoWidth": true },
                            { "data": "ProcessGroup", "autoWidth": true },
                            { "data": "PrcGrpDesc", "autoWidth": true },
                            { "data": "MaterialType", "autoWidth": true },
                            { "data": "MaterialClass", "autoWidth": true },
                            { "data": "UOM", "autoWidth": true },
                            { "data": "PlantStatus", "autoWidth": true },
                            { "data": "SAPProcType", "autoWidth": true },
                            { "data": "SAPSpProcType", "autoWidth": true },
                            { "data": "Product", "autoWidth": true },
                            { "data": "PIRType", "autoWidth": true },
                            { "data": "PIRJobType", "autoWidth": true },
                            { "data": "NetUnit", "autoWidth": true },
                            {
                                "data": "IsAllcostAllow",
                                "render": function (data, type, row, meta) {
                                    if (type === 'display') {
                                        var disabledCbb = "";
                                        if (row.Layout == "LAYOUT7") {
                                            disabledCbb = "disabled";
                                        }
                                        if (data == true) {
                                            return '<input type="checkbox" class="chkAllRefRwSelected_" id="chkAllRefRwSelected_' + row.QuoteNo + '" ' + disabledCbb + ' checked="checked" onclick="chkAllRefRwSelected_Click(\'' + row.QuoteNo + '\')" />';
                                        }
                                        else {
                                            return '<input type="checkbox" class="chkAllRefRwSelected_" id="chkAllRefRwSelected_' + row.QuoteNo + '" ' + disabledCbb + ' onclick="chkAllRefRwSelected_Click(\'' + row.QuoteNo + '\')" />';
                                        }
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": "IsMatcostAllow",
                                "render": function (data, type, row, meta) {
                                    var MatcostVal = row.MatcostVal;
                                    if (data == true) {
                                        return ' <input type="checkbox" class="chkMatRefSelected_" id="chkMatRefSelected_' + row.QuoteNo + '" checked="checked"  onclick="chkDetSelected_Click(\'' + row.QuoteNo + '\');" /><label for="chkMatRefSelected_' + row.QuoteNo + '" id="LbchkMatRefSelected_' + row.QuoteNo + '"> ' + "&nbsp;" + MatcostVal + '  </label> '
                                    }
                                    else {
                                        return ' <input type="checkbox" class="chkMatRefSelected_" id="chkMatRefSelected_' + row.QuoteNo + '"  onclick="chkDetSelected_Click(\'' + row.QuoteNo + '\');" /><label for="chkMatRefSelected_' + row.QuoteNo + '" id="LbchkMatRefSelected_' + row.QuoteNo + '"> ' + "&nbsp;" + MatcostVal + '  </label> '
                                    }
                                },
                                "autoWidth": true
                            },
                            {
                                "data": "IsProccostAllow",
                                "render": function (data, type, row, meta) {
                                    var ProccostVal = row.ProccostVal;
                                    var disabledCbb = "";
                                    if (row.Layout == "LAYOUT7") {
                                        disabledCbb = "disabled";
                                    }

                                    if (data == true) {
                                        return ' <input type="checkbox" class="chkProcRefSelected_" id="chkProcRefSelected_' + row.QuoteNo + '" checked="checked" ' + disabledCbb + ' onclick="chkDetSelected_Click(\'' + row.QuoteNo + '\'); "/><label for="chkProcRefSelected_' + row.QuoteNo + '" id="LbchkProcRefSelected_' + row.QuoteNo + '"> ' + "&nbsp;" + ProccostVal + '  </label> '
                                    }
                                    else {
                                        return ' <input type="checkbox" class="chkProcRefSelected_" id="chkProcRefSelected_' + row.QuoteNo + '" ' + disabledCbb + ' onclick="chkDetSelected_Click(\'' + row.QuoteNo + '\'); "/><label for="chkProcRefSelected_' + row.QuoteNo + '" id="LbchkProcRefSelected_' + row.QuoteNo + '"> ' + "&nbsp;" + ProccostVal + '  </label> '
                                    }

                                },
                                "autoWidth": true
                            },
                            {
                                "data": "IsSubMatcostAllow",
                                "render": function (data, type, row, meta) {
                                    var SubMatcostVal = row.SubMatcostVal;
                                    var disabledCbb = "";
                                    if (row.Layout == "LAYOUT7") {
                                        disabledCbb = "disabled";
                                    }
                                    if (data == true) {
                                        return ' <input type="checkbox" class="chkSubMatRefSelected_" id="chkSubMatRefSelected_' + row.QuoteNo + '" checked="checked" ' + disabledCbb + ' onclick="chkDetSelected_Click(\'' + row.QuoteNo + '\');" /><label for="chkSubMatRefSelected_' + row.QuoteNo + '" id="LbchkSubMatRefSelected_' + row.QuoteNo + '"> ' + "&nbsp;" + SubMatcostVal + '  </label> '
                                    }
                                    else {
                                        return ' <input type="checkbox" class="chkSubMatRefSelected_" id="chkSubMatRefSelected_' + row.QuoteNo + '" ' + disabledCbb + ' onclick="chkDetSelected_Click(\'' + row.QuoteNo + '\');" /><label for="chkSubMatRefSelected_' + row.QuoteNo + '" id="LbchkSubMatRefSelected_' + row.QuoteNo + '"> ' + "&nbsp;" + SubMatcostVal + '  </label> '
                                    }

                                },
                                "autoWidth": true
                            },
                            {
                                "data": "IsOthcostAllow",
                                "render": function (data, type, row, meta) {
                                    var OthcostVal = row.OthcostVal;
                                    if (data == true) {
                                        return ' <input type="checkbox" class="chkOthRefSelected_" id="chkOthRefSelected_' + row.QuoteNo + '" checked="checked" onclick="chkDetSelected_Click(\'' + row.QuoteNo + '\'); "  /><label for="chkOthRef_' + row.QuoteNo + '" id="LbchkOthRefSelected_' + row.QuoteNo + '"> ' + "&nbsp;" + OthcostVal + '  </label> '
                                    }
                                    else {
                                        return ' <input type="checkbox" class="chkOthRefSelected_" id="chkOthRefSelected_' + row.QuoteNo + '" onclick="chkDetSelected_Click(\'' + row.QuoteNo + '\'); "  /><label for="chkOthRefSelected_' + row.QuoteNo + '" id="LbchkOthRefSelected_' + row.QuoteNo + '"> ' + "&nbsp;" + OthcostVal + '  </label> '
                                    }

                                },
                                "autoWidth": true
                            },
                            {
                                "data": "IsUseToolAmor",
                                "render": function (data, type, row, meta) {
                                    var ToolAmorExist = row.ToolAmorExist, ToolAmorExpired = row.ToolAmorExpired;
                                    var IsSubMatcostAllow = row.IsSubMatcostAllow, IsProccostAllow = row.IsProccostAllow;

                                    var Disabled = "disabled", DisabledNoChange = "disabled", DisabledADD = "disabled", DisabledREMOVE = "disabled";
                                    var selectedOptNoChange = "", selectedOptAdd = "", selectedOptRemove = "";

                                    if (IsSubMatcostAllow == true) {
                                        Disabled = "";
                                    }

                                    if (ToolAmorExist == false && ToolAmorExpired == false) {
                                        DisabledNoChange = "";
                                        DisabledADD = "";
                                        DisabledREMOVE = "disabled";
                                    }
                                    else if (ToolAmorExist == true && ToolAmorExpired == true) {
                                        DisabledNoChange = "disabled";
                                        DisabledADD = "";
                                        DisabledREMOVE = "";
                                    }
                                    else {
                                        DisabledNoChange = "";
                                        DisabledADD = "";
                                        DisabledREMOVE = "";
                                    }

                                    var newdata = data.split("|");

                                    if (newdata[0] == "NO CHANGE") {
                                        selectedOptNoChange = "selected";
                                    }
                                    else if (newdata[0] == "ADD") {
                                        selectedOptAdd = "selected";
                                    }
                                    else if (newdata[0] == "REMOVE") {
                                        selectedOptRemove = "selected";

                                        //var rowData = dataTableTbQuoteRefListSelected.row($(this).parents('tr')).data();
                                        //GetVendorToolAmorOld(row.QuoteNo);
                                    }

                                    var Option = ' <option value="NO CHANGE" ' + selectedOptNoChange + ' ' + DisabledNoChange + '>NO CHANGE</option> <option value="ADD" ' + selectedOptAdd + ' ' + DisabledADD + '>ADD</option><option value="REMOVE" ' + selectedOptRemove + ' ' + DisabledREMOVE + '>REMOVE</option>';
                                    //if (ToolAmorExpired == true) {
                                    //    if (data == null || data == "0") {
                                    //        Option = '<option value="0" selected disabled="disabled"> </option>' + Option;
                                    //    }
                                    //    else {
                                    //        Option = '<option value="0" disabled="disabled"> </option>' + Option;
                                    //    }
                                    //}
                                    return ' <select id="DdlToolAmorRefSelected_' + row.QuoteNo + '" ' + Disabled + ' class="ddlClassToolAmortize" attrrow="' + row.QuoteNo + '" defaultValue="' + newdata[1] + '"> ' + Option + ' </select>';
                                },
                                "autoWidth": true
                            },
                            {
                                "data": "IsUseMachineAmor",
                                "render": function (data, type, row, meta) {
                                    var MacAmorExist = row.MacAmorExist, MacAmorExpired = row.MacAmorExpired;
                                    var IsSubMatcostAllow = row.IsSubMatcostAllow, IsProccostAllow = row.IsProccostAllow;

                                    var Disabled = "disabled", DisabledNoChange = "disabled", DisabledADD = "disabled", DisabledREMOVE = "disabled";
                                    var selectedOptNoChange = "", selectedOptAdd = "", selectedOptRemove = "";

                                    if (IsProccostAllow == true && IsSubMatcostAllow == true) {
                                        Disabled = "";
                                    }

                                    if (MacAmorExist == false && MacAmorExpired == false) {
                                        DisabledNoChange = "";
                                        DisabledADD = "";
                                        DisabledREMOVE = "disabled";
                                    }
                                    else if (MacAmorExist == true && MacAmorExpired == true) {
                                        DisabledNoChange = "disabled";
                                        DisabledADD = "";
                                        DisabledREMOVE = "";
                                    }
                                    else {
                                        DisabledNoChange = "";
                                        DisabledADD = "";
                                        DisabledREMOVE = "";
                                    }

                                    var newdata = data.split("|");
                                    if (newdata[0] == "NO CHANGE") {
                                        selectedOptNoChange = "selected";
                                    }
                                    else if (newdata[0] == "ADD") {
                                        selectedOptAdd = "selected";
                                    }
                                    else if (newdata[0] == "REMOVE") {
                                        selectedOptRemove = "selected";
                                    }

                                    //var Option = ' <option value="NO CHANGE" ' + selectedOptNoChange + ' ' + DisabledNoChange + '>NO CHANGE</option> <option value="ADD" ' + selectedOptAdd + ' ' + DisabledADD + '>ADD</option><option value="REMOVE" ' + selectedOptRemove + ' ' + DisabledREMOVE + '>REMOVE</option>';
                                    var Option = ' <option value="NO CHANGE" ' + selectedOptNoChange + ' ' + DisabledNoChange + '>NO CHANGE</option> <option value="REMOVE" ' + selectedOptRemove + ' ' + DisabledREMOVE + '>REMOVE</option>';
                                    //if (MacAmorExpired == true) {
                                    //    if (data == null || data == "0") {
                                    //        Option = '<option value="0" selected disabled="disabled"> </option>' + Option;
                                    //    }
                                    //    else {
                                    //        Option = '<option value="0" disabled="disabled"> </option>' + Option;
                                    //    }
                                    //}
                                    return ' <select class="DdlMachineAmortizeRef_" id="DdlMachineAmortizeRef_' + row.QuoteNo + '" ' + Disabled + ' defaultValue="' + newdata[1] + '"> ' + Option + ' </select>'
                                },
                                "autoWidth": true
                            },
                            { "data": "ToolAmorExist", "autoWidth": true },
                            { "data": "ToolAmorExpired", "autoWidth": true },
                            { "data": "MacAmorExist", "autoWidth": true },
                            { "data": "MacAmorExpired", "autoWidth": true },
                            {
                                "data": null,
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        var DisplayCtrl = '';
                                        DisplayCtrl = '<select class="DdlReqPurposeSelected_" id="DdlReqPurposeSelected_' + row.QuoteNo + '" onchange="DdlReasonchange(\'DdlReqPurposeSelected_' + row.QuoteNo + '\', \'Txtreason_' + row.QuoteNo + '\', \'LblengtVCID_' + row.QuoteNo + '\')">'
                                        if (DtReqPurpose.length > 0) {
                                            DisplayCtrl += '<option value="0">-- Select --</option>';
                                            for (var i = 0; i < DtReqPurpose.length; i++) {
                                                DisplayCtrl += '<option value="' + DtReqPurpose[i].Value + '">' + DtReqPurpose[i].Text + '</option>';
                                            }
                                            DisplayCtrl += '<option value="Others">Others</option>';
                                        }
                                        else {
                                            DisplayCtrl += '<option value="0">Req Purpose Not Maintain</option>';
                                        }
                                        DisplayCtrl += '</select>';
                                        DisplayCtrl += ' <textarea class="Txtreason_"  id="Txtreason_' + row.QuoteNo + '" rows="2" cols="10" onkeyup="RemarkLght(\'Txtreason_' + row.QuoteNo + '\', \'LblengtVCID_' + row.QuoteNo + '\')" style="display:none"> </textarea>';
                                        DisplayCtrl += '<label class="LblengtVCID_" id="LblengtVCID_' + row.QuoteNo + '" style="display:none"></label>';
                                        return DisplayCtrl;
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": "MQty",
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        if (data == null) {
                                            return '<input type="text" class="TxtMQtySelected_" id="TxtMQtySelected_' + row.QuoteNo + '" value="" oninput="validateNumber(\'TxtMQtySelected_' + row.QuoteNo + '\')" onkeyup="SetBordrColor(\'TxtMQtySelected_' + row.QuoteNo + '\')" />';
                                        }
                                        else {
                                            return '<input type="text" class="TxtMQtySelected_" id="TxtMQtySelected_' + row.QuoteNo + '" value="' + data + '" oninput="validateNumber(\'TxtMQtySelected_' + row.QuoteNo + '\')" onkeyup="SetBordrColor(\'TxtMQtySelected_' + row.QuoteNo + '\')" />';
                                        }
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": "BaseUOM",
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        return '<input type="text" class="TxtUomSelected_" id="TxtUomSelected_' + row.QuoteNo + '" value="' + data + '" onkeyup="SetBordrColor(\'TxtUomSelected_' + row.QuoteNo + '\')" />';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": null,
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        return '<input type="text"  id="TxtResDueDate_' + row.QuoteNo + '" value="" onkeydown="javascript:preventInput(event);" class="form_datetime TxtResDueDate_" onkeyup="SetBordrColor(\'TxtResDueDate_' + row.QuoteNo + '\')" />';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": null,
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        return '<input type="text" id="TxtEffDate_' + row.QuoteNo + '" value="" onkeydown="javascript:preventInput(event);" attrrow="' + row.QuoteNo + '" class="form_datetime effectiveDate" onkeyup="SetBordrColor(\'TxtEffDate_' + row.QuoteNo + '\')" />';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": null,
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        var d = new Date();
                                        var mydate = new Date(DateDuenextRev);
                                        var dd = ((mydate.getDate() < 10 ? '0' : '')) + (mydate.getDate());
                                        var MM = ((mydate.getMonth() + 1) < 10 ? '0' : '') + (mydate.getMonth() + 1);
                                        var yyyy = mydate.getFullYear()
                                        var newDate = dd + '-' + MM + '-' + yyyy;
                                        if (d < mydate) {
                                            return '<input type="text" id="TxtDueDateNextRev_' + row.QuoteNo + '"  value="' + newDate + '" onkeydown="javascript:preventInput(event);" disabled="disabled" class="form_datetime TxtDueDateNextRev_" onkeyup="SetBordrColor(\'TxtDueDateNextRev_' + row.QuoteNo + '\')" />';
                                        }
                                        else {
                                            return '<input type="text" id="TxtDueDateNextRev_' + row.QuoteNo + '" value="' + newDate + '" onkeydown="javascript:preventInput(event);" class="form_datetime TxtDueDateNextRev_" onkeyup="SetBordrColor(\'TxtDueDateNextRev_' + row.QuoteNo + '\')" />';
                                        }
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": null,
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        var DisplayCtrl = '';
                                        var layout = row.Layout;
                                        var disabled = "";
                                        if (layout != "LAYOUT1") {
                                            disabled = "disabled";
                                        }
                                        DisplayCtrl = '<select class="DdlRecycleRatioSelected_" id="DdlRecycleRatioSelected_' + row.QuoteNo + '" ' + disabled + '>'
                                        if (DtRecycleRatio.length > 0) {
                                            DisplayCtrl += '<option value="">-- Select --</option>';
                                            for (var i = 0; i < DtRecycleRatio.length; i++) {
                                                DisplayCtrl += '<option value="' + DtRecycleRatio[i].Value + '">' + DtRecycleRatio[i].Text + '</option>';
                                            }
                                        }
                                        else {
                                            DisplayCtrl += '<option value="">Recycle Ratio Not Maintain</option>';
                                        }
                                        DisplayCtrl += '</select>';
                                        return DisplayCtrl;
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": "QuoteNo",
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        return '<input type="file" class="FlAtc" id="FlAtc' + data + '" onchange="validateFileUpload(\'FlAtc' + data + '\')" />';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": null,
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        return '<div id="divToolAmortize_' + row.QuoteNo + '">No Data</div>';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                            {
                                "data": null,
                                "render": function (data, type, row, meta, value) {
                                    if (type === 'display') {
                                        return '<a id="BtnDel' + row.QuoteNo + '" class="btn btn-danger btn-sm btndelete" onclick="DeleteSelectedQuote()"> Delete </a>';
                                    }
                                    return data;
                                },
                                "autoWidth": true
                            },
                        ],
                        "ordering": false,
                        language: {
                            "lengthMenu": "Show <input class='form-control form-control-sm' id='lcDatatablesListSelected' value='10' style='width:70px; display:unset; text-align:center;' type='number' min='1'/> entries &nbsp; &nbsp; "
                        },
                        dom: 'lf<"toolbar">rtip',
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.QuoteNo;
                        }
                    });

                    $("#lcDatatablesListSelected").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesListSelected").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTableTbQuoteRefListSelected.page.len(1).draw();
                        } else {
                            dataTableTbQuoteRefListSelected.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesListSelected").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });

                    dataTableTbQuoteRefListSelected.column(8).visible(false);
                    dataTableTbQuoteRefListSelected.column(9).visible(false);
                    dataTableTbQuoteRefListSelected.column(10).visible(false);
                    dataTableTbQuoteRefListSelected.column(11).visible(false);
                    dataTableTbQuoteRefListSelected.column(12).visible(false);
                    dataTableTbQuoteRefListSelected.column(13).visible(false);
                    dataTableTbQuoteRefListSelected.column(14).visible(false);
                    dataTableTbQuoteRefListSelected.column(15).visible(false);
                    dataTableTbQuoteRefListSelected.column(16).visible(false);
                    dataTableTbQuoteRefListSelected.column(17).visible(false);

                    dataTableTbQuoteRefListSelected.column(25).visible(false);
                    dataTableTbQuoteRefListSelected.column(26).visible(false);
                    dataTableTbQuoteRefListSelected.column(27).visible(false);
                    dataTableTbQuoteRefListSelected.column(28).visible(false);
                } catch (e) {
                    alert("GenerateTbQuoteRefListSelected() : " + e);
                }
            }
            else {
                alert('GenerateTbQuoteRefListSelected : Browser Not Support')
            }
        }

        function GenerateTbCreateReqTemp() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    dataTableTbCreateReqTemp = $("#TbCreateReqTemp").DataTable({
                        "bDestroy": true,
                        "language": {
                            "emptytable": "No data found"
                        },
                        "drawCallback": function () {
                            $('div.dataTables_filter input').addClass('form-control form-control-sm');
                            $(".paginate_button").click(function () {
                                currentPage = dataTableTbCreateReqTemp.page.info().page;
                            });
                        },
                        "deferRender": false,
                        "columns": [
                        { "data": "QuoteNoRef", "autoWidth": true },
                        { "data": "ReqNo", "autoWidth": true },
                        { "data": "Plant", "autoWidth": true },
                        { "data": "CompMaterial", "autoWidth": true },
                        { "data": "CompMaterialDesc", "autoWidth": true },
                        { "data": "VendorName", "autoWidth": true },
                        { "data": "VendorCode1", "autoWidth": true },
                        { "data": "SearchTerm", "autoWidth": true },
                        { "data": "QuoteNo", "autoWidth": true },
                        { "data": "VenPIC", "autoWidth": true },
                        { "data": "PICEmail", "autoWidth": true },
                        { "data": "SellCurrency", "autoWidth": true },
                        { "data": "AmtScur", "autoWidth": true },
                        { "data": "ExchangeRate", "autoWidth": true },
                        { "data": "VndCurrency", "autoWidth": true },
                        { "data": "AmtVcur", "autoWidth": true },
                        { "data": "Unit", "autoWidth": true },
                        { "data": "UOM", "autoWidth": true },
                        {
                            "data": "ValidFrom",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "CusMatValFrom",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        {
                            "data": "CusMatValTo",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).format('DD-MM-YYYY');
                            },
                            "autoWidth": true
                        },
                        ],
                        "ordering": false,
                        language: {
                            "lengthMenu": "Show <input class='form-control form-control-sm' id='lcDatatablesTbCreateReqTemp' value='10' style='width:70px; display:unset; text-align:center;' type='number' min='1'/> entries &nbsp; &nbsp;"
                        },
                        "scrollX": true,
                        rowId: function (a) {
                            return "id_" + a.QuoteNo;
                        }
                    });

                    $("#lcDatatablesTbCreateReqTemp").keydown(function (e) {
                        if (e.which == 69 || e.which == 189 || e.which == 187 || e.which == 190 || e.which == 107 && (e.which == 86 || e.which == 67)) {
                            e.preventDefault();
                        }
                    });

                    $("#lcDatatablesTbCreateReqTemp").on("input", function (e) {
                        var length = $(this).val();
                        var res = length.charAt(0);

                        if (length.length > 1) {
                            if (res == "0") {
                                length = length.substring(1);
                                $(this).val(length)
                            }
                        }

                        if ($(this).val() == "" || $(this).val() == "0") {
                            dataTableTbCreateReqTemp.page.len(1).draw();
                        } else {
                            dataTableTbCreateReqTemp.page.len(length).draw();
                        }
                    });

                    $("#lcDatatablesTbCreateReqTemp").change(function (e) {
                        if ($(this).val() == "" || $(this).val() == "0") {
                            $(this).val("1");
                        }
                    });

                } catch (e) {
                    alert("GenerateTbCreateReqTemp() : " + e);
                }
            }
            else {
                alert('GenerateTbCreateReqTemp : Browser Not Support')
            }
        }

        function DeleteSelectedQuote() {
            if (typeof (Worker) !== "undefined") {
                try {
                    jQuery.noConflict();
                    var myTable = $('#TbQuoteRefListSelected').DataTable();
                    $('#TbQuoteRefListSelected tbody').on('click', '.btndelete', function () {
                        myTable.row($(this).parents('tr')).remove().draw();
                    });
                    dataTableTbCreateReqTemp.clear().draw();
                } catch (e) {
                    alert("DeleteSelectedQuote : " + e);
                }
            }
            else {
                alert('DeleteSelectedQuote : Browser Not Support')
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
                    dataType: 'json',
                    async: false,
                    data:{ReasonType:"Revision Of eMET"},
                    beforeSend: function () {
                        ShowLoading();
                    },
                    complete: function () {
                        CloseLoading();
                    },
                    success: function (data) {
                        if (data.success == true) {
                            DtReqPurpose = data.MyData;
                        }
                    },
                    error: function (reponse) {
                    }
                });
            }
            else {
                alert('GetDdlReason : Browser Not Support')
            }
        }

        function GetImRecycleRatio() {
            if (typeof (Worker) !== "undefined") {
                var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/GetImRecycleRatio";
                $.ajax({
                    url: url,
                    cache: false,
                    type: "POST",
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
                            DtRecycleRatio = data.MyData;
                        }
                    },
                    error: function (reponse) {
                    }
                });
            }
            else {
                alert('GetImRecycleRatio : Browser Not Support')
            }
        }

        function GetDateDuenextRev() {
            if (typeof (Worker) !== "undefined") {
                var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/GetDateDuenextRev";
                $.ajax({
                    url: url,
                    cache: false,
                    type: "POST",
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
                            DateDuenextRev = data.message;
                        }
                    },
                    error: function (reponse) {
                    }
                });
            }
            else {
                alert('GetDateDuenextRev : Browser Not Support')
            }
        }

        function GetDdlProduct() {
            if (typeof (Worker) !== "undefined") {
                var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/GetDdlProduct";
                var procemessage = "<option value='0' class='optionPlaceHolder'> Please wait...</option>";
                $("#DdlProduct").html(procemessage).show();
                var markup = "";
                $.ajax({
                    url: url,
                    cache: false,
                    type: "POST",
                    dataType: 'json',
                    async: false,
                    beforeSend: function () {
                        ShowLoading();
                    },
                    complete: function () {
                        GetDdlMatClassDesc();
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
                        $("#DdlProduct").html(markup).show();
                    },
                    error: function (xhr, status, error) {
                        //alert(error);
                    }
                },
                $('#DdlProduct :nth-child(0)').prop('selected', true)
                );
            }
            else {
                alert('GetDdlProduct : Browser Not Support')
            }
        }

        function GetDdlMatClassDesc() {
            if (typeof (Worker) !== "undefined") {
                try {
                    var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/GetDdlMatClassDesc";
                    var procemessage = "<option value='0' class='optionPlaceHolder'> Please wait...</option>";
                    $("#DdlMatClassDesc").html(procemessage).show();
                    var _Product = document.getElementById("DdlProduct").value;
                    var markup = "";
                    $.ajax({
                        url: url,
                        cache: false,
                        type: "POST",
                        dataType: 'json',
                        data: { Product: _Product },
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
                            $("#DdlMatClassDesc").html(markup).show();
                        },
                        error: function (xhr, status, error) {
                            alert(error);
                        }
                    },
                    $('#DdlMatClassDesc :nth-child(0)').prop('selected', true)
                    );
                } catch (e) {
                    alert(e);
                }
            }
            else {
                alert('GetDdlMatClassDesc : Browser Not Support')
            }
        }

        function GetDdlProcGroup() {
            if (typeof (Worker) !== "undefined") {
                try {
                    var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/GetDdlProcGroup";
                    var procemessage = "<option value='0' class='optionPlaceHolder'> Please wait...</option>";
                    $("#DdlProcGroup").html(procemessage).show();
                    var markup = "";
                    $.ajax({
                        url: url,
                        cache: false,
                        type: "POST",
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
                            $("#DdlProcGroup").html(markup).show();
                        },
                        error: function (xhr, status, error) {
                            alert(error);
                        }
                    },
                    $('#DdlProcGroup :nth-child(0)').prop('selected', true)
                    );
                } catch (e) {
                    alert(e);
                }
            }
            else {
                alert('GetDdlProcGroup : Browser Not Support')
            }
        }
    </script>
    
    <%--Process Logic--%>
    <script type="text/javascript">
        function test() {
            var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/HelloWorld";
            $.ajax({
                url: url,
                cache: false,
                type: "POST",
                dataType: 'json',
                async: false,
                complete: function () {

                },
                success: function (data) {
                    console.log(JSON.stringify(data));
                },
                error: function (reponse) {
                }
            });
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
                    if (data.success == true){
                        window.location = mainUrl + "/Login.aspx";
                    }
                },
                error: function (reponse) {
                }
            });
        }

        function GetQuoteList() {
            jQuery.noConflict();
            ShowLoading();
            if (typeof (Worker) !== "undefined") {
                setTimeout(function () {
                    var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/GetQuoteList";
                    var ActiveMaterial = document.getElementById('chkActiveMaterial').checked;
                    var ReqType = document.getElementById('DdlReqType').value;
                    var Product = document.getElementById('DdlProduct').value;
                    var MatClassDesc = document.getElementById('DdlMatClassDesc').value;
                    var ProcGroup = document.getElementById('DdlProcGroup').value;
                    var SubProc = document.getElementById('TxtSubProc').value;
                    var Filter = document.getElementById('DdlFilter').value;
                    var FilterValue = document.getElementById('TxtFilter').value;
                    var IsExternal = document.getElementById('RbExternal').checked;

                    var TbSelected = $('#TbQuoteRefListSelected').DataTable();
                    var TbSelectedData = TbSelected.rows().data();
                    var VndVsMaterialSelectedData = "";
                    if (TbSelectedData.length > 0) {
                        for (var i = 0; i < TbSelectedData.length; i++) {
                            var Vnd = TbSelectedData[i].Vendor;
                            var Mat = TbSelectedData[i].Material;

                            VndVsMaterialSelectedData += "\'" + Vnd.concat('-', Mat) + "\',";
                        }
                        VndVsMaterialSelectedData = VndVsMaterialSelectedData.substring(0, VndVsMaterialSelectedData.length - 1);
                    }

                    var TbInvalid = $('#TbQuoteRefListInvalid').DataTable();
                    var TbInvalidData = TbInvalid.rows().data();
                    var VndVsMaterialInvalidData = "";
                    if (TbInvalidData.length > 0) {
                        for (var i = 0; i < TbInvalidData.length; i++) {
                            var Vnd = TbInvalidData[i].VendorCode1;
                            var Mat = TbInvalidData[i].Material;

                            VndVsMaterialInvalidData += "\'" + Vnd.concat('-', Mat) + "\',";
                        }
                        VndVsMaterialInvalidData = VndVsMaterialInvalidData.substring(0, VndVsMaterialInvalidData.length - 1);
                    }

                    $.ajax({
                        url: url,
                        cache: false,
                        type: "POST",
                        dataType: 'json',
                        //contentType: "application/json; charset=utf-8",
                        data: {
                            ActiveMaterial: ActiveMaterial, ReqType: ReqType, Product: Product, MatClassDesc: MatClassDesc,
                            ProcGroup: ProcGroup, SubProc: SubProc, Filter: Filter, FilterValue: FilterValue, IsExternal: IsExternal,
                            VndVsMaterialInvalidData: VndVsMaterialInvalidData, VndVsMaterialSelectedData: VndVsMaterialSelectedData
                        },
                        async: false,
                        beforeSend: function () {
                            ShowLoading();
                        },
                        complete: function () {
                            checkPlantStatusAndLayout();
                            CloseLoading();
                        },
                        success: function (data) {
                            if (data.success == true) {
                                var length = $("#lcDatatables").val();
                                if (length == "" || length == "0") {
                                    length = "1";
                                    $("#lcDatatables").val("1");
                                }
                                dataTableTbQuoteRefList.clear().draw();
                                dataTableTbQuoteRefList.rows.add(data.QueteRef).draw();
                                //length change input textbox
                                dataTableTbQuoteRefList.page.len(length).draw();
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
                alert('GetQuoteList : Browser Not Support')
            }
            
        }

        function CekDuplicateWithExpiredReq() {
            ShowLoading();
            setTimeout(function () {
                try {
                    if (typeof (Worker) !== "undefined") {
                        jQuery.noConflict();
                        var url = mainUrl + "/EmetServices/RevisionEMET/MyXml.asmx/CekVendorVsMaterialExpiredReq";
                        var Mytable = $('#TbQuoteRefList').DataTable();
                        var Mydata = Mytable.rows().data();
                        var DtExpiredInvalid = [];
                        var _VendVsMat = [];
                        var _VendVsMatOriginal = [];
                        var _TotalSelectedByIndex = [];

                        //checkboxes should have a general class to traverse
                        var rowcollection = dataTableTbQuoteRefList.$(".checkAllRefRw:checked, .checkAllMaterialCost:checked, .checkAllProcessCost:checked, .checkAllSubMaterialCost:checked, .checkAllOtheritemsCost:checked", { "page": "all" });

                        //Now loop through all the selected checkboxes to perform desired actions
                        rowcollection.each(function (index, elem) {
                            //You have access to the current iterating row
                            var checkbox_value = $(elem).val();
                            //Do something with 'checkbox_value'
                            var idxChkAllRef = $(elem).attr("attrrow");
                            _TotalSelectedByIndex.push($(elem).attr("attrrow"));


                            //console.log(dataTableTbQuoteRefList.data()[idxChkAllRef]);
                        });

                        Array.prototype.contains = function (v) {
                            for (var i = 0; i < this.length; i++) {
                                if (this[i] === v) return true;
                            }
                            return false;
                        };

                        Array.prototype.unique = function () {
                            var arr = [];
                            for (var i = 0; i < this.length; i++) {
                                if (!arr.contains(this[i])) {
                                    arr.push(this[i]);
                                }
                            }
                            return arr;
                        }
                        var uniquesIndex = _TotalSelectedByIndex.unique();
                        for (var i = 0; i < uniquesIndex.length; i++) {
                            var QutNo = dataTableTbQuoteRefList.data()[uniquesIndex[i]].QuoteNo;
                            var IsAllcostAllow = dataTableTbQuoteRefList.row(uniquesIndex[i]).column(8).nodes().to$().find('#chkAllRefRw_' + QutNo).prop('checked');
                            var IsMatcostAllow = dataTableTbQuoteRefList.row(uniquesIndex[i]).column(9).nodes().to$().find('#chkMatRef_' + QutNo).prop('checked');
                            var IsProccostAllow = dataTableTbQuoteRefList.row(uniquesIndex[i]).column(10).nodes().to$().find('#chkProcRef_' + QutNo).prop('checked');
                            var IsSubMatcostAllow = dataTableTbQuoteRefList.row(uniquesIndex[i]).column(11).nodes().to$().find('#chkSubMatRef_' + QutNo).prop('checked');
                            var IsOthcostAllow = dataTableTbQuoteRefList.row(uniquesIndex[i]).column(12).nodes().to$().find('#chkOthRef_' + QutNo).prop('checked');
                            var IsUseToolAmor = dataTableTbQuoteRefList.row(uniquesIndex[i]).column(13).nodes().to$().find('#DdlToolAmorRef_' + QutNo).val();
                            var IsUseMachineAmor = dataTableTbQuoteRefList.row(uniquesIndex[i]).column(14).nodes().to$().find('#DdlMachineAmortizeRef_' + QutNo).val();

                            var defaultValueToolAmor = dataTableTbQuoteRefList.row(uniquesIndex[i]).column(13).nodes().to$().find('#DdlToolAmorRef_' + QutNo).attr("defaultValue");
                            var defaultValueMacAmor = dataTableTbQuoteRefList.row(uniquesIndex[i]).column(14).nodes().to$().find('#DdlMachineAmortizeRef_' + QutNo).attr("defaultValue");
                            _VendVsMat.push({
                                "QuoteNo": dataTableTbQuoteRefList.data()[uniquesIndex[i]].QuoteNo,
                                "Vendor": dataTableTbQuoteRefList.data()[uniquesIndex[i]].VendorCode1,
                                "VendorName": dataTableTbQuoteRefList.data()[uniquesIndex[i]].VendorName,
                                "Material": dataTableTbQuoteRefList.data()[uniquesIndex[i]].Material,
                                "MaterialDesc": dataTableTbQuoteRefList.data()[uniquesIndex[i]].MaterialDesc,
                                "ProcessGroup": dataTableTbQuoteRefList.data()[uniquesIndex[i]].ProcessGroup,
                                "PrcGrpDesc": dataTableTbQuoteRefList.data()[uniquesIndex[i]].PrcGrpDesc,
                                "MQty": dataTableTbQuoteRefList.data()[uniquesIndex[i]].MQty,
                                "BaseUOM": dataTableTbQuoteRefList.data()[uniquesIndex[i]].BaseUOM,

                                "MaterialType": dataTableTbQuoteRefList.data()[uniquesIndex[i]].MaterialType,
                                "MaterialClass": dataTableTbQuoteRefList.data()[uniquesIndex[i]].MaterialClass,
                                "UOM": dataTableTbQuoteRefList.data()[uniquesIndex[i]].UOM,
                                "PlantStatus": dataTableTbQuoteRefList.data()[uniquesIndex[i]].PlantStatus,
                                "SAPProcType": dataTableTbQuoteRefList.data()[uniquesIndex[i]].SAPProcType,
                                "SAPSpProcType": dataTableTbQuoteRefList.data()[uniquesIndex[i]].SAPSpProcType,
                                "Product": dataTableTbQuoteRefList.data()[uniquesIndex[i]].Product,
                                "PIRType": dataTableTbQuoteRefList.data()[uniquesIndex[i]].PIRType,
                                "PIRJobType": dataTableTbQuoteRefList.data()[uniquesIndex[i]].PIRJobType,
                                "NetUnit": dataTableTbQuoteRefList.data()[uniquesIndex[i]].NetUnit,

                                "IsAllcostAllow": IsAllcostAllow,
                                "IsMatcostAllow": IsMatcostAllow,
                                "IsProccostAllow": IsProccostAllow,
                                "IsSubMatcostAllow": IsSubMatcostAllow,
                                "IsOthcostAllow": IsOthcostAllow,
                                "IsUseToolAmor": IsUseToolAmor + "|" + defaultValueToolAmor,
                                "IsUseMachineAmor": IsUseMachineAmor + "|" + defaultValueMacAmor,

                                "MatcostVal": dataTableTbQuoteRefList.data()[uniquesIndex[i]].TotalMaterialCost,
                                "ProccostVal": dataTableTbQuoteRefList.data()[uniquesIndex[i]].TotalProcessCost,
                                "SubMatcostVal": dataTableTbQuoteRefList.data()[uniquesIndex[i]].TotalSubMaterialCost,
                                "OthcostVal": dataTableTbQuoteRefList.data()[uniquesIndex[i]].TotalOtheritemsCost,

                                "ToolAmorExist": dataTableTbQuoteRefList.data()[uniquesIndex[i]].ToolAmorExist,
                                "ToolAmorExpired": dataTableTbQuoteRefList.data()[uniquesIndex[i]].ToolAmorExpired,
                                "MacAmorExist": dataTableTbQuoteRefList.data()[uniquesIndex[i]].MacAmorExist,
                                "MacAmorExpired": dataTableTbQuoteRefList.data()[uniquesIndex[i]].MacAmorExpired,
                                "Layout": dataTableTbQuoteRefList.data()[uniquesIndex[i]].Layout
                            });
                        }
                        //for (var i = 0; i < Mydata.length; i++) {
                        //    var chkMatRef = document.getElementById("chkMatRef_" + i);
                        //    var chkProcRef = document.getElementById("chkProcRef_" + i);
                        //    var chkSubMatRef = document.getElementById("chkSubMatRef_" + i);
                        //    var chkOthRef = document.getElementById("chkOthRef_" + i);
                        //    if (chkMatRef != null && chkProcRef != null && chkSubMatRef != null && chkOthRef != null) {
                        //        if (chkMatRef.checked == true || chkProcRef.checked == true || chkSubMatRef.checked == true || chkOthRef.checked == true) {
                        //            var IsAllcostAllow = document.getElementById("chkAllRefRw_" + i).checked;
                        //            var IsMatcostAllow = document.getElementById("chkMatRef_" + i).checked;
                        //            var IsProccostAllow = document.getElementById("chkProcRef_" + i).checked;
                        //            var IsSubMatcostAllow = document.getElementById("chkSubMatRef_" + i).checked;
                        //            var IsOthcostAllow = document.getElementById("chkOthRef_" + i).checked;
                        //            var IsUseToolAmor = document.getElementById("DdlToolAmorRef_" + i).value;
                        //            var IsUseMachineAmor = document.getElementById("DdlMachineAmortizeRef_" + i).value;
                        //            _VendVsMat.push({
                        //                "QuoteNo": Mydata[i].QuoteNo,
                        //                "Vendor": Mydata[i].VendorCode1,
                        //                "VendorName": Mydata[i].VendorName,
                        //                "Material": Mydata[i].Material,
                        //                "MaterialDesc": Mydata[i].MaterialDesc,
                        //                "ProcessGroup": Mydata[i].ProcessGroup,
                        //                "PrcGrpDesc": Mydata[i].PrcGrpDesc,
                        //                "MQty": Mydata[i].MQty,
                        //                "BaseUOM": Mydata[i].BaseUOM,

                        //                "MaterialType": Mydata[i].MaterialType,
                        //                "MaterialClass": Mydata[i].MaterialClass,
                        //                "UOM": Mydata[i].UOM,
                        //                "PlantStatus": Mydata[i].PlantStatus,
                        //                "SAPProcType": Mydata[i].SAPProcType,
                        //                "SAPSpProcType": Mydata[i].SAPSpProcType,
                        //                "Product": Mydata[i].Product,
                        //                "PIRType": Mydata[i].PIRType,
                        //                "PIRJobType": Mydata[i].PIRJobType,
                        //                "NetUnit": Mydata[i].NetUnit,

                        //                "IsAllcostAllow": IsAllcostAllow,
                        //                "IsMatcostAllow": IsMatcostAllow,
                        //                "IsProccostAllow": IsProccostAllow,
                        //                "IsSubMatcostAllow": IsSubMatcostAllow,
                        //                "IsOthcostAllow": IsOthcostAllow,
                        //                "IsUseToolAmor": IsUseToolAmor,
                        //                "IsUseMachineAmor": IsUseMachineAmor,

                        //                "MatcostVal": Mydata[i].TotalMaterialCost,
                        //                "ProccostVal": Mydata[i].TotalProcessCost,
                        //                "SubMatcostVal": Mydata[i].TotalSubMaterialCost,
                        //                "OthcostVal": Mydata[i].TotalOtheritemsCost,

                        //                "ToolAmorExist": Mydata[i].ToolAmorExist,
                        //                "ToolAmorExpired": Mydata[i].ToolAmorExpired,
                        //                "MacAmorExist": Mydata[i].MacAmorExist,
                        //                "MacAmorExpired": Mydata[i].MacAmorExpired
                        //            });
                        //        }
                        //    }
                        //}


                        if (_VendVsMat.length <= 0) {
                            alert('No Data Selected');
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
                                        if (DtExpiredInvalid == null) {
                                            DtExpiredInvalid = data;
                                        }
                                        else {
                                            DtExpiredInvalid.push({ data })
                                        }
                                    },
                                    error: function (xhr, status, error) {
                                        alert(error);
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

                            if (DtExpiredInvalid.length > 0) {
                                if (DtExpiredInvalid.success == true && DtExpiredInvalid.message == "Data Invalid Found") {
                                    OpenModalDuplicateExpired();

                                    setTimeout(function () {
                                        var length = $("#lcdatatables").val();
                                        if (length == "" || length == "0") {
                                            length = "1";
                                            $("#lcdatatables").val("1");
                                        }
                                        dataTableDuplicateWithExpiredReq.clear().draw();
                                        dataTableDuplicateWithExpiredReq.rows.add(DtExpiredInvalid.InvalidDataSelected).draw();
                                        dataTableDuplicateWithExpiredReq.columns.adjust().draw();
                                        //length change input textbox
                                        //dataTableDuplicateWithExpiredReq.page.len(length).draw();
                                    }, 500);
                                    CloseLoading();
                                    return false;
                                }
                                else {
                                    if (CekVendorVsMaterial(_VendVsMatOriginal) == true) {
                                        CloseModalQuoteRef();
                                        CloseLoading();
                                        dataTableTbQuoteRefListSelected.column(23).nodes().to$().find('.ddlClassToolAmortize').trigger("change");
                                        return true;
                                    }
                                    else {
                                        return false;
                                    }
                                }
                            }
                            else {
                                if (CekVendorVsMaterial(_VendVsMatOriginal) == true) {
                                    CloseModalQuoteRef();
                                    CloseLoading();
                                    dataTableTbQuoteRefListSelected.column(23).nodes().to$().find('.ddlClassToolAmortize').trigger("change");
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
            if (typeof (Worker) !== "undefined") {
                var Ok = false;
                try {
                    if (typeof (Worker) !== "undefined") {
                        jQuery.noConflict();
                        var url = mainUrl + "/EmetServices/RevisionEMET/MyXml.asmx/CekVendorVsMaterial";
                        var DataInvlid = [];
                        var _VendVsMatOriginal = [];

                        if (_VendVsMat.length <= 0) {
                            alert('No Data Selected');
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
                                        ShowLoading();
                                    },
                                    complete: function () {
                                        CloseLoading();
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
                                                        DataInvlid.push(Dta[0]);
                                                    }
                                                }
                                            }
                                        }
                                        else {
                                            alert(data.message);
                                        }
                                    },
                                    error: function (xhr, status, error) {
                                        alert("CekVendorVsMaterial(_VendVsMat) : " + error)
                                    }
                                });

                                if (cek == false) {
                                    break;
                                }
                                if (_VendVsMat.length <= 100) {
                                    break;
                                }
                            }
                            Ok = true;
                        }
                    }
                } catch (e) {
                    alert("CekVendorVsMaterial(_VendVsMat) : " + e)
                }

                if (Ok = true) {
                    if (DataInvlid.length > 0) {
                        proceedAddToListInvalid(DataInvlid);
                        var Dtfilter = [];
                        for (var i = 0; i < DataInvlid.length; i++) {
                            debugger;
                            var V2 = DataInvlid[i].VendorCode1;
                            var M2 = DataInvlid[i].Material;
                            var VM2 = V2.concat('-', M2);
                            for (var v = 0; v < _VendVsMatOriginal.length; v++) {
                                if (_VendVsMatOriginal[v] != null) {
                                    var V1 = _VendVsMatOriginal[v].Vendor;
                                    var M1 = _VendVsMatOriginal[v].Material;
                                    var VM1 = V1.concat('-', M1);

                                    if (VM1 === VM2) {
                                        delete _VendVsMatOriginal[v];
                                        break;
                                    }
                                    else {
                                        //Dtfilter.push(_VendVsMatOriginal[v]);
                                    }
                                }
                            }
                        }

                        for (var i = 0; i < _VendVsMatOriginal.length; i++) {
                            if (_VendVsMatOriginal[i] != null) {
                                Dtfilter.push(_VendVsMatOriginal[i]);
                            }
                        }

                        proceedAddToList(Dtfilter);
                    }
                    else {
                        proceedAddToList(_VendVsMatOriginal);
                    }
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                alert('CekVendorVsMaterial : Browser Not Support')
            }
        }

        function DisplayDataReqTemp(DataReq) {
            try {
                if (typeof (Worker) !== "undefined") {
                    var length = $("#lcdatatables").val();
                    if (length == "" || length == "0") {
                        length = "1";
                        $("#lcdatatables").val("1");
                    }
                    dataTableTbTbCreateReqTemp.clear().draw();
                    dataTableTbTbCreateReqTemp.rows.add(DataReq).draw();
                    dataTableTbTbCreateReqTemp.columns.adjust().draw();
                }
                else {
                    alert('DisplayDataReqTemp : Browser Not Support')
                }
            } catch (e) {
                alert(e);
            }
        }

        function createRequestTemp() {
            try {
                if (typeof (Worker) !== "undefined") {
                    ShowLoading();
                    setTimeout(function () {
                        var MytableListSelected = $('#TbQuoteRefListSelected').DataTable();
                        var MydataListSelected = MytableListSelected.rows().data();
                        var DataTemp = [];

                        //dataTableTbQuoteRefListSelected.rows().every(function (rowIdx, tableLoop, rowLoop) {
                        //    var data = this.data();
                        //    var id = this.id();

                        //    var QuoteNo = this.data().QuoteNo;
                        //    var Vendor = this.data().Vendor;
                        //    var VendorName = this.data().VendorName;
                        //    var MaterialType = this.data().MaterialType;
                        //    var MaterialClass = this.data().MaterialClass;
                        //    var Material = this.data().Material
                        //    var MaterialDesc = this.data().MaterialDesc
                        //    var ProcessGroup = this.data().ProcessGroup
                        //    var PrcGrpDesc = this.data().PrcGrpDesc

                        //    var UOM = this.data().UOM;
                        //    var PlantStatus = this.data().PlantStatus;
                        //    var SAPProcType = this.data().SAPProcType;
                        //    var SAPSpProcType = this.data().SAPSpProcType;
                        //    var Product = this.data().Product;
                        //    var PIRType = this.data().PIRType;
                        //    var PIRJobType = this.data().PIRJobType;
                        //    var NetUnit = this.data().NetUnit;

                        //    var IsMatcostAllow = $(this.node().cells[9]).find('.chkMatRefSelected_').prop('checked');
                        //    var IsProccostAllow = $(this.node().cells[10]).find('.chkProcRefSelected_').prop('checked');
                        //    var IsSubMatcostAllow = $(this.node().cells[11]).find('.chkSubMatRefSelected_').prop('checked');
                        //    var IsOthcostAllow = $(this.node().cells[12]).find('.chkOthRefSelected_').prop('checked');

                        //    var IsUseToolAmor = $(this.node().cells[13]).find('.ddlClassToolAmortize').val();
                        //    var IsUseMachineAmor = $(this.node().cells[14]).find('.DdlMachineAmortizeRef_').val();

                        //    var ReqPurpose = $(this.node().cells[15]).find('.DdlReqPurposeSelected_').val();
                        //    var Remark = $(this.node().cells[15]).find('.Txtreason_').val();

                        //    var MQty = $(this.node().cells[16]).find('.TxtMQtySelected_').val();
                        //    var BaseUOM = $(this.node().cells[17]).find('.TxtUomSelected_').val();

                        //    var DateDdMmYyyy = $(this.node().cells[18]).find('.TxtResDueDate_').val();
                        //    var splitDdMmYyyy = DateDdMmYyyy.split('-');
                        //    var DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                        //    var myDuedate = new Date(DateYyyyMmDd);
                        //    var ResDueDate = myDuedate;

                        //    DateDdMmYyyy = $(this.node().cells[19]).find('.effectiveDate').val();
                        //    splitDdMmYyyy = DateDdMmYyyy.split('-');
                        //    DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                        //    var myEffdate = new Date(DateYyyyMmDd);
                        //    var EffectiveDate = myEffdate;

                        //    DateDdMmYyyy = $(this.node().cells[20]).find('.TxtDueDateNextRev_').val();
                        //    splitDdMmYyyy = DateDdMmYyyy.split('-');
                        //    DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                        //    var myDuedateNext = new Date(DateYyyyMmDd);
                        //    var DueDateNextRev = myDuedateNext;

                        //    var RecycleRatio = $(this.node().cells[21]).find('.DdlRecycleRatioSelected_').val();
                        //    var FU = $(this.node().cells[22]).find('.FlAtc');
                        //    var MyFileName = "";
                        //    if (FU != null) {
                        //        //var files = FU.files[0];
                        //        var files = FU.prop('files');
                        //        if (files != null) {
                        //            if (files[0] != null) {
                        //                MyFileName = files[0].name;
                        //            }
                        //        }
                        //    }

                        //    DataTemp.push({
                        //        "QuoteNo": QuoteNo,
                        //        "Vendor": Vendor,
                        //        "VendorName": VendorName,
                        //        "MaterialType": MaterialType,
                        //        "MaterialClass": MaterialClass,
                        //        "Material": Material,
                        //        "MaterialDesc": MaterialDesc,
                        //        "ProcessGroup": ProcessGroup,
                        //        "PrcGrpDesc": PrcGrpDesc,
                        //        "MQty": MQty,
                        //        "BaseUOM": BaseUOM,
                        //        "UOM": UOM,
                        //        "PlantStatus": PlantStatus,
                        //        "SAPProcType": SAPProcType,
                        //        "SAPSpProcType": SAPSpProcType,
                        //        "Product": Product,
                        //        "PIRType": PIRType,
                        //        "PIRJobType": PIRJobType,
                        //        "NetUnit": NetUnit,

                        //        "IsMatcostAllow": IsMatcostAllow,
                        //        "IsProccostAllow": IsProccostAllow,
                        //        "IsSubMatcostAllow": IsSubMatcostAllow,
                        //        "IsOthcostAllow": IsOthcostAllow,
                        //        "IsUseToolAmor": IsUseToolAmor,
                        //        "IsUseMachineAmor": IsUseMachineAmor,
                        //        "ReqPurpose": ReqPurpose,
                        //        "Remark": Remark,

                        //        "ResDueDate": ResDueDate,
                        //        "EffectiveDate": EffectiveDate,
                        //        "DueDateNextRev": DueDateNextRev,
                        //        "RecycleRatio": RecycleRatio,
                        //        "FileName": MyFileName
                        //    });
                        //});
                        for (var i = 0; i < MydataListSelected.length; i++) {
                            var data = dataTableTbQuoteRefListSelected.row(i).data();

                            var QuoteNo = dataTableTbQuoteRefListSelected.row(i).data().QuoteNo;
                            var Vendor = dataTableTbQuoteRefListSelected.row(i).data().Vendor;
                            var VendorName = dataTableTbQuoteRefListSelected.row(i).data().VendorName;
                            var MaterialType = dataTableTbQuoteRefListSelected.row(i).data().MaterialType;
                            var MaterialClass = dataTableTbQuoteRefListSelected.row(i).data().MaterialClass;
                            var Material = dataTableTbQuoteRefListSelected.row(i).data().Material
                            var MaterialDesc = dataTableTbQuoteRefListSelected.row(i).data().MaterialDesc
                            var ProcessGroup = dataTableTbQuoteRefListSelected.row(i).data().ProcessGroup
                            var PrcGrpDesc = dataTableTbQuoteRefListSelected.row(i).data().PrcGrpDesc

                            var UOM = dataTableTbQuoteRefListSelected.row(i).data().UOM;
                            var PlantStatus = dataTableTbQuoteRefListSelected.row(i).data().PlantStatus;
                            var SAPProcType = dataTableTbQuoteRefListSelected.row(i).data().SAPProcType;
                            var SAPSpProcType = dataTableTbQuoteRefListSelected.row(i).data().SAPSpProcType;
                            var Product = dataTableTbQuoteRefListSelected.row(i).data().Product;
                            var PIRType = dataTableTbQuoteRefListSelected.row(i).data().PIRType;
                            var PIRJobType = dataTableTbQuoteRefListSelected.row(i).data().PIRJobType;
                            var NetUnit = dataTableTbQuoteRefListSelected.row(i).data().NetUnit;
                                                        
                            var IsMatcostAllow = dataTableTbQuoteRefListSelected.cell(i, 19).nodes().to$().find('.chkMatRefSelected_').prop('checked');
                            var IsProccostAllow = dataTableTbQuoteRefListSelected.cell(i, 20).nodes().to$().find('.chkProcRefSelected_').prop('checked');
                            var IsSubMatcostAllow = dataTableTbQuoteRefListSelected.cell(i, 21).nodes().to$().find('.chkSubMatRefSelected_').prop('checked');
                            var IsOthcostAllow = dataTableTbQuoteRefListSelected.cell(i, 22).nodes().to$().find('.chkOthRefSelected_').prop('checked');

                            var IsUseToolAmor = dataTableTbQuoteRefListSelected.cell(i, 23).nodes().to$().find('.ddlClassToolAmortize option:selected').val();
                            var IsUseMachineAmor = dataTableTbQuoteRefListSelected.cell(i, 24).nodes().to$().find('.DdlMachineAmortizeRef_ option:selected').val();

                            var ReqPurpose = dataTableTbQuoteRefListSelected.cell(i, 29).nodes().to$().find('.DdlReqPurposeSelected_').val();
                            var Remark = dataTableTbQuoteRefListSelected.cell(i, 29).nodes().to$().find('.Txtreason_').val();

                            var MQty = dataTableTbQuoteRefListSelected.cell(i, 30).nodes().to$().find('.TxtMQtySelected_').val();
                            var BaseUOM = dataTableTbQuoteRefListSelected.cell(i, 31).nodes().to$().find('.TxtUomSelected_').val();

                            var DateDdMmYyyy = dataTableTbQuoteRefListSelected.cell(i, 32).nodes().to$().find('.TxtResDueDate_').val();
                            var splitDdMmYyyy = DateDdMmYyyy.split('-');
                            var DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                            var myDuedate = new Date(DateYyyyMmDd);
                            var ResDueDate = myDuedate;

                            DateDdMmYyyy = dataTableTbQuoteRefListSelected.cell(i, 33).nodes().to$().find('.effectiveDate').val();
                            splitDdMmYyyy = DateDdMmYyyy.split('-');
                            DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                            var myEffdate = new Date(DateYyyyMmDd);
                            var EffectiveDate = myEffdate;

                            DateDdMmYyyy = dataTableTbQuoteRefListSelected.cell(i, 34).nodes().to$().find('.TxtDueDateNextRev_').val();
                            splitDdMmYyyy = DateDdMmYyyy.split('-');
                            DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                            var myDuedateNext = new Date(DateYyyyMmDd);
                            var DueDateNextRev = myDuedateNext;

                            var RecycleRatio = dataTableTbQuoteRefListSelected.cell(i, 35).nodes().to$().find('.DdlRecycleRatioSelected_').val();
                            var FU = dataTableTbQuoteRefListSelected.cell(i, 36).nodes().to$().find('.FlAtc');
                            var MyFileName = "";
                            if (FU != null) {
                                //var files = FU.files[0];
                                var files = FU.prop('files');
                                if (files != null) {
                                    if (files[0] != null) {
                                        MyFileName = files[0].name;
                                    }
                                }
                            }

                            DataTemp.push({
                                "QuoteNo": QuoteNo,
                                "Vendor": Vendor,
                                "VendorName": VendorName,
                                "MaterialType": MaterialType,
                                "MaterialClass": MaterialClass,
                                "Material": Material,
                                "MaterialDesc": MaterialDesc,
                                "ProcessGroup": ProcessGroup,
                                "PrcGrpDesc": PrcGrpDesc,
                                "MQty": MQty,
                                "BaseUOM": BaseUOM,
                                "UOM": UOM,
                                "PlantStatus": PlantStatus,
                                "SAPProcType": SAPProcType,
                                "SAPSpProcType": SAPSpProcType,
                                "Product": Product,
                                "PIRType": PIRType,
                                "PIRJobType": PIRJobType,
                                "NetUnit": NetUnit,

                                "IsMatcostAllow": IsMatcostAllow,
                                "IsProccostAllow": IsProccostAllow,
                                "IsSubMatcostAllow": IsSubMatcostAllow,
                                "IsOthcostAllow": IsOthcostAllow,
                                "IsUseToolAmor": IsUseToolAmor,
                                "IsUseMachineAmor": IsUseMachineAmor,
                                "ReqPurpose": ReqPurpose,
                                "Remark": Remark,

                                "ResDueDate": ResDueDate,
                                "EffectiveDate": EffectiveDate,
                                "DueDateNextRev": DueDateNextRev,
                                "RecycleRatio": RecycleRatio,
                                "FileName": MyFileName
                            });
                        }
                        //for (var i = 0; i < MydataListSelected.length; i++) {
                        //    var QuoteNo = MydataListSelected[i].QuoteNo;
                        //    var Vendor = MydataListSelected[i].Vendor;
                        //    var VendorName = MydataListSelected[i].VendorName;
                        //    var MaterialType = MydataListSelected[i].MaterialType;
                        //    var MaterialClass = MydataListSelected[i].MaterialClass;
                        //    var Material = MydataListSelected[i].Material
                        //    var MaterialDesc = MydataListSelected[i].MaterialDesc
                        //    var ProcessGroup = MydataListSelected[i].ProcessGroup
                        //    var PrcGrpDesc = MydataListSelected[i].PrcGrpDesc
                        //    var MQty = MydataListSelected[i].MQty
                        //    var BaseUOM = MydataListSelected[i].BaseUOM
                        //    var UOM = MydataListSelected[i].UOM;
                        //    var PlantStatus = MydataListSelected[i].PlantStatus;
                        //    var SAPProcType = MydataListSelected[i].SAPProcType;
                        //    var SAPSpProcType = MydataListSelected[i].SAPSpProcType;
                        //    var Product = MydataListSelected[i].Product;
                        //    var PIRType = MydataListSelected[i].PIRType;
                        //    var PIRJobType = MydataListSelected[i].PIRJobType;
                        //    var NetUnit = MydataListSelected[i].NetUnit;

                        //    var IsMatcostAllow = document.getElementById('chkMatRefSelected_' + QuoteNo).checked;
                        //    var IsProccostAllow = document.getElementById('chkProcRefSelected_' + QuoteNo).checked;
                        //    var IsSubMatcostAllow = document.getElementById('chkSubMatRefSelected_' + QuoteNo).checked;
                        //    var IsOthcostAllow = document.getElementById('chkOthRefSelected_' + QuoteNo).checked;
                        //    var IsUseToolAmor = document.getElementById('DdlToolAmorRefSelected_' + QuoteNo).value;
                        //    var IsUseMachineAmor = document.getElementById('DdlMachineAmortizeRef_' + QuoteNo).value;
                        //    var ReqPurpose = document.getElementById('DdlReqPurposeSelected_' + QuoteNo).value;
                        //    var Remark = document.getElementById('Txtreason_' + QuoteNo).value;

                        //    var DateDdMmYyyy = document.getElementById('TxtResDueDate_' + QuoteNo).value;
                        //    var splitDdMmYyyy = DateDdMmYyyy.split('-');
                        //    var DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                        //    var myDuedate = new Date(DateYyyyMmDd);
                        //    var ResDueDate = myDuedate;

                        //    DateDdMmYyyy = document.getElementById('TxtEffDate_' + QuoteNo).value;
                        //    splitDdMmYyyy = DateDdMmYyyy.split('-');
                        //    DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                        //    var myEffdate = new Date(DateYyyyMmDd);
                        //    var EffectiveDate = myEffdate;

                        //    DateDdMmYyyy = document.getElementById('TxtDueDateNextRev_' + QuoteNo).value;
                        //    splitDdMmYyyy = DateDdMmYyyy.split('-');
                        //    DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                        //    var myDuedateNext = new Date(DateYyyyMmDd);
                        //    var DueDateNextRev = myDuedateNext;

                        //    var RecycleRatio = document.getElementById('DdlRecycleRatioSelected_' + QuoteNo).value;

                        //    var FU = document.getElementById("FlAtc" + QuoteNo);
                        //    var MyFileName = "";
                        //    if (FU != null) {
                        //        var files = FU.files[0];
                        //        if(files != null){
                        //            MyFileName = files.name;
                        //        }
                        //    }

                        //    DataTemp.push({
                        //        "QuoteNo": QuoteNo,
                        //        "Vendor": Vendor,
                        //        "VendorName": VendorName,
                        //        "MaterialType": MaterialType,
                        //        "MaterialClass": MaterialClass,
                        //        "Material": Material,
                        //        "MaterialDesc": MaterialDesc,
                        //        "ProcessGroup": ProcessGroup,
                        //        "PrcGrpDesc": PrcGrpDesc,
                        //        "MQty": MQty,
                        //        "BaseUOM": BaseUOM,
                        //        "UOM": UOM,
                        //        "PlantStatus": PlantStatus,
                        //        "SAPProcType": SAPProcType,
                        //        "SAPSpProcType": SAPSpProcType,
                        //        "Product": Product,
                        //        "PIRType": PIRType,
                        //        "PIRJobType": PIRJobType,
                        //        "NetUnit": NetUnit,

                        //        "IsMatcostAllow": IsMatcostAllow,
                        //        "IsProccostAllow": IsProccostAllow,
                        //        "IsSubMatcostAllow": IsSubMatcostAllow,
                        //        "IsOthcostAllow": IsOthcostAllow,
                        //        "IsUseToolAmor": IsUseToolAmor,
                        //        "IsUseMachineAmor": IsUseMachineAmor,
                        //        "ReqPurpose": ReqPurpose,
                        //        "Remark": Remark,

                        //        "ResDueDate": ResDueDate,
                        //        "EffectiveDate": EffectiveDate,
                        //        "DueDateNextRev": DueDateNextRev,
                        //        "RecycleRatio": RecycleRatio,
                        //        "FileName": MyFileName
                        //    });
                        //}

                        var jsonText = JSON.stringify({ MyDataTemp: DataTemp });
                        var url = mainUrl + "/EmetServices/RevisionEMET/MyXml.asmx/createRequestTemp";
                        var DataTemp = [];
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
                                ShowLoading();
                            },
                            complete: function () {
                                var length = $("#lcdatatables").val();
                                if (length == "" || length == "0") {
                                    length = "1";
                                    $("#lcdatatables").val("1");
                                }
                                dataTableTbCreateReqTemp.clear().draw();
                                dataTableTbCreateReqTemp.rows.add(DataTemp).draw();
                                dataTableTbCreateReqTemp.columns.adjust().draw();

                                $("#TbQuoteRefListSelected").addClass("tbdisabled");
                                document.getElementById("BtnCreateReq1").disabled = true;
                                document.getElementById("BtnCancelSubmit").style.display = "block";
                                document.getElementById("LbTitleDisabled").style.display = "block";

                                CloseLoading();
                            },
                            success: function (xml, ajaxStatus) {
                                var data = JSON.parse(xml.d.toString());
                                if (data.success == true) {
                                    DataTemp = data.MyDataTemp;
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
            } catch (e) {
                alert("createRequestTemp() : " + e);
                CloseLoading();
            }
        }

        function ProceedSubmitRequestRevisionEmet() {
            var Sccs = false;
            DataValidRequest = [];
            ShowLoading();
            var submitMsg = "";
            setTimeout(function () {
                if (typeof (Worker) !== "undefined") {
                    try {
                        if (typeof (Worker) !== "undefined") {
                            var MyTbCreateReqTemp = $('#TbCreateReqTemp').DataTable();
                            var MydataTbCreateReqTemp = MyTbCreateReqTemp.rows().data();

                            var a = $('#TbQuoteRefListSelected').DataTable();
                            var b = a.rows().data();
                            var formdata = new FormData();
                            for (var i = 0; i < MydataTbCreateReqTemp.length; i++) {
                                var ReqNo = MydataTbCreateReqTemp[i].ReqNo;
                                var QuoteNo = MydataTbCreateReqTemp[i].QuoteNo;
                                var QuoteNoRef = MydataTbCreateReqTemp[i].QuoteNoRef;
                                var VendorCode1 = MydataTbCreateReqTemp[i].VendorCode1;
                                var VendorName = MydataTbCreateReqTemp[i].VendorName;
                                var Material = MydataTbCreateReqTemp[i].Material;
                                var MaterialDesc = MydataTbCreateReqTemp[i].MaterialDesc;
                                var ProcessGroup = MydataTbCreateReqTemp[i].ProcessGroup;
                                var ResDueDate;
                                formdata.append('ReqNo_' + i, ReqNo);
                                formdata.append('QuoteNo_' + i, QuoteNo);
                                formdata.append('QuoteNoRef_' + i, QuoteNoRef);
                                formdata.append('VendorCode1_' + i, VendorCode1);
                                formdata.append('VendorName_' + i, VendorName);
                                formdata.append('Material_' + i, Material);
                                formdata.append('MaterialDesc_' + i, MaterialDesc);
                                formdata.append('ProcessGroup_' + i, ProcessGroup);

                                var MyFileName = "";
                                var MyFile;
                                var DdlToolAmorRef;
                                //dataTableTbQuoteRefListSelected.rows().every(function (rowIdx, tableLoop, rowLoop) {
                                //    var QN = this.data().QuoteNo;

                                //    if (QN == QuoteNoRef) {
                                //        var FU = $(this.node().cells[22]).find('.FlAtc');
                                //        if (FU != null) {
                                //            //var files = FU.files[0];
                                //            var files = FU.prop('files');
                                //            if (files != null) {
                                //                if (files[0] != null) {
                                //                    MyFile = files[0];
                                //                    MyFileName = files[0].name;
                                //                }
                                //            }
                                //        }
                                //        DdlToolAmorRef = $(this.node().cells[13]).find('.ddlClassToolAmortize');
                                //        DdlToolAmorRef = $(this.node().cells[13]).find('.ddlClassToolAmortize');

                                //        var DateDdMmYyyy = $(this.node().cells[18]).find('.TxtResDueDate_').val();
                                //        var splitDdMmYyyy = DateDdMmYyyy.split('-');
                                //        var DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                                //        var myDuedate = new Date(DateYyyyMmDd);
                                //        ResDueDate = myDuedate;
                                //    }
                                //})

                                for (var x = 0; x < dataTableTbQuoteRefListSelected.data().length; x++) {
                                    var QN = dataTableTbQuoteRefListSelected.row(x).data().QuoteNo;
                                    
                                    if (QN == QuoteNoRef) {
                                        var FU = dataTableTbQuoteRefListSelected.cell(x, 36).nodes().to$().find('.FlAtc');
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
                                        DdlToolAmorRef = dataTableTbQuoteRefListSelected.cell(x, 23).nodes().to$().find('.ddlClassToolAmortize');

                                        var DateDdMmYyyy = dataTableTbQuoteRefListSelected.cell(x, 32).nodes().to$().find('.TxtResDueDate_').val();
                                        var splitDdMmYyyy = DateDdMmYyyy.split('-');
                                        var DateYyyyMmDd = splitDdMmYyyy[2].toString() + '-' + splitDdMmYyyy[1].toString() + '-' + splitDdMmYyyy[0].toString();
                                        var myDuedate = new Date(DateYyyyMmDd);
                                        ResDueDate = myDuedate;
                                    }
                                }
                                

                                //var f = b.filter(function (el) {
                                //    return el.QuoteNo === QuoteNoRef
                                //});
                                //var dataFinal = f[0];
                                //var FU = document.getElementById("FlAtc" + dataFinal.QuoteNo);
                                //var files = FU.files[0];
                                var ToolAmorID = "";
                                debugger
                                if (DdlToolAmorRef.val() == "ADD") {
                                    ToolAmorID = dataTableTbQuoteRefListSelected.cell(i, 37).nodes().to$().find('input[name="rb_' + QuoteNoRef + '"]:checked').val();
                                }
                                if (MyFile != null) {
                                    formdata.append('FlAtc' + QuoteNoRef, MyFile);
                                }
                                else {
                                    formdata.append('FlAtc' + QuoteNoRef, "NO FILE");
                                }
                                formdata.append('IsUseToolAmor_' + i, DdlToolAmorRef.val());
                                formdata.append('ToolAmorID_' + i, ToolAmorID);


                                DataValidRequest.push({
                                    "ReqNo": ReqNo,
                                    "QuoteNo": QuoteNo,
                                    "QuoteNoRef": QuoteNoRef,
                                    "VendorCode1": VendorCode1,
                                    "VendorName": VendorName,
                                    "Material": Material,
                                    "MaterialDesc": MaterialDesc,
                                    "ProcessGroup": ProcessGroup,
                                    "ResDueDate": ResDueDate
                                });
                            }

                            $.ajax({
                                type: "POST",
                                url: mainUrl + "/EmetServices/RevisionEMET/SubmitRequestRevisionEmet.ashx",
                                data: formdata,
                                cache: false,
                                async: false,
                                processData: false,
                                contentType: false,
                                beforeSend: function () {
                                    ShowLoading();
                                },
                                complete: function () {
                                    
                                },
                                success: function (result) {
                                    Sccs = true;
                                    submitMsg = result;
                                },
                                error: function (err) {
                                    submitMsg = err.statusText;
                                }
                            });
                        }
                    } catch (e) {
                        Sccs = false;
                        CloseLoading();
                        submitMsg = "ProceedSubmitRequestRevisionEmet : " + e;
                    }
                }
                else {
                    Sccs = false;
                    submitMsg = "ProceedSubmitRequestRevisionEmet : Browser Not Support";
                }

                if (Sccs == true) {
                    setTimeout(function () {
                        SendingMail(submitMsg);
                    }, 100);
                    return true;
                }
                else {
                    alert(submitMsg);
                    return false;
                }
            }, 100);
        }

        function CancelSubmit() {
            $("#TbQuoteRefListSelected").removeClass("tbdisabled");
            document.getElementById("BtnCreateReq1").disabled = false;
            document.getElementById("BtnCancelSubmit").style.display = "none";
            document.getElementById("LbTitleDisabled").style.display = "none";
            dataTableTbCreateReqTemp.clear().draw();
            dataTableTbCreateReqTemp.columns.adjust().draw();
        }

        function SendingMail(submitMsg) {
            ShowLoading();
            setTimeout(function () {
                try {
                    if (typeof (Worker) !== "undefined") {
                        ShowLoading();
                        var jsonText = JSON.stringify({ DataValidRequest: DataValidRequest });
                        var url = mainUrl + "/EmetServices/RevisionEMET/MyXml.asmx/SendingMail";
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
                                ShowLoading();
                            },
                            complete: function () {
                                DataValidRequest = [];
                                window.location = mainUrl +"/Home.aspx";
                                CloseLoading();
                            },
                            success: function (xml, ajaxStatus) {
                                var data = JSON.parse(xml.d.toString());
                                alert(submitMsg + "\n\r" + "SendingMail : " +  data.message);
                            },
                            error: function (xhr, status, error) {
                                alert(submitMsg + "\n\r" + "SendingMail : " + error);
                            }
                        });
                    }
                } catch (e) {
                    DataValidRequest = [];
                    alert("createRequestTemp() : " + submitMsg + "\n\r" + "SendingMail : " + e);
                    CloseLoading();
                }
            }, 100);
        }

        function GetVendorToolAmor(VendorCode, processgrp, EffectiveDate, Material, dataTableToolAmor) {
            if (typeof (Worker) !== "undefined") {
                jQuery.noConflict();
                ShowLoading();
                var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/GetVendorToolAmor";
                var IsExternal = document.getElementById('RbExternal').checked;
                //var jsonText = JSON.stringify({ VendorCode: VendorCode, processgrp: processgrp, IsExternal: IsExternal, EffectiveDate: EffectiveDate, Material: Material });
                $.ajax({
                    url: url,
                    cache: false,
                    type: "POST",
                    dataType: 'json',
                    //contentType: "application/json; charset=utf-8",
                    data: { VendorCode: VendorCode, processgrp: processgrp, IsExternal: IsExternal, EffectiveDate: EffectiveDate, Material: Material },
                    async: false,
                    beforeSend: function () {
                        ShowLoading();
                    },
                    complete: function () {
                        CloseLoading();
                    },
                    success: function (data) {
                        if (data.success == true) {
                            var length = $("#lcDatatables").val();
                            if (length == "" || length == "0") {
                                length = "1";
                                $("#lcDatatables").val("1");
                            }
                            for (var i = 0; i < test11.length; i++) {
                                if (test11[i].QuoteNo == dataTableToolAmor) {
                                    test11[i].table.clear().draw();
                                    test11[i].table.rows.add(data.VendorToolAmortize).draw();
                                    test11[i].table.page.len(length).draw();
                                }
                            }
                            dataTableTbQuoteRefListSelected.columns.adjust().draw();
                            if ($('input[name="rb_' + dataTableToolAmor + '"]:checked').length == 1) {

                            }
                            //dataTableToolAmor.clear().draw();
                            //dataTableToolAmor.rows.add(data.VendorToolAmortize).draw();
                            ////length change input textbox
                            //dataTableToolAmor.page.len(length).draw();
                        }
                        else {
                            alert(data.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });
            }
            else {
                alert('GetVendorToolAmor : Browser Not Support')
            }
        }

        function GetVendorToolAmorOld(QuoteNo) {
            if (typeof (Worker) !== "undefined") {
                jQuery.noConflict();
                ShowLoading();
                var url = mainUrl + "/EmetServices/RevisionEMET/MyJson.asmx/GetVendorToolAmorOld";
                $.ajax({
                    url: url,
                    cache: false,
                    type: "POST",
                    dataType: 'json',
                    //contentType: "application/json; charset=utf-8",
                    data: { QuoteNo: QuoteNo },
                    async: false,
                    beforeSend: function () {
                        ShowLoading();
                    },
                    complete: function () {
                        CloseLoading();
                    },
                    success: function (data) {
                        if (data.success == true) {
                            var length = $("#lcDatatables").val();
                            if (length == "" || length == "0") {
                                length = "1";
                                $("#lcDatatables").val("1");
                            }
                            var toolAmorDet = "";
                            toolAmorDet = data.VendorToolAmortize[0].Amortize_Tool_ID + " : " + data.VendorToolAmortize[0].Amortize_Tool_Desc
                            $("#divToolAmortize_" + QuoteNo).html(toolAmorDet);
                            dataTableTbQuoteRefListSelected.columns.adjust().draw();
                        }
                        else {
                            alert(data.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });
            }
            else {
                alert('GetVendorToolAmorOld : Browser Not Support')
            }
        }
    </script>

    <%--script logic for checkbox quote reference--%>
    <script type="text/javascript">
        function chkAllRefRw_Click(IdNo) {
            var chkAllRefHd = document.getElementById("chkAllRefHd");

            var chkAllMatRefHdr = document.getElementById("chkAllMatRef");
            var chkAllProcRefHdr = document.getElementById("chkAllProcRef");
            var chkAllSubMatRefHdr = document.getElementById("chkAllSubMatRef");
            var chkAllOthRefHdr = document.getElementById("chkAllOthRef");

            var chkAllRefRw = document.getElementById("chkAllRefRw_" + IdNo);
            var chkMatRef = document.getElementById("chkMatRef_" + IdNo);
            var chkProcRef = document.getElementById("chkProcRef_" + IdNo);
            var chkSubMatRef = document.getElementById("chkSubMatRef_" + IdNo);
            var chkOthRef = document.getElementById("chkOthRef_" + IdNo);

            var DdlToolAmor = document.getElementById("DdlToolAmorRef_" + IdNo);
            var DdlMachineAmor = document.getElementById("DdlMachineAmortizeRef_" + IdNo);

            if (chkAllRefRw.checked == true) {
                chkMatRef.checked = true;
                chkProcRef.checked = true;
                chkSubMatRef.checked = true;
                chkOthRef.checked = true;
                DdlToolAmor.disabled = false;
                DdlMachineAmor.disabled = false;

                var chkMatcheckCount = 0;
                var chkProccheckCount = 0;
                var chkSubMatcheckCount = 0;
                var chkOthcheckCount = 0;
                dataTableTbQuoteRefList.column(9).nodes().to$().each(function (index) {
                    if ($(this).find('.checkAllMaterialCost').prop('checked') == false) {
                        chkAllMatRefHdr = false;
                    }
                    else {
                        chkMatcheckCount++;
                    }
                });

                dataTableTbQuoteRefList.column(10).nodes().to$().each(function (index) {
                    if ($(this).find('.checkAllProcessCost').prop('checked') == false) {
                        chkAllProcRefHdr = false;
                    }
                    else {
                        chkProccheckCount++;
                    }
                });

                dataTableTbQuoteRefList.column(11).nodes().to$().each(function (index) {
                    if ($(this).find('.checkAllSubMaterialCost').prop('checked') == false) {
                        chkAllSubMatRefHdr = false;
                    }
                    else {
                        chkSubMatcheckCount++;
                    }
                });

                dataTableTbQuoteRefList.column(12).nodes().to$().each(function (index) {
                    if ($(this).find('.checkAllOtheritemsCost').prop('checked') == false) {
                        chkAllOthRefHdr = false;
                    }
                    else {
                        chkOthcheckCount++;
                    }
                });

                var Mytable = $('#TbQuoteRefList').DataTable();
                var Mydata = Mytable.rows().data();
                if (Mydata.length == chkMatcheckCount) {
                    chkAllMatRefHdr.checked = true;
                }

                if (Mydata.length == chkProccheckCount) {
                    chkAllProcRefHdr.checked = true;
                }

                if (Mydata.length == chkSubMatcheckCount) {
                    chkAllSubMatRefHdr.checked = true;
                }

                if (Mydata.length == chkOthcheckCount) {
                    chkAllOthRefHdr.checked = true;
                }

                if (chkAllMatRefHdr.checked == true && chkAllProcRefHdr.checked == true && chkAllSubMatRefHdr.checked == true && chkAllOthRefHdr.checked == true) {
                    chkAllRefHd.checked = true;
                }
                else {
                    chkAllRefHd.checked = false;
                }
            }
            else {
                chkAllRefHd.checked = false;
                chkAllMatRefHdr.checked = false;
                chkAllProcRefHdr.checked = false;
                chkAllSubMatRefHdr.checked = false;
                chkAllOthRefHdr.checked = false;

                chkMatRef.checked = false;
                chkProcRef.checked = false;
                chkSubMatRef.checked = false;
                chkOthRef.checked = false;
                DdlToolAmor.disabled = true;
                DdlMachineAmor.disabled = true;
            }
        }

        function chkAllRefRwSelected_Click(IdNo) {
            var chkAllRefRw = document.getElementById("chkAllRefRwSelected_" + IdNo);
            var chkMatRef = document.getElementById("chkMatRefSelected_" + IdNo);
            var chkProcRef = document.getElementById("chkProcRefSelected_" + IdNo);
            var chkSubMatRef = document.getElementById("chkSubMatRefSelected_" + IdNo);
            var chkOthRef = document.getElementById("chkOthRefSelected_" + IdNo);

            var DdlToolAmor = document.getElementById("DdlToolAmorRefSelected_" + IdNo);
            var DdlMachineAmor = document.getElementById("DdlMachineAmortizeRef_" + IdNo);

            if (chkAllRefRw.checked == true) {
                chkMatRef.checked = true;
                chkProcRef.checked = true;
                chkSubMatRef.checked = true;
                chkOthRef.checked = true;
                DdlToolAmor.disabled = false;
                DdlMachineAmor.disabled = false;
            }
            else {
                chkMatRef.checked = false;
                chkProcRef.checked = false;
                chkSubMatRef.checked = false;
                chkOthRef.checked = false;
                DdlToolAmor.disabled = true;
                DdlMachineAmor.disabled = true;
            }
        }

        function chkDet_Click(IdNo) {
            var chkAllRefRw = document.getElementById("chkAllRefRw_" + IdNo);
            var chkMatRef = document.getElementById("chkMatRef_" + IdNo);
            var chkProcRef = document.getElementById("chkProcRef_" + IdNo);
            var chkSubMatRef = document.getElementById("chkSubMatRef_" + IdNo);
            var chkOthRef = document.getElementById("chkOthRef_" + IdNo);
            if (chkMatRef.checked == true && chkProcRef.checked == true && chkSubMatRef.checked == true && chkOthRef.checked == true) {
                chkAllRefRw.checked = true;
            }
            else {
                chkAllRefRw.checked = false;
            }

            var ddlToolAmor = document.getElementById("DdlToolAmorRef_" + IdNo);
            var ddlMacAmor = document.getElementById("DdlMachineAmortizeRef_" + IdNo);
            if (chkSubMatRef.checked == true) {
                ddlToolAmor.removeAttribute("disabled");
                if (chkProcRef.checked == true) {
                    ddlMacAmor.removeAttribute("disabled");
                }
                else {
                    ddlMacAmor.setAttribute("disabled", "disabled");
                }
            }
            else {
                ddlToolAmor.setAttribute("disabled", "disabled");
                ddlMacAmor.setAttribute("disabled", "disabled");
            }
        }
        
        function chkDetSelected_Click(IdNo) {
            var chkAllRefRw = document.getElementById("chkAllRefRwSelected_" + IdNo);
            var chkMatRef = document.getElementById("chkMatRefSelected_" + IdNo);
            var chkProcRef = document.getElementById("chkProcRefSelected_" + IdNo);
            var chkSubMatRef = document.getElementById("chkSubMatRefSelected_" + IdNo);
            var chkOthRef = document.getElementById("chkOthRefSelected_" + IdNo);
            if (chkMatRef.checked == true && chkProcRef.checked == true && chkSubMatRef.checked == true && chkOthRef.checked == true) {
                chkAllRefRw.checked = true;
            }
            else {
                chkAllRefRw.checked = false;
            }
            debugger
            var ddlToolAmor = document.getElementById("DdlToolAmorRefSelected_" + IdNo);
            var ddlMacAmor = document.getElementById("DdlMachineAmortizeRef_" + IdNo);
            if (chkSubMatRef.checked == true) {
                ddlToolAmor.removeAttribute("disabled");
                if (chkProcRef.checked == true) {
                    ddlMacAmor.removeAttribute("disabled");
                }
                else {
                    ddlMacAmor.setAttribute("disabled", "disabled");
                    var defValueMacAmor = $("#DdlMachineAmortizeRef_" + IdNo).attr("defaultValue");
                    ddlMacAmor.value = defValueMacAmor;
                }
            }
            else {
                ddlToolAmor.setAttribute("disabled", "disabled");
                ddlMacAmor.setAttribute("disabled", "disabled");

                var defValueToolAmor = $("#DdlToolAmorRefSelected_" + IdNo).attr("defaultValue"); 
                var defValueMacAmor = $("#DdlMachineAmortizeRef_" + IdNo).attr("defaultValue");

                ddlToolAmor.value = defValueToolAmor;
                ddlMacAmor.value = defValueMacAmor;
                $("#DdlToolAmorRefSelected_" + IdNo).val(defValueToolAmor);
                $("#DdlMachineAmortizeRef_" + IdNo).val(defValueMacAmor);
                $("#DdlToolAmorRefSelected_" + IdNo).trigger("change");
            }
        }

        function CheckBasedOnColumn(Id) {
            var Chk = document.getElementById(Id);
            var ChkAll;
            if (Id.includes("chkMatRef_")) {
                ChkAll = document.getElementById("chkAllMatRef");
            }
            else if (Id.includes("chkProcRef_")) {
                ChkAll = document.getElementById("chkAllProcRef");
            }
            else if (Id.includes("chkSubMatRef_")) {
                ChkAll = document.getElementById("chkAllSubMatRef");
            }
            else if (Id.includes("chkOthRef_")) {
                ChkAll = document.getElementById("chkAllOthRef");
            }

            if (Chk.checked == true) {
                var Mytable = $('#TbQuoteRefList').DataTable();
                var Mydata = Mytable.rows().data();
                var IsAllCheck = true;
                for (var IdNo = 0; IdNo < Mydata.length; IdNo++) {
                    var ChkRef;
                    if (Id.includes("chkMatRef_")) {
                        ChkRef = document.getElementById("chkMatRef_" + Mydata[IdNo].QuoteNo);
                    }
                    else if (Id.includes("chkProcRef_")) {
                        ChkRef = document.getElementById("chkProcRef_" + Mydata[IdNo].QuoteNo);
                    }
                    else if (Id.includes("chkSubMatRef_")) {
                        ChkRef = document.getElementById("chkSubMatRef_" + Mydata[IdNo].QuoteNo);
                    }
                    else if (Id.includes("chkOthRef_")) {
                        ChkRef = document.getElementById("chkOthRef_" + Mydata[IdNo].QuoteNo);
                    }

                    if (ChkRef != null) {
                        if (ChkRef.checked == false) {
                            IsAllCheck = false;
                            break;
                        }
                    }
                }

                if (IsAllCheck == true) {
                    ChkAll.checked = true;
                }
                else {
                    ChkAll.checked = false;
                }
            }
            else {
                ChkAll.checked = false;
            }
        }

        function chkAllRefHd_Click() {
            ShowLoading();
            setTimeout(function () {
                if (typeof (Worker) !== "undefined") {
                    var checked = $("#chkAllRefHd").prop("checked");
                    for (var i = 8; i < 13; i++) {
                        dataTableTbQuoteRefList.column(i).nodes().to$().each(function (index) {
                            if (checked) {
                                $(this).find('.checkAllRefRw').prop('checked', 'checked');
                                $(this).find('.checkAllMaterialCost').prop('checked', 'checked');
                                $(this).find('.checkAllProcessCost').prop('checked', 'checked');
                                $(this).find('.checkAllSubMaterialCost').prop('checked', 'checked');
                                $(this).find('.checkAllOtheritemsCost').prop('checked', 'checked');
                                dataTableTbQuoteRefList.row($(this).parents('tr')).column(13).nodes().to$().find('.ToolAmortizeDropdown').prop("disabled", false);
                                dataTableTbQuoteRefList.row($(this).parents('tr')).column(14).nodes().to$().find('.MachineAmortizeDropdown').prop("disabled", false);
                            } else {
                                $(this).find('.checkAllRefRw').prop('checked', false);
                                $(this).find('.checkAllMaterialCost').prop('checked', false);
                                $(this).find('.checkAllProcessCost').prop('checked', false);
                                $(this).find('.checkAllSubMaterialCost').prop('checked', false);
                                $(this).find('.checkAllOtheritemsCost').prop('checked', false);
                                dataTableTbQuoteRefList.row($(this).parents('tr')).column(13).nodes().to$().find('.ToolAmortizeDropdown').prop("disabled", true);
                                dataTableTbQuoteRefList.row($(this).parents('tr')).column(14).nodes().to$().find('.MachineAmortizeDropdown').prop("disabled", true);
                            }
                        });
                        dataTableTbQuoteRefList.draw();
                    }

                    var Mytable = $('#TbQuoteRefList').DataTable();
                    var Mydata = Mytable.rows().data();
                    var chkAllRefHd = document.getElementById('chkAllRefHd');
                    if (chkAllRefHd.checked == true) {
                        document.getElementById("chkAllMatRef").checked = true;
                        document.getElementById("chkAllProcRef").checked = true;
                        document.getElementById("chkAllSubMatRef").checked = true;
                        document.getElementById("chkAllOthRef").checked = true;

                        for (var IdNo = 0; IdNo < Mydata.length; IdNo++) {
                            var chkAllRefRw = document.getElementById("chkAllRefRw_" + Mydata[IdNo].QuoteNo);
                            var chkMatRef = document.getElementById("chkMatRef_" + Mydata[IdNo].QuoteNo);
                            var chkProcRef = document.getElementById("chkProcRef_" + Mydata[IdNo].QuoteNo);
                            var chkSubMatRef = document.getElementById("chkSubMatRef_" + Mydata[IdNo].QuoteNo);
                            var chkOthRef = document.getElementById("chkOthRef_" + Mydata[IdNo].QuoteNo);

                            if (chkAllRefRw != null && chkMatRef != null && chkProcRef != null && chkSubMatRef != null && chkOthRef != null) {
                                chkAllRefRw.checked = true;
                                chkMatRef.checked = true;
                                chkProcRef.checked = true;
                                chkSubMatRef.checked = true;
                                chkOthRef.checked = true;
                            }
                        }
                    }
                    else {
                        document.getElementById("chkAllMatRef").checked = false;
                        document.getElementById("chkAllProcRef").checked = false;
                        document.getElementById("chkAllSubMatRef").checked = false;
                        document.getElementById("chkAllOthRef").checked = false;

                        for (var IdNo = 0; IdNo < Mydata.length; IdNo++) {
                            var chkAllRefRw = document.getElementById("chkAllRefRw_" + Mydata[IdNo].QuoteNo);
                            var chkMatRef = document.getElementById("chkMatRef_" + Mydata[IdNo].QuoteNo);
                            var chkProcRef = document.getElementById("chkProcRef_" + Mydata[IdNo].QuoteNo);
                            var chkSubMatRef = document.getElementById("chkSubMatRef_" + Mydata[IdNo].QuoteNo);
                            var chkOthRef = document.getElementById("chkOthRef_" + Mydata[IdNo].QuoteNo);

                            if (chkAllRefRw != null && chkMatRef != null && chkProcRef != null && chkSubMatRef != null && chkOthRef != null) {
                                chkAllRefRw.checked = false;
                                chkMatRef.checked = false;
                                chkProcRef.checked = false;
                                chkSubMatRef.checked = false;
                                chkOthRef.checked = false;
                            }
                        }
                    }
                    CloseLoading();
                } else {
                    // Sorry! No Web Worker support..
                }
            }, 2000);
            
        }

        function chkHeaderColumn_Click(Type) {
            ShowLoading();
            setTimeout(function () {
                if (typeof (Worker) !== "undefined") {
                    var Mytable = $('#TbQuoteRefList').DataTable();
                    var Mydata = Mytable.rows().data();
                    var ChkHeaderColumn;
                    if (Type == "MatCost") {
                        ChkHeaderColumn = document.getElementById("chkAllMatRef");

                        var checked = $("#chkAllMatRef").prop("checked");
                        dataTableTbQuoteRefList.column(9).nodes().to$().each(function (index) {
                            if (checked) {
                                $(this).find('.checkAllMaterialCost').prop('checked', 'checked');
                            } else {
                                $(this).find('.checkAllMaterialCost').prop('checked', false);
                            }
                        });
                        dataTableTbQuoteRefList.draw();
                    }
                    else if (Type == "ProcCost") {
                        ChkHeaderColumn = document.getElementById("chkAllProcRef");

                        var checked = $("#chkAllProcRef").prop("checked");
                        dataTableTbQuoteRefList.column(10).nodes().to$().each(function (index) {
                            if (checked) {
                                $(this).find('.checkAllProcessCost').prop('checked', 'checked');

                                var chkSubmatref = dataTableTbQuoteRefList.row($(this).parents('tr')).column(11).nodes().to$().find('.checkAllSubMaterialCost').prop("checked");
                                if (chkSubmatref) {
                                    dataTableTbQuoteRefList.row($(this).parents('tr')).column(14).nodes().to$().find('.MachineAmortizeDropdown').prop("disabled", false);
                                } else {
                                    dataTableTbQuoteRefList.row($(this).parents('tr')).column(14).nodes().to$().find('.MachineAmortizeDropdown').prop("disabled", true);
                                }
                            } else {
                                $(this).find('.checkAllProcessCost').prop('checked', false);
                                dataTableTbQuoteRefList.row($(this).parents('tr')).column(14).nodes().to$().find('.MachineAmortizeDropdown').prop("disabled", true);
                            }
                        });
                        dataTableTbQuoteRefList.draw();
                    }
                    else if (Type == "SubMatCost") {
                        ChkHeaderColumn = document.getElementById("chkAllSubMatRef");

                        var checked = $("#chkAllSubMatRef").prop("checked");
                        dataTableTbQuoteRefList.column(11).nodes().to$().each(function (index) {
                            if (checked) {
                                $(this).find('.checkAllSubMaterialCost').prop('checked', 'checked');
                                dataTableTbQuoteRefList.row($(this).parents('tr')).column(13).nodes().to$().find('.ToolAmortizeDropdown').prop("disabled", false);

                                var chkProcref = dataTableTbQuoteRefList.row($(this).parents('tr')).column(10).nodes().to$().find('.checkAllProcessCost').prop("checked");
                                if (chkProcref) {
                                    dataTableTbQuoteRefList.row($(this).parents('tr')).column(14).nodes().to$().find('.MachineAmortizeDropdown').prop("disabled", false);
                                } else {
                                    dataTableTbQuoteRefList.row($(this).parents('tr')).column(14).nodes().to$().find('.MachineAmortizeDropdown').prop("disabled", true);
                                }
                            } else {
                                $(this).find('.checkAllSubMaterialCost').prop('checked', false);
                                dataTableTbQuoteRefList.row($(this).parents('tr')).column(13).nodes().to$().find('.ToolAmortizeDropdown').prop("disabled", true);
                                dataTableTbQuoteRefList.row($(this).parents('tr')).column(14).nodes().to$().find('.MachineAmortizeDropdown').prop("disabled", true);
                            }
                        });
                        dataTableTbQuoteRefList.draw();
                    }
                    else if (Type == "OthCost") {
                        ChkHeaderColumn = document.getElementById("chkAllOthRef");

                        var checked = $("#chkAllOthRef").prop("checked");
                        dataTableTbQuoteRefList.column(12).nodes().to$().each(function (index) {
                            if (checked) {
                                $(this).find('.checkAllOtheritemsCost').prop('checked', 'checked');
                            } else {
                                $(this).find('.checkAllOtheritemsCost').prop('checked', false);
                            }
                        });
                        dataTableTbQuoteRefList.draw();
                    }

                    if (ChkHeaderColumn.checked == true) {
                        for (var IdNo = 0; IdNo < Mydata.length; IdNo++) {
                            var Chk;
                            if (Type == "MatCost") {
                                Chk = document.getElementById("chkMatRef_" + Mydata[IdNo].QuoteNo);
                            }
                            else if (Type == "ProcCost") {
                                Chk = document.getElementById("chkProcRef_" + Mydata[IdNo].QuoteNo);
                            }
                            else if (Type == "SubMatCost") {
                                Chk = document.getElementById("chkSubMatRef_" + Mydata[IdNo].QuoteNo);
                            }
                            else if (Type == "OthCost") {
                                Chk = document.getElementById("chkOthRef_" + Mydata[IdNo].QuoteNo);
                            }

                            if (Chk != null) {
                                Chk.checked = true;
                            }
                        }
                    }
                    else {
                        for (var IdNo = 0; IdNo < Mydata.length; IdNo++) {
                            var Chk;
                            if (Type == "MatCost") {
                                Chk = document.getElementById("chkMatRef_" + Mydata[IdNo].QuoteNo);
                            }
                            else if (Type == "ProcCost") {
                                Chk = document.getElementById("chkProcRef_" + Mydata[IdNo].QuoteNo);
                            }
                            else if (Type == "SubMatCost") {
                                Chk = document.getElementById("chkSubMatRef_" + Mydata[IdNo].QuoteNo);
                            }
                            else if (Type == "OthCost") {
                                Chk = document.getElementById("chkOthRef_" + Mydata[IdNo].QuoteNo);
                            }
                            if (Chk != null) {
                                Chk.checked = false;
                            }
                        }
                    }
                    CloseLoading();
                } else {
                    // Sorry! No Web Worker support..
                }
            }, 2000);
            
        }
    </script>

    <%--submit & action display data duplicate reuest with expired request--%>
    <script type="text/javascript">
        function ValidateDuplicateReqList() {
            try {
                var Mytable = $('#TbDuplicateWithExpiredReq').DataTable();
                var Mydata = Mytable.rows().data();
                var RbRej, RbchangeDate, TxtNewResDueDate;
                var IsAllCheck = true;
                var IsValidNewResDueDate = true;
                var ReqNo = "";

                for (var i = 0; i < Mydata.length; i++) {
                    RbRej = document.getElementById("RbRej_" + Mydata[i].QuoteNo);
                    RbchangeDate = document.getElementById("RbchangeDate_" + Mydata[i].QuoteNo);

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
                        RbRej = document.getElementById("RbRej_" + Mydata[i].QuoteNo);
                        RbchangeDate = document.getElementById("RbchangeDate_" + Mydata[i].QuoteNo);
                        TxtNewResDueDate = document.getElementById("TxtNewResDueDate_" + Mydata[i].QuoteNo);

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
                var url = mainUrl + "/EmetServices/Revision.asmx/SumbitDuplicateReqList";
                var Mytable = $('#TbDuplicateWithExpiredReq').DataTable();
                var Mydata = Mytable.rows().data();

                var _DuplicateReqListAction = [];
                for (var i = 0; i < Mydata.length; i++) {
                    var ActionRej = document.getElementById("RbRej_" + Mydata[i].QuoteNo).checked;
                    var NewResDueDate = document.getElementById("TxtNewResDueDate_" + Mydata[i].QuoteNo).value;
                    _DuplicateReqListAction.push({
                        "RequestNumber": Mydata[i].RequestNumber.toString(),
                        "QuoteNo": Mydata[i].QuoteNo.toString(),
                        "ActionRej": ActionRej,
                        "NewResDueDate": NewResDueDate,
                    })
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
                            alert(Mydata.message);
                            CloseModalDuplicateExpired();
                        }
                        else {
                            alert(Mydata.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });
            } catch (e) {
                alert(e)
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
                        RbRej = document.getElementById("RbRej_" + Mydata[i].QuoteNo);
                        RbchangeDate = document.getElementById("RbchangeDate_" + Mydata[i].QuoteNo);
                        if (RbRej != null) {
                            RbRej.checked = true;
                            RbRejectExpReq(i);
                        }
                    }
                }
                else {
                    for (var i = 0; i < Mydata.length; i++) {
                        RbRej = document.getElementById("RbRej_" + Mydata[i].QuoteNo);
                        RbchangeDate = document.getElementById("RbchangeDate_" + Mydata[i].QuoteNo);

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
            }
            catch (err) {
                alert(err + ": DatePitcker(txtID)");
            }
        }

        function preventInput(evnt) {
            if (evnt.which != 9) evnt.preventDefault();
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
                        var RbRej = document.getElementById("RbRej_" + Mydata[i].QuoteNo);
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
                        var RbchangeDate = document.getElementById("RbchangeDate_" + Mydata[i].QuoteNo);
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

        function ConfirmChange() {
            try {
                var Mytable = $('#TbQuoteRefListSelected').DataTable();
                var Mydata = Mytable.rows().data();
                if (Mydata.length >0 ) {
                    if (confirm('Are you sure want to change vendor type and reset the Quote Reference List ?')) {
                        dataTableTbQuoteRefListSelected.clear().draw();
                        dataTableTbQuoteRefListSelected.columns.adjust().draw();
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            catch (err) {
                alert(err);
                return false;
            }
        }

        function ConfirmAddMore() {
            try {
                var Mytable = $('#TbCreateReqTemp').DataTable();
                var Mydata = Mytable.rows().data();

                if (Mydata.length > 0) {
                    if (confirm('Add more Quote Reference List will reset quote request list, are you sure ?')) {
                        dataTableTbCreateReqTemp.clear().draw();
                        dataTableTbCreateReqTemp.columns.adjust().draw();
                        CancelSubmit();
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (err) {
                alert(err + ": ConfirmAddMore");
                return false;
            }
        }

        function CheckVendorType() {
            try {
                var RbExternal = $("#RbExternal").prop("checked");
                var RbTeamSMN = $("#RbTeamShimano").prop("checked");
                var textHeader = "";
                if (RbExternal == false && RbTeamSMN == false) {
                    alert('Please Select Vendor Type !');
                    return false;
                }
                else {
                    if (RbExternal == true) {
                        textHeader = "Find Quote Reference List (External)";
                    }
                    else {
                        textHeader = "Find Quote Reference List (SBM)";
                    }
                    document.getElementById('LbQoRefTitle').innerHTML = textHeader;
                }
            }
            catch (err) {
                alert(err + ": CheckVendorType");
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

                var txtIDLght = document.getElementById(txtID).value.length;
                if (txtIDLght == 0) {
                    document.getElementById(txtID).style.border = "1px solid #ff0000";
                }
                else {
                    document.getElementById(txtID).style.border = "1px solid #CCCCCC";
                }
            }
            catch (err) {
                alert(err + " : validateNumber(txtID)")
            }
        }

        function SetBordrColor(txtID) {
            try {
                var txtIDLght = document.getElementById(txtID).value.length;
                if (txtIDLght == 0) {
                    document.getElementById(txtID).style.border = "1px solid #ff0000";
                }
                else {
                    document.getElementById(txtID).style.border = "1px solid #CCCCCC";
                }

                HideBtnSubmit();
            }
            catch (err) {
                alert(err + " : validateNumber(txtID)")
            }
        }

        function validateFileUpload(FileUploadId) {
            try {

                var iserr = false;
                var fu = document.getElementById(FileUploadId);
                var val = $("#"+ FileUploadId +"").val().toLowerCase();
                //var regex = new RegExp("(.*?)\.(pdf)$");
                var regex = new RegExp("\.(pdf)$");

                var err = "";
                if (fu.value.length <= 0) {
                    err = "Please select the file !";
                    iserr = true;
                }
                else {
                    if (!(regex.test(val))) {
                        $("#"+ FileUploadId +"").css("border", "1px solid #ff0000");
                        err = "Invalid File. Please upload a File with (.pdf)extension";
                        $("#"+ FileUploadId +"").val("");
                        iserr = true;
                    }
                    else {
                        var fileSize = fu.files[0].size;
                        if (fileSize <= 3145728) {
                            var format = /[!@#$%^&*()+\=\[\]{};':"\\|,.<>\/?]/;
                            var objRE = new RegExp(/([^\/\\]+)$/);
                            var FlName = objRE.exec(document.getElementById(FileUploadId).value);
                            var NewFlName = FlName[0].replace('.pdf', '');
                            if (format.test(NewFlName)) {
                                $("#" + FileUploadId + "").css("border", "1px solid #ff0000");
                                err = "Invalid File Name. \n File name cannot contain below character : \n [ ! @ # $ % ^ & * ( ) + \ = \ [ \ ] { } ; ' : \" \\ | , . < > \ / ? ] ";
                                $("#" + FileUploadId + "").val("");
                                iserr = true;
                            }
                        }
                        else {
                            var MB = fileSize / 1048576;
                            $("#" + FileUploadId + "").css("border", "1px solid #ff0000");
                            err = "File is too large, Maximum file size 3 Mb. File  Size: " + MB.toFixed(1) + " Mb";
                            $("#" + FileUploadId + "").val("");
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
                alert(err + ": validateBtnUpload()");
                return false;
            }
        }

        function ValidateCreatereq(){
            var ok = false;
            try {
                var Mytable = $('#TbQuoteRefListSelected').DataTable();
                var Mydata = Mytable.rows().data();
                if(Mydata.length <= 0){
                    alert('No Data Selected to Process');
                }
                else{
                    var ErrMsg = "";
                    var i = 0;
                    //dataTableTbQuoteRefListSelected.rows().every(function (rowIdx, tableLoop, rowLoop) {
                    //    var data = this.data();
                    //    var id = this.id();

                    //    var cMatCost = $(this.node().cells[9]).find('.chkMatRefSelected_');
                    //    var cProcCost = $(this.node().cells[10]).find('.chkProcRefSelected_');
                    //    var cSubMatCost = $(this.node().cells[11]).find('.chkSubMatRefSelected_');
                    //    var cOthCost = $(this.node().cells[12]).find('.chkOthRefSelected_');

                    //    var ToolAmor = $(this.node().cells[13]).find('.ddlClassToolAmortize');
                    //    var MachineAmor = $(this.node().cells[14]).find('.DdlMachineAmortizeRef_');
                    //    var ReqPurpose = $(this.node().cells[15]).find('.DdlReqPurposeSelected_');
                    //    var M_EstQty = $(this.node().cells[16]).find('.TxtMQtySelected_');
                    //    var BaseUOM = $(this.node().cells[17]).find('.TxtUomSelected_');
                    //    var DueDate = $(this.node().cells[18]).find('.TxtResDueDate_');
                    //    var EffDate = $(this.node().cells[19]).find('.effectiveDate');
                    //    var DueDateNextRev = $(this.node().cells[20]).find('.TxtDueDateNextRev_');
                    //    var RecRatio = $(this.node().cells[21]).find('.DdlRecycleRatioSelected_');
                    //    var Layout = this.data().Layout;
                    //    var QuoteNo = this.data().QuoteNo;

                    //    if (cMatCost.prop('checked') == false && cProcCost.prop('checked') == false && cSubMatCost.prop('checked') == false && cOthCost.prop('checked') == false) {
                    //        ErrMsg += "Select at least one cost " + " Row No : " + (i + 1) + "\r\n";

                    //        cMatCost.css("outline-color", "red");
                    //        cMatCost.css("outline-style", "solid");
                    //        cMatCost.css("outline-width", "1px");

                    //        cProcCost.css("outline-color", "red");
                    //        cProcCost.css("outline-style", "solid");
                    //        cProcCost.css("outline-width", "1px");

                    //        cSubMatCost.css("outline-color", "red");
                    //        cSubMatCost.css("outline-style", "solid");
                    //        cSubMatCost.css("outline-width", "1px");

                    //        cOthCost.css("outline-color", "red");
                    //        cOthCost.css("outline-style", "solid");
                    //        cOthCost.css("outline-width", "1px");
                    //    }

                    //    if (ToolAmor.val() == "ADD") {
                    //        var hasDataTool = $("#tableAmortize_" + QuoteNo).DataTable().data().length;
                    //        var selectDataTool = $('input[name="rb_' + QuoteNo + '"]:checked').length;

                    //        if (hasDataTool == 0) {
                    //            ErrMsg += "Tool Amortize Not Exists " + " Row No : " + (i + 1) + "\r\n";
                    //        }
                    //        if (hasDataTool > 0 && selectDataTool == 0) {
                    //            ErrMsg += "Select Tool Amortize " + " Row No : " + (i + 1) + "\r\n";
                    //        }
                    //    }

                    //    if (ToolAmor.val() == "0") {
                    //        ErrMsg += "Select Action for Tool Amortize " + " Row No : " + (i + 1) + "\r\n";
                    //        ToolAmor.css("border", "1px solid #ff0000");
                    //    }

                    //    if (MachineAmor.val() == "0") {
                    //        ErrMsg += "Select Action for Machine Amortize" + " Row No : " + (i + 1) + "\r\n";
                    //        MachineAmor.css("border", "1px solid #ff0000");
                    //    }

                    //    if (ReqPurpose.val() == "0") {
                    //        ReqPurpose.css("border", "1px solid #ff0000");
                    //        ErrMsg += "Select Request Purpose" + " Row No : " + (i + 1) + "\r\n";
                    //    }

                    //    if (M_EstQty.val().trim() == "") {
                    //        M_EstQty.css("border", "1px solid #ff0000");
                    //        ErrMsg += "Enter Mnth. Est. Qty" + " Row No : " + (i + 1) + "\r\n";
                    //    }

                    //    if (BaseUOM.val().trim() == "") {
                    //        BaseUOM.css("border", "1px solid #ff0000");
                    //        ErrMsg += "Enter Base UOM" + " Row No : " + (i + 1) + "\r\n";
                    //    }

                    //    if (DueDate.val().trim() == "") {
                    //        DueDate.css("border", "1px solid #ff0000");
                    //        ErrMsg += "Enter Res.Due Date" + "Row No : " + (i + 1) + "\r\n";
                    //    }

                    //    if (EffDate.val().trim() == "") {
                    //        EffDate.css("border", "1px solid #ff0000");
                    //        ErrMsg += "Enter Effective Date" + "Row No : " + (i + 1) + "\r\n";
                    //    }

                    //    if (DueDateNextRev.val().trim() == "") {
                    //        DueDateNextRev.css("border", "1px solid #ff0000");
                    //        ErrMsg += "Enter Due Dt Next Rev" + "Row No : " + (i + 1) + "\r\n";
                    //    }

                    //    if (EffDate.val() != "" && DueDateNextRev.val() != "") {
                    //        var StrEffDate = EffDate.toString().replace(/\-/g, '.');
                    //        var strDueOn = DueDateNextRev.toString().replace(/\-/g, '.');

                    //        var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
                    //        var dtEffDate = new Date(StrEffDate.replace(pattern, '$3-$2-$1'));
                    //        var dtDueOn = new Date(strDueOn.replace(pattern, '$3-$2-$1'));

                    //        if (dtEffDate > dtDueOn) {
                    //            ErrMsg += "Due Dt Next Rev cannot be small than Effective date  " + "Row No : " + (i + 1) + "\r\n";

                    //            EffDate.css("border", "1px solid #ff0000");
                    //            DueDateNextRev.css("border", "1px solid #ff0000");
                    //        }
                    //    }

                    //    if (RecRatio.val() == "" && Layout == "LAYOUT1") {
                    //        ErrMsg += "Select Recycle Ratio (%)	" + "Row No : " + (i + 1) + "\r\n";
                    //        RecRatio.css("border", "1px solid #ff0000");
                    //    }

                    //    i++
                    //});

                    for (var i = 0; i < Mydata.length; i++) {
                        var cMatCost = dataTableTbQuoteRefListSelected.cell(i, 19).nodes().to$().find('.chkMatRefSelected_');
                        var cProcCost = dataTableTbQuoteRefListSelected.cell(i, 20).nodes().to$().find('.chkProcRefSelected_');
                        var cSubMatCost = dataTableTbQuoteRefListSelected.cell(i, 21).nodes().to$().find('.chkSubMatRefSelected_');
                        var cOthCost = dataTableTbQuoteRefListSelected.cell(i, 22).nodes().to$().find('.chkOthRefSelected_');
                        var ToolAmor = dataTableTbQuoteRefListSelected.cell(i, 23).nodes().to$().find('.ddlClassToolAmortize');
                        var MachineAmor = dataTableTbQuoteRefListSelected.cell(i, 24).nodes().to$().find('.DdlMachineAmortizeRef_');
                        var ReqPurpose = dataTableTbQuoteRefListSelected.cell(i, 29).nodes().to$().find('.DdlReqPurposeSelected_');
                        var M_EstQty = dataTableTbQuoteRefListSelected.cell(i, 30).nodes().to$().find('.TxtMQtySelected_');
                        var BaseUOM = dataTableTbQuoteRefListSelected.cell(i, 31).nodes().to$().find('.TxtUomSelected_');
                        var DueDate = dataTableTbQuoteRefListSelected.cell(i, 32).nodes().to$().find('.TxtResDueDate_');
                        var EffDate = dataTableTbQuoteRefListSelected.cell(i, 33).nodes().to$().find('.effectiveDate');
                        var DueDateNextRev = dataTableTbQuoteRefListSelected.cell(i, 34).nodes().to$().find('.TxtDueDateNextRev_');
                        var RecRatio = dataTableTbQuoteRefListSelected.cell(i, 35).nodes().to$().find('.DdlRecycleRatioSelected_');
                        var Layout = dataTableTbQuoteRefListSelected.row(i).data().Layout;
                        var QuoteNo = dataTableTbQuoteRefListSelected.row(i).data().QuoteNo;
                        
                        if (cMatCost.prop('checked') == false && cProcCost.prop('checked') == false && cSubMatCost.prop('checked') == false && cOthCost.prop('checked') == false) {
                            ErrMsg += "Select at least one cost " + " Row No : " + (i + 1) + "\r\n";

                            cMatCost.css("outline-color", "red");
                            cMatCost.css("outline-style", "solid");
                            cMatCost.css("outline-width", "1px");

                            cProcCost.css("outline-color", "red");
                            cProcCost.css("outline-style", "solid");
                            cProcCost.css("outline-width", "1px");

                            cSubMatCost.css("outline-color", "red");
                            cSubMatCost.css("outline-style", "solid");
                            cSubMatCost.css("outline-width", "1px");

                            cOthCost.css("outline-color", "red");
                            cOthCost.css("outline-style", "solid");
                            cOthCost.css("outline-width", "1px");
                        }

                        if (ToolAmor.val() == "ADD") {
                            var table = "";
                            for (var x = 0; x < test11.length; x++) {
                                if (test11[x].QuoteNo == QuoteNo) {
                                    table = test11[x].table;
                                }
                            }
                            var hasDataTool = 0;
                            if (table != "") {
                                hasDataTool = table.data().length;
                            }
                            var selectDataTool = dataTableTbQuoteRefListSelected.cell(i, 37).nodes().to$().find('input[name="rb_' + QuoteNo + '"]:checked').length;

                            if (hasDataTool == 0) {
                                ErrMsg += "Tool Amortize Not Exists " + " Row No : " + (i + 1) + "\r\n";
                            }
                            if (hasDataTool > 0 && selectDataTool == 0) {
                                ErrMsg += "Select Tool Amortize " + " Row No : " + (i + 1) + "\r\n";
                            }
                        }

                        if (ToolAmor.val() == "0") {
                            ErrMsg += "Select Action for Tool Amortize " + " Row No : " + (i + 1) + "\r\n";
                            ToolAmor.css("border", "1px solid #ff0000");
                        }

                        if (MachineAmor.val() == "0") {
                            ErrMsg += "Select Action for Machine Amortize" + " Row No : " + (i + 1) + "\r\n";
                            MachineAmor.css("border", "1px solid #ff0000");
                        }

                        if (ReqPurpose.val() == "0") {
                            ReqPurpose.css("border", "1px solid #ff0000");
                            ErrMsg += "Select Request Purpose" + " Row No : " + (i + 1) + "\r\n";
                        }

                        if (M_EstQty.val().trim() == "") {
                            M_EstQty.css("border", "1px solid #ff0000");
                            ErrMsg += "Enter Mnth. Est. Qty" + " Row No : " + (i + 1) + "\r\n";
                        }

                        if (BaseUOM.val().trim() == "") {
                            BaseUOM.css("border", "1px solid #ff0000");
                            ErrMsg += "Enter Base UOM" + " Row No : " + (i + 1) + "\r\n";
                        }

                        if (DueDate.val().trim() == "") {
                            DueDate.css("border", "1px solid #ff0000");
                            ErrMsg += "Enter Res.Due Date" + "Row No : " + (i + 1) + "\r\n";
                        }

                        if (EffDate.val().trim() == "") {
                            EffDate.css("border", "1px solid #ff0000");
                            ErrMsg += "Enter Effective Date" + "Row No : " + (i + 1) + "\r\n";
                        }

                        if (DueDateNextRev.val().trim() == "") {
                            DueDateNextRev.css("border", "1px solid #ff0000");
                            ErrMsg += "Enter Due Dt Next Rev" + "Row No : " + (i + 1) + "\r\n";
                        }

                        if (EffDate.val() != "" && DueDateNextRev.val() != "") {
                            var StrEffDate = EffDate.toString().replace(/\-/g, '.');
                            var strDueOn = DueDateNextRev.toString().replace(/\-/g, '.');

                            var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
                            var dtEffDate = new Date(StrEffDate.replace(pattern, '$3-$2-$1'));
                            var dtDueOn = new Date(strDueOn.replace(pattern, '$3-$2-$1'));

                            if (dtEffDate > dtDueOn) {
                                ErrMsg += "Due Dt Next Rev cannot be small than Effective date  " + "Row No : " + (i + 1) + "\r\n";

                                EffDate.css("border", "1px solid #ff0000");
                                DueDateNextRev.css("border", "1px solid #ff0000");
                            }
                        }

                        if (RecRatio.val() == "" && Layout == "LAYOUT1") {
                            ErrMsg += "Select Recycle Ratio (%)	" + "Row No : " + (i + 1) + "\r\n";
                            RecRatio.css("border", "1px solid #ff0000");
                        }
                    }

                    //for (var i = 0; i < Mydata.length; i++) {
                    //    //dataTableTbQuoteRefListSelected.column(9).nodes().to$().each(function (index) {
                    //    //    var cMatCost = $(this).find('#chkMatRefSelected_').prop('checked');
                    //    //});

                    //    var cMatCost = document.getElementById("chkMatRefSelected_" + Mydata[i].QuoteNo + "").checked;
                    //    var cProcCost = document.getElementById("chkProcRefSelected_" + Mydata[i].QuoteNo + "").checked;
                    //    var cSubMatCost = document.getElementById("chkSubMatRefSelected_" + Mydata[i].QuoteNo + "").checked;
                    //    var cOthCost = document.getElementById("chkOthRefSelected_" + Mydata[i].QuoteNo + "").checked;

                    //    var ToolAmor = document.getElementById("DdlToolAmorRefSelected_"+Mydata[i].QuoteNo).value;
                    //    var MachineAmor = document.getElementById("DdlMachineAmortizeRef_"+Mydata[i].QuoteNo).value;
                    //    var ReqPurpose = document.getElementById("DdlReqPurposeSelected_"+Mydata[i].QuoteNo).value;
                    //    var M_EstQty = document.getElementById("TxtMQtySelected_"+Mydata[i].QuoteNo).value;
                    //    var BaseUOM = document.getElementById("TxtUomSelected_"+Mydata[i].QuoteNo).value;
                    //    var DueDate = document.getElementById("TxtResDueDate_"+Mydata[i].QuoteNo).value;
                    //    var EffDate = document.getElementById("TxtEffDate_"+Mydata[i].QuoteNo).value;
                    //    var DueDateNextRev = document.getElementById("TxtDueDateNextRev_"+Mydata[i].QuoteNo).value;
                    //    var RecRatio = document.getElementById("DdlRecycleRatioSelected_"+Mydata[i].QuoteNo).value;
                    //    var Layout = Mydata[i].Layout;

                    //    if (cMatCost == false && cProcCost == false && cSubMatCost == false && cOthCost == false) {
                    //        ErrMsg += "Select at least one cost " + " Row No : " + (i+1) + "\r\n";

                    //        $("#chkMatRefSelected_" + Mydata[i].QuoteNo + "").css("outline-color", "red");
                    //        $("#chkMatRefSelected_" + Mydata[i].QuoteNo + "").css("outline-style", "solid");
                    //        $("#chkMatRefSelected_" + Mydata[i].QuoteNo + "").css("outline-width", "1px");

                    //        $("#chkProcRefSelected_" + Mydata[i].QuoteNo + "").css("outline-color", "red");
                    //        $("#chkProcRefSelected_" + Mydata[i].QuoteNo + "").css("outline-style", "solid");
                    //        $("#chkProcRefSelected_" + Mydata[i].QuoteNo + "").css("outline-width", "1px");

                    //        $("#chkSubMatRefSelected_" + Mydata[i].QuoteNo + "").css("outline-color", "red");
                    //        $("#chkSubMatRefSelected_" + Mydata[i].QuoteNo + "").css("outline-style", "solid");
                    //        $("#chkSubMatRefSelected_" + Mydata[i].QuoteNo + "").css("outline-width", "1px");

                    //        $("#chkOthRefSelected_" + Mydata[i].QuoteNo + "").css("outline-color", "red");
                    //        $("#chkOthRefSelected_" + Mydata[i].QuoteNo + "").css("outline-style", "solid");
                    //        $("#chkOthRefSelected_" + Mydata[i].QuoteNo + "").css("outline-width", "1px");
                    //    }

                    //    if (ToolAmor == "ADD") {
                    //        var QuoteNo = Mydata[i].QuoteNo;
                    //        var hasDataTool = $("#tableAmortize_" + Mydata[i].QuoteNo).DataTable().data().length;
                    //        var selectDataTool = $('input[name="rb_' + QuoteNo + '"]:checked').length;

                    //        if (hasDataTool == 0) {
                    //            ErrMsg += "Tool Amortize Not Exists " + " Row No : " + (i + 1) + "\r\n";
                    //        }
                    //        if (hasDataTool > 0 && selectDataTool == 0) {
                    //            ErrMsg += "Select Tool Amortize " + " Row No : " + (i + 1) + "\r\n";
                    //        }
                    //    }

                    //    if(ToolAmor == "0"){
                    //        ErrMsg += "Select Action for Tool Amortize " + " Row No : " + (i+1) + "\r\n";
                    //        document.getElementById("DdlToolAmorRefSelected_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //    }

                    //    if(MachineAmor == "0"){
                    //        ErrMsg += "Select Action for Machine Amortize" + " Row No : " + (i+1) + "\r\n";
                    //        document.getElementById("DdlMachineAmortizeRef_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //    }

                    //    if(ReqPurpose == "0"){
                    //        document.getElementById("DdlReqPurposeSelected_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //        ErrMsg += "Select Request Purpose" + " Row No : " + (i+1) + "\r\n";
                    //    }

                    //    if(M_EstQty.trim() == ""){
                    //        document.getElementById("TxtMQtySelected_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //        ErrMsg += "Enter Mnth. Est. Qty" + " Row No : " + (i+1) + "\r\n";
                    //    }

                    //    if(BaseUOM.trim() == ""){
                    //        document.getElementById("TxtUomSelected_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //        ErrMsg += "Enter Base UOM" + " Row No : " + (i+1) + "\r\n";
                    //    }

                    //    if(DueDate.trim() == ""){
                    //        document.getElementById("TxtResDueDate_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //        ErrMsg += "Enter Res.Due Date" + "Row No : " + (i+1) + "\r\n";
                    //    }

                    //    if(EffDate.trim() == ""){
                    //        document.getElementById("TxtEffDate_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //        ErrMsg += "Enter Effective Date" + "Row No : " + (i+1) + "\r\n";
                    //    }

                    //    if(DueDateNextRev.trim() == ""){
                    //        document.getElementById("TxtDueDateNextRev_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //        ErrMsg += "Enter Due Dt Next Rev" + "Row No : " + (i+1) + "\r\n";
                    //    }
                        
                    //    if (EffDate != "" && DueDateNextRev != "") {
                    //        var StrEffDate = EffDate.toString().replace(/\-/g, '.');
                    //        var strDueOn = DueDateNextRev.toString().replace(/\-/g, '.');

                    //        var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
                    //        var dtEffDate = new Date(StrEffDate.replace(pattern, '$3-$2-$1'));
                    //        var dtDueOn = new Date(strDueOn.replace(pattern, '$3-$2-$1'));

                    //        if (dtEffDate > dtDueOn) {
                    //            ErrMsg += "Due Dt Next Rev cannot be small than Effective date  " + "Row No : " + (i+1) + "\r\n";

                    //            document.getElementById("TxtDueDateNextRev_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //            document.getElementById("TxtEffDate_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //        }
                    //    }

                    //    if(RecRatio == "" && Layout == "LAYOUT1"){
                    //        ErrMsg += "Select Recycle Ratio (%)	" + "Row No : " + (i+1) + "\r\n";
                    //        document.getElementById("DdlRecycleRatioSelected_" + Mydata[i].QuoteNo + "").style.border = "1px solid #ff0000";
                    //    }
                    //}
                    
                    if(ErrMsg.length > 0){
                        alert(ErrMsg);
                    }
                    else
                    {
                        ok = true;
                    }
                }

                return ok;
            } catch (e) {
                alert("ValidateCreatereq : " + e);
                return ok;
            }
        }

        function ValidateSubmit() {
            try {
                var Mytable = $('#TbCreateReqTemp').DataTable();
                var Mydata = Mytable.rows().data();
                if(Mydata.length <= 0){
                    alert('No Data To Submit, Please Create Request !');
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (err) {
                alert("ValidateSubmit : " + err);
                return false;
            }
        }
    </script>

    <%--extra script--%>
    <script type="text/javascript">
        function ResetForm(){
            try {
                dataTableTbCreateReqTemp.clear().draw();
                dataTableTbQuoteRefListSelected.clear().draw();
                dataTableTbQuoteRefListInvalid.clear().draw();
            } catch (e) {
                alert(e + ' : ResetForm');
            }
        }

        function OpenModalQuoteRef() {
            try {
                jQuery.noConflict();
                $("#ModalQuoteRef").modal({
                    backdrop: 'static',
                    keyboard: false
                });
                dataTableTbQuoteRefList.clear().draw();
            }
            catch (err) {
                alert(err + ' : OpenModalQuoteRef');
            }
        }

        function CloseModalQuoteRef() {
            try {
                jQuery.noConflict();
                $("#ModalQuoteRef").modal('hide');
            }
            catch (err) {
                alert(err + ' : CloseModalQuoteRef');
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

        function SidebarMenu() {
            var SideBarMenu = document.getElementById("SideBarMenu");
            if (SideBarMenu.style.display === "none") {
                SideBarMenu.style.display = "block";
            } else {
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

        function openInNewTab(url, id) {
            var QuNoRef = document.getElementById(id).innerHTML;
            var fullUrl = url + "?Number=" + QuNoRef.replace(": ", "");
            var win = window.open(fullUrl, '_blank');
            win.focus();
        }

        function freezeheaderMdQuoteDet() {
            try {
                (function ($) {
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

        function openInNewTab2(url) {
            var win = window.open(url, '_blank');
            win.focus();
        }

        //not ready yet
        function GetVndToolSelected() {
            try {
                DataListVndAmor = [];
                var Maintable = document.getElementById("GvTemp");
                if (Maintable != null) {
                    var MaintableRowscount = Maintable.rows.length;
                    if (MaintableRowscount > 1) {
                        for (var i = 0; i < MaintableRowscount; i++) {
                            var ChkToolVnd = document.getElementById("GvTemp_chkToolAmortize_" + Mydata[i].QuoteNo + "");
                            if (ChkToolVnd != null) {
                                if (ChkToolVnd.checked == true) {
                                    var table = document.getElementById("GvTemp_GvDetToolAmor_" + Mydata[i].QuoteNo + "");
                                    if (table != null) {
                                        var rowscount = table.rows.length;
                                        if (rowscount > 1) {
                                            for (var v = 0; v < rowscount; v++) {
                                                var chk = document.getElementById("GvTemp_GvDetToolAmor_" + Mydata[i].QuoteNo + "_chkVndToolAmor_" + v + "");
                                                if (chk != null) {
                                                    if (chk.checked == true) {
                                                        if (document.getElementById("GvTemp_LbQNModal_" + Mydata[i].QuoteNo + "") != null && table.rows[(v + 1)] != null) {
                                                            DataListVndAmor.push({
                                                                "QuoteNoRef": document.getElementById("GvTemp_LbQNModal_" + Mydata[i].QuoteNo + "").innerHTML,
                                                                "ToolID": table.rows[(v + 1)].cells[2].innerHTML
                                                            })
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                document.getElementById("TxtDataListVndAmor").value = JSON.stringify(DataListVndAmor);
            } catch (e) {
                alert("GetVndToolSelected : " + e);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" onsubmit="return false"  >
    <asp:ScriptManager ID="scriptmanager1" runat="server" AsyncPostBackTimeout="36000"></asp:ScriptManager>
        <div class="col-md-12" id="DvMsgErr" runat="server" visible="false">
            <asp:Label runat="server" ID="LbMsgErr" Font-Bold="true" Visible="true"></asp:Label>
        </div>
        <div class="row">
            <div id="loading" class="col-md-12" style="padding-top:200px;" >
                <img id="loading-image" src="images/loading.gif" alt="Loading..."/>
                <div class="col-md-12" style="text-align:center; opacity:1;">
                    <asp:Label ID="lbLoading" runat="server" Text="please Wait..." Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                </div>
            </div>
        </div>
        <!-- Header -->
        <asp:UpdatePanel runat="server" ID="UpsidebarToggle">
        <ContentTemplate>
        <div class="container-fluid">
            <div class="col-md-12" style="padding:5px;">
            <div class="row">
                <div class="col-md-10" style="padding-top:5px;">
                    <a onclick="ShowLoading();" href="Home.aspx"><asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                        <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" OnClick="sidebarToggle_Click"><i class="fas fa-bars"></i> 
                        </asp:LinkButton>
                    <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                    <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-2 fa-pull-right" style="background-color:#E9ECEF;">
                    <asp:Label ID="lbluser1"  runat="server" Width="147px"></asp:Label><br />
                    <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
                    <asp:LinkButton runat="server"  ID="BtnLogOut" OnClientClick="LogOut()" Text="Logout"></asp:LinkButton>
                </div>
            </div>
            </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>

        <div id="wrapper">
            <!-- Sidebar -->
            <div id="SideBarMenu" style="width:300px;" runat="server" class="SideBarMenu">
          <ul class="sidebar">
            <li class="sideMenu">
              <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=1">
                <i class="fas fa-fw fa-tachometer-alt"></i>
                <span >Home</span>
              </a>
            </li>
         

            <li class="sideMenu">
              <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=2" >
                <i class="fas fa-fw fa-newspaper"></i>
                    <span >Create Request</span></a>
            </li>

             <li class="sideMenu">
              <a class="linkMenu" onclick="ShowLoading();" href="Revision.aspx">
                <i class="fas fa-fw fa-table" ></i>
               <span > Revision of MET</span></a>
			
            </li>

            <li class="sideMenu">
              <a class="linkMenu" onclick="ShowLoading();" href="MassRevision.aspx">
                <i class="fas fa-fw fa-chart-area"></i>
                <span >Mass Revision</span></a>
            </li>
      
		     <li class="sideMenu">
              <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=16">
                <i class="fas fa-fw fa-table"></i>
                <span >PIR Generation</span></a>

            </li>

          	    <li class="sideMenu">
              <a class="linkMenu" onclick="ShowLoading();" href="PIRGenMassRev.aspx">
                <i class="fas fa-fw fa-table"></i>
                <span >PIR Generation Mass Revision</span></a>

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
                    <span> About</span></a>

                </li>
          </ul>
          </div>

            <div id="content-wrapper" style="background-color:white;">
                <div class="container-fluid">
                    <div class="row" style="padding-bottom:10px;">
                        <div class="col-md-12">
                            <div class="col-md-12 card" style="padding:10px;background-color:white;">
                                <div class="col-md-12 card-body Padding-Nol">
                                    <div class="col-md-12" style="background-color:white;">
                                    
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                        <div class="row" style="padding-top:5px; padding-bottom:5px;">
                                            <div class="col-md-6 nopadding">
                                                <asp:Label ID="lbTitle" runat="server" Text="Revision of MET" Font-Bold="true" Font-Size="Large"/>
                                            </div>
                                            <div class="col-md-6 text-right nopadding">
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-sm btn-warning" OnClientClick="ResetForm();" />
                                                <%--<asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger" OnClientClick="Close" PostBackUrl="Home.aspx" />--%>
                                                <a href="Home.aspx" class="btn btn-sm btn-danger" > Close </a>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 nopadding">
                                                <div class="col-md-12" style="border-bottom:2px solid #006EB7"></div>
                                            </div>
                                        </div>
                                        
                                        <div class="row" style="padding-top:5px; ">
                                            <div class="col-md-6 ">
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label25" runat="server" Text="Date"></asp:Label>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="txtReqDate" runat="server" AutoCompleteType="Disabled" autocomplete="off" Enabled="false" ></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 ">
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label2" runat="server" Text="Plant"></asp:Label>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <asp:TextBox runat="server" ID="TxtPlant" Text="" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" style="padding-top:5px; ">
                                            <div class="col-md-6 ">
                                                <div class="row">
                                                    <div class="col-md-5" style="padding-top:5px;">
                                                        <asp:Label ID="Label13" runat="server" Text="Vendor Type"  ForeColor="Black"></asp:Label>
                                                    </div>
                                                    <div class="col-md-7" style="padding-top:5px;" runat="server" id="DvVendorType">
                                                        <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:RadioButton ID="RbExternal" runat="server" Text="&nbsp; External" Checked="true"
                                                                onclick="if(ConfirmChange()==false) return false;"
                                                                GroupName="RbVendorType" TextAlign="Right" AutoPostBack="false" />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:RadioButton ID="RbTeamShimano" runat="server" Text="&nbsp; SBM" 
                                                                onclick="if(ConfirmChange()==false) return false;"
                                                                GroupName="RbVendorType" TextAlign="Right" AutoPostBack="false" />
                                                        </div>
                                                    </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6" >
                                                <div class="row">
                                                    <div class="col-md-5" style="padding-top:5px;">
                                                    </div>
                                                    <div class="col-md-7" style="padding-top:5px;">
                                                        <%--OnClientClick="if(CheckVendorType()==false) return false;if(ConfirmAddMore()==false) return false;OpenModalQuoteRef();HiglightGrid();" --%>
                                                        <%--<asp:Button ID="BtnFindQuoteRef" runat="server" Text="Find Quote Reference" Width="100%"
                                                            CssClass="Login-button"
                                                            OnClientClick="if(CheckVendorType()==false) return false;OpenModalQuoteRef();"
                                                            autopostback="false"
                                                             />--%>
                                                        <a id="BtnFindQuoteRef" class="btn btn-sm btn-primary btn-block" onclick="if(CheckVendorType()==false) return false;if(ConfirmAddMore()==false) return false;OpenModalQuoteRef();" >Find Quote Reference</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <asp:UpdatePanel ID="UpInvalidRequest" runat="server">
                                        <ContentTemplate>
                                        <div class="row" style="padding-bottom:10px; padding-top:10px;" runat="server" id="DvInvalidRequestOld" visible="false">
                                            <div class="col-md-12">
                                                <div class="col-md-12" style="background:#fa0606">
                                                    <asp:Label ID="Label7" runat="server" Text="Below vendor with material is in progress"
                                                     Visible="true" ForeColor="White" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="table table-responsive table-sm">
                                                    <asp:GridView ID="GvInvalidRequest" runat="server" AutoGenerateColumns="False" 
                                                        AllowPaging="false" PageSize="10" OnRowDataBound="GvInvalidRequest_RowDataBound"
                                                        CssClass="table-sm table-bordered table-nowrap WrapCnt" Font-Bold="False" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date"  HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                            <asp:BoundField DataField="RequestNumber" HeaderText="Req. No" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                            <asp:BoundField DataField="QuoteResponseDueDate" HeaderText="Res Due Date"  HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                            <asp:BoundField DataField="QuoteNo" HeaderText="QuoteNo"   HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                            <asp:BoundField DataField="Material" HeaderText="Material" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc"  HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                            <asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                        <PagerSettings PageButtonCount="10"  />
                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" BorderColor="White"/>
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
                                        
                                        <div class="row"  style="padding:5px 0px 5px 0px;" >
                                            <div class="col-md-12">
                                                
                                            </div>
                                        </div>

                                        <div class="row" >
                                            <div class="col-md-12" style="padding-bottom:0px;">
                                            <div class="col-lg-12 table-sm table-responsive" style="padding:0px;" >
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GvTemp" runat="server" ShowHeaderWhenEmpty="false" Width="100%" BackColor="White"
                                                            AutoGenerateColumns="False"  OnRowDataBound="GvTemp_RowDataBound" OnRowCommand="GvTemp_RowCommand"
                                                            CssClass="table table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt" >
                                                            <HeaderStyle  HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="No.">
                                                                    <ItemTemplate> 
                                                                        <%#(Container.DataItemIndex+1)%> 
                                                                        <asp:Image ID="Image1" runat="server" style="cursor:pointer" ImageUrl="~/images/plus1.png" Width="0px" Height="0px"/>
                                                                    </ItemTemplate><ItemStyle Width="10px" />
                                                                </asp:TemplateField>
                                                                <%--<asp:BoundField DataField="QuoteNo" HeaderText="Quote No Ref" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                                <asp:TemplateField HeaderText="Old Quote No" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LbQNModal" runat="server" Text='<%# Eval("QuoteNo") %>' ForeColor="Blue" />
                                                                        <div style="display:none">
                                                                            <asp:LinkButton ID="LbOldQNTemp" runat="server" Text='<%# Eval("QuoteNo") %>'
                                                                            CommandName="QuoteDetails" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ForeColor="Blue"  />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Material" HeaderText="Material" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="ProcessGroup" HeaderText="Process Group" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="PrcGrpDesc" HeaderText="Prc Grp Desc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="All Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkAllCost" runat="server" autopostback="true" onchange="ShowLoading();"  />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mat Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkMatCost" runat="server"  />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Proc Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkProcCost" runat="server" autopostback="true" onchange="ShowLoading();" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sub Mat Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSubMatCost" runat="server" autopostback="true" onchange="ShowLoading();" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Others Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkOthCost" runat="server"   />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tool Amortize" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <div style="display:none;">
                                                                        <asp:CheckBox ID="chkToolAmortize" runat="server" autopostback="true" onchange="ShowLoading();"/>
                                                                        </div>
                                                                        <asp:DropDownList ID="DdlToolAmortize" runat="server" EnableViewState="true" autopostback="true"   >
                                                                            <asp:ListItem Value="ADD"> ADD </asp:ListItem>
                                                                            <asp:ListItem Value="REMOVE" Selected="True"> REMOVE </asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Machine Amortize" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <div style="display:none;">
                                                                            <asp:CheckBox ID="chkMachineAmortize" runat="server" />
                                                                        </div>
                                                                        <asp:DropDownList ID="DdlMachineAmortize" runat="server" EnableViewState="true" >
                                                                            <asp:ListItem Value="ADD"> ADD </asp:ListItem>
                                                                            <asp:ListItem Value="REMOVE" Selected="True"> REMOVE </asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Request Purpose" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="DdlReason" runat="server"  Width="50px" /><br />
                                                                        <div style="padding-top:3px;">
                                                                            <asp:TextBox ID="TxtOthReason" runat="server" TextMode="MultiLine" Text=""
                                                                            AutoCompleteType="Disabled" autocomplete="off" Width="100%" ToolTip="maximal 200 character" />
                                                                            <asp:Label ID="LblengtVC" runat="server" Text="200 Character left" Font-Size="12px" CssClass="fa pull-right" Font-Bold="false" Font-Names="calibri"></asp:Label>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mnth. Est. Qty" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                    <asp:TextBox ID="TxtMQty" runat="server" Text="" onkeydown="return (event.keyCode!=13);" 
                                                                        AutoCompleteType="Disabled" autocomplete="off" Width="50px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Base UOM" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="TxtBaseUOM" runat="server" MaxLength="10" ToolTip="max 10 character"
                                                                            ReadOnly="true" Text='<%# Eval("BaseUOM") %>'
                                                                            AutoCompleteType="Disabled" autocomplete="off" onkeydown="return (event.keyCode!=13);" Width="50px"/>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Res.Due Date" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                    <div class="group-main">
                                                                        <div style="padding:0px;">
                                                                            <asp:TextBox ID="TxtResDueDate" OnclientClick="return false;"
                                                                                onkeydown="javascript:preventInput(event);" 
                                                                                autocomplete="off" AutoCompleteType="Disabled" Width="60px"
                                                                                runat="server"  ForeColor="Black">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                        <span class="SearchBox-btn-cal" style="background-color:#E9ECEF; padding:1px 3px 0px 1px;">
                                                                            <span class="fa fa-calendar" runat="server" id="IcnCalResduedate" style="color:#005496;"></span>
                                                                        </span>
                                                                    </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Effective Date" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                    <div class="group-main">
                                                                        <div style="padding:0px;">
                                                                            <asp:TextBox ID="TxtEffectiveDate" autopostback="true"
                                                                                onkeydown="javascript:preventInput(event);" 
                                                                                autocomplete="off" AutoCompleteType="Disabled" Width="60px" onchange="ShowLoading();"
                                                                                runat="server"  ForeColor="Black">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                        <span class="SearchBox-btn-cal" style="background-color:#E9ECEF; padding:1px 3px 0px 1px;">
                                                                            <span class="fa fa-calendar" runat="server" id="IcnCalEffectiveDate" style="color:#005496;"></span>
                                                                        </span>
                                                                    </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Due Dt Next Rev" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                    <div class="group-main">
                                                                        <div style="padding:0px;">
                                                                            <asp:TextBox ID="TxtDueDateNextRev" OnclientClick="return false;"
                                                                                onkeydown="javascript:preventInput(event);" 
                                                                                autocomplete="off" AutoCompleteType="Disabled" Width="60px"
                                                                                runat="server"  ForeColor="Black">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                        <span class="SearchBox-btn-cal" style="background-color:#E9ECEF; padding:1px 3px 0px 1px;">
                                                                            <span class="fa fa-calendar" runat="server" id="IcnDueDateNextRev" style="color:#005496;"></span>
                                                                        </span>
                                                                    </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Recycle Ratio (%)" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="DdlRecycleRatio" runat="server"  Width="50px" /><br />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Attch" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" ID="LbAttach" AutoPostBack="false" CssClass="lbattachpad text-center" Width="100%"
                                                                            OnClientClick="ClcBtnFlUpload();" CommandName="CUploadFile" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                            <i class="glyphicon glyphicon-paperclip" style="padding:0px;"></i>
                                                                        </asp:LinkButton>
                                                                        <asp:Label runat="server" ID="lbFileName" Text="No File"></asp:Label>
                                                                        <asp:LinkButton runat="server" ID="LbPreview" AutoPostBack="false" CommandName="CPreviewFile" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                CssClass="lbattachpad text-center" Font-Size="14px"  Width="100%"> 
                                                                                <i class="glyphicon glyphicon-download" style="padding:0px; color:#34eb08"></i> 
                                                                        </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                <%--Gridview Level GvDetToolAmor--%>
                                                                <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Tool Amortize List" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:Panel ID="pnlDetToolAmortize" runat="server" Style="display: block">
                                                                            <asp:GridView ID="GvDetToolAmor" runat="server" AutoGenerateColumns="false" BackColor="White" EmptyDataText="No records Found" ShowHeaderWhenEmpty="false"
                                                                                OnRowDataBound="GvDetToolAmor_RowDataBound" Width="100%"
                                                                                CssClass="table-hover Padding-Nol" DataKeyNames="Amortize_Tool_ID">
                                                                                    <Columns>
                                                                                    <asp:TemplateField HeaderStyle-BackColor="#FBDCA3" HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkVndToolAmor" runat="server" AutoPostBack="false" ForeColor="Transparent" Width="0px"  />
                                                                                            <div style="display:none">
                                                                                                <asp:Label ID="LbTotRecord" runat="server" Text='<%# Eval("TotRecord") %>' ForeColor="Blue" />
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="20px" />
                                                                                    </asp:TemplateField>

                                                                                    <%--   <asp:BoundField DataField="VendorCode" HeaderText="Vendor ID" />
                                                                                    <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                                                                                    <asp:BoundField DataField="SearchTerm" HeaderText="Search Term" />--%>

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
                                                                                <HeaderStyle BackColor="#4d94ff" Font-Bold="True" ForeColor="White" />
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Del" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="BtnDelList" runat="server" CssClass="lbattachpad text-center" 
                                                                            CommandName="DeleteRec" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                            <span class="glyphicon glyphicon-trash" style="color:#d70707;padding:0px;">
                                                                        </asp:LinkButton>
                                                                        <asp:Label runat="server" ID="LbRowParentGv" Text="<%# ((GridViewRow) Container).RowIndex %>" Width="0px" Height="0px" ForeColor="Transparent" Visible="false"></asp:Label>
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

                                        
                                        <%--TbQuoteRefListInvalid--%>
                                        <div class="row" id="DvInvalidRequest" style="padding-bottom:10px;display:none">
                                            <div class="col-md-12">
                                            <div class="col-md-12" style="background-color:#d93702; margin-bottom:5px;">
                                                <asp:Label ID="Label8" runat="server" Text="Below vendor with material is in progress"
                                                     Visible="true" ForeColor="White" Font-Bold="true"></asp:Label>
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

                                        <%--TbQuoteRefListSelected--%>
                                        <div class="row" id="DivQuoteSelected">
                                            <div class="col-md-12">
                                            <div class="col-md-12" style="background-color:#02850d; margin-bottom:5px;">
                                                <div class="row" style="padding-left:10px;">
                                                    <label id="LbTitleQrefList" style="color:white;font-weight:bolder;">Selected Vendor</label>
                                                    <label class="pull-right text-right" style="color:yellow;font:lighter 14px calibri light"> Please maintain MDM data to allow Vendor to add in Machine Amortization details in quotation  &nbsp;&nbsp;</label>
                                                </div>
                                            </div>
                                            </div>
                                            <div class="col-md-12">
                                                <table id="TbQuoteRefListSelected" class="table table-striped table-bordered nopadding">
                                                    <thead>
                                                        <tr>
                                                            <th>No</th>
                                                            <th>Old Quote No</th>
                                                            <th>Vendor Code</th>
                                                            <th>Vendor Name</th>
                                                            <th>Material</th>
                                                            <th>Material Desc</th>
                                                            <th>Process Group</th>
                                                            <th>Prc Grp Desc</th>
                                                            <th>MaterialType</th>
                                                            <th>MaterialClass</th>
                                                            <th>UOM</th>
                                                            <th>PlantStatus</th>
                                                            <th>SAPProcType</th>
                                                            <th>SAPSpProcType</th>
                                                            <th>Product</th>
                                                            <th>PIRType</th>
                                                            <th>PIRJobType</th>
                                                            <th>NetUnit</th>
                                                            <th>All Cost</th>
                                                            <th>Mat Cost</th>
                                                            <th>Proc Cost</th>
                                                            <th>Sub Mat Cost</th>
                                                            <th>Others Cost</th>
                                                            <th>Tool Amortize</th>
                                                            <th>Machine Amortize</th>
                                                            <th>ToolAmorExist</th>
                                                            <th>ToolAmorExpired</th>
                                                            <th>MacAmorExist</th>
                                                            <th>MacAmorExpired</th>
                                                            <th>Request Purpose</th>
                                                            <th>Mnth. Est. Qty</th>
                                                            <th>Base UOM</th>
                                                            <th style="min-width:62px;">Res.Due Date</th>
                                                            <th style="min-width:62px;">Effective Date</th>
                                                            <th style="min-width:62px;">Due Dt Next Rev</th>
                                                            <th>Recycle Ratio (%)</th>
                                                            <th style="max-width:100px;">Attch</th>
                                                            <th>Tool Amortize List</th>
                                                            <th>Del</th>
                                                        </tr>
                                                    </thead>
                                                </table>
                                            </div>
                                        </div>

                                        <div class="row" style="padding-bottom:5px; padding-top:5px;">
                                            <div class="col-md-6"></div>
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <div style="display:none;">
                                                        <asp:Button runat="server" ID="BtnResetQuoteList" Text="Reset List" CssClass="Login-button" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-7" >
                                                        <%--<asp:Button runat="server" ID="BtnCreateReq" Text="Create Request" OnClick="BtnCreateReq_Click"
                                                        OnClientClick="if(valCreateReq()==false) return false;ShowLoading();"
                                                            CssClass="Login-button"  Width="100%" Visible="true"/>--%>
                                                        <asp:Button runat="server" ID="BtnCreateReq" Text="Create Request" autopostback="false" OnClick="BtnCreateReq_Click"
                                                        OnClientClick="createRequestTemp()" CssClass="Login-button"  Width="100%" Visible="false"/>
                                                        <button id="BtnCreateReq1" class="btn btn-sm btn-success btn-block" onclick="if(ValidateCreatereq()==false)return false;createRequestTemp()" >Create Request</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" style="padding-bottom:10px">
                                            <div class="col-md-12">
                                                <div class="table table-responsive table-sm table-nowrap">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GvCreateReqTemp" runat="server" AutoGenerateColumns="False" 
                                                            AllowPaging="false" PageSize="10" OnRowDataBound="GvCreateReqTemp_RowDataBound"
                                                            CssClass="table-sm table-bordered table-nowrap WrapCnt" Font-Bold="False" Width="100%">
                                                            <Columns>
                                                                <asp:BoundField DataField="Quote No Ref" HeaderText="Quote No Ref"  HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <%--<asp:BoundField DataField="Req Date" HeaderText="Req Date" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                                <asp:BoundField DataField="Req No" HeaderText="Req No"  HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Plant" HeaderText="Plant"   HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                <asp:BoundField DataField="Comp Material" HeaderText="Comp Material" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Comp Material Desc" HeaderText="Comp Material Desc"  HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Vendor Name" HeaderText="Vendor Name" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Vendor Code" HeaderText="Vendor Code" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="SearchTerm" HeaderText="SearchTerm" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Quote No New" HeaderText="Quote No New" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Ven PIC" HeaderText="Ven PIC" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="PIC Email" HeaderText="PIC Email" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Selling Crcy" HeaderText="Selling Crcy" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Amt SCur" HeaderText="Amt SCur" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Exch Rate" HeaderText="Exch Rate" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Vendor Crcy" HeaderText="Venor Crcy" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Amt VCur" HeaderText="Amt VCur" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Unit" HeaderText="Unit" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="ValidFrom" HeaderText="ValidFrom" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="CusMatValFrom" HeaderText="CusMatValFrom" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="CusMatValTo" HeaderText="CusMatValTo" HeaderStyle-CssClass="text-center"></asp:BoundField>

                                                            </Columns>
                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                            <PagerSettings PageButtonCount="10"  />
                                                            <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" BorderColor="White"/>
                                                            <RowStyle ForeColor="Black" />
                                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="White" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>

                                                
                                            </div>
                                        </div>

                                        <%--TbCreateReqTemp--%>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="col-md-12" style="background-color:#02640a; margin-bottom:5px;">
                                                <asp:Label ID="Label9" runat="server" Text="Quote Request can be Created only for below Vendors" Font-Bold="true" ForeColor="White" Visible="true"/>
                                            </div>
                                            </div>
                                            <div class="col-md-12">
                                                <table id="TbCreateReqTemp" class="table table-striped table-bordered nopadding">
                                                    <thead>
                                                        <tr>
                                                            <th>Quote No Ref</th>
                                                            <th>Req No</th>
                                                            <th>Plant</th>
                                                            <th>Comp Material</th>
                                                            <th>Comp Material Desc</th>
                                                            <th>Vendor Name</th>
                                                            <th>Vendor Code</th>
                                                            <th>SearchTerm</th>
                                                            <th>Quote No New</th>
                                                            <th>Ven PIC</th>
                                                            <th>PIC Email</th>
                                                            <th>Selling Crcy</th>
                                                            <th>Amt SCur</th>
                                                            <th>Exch Rate</th>
                                                            <th>Venor Crcy</th>
                                                            <th>Amt VCur</th>
                                                            <th>Unit</th>
                                                            <th>UOM</th>
                                                            <th>ValidFrom</th>
                                                            <th>CusMatValFrom</th>
                                                            <th>CusMatValTo</th>
                                                        </tr>
                                                    </thead>
                                                </table>
                                            </div>
                                        </div>

                                        <div class="row" style="padding-bottom:10px;padding-top:5px;">
                                            <div class="col-md-6"></div>
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-md-5">
                                                        <a id="BtnCancelSubmit" class="btn btn-sm btn-danger btn-block" style="display:none" onclick="CancelSubmit()">Cancel</a>
                                                    </div>
                                                    <div class="col-md-7" >
                                                        <a class="btn btn-sm btn-success btn-block" onclick="if(ValidateSubmit()==false)return false; if(ProceedSubmitRequestRevisionEmet() == false) return false;">Submit</a>
                                                        <asp:HiddenField ID="hdnReqNo" runat="server" />
                                                        <asp:Button ID="BtnSubmitRequest" runat="server" CssClass="Login-button" OnClientClick="return ValidateSubmit();ShowLoading();"
                                                            Text="Submit" Visible="false" Width="100%"  OnClick="BtnSubmitRequest_Click" />
                                                        <asp:Label ID="Label18" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="Label21" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="Label24" runat="server" Visible="False"></asp:Label>
                                                    </div>
                                                </div>
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


            <div class="row" style="display:none">
                <div class="col-md-12" style="padding-bottom:0px;">
                    <div class="col-lg-12 table-sm table-responsive" style="padding:0px; ">
                        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="TxtGvTempLeng" runat="server"></asp:TextBox>

                                <asp:TextBox ID="TxtGvtempRowFoc" runat="server"></asp:TextBox>
                                <asp:TextBox ID="TxtMatGvTempFoc" runat="server"></asp:TextBox>
                                <asp:TextBox ID="TxtProcGrpGvTempFoc" runat="server"></asp:TextBox>
                                <asp:TextBox ID="TxtEffGvTempFoc" runat="server"></asp:TextBox>
                                <asp:CheckBox ID="ChkToolAmo"  runat="server" Checked="false"></asp:CheckBox>
                                <asp:TextBox ID="TxtDataListVndAmor" runat="server"></asp:TextBox>

                                <asp:TextBox ID="TxtDefFocus" runat="server"></asp:TextBox>
                                <asp:Button ID="BtnGetVndTool" runat="server" autopostback="true" OnClick="BtnGetVndTool_OnClick" Text="Get Vnd Tool"></asp:Button>

                                <asp:GridView ID="GvlayoutList" runat="server" ShowHeaderWhenEmpty="false" Width="100%" BackColor="White"
                                    AutoGenerateColumns="False"  OnRowDataBound="GvlayoutList_RowDataBound"
                                    CssClass="table table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt" >
                                    <HeaderStyle  HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:BoundField DataField="ProcessGrp" HeaderText="ProcessGrp" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                            <asp:BoundField DataField="ScreenLayout" HeaderText="ScreenLayout" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
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

            <div id="DvHdnField" runat="server" style="display:none" >
                <asp:TextBox runat="server" ID="TxtExtraUrl" value=""></asp:TextBox>
            </div>
        </div>

    <!-- Start control support-->
    <div class="col-md-12" style="display:none;">
        <div class="col-md-1">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <asp:TextBox runat="server" ID="TxtRowGvTemp" Text=""></asp:TextBox>
                <asp:TextBox runat="server" ID="TxtIsLY7InTheList" Text=""></asp:TextBox>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-2">
            <asp:FileUpload ID="FlAttachment" runat="server" onchange="UploadFile()" />
        </div>
        <div class="col-md-1">
            <asp:Button runat="server" ID="BtnUpload" Text="upload" OnClientClick="if(validateBtnUpload()==false) return false;" OnClick="BtnUpload_Click" />
            <asp:Button runat="server" ID="BtnPreview" Text="Preview" OnClick="BtnPreview_Click" />
        </div>
    </div>
    <!--end control support-->

    <!-- Footer -->
    <div class="container-fluid" style="background-color:#F5F5F5">
        <div class="row">
            <div class="col-md-12" style="padding:5px; align-content:center; text-align:center">
                <span style="font:bold 13px calibri, calibri">Copyright © ShimanoDT 2018</span>
            </div>
        </div>
    </div>

    <a class="scroll-to-top rounded" href="#page-top"><i class="fas fa-angle-up"></i></a>

    <!-- Modal quote reference-->
    <div class="modal fade" id="ModalQuoteRef" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="myModalLabecl" aria-hidden="true" keyboard="false" >
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <div class="modal-dialog" style="width:95%; position: absolute; margin-top:0px; top:0px; margin-left:2%; ">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel14" UpdateMode="Conditional">
                        <ContentTemplate>
                        <div class="row"  style="background-color:#F7F7F7; padding-bottom:10px; padding-left:10px; padding-right:10px;
                                          box-shadow: 1px 1px 1000px 1px;
                                          border-top-left-radius:10px; border-top-right-radius:10px;
                                          border-bottom-left-radius:10px; border-bottom-right-radius:10px;">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-lg-12 text-center" style="padding-top:10px; padding-bottom:10px;">
                                        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="LbQoRefTitle" runat="server" Text="Find Quote Reference List" Font-Size="Large" 
                                                Font-Bold="true" ></asp:Label>
                                                <asp:Button runat="server" id="BtnCloseModal" Text="X" OnClientClick="CloseModalQuoteRef();" OnClick="BtnCancel_Click"
                                                    CssClass="btn btn-sm btn-danger fa-pull-right" Font-Names="calibri" Font-Size="10px"/>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                    <div class="col-lg-12" style="border-bottom:2px solid #006EB7"></div>
                                    </div>
                                </div>

                                <div class="row" style="padding-top:5px; ">
                                    <div class="col-md-5">
                                        <div class="row">
                                            <div class="col-md-5" style="padding-bottom:5px;">
                                                <asp:Label ID="Label48" runat="server" Text="Req. Type"></asp:Label>
                                            </div>
                                            <div class="col-md-7" style="padding-bottom:5px;">
                                                <%--<asp:DropDownList runat="server" ID="DdlReqType" AutoPostBack="false" onchange="ShowLoading();" OnSelectedIndexChanged="DdlReqType_SelectedIndexChanged" >
                                                    <asp:ListItem Value="ALL" Text="ALL"></asp:ListItem>
                                                    <asp:ListItem Value="New" Text="NEW"></asp:ListItem>
                                                    <asp:ListItem Value="Revision" Text="REVISION"></asp:ListItem>
                                                    <asp:ListItem Value="MassRev" Text="MASS REVISION"></asp:ListItem>
                                                </asp:DropDownList>--%>
                                                <asp:DropDownList runat="server" ID="DdlReqType" AutoPostBack="false" >
                                                    <asp:ListItem Value="0" Text="-- Select --"></asp:ListItem>
                                                    <asp:ListItem Value="New" Text="NEW"></asp:ListItem>
                                                    <asp:ListItem Value="Revision" Text="REVISION"></asp:ListItem>
                                                    <asp:ListItem Value="MassRev" Text="MASS REVISION"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-7">
                                        <div class="row">
                                            <div class="col-md-3" style="padding-bottom:5px;">
                                                <asp:CheckBox runat="server" AutoPostBack="false" Text=" &nbsp; Active Material Only" Checked="false" ID="chkActiveMaterial" />
                                                <%--<asp:Label ID="Label49" runat="server" Text="Material Class Desc."></asp:Label>--%>
                                            </div>
                                            <div class="col-md-9" style="padding-bottom:5px;">
                                                <%--<asp:DropDownList runat="server" ID="DropDownList2" AutoPostBack="true" OnSelectedIndexChanged="DdlMatClassDesc_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select Product Code" Value=""></asp:ListItem>
                                                </asp:DropDownList>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-top:5px; ">
                                    <div class="col-md-5">
                                        <div class="row">
                                            <div class="col-md-5" style="padding-bottom:5px;">
                                                <asp:Label ID="Label3" runat="server" Text="Product"></asp:Label>
                                            </div>
                                            <div class="col-md-7" style="padding-bottom:5px;">
                                                <%--<asp:DropDownList runat="server" ID="DdlProduct" AutoPostBack="true" onchange="ShowLoading();" OnSelectedIndexChanged="DdlProduct_SelectedIndexChanged" ></asp:DropDownList>--%>
                                                <asp:DropDownList runat="server" ID="DdlProduct" AutoPostBack="false" onchange="GetDdlMatClassDesc();" ></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-7">
                                        <div class="row">
                                            <div class="col-md-3" style="padding-bottom:5px;">
                                                <asp:Label ID="Label6" runat="server" Text="Material Class Desc."></asp:Label>
                                            </div>
                                            <div class="col-md-9" style="padding-bottom:5px;">
                                                <%--<asp:DropDownList runat="server" ID="DdlMatClassDesc" AutoPostBack="true" onchange="ShowLoading();" OnSelectedIndexChanged="DdlMatClassDesc_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select Product Code" Value=""></asp:ListItem>
                                                </asp:DropDownList>--%>
                                                <asp:DropDownList runat="server" ID="DdlMatClassDesc" AutoPostBack="false">
                                                    <asp:ListItem Text="Select Product Code" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-top:5px; ">
                                    <div class="col-md-5">
                                        <div class="row">
                                            <div class="col-md-5" style="padding-bottom:5px;">
                                                <asp:Label ID="Label1" runat="server" Text="Process Group"></asp:Label>
                                            </div>
                                            <div class="col-md-7" style="padding-bottom:5px;">
                                                <%--<asp:DropDownList runat="server" ID="DdlProcGroup" AutoPostBack="true" onchange="ShowLoading();" OnSelectedIndexChanged="DdlProcGroup_SelectedIndexChanged" ></asp:DropDownList>--%>
                                                <asp:DropDownList runat="server" ID="DdlProcGroup" AutoPostBack="false"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-7">
                                        <div class="row">
                                            <div class="col-md-3" style="padding-bottom:5px;">
                                                <asp:Label ID="Label4" runat="server" Text="Sub process"></asp:Label>
                                            </div>
                                            <div class="col-md-9" style="padding-bottom:5px;">
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <%--<asp:TextBox runat="server" ID="TxtSubProc" ClientMode="Static" AutoPostBack="false" Text="" OnTextChanged="TxtSubProc_TextChanged"></asp:TextBox></div>
                                                        <span class="SearchBox-btn" style="background-color:#E9ECEF; padding: 1px 0px 1px 13px; display:none;">
                                                        <asp:LinkButton ID="btnSearchSubProc" runat="server" autopostback="false" OnClientClick="ShowLoading();"  OnClick="btnSearchSubProc_Click"><i class="fa fa-search" aria-hidden="true" 
                                                                        style="color:#005496;" ></i></asp:LinkButton>--%>
                                                        <asp:TextBox runat="server" ID="TxtSubProc" ClientMode="Static" AutoPostBack="false" Text=""></asp:TextBox></div>
                                                        <span class="SearchBox-btn" style="background-color:#E9ECEF; padding: 1px 0px 1px 13px; display:none;">
                                                        <asp:LinkButton ID="btnSearchSubProc" runat="server" autopostback="false" OnClientClick="ShowLoading();" ><i class="fa fa-search" aria-hidden="true" 
                                                                        style="color:#005496;" ></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-top:5px; ">
                                    <div class="col-md-5">
                                        <div class="row">
                                            <div class="col-md-5" style="padding-bottom:5px;">
                                                <asp:Label ID="Label5" runat="server" Text="Find By"></asp:Label>
                                            </div>
                                            <div class="col-md-7" style="padding-bottom:5px;">
                                                <%--<asp:DropDownList runat="server" ID="DdlFilter" AutoPostBack="false" onchange="ShowLoading();" OnSelectedIndexChanged="DdlFilter_SelectedIndexChanged" >
                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                    <asp:ListItem Text="Vendor Code" Value="VendorCode"></asp:ListItem>
                                                    <asp:ListItem Text="Vendor Name" Value="VendorName"></asp:ListItem>
                                                    <asp:ListItem Text="Quote No" Value="QuoteNo"></asp:ListItem>
                                                    <asp:ListItem Text="Material (SAP Code)" Value="Material"></asp:ListItem>
                                                    <asp:ListItem Text="Material Desc" Value="MaterialDesc"></asp:ListItem>
                                                    <asp:ListItem Text="Department" Value="UseDep"></asp:ListItem>
                                                    <asp:ListItem Text="SMN PIC" Value="CreatedBy"></asp:ListItem>
                                                </asp:DropDownList>--%>

                                                <asp:DropDownList runat="server" ID="DdlFilter" AutoPostBack="false" >
                                                    <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Vendor Code" Value="VendorCode"></asp:ListItem>
                                                    <asp:ListItem Text="Vendor Name" Value="VendorName"></asp:ListItem>
                                                    <asp:ListItem Text="Quote No" Value="QuoteNo"></asp:ListItem>
                                                    <asp:ListItem Text="Material (SAP Code)" Value="Material"></asp:ListItem>
                                                    <asp:ListItem Text="Material Desc" Value="MaterialDesc"></asp:ListItem>
                                                    <asp:ListItem Text="Department" Value="UseDep"></asp:ListItem>
                                                    <asp:ListItem Text="SMN PIC" Value="CreatedBy"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-7">
                                        <div class="group-main">
                                            <div class="SearchBox-txt"><asp:TextBox runat="server" ID="TxtFilter" Text="" OnTextChanged="TxtFilter_TextChanged" AutoPostBack="false"></asp:TextBox></div>
                                                <span class="SearchBox-btn" style="background-color:#E9ECEF; padding: 1px 0px 1px 13px;"">
                                                <%--<asp:LinkButton ID="btnSearch" runat="server" autopostback="false" OnClientClick="GetQuoteList();" ><i class="fa fa-search" aria-hidden="true" 
                                                        style="color:#005496;" ></i></asp:LinkButton>--%>
                                                <a id="btnSearch" onclick="GetQuoteList();" ><i class="fa fa-search" aria-hidden="true" 
                                                        style="color:#005496;" ></i> <b> Search </b> </a>
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                    <div class="col-lg-12" style="border-bottom:2px solid #006EB7"></div>
                                    </div>
                                </div>

                                <%--<div class="row" runat="server" id="DvTitleNoData" visible="false">
                                    <div class="col-md-12">
                                        <div class="col-lg-12 text-center" style="padding-top:10px;">
                                            <asp:Label ID="LbTitleNoData" runat="server" Text="No Data Found"
                                                Font-Size="18px" ForeColor="DarkRed" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                </div>--%>

                                <div class="row">
                                    <div class="col-md-12">
                                    <div class="col-lg-12 table table-responsive" style="padding:0px; max-height:400px;">
                                        <asp:UpdatePanel ID="UpdatePanel18"  runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional"  RenderMode="Block">
                                            <ContentTemplate>
                                                <asp:GridView ID="GvQuoteRefList" runat="server" ShowHeaderWhenEmpty="false"
                                                    AutoGenerateColumns="False"  OnRowDataBound="GvQuoteRefList_RowDataBound" OnRowCommand="GvQuoteRefList_RowCommand"
                                                    CssClass="table-responsive  table-sm table-bordered table-nowrap Padding-Nol WrapCnt" >
                                                    <HeaderStyle  HorizontalAlign="Center" />
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="QuoteNo" HeaderText="Quote No" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="Quote No" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center" HeaderStyle-Width="7%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LbQNModal" runat="server" Text='<%# Eval("QuoteNo") %>' ForeColor="Blue" Width="0px" Height="0px" />
                                                                <div style="display:none;">
                                                                    <asp:LinkButton ID="LbMdQuoteDetails" runat="server" CssClass="btn-link" Text='<%# Eval("QuoteNo") %>' 
                                                                    CommandName="QuoteDetails" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                </asp:LinkButton>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:BoundField DataField="Product" HeaderText="Product" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:BoundField DataField="MaterialClass" HeaderText="Material Class" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:BoundField DataField="Material" HeaderText="Material" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:BoundField DataField="ProcessGroup" HeaderText="Process Group" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:BoundField DataField="PrcGrpDesc" HeaderText="Prc Grp Desc" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#009933" HeaderText="All Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAllRefHd" runat="server" Text="All Cost"/>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAllRefRw" runat="server" EnableViewState="true"  />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#009933" HeaderText="Mat Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAllMatRef" runat="server" Text="Mat Cost" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkMatRef" runat="server" Text='<%# Eval("TotalMaterialCost") %>' EnableViewState="true"  />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#009933" HeaderText="Proc Cost"  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAllProcRef" runat="server" Text="Proc Cost" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkProcRef" runat="server" Text='<%# Eval("TotalProcessCost") %>' EnableViewState="true"  />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#009933" HeaderText="Sub Mat Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAllSubMatRef" runat="server" Text="Sub Mat Cost" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSubMatRef" runat="server" Text='<%# Eval("TotalSubMaterialCost") %>' EnableViewState="true"  />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#009933" HeaderText="Others Cost" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAllOthRef" runat="server" Text="Others Cost" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkOthRef" runat="server" Text='<%# Eval("TotalOtheritemsCost") %>' EnableViewState="true"  />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#236802" HeaderText="Tool Amortize" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <HeaderTemplate>
                                                                Tool Amortize
                                                                <div style="display:none;">
                                                                    <asp:CheckBox ID="chkAllToolAmortizeRef" runat="server" Text="Tool Amortize" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="display:none;">
                                                                    <asp:CheckBox ID="chkToolAmortizeRef" runat="server" EnableViewState="true"  />
                                                                </div>
                                                                <asp:DropDownList ID="DdlToolAmortizeRef"  runat="server" EnableViewState="true" >
                                                                    <asp:ListItem Value="ADD"> ADD </asp:ListItem>
                                                                    <asp:ListItem Value="REMOVE" Selected="True"> REMOVE </asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#236802" HeaderText="Machine Amortize" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <HeaderTemplate>
                                                                Machine Amortize
                                                                <div style="display:none;">
                                                                <asp:CheckBox ID="chkAllMachineAmortizeRef" runat="server" Text="Machine Amortize" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="display:none;">
                                                                    <asp:CheckBox ID="chkMachineAmortizeRef" runat="server" EnableViewState="true"  />
                                                                </div>
                                                                <asp:DropDownList ID="DdlMachineAmortizeRef" runat="server" EnableViewState="true" >
                                                                    <asp:ListItem Value="ADD"> ADD </asp:ListItem>
                                                                    <asp:ListItem Value="REMOVE" Selected="True"> REMOVE </asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="TotalMaterialCost" HeaderText="Tot.Mat. Cost" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                        <%--<asp:BoundField DataField="TotalProcessCost" HeaderText="Tot.Proc. Cost" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                        <%--<asp:BoundField DataField="TotalSubMaterialCost" HeaderText="Tot.SubMat. Cost" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                        <%--<asp:BoundField DataField="TotalOtheritemsCost" HeaderText="Tot.Oth. Cost" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                        <%--<asp:BoundField DataField="GrandTotalCost" HeaderText="Gr.Tot. Cost" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                        <asp:BoundField DataField="FinalQuotePrice" HeaderText="Final Price" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:BoundField DataField="ReqType" HeaderText="Req Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:BoundField DataField="MMPlantStatus" HeaderText="Plant Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                    <PagerSettings PageButtonCount="10" />
                                                    <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                    <RowStyle ForeColor="Black" BackColor="White" />
                                                    </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    </div>

                                    <div class="col-md-12">
                                        <table id="TbQuoteRefList" class="table table-striped table-bordered nopadding">
                                            <thead>
                                                <tr>
                                                    <th>Quote No Ref</th>
                                                    <th>Vendor Code</th>
                                                    <th>Product</th>
                                                    <th>Material Class</th>
                                                    <th>Material</th>
                                                    <th>Material Desc</th>
                                                    <th>Process Group</th>
                                                    <th>Prc Grp Desc</th>
                                                    <th><input type="checkbox" id="chkAllRefHd" onclick="chkAllRefHd_Click()" /> <label id="LbchkAllRefHd" for="chkAllRefHd"> All Cost </label></th>
                                                    <th><input type="checkbox" id="chkAllMatRef" onclick="chkHeaderColumn_Click('MatCost')" /> <label id="LbchkAllMatRef" for="chkAllMatRef"> Mat Cost </label></th>
                                                    <th><input type="checkbox" id="chkAllProcRef" onclick="chkHeaderColumn_Click('ProcCost')" /> <label id="LbchkAllProcRef" for="chkAllProcRef"> Proc Cost </label></th>
                                                    <th><input type="checkbox" id="chkAllSubMatRef" onclick="chkHeaderColumn_Click('SubMatCost')" /> <label id="LbchkAllSubMatRef" for="chkAllSubMatRef"> Sub Mat Cost </label></th>
                                                    <th><input type="checkbox" id="chkAllOthRef" onclick="chkHeaderColumn_Click('OthCost')" /> <label id="LbchkAllOthRef" for="chkAllOthRef">  Others Cost </label></th>
                                                    <th>Tool Amortize</th>
                                                    <th>Machine Amortize</th>
                                                    <th>Final Price</th>
                                                    <th>Req Type</th>
                                                    <th>Plant Status</th>

                                                    <th>ToolAmorExist</th>
                                                    <th>ToolAmorExpired</th>
                                                    <th>MacAmorExist</th>
                                                    <th>MacAmorExpired</th>
                                                    <th>Layout</th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                </div>

                                <div class="row " style="padding-top:10px;">
                                    <div class="col-lg-12">
                                    <div class="col-lg-12" style="border-bottom:2px solid #006EB7"></div>
                                    </div>
                                </div>

                                <div class="row" style="border-top-left-radius: 15px;border-top-right-radius: 15px; padding-top:10px; padding-bottom:0px;">
                                    <div class="col-md-8">
                                        <label style="color:orangered;font-weight:bolder;font:bold 18px calibri light"> Please maintain MDM data to allow Vendor to add in Machine Amortization details in quotation </label>
                                    </div>
                                    <div class="col-md-4 text-right">
                                        <asp:Button ID="BtnAddToList" runat="server" Text="Add To List" Visible="true" OnClientClick="if(CekDuplicateWithExpiredReq()==false) return false; " autopostback="false"
                                            OnClick="BtnAddToList_Click" CssClass="btn btn-sm btn-primary btn-sm" Font-Names="calibri" Font-Size="14px"/>
                                        <asp:LinkButton ID="BtnCancel" runat="server" Text="Cancel" OnClick="BtnCancel_Click" OnClientClick="CloseModalQuoteRef();" CssClass="btn btn-sm btn-default btn-sm" Font-Names="calibri" Font-Size="14px" />
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

    <!-- Modal confirmation-->
    <div class="modal fade" id="MdConfirm" data-backdrop="static" tabindex="-1" role="dialog"
                        aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
            <div class="modal-dialog">
                    <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header" style="padding:5px; background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5);border-bottom-left-radius: 15px;border-bottom-right-radius: 15px;">
                        <div class="col-md-12 Padding-Nol" style="font:bold 22px calibri, calibri; text-align:center; align-content:center;"> 
                            <span class="glyphicon glyphicon-question-sign" style="color:#005496;font-size:24px;"></span>
                            Confirmation 
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-12" style="padding:10px">
                                        Add More Data into the List ?
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer" style="padding:5px; background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5);border-top-left-radius: 15px;border-top-right-radius: 15px;">
                        <asp:Button ID="BtnYes" runat="server" Text="Yes" OnClientClick="CloseModalConfirm(); return false;" autopostback="false"  CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="14px"/>
                        <asp:Button ID="BtnNo" runat="server" Text="No" OnClientClick="CloseModalConfirm();CloseModalQuoteRef();ChgEmptyFlColor();return false;" autopostback="false" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="14px"/>
                    </div>
                </div>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    <!-- Modal duplicate reuest with expired request-->
    <div class="modal fade" id="MdDuplicateReq" data-backdrop="static" tabindex="-1" role="dialog" 
        aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false" >
            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
            <ContentTemplate>
            <div class="modal-dialog" style="width:95%; position: absolute; margin-top:0px; top:0px; margin-left:2%; ">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel21" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row"  style="background-color:#F7F7F7; padding-bottom:10px; padding-left:10px; padding-right:10px;
                                          box-shadow: 1px 1px 1000px 1px;
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
                                            <div class="table table-responsive table-sm">
                                                <asp:GridView ID="GvDuplicateWithExpiredReq" runat="server" AutoGenerateColumns="False" 
                                                    AllowPaging="false" PageSize="10" OnRowDataBound="GvDuplicateWithExpiredReq_RowDataBound"
                                                    CssClass="table-sm table-bordered table-nowrap WrapCnt" Font-Bold="False" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="RequestDate" HeaderText="Request Date"  HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="RequestNumber" HeaderText="Req. No" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="QuoteResponseDueDate" HeaderText="Res Due Date"  HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="QuoteNo" HeaderText="QuoteNo"   HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="Material" HeaderText="Material" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc"  HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Reject" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <HeaderTemplate>
                                                                <asp:RadioButton ID="RbAllReject" runat="server" Text=" &nbsp; Reject" GroupName="RbActionHeader" onchange="CheckAllRejOrChgDate('Reject')"/>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="RbReject" GroupName="RbAction" runat="server" ></asp:RadioButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Change Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <HeaderTemplate>
                                                                <asp:RadioButton ID="RbAllchangeDate" runat="server" Text=" &nbsp; Change Date" GroupName="RbActionHeader" onchange="CheckAllRejOrChgDate('Changedate')"/>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="RbChangeDate" GroupName="RbAction" runat="server" ></asp:RadioButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="New Response Due Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <div class="group-main">
                                                                    <asp:TextBox ID="TxtNewDueDate" OnclientClick="return false;" runat="server"  Text='<%# Eval("QuoteResponseDueDate") %>' Width="100%" Enabled="false"
                                                                        CssClass="TxtGrid" ToolTip="New Response Due Date" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" >
                                                                    </asp:TextBox>
                                                                    <span class="SearchBox-btn-cal" style="background-color:#E9ECEF; padding:2px 3px 2px 3px;">
                                                                    <span class="fa fa-calendar" style="color:#005496;" id="IcnCalendarNewDueDate" runat="server" ></span>
                                                                    </span>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                    <PagerSettings PageButtonCount="10"  />
                                                    <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" BorderColor="White"/>
                                                    <RowStyle ForeColor="#000066" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <table id="TbDuplicateWithExpiredReq" class="table table-striped table-bordered nopadding">
                                            <thead>
                                                <tr>
                                                    <th>Request Date</th>
                                                    <th>Req. No</th>
                                                    <th>Res Due Date</th>
                                                    <th>QuoteNo</th>
                                                    <th>Material</th>
                                                    <th>Material Desc</th>
                                                    <th>Vendor Code</th>
                                                    <th>Vendor Name</th>
                                                    <th><input type="radio" id="RbAllReject" onclick="CheckAllRejOrChgDate('Reject')" name="RejOrChgDateHeader" /> <label id="LbRbAllReject" onclick="CheckAllRejOrChgDate('Reject')" for="RbAllReject"> Reject </label></th>
                                                    <th><input type="radio" id="RbAllchangeDate" onclick="CheckAllRejOrChgDate('Changedate')" name="RejOrChgDateHeader" /> <label id="LbRbAllchangeDate" onclick="CheckAllRejOrChgDate('Changedate')" for="RbAllchangeDate"> Change Date </label></th>
                                                    <th>New Response Due Date</th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>

                                    <div class="row pull-right" style="padding-top:10px;">	
                                        <asp:Button ID="BtnSubmitProcDuplicateReg" runat="server" Text="Submit" OnClientClick="if(ValidateDuplicateReqList()==false) return false;SumbitDuplicateReqList();" autopostback="false"  OnClick="BtnSubmitProcDuplicateReg_Click"  CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="14px"/>
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
    
</body>
</html>
