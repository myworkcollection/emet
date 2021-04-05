<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home_.aspx.cs" Inherits="Material_Evaluation.Home_" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">

  <head>
      <title>eMET</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>eMET</title>

    <!-- Bootstrap core CSS-->
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">

    <!-- Page level plugin CSS-->
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet">

    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet">

     <style type="text/css">
        .tetxboxcolor
        {
            background-color: #CAC8BF;
        }
        
        .GridStyle
        {
            font-family: calibri;
            font-size: 14px;
            overflow: auto;
            width: 100%;
        }
        
        
        .Login-button
        {
            border-radius: 5px;
            border: 1px solid #032742;
            background-color: #005496;
            color: #FFFFFF;
            font-family: Arial;
        }
        
        
        .HeaderStyle1
        {
            text-align: center;
            font-weight: bold;
            font-size: 12px;
            color: Black;
        }
        .GridStyle, .GridStyle th, .GridStyle td
        {
            border: 1px solid #010a19;
        }
        
        
        .GridPosition
        {
            position: absolute;
            left: 100px;
            height: 200px;
            width: 200px;
        }
        
        .tdpossition
        {
            position: absolute;
            left: 250px;
            height: 200px;
            width: 200px;
            font-family: Calibri;
            color: #7da1db;
        }
        .style3
        {
            width: 100%;
        }
        .style5
        {
            width: 242px;
        }
        .style6
        {
            width: 281px;
        }
    </style>


  </head>

  <body id="page-top">

      <form id="form1" runat="server">

    <nav class="navbar navbar-expand navbar-dark bg-dark static-top">
      <a class="navbar-brand mr-1" href="Home.aspx"><asp:Image ID="Image1" 
          runat="server" Height="31px" 
          ImageUrl="~/images/logo.gif" Width="179px" /></a>
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
              <asp:Label ID="lblUser"  runat="server" Width="147px"></asp:Label>
             <br/>
                <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
              <a  href="login.aspx">Logout</a>
            </li>
          </ol>
          

      <%--  <li class="nav-item dropdown no-arrow">
          <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-user-circle fa-fw"></i>
          </a>--%>
          <%-- <ol class="breadcrumb">
            <li class="breadcrumb-item">
             <asp:Label ID="lblUser" CssClass="breadcrumb-item" runat="server" Text=""></asp:Label>
             <br />
              <a class="breadcrumb-item" href="login.aspx">Logout</a>
            </li>

          
          
          </ol>--%>
           
      <%--  <div class="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
          
             <asp:Label ID="lblUser" CssClass="dropdown-item" runat="server" Text=""></asp:Label>
              <a class="dropdown-item" href="login.aspx">Logout</a>
             
            
            <div class="dropdown-divider"></div>
            
          </div>--%>

        
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
         

        <li class="nav-item active">
          <a class="nav-link" href="Emet_author.aspx?num=2" >
            <i class="fas fa-fw fa-tachometer-alt"></i>
                <span>New Request</span></a>
        </li>

         <li class="nav-item active">
          <a class="nav-link" href="Revision.aspx">
            <i class="fas fa-fw fa-table" ></i>
           <span > Revision of MET</span></a>
			
        </li>

        <li class="nav-item active">
          <a class="nav-link" href="MassRevision.aspx">
            <i class="fas fa-fw fa-chart-area"></i>
            <span>Mass Revision</span></a>
        </li>
      
		 <li class="nav-item active">
          <a class="nav-link" href="Emet_author.aspx?num=16">
            <i class="fas fa-fw fa-table"></i>
            <span>PIR Generation</span></a>

        </li>

          	<li class="nav-item active">
          <a class="nav-link" href="Emet_author.aspx?num=2">
            <i class="fas fa-fw fa-table"></i>
            <span>Reports</span></a>

        </li>
      </ul>


      <div id="content-wrapper">

        <div class="container-fluid">

          <!-- Breadcrumbs-->
          <ol class="breadcrumb">
            <li class="breadcrumb-item">
              <a href="#">Dashboard</a>
            </li>

            <li class="breadcrumb-item active">Overview</li>
                <%--<asp:Label ID="lblUserDetails" runat="server" Text="Pinnsoft1"></asp:Label>--%>
          </ol>
            

          <!-- Icon Cards-->
          
            <table class="w-100">

             <tr>
                <td style="width:20%;">
                </br>
                </br>
                </br>
        
                </td>
                    <td style="width:80%;">
                    </br>
                    </br>
                    </br>
                  
                    </td>
                    </tr>


                <tr>
                <td style="width:20%;">
                
                </td>
                    <td style="width:80%;">
                                  <div class="row">

                                        <div class="col-xl-3 col-sm-6 mb-3">
              <div class="card text-white bg-primary o-hidden h-100">
                <div class="card-body">
                  <div class="card-body-icon">
                  </div>
                  <div class="mr-5">Vendor Response--></div>
                </div>
                <a class="card-footer text-white clearfix small z-1" href="Emet_author.aspx?num=3">
                  <span class="float-left">Pending Request</span>
                  <span class="float-right">
                   <asp:Label ID="lblvenrresponse" runat="server" Text="" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                    <i class="fas fa-angle-right"></i>
                  </span>
                </a>
              </div>
            </div>
            <div class="col-xl-3 col-sm-6 mb-3">
              <div class="card text-white bg-primary o-hidden h-100">
                <div class="card-body">
                  <div class="card-body-icon">
                  </div>
                  <div class="mr-5">PIC Approval--></div>
                </div>
                <a class="card-footer text-white clearfix small z-1" href="Emet_author.aspx?num=17">
                  <span class="float-left" >Pending Request</span>
                  <span class="float-right">
                      <asp:Label ID="lblCount" runat="server" Text="" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                    <i class="fas fa-angle-right"></i>
                  </span>
                </a>
              </div>
            </div>

            <div class="col-xl-3 col-sm-6 mb-3">
              <div class="card text-white bg-primary o-hidden h-100">
                <div class="card-body">
                  <div class="card-body-icon">
                  </div>
                  <div class="mr-5">Manager Approval--></div>
                </div>
                <a class="card-footer text-white clearfix small z-1" href="Emet_author.aspx?num=5">
                  <span class="float-left">Pending Request</span>
                  <span class="float-right">
                   <asp:Label ID="lblmgr" runat="server" Text="" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                    <i class="fas fa-angle-right"></i>
                  </span>
                </a>
              </div>
            </div>

              
            
            </div>
            </td>
            </tr>
                       
<tr>

 <td style="width:20%;">
                
                </td>
                
                    <td style="width:80%;">
                    </br>
          <div class="row">

          
            <div class="col-xl-3 col-sm-6 mb-3">
              <div class="card text-white bg-primary o-hidden h-100">
                <div class="card-body">
                  <div class="card-body-icon">
                  </div>
                  <div class="mr-5">Director Approval--></div>
                </div>
                <a class="card-footer text-white clearfix small z-1" href="Emet_author.aspx?num=6">
                  <span class="float-left">Pending Request</span>
                  <span class="float-right">
                   <asp:Label ID="lblDirector" runat="server" Text="" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                    <i class="fas fa-angle-right"></i>
                  </span>
                </a>
              </div>
            </div>


                 <div class="col-xl-3 col-sm-6 mb-3">
              <div class="card text-white bg-primary o-hidden h-100">
                <div class="card-body">
                  <div class="card-body-icon">
                  </div>
                  <div class="mr-5">Waiting for PIR</div>
                </div>
                <a class="card-footer text-white clearfix small z-1" href="Emet_author.aspx?num=7">
                  <span class="float-left">Closed Request</span>
                  <span class="float-right">
                   <asp:Label ID="lblclosed" runat="server" Text="" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                    <i class="fas fa-angle-right"></i>
                  </span>
                </a>
              </div>
            </div>



           

             
                 <div class="col-xl-3 col-sm-6 mb-3">
              <div class="card text-white bg-primary o-hidden h-100">
                <div class="card-body">
                  <div class="card-body-icon">
                  </div>
                  <div class="mr-5">Announcements</div>
                </div>
                <a class="card-footer text-white clearfix small z-1" href="Emet_author.aspx?num=8">
                  <span class="float-left">Details</span>
                  <span class="float-right">
                   <asp:Label ID="Label1" runat="server" Text="Here" Font-Bold="true" Font-Size="X-Large" ></asp:Label>
                    <i class="fas fa-angle-right"></i>
                  </span>
                </a>
              </div>
            </div>
          </div>
                        
                        </td>
                </tr>
            </table>
          


          <!-- Area Chart Example-->

          <!-- DataTables Example -->

        </div>
        <!-- /.container-fluid -->

        <!-- Sticky Footer -->
        <footer class="sticky-footer">
          <div class="container my-auto">
            <div class="copyright text-center my-auto">
              <span>Copyright © ShimanoDT 2018</span>
            </div>
          </div>
        </footer>

      </div>
      <!-- /.content-wrapper -->

    </div>
    <!-- /#wrapper -->

    <!-- Scroll to Top Button-->
    <a class="scroll-to-top rounded" href="#page-top">
      <i class="fas fa-angle-up"></i>
    </a>

    <!-- Logout Modal-->
    <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Ready to Leave?</h5>
            <button class="close" type="button" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">×</span>
            </button>
          </div>
          <div class="modal-body">Select "Logout" below if you are ready to end your current session.</div>
          <div class="modal-footer">
            <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
            <a class="btn btn-primary" href="login.html">Logout</a>
          </div>
        </div>
      </div>
    </div>

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

    <!-- Page level plugin JavaScript-->
    <script src="vendor/chart.js/Chart.min.js"></script>
    <script src="vendor/datatables/jquery.dataTables.js"></script>
    <script src="vendor/datatables/dataTables.bootstrap4.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="js/sb-admin.min.js"></script>

    <!-- Demo scripts for this page-->
    <script src="js/demo/datatables-demo.js"></script>
    <script src="js/demo/chart-area-demo.js"></script>

      </form>

  </body>

</html>

