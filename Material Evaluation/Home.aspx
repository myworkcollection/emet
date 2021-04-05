<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Material_Evaluation.Home" %>

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

    
    <%--<script src="Scripts/jquery.min.js" type="text/javascript"></script>--%>
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>

      <style type="text/css">
          .SideBarMenu {
          width:300px;
          }
          .MyLink :hover{
              color:#ff6a00;
              text-decoration-line:underline;
          }
          .card:hover {
          box-shadow: 0 7px 14px 0 rgba(79, 209, 251, 0.20);
          }
      </style>
  </head>

  <body id="page-top">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
        <div class="col-md-12" id="DvMsgErr" runat="server" visible="false">
            <asp:Label runat="server" ID="LbMsgErr" Font-Bold="true" Visible="true"></asp:Label>
        </div>
        <!-- Header -->
        <asp:UpdatePanel runat="server" ID="UpsidebarToggle">
        <ContentTemplate>
        <div class="container-fluid">
            <div class="col-lg-12" style="padding:5px;">
            <div class="row">
                <div class="col-lg-10" style="padding-top:5px;">
                    <a onclick="ShowLoading();" href="Home.aspx"><asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                    <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" OnClick="sidebarToggle_Click"><i class="fas fa-bars"></i> </asp:LinkButton>
                    <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                    <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-lg-2 fa-pull-right" style="background-color:#E9ECEF;">
                    <asp:Label ID="lblUser"  runat="server" Width="147px"></asp:Label><br />
                    <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
                    <asp:LinkButton runat="server"  ID="BtnLogOut" OnClick="BtnLogOut_Click" Text="Logout"></asp:LinkButton>
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

          <!-- content -->
          <div id="content-wrapper" style="background-color:white;">
            <div class="container-fluid">
              <!-- Breadcrumbs-->
              <ol class="breadcrumb" style="background-color:#E9ECEF;">
                <li style="font-size:18px;"><a href="#">Dashboard / Overview</a></li>
              </ol>
            
             <div class="row">
                <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label2" runat="server" CssClass="text-center" Text="Quote Request </br> With SAP Code" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer">
                            <a onclick="ShowLoading();" href="Emet_author.aspx?num=3" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="lblvenrresponse" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                <i class="fas fa-angle-right"></i>
                                </span>
                            </a>
                        </div>
                    </div>
                </div>

                 <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label10" runat="server" CssClass="text-center" Text="Quote Request <br> With SAP Code (Revision)" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer">
                            <a onclick="ShowLoading();" href="Emet_author.aspx?num=22" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LblRevison" runat="server" Text="Here" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                                <i class="fas fa-angle-right"></i>
                                </span>
                            </a>
                        </div>
                    </div>
                </div>

                 <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label13" runat="server" CssClass="text-center" Text="Quote Request <br> With SAP Code <br> (Mass Revision)" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer" style="padding:10px 30px 10px 20px;">
                            <div class="row">
                            <!--<a onclick="ShowLoading();" href="Emet_author.aspx?num=8">-->
                            <a onclick="ShowLoading();" href="MassRevReqWait.aspx?num=1" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LbMassMatRevWait" runat="server" Text="Before Submited" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                            <div class="row">
                            <!--<a onclick="ShowLoading();" href="Emet_author.aspx?num=8">-->
                            <a onclick="ShowLoading();" href="MassRevReqWait.aspx?num=2" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LbMassMatRevSubmit" runat="server" Text="Submited" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label11" runat="server" CssClass="text-center" Text="Quote Request </br> With SAP Code </br> (Mass Revision All)" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer">
                            <a onclick="ShowLoading();" href="MassRevReqWaitAll.aspx" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LbTotRecMassRevAllWait" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                <i class="fas fa-angle-right"></i>
                                </span>
                            </a>
                        </div>
                    </div>
                </div>
             </div>

            <div class="row" style="padding-top:10px;">
                <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label6" runat="server" CssClass="text-center" Text="Manager Approval Pending" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer" style="padding:10px 30px 10px 20px;">
                            <div class="row">
                            <a onclick="ShowLoading();" href="Approval.aspx?num=1" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="lblCount" runat="server" Text="New" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                            <div class="row">
                            <a onclick="ShowLoading();" href="Approval.aspx?num=2" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LbMassRevision" runat="server" Text="Mass Revision" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                        </div>
                    </div>
                </div>
                    
                <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label7" runat="server" CssClass="text-center" Text="DIR Approval Pending" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer" style="padding:10px 30px 10px 20px;">
                            <div class="row">
                            <a onclick="ShowLoading();" href="ManagerApproval.aspx?num=1" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="lblD" runat="server" Text="New" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                            <div class="row">
                            <a onclick="ShowLoading();" href="ManagerApproval.aspx?num=2" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LbDMass" runat="server" Text="Mass Revision" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label3" runat="server" CssClass="text-center" Text="Quote Request <br> Without SAP Code" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer" style="padding:10px 30px 10px 20px;">
                            <div class="row">
                            <!--<a onclick="ShowLoading();" href="Emet_author.aspx?num=8">-->
                            <a onclick="ShowLoading();" href="EWhitoutSAPCode.aspx?num=1" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LblBeforeSubmited" runat="server" Text="Before Submited" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                            <div class="row">
                            <!--<a onclick="ShowLoading();" href="Emet_author.aspx?num=8">-->
                            <a onclick="ShowLoading();" href="EWhitoutSAPCode.aspx?num=2" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LblAfterSubmited" runat="server" Text="Submited" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label8" runat="server" CssClass="text-center" Text="Quote Request <br> Without SAP Code (GP)" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer" style="padding:10px 30px 10px 20px;">
                            <div class="row">
                            <!--<a onclick="ShowLoading();" href="Emet_author.aspx?num=8">-->
                            <a onclick="ShowLoading();" href="EWhitoutSAPCodeGp.aspx?num=1" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LblBeforeSubmitedGp" runat="server" Text="Before Submited" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                            <div class="row">
                            <!--<a onclick="ShowLoading();" href="Emet_author.aspx?num=8">-->
                            <a onclick="ShowLoading();" href="EWhitoutSAPCodeGp.aspx?num=2" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LblAfterSubmitedGp" runat="server" Text="Submited" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                </span>
                            </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" style="padding-top:10px;">
                <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label4" runat="server" CssClass="text-center" Text="Completed Requests" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer">
                            <a onclick="ShowLoading();" href="Emet_author.aspx?num=7" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="lblclosed" runat="server" Text="" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                                <i class="fas fa-angle-right"></i>
                                </span>
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label9" runat="server" CssClass="text-center" Text="All Request" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer">
                            <a onclick="ShowLoading();" href="EAllRequest.aspx" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="LblAllReq" runat="server" Text="Here" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                                <i class="fas fa-angle-right"></i>
                                </span>
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-lg-3" style="padding-bottom:10px;">
                    <div class="card" style="padding:2px;background-color:#CCCFD1">
                        <div class="card-title" style="height:80px;"> 
                            <asp:Label ID="Label1" runat="server" CssClass="text-center" Text="Announcements" ForeColor="#15609D" Width="100%"></asp:Label>
                        </div>
                        <div class="card-footer">
                            <a onclick="ShowLoading();" href="Emet_author.aspx?num=8" class="MyLink">
                                <span class="pull-right">
                                <asp:Label ID="Label5" runat="server" Text="Here" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                                <i class="fas fa-angle-right"></i>
                                </span>
                            </a>
                        </div>
                    </div>
                </div>
            </div>

            </div>
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

    <!-- Scroll to Top Button-->
    <a class="scroll-to-top rounded" href="#page-top">
      <i class="fas fa-angle-up"></i>
    </a>

    <!-- Modal -->
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
                                        do u want to stay Sign In?</ br>

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
           //$(function () {
           //    var timeout = 570000;
           //     $(document).bind("idle.idleTimer", function () {
           //         // function you want to fire when the user goes idle
           //         OpenModalSession();
           //         $("#StartTimer").click();
           //         //$.timeoutDialog({ timeout: 0.25, countdown: 15, logout_redirect_url: 'Login.aspx', restart_on_yes: true });
           //     });
           //     $(document).bind("active.idleTimer", function () {
           //         // function you want to fire when the user becomes active again
           //     });
           //     $.idleTimer(timeout);
           //});
        </script>
    </body>

</html>
