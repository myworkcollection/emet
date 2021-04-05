<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MasterData.aspx.cs" Inherits="Material_Evaluation.MasterData" %>

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


    <%--<script src="Scripts/jquery.min.js" type="text/javascript"></script>--%>
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>

    <style type="text/css">
        .close-button {
            border-radius: 5px;
            border: 1px solid #0634b1;
            background-image: linear-gradient(to top, #009DDD, #0634b1);
            color: #FFFFFF;
            font-family: Calibri;
            height: 90px;
            text-align: center;
            box-shadow: 2px 2px #888888;
            word-wrap: break-word;
            white-space: normal;
        }

            .close-button:hover {
                border-radius: 5px;
                border: 3px solid #0634b1;
                background-image: linear-gradient(to top, #0634b1, #009DDD);
                color: #FFFFFF;
                font-family: Calibri;
                height: 90px;
                text-align: center;
                box-shadow: 2px 2px #b6e7fc;
                word-wrap: break-word;
                white-space: normal;
                cursor: pointer;
            }

        .SideBarMenu {
            width: 300px;
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

            <div id="content-wrapper" style="background-color: white;">
                <div class="container-fluid">
                    <div class="card">
                        <div class="card-header">
                            <div class="card-header-content ">
                                <i class="fas fa-fw fa-table"></i>Master Data
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger fa-pull-right" PostBackUrl="Vendor.aspx" />
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-3 " style="padding-bottom: 10px">
                                    <asp:Button ID="BtnProcGrpVsSubProc" runat="server" Width="100%" Text="Process Group Vs Sub Process" CssClass="close-button" OnClientClick="openInNewTab('ProcGrpVsSubProc.aspx');" />
                                </div>
                                <div class="col-md-3" style="padding-bottom: 10px">
                                    <asp:Button ID="BtnProcGroup" runat="server" Width="100%" Text="Proces Group" CssClass="close-button" OnClientClick="openInNewTab('VndPrcGrp.aspx');" />
                                </div>
                                <div class="col-md-3" style="padding-bottom: 10px">
                                    <asp:Button ID="BtnMachineList" runat="server" Width="100%" Text="Machine List" CssClass="close-button" OnClientClick="openInNewTab('VndMachineList.aspx');" />
                                </div>
                                <div class="col-md-3" style="padding-bottom: 10px">
                                    <asp:Button ID="BtnPICEmail" runat="server" Width="100%" Text="PIC Email" CssClass="close-button" OnClientClick="openInNewTab('VndPICEmail.aspx');" />
                                </div>

                                <%--<div class="col-md-3" style="padding-bottom:10px">
                                <asp:Button ID="BtnLabRate" runat="server" Width="100%" Text="Labour Rate" CssClass="close-button" OnClientClick="openInNewTab('VndLabRate.aspx');" />
                                </div>--%>
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
                                    <asp:Button ID="StartTimer" runat="server" Text="Start" OnClick="StartTimer_Click" CssClass="btn btn-sm btn-primary" /></div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>

    <%--script loading page--%>
    <script lang="javascript" type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });

        $(document).ready(function () {
            playAudio();
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
        function openInNewTab(url) {
            var win = window.open(url, '_blank');
            win.focus();
        }

        function ShowLoading() {
            $('#loading').show();
        }
        function CloseLoading() {
            $('#loading').fadeOut("fast");
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
