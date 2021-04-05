<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VndLabRate.aspx.cs" Inherits="Material_Evaluation.VndLabRate" %>

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
          padding-top:2px;
          padding-left:13px;
          padding-right:0px;
          border-top-right-radius: 4px;
          border-bottom-right-radius: 4px;
        }

        input[type=text] {
          padding: 0px 5px;
          display: inline-block;
          border: 1px solid #ccc;
          border-top-left-radius: 4px;
          border-bottom-left-radius: 4px;
          border-top-right-radius: 0px;
          border-bottom-right-radius: 0px;
          box-sizing: border-box;
          width: 100%;
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
        .pagination-sm span { color:yellow; font-weight:bold; font-size:18pt; align-content:center; }
        .table-nowrap {
            white-space: nowrap;
        }
    </style>

    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />

  </head>

  <body id="page-top">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
        <div class="row">
            <div id="loading" class="col-sm-12" style="padding-top:200px;" >
                <img id="loading-image" src="images/loading.gif" alt="Loading..."/>
                <div class="col-sm-12" style="text-align:center; opacity:1;">
                    <asp:Label ID="lbLoading" runat="server" Text="please Wait..." Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                </div>
            </div>
        </div>
    <nav class="navbar navbar-expand navbar-dark bg-dark static-top">
      <a class="navbar-brand mr-1" href="Vendor.aspx"><asp:Image ID="Image1" 
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
      </ul>
      <ol class="breadcrumb">
        <li >
            <asp:Label ID="lblUser"  runat="server" Width="147px"></asp:Label>
            <br/>
            <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
            <asp:LinkButton ID="LbBtnLogOut" runat="server" Text="Logout" OnClick="LbBtnLogOut_Click"></asp:LinkButton>
            <%--<a  href="login.aspx">Logout</a>--%>
        </li>
      </ol>
    </nav>



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
          <a class="nav-link" onclick="ShowLoading();" href="MasterData.aspx">
            <i class="fas fa-fw fa-table"></i>
            <span>Master Data</span>
          </a>
        </li>
        <li class="nav-item active">
            <a class="nav-link" onclick="ShowLoading();" href="ChangePwd.aspx?num=15">
                <i class="fas fa-fw fa-key"></i>
                <span>Change Password</span>
            </a>
        </li>
      </ul>


      <div id="content-wrapper" style="background-color:white;">

        <div class="container-fluid">
            <div class="card mb-3">
                <div class="card-header"  style="width: 100%; overflow: auto">
                    <i class="fas fa-fw fa-list"></i> Labour Rate
                    <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="Login-button fa-pull-right" PostBackUrl="Vendor.aspx" />
                </div>
                <div class="card-body">
                    <div class="col-md-12 Padding-Nol">
                        <asp:UpdatePanel ID="UpForm" runat="server">
                            <ContentTemplate>
                                <asp:Panel runat="server" DefaultButton="btnSearch">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label runat="server" ID="LbFilter" Text="Filter By"></asp:Label>
                                    </div>
                                    <div class="col-md-3" style="padding-bottom:5px;">
                                        <asp:DropDownList runat="server" ID="DdlFilterBy" >
                                            <asp:ListItem Text="Standard Labour Rate/Hr" Value="StdLabourRateHr"></asp:ListItem>
                                            <asp:ListItem Text="Vendor Country" Value="VendorCountry"></asp:ListItem>
                                            <asp:ListItem Text="Currency" Value="Currency"></asp:ListItem>
                                            <asp:ListItem Text="Follow Standard Rate" Value="FollowStdRate"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-7">
                                        <div class="group-main">
                                            <div class="SearchBox-txt"><asp:TextBox runat="server" ID="txtFind" Text=""></asp:TextBox></div>
                                            <span class="SearchBox-btn" style="background-color:#E9ECEF;">
                                                <asp:LinkButton ID="btnSearch" runat="server" autopostback="true" OnClientClick="ShowLoading();" onclick="btnSearch_Click"><i class="fa fa-search" aria-hidden="true" 
                                                    style="color:#005496;" ></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table table-responsive table-sm">
                                            <asp:UpdatePanel ID="UpdatePanel1"  runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional"  RenderMode="Block">
                                                    <ContentTemplate>
                                                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                                                                 AllowPaging="True" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging" 
                                                                  OnRowDataBound="GridView1_RowDataBound" CssClass="table-sm table-bordered table-nowrap" Font-Bold="False" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="No.">
                                                                                    <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="StdLabourRateHr" HeaderText="Standard Labour Rate/Hr"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorCountry" HeaderText="Country"></asp:BoundField>
                                                                <asp:BoundField DataField="Currency" HeaderText="CURR"></asp:BoundField>
                                                                <asp:BoundField DataField="FollowStdRate" HeaderText="Follow Standard Rate"></asp:BoundField>
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
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="LbTtlRecords" runat="server" Text="Total Record : 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                    </div>
                                </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
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
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>

    <%--<script src="http://code.jquery.com/jquery-1.12.0.min.js"></script>
<script src="http://code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>
<script src="Scripts/jquerysessionTimeout/jquery.sessionTimeout.js"></script>--%>
      </form>

      <%--script loading page--%>
    <script lang="javascript" type="text/javascript">
         $(window).load(function() {
         $('#loading').fadeOut("fast");
         });
    </script>
        
    <script type="text/javascript">
        function ShowLoading() {
            $('#loading').show();
        }
        function HideLoading() {
            $('#loading').fadeOut("fast");
        }
    </script>

  </body>
</html>
