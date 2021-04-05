<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReqApproval.aspx.cs" Inherits="Material_Evaluation.ReqApproval" %>

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
    <link rel="stylesheet" href="Scripts/jquery-ui-1.12.1/jquery-ui.css" />
    <link rel="stylesheet" href="js/jsextendsession/css/timeout-dialog.css" />
    <link href="js/BootstrapDatePcr/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <style type="text/css">
        .SideBarMenu {
            width: 300px;
        }

        .WrapCnt td, th {
            white-space: normal !important;
            /*word-wrap: break-word;*/
            font-size: 14px !important;
        }

        .WrapCnt a {
            padding: 0px;
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
    <script type="text/javascript">
        function preventInput(evnt) {

            if (evnt.which != 9) evnt.preventDefault();

        }
        function ValidateAll() {
            var count = 0;
            var rowspancount = 0;
            var rowreasoncount = 0;
            $('.dummyClass').each(function (index, item) {
                count++;
                var rr = $(this).parent().siblings(":first").attr('rowspan');

                rowspancount = rr;

                alert(rowspancount);
                alert(count);

                if (count <= rowspancount) {
                    if ($(this).val() != "") {
                        //count = 1;
                        rowreasoncount++;
                    }
                    else {
                        alert("fill all the Reasons");

                        $("#lblhdnreason").val("1");

                        return false;
                    }

                }

                if (rowspancount == rowreasoncount) {
                    alert("IN");
                    $("#lblhdnreason").val("");
                }

            });

        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            DatePitcker();
        });

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
    </script>

    <%--script loading page--%>
    <script language="javascript" type="text/javascript">
        $(window).load(function () {
            try {
                $('#loading').fadeOut("fast");

                if ($('#IsFirstLoad').val("1")) {
                    $('#BtnCekLatestNested').click();
                }
            }
            catch (err) {
                alert(err + ' : on load page');
            }
        });
    </script>
    <script type="text/javascript">
        function Layout7Condition() {
            try {
                if ($('#Txtlayout').val() == "Layout7") {
                    $('#TabProcess').hide();
                    $('#TabSubMat').hide();

                    //var table = document.getElementById('GvCompSumarizeQuote');
                    //var rowscount = table.rows.length;
                    //for (var i = 0; i < (rowscount - 2) ; i++) {
                    //    table.rows[i].cells[5].style.display = "none";
                    //}
                }
                else {
                    $('#TabProcess').show();
                    $('#TabSubMat').show();
                }
            } catch (err) {
                alert("Layout7Condition : " + err)
            }
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
        function ApprReason() {
            var MaxLength = 200;
            var a = document.getElementById('TxtReasonApproval').value;

            $('#TxtReasonApproval').keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });

            a = document.getElementById('TxtReasonApproval').value
            document.getElementById('LblengtVC').innerHTML = ' ' + (200 - a.length) + ' character left'

            if (a.length > 200) {
                a = a.slice(0, 200);
                document.getElementById('LblengtVC').innerHTML = ' ' + (200 - a.length) + ' character left';
                $("#TxtReasonApproval").val(a);
            }
        }

        function ApprReasonAndUpdDate() {
            var MaxLength = 200;
            var a = document.getElementById('TxtreasonApprAndUpdDate').value;

            $('#TxtreasonApprAndUpdDate').keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });

            a = document.getElementById('TxtreasonApprAndUpdDate').value
            document.getElementById('LblengtAprnDate').innerHTML = ' ' + (200 - a.length) + ' character left'

            if (a.length > 200) {
                a = a.slice(0, 200);
                document.getElementById('LblengtAprnDate').innerHTML = ' ' + (200 - a.length) + ' character left';
                $("#TxtreasonApprAndUpdDate").val(a);
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

        function freezeheader() {
            try {
                (function ($) {
                    $("#GvCompSumarizeQuote").tableHeadFixer({ 'left': 3 });
                    $("#GvCmpDataMaterial").tableHeadFixer({ 'left': 5, head: true });
                    $("#GvCmpDataProcCost").tableHeadFixer({ 'left': 5, head: true });
                    $("#GvCmpDataSubMatCost").tableHeadFixer({ 'left': 4, head: true });
                    $("#GvCmpDataOthCost").tableHeadFixer({ 'left': 4, head: true });
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : freezeheader');
            }
        }
        function Tabs() {
            try {
                (function ($) {
                    $("#tabs").tabs();
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : Tabs');
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

        function OpenModalYesNo() {
            try {
                jQuery.noConflict();
                $("#myModalYesNo").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            catch (err) {
                alert(err + ' : OpenModalYesNo');
            }
        }
        function CloseModalYesNo() {
            try {
                jQuery.noConflict();
                $("#myModalYesNo").modal('hide');
            }
            catch (err) {
                alert(err + ' : CloseModalYesNo');
            }
        }

        function OpenModalUpdateDate() {
            try {
                jQuery.noConflict();
                $("#myModalUpdateDate").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            catch (err) {
                alert(err + ' : OpenModalYesNo');
            }
        }
        function CloseModalUpdateDate() {
            try {
                jQuery.noConflict();
                $("#myModalUpdateDate").modal('hide');
            }
            catch (err) {
                alert(err + ' : CloseModalUpdateDate');
            }
        }

        function OpenModalReasonRejection() {
            try {
                jQuery.noConflict();
                $("#myModalReasonRejection").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            catch (err) {
                alert(err + ' : OpenModalReasonRejection');
            }
        }
        function CloseModalReasonRejection() {
            try {
                jQuery.noConflict();
                $("#myModalReasonRejection").modal('hide');
            }
            catch (err) {
                alert(err + ' : CloseModalReasonRejection');
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
        function OpenModalCompare() {
            try {
                jQuery.noConflict();
                $("#MdCompare").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            catch (err) {
                alert(err + ' : OpenModalCompare');
            }
        }
        function CloseModalCompare() {
            try {
                jQuery.noConflict();
                $("#MdCompare").modal('hide');
            }
            catch (err) {
                alert(err + ' : CloseModalCompare');
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
    </script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "images/minus1.png");

            //var ImageID = $(this).attr('ID');
            //var ArrImageID = ImageID.split('_');
            //$('#GridView1_BtnNstIdPls_' + ArrImageID[2]).click();
        });

        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "images/plus1.png");
            $(this).closest("tr").next().remove();

            //var ImageID = $(this).attr('ID');
            //var ArrImageID = ImageID.split('_');
            //$('#GridView1_BtnNstIdMin_' + ArrImageID[2]).click();
        });

        $(document).on('keypress', '#TxtEffDate', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });
        $(document).on('keypress', '#TxtDueDate', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });
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

        <asp:UpdatePanel runat="server" ID="UpsidebarToggle">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="col-lg-12" style="padding: 5px;">
                        <div class="row">
                            <div class="col-md-10" style="padding-top: 5px;">
                                <a onclick="ShowLoading();" href="Home.aspx">
                                    <asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                                <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" ID="sidebarToggle" OnClick="sidebarToggle_Click"><i class="fas fa-bars"></i> </asp:LinkButton>
                                <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                                <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-md-2 fa-pull-right" style="background-color: #E9ECEF;">
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
                                <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION STATUS - <b><asp:Label runat="server" ID="LbPageType" Text="Manager Approval Pending "></asp:Label> </b>
                            </div>
                        </div>
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-8" style="padding-bottom: 5px;">
                                            <asp:Label runat="server" ID="LbFilter" Text="Filter By :"></asp:Label>
                                        </div>
                                        <div class="col-sm-4 text-right" style="padding-bottom: 5px;">
                                            <asp:Button runat="server" ID="BtnReset" Text="Reset" CssClass="btn btn-sm btn-warning" OnClick="BtnReset_Click" autopostback="true"></asp:Button>
                                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger" PostBackUrl="Home.aspx" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:Panel runat="server" DefaultButton="btnSearch">
                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" ID="Label12" Text="Req. Type"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <asp:DropDownList runat="server" ID="DdlReqStatus" AutoPostBack="true" OnSelectedIndexChanged="DdlReqStatus_SelectedIndexChanged">
                                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                            <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                                            <asp:ListItem Text="Revision" Value="Revision"></asp:ListItem>
                                                            <asp:ListItem Text="Mass Revision" Value="MassRevision"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFilterBy" OnSelectedIndexChanged="DdlFilterBy_SelectedIndexChanged" AutoPostBack="true">
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
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <asp:TextBox runat="server" ID="txtFind" Text=""></asp:TextBox></div>
                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF; padding: 4px 5px 1px 5px;">
                                                        <asp:LinkButton ID="btnSearch" CssClass="Padding-Nol" runat="server" autopostback="true" OnClientClick="ShowLoading();" OnClick="btnSearch_Click"><i class="fa fa-search" aria-hidden="true" 
                                                            style="color:#005496;" ></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFltrDate" OnSelectedIndexChanged="DdlFltrDate_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Request Date" Value="RequestDate"></asp:ListItem>
                                                    <asp:ListItem Text="Quote Response Due Date" Value="QuoteResponseDueDate"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <asp:TextBox ID="TxtFrom" OnclientClick="return false;" runat="server" placeholder="Date From" OnTextChanged="TxtFrom_TextChanged" AutoPostBack="true"
                                                            ToolTip="Date From" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                        </asp:TextBox>
                                                    </div>
                                                    <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 2px 3px 2px 3px;">
                                                        <a class="fa fa-calendar" style="color: #005496; padding: 2px 3px 2px 3px;" onclick="javascript: $('#TxtFrom').focus();"></a>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <asp:TextBox ID="TxtTo" OnclientClick="return false;" runat="server" placeholder="Date To" OnTextChanged="TxtTo_TextChanged" AutoPostBack="true"
                                                            ToolTip="Date To" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                        </asp:TextBox>
                                                    </div>
                                                    <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 2px 3px 2px 3px;">
                                                        <a class="fa fa-calendar" style="color: #005496; padding: 2px 3px 2px 3px;" onclick="javascript: $('#TxtTo').focus();"></a>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="UpForm" runat="server">
                                <ContentTemplate>
                                    <div class="row" style="padding-top: 10px; padding-bottom: 10px;">
                                        <div class="col-sm-6 ">
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                <ContentTemplate>
                                                    <asp:LinkButton runat="server" ID="BtnExpandAll" CssClass="btn btn-sm btn-primary btn-sm" Font-Size="14px"
                                                        OnClientClick="ExpandAll();return false;" autopostback="false"><i class="glyphicon glyphicon-collapse-down"></i> Expand All </asp:LinkButton>
                                                    <asp:LinkButton runat="server" ID="BtnColapsAll" CssClass="btn btn-sm btn-info btn-sm" Font-Size="14px"
                                                        OnClientClick="ColapsAll();return false;" autopostback="false"><i class="glyphicon glyphicon-collapse-up"></i> Collapse All </asp:LinkButton>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-sm-6 ">
                                            <asp:Label runat="server" ID="Label6" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                            <asp:TextBox runat="server" ID="TxtShowEntry" Text="10" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                Width="50px" CssClass="fa-pull-right" Style="text-align: center"></asp:TextBox>
                                            <asp:Label runat="server" ID="Label8" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-12 table table-responsive" style="padding: 0px;">
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
                                                        <%--Gridview Level 1--%>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Image ID="Image1" runat="server" Style="cursor: pointer" ImageUrl="~/images/plus1.png" Width="15px" />
                                                                <asp:Panel ID="pnlDet" runat="server" Style="display: none">
                                                                    <asp:GridView ID="GvDet" runat="server" AutoGenerateColumns="false" CssClass="table-hover Padding-Nol table-bordered"
                                                                        OnRowCreated="GvDet_RowCreated" OnRowDataBound="GvlDet_RowDataBound" OnRowCommand="GvDet_RowCommand" DataKeyNames="QuoteNo">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="No.">
                                                                                <ItemTemplate><%# Eval("ParentGvRowNo") %>.<%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="VendorCode1" HeaderText="ID"></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorName" HeaderText="Name"></asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Quote No">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ForeColor="#0033cc" Text='<%# Eval("QuoteNo") %>' runat="server" ID="LbQuoteNo" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="TotalMaterialCost" HeaderText="Material"></asp:BoundField>
                                                                            <asp:BoundField DataField="TotalProcessCost" HeaderText="Process"></asp:BoundField>
                                                                            <asp:BoundField DataField="TotalSubMaterialCost" HeaderText="SubMat"></asp:BoundField>
                                                                            <asp:BoundField DataField="TotalOtheritemsCost" HeaderText="Others"></asp:BoundField>
                                                                            <asp:BoundField DataField="GrandTotalCost" HeaderText="Grand"></asp:BoundField>
                                                                            <asp:BoundField DataField="FinalQuotePrice" HeaderText="Final"></asp:BoundField>
                                                                            <%--<asp:BoundField DataField="NetProfit/Discount" HeaderText="Net Prof/Disc"></asp:BoundField>--%>
                                                                            <asp:BoundField DataField="MngResponseStatus" HeaderText="Decision"></asp:BoundField>
                                                                            <asp:BoundField DataField="AprRejByMng" HeaderText="Name"></asp:BoundField>
                                                                            <asp:BoundField DataField="AprRejDateMng" HeaderText="Updated Date"></asp:BoundField>
                                                                            <asp:BoundField DataField="ManagerReason" HeaderText="Reason"></asp:BoundField>
                                                                            <asp:BoundField DataField="ManagerRemark" HeaderText="Remark"></asp:BoundField>
                                                                            <asp:BoundField DataField="PIRStatus" HeaderText="Overall status"></asp:BoundField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#4d94ff" Font-Bold="True" ForeColor="White" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="No.">
                                                            <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                            <ItemStyle Width="10px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant"></asp:BoundField>
                                                        <asp:BoundField DataField="RequestNumber" HeaderText="Req. No" SortExpression="RequestNumber" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="NoQuote" HeaderText="No.Que" SortExpression="NoQuote" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="RequestDate" HeaderText="Req. Date" SortExpression="RequestDate" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="QuoteResponseDueDate" HeaderText="Respon Dates" SortExpression="QuoteResponseDueDate" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="Product" HeaderText="Product" SortExpression="Product" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="Material" HeaderText="Material" SortExpression="Material" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="CreatedBy" HeaderText="SMN PIC" SortExpression="CreatedBy" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="UseDep" HeaderText="Dept" SortExpression="UseDep" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="ReqStatus" HeaderText="Req Type" SortExpression="ReqStatus" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:Button Text="Compare" ID="BtnCompare" runat="server" CssClass="btn Table-button" CommandName="Compare" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                <div style="display: none">
                                                                    <asp:Button Text=".." ID="BtnNstIdPls" runat="server" CssClass="btn Table-button btn-block" CommandName="TrgNestedExpand" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                    <asp:Button Text=".." ID="BtnNstIdMin" runat="server" CssClass="btn Table-button btn-block" CommandName="TrgNestedColapse" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                </div>
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
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="LbTtlRecords" runat="server" Text="Total Record : 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                        </div>
                                    </div>

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

        <!--modal data compare-->
        <div class="modal fade" id="MdCompare" data-backdrop="static" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog" style="width: 95%; position: absolute; margin-top: 0px; top: 0px; margin-left: 2%;">
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row" style="background-color: #F7F7F7; padding-bottom: 30px; padding-left: 10px; padding-right: 10px; box-shadow: 1px 1px 1000px 1px; border-top-left-radius: 10px; border-top-right-radius: 10px; border-bottom-left-radius: 10px; border-bottom-right-radius: 10px;">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-lg-12 text-center" style="padding-top: 10px; padding-bottom: 10px;">
                                                    <asp:UpdatePanel ID="UPClose" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text="Compare Data Quotation" Font-Size="Large"
                                                                Font-Bold="true"></asp:Label>
                                                            <asp:Button runat="server" ID="BtnCloseCompare" Text="X" OnClientClick="CloseModalCompare();" OnClick="BtnCloseCompare_Click"
                                                                CssClass="btn btn-sm btn-danger fa-pull-right" Font-Names="calibri" Font-Size="10px" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>

                                            <div class="row" style="padding-top: 10px; background-color: white;">
                                                <div class="col-md-3">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server" Text="Request No" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid #2a37c8">
                                                                <asp:Label ID="LbReqNo" runat="server" Text=""></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label3" runat="server" Text="Request Date" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid #2a37c8">
                                                                <asp:Label ID="LbReqDate" runat="server" Text=""></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="Quote Res. Due Date" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid #2a37c8">
                                                                <asp:Label ID="LbQuoteResDuDate" runat="server" Text=""></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label5" runat="server" Text="Product" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid #2a37c8">
                                                                <asp:Label ID="LbProduct" runat="server" Text=""></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server" Text="SMN PIC" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid #2a37c8">
                                                                <asp:Label ID="LbSMNPIC" runat="server" Text=":"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <asp:Label ID="Label7" runat="server" Text="Material" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <div class="col-md-9">
                                                            <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid #2a37c8">
                                                                <asp:Label ID="LbMaterial" runat="server" Text=""></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <asp:Label ID="Label9" runat="server" Text="Material Desc" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <div class="col-md-9">
                                                            <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid #2a37c8">
                                                                <asp:Label ID="LbMatDesc" runat="server" Text=""></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="row" style="padding-top: 10px; background-color: white;">
                                                <div id="tabs" class="col-lg-12" style="padding: 0px 10px 0px 10px; border: none;">
                                                    <ul>
                                                        <li id="TabSumarize"><a href="#tabs-1">Sumarize</a></li>
                                                        <li id="Tabmaterial"><a href="#tabs-2">Material</a></li>
                                                        <li id="TabProcess"><a href="#tabs-3">Proces Cost</a></li>
                                                        <li id="TabSubMat"><a href="#tabs-4">Sub-Mat/T&J Cost</a></li>
                                                        <li id="TabOth"><a href="#tabs-5">Other Cost</a></li>
                                                    </ul>

                                                    <div id="tabs-1" style="border: 3px solid white; padding: 0px;">
                                                        <div class="row">
                                                            <div class="col-lg-12">
                                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row" style="padding: 0px 15px 0px 15px;">
                                                                            <div class="col-lg-12 table-responsive" style="padding: 0px; border: none;">
                                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel9" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="GvCompSumarizeQuote" runat="server" AutoGenerateColumns="false"
                                                                                            OnRowDataBound="GvCompSumarizeQuote_RowDataBound" OnRowCreated="GvCompSumarizeQuote_RowCreated"
                                                                                            CssClass="table-responsive table-bordered table-hover table-nowrap  Padding-Nol WrapCnt">
                                                                                            <FooterStyle BackColor="White" />
                                                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="QuoteNo" HeaderText="Quote No" />
                                                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                                                                                                <asp:BoundField DataField="crcy" HeaderText="CURR"></asp:BoundField>
                                                                                                <%--Gridview Level MatCostDet--%>
                                                                                                <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Panel ID="pnlDet" runat="server" Style="display: block">
                                                                                                            <asp:GridView ID="GvDetMatCost" runat="server" AutoGenerateColumns="false"
                                                                                                                OnRowDataBound="GvDetMatCost_RowDataBound" Width="100%"
                                                                                                                CssClass="table-hover Padding-Nol" DataKeyNames="QuoteNo">
                                                                                                                <Columns>
                                                                                                                    <asp:BoundField DataField="MaterialDescription" HeaderText="Raw Material Description" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                                                                    <asp:BoundField DataField="RawMaterialCost/kg" HeaderText="Raw Material Cost / kg" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                                                                    <asp:BoundField DataField="MaterialCost/pcs" HeaderText="Material Cost / pcs" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                                                                </Columns>
                                                                                                                <HeaderStyle BackColor="#4d94ff" Font-Bold="True" ForeColor="White" />
                                                                                                            </asp:GridView>
                                                                                                        </asp:Panel>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="TotalMaterialCost" HeaderText="Total Material Cost" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" />

                                                                                                <%--Gridview Level Process Det--%>
                                                                                                <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Panel ID="pnlDetProc" runat="server" Style="display: block">
                                                                                                            <asp:GridView ID="GvDetProcCost" runat="server" AutoGenerateColumns="false"
                                                                                                                CssClass="table-hover Padding-Nol" DataKeyNames="QuoteNo" Width="100%">
                                                                                                                <Columns>
                                                                                                                    <asp:BoundField DataField="SubProcess" HeaderText="Sub Process" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                                                                    <asp:BoundField DataField="ProcessCost/pc" HeaderText="Process Cost / pc" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                                                                </Columns>
                                                                                                                <HeaderStyle BackColor="#4d94ff" Font-Bold="True" ForeColor="White" />
                                                                                                            </asp:GridView>
                                                                                                        </asp:Panel>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="TotProcOri" HeaderText="Actual Process Cost" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" />
                                                                                                <asp:BoundField DataField="Profit" HeaderText="Profit (%)" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="Discount" HeaderText="Discount (%)" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="TotalProcessCost" HeaderText="Final Process Cost" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" />

                                                                                                <%--Gridview Level SubMat Det--%>
                                                                                                <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Panel ID="pnlDetSubMat" runat="server" Style="display: block">
                                                                                                            <asp:GridView ID="GvDetSubMatCost" runat="server" AutoGenerateColumns="false" OnRowDataBound="GvDetSubMatCost_RowDataBound"
                                                                                                                CssClass="table-hover Padding-Nol" DataKeyNames="QuoteNo" Width="100%">
                                                                                                                <Columns>
                                                                                                                    <asp:BoundField DataField="Sub-Mat/T&JDescription" HeaderText="Sub-Mat / T&J Description" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                                                                    <asp:BoundField DataField="Sub-Mat/T&JCost/pcs" HeaderText="Sub-Mat / T&J Cost/pcs" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                                                                </Columns>
                                                                                                                <HeaderStyle BackColor="#4d94ff" Font-Bold="True" ForeColor="White" />
                                                                                                            </asp:GridView>
                                                                                                        </asp:Panel>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="TotalSubMaterialCost" HeaderText="Total Sub Material Cost" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" />

                                                                                                <%--Gridview Level Oth Det--%>
                                                                                                <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Panel ID="pnlDetOth" runat="server" Style="display: block; padding-top: 0px;">
                                                                                                            <asp:GridView ID="GvDetOthCost" runat="server" AutoGenerateColumns="false" OnRowDataBound="GvDetOthCost_RowDataBound"
                                                                                                                CssClass="table-hover Padding-Nol" DataKeyNames="QuoteNo" Width="100%">
                                                                                                                <Columns>
                                                                                                                    <asp:BoundField DataField="ItemsDescription" HeaderText="Item Description" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                                                                    <asp:BoundField DataField="OtherItemCost/pcs" HeaderText="ItemCost / pcs" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                                                                </Columns>
                                                                                                                <HeaderStyle BackColor="#4d94ff" Font-Bold="True" ForeColor="White" />
                                                                                                            </asp:GridView>
                                                                                                        </asp:Panel>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="TotalOtheritemsCost" HeaderText="Total Other items Cost" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" />

                                                                                                <asp:BoundField DataField="GrandTotalCost" HeaderText="Grand Total Cost" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" />
                                                                                                <asp:BoundField DataField="FinalQuotePrice" HeaderText="Final Quote Price" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" />
                                                                                                <asp:BoundField DataField="NetProfit/Discount" HeaderText="Prof / Disc" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" />
                                                                                                <%--<asp:BoundField DataField="Profit" HeaderText="Profit"/>
                                                                                <asp:BoundField DataField="Discount" HeaderText="Discount"/>--%>
                                                                                                <%--<asp:BoundField DataField="ActualNU" HeaderText="ActualNU" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right"/>--%>
                                                                                                <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Center" />
                                                                                                <asp:BoundField DataField="DueOn" HeaderText="Due Dt Next Rev" ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Center" />
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div id="tabs-2" class="col-lg-12" style="border: 3px solid white; padding: 0px;">
                                                        <div class="row">
                                                            <div class="col-lg-12">
                                                                <asp:UpdatePanel ID="UpCmpMaterial" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row" style="padding: 0px 15px 0px 15px;">
                                                                            <div class="col-lg-12 table-responsive" style="padding: 0px; border: none;">
                                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel7" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="GvCmpDataMaterial" runat="server" AutoGenerateColumns="true" Font-Size="14px"
                                                                                            OnRowDataBound="GvCmpDataMaterial_RowDataBound"
                                                                                            CssClass="table-responsive  table-sm table-bordered table-hover table-nowrap  Padding-Nol WrapCnt">
                                                                                            <Columns>
                                                                                                <%--<asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" />
                                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                                                                                <asp:BoundField DataField="MaterialSAPCode" HeaderText="Material SAP Code" />
                                                                                <asp:BoundField DataField="MaterialDescription" HeaderText="Material Description"/>
                                                                                <asp:BoundField DataField="RawMaterialCost/kg" HeaderText="Raw Material Cost/ kg" />
                                                                                <asp:BoundField DataField="TotalRawMaterialCost/g" HeaderText="Total Raw Material Cost/ g"  />
                                                                                <asp:BoundField DataField="PartNetUnitWeight(g)" HeaderText="Part Net Unit Weight (g)" />
                                                                                <asp:BoundField DataField="~~Thickness(mm)" HeaderText="Thickness (mm)" />
                                                                                <asp:BoundField DataField="~~Width(mm)" HeaderText="Width (mm)" />
                                                                                <asp:BoundField DataField="~~Pitch(mm)" HeaderText="Pitch (mm)"/>
                                                                                <asp:BoundField DataField="~MaterialDensity" HeaderText="Material Density"/>
                                                                                <asp:BoundField DataField="~RunnerWeight/shot(g)" HeaderText="Runner Weight/shot (g)"/>
                                                                                <asp:BoundField DataField="~RunnerRatio/pcs(%)" HeaderText="Runner Ratio/pc (%)"/>
                                                                                <asp:BoundField DataField="~RecycleMaterialRatio(%)" HeaderText="Recycle Material Ratio (%)"/>
                                                                                <asp:BoundField DataField="Cavity" HeaderText="Base Qty / Cavity"/>
                                                                                <asp:BoundField DataField="MaterialYield/MeltingLoss(%)" HeaderText="Material Loss/Melting Loss (%)"/>
                                                                                <asp:BoundField DataField="MaterialGrossWeight/pc(g)" HeaderText="Material Gross Weight/pc (g)"/>
                                                                                <asp:BoundField DataField="MaterialScrapWeight(g)" HeaderText="Material Scrap Weight (g)"/>
                                                                                <asp:BoundField DataField="ScrapLossAllowance(%)" HeaderText="Scrap Loss Allowance (%)"/>
                                                                                <asp:BoundField DataField="ScrapPrice/kg" HeaderText="Scrap Price/kg"/>
                                                                                <asp:BoundField DataField="ScrapRebate/pcs" HeaderText="Scrap Rebate/pc"/>
                                                                                <asp:BoundField DataField="MaterialCost/pcs" HeaderText="Material Cost/pc"/>
                                                                                <asp:BoundField DataField="TotalMaterialCost/pcs" HeaderText="Total Material Cost/pc"/>--%>
                                                                                            </Columns>
                                                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                                            <PagerSettings PageButtonCount="10" />
                                                                                            <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                                                            <RowStyle ForeColor="#000066" />
                                                                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div id="tabs-3" class="col-lg-12" style="border: 3px solid white; padding: 0px;">
                                                        <div class="row">
                                                            <div class="col-lg-12">
                                                                <asp:UpdatePanel ID="UpCmpDataProcCost" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row" style="padding: 0px 15px 0px 15px;">
                                                                            <div class="col-lg-12 table-responsive" style="padding: 0px; border: none;">
                                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="GvCmpDataProcCost" runat="server" AutoGenerateColumns="false" Font-Size="14px"
                                                                                            CssClass="table-responsive  table-sm table-bordered table-hover table-nowrap  Padding-Nol WrapCnt">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="crcy" HeaderText="CURR" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="ProcessGrpCode" HeaderText="Process Grp Code" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="SubProcess" HeaderText="Sub Process" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="IfSubcon-SubconName" HeaderText="If Subcon - Subcon Name" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="IfTurnkey-Subvendorname" HeaderText="IfTurnkey-Subvendor Name" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="Machine/Labor" HeaderText="Machine/Labor" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="Machine" HeaderText="Machine" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="StandardRate/HR" HeaderText="Standard Rate/HR" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="VendorRate" HeaderText="Vendor Rate/HR" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="ProcessUOM" HeaderText="Process UOM" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="Baseqty" HeaderText="Base Qty" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="DurationperProcessUOM(Sec)" HeaderText="Duration per Process UOM (Sec)" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="Efficiency/ProcessYield(%)" HeaderText="Efficiency" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="TurnKeyCost" HeaderText="Turnkey Cost/pc" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="TurnkeyFees" HeaderText="Turnkey Fees" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="ProcessCost/pc" HeaderText="Process Cost/pc" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="TotalProcessesCost/pcs" HeaderText="Total Process Cost/pc" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                            </Columns>
                                                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                                            <PagerSettings PageButtonCount="10" />
                                                                                            <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                                                            <RowStyle ForeColor="#000066" />
                                                                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div id="tabs-4" class="col-lg-12" style="border: 3px solid white; padding: 0px;">
                                                        <div class="row">
                                                            <div class="col-lg-12">
                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row" style="padding: 0px 15px 0px 15px;">
                                                                            <div class="col-lg-12 table-responsive" style="padding: 0px; border: none;">
                                                                                <asp:UpdatePanel runat="server" ID="UpCmpDataSubMatCost" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="GvCmpDataSubMatCost" runat="server" AutoGenerateColumns="false" Font-Size="14px"
                                                                                            CssClass="table-responsive  table-sm table-bordered table-hover table-nowrap  Padding-Nol">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="crcy" HeaderText="CURR" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="Sub-Mat/T&JDescription" HeaderText="Sub-Mat/T&J Description" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="Sub-Mat/T&JCost" HeaderText="Sub-Mat/T&J Cost" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="Consumption(pcs)" HeaderText="Consumption (pc)" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="Sub-Mat/T&JCost/pcs" HeaderText="Sub-Mat/T&J Cost/pc" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="TotalSub-Mat/T&JCost/pcs" HeaderText="Total Sub-Mat/T&J Cost/pc" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                            </Columns>
                                                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                                            <PagerSettings PageButtonCount="10" />
                                                                                            <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                                                            <RowStyle ForeColor="#000066" />
                                                                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div id="tabs-5" class="col-lg-12" style="border: 3px solid white; padding: 0px;">
                                                        <div class="row">
                                                            <div class="col-lg-12">
                                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row" style="padding: 0px 15px 0px 15px;">
                                                                            <div class="col-lg-12 table-responsive" style="padding: 0px; border: none;">
                                                                                <asp:UpdatePanel runat="server" ID="UpCmpDataOtherCost" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="GvCmpDataOthCost" runat="server" AutoGenerateColumns="false" Font-Size="14px"
                                                                                            CssClass="table-responsive  table-sm table-bordered table-hover table-nowrap  Padding-Nol">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="crcy" HeaderText="CURR" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="ItemsDescription" HeaderText="Items Description" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="OtherItemCost/pcs" HeaderText="Other Item Cost/pc" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                                <asp:BoundField DataField="TotalOtherItemCost/pcs" HeaderText="Total Other Item Cost/pc" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-center" />
                                                                                            </Columns>
                                                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                                            <PagerSettings PageButtonCount="10" />
                                                                                            <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                                                            <RowStyle ForeColor="#000066" />
                                                                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="display: none;">
                                                <asp:TextBox runat="server" ID="Txtlayout"></asp:TextBox>
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
    </form>
</body>
</html>
