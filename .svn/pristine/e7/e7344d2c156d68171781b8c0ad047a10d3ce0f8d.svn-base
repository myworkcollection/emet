<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PIC_Approval.aspx.cs" Inherits="Material_Evaluation.PIC_Approval" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<html xmlns="http://www.w3.org/1999/xhtml">

  <head>

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
        <li class="nav-item dropdown no-arrow">
          <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-user-circle fa-fw"></i>
          </a>
          <div class="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
            <a class="dropdown-item" href="#">Settings</a>
            <a class="dropdown-item" href="#">Activity Log</a>
            <div class="dropdown-divider"></div>
            <a class="dropdown-item" href="#" data-toggle="modal" data-target="#logoutModal">Logout</a>
          </div>
        </li>
      </ul>
    </nav>



    <div id="wrapper">

      <!-- Sidebar -->
      <ul class="sidebar navbar-nav">
        <li class="nav-item active">
          <a class="nav-link" href="Home.aspx">
            <i class="fas fa-fw fa-tachometer-alt"></i>
            <span>Home</span>
          </a>
        </li>
        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-fw fa-folder"></i>
            <span>PIC Approval</span>
          </a>
          <div class="dropdown-menu" aria-labelledby="pagesDropdown">
           <%-- <h6 class="dropdown-header">Active Material</h6>--%>
            <a class="dropdown-item" href="NewRequest.aspx">First Article Item</a>
            <a class="dropdown-item" href="ChangeofVendor.aspx">Change of Vendor</a>
            <a class="dropdown-item" href="WithSApCode.aspx">Draft & Cost Planning</a>
            <%--<div class="dropdown-divider"></div>
            <h6 class="dropdown-header">Other Pages:</h6>
            <a class="dropdown-item" href="404.html">404 Page</a>
            <a class="dropdown-item" href="blank.html">Blank Page</a>--%>
          </div>
        </li>

         <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="A1" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-fw fa-folder"></i>
            <span>Revision of MET</span>
          </a>
          <div class="dropdown-menu" aria-labelledby="pagesDropdown">
           <%-- <h6 class="dropdown-header">Active Material</h6>--%>
            <a class="dropdown-item" href="login.html">Changeable Elements</a>
            <a class="dropdown-item" href="register.html">Design Change</a>
           
            <%--<div class="dropdown-divider"></div>
            <h6 class="dropdown-header">Other Pages:</h6>
            <a class="dropdown-item" href="404.html">404 Page</a>
            <a class="dropdown-item" href="blank.html">Blank Page</a>--%>
          </div>
        </li>

        <li class="nav-item">
          <a class="nav-link" href="NewRequest.aspx">
            <i class="fas fa-fw fa-chart-area"></i>
            <span>Price Revision</span></a>
        </li>
       <%-- <li class="nav-item">
          <a class="nav-link" href="tables.html">
            <i class="fas fa-fw fa-table"></i>
            <span>Change Request</span></a>
        </li>--%>
		
		      <%--  <li class="nav-item">
          <a class="nav-link" href="tables.html">
            <i class="fas fa-fw fa-table"></i>
            <span>Approval pending</span></a>
        </li>--%>
		
		        <li class="nav-item">
          <a class="nav-link" href="tables.html">
            <i class="fas fa-fw fa-table"></i>
            <span>PIR Generation</span></a>
			
			        <li class="nav-item">
          <a class="nav-link" href="tables.html">
            <i class="fas fa-fw fa-table"></i>
            <span>Reports</span></a>
			
			
        </li>
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
                    <li class="breadcrumb-item"><a href="#">First Article Item</a> </li>
                </ol>
                <!-- Icon Cards-->
                <div class="row">
                </div>
                <!-- Area Chart Example-->
                <div class="card mb-3">
                    <div class="card-header">
                        <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION</div>
                    <div class="card-body">
                        <!--<canvas id="myAreaChart" width="100%" height="30"></canvas>-->
                       
                <table class="style3">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblRequest_view" runat="server" Font-Bold="true" Font-Names="calibri"
                                Font-Size="Medium" Text="PIC APPROVAL "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <div style="height: 300px; width: 750px; overflow:scroll; overflow-y: hidden;" >
                            <asp:GridView ID="grdPICApproval" runat="server" AutoGenerateColumns="False" DataKeyNames="requestno" 
                                CellPadding="4" GridLines="both" Font-Names="calibri" Font-Size="Medium" ForeColor="#333333">
                                <HeaderStyle CssClass="HeaderStyle1" Height="40px" BackColor="#5D7B9D" ForeColor="White">
                                </HeaderStyle>
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="requestno" HeaderText="Request No" HeaderStyle-Width="100px" AccessibleHeaderText="Request Number"  />
                                    <asp:BoundField DataField="plant" HeaderText="Plant" AccessibleHeaderText="Plant" HeaderStyle-Width="80px" />
                                    <asp:BoundField DataField="status" HeaderText="Status" AccessibleHeaderText="status" HeaderStyle-Width="50px" />
                                    <asp:BoundField DataField="requeststatus" HeaderText="Request Status" HeaderStyle-Width="150px" AccessibleHeaderText="Request status" />
                                    <asp:BoundField DataField="vendorname" HeaderText="Vendor Name" AccessibleHeaderText="VendorName" HeaderStyle-Width="100px" />
                                    <asp:BoundField DataField="city" HeaderText="Vendor Country" AccessibleHeaderText="country" HeaderStyle-Width="80px" />
                                    <asp:BoundField HeaderText="Comments" DataField="comments" AccessibleHeaderText="Comments" HeaderStyle-Width="100px" />
                                    <asp:TemplateField HeaderText="Approval">
                                        <ItemTemplate>
                                            <asp:Button ID="Button1" Text="Approve" runat="server" CssClass="Login-button" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reject">
                                        <ItemTemplate>
                                            <asp:Button ID="btnreject" Text="Reject" runat="server" CssClass="Login-button" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowEditButton="true" CancelText="" DeleteText="Edit" EditText="Edit" />
                                    <asp:CommandField ShowEditButton="true" CancelText="" DeleteText="Edit" EditText="View" />
                                    <asp:CommandField ShowEditButton="true" CancelText="" DeleteText="Edit" EditText="Delete" />
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                <HeaderStyle BackColor="#7da1db" Font-Bold="True" ForeColor="black" Font-Size="Medium" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                         <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="Login-button" PostBackUrl="Home.aspx"
                          Width="100px"
                    Height="23px"  />
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>

                </table>
                        
                    </div>
                   <%-- <div class="card-footer small text-muted">
                        Updated yesterday at 11:59 PM</div>--%>
                </div>
                <!-- DataTables Example -->
               <%-- <div class="card mb-3">
                    <div class="card-header">
                        <i class="fas fa-table"></i>Data Table Example</div>
                    <div class="card-body">
                        <div class="table-responsive">
                        </div>
                    </div>
                    <div class="card-footer small text-muted">
                        Updated yesterday at 11:59 PM</div>
                </div>--%>
            </div>
            <!-- /.container-fluid -->
            <!-- Sticky Footer -->
           <%-- <footer class="sticky-footer">
          <div class="container my-auto">
            <div class="copyright text-center my-auto">
              <span>Copyright © Your Website 2018</span>
            </div>
          </div>
        </footer>--%>
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
                        Ready to Leave?</h5>
                    <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    Select "Logout" below if you are ready to end your current session.</div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" type="button" data-dismiss="modal">
                        Cancel</button>
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

