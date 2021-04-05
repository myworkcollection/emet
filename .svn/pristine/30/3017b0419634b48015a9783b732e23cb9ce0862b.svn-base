<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vannouncement.aspx.cs" Inherits="Material_Evaluation.Vannouncement" %>

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
    </style>
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


                    <!-- <li class="nav-item active">
                      <a class="nav-link" onclick="ShowLoading();" href="aboutemet.aspx?">
                        <i class="fas  fa-fw fa-info"></i>
                        <span> About</span>
                      </a>
                    </li> -->
                </ul>
            </div>

            <!-- Content -->
            <div id="content-wrapper">
                <div class="container-fluid">
                    <div class="card">
                        <div class="card-header">
                            <div class="card-header-content ">
                                <i class="fas fa-info-circle"></i>Announcement
                            </div>
                        </div>

                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-12" style="padding-bottom: 5px;">
                                    <asp:UpdatePanel ID="UpForm" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" DefaultButton="btnSearch">
                                                <div class="row" id="DvFilter" runat="server">
                                                    <div class="col-lg-12" style="padding-bottom: 5px;">
                                                        <div class="group-main">
                                                            <div class="SearchBox-txt">
                                                                <asp:TextBox runat="server" ID="txtFind" Text="" placeholder="Find By Subject" ToolTip="Find by Subject"></asp:TextBox></div>
                                                            <span class="SearchBox-btn" style="background-color: #E9ECEF;">
                                                                <asp:LinkButton ID="btnSearch" runat="server" autopostback="true" OnClientClick="ShowLoading();" OnClick="btnSearch_Click"><i class="fa fa-search" aria-hidden="true" 
                                                                style="color:#005496;" ></i></asp:LinkButton>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" runat="server" id="DvSubjectList">
                                                    <div class="col-md-12">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional" RenderMode="Block">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="GridView1" runat="server"
                                                                    AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                                    AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand"
                                                                    AllowSorting="True" OnSorting="GridView1_Sorting"
                                                                    OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound"
                                                                    CssClass="table-responsive  table-sm table-bordered table-nowrap Padding-Nol" Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Subject" SortExpression="Subject">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="LbSubject" Text='<%# Eval("Subject") %>' runat="server" ForeColor="Black"
                                                                                    CommandName="CRead" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                <asp:HiddenField ID="HiddenId" Value='<%# Eval("id") %>' runat="server" />
                                                                                <asp:HiddenField ID="HiddenStatus" Value="" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-Width="50px" SortExpression="CreatedDate"></asp:BoundField>
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
                                                </div>


                                                <div class="row" runat="server" id="DvLbTotRecord">
                                                    <div class="col-md-4">
                                                        <asp:Label ID="LbRead" runat="server" Text="Read : 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="LbUnread" runat="server" Text="UnRead : 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="LbTtlRecords" runat="server" Text="Total Record : 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row" runat="server" id="DvContent" visible="false">
                                                    <div class="col-md-12">
                                                        <asp:Button ID="BtnCloseCntnt" runat="server" CssClass="btn btn-sm btn-primary fa-pull-right" Text="Back" OnClick="BtnCloseCntnt_Click" aria-hidden="true" Width="50px" />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="Label1" runat="server" Text="Subject" Font-Bold="true"></asp:Label>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <div class="col-md-12 Padding-Nol" style="border-bottom: 1px solid black">
                                                            <asp:Label ID="LbSubject" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="padding-top: 20px">
                                                        <asp:TextBox ID="TxtContent" runat="server" Text="" Enabled="false" TextMode="MultiLine" Width="100%" Height="100%" ReadOnly="true"></asp:TextBox>
                                                    </div>

                                                    <div class="col-md-12" id="DvdAttachmanetOld" runat="server" style="padding-bottom: 5px;">
                                                        <div class="row">
                                                            <div class="col-sm-10">
                                                                <asp:Label ID="LblFileName" runat="server" Text="" placeholder=""></asp:Label>
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <asp:LinkButton ID="LBtnDownloadOld" runat="server" Text="Download And View" CssClass="fa-pull-right"
                                                                    OnClientClick="if(CheckFileUploadOld()==false) return false;" OnClick="LBtnDownloadOld_Click"></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                        <div style="display: none">
                                                            <asp:TextBox ID="TxtFilePath" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="LBtnDownloadOld" />
                                        </Triggers>
                                    </asp:UpdatePanel>
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

        function DatePitcker() {
            try {
                (function ($) {
                    $('#TxtFrom').datepicker({
                        buttonImageOnly: true,
                        dateFormat: "dd/mm/yy"
                    });

                    $('#TxtTo').datepicker({
                        buttonImageOnly: true,
                        dateFormat: "dd/mm/yy"
                    });
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : DatePitcker');
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
