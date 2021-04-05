<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PIRGeneration_C.aspx.cs" Inherits="Material_Evaluation.PIRGeneration_C" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
    <style type="text/css">
        .ui-datepicker-trigger {
            margin-top: 5px;
        }

        input[type=text] {
            margin-right: 5px;
            position: relative;
            top: 0px
        }

        .tetxboxcolor {
            background-color: #CAC8BF;
        }

        .GridStyle {
            font-family: calibri;
            font-size: 14px;
            overflow: auto;
            width: 100%;
        }


        .Login-button {
            border-radius: 5px;
            border: 1px solid #032742;
            background-color: #005496;
            color: #FFFFFF;
            font-family: Verdana;
            font-size: 16px;
        }


        .HeaderStyle1 {
            text-align: center;
            font-weight: bold;
            font-size: 12px;
            color: Black;
        }

        .GridStyle, .GridStyle th, .GridStyle td {
            border: 1px solid #010a19;
        }


        .GridPosition {
            position: absolute;
            left: 100px;
            height: 200px;
            width: 200px;
        }

        .tdpossition {
            position: absolute;
            left: 250px;
            height: 200px;
            width: 200px;
            font-family: Calibri;
            color: #7da1db;
        }
    </style>
    <style type="text/css">
        label {
            display: inline-block;
            float: left;
            clear: left;
            width: 250px;
            text-align: right;
            padding-right: 5px;
        }

        input[type=text], select, textarea {
            display: inline-block;
            float: left;
            border: 1px solid #0B243B;
            border-radius: 3px;
            box-sizing: border-box;
            padding: 3px 5px;
            left: 1px;
            width: 345px;
            height: 31px;
        }

        select {
            display: inline-block;
            float: left;
        }

        td {
            padding-bottom: 15px !important;
        }

        table {
            font-family: Verdana;
            font-weight: 600;
            font-size: 12px;
        }
        </style>
     <script src="Scripts/jquery.min.js" type="text/javascript"></script>
     <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
     <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />
</head>
<body id="page-top">
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand navbar-dark bg-dark static-top">
        <a class="navbar-brand mr-1" href="Home.aspx">
        <asp:Image ID="Image1" 
          runat="server" Height="31px" 
          ImageUrl="~/images/logo.gif" Width="179px" />
        </a>
        <button class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" href="#">
        <i class="fas fa-bars"></i>
        </button>
        <asp:Image ID="Image2" 
          runat="server" Height="24px" 
          ImageUrl="~/images/caption1.gif" Width="71px" />
        <!-- Navbar Search -->
        <div class="input-group">
          <div class="input-group-append">
          </div>
        </div>
        <!-- Navbar -->
        <ul class="navbar-nav ml-auto ml-md-0">
        <li class="nav-item dropdown no-arrow mx-1">
          <a class="nav-link dropdown-toggle" href="#" id="alertsDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            </a><div class="dropdown-menu dropdown-menu-right" aria-labelledby="alertsDropdown">
            <a class="dropdown-item" href="#">Action</a>
            <a class="dropdown-item" href="#">Another action</a>
            <div class="dropdown-divider"></div>
            <a class="dropdown-item" href="#">Something else here</a>
          </div>
        </li>
        <li class="nav-item dropdown no-arrow mx-1">
          <a class="nav-link dropdown-toggle" href="#" id="messagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            &nbsp;</a><div class="dropdown-menu dropdown-menu-right" aria-labelledby="messagesDropdown">
            <a class="dropdown-item" href="#">Approval Pending</a>
            <a class="dropdown-item" href="#">Revised Quote</a>
            <div class="dropdown-divider"></div>
            <a class="dropdown-item" href="#">Something else here</a>
          </div>
        </li>


		  <ol class="breadcrumb">
            <li >
              <asp:Label ID="lbluser1"  runat="server" Width="147px"></asp:Label>
             <br/>
                <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
              <a  href="login.aspx">Logout</a>
            </li>
          </ol>
      </ul>
    </nav>
        <div id="wrapper">
            <!-- Sidebar -->
                <ul class="sidebar navbar-nav">
                    <li class="nav-item active">
                      <a class="nav-link" href="Emet_author.aspx?num=1">
                        <i class="fas fa-fw fa-tachometer-alt"></i>
                        <span>Home</span>
                      </a>
                    </li>
            </ul>
                <div id="header">
                <asp:ScriptManager ID="scriptmanager1" runat="server">
                </asp:ScriptManager>
            </div>
                <div id="content-wrapper">
                <div class="container-fluid">
                    <!-- Breadcrumbs-->
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item"><%--<a href="#">First Article Item</a>--%> 
                        </li>
                        <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">
                            <tr>
                                <td>

                                    Clear All Transaction Details <br />
                                    <br />
                                        <asp:Button ID="btnclose" runat="server" Text="Clear All" CssClass="Login-button" Width="100px" Height="23px" OnClick="btnclose_Click" />

                                    </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>

                        </table>


                    </ol>
                    <!-- Icon Cards-->
                    <div class="row">
                    </div>
                    <!-- Area Chart Example-->
                    <div class="card mb-3">
                        <%--<div class="card-header">
                        <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION
                        
                       
                        </div>--%>
                        <div class="card-body">
                            <!--<canvas id="myAreaChart" width="100%" height="30"></canvas>-->

                            <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">
                                <tr style="height:auto">
                                    <td align="right">
                                        &nbsp;</td>
                                </tr>

                                <%--<tr>
                        <td class="style6">
                            <asp:Label ID="Label6" runat="server" Text="SAP Part Code Description :" Font-Bold="True" 
                                ForeColor="Black"></asp:Label>
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="txtpartdescription" runat="server" AutoPostBack="true" Height="29px" TextMode="MultiLine" CssClass="tetxboxcolor"
                                Width="300px"></asp:TextBox>
                           
                        </td>
                    </tr>--%>


                                <tr>

                                    <td>

                                         <asp:Label ID="Label1" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>

                                    <td>

                                          <asp:UpdatePanel ID="UpdatePanel1"  runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional"  RenderMode="Block">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnclose" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </td>
                                </tr>

                                <tr align="center">
                                     <td>
                                          <asp:HiddenField ID="hdnReqNo" runat="server" />
                                         </td>
                                </tr>
                                   
                            </table>
                            <div id="dialog1" title="Full Image View" style="display: none">
                                <asp:Image ID="img1" runat="server" />
                            </div>
                            <!-- DataTables Example -->

                        </div>
                        <!-- /.container-fluid -->
                        <!-- Sticky Footer -->
                        <%--<asp:GridView ID="GridView2" runat="server">
                                  </asp:GridView>--%>
                    </div>
                    <!-- /.content-wrapper -->
                </div>
                <!-- /#wrapper -->

                <!-- Scroll to Top Button-->
                <a class="scroll-to-top rounded" href="#page-top"><i class="fas fa-angle-up"></i>
                </a>
                <!-- Logout Modal-->
                <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
                    aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel">
                                Ready to Leave?dalLabel">
                        Ready to Leave?    </button>
                            </div>
                            <div class="modal-body">
                                Select "Logout" below if you are ready to end your current session.
                            </div>
                            <div class="modal-footer">
                                <button class="btn btn-secondary" type="button" data-dismiss="modal">
                                    Cancel</button>
                                <a class="btn btn-primary" href="login.html">Logout</a>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Bootstrap core JavaScript-->
          </div>
        </div>
    </form>
</body>
</html>
